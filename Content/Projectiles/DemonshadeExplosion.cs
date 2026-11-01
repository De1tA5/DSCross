using CalamityMod;
using ClickerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Projectiles
{
    //26.11.1 使用灰度图，完成魔影爆破特效绘制
    [JITWhenModsEnabled("ClickerClass")]
    [ExtendsFromMod("ClickerClass")]
    public class DemonshadeExplosin : ModProjectile
    {
        //控制判定大小和视觉大小
        public float explosionRadius = 600f;

        public float maxTime = 20f;

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public Player Player => Main.player[Projectile.owner];

        public static Asset<Texture2D> Explosion { get; private set; }
        public static Asset<Texture2D> BloomCircle { get; private set; }
        public static Asset<Texture2D> GlowFlare2 { get; private set; }

        //原版空弹幕
        public override string Texture => "Terraria/Images/Projectile_0";

        public override void Load()
        {
            //客户端加载
            if (!Main.dedServ)
            {
                Explosion = Mod.Assets.Request<Texture2D>("Assets/Bomb");
                BloomCircle = Mod.Assets.Request<Texture2D>("Assets/BloomCircle");
                GlowFlare2 = Mod.Assets.Request<Texture2D>("Assets/GlowFlare2");
            }
        }

        public override void SetDefaults()
        {
            Projectile.height = Projectile.width = (int)explosionRadius;
            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
            Projectile.penetrate = -1; //无限穿透
            Projectile.maxPenetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            switch (DSConfig.Instance.ClickerImmunityFrameType)
            {
                case DSConfig.ImmunityFrameType.Static:
                    Projectile.usesIDStaticNPCImmunity = true; //同种弹幕
                    Projectile.idStaticNPCHitCooldown = 5;
                    break;
                case DSConfig.ImmunityFrameType.Local:
                    Projectile.usesLocalNPCImmunity = true; //同一个弹幕
                    Projectile.localNPCHitCooldown = 3; //-1为每个NPC只能被该弹幕命中一次，10则该弹幕10帧以后才能继续攻击
                    break;
                case DSConfig.ImmunityFrameType.Global:
                default:
                    break;
            }
        }

        public override void OnSpawn(IEntitySource source)
        {
            Timer = maxTime;

            //上个版本调用灾厄粒子库
            //Particle explosion = new DetailedExplosion(Projectile.Center, Vector2.Zero, Color.BlueViolet, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, explosionRadius * 0.0065f + 0.1f, (int)Timer - Main.rand.Next(10, 15));
            //GeneralParticleHandler.SpawnParticle(explosion, false, GeneralDrawLayer.BeforeNPCs);
            //Particle explosion2 = new DetailedExplosion(Projectile.Center, Vector2.Zero, Color.Black, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, explosionRadius * 0.0045f + 0.1f, (int)Timer - Main.rand.Next(10, 15), false);
            //GeneralParticleHandler.SpawnParticle(explosion2, false, GeneralDrawLayer.BeforeNPCs);
            //Particle explosion3 = new DetailedExplosion(Projectile.Center, Vector2.Zero, Color.Black, Vector2.One, Main.rand.NextFloat(-5, 5), 0f, explosionRadius * 0.0030f + 0.1f, (int)Timer - Main.rand.Next(10, 15), false);
            //GeneralParticleHandler.SpawnParticle(explosion3, false, GeneralDrawLayer.BeforeNPCs);

            SoundStyle style = new SoundStyle("CalamityMod/Sounds/Custom/PumpkinExplodeGFB2");
            SoundEngine.PlaySound(style with { Volume = 1.7f }, Projectile.position);
            //屏幕震动通过 Main.screenPosition += Main.rand.NextVector2Circular()来实现
            //这里直接调用灾厄封装好的屏幕震动
            Player.SetScreenshake(9f);
        }

        public override bool ShouldUpdatePosition() => false;

        //PreDraw绑定AI
        public override bool PreDraw(ref Color lightColor)
        {
            if (Main.dedServ)
                return false;

            Vector2 center = Projectile.Center - Main.screenPosition;
            //从0到1经过时间
            float elapsed = maxTime - Timer;
            float progress = MathHelper.Clamp(elapsed / maxTime, 0f, 1f);

            //逐渐消失
            float fade = (float)Math.Pow(1f - progress, 2f);
            //爆炸，3次方消失
            float burst = EaseOutCubic(MathHelper.Clamp(elapsed / 8f, 0f, 1f));
            //所有光照总体距离
            float diameter = MathHelper.Lerp(90f, explosionRadius * 6, burst);
            //收缩白芯
            float flash = (float)Math.Pow(MathHelper.Clamp(1f - elapsed / 5f, 0f, 1f), 2f);
            float coreFade = (float)Math.Pow(MathHelper.Clamp(1f - elapsed / 7f, 0f, 1f), 2f);

            //中心横向，紫色
            DrawLight(Explosion.Value, center, new Vector2(diameter * 0.7f), new Color(126, 38, 255), fade * 0.01f);
            DrawLight(Explosion.Value, center, new Vector2(diameter * 0.7f), new Color(183, 88, 255), fade * 0.6f, 0f);

            //十字光，紫色，拉伸
            Vector2 flareSize = new Vector2(diameter * 1.25f, diameter * 0.5f);
            DrawLight(GlowFlare2.Value, center, flareSize, new Color(232, 195, 255), coreFade * 0.9f);
            DrawLight(GlowFlare2.Value, center, flareSize * 0.7f, new Color(201, 134, 255), coreFade * 0.4f, MathHelper.PiOver2);

            DrawLight(Explosion.Value, center, new Vector2(MathHelper.Lerp(90f, 230f, burst)), Color.White, flash * 0.9f);

            return false;

        }

        public override void AI()
        {
            if (Timer >= 0)
            {
                Timer--;
            }
            else
            {
                Projectile.Kill();
            }

            float progress = MathHelper.Clamp(Timer / maxTime, 0f, 1f);
            float strength = (float)Math.Pow(1f - progress, 2f);
            Lighting.AddLight(Projectile.Center, new Vector3(0.65f, 0.22f, 1f) * strength * 2f);
        }

        //3次方缓出
        private static float EaseOutCubic(float value)
        {
            float remaining = 1f - value;
            return 1f - remaining * remaining * remaining;
        }

        //光绘制,使用灰度图
        private static void DrawLight(Texture2D texture, Vector2 position, Vector2 size, Color color, float strength, float rotation = 0f)
        {
            if (strength <= 0.001f)
                return;

            //默认使用AlphaBlend
            //RGB 保留、A=0 可叠加光照
            //黑色不覆盖背景，灰度控制光强，白色区域最亮
            //出现四方框框，需预处理灰度图四个角落，设置为纯黑即（0，0，0）
            //必须先保留 RGB 再将 A 清零，不能把整个颜色乘以0
            Color light = color * strength;
            light.A = 0;
            Vector2 scale = new Vector2(size.X / texture.Width, size.Y / texture.Height);

            Main.EntitySpriteDraw(
                texture,
                position,
                null,
                light,
                rotation,
                texture.Size() * 0.5f,
                scale,
                SpriteEffects.None,
                0f);
        }
    }
}
