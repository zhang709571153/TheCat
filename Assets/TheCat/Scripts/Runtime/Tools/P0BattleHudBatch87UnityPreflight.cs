using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Tools
{
    public delegate bool P0AssetPngVisualContentReader(string path, out string summary, out string error);

    public sealed class P0BattleHudBatch87UnityPreflightReport
    {
        private readonly List<string> issues = new List<string>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<string> blockingItems = new List<string>();
        private readonly List<P0BattleHudBatch87CandidateAsset> candidates = new List<P0BattleHudBatch87CandidateAsset>();

        public IReadOnlyList<string> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<string> BlockingItems => blockingItems.AsReadOnly();

        public IReadOnlyList<P0BattleHudBatch87CandidateAsset> Candidates => candidates.AsReadOnly();

        public int CandidateCount { get; private set; }

        public int SourceCandidatePngCount { get; private set; }

        public int SourceCandidateNoMetaCount { get; private set; }

        public int ImportedCandidatePngCount { get; private set; }

        public int ImportedCandidateMetaCount { get; private set; }

        public int DimensionMatchedCount { get; private set; }

        public int UnityPreflightBindingCount { get; private set; }

        public int FormalRuntimeBindingLeakCount { get; private set; }

        public int UnityEvidenceCount { get; private set; }

        public int UnityEvidenceRequiredCount { get; private set; }

        public bool QueueEntryReadyForUnityReview { get; private set; }

        public bool UnityEditorImportValidationReady { get; private set; }

        public bool UnityEvidenceComplete { get; private set; }

        public bool FormalRuntimeBindingDecisionApproved { get; private set; }

        public bool FormalInstallAllowed { get; private set; }

        public bool SharedConsoleClassifierContractReady { get; private set; }

        public string ConsoleClassifierPolicySummary { get; private set; } = string.Empty;

        public int FailureCount => issues.Count;

        public bool CandidatePreflightChecksReady => FailureCount == 0
            && coveredChecks.Count >= P0BattleHudBatch87UnityPreflight.ExpectedCoveredCheckCount;

        public bool IsReadyForUnityPreflight => CandidatePreflightChecksReady
            && UnityEditorImportValidationReady;

        public void SetCounts(
            int candidateCount,
            int sourceCandidatePngCount,
            int sourceCandidateNoMetaCount,
            int importedCandidatePngCount,
            int importedCandidateMetaCount,
            int dimensionMatchedCount,
            int unityPreflightBindingCount,
            int formalRuntimeBindingLeakCount,
            int unityEvidenceCount,
            int unityEvidenceRequiredCount,
            bool queueEntryReadyForUnityReview,
            bool unityEditorImportValidationReady)
        {
            CandidateCount = candidateCount;
            SourceCandidatePngCount = sourceCandidatePngCount;
            SourceCandidateNoMetaCount = sourceCandidateNoMetaCount;
            ImportedCandidatePngCount = importedCandidatePngCount;
            ImportedCandidateMetaCount = importedCandidateMetaCount;
            DimensionMatchedCount = dimensionMatchedCount;
            UnityPreflightBindingCount = unityPreflightBindingCount;
            FormalRuntimeBindingLeakCount = formalRuntimeBindingLeakCount;
            UnityEvidenceCount = unityEvidenceCount;
            UnityEvidenceRequiredCount = unityEvidenceRequiredCount;
            QueueEntryReadyForUnityReview = queueEntryReadyForUnityReview;
            UnityEditorImportValidationReady = unityEditorImportValidationReady;
            FormalInstallAllowed = false;
        }

        public void SetConsoleClassifierPolicy(bool ready, string summary)
        {
            SharedConsoleClassifierContractReady = ready;
            ConsoleClassifierPolicySummary = summary ?? string.Empty;
        }

        public void FinalizeFormalInstallGate()
        {
            UnityEvidenceComplete = IsReadyForUnityPreflight
                && UnityEvidenceRequiredCount > 0
                && UnityEvidenceCount >= UnityEvidenceRequiredCount;
            FormalRuntimeBindingDecisionApproved = false;
            FormalInstallAllowed = UnityEvidenceComplete && FormalRuntimeBindingDecisionApproved;
        }

        public void AddCandidate(P0BattleHudBatch87CandidateAsset candidate)
        {
            candidates.Add(candidate);
        }

        public void AddIssue(string issue)
        {
            if (!string.IsNullOrWhiteSpace(issue))
            {
                issues.Add(issue);
            }
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public void AddBlockingItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                blockingItems.Add(item);
            }
        }

        public string BuildSummary()
        {
            if (!CandidatePreflightChecksReady)
            {
                return "Batch 87 battle HUD Unity preflight has " + FailureCount + " failure(s).";
            }

            if (!UnityEditorImportValidationReady)
            {
                return "Batch 87 battle HUD candidates need Unity editor import validation before preflight is ready.";
            }

            if (!UnityEvidenceComplete)
            {
                return "Batch 87 battle HUD candidates are Unity-import preflight ready; formal install remains blocked by runtime evidence and approval.";
            }

            return "Batch 87 battle HUD Unity evidence is complete, but formal install remains blocked by the formal runtime binding decision.";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Candidates: " + CandidateCount,
                "Source candidate PNGs: " + SourceCandidatePngCount,
                "Source candidate PNGs without meta: " + SourceCandidateNoMetaCount,
                "Imported candidate PNGs: " + ImportedCandidatePngCount,
                "Imported candidate meta files: " + ImportedCandidateMetaCount,
                "Dimension matches: " + DimensionMatchedCount,
                "Unity preflight bindings: " + UnityPreflightBindingCount,
                "Formal runtime binding leaks: " + FormalRuntimeBindingLeakCount,
                "Unity evidence: " + UnityEvidenceCount + "/" + UnityEvidenceRequiredCount,
                "Unity evidence complete: " + (UnityEvidenceComplete ? "yes" : "no"),
                "Queue entry ready for Unity review: " + (QueueEntryReadyForUnityReview ? "yes" : "no"),
                "Unity editor import validation ready: " + (UnityEditorImportValidationReady ? "yes" : "no"),
                "Formal runtime binding decision approved: " + (FormalRuntimeBindingDecisionApproved ? "yes" : "no"),
                "Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no"),
                "Shared Console classifier contract: " + (SharedConsoleClassifierContractReady ? "ready" : "not ready"),
                "Console classifier policy: " + ConsoleClassifierPolicySummary
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < blockingItems.Count; i++)
            {
                lines.Add("[Blocked] " + blockingItems[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[Failure] " + issues[i]);
            }

            return string.Join(Environment.NewLine, lines);
        }

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 87 Battle HUD Unity Preflight");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Decision");
            builder.AppendLine();
            builder.AppendLine("- Ready for Unity preflight: " + (IsReadyForUnityPreflight ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no"));
            builder.AppendLine("- Unity editor import validation ready: " + (UnityEditorImportValidationReady ? "yes" : "no"));
            builder.AppendLine("- Runtime evidence: " + UnityEvidenceCount + "/" + UnityEvidenceRequiredCount);
            builder.AppendLine("- Unity evidence complete: " + (UnityEvidenceComplete ? "yes" : "no"));
            builder.AppendLine("- Formal runtime binding decision approved: " + (FormalRuntimeBindingDecisionApproved ? "yes" : "no"));
            builder.AppendLine("- Candidate policy: `candidate-backed Unity preflight only`");
            builder.AppendLine("- Shared Console classifier: " + (SharedConsoleClassifierContractReady ? "active strict-clean contract" : "not ready"));
            builder.AppendLine("- Console classifier policy: " + EscapeTable(ConsoleClassifierPolicySummary));
            builder.AppendLine();
            builder.AppendLine("## Candidate Imports");
            builder.AppendLine();
            builder.AppendLine("| component | variant | source candidate | Unity preflight import | size |");
            builder.AppendLine("| --- | --- | --- | --- | --- |");

            for (int i = 0; i < candidates.Count; i++)
            {
                P0BattleHudBatch87CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                builder.Append("| ");
                builder.Append(EscapeTable(candidate.ComponentId));
                builder.Append(" | ");
                builder.Append(EscapeTable(candidate.VariantId));
                builder.Append(" | `");
                builder.Append(EscapeTable(candidate.SourceCandidatePath));
                builder.Append("` | `");
                builder.Append(EscapeTable(entry.UnityImportPath));
                builder.Append("` | ");
                builder.Append(EscapeTable(entry.Size));
                builder.AppendLine(" |");
            }

            builder.AppendLine();
            builder.AppendLine("## Blocking Runtime Evidence");
            builder.AppendLine();
            if (blockingItems.Count == 0)
            {
                builder.AppendLine("- none");
            }
            else
            {
                for (int i = 0; i < blockingItems.Count; i++)
                {
                    builder.AppendLine("- " + blockingItems[i]);
                }
            }

            builder.AppendLine();
            builder.AppendLine("## Protected Runtime State");
            builder.AppendLine();
            builder.AppendLine("- Human review request packet: `" + P0BattleHudBatch87UnityPreflight.HumanReviewRequestPath + "`.");
            builder.AppendLine("- Formal runtime binding decision request packet: `" + P0BattleHudBatch87UnityPreflight.FormalRuntimeBindingDecisionRequestPath + "`.");
            builder.AppendLine("- Batch 87 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.");
            builder.AppendLine("- Current IMGUI battle HUD remains the playable runtime path until screenshots, Console, click-target, and review gates pass.");
            builder.AppendLine("- Do not mark Batch 87 as formally installed before explicit review approval.");

            if (issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                builder.AppendLine();
                for (int i = 0; i < issues.Count; i++)
                {
                    builder.AppendLine("- [Failure] " + issues[i]);
                }
            }

            return builder.ToString();
        }

        public string BuildHumanReviewRequestMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 87 Battle HUD Human Review Request");
            builder.AppendLine();
            builder.AppendLine("This is a review-request packet only. It is not `human_review_approval.md` and must not be counted as formal approval evidence.");
            builder.AppendLine();
            builder.AppendLine("## Decision Boundary");
            builder.AppendLine();
            builder.AppendLine("- Review request ready: " + (IsReadyForUnityPreflight ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no"));
            builder.AppendLine("- Unity evidence complete: " + (UnityEvidenceComplete ? "yes" : "no"));
            builder.AppendLine("- Formal runtime binding decision approved: " + (FormalRuntimeBindingDecisionApproved ? "yes" : "no"));
            builder.AppendLine("- Candidate policy: `candidate-backed Unity preflight only`");
            builder.AppendLine("- Shared Console classifier: " + (SharedConsoleClassifierContractReady ? "active strict-clean contract" : "not ready"));
            builder.AppendLine("- Console classifier policy: " + EscapeTable(ConsoleClassifierPolicySummary));
            builder.AppendLine();
            builder.AppendLine("## Evidence Packet");
            builder.AppendLine();
            builder.AppendLine("- Preflight report: `" + P0BattleHudBatch87UnityPreflight.ReportPath + "`");
            builder.AppendLine("- Formal runtime binding decision request: `" + P0BattleHudBatch87UnityPreflight.FormalRuntimeBindingDecisionRequestPath + "`");
            builder.AppendLine("- Runtime evidence report: `" + P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath + "`");
            builder.AppendLine("- Evidence triage: `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_EVIDENCE_TRIAGE_2026-06-25.md`");
            builder.AppendLine("- Console blockers: `design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_CONSOLE_BLOCKERS_2026-06-25.md`");
            for (int i = 0; i < P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                builder.AppendLine("- Screenshot: `" + P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[i].EvidencePath + "`");
            }

            builder.AppendLine("- Automatic review: `" + P0BattleHudBatch87RuntimeEvidence.TextAndSkillStateReviewPath + "`");
            builder.AppendLine("- Automatic review: `" + P0BattleHudBatch87RuntimeEvidence.TelegraphOcclusionClickTargetReviewPath + "`");
            builder.AppendLine();
            builder.AppendLine("## Reviewer Checklist");
            builder.AppendLine();
            builder.AppendLine("- Confirm the four candidate-backed screenshots remain readable at 1920x1080, 1365x768, 1280x720, and 1024x768.");
            builder.AppendLine("- Confirm battle HUD Chinese copy, gauge values, skill state language, enemy pressure, and runtime controls match the live P0 design intent.");
            builder.AppendLine("- Confirm skill trays, warning/telegraph areas, pause/speed/restart controls, and click targets remain readable and non-overlapping.");
            builder.AppendLine("- Confirm Batch 87 remains outside `P0VisualAssetCatalog` until strict-clean Console evidence and a formal runtime binding decision exist.");
            builder.AppendLine("- Confirm no starter-cat source-lock, body-art replacement, or unrelated UI candidate lane is implicitly approved by this review.");
            builder.AppendLine();
            builder.AppendLine("## Required Before Approval File");
            builder.AppendLine();
            builder.AppendLine("- `console_clean_report.md` must reference an existing Batch 87 runtime evidence log that passes the shared `StrictClean` Console classifier.");
            builder.AppendLine("- A human reviewer must explicitly approve formal install in a separate `human_review_approval.md` file with the required reviewer and approval-date tokens.");
            builder.AppendLine("- The formal runtime binding decision must explicitly approve adding Batch 87 to the runtime scene/presenter/catalog path.");
            builder.AppendLine();
            builder.AppendLine("## Current Blocking Items");
            builder.AppendLine();
            if (blockingItems.Count == 0)
            {
                builder.AppendLine("- none");
            }
            else
            {
                for (int i = 0; i < blockingItems.Count; i++)
                {
                    builder.AppendLine("- " + blockingItems[i]);
                }
            }

            builder.AppendLine();
            builder.AppendLine("## Protected Runtime State");
            builder.AppendLine();
            builder.AppendLine("- Do not create or count `human_review_approval.md` from this request packet.");
            builder.AppendLine("- Do not create or count `console_clean_report.md` until the referenced Unity log is strict clean.");
            builder.AppendLine("- Do not add Batch 87 candidate ids to formal runtime bindings from this request packet.");
            builder.AppendLine("- Keep the current IMGUI battle HUD as the playable runtime path.");

            return builder.ToString();
        }

        public string BuildFormalRuntimeBindingDecisionRequestMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 87 Battle HUD Formal Runtime Binding Decision Request");
            builder.AppendLine();
            builder.AppendLine("This is a formal-runtime-binding decision request only. It is not an approval file, does not change runtime bindings, and must not be counted as formal install evidence.");
            builder.AppendLine();
            builder.AppendLine("## Decision Boundary");
            builder.AppendLine();
            builder.AppendLine("- Decision request ready: " + (IsReadyForUnityPreflight ? "yes" : "no"));
            builder.AppendLine("- Formal runtime binding decision approved: " + (FormalRuntimeBindingDecisionApproved ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no"));
            builder.AppendLine("- Unity evidence complete: " + (UnityEvidenceComplete ? "yes" : "no"));
            builder.AppendLine("- Candidate policy: `candidate-backed Unity preflight only`");
            builder.AppendLine("- Shared Console classifier: " + (SharedConsoleClassifierContractReady ? "active strict-clean contract" : "not ready"));
            builder.AppendLine("- Console classifier policy: " + EscapeTable(ConsoleClassifierPolicySummary));
            builder.AppendLine();
            builder.AppendLine("## Proposed Binding Scope");
            builder.AppendLine();
            builder.AppendLine("- Surface: battle HUD runtime visual frames and supporting HUD panels.");
            builder.AppendLine("- Candidate import root: `Assets/TheCat/Art/UI/BattleHUD`.");
            builder.AppendLine("- Runtime catalog under review: `P0VisualAssetCatalog.CreateP0RuntimeBindings()`.");
            builder.AppendLine("- Current playable runtime path: IMGUI battle HUD, protected until all gates pass.");
            builder.AppendLine("- Candidate component count: " + CandidateCount);
            builder.AppendLine();
            builder.AppendLine("| component | variant | Unity preflight import | size |");
            builder.AppendLine("| --- | --- | --- | --- |");
            for (int i = 0; i < candidates.Count; i++)
            {
                P0BattleHudBatch87CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                builder.Append("| ");
                builder.Append(EscapeTable(candidate.ComponentId));
                builder.Append(" | ");
                builder.Append(EscapeTable(candidate.VariantId));
                builder.Append(" | `");
                builder.Append(EscapeTable(entry.UnityImportPath));
                builder.Append("` | ");
                builder.Append(EscapeTable(entry.Size));
                builder.AppendLine(" |");
            }

            builder.AppendLine();
            builder.AppendLine("## Evidence Required Before Decision");
            builder.AppendLine();
            builder.AppendLine("- Batch 87 preflight report remains ready and linked: `" + P0BattleHudBatch87UnityPreflight.ReportPath + "`.");
            builder.AppendLine("- Human review approval exists separately; this request packet is not that approval.");
            builder.AppendLine("- Clean Console evidence exists separately and passes the shared `StrictClean` classifier.");
            builder.AppendLine("- Runtime binding review confirms which scene, presenter, and catalog path will consume the candidate frames.");
            builder.AppendLine("- Post-binding validation plan covers four-resolution screenshots, click targets, warning/telegraph visibility, and rollback.");
            builder.AppendLine();
            builder.AppendLine("## Decision Questions");
            builder.AppendLine();
            builder.AppendLine("- Should Batch 87 replace any current IMGUI battle HUD framing, or remain an overlay/reference until a later UI-system pass?");
            builder.AppendLine("- Which exact `P0VisualAssetCatalog` rows would be added or changed if the decision later passes?");
            builder.AppendLine("- Which presenter/controller owns layout responsibilities after binding, and which test proves no click-target or text regression?");
            builder.AppendLine("- What rollback condition returns the battle HUD to the current playable IMGUI path?");
            builder.AppendLine("- Does this decision intentionally exclude starter-cat body art, other UI candidate batches, and final visual acceptance?");
            builder.AppendLine();
            builder.AppendLine("## Current Blocking Items");
            builder.AppendLine();
            if (blockingItems.Count == 0)
            {
                builder.AppendLine("- none");
            }
            else
            {
                for (int i = 0; i < blockingItems.Count; i++)
                {
                    builder.AppendLine("- " + blockingItems[i]);
                }
            }

            builder.AppendLine();
            builder.AppendLine("## Protected Runtime State");
            builder.AppendLine();
            builder.AppendLine("- Do not add Batch 87 candidate ids to `P0VisualAssetCatalog` from this request packet.");
            builder.AppendLine("- Do not alter battle HUD scene, presenter, or controller runtime binding from this request packet.");
            builder.AppendLine("- Do not create or count clean Console or human approval evidence from this request packet.");
            builder.AppendLine("- Keep the current IMGUI battle HUD as the playable runtime path.");

            return builder.ToString();
        }

        private static string EscapeTable(string value)
        {
            return (value ?? string.Empty)
                .Replace("|", "\\|")
                .Replace("\r", " ")
                .Replace("\n", " ");
        }
    }

    public static class P0BattleHudBatch87UnityPreflight
    {
        public const int ExpectedCoveredCheckCount = 9;
        public const int ExpectedUnityEvidenceRequiredCount = 8;
        public const string ConsoleClassifierPolicySummary = P0FormalInstallGate.ConsoleClassifierPolicySummary;
        public const string ReportPath = "design/development/asset_review/BATCH87_BATTLE_HUD_UNITY_PREFLIGHT_REPORT_2026-06-25.md";
        public const string HumanReviewRequestPath = "design/development/asset_review/BATCH87_BATTLE_HUD_HUMAN_REVIEW_REQUEST_2026-06-26.md";
        public const string FormalRuntimeBindingDecisionRequestPath = "design/development/asset_review/BATCH87_BATTLE_HUD_FORMAL_RUNTIME_BINDING_DECISION_REQUEST_2026-06-26.md";
        private const string RuntimeEvidenceLogPrefix = "Batch 87 runtime evidence log:";
        private const string RuntimeEvidencePassedLogToken = "[TheCat] Batch 87 battle HUD runtime evidence passed:";

        public static readonly string[] RequiredUnityEvidencePaths =
        {
            "design/development/screenshots/batch_87_battle_hud_unity_preflight/01-battle-hud-batch87-1920x1080.png",
            "design/development/screenshots/batch_87_battle_hud_unity_preflight/02-battle-hud-batch87-1365x768.png",
            "design/development/screenshots/batch_87_battle_hud_unity_preflight/03-battle-hud-batch87-1280x720.png",
            "design/development/screenshots/batch_87_battle_hud_unity_preflight/04-battle-hud-batch87-1024x768.png",
            "design/development/asset_review/batch_87_battle_hud_unity_preflight/console_clean_report.md",
            "design/development/asset_review/batch_87_battle_hud_unity_preflight/text_and_skill_state_review.md",
            "design/development/asset_review/batch_87_battle_hud_unity_preflight/telegraph_occlusion_click_target_review.md",
            "design/development/asset_review/batch_87_battle_hud_unity_preflight/human_review_approval.md"
        };

        public static P0BattleHudBatch87UnityPreflightReport EvaluateCurrentPreflight()
        {
            return Evaluate(
                P0BattleHudBatch87CandidateCatalog.CreateCandidates(),
                DefaultFileExists,
                DefaultReadPngDimensions,
                unityEditorImportValidationReady: false);
        }

        public static P0BattleHudBatch87UnityPreflightReport EvaluateCurrentPreflight(bool unityEditorImportValidationReady)
        {
            return Evaluate(
                P0BattleHudBatch87CandidateCatalog.CreateCandidates(),
                DefaultFileExists,
                DefaultReadPngDimensions,
                unityEditorImportValidationReady);
        }

        public static P0BattleHudBatch87UnityPreflightReport Evaluate(
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady)
        {
            return Evaluate(
                candidates,
                fileExists,
                readPngDimensions,
                unityEditorImportValidationReady,
                DefaultReadText);
        }

        public static P0BattleHudBatch87UnityPreflightReport Evaluate(
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady,
            Func<string, string> readText)
        {
            return Evaluate(
                candidates,
                fileExists,
                readPngDimensions,
                unityEditorImportValidationReady,
                readText,
                DefaultHasUsefulPngVisualContent);
        }

        public static P0BattleHudBatch87UnityPreflightReport Evaluate(
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady,
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            P0BattleHudBatch87UnityPreflightReport report = new P0BattleHudBatch87UnityPreflightReport();
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            P0AssetPngDimensionReader readDimensions = readPngDimensions ?? DefaultReadPngDimensions;
            Func<string, string> readEvidenceText = readText ?? DefaultReadText;
            P0AssetPngVisualContentReader readScreenshotContent = readVisualContent ?? DefaultHasUsefulPngVisualContent;

            int candidateCount = candidates == null ? 0 : candidates.Count;
            int sourceCandidatePngCount = 0;
            int sourceCandidateNoMetaCount = 0;
            int importedCandidatePngCount = 0;
            int importedCandidateMetaCount = 0;
            int dimensionMatchedCount = 0;

            if (candidates != null)
            {
                for (int i = 0; i < candidates.Count; i++)
                {
                    P0BattleHudBatch87CandidateAsset candidate = candidates[i];
                    P0AssetManifestEntry entry = candidate.ManifestEntry;
                    report.AddCandidate(candidate);

                    if (IsDesignCandidatePath(candidate.SourceCandidatePath) && exists(candidate.SourceCandidatePath))
                    {
                        sourceCandidatePngCount++;
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " source candidate is missing or outside design asset candidates: " + candidate.SourceCandidatePath);
                    }

                    if (!exists(candidate.SourceCandidatePath + ".meta"))
                    {
                        sourceCandidateNoMetaCount++;
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " source candidate has a Unity meta file; candidate source should stay review-only.");
                    }

                    if (IsBattleHudUnityImportPath(entry.UnityImportPath) && exists(entry.UnityImportPath))
                    {
                        importedCandidatePngCount++;
                        if (ValidateDimensions(entry, readDimensions, report))
                        {
                            dimensionMatchedCount++;
                        }
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " Unity preflight import is missing or outside BattleHUD: " + entry.UnityImportPath);
                    }

                    if (exists(entry.UnityImportPath + ".meta"))
                    {
                        importedCandidateMetaCount++;
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " Unity preflight import is missing .meta file: " + entry.UnityImportPath + ".meta");
                    }
                }
            }

            int unityPreflightBindingCount = CountReadyBindings(P0BattleHudBatch87CandidateCatalog.CreateUnityPreflightBindings());
            int formalRuntimeBindingLeakCount = CountFormalRuntimeBindingLeaks();
            int unityEvidenceCount = CountUnityEvidence(exists, readEvidenceText, readDimensions, readScreenshotContent, report);
            bool queueEntryReadyForUnityReview = IsQueueEntryReadyForUnityReview();
            bool sharedConsoleClassifierContractReady = IsSharedConsoleClassifierContractReady();

            report.SetCounts(
                candidateCount,
                sourceCandidatePngCount,
                sourceCandidateNoMetaCount,
                importedCandidatePngCount,
                importedCandidateMetaCount,
                dimensionMatchedCount,
                unityPreflightBindingCount,
                formalRuntimeBindingLeakCount,
                unityEvidenceCount,
                RequiredUnityEvidencePaths.Length,
                queueEntryReadyForUnityReview,
                unityEditorImportValidationReady);
            report.SetConsoleClassifierPolicy(
                sharedConsoleClassifierContractReady,
                ConsoleClassifierPolicySummary);

            Require(report, candidateCount == P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, "Batch 87 catalog declares six battle HUD component candidates.", "Batch 87 candidate catalog count is stale.");
            Require(report, sourceCandidatePngCount == candidateCount && sourceCandidateNoMetaCount == candidateCount, "Batch 87 source candidates stay in design asset candidates and have no Unity meta files.", "Batch 87 source candidate policy is unsafe.");
            Require(report, importedCandidatePngCount == candidateCount, "Batch 87 Unity preflight imports exist under Assets/TheCat/Art/UI/BattleHUD.", "Batch 87 Unity preflight imports are incomplete.");
            Require(report, importedCandidateMetaCount == candidateCount, "Batch 87 Unity preflight imports have Unity meta files.", "Batch 87 Unity preflight import meta files are incomplete.");
            Require(report, dimensionMatchedCount == candidateCount, "Batch 87 Unity preflight PNG dimensions match the candidate manifest.", "Batch 87 Unity preflight PNG dimensions do not match the manifest.");
            Require(report, unityPreflightBindingCount == candidateCount, "Batch 87 preflight bindings cover all imported HUD components outside the formal runtime catalog.", "Batch 87 preflight bindings are incomplete.");
            Require(report, formalRuntimeBindingLeakCount == 0, "Batch 87 candidate ids do not leak into formal P0 runtime visual bindings.", "Batch 87 candidates leaked into formal runtime bindings.");
            Require(report, queueEntryReadyForUnityReview, "Batch 87 queue entry is ready for candidate-backed Unity review.", "Batch 87 queue entry is not ready for candidate-backed Unity review.");
            Require(report, sharedConsoleClassifierContractReady, "Batch 87 clean-Console evidence is bound to the shared strict-clean Unity Console classifier contract.", "Batch 87 clean-Console evidence is not bound to the shared Unity Console classifier contract.");
            if (!unityEditorImportValidationReady)
            {
                report.AddBlockingItem("Unity editor import validation has not passed.");
            }

            report.FinalizeFormalInstallGate();
            if (!report.FormalRuntimeBindingDecisionApproved)
            {
                report.AddBlockingItem("Formal runtime binding decision has not passed.");
            }

            if (!report.FormalInstallAllowed)
            {
                report.AddCoveredCheck("Batch 87 remains blocked from formal install until runtime evidence, human review, and formal runtime binding decision pass.");
            }

            return report;
        }

        private static bool ValidateDimensions(
            P0AssetManifestEntry entry,
            P0AssetPngDimensionReader readDimensions,
            P0BattleHudBatch87UnityPreflightReport report)
        {
            if (!P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out int expectedWidth, out int expectedHeight))
            {
                report.AddIssue(entry.AssetId + " has unsupported size '" + entry.Size + "'.");
                return false;
            }

            if (!readDimensions(entry.UnityImportPath, out int actualWidth, out int actualHeight, out string error))
            {
                report.AddIssue(entry.AssetId + " could not read PNG dimensions: " + error);
                return false;
            }

            if (actualWidth == expectedWidth && actualHeight == expectedHeight)
            {
                return true;
            }

            report.AddIssue(entry.AssetId + " expected " + expectedWidth + "x" + expectedHeight + " but found " + actualWidth + "x" + actualHeight + ".");
            return false;
        }

        private static int CountReadyBindings(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            if (bindings == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].IsReady
                    && bindings[i].SurfaceId == "battle_hud_preflight"
                    && ContainsOrdinal(bindings[i].SourceAuthority, "not formal runtime binding"))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountFormalRuntimeBindingLeaks()
        {
            int count = 0;
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates = P0BattleHudBatch87CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            for (int runtimeIndex = 0; runtimeIndex < runtimeBindings.Length; runtimeIndex++)
            {
                for (int candidateIndex = 0; candidateIndex < candidates.Count; candidateIndex++)
                {
                    P0AssetManifestEntry entry = candidates[candidateIndex].ManifestEntry;
                    if (runtimeBindings[runtimeIndex].Asset.AssetId == entry.AssetId
                        || runtimeBindings[runtimeIndex].Asset.UnityImportPath == entry.UnityImportPath)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int CountUnityEvidence(
            Func<string, bool> exists,
            Func<string, string> readText,
            P0AssetPngDimensionReader readPngDimensions,
            P0AssetPngVisualContentReader readVisualContent,
            P0BattleHudBatch87UnityPreflightReport report)
        {
            int count = 0;
            for (int i = 0; i < RequiredUnityEvidencePaths.Length; i++)
            {
                string path = RequiredUnityEvidencePaths[i];
                if (!exists(path))
                {
                    report.AddBlockingItem("Missing Unity evidence: `" + path + "`");
                    continue;
                }

                if (IsScreenshotEvidencePath(path)
                    && !IsScreenshotEvidenceComplete(path, readPngDimensions, readVisualContent, readText, report))
                {
                    continue;
                }

                if (IsMarkdownEvidencePath(path)
                    && !IsMarkdownEvidenceComplete(path, exists, readText, report))
                {
                    continue;
                }

                count++;
            }

            return count;
        }

        private static bool IsScreenshotEvidencePath(string path)
        {
            return (path ?? string.Empty).EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                && ContainsOrdinal(path, "design/development/screenshots/batch_87_battle_hud_unity_preflight/");
        }

        private static bool IsScreenshotEvidenceComplete(
            string path,
            P0AssetPngDimensionReader readPngDimensions,
            P0AssetPngVisualContentReader readVisualContent,
            Func<string, string> readText,
            P0BattleHudBatch87UnityPreflightReport report)
        {
            if (!TryGetExpectedScreenshotDimensions(path, out int expectedWidth, out int expectedHeight))
            {
                report.AddBlockingItem("Unknown Unity screenshot evidence target: `" + path + "`");
                return false;
            }

            if (!readPngDimensions(path, out int actualWidth, out int actualHeight, out string error))
            {
                report.AddBlockingItem("Unreadable Unity screenshot evidence: `" + path + "` (" + error + ")");
                return false;
            }

            if (actualWidth == expectedWidth && actualHeight == expectedHeight)
            {
                if (!readVisualContent(path, out string visualSummary, out string visualError))
                {
                    report.AddBlockingItem(
                        "Invalid Unity screenshot evidence: `"
                        + path
                        + "` is visually blank or incomplete ("
                        + visualSummary
                        + (string.IsNullOrWhiteSpace(visualError) ? string.Empty : "; " + visualError)
                        + ").");
                    return false;
                }

                return RuntimeReportConfirmsScreenshot(path, readText, report);
            }

            report.AddBlockingItem(
                "Invalid Unity screenshot evidence: `"
                + path
                + "` expected "
                + expectedWidth
                + "x"
                + expectedHeight
                + " but found "
                + actualWidth
                + "x"
                + actualHeight
                + ".");
            return false;
        }

        private static bool RuntimeReportConfirmsScreenshot(
            string path,
            Func<string, string> readText,
            P0BattleHudBatch87UnityPreflightReport report)
        {
            string content;
            try
            {
                content = readText(P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Missing Batch 87 runtime report for screenshot evidence: `"
                    + P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath
                    + "` ("
                    + exception.GetType().Name
                    + ")");
                return false;
            }

            if (!ContainsOrdinalIgnoreCase(content, P0BattleHudBatch87RuntimeEvidence.CompleteScreenshotEvidenceToken)
                || !ContainsOrdinalIgnoreCase(content, P0BattleHudBatch87RuntimeEvidence.CandidateFrameDrawToken)
                || !ContainsOrdinalIgnoreCase(content, P0BattleHudBatch87RuntimeEvidence.NoCandidateTextureFallbackToken)
                || !ContainsOrdinal(content, path))
            {
                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not confirmed by a complete candidate-drawn Batch 87 runtime evidence report requiring `"
                    + P0BattleHudBatch87RuntimeEvidence.CompleteScreenshotEvidenceToken
                    + "`, `"
                    + P0BattleHudBatch87RuntimeEvidence.CandidateFrameDrawToken
                    + "`, and `"
                    + P0BattleHudBatch87RuntimeEvidence.NoCandidateTextureFallbackToken
                    + "`.");
                return false;
            }

            return true;
        }

        private static bool TryGetExpectedScreenshotDimensions(string path, out int width, out int height)
        {
            for (int i = 0; i < P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                P0BattleHudBatch87ScreenshotTarget target = P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[i];
                if (target.EvidencePath == path)
                {
                    width = target.Width;
                    height = target.Height;
                    return true;
                }
            }

            width = 0;
            height = 0;
            return false;
        }

        private static bool IsMarkdownEvidencePath(string path)
        {
            return (path ?? string.Empty).EndsWith(".md", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsMarkdownEvidenceComplete(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0BattleHudBatch87UnityPreflightReport report)
        {
            string content;
            try
            {
                content = readText(path) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable Unity evidence: `" + path + "` (" + exception.GetType().Name + ")");
                return false;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` is empty.");
                return false;
            }

            if (!ContainsOrdinalIgnoreCase(content, "Batch 87"))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not name Batch 87.");
                return false;
            }

            if (path.EndsWith("console_clean_report.md", StringComparison.Ordinal))
            {
                if (!RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[] { "Console clean: yes", "Unity log reviewed: yes", RuntimeEvidenceLogPrefix }))
                {
                    return false;
                }

                return ValidateConsoleCleanReport(path, content, exists, readText, report);
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[] { "Formal install approved: yes", "Reviewer:", "Approval date:" });
            }

            return RequireEvidenceToken(
                report,
                path,
                content,
                "Review result: pass",
                "PASS");
        }

        private static bool ValidateConsoleCleanReport(
            string path,
            string content,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0BattleHudBatch87UnityPreflightReport report)
        {
            if (!TryExtractEvidenceValue(content, RuntimeEvidenceLogPrefix, out string logPath))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not identify a runtime evidence log path.");
                return false;
            }

            if (!exists(logPath))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references missing runtime evidence log `" + logPath + "`.");
                return false;
            }

            string logContent;
            try
            {
                logContent = readText(logPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable runtime evidence log: `" + logPath + "` (" + exception.GetType().Name + ")");
                return false;
            }

            if (!ContainsOrdinalIgnoreCase(logContent, RuntimeEvidencePassedLogToken))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references a log without the Batch 87 runtime pass token.");
                return false;
            }

            P0UnityConsoleLogClassifierReport consoleLogReport = P0UnityConsoleLogClassifier.Classify(logContent);
            if (!consoleLogReport.StrictClean)
            {
                report.AddBlockingItem("Console clean report is not accepted because `"
                    + logPath
                    + "` is not strict clean; first signal `"
                    + consoleLogReport.FirstSignalToken
                    + "`. "
                    + consoleLogReport.BuildSummary()
                    + " Policy: "
                    + ConsoleClassifierPolicySummary);
                return false;
            }

            return true;
        }

        private static bool IsSharedConsoleClassifierContractReady()
        {
            string cleanLog = RuntimeEvidencePassedLogToken + " clean synthetic pass\n";
            P0UnityConsoleLogClassifierReport clean = P0UnityConsoleLogClassifier.Classify(cleanLog);

            string knownNoiseLog = RuntimeEvidencePassedLogToken
                + " synthetic pass with Unity environment noise\n"
                + "[Licensing::Client] Error: HandshakeResponse reported an error\n"
                + "Unity.AI.Tracing.ConsoleSink:LogToConsole (string,string,System.Exception)\n";
            P0UnityConsoleLogClassifierReport knownNoise = P0UnityConsoleLogClassifier.Classify(knownNoiseLog);

            string unknownBlockingLog = RuntimeEvidencePassedLogToken
                + " synthetic pass before unknown plugin failure\n"
                + "SomePlugin Error: missing battle HUD binding\n";
            P0UnityConsoleLogClassifierReport unknownBlocking = P0UnityConsoleLogClassifier.Classify(unknownBlockingLog);

            return clean.StrictClean
                && clean.ProjectOwnedClean
                && !knownNoise.StrictClean
                && knownNoise.ProjectOwnedClean
                && knownNoise.KnownEnvironmentNoiseCount > 0
                && !unknownBlocking.StrictClean
                && !unknownBlocking.ProjectOwnedClean
                && unknownBlocking.UnknownBlockingTokenCount > 0;
        }

        private static bool TryExtractEvidenceValue(string content, string prefix, out string value)
        {
            value = string.Empty;
            string[] lines = (content ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i] ?? string.Empty;
                int index = line.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
                if (index < 0)
                {
                    continue;
                }

                value = line.Substring(index + prefix.Length).Trim();
                value = value.Trim('`', '"', '\'', ' ');
                return !string.IsNullOrWhiteSpace(value);
            }

            return false;
        }

        private static bool RequireEvidenceToken(
            P0BattleHudBatch87UnityPreflightReport report,
            string path,
            string content,
            string primaryToken,
            string alternateToken)
        {
            if (ContainsOrdinalIgnoreCase(content, primaryToken)
                || ContainsOrdinalIgnoreCase(content, alternateToken))
            {
                return true;
            }

            report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` is missing an approval token.");
            return false;
        }

        private static bool RequireEvidenceTokens(
            P0BattleHudBatch87UnityPreflightReport report,
            string path,
            string content,
            IReadOnlyList<string> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (ContainsOrdinalIgnoreCase(content, tokens[i]))
                {
                    continue;
                }

                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` is missing `" + tokens[i] + "`.");
                return false;
            }

            return true;
        }

        private static string DefaultReadText(string path)
        {
            return File.ReadAllText(ResolveProjectPath(path));
        }

        private static bool ContainsOrdinalIgnoreCase(string value, string token)
        {
            return (value ?? string.Empty).IndexOf(token ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsQueueEntryReadyForUnityReview()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();
            for (int i = 0; i < queue.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queue[i];
                if (entry.QueueId == P0AssetProductionQueueCatalog.BattleHudPreflightCandidateQueueId)
                {
                    return entry.State == P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview
                        && entry.Phase == P0AssetProductionQueuePhase.CodexCandidateProduction
                        && ContainsOrdinal(entry.CandidateDirectory, P0BattleHudBatch87CandidateCatalog.BatchSlug)
                        && ContainsOrdinal(entry.UnityImportRoot, "Assets/TheCat/Art/UI/BattleHUD")
                        && ContainsOrdinal(entry.NextAction, "Batch 87")
                        && ContainsOrdinal(entry.NextAction, "battle HUD")
                        && ContainsOrdinal(entry.NextAction, "candidate-backed")
                        && ContainsOrdinal(entry.NextAction, "clean Console")
                        && ContainsOrdinal(entry.NextAction, "formal runtime binding decision");
                }
            }

            return false;
        }

        private static bool IsDesignCandidatePath(string path)
        {
            return ContainsOrdinal(path, "design/development/asset_candidates/")
                && !ContainsOrdinal(path, "Assets/");
        }

        private static bool IsBattleHudUnityImportPath(string path)
        {
            return ContainsOrdinal(path, "Assets/TheCat/Art/UI/BattleHUD/")
                && !ContainsOrdinal(path, "design/development/asset_candidates/");
        }

        private static bool ContainsOrdinal(string value, string token)
        {
            return (value ?? string.Empty).IndexOf(token ?? string.Empty, StringComparison.Ordinal) >= 0;
        }

        private static bool DefaultFileExists(string path)
        {
            return File.Exists(ResolveProjectPath(path));
        }

        private static bool DefaultReadPngDimensions(string unityImportPath, out int width, out int height, out string error)
        {
            width = 0;
            height = 0;
            error = string.Empty;

            try
            {
                using (FileStream stream = File.OpenRead(ResolveProjectPath(unityImportPath)))
                {
                    byte[] header = new byte[24];
                    int read = stream.Read(header, 0, header.Length);
                    if (read < header.Length)
                    {
                        error = "file is shorter than a PNG header";
                        return false;
                    }

                    if (header[0] != 0x89
                        || header[1] != 0x50
                        || header[2] != 0x4E
                        || header[3] != 0x47
                        || header[4] != 0x0D
                        || header[5] != 0x0A
                        || header[6] != 0x1A
                        || header[7] != 0x0A
                        || header[12] != 0x49
                        || header[13] != 0x48
                        || header[14] != 0x44
                        || header[15] != 0x52)
                    {
                        error = "file header is not PNG IHDR";
                        return false;
                    }

                    width = ReadBigEndianInt32(header, 16);
                    height = ReadBigEndianInt32(header, 20);
                    return width > 0 && height > 0;
                }
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static bool DefaultHasUsefulPngVisualContent(string path, out string summary, out string error)
        {
            summary = string.Empty;
            error = string.Empty;

            try
            {
                byte[] bytes = File.ReadAllBytes(ResolveProjectPath(path));
                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                try
                {
                    if (!ImageConversion.LoadImage(texture, bytes, markNonReadable: false))
                    {
                        error = "LoadImage returned false";
                        return false;
                    }

                    return P0BattleHudBatch87RuntimeEvidence.HasUsefulVisualContent(
                        texture.GetPixels32(),
                        texture.width,
                        texture.height,
                        out summary);
                }
                finally
                {
                    UnityEngine.Object.DestroyImmediate(texture);
                }
            }
            catch (Exception exception)
            {
                error = exception.GetType().Name + ": " + exception.Message;
                return false;
            }
        }

        private static int ReadBigEndianInt32(byte[] bytes, int startIndex)
        {
            return (bytes[startIndex] << 24)
                | (bytes[startIndex + 1] << 16)
                | (bytes[startIndex + 2] << 8)
                | bytes[startIndex + 3];
        }

        private static string ResolveProjectPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path))
            {
                return path ?? string.Empty;
            }

            string normalized = path.Replace('/', Path.DirectorySeparatorChar);
            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, normalized);
                if (File.Exists(candidate) || Directory.Exists(Path.GetDirectoryName(candidate) ?? string.Empty))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return normalized;
        }

        private static void Require(
            P0BattleHudBatch87UnityPreflightReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(failureMessage);
        }
    }
}
