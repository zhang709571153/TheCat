using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0BattleHudPromptPresenterTests
    {
        [Test]
        public void Build_NullBattlePromptsStart()
        {
            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(null, null);

            Assert.AreEqual(P0BattleHudPromptLevel.Info, prompt.Level);
            StringAssert.Contains("开始战斗", prompt.ActionText);
        }

        [Test]
        public void Build_DefeatAndVictoryPromptRouteAction()
        {
            BattleSimulation defeat = CreateLayerOneBattle();
            defeat.DebugDamageOwnerSleep(999f);

            P0BattleHudPrompt defeatPrompt = P0BattleHudPromptPresenter.Build(defeat, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Result, defeatPrompt.Level);
            Assert.AreEqual("失败", defeatPrompt.Title);

            BattleSimulation victory = CreateLayerOneBattle();
            for (int i = 0; i < 20 && victory.Outcome == BattleOutcome.InProgress; i++)
            {
                victory.Tick(5f);
                while (victory.ApplyDamageToNearestEnemy(999f))
                {
                }
            }

            P0BattleHudPrompt victoryPrompt = P0BattleHudPromptPresenter.Build(victory, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Result, victoryPrompt.Level);
            Assert.AreEqual("胜利", victoryPrompt.Title);
            StringAssert.Contains("继续路线", victoryPrompt.ActionText);
        }

        [Test]
        public void Build_SleepDangerOverridesOtherWarnings()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugSpendHunger(50f);
            battle.DebugDamageOwnerSleep(80f);

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Critical, prompt.Level);
            StringAssert.Contains("睡眠", prompt.Title);
            StringAssert.Contains("床", prompt.ActionText);
        }

        [Test]
        public void Build_PoopCountdownPromptsLitterBox()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugForcePoopCountdown();

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Critical, prompt.Level);
            Assert.AreEqual("屎意倒计时", prompt.Title);
            StringAssert.Contains("猫砂盆", prompt.ActionText);
        }

        [Test]
        public void Build_EmptyHungerPromptsFeeder()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugSpendHunger(100f);

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Critical, prompt.Level);
            Assert.AreEqual("饱肚见底", prompt.Title);
            StringAssert.Contains("喂食器", prompt.ActionText);
        }

        [Test]
        public void Build_WeakCatPromptsSwitch()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            List<CatBattleState> cats = CreateCats();
            cats[0].ApplyDamage(999f);

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, cats);

            Assert.AreEqual(P0BattleHudPromptLevel.Warning, prompt.Level);
            Assert.AreEqual("猫咪虚弱", prompt.Title);
            StringAssert.Contains("切换", prompt.ActionText);
        }

        [Test]
        public void Build_BossWarningPromptsPatternRead()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            BattleEnemyState boss = battle.DebugSpawnEnemy(P0PrototypeCatalog.CallTyrantId, 8f);
            boss.DebugSetBossTimers(1f, 1.5f);

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Warning, prompt.Level);
            Assert.AreEqual("首领行动", prompt.Title);
            StringAssert.Contains("首领", prompt.DetailText);
        }

        [Test]
        public void Build_BedContactPromptsKnockback()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 1.5f);

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Warning, prompt.Level);
            Assert.AreEqual("即将压床", prompt.Title);
            StringAssert.Contains("击退", prompt.ActionText);
        }

        [Test]
        public void Build_StableBattlePromptsFocusThreats()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.DebugSpawnEnemy(P0PrototypeCatalog.BlackMudNightmareId, 6f);

            P0BattleHudPrompt prompt = P0BattleHudPromptPresenter.Build(battle, CreateCats());

            Assert.AreEqual(P0BattleHudPromptLevel.Info, prompt.Level);
            Assert.AreEqual("守住床", prompt.Title);
            StringAssert.Contains("释放技能", prompt.ActionText);
        }

        [Test]
        public void BuildSummary_IncludesLevelTitleAndAction()
        {
            P0BattleHudPrompt prompt = new P0BattleHudPrompt(
                P0BattleHudPromptLevel.Warning,
                "屎意偏高",
                "规划一次猫砂盆行程。",
                "80/100");

            string summary = prompt.BuildSummary();

            StringAssert.Contains("警告", summary);
            StringAssert.Contains("屎意偏高", summary);
            StringAssert.Contains("猫砂盆", summary);
            StringAssert.Contains("80/100", summary);
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
    }
}
