using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public static class P0SkillPresenter
    {
        public static SkillPresentation Describe(SkillDefinition skill)
        {
            return skill == null ? CreateFallback(string.Empty) : Describe(skill.Id);
        }

        public static SkillPresentation Describe(string skillId)
        {
            if (TryDescribeCatUpgradeSkill(skillId, out SkillPresentation upgradePresentation))
            {
                return upgradePresentation;
            }

            switch (skillId)
            {
                case "saiban_oath_shield":
                    return new SkillPresentation(
                        skillId,
                        "银誓护盾",
                        "防守窗口",
                        "为塞班加护盾并强化床线",
                        "敌意，到我这里来。");
                case "saiban_sword_sweep":
                    return new SkillPresentation(
                        skillId,
                        "圆盾冲锋",
                        "床线重置",
                        "造成伤害并将敌人从床边击退",
                        "让开。别碰枕头。");
                case "saiban_sun_charge":
                    return new SkillPresentation(
                        skillId,
                        "王冠裁决",
                        "终结爆发",
                        "高额伤害、击退，并获得短暂护盾",
                        "王冠之下，梦魇退散。");
                case "nephthys_moon_sand_obelisk":
                    return new SkillPresentation(
                        skillId,
                        "月砂方尖碑",
                        "召唤控场",
                        "召唤砂碑并缓速入侵者",
                        "方尖碑，替我教训他们。");
                case "nephthys_quicksand_trap":
                    return new SkillPresentation(
                        skillId,
                        "流砂陷阱",
                        "区域控场",
                        "在王权砂地中缓速敌人",
                        "这片地面，就是我的边界。");
                case "nephthys_royal_mark":
                    return new SkillPresentation(
                        skillId,
                        "王权标记",
                        "集火目标",
                        "标记优先入侵者，使其受到更多伤害",
                        "跪下，小小的梦。");
                case "suzune_sleep_bell":
                    return new SkillPresentation(
                        skillId,
                        "安眠铃",
                        "睡眠安全",
                        "恢复主人睡眠、治疗猫，并延缓屎意危机",
                        "小铃守在这里，不许再痛了。");
                case "suzune_healing_bell":
                    return new SkillPresentation(
                        skillId,
                        "冰花祈愿",
                        "猫咪回复",
                        "恢复当前猫咪生命",
                        "绽开吧，让疼痛散去。");
                case "suzune_moon_torii":
                    return new SkillPresentation(
                        skillId,
                        "月鸟居结界",
                        "终结救场",
                        "恢复睡眠，并将敌人推离床边",
                        "鸟居升起。梦魇止步。");
                default:
                    return CreateFallback(skillId);
            }
        }

        private static bool TryDescribeCatUpgradeSkill(string skillId, out SkillPresentation presentation)
        {
            switch (skillId)
            {
                case "saiban_oath_shield_focus":
                    presentation = new SkillPresentation(skillId, "誓约护盾专注", "护盾救急", "提供更厚的单体护盾窗口", "守住床线。");
                    return true;
                case "saiban_sword_sweep_arc":
                    presentation = new SkillPresentation(skillId, "王剑横扫弧光", "击退清线", "造成伤害，并把敌人推得更远", "退到床外。");
                    return true;
                case "saiban_bedline_intercept":
                    presentation = new SkillPresentation(skillId, "床线拦截", "拦截入侵", "重击并击退最近的入侵者", "拦住缺口。");
                    return true;
                case "saiban_oath_counter":
                    presentation = new SkillPresentation(skillId, "誓约反制", "护盾反击", "获得护盾，并反击最近敌人", "先守住，再回应。");
                    return true;
                case "saiban_sun_crown":
                    presentation = new SkillPresentation(skillId, "日冕圣裁", "爆发击退", "高额伤害、强击退，并获得护盾", "日冕落下。");
                    return true;
                case "saiban_oath_domain":
                    presentation = new SkillPresentation(skillId, "誓约领域", "守护领域", "展开巨量护盾，并推开敌人", "谁也不能越过这里。");
                    return true;
                case "nephthys_moon_sand_focus":
                    presentation = new SkillPresentation(skillId, "月沙方尖碑", "召唤减速", "召唤控场压力，并缓速敌人", "方尖碑，回应我。");
                    return true;
                case "nephthys_quicksand_prison":
                    presentation = new SkillPresentation(skillId, "流沙牢笼", "强减速", "铺开更强的缓速领域", "沉进边界里。");
                    return true;
                case "nephthys_royal_command":
                    presentation = new SkillPresentation(skillId, "支配沙令", "标记集火", "标记并缓速优先目标", "跪下，小小的梦。");
                    return true;
                case "nephthys_sand_sentinel":
                    presentation = new SkillPresentation(skillId, "沙卫哨", "召唤压制", "召唤沙卫，造成伤害并缓速", "守住王线。");
                    return true;
                case "nephthys_sand_throne":
                    presentation = new SkillPresentation(skillId, "沙海王座", "大范围控场", "用缓速和标记压制大范围敌人", "王座降临。");
                    return true;
                case "nephthys_eclipse_obelisk":
                    presentation = new SkillPresentation(skillId, "月蚀方尖碑", "召唤压制", "召唤压制区域，造成伤害并缓速", "月影即是王权。");
                    return true;
                case "suzune_sleep_bell_focus":
                    presentation = new SkillPresentation(skillId, "安眠铃音", "安眠恢复", "恢复主人睡眠，并治疗当前猫咪", "在这里轻轻睡吧。");
                    return true;
                case "suzune_healing_bell_bloom":
                    presentation = new SkillPresentation(skillId, "治愈铃", "猫咪急救", "大量治疗当前猫咪", "疼痛，松开手。");
                    return true;
                case "suzune_moon_torii_guard":
                    presentation = new SkillPresentation(skillId, "月眠鸟居", "守床结界", "恢复睡眠，并推开威胁", "门会守住。");
                    return true;
                case "suzune_dream_chime":
                    presentation = new SkillPresentation(skillId, "梦响小铃", "护盾治疗", "治疗并护盾当前猫咪", "小小铃声，会留下来。");
                    return true;
                case "suzune_moon_sleep_domain":
                    presentation = new SkillPresentation(skillId, "月眠结界", "强力守梦", "大量恢复睡眠，并推开威胁", "梦境封上了。");
                    return true;
                case "suzune_kagura_cleanse":
                    presentation = new SkillPresentation(skillId, "神乐净梦", "净梦续航", "恢复睡眠、治疗猫咪，并提供护盾", "净化这场梦魇。");
                    return true;
                default:
                    presentation = default(SkillPresentation);
                    return false;
            }
        }

        public static string BuildButtonLabel(SkillDefinition skill, float cooldownSeconds)
        {
            SkillPresentation presentation = Describe(skill);
            if (skill == null)
            {
                return presentation.DisplayName;
            }

            string label = presentation.DisplayName;
            if (cooldownSeconds > 0f)
            {
                label += " (" + cooldownSeconds.ToString("0.0") + "s)";
            }

            label += "\n" + FormatSlot(skill.Slot) + " 消耗 " + skill.HungerCost.ToString("0.#") + " 饱肚度";
            if (!string.IsNullOrWhiteSpace(presentation.EffectHint))
            {
                label += " | " + presentation.EffectHint;
            }

            return label;
        }

        private static SkillPresentation CreateFallback(string skillId)
        {
            string fallbackId = string.IsNullOrWhiteSpace(skillId) ? "unknown_skill" : skillId;
            return new SkillPresentation(
                fallbackId,
                fallbackId,
                "未配置",
                "缺少 P0 技能展示",
                string.Empty);
        }

        private static string FormatSlot(SkillSlot slot)
        {
            switch (slot)
            {
                case SkillSlot.SmallSkill1:
                    return "小技能1";
                case SkillSlot.SmallSkill2:
                    return "小技能2";
                case SkillSlot.SmallSkill3:
                    return "小技能3";
                case SkillSlot.SmallSkill4:
                    return "小技能";
                case SkillSlot.Ultimate1:
                case SkillSlot.Ultimate2:
                    return "大招";
                default:
                    return "技能";
            }
        }
    }
}
