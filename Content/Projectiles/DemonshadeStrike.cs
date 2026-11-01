//using CalamityMod.Dusts;
//using ClickerClass;
//using DemonShadeCrossMod.Content.Buffs;
//using DemonShadeCrossMod.Core.Utils;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using Terraria;
//using Terraria.Audio;
//using Terraria.DataStructures;
//using Terraria.ID;
//using Terraria.ModLoader;

//namespace DemonShadeCrossMod.Content.Projectiles
//{
//    [JITWhenModsEnabled("ClickerClass")]
//    [ExtendsFromMod("ClickerClass")]
//    //TODO - shader,弹幕结束时冲击波和glowmask

//    public class DemonshadeStrike : ModProjectile
//    {
//        public int maxTime => 100; //靠近所需时间
//        public int maxRadius => 370; //最大范围
//        public int rotationSpeed => 9;//角速度

//        public Vector2 originPosition = Vector2.Zero;
//        public float Timer
//        {
//            get => Projectile.ai[0];
//            set => Projectile.ai[0] = value;
//        }

//        public float randomPhase
//        {
//            get => Projectile.ai[1];
//            set => Projectile.ai[1] = value;
//        }

//        public float constantPhase
//        {
//            get => Projectile.ai[2];
//            set => Projectile.ai[2] = value;
//        }

//        //public NPC Target 
//        //{
//        //    get => Projectile.ai[1] == 0 ? null : Main.npc[(int)Projectile.ai[1] - 1];
//        //    set => Projectile.ai[1] = value == null ? 0 : value.whoAmI + 1;
//        //}

//        public override bool IsLoadingEnabled(Mod mod)
//        {
//            return Clicker.Load;
//        }

//        public override void SetStaticDefaults()
//        {
//            Main.projFrames[Projectile.type] = 6;//spirit图6帧动画
//            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;//记录弹幕拖尾数量
//            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;//模式0仅记录弹幕位置
//        }

//        public override void SetDefaults()
//        {
//            Projectile.width = 30;
//            Projectile.height = 50;
//            Projectile.friendly = true;
//            Projectile.DamageType = ModContent.GetInstance<ClickerDamage>();//对应伤害类型
//            Projectile.penetrate = -1; //无限穿透
//            Projectile.maxPenetrate = -1; 
//            Projectile.tileCollide = false; //穿墙
//            Projectile.ignoreWater = true; 


//        }

//        public override void OnSpawn(IEntitySource source)
//        {
//            SoundEngine.PlaySound(
//                SoundID.Item4 with { Volume = 0.5f },
//                Projectile.position);
//            originPosition = Main.MouseWorld;
//            Timer = maxTime;
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            Texture2D texture = Mod.Assets.Request<Texture2D>("Content/Projectiles/DemonshadeStrike").Value;
//            Rectangle frame = texture.Frame(1, Main.projFrames[Projectile.type], 0, Projectile.frame);//前2参数为spirt图有几列几行，后2参数为取第几列几行的帧图，默认从0开始
//            Vector2 drawOrigin = texture.Size() / 2;//图片中心
//            for (int k = 0; k < Projectile.oldPos.Length; k++)
//            {
//                Vector2 drawPos = (Projectile.oldPos[k] - Main.screenPosition) + drawOrigin + new Vector2(2f * k, 2f * k);//最后参数修改每个拖影绘制的距离
//                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);//插值计算透明度
//                if (!Projectile.oldPos[k].Equals(Projectile.position))
//                {
//                    Main.spriteBatch.Draw(
//                        texture,
//                        drawPos,
//                        frame,
//                        color,
//                        Projectile.rotation,
//                        drawOrigin,
//                        Projectile.scale, //缩放大小插值
//                        SpriteEffects.None,
//                        0f);
//                }
//            }

//            return true;
//        }

//        public override void AI()
//        {
//            Projectile.frameCounter++;//动画计数器，游戏内几帧后切换新动画
//            if (Projectile.frameCounter > 3)
//            {
//                Projectile.frame++;//当前动画帧
//                Projectile.frameCounter = 0;
//            }
//            if (Projectile.frame >= Main.projFrames[Type])
//            {
//                Projectile.frame = 0;
//            }

//            //结算弹幕死亡
//            if (Timer > 0)
//            {
//                Timer--;
//            }
//            else
//            {
//                Projectile.Kill();
//            }

//            float timeFactor = (maxTime - Timer) / maxTime;//0 -> 1 线性插值
//            int radius = (int)(maxRadius * (1 - timeFactor) / 1);


//            Projectile.Center = originPosition + new Vector2(
//                (float)Math.Cos(rotationSpeed * timeFactor + randomPhase + constantPhase) * radius,
//                (float)Math.Sin(rotationSpeed * timeFactor + randomPhase + constantPhase) * radius
//            );


//            Lighting.AddLight(Projectile.Center, 0.2f, 0.2f, 1f);
//            if (Main.rand.NextBool(4))
//                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<ShadowspecBarDust>());
//        }

//        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            target.AddBuff(ModContent.BuffType<DemonFlames>(), 120);
//        }

//        public override void OnKill(int timeLeft)
//        {
//            Player player = Main.player[Projectile.owner];
//            if (player.ownedProjectileCounts[ModContent.ProjectileType<DemonshadeExplosin>()] < 1)
//            {
//                Projectile.NewProjectile(
//                    Projectile.GetSource_FromThis(),
//                    originPosition,
//                    Vector2.Zero,
//                    ModContent.ProjectileType<DemonshadeExplosin>(),
//                    20000,
//                    0);
//            }
//        }

//        //private NPC FindClosestNPC(float maxDetectDistance) 
//        //{
//        //    NPC closestNPC = null;
//        //    float distacne = maxDetectDistance;//最初检测范围
//        //    Vector2 mouse = Main.MouseWorld;
//        //    for (int index = 0; index < Main.npc.Length; index++)
//        //    {
//        //        if (Main.npc[index].CanBeChasedBy(null, false))//集合大部分属性如active，!friendly
//        //        {
//        //            float extraDistance = (Main.npc[index].width / 2) + (Main.npc[index].height / 2);

//        //            bool canHit = true;
//        //            if (extraDistance < distacne)
//        //                canHit = Collision.CanHit(mouse, 1, 1, Main.npc[index].Center, 1, 1);

//        //            if (Vector2.Distance(mouse, Main.npc[index].Center) < distacne && canHit)
//        //            {
//        //                distacne = Vector2.Distance(mouse, Main.npc[index].Center);
//        //                Target = Main.npc[index];
//        //            }
//        //        }
//        //    }

//        //    return closestNPC;
//        //}
//    }
//}
