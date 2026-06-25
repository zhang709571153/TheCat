using System;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0CatPressureApplierTests
    {
        [Test]
        public void Apply_RecordsIncomingTakenAndShieldAbsorbedDamage()
        {
            NodeMetrics metrics = new NodeMetrics(1, "cat_pressure_test", 100f);
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));
            cat.ApplyStatus(GetStatus(StatusTagIds.Shield), 10f);
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            float incoming = enemy.Definition.PlayerDamage;

            P0CatPressureApplication application = P0CatPressureApplier.Apply(
                metrics,
                cat,
                enemy,
                damageScale: 10f,
                damageMultiplier: 1f);

            Assert.AreEqual(incoming, application.IncomingDamage, 0.001f);
            Assert.AreEqual(Math.Max(0f, incoming - 10f), application.DamageTaken, 0.001f);
            Assert.AreEqual(Math.Min(incoming, 10f), application.DamageAbsorbed, 0.001f);
            Assert.IsFalse(application.BecameWeak);
            Assert.AreEqual(1, metrics.CatPressureEvents);
            Assert.AreEqual(incoming, metrics.CatDamageIncoming, 0.001f);
            Assert.AreEqual(application.DamageTaken, metrics.CatDamageTaken, 0.001f);
            Assert.AreEqual(application.DamageAbsorbed, metrics.CatDamageAbsorbed, 0.001f);
            Assert.AreEqual(0, metrics.WeakIncidents);
        }

        [Test]
        public void Apply_RecordsWeakIncidentWhenPressureDropsCatToWeak()
        {
            NodeMetrics metrics = new NodeMetrics(1, "cat_pressure_test", 100f);
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId), currentHp: 5f);
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.RedEyeAlarmId);

            P0CatPressureApplication application = P0CatPressureApplier.Apply(
                metrics,
                cat,
                enemy,
                damageScale: 1f,
                damageMultiplier: 1f);

            Assert.IsTrue(application.BecameWeak);
            Assert.IsTrue(cat.Vital.IsWeak);
            Assert.AreEqual(1, metrics.CatPressureEvents);
            Assert.AreEqual(1, metrics.WeakIncidents);
            Assert.Greater(metrics.CatDamageTaken, 0f);
        }

        [Test]
        public void Apply_RejectsMissingInputsAndNegativeMultipliers()
        {
            NodeMetrics metrics = new NodeMetrics(1, "cat_pressure_test", 100f);
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);

            Assert.Throws<ArgumentNullException>(() => P0CatPressureApplier.Apply(null, cat, enemy, 1f, 1f));
            Assert.Throws<ArgumentNullException>(() => P0CatPressureApplier.Apply(metrics, null, enemy, 1f, 1f));
            Assert.Throws<ArgumentNullException>(() => P0CatPressureApplier.Apply(metrics, cat, null, 1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => P0CatPressureApplier.Apply(metrics, cat, enemy, -1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => P0CatPressureApplier.Apply(metrics, cat, enemy, 1f, -1f));
        }

        private static CatDefinition GetCat(string catId)
        {
            foreach (CatDefinition cat in P0PrototypeCatalog.CreateStarterCats())
            {
                if (cat.Id == catId)
                {
                    return cat;
                }
            }

            Assert.Fail("Missing cat: " + catId);
            return null;
        }

        private static BattleEnemyState CreateEnemy(string enemyId)
        {
            foreach (EnemyDefinition enemy in P0PrototypeCatalog.CreateCoreEnemies())
            {
                if (enemy.Id == enemyId)
                {
                    return new BattleEnemyState(1, enemy, 4f);
                }
            }

            Assert.Fail("Missing enemy: " + enemyId);
            return null;
        }

        private static StatusTagDefinition GetStatus(string statusId)
        {
            foreach (StatusTagDefinition status in P0PrototypeCatalog.CreateStatusTags())
            {
                if (status.Id == statusId)
                {
                    return status;
                }
            }

            Assert.Fail("Missing status: " + statusId);
            return default(StatusTagDefinition);
        }
    }
}
