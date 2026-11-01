# DemonShadeCrossMod 中英翻译文本库

来源：Localization/zh-Hans；译文：Localization/en-US。日期：2026-11-01。

本次仅更新 3 个对应的英文 HJSON 文件；中文文件逐字节保持不变。

最新 CSV：`zh-Hans_en-US_translation_catalog_wiki_revision.csv`（原表被占用，修订另存）。

CSV 为可编辑校对文本库，含完整键名、中文原文、英文译文、原英文、校对状态、备注及人工修订英文列。
CSV 使用 UTF-8 BOM，便于 Excel 识别中文；多行文本以字面量 `\n` 表示换行。此 Markdown 中用换行展示。
“原文为空”的提示保持为空；“待人工校对”表示已生成英文，尚未人工确认。
修改 CSV 或此文档不会自动回写游戏文件；确认后请按键名更新 en-US 中的对应值。
原英文备份位于 Temp/LocalizationTranslation_20261101/en-US_before。

## 数量

| 文件 | 总键数 | 非空译文 | 原文为空 |
| --- | ---: | ---: | ---: |
| Mods.DemonShadeCrossMod.hjson | 35 | 35 | 0 |
| Mods.DemonShadeCrossMod.Configs.DSClientConfig.hjson | 45 | 39 | 6 |
| Mods.DemonShadeCrossMod.Configs.DSConfig.hjson | 79 | 53 | 26 |

## 优先校对

- `ExtraSetBonus.Bard` 的 `{1}`、`{2}` 保持中文含义；代码传参顺序疑似相反，详见该条备注。
- “诸神的黄昏”暂译为 `Ragnarok`，需确认具体模组名称。
- 其他模组的等级、技巧点限制以及独立乘算说明均按中文翻译，未改写游戏规则。

## 术语表

| 中文 | 英文 |
| --- | --- |
| 魔影套 | Demonshade armor / Demonshade set |
| 魔影激怒 | Demonshade Enrage |
| 魔影焰击 | Demonshade Strike |
| 魔影爆破 | Demonshade Explosion |
| 恶魔之炎 | Demon Flames |
| 红恶魔 | Red Devil |
| 盗贼 / 牧师 / 吟游诗人 / 投手 / 点击者 / 虚空 | Rogue / Healer / Bard / Thrower / Clicker / Void |
| 潜伏值 | stealth |
| 技巧点 / 技巧值 | technique points |
| 疲劳值 | exhaustion |
| 灵感值 | inspiration |
| 灵感音符 | inspiration notes |
| 咒音增幅 | empowerment |
| 轮廓光 | glowing outline / outline |
| 残影 | shadow |
| 全局 / 静态 / 独立无敌帧 | Global / Static / Local Invincibility |

## 无敌帧术语修订（2026-11-01）

参考：[Terraria Wiki · Invincibility frame](https://terraria.wiki.gg/wiki/Invincibility_frame)、[tModLoader · Projectile](https://docs.tmodloader.net/docs/stable/class_projectile.html)。
名称统一使用 Global / Static / Local Invincibility；英文提示按命中冷却的共享范围重新表述，推荐场景仍保留中文原意。中文原文未修改。

## Mods.DemonShadeCrossMod.hjson

### 001 · `Mods.DemonShadeCrossMod.ModName`

**中文原文**

```text
跨模组魔影套强化
```

**英文译文**

```text
Demonshade Set Crossmod Enhancement
```

校对状态：待人工校对

### 002 · `Mods.DemonShadeCrossMod.Keybinds.DemonShadeEnrage.DisplayName`

**中文原文**

```text
魔影激怒
```

**英文译文**

```text
Demonshade Enrage
```

校对状态：待人工校对

### 003 · `Mods.DemonShadeCrossMod.ClickEffect.DemonShadeClickerEffect.Name`

**中文原文**

```text
魔影焰击
```

**英文译文**

```text
Demonshade Strike
```

校对状态：待人工校对

### 004 · `Mods.DemonShadeCrossMod.ClickEffect.DemonShadeClickerEffect.Description`

**中文原文**

```text
降下恐怖魔焰焚毁你的敌人！
```

**英文译文**

```text
Rain down terrifying demon flames to incinerate your enemies!
```

校对状态：待人工校对

### 005 · `Mods.DemonShadeCrossMod.Buffs.DemonFlames.DisplayName`

**中文原文**

```text
恶魔之炎
```

**英文译文**

```text
Demon Flames
```

校对状态：待人工校对

### 006 · `Mods.DemonShadeCrossMod.Buffs.DemonFlames.Description`

**中文原文**

```text
来自异界的恐怖焚烧已然归来
```

**英文译文**

```text
The terror of otherworldly flames has returned
```

校对状态：待人工校对

### 007 · `Mods.DemonShadeCrossMod.Buffs.DemonFlames.ItemTooltipPlayer`

**中文原文**

```text
火系 - 1250 DPS
```

**英文译文**

```text
Fire - 1250 DPS
```

校对状态：待人工校对

### 008 · `Mods.DemonShadeCrossMod.Buffs.DemonFlames.ItemTooltipEnemy`

**中文原文**

```text
火系 - 1250 DPS
额外增加15%对敌怪造成的伤害
```

**英文译文**

```text
Fire - 1250 DPS
Increases damage dealt to enemies by an additional 15%
```

校对状态：待人工校对

### 009 · `Mods.DemonShadeCrossMod.ExtraSetBonus.RedDevilSetBonus`

**中文原文**

```text
重新召唤一只红恶魔为你而战
```

**英文译文**

```text
Once again summons a Red Devil to fight for you
```

校对状态：待人工校对

### 010 · `Mods.DemonShadeCrossMod.ExtraSetBonus.DemonFlamesSetBonus`

**中文原文**

```text
击中敌怪施加其[cbuff:DemonShadeCrossMod/DemonFlames]减益
```

**英文译文**

```text
Hits inflict the [cbuff:DemonShadeCrossMod/DemonFlames] debuff on enemies
```

校对状态：待人工校对

### 011 · `Mods.DemonShadeCrossMod.ExtraSetBonus.Rogue`

**中文原文**

```text
[C/A74F01:盗贼]
增加{0}点最大潜伏值
提升{1}%移动时潜伏值恢复速度
提升{2}%静止时潜伏值恢复速度
提升{3}%盗贼投掷弹幕速度
```

**英文译文**

```text
[C/A74F01:Rogue]
Increases maximum stealth by {0}
Increases stealth generation rate while moving by {1}%
Increases stealth generation rate while standing still by {2}%
Increases rogue projectile velocity by {3}%
```

校对状态：待人工校对

### 012 · `Mods.DemonShadeCrossMod.ExtraSetBonus.Thrower`

**中文原文**

```text
[C/FC9C41:投手]
增加{0}点技巧值
增加{1}点最大疲劳值
提升{2}%疲劳值恢复速度
提升{3}%技巧值充能速度
```

**英文译文**

```text
[C/FC9C41:Thrower]
Increases technique points by {0}
Increases maximum exhaustion by {1}
Increases exhaustion regeneration rate by {2}%
Increases technique point recharge rate by {3}%
```

校对状态：待人工校对

### 013 · `Mods.DemonShadeCrossMod.ExtraSetBonus.Bard`

**中文原文**

```text
[C/95F456:吟游诗人]
增加{0}点额外最大灵感值
提升{1}秒咒音增幅持续时间
增加{2}%咒音增幅持续时间
增加{3}%咒音增幅覆盖范围
提升{4}%灵感值再生速度
增加{5}%灵感音符掉落掉落概率
咒音增幅额外提升{6}级
```

**英文译文**

```text
[C/95F456:Bard]
Increases maximum inspiration by {0}
Increases empowerment duration by {1} seconds
Increases empowerment duration by {2}%
Increases empowerment effect radius by {3}%
Increases inspiration regeneration rate by {4}%
Increases inspiration note drop chance by {5}%
Increases empowerment levels by an additional {6}
```

校对状态：待人工校对

校对备注：优先核对：保持中文占位符语义（{1} 秒、{2}%）。但 BardSetBonus.cs 的 TooltipArgs 当前先传 BardBuffDurationBonus，再传 ExtraBardBuffDuration，疑似与中英文文本顺序相反；本次未修改代码或中文。中文“掉落掉落”按一次“掉落”译出。

### 014 · `Mods.DemonShadeCrossMod.ExtraSetBonus.Healer`

**中文原文**

```text
[C/FFF850:牧师]
治疗法术会额外治疗{0}点生命值
提升{1}%治疗法术施法速度
```

**英文译文**

```text
[C/FFF850:Healer]
Healing spells restore an additional {0} life
Increases healing spell cast speed by {1}%
```

校对状态：待人工校对

### 015 · `Mods.DemonShadeCrossMod.ExtraSetBonus.Clicker`

**中文原文**

```text
[C/D2F8FB:点击者]
增加{0}%基础点击范围
减少{1}%点击效果所需次数
```

**英文译文**

```text
[C/D2F8FB:Clicker]
Increases base click radius by {0}%
Reduces the number of clicks required for click effects by {1}%
```

校对状态：待人工校对

### 016 · `Mods.DemonShadeCrossMod.ExtraSetBonus.Void`

**中文原文**

```text
[C/6F00D8:虚空]
增加{0}点最大虚空值
提升{1}%虚空值恢复速度
增加{2}虚空恢复量
```

**英文译文**

```text
[C/6F00D8:Void]
Increases maximum void by {0}
Increases void regeneration rate by {1}%
Increases void gain by {2}
```

校对状态：待人工校对

### 017 · `Mods.DemonShadeCrossMod.Projectiles.Typeless.DemonshadeRedDevil.DisplayName`

**中文原文**

```text
红恶魔
```

**英文译文**

```text
Red Devil
```

校对状态：待人工校对

### 018 · `Mods.DemonShadeCrossMod.Projectiles.DemonshadeStrike.DisplayName`

**中文原文**

```text
魔影焰击
```

**英文译文**

```text
Demonshade Strike
```

校对状态：待人工校对

### 019 · `Mods.DemonShadeCrossMod.Projectiles.DemonshadeExplosin.DisplayName`

**中文原文**

```text
魔影爆破
```

**英文译文**

```text
Demonshade Explosion
```

校对状态：待人工校对

校对备注：仅修正显示名称为 Explosion；原键名中的 Explosin 保持不变。

### 020 · `Mods.DemonShadeCrossMod.Projectiles.DemonshadeStrike2.DisplayName`

**中文原文**

```text
魔影焰击
```

**英文译文**

```text
Demonshade Strike
```

校对状态：待人工校对

校对备注：与中文一致显示为魔影焰击，不在显示名后添加 2。

### 021 · `Mods.DemonShadeCrossMod.Items.CalamityIcon.DisplayName`

**中文原文**

```text
灾厄图标
```

**英文译文**

```text
Calamity Icon
```

校对状态：待人工校对

### 022 · `Mods.DemonShadeCrossMod.Items.CalamityIcon.Tooltip`

**中文原文**

```text
你不该看到的
```

**英文译文**

```text
You are not supposed to see this
```

校对状态：待人工校对

### 023 · `Mods.DemonShadeCrossMod.Items.ClickerIcon.DisplayName`

**中文原文**

```text
点击者图标
```

**英文译文**

```text
Clicker Class Icon
```

校对状态：待人工校对

### 024 · `Mods.DemonShadeCrossMod.Items.ClickerIcon.Tooltip`

**中文原文**

```text
你不该看到的
```

**英文译文**

```text
You are not supposed to see this
```

校对状态：待人工校对

### 025 · `Mods.DemonShadeCrossMod.Items.SOTSIcon.DisplayName`

**中文原文**

```text
暗影奥秘图标
```

**英文译文**

```text
Secrets Of The Shadows Icon
```

校对状态：待人工校对

### 026 · `Mods.DemonShadeCrossMod.Items.SOTSIcon.Tooltip`

**中文原文**

```text
你不该看到的
```

**英文译文**

```text
You are not supposed to see this
```

校对状态：待人工校对

### 027 · `Mods.DemonShadeCrossMod.Items.ThoriumIcon.DisplayName`

**中文原文**

```text
瑟银图标
```

**英文译文**

```text
Thorium Icon
```

校对状态：待人工校对

### 028 · `Mods.DemonShadeCrossMod.Items.ThoriumIcon.Tooltip`

**中文原文**

```text
你不该看到的
```

**英文译文**

```text
You are not supposed to see this
```

校对状态：待人工校对

### 029 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Tooltip`

**中文原文**

```text
决定魔影焰击的无敌帧类型
```

**英文译文**

```text
Determines the type of invincibility frames Demonshade Strike applies to enemies
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。

### 030 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Global.Label`

**中文原文**

```text
全局无敌帧
```

**英文译文**

```text
Global Invincibility
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。

### 031 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Global.Tooltip`

**中文原文**

```text
会受到或导致所有攻击骗伤
（没人喜欢骗伤）
```

**英文译文**

```text
After a hit, the enemy temporarily cannot be damaged by other piercing attacks and melee hits from the same player that use global invincibility
(Nobody likes losing damage to shared i-frames)
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。将“所有攻击”收窄为同一玩家使用全局无敌帧的穿透攻击与近战命中。

### 032 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Static.Label`

**中文原文**

```text
静态无敌帧
```

**英文译文**

```text
Static Invincibility
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。

### 033 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Static.Tooltip`

**中文原文**

```text
导致同类型弹幕之间攻击的骗伤
敌怪越小，弹幕种类越多越推荐该类型
```

**英文译文**

```text
After a hit, the enemy temporarily cannot be damaged by projectiles of the same type that use static invincibility
Other projectile types can still hit
Recommended for smaller enemies and when using many different projectile types
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。按同类型弹幕共享冷却表达，未加入 Wiki 当前页面中可能存在版本差异的多人范围描述；推荐场景保留原文。

### 034 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Local.Label`

**中文原文**

```text
独立无敌帧
```

**英文译文**

```text
Local Invincibility
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。

### 035 · `Mods.DemonShadeCrossMod.Configs.ImmunityFrameType.Local.Tooltip`

**中文原文**

```text
导致同一个弹幕攻击的骗伤
最通用且敌怪越大，接触时间越长，越推荐该类型
```

**英文译文**

```text
After a hit, only that specific projectile must wait before it can damage the enemy again
Other projectiles, even those of the same type, can still hit
The most versatile option; recommended for larger enemies and longer contact times
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。明确单个弹幕实例的冷却不会阻止其他同类型弹幕命中；推荐场景保留原文。


## Mods.DemonShadeCrossMod.Configs.DSClientConfig.hjson

### 036 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.DisplayName`

**中文原文**

```text
客户端配置
```

**英文译文**

```text
Client Configuration
```

校对状态：待人工校对

### 037 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.Headers.TooltipOrder`

**中文原文**

```text
套装文本
```

**英文译文**

```text
Set Bonus Tooltips
```

校对状态：待人工校对

### 038 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.Headers.VFX`

**中文原文**

```text
视觉效果
```

**英文译文**

```text
Visual Effects
```

校对状态：待人工校对

### 039 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.RogueOrder.Label`

**中文原文**

```text
盗贼
```

**英文译文**

```text
Rogue
```

校对状态：待人工校对

### 040 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.RogueOrder.Tooltip`

**中文原文**

```text
数值越小，越靠前
```

**英文译文**

```text
Lower values appear first
```

校对状态：待人工校对

### 041 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.HealerOrder.Label`

**中文原文**

```text
牧师
```

**英文译文**

```text
Healer
```

校对状态：待人工校对

### 042 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.HealerOrder.Tooltip`

**中文原文**

```text
数值越小，越靠前
```

**英文译文**

```text
Lower values appear first
```

校对状态：待人工校对

### 043 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.BardOrder.Label`

**中文原文**

```text
乐师
```

**英文译文**

```text
Bard
```

校对状态：待人工校对

### 044 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.BardOrder.Tooltip`

**中文原文**

```text
数值越小，越靠前
```

**英文译文**

```text
Lower values appear first
```

校对状态：待人工校对

### 045 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ThrowerOrder.Label`

**中文原文**

```text
投手
```

**英文译文**

```text
Thrower
```

校对状态：待人工校对

### 046 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ThrowerOrder.Tooltip`

**中文原文**

```text
数值越小，越靠前
```

**英文译文**

```text
Lower values appear first
```

校对状态：待人工校对

### 047 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ClickerOrder.Label`

**中文原文**

```text
点击者
```

**英文译文**

```text
Clicker
```

校对状态：待人工校对

### 048 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ClickerOrder.Tooltip`

**中文原文**

```text
数值越小，越靠前
```

**英文译文**

```text
Lower values appear first
```

校对状态：待人工校对

### 049 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.VoidOrder.Label`

**中文原文**

```text
虚空
```

**英文译文**

```text
Void
```

校对状态：待人工校对

### 050 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.VoidOrder.Tooltip`

**中文原文**

```text
数值越小，越靠前
```

**英文译文**

```text
Lower values appear first
```

校对状态：待人工校对

### 051 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.DrawDataTest.Label`

**中文原文**

```text
绘制调试
```

**英文译文**

```text
Drawing Debug Tools
```

校对状态：待人工校对

### 052 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.DrawDataTest.Tooltip`

**中文原文**

```text
开发者使用调试功能
```

**英文译文**

```text
Debug tools for developers
```

校对状态：待人工校对

### 053 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ShadowOutline.Label`

**中文原文**

```text
绘制残影轮廓光
```

**英文译文**

```text
Shadow Outlines
```

校对状态：待人工校对

### 054 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ShadowOutline.Tooltip`

**中文原文**

```text
给残影也进行绘制轮廓光
```

**英文译文**

```text
Also draws glowing outlines around shadows
```

校对状态：待人工校对

### 055 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.DemonshadeOutline.Label`

**中文原文**

```text
魔影套轮廓光
```

**英文译文**

```text
Demonshade Armor Outline
```

校对状态：待人工校对

### 056 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.DemonshadeOutline.Tooltip`

**中文原文**

```text
启用该选项后，以下配置参数才有作用
```

**英文译文**

```text
Enable this option for the settings below to take effect
```

校对状态：待人工校对

### 057 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.AllArmorOutline.Label`

**中文原文**

```text
实验性：全部盔甲轮廓光绘制
```

**英文译文**

```text
Experimental: Outlines for All Armor
```

校对状态：待人工校对

### 058 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.AllArmorOutline.Tooltip`

**中文原文**

```text
支持原版和其他模组盔甲的轮廓光绘制
但由于部分盔甲带有额外材质贴图，无法保证全部有轮廓包围
```

**英文译文**

```text
Supports glowing outlines for vanilla and modded armor
Some armor uses additional textures, so the outline may not cover every part
```

校对状态：待人工校对

### 059 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineColor.Label`

**中文原文**

```text
轮廓光颜色
```

**英文译文**

```text
Outline Color
```

校对状态：待人工校对

### 060 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineColor.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 061 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineCoreWidth.Label`

**中文原文**

```text
轮廓光宽度
```

**英文译文**

```text
Outline Width
```

校对状态：待人工校对

### 062 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineCoreWidth.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 063 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineHaloWidth.Label`

**中文原文**

```text
轮廓光光晕宽度
```

**英文译文**

```text
Outline Halo Width
```

校对状态：待人工校对

### 064 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineHaloWidth.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 065 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineHaloStrength.Label`

**中文原文**

```text
轮廓光光晕强度
```

**英文译文**

```text
Outline Halo Strength
```

校对状态：待人工校对

### 066 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineHaloStrength.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 067 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineIntensity.Label`

**中文原文**

```text
轮廓光总体强度
```

**英文译文**

```text
Overall Outline Intensity
```

校对状态：待人工校对

### 068 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.OutlineIntensity.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 069 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.RogueDisplay.Label`

**中文原文**

```text
显示盗贼文本
```

**英文译文**

```text
Show Rogue Tooltip
```

校对状态：待人工校对

### 070 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.RogueDisplay.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 071 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.HealerDisplay.Label`

**中文原文**

```text
显示牧师文本
```

**英文译文**

```text
Show Healer Tooltip
```

校对状态：待人工校对

### 072 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.HealerDisplay.Tooltip`

**中文原文**

```text
仅移除文本，实际效果不移除
```

**英文译文**

```text
Only hides the tooltip; the effects still apply
```

校对状态：待人工校对

### 073 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.BardDisplay.Label`

**中文原文**

```text
显示吟游诗人文本
```

**英文译文**

```text
Show Bard Tooltip
```

校对状态：待人工校对

### 074 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.BardDisplay.Tooltip`

**中文原文**

```text
仅移除文本，实际效果不移除
```

**英文译文**

```text
Only hides the tooltip; the effects still apply
```

校对状态：待人工校对

### 075 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ThrowerDisplay.Label`

**中文原文**

```text
显示投手文本
```

**英文译文**

```text
Show Thrower Tooltip
```

校对状态：待人工校对

### 076 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ThrowerDisplay.Tooltip`

**中文原文**

```text
仅移除文本，实际效果不移除
```

**英文译文**

```text
Only hides the tooltip; the effects still apply
```

校对状态：待人工校对

### 077 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ClickerDisplay.Label`

**中文原文**

```text
显示点击者文本
```

**英文译文**

```text
Show Clicker Tooltip
```

校对状态：待人工校对

### 078 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.ClickerDisplay.Tooltip`

**中文原文**

```text
仅移除文本，实际效果不移除
```

**英文译文**

```text
Only hides the tooltip; the effects still apply
```

校对状态：待人工校对

### 079 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.VoidDisplay.Label`

**中文原文**

```text
显示虚空文本
```

**英文译文**

```text
Show Void Tooltip
```

校对状态：待人工校对

### 080 · `Mods.DemonShadeCrossMod.Configs.DSClientConfig.VoidDisplay.Tooltip`

**中文原文**

```text
仅移除文本，实际效果不移除
```

**英文译文**

```text
Only hides the tooltip; the effects still apply
```

校对状态：待人工校对


## Mods.DemonShadeCrossMod.Configs.DSConfig.hjson

### 081 · `Mods.DemonShadeCrossMod.Configs.DSConfig.DisplayName`

**中文原文**

```text
服务器配置
```

**英文译文**

```text
Server Configuration
```

校对状态：待人工校对

### 082 · `Mods.DemonShadeCrossMod.Configs.DSConfig.Headers.Calamity`

**中文原文**

```text
[i:DemonShadeCrossMod/CalamityIcon]灾厄
```

**英文译文**

```text
[i:DemonShadeCrossMod/CalamityIcon]Calamity
```

校对状态：待人工校对

### 083 · `Mods.DemonShadeCrossMod.Configs.DSConfig.Headers.Thorium`

**中文原文**

```text
[i:DemonShadeCrossMod/ThoriumIcon]瑟银
```

**英文译文**

```text
[i:DemonShadeCrossMod/ThoriumIcon]Thorium
```

校对状态：待人工校对

### 084 · `Mods.DemonShadeCrossMod.Configs.DSConfig.Headers.ClickerClass`

**中文原文**

```text
[i:DemonShadeCrossMod/ClickerIcon]点击者
```

**英文译文**

```text
[i:DemonShadeCrossMod/ClickerIcon]Clicker Class
```

校对状态：待人工校对

### 085 · `Mods.DemonShadeCrossMod.Configs.DSConfig.Headers.SOTS`

**中文原文**

```text
[i:DemonShadeCrossMod/SOTSIcon]暗影奥秘
```

**英文译文**

```text
[i:DemonShadeCrossMod/SOTSIcon]Secrets Of The Shadows
```

校对状态：待人工校对

### 086 · `Mods.DemonShadeCrossMod.Configs.DSConfig.TriggerTime.Label`

**中文原文**

```text
自动触发魔影激怒时间 （秒）
```

**英文译文**

```text
Demonshade Enrage Auto-Activation Delay (Seconds)
```

校对状态：待人工校对

### 087 · `Mods.DemonShadeCrossMod.Configs.DSConfig.TriggerTime.Tooltip`

**中文原文**

```text
长按本模组提供魔影激怒键，自动触发所需的时间
```

**英文译文**

```text
How long to hold the Demonshade Enrage key provided by this mod before it activates automatically
```

校对状态：待人工校对

### 088 · `Mods.DemonShadeCrossMod.Configs.DSConfig.RemoveEnrageSound.Label`

**中文原文**

```text
移除魔影激怒的音效
```

**英文译文**

```text
Remove Demonshade Enrage Sound
```

校对状态：待人工校对

### 089 · `Mods.DemonShadeCrossMod.Configs.DSConfig.RemoveEnrageSound.Tooltip`

**中文原文**

```text
仅移除本模组提供的魔影激怒音效
```

**英文译文**

```text
Only removes the Demonshade Enrage sound added by this mod
```

校对状态：待人工校对

### 090 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ReAddRedDevil.Label`

**中文原文**

```text
重新添加红恶魔
```

**英文译文**

```text
Restore Red Devil
```

校对状态：待人工校对

### 091 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ReAddRedDevil.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 092 · `Mods.DemonShadeCrossMod.Configs.DSConfig.DemonFlamesBack.Label`

**中文原文**

```text
攻击施加恶魔之炎
```

**英文译文**

```text
Inflict Demon Flames on Hit
```

校对状态：待人工校对

### 093 · `Mods.DemonShadeCrossMod.Configs.DSConfig.DemonFlamesBack.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 094 · `Mods.DemonShadeCrossMod.Configs.DSConfig.RemoveDsSetNerf.Label`

**中文原文**

```text
移除魔影激怒的削弱
```

**英文译文**

```text
Revert Demonshade Enrage Nerf
```

校对状态：待人工校对

### 095 · `Mods.DemonShadeCrossMod.Configs.DSConfig.RemoveDsSetNerf.Tooltip`

**中文原文**

```text
由1.5倍回调至未削弱的2.25倍
```

**英文译文**

```text
Restores the multiplier from 1.5x to its original 2.25x
```

校对状态：待人工校对

### 096 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableStealthMeter.Label`

**中文原文**

```text
启用额外潜伏条
```

**英文译文**

```text
Enable Additional Stealth Meter
```

校对状态：待人工校对

### 097 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableStealthMeter.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 098 · `Mods.DemonShadeCrossMod.Configs.DSConfig.StealthMeterMax.Label`

**中文原文**

```text
额外最大潜伏值
```

**英文译文**

```text
Additional Maximum Stealth
```

校对状态：待人工校对

### 099 · `Mods.DemonShadeCrossMod.Configs.DSConfig.StealthMeterMax.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 100 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraRogueVelocity.Label`

**中文原文**

```text
盗贼弹幕速度
```

**英文译文**

```text
Rogue Projectile Velocity
```

校对状态：待人工校对

### 101 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraRogueVelocity.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 102 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraStealthGenMoving.Label`

**中文原文**

```text
移动时潜伏值恢复速度
```

**英文译文**

```text
Stealth Generation Rate While Moving
```

校对状态：待人工校对

### 103 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraStealthGenMoving.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 104 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraStealthGenStandStill.Label`

**中文原文**

```text
静止时潜伏值恢复速度
```

**英文译文**

```text
Stealth Generation Rate While Standing Still
```

校对状态：待人工校对

### 105 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraStealthGenStandStill.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 106 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableHealerSet.Label`

**中文原文**

```text
启用牧师套装奖励
```

**英文译文**

```text
Enable Healer Set Bonus
```

校对状态：待人工校对

### 107 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableHealerSet.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 108 · `Mods.DemonShadeCrossMod.Configs.DSConfig.HealBonus.Label`

**中文原文**

```text
额外治疗量
```

**英文译文**

```text
Additional Healing
```

校对状态：待人工校对

### 109 · `Mods.DemonShadeCrossMod.Configs.DSConfig.HealBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 110 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableBardSet.Label`

**中文原文**

```text
启用吟游诗人套装奖励
```

**英文译文**

```text
Enable Bard Set Bonus
```

校对状态：待人工校对

### 111 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableBardSet.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 112 · `Mods.DemonShadeCrossMod.Configs.DSConfig.OverrideCalBardHealer.Label`

**中文原文**

```text
[C/FF9900:覆盖《非官方灾厄吟游诗人和牧师》]
```

**英文译文**

```text
[C/FF9900:Override Unofficial Calamity Bard & Healer]
```

校对状态：待人工校对

### 113 · `Mods.DemonShadeCrossMod.Configs.DSConfig.OverrideCalBardHealer.Tooltip`

**中文原文**

```text
取消《非官方灾厄吟游诗人和牧师》提供魔影套的牧师和吟游诗人强化
```

**英文译文**

```text
Disables the Healer and Bard bonuses to Demonshade armor provided by Unofficial Calamity Bard & Healer
```

校对状态：待人工校对

### 114 · `Mods.DemonShadeCrossMod.Configs.DSConfig.HealToolSpeed.Label`

**中文原文**

```text
治疗法术使用速度
```

**英文译文**

```text
Healing Spell Cast Speed
```

校对状态：待人工校对

### 115 · `Mods.DemonShadeCrossMod.Configs.DSConfig.HealToolSpeed.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 116 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardInspirationRegenBonus.Label`

**中文原文**

```text
灵感值再生速度
```

**英文译文**

```text
Inspiration Regeneration Rate
```

校对状态：待人工校对

### 117 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardInspirationRegenBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 118 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardResourceDropBonus.Label`

**中文原文**

```text
灵感音符掉落提升
```

**英文译文**

```text
Inspiration Note Drop Chance Bonus
```

校对状态：待人工校对

### 119 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardResourceDropBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 120 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardResourceBonus.Label`

**中文原文**

```text
额外最大灵感值
```

**英文译文**

```text
Additional Maximum Inspiration
```

校对状态：待人工校对

### 121 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardResourceBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 122 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardBuffDurationBonus.Label`

**中文原文**

```text
咒音增幅持续时间乘算加成
```

**英文译文**

```text
Empowerment Duration Multiplier Bonus
```

校对状态：待人工校对

### 123 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardBuffDurationBonus.Tooltip`

**中文原文**

```text
此为最高的独立乘算区间
```

**英文译文**

```text
This is a separate multiplier applied at the final stage
```

校对状态：待人工校对

校对备注：“最高的独立乘算区间”按最终阶段的独立乘数表达；请结合实际叠加规则校对。

### 124 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraBardBuffDuration.Label`

**中文原文**

```text
额外咒音增幅持续时间 （秒）
```

**英文译文**

```text
Additional Empowerment Duration (Seconds)
```

校对状态：待人工校对

### 125 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraBardBuffDuration.Tooltip`

**中文原文**

```text
此为加算区间
```

**英文译文**

```text
This bonus is additive
```

校对状态：待人工校对

### 126 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardRangeBonus.Label`

**中文原文**

```text
咒音增幅覆盖范围
```

**英文译文**

```text
Empowerment Effect Radius
```

校对状态：待人工校对

### 127 · `Mods.DemonShadeCrossMod.Configs.DSConfig.BardRangeBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 128 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraLevelBonus.Label`

**中文原文**

```text
额外咒音增幅等级
```

**英文译文**

```text
Additional Empowerment Levels
```

校对状态：待人工校对

### 129 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ExtraLevelBonus.Tooltip`

**中文原文**

```text
原版《瑟银》最高只有4级，要更高等级需要《诸神的黄昏》
```

**英文译文**

```text
Thorium alone only supports up to level 4; higher levels require Ragnarok
```

校对状态：待人工校对

校对备注：“诸神的黄昏”暂译为 Ragnarok；请确认所指扩展模组及其正式英文名。等级限制按中文原文翻译，未验证第三方模组当前行为。

### 130 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableThrowerSet.Label`

**中文原文**

```text
启用投手套装奖励
```

**英文译文**

```text
Enable Thrower Set Bonus
```

校对状态：待人工校对

### 131 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableThrowerSet.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 132 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerTechPointsBonus.Label`

**中文原文**

```text
投手技巧点加成
```

**英文译文**

```text
Technique Point Bonus
```

校对状态：待人工校对

### 133 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerTechPointsBonus.Tooltip`

**中文原文**

```text
原瑟银只能多2点额外技巧点
```

**英文译文**

```text
Thorium alone only supports 2 additional technique points
```

校对状态：待人工校对

校对备注：技巧点上限按中文原文翻译，未验证第三方模组当前行为。

### 134 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerExhaustionMaxBonus.Label`

**中文原文**

```text
投手最大疲劳值加成
```

**英文译文**

```text
Maximum Exhaustion Bonus
```

校对状态：待人工校对

### 135 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerExhaustionMaxBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 136 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerExhaustionRegenBonus.Label`

**中文原文**

```text
投手疲劳值再生加成
```

**英文译文**

```text
Exhaustion Regeneration Bonus
```

校对状态：待人工校对

### 137 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerExhaustionRegenBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 138 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerTechRechargeBonus.Label`

**中文原文**

```text
投手技巧点充能加成
```

**英文译文**

```text
Technique Point Recharge Bonus
```

校对状态：待人工校对

### 139 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ThrowerTechRechargeBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 140 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableClickerSet.Label`

**中文原文**

```text
启用点击者套装奖励
```

**英文译文**

```text
Enable Clicker Set Bonus
```

校对状态：待人工校对

### 141 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableClickerSet.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 142 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerRadiusBonus.Label`

**中文原文**

```text
点击范围加成
```

**英文译文**

```text
Click Radius Bonus
```

校对状态：待人工校对

### 143 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerRadiusBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 144 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerEffectBonus.Label`

**中文原文**

```text
点击效果加成
```

**英文译文**

```text
Click Effect Bonus
```

校对状态：待人工校对

### 145 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerEffectBonus.Tooltip`

**中文原文**

```text
降低触发点击效果所需次数
```

**英文译文**

```text
Reduces the number of clicks required to trigger click effects
```

校对状态：待人工校对

### 146 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerEffect.Label`

**中文原文**

```text
额外点击效果
```

**英文译文**

```text
Additional Click Effect
```

校对状态：待人工校对

### 147 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerEffect.Tooltip`

**中文原文**

```text
穿戴魔影套提供魔影焰击点击效果
```

**英文译文**

```text
Wearing Demonshade armor grants the Demonshade Strike click effect
```

校对状态：待人工校对

### 148 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableVoidSet.Label`

**中文原文**

```text
启用虚空套装奖励
```

**英文译文**

```text
Enable Void Set Bonus
```

校对状态：待人工校对

### 149 · `Mods.DemonShadeCrossMod.Configs.DSConfig.EnableVoidSet.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 150 · `Mods.DemonShadeCrossMod.Configs.DSConfig.VoidMax2Bonus.Label`

**中文原文**

```text
额外最大虚空值加成
```

**英文译文**

```text
Additional Maximum Void
```

校对状态：待人工校对

### 151 · `Mods.DemonShadeCrossMod.Configs.DSConfig.VoidMax2Bonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 152 · `Mods.DemonShadeCrossMod.Configs.DSConfig.VoidRegenBonus.Label`

**中文原文**

```text
虚空值恢复速度加成
```

**英文译文**

```text
Void Regeneration Rate Bonus
```

校对状态：待人工校对

### 153 · `Mods.DemonShadeCrossMod.Configs.DSConfig.VoidRegenBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 154 · `Mods.DemonShadeCrossMod.Configs.DSConfig.VoidGainBonus.Label`

**中文原文**

```text
虚空值恢复量加成
```

**英文译文**

```text
Void Gain Bonus
```

校对状态：待人工校对

### 155 · `Mods.DemonShadeCrossMod.Configs.DSConfig.VoidGainBonus.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。

### 156 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerEffectDamage.Label`

**中文原文**

```text
点击效果伤害
```

**英文译文**

```text
Click Effect Damage
```

校对状态：待人工校对

### 157 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerEffectDamage.Tooltip`

**中文原文**

```text
决定魔影焰击的伤害，魔影爆破默认为前者6倍伤害
```

**英文译文**

```text
Determines the damage of Demonshade Strike; Demonshade Explosion deals 6 times that damage by default
```

校对状态：待人工校对

### 158 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerImmunityFrameType.Label`

**中文原文**

```text
点击效果无敌帧类型
```

**英文译文**

```text
Click Effect Invincibility Type
```

校对状态：待人工校对

校对备注：已按 Terraria Wiki 的 Global / Static / Local invincibility 术语修订；机制说明结合当前 tModLoader 文档。

### 159 · `Mods.DemonShadeCrossMod.Configs.DSConfig.ClickerImmunityFrameType.Tooltip`

**中文原文**

```text
（空）
```

**英文译文**

```text
（空）
```

校对状态：原文为空

校对备注：中文原文为空，英文对应留空。
