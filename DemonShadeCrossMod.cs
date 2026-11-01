using Terraria.Localization;
using Terraria.ModLoader;

namespace DemonShadeCrossMod
{
    public class DemonShadeCrossMod : Mod
    {
        private class ModNameUpdater : ModSystem
        {
            public override void OnLocalizationsLoaded()
            {
                var _modName = Mod?.GetType().GetProperty("DisplayName");
                _modName.SetValue(Mod, Language.GetTextValue("Mods.DemonShadeCrossMod.ModName"));
            }
        }
    }
}
