using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Tools
{
    public sealed class P0CharacterSelectBatch88UnityPreflightReport
    {
        private readonly List<string> issues = new List<string>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<string> blockingItems = new List<string>();
        private readonly List<P0CharacterSelectBatch88CandidateAsset> candidates = new List<P0CharacterSelectBatch88CandidateAsset>();

        public IReadOnlyList<string> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<string> BlockingItems => blockingItems.AsReadOnly();

        public IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> Candidates => candidates.AsReadOnly();

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
            && coveredChecks.Count >= P0CharacterSelectBatch88UnityPreflight.ExpectedCoveredCheckCount;

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

        public void AddCandidate(P0CharacterSelectBatch88CandidateAsset candidate)
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
                return "Batch 88 character-select Unity preflight has " + FailureCount + " failure(s).";
            }

            if (!UnityEditorImportValidationReady)
            {
                return "Batch 88 character-select candidates need Unity editor import validation before preflight is ready.";
            }

            return "Batch 88 character-select candidates are Unity-import preflight ready; formal install remains blocked until all runtime evidence, scene/presenter/Console, and approval gates pass.";
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
            builder.AppendLine("# Batch 88 Character Select Unity Preflight");
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
                P0CharacterSelectBatch88CandidateAsset candidate = candidates[i];
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
            builder.AppendLine("- Batch 88 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.");
            builder.AppendLine("- Current IMGUI main-menu character-select contract remains the playable runtime path until the remaining scene/presenter/Console and human approval gates pass.");
            builder.AppendLine("- Do not mark Batch 88 as formally installed before explicit review approval.");

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

    public static class P0CharacterSelectBatch88UnityPreflight
    {
        public const int ExpectedCoveredCheckCount = 8;
        public const int ExpectedUnityEvidenceRequiredCount = 8;
        public const string ReportPath = "design/development/asset_review/BATCH88_CHARACTER_SELECT_UNITY_PREFLIGHT_REPORT_2026-06-25.md";
        public const string RuntimeEvidenceReportPath = "design/development/asset_review/batch_88_character_select_unity_preflight/runtime_evidence_report.md";
        private const string RuntimeEvidenceLogPrefix = "Batch 88 runtime evidence log:";
        private const string RuntimeEvidencePassedLogToken = "[TheCat] Batch 88 character-select runtime evidence passed:";
        public const string CompleteScreenshotEvidenceToken = "Complete screenshot evidence: yes";
        public const string CharacterSelectSurfaceCapturedToken = "Character-select surface captured: yes";
        public const string CandidateFrameDrawToken = "Candidate frame draws: 6/6";
        public const string NoCandidateTextureFallbackToken = "No candidate texture fallback: yes";
        public const string SourceLockedAvatarsVisibleToken = "Source-locked HUD avatars visible: yes";
        public const string SelectedIdleStatesVisibleToken = "Selected and idle card states visible: yes";

        public static readonly string[] RequiredUnityEvidencePaths =
        {
            "design/development/screenshots/batch_88_character_select_unity_preflight/01-character-select-batch88-1920x1080.png",
            "design/development/screenshots/batch_88_character_select_unity_preflight/02-character-select-batch88-1365x768.png",
            "design/development/screenshots/batch_88_character_select_unity_preflight/03-character-select-batch88-1280x720.png",
            "design/development/screenshots/batch_88_character_select_unity_preflight/04-character-select-batch88-1024x768.png",
            "design/development/asset_review/batch_88_character_select_unity_preflight/source_locked_avatar_consistency_review.md",
            "design/development/asset_review/batch_88_character_select_unity_preflight/chinese_text_density_click_target_review.md",
            "design/development/asset_review/batch_88_character_select_unity_preflight/scene_binding_console_clean_report.md",
            "design/development/asset_review/batch_88_character_select_unity_preflight/human_review_approval.md"
        };

        public static P0CharacterSelectBatch88UnityPreflightReport EvaluateCurrentPreflight()
        {
            return Evaluate(
                P0CharacterSelectBatch88CandidateCatalog.CreateCandidates(),
                DefaultFileExists,
                DefaultReadPngDimensions,
                unityEditorImportValidationReady: false);
        }

        public static P0CharacterSelectBatch88UnityPreflightReport EvaluateCurrentPreflight(bool unityEditorImportValidationReady)
        {
            return Evaluate(
                P0CharacterSelectBatch88CandidateCatalog.CreateCandidates(),
                DefaultFileExists,
                DefaultReadPngDimensions,
                unityEditorImportValidationReady);
        }

        public static P0CharacterSelectBatch88UnityPreflightReport Evaluate(
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates,
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

        public static P0CharacterSelectBatch88UnityPreflightReport Evaluate(
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates,
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

        public static P0CharacterSelectBatch88UnityPreflightReport Evaluate(
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady,
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            P0CharacterSelectBatch88UnityPreflightReport report = new P0CharacterSelectBatch88UnityPreflightReport();
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
                    P0CharacterSelectBatch88CandidateAsset candidate = candidates[i];
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

                    if (IsCharacterSelectUnityImportPath(entry.UnityImportPath) && exists(entry.UnityImportPath))
                    {
                        importedCandidatePngCount++;
                        if (ValidateDimensions(entry, readDimensions, report))
                        {
                            dimensionMatchedCount++;
                        }
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " Unity preflight import is missing or outside CharacterSelect: " + entry.UnityImportPath);
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

            int unityPreflightBindingCount = CountReadyBindings(P0CharacterSelectBatch88CandidateCatalog.CreateUnityPreflightBindings());
            int formalRuntimeBindingLeakCount = CountFormalRuntimeBindingLeaks();
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

            Require(report, candidateCount == P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, "Batch 88 catalog declares six character-select component candidates.", "Batch 88 candidate catalog count is stale.");
            Require(report, sourceCandidatePngCount == candidateCount && sourceCandidateNoMetaCount == candidateCount, "Batch 88 source candidates stay in design asset candidates and have no Unity meta files.", "Batch 88 source candidate policy is unsafe.");
            Require(report, importedCandidatePngCount == candidateCount, "Batch 88 Unity preflight imports exist under Assets/TheCat/Art/UI/CharacterSelect.", "Batch 88 Unity preflight imports are incomplete.");
            Require(report, importedCandidateMetaCount == candidateCount, "Batch 88 Unity preflight imports have Unity meta files.", "Batch 88 Unity preflight import meta files are incomplete.");
            Require(report, dimensionMatchedCount == candidateCount, "Batch 88 Unity preflight PNG dimensions match the candidate manifest.", "Batch 88 Unity preflight PNG dimensions do not match the manifest.");
            Require(report, unityPreflightBindingCount == candidateCount, "Batch 88 preflight bindings cover all imported character-select components outside the formal runtime catalog.", "Batch 88 preflight bindings are incomplete.");
            Require(report, formalRuntimeBindingLeakCount == 0, "Batch 88 candidate ids do not leak into formal P0 runtime visual bindings.", "Batch 88 candidates leaked into formal runtime bindings.");
            Require(report, queueEntryReadyForUnityReview, "Batch 88 queue entry is ready for candidate-backed Unity review.", "Batch 88 queue entry is not ready for candidate-backed Unity review.");
            if (!unityEditorImportValidationReady)
            {
                report.AddBlockingItem("Unity editor import validation has not passed.");
            }

            report.FinalizeFormalInstallGate();
            if (!report.FormalInstallAllowed)
            {
                report.AddCoveredCheck("Batch 88 remains blocked from formal install until character-select runtime evidence and human review pass.");
            }

            return report;
        }

        private static bool ValidateDimensions(
            P0AssetManifestEntry entry,
            P0AssetPngDimensionReader readDimensions,
            P0CharacterSelectBatch88UnityPreflightReport report)
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
                    && bindings[i].SurfaceId == "character_select_preflight"
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
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates = P0CharacterSelectBatch88CandidateCatalog.CreateCandidates();
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
            P0CharacterSelectBatch88UnityPreflightReport report)
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
                    && !IsScreenshotEvidenceComplete(path, exists, readText, readPngDimensions, readVisualContent, report))
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
                && ContainsOrdinal(path, "design/development/screenshots/batch_88_character_select_unity_preflight/");
        }

        private static bool IsScreenshotEvidenceComplete(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0AssetPngDimensionReader readPngDimensions,
            P0AssetPngVisualContentReader readVisualContent,
            P0CharacterSelectBatch88UnityPreflightReport report)
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

                return RuntimeReportConfirmsScreenshot(path, exists, readText, report);
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
            Func<string, bool> exists,
            Func<string, string> readText,
            P0CharacterSelectBatch88UnityPreflightReport report)
        {
            if (!exists(RuntimeEvidenceReportPath))
            {
                report.AddBlockingItem("Missing Batch 88 runtime evidence report for screenshot evidence: `" + RuntimeEvidenceReportPath + "`");
                return false;
            }

            string content;
            try
            {
                content = readText(RuntimeEvidenceReportPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable Batch 88 runtime evidence report: `"
                    + RuntimeEvidenceReportPath
                    + "` ("
                    + exception.GetType().Name
                    + ")");
                return false;
            }

            string[] requiredTokens =
            {
                CompleteScreenshotEvidenceToken,
                CharacterSelectSurfaceCapturedToken,
                CandidateFrameDrawToken,
                NoCandidateTextureFallbackToken,
                SourceLockedAvatarsVisibleToken,
                SelectedIdleStatesVisibleToken
            };

            for (int i = 0; i < requiredTokens.Length; i++)
            {
                if (ContainsOrdinalIgnoreCase(content, requiredTokens[i]))
                {
                    continue;
                }

                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not confirmed by a complete candidate-drawn Batch 88 runtime evidence report requiring `"
                    + requiredTokens[i]
                    + "`.");
                return false;
            }

            if (!ContainsOrdinal(content, path))
            {
                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not listed in the Batch 88 runtime evidence report.");
                return false;
            }

            return true;
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

        private static bool IsMarkdownEvidencePath(string path)
        {
            return (path ?? string.Empty).EndsWith(".md", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsMarkdownEvidenceComplete(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0CharacterSelectBatch88UnityPreflightReport report)
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

            if (!ContainsOrdinalIgnoreCase(content, "Batch 88"))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not name Batch 88.");
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

            if (path.EndsWith("source_locked_avatar_consistency_review.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[]
                    {
                        "Review result: pass",
                        "Source-lock HUD avatar consistency: pass",
                        "Saiban avatar: source-locked",
                        "Nephthys avatar: source-locked",
                        "Suzune avatar: source-locked"
                    });
            }

            if (path.EndsWith("chinese_text_density_click_target_review.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[]
                    {
                        "Review result: pass",
                        "Chinese names/roles/descriptions/start labels: pass",
                        "Selected/idle distinction: pass",
                        "1024x768 density: pass",
                        "Click targets: pass"
                    });
            }

            report.AddBlockingItem("Unknown Batch 88 markdown evidence path: `" + path + "`.");
            return false;
        }

        private static bool ValidateSceneBindingConsoleCleanReport(
            string path,
            string content,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0CharacterSelectBatch88UnityPreflightReport report)
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
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references a log without the Batch 88 runtime pass token.");
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
            P0CharacterSelectBatch88UnityPreflightReport report,
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
            P0CharacterSelectBatch88UnityPreflightReport report,
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
                if (entry.QueueId == P0AssetProductionQueueCatalog.CharacterSelectPreflightCandidateQueueId)
                {
                    return entry.State == P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview
                        && entry.Phase == P0AssetProductionQueuePhase.CodexCandidateProduction
                        && ContainsOrdinal(entry.CandidateDirectory, P0CharacterSelectBatch88CandidateCatalog.BatchSlug)
                        && ContainsOrdinal(entry.UnityImportRoot, "Assets/TheCat/Art/UI/CharacterSelect")
                        && ContainsOrdinal(entry.NextAction, "Unity-rendered character-select screenshots");
                }
            }

            return false;
        }

        private static bool IsDesignCandidatePath(string path)
        {
            return ContainsOrdinal(path, "design/development/asset_candidates/")
                && !ContainsOrdinal(path, "Assets/");
        }

        private static bool IsCharacterSelectUnityImportPath(string path)
        {
            return ContainsOrdinal(path, "Assets/TheCat/Art/UI/CharacterSelect/")
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
            P0CharacterSelectBatch88UnityPreflightReport report,
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
