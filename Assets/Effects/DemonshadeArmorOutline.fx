//============================================================================
//魔影套盔甲轮廓光：细亮边 + 淡光晕 + 缓慢流动（流动看不出来）
//
//C#端复制原盔甲图层，进行预处理后，每一层传入s0进行处理
//C#逐条传入纹理尺寸和帧范围，本shader计算外围的光
//
//无噪声纹理；动画由“帧内坐标 + 时间”生成
//每像素 17 次 alpha 采样：中心 1 次 + 亮边 8 次 + 外晕 8 次
//============================================================================

//着色器就该tm让AI写

//传入处理过的复制的盔甲贴图
sampler uImage0 : register(s0);

//原图归一化 UV 中占多少距离，即 (1/纹理宽, 1/纹理高)
//例如 40x1120 的图集为 (1/40, 1/1120)
float2 uTexel;

//未外扩 sourceRect 的 UV 边界：xy=左上，zw=右下，采用 [min,max) 半开区间
//C# 用 frame.Left/Right 除以纹理宽、Top/Bottom 除以纹理高计算
//头盔扩展使用扩展贴图自己的帧，不能套用普通头盔的帧尺寸
float4 uFrameBounds;

//未外扩 sourceRect 左上角的图集像素坐标 (frame.X, frame.Y)
//用来消除切换动画帧时的图集偏移，得到当前部件帧内的局部像素坐标
float2 uFramePixels;

//单位为秒，C# 传 Main.GlobalTimeWrappedHourly % 60f
float uFlowTime;

//-------------------- C# 加载时设置的外观参数 --------------------

//半径单位为原贴图像素，会随玩家/镜头缩放，不是固定屏幕像素
//必须满足 0 < uCoreWidth <= uHaloWidth <= C# Padding - 1
//shader 只能在已提交四边形内画光，所以扩大半径时也要同步扩大 C# Padding
float uCoreWidth;      //亮边采样半径，默认 1.25
float uHaloWidth;      //外晕采样半径，默认 2.75
float uHaloStrength;   //外晕权重，设为 0 可只显示亮边
float uIntensity;      //整体强度
float3 uOutlineColor;  //紫色RGB，默认 (0.70, 0.34, 1.00)

//安全读取当前帧的 alpha：帧外返回 0，防止扩边时采到相邻帧
float FrameAlpha(float2 uv)
{
    //像素中心在边界向内半个像素的位置
    //先把“实际用于读取纹理的坐标”夹到当前帧首/末像素中心之间，
    //即使开启线性采样，也不会跨过当前帧边界读到其他动画帧
    float2 sampleUv = clamp(uv, uFrameBounds.xy + uTexel * 0.5,
                               uFrameBounds.zw - uTexel * 0.5);

    //step(edge,x)：x >= edge 返回 1，否则返回 0
    //第一项判断 uv >= min；第二项判断 uv < max
    //inside.x/y 分别表示横/纵坐标是否落在未外扩的原始帧中
    float2 inside = step(uFrameBounds.xy, uv) * (1.0 - step(uFrameBounds.zw, uv));

    //clamp 本身不能代替这个乘法：若只夹坐标，帧边缘的像素会被向外拉成长条
    //读取被限制在安全范围后，仍要让逻辑上位于帧外的采样 alpha 等于 0
    //只使用 alpha，原纹理的 RGB 不参与形状计算
    return tex2D(uImage0, sampleUv).a * inside.x * inside.y;
}

//八方向近似膨胀：寻找指定半径附近是否存在盔甲像素
float Dilate8(float2 uv, float radius, float center)
{
    //把贴图像素半径换算成 UV 偏移；两个坐标轴使用各自真实纹理尺寸
    float2 axial = uTexel * radius;

    //斜方向 x/y 各取 radius/sqrt(2)，与轴向保持相同的径向距离
    //直接使用 (radius,radius) 会让斜向半径变为 sqrt(2) 倍，轮廓更像方框
    float2 diagonal = axial * 0.70710678;

    //复用主函数已经读取的中心 alpha，避免每圈重复采样中心
    //取 max 而非相加，避免邻接多个盔甲像素时亮度不断叠高
    float a = center;

    //右、左、下、上UV 的正 Y 朝贴图下方
    a = max(a, FrameAlpha(uv + float2( axial.x, 0.0)));
    a = max(a, FrameAlpha(uv + float2(-axial.x, 0.0)));
    a = max(a, FrameAlpha(uv + float2(0.0,  axial.y)));
    a = max(a, FrameAlpha(uv + float2(0.0, -axial.y)));

    //右下、左下、右上、左上
    a = max(a, FrameAlpha(uv + float2( diagonal.x,  diagonal.y)));
    a = max(a, FrameAlpha(uv + float2(-diagonal.x,  diagonal.y)));
    a = max(a, FrameAlpha(uv + float2( diagonal.x, -diagonal.y)));
    a = max(a, FrameAlpha(uv + float2(-diagonal.x, -diagonal.y)));

    //半径太大可能出现间隙或明显的八方向结构，默认建议维持小半径
    return a;
}

//像素着色器入口：
//TEXCOORD0 = 图集 UV，包括 C# 外扩出来的区域；
//输入 COLOR0 = DrawData 的顶点颜色，只取它的 alpha 作为可见度；
//返回 COLOR0 = 交给玩家 AlphaBlend 的预乘 RGBA
float4 ArmorOutlinePass(float2 uv : TEXCOORD0, float4 tint : COLOR0) : COLOR0
{
    //原盔甲在当前像素的覆盖度
    float center = FrameAlpha(uv);

    //膨胀结果减去原图，挖掉本体，保留外边缘
    //saturate(x)=clamp(x,0,1)，防止减法结果为负
    //不透明内部 center=1，core=0；外部 center=0 且邻接盔甲时 core>0
    //半透明边缘保留连续差值，并非只有 0/1 两种状态
    float core = saturate(Dilate8(uv, uCoreWidth, center) - center);

    //更大半径生成淡外晕两个稀疏采样环不保证严格包含关系，
    //用 max(...,core) 确保亮边位置也属于外晕范围，避免在那里出现缺口
    float halo = saturate(max(Dilate8(uv, uHaloWidth, center) - center, core));

    //uv/uTexel 还原成图集像素坐标，再减去当前帧左上角
    //切帧时，图集 Y 可能突然增加几十像素；减掉帧偏移后，光带不会因此跳变
    //这是“每个盔甲部件”的局部坐标，不是整套盔甲共享的世界坐标
    float2 localPixel = uv / uTexel - uFramePixels;

    //sin 使用弧度；tau=2*pi，秒*tau*Hz 得到周期相位
    const float tau = 6.28318530718;

    //0.5 Hz：每 2 秒呼吸一次，亮度范围 0.84～1.00，不会完全熄灭
    float breathe = 0.92 + 0.08 * sin(uFlowTime * tau * 0.5);

    //x/y 系数决定光带的空间密度和倾斜方向；负时间项让光带持续移动
    //流动频率为 0.75 Hz，亮度范围 0.80～1.00
    float flow = 0.90 + 0.10 * sin(localPixel.y * 0.18 + localPixel.x * 0.09
                                 - uFlowTime * tau * 0.75);

    //先组合两层形状，再统一乘整体强度、呼吸、流动和部件可见度
    //亮边权重 0.86，外晕权重可调；两者会在亮边处叠加，最后限制到 [0,1]
    //tint.a 来自原 DrawData.color.A，部件淡出时轮廓一起淡出
    //不乘 tint.rgb，以免环境光过暗时发光效果也跟着消失
    float alpha = saturate((core * 0.86 + halo * uHaloStrength)
                           * uIntensity * breathe * flow * tint.a);

    //lerp(a,b,t)=a*(1-t)+b*t：外晕使用稍暗紫色，亮边使用配置的原色
    float3 color = lerp(uOutlineColor * 0.72, uOutlineColor, core);

    //玩家使用预乘 AlphaBlend：最终 RGB=源 RGB+背景 RGB*(1-源 alpha)
    //因此输出 RGB 必须先乘 alpha；直接返回 color 会让透明处也过亮
    //不把 A 强制设为 0，以免变成另一种加色混合语义
    return float4(color * alpha, alpha);
}

technique Technique1
{
    pass ArmorOutlinePass
    {
        //采样太大没招了，使用PS3模型
        PixelShader = compile ps_3_0 ArmorOutlinePass();
    }
}
