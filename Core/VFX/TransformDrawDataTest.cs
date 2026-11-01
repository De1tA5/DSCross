using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.UI;

namespace DemonShadeCrossMod.Core.VFX
{
    //测试TransformDrawData
    public class TransformDrawDataTest : ModPlayer
    {
        private double SnapshotIntervalSeconds = 1;
        private double nextSnapshotAt;

        //每位玩家自己的列表不要在 ModPlayer 字段初始化器中 new 一个可变列表，
        //因为具体玩家实例由模板克隆，容易不小心共享列表
        internal List<DrawData> Snapshot { get; private set; }
        internal int SnapshotVersion { get; private set; }

        public override void Initialize()
        {
            Snapshot = new List<DrawData>(64);
            SnapshotVersion = 0;
            nextSnapshotAt = 0;
        }

        //运行于全部PlayerDrawLayer后，便于整个玩家的绘制，通过直接修改PlayerDrawSet图层来实现
        //玩家单独部件则推荐使用PlayerDrawLayer
        public override void TransformDrawData(ref PlayerDrawSet drawInfo)
        {
            if (!DSClientConfig.Instance.DrawDataTest)
                return;

            Player player = drawInfo.drawPlayer;
            //同一玩家可能在一帧内被画多次：残影、头像、UI 预览等
            //这里只接收本地玩家在游戏中的普通身体绘制，避免快照被头像/残影覆盖
            //其他 Mod 若主动额外绘制 shadow=0 的同一玩家，仍可能进入本回调
            if (Main.dedServ || Main.gameMenu || player == null
                || !ReferenceEquals(player, Main.LocalPlayer)
                || drawInfo.headOnlyRender || drawInfo.shadow != 0f
                || drawInfo.hideEntirePlayer || !player.active || player.dead)
                return;

            List<DrawData> cache = drawInfo.DrawDataCache;
            if (cache == null)
                return;

            //Stopwatch 是单调时间，不随电脑时间校准跳动
            //“每 60 次 Draw”不等于一秒：帧率、跳帧和残影都会改变绘制次数
            //此处第一次立即采样，以后至少隔一秒，在下一个有效绘制回调中采样
            //游戏暂停但仍绘制时也会计时；最小化导致不绘制时，不会凭空生成新快照
            double now = Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
            if (SnapshotVersion != 0 && now < nextSnapshotAt)
                return;

            Snapshot ??= new List<DrawData>(64);
            Snapshot.Clear();
            Snapshot.AddRange(cache);
            //AddRange 复制的是 DrawData 结构体，因此帧矩形、位置、颜色等数值被冻结
            //texture 字段仍是原 Texture2D 引用，没有复制 GPU 像素
            //原版图集的动画由 sourceRect 控制，所以能看到采样时的动画帧；
            //若 texture 本身是每帧重绘的 RenderTarget，里面的像素仍可能变化

            SnapshotVersion++;
            nextSnapshotAt = now + SnapshotIntervalSeconds;
            //不在此处 Begin/Draw/End此时引擎还要提交玩家缓存，批次不属于本回调
        }
    }

    //UI绘制，懒得自己调用Main.spriteBatch
    public class DrawDataSnapshotTestSystem : ModSystem
    {
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!DSClientConfig.Instance.DrawDataTest)
                return;

            if (Main.dedServ)
                return;

            int index = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (index < 0)
                index = layers.Count;

            layers.Insert(index, new LegacyGameInterfaceLayer(
                "DemonShadeCrossMod: DrawData Snapshot Test",
                DrawSnapshot,
                //None 表示屏幕像素坐标，因此可直接使用 Main.MouseScreen
                //若改成 UI，布局与鼠标坐标也应按 UI 缩放转换
                InterfaceScaleType.None));
        }

        private static bool DrawSnapshot()
        {
            if (!DSClientConfig.Instance.DrawDataTest)
                return true;

            if (Main.gameMenu || Main.dedServ || !Main.LocalPlayer.active)
                return true;

            TransformDrawDataTest test = Main.LocalPlayer.GetModPlayer<TransformDrawDataTest>();
            List<DrawData> snapshot = test.Snapshot;

            if (test.SnapshotVersion == 0 || snapshot == null)
                return true;

            SpriteBatch batch = Main.spriteBatch;
            //GameInterfaceLayer 管理自己的批次，这里直接 Draw，不要再次 Begin/End
            //每次界面绘制都调用本方法，而不是仅在一秒到期时调用它
            //帧缓冲会重画；只 Draw 一帧不会把图片保留在以后所有帧中

            const float margin = 8f;
            const float header = 44f;
            int columns = Math.Min(Math.Max(snapshot.Count, 1), Math.Clamp(Main.screenWidth / 100, 1, 8));
            int rows = Math.Max(1, (snapshot.Count + columns - 1) / columns);
            //条目较多时整体缩小，尽量把全部命令都显示在屏幕里，而不是静默截断列表
            float cell = Math.Min(90f, Math.Min((Main.screenWidth - margin * 2) / columns,
                (Main.screenHeight - header - margin * 2) / rows));
            if (cell < 4f)
                return true; //极端小窗口/超多命令不适合此简易面板

            Vector2 size = new(columns * cell, rows * cell + header);
            Vector2 anchor = Main.MouseScreen + new Vector2(20f);
            anchor.X = MathHelper.Clamp(anchor.X, margin, Math.Max(margin, Main.screenWidth - size.X - margin));
            anchor.Y = MathHelper.Clamp(anchor.Y, margin, Math.Max(margin, Main.screenHeight - size.Y - margin));

            Fill(batch, anchor, size, new Color(12, 15, 24, 240));
            Terraria.Utils.DrawBorderString(batch,
                $"快照 {test.SnapshotVersion} / {snapshot.Count} 条 / 每秒采样",
                anchor + new Vector2(4, 2), Color.White, 1f);
            Terraria.Utils.DrawBorderString(batch, "原始裁片预览；编号是缓存索引；* 表示裁片超出纹理",
                anchor + new Vector2(4, 22), Color.LightGray, 1f);

            for (int i = 0; i < snapshot.Count; i++)
            {
                DrawData source = snapshot[i];
                Vector2 corner = anchor + new Vector2((i % columns) * cell, header + (i / columns) * cell);
                Fill(batch, corner + Vector2.One, new Vector2(cell - 2f),
                    i % 2 == 0 ? new Color(35, 39, 48) : new Color(49, 53, 63));

                Texture2D texture = source.texture;
                string caption = $"#{i}";
                if (texture != null && !texture.IsDisposed)
                {
                    Rectangle requested = source.sourceRect ?? texture.Bounds;
                    //一些 Shader 命令故意扩展 UV 到图集之外普通 UI 绘制不会执行那个
                    //Shader，因此这里仅展示与真实纹理相交的部分，并用 * 标出差异
                    Rectangle frame = Rectangle.Intersect(requested, texture.Bounds);
                    if (frame.Width > 0 && frame.Height > 0)
                    {
                        float available = cell * 0.75f;
                        float scale = Math.Min(available / frame.Width, available / frame.Height);
                        Vector2 center = corner + new Vector2(cell * 0.5f, cell * 0.43f);
                        batch.Draw(texture, center, frame, Color.White,
                            0f, new Vector2(frame.Width, frame.Height) * 0.5f,
                            scale, SpriteEffects.None, 0f);
                        caption += $" {requested.Width}x{requested.Height}";
                        if (frame != requested) caption += "*";
                    }
                    else caption += " 空裁片";
                }
                else caption += " 无纹理";

                //白色/无旋转/无染料：便于观察图片本身；不模拟原人物的最终外观
                //原命令的 color、rotation、origin、scale、effect、shader 等仍留在 Snapshot
                Terraria.Utils.DrawBorderString(batch, caption, corner + new Vector2(2f, cell * 0.78f),
                    Color.White, 1f);
            }
            return true;
        }

        private static void Fill(SpriteBatch batch, Vector2 position, Vector2 size, Color color)
        {
            batch.Draw(TextureAssets.MagicPixel.Value, position, null, color,
                0f, Vector2.Zero, size, SpriteEffects.None, 0f);
        }
    }
}
