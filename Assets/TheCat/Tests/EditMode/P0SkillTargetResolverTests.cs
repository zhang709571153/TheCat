using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0SkillTargetResolverTests
    {
        [Test]
        public void Resolve_SelfAndBedZoneSkillsDoNotRequireEnemyTarget()
        {
            SkillDefinition shield = GetSkill("saiban_oath_shield");
            SkillDefinition sleepBell = GetSkill("suzune_sleep_bell");

            P0SkillTargetResult shieldResult = P0SkillTargetResolver.Resolve(
                shield,
                new BattleEnemyState[0],
                Vector2.zero,
                _ => Vector2.zero);
            P0SkillTargetResult sleepBellResult = P0SkillTargetResolver.Resolve(
                sleepBell,
                new BattleEnemyState[0],
                Vector2.zero,
                _ => Vector2.zero);

            Assert.IsFalse(shieldResult.RequiresEnemyTarget);
            Assert.IsTrue(shieldResult.CanCast);
            Assert.IsFalse(sleepBellResult.RequiresEnemyTarget);
            Assert.IsTrue(sleepBellResult.CanCast);
        }

        [Test]
        public void Resolve_DirectionalSkillRequiresCloseTarget()
        {
            SkillDefinition sweep = GetSkill("saiban_sword_sweep");
            BattleEnemyState mud = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);

            P0SkillTargetResult farResult = P0SkillTargetResolver.Resolve(
                sweep,
                new[] { mud },
                Vector2.zero,
                _ => new Vector2(3f, 0f));
            P0SkillTargetResult closeResult = P0SkillTargetResolver.Resolve(
                sweep,
                new[] { mud },
                Vector2.zero,
                _ => new Vector2(1f, 0f));

            Assert.IsTrue(farResult.RequiresEnemyTarget);
            Assert.IsFalse(farResult.CanCast);
            Assert.IsTrue(closeResult.CanCast);
            Assert.AreSame(mud, closeResult.Enemy);
            Assert.AreEqual(P0SkillTargetResolver.DirectionalRange, closeResult.Range, 0.001f);
        }

        [Test]
        public void Resolve_AreaAtTargetSkillCanReachFartherThanDirectional()
        {
            SkillDefinition sweep = GetSkill("saiban_sword_sweep");
            SkillDefinition quicksand = GetSkill("nephthys_quicksand_trap");
            BattleEnemyState mud = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);

            P0SkillTargetResult sweepResult = P0SkillTargetResolver.Resolve(
                sweep,
                new[] { mud },
                Vector2.zero,
                _ => new Vector2(3.5f, 0f));
            P0SkillTargetResult quicksandResult = P0SkillTargetResolver.Resolve(
                quicksand,
                new[] { mud },
                Vector2.zero,
                _ => new Vector2(3.5f, 0f));

            Assert.IsFalse(sweepResult.CanCast);
            Assert.IsTrue(quicksandResult.CanCast);
            Assert.AreEqual(P0SkillTargetResolver.AreaAtTargetRange, quicksandResult.Range, 0.001f);
        }

        [Test]
        public void Resolve_ChoosesNearestInRangeTarget()
        {
            SkillDefinition mark = GetSkill("nephthys_royal_mark");
            BattleEnemyState far = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            BattleEnemyState near = CreateEnemy(P0PrototypeCatalog.ColdLightShadowId, instanceId: 2);

            P0SkillTargetResult result = P0SkillTargetResolver.Resolve(
                mark,
                new[] { far, near },
                Vector2.zero,
                enemy => enemy == near ? new Vector2(2f, 0f) : new Vector2(4f, 0f));

            Assert.IsTrue(result.CanCast);
            Assert.AreSame(near, result.Enemy);
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            return P0PrototypeCatalog.CreateStarterSkills().Single(skill => skill.Id == skillId);
        }

        private static BattleEnemyState CreateEnemy(string enemyId, int instanceId = 1)
        {
            EnemyDefinition definition = P0PrototypeCatalog.CreateCoreEnemies().Single(enemy => enemy.Id == enemyId);
            return new BattleEnemyState(instanceId, definition, 4f);
        }
    }
}
