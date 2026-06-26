using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0BattleHudBatch87UnityPreflightTests
    {
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/batch87_clean.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0BattleHudBatch87UnityPreflightReport report = P0BattleHudBatch87UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEvidenceComplete, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalRuntimeBindingDecisionApproved, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.LessOrEqual(report.UnityEvidenceCount, P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            Assert.IsTrue(report.SharedConsoleClassifierContractReady, report.BuildDetailedSummary());
            StringAssert.Contains("Shared Console classifier contract: ready", report.BuildDetailedSummary());
            StringAssert.Contains("known environment noise is classified but still blocks formal clean-Console approval", report.ConsoleClassifierPolicySummary);
            Assert.Greater(report.BlockingItems.Count, 0);
            Assert.GreaterOrEqual(report.CoveredChecks.Count, P0BattleHudBatch87UnityPreflight.ExpectedCoveredCheckCount);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
            StringAssert.Contains("Formal runtime binding decision has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresRuntimeEvidence()
        {
            P0BattleHudBatch87UnityPreflightReport report =
                P0BattleHudBatch87UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEvidenceComplete, report.BuildDetailedSummary());
            StringAssert.Contains("human_review_approval.md", report.BuildDetailedSummary());
        }

        [Test]
        public void CandidateCatalog_DoesNotLeakIntoFormalRuntimeVisualBindings()
        {
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates = P0BattleHudBatch87CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings = P0BattleHudBatch87CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();

            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/BattleHUD/", entry.UnityImportPath);
                StringAssert.StartsWith(P0BattleHudBatch87CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0BattleHudBatch87UnityPreflightReport report =
                P0BattleHudBatch87UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 87 Battle HUD Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Unity evidence complete: no", markdown);
            StringAssert.Contains("Formal runtime binding decision approved: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/BattleHUD", markdown);
            StringAssert.Contains("battle_top_resource_rail_frame", markdown);
            StringAssert.Contains("batch_87_battle_hud_unity_preflight", markdown);
            StringAssert.Contains("Shared Console classifier: active strict-clean contract", markdown);
            StringAssert.Contains("known environment noise is classified but still blocks formal clean-Console approval", markdown);
            StringAssert.Contains(P0BattleHudBatch87UnityPreflight.HumanReviewRequestPath, markdown);
            StringAssert.Contains(P0BattleHudBatch87UnityPreflight.FormalRuntimeBindingDecisionRequestPath, markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void BuildHumanReviewRequestMarkdown_ListsEvidenceWithoutCreatingApproval()
        {
            P0BattleHudBatch87UnityPreflightReport report =
                P0BattleHudBatch87UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildHumanReviewRequestMarkdown();

            StringAssert.Contains("Batch 87 Battle HUD Human Review Request", markdown);
            StringAssert.Contains("review-request packet only", markdown);
            StringAssert.Contains("must not be counted as formal approval evidence", markdown);
            StringAssert.Contains("Review request ready: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains(P0BattleHudBatch87UnityPreflight.ReportPath, markdown);
            StringAssert.Contains(P0BattleHudBatch87UnityPreflight.FormalRuntimeBindingDecisionRequestPath, markdown);
            StringAssert.Contains(P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath, markdown);
            StringAssert.Contains(P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[0].EvidencePath, markdown);
            StringAssert.Contains(P0BattleHudBatch87RuntimeEvidence.TextAndSkillStateReviewPath, markdown);
            StringAssert.Contains("StrictClean", markdown);
            StringAssert.Contains("human_review_approval.md", markdown);
            Assert.IsFalse(markdown.Contains("Formal install approved: yes"), markdown);
            StringAssert.Contains("Do not add Batch 87 candidate ids to formal runtime bindings", markdown);
            StringAssert.Contains("Keep the current IMGUI battle HUD as the playable runtime path", markdown);
        }

        [Test]
        public void BuildFormalRuntimeBindingDecisionRequestMarkdown_ListsScopeWithoutApprovingBinding()
        {
            P0BattleHudBatch87UnityPreflightReport report =
                P0BattleHudBatch87UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildFormalRuntimeBindingDecisionRequestMarkdown();

            StringAssert.Contains("Batch 87 Battle HUD Formal Runtime Binding Decision Request", markdown);
            StringAssert.Contains("decision request only", markdown);
            StringAssert.Contains("does not change runtime bindings", markdown);
            StringAssert.Contains("Decision request ready: yes", markdown);
            StringAssert.Contains("Formal runtime binding decision approved: no", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.CreateP0RuntimeBindings()", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/BattleHUD", markdown);
            StringAssert.Contains("battle_top_resource_rail_frame", markdown);
            StringAssert.Contains("StrictClean", markdown);
            Assert.IsFalse(markdown.Contains("Formal runtime binding decision approved: yes"), markdown);
            StringAssert.Contains("Do not add Batch 87 candidate ids to `P0VisualAssetCatalog`", markdown);
            StringAssert.Contains("Keep the current IMGUI battle HUD as the playable runtime path", markdown);
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutFormalApproval()
        {
            Assert.IsTrue(P0BattleHudBatch87RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.IsTrue(P0BattleHudBatch87RuntimeEvidence.DoesNotAutoGenerateFormalApprovalEvidence());
            Assert.AreEqual(P0BattleHudBatch87RuntimeEvidence.ExpectedScreenshotCount, P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0BattleHudBatch87RuntimeEvidence.ExpectedAutomaticReviewCount, P0BattleHudBatch87RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);

            for (int i = 0; i < P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                P0BattleHudBatch87ScreenshotTarget target = P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[i];
                Assert.AreEqual(P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[i], target.EvidencePath);
                StringAssert.Contains(target.ResolutionLabel, target.FileName);
                Assert.GreaterOrEqual(target.Width, 1024);
                Assert.GreaterOrEqual(target.Height, 720);
            }

            Assert.AreEqual(
                P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[5],
                P0BattleHudBatch87RuntimeEvidence.TextAndSkillStateReviewPath);
            Assert.AreEqual(
                P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[6],
                P0BattleHudBatch87RuntimeEvidence.TelegraphOcclusionClickTargetReviewPath);
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            P0BattleHudBatch87ScreenshotTarget target = P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[0];
            try
            {
                P0BattleHudBatch87RuntimeEvidence.SetBeforeCaptureResolutionOverride(
                    requested => "configured " + requested.ResolutionLabel);

                string summary = P0BattleHudBatch87RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);

                StringAssert.Contains(target.ResolutionLabel, summary);
            }
            finally
            {
                P0BattleHudBatch87RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            }

            Assert.AreEqual("none", P0BattleHudBatch87RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));
        }

        [Test]
        public void RuntimeEvidenceVisualContentGuard_RejectsFlatScreenshots()
        {
            Color32[] flatPixels = BuildPixels(64, 64, new Color32(191, 191, 191, 255), varied: false);
            Color32[] variedPixels = BuildPixels(64, 64, new Color32(32, 58, 72, 255), varied: true);

            Assert.IsFalse(P0BattleHudBatch87RuntimeEvidence.HasUsefulVisualContent(flatPixels, 64, 64, out string flatSummary), flatSummary);
            Assert.IsTrue(P0BattleHudBatch87RuntimeEvidence.HasUsefulVisualContent(variedPixels, 64, 64, out string variedSummary), variedSummary);
        }

        [Test]
        public void Evaluate_VisuallyBlankScreenshotEvidenceDoesNotCount()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(
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
                P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount - P0BattleHudBatch87RuntimeEvidence.ExpectedScreenshotCount,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("visually blank or incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightApprovalTokensButKeepsManualGates()
        {
            string textReview = P0BattleHudBatch87RuntimeEvidence.BuildTextAndSkillStateReviewMarkdown(
                "hud summary",
                P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets);
            string telegraphReview = P0BattleHudBatch87RuntimeEvidence.BuildTelegraphOcclusionClickTargetReviewMarkdown(
                "layout summary",
                P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets);
            string runtimeReport = P0BattleHudBatch87RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0BattleHudBatch87RuntimeEvidenceState.Passed,
                "summary",
                "detail",
                new[] { P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[0].EvidencePath },
                P0BattleHudBatch87RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Review result: pass", textReview);
            StringAssert.Contains("ready", textReview);
            StringAssert.Contains("selected", textReview);
            StringAssert.Contains("cooldown", textReview);
            StringAssert.Contains("disabled", textReview);
            StringAssert.Contains("low-resource", textReview);
            StringAssert.Contains("Review result: pass", telegraphReview);
            StringAssert.Contains("44 px", telegraphReview);
            StringAssert.Contains("Complete screenshot evidence: no", runtimeReport);
            StringAssert.Contains("Candidate frame draws: incomplete", runtimeReport);
            StringAssert.Contains("No candidate texture fallback: no", runtimeReport);
            StringAssert.Contains("Formal install allowed: no", runtimeReport);
            StringAssert.Contains("console_clean_report.md", runtimeReport);
            StringAssert.Contains("human_review_approval.md", runtimeReport);
        }

        [Test]
        public void Evaluate_AllEvidenceButEmptyApprovalBlocksFormalInstall()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                path => path.EndsWith("human_review_approval.md", StringComparison.Ordinal)
                    ? string.Empty
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount - 1, report.UnityEvidenceCount);
            StringAssert.Contains("human_review_approval.md", report.BuildDetailedSummary());
            StringAssert.Contains("is empty", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsCompletesEvidenceButDoesNotAllowFormalInstall()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.UnityEvidenceComplete, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalRuntimeBindingDecisionApproved, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            StringAssert.Contains("Formal runtime binding decision has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_TokenOnlyConsoleAndHumanApprovalDoNotAllowFormalInstall()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 87 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 87 Human Approval\n\nFormal install approved: yes\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Unity log reviewed: yes", report.BuildDetailedSummary());
            StringAssert.Contains("Reviewer:", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ConsoleCleanReportWithMissingLogDoesNotAllowFormalInstall()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                path => path.EndsWith("console_clean_report.md", StringComparison.Ordinal)
                    ? "# Batch 87 Console Clean Report\n\nConsole clean: yes\nUnity log reviewed: yes\nBatch 87 runtime evidence log: Logs/missing_batch87.log\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("references missing runtime evidence log", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ConsoleCleanReportWithDirtyLogDoesNotAllowFormalInstall()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 87 battle HUD runtime evidence passed: clean until here\n[Licensing::Client] Error: noisy entitlement line\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Console clean report is not accepted", report.BuildDetailedSummary());
            StringAssert.Contains("not strict clean", report.BuildDetailedSummary());
            StringAssert.Contains("known environment noise is classified but still blocks formal clean-Console approval", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_CorrectScreenshotDimensionsWithoutCompleteRuntimeReportDoNotCount()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
                path == P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath
                    ? "# Batch 87 Runtime Evidence\n\n- Complete screenshot evidence: no\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount - P0BattleHudBatch87RuntimeEvidence.ExpectedScreenshotCount,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("not confirmed by a complete candidate-drawn Batch 87 runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_CorrectScreenshotDimensionsWithoutCandidateDrawAuditDoNotCount()
        {
            P0BattleHudBatch87UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
                path == P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath
                    ? "# Batch 87 Runtime Evidence\n\n"
                        + "- Complete screenshot evidence: yes\n"
                        + "- "
                        + string.Join("\n- ", P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths)
                        + "\n"
                    : BuildValidEvidenceText(path));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0BattleHudBatch87UnityPreflight.ExpectedUnityEvidenceRequiredCount - P0BattleHudBatch87RuntimeEvidence.ExpectedScreenshotCount,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains(P0BattleHudBatch87RuntimeEvidence.CandidateFrameDrawToken, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0BattleHudBatch87UnityPreflightReport report = P0BattleHudBatch87UnityPreflight.Evaluate(
                P0BattleHudBatch87CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_battle_hud_battle_runtime_control_cluster_360x96_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0BattleHudBatch87UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
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

        private static P0BattleHudBatch87UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates =
                P0BattleHudBatch87CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);

            return P0BattleHudBatch87UnityPreflight.Evaluate(
                candidates,
                existingPaths.Contains,
                ReadExpectedDimensions,
                unityEditorImportValidationReady: true,
                readText,
                readVisualContent);
        }

        private static Color32[] BuildPixels(int width, int height, Color32 baseColor, bool varied)
        {
            Color32[] pixels = new Color32[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color32 pixel = baseColor;
                    if (varied)
                    {
                        pixel.r = (byte)((baseColor.r + x * 4) % 255);
                        pixel.g = (byte)((baseColor.g + y * 5) % 255);
                        pixel.b = (byte)((baseColor.b + (x + y) * 3) % 255);
                    }

                    pixels[y * width + x] = pixel;
                }
            }

            return pixels;
        }

        private static string BuildValidEvidenceText(string path)
        {
            if (path.EndsWith("console_clean_report.md", StringComparison.Ordinal))
            {
                return "# Batch 87 Console Clean Report\n\nConsole clean: yes\nUnity log reviewed: yes\nBatch 87 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 87 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-25\n";
            }

            if (path == P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath)
            {
                return "# Batch 87 Runtime Evidence\n\n"
                    + "- Complete screenshot evidence: yes\n"
                    + "- Candidate frame draws: 6/6\n"
                    + "- No candidate texture fallback: yes\n"
                    + "- "
                    + string.Join("\n- ", P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths)
                    + "\n";
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 87 battle HUD runtime evidence passed: synthetic clean log\n";
            }

            return "# Batch 87 Review\n\nReview result: pass\n";
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

            foreach (P0BattleHudBatch87CandidateAsset candidate in P0BattleHudBatch87CandidateCatalog.CreateCandidates())
            {
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                if (entry.UnityImportPath != unityImportPath)
                {
                    continue;
                }

                return P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out width, out height);
            }

            for (int i = 0; i < P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length; i++)
            {
                P0BattleHudBatch87ScreenshotTarget target = P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets[i];
                if (target.EvidencePath != unityImportPath)
                {
                    continue;
                }

                width = target.Width;
                height = target.Height;
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
