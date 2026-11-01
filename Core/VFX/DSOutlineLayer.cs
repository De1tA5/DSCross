using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

//26.10.24 正式启用轮廓描边，同时添加配置文件利于修改
namespace DemonShadeCrossMod.Core.VFX
{
    //绘制装备轮廓发光，基本原理如下
    //通过复制一遍原装备贴图，使用着色器进行轮廓处理后
    //移至正常装备贴图前，使后者绘制时覆盖前者，内部轮廓框线会被覆盖，最后只留下最外层轮廓光
    //仅独立轮廓光，游戏内光照并未增加
    public class DSOutlineLayer : ModPlayer
    {
        //Padding为采样留白
        //Shader 采样半径变大却不扩四边形，会把光裁在矩形边缘
        //保证 HaloWidth + 1 <= Padding
        internal const int Padding = 7;

        //static 保存一次加载的共享注册/资源，实例字段保存每位玩家的临时列表
        //packedShader 不等于物品 ID，也不等于注册字典 key：它编码了 shader 类别与索引
        //0 在本例表示尚未就绪；正式描边条目被设置为非零的专用 ArmorShader 编码
        private static int packedShader;
        private static bool errorLogged;
        //灾厄有额外的头部和手臂移动帧图
        private static Asset<Texture2D> demonshadeHeadExtension;
        private static Asset<Texture2D> demonshadeFrontArm;
        //临时描边列表不设 static，避免多人绘制时互相覆盖；列表容量可以跨帧复用
        private List<DrawData> outlines;

        public override void Load()
        {
            //非服务器加载贴图和着色器
            if (Main.dedServ)
                return;

            errorLogged = false;
            packedShader = 0;

            //这里使用AssetRequestMode.ImmediateLoad在于，下方ArmorShader也要注册，存在加载未完成时的空引用风险
            //同时被注册确保不为空使用AssetRequestMode.ImmediateLoad
            Asset<Effect> effect = ModContent.Request<Effect>("DemonShadeCrossMod/Assets/Effects/DemonshadeArmorOutline", AssetRequestMode.ImmediateLoad);

            //分别为玩家移动时 头部 和 手臂 的帧图
            //这里仅预加载并取得引用，后面只有缓存中确实存在它们，才会为它们描边
            demonshadeHeadExtension = ModContent.Request<Texture2D>("CalamityMod/Items/Armor/Demonshade/DemonshadeHelm_Extension");
            demonshadeFrontArm = ModContent.Request<Texture2D>("CalamityMod/Items/Armor/Demonshade/DemonshadeBreastplate_Arms");


            //神奇写法之使用负id绑定ArmorShader，既使用原版着色器，也不占用额外染料
            int key = -1;
            //查找未绑定其他shader的id，负物品id
            while (GameShaders.Armor.GetShaderIdFromItemId(key) != 0)
                //防边界溢出
                key = checked(key - 1);

            //注册的是我们自定义的 ArmorShaderData 子类；真正逐部件上传参数由它的 Apply 负责
            GameShaders.Armor.BindShader(key, new OutlineShaderData(effect));

            //PackShader 让渲染器能区分 ArmorShader/HairShader 等类型，不要把字典 key 直接写进 DrawData.shader
            //PackShader 与 UnpackShader 一起用，返回的数值通过运算 ，返回shader类和shader的id
            packedShader = PlayerDrawHelper.PackShader(GameShaders.Armor.GetShaderIdFromItemId(key), PlayerDrawHelper.ShaderConfiguration.ArmorShader);
        }

        public override void Unload()
        {
            packedShader = 0;
            errorLogged = false;
            demonshadeHeadExtension = null;
            demonshadeFrontArm = null;
        }

        //[5] 初始化具体玩家实例的临时数据ModPlayer 的实例创建/克隆不能简单等同于 new 此类
        //在 Initialize 给每个实例新列表，避免从模板复制同一个可变 List 引用
        //这是当前实现；上一轮优化示例改成按需分配，此副本没有替你改动
        public override void Initialize()
        {
            //ModPlayer 模板会克隆；在 Initialize 为每个玩家分配独立的可复用列表
            outlines = new List<DrawData>(8);
        }

        //本机实际顺序还包括：原版整体旋转 -> 整体缩放 -> ModPlayer.TransformDrawData
        //因此 source.position/rotation/scale 已是当前绘制实例的结果，再减一次屏幕坐标会错位
        //ref 允许同步修改 drawInfo 的字段；后面手持弹幕的插入下标确实需要被改回去
        //其他模组的 TransformDrawData 仍可能在本回调之后运行，不能宣称自己是全局最后一步
        public override void TransformDrawData(ref PlayerDrawSet drawInfo)
        {
            if (!DSClientConfig.Instance.DemonshadeOutline)
                return;

            //取本次被画的玩家；只用 Main.LocalPlayer 会让远端玩家、角色预览或克隆引用错对象
            Player player = drawInfo.drawPlayer;

            //短路条件从左到右检查注册没就绪、已有错误或没有玩家时，直接退出
            //headOnlyRender 是 UI/地图头像专用绘制，不是世界中的头部
            //hideEntirePlayer 表示这一绘制实例应隐藏，不能靠补画轮廓把隐藏角色重新显示出来
            //不检查 mount.Active 或 wingTime，因此骑乘/飞行主角色仍可生效
            if (Main.dedServ || packedShader == 0 || errorLogged || player == null
                || player.whoAmI != Main.myPlayer
                || player.dead || player.invis
                || drawInfo.headOnlyRender || drawInfo.hideEntirePlayer)
                return;

            //实验性：全部盔甲绘制轮廓
            if (!player.Calamity().demonshadeSet && !DSClientConfig.Instance.AllArmorOutline)
                return;

            //残影绘制
            if (!DSClientConfig.Instance.ShadowOutline && drawInfo.shadow != 0)
                return;

            //渲染器会复用/清空该列表，不要把它当上一帧稳定的数据长期保存
            //drawInfo.DrawDataCache会被每帧清空，这里单独存储
            List<DrawData> cache = drawInfo.DrawDataCache;
            if (cache == null || cache.Count == 0)
                return;

            //??= 只在列表为 null 时分配；8 是初始容量，超过后 List 会扩容，不是最多只能描 8 个部件
            outlines ??= new List<DrawData>(8);
            //Clear 复用已有容量，并删除上一绘制实例的姿态数据；不会重新申请一张纹理或清空 GPU 画面
            outlines.Clear();
            try
            {
                //以下TextureAssets存储游戏中所有已加载或可按需加载的身体装备贴图资源
                //获取当前显示的身体装备的贴图ID：player.body（为显示贴图，故即使装备在时装栏显示的也是该id）
                //获取对应身体装备贴图：TextureAssets.ArmorBody[player.body]

                if (Main.GameUpdateCount % 60 == 0 && DSClientConfig.Instance.DrawDataTest)
                    Main.NewText($"{player.body}");

                //时装覆盖时跟随当前可见盔甲，所以穿魔影套并覆盖时装时，本例会描当前可见的盔甲
                //检测是否全部加载

                Texture2D head = LoadedTexture(TextureAssets.ArmorHead, player.head);
                //某些头盔背面在另一个 ArmorHead 槽位，不能只检查前侧的 player.head
                Texture2D headBack = LoadedTexture(TextureAssets.ArmorHead, BackHeadSlot(player.head));
                Texture2D headExtension = LoadedTexture(demonshadeHeadExtension);
                Texture2D frontArm = LoadedTexture(demonshadeFrontArm);

                //传统身体/男女身体/手臂和复合身体采用不同贴图；兼容这些来源才能覆盖不同盔甲绘制方式
                //复合手臂的多个帧、肩膀和躯干可能共用一张图集
                Texture2D composite = LoadedTexture(TextureAssets.ArmorBodyComposite, player.body);
                Texture2D body = LoadedTexture(TextureAssets.ArmorBody, player.body);
                Texture2D female = LoadedTexture(TextureAssets.FemaleBody, player.body);
                Texture2D arm = LoadedTexture(TextureAssets.ArmorArm, player.body);
                Texture2D legs = LoadedTexture(TextureAssets.ArmorLeg, player.legs);

                //记录第一条真正有效的盔甲命令位置，-1是尚未找到
                //用于后续插入
                int insertIndex = -1;

                //先遍历原缓存，把结果放到另一个列表，遍历期间不向 cache 插入，避免下标错乱或重复遍历自己
                for (int i = 0; i < cache.Count; i++)
                {
                    //DrawData是struct为值类型：这里取得副本仅改副本而不赋回 cache，不会改变原条目
                    DrawData source = cache[i];
                    Texture2D texture = source.texture;

                    //跳过空纹理、失效纹理、alpha 为零、本例已生成的数据，以及目标矩形绘制路径
                    //useDestinationRectangle 的几何由 destinationRectangle 决定，不能套用下面的 sourceRect/origin 扩边公式
                    //注意 A=0 在加色惯例里也可能有非零 RGB；本例选择不描这类条目，不代表它们一定不可见
                    //source.shader == packedShader 只跳过旧描边本身；当前版本没有保证整份缓存重复处理时幂等
                    if (texture == null || texture.IsDisposed || source.color.A == 0 || source.useDestinationRectangle || source.shader == packedShader)
                        continue;

                    if (texture != head && texture != headBack && texture != headExtension
                        && texture != frontArm && texture != composite && texture != body
                        && texture != female && texture != arm && texture != legs)
                        continue;

                    //没有显式裁切时按整张纹理；否则保留该部件真实裁切，不能写死头/腿/手臂都是 40x56
                    //texture.Bounds = new Rectangle(0, 0, texture.Width, texture.Height)
                    //source.sourceRect 为当前精灵图框选绘制帧，texture.Bounds则为整个精灵图
                    Rectangle frame = source.sourceRect ?? texture.Bounds;

                    //原帧必须有面积且位于图集内；这是扩边前的检查，扩边后允许 UV 跨出图集，由 Shader 帧保护处理
                    if (frame.Width <= 0 || frame.Height <= 0 || !texture.Bounds.Contains(frame))
                        continue;

                    //扩的是 sourceRect/几何覆盖范围，不是拉伸原盔甲
                    //UV 可以超出图集，shader 会将当前帧外的 alpha 强制归零
                    //对称扩边后 origin 同量增加，原像素位置保持不变，翻转也适用

                    //复用 position/rotation/scale/effect 保持坐姿、飞行、倒重力对齐
                    DrawData outline = source;


                    //帧图上下左右全部增加Padding距离，用于描边
                    //为框选大小变大，框选点位置也会修正
                    frame.Inflate(Padding, Padding);
                    outline.sourceRect = frame;

                    //旧像素相对新矩形左上角向右下移动了 Padding，因此原点也同量增加，保持屏幕位置不变
                    outline.origin += new Vector2(Padding);

                    //保留原可见度，RGB 交给 Shader 输出紫色本例的 Shader 只读顶点颜色 alpha，不随环境光 RGB 变暗
                    //纯白外框仅透明度由原图控制，便于shader涂色
                    outline.color = new Color(255, 255, 255, (int)source.color.A);

                    //指定逐条绘制时使用哪个 Shader；这里只排命令，并没有调用 GPU，也没有立即绘制一次贴图
                    outline.shader = packedShader;

                    //SpriteBatch sb = Main.spriteBatch;
                    //sb.Draw(outline.texture, Main.MouseScreen, Color.White);

                    //直接复用当前动作的 position/rotation/scale/effect/sourceRect
                    //骑乘坐姿、飞行帧、倒重力和头部扩展的 -4 像素偏移都已在原数据中
                    //不用 projectile/player.Center 重算，避免不同姿态下描边和本体错位
                    if (insertIndex < 0)
                        insertIndex = i;

                    //需要描边的贴图
                    outlines.Add(outline);

                }

                if (outlines.Count == 0)
                    return;

                //插在第一条有效盔甲数据前
                //优先插入所有描边贴图，再插入正常贴图覆盖，实现只有最外轮廓发光描边
                //坐骑后层/翅膀等原本在盔甲后面的图像先画，再画盔甲描边和盔甲本体

                //避免旧版把描边压到坐骑后层下方坐骑前层仍按原层级遮挡玩家

                //原盔甲随后遮住内部接缝；本体颜色、染料、相对顺序均保留

                //一次插入，后面的原本体再画，就能遮住落在其他不透明盔甲内部的局部描边
                //插入点保留了此前的后层顺序，但不能保证任意模组的所有坐骑/饰品遮挡都正确
                cache.InsertRange(insertIndex, outlines);

                //原版用列表下标决定手持弹幕的插入点，前插数据后必须同步偏移
                //手持弹幕不是简单的一张 HeldItem 图，它可能由渲染器在特定缓存下标单独画出
                //插入命令改变了后方下标，所以等于/晚于插入点的标记必须加上新增数量
                //-1 代表没有该插入点；由于 insertIndex>=0，它会自然保持 -1，不会被错误移动
                if (drawInfo.projectileDrawPosition >= insertIndex)
                    drawInfo.projectileDrawPosition += outlines.Count;
            }
            catch (Exception e)
            {
                //这里只捕获准备 DrawData 的异常；真正 GPU 渲染和下方 Shader.Apply 发生得更晚，通常不在此 try 内
                //不修改全局绘制状态；失败后停用该示例，避免每帧重复异常
                errorLogged = true;
                Mod.Logger.Warn("[魔影轮廓光·优化示例] 收集描边失败，已停用至下次重载", e);
            }
            finally
            {
                //return 或异常也会执行清空；避免本次留下的纹理引用和姿态影响下一次绘制
                outlines.Clear();
            }
        }

        //检查空数组、无装备槽和越界，再拿到已加载纹理
        private static Texture2D LoadedTexture(Asset<Texture2D>[] assets, int slot)
        {
            //slot 0 代表没有该部位盔甲；只检查已加载资产，不在绘制中触发加载
            if (assets == null || slot <= 0 || slot >= assets.Length)
                return null;

            return LoadedTexture(assets[slot]);
        }

        //检查 IsLoaded 后再读 Value；Asset 存在不代表它已经完成异步加载
        private static Texture2D LoadedTexture(Asset<Texture2D> asset) => asset != null && asset.IsLoaded ? asset.Value : null;

        private static int BackHeadSlot(int frontSlot)
        {
            //某些头盔有单独的后脑贴图，这里还需额外处理，比如说原版的狗耳朵
            int[] mapping = ArmorIDs.Head.Sets.FrontToBackID;
            return frontSlot > 0 && frontSlot < mapping.Length ? mapping[frontSlot] : -1;
        }

        //继承原版ArmorShaderData
        //sealed不允许再继承
        private sealed class OutlineShaderData : ArmorShaderData
        {
            private static DSClientConfig ClientCfg => DSClientConfig.Instance;
            private readonly EffectPass pass;
            private readonly EffectParameter texel;
            private readonly EffectParameter frameBounds;
            private readonly EffectParameter framePixels;
            private readonly EffectParameter time;
            //可配置参数
            private readonly EffectParameter coreWidth;
            private readonly EffectParameter haloWidth;
            private readonly EffectParameter haloStrength;
            private readonly EffectParameter intensity;
            private readonly EffectParameter outlineColor;

            //base告诉ArmorShaderData所属资产和Pass名称
            //构造发生在 Load 注册时；asset 使用 ImmediateLoad，因此此时可以读取参数/通道
            public OutlineShaderData(Asset<Effect> asset) : base(asset, "ArmorOutlinePass")
            {
                Effect effect = asset.Value;
                pass = effect.CurrentTechnique.Passes["ArmorOutlinePass"];
                //不可调参数
                texel = Required(effect, "uTexel");
                frameBounds = Required(effect, "uFrameBounds");
                framePixels = Required(effect, "uFramePixels");
                time = Required(effect, "uFlowTime");
                //配置文件调整参数，具体值在Apply设置
                coreWidth = Required(effect, "uCoreWidth");
                haloWidth = Required(effect, "uHaloWidth");
                haloStrength = Required(effect, "uHaloStrength");
                intensity = Required(effect, "uIntensity");
                outlineColor = Required(effect, "uOutlineColor");
            }

            //这边重写ArmorShaderData的Apply，设置并使用自己的参数，最后Apply自己的Pass
            public override void Apply(Entity entity, DrawData? drawData = null)
            {

                //遇到不完整输入时恢复默认像素Pass，避免上一条轮廓的GPU状态残留
                //未启用轮廓描光也直接退出，使用原版像素着色器
                if (!drawData.HasValue || drawData.Value.texture == null
                    || drawData.Value.texture.IsDisposed || !drawData.Value.sourceRect.HasValue
                    || !ClientCfg.DemonshadeOutline)
                {
                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                    return;
                }

                //shader设置参数，在正式Apply前，数据会全部缓存

                //并非完全单格像素描光，过大会导致膨胀
                coreWidth.SetValue(ClientCfg.OutlineCoreWidth);
                //淡外晕半径它比 CoreWidth 大，八方向采样只是近似，半径很大可能显出间隙
                haloWidth.SetValue(ClientCfg.OutlineHaloWidth);
                haloStrength.SetValue(ClientCfg.OutlineHaloStrength);
                //整体强度；.fx 中同时影响输出 alpha 和预乘 RGB，不是为世界添加 Lighting 光源
                intensity.SetValue(ClientCfg.OutlineIntensity);
                outlineColor.SetValue(ClientCfg.OutlineColor.ToVector3());//自动除255转float

                //到这里已经验证纹理/sourceRect 存在；仍约定传入的是本例外扩后的命令
                DrawData data = drawData.Value;
                Texture2D texture = data.texture;
                Rectangle frame = data.sourceRect.Value;

                //留白属于这条 DrawData 的几何约定，当前半径只是像素着色器的采样距离
                frame.Inflate(-Padding, -Padding); //从外扩矩形恢复原始帧

                Vector2 inverseSize = new(1f / texture.Width, 1f / texture.Height);
                texel.SetValue(inverseSize);

                //xy 为原帧左上 UV，zw 为原帧右下 UV
                //.fx 用它隔离相邻动画帧，并把原帧外 alpha 设为 0
                frameBounds.SetValue(new Vector4(
                    frame.Left * inverseSize.X, frame.Top * inverseSize.Y,
                    frame.Right * inverseSize.X, frame.Bottom * inverseSize.Y));

                //给流动纹理使用的帧内坐标扣除图集偏移；切到下一动画帧时，光带不会因图集 Y 改变而突然跳位
                framePixels.SetValue(new Vector2(frame.X, frame.Y));

                //用秒而非 GameUpdateCount * 60，60 秒周期与 shader 内的频率整除，避免环绕跳变
                time.SetValue(Main.GlobalTimeWrappedHourly % 60f);

                //使用自己的通道
                pass.Apply();
            }

            //[16] 必要参数缺失就明确报错，通常意味着 .cs 和 .fxc 版本不配套
            //若改成 effect.Parameters[name]?.SetValue，缺少必要参数可能悄悄被忽略，效果错误却无从排查
            private static EffectParameter Required(Effect effect, string name)
            {
                return effect.Parameters[name]
                    ?? throw new InvalidOperationException("描边 shader 参数缺失，请重新编译 .fxc：" + name);
            }
        }
    }
}
