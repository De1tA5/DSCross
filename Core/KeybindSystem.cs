using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind EnrageKeybind { get; private set; }

        public override void Load()
        {
            EnrageKeybind = KeybindLoader.RegisterKeybind(Mod, "DemonShadeEnrage", "None");
        }

        public override void Unload()
        {
            EnrageKeybind = null;
        }
    }
}
