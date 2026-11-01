using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core.Utils
{
    public static class IEoR
    {
        public static bool Load
        {
            get
            {
                return ModLoader.TryGetMod("InfernalEclipseAPI", out Mod mod);
            }
        }

        public static Mod Mod
        {
            get
            {
                Mod mod;
                ModLoader.TryGetMod("InfernalEclipseAPI", out mod);
                return mod;
            }
        }

        public static string Name
        {
            get
            {
                ModLoader.TryGetMod("InfernalEclipseAPI", out Mod mod);
                return mod.Name;
            }
        }
    }
    public static class Calamity
    {
        public static bool Load
        {
            get
            {
                return ModLoader.TryGetMod("CalamityMod", out Mod mod);
            }
        }

        public static Mod Mod
        {
            get
            {
                Mod mod;
                ModLoader.TryGetMod("CalamityMod", out mod);
                return mod;
            }
        }

        public static string Name
        {
            get
            {
                ModLoader.TryGetMod("CalamityMod", out Mod mod);
                return mod.Name;
            }
        }
    }
    public static class Clicker
    {
        public static bool Load
        {
            get
            {
                return ModLoader.TryGetMod("ClickerClass", out Mod mod);
            }
        }

        public static Mod Mod
        {
            get
            {
                Mod mod;
                ModLoader.TryGetMod("ClickerClass", out mod);
                return mod;
            }
        }

        public static string Name
        {
            get
            {
                ModLoader.TryGetMod("ClickerClass", out Mod mod);
                return mod.Name;
            }
        }
    }
}
