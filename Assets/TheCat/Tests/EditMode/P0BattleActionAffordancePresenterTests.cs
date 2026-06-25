using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0BattleActionAffordancePresenterTests
    {
        [Test]
        public void BuildSkill_CooldownDisablesAction()
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(
                skill,
                cooldownSeconds: 2.5f,
                currentHunger: 80f,
                target: CreateTarget(CreateEnemy(P0PrototypeCatalog.BlackMudNightmareId)),
                battleActive: true);

            Assert.IsFalse(affordance.IsEnabled);
            Assert.AreEqual("圆盾冲锋", affordance.Title);
            Assert.AreEqual("冷却 2.5s", affordance.Status);
            StringAssert.Contains("饱肚", affordance.Detail);
        }

        [Test]
        public void BuildSkill_LowHungerStillEnabledForLightPenaltyModel()
        {
            SkillDefinition skill = GetSkill("saiban_oath_shield");
            P0SkillTargetResult target = new P0SkillTargetResult(false, null, 0f, 0f);

            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(
                skill,
                cooldownSeconds: 0f,
                currentHunger: 1f,
                target: target,
                battleActive: true);

            Assert.IsTrue(affordance.IsEnabled);
            Assert.AreEqual("就绪，饱肚度偏低", affordance.Status);
            StringAssert.Contains("饱肚 1->0", affordance.Detail);
        }

        [Test]
        public void BuildSkill_NoTargetDisablesActionAndShowsRange()
        {
            SkillDefinition skill = GetSkill("nephthys_royal_mark");
            P0SkillTargetResult target = new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: null,
                distance: 0f,
                range: 5.25f);

            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(
                skill,
                cooldownSeconds: 0f,
                currentHunger: 60f,
                target: target,
                battleActive: true);

            Assert.IsFalse(affordance.IsEnabled);
            Assert.AreEqual("需要目标 <= 5.3m", affordance.Status);
            StringAssert.Contains("无目标 <= 5.3m", affordance.Detail);
        }

        [Test]
        public void BuildInteractions_ShowRangeStateAndCoreValues()
        {
            OwnerSleepState sleep = new OwnerSleepState(74f, 90f, 100f);
            TeamHungerGauge hunger = new TeamHungerGauge(36f);
            TeamPoopGauge poop = new TeamPoopGauge(100f);
            poop.Tick(0.1f, P0Tuning.Default, isDigesting: false, layer: 1);

            P0BattleActionAffordance bed = P0BattleActionAffordancePresenter.BuildBedCare(
                battleActive: true,
                inRange: true,
                sleep: sleep,
                hunger: hunger);
            P0BattleActionAffordance litter = P0BattleActionAffordancePresenter.BuildLitterBox(
                battleActive: true,
                inRange: false,
                poop: poop);
            P0BattleActionAffordance feeder = P0BattleActionAffordancePresenter.BuildFeeder(
                battleActive: false,
                inRange: true,
                hunger: hunger);

            Assert.IsTrue(bed.IsEnabled);
            Assert.AreEqual("就绪", bed.Status);
            StringAssert.Contains("睡眠 74/90", bed.Detail);
            Assert.IsFalse(litter.IsEnabled);
            Assert.AreEqual("靠近猫砂盆", litter.Status);
            StringAssert.Contains("倒计时", litter.Detail);
            Assert.IsFalse(feeder.IsEnabled);
            Assert.AreEqual("未激活", feeder.Status);
        }

        [Test]
        public void HasP0BattleActionAffordances_RequiresThreeSkillsAndCoreInteractions()
        {
            SkillDefinition skill = GetSkill("saiban_oath_shield");
            P0SkillTargetResult target = new P0SkillTargetResult(false, null, 0f, 0f);
            P0BattleActionAffordance[] complete =
            {
                P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 100f, target, true),
                P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 100f, target, true),
                P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 100f, target, true),
                P0BattleActionAffordancePresenter.BuildBedCare(true, true, new OwnerSleepState(), new TeamHungerGauge()),
                P0BattleActionAffordancePresenter.BuildLitterBox(true, false, new TeamPoopGauge()),
                P0BattleActionAffordancePresenter.BuildFeeder(true, false, new TeamHungerGauge())
            };
            P0BattleActionAffordance[] incomplete =
            {
                complete[0],
                complete[1],
                complete[3],
                complete[4],
                complete[5]
            };

            Assert.IsTrue(P0BattleActionAffordancePresenter.HasP0BattleActionAffordances(complete));
            Assert.IsFalse(P0BattleActionAffordancePresenter.HasP0BattleActionAffordances(incomplete));
            StringAssert.Contains("技能 3", P0BattleActionAffordancePresenter.BuildCompactSummary(complete));
            StringAssert.Contains("交互 3", P0BattleActionAffordancePresenter.BuildCompactSummary(complete));
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            return P0PrototypeCatalog.CreateStarterSkills().Single(skill => skill.Id == skillId);
        }

        private static P0SkillTargetResult CreateTarget(BattleEnemyState enemy)
        {
            return new P0SkillTargetResult(
                requiresEnemyTarget: true,
                enemy: enemy,
                distance: 1.5f,
                range: 2.35f);
        }

        private static BattleEnemyState CreateEnemy(string enemyId)
        {
            EnemyDefinition definition = P0PrototypeCatalog.CreateCoreEnemies().Single(enemy => enemy.Id == enemyId);
            return new BattleEnemyState(1, definition, 3f);
        }
    }
}
