using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Dusts
{
    //26.10.23 卸载资源以及使用贴图
    public class GlowFlare : ModDust
    {
        private static Asset<Texture2D> GlowFlareTexture { get; set; }
        public override string Texture => "DemonShadeCrossMod/Assets/Empty";
        public override void Load()
        {
            //客户端加载
            if (!Main.dedServ)
            {
                GlowFlareTexture = ModContent.Request<Texture2D>("DemonShadeCrossMod/Assets/GlowFlare");
            }
        }

        public override void Unload()
        {
            GlowFlareTexture = null;
        }

        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            //充当初始化记录
            //dust.noLightEmittence = false;      
            dust.frame = new(0, 0, 64, 64); //使用贴图为64*64大小
            dust.shader = new(ModContent.Request<Effect>("DemonShadeCrossMod/Assets/Effects/GlowFlare"), "GlowFlare");
            dust.fadeIn = 1f; //渐消时长
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color * dust.fadeIn;
        }

        public override bool Update(Dust dust)
        {
            //以下为自动绘制时补偿需要
            //if (!dust.noLightEmittence)
            //{
            //    //基准位置，这里不调整贴图，调增代码位置
            //    dust.position -= Vector2.One * 32f * dust.scale;
            //    dust.noLightEmittence = true;
            //}
            //自动绘制时补偿，32由于贴图为64，故为其一半
            //Vector2 drawPosition = dust.position + Vector2.One.RotatedBy(dust.rotation) * 32f * dust.scale;

            if (dust.noGravity)
            {
                dust.scale *= 1f - (0.04f + 0.04f * dust.fadeIn);
            }
            else
            {
                dust.scale *= 0.96f;
            }

            //自动绘制时补偿缩放和旋转导致的位置变化
            //Vector2 rotatedOffset = dust.position + Vector2.One.RotatedBy(dust.rotation + 0.06f) * 32f * dust.scale;
            //dust.rotation += 0.06f;
            //dust.position += drawPosition - rotatedOffset;

            //每帧旋转
            dust.rotation += 0.06f;
            dust.position += dust.velocity;
            dust.velocity *= 0.94f;

            //允许粒子发光，就按粒子颜色和当前大小向环境添加光照
            //if (!dust.noLight)
            //{
            //    Lighting.AddLight(drawPosition, (float)(int)dust.color.R * dust.scale * 0.005f, (float)(int)dust.color.G * dust.scale * 0.005f, (float)(int)dust.color.B * dust.scale * 0.005f);
            //}

            if (!dust.noLight)
            {
                Lighting.AddLight(dust.position, (float)(int)dust.color.R * dust.scale * 0.005f, (float)(int)dust.color.G * dust.scale * 0.005f, (float)(int)dust.color.B * dust.scale * 0.005f);
            }
            if (dust.scale < 0.05f)
            {
                dust.active = false;
            }

            dust.shader.UseColor(dust.color * dust.fadeIn);
            dust.shader.UseOpacity(0.4f);
            dust.shader.UseSaturation(2.5f);
            dust.shader.UseTargetPosition(new Vector2(1f, 1f));

            return false;
        }

        //手动绘制
        public override bool PreDraw(Dust dust)
        {
            //调整非标准粒子格式的贴图，解决绘制原点和粒子实际位置不齐问题
            Main.spriteBatch.Draw(
                texture: GlowFlareTexture.Value,
                position: dust.position - Main.screenPosition, //屏幕绘制位置
                sourceRectangle: dust.frame,
                color: dust.color * dust.fadeIn,
                rotation: dust.rotation,
                origin: GlowFlareTexture.Size() * 0.5f, // origin 是当前绘制帧内部的局部坐标，对于 64×64 的帧，中心就是 (32, 32)。
                scale: dust.scale,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );

            return false;
        }
    }
}
