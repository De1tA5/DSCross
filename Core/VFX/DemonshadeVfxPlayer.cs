//using CalamityMod;
//using DemonShadeCrossMod.Content.Dusts;
//using Microsoft.Xna.Framework;
//using System;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace DemonShadeCrossMod.Core.VFX
//{
//    /// <summary>
//    /// 魔影套的"特效驱动"：只负责按节奏往 <see cref="DemonshadeAuraSystem"/> 里丢粒子，
//    /// 以及给屏幕滤镜提供强度值。所有绘制都在 Content/Effects 里，这里不碰任何 SpriteBatch。
//    ///
//    /// 视野友好原则：
//    ///   1. 世界层元素画在 PostDrawTiles（玩家与弹幕之前），用加法混合 —— 加法只会提亮，物理上无法遮挡；
//    ///   2. 粒子只在身体周围的椭圆内生成、向上飘，alpha 上限受总强度控制；
//    ///   3. 屏幕滤镜只处理画面外围，正中 edge = 0，逐像素原样返回。
//    /// </summary>
//    public class DemonshadeVfxPlayer : ModPlayer
//    {
//        private bool hadFullSet;
//        private int emberTimer;
//        private int streakTimer;
//        private int glintTimer;
//        private float sigilStrength;
//        private float veilPulse;

//        /// <summary>脚下法阵的当前强度（0..1），由 <see cref="DemonshadeAuraSystem"/> 读取。</summary>
//        public float SigilStrength => sigilStrength;

//        /// <summary>面纱的瞬时脉冲（0..1），穿齐 / 激怒时置 1 后缓慢衰减。</summary>
//        public float VeilPulse => veilPulse;

//        public override void PostUpdate()
//        {
//            if (veilPulse > 0f)
//                veilPulse = MathF.Max(0f, veilPulse - 1f / 80f);

//            // 视觉设置是 ClientSide 配置：别人服务器上开了什么特效，不该影响你自己关不关
//            //DSVisualConfig cfg = DSVisualConfig.Instance;

//            float intensity = cfg is null ? 0f : MathHelper.Clamp(cfg.SetVfxIntensity / 100f, 0f, 1f);

//            // 强度为 0 等同于关闭：不再生成新粒子，存量自然淡出
//            bool fullSet = cfg is not null
//                        && cfg.EnableSetVfx
//                        && intensity > 0f
//                        && Player.active
//                        && !Player.dead
//                        && Player.Calamity().demonshadeSet;

//            // 刚穿齐的那一帧：一圈冲击环 + 面纱脉冲，作为"套装生效"的即时反馈
//            if (fullSet && !hadFullSet)
//            {
//                SpawnShockRing(Player.Center, 1.15f, intensity);
//                veilPulse = 1f;
//            }

//            hadFullSet = fullSet;

//            // 法阵强度渐变，避免穿脱瞬间硬切换
//            float target = fullSet && cfg.EnableGroundSigil ? intensity : 0f;
//            sigilStrength = MathHelper.Lerp(sigilStrength, target, 0.10f);

//            if (!fullSet)
//                return;

//            // 屏幕外不为这个玩家烧随机数、三角函数和全局粒子名额（联机时别人在地图另一头）
//            if (!IsNearScreen())
//                return;

//            SpawnEmberTick(intensity);
//            SpawnStreakTick(intensity);
//            SpawnGlintTick(cfg, intensity);
//        }

//        /// <summary>粗裁剪：屏幕外半屏内都不生成。放大缩放时也不会误判（留了一整屏余量）。</summary>
//        private bool IsNearScreen()
//        {
//            Rectangle area = new Rectangle(
//                (int)Main.screenPosition.X,
//                (int)Main.screenPosition.Y,
//                Main.screenWidth,
//                Main.screenHeight);

//            area.Inflate(Main.screenWidth / 2, Main.screenHeight / 2);

//            return area.Contains(Player.Center.ToPoint());
//        }

//        private void SpawnEmberTick(float intensity)
//        {
//            if (++emberTimer < 4)
//                return;

//            emberTimer = 0;

//            if (DemonshadeAuraSystem.Instance is not { } system)
//                return;

//            Vector2 position = Player.Center + new Vector2(
//                Main.rand.NextFloat(-Player.width * 0.85f, Player.width * 0.85f),
//                Main.rand.NextFloat(-Player.height * 0.55f, Player.height * 0.70f));

//            int life = Main.rand.Next(38, 64);

//            system.Add(new AuraParticle
//            {
//                Kind = AuraParticleKind.Ember,
//                Position = position,
//                Velocity = new Vector2(Main.rand.NextFloat(-0.22f, 0.22f), Main.rand.NextFloat(-1.7f, -0.7f)),
//                Size = new Vector2(Main.rand.NextFloat(18f, 30f)),
//                Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
//                RotationSpeed = Main.rand.NextFloat(-0.03f, 0.03f),
//                Scale = 1f,
//                ScaleSpeed = 0.004f,
//                Drag = 0.985f,
//                Alpha = 0.68f * intensity,
//                Life = life,
//                MaxLife = life,
//                // 紫 → 腥红，贴合魔影套本体配色
//                Color = Color.Lerp(new Color(150, 70, 255), new Color(255, 72, 96), Main.rand.NextFloat()),
//            });
//        }

//        private void SpawnStreakTick(float intensity)
//        {
//            float speed = Player.velocity.Length();
//            if (speed < 3.2f)
//                return;

//            if (++streakTimer < 2)
//                return;

//            streakTimer = 0;

//            if (DemonshadeAuraSystem.Instance is not { } system)
//                return;

//            Vector2 direction = Player.velocity.SafeNormalize(Vector2.UnitX);

//            // 竖条贴图：把长轴转到运动方向上
//            float rotation = direction.ToRotation() - MathHelper.PiOver2;

//            int life = Main.rand.Next(16, 26);

//            system.Add(new AuraParticle
//            {
//                Kind = AuraParticleKind.Streak,
//                Position = Player.Center - direction * Main.rand.NextFloat(8f, 40f),
//                Velocity = -direction * Main.rand.NextFloat(0.2f, 1.1f),
//                Size = new Vector2(14f + speed * 1.1f, 48f + speed * 9f),
//                Rotation = rotation,
//                RotationSpeed = 0f,
//                Scale = 1f,
//                ScaleSpeed = 0.006f,
//                Drag = 0.94f,
//                Alpha = 0.50f * intensity,
//                Life = life,
//                MaxLife = life,
//                Color = new Color(196, 140, 255),
//            });
//        }

//        /// <summary>
//        /// 前景的火星：故意用 Dust 而不是自绘 —— Dust 在原版绘制顺序里位于玩家之后，
//        /// 这样"肩头的亮点"才会盖在护甲上，而且完全复用了工程里已有的 <see cref="SparkleDust"/>。
//        /// </summary>
//        private void SpawnGlintTick(DSVisualConfig cfg, float intensity)
//        {
//            if (cfg is null || !cfg.EnableSetVfx)
//                return;

//            if (++glintTimer < 14)
//                return;

//            glintTimer = 0;

//            Vector2 shoulder = Player.Center + new Vector2(-6f * Player.direction, -Player.height * 0.34f);

//            if (Main.rand.NextBool(2))
//            {
//                Dust sparkle = Dust.NewDustPerfect(
//                    shoulder + Main.rand.NextVector2Circular(10f, 6f),
//                    ModContent.DustType<SparkleDust>(),
//                    Main.rand.NextVector2Circular(0.4f, 0.4f),
//                    0,
//                    Color.Lerp(Color.MediumPurple, Color.Crimson, Main.rand.NextFloat()),
//                    Main.rand.NextFloat(0.9f, 1.5f) * (0.4f + 0.6f * intensity));
//                sparkle.noGravity = true;
//                sparkle.fadeIn = 0.4f;
//            }

//            if (Main.rand.NextBool(3))
//            {
//                Dust ember = Dust.NewDustPerfect(
//                    shoulder + Main.rand.NextVector2Circular(12f, 8f),
//                    DustID.Shadowflame,
//                    new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.9f, -0.3f)),
//                    120,
//                    default,
//                    Main.rand.NextFloat(0.8f, 1.2f) * (0.4f + 0.6f * intensity));
//                ember.noGravity = true;
//                ember.noLight = true;
//            }
//        }

//        public void SpawnShockRing(Vector2 center, float strength, float intensity)
//        {
//            if (Main.dedServ || intensity <= 0f)
//                return;

//            if (DemonshadeAuraSystem.Instance is not { } system)
//                return;

//            system.Add(new AuraParticle
//            {
//                Kind = AuraParticleKind.ShockRing,
//                Position = center,
//                Velocity = Vector2.Zero,
//                Size = new Vector2(150f),
//                Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
//                RotationSpeed = 0.012f,
//                Scale = 0.35f * strength,
//                ScaleSpeed = 0.075f * strength,
//                Drag = 1f,
//                Alpha = MathHelper.Clamp(0.85f * strength, 0f, 1f) * intensity,
//                Life = 34,
//                MaxLife = 34,
//                Color = new Color(236, 150, 255),
//            });
//        }

//        /// <summary>激怒时由 <see cref="DemonshadeEnrage"/> 调用。</summary>
//        public void NotifyEnrage()
//        {
//            DSVisualConfig cfg = DSVisualConfig.Instance;
//            float intensity = cfg is null ? 0f : MathHelper.Clamp(cfg.SetVfxIntensity / 100f, 0f, 1f);

//            // 总开关关掉时，连脉冲也不该留 —— 否则重开开关会冒出一个迟到的闪光
//            if (cfg is null || !cfg.EnableSetVfx || intensity <= 0f)
//                return;

//            SpawnShockRing(Player.Center, 1.4f, intensity);
//            veilPulse = 1f;
//        }
//    }
//}
