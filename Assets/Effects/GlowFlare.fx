
//.fx文件使用hsls编写格式，shaderToy 为gsls

sampler uImage0 : register(s0); //原版实体基本图层，在drawdate数据结构中，如玩家贴图、粒子贴图等
sampler uImage1 : register(s1); //UseImage方法可修改

//以下为原版ArmorShader已设置的基本参数
float3 uColor; //颜色，UseColor方法可修改
float uTime;
float3 uSecondaryColor; //第二颜色，UseSecondaryColor方法可修改
float uOpacity; //透明度，UseOpacity方法可修改
float uSaturation; //饱和度，UseSaturation方法可修改
float2 uTargetPosition; //UseTargetPosition方法可修改

float uDirection;
float uRotation;
float4 uSourceRect; //当前绘制帧率位置和大小
float2 uWorldPosition;
float3 uLightSource; //有反射效果
float2 uImageSize0;
float2 uImageSize1; //UseImage方法可修改

float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;

//执行方法
//coords就是UV
float4 Function(float2 coords : TEXCOORD0) : COLOR0
{
    //使用 UseOpacity 作为阈值，UseSaturation为光照强度
    float threshold = uOpacity;
    float boost = uSaturation;
    //根据TargetPosition是否有亮光调整
    float totalboost = uTargetPosition.x;
    
    //处理当前像素的纹理坐标，再从 uImage0 纹理中读取颜色，保存到 color
    //对横向坐标取余，让它落在 -1～1 之间，fmod(coords.x, 1.0)
    //取绝对值，使横向采样坐标落在 [0, 1)，abs(fmod(coords.x, 1.0))
    //最后纵向保持不变
    //tex2D为读取颜色，前参数为图片，后参数为采样坐标
    //最后获取color有RGBA
    float4 color = tex2D(uImage0, float2(abs(fmod(coords.x, 1.0)), coords.y));
    
    //基础染色外再加高于 Alpha 阈值的亮芯
    //这里根据贴图透明度是否超过阈值，来增加实际显示亮度，这里“+”为三通道一起加
    //color.xyz * color.w 为该像素基础亮度， uColor 调整颜色
    float3 bright = color.xyz * color.w * uColor + (color.w > threshold ? ((color.w - threshold) * boost) : float3(0, 0, 0));
	 
    //平均颜色，略压低最终输出光照，避免过曝
    float avg = ((uColor.x + uColor.y + uColor.z) / 3.0);

    //RGB 最终乘 avg × totalboost
    //Alpha 在构造 float4 时已经乘过一次 avg
    //随后整个 float4 再乘 avg，因此最终Alpha为
    //贴图 Alpha × avg² × totalboost
    return (float4(bright, color.w * avg) * avg) * totalboost;
}

//默认命名为Technique1，实际可以随意命名
technique Technique1
{
    //pass通道，可随意命名，C#中 effect.Passes[名称].Apply() 
    //实际绘制由后续 SpriteBatch 执行
    pass GlowFlare
    {
        //像素着色器，使用ps_2_0模型进行编译
        PixelShader = compile ps_2_0 Function();
    }
}

//.fx文件之后还需进行编译转换为.xnb/.fxc文件，tmod只支持读取这两种格式
//使用EasyXnb进行编译