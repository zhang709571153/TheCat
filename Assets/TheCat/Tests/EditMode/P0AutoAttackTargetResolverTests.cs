using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0AutoAttackTargetResolverTests
    {
        [Test]
        public void FindBestTarget_DefenderRequiresCloseEnemy()
        {
            CatBattleState saiban = CreateCat(P0PrototypeCatalog.SaibanId);
            BattleEnemyState mud = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);

            P0AutoAttackTargetResult farResult = P0AutoAttackTargetResolver.FindBestTarget(
                new[] { mud },
                Vector2.zero,
                saiban,
                _ => new Vector2(2f, 0f));
            P0AutoAttackTargetResult closeResult = P0AutoAttackTargetResolver.FindBestTarget(
                new[] { mud },
                Vector2.zero,
                saiban,
                _ => new Vector2(0.8f, 0f));

            Assert.IsFalse(farResult.HasTarget);
            Assert.IsTrue(closeResult.HasTarget);
            Assert.AreSame(mud, closeResult.Enemy);
        }

        [Test]
        public void FindBestTarget_ControllerCanReachFartherThanDefender()
        {
            CatBattleState saiban = CreateCat(P0PrototypeCatalog.SaibanId);
            CatBattleState nephthys = CreateCat(P0PrototypeCatalog.NephthysId);
            BattleEnemyState mud = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);

            P0AutoAttackTargetResult defenderResult = P0AutoAttackTargetResolver.FindBestTarget(
                new[] { mud },
                Vector2.zero,
                saiban,
                _ => new Vector2(3f, 0f));
            P0AutoAttackTargetResult controllerResult = P0AutoAttackTargetResolver.FindBestTarget(
                new[] { mud },
                Vector2.zero,
                nephthys,
                _ => new Vector2(3f, 0f));

            Assert.IsFalse(defenderResult.HasTarget);
            Assert.IsTrue(controllerResult.HasTarget);
            Assert.AreEqual(P0AutoAttackTargetResolver.ControllerAttackRange, controllerResult.Range, 0.001f);
        }

        [Test]
        public void FindBestTarget_ChoosesNearestEnemyWithinRange()
        {
            CatBattleState suzune = CreateCat(P0PrototypeCatalog.SuzuneId);
            BattleEnemyState far = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            BattleEnemyState near = CreateEnemy(P0PrototypeCatalog.ColdLightShadowId, instanceId: 2);

            P0AutoAttackTargetResult result = P0AutoAttackTargetResolver.FindBestTarget(
                new[] { far, near },
                Vector2.zero,
                suzune,
                enemy => enemy == near ? new Vector2(1f, 0f) : new Vector2(2f, 0f));

            Assert.IsTrue(result.HasTarget);
            Assert.AreSame(near, result.Enemy);
        }

        [Test]
        public void GetAttackRange_CoversStarterRoles()
        {
            Assert.Less(P0AutoAttackTargetResolver.GetAttackRange(CatRole.Defender), P0AutoAttackTargetResolver.GetAttackRange(CatRole.Controller));
            Assert.Greater(P0AutoAttackTargetResolver.GetAttackRange(CatRole.Healer), P0AutoAttackTargetResolver.GetAttackRange(CatRole.Defender));
        }

        private static CatBattleState CreateCat(string catId)
        {
            CatDefinition definition = P0PrototypeCatalog.CreateStarterCats().Single(cat => cat.Id == catId);
            return new CatBattleState(definition);
        }

        private static BattleEnemyState CreateEnemy(string enemyId, int instanceId = 1)
        {
            EnemyDefinition definition = P0PrototypeCatalog.CreateCoreEnemies().Single(enemy => enemy.Id == enemyId);
            return new BattleEnemyState(instanceId, definition, 4f);
        }
    }
}
