//魔影套盔甲轮廓光（DemonshadeArmorOutline.fx）
//风格：ps_2_0，无顶点着色器 —— 画层里 SpriteBatch 画的是屏幕空间四边形，顶点变换固定
//思路：对轮廓贴图做 3×3 邻域最大值膨胀（9 次采样，ps_2_0 指令预算内完全安全），
//      邻域里只要有盔甲像素，当前像素就发光；原盔甲位置不画（挖空，免得糊住盔甲本体）
//      发光色 = uOutlineColor + 噪声脉动（uTime 驱动）

sampler uImage0 : register(s0);   //盔甲轮廓贴图（SpriteBatch 自动绑定）
sampler uImage1 : register(s1);   //噪声图（可选，C# 侧可绑 DemonshadeNoise 做脉动）

float uOutlineWidth;              //轮廓宽度（像素），1.0~2.0 合适
float uIntensity;                 //整体强度（0~1）
float uTime;                      //驱动脉动
float3 uOutlineColor;             //轮廓颜色（魔影紫：0.66, 0.30, 1.0）

float4 ArmorOutlinePass(float2 uv : TEXCOORD0) : COLOR0
{
    float4 center = tex2D(uImage0, uv);

    //--- 3×3 邻域膨胀：找邻域内最大 alpha ---
    float2 texel = uOutlineWidth / float2(512.0, 512.0);   //贴图尺寸按实际改（盔甲帧一般 40×1120，但 C# 侧会传正确值）
    float maxNeighborAlpha = 0.0;

    //ps_2_0 不允许动态循环，但固定 9 次展开完全在指令预算内
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2(-texel.x, -texel.y)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2(      0.0, -texel.y)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2( texel.x, -texel.y)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2(-texel.x,       0.0)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, center.a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2( texel.x,       0.0)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2(-texel.x,  texel.y)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2(      0.0,  texel.y)).a);
    maxNeighborAlpha = max(maxNeighborAlpha, tex2D(uImage0, uv + float2( texel.x,  texel.y)).a);

    //--- 轮廓判定：邻域有盔甲（maxNeighborAlpha > 0）但当前像素不是盔甲（center.a 低）---
    float outline = maxNeighborAlpha * (1.0 - center.a);

    //基础脉动
    float pulse = 0.85 + 0.15 * sin(uTime * 2.0);
    //有噪声时叠加一层流动
    if (uIntensity > 0.01)   
    {
        float2 noiseUv = uv * 2.0 + float2(uTime * 0.05, -uTime * 0.03);
        float n = tex2D(uImage1, noiseUv).r;
        pulse *= 0.8 + 0.4 * n;
    }

    //--- 挖空中心：原盔甲位置不画轮廓光，避免糊住 ---
    float3 glowColor = uOutlineColor * outline * pulse * uIntensity;
    float alpha = outline * uIntensity;

    return float4(glowColor, alpha);
}

technique Technique1
{
    pass ArmorOutlinePass
    {
        PixelShader = compile ps_2_0 ArmorOutlinePass();
    }
}
