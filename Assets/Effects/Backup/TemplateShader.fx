
//.fx文件使用hsls编写格式

sampler uImage0 : register(s0); //原版实体基本图层，在drawdate数据结构中如玩家贴图、粒子贴图等
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
//coords就是UV，左下角为(0,0)
float4 Function(float2 coords: TEXCOORD0) : COLOR0
{
    //最后必须返回一个float4
    return float4(0,0,0,0);
}

//默认命名为Technique1，实际可以随意命名
technique Technique1
{
    //pass通道，可随意命名，C#中 effect.Passes[名称].Apply() 
    //实际绘制由后续 SpriteBatch 执行
    pass MyShader
    {
        //像素着色器，使用ps_2_0模型进行编译
        PixelShader = compile ps_2_0 Function();
    }
}

//.fx文件之后还需进行编译转换为.xnb/.fxc文件，tmod只支持读取这两种格式
//使用EasyXnb进行编译