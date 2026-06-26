using System.Collections.Generic;
using TheCat.Roguelite;

namespace TheCat.Data.Catalogs
{
    public static class P0SettlementPresenter
    {
        public static IReadOnlyList<string> BuildRows(P0RunSettlementSummary summary)
        {
            List<string> rows = new List<string>
            {
                "结算：" + summary.ResultLabel,
                "路线：" + summary.CompletedNodes + "/" + summary.TotalLayers + " 节点",
                "战斗：" + summary.BattleSuccesses + "胜 / " + summary.BattleFailures + "负  用时 " + summary.DurationSeconds.ToString("0.0") + "s",
                "睡眠变化：" + summary.SleepDelta.ToString("0.0") + "  上限损失 " + summary.SleepMaxLost.ToString("0") + "  拉屎 " + summary.PoopIncidents + "  虚弱 " + summary.WeakIncidents,
                "敌人压力：事件 " + summary.EnemySleepPressureEvents + "  床 " + summary.BedPressureHits + "  首领投掷 " + summary.BossThrowPressureHits + "  睡眠 " + summary.EnemySleepDamageTaken.ToString("0.#") + "/" + summary.EnemySleepDamageIncoming.ToString("0.#") + "  吸收 " + summary.EnemySleepDamageAbsorbed.ToString("0.#"),
                "猫生命：受压 " + summary.CatPressureEvents + "  伤害 " + summary.CatDamageTaken.ToString("0.#") + "/" + summary.CatDamageIncoming.ToString("0.#") + "  吸收 " + summary.CatDamageAbsorbed.ToString("0.#") + "  治疗 " + summary.CatHealEvents + " " + summary.CatHealingApplied.ToString("0.#") + "  护盾 " + summary.CatShieldEvents + " " + summary.CatShieldApplied.ToString("0.#"),
                "床 / 猫砂盆 / 喂食器：" + summary.BedCareUses + " / " + summary.LitterBoxUses + " / " + summary.FeederUses,
                "行动：切换 " + summary.CatSwitchesSucceeded + "/" + summary.CatSwitchAttempts + "  猫咪虚弱未能切换 " + summary.CatSwitchesBlockedByWeak + "  自动锁定目标 " + summary.AutoTargetsAcquired + "/" + summary.AutoTargetAttempts + " 技能锁定目标 " + summary.SkillTargetsAcquired + "/" + summary.SkillTargetAttempts + "  技能 " + summary.SkillCastsSucceeded + "/" + summary.SkillCastAttempts + "  冷却中 " + summary.SkillCastsBlockedByCooldown + "  没有目标 " + summary.SkillCastsBlockedByTarget + "  交互 " + summary.InteractionSuccesses + "/" + summary.InteractionAttempts + "  距离太远 " + summary.InteractionBlockedByRange,
                "路线状态：梦屑 " + summary.DreamShards + " 小鱼干 " + summary.FishTreats + " 猫 " + summary.RosterCount + " 祝福 " + summary.BlessingCount + " 等级 " + summary.BlessingTotalLevel,
                "最终核心：" + P0CoreValuePresenter.BuildSettlementCoreSummary(summary),
                "最终猫咪生命：记录 " + summary.CatVitalCount + " 虚弱 " + summary.WeakCatCount + " 最低 " + (summary.LowestCatHpRatio * 100f).ToString("0") + "%"
            };

            return rows.AsReadOnly();
        }

        public static IReadOnlyList<string> BuildPlayerFocusRows(P0RunSettlementSummary summary)
        {
            List<string> rows = new List<string>
            {
                "结算：" + summary.ResultLabel,
                "路线：" + summary.CompletedNodes + "/" + summary.TotalLayers + " 节点",
                "战斗：" + summary.BattleSuccesses + "胜 / " + summary.BattleFailures + "负  用时 " + summary.DurationSeconds.ToString("0.0") + "s",
                "路线状态：梦屑 " + summary.DreamShards + " 小鱼干 " + summary.FishTreats + " 猫 " + summary.RosterCount + " 祝福 " + summary.BlessingCount + " 等级 " + summary.BlessingTotalLevel,
                "最终核心：" + P0CoreValuePresenter.BuildSettlementCoreSummary(summary),
                "最终猫咪生命：记录 " + summary.CatVitalCount + " 虚弱 " + summary.WeakCatCount + " 最低 " + (summary.LowestCatHpRatio * 100f).ToString("0") + "%"
            };

            return rows.AsReadOnly();
        }

        public static string BuildActionTelemetrySummary(P0RunSettlementSummary summary)
        {
            return "行动 切换 "
                + summary.CatSwitchesSucceeded
                + "/"
                + summary.CatSwitchAttempts
                + " 猫咪虚弱未能切换 "
                + summary.CatSwitchesBlockedByWeak
                + " 自动锁定目标 "
                + summary.AutoTargetsAcquired
                + "/"
                + summary.AutoTargetAttempts
                + " 技能锁定目标 "
                + summary.SkillTargetsAcquired
                + "/"
                + summary.SkillTargetAttempts
                + " 技能 "
                + summary.SkillCastsSucceeded
                + "/"
                + summary.SkillCastAttempts
                + " 冷却中 "
                + summary.SkillCastsBlockedByCooldown
                + " 没有目标 "
                + summary.SkillCastsBlockedByTarget
                + " 技能暂不可用 "
                + summary.SkillCastsBlockedByMissingDefinition
                + " 交互 "
                + summary.InteractionSuccesses
                + "/"
                + summary.InteractionAttempts
                + " 距离太远 "
                + summary.InteractionBlockedByRange;
        }

        public static string BuildCompactSummary(P0RunSettlementSummary summary)
        {
            return summary.ResultLabel
                + " 路线 "
                + summary.CompletedNodes
                + "/"
                + summary.TotalLayers
                + " 战斗 "
                + summary.BattleSuccesses
                + "胜/"
                + summary.BattleFailures
                + "负 小鱼干 "
                + summary.FishTreats
                + " 梦屑 "
                + summary.DreamShards
                + " 猫 "
                + summary.RosterCount
                + " 祝福 "
                + summary.BlessingCount
                + " 等级 "
                + summary.BlessingTotalLevel;
        }

        public static bool HasP0ClearedSettlementRows(P0RunSettlementSummary summary)
        {
            if (!summary.IsCleared
                || summary.CompletedNodes != summary.TotalLayers
                || summary.TotalLayers != 10
                || summary.BattleSuccesses < 5
                || summary.BattleFailures != 0)
            {
                return false;
            }

            IReadOnlyList<string> rows = BuildRows(summary);
            return Contains(rows, "结算：路线通关")
                && Contains(rows, "路线：10/10 节点")
                && Contains(rows, "战斗：5胜 / 0负")
                && Contains(rows, "行动：")
                && Contains(rows, "敌人压力：")
                && Contains(rows, "猫生命：")
                && Contains(rows, "路线状态：")
                && Contains(rows, "最终核心：")
                && Contains(rows, "最终猫咪生命：");
        }

        public static bool HasP0FailedSettlementRows(P0RunSettlementSummary summary)
        {
            if (!summary.IsFailed
                || summary.CompletedNodes <= 0
                || summary.CompletedNodes >= summary.TotalLayers
                || summary.TotalLayers != 10
                || summary.BattleFailures <= 0)
            {
                return false;
            }

            IReadOnlyList<string> rows = BuildRows(summary);
            return Contains(rows, "结算：路线失败")
                && Contains(rows, "路线：" + summary.CompletedNodes + "/10 节点")
                && Contains(rows, "战斗：" + summary.BattleSuccesses + "胜 / " + summary.BattleFailures + "负")
                && Contains(rows, "行动：")
                && Contains(rows, "敌人压力：")
                && Contains(rows, "猫生命：")
                && Contains(rows, "路线状态：")
                && Contains(rows, "最终核心：")
                && Contains(rows, "最终猫咪生命：");
        }

        public static bool HasP0ActionTelemetry(P0RunSettlementSummary summary)
        {
            return summary.CatSwitchAttempts > 0
                && summary.CatSwitchesSucceeded > 0
                && summary.AutoTargetAttempts > 0
                && summary.AutoTargetsAcquired > 0
                && summary.SkillTargetAttempts > 0
                && summary.SkillTargetsAcquired > 0
                && summary.SkillCastAttempts > 0
                && summary.SkillCastsSucceeded > 0
                && summary.InteractionAttempts > 0
                && summary.InteractionSuccesses > 0;
        }

        private static bool Contains(IReadOnlyList<string> rows, string expected)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Contains(expected))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
