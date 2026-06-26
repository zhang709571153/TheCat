using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Tools
{
    public sealed class P0CatRoomBatch90UnityPreflightReport
    {
        private readonly List<string> issues = new List<string>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<string> blockingItems = new List<string>();
        private readonly List<P0CatRoomBatch90CandidateAsset> candidates = new List<P0CatRoomBatch90CandidateAsset>();

        public IReadOnlyList<string> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<string> BlockingItems => blockingItems.AsReadOnly();

        public IReadOnlyList<P0CatRoomBatch90CandidateAsset> Candidates => candidates.AsReadOnly();

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

        public bool FormalInstallAllowed { get; private set; }

        public int FailureCount => issues.Count;

        public bool CandidatePreflightChecksReady => FailureCount == 0
            && coveredChecks.Count >= P0CatRoomBatch90UnityPreflight.ExpectedCoveredCheckCount;

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

        public void FinalizeFormalInstallGate()
        {
            FormalInstallAllowed = IsReadyForUnityPreflight
                && UnityEvidenceRequiredCount > 0
                && UnityEvidenceCount >= UnityEvidenceRequiredCount
                && blockingItems.Count == 0;
        }

        public void AddCandidate(P0CatRoomBatch90CandidateAsset candidate)
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
                return "Batch 90 cat-room Unity preflight has " + FailureCount + " failure(s).";
            }

            if (!UnityEditorImportValidationReady)
            {
                return "Batch 90 cat-room candidates need Unity editor import validation before preflight is ready.";
            }

            return "Batch 90 cat-room candidates are Unity-import preflight ready; formal install remains blocked by screenshots, interaction semantics, Console, and approval gates.";
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
                "Queue entry ready for Unity review: " + (QueueEntryReadyForUnityReview ? "yes" : "no"),
                "Unity editor import validation ready: " + (UnityEditorImportValidationReady ? "yes" : "no"),
                "Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no")
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
            builder.AppendLine("# Batch 90 Cat Room Unity Preflight");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Decision");
            builder.AppendLine();
            builder.AppendLine("- Ready for Unity preflight: " + (IsReadyForUnityPreflight ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no"));
            builder.AppendLine("- Unity editor import validation ready: " + (UnityEditorImportValidationReady ? "yes" : "no"));
            builder.AppendLine("- Runtime evidence: " + UnityEvidenceCount + "/" + UnityEvidenceRequiredCount);
            builder.AppendLine("- Candidate policy: `candidate-backed Unity preflight only`");
            builder.AppendLine();
            builder.AppendLine("## Candidate Imports");
            builder.AppendLine();
            builder.AppendLine("| component | variant | source candidate | Unity preflight import | size |");
            builder.AppendLine("| --- | --- | --- | --- | --- |");

            for (int i = 0; i < candidates.Count; i++)
            {
                P0CatRoomBatch90CandidateAsset candidate = candidates[i];
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
            builder.AppendLine("- Batch 90 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.");
            builder.AppendLine("- Current P0 cat-room surface remains authoritative until candidate-backed screenshots, interaction state proof, Console, and human approval gates pass.");
            builder.AppendLine("- Do not mark Batch 90 as formally installed before explicit review approval.");

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

        private static string EscapeTable(string value)
        {
            return (value ?? string.Empty)
                .Replace("|", "\\|")
                .Replace("\r", " ")
                .Replace("\n", " ");
        }
    }

    public static class P0CatRoomBatch90UnityPreflight
    {
        public const int ExpectedCoveredCheckCount = 7;
        public const int ExpectedUnityEvidenceRequiredCount = 8;
        public const string ReportPath = "design/development/asset_review/BATCH90_CAT_ROOM_UNITY_PREFLIGHT_REPORT_2026-06-26.md";
        public const string RuntimeEvidenceReportPath = "design/development/asset_review/batch_90_cat_room_unity_preflight/runtime_evidence_report.md";
        public const string EvidenceLogPrefix = "Batch 90 preflight log:";
        public const string PreflightPassedLogToken = "[TheCat] Batch 90 cat-room preflight:";
        private const string RuntimeEvidenceLogPrefix = "Batch 90 runtime evidence log:";
        private const string RuntimeEvidencePassedLogToken = "[TheCat] Batch 90 cat-room runtime evidence passed:";
        public const string CompleteScreenshotEvidenceToken = "Complete screenshot evidence: yes";
        public const string CatRoomSurfaceCapturedToken = "Cat-room surface captured: yes";
        public const string CandidateFrameDrawToken = "Candidate frame draws: 6/6";
        public const string NoCandidateTextureFallbackToken = "No candidate texture fallback: yes";
        public const string InteractionStatesVisibleToken = "Bed/feeder/litter/dream entrance states visible: yes";
        public const string HoverDisabledRangeStatesVisibleToken = "Hover/disabled/blocked/range states visible: yes";

        public static readonly string[] RequiredUnityEvidencePaths =
        {
            "design/development/screenshots/batch_90_cat_room_unity_preflight/01-cat-room-batch90-1920x1080.png",
            "design/development/screenshots/batch_90_cat_room_unity_preflight/02-cat-room-batch90-1365x768.png",
            "design/development/screenshots/batch_90_cat_room_unity_preflight/03-cat-room-batch90-1280x720.png",
            "design/development/screenshots/batch_90_cat_room_unity_preflight/04-cat-room-batch90-1024x768.png",
            "design/development/asset_review/batch_90_cat_room_unity_preflight/interaction_state_text_density_review.md",
            "design/development/asset_review/batch_90_cat_room_unity_preflight/click_target_prop_scale_review.md",
            "design/development/asset_review/batch_90_cat_room_unity_preflight/scene_binding_console_clean_report.md",
            "design/development/asset_review/batch_90_cat_room_unity_preflight/human_review_approval.md"
        };

        public static P0CatRoomBatch90UnityPreflightReport EvaluateCurrentPreflight()
        {
            return EvaluateCurrentPreflight(unityEditorImportValidationReady: false);
        }

        public static P0CatRoomBatch90UnityPreflightReport EvaluateCurrentPreflight(bool unityEditorImportValidationReady)
        {
            return Evaluate(
                P0CatRoomBatch90CandidateCatalog.CreateCandidates(),
                DefaultFileExists,
                DefaultReadPngDimensions,
                unityEditorImportValidationReady);
        }

        public static P0CatRoomBatch90UnityPreflightReport Evaluate(
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady)
        {
            return Evaluate(
                candidates,
                fileExists,
                readPngDimensions,
                unityEditorImportValidationReady,
                DefaultReadText,
                DefaultHasUsefulPngVisualContent);
        }

        public static P0CatRoomBatch90UnityPreflightReport Evaluate(
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady,
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            P0CatRoomBatch90UnityPreflightReport report = new P0CatRoomBatch90UnityPreflightReport();
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
                    P0CatRoomBatch90CandidateAsset candidate = candidates[i];
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

                    if (IsCatRoomUnityImportPath(entry.UnityImportPath) && exists(entry.UnityImportPath))
                    {
                        importedCandidatePngCount++;
                        if (ValidateDimensions(entry, readDimensions, report))
                        {
                            dimensionMatchedCount++;
                        }
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " Unity preflight import is missing or outside CatRoom: " + entry.UnityImportPath);
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

            int unityPreflightBindingCount = CountReadyBindings(P0CatRoomBatch90CandidateCatalog.CreateUnityPreflightBindings());
            int formalRuntimeBindingLeakCount = CountFormalRuntimeBindingLeaks(candidates);
            int unityEvidenceCount = CountUnityEvidence(exists, readEvidenceText, readDimensions, readScreenshotContent, report);
            bool queueEntryReadyForUnityReview = IsQueueEntryReadyForUnityReview();

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

            Require(report, candidateCount == P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, "Batch 90 catalog declares six cat-room component candidates.", "Batch 90 candidate catalog count is stale.");
            Require(report, sourceCandidatePngCount == candidateCount && sourceCandidateNoMetaCount == candidateCount, "Batch 90 source candidates stay review-only under design/development and have no Unity meta files.", "Batch 90 source candidate folder is incomplete or polluted by Unity meta files.");
            Require(report, importedCandidatePngCount == candidateCount && importedCandidateMetaCount == candidateCount, "Batch 90 Unity preflight imports exist with Unity meta files.", "Batch 90 Unity preflight imports are incomplete.");
            Require(report, dimensionMatchedCount == candidateCount, "Batch 90 Unity preflight imports match manifest dimensions.", "Batch 90 Unity preflight import dimensions do not match manifest rows.");
            Require(report, unityPreflightBindingCount == candidateCount, "Batch 90 preflight bindings cover every candidate component and variant.", "Batch 90 preflight binding count does not match candidate count.");
            Require(report, formalRuntimeBindingLeakCount == 0, "Batch 90 candidates do not leak into formal runtime visual bindings.", "Batch 90 candidate imports leaked into the formal runtime visual catalog.");
            Require(report, queueEntryReadyForUnityReview, "Batch 90 queue entry remains candidate-pack-complete pending Unity review.", "Batch 90 queue entry is missing or no longer points at the cat-room preflight candidate pack.");

            if (unityEditorImportValidationReady)
            {
                report.AddCoveredCheck("Unity editor import settings validation has passed for Batch 90 preflight imports.");
            }
            else
            {
                report.AddBlockingItem("Unity editor import validation has not passed for Batch 90 CatRoom candidate imports.");
            }

            report.FinalizeFormalInstallGate();
            return report;
        }

        private static int CountReadyBindings(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            if (bindings == null)
            {
                return 0;
            }

            int count = 0;
            HashSet<string> bindingIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < bindings.Count; i++)
            {
                P0VisualAssetBinding binding = bindings[i];
                if (binding.IsReady
                    && binding.BindingId.StartsWith("batch90.", StringComparison.Ordinal)
                    && IsCatRoomUnityImportPath(binding.Asset.UnityImportPath)
                    && bindingIds.Add(binding.BindingId))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountFormalRuntimeBindingLeaks(IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates)
        {
            if (candidates == null)
            {
                return 0;
            }

            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            int leaks = 0;
            for (int i = 0; i < runtimeBindings.Length; i++)
            {
                P0VisualAssetReference runtimeAsset = runtimeBindings[i].Asset;
                for (int j = 0; j < candidates.Count; j++)
                {
                    P0AssetManifestEntry entry = candidates[j].ManifestEntry;
                    if (runtimeAsset.AssetId == entry.AssetId
                        || runtimeAsset.UnityImportPath == entry.UnityImportPath)
                    {
                        leaks++;
                    }
                }
            }

            return leaks;
        }

        private static int CountUnityEvidence(
            Func<string, bool> exists,
            Func<string, string> readText,
            P0AssetPngDimensionReader readDimensions,
            P0AssetPngVisualContentReader readVisualContent,
            P0CatRoomBatch90UnityPreflightReport report)
        {
            int count = 0;
            for (int i = 0; i < RequiredUnityEvidencePaths.Length; i++)
            {
                string path = RequiredUnityEvidencePaths[i];
                if (!exists(path))
                {
                    report.AddBlockingItem("Missing Unity evidence: `" + path + "`.");
                    continue;
                }

                if (path.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    if (IsScreenshotEvidenceComplete(path, exists, readText, readDimensions, readVisualContent, report))
                    {
                        count++;
                    }

                    continue;
                }

                if (IsMarkdownEvidenceComplete(path, exists, readText, report))
                {
                    count++;
                }
            }

            return count;
        }

        private static bool IsScreenshotEvidenceComplete(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0AssetPngDimensionReader readDimensions,
            P0AssetPngVisualContentReader readVisualContent,
            P0CatRoomBatch90UnityPreflightReport report)
        {
            if (!TryGetExpectedScreenshotDimensions(path, out int expectedWidth, out int expectedHeight))
            {
                report.AddBlockingItem("Unknown Batch 90 screenshot evidence resolution: `" + path + "`");
                return false;
            }

            if (!readDimensions(path, out int actualWidth, out int actualHeight, out string error))
            {
                report.AddBlockingItem("Unreadable Unity screenshot evidence: `" + path + "` (" + error + ")");
                return false;
            }

            if (actualWidth != expectedWidth || actualHeight != expectedHeight)
            {
                report.AddBlockingItem("Unity screenshot evidence has wrong dimensions: `"
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

            if (!readVisualContent(path, out string summary, out string visualError))
            {
                report.AddBlockingItem("Unity screenshot evidence is visually blank or incomplete: `"
                    + path
                    + "` ("
                    + (string.IsNullOrWhiteSpace(visualError) ? summary : visualError)
                    + ")");
                return false;
            }

            return RuntimeReportConfirmsScreenshot(path, exists, readText, report);
        }

        private static bool RuntimeReportConfirmsScreenshot(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0CatRoomBatch90UnityPreflightReport report)
        {
            if (!exists(RuntimeEvidenceReportPath))
            {
                report.AddBlockingItem("Missing Batch 90 runtime evidence report for screenshot evidence: `" + RuntimeEvidenceReportPath + "`");
                return false;
            }

            string content;
            try
            {
                content = readText(RuntimeEvidenceReportPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable Batch 90 runtime evidence report: `"
                    + RuntimeEvidenceReportPath
                    + "` ("
                    + exception.GetType().Name
                    + ")");
                return false;
            }

            string[] requiredTokens =
            {
                CompleteScreenshotEvidenceToken,
                CatRoomSurfaceCapturedToken,
                CandidateFrameDrawToken,
                NoCandidateTextureFallbackToken,
                InteractionStatesVisibleToken,
                HoverDisabledRangeStatesVisibleToken
            };

            for (int i = 0; i < requiredTokens.Length; i++)
            {
                if (ContainsOrdinalIgnoreCase(content, requiredTokens[i]))
                {
                    continue;
                }

                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not confirmed by a complete candidate-drawn Batch 90 runtime evidence report requiring `"
                    + requiredTokens[i]
                    + "`.");
                return false;
            }

            if (!ContainsOrdinal(content, path))
            {
                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not listed in the Batch 90 runtime evidence report.");
                return false;
            }

            return true;
        }

        private static bool IsMarkdownEvidenceComplete(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0CatRoomBatch90UnityPreflightReport report)
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

            if (!ContainsOrdinalIgnoreCase(content, "Batch 90"))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not name Batch 90.");
                return false;
            }

            if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
            {
                bool hasRequiredTokens = RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[] { "Runtime scene/presenter binding: yes", "Console clean: yes", "Unity log reviewed: yes" });

                return hasRequiredTokens
                    && ValidateSceneBindingConsoleCleanReport(path, content, exists, readText, report);
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[] { "Formal install approved: yes", "Reviewer:", "Approval date:" });
            }

            if (path.EndsWith("interaction_state_text_density_review.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[]
                    {
                        "Review result: pass",
                        "Bed/feeder/litter/dream entrance states: pass",
                        "Hover/disabled/blocked/range states: pass",
                        "Chinese labels and values: pass",
                        "1024x768 density: pass"
                    });
            }

            if (path.EndsWith("click_target_prop_scale_review.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[]
                    {
                        "Review result: pass",
                        "Bed click target: pass",
                        "Feeder click target: pass",
                        "Litter click target: pass",
                        "Dream entrance semantics: pass",
                        "Prop scale: pass"
                    });
            }

            report.AddBlockingItem("Unknown Batch 90 markdown evidence path: `" + path + "`.");
            return false;
        }

        private static bool ValidateSceneBindingConsoleCleanReport(
            string path,
            string content,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0CatRoomBatch90UnityPreflightReport report)
        {
            if (!TryExtractEvidenceValue(content, RuntimeEvidenceLogPrefix, out string logPath))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not identify a Batch 90 runtime evidence log path.");
                return false;
            }

            if (!exists(logPath))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references missing Batch 90 runtime evidence log `" + logPath + "`.");
                return false;
            }

            string logContent;
            try
            {
                logContent = readText(logPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable Batch 90 runtime evidence log: `" + logPath + "` (" + exception.GetType().Name + ")");
                return false;
            }

            if (!ContainsOrdinalIgnoreCase(logContent, RuntimeEvidencePassedLogToken))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references a log without the Batch 90 runtime evidence token.");
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
                    + consoleLogReport.BuildSummary());
                return false;
            }

            return true;
        }

        private static bool ValidateDimensions(
            P0AssetManifestEntry entry,
            P0AssetPngDimensionReader readPngDimensions,
            P0CatRoomBatch90UnityPreflightReport report)
        {
            if (!P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out int expectedWidth, out int expectedHeight))
            {
                report.AddIssue(entry.AssetId + " has unsupported manifest size '" + entry.Size + "'.");
                return false;
            }

            if (!readPngDimensions(entry.UnityImportPath, out int actualWidth, out int actualHeight, out string error))
            {
                report.AddIssue(entry.AssetId + " could not read PNG dimensions from " + entry.UnityImportPath + ": " + error);
                return false;
            }

            if (actualWidth == expectedWidth && actualHeight == expectedHeight)
            {
                return true;
            }

            report.AddIssue(entry.AssetId
                + " expected "
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

        private static bool TryGetExpectedScreenshotDimensions(string path, out int width, out int height)
        {
            if (ContainsOrdinal(path, "1920x1080"))
            {
                width = 1920;
                height = 1080;
                return true;
            }

            if (ContainsOrdinal(path, "1365x768"))
            {
                width = 1365;
                height = 768;
                return true;
            }

            if (ContainsOrdinal(path, "1280x720"))
            {
                width = 1280;
                height = 720;
                return true;
            }

            if (ContainsOrdinal(path, "1024x768"))
            {
                width = 1024;
                height = 768;
                return true;
            }

            width = 0;
            height = 0;
            return false;
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

        private static bool RequireEvidenceTokens(
            P0CatRoomBatch90UnityPreflightReport report,
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

                    return HasUsefulVisualContent(texture.GetPixels32(), texture.width, texture.height, out summary);
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

        private static bool HasUsefulVisualContent(Color32[] pixels, int width, int height, out string summary)
        {
            summary = string.Empty;
            if (pixels == null || pixels.Length == 0 || width <= 0 || height <= 0)
            {
                summary = "missing pixels";
                return false;
            }

            int nonTransparent = 0;
            byte minR = byte.MaxValue;
            byte minG = byte.MaxValue;
            byte minB = byte.MaxValue;
            byte maxR = 0;
            byte maxG = 0;
            byte maxB = 0;

            for (int i = 0; i < pixels.Length; i++)
            {
                Color32 pixel = pixels[i];
                if (pixel.a < 8)
                {
                    continue;
                }

                nonTransparent++;
                minR = Math.Min(minR, pixel.r);
                minG = Math.Min(minG, pixel.g);
                minB = Math.Min(minB, pixel.b);
                maxR = Math.Max(maxR, pixel.r);
                maxG = Math.Max(maxG, pixel.g);
                maxB = Math.Max(maxB, pixel.b);
            }

            int requiredPixels = Math.Max(256, pixels.Length / 20);
            int colorRange = (maxR - minR) + (maxG - minG) + (maxB - minB);
            summary = "nonTransparent=" + nonTransparent + ", colorRange=" + colorRange;
            return nonTransparent >= requiredPixels && colorRange >= 24;
        }

        private static bool IsQueueEntryReadyForUnityReview()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();
            for (int i = 0; i < queue.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queue[i];
                if (entry.QueueId == P0AssetProductionQueueCatalog.CatRoomPreflightCandidateQueueId)
                {
                    return entry.State == P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview
                        && entry.Phase == P0AssetProductionQueuePhase.CodexCandidateProduction
                        && ContainsOrdinal(entry.CandidateDirectory, P0CatRoomBatch90CandidateCatalog.BatchSlug)
                        && ContainsOrdinal(entry.UnityImportRoot, "Assets/TheCat/Art/UI/CatRoom")
                        && ContainsOrdinal(entry.NextAction, "Unity-rendered")
                        && ContainsOrdinal(entry.NextAction, "cat-room");
                }
            }

            return false;
        }

        private static bool IsDesignCandidatePath(string path)
        {
            return ContainsOrdinal(path, "design/development/asset_candidates/")
                && !ContainsOrdinal(path, "Assets/");
        }

        private static bool IsCatRoomUnityImportPath(string path)
        {
            return ContainsOrdinal(path, "Assets/TheCat/Art/UI/CatRoom/")
                && !ContainsOrdinal(path, "design/development/asset_candidates/");
        }

        private static bool ContainsOrdinal(string value, string token)
        {
            return (value ?? string.Empty).IndexOf(token ?? string.Empty, StringComparison.Ordinal) >= 0;
        }

        private static bool ContainsOrdinalIgnoreCase(string value, string token)
        {
            return (value ?? string.Empty).IndexOf(token ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultFileExists(string path)
        {
            return File.Exists(ResolveProjectPath(path));
        }

        private static bool DefaultReadPngDimensions(string path, out int width, out int height, out string error)
        {
            width = 0;
            height = 0;
            error = string.Empty;

            try
            {
                using (FileStream stream = File.OpenRead(ResolveProjectPath(path)))
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
            P0CatRoomBatch90UnityPreflightReport report,
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
