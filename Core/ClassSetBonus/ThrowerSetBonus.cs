using DemonShadeCrossMod.Core.ClassSetBonus;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace DemonShadeCrossMod.Core.Players
{
    //26.10.24 最大疲劳值调整，不再强制要求覆盖灾瑟拓展，才能生效
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class ThrowerSetBonus : SetBonusBase
    {
        internal override bool IsEnabled => ModLoader.HasMod("ThoriumMod") && Cfg.EnableThrowerSet;
        internal override bool DisplayTooltip => ClientCfg.ThrowerDisplay;
        internal override string LocalizationKey => "Thrower";
        internal override object[] TooltipArgs => [
            Cfg.ThrowerTechPointsBonus,
            Cfg.ThrowerExhaustionMaxBonus,
            (float)Cfg.ThrowerExhaustionRegenBonus,
            Cfg.ThrowerTechRechargeBonus,
            ];
        internal override int DisplayOrder => ClientCfg.ThrowerOrder;
        //public float ThrowerExhaustionRegenBonus => (float)DSConfig.Instance.ThrowerExhaustionRegenBonus;
        //public int ThrowerTechRechargeBonus => DSConfig.Instance.ThrowerTechRechargeBonus;
        //public int ThrowerTechPointsBonus => DSConfig.Instance.ThrowerTechPointsBonus;
        public override bool InstancePerEntity => true;
        internal override void ApplyBonus(Player player)
        {
            ThoriumPlayer modPlayer = player.GetThoriumPlayer();
            modPlayer.techPointsMax += Cfg.ThrowerTechPointsBonus;
            modPlayer.techRechargeBonus += Cfg.ThrowerTechRechargeBonus;
            modPlayer.throwerExhaustionRegenBonus += (float)Cfg.ThrowerExhaustionRegenBonus / 100;
            modPlayer.throwerExhaustionMax += Cfg.ThrowerExhaustionMaxBonus;
        }
    }
}
