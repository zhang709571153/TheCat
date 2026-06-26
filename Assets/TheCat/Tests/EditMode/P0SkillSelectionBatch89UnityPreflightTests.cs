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
    public sealed class P0SkillSelectionBatch89UnityPreflightTests
    {
        private const string SyntheticPreflightLogPath = "Logs/synthetic_batch89_skill_selection_preflight.log";
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/synthetic_batch89_skill_selection_runtime_evidence.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0SkillSelectionBatch89UnityPreflightReport report =
                P0SkillSelectionBatch89UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.Less(report.UnityEvidenceCount, P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            Assert.Greater(report.BlockingItems.Count, 0);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresManualEvidence()
        {
            P0SkillSelectionBatch89UnityPreflightReport report =
                P0SkillSelectionBatch89UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

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
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates =
                P0SkillSelectionBatch89CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings =
                P0SkillSelectionBatch89CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            HashSet<string> preflightBindingIds = new HashSet<string>(StringComparer.Ordinal);

            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/SkillSelection/", entry.UnityImportPath);
                StringAssert.StartsWith(P0SkillSelectionBatch89CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                Assert.IsTrue(preflightBindingIds.Add(preflightBindings[i].BindingId), preflightBindings[i].BindingId);
                StringAssert.Contains(candidates[i].VariantId, preflightBindings[i].BindingId);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0SkillSelectionBatch89UnityPreflightReport report =
                P0SkillSelectionBatch89UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 89 Skill Selection Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/SkillSelection", markdown);
            StringAssert.Contains("skill_choice_card_selected", markdown);
            StringAssert.Contains("batch_89_skill_selection_unity_preflight", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsAllowsFormalInstall()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            Assert.AreEqual(0, report.BlockingItems.Count, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_TokenOnlyConsoleAndHumanApprovalDoNotAllowFormalInstall()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 89 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 89 Human Approval\n\nFormal install approved: yes\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Runtime scene/presenter binding: yes", report.BuildDetailedSummary());
            StringAssert.Contains("Reviewer:", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RuntimeEvidenceCompleteSummaryNamesOnlyManualGates()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 89 Scene Binding Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 89 Human Approval\n\nReviewer: pending manual review\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount - 2,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("runtime screenshots/reviews passed", report.BuildSummary());
            StringAssert.Contains("scene/Console and human approval gates", report.BuildSummary());
            StringAssert.DoesNotContain("blocked by screenshots, state semantics", report.BuildSummary());
        }

        [Test]
        public void Evaluate_SceneBindingConsoleReportWithMissingLogDoesNotAllowFormalInstall()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                path => path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal)
                    ? "# Batch 89 Scene Binding Console Clean Report\n\nRuntime scene/presenter binding: yes\nConsole clean: yes\nUnity log reviewed: yes\nBatch 89 runtime evidence log: Logs/missing_batch89.log\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("references missing Batch 89 runtime evidence log", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_SceneBindingConsoleReportWithDirtyLogDoesNotAllowFormalInstall()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 89 skill-selection runtime evidence passed: clean until here\n[Licensing::Client] Error: noisy entitlement line\n";
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
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("state_semantics_text_density_review.md", StringComparison.Ordinal)
                    || path.EndsWith("cooldown_low_resource_click_target_review.md", StringComparison.Ordinal))
                {
                    return "# Batch 89 Review\n\nReview result: pass\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Selected/ready/disabled/locked states: pass", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_VisuallyBlankScreenshotsDoNotCount()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(
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
                P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("visually blank or incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ScreenshotDimensionsWithoutRuntimeReportDoNotCount()
        {
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates =
                P0SkillSelectionBatch89CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = BuildSyntheticExistingPaths(candidates);
            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);
            existingPaths.Remove(P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath);

            P0SkillSelectionBatch89UnityPreflightReport report = P0SkillSelectionBatch89UnityPreflight.Evaluate(
                candidates,
                existingPaths.Contains,
                ReadExpectedDimensions,
                unityEditorImportValidationReady: true,
                BuildValidEvidenceText,
                (string path, out string summary, out string error) =>
                {
                    summary = "synthetic visual content";
                    error = string.Empty;
                    return true;
                });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RuntimeReportMissingScreenshotPathDoesNotCountThatScreenshot()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath)
                {
                    return BuildRuntimeEvidenceReportText(includeAllScreenshots: false, candidateDrawToken: P0SkillSelectionBatch89UnityPreflight.CandidateFrameDrawToken);
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount - 1,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("not listed in the Batch 89 runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RuntimeReportWithFallbackDoesNotCountScreenshots()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath)
                {
                    return BuildRuntimeEvidenceReportText(includeAllScreenshots: true, candidateDrawToken: "Candidate frame draws: 7/8");
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0SkillSelectionBatch89UnityPreflight.ExpectedUnityEvidenceRequiredCount - 4,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains(P0SkillSelectionBatch89UnityPreflight.CandidateFrameDrawToken, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_PreflightLogCannotStandInForRuntimeEvidenceLog()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 89 Scene Binding Console Clean Report\n\n"
                        + "Runtime scene/presenter binding: yes\n"
                        + "Console clean: yes\n"
                        + "Unity log reviewed: yes\n"
                        + "Batch 89 preflight log: " + SyntheticPreflightLogPath + "\n";
                }

                if (path == SyntheticPreflightLogPath)
                {
                    return "[TheCat] Batch 89 skill-selection preflight: synthetic clean log\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Batch 89 runtime evidence log", report.BuildDetailedSummary());
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutManualGates()
        {
            Assert.IsTrue(P0SkillSelectionBatch89RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.AreEqual(P0SkillSelectionBatch89RuntimeEvidence.ExpectedScreenshotCount, P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0SkillSelectionBatch89RuntimeEvidence.ExpectedAutomaticReviewCount, P0SkillSelectionBatch89RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);
            Assert.AreEqual(P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath, P0SkillSelectionBatch89RuntimeEvidence.RuntimeReportPath);
            Assert.IsTrue(P0SkillSelectionBatch89RuntimeEvidence.DoesNotAutoGenerateManualGateEvidence());
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            P0SkillSelectionBatch89ScreenshotTarget target = P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets[0];
            P0SkillSelectionBatch89RuntimeEvidence.SetBeforeCaptureResolutionOverride(t => "override " + t.ResolutionLabel);

            Assert.AreEqual("override " + target.ResolutionLabel, P0SkillSelectionBatch89RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));

            P0SkillSelectionBatch89RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            Assert.AreEqual("none", P0SkillSelectionBatch89RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightTokensButKeepsManualGates()
        {
            string stateReview = P0SkillSelectionBatch89RuntimeEvidence.BuildStateSemanticsTextDensityReviewMarkdown(
                "layout summary",
                P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets);
            string clickReview = P0SkillSelectionBatch89RuntimeEvidence.BuildCooldownLowResourceClickTargetReviewMarkdown(
                "click summary",
                P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets);
            string report = P0SkillSelectionBatch89RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0SkillSelectionBatch89RuntimeEvidenceState.Passed,
                "summary",
                "Candidate frame draw audit passed for 1920x1080: 8/8 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1365x768: 8/8 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1280x720: 8/8 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1024x768: 8/8 candidate textures drawn; fallback=0\n"
                + "Selected/ready/disabled/locked states visible: selected=1 ready=1 disabled=1 locked=1\n"
                + "Cooldown/low-resource/no-target semantics visible: cooldown=1 low-resource=1 no-target=1\n"
                + "Chinese skill labels and values visible: yes\n"
                + "Card/detail/confirm click targets visible: yes\n",
                P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths,
                P0SkillSelectionBatch89RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Selected/ready/disabled/locked states: pass", stateReview);
            StringAssert.Contains("Cooldown semantics: pass", clickReview);
            StringAssert.Contains(P0SkillSelectionBatch89UnityPreflight.CandidateFrameDrawToken, report);
            StringAssert.Contains(P0SkillSelectionBatch89UnityPreflight.SkillBlockSemanticsVisibleToken, report);
            StringAssert.Contains("Remaining manual gates", report);
            StringAssert.DoesNotContain("Formal install approved: yes", report);
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0SkillSelectionBatch89UnityPreflightReport report = P0SkillSelectionBatch89UnityPreflight.Evaluate(
                P0SkillSelectionBatch89CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_skill_selection_skill_confirm_button_frame_420x112_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0SkillSelectionBatch89UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
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

        private static P0SkillSelectionBatch89UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates =
                P0SkillSelectionBatch89CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = BuildSyntheticExistingPaths(candidates);
            existingPaths.Add(SyntheticPreflightLogPath);
            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);

            return P0SkillSelectionBatch89UnityPreflight.Evaluate(
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
                return "# Batch 89 Scene Binding Console Clean Report\n\n"
                    + "Runtime scene/presenter binding: yes\n"
                    + "Console clean: yes\n"
                    + "Unity log reviewed: yes\n"
                    + "Batch 89 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 89 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-26\n";
            }

            if (path.EndsWith("state_semantics_text_density_review.md", StringComparison.Ordinal))
            {
                return "# Batch 89 State Semantics Text Density Review\n\n"
                    + "Review result: pass\n"
                    + "Selected/ready/disabled/locked states: pass\n"
                    + "Chinese skill labels and values: pass\n"
                    + "1024x768 density: pass\n";
            }

            if (path.EndsWith("cooldown_low_resource_click_target_review.md", StringComparison.Ordinal))
            {
                return "# Batch 89 Cooldown Low Resource Click Target Review\n\n"
                    + "Review result: pass\n"
                    + "Cooldown semantics: pass\n"
                    + "Low-resource semantics: pass\n"
                    + "No-target semantics: pass\n"
                    + "Click targets: pass\n";
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 89 skill-selection runtime evidence passed: synthetic clean log\n";
            }

            if (path == SyntheticPreflightLogPath)
            {
                return "[TheCat] Batch 89 skill-selection preflight: synthetic clean log\n";
            }

            if (path == P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath)
            {
                return BuildRuntimeEvidenceReportText(includeAllScreenshots: true, candidateDrawToken: P0SkillSelectionBatch89UnityPreflight.CandidateFrameDrawToken);
            }

            return "# Batch 89 Review\n\nReview result: pass\n";
        }

        private static HashSet<string> BuildSyntheticExistingPaths(IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates)
        {
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            existingPaths.Add(P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath);
            return existingPaths;
        }

        private static string BuildRuntimeEvidenceReportText(bool includeAllScreenshots, string candidateDrawToken)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 89 Skill Selection Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: passed");
            builder.AppendLine("- Complete screenshot evidence: yes");
            builder.AppendLine("- Skill-selection surface captured: yes");
            builder.AppendLine("- " + candidateDrawToken);
            builder.AppendLine("- No candidate texture fallback: yes");
            builder.AppendLine("- Selected/ready/disabled/locked states visible: yes");
            builder.AppendLine("- Cooldown/low-resource/no-target semantics visible: yes");
            builder.AppendLine("- Chinese skill labels and values visible: yes");
            builder.AppendLine("- Card/detail/confirm click targets visible: yes");
            builder.AppendLine("- Formal install allowed: no");
            builder.AppendLine();
            builder.AppendLine("## Screenshots");
            int count = includeAllScreenshots
                ? P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths.Length
                : P0SkillSelectionBatch89RuntimeEvidence.ExpectedScreenshotCount - 1;
            for (int i = 0; i < count && i < P0SkillSelectionBatch89RuntimeEvidence.ExpectedScreenshotCount; i++)
            {
                builder.AppendLine("- `" + P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[i] + "`");
            }

            return builder.ToString();
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

            foreach (P0SkillSelectionBatch89CandidateAsset candidate in P0SkillSelectionBatch89CandidateCatalog.CreateCandidates())
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
