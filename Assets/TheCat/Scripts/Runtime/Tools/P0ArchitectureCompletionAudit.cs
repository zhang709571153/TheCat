using System;
using System.Collections.Generic;
using System.Text;
using TheCat.Data.Catalogs;

namespace TheCat.Tools
{
    public enum P0ArchitectureCompletionAuditGateState
    {
        Passed,
        PendingUnityValidation,
        Failed
    }

    public readonly struct P0ArchitectureCompletionAuditGate
    {
        public P0ArchitectureCompletionAuditGate(
            string gateId,
            string displayName,
            P0ArchitectureCompletionAuditGateState state,
            string message)
        {
            GateId = gateId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            State = state;
            Message = message ?? string.Empty;
        }

        public string GateId { get; }

        public string DisplayName { get; }

        public P0ArchitectureCompletionAuditGateState State { get; }

        public string Message { get; }

        public string BuildSummary()
        {
            return DisplayName + ": " + State + " - " + Message;
        }
    }

    public sealed class P0ArchitectureCompletionAuditReport
    {
        private readonly List<P0ArchitectureCompletionAuditGate> gates =
            new List<P0ArchitectureCompletionAuditGate>();

        public IReadOnlyList<P0ArchitectureCompletionAuditGate> Gates => gates.AsReadOnly();

        public P0GoldenPathReport GoldenPath { get; private set; }

        public P0GoldenPathAcceptanceReport GoldenPathAcceptance { get; private set; }

        public P0PlayableReadinessReport PlayableReadiness { get; private set; }

        public P0CodeSmokeSuiteReport CodeSmokeSuite { get; private set; }

        public P0ChineseUiScaleValidationReport ChineseUiScaleValidation { get; private set; }

        public P0VisualAcceptanceReport VisualAcceptance { get; private set; }

        public P0AssetProductionReadinessReport AssetProductionReadiness { get; private set; }

        public P0AssetProductionQueueCoverageReport AssetProductionQueue { get; private set; }

        public P0PlayModeScreenshotFileEvidenceReport ScreenshotFileEvidence { get; private set; }

        public P0PlayModeEvidenceReport PlayModeEvidence { get; private set; }

        public P0StarterCatFormalImportReadinessReport StarterCatFormalImport { get; private set; }

        public int PassedGateCount => Count(P0ArchitectureCompletionAuditGateState.Passed);

        public int PendingUnityValidationGateCount => Count(P0ArchitectureCompletionAuditGateState.PendingUnityValidation);

        public int FailedGateCount => Count(P0ArchitectureCompletionAuditGateState.Failed);

        public bool HasBlockingFailures => FailedGateCount > 0;

        public bool HasPlayableArchitecture =>
            PlayableReadiness != null
            && PlayableReadiness.IsReady
            && GoldenPathAcceptance != null
            && GoldenPathAcceptance.IsAccepted
            && CodeSmokeSuite != null
            && CodeSmokeSuite.IsPassed
            && ChineseUiScaleValidation != null
            && ChineseUiScaleValidation.IsReady;

        public bool IsReadyForSystematicAssetProduction =>
            !HasBlockingFailures
            && HasPlayableArchitecture
            && VisualAcceptance != null
            && VisualAcceptance.IsArchitectureReadyForSystematicAssetProduction
            && AssetProductionReadiness != null
            && AssetProductionReadiness.IsReady
            && AssetProductionQueue != null
            && AssetProductionQueue.IsReady;

        public bool HasFinalUnityRuntimeEvidence =>
            VisualAcceptance != null
            && VisualAcceptance.IsFinalP0VisualAcceptanceReady;

        public bool RequiresUnityRuntimeValidation => !HasFinalUnityRuntimeEvidence;

        public bool IsFinalP0PlayableComplete =>
            IsReadyForSystematicAssetProduction
            && HasFinalUnityRuntimeEvidence
            && StarterCatFormalImport != null
            && StarterCatFormalImport.IsImportAllowed;

        public int TotalRouteLayers => GoldenPath == null ? 0 : GoldenPath.Settlement.TotalLayers;

        public int CompletedRouteLayers => GoldenPath == null ? 0 : GoldenPath.Settlement.CompletedNodes;

        public int BattleCount => GoldenPath == null ? 0 : GoldenPath.BattleCount;

        public int QueueCount => AssetProductionQueue == null ? 0 : AssetProductionQueue.QueueCount;

        public int CodexRunnableAssetQueueCount =>
            AssetProductionQueue == null ? 0 : AssetProductionQueue.CodexRunnableCount;

        public int CandidatePacksPendingUnityReviewCount =>
            AssetProductionQueue == null
                ? 0
                : AssetProductionQueue.CandidatePackCompletePendingUnityReviewCount;

        public int UnityBlockedAssetQueueCount =>
            AssetProductionQueue == null ? 0 : AssetProductionQueue.UnityBlockedCount;

        public int ExistingScreenshotEvidenceCount =>
            ScreenshotFileEvidence == null ? 0 : ScreenshotFileEvidence.ExistingExpectedFileCount;

        public int ExpectedScreenshotEvidenceCount =>
            ScreenshotFileEvidence == null ? 0 : ScreenshotFileEvidence.ExpectedFileCount;

        public void SetReports(
            P0GoldenPathReport goldenPath,
            P0GoldenPathAcceptanceReport goldenPathAcceptance,
            P0PlayableReadinessReport playableReadiness,
            P0CodeSmokeSuiteReport codeSmokeSuite,
            P0ChineseUiScaleValidationReport chineseUiScaleValidation,
            P0VisualAcceptanceReport visualAcceptance,
            P0AssetProductionReadinessReport assetProductionReadiness,
            P0AssetProductionQueueCoverageReport assetProductionQueue,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence,
            P0PlayModeEvidenceReport playModeEvidence,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            GoldenPath = goldenPath;
            GoldenPathAcceptance = goldenPathAcceptance;
            PlayableReadiness = playableReadiness;
            CodeSmokeSuite = codeSmokeSuite;
            ChineseUiScaleValidation = chineseUiScaleValidation;
            VisualAcceptance = visualAcceptance;
            AssetProductionReadiness = assetProductionReadiness;
            AssetProductionQueue = assetProductionQueue;
            ScreenshotFileEvidence = screenshotFileEvidence;
            PlayModeEvidence = playModeEvidence;
            StarterCatFormalImport = starterCatFormalImport;
        }

        public void AddGate(
            string gateId,
            string displayName,
            P0ArchitectureCompletionAuditGateState state,
            string message)
        {
            gates.Add(new P0ArchitectureCompletionAuditGate(gateId, displayName, state, message));
        }

        public bool TryGetGate(string gateId, out P0ArchitectureCompletionAuditGate gate)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                if (gates[i].GateId == gateId)
                {
                    gate = gates[i];
                    return true;
                }
            }

            gate = default(P0ArchitectureCompletionAuditGate);
            return false;
        }

        public string BuildSummary()
        {
            if (IsFinalP0PlayableComplete)
            {
                return "P0 architecture and Unity runtime evidence are complete for the current playable target.";
            }

            if (IsReadyForSystematicAssetProduction)
            {
                return "P0 architecture is ready for systematic Codex-side asset production; final Unity runtime evidence remains pending.";
            }

            return "P0 architecture audit found " + FailedGateCount + " blocking failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Gates: " + PassedGateCount + " passed, " + PendingUnityValidationGateCount + " pending Unity validation, " + FailedGateCount + " failed.",
                "Golden path: " + CompletedRouteLayers + "/" + TotalRouteLayers + " layer(s), " + BattleCount + " battle(s).",
                "Chinese UI scale validation: " + (ChineseUiScaleValidation == null ? "missing" : ChineseUiScaleValidation.BuildSummary()),
                "Asset queue: " + QueueCount + " item(s), " + CodexRunnableAssetQueueCount + " Codex-runnable, " + CandidatePacksPendingUnityReviewCount + " candidate pack(s) pending Unity review, " + UnityBlockedAssetQueueCount + " Unity-blocked.",
                "Screenshot evidence: " + ExistingScreenshotEvidenceCount + "/" + ExpectedScreenshotEvidenceCount + " expected capture(s)."
            };

            for (int i = 0; i < gates.Count; i++)
            {
                lines.Add("[" + gates[i].State + "] " + gates[i].BuildSummary());
            }

            return string.Join(Environment.NewLine, lines);
        }

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# P0 Architecture Completion Audit");
            builder.AppendLine();
            builder.AppendLine("## Summary");
            builder.AppendLine();
            builder.AppendLine("- Ready for systematic Codex-side asset production: " + YesNo(IsReadyForSystematicAssetProduction));
            builder.AppendLine("- Final P0 Unity runtime complete: " + YesNo(IsFinalP0PlayableComplete));
            builder.AppendLine("- Requires Unity runtime validation: " + YesNo(RequiresUnityRuntimeValidation));
            builder.AppendLine("- Gates: " + PassedGateCount + " passed, " + PendingUnityValidationGateCount + " pending Unity validation, " + FailedGateCount + " failed.");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Gate Table");
            builder.AppendLine();
            builder.AppendLine("| gate | state | summary |");
            builder.AppendLine("| --- | --- | --- |");
            for (int i = 0; i < gates.Count; i++)
            {
                builder.Append("| ");
                builder.Append(Escape(gates[i].DisplayName));
                builder.Append(" | ");
                builder.Append(gates[i].State);
                builder.Append(" | ");
                builder.Append(Escape(gates[i].Message));
                builder.AppendLine(" |");
            }

            builder.AppendLine();
            builder.AppendLine("## Architecture Evidence");
            builder.AppendLine();
            builder.AppendLine("- Golden path: " + CompletedRouteLayers + "/" + TotalRouteLayers + " layer(s), " + BattleCount + " battle(s), Boss cleared " + YesNo(GoldenPath != null && GoldenPath.BossBattleCleared) + ".");
            builder.AppendLine("- Playable readiness: " + (PlayableReadiness == null ? "missing" : PlayableReadiness.BuildSummary()));
            builder.AppendLine("- Code smoke suite: " + (CodeSmokeSuite == null ? "missing" : CodeSmokeSuite.BuildSummary()));
            builder.AppendLine("- Chinese UI scale validation: " + (ChineseUiScaleValidation == null ? "missing" : ChineseUiScaleValidation.BuildSummary()));
            builder.AppendLine("- Visual acceptance: " + (VisualAcceptance == null ? "missing" : VisualAcceptance.BuildSummary()));
            builder.AppendLine();
            builder.AppendLine("## Asset Production Boundary");
            builder.AppendLine();
            builder.AppendLine("- Codex owns candidate production: image generation, cleanup, transparent PNG packaging, manifests, contact sheets, review sheets, process notes, and agent prompts.");
            builder.AppendLine("- Unity owns formal install: `.meta` files, Sprite import settings, AssetDatabase refresh, Console checks, scene/prefab binding, Play Mode screenshots, runtime scale, HUD readability, and final acceptance.");
            builder.AppendLine("- Current queue: " + QueueCount + " item(s), " + CodexRunnableAssetQueueCount + " Codex-runnable, " + CandidatePacksPendingUnityReviewCount + " candidate pack(s) pending Unity review, " + UnityBlockedAssetQueueCount + " Unity-blocked.");
            builder.AppendLine("- Starter cat formal import: " + (StarterCatFormalImport == null ? "missing" : StarterCatFormalImport.BuildSummary()));
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Rule");
            builder.AppendLine();
            builder.AppendLine("- Saiban, Nephthys, and Suzune body assets must remain strictly matched to the locked colored three-view turnarounds.");
            builder.AppendLine("- Cat-body candidates may be produced in Codex only as review material under `design/development/asset_candidates`; they must not be copied into `Assets` until active-cat Play Mode screenshots are accepted against the source turnarounds.");
            builder.AppendLine("- Symbolic VFX may reference authority, prop, and role language, but must not redraw or drift cat bodies unless the batch is explicitly a starter-cat candidate batch.");
            builder.AppendLine();
            builder.AppendLine("## Unity Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Screenshot files: " + ExistingScreenshotEvidenceCount + "/" + ExpectedScreenshotEvidenceCount + " expected capture(s).");
            builder.AppendLine("- Play Mode evidence: " + (PlayModeEvidence == null ? "missing" : PlayModeEvidence.BuildSummary()));
            builder.AppendLine("- Final visual acceptance: " + YesNo(HasFinalUnityRuntimeEvidence));
            return builder.ToString();
        }

        private int Count(P0ArchitectureCompletionAuditGateState state)
        {
            int count = 0;
            for (int i = 0; i < gates.Count; i++)
            {
                if (gates[i].State == state)
                {
                    count++;
                }
            }

            return count;
        }

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("|", "/");
        }
    }

    public static class P0ArchitectureCompletionAudit
    {
        public const string GoldenPathGateId = "golden_path";
        public const string PlayableReadinessGateId = "playable_readiness";
        public const string CodeSmokeGateId = "code_smoke";
        public const string ChineseUiScaleValidationGateId = "chinese_ui_scale_validation";
        public const string AssetProductionReadinessGateId = "asset_production_readiness";
        public const string VisualAcceptanceArchitectureGateId = "visual_acceptance_architecture";
        public const string AssetProductionQueueGateId = "asset_production_queue";
        public const string CodexUnityBoundaryGateId = "codex_unity_asset_boundary";
        public const string StarterCatFormalImportGateId = "starter_cat_formal_import";
        public const string PlayModeEvidenceGateId = "play_mode_evidence";
        public const string FinalP0RuntimeGateId = "final_p0_runtime";

        public static P0ArchitectureCompletionAuditReport EvaluateCurrentProject()
        {
            P0GoldenPathReport goldenPath = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport goldenPathAcceptance = P0GoldenPathAcceptance.Evaluate(goldenPath);
            P0PlayableReadinessReport playableReadiness = P0PlayableReadiness.EvaluatePrototypeBuild();
            P0CodeSmokeSuiteReport codeSmokeSuite = P0CodeSmokeSuite.EvaluatePrototypeBuild();
            P0ChineseUiScaleValidationReport chineseUiScaleValidation = P0ChineseUiScaleValidationPlan.EvaluateCurrentPlan();
            P0VisualAcceptanceReport visualAcceptance = P0VisualAcceptance.EvaluateCurrent();
            P0AssetProductionQueueCoverageReport assetProductionQueue = P0AssetProductionQueueCoverage.EvaluateP0Queue();

            return Evaluate(
                goldenPath,
                goldenPathAcceptance,
                playableReadiness,
                codeSmokeSuite,
                chineseUiScaleValidation,
                visualAcceptance,
                visualAcceptance == null ? null : visualAcceptance.AssetProductionReadiness,
                assetProductionQueue,
                visualAcceptance == null ? null : visualAcceptance.ScreenshotFileEvidence,
                visualAcceptance == null ? null : visualAcceptance.PlayModeEvidence,
                visualAcceptance == null ? null : visualAcceptance.StarterCatFormalImport);
        }

        public static P0ArchitectureCompletionAuditReport Evaluate(
            P0GoldenPathReport goldenPath,
            P0GoldenPathAcceptanceReport goldenPathAcceptance,
            P0PlayableReadinessReport playableReadiness,
            P0CodeSmokeSuiteReport codeSmokeSuite,
            P0ChineseUiScaleValidationReport chineseUiScaleValidation,
            P0VisualAcceptanceReport visualAcceptance,
            P0AssetProductionReadinessReport assetProductionReadiness,
            P0AssetProductionQueueCoverageReport assetProductionQueue,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence,
            P0PlayModeEvidenceReport playModeEvidence,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            P0ArchitectureCompletionAuditReport report = new P0ArchitectureCompletionAuditReport();
            report.SetReports(
                goldenPath,
                goldenPathAcceptance,
                playableReadiness,
                codeSmokeSuite,
                chineseUiScaleValidation,
                visualAcceptance,
                assetProductionReadiness,
                assetProductionQueue,
                screenshotFileEvidence,
                playModeEvidence,
                starterCatFormalImport);

            AddGoldenPathGate(report, goldenPath, goldenPathAcceptance);
            AddPassFailGate(
                report,
                PlayableReadinessGateId,
                "Playable Readiness",
                playableReadiness != null && playableReadiness.IsReady,
                playableReadiness == null ? "Playable readiness report is missing." : playableReadiness.BuildSummary());
            AddPassFailGate(
                report,
                CodeSmokeGateId,
                "Code Smoke Suite",
                codeSmokeSuite != null && codeSmokeSuite.IsPassed,
                codeSmokeSuite == null ? "Code smoke suite report is missing." : codeSmokeSuite.BuildSummary());
            AddPassFailGate(
                report,
                ChineseUiScaleValidationGateId,
                "Chinese UI Scale Validation",
                chineseUiScaleValidation != null && chineseUiScaleValidation.IsReady,
                chineseUiScaleValidation == null ? "Chinese UI scale validation report is missing." : chineseUiScaleValidation.BuildSummary());
            AddPassFailGate(
                report,
                AssetProductionReadinessGateId,
                "Asset Production Readiness",
                assetProductionReadiness != null && assetProductionReadiness.IsReady,
                assetProductionReadiness == null ? "Asset production readiness report is missing." : assetProductionReadiness.BuildSummary());
            AddPassFailGate(
                report,
                VisualAcceptanceArchitectureGateId,
                "Visual Acceptance Architecture",
                visualAcceptance != null && visualAcceptance.IsArchitectureReadyForSystematicAssetProduction,
                visualAcceptance == null ? "Visual acceptance report is missing." : visualAcceptance.BuildSummary());
            AddPassFailGate(
                report,
                AssetProductionQueueGateId,
                "Asset Production Queue",
                assetProductionQueue != null && assetProductionQueue.IsReady,
                assetProductionQueue == null ? "Asset production queue report is missing." : assetProductionQueue.BuildSummary());
            AddCodexUnityBoundaryGate(report, assetProductionQueue);
            AddStarterCatFormalImportGate(report, starterCatFormalImport);
            AddPlayModeEvidenceGate(report, screenshotFileEvidence, playModeEvidence);
            AddFinalRuntimeGate(report, visualAcceptance, starterCatFormalImport);
            return report;
        }

        private static void AddGoldenPathGate(
            P0ArchitectureCompletionAuditReport report,
            P0GoldenPathReport goldenPath,
            P0GoldenPathAcceptanceReport acceptance)
        {
            bool passed = goldenPath != null
                && goldenPath.IsCleared
                && goldenPath.BossBattleCleared
                && goldenPath.BossBehaviorObserved
                && acceptance != null
                && acceptance.IsAccepted;

            AddPassFailGate(
                report,
                GoldenPathGateId,
                "Golden Path",
                passed,
                acceptance == null
                    ? "Golden path acceptance report is missing."
                    : acceptance.BuildSummary());
        }

        private static void AddCodexUnityBoundaryGate(
            P0ArchitectureCompletionAuditReport report,
            P0AssetProductionQueueCoverageReport queue)
        {
            bool passed = queue != null
                && queue.IsReady
                && queue.CodexRunnableCount == P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount
                && queue.CandidatePackCompletePendingUnityReviewCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount
                && queue.UnityBlockedCount == P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount;

            AddPassFailGate(
                report,
                CodexUnityBoundaryGateId,
                "Codex/Unity Asset Boundary",
                passed,
                queue == null
                    ? "Asset queue is missing, so Codex candidate production and Unity formal install boundaries cannot be verified."
                    : "Codex candidate production is exhausted for the current queue; completed packs wait for Unity validation before formal install.");
        }

        private static void AddStarterCatFormalImportGate(
            P0ArchitectureCompletionAuditReport report,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            if (starterCatFormalImport == null || !starterCatFormalImport.IsGateValid)
            {
                report.AddGate(
                    StarterCatFormalImportGateId,
                    "Starter Cat Formal Import",
                    P0ArchitectureCompletionAuditGateState.Failed,
                    starterCatFormalImport == null
                        ? "Starter cat formal import report is missing."
                        : starterCatFormalImport.BuildSummary());
                return;
            }

            report.AddGate(
                StarterCatFormalImportGateId,
                "Starter Cat Formal Import",
                starterCatFormalImport.IsImportAllowed
                    ? P0ArchitectureCompletionAuditGateState.Passed
                    : P0ArchitectureCompletionAuditGateState.PendingUnityValidation,
                starterCatFormalImport.BuildSummary());
        }

        private static void AddPlayModeEvidenceGate(
            P0ArchitectureCompletionAuditReport report,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence,
            P0PlayModeEvidenceReport playModeEvidence)
        {
            bool complete = screenshotFileEvidence != null
                && screenshotFileEvidence.IsComplete
                && playModeEvidence != null
                && playModeEvidence.IsComplete;

            report.AddGate(
                PlayModeEvidenceGateId,
                "Play Mode Evidence",
                complete
                    ? P0ArchitectureCompletionAuditGateState.Passed
                    : P0ArchitectureCompletionAuditGateState.PendingUnityValidation,
                playModeEvidence == null
                    ? "Play Mode evidence is missing."
                    : playModeEvidence.BuildSummary());
        }

        private static void AddFinalRuntimeGate(
            P0ArchitectureCompletionAuditReport report,
            P0VisualAcceptanceReport visualAcceptance,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            bool complete = visualAcceptance != null
                && visualAcceptance.IsFinalP0VisualAcceptanceReady
                && starterCatFormalImport != null
                && starterCatFormalImport.IsImportAllowed;

            report.AddGate(
                FinalP0RuntimeGateId,
                "Final P0 Runtime",
                complete
                    ? P0ArchitectureCompletionAuditGateState.Passed
                    : P0ArchitectureCompletionAuditGateState.PendingUnityValidation,
                complete
                    ? "Unity runtime evidence and starter-cat formal import are complete."
                    : "Final P0 runtime remains pending until Console, Play Mode screenshots, scene/prefab binding, Sprite import settings, runtime scale, and starter-cat turnaround review pass.");
        }

        private static void AddPassFailGate(
            P0ArchitectureCompletionAuditReport report,
            string gateId,
            string displayName,
            bool passed,
            string message)
        {
            report.AddGate(
                gateId,
                displayName,
                passed
                    ? P0ArchitectureCompletionAuditGateState.Passed
                    : P0ArchitectureCompletionAuditGateState.Failed,
                message);
        }
    }
}
