using DemonShadeCrossMod.Core.ClassSetBonus;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace DemonShadeCrossMod.Core.Players
{
    //26.10.24 不再强制要求覆盖灾瑟拓展，才能生效
    [JITWhenModsEnabled("ThoriumMod")]
    [ExtendsFromMod("ThoriumMod")]
    public class BardSetBonus : SetBonusBase
    {
        internal override bool IsEnabled => ModLoader.HasMod("ThoriumMod") && Cfg.EnableBardSet;
        internal override bool DisplayTooltip => ClientCfg.BardDisplay;
        internal override string LocalizationKey => "Bard";
        internal override object[] TooltipArgs => [
            Cfg.BardResourceBonus,
            Cfg.ExtraBardBuffDuration,
            Cfg.BardBuffDurationBonus,
            Cfg.BardRangeBonus,
            (float)Cfg.BardInspirationRegenBonus,
            (float)Cfg.BardResourceDropBonus,
            Cfg.ExtraLevelBonus,
            ];
        internal override int DisplayOrder => ClientCfg.BardOrder;
        //加成不包含other
        private static readonly BardInstrumentType[] instrumentTypes = [
            BardInstrumentType.Wind,
            BardInstrumentType.Brass,
            BardInstrumentType.String,
            BardInstrumentType.Percussion,
            BardInstrumentType.Electronic
            ];

        public override bool InstancePerEntity => true;
        internal override void ApplyBonus(Player player)
        {
            ThoriumPlayer modPlayer = player.GetThoriumPlayer();
            modPlayer.bardResourceMax2 += Cfg.BardResourceBonus;
            modPlayer.inspirationRegenBonus += Cfg.BardInspirationRegenBonus / 100;
            modPlayer.bardResourceDropBoost += Cfg.BardResourceDropBonus / 100;
            modPlayer.bardRangeBoost += Cfg.BardRangeBonus;

            // realtime = (300 + thoriumPlayer.bardBuffDuration + DurationBonus) * thoriumPlayer.bardBuffDurationX)
            // DurationBonus - 武器前缀提供增益
            // bardBuffDurationX为百分比加乘算提升 - 虫耳药水等
            modPlayer.bardBuffDurationX += Cfg.BardBuffDurationBonus;
            // bardBuffDuration为直接增加咒音持续时间 - 装备或饰品
            modPlayer.bardBuffDuration += (short)(Cfg.ExtraBardBuffDuration * 60);

            //额外咒音增幅等级
            foreach (BardInstrumentType type in instrumentTypes)
            {
                //初始化需玩家使用一次对应乐器才能提供加成，否则会null报错
                modPlayer.musicPlayerLevels.TryAdd(type, 0);
                modPlayer.musicPlayerLevels[type] += (short)Cfg.ExtraLevelBonus;
            }
            //modPlayer.musicPlayerLevels[BardInstrumentType.Wind] += (short)Cfg.ExtraLevelBonus;
            //modPlayer.musicPlayerLevels[BardInstrumentType.Percussion] += (short)Cfg.ExtraLevelBonus;
            //modPlayer.musicPlayerLevels[BardInstrumentType.Brass] += (short)Cfg.ExtraLevelBonus;
            //modPlayer.musicPlayerLevels[BardInstrumentType.Electronic] += (short)Cfg.ExtraLevelBonus;
            //modPlayer.musicPlayerLevels[BardInstrumentType.String] += (short)Cfg.ExtraLevelBonus;
        }
    }
}
