using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0SkillHudCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0SkillHudCoverageIssue
    {
        public P0SkillHudCoverageIssue(P0SkillHudCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0SkillHudCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0SkillHudCoverageReport
    {
        private readonly List<P0SkillHudCoverageIssue> issues = new List<P0SkillHudCoverageIssue>();
        private readonly List<string> coveredCards = new List<string>();

        public IReadOnlyList<P0SkillHudCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredCards => coveredCards.AsReadOnly();

        public int FailureCount => Count(P0SkillHudCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredCards.Count >= P0SkillHudCoverage.ExpectedCoveredCardCount;

        public void AddIssue(P0SkillHudCoverageSeverity severity, string message)
        {
            issues.Add(new P0SkillHudCoverageIssue(severity, message));
        }

        public void AddCoveredCard(string card)
        {
            if (!string.IsNullOrWhiteSpace(card))
            {
                coveredCards.Add(card);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 skill HUD coverage complete for " + coveredCards.Count + " card check(s)."
                : "P0 skill HUD coverage has " + FailureCount + " failure(s) across " + coveredCards.Count + " card check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredCards.Count; i++)
            {
                lines.Add("- " + coveredCards[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0SkillHudCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0SkillHudCoverage
    {
        public const int ExpectedCoveredCardCount = 5;

        public static P0SkillHudCoverageReport EvaluatePrototypeCards()
        {
            P0SkillHudCoverageReport report = new P0SkillHudCoverageReport();

            EvaluateReadyCards(report);
            EvaluateCooldownCard(report);
            EvaluateNoTargetCard(report);
            EvaluateLowHungerCard(report);
            EvaluateCompactSummary(report);

            return report;
        }

        private static void EvaluateReadyCards(P0SkillHudCoverageReport report)
        {
            IReadOnlyList<P0SkillHudCard> cards = BuildSaibanCards(cooldownSeconds: 0f, hunger: 100f, target: CreateTarget());

            Require(
                report,
                P0SkillHudPresenter.HasP0SkillHudCards(cards)
                && cards[0].SlotToken == "S1"
                && cards[1].SlotToken == "S2"
                && cards[2].SlotToken == "ULT"
                && cards[0].StatusToken == "ready"
                && cards[0].StatusVisualAsset.AssetId == P0VisualAssetCatalog.SkillReadyFrameId
                && cards[1].TargetReticleAsset.AssetId == P0VisualAssetCatalog.AutoTargetReticleId
                && cards[1].HungerCostVisualAsset.AssetId == P0VisualAssetCatalog.SkillHungerCostChipId
                && cards[1].TargetLabel.Contains("目标"),
                "Skill HUD cards cover S1/S2/ULT ready states, target labels, and Batch 60 state visuals.",
                "Skill HUD cards did not expose ready slot, target state, and Batch 60 visuals.");
        }

        private static void EvaluateCooldownCard(P0SkillHudCoverageReport report)
        {
            SkillDefinition skill = GetSkill("saiban_sword_sweep");
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(skill, 3f, 80f, CreateTarget(), true);
            P0SkillHudCard card = P0SkillHudPresenter.BuildCard(skill, affordance, 3f, 80f, CreateTarget());

            Require(
                report,
                card.StatusToken == "cooldown"
                && card.IsCoolingDown
                && card.CooldownRatio > 0.49f
                && card.CooldownRatio < 0.51f
                && !card.IsEnabled
                && card.StatusVisualAsset.AssetId == P0VisualAssetCatalog.SkillCooldownOverlayId
                && card.BuildButtonLabel().Contains("3.0s"),
                "Skill HUD card shows cooldown state, normalized cooldown fill, and Batch 60 cooldown visual.",
                "Skill HUD card did not expose cooldown state and visual.");
        }

        private static void EvaluateNoTargetCard(P0SkillHudCoverageReport report)
        {
            SkillDefinition skill = GetSkill("nephthys_royal_mark");
            P0SkillTargetResult target = new P0SkillTargetResult(true, null, 0f, 5.25f);
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 80f, target, true);
            P0SkillHudCard card = P0SkillHudPresenter.BuildCard(skill, affordance, 0f, 80f, target);

            Require(
                report,
                card.StatusToken == "no_target"
                && card.HasTargetIssue
                && !card.IsEnabled
                && card.StatusVisualAsset.AssetId == P0VisualAssetCatalog.SkillNoTargetMarkerId
                && card.TargetLabel.Contains("无目标 <= 5.3m"),
                "Skill HUD card surfaces missing target range, disabled state, and Batch 60 no-target visual.",
                "Skill HUD card did not expose missing target state and visual.");
        }

        private static void EvaluateLowHungerCard(P0SkillHudCoverageReport report)
        {
            SkillDefinition skill = GetSkill("saiban_oath_shield");
            P0SkillTargetResult target = new P0SkillTargetResult(false, null, 0f, 0f);
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(skill, 0f, 1f, target, true);
            P0SkillHudCard card = P0SkillHudPresenter.BuildCard(skill, affordance, 0f, 1f, target);

            Require(
                report,
                card.StatusToken == "low_hunger"
                && card.IsLowHunger
                && card.IsEnabled
                && card.HungerAfterCast <= 0f
                && card.StatusVisualAsset.AssetId == P0VisualAssetCatalog.SkillHungerCostChipId
                && card.HungerCostVisualAsset.AssetId == P0VisualAssetCatalog.SkillHungerCostChipId
                && card.BuildSummary().Contains("饱肚 1->0"),
                "Skill HUD card keeps low-hunger P0 light-penalty skills visible, castable, and visually marked.",
                "Skill HUD card did not expose low-hunger castability and visual.");
        }

        private static void EvaluateCompactSummary(P0SkillHudCoverageReport report)
        {
            List<P0SkillHudCard> cards = new List<P0SkillHudCard>
            {
                BuildSingleCard(GetSkill("saiban_oath_shield"), 0f, 1f, new P0SkillTargetResult(false, null, 0f, 0f)),
                BuildSingleCard(GetSkill("saiban_sword_sweep"), 3f, 80f, CreateTarget()),
                BuildSingleCard(GetSkill("nephthys_royal_mark"), 0f, 80f, new P0SkillTargetResult(true, null, 0f, 5.25f))
            };
            string summary = P0SkillHudPresenter.BuildCompactSummary(cards);

            Require(
                report,
                summary.Contains("3 可用 1")
                && summary.Contains("冷却 1")
                && summary.Contains("目标问题 1")
                && summary.Contains("低饱肚 1"),
                "Skill HUD compact summary reports enabled, cooldown, target issue, and low-hunger totals.",
                "Skill HUD compact summary did not report required totals.");
        }

        private static IReadOnlyList<P0SkillHudCard> BuildSaibanCards(float cooldownSeconds, float hunger, P0SkillTargetResult target)
        {
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
            List<P0SkillHudCard> cards = new List<P0SkillHudCard>();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].OwnerCatId != P0PrototypeCatalog.SaibanId)
                {
                    continue;
                }

                float cooldown = skills[i].Id == "saiban_sword_sweep" ? cooldownSeconds : 0f;
                P0SkillTargetResult cardTarget = P0SkillTargetResolver.RequiresEnemyTarget(skills[i])
                    ? target
                    : new P0SkillTargetResult(false, null, 0f, 0f);
                cards.Add(BuildSingleCard(skills[i], cooldown, hunger, cardTarget));
            }

            return cards.AsReadOnly();
        }

        private static P0SkillHudCard BuildSingleCard(
            SkillDefinition skill,
            float cooldown,
            float hunger,
            P0SkillTargetResult target)
        {
            P0BattleActionAffordance affordance = P0BattleActionAffordancePresenter.BuildSkill(skill, cooldown, hunger, target, true);
            return P0SkillHudPresenter.BuildCard(skill, affordance, cooldown, hunger, target);
        }

        private static P0SkillTargetResult CreateTarget()
        {
            return new P0SkillTargetResult(
                true,
                new BattleEnemyState(1, GetEnemy(P0PrototypeCatalog.BlackMudNightmareId), 4f),
                1.5f,
                2.35f);
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Id == skillId)
                {
                    return skills[i];
                }
            }

            throw new InvalidOperationException("Missing skill: " + skillId);
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

            throw new InvalidOperationException("Missing enemy: " + enemyId);
        }

        private static void Require(
            P0SkillHudCoverageReport report,
            bool condition,
            string coveredCard,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCard(coveredCard);
                return;
            }

            report.AddIssue(P0SkillHudCoverageSeverity.Failure, failureMessage);
        }
    }

    public enum P0SkillSelectionAcceptanceSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0SkillSelectionAcceptanceIssue
    {
        public P0SkillSelectionAcceptanceIssue(P0SkillSelectionAcceptanceSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0SkillSelectionAcceptanceSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0SkillSelectionAcceptanceReport
    {
        private readonly List<P0SkillSelectionAcceptanceIssue> issues = new List<P0SkillSelectionAcceptanceIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0SkillSelectionAcceptanceIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0SkillSelectionAcceptanceSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0SkillSelectionAcceptanceCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0SkillSelectionAcceptanceSeverity severity, string message)
        {
            issues.Add(new P0SkillSelectionAcceptanceIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 skill-selection acceptance complete for " + coveredChecks.Count + " check(s)."
                : "P0 skill-selection acceptance has " + FailureCount + " failure(s) across " + coveredChecks.Count + " covered check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0SkillSelectionAcceptanceSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0SkillSelectionAcceptanceCoverage
    {
        public const int ExpectedCoveredCheckCount = 6;

        public static P0SkillSelectionAcceptanceReport EvaluatePrototypeSkillSelection()
        {
            P0SkillSelectionAcceptanceReport report = new P0SkillSelectionAcceptanceReport();

            EvaluatePendingOfferSurface(report);
            EvaluateSelectedConfirmSurface(report);
            EvaluateSmallSkillRuntimeMapping(report);
            EvaluateUltimateRuntimeMapping(report);
            EvaluateLockedAndEmptyStates(report);
            EvaluateBatch89Boundary(report);

            return report;
        }

        private static void EvaluatePendingOfferSurface(P0SkillSelectionAcceptanceReport report)
        {
            RunProgressionState run = CreateRunWithPendingPassive();
            P0SkillSelectionSurface surface = P0SkillSelectionPresenter.BuildSurface(run, string.Empty);

            Require(
                report,
                surface.HasPendingUpgrade
                && !surface.HasSelectedChoice
                && !surface.CanConfirmSelection
                && surface.Choices.Count >= 3
                && CountState(surface, "ready") == surface.Choices.Count
                && !ContainsInternalToken(surface),
                "Skill-selection pending surface shows at least three ready choices and keeps confirm disabled until selection.",
                "Skill-selection pending surface is missing ready choices, selection gating, or clean labels.");
        }

        private static void EvaluateSelectedConfirmSurface(P0SkillSelectionAcceptanceReport report)
        {
            RunProgressionState run = CreateRunWithPendingSmallSkill();
            CatUpgradeCandidate selected = FindStageOffer(run, CatUpgradeStage.SmallSkill);
            P0SkillSelectionSurface surface = P0SkillSelectionPresenter.BuildSurface(run, selected.Id);

            Require(
                report,
                P0SkillSelectionPresenter.HasP0SkillSelectionSurface(surface)
                && surface.HasSelectedChoice
                && surface.CanConfirmSelection
                && surface.ConfirmActionLabel.Contains("确认")
                && CountState(surface, "selected") == 1
                && CountState(surface, "disabled") >= 1
                && surface.TryGetChoice(selected.Id, out P0SkillSelectionChoiceCard card)
                && card.CanConfirm
                && card.HasRuntimeSkill,
                "Skill-selection selected surface exposes one selected card, disabled alternatives, and a confirm CTA.",
                "Skill-selection selected surface failed selected/disabled/confirm semantics.");
        }

        private static void EvaluateSmallSkillRuntimeMapping(P0SkillSelectionAcceptanceReport report)
        {
            RunProgressionState run = CreateRunWithPendingSmallSkill();
            CatUpgradeCandidate selected = FindStageOffer(run, CatUpgradeStage.SmallSkill);
            SkillDefinition mappedSkill = P0SkillSelectionPresenter.ResolveUnlockedSkill(selected);
            bool selectedUpgrade = run.CatUpgrades.TrySelect(selected.Id, run.Roster, out CatUpgradeCandidate resolved);
            IReadOnlyList<SkillDefinition> selectedSkills = P0CatUpgradeRuntimeCatalog.CreateSelectedSkillDefinitions(run.CatUpgrades);

            Require(
                report,
                mappedSkill != null
                && P0CatUpgradeRuntimeCatalog.GetUnlockingUpgradeId(mappedSkill.Id) == selected.Id
                && selectedUpgrade
                && resolved.Id == selected.Id
                && selectedSkills.Count >= 1
                && HasSkill(selectedSkills, mappedSkill.Id),
                "Skill-selection small-skill choice maps through P0SkillPresenter and RunCatUpgradeState.TrySelect without combat-rule changes.",
                "Skill-selection small-skill runtime mapping failed.");
        }

        private static void EvaluateUltimateRuntimeMapping(P0SkillSelectionAcceptanceReport report)
        {
            RunProgressionState run = CreateRunWithPendingUltimate();
            CatUpgradeCandidate selected = FindStageOffer(run, CatUpgradeStage.Ultimate);
            P0SkillSelectionSurface surface = P0SkillSelectionPresenter.BuildSurface(run, selected.Id);
            SkillDefinition mappedSkill = P0SkillSelectionPresenter.ResolveUnlockedSkill(selected);
            bool selectedUpgrade = run.CatUpgrades.TrySelect(selected.Id, run.Roster, out CatUpgradeCandidate resolved);
            IReadOnlyList<SkillDefinition> selectedSkills = P0CatUpgradeRuntimeCatalog.CreateSelectedSkillDefinitions(run.CatUpgrades);

            Require(
                report,
                mappedSkill != null
                && surface.TryGetChoice(selected.Id, out P0SkillSelectionChoiceCard card)
                && card.StageLabel == selected.StageLabel
                && card.HasRuntimeSkill
                && selectedUpgrade
                && resolved.Stage == CatUpgradeStage.Ultimate
                && HasSkill(selectedSkills, mappedSkill.Id),
                "Skill-selection ultimate choice keeps ultimate stage distinct and maps to unlocked runtime skill definitions.",
                "Skill-selection ultimate runtime mapping failed.");
        }

        private static void EvaluateLockedAndEmptyStates(P0SkillSelectionAcceptanceReport report)
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            P0SkillSelectionSurface empty = P0SkillSelectionPresenter.BuildSurface(run, string.Empty);
            P0SkillSelectionChoiceCard locked = P0SkillSelectionPresenter.BuildLockedPreview(P0PrototypeCatalog.SaibanId, CatUpgradeStage.Ultimate);

            Require(
                report,
                !empty.HasPendingUpgrade
                && !empty.CanConfirmSelection
                && empty.Choices.Count == 0
                && locked.StateToken == "locked"
                && !locked.CanConfirm
                && !locked.BuildButtonLabel().Contains("cat_upgrade")
                && !locked.BuildButtonLabel().Contains("_"),
                "Skill-selection empty and locked states are explicit, disabled, and do not expose internal ids.",
                "Skill-selection empty or locked states are ambiguous or expose internal ids.");
        }

        private static void EvaluateBatch89Boundary(P0SkillSelectionAcceptanceReport report)
        {
            RunProgressionState run = CreateRunWithPendingSmallSkill();
            CatUpgradeCandidate selected = FindStageOffer(run, CatUpgradeStage.SmallSkill);
            P0SkillSelectionSurface surface = P0SkillSelectionPresenter.BuildSurface(run, selected.Id);

            Require(
                report,
                !surface.BuildSummary().Contains("batch_89")
                && !surface.BuildSummary().Contains(".png")
                && !surface.BuildSummary().Contains(".meta")
                && !surface.BuildSummary().Contains("Sprite")
                && !ContainsInternalToken(surface),
                "Batch 89 boundary stays candidate-only: D2 skill-selection acceptance has no runtime asset import requirement.",
                "Batch 89 candidate assets leaked into skill-selection acceptance.");
        }

        private static RunProgressionState CreateRunWithPendingPassive()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            return run;
        }

        private static RunProgressionState CreateRunWithPendingSmallSkill()
        {
            RunProgressionState run = CreateRunWithPendingPassive();
            CatUpgradeCandidate passive = FindStageOffer(run, CatUpgradeStage.Passive);
            run.CatUpgrades.TrySelect(passive.Id, run.Roster, out CatUpgradeCandidate _);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            return run;
        }

        private static RunProgressionState CreateRunWithPendingUltimate()
        {
            RunProgressionState run = CreateRunWithPendingSmallSkill();
            CatUpgradeCandidate small = FindStageOffer(run, CatUpgradeStage.SmallSkill);
            run.CatUpgrades.TrySelect(small.Id, run.Roster, out CatUpgradeCandidate _);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            return run;
        }

        private static CatUpgradeCandidate FindStageOffer(RunProgressionState run, CatUpgradeStage stage)
        {
            IReadOnlyList<CatUpgradeCandidate> offer = run.CatUpgrades.CreateCurrentOffer(run.Roster);
            for (int i = 0; i < offer.Count; i++)
            {
                if (offer[i].Stage == stage)
                {
                    return offer[i];
                }
            }

            throw new InvalidOperationException("Missing upgrade stage: " + stage);
        }

        private static int CountState(P0SkillSelectionSurface surface, string stateToken)
        {
            int count = 0;
            for (int i = 0; i < surface.Choices.Count; i++)
            {
                if (surface.Choices[i].StateToken == stateToken)
                {
                    count++;
                }
            }

            return count;
        }

        private static bool HasSkill(IReadOnlyList<SkillDefinition> skills, string skillId)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Id == skillId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsInternalToken(P0SkillSelectionSurface surface)
        {
            if (ContainsInternalToken(surface.BuildSummary()))
            {
                return true;
            }

            for (int i = 0; i < surface.Choices.Count; i++)
            {
                if (ContainsInternalToken(surface.Choices[i].BuildButtonLabel()))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsInternalToken(string text)
        {
            return (text ?? string.Empty).Contains("cat_upgrade")
                || (text ?? string.Empty).Contains("saiban")
                || (text ?? string.Empty).Contains("nephthys")
                || (text ?? string.Empty).Contains("suzune")
                || (text ?? string.Empty).Contains("_");
        }

        private static void Require(
            P0SkillSelectionAcceptanceReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0SkillSelectionAcceptanceSeverity.Failure, failureMessage);
        }
    }
}
