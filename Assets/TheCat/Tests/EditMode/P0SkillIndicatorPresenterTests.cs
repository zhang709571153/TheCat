using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0SkillIndicatorPresenterTests
    {
        [Test]
        public void Build_TargetReadyShowsRangeAndTargetPosition()
        {
            SkillDefinition skill = GetSkill("nephthys_royal_mark");
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.ColdLightShadowId);
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: enemy,
                distance: 0f,
                range: P0SkillTargetResolver.AutoNearestRange);

            P0SkillIndicatorState indicator = P0SkillIndicatorPresenter.Build(
                skill,
                cooldownSeconds: 0f,
                origin: Vector2.zero,
                target: target,
                targetPositionResolver: _ => new Vector2(2.25f, 0f));

            Assert.IsTrue(indicator.HasSkill);
            Assert.IsTrue(indicator.CanCast);
            Assert.IsTrue(indicator.ShowsRange);
            Assert.IsTrue(indicator.ShowsTarget);
            Assert.AreEqual("冷光灯影", indicator.TargetDisplayName);
            Assert.AreEqual(2.25f, indicator.Distance, 0.001f);
            Assert.AreEqual(new Vector2(2.25f, 0f), indicator.TargetPosition);
            StringAssert.Contains("技能指示：王权标记 目标 冷光灯影 2.3/5.3m", indicator.BuildSummary());
        }

        [Test]
        public void Build_MissingTargetShowsRangeAndBlocksCast()
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: null,
                distance: 0f,
                range: P0SkillTargetResolver.DirectionalRange);

            P0SkillIndicatorState indicator = P0SkillIndicatorPresenter.Build(
                skill,
                cooldownSeconds: 0f,
                origin: Vector2.zero,
                target: target,
                targetPositionResolver: _ => Vector2.zero);

            Assert.IsFalse(indicator.CanCast);
            Assert.IsTrue(indicator.ShowsRange);
            Assert.IsFalse(indicator.ShowsTarget);
            StringAssert.Contains("无目标 <= 2.4m", indicator.BuildSummary());
        }

        [Test]
        public void Build_NoTargetSkillSkipsEnemyRange()
        {
            SkillDefinition skill = GetSkill("saiban_oath_shield");
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: false,
                enemy: null,
                distance: 0f,
                range: 0f);

            P0SkillIndicatorState indicator = P0SkillIndicatorPresenter.Build(
                skill,
                cooldownSeconds: 0f,
                origin: Vector2.zero,
                target: target,
                targetPositionResolver: _ => Vector2.zero);

            Assert.IsTrue(indicator.CanCast);
            Assert.IsFalse(indicator.ShowsRange);
            Assert.IsFalse(indicator.ShowsTarget);
            Assert.AreEqual("技能指示：银誓护盾 不需要敌人目标", indicator.BuildSummary());
        }

        [Test]
        public void Build_CooldownKeepsTargetVisibleButBlocksCast()
        {
            SkillDefinition skill = GetSkill("nephthys_quicksand_trap");
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId);
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: enemy,
                distance: 1.5f,
                range: P0SkillTargetResolver.AreaAtTargetRange);

            P0SkillIndicatorState indicator = P0SkillIndicatorPresenter.Build(
                skill,
                cooldownSeconds: 3f,
                origin: Vector2.zero,
                target: target,
                targetPositionResolver: _ => new Vector2(1.5f, 0f));

            Assert.IsFalse(indicator.CanCast);
            Assert.IsTrue(indicator.IsCoolingDown);
            Assert.IsTrue(indicator.ShowsRange);
            Assert.IsTrue(indicator.ShowsTarget);
            StringAssert.Contains("冷却 3.0s", indicator.BuildSummary());
            StringAssert.Contains("目标 黑泥梦魇 1.5/4.2m", indicator.BuildSummary());
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            return P0PrototypeCatalog.CreateStarterSkills().Single(skill => skill.Id == skillId);
        }

        private static BattleEnemyState CreateEnemy(string enemyId)
        {
            EnemyDefinition definition = P0PrototypeCatalog.CreateCoreEnemies().Single(enemy => enemy.Id == enemyId);
            return new BattleEnemyState(1, definition, 4f);
        }
    }
}
