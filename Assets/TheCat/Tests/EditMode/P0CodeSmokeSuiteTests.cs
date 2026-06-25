using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0CodeSmokeSuiteTests
    {
        [Test]
        public void EvaluatePrototypeBuild_PassesCurrentCodeSmokeSuite()
        {
            P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.EvaluatePrototypeBuild();

            Assert.IsTrue(report.IsPassed, report.BuildDetailedSummary());
            Assert.AreEqual(28, report.Checks.Count);
            Assert.AreEqual(0, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.GoldenPathSimulationCheckId, out P0CodeSmokeSuiteCheck golden));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, golden.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.CharacterDesignCoverageCheckId, out P0CodeSmokeSuiteCheck characterDesign));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, characterDesign.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.AssetManifestCoverageCheckId, out P0CodeSmokeSuiteCheck assetManifest));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, assetManifest.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.AssetGenerationBatchCoverageCheckId, out P0CodeSmokeSuiteCheck assetBatches));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, assetBatches.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.AssetImportReadinessCheckId, out P0CodeSmokeSuiteCheck assetImport));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, assetImport.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.AssetMetaImportSettingsCheckId, out P0CodeSmokeSuiteCheck assetMetaImport));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, assetMetaImport.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.RuntimeVisualBindingCoverageCheckId, out P0CodeSmokeSuiteCheck runtimeVisualBindings));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, runtimeVisualBindings.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.AssetReviewPacketCheckId, out P0CodeSmokeSuiteCheck assetReviewPacket));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, assetReviewPacket.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.StarterCatTurnaroundSourceLockCheckId, out P0CodeSmokeSuiteCheck starterCatLocks));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, starterCatLocks.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.HardReferenceSourceLockCheckId, out P0CodeSmokeSuiteCheck hardReferenceLocks));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, hardReferenceLocks.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.StatusTagCoverageCheckId, out P0CodeSmokeSuiteCheck status));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, status.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.StatusHudCoverageCheckId, out P0CodeSmokeSuiteCheck statusHud));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, statusHud.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.MainMenuCoverageCheckId, out P0CodeSmokeSuiteCheck mainMenu));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, mainMenu.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.RouteChoiceCoverageCheckId, out P0CodeSmokeSuiteCheck routeChoices));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, routeChoices.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.RouteMapInputCoverageCheckId, out P0CodeSmokeSuiteCheck routeMapInput));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, routeMapInput.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.RouteMapSurfaceCoverageCheckId, out P0CodeSmokeSuiteCheck routeMapSurface));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, routeMapSurface.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.RuntimeSettingsCoverageCheckId, out P0CodeSmokeSuiteCheck runtimeSettings));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, runtimeSettings.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.ChineseUiCoverageCheckId, out P0CodeSmokeSuiteCheck chineseUi));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, chineseUi.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.ChineseUiScaleValidationCheckId, out P0CodeSmokeSuiteCheck chineseUiScale));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, chineseUiScale.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.EnemyHudCoverageCheckId, out P0CodeSmokeSuiteCheck enemyHud));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, enemyHud.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.CatHudCoverageCheckId, out P0CodeSmokeSuiteCheck catHud));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, catHud.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.SkillHudCoverageCheckId, out P0CodeSmokeSuiteCheck skillHud));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, skillHud.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.BattleFeedbackCoverageCheckId, out P0CodeSmokeSuiteCheck battleFeedback));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, battleFeedback.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.BattleFeedbackVisualCoverageCheckId, out P0CodeSmokeSuiteCheck battleFeedbackVisuals));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, battleFeedbackVisuals.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.BattleResultCoverageCheckId, out P0CodeSmokeSuiteCheck battleResult));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, battleResult.State);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.GrayboxTelemetryCheckId, out P0CodeSmokeSuiteCheck telemetry));
            Assert.AreEqual(P0CodeSmokeSuiteState.Passed, telemetry.State);
        }

        [Test]
        public void Evaluate_MissingReportsFailAllChecks()
        {
            P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.Evaluate(
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            Assert.IsFalse(report.IsPassed);
            Assert.AreEqual(28, report.FailureCount);
            StringAssert.Contains("missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_AcceptedReportWithWarningsKeepsSuitePassing()
        {
            P0GoldenPathReport golden = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(golden);
            acceptance.Add(P0GoldenPathAcceptanceSeverity.Warning, "Injected tuning warning.");
            P0CharacterDesignCoverageReport characterDesign = P0CharacterDesignCoverage.EvaluatePrototypeRoster();
            P0AssetManifestCoverageReport assetManifest = P0AssetManifestCoverage.EvaluateP0Manifest();
            P0AssetGenerationBatchCoverageReport assetBatches = P0AssetGenerationBatchCoverage.EvaluateP0Batches();
            P0AssetImportReadinessReport assetImport = P0AssetImportReadiness.EvaluateP0Manifest(IsGeneratedAcceptedAssetPath);
            P0AssetMetaImportSettingsReport assetMetaImport = P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest();
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings = P0RuntimeVisualBindingCoverage.Evaluate(P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true);
            P0AssetReviewPacketReport assetReviewPacket = P0AssetReviewPacket.Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), P0HardReferenceSourceLocks.CreateP0Locks(), P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true);
            P0StarterCatTurnaroundSourceLockReport starterCatSourceLocks = P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks();
            P0HardReferenceSourceLockReport hardReferenceSourceLocks = P0HardReferenceSourceLocks.EvaluateP0Locks();
            P0StatusTagCoverageReport statusTags = P0StatusTagCoverage.EvaluatePrototypeCatalog();
            P0StatusHudCoverageReport statusHud = P0StatusHudCoverage.EvaluatePrototypeHud();
            P0MainMenuCoverageReport mainMenu = P0MainMenuCoverage.EvaluatePrototypeSurface();
            P0RouteChoiceCoverageReport routeChoices = P0RouteChoiceCoverage.EvaluatePrototypeRoute();
            P0RouteMapInputCoverageReport routeMapInput = P0RouteMapInputCoverage.EvaluatePrototypeRouteMap();
            P0RouteMapSurfaceCoverageReport routeMapSurface = P0RouteMapSurfaceCoverage.EvaluatePrototypeRouteMap();
            P0RuntimeSettingsCoverageReport runtimeSettings = P0RuntimeSettingsCoverage.EvaluatePrototypeSettings();
            P0ChineseUiCoverageReport chineseUi = P0ChineseUiCoverage.EvaluatePrototypeUi();
            P0ChineseUiScaleValidationReport chineseUiScale = P0ChineseUiScaleValidationPlan.EvaluateCurrentPlan();
            P0EnemyHudCoverageReport enemyHud = P0EnemyHudCoverage.EvaluatePrototypeCards();
            P0CatHudCoverageReport catHud = P0CatHudCoverage.EvaluatePrototypeCards();
            P0SkillHudCoverageReport skillHud = P0SkillHudCoverage.EvaluatePrototypeCards();
            P0BattleFeedbackCoverageReport battleFeedback = P0BattleFeedbackCoverage.EvaluatePrototypeFeedback();
            P0BattleFeedbackVisualCoverageReport battleFeedbackVisuals = P0BattleFeedbackVisualCoverage.EvaluatePrototypeVisuals();
            P0BattleResultCoverageReport battleResult = P0BattleResultCoverage.EvaluatePrototypeSurface();
            P0PlayableReadinessReport readiness = P0PlayableReadiness.EvaluatePrototypeBuild();
            P0GrayboxTelemetryReport telemetry = P0GrayboxTelemetry.Evaluate(golden);

            P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.Evaluate(
                golden,
                acceptance,
                characterDesign,
                assetManifest,
                assetBatches,
                assetImport,
                assetMetaImport,
                runtimeVisualBindings,
                assetReviewPacket,
                starterCatSourceLocks,
                hardReferenceSourceLocks,
                statusTags,
                statusHud,
                mainMenu,
                routeChoices,
                routeMapInput,
                routeMapSurface,
                runtimeSettings,
                chineseUi,
                chineseUiScale,
                enemyHud,
                catHud,
                skillHud,
                battleFeedback,
                battleFeedbackVisuals,
                battleResult,
                readiness,
                telemetry);

            Assert.IsTrue(report.IsPassed, report.BuildDetailedSummary());
            Assert.Greater(report.WarningCount, 0);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.GoldenPathAcceptanceCheckId, out P0CodeSmokeSuiteCheck check));
            Assert.AreEqual(P0CodeSmokeSuiteState.Warning, check.State);
        }

        [Test]
        public void Evaluate_FailedStatusCoverageFailsSuite()
        {
            P0GoldenPathReport golden = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(golden);
            P0CharacterDesignCoverageReport characterDesign = P0CharacterDesignCoverage.EvaluatePrototypeRoster();
            P0AssetManifestCoverageReport assetManifest = P0AssetManifestCoverage.EvaluateP0Manifest();
            P0AssetGenerationBatchCoverageReport assetBatches = P0AssetGenerationBatchCoverage.EvaluateP0Batches();
            P0AssetImportReadinessReport assetImport = P0AssetImportReadiness.EvaluateP0Manifest(IsGeneratedAcceptedAssetPath);
            P0AssetMetaImportSettingsReport assetMetaImport = P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest();
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings = P0RuntimeVisualBindingCoverage.Evaluate(P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true);
            P0AssetReviewPacketReport assetReviewPacket = P0AssetReviewPacket.Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), P0HardReferenceSourceLocks.CreateP0Locks(), P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true);
            P0StarterCatTurnaroundSourceLockReport starterCatSourceLocks = P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks();
            P0HardReferenceSourceLockReport hardReferenceSourceLocks = P0HardReferenceSourceLocks.EvaluateP0Locks();
            P0StatusTagCoverageReport statusTags = P0StatusTagCoverage.EvaluatePrototypeCatalog();
            statusTags.AddIssue(P0StatusTagCoverageSeverity.Failure, "Injected missing tag.");
            P0StatusHudCoverageReport statusHud = P0StatusHudCoverage.EvaluatePrototypeHud();
            P0MainMenuCoverageReport mainMenu = P0MainMenuCoverage.EvaluatePrototypeSurface();
            P0RouteChoiceCoverageReport routeChoices = P0RouteChoiceCoverage.EvaluatePrototypeRoute();
            P0RouteMapInputCoverageReport routeMapInput = P0RouteMapInputCoverage.EvaluatePrototypeRouteMap();
            P0RouteMapSurfaceCoverageReport routeMapSurface = P0RouteMapSurfaceCoverage.EvaluatePrototypeRouteMap();
            P0RuntimeSettingsCoverageReport runtimeSettings = P0RuntimeSettingsCoverage.EvaluatePrototypeSettings();
            P0ChineseUiCoverageReport chineseUi = P0ChineseUiCoverage.EvaluatePrototypeUi();
            P0ChineseUiScaleValidationReport chineseUiScale = P0ChineseUiScaleValidationPlan.EvaluateCurrentPlan();
            P0EnemyHudCoverageReport enemyHud = P0EnemyHudCoverage.EvaluatePrototypeCards();
            P0CatHudCoverageReport catHud = P0CatHudCoverage.EvaluatePrototypeCards();
            P0SkillHudCoverageReport skillHud = P0SkillHudCoverage.EvaluatePrototypeCards();
            P0BattleFeedbackCoverageReport battleFeedback = P0BattleFeedbackCoverage.EvaluatePrototypeFeedback();
            P0BattleFeedbackVisualCoverageReport battleFeedbackVisuals = P0BattleFeedbackVisualCoverage.EvaluatePrototypeVisuals();
            P0BattleResultCoverageReport battleResult = P0BattleResultCoverage.EvaluatePrototypeSurface();
            P0PlayableReadinessReport readiness = P0PlayableReadiness.EvaluatePrototypeBuild();
            P0GrayboxTelemetryReport telemetry = P0GrayboxTelemetry.Evaluate(golden);

            P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.Evaluate(
                golden,
                acceptance,
                characterDesign,
                assetManifest,
                assetBatches,
                assetImport,
                assetMetaImport,
                runtimeVisualBindings,
                assetReviewPacket,
                starterCatSourceLocks,
                hardReferenceSourceLocks,
                statusTags,
                statusHud,
                mainMenu,
                routeChoices,
                routeMapInput,
                routeMapSurface,
                runtimeSettings,
                chineseUi,
                chineseUiScale,
                enemyHud,
                catHud,
                skillHud,
                battleFeedback,
                battleFeedbackVisuals,
                battleResult,
                readiness,
                telemetry);

            Assert.IsFalse(report.IsPassed);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.StatusTagCoverageCheckId, out P0CodeSmokeSuiteCheck check));
            Assert.AreEqual(P0CodeSmokeSuiteState.Failed, check.State);
        }

        [Test]
        public void Evaluate_FailedTelemetryFailsSuite()
        {
            P0GoldenPathReport golden = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(golden);
            P0CharacterDesignCoverageReport characterDesign = P0CharacterDesignCoverage.EvaluatePrototypeRoster();
            P0AssetManifestCoverageReport assetManifest = P0AssetManifestCoverage.EvaluateP0Manifest();
            P0AssetGenerationBatchCoverageReport assetBatches = P0AssetGenerationBatchCoverage.EvaluateP0Batches();
            P0AssetImportReadinessReport assetImport = P0AssetImportReadiness.EvaluateP0Manifest(IsGeneratedAcceptedAssetPath);
            P0AssetMetaImportSettingsReport assetMetaImport = P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest();
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings = P0RuntimeVisualBindingCoverage.Evaluate(P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true);
            P0AssetReviewPacketReport assetReviewPacket = P0AssetReviewPacket.Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), P0HardReferenceSourceLocks.CreateP0Locks(), P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true);
            P0StarterCatTurnaroundSourceLockReport starterCatSourceLocks = P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks();
            P0HardReferenceSourceLockReport hardReferenceSourceLocks = P0HardReferenceSourceLocks.EvaluateP0Locks();
            P0StatusTagCoverageReport statusTags = P0StatusTagCoverage.EvaluatePrototypeCatalog();
            P0StatusHudCoverageReport statusHud = P0StatusHudCoverage.EvaluatePrototypeHud();
            P0MainMenuCoverageReport mainMenu = P0MainMenuCoverage.EvaluatePrototypeSurface();
            P0RouteChoiceCoverageReport routeChoices = P0RouteChoiceCoverage.EvaluatePrototypeRoute();
            P0RouteMapInputCoverageReport routeMapInput = P0RouteMapInputCoverage.EvaluatePrototypeRouteMap();
            P0RouteMapSurfaceCoverageReport routeMapSurface = P0RouteMapSurfaceCoverage.EvaluatePrototypeRouteMap();
            P0RuntimeSettingsCoverageReport runtimeSettings = P0RuntimeSettingsCoverage.EvaluatePrototypeSettings();
            P0ChineseUiCoverageReport chineseUi = P0ChineseUiCoverage.EvaluatePrototypeUi();
            P0ChineseUiScaleValidationReport chineseUiScale = P0ChineseUiScaleValidationPlan.EvaluateCurrentPlan();
            P0EnemyHudCoverageReport enemyHud = P0EnemyHudCoverage.EvaluatePrototypeCards();
            P0CatHudCoverageReport catHud = P0CatHudCoverage.EvaluatePrototypeCards();
            P0SkillHudCoverageReport skillHud = P0SkillHudCoverage.EvaluatePrototypeCards();
            P0BattleFeedbackCoverageReport battleFeedback = P0BattleFeedbackCoverage.EvaluatePrototypeFeedback();
            P0BattleFeedbackVisualCoverageReport battleFeedbackVisuals = P0BattleFeedbackVisualCoverage.EvaluatePrototypeVisuals();
            P0BattleResultCoverageReport battleResult = P0BattleResultCoverage.EvaluatePrototypeSurface();
            P0PlayableReadinessReport readiness = P0PlayableReadiness.EvaluatePrototypeBuild();
            P0GrayboxTelemetryReport telemetry = P0GrayboxTelemetry.Evaluate((RunProgressionState)null);

            P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.Evaluate(
                golden,
                acceptance,
                characterDesign,
                assetManifest,
                assetBatches,
                assetImport,
                assetMetaImport,
                runtimeVisualBindings,
                assetReviewPacket,
                starterCatSourceLocks,
                hardReferenceSourceLocks,
                statusTags,
                statusHud,
                mainMenu,
                routeChoices,
                routeMapInput,
                routeMapSurface,
                runtimeSettings,
                chineseUi,
                chineseUiScale,
                enemyHud,
                catHud,
                skillHud,
                battleFeedback,
                battleFeedbackVisuals,
                battleResult,
                readiness,
                telemetry);

            Assert.IsFalse(report.IsPassed);
            Assert.IsTrue(report.TryGetCheck(P0CodeSmokeSuite.GrayboxTelemetryCheckId, out P0CodeSmokeSuiteCheck check));
            Assert.AreEqual(P0CodeSmokeSuiteState.Failed, check.State);
        }

        [Test]
        public void BuildDetailedSummary_ListsAllGateNames()
        {
            P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.EvaluatePrototypeBuild();

            string summary = report.BuildDetailedSummary();

            StringAssert.Contains("Golden Path Simulation", summary);
            StringAssert.Contains("Golden Path Acceptance", summary);
            StringAssert.Contains("Character Design Coverage", summary);
            StringAssert.Contains("Asset Manifest Coverage", summary);
            StringAssert.Contains("Asset Generation Batch Coverage", summary);
            StringAssert.Contains("Asset Import Readiness", summary);
            StringAssert.Contains("Asset Meta Import Settings", summary);
            StringAssert.Contains("Runtime Visual Binding Coverage", summary);
            StringAssert.Contains("Asset Review Packet", summary);
            StringAssert.Contains("Starter Cat Turnaround Source Locks", summary);
            StringAssert.Contains("Hard Reference Source Locks", summary);
            StringAssert.Contains("Status Tag Coverage", summary);
            StringAssert.Contains("Status HUD Coverage", summary);
            StringAssert.Contains("Main Menu Coverage", summary);
            StringAssert.Contains("Route Choice Coverage", summary);
            StringAssert.Contains("Route Map Input Coverage", summary);
            StringAssert.Contains("Route Map Surface Coverage", summary);
            StringAssert.Contains("Runtime Settings Coverage", summary);
            StringAssert.Contains("Chinese UI Coverage", summary);
            StringAssert.Contains("Chinese UI Scale Validation", summary);
            StringAssert.Contains("Enemy HUD Coverage", summary);
            StringAssert.Contains("Cat HUD Coverage", summary);
            StringAssert.Contains("Skill HUD Coverage", summary);
            StringAssert.Contains("Battle Feedback Coverage", summary);
            StringAssert.Contains("Battle Feedback Visual Coverage", summary);
            StringAssert.Contains("Battle Result Coverage", summary);
            StringAssert.Contains("Playable Readiness", summary);
            StringAssert.Contains("Graybox Telemetry", summary);
        }

        [Test]
        public void Check_BuildSummaryIncludesStateAndMessage()
        {
            P0CodeSmokeSuiteCheck check = new P0CodeSmokeSuiteCheck(
                "example",
                "Example Gate",
                P0CodeSmokeSuiteState.Warning,
                "Needs tuning.");

            string summary = check.BuildSummary();

            StringAssert.Contains("Example Gate", summary);
            StringAssert.Contains("Warning", summary);
            StringAssert.Contains("Needs tuning", summary);
        }

        private static bool IsGeneratedAcceptedAssetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            foreach (P0AssetManifestEntry entry in P0AssetManifestCatalog.CreateP0PlannedManifest())
            {
                if (entry.Status == P0AssetManifestStatus.Generated
                    && path == entry.UnityImportPath)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
