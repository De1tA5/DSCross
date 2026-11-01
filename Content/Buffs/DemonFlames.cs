using CalamityMod.DataStructures;
using CalamityMod.Particles;
using CalamityMod.Systems.Collections;
using DemonShadeCrossMod.Content.Dusts;
using DemonShadeCrossMod.Core;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Buffs
{
    //26.10.22 使用着色器粒子
    public class DemonFlames : ModBuff
    {
        public static DebuffData debuffData = new DebuffData()
        {
            HeatDebuffScaling = 1f
        };

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            BuffID.Sets.LongerExpertDebuff[Type] = true;
            CalamityBuffSets.DebuffDataset[Type] = debuffData;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<DSNPC>().demonFlames = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<DSPlayer>().demonFlames = true;
        }

        internal static void DrawEffects(NPC npc, ref Color drawColor)
        {

            Color fadeColor = Color.Lerp(Color.BlueViolet, Color.HotPink, Main.rand.NextFloat(1f));

            if (Main.rand.NextBool(2))
            {
                Dust glowDust = Dust.NewDustDirect(
                    npc.position,//起始点
                    npc.width,
                    npc.height,
                    ModContent.DustType<GlowFlare>());

                //上升粒子
                glowDust.velocity.Y -= 1.8f;
                glowDust.velocity.Y *= 1.5f;
                glowDust.scale *= 1.1f;
                glowDust.color = Main.rand.NextBool(3) ? Color.BlueViolet : Color.DarkRed;
                Lighting.AddLight(npc.position, 0.05f, 0.01f, 0.01f);
            }

            if (Main.rand.NextBool(6))
            {
                {
                    Dust sparkleDust = Dust.NewDustDirect(
                    npc.position - new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(-npc.height / 1.5f, npc.height / 1.5f)),
                    npc.width,
                    npc.height,
                    ModContent.DustType<SparkleDust>());

                    sparkleDust.noGravity = true;
                    sparkleDust.velocity = Vector2.UnitY * Main.rand.NextFloat(1, -4);
                    sparkleDust.scale *= 1.8f;
                    sparkleDust.color = fadeColor;
                }
            }

            if (Main.rand.NextBool())
            {
                Vector2 position = new(npc.position.X + Main.rand.NextFloat(npc.width), npc.position.Y + Main.rand.NextFloat(npc.height));
                VoidSparkParticle voidSpark = new(position, -Vector2.UnitY.RotatedByRandom(MathHelper.Pi / 10f) * Main.rand.NextFloat(10f, 18f), false, 20, Main.rand.NextFloat(0.02f, 0.05f), Main.rand.NextBool() ? Color.Indigo : Color.Lerp(Color.Violet, Color.DarkBlue, 0.35f));
                GeneralParticleHandler.SpawnParticle(voidSpark);
            }
        }
    }
}
