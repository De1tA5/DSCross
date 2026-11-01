// 魔影面纱 —— 屏幕滤镜（ps only，由 Terraria 的 FilterManager 整屏抓图后调用）
//
// 设计红线：中心区域必须 100% 不被改动。
// 只在画面外围做"压暗 + 紫色辉光 + 噪声脉络"，因此再强也不会挡住玩家看怪。
//
// ⚠ 参数名必须与 Terraria.Graphics.Shaders.ScreenShaderData 的约定完全一致，
//   少一个参数引擎 SetValue 时就是 NullReference。这里按 Polarities/Global/Filters.fx 的清单写全。

sampler uImage0 : register(s0);   // 屏幕内容
sampler uImage1 : register(s1);   // 噪声（C# 侧用 UseImage 塞进来）
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);

float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 VeilPass(float2 coords : TEXCOORD0) : COLOR0
{
    float4 screen = tex2D(uImage0, coords);

    // 0 = 画面正中，1 = 四角
    float2 centered = coords - 0.5;
    float dist = length(centered) * 1.41421356;
    float edge = saturate((dist - 0.52) * 2.0);
    edge = edge * edge;

    // 噪声脉络：跟着屏幕走，不随相机抖动
    float2 noiseUv = coords * (uScreenResolution / 220.0)
                   + float2(uTime * 0.06, uTime * -0.04);
    float n = tex2D(uImage1, noiseUv).r;
    float vein = saturate((n - 0.52) * 3.2) * edge;

    float strength = uIntensity * uOpacity;

    // 1) 外围极轻压暗，把注意力收回画面中点（中心 edge=0，完全不动）
    float dark = 1.0 - edge * 0.22 * strength;

    // 2) 外围紫色辉光 + 脉络，叠在压暗之上
    float3 glow = uColor * (edge * 0.42 + vein * 0.85) * strength;
    float3 outc = screen.rgb * dark + glow * (0.55 + 0.45 * screen.rgb);

    // 3) 刚穿齐/激怒的一瞬间，外围再补一点点色。
    //    必须乘 edge 与 strength：否则中心像素也变色，而且强度调到 0 时脉冲还在（违背"不挡视野"的承诺）。
    outc += uColor * uProgress * 0.05 * screen.rgb * edge * strength;

    return float4(outc, screen.a);
}

technique Technique1
{
    pass VeilPass
    {
        PixelShader = compile ps_2_0 VeilPass();
    }
}
