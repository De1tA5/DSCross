using Terraria;

namespace DemonShadeCrossMod.Core.Utils
{
    public static class PlayerHelper
    {
        public static DSPlayer DSPlayer(this Player player)
        {
            return player.GetModPlayer<DSPlayer>();
        }
    }
}
