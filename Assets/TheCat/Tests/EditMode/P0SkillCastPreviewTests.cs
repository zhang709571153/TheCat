using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0SkillCastPreviewTests
    {
        [Test]
        public void BuildSummary_CooldownTakesPriority()
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            P0SkillCastPreview preview = new P0SkillCastPreview(
                skill,
                cooldownSeconds: 2.5f,
                currentHunger: 80f,
                target: default(P0SkillTargetResult));

            Assert.IsFalse(preview.IsReady);
            Assert.AreEqual("技能预览：冷却 2.5s", preview.BuildSummary());
        }

        [Test]
        public void BuildSummary_ShowsEnemyTargetDistanceAndHungerCost()
        {
            SkillDefinition skill = GetSkill("nephthys_royal_mark");
            BattleEnemyState enemy = CreateEnemy(P0PrototypeCatalog.ColdLightShadowId);
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: enemy,
                distance: 2.25f,
                range: 5.25f);
            P0SkillCastPreview preview = new P0SkillCastPreview(
                skill,
                cooldownSeconds: 0f,
                currentHunger: 40f,
                target: target);

            string summary = preview.BuildSummary();

            Assert.IsTrue(preview.IsReady);
            StringAssert.Contains("目标 冷光灯影 2.3/5.3m", summary);
            StringAssert.Contains("饱肚 40->37", summary);
        }

        [Test]
        public void BuildSummary_ShowsNoTargetRequirement()
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: null,
                distance: 0f,
                range: P0SkillTargetResolver.DirectionalRange);
            P0SkillCastPreview preview = new P0SkillCastPreview(skill, 0f, 50f, target);

            Assert.IsFalse(preview.IsReady);
            StringAssert.Contains("无目标 <= 2.4m", preview.BuildSummary());
        }

        [Test]
        public void BuildSummary_SelfSkillDoesNotNeedEnemyTargetAndShowsLowHunger()
        {
            SkillDefinition skill = GetSkill("saiban_oath_shield");
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: false,
                enemy: null,
                distance: 0f,
                range: 0f);
            P0SkillCastPreview preview = new P0SkillCastPreview(skill, 0f, 1f, target);

            string summary = preview.BuildSummary();

            Assert.IsTrue(preview.IsReady);
            StringAssert.Contains("不需要敌人目标", summary);
            StringAssert.Contains("饱肚 1->0 偏低", summary);
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
