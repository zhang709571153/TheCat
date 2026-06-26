using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0LoadingStartBatch83UnityPreflightTests
    {
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/batch83_loading_start_runtime_evidence_synthetic.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0LoadingStartBatch83UnityPreflightReport report =
                P0LoadingStartBatch83UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.Less(report.UnityEvidenceCount, P0LoadingStartBatch83UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0LoadingStartBatch83UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresRuntimeEvidence()
        {
            P0LoadingStartBatch83UnityPreflightReport report =
                P0LoadingStartBatch83UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.Less(report.UnityEvidenceCount, report.UnityEvidenceRequiredCount, report.BuildDetailedSummary());
            StringAssert.Contains("scene_binding_console_clean_report.md", report.BuildDetailedSummary());
            StringAssert.Contains("human_review_approval.md", report.BuildDetailedSummary());
        }

        [Test]
        public void CandidateCatalog_DoesNotLeakIntoFormalRuntimeVisualBindings()
        {
            IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates =
                P0LoadingStartBatch83CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings =
                P0LoadingStartBatch83CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            HashSet<string> preflightBindingIds = new HashSet<string>(StringComparer.Ordinal);

            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/LoadingStart/", entry.UnityImportPath);
                StringAssert.StartsWith(P0LoadingStartBatch83CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                Assert.IsTrue(preflightBindingIds.Add(preflightBindings[i].BindingId), preflightBindings[i].BindingId);
                StringAssert.Contains(candidates[i].VariantId, preflightBindings[i].BindingId);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0LoadingStartBatch83UnityPreflightReport report =
                P0LoadingStartBatch83UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 83 Loading Start Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/LoadingStart", markdown);
            StringAssert.Contains("spinner_crescent", markdown);
            StringAssert.Contains("batch_83_loading_start_unity_preflight", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsAllowsFormalInstall()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0LoadingStartBatch83UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            Assert.AreEqual(0, report.BlockingItems.Count, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ScreenshotEvidenceWithoutRuntimeReportDoesNotAllowFormalInstall()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                BuildValidEvidenceText,
                AlwaysUsefulVisualContent,
                paths => paths.Remove(P0LoadingStartBatch83UnityPreflight.RuntimeEvidenceReportPath));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_BlankScreenshotEvidenceDoesNotAllowFormalInstall()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                BuildValidEvidenceText,
                (string path, out string summary, out string error) =>
                {
                    summary = "synthetic blank";
                    error = string.Empty;
                    return false;
                });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("visually blank", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_DirtyRuntimeEvidenceLogDoesNotAllowFormalInstall()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 83 loading/start runtime evidence passed: synthetic\nLicensing Client Error: dirty log\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Licensing Client", report.BuildDetailedSummary());
            StringAssert.Contains("not strict clean", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_TokenOnlyConsoleAndHumanApprovalDoNotAllowFormalInstall()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 83 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 83 Human Approval\n\nFormal install approved: yes\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Runtime scene/presenter binding: yes", report.BuildDetailedSummary());
            StringAssert.Contains("Reviewer:", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_GenericPassReviewsDoNotAllowFormalInstall()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("spinner_placeholder_text_density_review.md", StringComparison.Ordinal)
                    || path.EndsWith("progress_crowding_click_target_review.md", StringComparison.Ordinal))
                {
                    return "# Batch 83 Review\n\nReview result: pass\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Spinner interpretation: pass", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RuntimeEvidenceCompleteSummaryNamesOnlyManualGates()
        {
            P0LoadingStartBatch83UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 83 Scene Binding Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 83 Human Approval\n\nReviewer: pending manual review\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0LoadingStartBatch83UnityPreflight.ExpectedUnityEvidenceRequiredCount - 2,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("runtime screenshots/reviews passed", report.BuildSummary());
            StringAssert.Contains("scene/Console and human approval gates", report.BuildSummary());
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutManualGates()
        {
            Assert.IsTrue(P0LoadingStartBatch83RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.AreEqual(P0LoadingStartBatch83RuntimeEvidence.ExpectedScreenshotCount, P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0LoadingStartBatch83RuntimeEvidence.ExpectedAutomaticReviewCount, P0LoadingStartBatch83RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);
            Assert.AreEqual(P0LoadingStartBatch83UnityPreflight.RuntimeEvidenceReportPath, P0LoadingStartBatch83RuntimeEvidence.RuntimeReportPath);
            Assert.IsTrue(P0LoadingStartBatch83RuntimeEvidence.DoesNotAutoGenerateManualGateEvidence());
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            P0LoadingStartBatch83ScreenshotTarget target = P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets[0];
            P0LoadingStartBatch83RuntimeEvidence.SetBeforeCaptureResolutionOverride(t => "override " + t.ResolutionLabel);

            Assert.AreEqual("override " + target.ResolutionLabel, P0LoadingStartBatch83RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));

            P0LoadingStartBatch83RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            Assert.AreEqual("none", P0LoadingStartBatch83RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightTokensButKeepsManualGates()
        {
            string spinnerReview = P0LoadingStartBatch83RuntimeEvidence.BuildSpinnerPlaceholderTextDensityReviewMarkdown(
                "layout summary",
                P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets);
            string progressReview = P0LoadingStartBatch83RuntimeEvidence.BuildProgressCrowdingClickTargetReviewMarkdown(
                "click summary",
                P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets);
            string report = P0LoadingStartBatch83RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0LoadingStartBatch83RuntimeEvidenceState.Passed,
                "summary",
                "Candidate frame draw audit passed for 1920x1080: 4/4 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1365x768: 4/4 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1280x720: 4/4 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1024x768: 4/4 candidate textures drawn; fallback=0\n"
                + "Spinner and placeholder states visible: spinner=1 placeholder=1 compact=1\n"
                + "Progress readability visible: progress=1 dots=1 compact=1\n"
                + "Start/continue click targets visible: start=1 continue=1 minClickTarget=44\n",
                new[]
                {
                    P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[0],
                    P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[1],
                    P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[2],
                    P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[3]
                },
                P0LoadingStartBatch83RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Spinner interpretation: pass", spinnerReview);
            StringAssert.Contains("Progress readability: pass", progressReview);
            StringAssert.Contains(P0LoadingStartBatch83UnityPreflight.CandidateFrameDrawToken, report);
            StringAssert.Contains(P0LoadingStartBatch83UnityPreflight.StartContinueClickTargetsVisibleToken, report);
            StringAssert.Contains("Remaining manual gates", report);
            StringAssert.DoesNotContain("Formal install approved: yes", report);
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0LoadingStartBatch83UnityPreflightReport report = P0LoadingStartBatch83UnityPreflight.Evaluate(
                P0LoadingStartBatch83CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_loading_spinner_crescent_128x128_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0LoadingStartBatch83UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
        {
            return EvaluateWithSyntheticEvidence(readText, AlwaysUsefulVisualContent);
        }

        private static P0LoadingStartBatch83UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            return EvaluateWithSyntheticEvidence(readText, readVisualContent, configureExistingPaths: null);
        }

        private static P0LoadingStartBatch83UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent,
            Action<HashSet<string>> configureExistingPaths)
        {
            IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates =
                P0LoadingStartBatch83CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = BuildSyntheticExistingPaths(candidates);
            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);
            if (configureExistingPaths != null)
            {
                configureExistingPaths(existingPaths);
            }

            return P0LoadingStartBatch83UnityPreflight.Evaluate(
                candidates,
                existingPaths.Contains,
                ReadExpectedDimensions,
                unityEditorImportValidationReady: true,
                readText,
                readVisualContent);
        }

        private static string BuildValidEvidenceText(string path)
        {
            if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
            {
                return "# Batch 83 Scene Binding Console Clean Report\n\n"
                    + "Runtime scene/presenter binding: yes\n"
                    + "Console clean: yes\n"
                    + "Unity log reviewed: yes\n"
                    + "Batch 83 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 83 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-26\n";
            }

            if (path.EndsWith("spinner_placeholder_text_density_review.md", StringComparison.Ordinal))
            {
                return "# Batch 83 Spinner Placeholder Text Density Review\n\n"
                    + "Review result: pass\n"
                    + "Spinner interpretation: pass\n"
                    + "Placeholder text replacement: pass\n"
                    + "1024x768 density: pass\n";
            }

            if (path.EndsWith("progress_crowding_click_target_review.md", StringComparison.Ordinal))
            {
                return "# Batch 83 Progress Crowding Click Target Review\n\n"
                    + "Review result: pass\n"
                    + "Progress readability: pass\n"
                    + "Start/continue affordance: pass\n"
                    + "Click targets: pass\n";
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 83 loading/start runtime evidence passed: synthetic clean log\n";
            }

            if (path == P0LoadingStartBatch83UnityPreflight.RuntimeEvidenceReportPath)
            {
                return BuildRuntimeEvidenceReportText(includeAllScreenshots: true);
            }

            return "# Batch 83 Review\n\nReview result: pass\n";
        }

        private static HashSet<string> BuildSyntheticExistingPaths(IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates)
        {
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            existingPaths.Add(P0LoadingStartBatch83UnityPreflight.RuntimeEvidenceReportPath);
            return existingPaths;
        }

        private static string BuildRuntimeEvidenceReportText(bool includeAllScreenshots)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 83 Loading Start Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: passed");
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.CompleteScreenshotEvidenceToken);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.LoadingStartSurfaceCapturedToken);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.CandidateFrameDrawToken);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.NoCandidateTextureFallbackToken);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.SpinnerPlaceholderVisibleToken);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.ProgressReadabilityVisibleToken);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.StartContinueClickTargetsVisibleToken);
            builder.AppendLine("- Formal install allowed: no");
            builder.AppendLine();
            builder.AppendLine("## Screenshots");

            int screenshotCount = 0;
            for (int i = 0; i < P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                string path = P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[i];
                if (!path.EndsWith(".png", StringComparison.Ordinal))
                {
                    continue;
                }

                if (!includeAllScreenshots && screenshotCount >= 3)
                {
                    break;
                }

                builder.AppendLine("- `" + path + "`");
                screenshotCount++;
            }

            return builder.ToString();
        }

        private static bool AlwaysUsefulVisualContent(string path, out string summary, out string error)
        {
            summary = "synthetic visual content";
            error = string.Empty;
            return true;
        }

        private static void AssertNoRuntimeBinding(P0VisualAssetBinding[] runtimeBindings, string assetId, string unityImportPath)
        {
            for (int i = 0; i < runtimeBindings.Length; i++)
            {
                Assert.AreNotEqual(assetId, runtimeBindings[i].Asset.AssetId);
                Assert.AreNotEqual(unityImportPath, runtimeBindings[i].Asset.UnityImportPath);
            }
        }

        private static bool ReadExpectedDimensions(string path, out int width, out int height, out string error)
        {
            width = 0;
            height = 0;
            error = string.Empty;

            foreach (P0LoadingStartBatch83CandidateAsset candidate in P0LoadingStartBatch83CandidateCatalog.CreateCandidates())
            {
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                if (entry.UnityImportPath != path)
                {
                    continue;
                }

                return P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out width, out height);
            }

            if (path.Contains("1920x1080"))
            {
                width = 1920;
                height = 1080;
                return true;
            }

            if (path.Contains("1365x768"))
            {
                width = 1365;
                height = 768;
                return true;
            }

            if (path.Contains("1280x720"))
            {
                width = 1280;
                height = 720;
                return true;
            }

            if (path.Contains("1024x768"))
            {
                width = 1024;
                height = 768;
                return true;
            }

            error = "unknown path";
            return false;
        }

        private static string ResolveProjectPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
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
    }
}
