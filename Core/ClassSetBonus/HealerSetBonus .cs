using DemonShadeCrossMod.Core.ClassSetBonus;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace DemonShadeCrossMod.Core.Players
{
    //26.10.24 不再强制要求覆盖灾瑟拓展，才能生效
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class HealerSetBonus : SetBonusBase
    {
        internal override bool IsEnabled => ModLoader.HasMod("ThoriumMod") && Cfg.EnableHealerSet;
        internal override bool DisplayTooltip => ClientCfg.HealerDisplay;
        internal override string LocalizationKey => "Healer";
        internal override object[] TooltipArgs => [
            Cfg.HealBonus,
            (float)Cfg.HealToolSpeed, Cfg.HealToolSpeed,
            ];
        internal override int DisplayOrder => ClientCfg.HealerOrder;
        //public int HealBonus => DSConfig.Instance.HealBonus;
        //public float HealToolSpeed => (float)DSConfig.Instance.HealToolSpeed;
        public override bool InstancePerEntity => true;

        internal override void ApplyBonus(Player player)
        {
            ThoriumPlayer modPlayer = player.GetThoriumPlayer();
            modPlayer.healBonus += Cfg.HealBonus;
            player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += Cfg.HealToolSpeed / 100;
        }
    }
}
