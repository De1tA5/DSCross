using CalamityMod.Particles;
using DemonShadeCrossMod.Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core
{
    //26.10.21 强化恶魔之焰减益
    //26.10.24 使用灾厄自带的火焰粒子绘制
    public class DSNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool demonFlames;
        public FireParticleSet DemonFlamesFireDrawer = null;

        public override void ResetEffects(NPC npc)
        {
            demonFlames = false;
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (demonFlames)
            {
                if (npc.lifeRegen > 0)
                {
                    npc.lifeRegen = 0;
                }
                //原版恶魔之焰为1250
                npc.lifeRegen -= 1250 * 2;
                //跳出伤害文本，仅有显示作用，数值越小，跳出频率越高
                damage = 1250;
            }
        }

        public override bool PreAI(NPC npc)
        {
            //每帧执行
            DemonFlamesFireDrawer?.Update();

            return true;
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (demonFlames)
            {
                DemonFlames.DrawEffects(npc, ref drawColor);
                drawColor = new Color(104, 56, 195);
            }
        }

        //在PreAI后
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            //服务器禁用绘制
            if (Main.dedServ)
                return true;

            if (demonFlames)
            {
                float compactness = npc.width * 1.2f;
                if (compactness < 20f)
                    compactness = 20f;
                float power = npc.height / 100f;
                if (power > 2.75f)
                    power = 2.75f;

                if (DemonFlamesFireDrawer is null || DemonFlamesFireDrawer.LocalTimer >= DemonFlamesFireDrawer.SetLifetime)
                    //查询是否有在加上buff持续时间
                    DemonFlamesFireDrawer = new FireParticleSet(4 * 60, 1, Color.DarkMagenta, Color.Indigo, compactness, power);

                else
                    DemonFlamesFireDrawer.DrawSet(npc.Bottom - Vector2.UnitY * (12f - npc.gfxOffY));
            }
            else
                DemonFlamesFireDrawer = null;

            return true;
        }
    }
}
