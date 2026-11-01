using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core.ClassSetBonus
{
    [JITWhenModsEnabled("SOTS")]
    [ExtendsFromMod("SOTS")]
    public class VoidSetBonus : SetBonusBase
    {
        internal override bool IsEnabled => ModLoader.HasMod("SOTS") && Cfg.EnableVoidSet;
        internal override bool DisplayTooltip => ClientCfg.VoidDisplay;
        internal override string LocalizationKey => "Void";
        internal override object[] TooltipArgs => [
            Cfg.VoidMax2Bonus,
            (float)Cfg.VoidRegenBonus,
            (float)Cfg.VoidGainBonus
            ];
        internal override int DisplayOrder => ClientCfg.VoidOrder;
        //public int VoidMax2Bonus => DSConfig.Instance.VoidMax2Bonus;
        //public int VoidMeterRegenBonus => DSConfig.Instance.VoidRegenBonus;
        //public int VoidMeterGainBonus => DSConfig.Instance.VoidGainBonus;
        public override bool InstancePerEntity => true;

        internal override void ApplyBonus(Player player)
        {
            VoidPlayer modplayer = player.GetModPlayer<VoidPlayer>();
            modplayer.voidMeterMax2 += Cfg.VoidMax2Bonus;
            modplayer.voidRegenSpeed += (float)Cfg.VoidRegenBonus / 100;
            modplayer.bonusVoidGain += (float)Cfg.VoidGainBonus;
        }
    }
}
