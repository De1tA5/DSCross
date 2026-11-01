using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Dusts;
using CalamityMod.Items.Armor.Demonshade;
using DemonShadeCrossMod.Content.Buffs;
using DemonShadeCrossMod.Content.Projectiles;
using DemonShadeCrossMod.Core.Utils;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core
{
    public class DSPlayer : ModPlayer
    {
        public bool RedDevil;
        public bool demonFlames;
        public int triggerTimer;
        public bool demonFlamesAttack;

        public override void ResetEffects()
        {
            RedDevil = DSConfig.Instance.ReAddRedDevil;
            demonFlames = false;
            demonFlamesAttack = DSConfig.Instance.DemonFlamesBack;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.Calamity().demonshadeSet && demonFlamesAttack)
            {
                target.AddBuff(ModContent.BuffType<DemonFlames>(), 3 * 60);
            }
            if (target.GetGlobalNPC<DSNPC>().demonFlames)
            {
                modifiers.FinalDamage += 0.1f;
            }
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.Calamity().demonshadeSet && demonFlamesAttack)
            {
                target.AddBuff(ModContent.BuffType<DemonFlames>(), 3 * 60);

            }
            if (target.GetGlobalNPC<DSNPC>().demonFlames)
            {
                modifiers.FinalDamage += 0.1f;
            }
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Player.Calamity().demonshadeSet && demonFlamesAttack)
            {
                target.AddBuff(ModContent.BuffType<DemonFlames>(), 3 * 60);
            }
            if (target.GetGlobalNPC<DSNPC>().demonFlames)
            {
                modifiers.FinalDamage += 0.1f;
            }
        }
        public override void PostUpdateEquips()
        {

            if (Player.Calamity().demonshadeSet)
            {
                //魔影激怒调整
                DemonshadeHelm.MultDamageBoost = DSConfig.Instance.RemoveDsSetNerf ? 1.25f : 0.5F;

                //生成红恶魔
                if (RedDevil)
                {
                    if (Player.ownedProjectileCounts[ModContent.ProjectileType<DemonshadeRedDevil>()] < 1)
                    {
                        Projectile.NewProjectile(
                            Player.GetSource_FromThis(),
                            Player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<DemonshadeRedDevil>(),
                            10000,
                            0);
                    }
                }

                //免疫破碎之誓
                if (IEoR.Load)
                {
                    ModContent.TryFind<ModBuff>(IEoR.Name, "BrokenOath", out ModBuff buff);
                    Player.buffImmune[buff.Type] = true;
                }
            }
        }
        public override void UpdateBadLifeRegen()
        {
            if (demonFlames)
            {
                if (Player.lifeRegen > 0)
                    Player.lifeRegen = 0;
                Player.lifeRegenTime = 0;
                //lifeRegen 每秒扣血要除2 - 若每秒扣除100血 要设为200
                Player.lifeRegen -= 100 * 2;
            }
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            //单点
            if (KeybindSystem.EnrageKeybind.JustPressed)
            {
                DemonShadeEnrage(Player);
            }

            //持续按住
            if (KeybindSystem.EnrageKeybind.Current)
            {

                triggerTimer++;
                if (triggerTimer >= 60 * DSConfig.Instance.TriggerTime)
                {
                    DemonShadeEnrage(Player);
                    triggerTimer = 0;
                }
            }

            //释放后重置计时器
            if (KeybindSystem.EnrageKeybind.JustReleased)
            {
                triggerTimer = 0;
            }
        }
        public static void DemonShadeEnrage(Player player)
        {
            if (player.Calamity().demonshadeSet)
            {
                //关闭激怒声音
                if (!DSConfig.Instance.RemoveEnrageSound)
                {
                    SoundEngine.PlaySound(DemonshadeHelm.ActivationSound, player.Center);
                }

                for (int i = 0; i < 36; i++)
                {
                    Dust brimDust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y + 16f), player.width, player.height - 16, (int)CalamityDusts.Brimstone, 0f, 0f, 0, default, 1f);
                    brimDust.velocity *= 3f;
                    brimDust.scale *= 1.15f;
                }

                int dustAmt = 36;
                for (int j = 0; j < dustAmt; j++)
                {
                    Vector2 dustRotation = Vector2.Normalize(player.velocity) * new Vector2((float)player.width / 2f, (float)player.height) * 0.75f;
                    dustRotation = dustRotation.RotatedBy((double)((float)(j - (dustAmt / 2 - 1)) * MathHelper.TwoPi / (float)dustAmt), default) + player.Center;
                    Vector2 dustVelocity = dustRotation - player.Center;
                    Dust brimDust2 = Dust.NewDustDirect(dustRotation + dustVelocity, 0, 0, (int)CalamityDusts.Brimstone, dustVelocity.X * 1.5f, dustVelocity.Y * 1.5f, 100, default, 1.4f);
                    brimDust2.noGravity = true;
                    brimDust2.noLight = true;
                    brimDust2.velocity = dustVelocity;
                }

                if (player.whoAmI == Main.myPlayer)
                {
                    player.AddBuff(ModContent.BuffType<Enraged>(), DemonshadeHelm.EnrageDuration, false);
                }
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    foreach (NPC npc in Main.ActiveNPCs)
                    {
                        if (!npc.friendly && !npc.dontTakeDamage && Vector2.Distance(player.Center, npc.Center) <= 3000f)
                            npc.AddBuff(ModContent.BuffType<Enraged>(), DemonshadeHelm.EnrageDuration, false);
                    }
                }
            }
        }
    }
}
