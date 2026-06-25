using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0EnemyPressureResolverTests
    {
        [Test]
        public void FindBestPressureSource_MeleeRequiresClosePosition()
        {
            BattleEnemyState mud = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);

            P0EnemyPressureResult farResult = P0EnemyPressureResolver.FindBestPressureSource(
                new[] { mud },
                Vector2.zero,
                _ => new Vector2(2f, 0f));
            P0EnemyPressureResult closeResult = P0EnemyPressureResolver.FindBestPressureSource(
                new[] { mud },
                Vector2.zero,
                _ => new Vector2(0.4f, 0f));

            Assert.IsFalse(farResult.HasEnemy);
            Assert.IsTrue(closeResult.HasEnemy);
            Assert.AreSame(mud, closeResult.Enemy);
        }

        [Test]
        public void FindBestPressureSource_RangedHarasserCanPressureAcrossBedroom()
        {
            BattleEnemyState alarm = CreateEnemy(P0PrototypeCatalog.RedEyeAlarmId);

            P0EnemyPressureResult result = P0EnemyPressureResolver.FindBestPressureSource(
                new[] { alarm },
                Vector2.zero,
                _ => new Vector2(3.5f, 0f));

            Assert.IsTrue(result.HasEnemy);
            Assert.AreSame(alarm, result.Enemy);
            Assert.AreEqual(P0EnemyPressureResolver.RangedPressureRange, result.Range, 0.001f);
            Assert.Less(result.DamageMultiplier, 1f);
        }

        [Test]
        public void FindBestPressureSource_ChoosesMostUrgentNormalizedRange()
        {
            BattleEnemyState mud = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            BattleEnemyState alarm = CreateEnemy(P0PrototypeCatalog.RedEyeAlarmId, instanceId: 2);

            P0EnemyPressureResult result = P0EnemyPressureResolver.FindBestPressureSource(
                new[] { mud, alarm },
                Vector2.zero,
                enemy => enemy == mud ? new Vector2(0.9f, 0f) : new Vector2(2f, 0f));

            Assert.IsTrue(result.HasEnemy);
            Assert.AreSame(alarm, result.Enemy);
        }

        [Test]
        public void GetPressureRangeAndDamageMultiplier_CoverP0EnemyBehaviors()
        {
            Assert.Greater(P0EnemyPressureResolver.GetPressureRange(EnemyBehaviorType.RangedHarasser), P0EnemyPressureResolver.MeleePressureRange);
            Assert.Greater(P0EnemyPressureResolver.GetPressureRange(EnemyBehaviorType.BossCallTyrant), P0EnemyPressureResolver.RangedPressureRange);
            Assert.Greater(P0EnemyPressureResolver.GetDamageMultiplier(EnemyBehaviorType.Charger), P0EnemyPressureResolver.GetDamageMultiplier(EnemyBehaviorType.MoveToBed));
            Assert.Less(P0EnemyPressureResolver.GetDamageMultiplier(EnemyBehaviorType.FlyingAttachment), P0EnemyPressureResolver.GetDamageMultiplier(EnemyBehaviorType.MoveToBed));
        }

        private static BattleEnemyState CreateEnemy(string enemyId, int instanceId = 1)
        {
            foreach (EnemyDefinition enemy in P0PrototypeCatalog.CreateCoreEnemies())
            {
                if (enemy.Id == enemyId)
                {
                    return new BattleEnemyState(instanceId, enemy, 4f);
                }
            }

            Assert.Fail("Missing enemy: " + enemyId);
            return null;
        }
    }
}
