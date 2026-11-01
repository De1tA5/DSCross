// 魔影帷幕 —— 世界层粒子着色器（自带顶点着色器，配合 DrawUserIndexedPrimitives 使用）
//
// 顶点布局合同（FNA）：
//   POSITION0 / COLOR0 / TEXCOORD0
// 贴图绑定：
//   s1 = 粒子贴图（余烬 / 残影 / 冲击环 / 法阵）
//   s2 = 无缝噪声图（负责"溶解"与流动感）
//
// 注意：ps_2_0 有 64 条算术指令上限，这里刻意写得很短。

sampler SpriteTexture : register(s1);
sampler NoiseTexture  : register(s2);

matrix uWorldViewProjection;
float uTime;
float uNoiseScale;
float uDissolve;
float uScrollSpeed;

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(in VertexShaderInput input)
{
    VertexShaderOutput output;
    output.Position = mul(input.Position, uWorldViewProjection);
    output.Color = input.Color;
    output.TextureCoordinates = input.TextureCoordinates;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 tex = tex2D(SpriteTexture, input.TextureCoordinates);

    // 两层噪声错开滚动，避免出现明显的"整体位移"
    float2 noiseUv = input.TextureCoordinates * uNoiseScale
                   + float2(uTime * uScrollSpeed, -uTime * uScrollSpeed * 0.63);
    float n = tex2D(NoiseTexture, noiseUv).r;

    float mask = saturate((n - uDissolve) * 2.6);

    float4 color = tex * input.Color;

    // 溶解：mask 同时缩放 RGB 与 A（A 只在需要混合时才有意义）
    color *= mask;

    // 轻微紫偏，让任何贴图都贴到魔影（灾厄 Demonshade）的色系上
    color.rgb *= float3(1.12, 0.92, 1.28);

    // 颜色合同：这里不再额外乘一次 color.a。
    // 顶点色已经是 Color * alpha（XNA 会把 RGB 一起缩），C# 侧又用 One/One 的加法混合，
    // 若此处再乘 color.a 就是 alpha 应用第二次，淡出会变成二次曲线（低 alpha 段直接看不见）。
    return color;
}

technique Technique1
{
    pass AuraPass
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader  = compile ps_2_0 PixelShaderFunction();
    }
}
