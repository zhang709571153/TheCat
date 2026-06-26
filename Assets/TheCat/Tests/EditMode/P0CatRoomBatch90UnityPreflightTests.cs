using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0CatRoomBatch90UnityPreflightTests
    {
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/synthetic_batch90_cat_room_runtime_evidence.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0CatRoomBatch90UnityPreflightReport report =
                P0CatRoomBatch90UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.Less(report.UnityEvidenceCount, P0CatRoomBatch90UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0CatRoomBatch90UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            Assert.Greater(report.BlockingItems.Count, 0);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresManualEvidence()
        {
            P0CatRoomBatch90UnityPreflightReport report =
                P0CatRoomBatch90UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

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
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates =
                P0CatRoomBatch90CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings =
                P0CatRoomBatch90CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            HashSet<string> preflightBindingIds = new HashSet<string>(StringComparer.Ordinal);

            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/CatRoom/", entry.UnityImportPath);
                StringAssert.StartsWith(P0CatRoomBatch90CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                Assert.IsTrue(preflightBindingIds.Add(preflightBindings[i].BindingId), preflightBindings[i].BindingId);
                StringAssert.Contains(candidates[i].VariantId, preflightBindings[i].BindingId);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0CatRoomBatch90UnityPreflightReport report =
                P0CatRoomBatch90UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 90 Cat Room Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/CatRoom", markdown);
            StringAssert.Contains("cat_room_stage_panel_frame", markdown);
            StringAssert.Contains("batch_90_cat_room_unity_preflight", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutManualGates()
        {
            Assert.IsTrue(P0CatRoomBatch90RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.IsTrue(P0CatRoomBatch90RuntimeEvidence.DoesNotAutoGenerateManualGateEvidence());
            Assert.AreEqual(P0CatRoomBatch90RuntimeEvidence.ExpectedScreenshotCount, P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0CatRoomBatch90RuntimeEvidence.ExpectedAutomaticReviewCount, P0CatRoomBatch90RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);
            Assert.AreEqual(
                P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[4],
                P0CatRoomBatch90RuntimeEvidence.InteractionStateTextDensityReviewPath);
            Assert.AreEqual(
                P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[5],
                P0CatRoomBatch90RuntimeEvidence.ClickTargetPropScaleReviewPath);
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            try
            {
                P0CatRoomBatch90RuntimeEvidence.SetBeforeCaptureResolutionOverride(
                    target => "override " + target.ResolutionLabel);

                Assert.AreEqual(
                    "override 1920x1080",
                    P0CatRoomBatch90RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(
                        P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets[0]));
            }
            finally
            {
                P0CatRoomBatch90RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            }

            Assert.AreEqual(
                "none",
                P0CatRoomBatch90RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(
                    P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets[0]));
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightTokensButKeepsManualGates()
        {
            string interactionReview = P0CatRoomBatch90RuntimeEvidence.BuildInteractionStateTextDensityReviewMarkdown(
                "1024x768 layout validated",
                P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets);
            string clickReview = P0CatRoomBatch90RuntimeEvidence.BuildClickTargetPropScaleReviewMarkdown(
                "click targets >= 44px",
                P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets);
            string runtimeReport = P0CatRoomBatch90RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0CatRoomBatch90RuntimeEvidenceState.Passed,
                "synthetic passed",
                BuildRuntimeEvidenceDetail(),
                BuildRuntimeEvidenceScreenshotPaths(),
                P0CatRoomBatch90RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Bed/feeder/litter/dream entrance states: pass", interactionReview);
            StringAssert.Contains("Hover/disabled/blocked/range states: pass", interactionReview);
            StringAssert.Contains("1024x768 density: pass", interactionReview);
            StringAssert.Contains("Dream entrance semantics: pass", clickReview);
            StringAssert.Contains("Prop scale: pass", clickReview);
            StringAssert.Contains(P0CatRoomBatch90UnityPreflight.CompleteScreenshotEvidenceToken, runtimeReport);
            StringAssert.Contains(P0CatRoomBatch90UnityPreflight.CandidateFrameDrawToken, runtimeReport);
            StringAssert.Contains(P0CatRoomBatch90UnityPreflight.NoCandidateTextureFallbackToken, runtimeReport);
            StringAssert.Contains("Remaining manual gates: scene_binding_console_clean_report.md, human_review_approval.md", runtimeReport);
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsAllowsFormalInstall()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0CatRoomBatch90UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            Assert.AreEqual(0, report.BlockingItems.Count, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_TokenOnlyConsoleAndHumanApprovalDoNotAllowFormalInstall()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 90 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 90 Human Approval\n\nFormal install approved: yes\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Runtime scene/presenter binding: yes", report.BuildDetailedSummary());
            StringAssert.Contains("Reviewer:", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_SceneBindingConsoleReportWithMissingLogDoesNotAllowFormalInstall()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                path => path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal)
                    ? "# Batch 90 Scene Binding Console Clean Report\n\nRuntime scene/presenter binding: yes\nConsole clean: yes\nUnity log reviewed: yes\nBatch 90 runtime evidence log: Logs/missing_batch90.log\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("references missing Batch 90 runtime evidence log", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_SceneBindingConsoleReportWithDirtyLogDoesNotAllowFormalInstall()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 90 cat-room runtime evidence passed: clean until here\nCurl error 56: entitlement CDN noise\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Console clean report is not accepted", report.BuildDetailedSummary());
            StringAssert.Contains("not strict clean", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ScreenshotDimensionsWithoutRuntimeReportDoNotCount()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                BuildValidEvidenceText,
                (string path, out string summary, out string error) =>
                {
                    summary = "synthetic visual content";
                    error = string.Empty;
                    return true;
                },
                includeRuntimeEvidenceReport: false);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0CatRoomBatch90UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("Missing Batch 90 runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_GenericPassReviewsDoNotAllowFormalInstall()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("interaction_state_text_density_review.md", StringComparison.Ordinal)
                    || path.EndsWith("click_target_prop_scale_review.md", StringComparison.Ordinal))
                {
                    return "# Batch 90 Review\n\nReview result: pass\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Bed/feeder/litter/dream entrance states: pass", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_VisuallyBlankScreenshotsDoNotCount()
        {
            P0CatRoomBatch90UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                BuildValidEvidenceText,
                (string path, out string summary, out string error) =>
                {
                    summary = path.EndsWith(".png", StringComparison.Ordinal) ? "flat capture" : "not a screenshot";
                    error = string.Empty;
                    return !path.EndsWith(".png", StringComparison.Ordinal);
                });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0CatRoomBatch90UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("visually blank or incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0CatRoomBatch90UnityPreflightReport report = P0CatRoomBatch90UnityPreflight.Evaluate(
                P0CatRoomBatch90CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_cat_room_cat_room_dream_entrance_button_frame_420x112_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0CatRoomBatch90UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
        {
            return EvaluateWithSyntheticEvidence(
                readText,
                (string path, out string summary, out string error) =>
                {
                    summary = "synthetic visual content";
                    error = string.Empty;
                    return true;
                });
        }

        private static P0CatRoomBatch90UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent,
            bool includeRuntimeEvidenceReport = true)
        {
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates =
                P0CatRoomBatch90CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            if (includeRuntimeEvidenceReport)
            {
                existingPaths.Add(P0CatRoomBatch90UnityPreflight.RuntimeEvidenceReportPath);
            }

            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);

            return P0CatRoomBatch90UnityPreflight.Evaluate(
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
                return "# Batch 90 Scene Binding Console Clean Report\n\n"
                    + "Runtime scene/presenter binding: yes\n"
                    + "Console clean: yes\n"
                    + "Unity log reviewed: yes\n"
                    + "Batch 90 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 90 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-26\n";
            }

            if (path.EndsWith("interaction_state_text_density_review.md", StringComparison.Ordinal))
            {
                return "# Batch 90 Interaction State Text Density Review\n\n"
                    + "Review result: pass\n"
                    + "Bed/feeder/litter/dream entrance states: pass\n"
                    + "Hover/disabled/blocked/range states: pass\n"
                    + "Chinese labels and values: pass\n"
                    + "1024x768 density: pass\n";
            }

            if (path.EndsWith("click_target_prop_scale_review.md", StringComparison.Ordinal))
            {
                return "# Batch 90 Click Target Prop Scale Review\n\n"
                    + "Review result: pass\n"
                    + "Bed click target: pass\n"
                    + "Feeder click target: pass\n"
                    + "Litter click target: pass\n"
                    + "Dream entrance semantics: pass\n"
                    + "Prop scale: pass\n";
            }

            if (path == P0CatRoomBatch90UnityPreflight.RuntimeEvidenceReportPath)
            {
                return P0CatRoomBatch90RuntimeEvidence.BuildRuntimeReportMarkdown(
                    P0CatRoomBatch90RuntimeEvidenceState.Passed,
                    "synthetic passed",
                    BuildRuntimeEvidenceDetail(),
                    BuildRuntimeEvidenceScreenshotPaths(),
                    P0CatRoomBatch90RuntimeEvidence.AutomaticallyGeneratedReviewPaths);
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 90 cat-room runtime evidence passed: synthetic clean log\n";
            }

            return "# Batch 90 Review\n\nReview result: pass\n";
        }

        private static IReadOnlyList<string> BuildRuntimeEvidenceScreenshotPaths()
        {
            List<string> paths = new List<string>();
            for (int i = 0; i < P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                paths.Add(P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets[i].EvidencePath);
            }

            return paths;
        }

        private static string BuildRuntimeEvidenceDetail()
        {
            List<string> lines = new List<string>();
            for (int i = 0; i < P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                lines.Add("Candidate frame draw audit passed for "
                    + P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets[i].ResolutionLabel
                    + ": 6/6 candidate textures drawn; fallback=0");
            }

            lines.Add("Bed/feeder/litter/dream entrance states visible: bed=ready feeder=ready litter=attention dream=available");
            lines.Add("Hover/disabled/blocked/range states visible: hover=1 disabled=1 blocked=1 range=1");
            return string.Join("\n", lines);
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

            foreach (P0CatRoomBatch90CandidateAsset candidate in P0CatRoomBatch90CandidateCatalog.CreateCandidates())
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
