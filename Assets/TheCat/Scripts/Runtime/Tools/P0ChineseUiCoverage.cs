using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;
using UnityEngine;

namespace TheCat.Tools
{
    public enum P0ChineseUiCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0ChineseUiCoverageIssue
    {
        public P0ChineseUiCoverageIssue(P0ChineseUiCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0ChineseUiCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0ChineseUiCoverageReport
    {
        private readonly List<P0ChineseUiCoverageIssue> issues = new List<P0ChineseUiCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0ChineseUiCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0ChineseUiCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0ChineseUiCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0ChineseUiCoverageSeverity severity, string message)
        {
            issues.Add(new P0ChineseUiCoverageIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 Chinese UI coverage complete for " + coveredChecks.Count + " check(s)."
                : "P0 Chinese UI coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " covered check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0ChineseUiCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0ChineseUiCoverage
    {
        public const int ExpectedCoveredCheckCount = 8;

        public static P0ChineseUiCoverageReport EvaluatePrototypeUi()
        {
            P0ChineseUiCoverageReport report = new P0ChineseUiCoverageReport();

            EvaluateMainMenuSurface(report);
            EvaluateRouteMapSurface(report);
            EvaluateBattlePrompts(report);
            EvaluateBattleHudSections(report);
            EvaluateSkillAndEnemyHud(report);
            EvaluateCoreValuesAndRuntimeSettings(report);
            EvaluatePendingRunText(report);
            EvaluateResponsiveLayoutHelpers(report);

            return report;
        }

        private static void EvaluateMainMenuSurface(P0ChineseUiCoverageReport report)
        {
            P0MainMenuSurface surface = P0MainMenuPresenter.BuildSurface(
                P0PrototypeCatalog.CreateStarterCats(),
                P0RunSession.CreateDefaultStarterCatIds(),
                P0RouteCatalog.CreateTenLayerRoute(),
                "就绪");

            string text = surface.Title + " " + surface.Subtitle + " " + surface.Message;
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                text += " " + surface.StarterCards[i].BuildSelectionLabel();
                text += " " + surface.StarterCards[i].BuildSkillPreview();
                text += " " + surface.StarterCards[i].BuildSummary();
            }

            for (int i = 0; i < surface.RouteRows.Count; i++)
            {
                text += " " + surface.RouteRows[i].PreviewLabel;
                text += " " + surface.RouteRows[i].BuildSummary();
            }

            Require(
                report,
                text.Contains("猫眠所")
                && text.Contains("塞班")
                && text.Contains("奈芙蒂斯")
                && text.Contains("铃音")
                && text.Contains("防守")
                && text.Contains("控场")
                && text.Contains("治疗")
                && text.Contains("银誓护盾")
                && text.Contains("安眠铃")
                && text.Contains("来电暴君首领")
                && text.Contains("第 10 层")
                && !ContainsLegacyUiToken(text),
                "Main menu surface keeps starter cards, route preview, role labels, and skill previews in Chinese.",
                "Main menu surface still has missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluateRouteMapSurface(P0ChineseUiCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "就绪");
            string text = surface.Title + " " + surface.Message + " " + surface.StatusLabel + " " + surface.ProgressLabel;
            text += " " + surface.CurrentNode.BuildSummary();
            text += " " + P0RouteMapPresenter.BuildCompactSummary(surface);
            for (int i = 0; i < surface.SummaryRows.Count; i++)
            {
                text += " " + surface.SummaryRows[i];
            }

            Require(
                report,
                surface.ProgressLabel == "进度：0/10"
                && surface.StatusLabel == "进行中"
                && surface.CurrentNode.NodeTypeToken == "守床"
                && text.Contains("资源：梦屑")
                && text.Contains("首领层 1")
                && text.Contains("祝福：0")
                && !ContainsLegacyUiToken(text),
                "Route map surface exposes Chinese progress, status, resource, node-type, and boss-row labels.",
                "Route map surface still has missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluateBattlePrompts(P0ChineseUiCoverageReport report)
        {
            P0BattleHudPrompt start = P0BattleHudPromptPresenter.Build(null, null);

            BattleSimulation sleepDanger = CreateBattle();
            sleepDanger.DebugDamageOwnerSleep(95f);
            P0BattleHudPrompt sleep = P0BattleHudPromptPresenter.Build(sleepDanger, CreateCats());

            BattleSimulation bossBattle = CreateBattle();
            BattleEnemyState boss = bossBattle.DebugSpawnEnemy(P0PrototypeCatalog.CallTyrantId, 8f);
            boss.DebugSetBossTimers(1f, 1.5f);
            P0BattleHudPrompt bossPrompt = P0BattleHudPromptPresenter.Build(bossBattle, CreateCats());

            BattleSimulation defeatBattle = CreateBattle();
            defeatBattle.DebugDamageOwnerSleep(999f);
            P0BattleHudPrompt defeat = P0BattleHudPromptPresenter.Build(defeatBattle, CreateCats());

            string text = start.BuildSummary()
                + " " + sleep.BuildSummary()
                + " " + bossPrompt.BuildSummary()
                + " " + defeat.BuildSummary();

            Require(
                report,
                text.Contains("就绪")
                && text.Contains("开始战斗")
                && text.Contains("睡眠")
                && text.Contains("守住床")
                && text.Contains("首领行动")
                && text.Contains("首领召唤")
                && text.Contains("失败")
                && !ContainsLegacyUiToken(text),
                "Battle prompt surface uses Chinese readiness, danger, boss-pattern, and result prompts.",
                "Battle prompt surface still has missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluateBattleHudSections(P0ChineseUiCoverageReport report)
        {
            BattleSimulation battle = CreateBattle();
            battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.2f);
            IReadOnlyList<P0BattleHudSection> sections = P0BattleHudSummaryPresenter.BuildSections(
                battle,
                CreateCats(),
                0,
                "位置 0.0, 床 0.9, 猫砂盆 3.4, 喂食器 3.4",
                CreateRun(),
                null);

            string text = P0BattleHudSummaryPresenter.BuildCompactSummary(sections);
            for (int i = 0; i < sections.Count; i++)
            {
                text += " " + sections[i].BuildSummary();
            }

            Require(
                report,
                P0BattleHudSummaryPresenter.HasP0BattleHudSections(sections)
                && text.Contains("目标")
                && text.Contains("核心数值")
                && text.Contains("主人睡眠度")
                && text.Contains("威胁")
                && text.Contains("队伍")
                && text.Contains("路线：0/10")
                && text.Contains("节点指标")
                && text.Contains("敌人压力")
                && !ContainsLegacyUiToken(text),
                "Battle HUD summary sections expose Chinese objective, core values, threat, team, route, and metric labels.",
                "Battle HUD summary sections still have missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluateSkillAndEnemyHud(P0ChineseUiCoverageReport report)
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            EnemyDefinition blackMud = GetEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            P0SkillTargetResult target = new P0SkillTargetResult(
                true,
                new BattleEnemyState(1, blackMud, 3f),
                1.5f,
                2.35f);
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 80f, target, true);
            P0SkillHudCard skillCard = P0SkillHudPresenter.BuildCard(skill, affordance, 0f, 80f, target);

            BattleEnemyState boss = new BattleEnemyState(2, GetEnemy(P0PrototypeCatalog.CallTyrantId), 8f);
            boss.DebugSetBossTimers(
                EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                EnemyWarningFormatter.BossThrowWarningThresholdSeconds);
            P0EnemyHudCard bossCard = P0EnemyHudPresenter.BuildCard(boss);
            CatBattleState cat = CreateCats()[0];
            cat.ApplyStatus(GetStatus(StatusTagIds.Shield), 24f);
            P0CatHudCard catCard = P0CatHudPresenter.BuildCard(cat, true, _ => 0f);
            P0StatusHudEntry catStatus = P0StatusHudPresenter.BuildCatEntry(cat);
            string text = skillCard.BuildButtonLabel()
                + " " + skillCard.BuildSummary()
                + " " + bossCard.BuildButtonLabel()
                + " " + bossCard.BuildSummary()
                + " " + P0EnemyHudPresenter.BuildCompactSummary(new[] { bossCard })
                + " " + catCard.BuildButtonLabel()
                + " " + catCard.BuildSummary()
                + " " + catStatus.ResponseSummary;

            Require(
                report,
                text.Contains("圆盾冲锋")
                && text.Contains("目标 黑泥梦魇")
                && text.Contains("首领机制")
                && text.Contains("首领召唤")
                && text.Contains("首领投掷")
                && text.Contains("首领 1")
                && text.Contains("生命")
                && text.Contains("猫护盾")
                && !ContainsLegacyUiToken(text),
                "Skill, enemy, cat, and status HUD surfaces expose Chinese target, boss warning, life, and shield labels.",
                "Skill, enemy, cat, or status HUD surface still has missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluateCoreValuesAndRuntimeSettings(P0ChineseUiCoverageReport report)
        {
            string hunger = P0CoreValuePresenter.DescribeTeamHunger(new TeamHungerGauge(8f)).BuildSummary();
            string runtime = P0RuntimeSettingsPresenter.Build(new P0RuntimeSettings()).BuildSummary();
            P0RuntimeSettings fastSettings = new P0RuntimeSettings();
            fastSettings.SetBattleSpeed(1.5f);
            string fast = P0RuntimeSettingsPresenter.Build(fastSettings).BuildSummary();
            string text = hunger + " " + runtime + " " + fast;

            Require(
                report,
                text.Contains("饱肚度")
                && text.Contains("伤害 0.65 倍")
                && text.Contains("运行设置：实时")
                && text.Contains("速度 1 倍")
                && text.Contains("速度 1.5 倍")
                && text.Contains("暂停键 P/Esc")
                && !ContainsLegacyUiToken(text),
                "Core value and runtime settings summaries expose Chinese multiplier and speed labels while preserving shortcuts.",
                "Core value or runtime settings summary still has missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluatePendingRunText(P0ChineseUiCoverageReport report)
        {
            RunPendingBattleModifiers pending = new RunPendingBattleModifiers();
            string empty = pending.BuildSummary();
            pending.Add(1.2f, 1.4f);
            string active = pending.BuildSummary();
            RunBlessingInventory blessings = new RunBlessingInventory();
            blessings.Add(P0BlessingCatalog.CreateAuthorityBlessings()[0]);
            string blessingSummary = blessings.BuildSummary();
            string text = empty + " " + active + " " + blessingSummary;

            Require(
                report,
                empty == "无"
                && blessingSummary.Contains("等级1")
                && active.Contains("下一战技能 1.2 倍")
                && active.Contains("屎意 1.4 倍")
                && active.Contains("来源 1")
                && !ContainsLegacyUiToken(text),
                "Pending modifier and empty blessing summaries use Chinese labels for route-map HUD rows.",
                "Pending modifier or blessing summaries still have missing Chinese labels or legacy English UI text.");
        }

        private static void EvaluateResponsiveLayoutHelpers(P0ChineseUiCoverageReport report)
        {
            Rect panel = new Rect(0f, 0f, 340f, 620f);
            float inner = P0ImGuiLayout.InnerWidth(panel, 14f);
            float scroll = P0ImGuiLayout.ScrollContentWidth(panel, 14f);
            bool narrowStacks = P0ImGuiLayout.ShouldStackControls(300f, 390f, 1f);
            bool wideStaysInline = !P0ImGuiLayout.ShouldStackControls(520f, 390f, 1f);

            Require(
                report,
                scroll > 0f
                && scroll < inner
                && narrowStacks
                && wideStaysInline,
                "Responsive IMGUI helpers reserve scrollbar width and stack controls only below the readable-width threshold.",
                "Responsive IMGUI helpers did not expose the expected narrow/wide layout behavior.");
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
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

        private static IReadOnlyList<CatBattleState> CreateCats()
        {
            List<CatBattleState> cats = new List<CatBattleState>();
            IReadOnlyList<CatDefinition> definitions = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < definitions.Count; i++)
            {
                cats.Add(new CatBattleState(definitions[i]));
            }

            return cats.AsReadOnly();
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Id == skillId)
                {
                    return skills[i];
                }
            }

            return null;
        }

        private static EnemyDefinition GetEnemy(string enemyId)
        {
            IReadOnlyList<EnemyDefinition> enemies = P0PrototypeCatalog.CreateCoreEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Id == enemyId)
                {
                    return enemies[i];
                }
            }

            return null;
        }

        private static StatusTagDefinition GetStatus(string statusId)
        {
            IReadOnlyList<StatusTagDefinition> statuses = P0PrototypeCatalog.CreateStatusTags();
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i].Id == statusId)
                {
                    return statuses[i];
                }
            }

            return default(StatusTagDefinition);
        }

        private static bool ContainsLegacyUiToken(string text)
        {
            return Contains(text, "Boss 召唤")
                || Contains(text, "Boss 投掷")
                || Contains(text, "Boss机制")
                || Contains(text, "Boss Pattern")
                || Contains(text, "Bed Contact")
                || Contains(text, "Hold The Bed")
                || Contains(text, "Poop Countdown")
                || Contains(text, "Hunger Empty")
                || Contains(text, "Cat Weak")
                || Contains(text, "Continue route")
                || Contains(text, "Start battle")
                || Contains(text, "Progress:")
                || Contains(text, "Wallet")
                || Contains(text, "No target")
                || Contains(text, "Round Shield Rush")
                || Contains(text, "Call Tyrant Boss")
                || Contains(text, "Layer 10")
                || Contains(text, "Soft Rain Window")
                || Contains(text, "Oath Bedline")
                || Contains(text, "伤害 x")
                || Contains(text, "next battle skill")
                || Contains(text, "HP")
                || Contains(text, " Lv")
                || Contains(text, " | Target")
                || Contains(text, " | hunger")
                || Contains(text, "Positive SkillCast")
                || Contains(text, "Warning SkillBlocked")
                || Contains(text, "RuntimeSettings");
        }

        private static bool Contains(string text, string token)
        {
            return !string.IsNullOrEmpty(text)
                && text.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0ChineseUiCoverageReport report,
            bool condition,
            string coveredMessage,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredMessage);
            }
            else
            {
                report.AddIssue(P0ChineseUiCoverageSeverity.Failure, failureMessage);
            }
        }
    }
}
