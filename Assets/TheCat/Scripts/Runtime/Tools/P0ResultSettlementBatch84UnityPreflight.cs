using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Tools
{
    public sealed class P0ResultSettlementBatch84UnityPreflightReport
    {
        private readonly List<string> issues = new List<string>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<string> blockingItems = new List<string>();
        private readonly List<P0ResultSettlementBatch84CandidateAsset> candidates = new List<P0ResultSettlementBatch84CandidateAsset>();

        public IReadOnlyList<string> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<string> BlockingItems => blockingItems.AsReadOnly();

        public IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> Candidates => candidates.AsReadOnly();

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
            && coveredChecks.Count >= P0ResultSettlementBatch84UnityPreflight.ExpectedCoveredCheckCount;

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

        public void AddCandidate(P0ResultSettlementBatch84CandidateAsset candidate)
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
                return "Batch 84 result/settlement Unity preflight has " + FailureCount + " failure(s).";
            }

            if (!UnityEditorImportValidationReady)
            {
                return "Batch 84 result/settlement candidates need Unity editor import validation before preflight is ready.";
            }

            if (FormalInstallAllowed)
            {
                return "Batch 84 result/settlement candidates are Unity-import preflight ready and formal install evidence is complete.";
            }

            if (UnityEvidenceCount >= UnityEvidenceRequiredCount - 2)
            {
                return "Batch 84 result/settlement candidates are Unity-import preflight ready; runtime screenshots/reviews passed, formal install remains blocked by scene/Console and human approval gates.";
            }

            return "Batch 84 result/settlement candidates are Unity-import preflight ready; formal install remains blocked until Unity victory/defeat/settlement screenshots, text replacement, 1024x768 crowding, scene/Console, and human approval gates pass.";
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
            builder.AppendLine("# Batch 84 Result Settlement Unity Preflight");
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
                P0ResultSettlementBatch84CandidateAsset candidate = candidates[i];
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
            builder.AppendLine("- Batch 84 imports are not included in `P0VisualAssetCatalog.P0RuntimeVisualBindingCount`.");
            builder.AppendLine("- Current battle-result and route-settlement presenters remain authoritative until nonblank runtime screenshots, text replacement, crowding, Console, and human approval gates pass.");
            builder.AppendLine("- Screenshot evidence must be confirmed by `" + P0ResultSettlementBatch84UnityPreflight.RuntimeEvidenceReportPath + "`; scene/Console evidence must identify the matching clean runtime evidence log.");
            builder.AppendLine("- Do not mark Batch 84 as formally installed before explicit review approval.");

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

    public static class P0ResultSettlementBatch84UnityPreflight
    {
        public const int ExpectedCoveredCheckCount = 7;
        public const int ExpectedUnityEvidenceRequiredCount = 8;
        public const string ReportPath = "design/development/asset_review/BATCH84_RESULT_SETTLEMENT_UNITY_PREFLIGHT_REPORT_2026-06-26.md";
        public const string RuntimeEvidenceReportPath = "design/development/asset_review/batch_84_result_settlement_unity_preflight/runtime_evidence_report.md";
        public const string EvidenceLogPrefix = "Batch 84 preflight log:";
        public const string PreflightPassedLogToken = "[TheCat] Batch 84 result/settlement preflight:";
        private const string RuntimeEvidenceLogPrefix = "Batch 84 runtime evidence log:";
        private const string RuntimeEvidencePassedLogToken = "[TheCat] Batch 84 result/settlement runtime evidence passed:";
        public const string CompleteScreenshotEvidenceToken = "Complete screenshot evidence: yes";
        public const string ResultSettlementSurfaceCapturedToken = "Result/settlement surface captured: yes";
        public const string CandidateFrameDrawToken = "Candidate frame draws: 7/7";
        public const string NoCandidateTextureFallbackToken = "No candidate texture fallback: yes";
        public const string BattleOutcomeStatesVisibleToken = "Battle outcome states visible: yes";
        public const string SettlementOutcomeStatesVisibleToken = "Settlement outcome states visible: yes";
        public const string RewardStatReadabilityVisibleToken = "Reward/stat readability visible: yes";
        public const string ResultSettlementClickTargetsVisibleToken = "Result/settlement click targets visible: yes";

        public static readonly string[] RequiredUnityEvidencePaths =
        {
            "design/development/screenshots/batch_84_result_settlement_unity_preflight/01-result-settlement-batch84-battle-victory-1920x1080.png",
            "design/development/screenshots/batch_84_result_settlement_unity_preflight/02-result-settlement-batch84-battle-defeat-1920x1080.png",
            "design/development/screenshots/batch_84_result_settlement_unity_preflight/03-result-settlement-batch84-run-cleared-1365x768.png",
            "design/development/screenshots/batch_84_result_settlement_unity_preflight/04-result-settlement-batch84-run-failed-1024x768.png",
            "design/development/asset_review/batch_84_result_settlement_unity_preflight/text_replacement_reward_readability_review.md",
            "design/development/asset_review/batch_84_result_settlement_unity_preflight/outcome_actions_click_target_review.md",
            "design/development/asset_review/batch_84_result_settlement_unity_preflight/scene_binding_console_clean_report.md",
            "design/development/asset_review/batch_84_result_settlement_unity_preflight/human_review_approval.md"
        };

        public static P0ResultSettlementBatch84UnityPreflightReport EvaluateCurrentPreflight()
        {
            return EvaluateCurrentPreflight(unityEditorImportValidationReady: false);
        }

        public static P0ResultSettlementBatch84UnityPreflightReport EvaluateCurrentPreflight(bool unityEditorImportValidationReady)
        {
            return Evaluate(
                P0ResultSettlementBatch84CandidateCatalog.CreateCandidates(),
                DefaultFileExists,
                DefaultReadPngDimensions,
                unityEditorImportValidationReady);
        }

        public static P0ResultSettlementBatch84UnityPreflightReport Evaluate(
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates,
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

        public static P0ResultSettlementBatch84UnityPreflightReport Evaluate(
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates,
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

        public static P0ResultSettlementBatch84UnityPreflightReport Evaluate(
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions,
            bool unityEditorImportValidationReady,
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            P0ResultSettlementBatch84UnityPreflightReport report = new P0ResultSettlementBatch84UnityPreflightReport();
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
                    P0ResultSettlementBatch84CandidateAsset candidate = candidates[i];
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

                    if (IsResultSettlementUnityImportPath(entry.UnityImportPath) && exists(entry.UnityImportPath))
                    {
                        importedCandidatePngCount++;
                        if (ValidateDimensions(entry, readDimensions, report))
                        {
                            dimensionMatchedCount++;
                        }
                    }
                    else
                    {
                        report.AddIssue(entry.AssetId + " Unity preflight import is missing or outside ResultSettlement: " + entry.UnityImportPath);
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

            int unityPreflightBindingCount = CountReadyBindings(P0ResultSettlementBatch84CandidateCatalog.CreateUnityPreflightBindings());
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

            Require(report, candidateCount == P0ResultSettlementBatch84CandidateCatalog.ExpectedCandidateSpriteCount, "Batch 84 catalog declares seven result/settlement component candidates.", "Batch 84 candidate catalog count is wrong.");
            Require(report, sourceCandidatePngCount == candidateCount, "Batch 84 source candidate PNGs exist in design asset candidates.", "Batch 84 source candidate PNGs are incomplete.");
            Require(report, sourceCandidateNoMetaCount == candidateCount, "Batch 84 source candidate PNGs remain without Unity meta files.", "Batch 84 source candidates leaked Unity meta files.");
            Require(report, importedCandidatePngCount == candidateCount, "Batch 84 Unity preflight imports exist under ResultSettlement.", "Batch 84 Unity preflight imports are incomplete.");
            Require(report, importedCandidateMetaCount == candidateCount, "Batch 84 Unity preflight imports have Unity meta files.", "Batch 84 Unity preflight import meta files are incomplete.");
            Require(report, dimensionMatchedCount == candidateCount, "Batch 84 Unity preflight imports match manifest dimensions.", "Batch 84 Unity preflight import dimensions do not match.");
            Require(report, unityPreflightBindingCount == candidateCount, "Batch 84 preflight-only bindings are complete and ready.", "Batch 84 preflight-only bindings are incomplete.");
            Require(report, formalRuntimeBindingLeakCount == 0, "Batch 84 candidates are isolated from formal runtime visual bindings.", "Batch 84 candidate assets leaked into formal runtime visual bindings.");
            Require(report, queueEntryReadyForUnityReview, "Batch 84 result/settlement queue entry is ready for Unity review.", "Batch 84 queue entry is missing or not ready for Unity review.");

            if (!unityEditorImportValidationReady)
            {
                report.AddBlockingItem("Unity editor import validation has not passed for Batch 84.");
            }

            report.FinalizeFormalInstallGate();
            return report;
        }

        public static void WriteCurrentReport(bool unityEditorImportValidationReady)
        {
            P0ResultSettlementBatch84UnityPreflightReport report =
                EvaluateCurrentPreflight(unityEditorImportValidationReady);
            string path = ResolveProjectPath(ReportPath);
            Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
            File.WriteAllText(path, report.BuildMarkdown());
        }

        private static int CountReadyBindings(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            int count = 0;
            if (bindings == null)
            {
                return count;
            }

            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].IsReady)
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountFormalRuntimeBindingLeaks()
        {
            int leaks = 0;
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates =
                P0ResultSettlementBatch84CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();

            for (int i = 0; i < runtimeBindings.Length; i++)
            {
                P0VisualAssetBinding binding = runtimeBindings[i];
                for (int j = 0; j < candidates.Count; j++)
                {
                    P0AssetManifestEntry entry = candidates[j].ManifestEntry;
                    if (binding.Asset.AssetId == entry.AssetId
                        || binding.Asset.UnityImportPath == entry.UnityImportPath
                        || binding.BindingId.IndexOf("batch84.", StringComparison.OrdinalIgnoreCase) >= 0)
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
            P0ResultSettlementBatch84UnityPreflightReport report)
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

                if (path.EndsWith(".png", StringComparison.Ordinal))
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
            P0ResultSettlementBatch84UnityPreflightReport report)
        {
            if (!TryGetExpectedScreenshotDimensions(path, out int expectedWidth, out int expectedHeight))
            {
                report.AddBlockingItem("Unknown Batch 84 screenshot target dimensions for `" + path + "`.");
                return false;
            }

            if (!readDimensions(path, out int actualWidth, out int actualHeight, out string error))
            {
                report.AddBlockingItem("Unreadable Batch 84 screenshot evidence: `" + path + "` (" + error + ").");
                return false;
            }

            if (actualWidth != expectedWidth || actualHeight != expectedHeight)
            {
                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
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
                    + ").");
                return false;
            }

            return RuntimeReportConfirmsScreenshot(path, exists, readText, report);
        }

        private static bool RuntimeReportConfirmsScreenshot(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0ResultSettlementBatch84UnityPreflightReport report)
        {
            if (!exists(RuntimeEvidenceReportPath))
            {
                report.AddBlockingItem("Missing Batch 84 runtime evidence report for screenshot evidence: `" + RuntimeEvidenceReportPath + "`.");
                return false;
            }

            string content;
            try
            {
                content = readText(RuntimeEvidenceReportPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable Batch 84 runtime evidence report: `"
                    + RuntimeEvidenceReportPath
                    + "` ("
                    + exception.GetType().Name
                    + ").");
                return false;
            }

            string[] requiredTokens =
            {
                CompleteScreenshotEvidenceToken,
                ResultSettlementSurfaceCapturedToken,
                CandidateFrameDrawToken,
                NoCandidateTextureFallbackToken,
                BattleOutcomeStatesVisibleToken,
                SettlementOutcomeStatesVisibleToken,
                RewardStatReadabilityVisibleToken,
                ResultSettlementClickTargetsVisibleToken
            };

            for (int i = 0; i < requiredTokens.Length; i++)
            {
                if (ContainsOrdinalIgnoreCase(content, requiredTokens[i]))
                {
                    continue;
                }

                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not confirmed by a complete candidate-drawn Batch 84 runtime evidence report requiring `"
                    + requiredTokens[i]
                    + "`.");
                return false;
            }

            if (!ContainsOrdinal(content, path))
            {
                report.AddBlockingItem("Incomplete Unity screenshot evidence: `"
                    + path
                    + "` is not listed in the Batch 84 runtime evidence report.");
                return false;
            }

            return true;
        }

        private static bool IsMarkdownEvidenceComplete(
            string path,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0ResultSettlementBatch84UnityPreflightReport report)
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

            if (!ContainsOrdinalIgnoreCase(content, "Batch 84"))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not name Batch 84.");
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

            if (path.EndsWith("text_replacement_reward_readability_review.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[] { "Review result: pass", "Unity-rendered text replacement: pass", "Reward/stat readability: pass", "1024x768 density: pass" });
            }

            if (path.EndsWith("outcome_actions_click_target_review.md", StringComparison.Ordinal))
            {
                return RequireEvidenceTokens(
                    report,
                    path,
                    content,
                    new[] { "Review result: pass", "Victory/defeat outcome semantics: pass", "Result action buttons: pass", "Settlement actions: pass" });
            }

            report.AddBlockingItem("Unknown Batch 84 markdown evidence path: `" + path + "`.");
            return false;
        }

        private static bool ValidateSceneBindingConsoleCleanReport(
            string path,
            string content,
            Func<string, bool> exists,
            Func<string, string> readText,
            P0ResultSettlementBatch84UnityPreflightReport report)
        {
            if (!TryExtractEvidenceValue(content, RuntimeEvidenceLogPrefix, out string logPath))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` does not identify a Batch 84 runtime evidence log path.");
                return false;
            }

            if (!exists(logPath))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references missing Batch 84 runtime evidence log `" + logPath + "`.");
                return false;
            }

            string logContent;
            try
            {
                logContent = readText(logPath) ?? string.Empty;
            }
            catch (Exception exception)
            {
                report.AddBlockingItem("Unreadable Batch 84 runtime evidence log: `" + logPath + "` (" + exception.GetType().Name + ").");
                return false;
            }

            if (!ContainsOrdinalIgnoreCase(logContent, RuntimeEvidencePassedLogToken))
            {
                report.AddBlockingItem("Incomplete Unity evidence: `" + path + "` references a log without the Batch 84 runtime evidence token.");
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
            P0ResultSettlementBatch84UnityPreflightReport report)
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

        private static bool RequireEvidenceTokens(
            P0ResultSettlementBatch84UnityPreflightReport report,
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

        private static bool IsQueueEntryReadyForUnityReview()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();
            for (int i = 0; i < queue.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queue[i];
                if (entry.QueueId == P0AssetProductionQueueCatalog.ResultSettlementPreflightCandidateQueueId)
                {
                    return entry.State == P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview
                        && entry.Phase == P0AssetProductionQueuePhase.CodexCandidateProduction
                        && ContainsOrdinal(entry.CandidateDirectory, P0ResultSettlementBatch84CandidateCatalog.BatchSlug)
                        && ContainsOrdinal(entry.UnityImportRoot, "Assets/TheCat/Art/UI/ResultSettlement")
                        && ContainsOrdinal(entry.NextAction, "Unity-rendered")
                        && ContainsOrdinal(entry.NextAction, "result/settlement");
                }
            }

            return false;
        }

        private static bool IsDesignCandidatePath(string path)
        {
            return ContainsOrdinal(path, "design/development/asset_candidates/")
                && !ContainsOrdinal(path, "Assets/");
        }

        private static bool IsResultSettlementUnityImportPath(string path)
        {
            return ContainsOrdinal(path, "Assets/TheCat/Art/UI/ResultSettlement/")
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
            P0ResultSettlementBatch84UnityPreflightReport report,
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
