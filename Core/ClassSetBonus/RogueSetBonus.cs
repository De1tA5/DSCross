using CalamityMod;
using DemonShadeCrossMod.Core.ClassSetBonus;
using Terraria;

namespace DemonShadeCrossMod.Core.Players
{
    public class RogueSetBonus : SetBonusBase
    {
        internal override bool IsEnabled => Cfg.EnableStealthMeter;
        internal override bool DisplayTooltip => ClientCfg.RogueDisplay;
        internal override string LocalizationKey => "Rogue";
        internal override object[] TooltipArgs => [
            Cfg.StealthMeterMax,
            Cfg.ExtraStealthGenMoving,
            Cfg.ExtraStealthGenStandStill,
            //弹幕飞行速度
            Cfg.ExtraRogueVelocity
            ];
        internal override int DisplayOrder => ClientCfg.RogueOrder;
        //public float StealthMeterMax => (float)DSConfig.Instance.StealthMeterMax;
        //public float ExtraStealthGenStandStill => (float)DSConfig.Instance.ExtraStealthGenStandStill;
        //public float ExtraStealthGenMoving => (float)DSConfig.Instance.ExtraStealthGenMoving;
        //public float ExtraRogueVelocity => (float)DSConfig.Instance.ExtraRogueVelocity;
        public override bool InstancePerEntity => true;

        internal override void ApplyBonus(Player player)
        {
            //增加穿戴盗贼套装判定
            player.Calamity().wearingRogueArmor = true;

            player.Calamity().rogueVelocity += Cfg.ExtraRogueVelocity / 100;
            player.Calamity().rogueStealthMax += Cfg.StealthMeterMax / 100;
            player.Calamity().stealthGenStandstill += Cfg.ExtraStealthGenStandStill / 100;
            player.Calamity().stealthGenMoving += Cfg.ExtraStealthGenMoving / 100;
        }
    }
}
