using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Dusts
{
    public class SparkleDust : ModDust
    {
        public static Asset<Texture2D> Sparkle { get; private set; }
        public override void Load()
        {
            //客户端加载
            if (!Main.dedServ)
            {
                Sparkle = ModContent.Request<Texture2D>("DemonShadeCrossMod/Assets/Sparkle");
            }
        }

        public override void OnSpawn(Dust dust)
        {
            dust.scale *= Main.rand.NextFloat(0.8f, 1f);
            dust.color = Color.AliceBlue;
            dust.velocity = Vector2.Zero;
        }

        public override bool Update(Dust dust)
        {
            dust.velocity = Vector2.Zero;
            //if (dust.noGravity)
            //    dust.scale += 0.02f;
            //else
            //    dust.scale -= 0.01f;

            return true;
        }

        public override bool PreDraw(Dust dust)
        {
            Main.spriteBatch.Draw(
                Sparkle.Value, //绘制闪光
                dust.position - Main.screenPosition, //屏幕位置,即绘制在对应dust的位置
                null,//贴图截取的范围，用null来绘制整个贴图
                dust.color with { A = 0 } * Utils.GetLerpValue(255, 0, dust.alpha), //透明度调整，由不透明转为透明
                dust.rotation,
                Sparkle.Size() * 0.5f, //贴图的原点，一般为中心点，参与旋转
                dust.scale * 0.4f, //实际大小
                SpriteEffects.None,
                0);

            return false;
        }
    }
}
