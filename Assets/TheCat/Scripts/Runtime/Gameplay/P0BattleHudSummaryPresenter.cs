using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public readonly struct P0BattleHudSection
    {
        public P0BattleHudSection(string title, IEnumerable<string> lines)
        {
            Title = title ?? string.Empty;
            Lines = lines == null
                ? new List<string>().AsReadOnly()
                : new List<string>(lines).AsReadOnly();
        }

        public string Title { get; }

        public IReadOnlyList<string> Lines { get; }

        public string BuildSummary()
        {
            return Title + "：" + string.Join(" | ", Lines);
        }
    }

    public static class P0BattleHudSummaryPresenter
    {
        public static IReadOnlyList<P0BattleHudSection> BuildSections(
            BattleSimulation battle,
            IReadOnlyList<CatBattleState> cats,
            int activeCatIndex,
            string navigationSummary,
            RunProgressionState run,
            RunNodeCompletionReport completionReport)
        {
            List<P0BattleHudSection> sections = new List<P0BattleHudSection>();
            sections.Add(BuildObjectiveSection(battle, cats, navigationSummary));
            if (battle == null)
            {
                return sections.AsReadOnly();
            }

            sections.Add(BuildCoreSection(battle));
            sections.Add(BuildThreatSection(battle, cats));
            sections.Add(BuildTeamSection(cats, activeCatIndex));
            sections.Add(BuildRunSection(run));
            sections.Add(BuildMetricsSection(battle));
            if (battle.Outcome != BattleOutcome.InProgress)
            {
                sections.Add(BuildResultSection(battle, completionReport));
            }

            return sections.AsReadOnly();
        }

        public static bool HasP0BattleHudSections(IReadOnlyList<P0BattleHudSection> sections)
        {
            return HasSection(sections, "目标")
                && HasSection(sections, "核心数值")
                && HasSection(sections, "威胁")
                && HasSection(sections, "队伍")
                && HasSection(sections, "路线")
                && HasSection(sections, "节点指标");
        }

        public static string BuildCompactSummary(IReadOnlyList<P0BattleHudSection> sections)
        {
            if (sections == null || sections.Count == 0)
            {
                return "战斗 HUD：空";
            }

            List<string> summaries = new List<string>();
            for (int i = 0; i < sections.Count; i++)
            {
                summaries.Add(sections[i].Title + " " + sections[i].Lines.Count);
            }

            return "战斗 HUD 分区：" + string.Join(", ", summaries);
        }

        private static P0BattleHudSection BuildObjectiveSection(
            BattleSimulation battle,
            IReadOnlyList<CatBattleState> cats,
            string navigationSummary)
        {
            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, cats);
            List<string> lines = new List<string>
            {
                prompt.BuildSummary()
            };

            if (battle != null)
            {
                lines.Add("结果：" + FormatOutcome(battle.Outcome));
                lines.Add("时间：" + battle.BattleTimeSeconds.ToString("0.0") + "s  敌人：" + battle.ActiveEnemies.Count);
            }

            if (!string.IsNullOrWhiteSpace(navigationSummary))
            {
                lines.Add("位置：" + navigationSummary);
            }

            return new P0BattleHudSection("目标", lines);
        }

        private static P0BattleHudSection BuildCoreSection(BattleSimulation battle)
        {
            return new P0BattleHudSection(
                "核心数值",
                new[]
                {
                    P0CoreValuePresenter.DescribeOwnerSleep(battle.OwnerSleep).BuildSummary(),
                    P0CoreValuePresenter.DescribeTeamPoop(battle.TeamPoop).BuildSummary(),
                    P0CoreValuePresenter.DescribeTeamHunger(battle.TeamHunger).BuildSummary()
                });
        }

        private static P0BattleHudSection BuildThreatSection(
            BattleSimulation battle,
            IReadOnlyList<CatBattleState> cats)
        {
            List<string> lines = new List<string>();
            AddWarningLines(battle, lines);
            AddStatusLines(battle, cats, lines);
            if (battle.BossSummonsTriggered > 0 || battle.BossThrowsTriggered > 0)
            {
                lines.Add("首领压力：召唤 "
                    + battle.BossSummonsTriggered
                    + " 投掷 "
                    + battle.BossThrowsTriggered);
            }

            if (lines.Count == 0)
            {
                lines.Add("暂无紧急预警。");
            }

            return new P0BattleHudSection("威胁", lines);
        }

        private static P0BattleHudSection BuildTeamSection(
            IReadOnlyList<CatBattleState> cats,
            int activeCatIndex)
        {
            List<string> lines = new List<string>();
            if (cats == null || cats.Count == 0)
            {
                lines.Add("猫队：未初始化");
                return new P0BattleHudSection("队伍", lines);
            }

            for (int i = 0; i < cats.Count; i++)
            {
                CatBattleState cat = cats[i];
                string line = (i == activeCatIndex ? "当前：" : "候补：")
                    + cat.Definition.DisplayName
                    + " 生命 "
                    + cat.Vital.CurrentHp.ToString("0")
                    + "/"
                    + cat.Vital.MaxHp.ToString("0");

                string statusText = StatusDisplayFormatter.FormatCollection(cat.Statuses);
                if (!string.IsNullOrWhiteSpace(statusText))
                {
                    line += " | " + statusText;
                }

                lines.Add(line);
            }

            return new P0BattleHudSection("队伍", lines);
        }

        private static P0BattleHudSection BuildRunSection(RunProgressionState run)
        {
            List<string> lines = new List<string>();
            if (run == null)
            {
                lines.Add("路线：未初始化");
                return new P0BattleHudSection("路线", lines);
            }

            lines.Add("路线：" + BuildRouteSummary(run.Route));
            lines.Add("资源：梦屑 " + run.Wallet.DreamShards + " 小鱼干 " + run.Wallet.FishTreats);
            lines.Add("祝福：" + run.Blessings.Count + " 总等级 " + run.Blessings.TotalLevel + " | " + run.Blessings.BuildSummary());
            lines.Add("持续核心：" + P0CoreValuePresenter.BuildRunCoreSummary(run.CoreValues));
            lines.Add("待触发事件：" + run.PendingBattleModifiers.BuildSummary());
            return new P0BattleHudSection("路线", lines);
        }

        private static P0BattleHudSection BuildMetricsSection(BattleSimulation battle)
        {
            return new P0BattleHudSection(
                "节点指标",
                new[]
                {
                    "照看：床 "
                        + battle.NodeMetrics.BedCareUses
                        + " 猫砂盆 "
                        + battle.NodeMetrics.LitterBoxUses
                        + " 喂食器 "
                        + battle.NodeMetrics.FeederUses,
                    "压力：拉屎 "
                        + battle.NodeMetrics.PoopIncidents
                        + " 睡眠上限损失 "
                        + battle.NodeMetrics.SleepMaxLost.ToString("0")
                        + " 虚弱 "
                        + battle.NodeMetrics.WeakIncidents,
                    "猫生命：受压 "
                        + battle.NodeMetrics.CatPressureEvents
                        + " 伤害 "
                        + battle.NodeMetrics.CatDamageTaken.ToString("0.#")
                        + "/"
                        + battle.NodeMetrics.CatDamageIncoming.ToString("0.#")
                        + " 吸收 "
                        + battle.NodeMetrics.CatDamageAbsorbed.ToString("0.#")
                        + " 治疗 "
                        + battle.NodeMetrics.CatHealEvents
                        + " "
                        + battle.NodeMetrics.CatHealingApplied.ToString("0.#")
                        + " 护盾 "
                        + battle.NodeMetrics.CatShieldEvents
                        + " "
                        + battle.NodeMetrics.CatShieldApplied.ToString("0.#"),
                    "敌人压力：床 "
                        + battle.NodeMetrics.BedPressureHits
                        + " 首领 "
                        + battle.NodeMetrics.BossThrowPressureHits
                        + " 睡眠 "
                        + battle.NodeMetrics.EnemySleepDamageTaken.ToString("0.#")
                        + "/"
                        + battle.NodeMetrics.EnemySleepDamageIncoming.ToString("0.#")
                        + " 吸收 "
                        + battle.NodeMetrics.EnemySleepDamageAbsorbed.ToString("0.#"),
                    "切换："
                        + battle.NodeMetrics.CatSwitchesSucceeded
                        + "/"
                        + battle.NodeMetrics.CatSwitchAttempts
                        + " 成功，猫咪虚弱未能切换 "
                        + battle.NodeMetrics.CatSwitchesBlockedByWeak,
                    "锁定目标：自动 "
                        + battle.NodeMetrics.AutoTargetsAcquired
                        + "/"
                        + battle.NodeMetrics.AutoTargetAttempts
                        + "，技能 "
                        + battle.NodeMetrics.SkillTargetsAcquired
                        + "/"
                        + battle.NodeMetrics.SkillTargetAttempts,
                    "技能："
                        + battle.NodeMetrics.SkillCastsSucceeded
                        + "/"
                        + battle.NodeMetrics.SkillCastAttempts
                        + " 释放，冷却中 "
                        + battle.NodeMetrics.SkillCastsBlockedByCooldown
                        + "，没有目标 "
                        + battle.NodeMetrics.SkillCastsBlockedByTarget,
                    "交互："
                        + battle.NodeMetrics.InteractionSuccesses
                        + "/"
                        + battle.NodeMetrics.InteractionAttempts
                        + " 使用，距离太远 "
                        + battle.NodeMetrics.InteractionBlockedByRange
                });
        }

        private static P0BattleHudSection BuildResultSection(
            BattleSimulation battle,
            RunNodeCompletionReport completionReport)
        {
            List<string> lines = new List<string>
            {
                "用时：" + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s",
                "睡眠变化：" + battle.NodeMetrics.SleepDelta.ToString("0.0"),
                "拉屎次数：" + battle.NodeMetrics.PoopIncidents
            };

            if (completionReport != null)
            {
                lines.Insert(0, "路线结果：" + completionReport.BuildSummary());
            }

            return new P0BattleHudSection("结算", lines);
        }

        private static void AddWarningLines(BattleSimulation battle, List<string> lines)
        {
            List<string> warnings = null;
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                string warningText = EnemyWarningFormatter.Format(enemy);
                if (string.IsNullOrWhiteSpace(warningText))
                {
                    continue;
                }

                if (warnings == null)
                {
                    warnings = new List<string>();
                }

                warnings.Add(enemy.Definition.DisplayName + "：" + warningText);
            }

            if (warnings != null)
            {
                lines.Add("预警：" + string.Join("；", warnings));
            }
        }

        private static void AddStatusLines(
            BattleSimulation battle,
            IReadOnlyList<CatBattleState> cats,
            List<string> lines)
        {
            string bedStatuses = StatusDisplayFormatter.FormatCollection(battle.BedStatuses);
            if (!string.IsNullOrWhiteSpace(bedStatuses))
            {
                lines.Add("床标签：" + bedStatuses);
            }

            string enemyStatuses = BuildEnemyStatusSummary(battle);
            if (!string.IsNullOrWhiteSpace(enemyStatuses))
            {
                lines.Add("敌人标签：" + enemyStatuses);
            }

            string catStatuses = BuildCatStatusSummary(cats);
            if (!string.IsNullOrWhiteSpace(catStatuses))
            {
                lines.Add("猫咪标签：" + catStatuses);
            }
        }

        private static string BuildEnemyStatusSummary(BattleSimulation battle)
        {
            List<string> summaries = null;
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                string statusText = StatusDisplayFormatter.FormatCollection(enemy.Statuses);
                if (string.IsNullOrWhiteSpace(statusText))
                {
                    continue;
                }

                if (summaries == null)
                {
                    summaries = new List<string>();
                }

                summaries.Add(enemy.Definition.DisplayName + "：" + statusText);
            }

            return summaries == null ? string.Empty : string.Join("；", summaries);
        }

        private static string BuildCatStatusSummary(IReadOnlyList<CatBattleState> cats)
        {
            if (cats == null)
            {
                return string.Empty;
            }

            List<string> summaries = null;
            for (int i = 0; i < cats.Count; i++)
            {
                string statusText = StatusDisplayFormatter.FormatCollection(cats[i].Statuses);
                if (string.IsNullOrWhiteSpace(statusText))
                {
                    continue;
                }

                if (summaries == null)
                {
                    summaries = new List<string>();
                }

                summaries.Add(cats[i].Definition.DisplayName + "：" + statusText);
            }

            return summaries == null ? string.Empty : string.Join("；", summaries);
        }

        private static string BuildRouteSummary(RunRouteState route)
        {
            if (route == null)
            {
                return "未初始化";
            }

            string routeText = route.CompletedCount + "/" + route.Route.LayerCount;
            if (route.CurrentNode != null)
            {
                routeText += " 下一层 " + route.CurrentNode.Layer + " " + P0RouteNodePresenter.Describe(route.CurrentNode).Title;
            }
            else if (route.IsCleared)
            {
                routeText += " 已通关";
            }
            else if (route.IsFailed)
            {
                routeText += " 已失败";
            }

            return routeText;
        }

        private static string FormatOutcome(BattleOutcome outcome)
        {
            switch (outcome)
            {
                case BattleOutcome.Victory:
                    return "胜利";
                case BattleOutcome.Defeat:
                    return "失败";
                case BattleOutcome.InProgress:
                default:
                    return "进行中";
            }
        }

        private static bool HasSection(IReadOnlyList<P0BattleHudSection> sections, string title)
        {
            if (sections == null)
            {
                return false;
            }

            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i].Title == title && sections[i].Lines.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
