using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0BattlePlayerBriefPresenterTests
    {
        [Test]
        public void Build_InProgressBattleStartsWithPriorityActionAndThreat()
        {
            BattleSimulation battle = CreateBattle();
            List<CatBattleState> cats = CreateCats();
            battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.2f);

            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                battle,
                cats,
                0,
                "床 0.8m",
                CreateRun(),
                null,
                CreateCommandDeck());

            Assert.IsTrue(P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief), brief.BuildSummary());
            Assert.LessOrEqual(brief.Lines.Count, P0BattlePlayerBriefPresenter.MaxDefaultLineCount);
            Assert.IsTrue(brief.HasCategory("优先"));
            Assert.IsTrue(brief.HasCategory("行动"));
            Assert.IsTrue(brief.HasCategory("威胁"));
            StringAssert.Contains("当前行动", brief.BuildSummary());
            AssertPlayerBriefHasNoInternalTokens(brief);
        }

        [Test]
        public void Build_MultipleThreatsAreCappedIntoOverflowLine()
        {
            BattleSimulation battle = CreateBattle();
            battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.2f);
            battle.DebugSpawnEnemy(P0PrototypeCatalog.DreamRailToyTrainId, 1.1f);
            battle.DebugSpawnEnemy(P0PrototypeCatalog.ColdLightShadowId, 1.4f);

            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                battle,
                CreateCats(),
                0,
                string.Empty,
                CreateRun(),
                null,
                CreateCommandDeck());

            Assert.IsTrue(P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief), brief.BuildSummary());
            Assert.AreEqual(P0BattlePlayerBriefPresenter.MaxDefaultLineCount, brief.Lines.Count);
            StringAssert.Contains("+", brief.BuildSummary());
            AssertPlayerBriefHasNoInternalTokens(brief);
        }

        [Test]
        public void Build_ResultBattleHoistsThreeResultActions()
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

            Assert.IsTrue(P0BattlePlayerBriefPresenter.HasP0BattleResultActions(brief), brief.BuildSummary());
            Assert.LessOrEqual(brief.Lines.Count, P0BattlePlayerBriefPresenter.MaxDefaultLineCount);
            StringAssert.Contains("继续路线", brief.BuildSummary());
            StringAssert.Contains("回到猫窝", brief.BuildSummary());
            StringAssert.Contains("重新开始", brief.BuildSummary());
        }

        [Test]
        public void Build_CandidateAssetBatchTextIsRejected()
        {
            P0BattlePlayerBrief brief = P0BattlePlayerBriefPresenter.Build(
                CreateBattle(),
                CreateCats(),
                0,
                "Batch 85 and Batch 89 are candidate references only.",
                CreateRun(),
                null,
                CreateCommandDeck());

            Assert.IsFalse(P0BattlePlayerBriefPresenter.HasP0BattlePlayerBrief(brief));
        }

        [Test]
        public void EvaluatePrototypeBattleBrief_CompletesE1ReadabilityCoverage()
        {
            P0BattleReadabilityCoverageReport report = P0BattleReadabilityCoverage.EvaluatePrototypeBattleBrief();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleReadabilityCoverage.ExpectedCoveredBriefCount, report.CoveredBriefs.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("battle readability coverage complete", report.BuildSummary());
            StringAssert.Contains("Battle result brief", report.BuildDetailedSummary());
            StringAssert.Contains("Candidate asset batches", report.BuildDetailedSummary());
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

        private static void AssertPlayerBriefHasNoInternalTokens(P0BattlePlayerBrief brief)
        {
            string summary = brief.BuildSummary();
            Assert.IsFalse(summary.Contains("Target"));
            Assert.IsFalse(summary.Contains("hunger"));
            Assert.IsFalse(summary.Contains("HP"));
            Assert.IsFalse(summary.Contains(" Lv"));
            Assert.IsFalse(summary.Contains("Batch"));
            Assert.IsFalse(summary.Contains("candidate"));
            Assert.IsFalse(summary.Contains("\n"));
        }
    }
}
