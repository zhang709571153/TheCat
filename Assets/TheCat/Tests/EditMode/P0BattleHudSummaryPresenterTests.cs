using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class P0BattleHudSummaryPresenterTests
    {
        [Test]
        public void BuildSections_NullBattleShowsStartObjectiveOnly()
        {
            IReadOnlyList<P0BattleHudSection> sections = P0BattleHudSummaryPresenter.BuildSections(
                null,
                null,
                0,
                string.Empty,
                null,
                null);

            Assert.AreEqual(1, sections.Count);
            Assert.AreEqual("目标", sections[0].Title);
            StringAssert.Contains("路线节点加载后即可开始战斗", sections[0].Lines[0]);
        }

        [Test]
        public void BuildSections_InProgressBattleIncludesDemoHudSections()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            List<CatBattleState> cats = CreateCats();
            RunProgressionState run = CreateRun();
            battle.NodeMetrics.RecordBedPressure(10f, 7f);
            battle.NodeMetrics.RecordBossThrowPressure(4f, 0f);
            battle.NodeMetrics.RecordCatPressure(12f, 5f);
            battle.NodeMetrics.RecordCatHeal(18f);
            battle.NodeMetrics.RecordCatShield(22f);
            battle.NodeMetrics.RecordCatSwitchSuccess();
            battle.NodeMetrics.RecordCatSwitchBlockedByWeak();
            battle.NodeMetrics.RecordAutoTargetAcquired();
            battle.NodeMetrics.RecordSkillTargetAcquired();
            battle.NodeMetrics.RecordSkillCastSuccess();
            battle.NodeMetrics.RecordSkillCastBlockedByTarget();
            battle.NodeMetrics.RecordInteractionBlockedByRange();

            IReadOnlyList<P0BattleHudSection> sections = P0BattleHudSummaryPresenter.BuildSections(
                battle,
                cats,
                1,
                "位置 0.0, 床 0.9, 猫砂盆 3.4, 喂食器 3.4",
                run,
                null);

            Assert.IsTrue(P0BattleHudSummaryPresenter.HasP0BattleHudSections(sections));
            StringAssert.Contains("主人睡眠度", GetSection(sections, "核心数值").BuildSummary());
            string teamSummary = GetSection(sections, "队伍").BuildSummary();
            string routeSummary = GetSection(sections, "路线").BuildSummary();
            StringAssert.Contains("当前：奈芙蒂斯", teamSummary);
            StringAssert.Contains("生命", teamSummary);
            Assert.IsFalse(teamSummary.Contains("HP"));
            StringAssert.Contains("路线：0/10", routeSummary);
            StringAssert.Contains("总等级", routeSummary);
            StringAssert.Contains("等级1", routeSummary);
            Assert.IsFalse(routeSummary.Contains(" Lv"));
            StringAssert.Contains("照看：床", GetSection(sections, "节点指标").BuildSummary());
            StringAssert.Contains("敌人压力：床 1 首领 1", GetSection(sections, "节点指标").BuildSummary());
            StringAssert.Contains("猫生命：受压 1 伤害 5/12", GetSection(sections, "节点指标").BuildSummary());
            StringAssert.Contains("切换：1/2", GetSection(sections, "节点指标").BuildSummary());
            StringAssert.Contains("锁定目标：自动 1/1，技能 1/1", GetSection(sections, "节点指标").BuildSummary());
            StringAssert.Contains("技能：1/2", GetSection(sections, "节点指标").BuildSummary());
            StringAssert.Contains("交互：0/1", GetSection(sections, "节点指标").BuildSummary());
            Assert.IsFalse(GetSection(sections, "节点指标").BuildSummary().Contains("阻止"));
            Assert.IsFalse(GetSection(sections, "节点指标").BuildSummary().Contains("索敌"));
        }

        [Test]
        public void BuildSections_ThreatSectionIncludesWarningsAndStatusTags()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            BattleEnemyState enemy = battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.2f);
            battle.DebugApplyStatusToEnemy(enemy, StatusTagIds.Mark);
            battle.DebugApplyBedStatus(StatusTagIds.Shield, 12f);
            List<CatBattleState> cats = CreateCats();
            cats[0].ApplyStatus(P0PrototypeCatalog.CreateStatusTags()[4]);

            IReadOnlyList<P0BattleHudSection> sections = P0BattleHudSummaryPresenter.BuildSections(
                battle,
                cats,
                0,
                "位置 0.0",
                CreateRun(),
                null);

            string threatSummary = GetSection(sections, "威胁").BuildSummary();

            StringAssert.Contains("预警", threatSummary);
            StringAssert.Contains("压床", threatSummary);
            StringAssert.Contains("床标签", threatSummary);
            StringAssert.Contains("敌人标签", threatSummary);
            StringAssert.Contains("猫咪标签", threatSummary);
        }

        [Test]
        public void BuildSections_ResultBattleIncludesResultSection()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugDamageOwnerSleep(999f);

            IReadOnlyList<P0BattleHudSection> sections = P0BattleHudSummaryPresenter.BuildSections(
                battle,
                CreateCats(),
                0,
                "位置 0.0",
                CreateRun(),
                null);

            P0BattleHudSection result = GetSection(sections, "结算");

            StringAssert.Contains("用时", result.BuildSummary());
            StringAssert.Contains("睡眠变化", result.BuildSummary());
            StringAssert.Contains("拉屎次数", result.BuildSummary());
        }

        [Test]
        public void BuildCompactSummary_ListsSectionCounts()
        {
            IReadOnlyList<P0BattleHudSection> sections = new[]
            {
                new P0BattleHudSection("目标", new[] { "one" }),
                new P0BattleHudSection("核心数值", new[] { "one", "two" })
            };

            string summary = P0BattleHudSummaryPresenter.BuildCompactSummary(sections);

            StringAssert.Contains("目标 1", summary);
            StringAssert.Contains("核心数值 2", summary);
        }

        private static BattleSimulation CreateLayerOneBattle()
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
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.Blessings.Add(P0BlessingCatalog.CreateAuthorityBlessings()[0]);
            return run;
        }

        private static P0BattleHudSection GetSection(IReadOnlyList<P0BattleHudSection> sections, string title)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                if (sections[i].Title == title)
                {
                    return sections[i];
                }
            }

            Assert.Fail("Missing section: " + title);
            return default(P0BattleHudSection);
        }
    }
}
