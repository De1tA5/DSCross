using CalamityMod;
using DemonShadeCrossMod.Core.Utils;
using System;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DemonShadeCrossMod.Core.ClassSetBonus
{
    //26.10.20 根据配置文件调整额外套装奖励文本顺序handler
    public class SetBonusTextHandler : ModPlayer
    {
        private readonly List<TooltipOrderEntry> entryList = new List<TooltipOrderEntry>();
        public override void ResetEffects()
        {
            entryList.Clear();
        }

        public override void UpdateDead()
        {
            entryList.Clear();
        }

        internal void AddEntry(TooltipOrderEntry entry) => entryList.Add(entry);

        public override void PostUpdateEquips()
        {
            if (!Player.Calamity().demonshadeSet)
                return;

            //获取原套装奖励
            string originalText = (Player.setBonus ?? string.Empty);

            List<string> sections = new List<string>();
            if (!string.IsNullOrWhiteSpace(originalText))
            {
                //原来的套装奖励进行修改
                Player.setBonus = $"\n{originalText}\n";
            }

            //红恶魔文本
            if (DSConfig.Instance.ReAddRedDevil)
            {
                Player.setBonus += $"\n{Language.GetTextValue("Mods.DemonShadeCrossMod.ExtraSetBonus.RedDevilSetBonus")}\n";
            }

            //恶魔之炎文本
            if (DSConfig.Instance.DemonFlamesBack)
            {
                Player.setBonus += $"\n{Language.GetTextValue("Mods.DemonShadeCrossMod.ExtraSetBonus.DemonFlamesSetBonus")}\n";
            }

            if (entryList.Count != 0)
            {
                //顺序调整
                entryList.Sort(CompareEntries);

                //拼接额外套装奖励
                foreach (TooltipOrderEntry entry in entryList)
                {
                    sections.Add(entry.text);
                }
            }

            Player.setBonus += string.Join("\n", sections);
            TooltipHelper.RemoveDoubleLF(ref Player.setBonus);
            TooltipHelper.RemoveZeroDate(ref Player.setBonus);
            entryList.Clear();
        }

        //顺序比较
        private static int CompareEntries(TooltipOrderEntry left, TooltipOrderEntry right)
        {
            int result = left.order.CompareTo(right.order);
            if (result != 0)
            {
                //不相等情况
                return result;
            }

            //defaultOrder均填写的情况，避免硬编码缺失
            if (left.defaultOrder != -1 && right.defaultOrder != -1)
            {

                result = left.defaultOrder.CompareTo(right.defaultOrder);
            }

            //相等或defaultOrder缺失，用key的字符串比较前后
            return result != 0 ? result : StringComparer.Ordinal.Compare(left.key, right.key);
        }
    }
}
