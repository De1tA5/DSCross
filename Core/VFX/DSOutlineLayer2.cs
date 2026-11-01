//using CalamityMod;
//using DemonShadeCrossMod.Core.Utils;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using ReLogic.Content;
//using Terraria;
//using Terraria.DataStructures;
//using Terraria.ModLoader;
//using System;

//namespace DemonShadeCrossMod.Core.VFX
//{
//    /// <summary>
//    /// 魔影套盔甲轮廓光 —— 只画盔甲边缘发光，不碰玩家本体。
//    /// 用 DemonshadeArmorOutline.fx 做 3×3 邻域膨胀，只在外围画紫色辉光。
//    /// </summary>
//    public class DSOutlineLayer : PlayerDrawLayer
//    {
//        private static Asset<Effect> outlineEffect;
//        private static Asset<Texture2D> noise;
//        private static bool errorLogged;

//        public override void Load()
//        {
//            if (!Main.dedServ)
//            {
//                outlineEffect = ModContent.Request<Effect>("DemonShadeCrossMod/Assets/Effects/DemonshadeArmorOutline");
//                noise = ModContent.Request<Texture2D>("DemonShadeCrossMod/Assets/NoiseMap");

//            }
//        }

//        public override void Unload()
//        {
//            outlineEffect = null;
//            noise = null;
//        }

//        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
//        {
//            return true;   // 手动控制，在 Draw 里判套装
//        }

//        public override Position GetDefaultPosition()
//        {
//            return new AfterParent(PlayerDrawLayers.ArmOverItem);   // 画在盔甲之后
//        }

//        protected override void Draw(ref PlayerDrawSet drawInfo)
//        {
//            Player player = drawInfo.drawPlayer;

//            //本地玩家生效
//            if (player.whoAmI != Main.myPlayer)
//                return;

//            if (!player.Calamity().demonshadeSet)
//                return;

//            //隐身 / 死亡 / 挂载状态不画
//            if (player.invis || player.dead || player.mount.Active)
//                return;

//            try
//            {
//                DrawOutline(ref drawInfo);
//            }
//            catch (Exception e)
//            {
//                if (!errorLogged)
//                {
//                    errorLogged = true;
//                    Mod.Logger.Warn("[魔影轮廓光] 绘制失败，本次跳过", e);
//                }
//            }
//        }

//        private void DrawOutline(ref PlayerDrawSet drawInfo)
//        {
//            if (outlineEffect?.Value is null)
//                return;

//            Effect effect = outlineEffect.Value;
//            GraphicsDevice gd = Main.graphics.GraphicsDevice;
//            SpriteBatch sb = Main.spriteBatch;


//            //噪音图
//            gd.Textures[1] = noise?.Value;
//            gd.SamplerStates[1] = SamplerState.LinearWrap;
//            //轮廓宽度（像素）、强度、时间、颜色
//            effect.Parameters["uOutlineWidth"]?.SetValue(1.6f);
//            effect.Parameters["uIntensity"]?.SetValue(0.7f);
//            effect.Parameters["uTime"]?.SetValue(Main.GameUpdateCount * 60f);
//            effect.Parameters["uOutlineColor"]?.SetValue(new Vector3(0.77f, 0.55f, 1f));   // 魔影紫


//            // 遍历 DrawDataCache，找到盔甲条目并画轮廓
//            for (int i = 0; i < drawInfo.DrawDataCache.Count; i++)
//            {
//                DrawData data = drawInfo.DrawDataCache[i];

//                // 只画盔甲（body/legs/head），不画玩家本体（skin）
//                // 盔甲贴图的特征：sourceRect 有值（帧图集），且 texture 不是 Players[skinVar]
//                if (data.sourceRect == null)
//                    continue;

//                // 跳过玩家本体皮肤（TextureAssets.Players）
//                if (IsPlayerSkin(data.texture))
//                    continue;

//                // 画轮廓：用 effect 包裹一次 Draw
//                sb.End();
//                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp,
//                                  DepthStencilState.None, RasterizerState.CullNone, effect, Main.GameViewMatrix.TransformationMatrix);

//                //套装贴图有
//                sb.Draw(data.texture, data.position, data.sourceRect, Color.White,
//                                 data.rotation, data.origin, data.scale, data.effect, 0f);

//                sb.End();
//                gd.Textures[1] = null;
//                sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
//                                  DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
//            }
//        }

//        /// <summary>
//        /// 判断这张贴图是不是玩家本体皮肤（Players[skinVar, ...]）。
//        /// 盔甲贴图在 TextureAssets.ArmorBodyComposite / ArmorArm / ArmorHead 里。
//        /// </summary>
//        private static bool IsPlayerSkin(Texture2D texture)
//        {
//            if (texture == null)
//                return false;

//            // 玩家皮肤贴图在 TextureAssets.Players[skinVar, part]
//            // 盔甲在 TextureAssets.ArmorBodyComposite[body] / ArmorArm[body] / ArmorHead[head]
//            // 简单判定：如果 texture 的 Name 包含 "Player" 或 "Skin"，跳过
//            return texture.Name?.Contains("Player") == true || texture.Name?.Contains("Skin") == true;
//        }
//    }
//}

