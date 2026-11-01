using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DemonShadeCrossMod
{
    public class DSConfig : ModConfig
    {
        //public override string LocalizationCategory => "Configs.DSConfig";
        public static DSConfig Instance;
        public override ConfigScope Mode => ConfigScope.ServerSide;
        //26.10.20 增加不同模组背景颜色，以及字体颜色
        //26.10.24 增加模组Icon
        //26.10.27 点击效果详细配置
        #region  灾厄
        [Header("Calamity")]

        [BackgroundColor(192, 54, 64, 192)]
        [Range(1, 100)]
        [DefaultValue(1)]
        public int TriggerTime { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(false)]
        public bool RemoveEnrageSound { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool ReAddRedDevil { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool DemonFlamesBack { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool RemoveDsSetNerf { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [DefaultValue(true)]
        public bool EnableStealthMeter { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [Range(0, 10000)]
        [DefaultValue(100)]
        public int StealthMeterMax { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [Range(0, 1000)]
        [DefaultValue(0)]
        public int ExtraStealthGenMoving { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [Range(0, 1000)]
        [DefaultValue(0)]
        public int ExtraStealthGenStandStill { get; set; }

        [BackgroundColor(192, 54, 64, 192)]
        [Range(0, 1000)]
        [DefaultValue(0)]
        public int ExtraRogueVelocity { get; set; }

        #endregion

        #region  瑟银
        [Header("Thorium")]

        [BackgroundColor(0, 207, 213)]
        [ReloadRequired]
        [DefaultValue(false)]
        public bool OverrideCalBardHealer { get; set; }

        [BackgroundColor(0, 207, 213)]
        [DefaultValue(true)]
        public bool EnableHealerSet { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 100)]
        [DefaultValue(3)]
        public int HealBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 1000)]
        [DefaultValue(0)]
        public int HealToolSpeed { get; set; }

        [BackgroundColor(0, 207, 213)]
        [DefaultValue(true)]
        public bool EnableBardSet { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 999)]
        [DefaultValue(3)]
        public int BardInspirationRegenBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 999)]
        [DefaultValue(3)]
        public int BardResourceDropBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 60)]
        [DefaultValue(0)]
        public int BardResourceBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 999)]
        [DefaultValue(0)]
        public int BardBuffDurationBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 60)]
        [DefaultValue(0)]
        public int ExtraBardBuffDuration { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 60)]
        [DefaultValue(0)]
        public int BardRangeBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 10)]
        [DefaultValue(2)]
        public int ExtraLevelBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [DefaultValue(true)]
        public bool EnableThrowerSet { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 2)]
        [DefaultValue(2)]
        public int ThrowerTechPointsBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 10000)]
        [DefaultValue(1000)]
        public int ThrowerExhaustionMaxBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 1000)]
        [DefaultValue(50)]
        public int ThrowerExhaustionRegenBonus { get; set; }

        [BackgroundColor(0, 207, 213)]
        [Range(0, 100)]
        [DefaultValue(50)]
        public int ThrowerTechRechargeBonus { get; set; }

        #endregion

        #region 点击者
        [Header("ClickerClass")]

        [BackgroundColor(231, 172, 82)]
        [DefaultValue(true)]
        public bool EnableClickerSet { get; set; }

        [BackgroundColor(231, 172, 82)]
        [Range(0, 1000)]
        [DefaultValue(200)]
        public int ClickerRadiusBonus { get; set; }

        [BackgroundColor(231, 172, 82)]
        [Range(0, 1000)]
        [DefaultValue(30)]
        public int ClickerEffectBonus { get; set; }

        [BackgroundColor(231, 172, 82)]
        [DefaultValue(true)]
        public bool ClickerEffect { get; set; }

        [BackgroundColor(231, 172, 82)]
        [DefaultValue(6000)]
        public int ClickerEffectDamage { get; set; }

        [Dropdown]
        [BackgroundColor(231, 172, 82)]
        [DefaultValue(ImmunityFrameType.Static)]
        public ImmunityFrameType ClickerImmunityFrameType { get; set; }

        public enum ImmunityFrameType
        {
            Global, //全局无敌帧，全体玩家共享一个冷却
            Static, //静态无敌帧，同一种弹幕共享一个冷却
            Local //独立无敌帧，同一个弹幕每个独立计算冷却，同一个实例弹幕
        }

        #endregion

        #region 暗影奥秘
        [Header("SOTS")]

        [BackgroundColor(166, 73, 160)]
        [DefaultValue(true)]
        public bool EnableVoidSet { get; set; }

        [BackgroundColor(166, 73, 160)]
        [Range(0, 1000)]
        [DefaultValue(100)]
        public int VoidMax2Bonus { get; set; }

        [BackgroundColor(166, 73, 160)]
        [Range(0, 1000)]
        [DefaultValue(50)]
        public int VoidRegenBonus { get; set; }

        [BackgroundColor(166, 73, 160)]
        [Range(0, 100)]
        [DefaultValue(5)]
        public int VoidGainBonus { get; set; }
        #endregion
    }
}
