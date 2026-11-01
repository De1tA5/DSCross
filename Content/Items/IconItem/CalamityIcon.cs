using Terraria.ModLoader;

namespace DemonShadeCrossMod.Content.Items.IconItem
{
    public class CalamityIcon : ModItem
    {
        public override string Texture => "CalamityMod/icon_small";

        public override void SetStaticDefaults()
        {
            Item.height = Item.width = 30;
        }
    }
}
