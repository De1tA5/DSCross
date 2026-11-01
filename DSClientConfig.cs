using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DemonShadeCrossMod
{
    //26.10.20 文本顺序以及特效的客户端配置
    //26.10.24 视觉效果轮廓光添加配置文件，进一步提升套装文本可配置性
    public class DSClientConfig : ModConfig
    {
        public static DSClientConfig Instance;
        public override ConfigScope Mode => ConfigScope.ClientSide;

        //0为原套装奖励
        #region 文本顺序

        [Header("TooltipOrder")]

        [DefaultValue(true)]
        public bool RogueDisplay { get; set; }

        [Range(1, 100)]
        [DefaultValue(1)]
        public int RogueOrder { get; set; } = 1;

        [DefaultValue(true)]
        public bool HealerDisplay { get; set; }

        [Range(1, 100)]
        [DefaultValue(2)]
        public int HealerOrder { get; set; } = 2;

        [DefaultValue(true)]
        public bool BardDisplay { get; set; }

        [Range(1, 100)]
        [DefaultValue(3)]
        public int BardOrder { get; set; } = 3;

        [DefaultValue(true)]
        public bool ThrowerDisplay { get; set; }

        [Range(1, 100)]
        [DefaultValue(4)]
        public int ThrowerOrder { get; set; } = 4;

        [DefaultValue(true)]
        public bool ClickerDisplay { get; set; }

        [Range(1, 100)]
        [DefaultValue(5)]
        public int ClickerOrder { get; set; } = 5;

        [DefaultValue(true)]
        public bool VoidDisplay { get; set; }

        [Range(1, 100)]
        [DefaultValue(6)]
        public int VoidOrder { get; set; } = 6;

        #endregion

        #region 视觉特效
        [Header("VFX")]

        [DefaultValue(true)]
        public bool DemonshadeOutline { get; set; }

        [DefaultValue(false)]
        public bool AllArmorOutline { get; set; }

        [DefaultValue(true)]
        public bool ShadowOutline { get; set; }

        //Alpha传入无用，使用原像素Alpha
        //默认魔影配色
        [DefaultValue(typeof(Color), "178, 87, 255, 255")]
        public Color OutlineColor { get; set; }

        [DefaultValue(1.25f)]
        [Range(0, 2.5f)]
        public float OutlineCoreWidth { get; set; }

        [DefaultValue(3f)]
        [Range(0, 4.5f)]
        public float OutlineHaloWidth { get; set; }

        [DefaultValue(0.24f)]
        [Range(0, 1f)]
        //仅外晕强度
        public float OutlineHaloStrength { get; set; }

        [DefaultValue(0.78f)]
        [Range(0, 1f)]
        public float OutlineIntensity { get; set; }

        [DefaultValue(false)]
        public bool DrawDataTest { get; set; }
        #endregion
    }
}
