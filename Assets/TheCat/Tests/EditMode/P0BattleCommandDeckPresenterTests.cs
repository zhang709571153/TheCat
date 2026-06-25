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
    public sealed class P0BattleCommandDeckPresenterTests
    {
        [Test]
        public void BuildDeck_UsesPlayerFacingRowsForDefaultHud()
        {
            BattleSimulation battle = CreateBattle();
            List<CatBattleState> cats = CreateCats();
            IReadOnlyList<P0CatHudCard> catCards = P0CatHudPresenter.BuildCards(cats, 0, _ => 0f);
            IReadOnlyList<P0SkillHudCard> skillCards = BuildSkillCards(battle, cats[0]);
            IReadOnlyList<P0BattleActionAffordance> interactions = BuildInteractions(battle);

            P0BattleCommandDeck deck = P0BattleCommandDeckPresenter.BuildDeck(catCards, skillCards, interactions);

            Assert.IsTrue(P0BattleCommandDeckPresenter.HasP0BattleCommandDeck(deck), deck.BuildSummary());
            Assert.AreEqual("当前行动", deck.Title);
            Assert.AreEqual(3, deck.CatCount);
            Assert.AreEqual(3, deck.SkillCount);
            Assert.AreEqual(3, deck.InteractionCount);
            StringAssert.Contains("上阵：", deck.BuildSummary());
            StringAssert.Contains("主技能：", deck.BuildSummary());
            StringAssert.Contains("交互：", deck.BuildSummary());
            StringAssert.Contains("当前行动：上阵：", deck.BuildCompactPlayerLine());
            Assert.LessOrEqual(deck.BuildCompactPlayerLine().Length, 72);
            Assert.IsFalse(deck.BuildSummary().Contains("Target"));
            Assert.IsFalse(deck.BuildSummary().Contains("hunger"));
            Assert.IsFalse(deck.BuildCompactPlayerLine().Contains("\n"));
            Assert.IsFalse(deck.BuildSummary().Contains("缺失技能"));
            Assert.IsFalse(deck.BuildSummary().Contains("缺少技能定义"));
        }

        [Test]
        public void BuildDeck_PrioritizesEnabledSkillAndInteraction()
        {
            BattleSimulation battle = CreateBattle();
            List<CatBattleState> cats = CreateCats();
            SkillDefinition readySkill = GetSkill("saiban_sun_charge");
            SkillDefinition blockedSkill = GetSkill("saiban_sword_sweep");
            P0SkillTargetResult noTarget = new P0SkillTargetResult(true, null, 0f, 2.4f);
            List<P0SkillHudCard> skillCards = new List<P0SkillHudCard>
            {
                BuildSkillCard(blockedSkill, battle, noTarget),
                BuildSkillCard(readySkill, battle, default(P0SkillTargetResult))
            };
            List<P0BattleActionAffordance> interactions = new List<P0BattleActionAffordance>
            {
                P0BattleActionAffordancePresenter.BuildLitterBox(true, false, battle.TeamPoop),
                P0BattleActionAffordancePresenter.BuildBedCare(true, true, battle.OwnerSleep, battle.TeamHunger),
                P0BattleActionAffordancePresenter.BuildFeeder(true, false, battle.TeamHunger)
            };

            P0BattleCommandDeck deck = P0BattleCommandDeckPresenter.BuildDeck(
                P0CatHudPresenter.BuildCards(cats, 0, _ => 0f),
                skillCards,
                interactions);

            StringAssert.Contains("王冠裁决", deck.Lines[1]);
            StringAssert.Contains("照看床", deck.Lines[2]);
        }

        [Test]
        public void HasP0BattleCommandDeck_RejectsIncompleteDecks()
        {
            P0BattleCommandDeck empty = P0BattleCommandDeckPresenter.BuildDeck(
                null,
                null,
                null);

            Assert.IsFalse(P0BattleCommandDeckPresenter.HasP0BattleCommandDeck(empty));
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

        private static IReadOnlyList<P0SkillHudCard> BuildSkillCards(BattleSimulation battle, CatBattleState cat)
        {
            List<P0SkillHudCard> cards = new List<P0SkillHudCard>();
            for (int i = 0; i < cat.Definition.SkillIds.Count; i++)
            {
                SkillDefinition skill = GetSkill(cat.Definition.SkillIds[i]);
                P0SkillTargetResult target = P0SkillTargetResolver.RequiresEnemyTarget(skill)
                    ? new P0SkillTargetResult(true, battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.4f), 1.4f, 2.4f)
                    : default(P0SkillTargetResult);
                cards.Add(BuildSkillCard(skill, battle, target));
            }

            return cards.AsReadOnly();
        }

        private static P0SkillHudCard BuildSkillCard(
            SkillDefinition skill,
            BattleSimulation battle,
            P0SkillTargetResult target)
        {
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(
                skill,
                0f,
                battle.TeamHunger.Current,
                target,
                true);
            return P0SkillHudPresenter.BuildCard(
                skill,
                affordance,
                0f,
                battle.TeamHunger.Current,
                target);
        }

        private static IReadOnlyList<P0BattleActionAffordance> BuildInteractions(BattleSimulation battle)
        {
            return new[]
            {
                P0BattleActionAffordancePresenter.BuildBedCare(true, true, battle.OwnerSleep, battle.TeamHunger),
                P0BattleActionAffordancePresenter.BuildLitterBox(true, false, battle.TeamPoop),
                P0BattleActionAffordancePresenter.BuildFeeder(true, true, battle.TeamHunger)
            };
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            foreach (SkillDefinition skill in P0PrototypeCatalog.CreateStarterSkills())
            {
                if (skill.Id == skillId)
                {
                    return skill;
                }
            }

            Assert.Fail("Missing skill: " + skillId);
            return null;
        }
    }
}
