using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StatusHudCoverageTests
    {
        [Test]
        public void BuildPrototypeEntries_CoversAllP0StatusScopes()
        {
            IReadOnlyList<P0StatusHudEntry> entries = P0StatusHudPresenter.BuildPrototypeEntries();

            Assert.IsTrue(P0StatusHudPresenter.HasP0StatusHudEntries(entries));
            Assert.AreEqual(3, entries.Count);
            StringAssert.Contains("标签 5", P0StatusHudPresenter.BuildCompactSummary(entries));
            StringAssert.Contains("图标 3", P0StatusHudPresenter.BuildCompactSummary(entries));
            StringAssert.Contains("响应 3", P0StatusHudPresenter.BuildCompactSummary(entries));
            AssertEntryHasIcon(entries, StatusTagIds.SleepStable, P0VisualAssetCatalog.SleepStableStatusIconId, P0VisualAssetCatalog.SleepStableStatusCompactIconId);
            AssertEntryHasIcon(entries, StatusTagIds.Slow, P0VisualAssetCatalog.SlowStatusIconId, P0VisualAssetCatalog.SlowStatusCompactIconId);
            AssertEntryHasIcon(entries, StatusTagIds.Knockback, P0VisualAssetCatalog.KnockbackStatusIconId, P0VisualAssetCatalog.KnockbackStatusCompactIconId);
            AssertEntryHasIcon(entries, StatusTagIds.Mark, P0VisualAssetCatalog.MarkStatusIconId, P0VisualAssetCatalog.MarkStatusCompactIconId);
            AssertEntryHasIcon(entries, StatusTagIds.Shield, P0VisualAssetCatalog.ShieldStatusIconId, P0VisualAssetCatalog.ShieldStatusCompactIconId);
        }

        [Test]
        public void BuildEnemyEntry_ExposesSlowKnockbackAndMarkResponse()
        {
            BattleEnemyState enemy = new BattleEnemyState(
                7,
                GetEnemy(P0PrototypeCatalog.BlackMudNightmareId),
                3f);
            enemy.ApplyStatus(GetStatus(StatusTagIds.Slow));
            enemy.ApplyStatus(GetStatus(StatusTagIds.Knockback));
            enemy.ApplyStatus(GetStatus(StatusTagIds.Mark));
            enemy.ApplyKnockback(2f);

            P0StatusHudEntry entry = P0StatusHudPresenter.BuildEnemyEntry(enemy);

            Assert.AreEqual(P0StatusHudTargetKind.Enemy, entry.TargetKind);
            Assert.IsTrue(entry.HasTag(StatusTagIds.Slow));
            Assert.IsTrue(entry.HasTag(StatusTagIds.Knockback));
            Assert.IsTrue(entry.HasTag(StatusTagIds.Mark));
            Assert.IsTrue(entry.HasIconFor(StatusTagIds.Slow));
            Assert.IsTrue(entry.HasIconFor(StatusTagIds.Knockback));
            Assert.IsTrue(entry.HasIconFor(StatusTagIds.Mark));
            Assert.IsTrue(entry.HasCompactIconFor(StatusTagIds.Slow));
            Assert.IsTrue(entry.HasCompactIconFor(StatusTagIds.Knockback));
            Assert.IsTrue(entry.HasCompactIconFor(StatusTagIds.Mark));
            Assert.AreEqual(3, entry.StatusIcons.Count);
            Assert.Less(entry.MovementRateMultiplier, 1f);
            Assert.AreEqual(0.65f, entry.MovementRateMultiplier, 0.001f);
            Assert.Greater(entry.DamageTakenMultiplier, 1f);
            StringAssert.Contains("移动 " + entry.MovementRateMultiplier.ToString("0.##") + " 倍", entry.ResponseSummary);
            StringAssert.Contains("承伤 1.25 倍", entry.ResponseSummary);
            StringAssert.Contains("标记", entry.Indicator.Text);
        }

        [Test]
        public void BuildBedAndCatEntries_DistinguishSleepAndShieldProtection()
        {
            StatusEffectCollection bedStatuses = new StatusEffectCollection();
            bedStatuses.Apply(GetStatus(StatusTagIds.SleepStable));
            bedStatuses.Apply(GetStatus(StatusTagIds.Shield), 16f);
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));
            cat.ApplyStatus(GetStatus(StatusTagIds.Shield), 24f);

            P0StatusHudEntry bed = P0StatusHudPresenter.BuildBedEntry(bedStatuses);
            P0StatusHudEntry catEntry = P0StatusHudPresenter.BuildCatEntry(cat);

            Assert.AreEqual(P0StatusHudTargetKind.Bed, bed.TargetKind);
            Assert.IsTrue(bed.HasTag(StatusTagIds.SleepStable));
            Assert.IsTrue(bed.HasIconFor(StatusTagIds.SleepStable));
            Assert.IsTrue(bed.HasIconFor(StatusTagIds.Shield));
            Assert.IsTrue(bed.HasCompactIconFor(StatusTagIds.SleepStable));
            Assert.IsTrue(bed.HasCompactIconFor(StatusTagIds.Shield));
            Assert.AreEqual(16f, bed.ShieldAmount, 0.001f);
            StringAssert.Contains("主人睡眠稳定", bed.ResponseSummary);
            StringAssert.Contains("床护盾", bed.ResponseSummary);
            Assert.AreEqual(P0StatusHudTargetKind.Cat, catEntry.TargetKind);
            Assert.IsTrue(catEntry.HasIconFor(StatusTagIds.Shield));
            Assert.IsTrue(catEntry.HasCompactIconFor(StatusTagIds.Shield));
            Assert.AreEqual(24f, catEntry.ShieldAmount, 0.001f);
            StringAssert.Contains("猫护盾", catEntry.ResponseSummary);
            StringAssert.Contains("生命吸收", catEntry.ResponseSummary);
            StringAssert.DoesNotContain("HP", catEntry.ResponseSummary);
        }

        [Test]
        public void EvaluatePrototypeHud_CompletesCoverageGate()
        {
            P0StatusHudCoverageReport report = P0StatusHudCoverage.EvaluatePrototypeHud();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0StatusHudCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("Status HUD shows enemy Mark", report.BuildDetailedSummary());
            StringAssert.Contains("generated icon assets", report.BuildDetailedSummary());
            StringAssert.Contains("compact 32px icon assets", report.BuildDetailedSummary());
        }

        private static void AssertEntryHasIcon(
            IReadOnlyList<P0StatusHudEntry> entries,
            string statusTagId,
            string expectedAssetId,
            string expectedCompactAssetId)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = 0; j < entries[i].StatusIcons.Count; j++)
                {
                    if (entries[i].StatusIcons[j].StatusTagId == statusTagId)
                    {
                        Assert.AreEqual(expectedAssetId, entries[i].StatusIcons[j].IconAsset.AssetId);
                        Assert.AreEqual(expectedCompactAssetId, entries[i].StatusIcons[j].CompactIconAsset.AssetId);
                        Assert.IsTrue(entries[i].StatusIcons[j].IsReady);
                        return;
                    }
                }
            }

            Assert.Fail("Missing status icon for " + statusTagId);
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

            Assert.Fail("Missing status: " + statusId);
            return default(StatusTagDefinition);
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

        private static CatDefinition GetCat(string catId)
        {
            IReadOnlyList<CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i];
                }
            }

            Assert.Fail("Missing cat: " + catId);
            return null;
        }
    }
}
