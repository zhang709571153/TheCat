using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0EnemyWarningIndicatorPresenterTests
    {
        [Test]
        public void Build_NearBedEnemyShowsBedContactRing()
        {
            BattleEnemyState enemy = CreateEnemy(
                P0PrototypeCatalog.BlackMudNightmareId,
                EnemyWarningFormatter.BedWarningThresholdSeconds);

            P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                enemy,
                new Vector2(0f, -2f),
                new Vector2(0f, -3.5f));

            Assert.AreEqual(P0EnemyWarningKind.BedContact, warning.Kind);
            Assert.IsTrue(warning.UsesRing);
            Assert.IsFalse(warning.UsesLine);
            Assert.AreEqual(P0VisualAssetCatalog.BlackMudBedClawWarningVfxId, warning.VisualAsset.AssetId);
            Assert.IsTrue(Contains(warning.VisualAsset.SourceLockIds, "black_mud_animation"));
            StringAssert.Contains("压床", warning.BuildSummary());
        }

        [Test]
        public void Build_ChargerShowsChargeLaneLine()
        {
            BattleEnemyState enemy = CreateEnemy(
                P0PrototypeCatalog.DreamRailToyTrainId,
                EnemyWarningFormatter.ChargeWarningThresholdSeconds);

            P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                enemy,
                new Vector2(1f, 1f),
                new Vector2(0f, -3.5f));

            Assert.AreEqual(P0EnemyWarningKind.ChargeLane, warning.Kind);
            Assert.IsTrue(warning.UsesLine);
            Assert.IsFalse(warning.UsesRing);
            Assert.AreEqual(new Vector2(0f, -3.5f), warning.Target);
        }

        [Test]
        public void Build_CoversRangedFlyerAndJumpWarnings()
        {
            BattleEnemyState coldLight = CreateEnemy(
                P0PrototypeCatalog.ColdLightShadowId,
                EnemyWarningFormatter.RangedPressureWarningThresholdSeconds);
            BattleEnemyState flyer = CreateEnemy(
                P0PrototypeCatalog.UnreadRedDotFlyerId,
                EnemyWarningFormatter.FlyingAttachWarningThresholdSeconds);
            BattleEnemyState teddy = CreateEnemy(
                P0PrototypeCatalog.FallingDreamTeddyId,
                EnemyWarningFormatter.JumpSlamWarningThresholdSeconds);

            P0EnemyWarningIndicatorState coldLightWarning = P0EnemyWarningIndicatorPresenter.Build(coldLight, Vector2.zero, Vector2.down);
            P0EnemyWarningIndicatorState flyerWarning = P0EnemyWarningIndicatorPresenter.Build(flyer, Vector2.zero, Vector2.down);
            P0EnemyWarningIndicatorState teddyWarning = P0EnemyWarningIndicatorPresenter.Build(teddy, Vector2.zero, Vector2.down);

            Assert.AreEqual(P0EnemyWarningKind.RangedPressure, coldLightWarning.Kind);
            Assert.IsTrue(coldLightWarning.UsesLine);
            Assert.AreEqual(P0VisualAssetCatalog.ColdLightBeamWarningVfxId, coldLightWarning.VisualAsset.AssetId);
            Assert.IsTrue(Contains(coldLightWarning.VisualAsset.SourceLockIds, "cold_light_animation"));
            Assert.AreEqual(P0EnemyWarningKind.FlyerAttach, flyerWarning.Kind);
            Assert.IsTrue(flyerWarning.UsesRing);
            Assert.AreEqual(P0EnemyWarningKind.JumpSlam, teddyWarning.Kind);
            Assert.IsTrue(teddyWarning.UsesRing);
        }

        [Test]
        public void Build_BossShowsMostUrgentSpecialWarning()
        {
            BattleEnemyState boss = CreateEnemy(P0PrototypeCatalog.CallTyrantId, 10f);
            boss.TickBossSummon(6.1f, 8f);
            boss.TickBossThrow(3.6f, 6f);

            P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                boss,
                new Vector2(0f, 3f),
                new Vector2(0f, -3.5f));

            Assert.AreEqual(P0EnemyWarningKind.BossThrow, warning.Kind);
            Assert.IsTrue(warning.UsesLine);
            Assert.AreEqual(P0VisualAssetCatalog.CallTyrantAppThrowVfxId, warning.VisualAsset.AssetId);
            Assert.IsTrue(warning.VisualAsset.RequiresWorkspaceFile);
            Assert.IsTrue(Contains(warning.VisualAsset.SourceLockIds, "call_tyrant_animation"));
            StringAssert.Contains("首领投掷", warning.BuildSummary());
        }

        [Test]
        public void Build_BossSummonWarningUsesCallTyrantWarningVfx()
        {
            BattleEnemyState boss = CreateEnemy(P0PrototypeCatalog.CallTyrantId, 10f);
            boss.DebugSetBossTimers(1f, 3f);

            P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                boss,
                new Vector2(0f, 3f),
                new Vector2(0f, -3.5f));

            Assert.AreEqual(P0EnemyWarningKind.BossSummon, warning.Kind);
            Assert.IsTrue(warning.UsesRing);
            Assert.AreEqual(P0VisualAssetCatalog.CallTyrantSummonPortalVfxId, warning.VisualAsset.AssetId);
        }

        [Test]
        public void Build_FarEnemyHasNoWarning()
        {
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId, 10f);

            P0EnemyWarningIndicatorState warning = P0EnemyWarningIndicatorPresenter.Build(
                enemy,
                Vector2.zero,
                Vector2.down);

            Assert.IsFalse(warning.HasWarning);
            Assert.AreEqual("敌人预警：无", warning.BuildSummary());
        }

        private static BattleEnemyState CreateEnemy(string enemyId, float timeToBedSeconds)
        {
            foreach (EnemyDefinition enemy in P0PrototypeCatalog.CreateCoreEnemies())
            {
                if (enemy.Id == enemyId)
                {
                    return new BattleEnemyState(1, enemy, timeToBedSeconds);
                }
            }

            Assert.Fail("Missing enemy: " + enemyId);
            return null;
        }

        private static bool Contains(System.Collections.Generic.IReadOnlyList<string> values, string expected)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
