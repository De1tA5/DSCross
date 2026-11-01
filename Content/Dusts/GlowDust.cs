using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Dusts
{
    public class GlowDust : ModDust
    {
        public static Asset<Texture2D> SolidCircle { get; private set; }
        public static Asset<Texture2D> BloomCircle { get; private set; }

        public override void Load()
        {
            //客户端加载
            if (!Main.dedServ)
            {
                SolidCircle = ModContent.Request<Texture2D>("CalamityMod/ExtraTextures/BasicCircle");
                BloomCircle = ModContent.Request<Texture2D>("DemonShadeCrossMod/Assets/BloomCircle");
            }
        }

        public override void OnSpawn(Dust dust)
        {
            dust.scale *= Main.rand.NextFloat(0.8f, 1f);
            dust.color = Color.AliceBlue;
        }

        public override bool Update(Dust dust)
        {

            dust.rotation += MathF.Sign(dust.velocity.X);
            dust.velocity *= 0.98f;

            //降落粒子不断缩小，上升粒子不断上升
            if (dust.noGravity)
                dust.scale += 0.02f;
            else
                dust.scale -= 0.01f;

            float light = MathHelper.Clamp(dust.scale * 0.8f, 0f, 1f);
            //粒子周围无光照时
            if (!dust.noLightEmittence)
                Lighting.AddLight(dust.position, dust.color.ToVector3() * light);

            return true;
        }

        public override bool PreDraw(Dust dust)
        {
            //光球
            //SpriteBatch spriteBatch = Main.spriteBatch;
            //Texture2D value = BloomCircle.Value;
            //Vector2 position = dust.position - Main.screenPosition;
            //Color color = dust.color;
            //color.A = 0;
            //spriteBatch.Draw(value, position, null, color * Utils.GetLerpValue(255f, 0f, dust.alpha), dust.rotation, BloomCircle.Size() * 0.5f, dust.scale * 0.1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(
                BloomCircle.Value, //绘制光团
                dust.position - Main.screenPosition, //屏幕位置
                null,
                dust.color with { A = 0 } * Utils.GetLerpValue(255, 0, dust.alpha), //透明度调整，由不透明转为透明
                dust.rotation,
                BloomCircle.Size() * 0.5f, //原贴图尺寸大小，200*200 -> 100*100
                dust.scale * 0.1f, //实际大小
                SpriteEffects.None,
                0);

            if (dust.alpha < 1)
            {
                //不透明时的绘制
                //SpriteBatch spriteBatch2 = Main.spriteBatch;
                //Texture2D value2 = BloomCircle.Value;
                //Vector2 position2 = dust.position - Main.screenPosition;
                //color = dust.color;
                //color.A = 0;
                //spriteBatch2.Draw(value2, position2, null, color * 0.85f * Utils.GetLerpValue(255f, 0f, dust.alpha), dust.rotation, BloomCircle.Size() * 0.5f, dust.scale * 0.04f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(
                    BloomCircle.Value,
                    dust.position - Main.screenPosition,
                    null,
                    dust.color with { A = 0 } * 0.85f * Utils.GetLerpValue(255, 0, dust.alpha),
                    dust.rotation,
                    BloomCircle.Size() * 0.5f,
                    dust.scale * 0.04f,
                    SpriteEffects.None,
                    0);
            }

            if (!dust.noLight)
            {
                //自带光照时绘制
                //SpriteBatch spriteBatch3 = Main.spriteBatch;
                //Texture2D value3 = SolidCircle.Value;
                //Vector2 position3 = dust.position - Main.screenPosition;
                //color = Color.Lerp(dust.color, Color.White, 0.3f);
                //color.A = 0;
                //spriteBatch3.Draw(value3, position3, null, color * Utils.GetLerpValue(255f, 0f, dust.alpha), dust.rotation, SolidCircle.Size() * 0.5f, dust.scale * 0.075f, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(
                    SolidCircle.Value,
                    dust.position - Main.screenPosition,
                    null,
                    Color.Lerp(dust.color, Color.White, 0.3f) with { A = 0 } * Utils.GetLerpValue(255, 0, dust.alpha),
                    dust.rotation,
                    SolidCircle.Size() * 0.5f,
                    dust.scale * 0.075f,
                    SpriteEffects.None,
                    0);
            }

            return false;
            //false 跳过自动绘制，需手动spriteBatch.Draw进行绘制
        }
    }
}
