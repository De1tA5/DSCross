using CalamityMod;
using ClickerClass;
using DemonShadeCrossMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core.ClassSetBonus
{
    [JITWhenModsEnabled("ClickerClass")]
    [ExtendsFromMod("ClickerClass")]
    public class ClickerSetBonus : SetBonusBase, ILocalizedModType
    {
        internal override bool IsEnabled => ModLoader.HasMod("ClickerClass") && Cfg.EnableClickerSet;
        internal override bool DisplayTooltip => ClientCfg.ClickerDisplay;
        internal override string LocalizationKey => "Clicker";
        internal override object[] TooltipArgs => [
            Cfg.ClickerRadiusBonus,
            Cfg.ClickerEffectBonus
            ];
        internal override int DisplayOrder => ClientCfg.ClickerOrder;
        //public float ClickerRadiusBonus => (float)DSConfig.Instance.ClickerRadiusBonus;
        //public float ClickerEffectBonus => (float)DSConfig.Instance.ClickerEffectBonus;
        public string LocalizationCategory => "ClickEffect";
        public static string ClickerEffect { get; internal set; } = string.Empty;
        public override bool InstancePerEntity => true;
        public override void SetStaticDefaults()
        {
            ClickerEffect = ClickerSystem.RegisterClickEffect(Mod, "DemonShadeClickerEffect", 10, new Color(218, 105, 233), delegate (Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, int type, int damage, float knockBack)
            {
                if (Main.myPlayer != player.whoAmI || !player.active || player.dead)
                    return;

                for (int i = 0; i < 4; i++)
                {
                    Projectile.NewProjectile(
                        source,
                        position,
                        Vector2.Zero,
                        ModContent.ProjectileType<DemonshadeStrike2>(),
                        6000,
                        0,
                        player.whoAmI,
                        ai1: i * MathHelper.Pi / 2
                        );
                }
            });
        }

        internal override void ApplyBonus(Player player)
        {
            ClickerPlayer modPlayer = player.GetModPlayer<ClickerPlayer>();

            modPlayer.clickerRadius += Cfg.ClickerRadiusBonus / 100;
            //百分比减少点击次数
            modPlayer.clickerBonusPercent -= Cfg.ClickerEffectBonus / 100;

            if (player.Calamity().demonshadeSet && Cfg.ClickerEffect)
            {
                modPlayer.EnableClickEffect(ClickerEffect);
            }
        }
    }
}
