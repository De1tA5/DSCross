using CalamityMod.Items.Armor.Demonshade;
using DemonShadeCrossMod.Core.Utils;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core.ClassSetBonus
{
    //26.10.20 使用基类优化套装效果实现，以及额外套装奖励的顺序调整
    public abstract class SetBonusBase : GlobalItem
    {
        internal static DSClientConfig ClientCfg => DSClientConfig.Instance;
        internal static DSConfig Cfg => DSConfig.Instance;
        internal abstract string LocalizationKey { get; }
        //必须按照本地化文件的文本顺序填入参数
        internal abstract object[] TooltipArgs { get; }
        //是否显示文本，不影影响属性
        internal virtual bool DisplayTooltip { get; set; } = true; //默认为true，免得我又忘加新配置选项
        internal abstract bool IsEnabled { get; }
        //配置文件修改
        internal abstract int DisplayOrder { get; }
        //默认值为硬编码，但由于存在忘记可能，以及本身存在配置文件控制，故只使用virtual
        internal virtual int DefaultDisplayOrder { get; }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (!IsEnabled)
                return "";

            if (head.type == ModContent.ItemType<DemonshadeHelm>() &&
                body.type == ModContent.ItemType<DemonshadeBreastplate>() &&
                legs.type == ModContent.ItemType<DemonshadeGreaves>())
            {
                string set = Language.GetTextValue($"Mods.DemonShadeCrossMod.ExtraSetBonus.{LocalizationKey}", TooltipArgs);
                //移除数值为0，以及作为最后一行的空格
                TooltipHelper.RemoveZeroDate(ref set);
                return set;
            }
            return "";
        }
        internal abstract void ApplyBonus(Player player);
        public override void UpdateArmorSet(Player player, string set)
        {
            if (set == "" || !IsEnabled)
                return;

            ApplyBonus(player);
            if (DisplayTooltip)
                player.GetModPlayer<SetBonusTextHandler>().AddEntry(new TooltipOrderEntry(LocalizationKey, set, DisplayOrder, DefaultDisplayOrder));
        }
    }

    //顺序结构体
    public struct TooltipOrderEntry
    {
        public readonly string key;
        public readonly string text;
        public readonly int order;
        public readonly int defaultOrder;

        public TooltipOrderEntry(string _key, string _text, int _order, int _defaultOrder = -1)
        {
            key = _key;
            text = _text;
            order = _order;
            defaultOrder = _defaultOrder;
        }
    }
}
