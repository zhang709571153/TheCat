using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public readonly struct P0BattlePlayerBriefLine
    {
        public P0BattlePlayerBriefLine(
            string category,
            string text,
            bool isUrgent = false,
            bool isAction = false,
            bool isResultAction = false)
        {
            Category = category ?? string.Empty;
            Text = text ?? string.Empty;
            IsUrgent = isUrgent;
            IsAction = isAction;
            IsResultAction = isResultAction;
        }

        public string Category { get; }

        public string Text { get; }

        public bool IsUrgent { get; }

        public bool IsAction { get; }

        public bool IsResultAction { get; }

        public string BuildSummary()
        {
            return Category + ": " + Text;
        }
    }

    public sealed class P0BattlePlayerBrief
    {
        private readonly List<P0BattlePlayerBriefLine> lines;

        public P0BattlePlayerBrief(string title, IEnumerable<P0BattlePlayerBriefLine> lines)
        {
            Title = title ?? string.Empty;
            this.lines = lines == null
                ? new List<P0BattlePlayerBriefLine>()
                : new List<P0BattlePlayerBriefLine>(lines);
        }

        public string Title { get; }

        public IReadOnlyList<P0BattlePlayerBriefLine> Lines => lines.AsReadOnly();

        public bool HasActionLine => HasLine(line => line.IsAction);

        public bool HasUrgentLine => HasLine(line => line.IsUrgent);

        public bool HasResultActionLine => HasLine(line => line.IsResultAction);

        public bool HasCategory(string category)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Category == category && !string.IsNullOrWhiteSpace(lines[i].Text))
                {
                    return true;
                }
            }

            return false;
        }

        public P0BattleHudSection ToHudSection()
        {
            List<string> sectionLines = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                sectionLines.Add(lines[i].BuildSummary());
            }

            return new P0BattleHudSection(Title, sectionLines);
        }

        public string BuildSummary()
        {
            return ToHudSection().BuildSummary();
        }

        private bool HasLine(Predicate<P0BattlePlayerBriefLine> predicate)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (predicate(lines[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class P0BattlePlayerBriefPresenter
    {
        public const int MaxDefaultLineCount = 3;

        public static P0BattlePlayerBrief Build(
            BattleSimulation battle,
            IReadOnlyList<CatBattleState> cats,
            int activeCatIndex,
            string navigationSummary,
            RunProgressionState run,
            RunNodeCompletionReport completionReport,
            P0BattleCommandDeck commandDeck)
        {
            List<P0BattlePlayerBriefLine> lines = new List<P0BattlePlayerBriefLine>();
            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, cats);
            lines.Add(new P0BattlePlayerBriefLine(
                "优先",
                prompt.BuildSummary(),
                prompt.Level == P0BattleHudPromptLevel.Warning || prompt.Level == P0BattleHudPromptLevel.Critical));

            if (battle == null)
            {
                lines.Add(new P0BattlePlayerBriefLine("行动", prompt.ActionText, isAction: true));
                return new P0BattlePlayerBrief("战斗提示", lines);
            }

            if (battle.Outcome != BattleOutcome.InProgress)
            {
                lines.Add(new P0BattlePlayerBriefLine(
                    "结算",
                    BuildResultLine(battle, completionReport),
                    isResultAction: true));
                lines.Add(new P0BattlePlayerBriefLine(
                    "行动",
                    "继续路线 / 回到猫窝 / 重新开始",
                    isAction: true,
                    isResultAction: true));
                return new P0BattlePlayerBrief("战斗提示", lines);
            }

            lines.Add(new P0BattlePlayerBriefLine(
                "行动",
                BuildActionLine(prompt, commandDeck),
                isAction: true));
            lines.Add(new P0BattlePlayerBriefLine(
                "威胁",
                BuildThreatLine(battle, navigationSummary, run),
                HasWarningEnemy(battle)));
            return new P0BattlePlayerBrief("战斗提示", lines);
        }

        public static bool HasP0BattlePlayerBrief(P0BattlePlayerBrief brief)
        {
            if (brief == null
                || string.IsNullOrWhiteSpace(brief.Title)
                || brief.Lines.Count < 2
                || brief.Lines.Count > MaxDefaultLineCount
                || !brief.HasCategory("优先")
                || !brief.HasCategory("行动")
                || !brief.HasActionLine)
            {
                return false;
            }

            string summary = brief.BuildSummary();
            return !ContainsInternalToken(summary);
        }

        public static bool HasP0BattleResultActions(P0BattlePlayerBrief brief)
        {
            return HasP0BattlePlayerBrief(brief)
                && brief.HasCategory("结算")
                && brief.HasResultActionLine
                && brief.BuildSummary().Contains("继续路线")
                && brief.BuildSummary().Contains("回到猫窝")
                && brief.BuildSummary().Contains("重新开始");
        }

        private static string BuildActionLine(P0BattleHudPrompt prompt, P0BattleCommandDeck commandDeck)
        {
            if (P0BattleCommandDeckPresenter.HasP0BattleCommandDeck(commandDeck))
            {
                return commandDeck.BuildCompactPlayerLine();
            }

            return string.IsNullOrWhiteSpace(prompt.ActionText)
                ? prompt.BuildSummary()
                : prompt.ActionText;
        }

        private static string BuildThreatLine(
            BattleSimulation battle,
            string navigationSummary,
            RunProgressionState run)
        {
            if (battle.ActiveEnemies.Count == 0)
            {
                return BuildRouteTail(navigationSummary, run);
            }

            int warningCount = 0;
            BattleEnemyState primary = battle.ActiveEnemies[0];
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                string warning = EnemyWarningFormatter.Format(enemy);
                if (!string.IsNullOrWhiteSpace(warning))
                {
                    warningCount++;
                    if (warningCount == 1)
                    {
                        primary = enemy;
                    }
                }
            }

            string line = battle.ActiveEnemies.Count
                + " 个敌人，"
                + primary.Definition.DisplayName
                + " 距床 "
                + primary.TimeToBedSeconds.ToString("0.0")
                + "s";
            if (warningCount > 0)
            {
                line += "，预警 " + warningCount;
            }

            if (battle.ActiveEnemies.Count > 1)
            {
                line += "，+" + (battle.ActiveEnemies.Count - 1);
            }

            string routeTail = BuildRouteTail(navigationSummary, run);
            return string.IsNullOrWhiteSpace(routeTail) ? line : line + " | " + routeTail;
        }

        private static string BuildRouteTail(string navigationSummary, RunProgressionState run)
        {
            string tail = string.Empty;
            if (run != null && run.Route != null)
            {
                tail = "路线 " + run.Route.CompletedCount + "/" + run.Route.Route.LayerCount;
            }

            if (!string.IsNullOrWhiteSpace(navigationSummary))
            {
                tail = string.IsNullOrWhiteSpace(tail) ? navigationSummary : tail + " | " + navigationSummary;
            }

            return tail;
        }

        private static string BuildResultLine(BattleSimulation battle, RunNodeCompletionReport completionReport)
        {
            string result = battle.Outcome == BattleOutcome.Victory ? "胜利" : "失败";
            result += "，用时 " + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s";
            if (completionReport != null)
            {
                result += "，" + completionReport.BuildSummary();
            }

            return result;
        }

        private static bool HasWarningEnemy(BattleSimulation battle)
        {
            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(EnemyWarningFormatter.Format(battle.ActiveEnemies[i])))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsInternalToken(string summary)
        {
            string[] tokens =
            {
                "Target",
                "hunger",
                "HP",
                " Lv",
                "Batch",
                "candidate",
                "缺失技能",
                "缺少技能定义"
            };
            for (int i = 0; i < tokens.Length; i++)
            {
                if (summary.Contains(tokens[i]))
                {
                    return true;
                }
            }

            return summary.Contains("\n");
        }
    }
}

namespace TheCat.Tools
{
    public enum P0BattleReadabilityCoverageSeverity
    {
        Failure
    }

    public readonly struct P0BattleReadabilityCoverageIssue
    {
        public P0BattleReadabilityCoverageIssue(P0BattleReadabilityCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0BattleReadabilityCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0BattleReadabilityCoverageReport
    {
        private readonly List<P0BattleReadabilityCoverageIssue> issues = new List<P0BattleReadabilityCoverageIssue>();
        private readonly List<string> coveredBriefs = new List<string>();

        public IReadOnlyList<P0BattleReadabilityCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredBriefs => coveredBriefs.AsReadOnly();

        public int FailureCount => issues.Count;

        public bool IsComplete => FailureCount == 0
            && coveredBriefs.Count >= P0BattleReadabilityCoverage.ExpectedCoveredBriefCount;

        public void AddFailure(string message)
        {
            issues.Add(new P0BattleReadabilityCoverageIssue(
                P0BattleReadabilityCoverageSeverity.Failure,
                message));
        }

        public void AddCoveredBrief(string brief)
        {
            if (!string.IsNullOrWhiteSpace(brief))
            {
                coveredBriefs.Add(brief);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 battle readability coverage complete for " + coveredBriefs.Count + " brief check(s)."
                : "P0 battle readability coverage has " + FailureCount + " failure(s) across " + coveredBriefs.Count + " brief check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };
            for (int i = 0; i < coveredBriefs.Count; i++)
            {
                lines.Add("- " + coveredBriefs[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }
    }

    public static class P0BattleReadabilityCoverage
    {
        public const int ExpectedCoveredBriefCount = 4;

        public static P0BattleReadabilityCoverageReport EvaluatePrototypeBattleBrief()
        {
            P0BattleReadabilityCoverageReport report = new P0BattleReadabilityCoverageReport();
            EvaluateInProgressBrief(report);
            EvaluateThreatOverflow(report);
            EvaluateBattleResultActions(report);
            EvaluateCandidateBoundary(report);
            return report;
        }

        private static void EvaluateInProgressBrief(P0BattleReadabilityCoverageReport report)
        {
            BattleSimulation battle = CreateBattle();
            List<CatBattleState> cats = CreateCats();
            BattleEnemyState enemy = battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.2f);
            battle.DebugApplyStatusToEnemy(enemy, StatusTagIds.Mark);
            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                battle,
                cats,
                0,
                "床 0.8m",
                CreateRun(),
                null,
                CreateCommandDeck());

            Require(
                report,
                P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief)
                && brief.Lines.Count <= P0BattlePlayerBriefPresenter.MaxDefaultLineCount
                && brief.HasCategory("威胁"),
                "Default battle brief keeps priority, action, and compact threat lines in first view.",
                "Default battle brief is missing priority/action/threat or exceeds line cap.");
        }

        private static void EvaluateThreatOverflow(P0BattleReadabilityCoverageReport report)
        {
            BattleSimulation battle = CreateBattle();
            List<CatBattleState> cats = CreateCats();
            battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.2f);
            battle.DebugSpawnEnemy(P0PrototypeCatalog.DreamRailToyTrainId, 1.1f);
            battle.DebugSpawnEnemy(P0PrototypeCatalog.ColdLightShadowId, 1.4f);
            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                battle,
                cats,
                0,
                string.Empty,
                CreateRun(),
                null,
                CreateCommandDeck());

            Require(
                report,
                P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief)
                && brief.BuildSummary().Contains("+")
                && brief.Lines.Count == P0BattlePlayerBriefPresenter.MaxDefaultLineCount,
                "Threat overflow is summarized in one capped player-facing line.",
                "Threat overflow was not compressed into the capped brief.");
        }

        private static void EvaluateBattleResultActions(P0BattleReadabilityCoverageReport report)
        {
            BattleSimulation battle = CreateBattle();
            battle.DebugDamageOwnerSleep(999f);
            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                battle,
                CreateCats(),
                0,
                string.Empty,
                CreateRun(),
                null,
                CreateCommandDeck());

            Require(
                report,
                P0BattlePlayerBriefPresenter.HasP0BattleResultActions(brief),
                "Battle result brief exposes continue, cat-room return, and restart actions above normal controls.",
                "Battle result brief did not expose all result actions.");
        }

        private static void EvaluateCandidateBoundary(P0BattleReadabilityCoverageReport report)
        {
            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                CreateBattle(),
                CreateCats(),
                0,
                "Batch 85 and Batch 89 are candidate references only.",
                CreateRun(),
                null,
                CreateCommandDeck());

            Require(
                report,
                !P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief),
                "Candidate asset batches remain outside the player-facing battle brief boundary.",
                "Battle brief allowed candidate asset batch text to appear player-facing.");
        }

        private static BattleSimulation CreateBattle()
        {
            return new BattleSimulation(
                new BattleSimulationConfig(
                    P0PrototypeCatalog.CreateLayerOneWave(),
                    P0PrototypeCatalog.CreateCoreEnemies(),
                    P0Tuning.Default,
                    statusTags: P0PrototypeCatalog.CreateStatusTags()),
                new RunMetrics());
        }

        private static List<CatBattleState> CreateCats()
        {
            List<CatBattleState> cats = new List<CatBattleState>();
            IReadOnlyList<CatDefinition> definitions = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < definitions.Count; i++)
            {
                cats.Add(new CatBattleState(definitions[i]));
            }

            return cats;
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
        }

        private static P0BattleCommandDeck CreateCommandDeck()
        {
            return new P0BattleCommandDeck(
                "当前行动",
                new[] { "上阵：塞班 生命稳", "主技能：王冠裁决 可用", "互动：照看床 可用" },
                3,
                3,
                3,
                3);
        }

        private static void Require(
            P0BattleReadabilityCoverageReport report,
            bool condition,
            string coveredBrief,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredBrief(coveredBrief);
                return;
            }

            report.AddFailure(failureMessage);
        }
    }
}
