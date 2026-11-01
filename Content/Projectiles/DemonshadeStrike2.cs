using CalamityMod.Dusts;
using ClickerClass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Projectiles
{
    [JITWhenModsEnabled("ClickerClass")]
    [ExtendsFromMod("ClickerClass")]
    //26.10.28 魔影焰击重做，采用光晕绘制
    public class DemonshadeStrike2 : ModProjectile
    {

        public int maxTime = 70; //靠近所需时间

        public int maxRadius = 400; //最大范围

        public int rotationSpeed = 10;//基础角速度

        public float accelerationStart = 0.7f; //经过总时长的多少后开始加速

        public float endRotationMultiplier = 4f; //结束时的角速度倍数

        //实际弹幕围绕中心，初始化为光标位置
        public Vector2 followCenter = Main.MouseWorld;

        //延迟一段时上光标因素 ，1为立即追
        public float delayHoming = 0.084f;

        //随机相位
        public float randomPhase = Main.rand.NextFloat(MathHelper.Pi / 2);

        //计时器，可能会用于联机同步
        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        //固定相位，配合套装点击效果传入参数
        public float ConstantPhase
        {
            get => Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public static Asset<Texture2D> GlowFlare2 { get; private set; }

        public Player Player => Main.player[Projectile.owner];

        public override void Load()
        {
            if (!Main.dedServ)
            {
                //这样写不需要加上模组，ModContent.Request<T>需要加上模组名称
                //已注册贴图不需要额外注册
                GlowFlare2 = Mod.Assets.Request<Texture2D>("Assets/GlowFlare2");
            }
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;//帧图4帧动画
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;//记录弹幕拖尾数量
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;//模式0仅记录弹幕位置
        }

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 44;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();
            Projectile.penetrate = -1; //无限穿透
            Projectile.maxPenetrate = -1;
            Projectile.tileCollide = false; //穿墙
            Projectile.ignoreWater = true;
            //这里统一先放大
            Projectile.scale *= 1.3f;

            //Projectile.gfxOffY = -2; //仅影响视觉效果，用PreDraw需要手动加

            switch (DSConfig.Instance.ClickerImmunityFrameType)
            {
                case DSConfig.ImmunityFrameType.Static:
                    Projectile.usesIDStaticNPCImmunity = true; //同种弹幕
                    Projectile.idStaticNPCHitCooldown = 3;
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
            //记录起始位置
            SoundStyle style = new SoundStyle("CalamityMod/Sounds/Custom/Providence/ProvidenceHolyBlastImpact");
            SoundEngine.PlaySound(style with { Volume = 100f }, Player.Center); //建议直接在玩家处播放
            //初始化时间
            Timer = maxTime;
        }

        public override void AI()
        {

            Projectile.frameCounter++; //动画计数器，游戏内几帧后切换新动画
            if (Projectile.frameCounter > 3)
            {
                Projectile.frame++; //当前动画帧
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Type])
            {
                Projectile.frame = 0;
            }

            //结算弹幕死亡
            if (Timer > 0)
            {
                Timer--;
            }
            else
            {
                Projectile.Kill();
            }

            //设置基础速度
            float finalSpeed = rotationSpeed;

            float timeFactor = MathHelper.Clamp((maxTime - Timer) / maxTime, 0, 1); //0 -> 1 线性插值，用clamp避免额外插值
            //随时间缓慢缩小
            int radius = (int)(maxRadius * Math.Pow(1 - timeFactor, 2));
            //经过开始
            float start = MathHelper.Clamp(accelerationStart, 0f, 0.99f);
            //持续时间
            float late = Math.Max(0f, timeFactor - start);
            //加速度、速度不断增加
            finalSpeed = finalSpeed + late * (endRotationMultiplier - finalSpeed / rotationSpeed) / (1 - start);

            float angle = finalSpeed * timeFactor + randomPhase + ConstantPhase;

            followCenter = Vector2.Lerp(followCenter, Main.MouseWorld, MathHelper.Clamp(delayHoming, 0f, 1f));

            Projectile.Center = followCenter + new Vector2(
                (float)Math.Cos(angle) * radius,
                (float)Math.Sin(angle) * radius
            );

            Lighting.AddLight(Projectile.Center, 0.2f, 0.2f, 1f);

            if (Main.rand.NextBool(2))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadowspecBarDust>());
        }

        //弹幕结束引发爆炸
        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center, //最后弹幕结束位置
                Vector2.Zero,
                ModContent.ProjectileType<DemonshadeExplosin>(),
                (int)(DSConfig.Instance.ClickerEffectDamage * 6f),
                0);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SoundStyle style = new SoundStyle("CalamityMod/Sounds/NPCHit/AstrumDeusHit1");
            SoundEngine.PlaySound(style with { Volume = 2f }, Player.position);
        }

        //手动绘制残影
        public override bool PreDraw(ref Color lightColor)
        {
            //取第几帧图
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            //前2参数为spirt图有几列几行，后2参数为取第几列几行的帧图，默认从0开始
            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);
            //帧图中一帧作为绘制中心
            Vector2 drawOrigin = frame.Size() * 0.5f; //图片中心

            int trailLength = Projectile.oldPos.Length;
            //残影
            for (int i = 0; i < trailLength; i++)
            {
                //获得弹幕速度的方向，使得残影间距保持平滑同时可以调整间距大小
                Vector2 direction = i + 1 >= trailLength ? Vector2.Zero : Vector2.Normalize(Projectile.oldPos[i + 1] - Projectile.oldPos[i]);
                //这里避免残影之间过小，导致一直偏移
                if (i + 1 < trailLength && Vector2.Distance(Projectile.oldPos[i], Projectile.oldPos[i + 1]) <= 12)
                {
                    direction = Vector2.Zero;
                }

                //拖尾越后面越小
                float trailProgress = i / (float)(trailLength - 1);
                float trailScale = Projectile.scale * MathHelper.Lerp(1f, 0.45f, trailProgress);

                //弹幕位置为左上角，这里设置处于中心绘制
                //Projectile.gfxOffY 同步原弹幕的位置调整，direction调整残影间距离
                //最后向上提高，使残影叠加升高出现类似火焰效果
                Vector2 drawPos = (Projectile.oldPos[i] - Main.screenPosition) + drawOrigin + new Vector2(0f, Projectile.gfxOffY) + direction * 11.8f * i + new Vector2(0, -6.5f * i);

                //插值计算透明度，越来越透明
                Color color = Projectile.GetAlpha(lightColor) * ((trailLength - i) / (float)trailLength) * 0.4f;

                //跳过初始0序列
                if (!Projectile.oldPos[i].Equals(Projectile.position))
                {
                    Main.spriteBatch.Draw(
                        texture,
                        drawPos,
                        frame,
                        color,
                        Projectile.rotation,
                        drawOrigin,
                        Projectile.scale * trailScale,
                        SpriteEffects.None,
                        0f);
                }
            }

            //保留原有绘制
            return true;
        }

        //光晕额外绘制
        public override void PostDraw(Color lightColor)
        {
            //绘制光晕
            Main.spriteBatch.Draw(
                GlowFlare2.Value,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                null, //基本都填null ，帧图外面切，整图不用切
                new Color(178, 87, 255) * Projectile.Opacity * 0.7f,
                Projectile.rotation,
                GlowFlare2.Size() / 2,
                Projectile.scale * 0.55f,
                SpriteEffects.None,
                0f);
        }
    }
}
