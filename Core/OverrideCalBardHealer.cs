using CalamityBardHealer;
using MonoMod.RuntimeDetour;
using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core
{
    [JITWhenModsEnabled("CalamityBardHealer")]
    [ExtendsFromMod("CalamityBardHealer")]
    public class OverrideCalBardHealer : ModSystem
    {
        private delegate bool Orig_AppliesToEntity(CalamityArmorChanges self, Item item, bool lateInstatiation);
        private static Hook overrideHook;
        public override void Load()
        {
            if (!ModLoader.TryGetMod("CalamityBardHealer", out Mod mod))
                return;

            if (!DSConfig.Instance.OverrideCalBardHealer)
                return;

            Type type = mod.Code.GetType("CalamityBardHealer.CalamityArmorChanges");

            MethodInfo method = type?.GetMethod("AppliesToEntity", BindingFlags.Public | BindingFlags.Instance);

            if (method is not null)
                overrideHook = new Hook(method, On_AppliesToEntity);

        }

        public override void Unload()
        {
            overrideHook?.Dispose();
            overrideHook = null;
        }

        private static bool On_AppliesToEntity(Orig_AppliesToEntity orig, CalamityArmorChanges self, Item item, bool lateInstatiation)
        {
            //移除原有的魔影套
            string[] itemName = [
                "AstralBreastplate",
                "AstralHelm",
                "WulfrumJacket"
                ];

            if (ModLoader.HasMod("CalamityMod"))
            {
                foreach (var name in itemName)
                {
                    if (!ModContent.TryFind<ModItem>($"CalamityMod/{name}", out ModItem moditem))
                        return false;

                    return item.type == moditem.Type;
                }
            }
            return false;
        }
    }
}
