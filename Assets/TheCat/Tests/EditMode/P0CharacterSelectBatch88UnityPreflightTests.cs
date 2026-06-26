using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0CharacterSelectBatch88UnityPreflightTests
    {
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/synthetic_batch88_runtime_evidence.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0CharacterSelectBatch88UnityPreflightReport report =
                P0CharacterSelectBatch88UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.LessOrEqual(report.UnityEvidenceCount, P0CharacterSelectBatch88UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0CharacterSelectBatch88UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            Assert.Greater(report.BlockingItems.Count, 0);
            Assert.GreaterOrEqual(report.CoveredChecks.Count, P0CharacterSelectBatch88UnityPreflight.ExpectedCoveredCheckCount);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresManualEvidence()
        {
            P0CharacterSelectBatch88UnityPreflightReport report =
                P0CharacterSelectBatch88UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

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
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates =
                P0CharacterSelectBatch88CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings =
                P0CharacterSelectBatch88CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            HashSet<string> preflightBindingIds = new HashSet<string>(StringComparer.Ordinal);

            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/CharacterSelect/", entry.UnityImportPath);
                StringAssert.StartsWith(P0CharacterSelectBatch88CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                Assert.IsTrue(preflightBindingIds.Add(preflightBindings[i].BindingId), preflightBindings[i].BindingId);
                StringAssert.Contains(candidates[i].VariantId, preflightBindings[i].BindingId);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutManualGates()
        {
            Assert.IsTrue(P0CharacterSelectBatch88RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.IsTrue(P0CharacterSelectBatch88RuntimeEvidence.DoesNotAutoGenerateManualGateEvidence());
            Assert.AreEqual(P0CharacterSelectBatch88RuntimeEvidence.ExpectedScreenshotCount, P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0CharacterSelectBatch88RuntimeEvidence.ExpectedAutomaticReviewCount, P0CharacterSelectBatch88RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);

            for (int i = 0; i < P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                P0CharacterSelectBatch88ScreenshotTarget target = P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets[i];
                Assert.AreEqual(P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[i], target.EvidencePath);
                StringAssert.Contains(target.ResolutionLabel, target.FileName);
                Assert.GreaterOrEqual(target.Width, 1024);
                Assert.GreaterOrEqual(target.Height, 720);
            }

            Assert.AreEqual(
                P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[4],
                P0CharacterSelectBatch88RuntimeEvidence.SourceLockedAvatarConsistencyReviewPath);
            Assert.AreEqual(
                P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[5],
                P0CharacterSelectBatch88RuntimeEvidence.ChineseTextDensityClickTargetReviewPath);
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            P0CharacterSelectBatch88ScreenshotTarget target = P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets[0];
            try
            {
                P0CharacterSelectBatch88RuntimeEvidence.SetBeforeCaptureResolutionOverride(
                    requested => "configured " + requested.ResolutionLabel);

                string summary = P0CharacterSelectBatch88RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);

                StringAssert.Contains(target.ResolutionLabel, summary);
            }
            finally
            {
                P0CharacterSelectBatch88RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            }

            Assert.AreEqual("none", P0CharacterSelectBatch88RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightTokensButKeepsManualGates()
        {
            string avatarReview = P0CharacterSelectBatch88RuntimeEvidence.BuildSourceLockedAvatarConsistencyReviewMarkdown(
                "avatar summary",
                P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets);
            string textReview = P0CharacterSelectBatch88RuntimeEvidence.BuildChineseTextDensityClickTargetReviewMarkdown(
                "layout summary",
                P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets);
            string detail = "Candidate frame draw audit passed for 1920x1080: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1365x768: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1280x720: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1024x768: 6/6 candidate textures drawn; fallback=0\n"
                + "Source-locked HUD avatars visible: Saiban avatar, Nephthys avatar, Suzune avatar\n"
                + "Selected and idle card states visible: selected=1 idle=2";
            string runtimeReport = P0CharacterSelectBatch88RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0CharacterSelectBatch88RuntimeEvidenceState.Passed,
                "summary",
                detail,
                new[]
                {
                    P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets[0].EvidencePath,
                    P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets[1].EvidencePath,
                    P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets[2].EvidencePath,
                    P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets[3].EvidencePath
                },
                P0CharacterSelectBatch88RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Review result: pass", avatarReview);
            StringAssert.Contains("Source-lock HUD avatar consistency: pass", avatarReview);
            StringAssert.Contains("Saiban avatar: source-locked", avatarReview);
            StringAssert.Contains("Chinese names/roles/descriptions/start labels: pass", textReview);
            StringAssert.Contains("Selected/idle distinction: pass", textReview);
            StringAssert.Contains("1024x768 density: pass", textReview);
            StringAssert.Contains("Click targets: pass", textReview);
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.CompleteScreenshotEvidenceToken, runtimeReport);
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.CharacterSelectSurfaceCapturedToken, runtimeReport);
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.CandidateFrameDrawToken, runtimeReport);
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.NoCandidateTextureFallbackToken, runtimeReport);
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.SourceLockedAvatarsVisibleToken, runtimeReport);
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.SelectedIdleStatesVisibleToken, runtimeReport);
            StringAssert.Contains("scene_binding_console_clean_report.md", runtimeReport);
            StringAssert.Contains("human_review_approval.md", runtimeReport);
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0CharacterSelectBatch88UnityPreflightReport report =
                P0CharacterSelectBatch88UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 88 Character Select Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/CharacterSelect", markdown);
            StringAssert.Contains("starter_card_frame_selected", markdown);
            StringAssert.Contains("batch_88_character_select_unity_preflight", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsAllowsFormalInstall()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0CharacterSelectBatch88UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            Assert.AreEqual(0, report.BlockingItems.Count, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_TokenOnlyConsoleAndHumanApprovalDoNotAllowFormalInstall()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 88 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 88 Human Approval\n\nFormal install approved: yes\n";
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
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                path => path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal)
                    ? "# Batch 88 Scene Binding Console Clean Report\n\nRuntime scene/presenter binding: yes\nConsole clean: yes\nUnity log reviewed: yes\nBatch 88 runtime evidence log: Logs/missing_batch88.log\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("references missing runtime evidence log", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_SceneBindingConsoleReportWithDirtyLogDoesNotAllowFormalInstall()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 88 character-select runtime evidence passed: clean until here\n[Licensing::Client] Error: noisy entitlement line\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Console clean report is not accepted", report.BuildDetailedSummary());
            StringAssert.Contains("not strict clean", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_GenericPassReviewsDoNotAllowFormalInstall()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("source_locked_avatar_consistency_review.md", StringComparison.Ordinal)
                    || path.EndsWith("chinese_text_density_click_target_review.md", StringComparison.Ordinal))
                {
                    return "# Batch 88 Review\n\nReview result: pass\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Source-lock HUD avatar consistency: pass", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ScreenshotDimensionsWithoutRuntimeReportDoNotCount()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
                path == P0CharacterSelectBatch88UnityPreflight.RuntimeEvidenceReportPath
                    ? "# Batch 88 Runtime Evidence\n\n- "
                        + P0CharacterSelectBatch88UnityPreflight.CompleteScreenshotEvidenceToken
                        + "\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0CharacterSelectBatch88UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains(P0CharacterSelectBatch88UnityPreflight.CharacterSelectSurfaceCapturedToken, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_VisuallyBlankScreenshotsDoNotCount()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = EvaluateWithSyntheticEvidence(
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
                P0CharacterSelectBatch88UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("visually blank or incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0CharacterSelectBatch88UnityPreflightReport report = P0CharacterSelectBatch88UnityPreflight.Evaluate(
                P0CharacterSelectBatch88CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_character_select_starter_launch_button_frame_420x112_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0CharacterSelectBatch88UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
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

        private static P0CharacterSelectBatch88UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates =
                P0CharacterSelectBatch88CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            existingPaths.Add(P0CharacterSelectBatch88UnityPreflight.RuntimeEvidenceReportPath);
            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);

            return P0CharacterSelectBatch88UnityPreflight.Evaluate(
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
                return "# Batch 88 Scene Binding Console Clean Report\n\nRuntime scene/presenter binding: yes\nConsole clean: yes\nUnity log reviewed: yes\nBatch 88 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 88 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-25\n";
            }

            if (path.EndsWith("source_locked_avatar_consistency_review.md", StringComparison.Ordinal))
            {
                return "# Batch 88 Source-Lock Avatar Consistency Review\n\n"
                    + "Review result: pass\n"
                    + "Source-lock HUD avatar consistency: pass\n"
                    + "Saiban avatar: source-locked\n"
                    + "Nephthys avatar: source-locked\n"
                    + "Suzune avatar: source-locked\n";
            }

            if (path.EndsWith("chinese_text_density_click_target_review.md", StringComparison.Ordinal))
            {
                return "# Batch 88 Chinese Text Density Click Target Review\n\n"
                    + "Review result: pass\n"
                    + "Chinese names/roles/descriptions/start labels: pass\n"
                    + "Selected/idle distinction: pass\n"
                    + "1024x768 density: pass\n"
                    + "Click targets: pass\n";
            }

            if (path == P0CharacterSelectBatch88UnityPreflight.RuntimeEvidenceReportPath)
            {
                return "# Batch 88 Runtime Evidence\n\n"
                    + "- " + P0CharacterSelectBatch88UnityPreflight.CompleteScreenshotEvidenceToken + "\n"
                    + "- " + P0CharacterSelectBatch88UnityPreflight.CharacterSelectSurfaceCapturedToken + "\n"
                    + "- " + P0CharacterSelectBatch88UnityPreflight.CandidateFrameDrawToken + "\n"
                    + "- " + P0CharacterSelectBatch88UnityPreflight.NoCandidateTextureFallbackToken + "\n"
                    + "- " + P0CharacterSelectBatch88UnityPreflight.SourceLockedAvatarsVisibleToken + "\n"
                    + "- " + P0CharacterSelectBatch88UnityPreflight.SelectedIdleStatesVisibleToken + "\n"
                    + "- "
                    + string.Join("\n- ", P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths)
                    + "\n";
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 88 character-select runtime evidence passed: synthetic clean log\n";
            }

            return "# Batch 88 Review\n\nReview result: pass\n";
        }

        private static void AssertNoRuntimeBinding(P0VisualAssetBinding[] runtimeBindings, string assetId, string unityImportPath)
        {
            for (int i = 0; i < runtimeBindings.Length; i++)
            {
                Assert.AreNotEqual(assetId, runtimeBindings[i].Asset.AssetId);
                Assert.AreNotEqual(unityImportPath, runtimeBindings[i].Asset.UnityImportPath);
            }
        }

        private static bool ReadExpectedDimensions(string unityImportPath, out int width, out int height, out string error)
        {
            width = 0;
            height = 0;
            error = string.Empty;

            foreach (P0CharacterSelectBatch88CandidateAsset candidate in P0CharacterSelectBatch88CandidateCatalog.CreateCandidates())
            {
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                if (entry.UnityImportPath != unityImportPath)
                {
                    continue;
                }

                return P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out width, out height);
            }

            if (unityImportPath.Contains("1920x1080"))
            {
                width = 1920;
                height = 1080;
                return true;
            }

            if (unityImportPath.Contains("1365x768"))
            {
                width = 1365;
                height = 768;
                return true;
            }

            if (unityImportPath.Contains("1280x720"))
            {
                width = 1280;
                height = 720;
                return true;
            }

            if (unityImportPath.Contains("1024x768"))
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
