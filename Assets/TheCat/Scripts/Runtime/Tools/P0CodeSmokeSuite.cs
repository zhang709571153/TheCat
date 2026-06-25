using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0CodeSmokeSuiteState
    {
        Passed,
        Warning,
        Failed
    }

    public readonly struct P0CodeSmokeSuiteCheck
    {
        public P0CodeSmokeSuiteCheck(string checkId, string displayName, P0CodeSmokeSuiteState state, string message)
        {
            CheckId = checkId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            State = state;
            Message = message ?? string.Empty;
        }

        public string CheckId { get; }

        public string DisplayName { get; }

        public P0CodeSmokeSuiteState State { get; }

        public string Message { get; }

        public string BuildSummary()
        {
            return DisplayName + ": " + State + " - " + Message;
        }
    }

    public sealed class P0CodeSmokeSuiteReport
    {
        private readonly List<P0CodeSmokeSuiteCheck> checks = new List<P0CodeSmokeSuiteCheck>();

        public IReadOnlyList<P0CodeSmokeSuiteCheck> Checks => checks.AsReadOnly();

        public int FailureCount => Count(P0CodeSmokeSuiteState.Failed);

        public int WarningCount => Count(P0CodeSmokeSuiteState.Warning);

        public bool IsPassed => FailureCount == 0;

        public void AddCheck(string checkId, string displayName, P0CodeSmokeSuiteState state, string message)
        {
            checks.Add(new P0CodeSmokeSuiteCheck(checkId, displayName, state, message));
        }

        public bool TryGetCheck(string checkId, out P0CodeSmokeSuiteCheck check)
        {
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].CheckId == checkId)
                {
                    check = checks[i];
                    return true;
                }
            }

            check = default(P0CodeSmokeSuiteCheck);
            return false;
        }

        public string BuildSummary()
        {
            return IsPassed
                ? "P0 code smoke suite passed " + checks.Count + " check(s) with " + WarningCount + " warning(s)."
                : "P0 code smoke suite failed " + FailureCount + " check(s) with " + WarningCount + " warning(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < checks.Count; i++)
            {
                lines.Add("[" + checks[i].State + "] " + checks[i].BuildSummary());
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0CodeSmokeSuiteState state)
        {
            int count = 0;
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].State == state)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0CodeSmokeSuite
    {
        public const string GoldenPathSimulationCheckId = "golden_path_simulation";
        public const string GoldenPathAcceptanceCheckId = "golden_path_acceptance";
        public const string CharacterDesignCoverageCheckId = "character_design_coverage";
        public const string AssetManifestCoverageCheckId = "asset_manifest_coverage";
        public const string AssetGenerationBatchCoverageCheckId = "asset_generation_batch_coverage";
        public const string AssetImportReadinessCheckId = "asset_import_readiness";
        public const string AssetMetaImportSettingsCheckId = "asset_meta_import_settings";
        public const string RuntimeVisualBindingCoverageCheckId = "runtime_visual_binding_coverage";
        public const string AssetReviewPacketCheckId = "asset_review_packet";
        public const string StarterCatTurnaroundSourceLockCheckId = "starter_cat_turnaround_source_lock";
        public const string HardReferenceSourceLockCheckId = "hard_reference_source_lock";
        public const string StatusTagCoverageCheckId = "status_tag_coverage";
        public const string StatusHudCoverageCheckId = "status_hud_coverage";
        public const string MainMenuCoverageCheckId = "main_menu_coverage";
        public const string RouteChoiceCoverageCheckId = "route_choice_coverage";
        public const string RouteMapInputCoverageCheckId = "route_map_input_coverage";
        public const string RouteMapSurfaceCoverageCheckId = "route_map_surface_coverage";
        public const string RuntimeSettingsCoverageCheckId = "runtime_settings_coverage";
        public const string ChineseUiCoverageCheckId = "chinese_ui_coverage";
        public const string ChineseUiScaleValidationCheckId = "chinese_ui_scale_validation";
        public const string EnemyHudCoverageCheckId = "enemy_hud_coverage";
        public const string CatHudCoverageCheckId = "cat_hud_coverage";
        public const string SkillHudCoverageCheckId = "skill_hud_coverage";
        public const string BattleFeedbackCoverageCheckId = "battle_feedback_coverage";
        public const string BattleFeedbackVisualCoverageCheckId = "battle_feedback_visual_coverage";
        public const string BattleResultCoverageCheckId = "battle_result_coverage";
        public const string PlayableReadinessCheckId = "playable_readiness";
        public const string GrayboxTelemetryCheckId = "graybox_telemetry";

        public static P0CodeSmokeSuiteReport EvaluatePrototypeBuild()
        {
            P0GoldenPathReport goldenPath = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(goldenPath);
            P0CharacterDesignCoverageReport characterDesign = P0CharacterDesignCoverage.EvaluatePrototypeRoster();
            P0AssetManifestCoverageReport assetManifest = P0AssetManifestCoverage.EvaluateP0Manifest();
            P0AssetGenerationBatchCoverageReport assetGenerationBatches = P0AssetGenerationBatchCoverage.EvaluateP0Batches();
            P0AssetImportReadinessReport assetImportReadiness = P0AssetImportReadiness.EvaluateP0Manifest();
            P0AssetMetaImportSettingsReport assetMetaImportSettings = P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest();
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings = P0RuntimeVisualBindingCoverage.EvaluateP0Bindings();
            P0AssetReviewPacketReport assetReviewPacket = P0AssetReviewPacket.EvaluateP0Packet();
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
            P0PlayableReadinessReport readiness = P0PlayableReadiness.Evaluate(new P0PlayableReadinessContext(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0PrototypeCatalog.CreateStarterCats(),
                P0PrototypeCatalog.CreateStarterSkills(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0PrototypeCatalog.CreateStatusTags(),
                P0PrototypeCatalog.CreateWaveForContentId,
                acceptance,
                statusTags));
            P0GrayboxTelemetryReport telemetry = P0GrayboxTelemetry.Evaluate(goldenPath);

            return Evaluate(goldenPath, acceptance, characterDesign, assetManifest, assetGenerationBatches, assetImportReadiness, assetMetaImportSettings, runtimeVisualBindings, assetReviewPacket, starterCatSourceLocks, hardReferenceSourceLocks, statusTags, statusHud, mainMenu, routeChoices, routeMapInput, routeMapSurface, runtimeSettings, chineseUi, chineseUiScale, enemyHud, catHud, skillHud, battleFeedback, battleFeedbackVisuals, battleResult, readiness, telemetry);
        }

        public static P0CodeSmokeSuiteReport Evaluate(
            P0GoldenPathReport goldenPath,
            P0GoldenPathAcceptanceReport acceptance,
            P0CharacterDesignCoverageReport characterDesign,
            P0AssetManifestCoverageReport assetManifest,
            P0AssetGenerationBatchCoverageReport assetGenerationBatches,
            P0AssetImportReadinessReport assetImportReadiness,
            P0AssetMetaImportSettingsReport assetMetaImportSettings,
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings,
            P0AssetReviewPacketReport assetReviewPacket,
            P0StarterCatTurnaroundSourceLockReport starterCatSourceLocks,
            P0HardReferenceSourceLockReport hardReferenceSourceLocks,
            P0StatusTagCoverageReport statusTags,
            P0StatusHudCoverageReport statusHud,
            P0MainMenuCoverageReport mainMenu,
            P0RouteChoiceCoverageReport routeChoices,
            P0RouteMapInputCoverageReport routeMapInput,
            P0RouteMapSurfaceCoverageReport routeMapSurface,
            P0RuntimeSettingsCoverageReport runtimeSettings,
            P0ChineseUiCoverageReport chineseUi,
            P0ChineseUiScaleValidationReport chineseUiScale,
            P0EnemyHudCoverageReport enemyHud,
            P0CatHudCoverageReport catHud,
            P0SkillHudCoverageReport skillHud,
            P0BattleFeedbackCoverageReport battleFeedback,
            P0BattleFeedbackVisualCoverageReport battleFeedbackVisuals,
            P0BattleResultCoverageReport battleResult,
            P0PlayableReadinessReport readiness,
            P0GrayboxTelemetryReport telemetry)
        {
            P0CodeSmokeSuiteReport report = new P0CodeSmokeSuiteReport();
            EvaluateGoldenPathSimulation(goldenPath, report);
            EvaluateGoldenPathAcceptance(acceptance, report);
            EvaluateCharacterDesignCoverage(characterDesign, report);
            EvaluateAssetManifestCoverage(assetManifest, report);
            EvaluateAssetGenerationBatchCoverage(assetGenerationBatches, report);
            EvaluateAssetImportReadiness(assetImportReadiness, report);
            EvaluateAssetMetaImportSettings(assetMetaImportSettings, report);
            EvaluateRuntimeVisualBindingCoverage(runtimeVisualBindings, report);
            EvaluateAssetReviewPacket(assetReviewPacket, report);
            EvaluateStarterCatTurnaroundSourceLocks(starterCatSourceLocks, report);
            EvaluateHardReferenceSourceLocks(hardReferenceSourceLocks, report);
            EvaluateStatusTagCoverage(statusTags, report);
            EvaluateStatusHudCoverage(statusHud, report);
            EvaluateMainMenuCoverage(mainMenu, report);
            EvaluateRouteChoiceCoverage(routeChoices, report);
            EvaluateRouteMapInputCoverage(routeMapInput, report);
            EvaluateRouteMapSurfaceCoverage(routeMapSurface, report);
            EvaluateRuntimeSettingsCoverage(runtimeSettings, report);
            EvaluateChineseUiCoverage(chineseUi, report);
            EvaluateChineseUiScaleValidation(chineseUiScale, report);
            EvaluateEnemyHudCoverage(enemyHud, report);
            EvaluateCatHudCoverage(catHud, report);
            EvaluateSkillHudCoverage(skillHud, report);
            EvaluateBattleFeedbackCoverage(battleFeedback, report);
            EvaluateBattleFeedbackVisualCoverage(battleFeedbackVisuals, report);
            EvaluateBattleResultCoverage(battleResult, report);
            EvaluatePlayableReadiness(readiness, report);
            EvaluateGrayboxTelemetry(telemetry, report);
            return report;
        }

        private static void EvaluateGoldenPathSimulation(P0GoldenPathReport goldenPath, P0CodeSmokeSuiteReport report)
        {
            if (goldenPath == null)
            {
                report.AddCheck(
                    GoldenPathSimulationCheckId,
                    "Golden Path Simulation",
                    P0CodeSmokeSuiteState.Failed,
                    "Golden path report is missing.");
                return;
            }

            if (goldenPath.IsCleared
                && goldenPath.Settlement.CompletedNodes == goldenPath.Settlement.TotalLayers
                && goldenPath.BossBattleCleared
                && goldenPath.BossBehaviorObserved)
            {
                report.AddCheck(
                    GoldenPathSimulationCheckId,
                    "Golden Path Simulation",
                    P0CodeSmokeSuiteState.Passed,
                    goldenPath.BuildSummary());
                return;
            }

            report.AddCheck(
                GoldenPathSimulationCheckId,
                "Golden Path Simulation",
                P0CodeSmokeSuiteState.Failed,
                goldenPath.BuildSummary());
        }

        private static void EvaluateGoldenPathAcceptance(
            P0GoldenPathAcceptanceReport acceptance,
            P0CodeSmokeSuiteReport report)
        {
            if (acceptance == null)
            {
                report.AddCheck(
                    GoldenPathAcceptanceCheckId,
                    "Golden Path Acceptance",
                    P0CodeSmokeSuiteState.Failed,
                    "Golden path acceptance report is missing.");
                return;
            }

            if (!acceptance.IsAccepted)
            {
                report.AddCheck(
                    GoldenPathAcceptanceCheckId,
                    "Golden Path Acceptance",
                    P0CodeSmokeSuiteState.Failed,
                    acceptance.BuildSummary());
                return;
            }

            report.AddCheck(
                GoldenPathAcceptanceCheckId,
                "Golden Path Acceptance",
                acceptance.WarningCount > 0 ? P0CodeSmokeSuiteState.Warning : P0CodeSmokeSuiteState.Passed,
                acceptance.BuildSummary());
        }

        private static void EvaluateCharacterDesignCoverage(
            P0CharacterDesignCoverageReport characterDesign,
            P0CodeSmokeSuiteReport report)
        {
            if (characterDesign == null)
            {
                report.AddCheck(
                    CharacterDesignCoverageCheckId,
                    "Character Design Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Character design coverage report is missing.");
                return;
            }

            report.AddCheck(
                CharacterDesignCoverageCheckId,
                "Character Design Coverage",
                characterDesign.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                characterDesign.IsComplete ? characterDesign.BuildSummary() : characterDesign.BuildDetailedSummary());
        }

        private static void EvaluateAssetManifestCoverage(
            P0AssetManifestCoverageReport assetManifest,
            P0CodeSmokeSuiteReport report)
        {
            if (assetManifest == null)
            {
                report.AddCheck(
                    AssetManifestCoverageCheckId,
                    "Asset Manifest Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Asset manifest coverage report is missing.");
                return;
            }

            report.AddCheck(
                AssetManifestCoverageCheckId,
                "Asset Manifest Coverage",
                assetManifest.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                assetManifest.IsComplete ? assetManifest.BuildSummary() : assetManifest.BuildDetailedSummary());
        }

        private static void EvaluateAssetGenerationBatchCoverage(
            P0AssetGenerationBatchCoverageReport assetGenerationBatches,
            P0CodeSmokeSuiteReport report)
        {
            if (assetGenerationBatches == null)
            {
                report.AddCheck(
                    AssetGenerationBatchCoverageCheckId,
                    "Asset Generation Batch Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Asset generation batch coverage report is missing.");
                return;
            }

            report.AddCheck(
                AssetGenerationBatchCoverageCheckId,
                "Asset Generation Batch Coverage",
                assetGenerationBatches.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                assetGenerationBatches.IsComplete ? assetGenerationBatches.BuildSummary() : assetGenerationBatches.BuildDetailedSummary());
        }

        private static void EvaluateAssetImportReadiness(
            P0AssetImportReadinessReport assetImportReadiness,
            P0CodeSmokeSuiteReport report)
        {
            if (assetImportReadiness == null)
            {
                report.AddCheck(
                    AssetImportReadinessCheckId,
                    "Asset Import Readiness",
                    P0CodeSmokeSuiteState.Failed,
                    "Asset import readiness report is missing.");
                return;
            }

            report.AddCheck(
                AssetImportReadinessCheckId,
                "Asset Import Readiness",
                assetImportReadiness.IsReady
                    ? (assetImportReadiness.WarningCount > 0 ? P0CodeSmokeSuiteState.Warning : P0CodeSmokeSuiteState.Passed)
                    : P0CodeSmokeSuiteState.Failed,
                assetImportReadiness.BuildSummary());
        }

        private static void EvaluateAssetMetaImportSettings(
            P0AssetMetaImportSettingsReport assetMetaImportSettings,
            P0CodeSmokeSuiteReport report)
        {
            if (assetMetaImportSettings == null)
            {
                report.AddCheck(
                    AssetMetaImportSettingsCheckId,
                    "Asset Meta Import Settings",
                    P0CodeSmokeSuiteState.Failed,
                    "Asset meta import settings report is missing.");
                return;
            }

            report.AddCheck(
                AssetMetaImportSettingsCheckId,
                "Asset Meta Import Settings",
                assetMetaImportSettings.IsReady ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                assetMetaImportSettings.BuildSummary());
        }

        private static void EvaluateRuntimeVisualBindingCoverage(
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings,
            P0CodeSmokeSuiteReport report)
        {
            if (runtimeVisualBindings == null)
            {
                report.AddCheck(
                    RuntimeVisualBindingCoverageCheckId,
                    "Runtime Visual Binding Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Runtime visual binding coverage report is missing.");
                return;
            }

            report.AddCheck(
                RuntimeVisualBindingCoverageCheckId,
                "Runtime Visual Binding Coverage",
                runtimeVisualBindings.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                runtimeVisualBindings.BuildSummary());
        }

        private static void EvaluateAssetReviewPacket(
            P0AssetReviewPacketReport assetReviewPacket,
            P0CodeSmokeSuiteReport report)
        {
            if (assetReviewPacket == null)
            {
                report.AddCheck(
                    AssetReviewPacketCheckId,
                    "Asset Review Packet",
                    P0CodeSmokeSuiteState.Failed,
                    "Asset review packet report is missing.");
                return;
            }

            report.AddCheck(
                AssetReviewPacketCheckId,
                "Asset Review Packet",
                assetReviewPacket.IsReady ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                assetReviewPacket.BuildSummary());
        }

        private static void EvaluateStarterCatTurnaroundSourceLocks(
            P0StarterCatTurnaroundSourceLockReport starterCatSourceLocks,
            P0CodeSmokeSuiteReport report)
        {
            if (starterCatSourceLocks == null)
            {
                report.AddCheck(
                    StarterCatTurnaroundSourceLockCheckId,
                    "Starter Cat Turnaround Source Locks",
                    P0CodeSmokeSuiteState.Failed,
                    "Starter cat turnaround source lock report is missing.");
                return;
            }

            report.AddCheck(
                StarterCatTurnaroundSourceLockCheckId,
                "Starter Cat Turnaround Source Locks",
                starterCatSourceLocks.IsReady ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                starterCatSourceLocks.BuildSummary());
        }

        private static void EvaluateHardReferenceSourceLocks(
            P0HardReferenceSourceLockReport hardReferenceSourceLocks,
            P0CodeSmokeSuiteReport report)
        {
            if (hardReferenceSourceLocks == null)
            {
                report.AddCheck(
                    HardReferenceSourceLockCheckId,
                    "Hard Reference Source Locks",
                    P0CodeSmokeSuiteState.Failed,
                    "Hard reference source lock report is missing.");
                return;
            }

            report.AddCheck(
                HardReferenceSourceLockCheckId,
                "Hard Reference Source Locks",
                hardReferenceSourceLocks.IsReady ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                hardReferenceSourceLocks.IsReady ? hardReferenceSourceLocks.BuildSummary() : hardReferenceSourceLocks.BuildDetailedSummary());
        }

        private static void EvaluateStatusTagCoverage(
            P0StatusTagCoverageReport statusTags,
            P0CodeSmokeSuiteReport report)
        {
            if (statusTags == null)
            {
                report.AddCheck(
                    StatusTagCoverageCheckId,
                    "Status Tag Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Status tag coverage report is missing.");
                return;
            }

            report.AddCheck(
                StatusTagCoverageCheckId,
                "Status Tag Coverage",
                statusTags.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                statusTags.BuildSummary());
        }

        private static void EvaluateStatusHudCoverage(
            P0StatusHudCoverageReport statusHud,
            P0CodeSmokeSuiteReport report)
        {
            if (statusHud == null)
            {
                report.AddCheck(
                    StatusHudCoverageCheckId,
                    "Status HUD Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Status HUD coverage report is missing.");
                return;
            }

            report.AddCheck(
                StatusHudCoverageCheckId,
                "Status HUD Coverage",
                statusHud.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                statusHud.BuildSummary());
        }

        private static void EvaluateMainMenuCoverage(
            P0MainMenuCoverageReport mainMenu,
            P0CodeSmokeSuiteReport report)
        {
            if (mainMenu == null)
            {
                report.AddCheck(
                    MainMenuCoverageCheckId,
                    "Main Menu Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Main menu coverage report is missing.");
                return;
            }

            report.AddCheck(
                MainMenuCoverageCheckId,
                "Main Menu Coverage",
                mainMenu.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                mainMenu.IsComplete ? mainMenu.BuildSummary() : mainMenu.BuildDetailedSummary());
        }

        private static void EvaluatePlayableReadiness(
            P0PlayableReadinessReport readiness,
            P0CodeSmokeSuiteReport report)
        {
            if (readiness == null)
            {
                report.AddCheck(
                    PlayableReadinessCheckId,
                    "Playable Readiness",
                    P0CodeSmokeSuiteState.Failed,
                    "Playable readiness report is missing.");
                return;
            }

            if (!readiness.IsReady)
            {
                report.AddCheck(
                    PlayableReadinessCheckId,
                    "Playable Readiness",
                    P0CodeSmokeSuiteState.Failed,
                    readiness.BuildSummary());
                return;
            }

            report.AddCheck(
                PlayableReadinessCheckId,
                "Playable Readiness",
                readiness.WarningCount > 0 ? P0CodeSmokeSuiteState.Warning : P0CodeSmokeSuiteState.Passed,
                readiness.BuildSummary());
        }

        private static void EvaluateRouteChoiceCoverage(
            P0RouteChoiceCoverageReport routeChoices,
            P0CodeSmokeSuiteReport report)
        {
            if (routeChoices == null)
            {
                report.AddCheck(
                    RouteChoiceCoverageCheckId,
                    "Route Choice Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Route choice coverage report is missing.");
                return;
            }

            report.AddCheck(
                RouteChoiceCoverageCheckId,
                "Route Choice Coverage",
                routeChoices.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                routeChoices.BuildSummary());
        }

        private static void EvaluateRouteMapInputCoverage(
            P0RouteMapInputCoverageReport routeMapInput,
            P0CodeSmokeSuiteReport report)
        {
            if (routeMapInput == null)
            {
                report.AddCheck(
                    RouteMapInputCoverageCheckId,
                    "Route Map Input Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Route map input coverage report is missing.");
                return;
            }

            report.AddCheck(
                RouteMapInputCoverageCheckId,
                "Route Map Input Coverage",
                routeMapInput.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                routeMapInput.BuildSummary());
        }

        private static void EvaluateRouteMapSurfaceCoverage(
            P0RouteMapSurfaceCoverageReport routeMapSurface,
            P0CodeSmokeSuiteReport report)
        {
            if (routeMapSurface == null)
            {
                report.AddCheck(
                    RouteMapSurfaceCoverageCheckId,
                    "Route Map Surface Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Route map surface coverage report is missing.");
                return;
            }

            report.AddCheck(
                RouteMapSurfaceCoverageCheckId,
                "Route Map Surface Coverage",
                routeMapSurface.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                routeMapSurface.BuildSummary());
        }

        private static void EvaluateRuntimeSettingsCoverage(
            P0RuntimeSettingsCoverageReport runtimeSettings,
            P0CodeSmokeSuiteReport report)
        {
            if (runtimeSettings == null)
            {
                report.AddCheck(
                    RuntimeSettingsCoverageCheckId,
                    "Runtime Settings Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Runtime settings coverage report is missing.");
                return;
            }

            report.AddCheck(
                RuntimeSettingsCoverageCheckId,
                "Runtime Settings Coverage",
                runtimeSettings.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                runtimeSettings.BuildSummary());
        }

        private static void EvaluateChineseUiCoverage(
            P0ChineseUiCoverageReport chineseUi,
            P0CodeSmokeSuiteReport report)
        {
            if (chineseUi == null)
            {
                report.AddCheck(
                    ChineseUiCoverageCheckId,
                    "Chinese UI Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Chinese UI coverage report is missing.");
                return;
            }

            report.AddCheck(
                ChineseUiCoverageCheckId,
                "Chinese UI Coverage",
                chineseUi.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                chineseUi.BuildSummary());
        }

        private static void EvaluateChineseUiScaleValidation(
            P0ChineseUiScaleValidationReport chineseUiScale,
            P0CodeSmokeSuiteReport report)
        {
            if (chineseUiScale == null)
            {
                report.AddCheck(
                    ChineseUiScaleValidationCheckId,
                    "Chinese UI Scale Validation",
                    P0CodeSmokeSuiteState.Failed,
                    "Chinese UI scale validation report is missing.");
                return;
            }

            report.AddCheck(
                ChineseUiScaleValidationCheckId,
                "Chinese UI Scale Validation",
                chineseUiScale.IsReady ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                chineseUiScale.BuildSummary());
        }

        private static void EvaluateEnemyHudCoverage(
            P0EnemyHudCoverageReport enemyHud,
            P0CodeSmokeSuiteReport report)
        {
            if (enemyHud == null)
            {
                report.AddCheck(
                    EnemyHudCoverageCheckId,
                    "Enemy HUD Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Enemy HUD coverage report is missing.");
                return;
            }

            report.AddCheck(
                EnemyHudCoverageCheckId,
                "Enemy HUD Coverage",
                enemyHud.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                enemyHud.BuildSummary());
        }

        private static void EvaluateCatHudCoverage(
            P0CatHudCoverageReport catHud,
            P0CodeSmokeSuiteReport report)
        {
            if (catHud == null)
            {
                report.AddCheck(
                    CatHudCoverageCheckId,
                    "Cat HUD Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Cat HUD coverage report is missing.");
                return;
            }

            report.AddCheck(
                CatHudCoverageCheckId,
                "Cat HUD Coverage",
                catHud.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                catHud.BuildSummary());
        }

        private static void EvaluateSkillHudCoverage(
            P0SkillHudCoverageReport skillHud,
            P0CodeSmokeSuiteReport report)
        {
            if (skillHud == null)
            {
                report.AddCheck(
                    SkillHudCoverageCheckId,
                    "Skill HUD Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Skill HUD coverage report is missing.");
                return;
            }

            report.AddCheck(
                SkillHudCoverageCheckId,
                "Skill HUD Coverage",
                skillHud.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                skillHud.IsComplete ? skillHud.BuildSummary() : skillHud.BuildDetailedSummary());
        }

        private static void EvaluateBattleFeedbackCoverage(
            P0BattleFeedbackCoverageReport battleFeedback,
            P0CodeSmokeSuiteReport report)
        {
            if (battleFeedback == null)
            {
                report.AddCheck(
                    BattleFeedbackCoverageCheckId,
                    "Battle Feedback Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Battle feedback coverage report is missing.");
                return;
            }

            report.AddCheck(
                BattleFeedbackCoverageCheckId,
                "Battle Feedback Coverage",
                battleFeedback.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                battleFeedback.IsComplete ? battleFeedback.BuildSummary() : battleFeedback.BuildDetailedSummary());
        }

        private static void EvaluateBattleFeedbackVisualCoverage(
            P0BattleFeedbackVisualCoverageReport battleFeedbackVisuals,
            P0CodeSmokeSuiteReport report)
        {
            if (battleFeedbackVisuals == null)
            {
                report.AddCheck(
                    BattleFeedbackVisualCoverageCheckId,
                    "Battle Feedback Visual Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Battle feedback visual coverage report is missing.");
                return;
            }

            report.AddCheck(
                BattleFeedbackVisualCoverageCheckId,
                "Battle Feedback Visual Coverage",
                battleFeedbackVisuals.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                battleFeedbackVisuals.IsComplete ? battleFeedbackVisuals.BuildSummary() : battleFeedbackVisuals.BuildDetailedSummary());
        }

        private static void EvaluateBattleResultCoverage(
            P0BattleResultCoverageReport battleResult,
            P0CodeSmokeSuiteReport report)
        {
            if (battleResult == null)
            {
                report.AddCheck(
                    BattleResultCoverageCheckId,
                    "Battle Result Coverage",
                    P0CodeSmokeSuiteState.Failed,
                    "Battle result coverage report is missing.");
                return;
            }

            report.AddCheck(
                BattleResultCoverageCheckId,
                "Battle Result Coverage",
                battleResult.IsComplete ? P0CodeSmokeSuiteState.Passed : P0CodeSmokeSuiteState.Failed,
                battleResult.BuildSummary());
        }

        private static void EvaluateGrayboxTelemetry(
            P0GrayboxTelemetryReport telemetry,
            P0CodeSmokeSuiteReport report)
        {
            if (telemetry == null)
            {
                report.AddCheck(
                    GrayboxTelemetryCheckId,
                    "Graybox Telemetry",
                    P0CodeSmokeSuiteState.Failed,
                    "Graybox telemetry report is missing.");
                return;
            }

            if (!telemetry.IsUsable)
            {
                report.AddCheck(
                    GrayboxTelemetryCheckId,
                    "Graybox Telemetry",
                    P0CodeSmokeSuiteState.Failed,
                    telemetry.BuildSummary());
                return;
            }

            report.AddCheck(
                GrayboxTelemetryCheckId,
                "Graybox Telemetry",
                telemetry.WarningCount > 0 ? P0CodeSmokeSuiteState.Warning : P0CodeSmokeSuiteState.Passed,
                telemetry.BuildSummary());
        }
    }
}
