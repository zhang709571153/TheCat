using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0EnemyHudCoverageTests
    {
        [Test]
        public void BuildPrototypeCards_CoversP0CoreThreats()
        {
            IReadOnlyList<P0EnemyHudCard> cards = P0EnemyHudPresenter.BuildPrototypeCards();

            Assert.IsTrue(P0EnemyHudPresenter.HasP0EnemyHudCards(cards));
            Assert.AreEqual(3, cards.Count);
            StringAssert.Contains("首领 1", P0EnemyHudPresenter.BuildCompactSummary(cards));
            StringAssert.Contains("预警 3", P0EnemyHudPresenter.BuildCompactSummary(cards));
            StringAssert.Contains("压力源 1", P0EnemyHudPresenter.BuildCompactSummary(cards));
        }

        [Test]
        public void BuildCard_BlackMudShowsCriticalBedPressure()
        {
            BattleEnemyState enemy = new BattleEnemyState(
                1,
                GetEnemy(P0PrototypeCatalog.BlackMudNightmareId),
                EnemyWarningFormatter.BedWarningThresholdSeconds);

            P0EnemyHudCard card = P0EnemyHudPresenter.BuildCard(enemy);

            Assert.AreEqual("压床", card.ThreatToken);
            Assert.AreEqual("床", card.TargetToken);
            Assert.AreEqual("危急", card.PriorityToken);
            StringAssert.Contains("压床", card.WarningText);
            StringAssert.Contains("拦截", card.CounterHint);
            StringAssert.Contains("黑泥梦魇", card.BuildButtonLabel());
            StringAssert.Contains("生命", card.BuildButtonLabel());
            Assert.IsFalse(card.BuildButtonLabel().Contains("HP"));
            Assert.IsFalse(card.BuildSummary().Contains("HP"));
        }

        [Test]
        public void BuildCards_ColdLightShowsRangedPressureSource()
        {
            BattleEnemyState coldLight = new BattleEnemyState(
                2,
                GetEnemy(P0PrototypeCatalog.ColdLightShadowId),
                EnemyWarningFormatter.RangedPressureWarningThresholdSeconds);
            P0EnemyPressureResult pressure = new P0EnemyPressureResult(
                coldLight,
                2f,
                P0EnemyPressureResolver.GetPressureRange(coldLight.Definition.BehaviorType),
                P0EnemyPressureResolver.GetDamageMultiplier(coldLight.Definition.BehaviorType));

            IReadOnlyList<P0EnemyHudCard> cards = P0EnemyHudPresenter.BuildCards(new[] { coldLight }, pressure);

            Assert.AreEqual(1, cards.Count);
            Assert.AreEqual("远程压制", cards[0].ThreatToken);
            Assert.AreEqual("猫", cards[0].TargetToken);
            Assert.IsTrue(cards[0].IsPressureSource);
            Assert.Greater(cards[0].PressureRange, 4f);
            Assert.Greater(cards[0].CatDamage, cards[0].BedDamage);
            StringAssert.Contains("远程压制", cards[0].WarningText);
        }

        [Test]
        public void BuildCard_CallTyrantShowsBossSummonAndThrowTimers()
        {
            BattleEnemyState boss = new BattleEnemyState(
                3,
                GetEnemy(P0PrototypeCatalog.CallTyrantId),
                8f);
            boss.DebugSetBossTimers(
                EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                EnemyWarningFormatter.BossThrowWarningThresholdSeconds);

            P0EnemyHudCard card = P0EnemyHudPresenter.BuildCard(boss);

            Assert.IsTrue(card.IsBoss);
            Assert.AreEqual("首领机制", card.ThreatToken);
            Assert.AreEqual("床+猫", card.TargetToken);
            Assert.AreEqual("危急", card.PriorityToken);
            StringAssert.Contains("首领召唤", card.WarningText);
            StringAssert.Contains("首领投掷", card.WarningText);
            StringAssert.Contains("投掷", card.CounterHint);
        }

        [Test]
        public void EvaluatePrototypeCards_CompletesCoverageGate()
        {
            P0EnemyHudCoverageReport report = P0EnemyHudCoverage.EvaluatePrototypeCards();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0EnemyHudCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("Call Tyrant chief summon", report.BuildDetailedSummary());
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

            Assert.Fail("Missing enemy: " + enemyId);
            return null;
        }
    }
}
