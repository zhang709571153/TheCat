using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0SkillHudCoverageTests
    {
        [Test]
        public void BuildCard_ReadySkillShowsSlotTargetAndHunger()
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            P0SkillTargetResult target = CreateTarget();
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 80f, target, true);

            P0SkillHudCard card = P0SkillHudPresenter.BuildCard(skill, affordance, 0f, 80f, target);

            Assert.IsTrue(card.IsEnabled);
            Assert.AreEqual("S2", card.SlotToken);
            Assert.AreEqual("ready", card.StatusToken);
            Assert.AreEqual(0f, card.CooldownRatio, 0.001f);
            Assert.AreEqual(77f, card.HungerAfterCast, 0.001f);
            Assert.AreEqual(P0VisualAssetCatalog.SkillReadyFrameId, card.StatusVisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SkillHungerCostChipId, card.HungerCostVisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AutoTargetReticleId, card.TargetReticleAsset.AssetId);
            StringAssert.Contains("目标", card.TargetLabel);
            StringAssert.Contains("圆盾冲锋", card.BuildButtonLabel());
            StringAssert.Contains("饱肚", card.BuildSummary());
            Assert.IsFalse(card.BuildSummary().Contains("hunger"));
            Assert.IsFalse(card.BuildButtonLabel().Contains("Target"));
        }

        [Test]
        public void BuildCard_CooldownAndNoTargetStatesDisableCard()
        {
            SkillDefinition cooldownSkill = GetSkill("saiban_sword_sweep");
            P0BattleActionAffordance cooldownAffordance = P0BattleActionAffordancePresenter.BuildSkill(cooldownSkill, 3f, 80f, CreateTarget(), true);
            P0SkillHudCard cooldownCard = P0SkillHudPresenter.BuildCard(cooldownSkill, cooldownAffordance, 3f, 80f, CreateTarget());

            SkillDefinition targetSkill = GetSkill("nephthys_royal_mark");
            P0SkillTargetResult noTarget = new P0SkillTargetResult(true, null, 0f, 5.25f);
            P0BattleActionAffordance targetAffordance = P0BattleActionAffordancePresenter.BuildSkill(targetSkill, 0f, 80f, noTarget, true);
            P0SkillHudCard targetCard = P0SkillHudPresenter.BuildCard(targetSkill, targetAffordance, 0f, 80f, noTarget);

            Assert.IsFalse(cooldownCard.IsEnabled);
            Assert.AreEqual("cooldown", cooldownCard.StatusToken);
            Assert.AreEqual(0.5f, cooldownCard.CooldownRatio, 0.001f);
            Assert.AreEqual(P0VisualAssetCatalog.SkillCooldownOverlayId, cooldownCard.StatusVisualAsset.AssetId);
            Assert.IsFalse(targetCard.IsEnabled);
            Assert.AreEqual("no_target", targetCard.StatusToken);
            Assert.AreEqual(P0VisualAssetCatalog.SkillNoTargetMarkerId, targetCard.StatusVisualAsset.AssetId);
            Assert.IsFalse(targetCard.TargetReticleAsset.HasAsset);
            StringAssert.Contains("无目标 <= 5.3m", targetCard.TargetLabel);
        }

        [Test]
        public void EvaluatePrototypeCards_CompletesSkillHudCoverage()
        {
            P0SkillHudCoverageReport report = P0SkillHudCoverage.EvaluatePrototypeCards();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0SkillHudCoverage.ExpectedCoveredCardCount, report.CoveredCards.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("skill HUD coverage complete", report.BuildSummary());
            StringAssert.Contains("cooldown state", report.BuildDetailedSummary());
            StringAssert.Contains("low-hunger", report.BuildDetailedSummary());
        }

        [Test]
        public void SkillSelectionPresenter_BuildsPendingSelectedAndLockedStates()
        {
            RunProgressionState run = CreateRunWithPendingSmallSkill();
            CatUpgradeCandidate selected = FindStageOffer(run, CatUpgradeStage.SmallSkill);

            P0SkillSelectionSurface pending = P0SkillSelectionPresenter.BuildSurface(run, string.Empty);
            P0SkillSelectionSurface selectedSurface = P0SkillSelectionPresenter.BuildSurface(run, selected.Id);
            P0SkillSelectionChoiceCard locked = P0SkillSelectionPresenter.BuildLockedPreview(
                P0PrototypeCatalog.SaibanId,
                CatUpgradeStage.Ultimate);

            Assert.IsTrue(pending.HasPendingUpgrade);
            Assert.IsFalse(pending.CanConfirmSelection);
            Assert.GreaterOrEqual(pending.Choices.Count, 3);
            Assert.AreEqual("ready", pending.Choices[0].StateToken);
            Assert.IsTrue(P0SkillSelectionPresenter.HasP0SkillSelectionSurface(selectedSurface));
            Assert.IsTrue(selectedSurface.CanConfirmSelection);
            Assert.IsTrue(selectedSurface.TryGetChoice(selected.Id, out P0SkillSelectionChoiceCard selectedCard));
            Assert.AreEqual("selected", selectedCard.StateToken);
            Assert.IsTrue(selectedCard.CanConfirm);
            Assert.IsTrue(selectedCard.HasRuntimeSkill);
            StringAssert.Contains("确认", selectedSurface.ConfirmActionLabel);
            Assert.AreEqual("locked", locked.StateToken);
            Assert.IsFalse(locked.CanConfirm);
            Assert.IsFalse(selectedCard.BuildButtonLabel().Contains("cat_upgrade"));
            Assert.IsFalse(selectedCard.BuildButtonLabel().Contains("_"));
        }

        [Test]
        public void EvaluateSkillSelectionAcceptance_CompletesD2Contract()
        {
            P0SkillSelectionAcceptanceReport report = P0SkillSelectionAcceptanceCoverage.EvaluatePrototypeSkillSelection();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0SkillSelectionAcceptanceCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("skill-selection acceptance complete", report.BuildSummary());
            StringAssert.Contains("Batch 89 boundary", report.BuildDetailedSummary());
            StringAssert.Contains("ultimate", report.BuildDetailedSummary());
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            foreach (SkillDefinition skill in P0PrototypeCatalog.CreateStarterSkills())
            {
                if (skill.Id == skillId)
                {
                    return skill;
                }
            }

            Assert.Fail("Missing skill: " + skillId);
            return null;
        }

        private static RunProgressionState CreateRunWithPendingSmallSkill()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            CatUpgradeCandidate passive = FindStageOffer(run, CatUpgradeStage.Passive);
            Assert.IsTrue(run.CatUpgrades.TrySelect(passive.Id, run.Roster, out CatUpgradeCandidate _));
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            return run;
        }

        private static CatUpgradeCandidate FindStageOffer(RunProgressionState run, CatUpgradeStage stage)
        {
            foreach (CatUpgradeCandidate candidate in run.CatUpgrades.CreateCurrentOffer(run.Roster))
            {
                if (candidate.Stage == stage)
                {
                    return candidate;
                }
            }

            Assert.Fail("Missing upgrade stage: " + stage);
            return default(CatUpgradeCandidate);
        }

        private static P0SkillTargetResult CreateTarget()
        {
            EnemyDefinition enemy = null;
            foreach (EnemyDefinition candidate in P0PrototypeCatalog.CreateCoreEnemies())
            {
                if (candidate.Id == P0PrototypeCatalog.BlackMudNightmareId)
                {
                    enemy = candidate;
                    break;
                }
            }

            Assert.IsNotNull(enemy);
            return new P0SkillTargetResult(true, new BattleEnemyState(1, enemy, 3f), 1.5f, 2.35f);
        }
    }
}
