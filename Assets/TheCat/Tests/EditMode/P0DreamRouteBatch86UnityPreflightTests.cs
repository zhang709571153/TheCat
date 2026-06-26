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
    public sealed class P0DreamRouteBatch86UnityPreflightTests
    {
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/synthetic_batch86_dream_route_runtime_evidence.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0DreamRouteBatch86UnityPreflightReport report =
                P0DreamRouteBatch86UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.Less(report.UnityEvidenceCount, P0DreamRouteBatch86UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0DreamRouteBatch86UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresRuntimeEvidence()
        {
            P0DreamRouteBatch86UnityPreflightReport report =
                P0DreamRouteBatch86UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

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
            IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates =
                P0DreamRouteBatch86CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings =
                P0DreamRouteBatch86CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            HashSet<string> preflightBindingIds = new HashSet<string>(StringComparer.Ordinal);

            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/DreamRoute/", entry.UnityImportPath);
                StringAssert.StartsWith(P0DreamRouteBatch86CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                CollectionAssert.Contains(entry.SourceLockIds, "p0_dream_entry_surface");
                Assert.IsTrue(preflightBindingIds.Add(preflightBindings[i].BindingId), preflightBindings[i].BindingId);
                StringAssert.Contains(candidates[i].VariantId, preflightBindings[i].BindingId);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0DreamRouteBatch86UnityPreflightReport report =
                P0DreamRouteBatch86UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 86 Dream Route Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/DreamRoute", markdown);
            StringAssert.Contains("route_boss_gate_frame", markdown);
            StringAssert.Contains("batch_86_dream_route_unity_preflight", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsAllowsFormalInstall()
        {
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0DreamRouteBatch86UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            Assert.AreEqual(0, report.BlockingItems.Count, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ScreenshotEvidenceWithoutRuntimeReportDoesNotAllowFormalInstall()
        {
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                BuildValidEvidenceText,
                AlwaysUsefulVisualContent,
                paths => paths.Remove(P0DreamRouteBatch86UnityPreflight.RuntimeEvidenceReportPath));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_BlankScreenshotEvidenceDoesNotAllowFormalInstall()
        {
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(
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
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 86 dream-route runtime evidence passed: synthetic\nLicensing Client Error: dirty log\n";
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
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 86 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 86 Human Approval\n\nFormal install approved: yes\n";
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
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("text_replacement_node_path_semantics_review.md", StringComparison.Ordinal)
                    || path.EndsWith("route_choice_click_target_density_review.md", StringComparison.Ordinal))
                {
                    return "# Batch 86 Review\n\nReview result: pass\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("Unity-rendered text replacement: pass", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RuntimeEvidenceCompleteSummaryNamesOnlyManualGates()
        {
            P0DreamRouteBatch86UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 86 Scene Binding Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 86 Human Approval\n\nReviewer: pending manual review\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0DreamRouteBatch86UnityPreflight.ExpectedUnityEvidenceRequiredCount - 2,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("runtime screenshots/reviews passed", report.BuildSummary());
            StringAssert.Contains("scene/Console and human approval gates", report.BuildSummary());
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutManualGates()
        {
            Assert.IsTrue(P0DreamRouteBatch86RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.AreEqual(P0DreamRouteBatch86RuntimeEvidence.ExpectedScreenshotCount, P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0DreamRouteBatch86RuntimeEvidence.ExpectedAutomaticReviewCount, P0DreamRouteBatch86RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);
            Assert.AreEqual(P0DreamRouteBatch86UnityPreflight.RuntimeEvidenceReportPath, P0DreamRouteBatch86RuntimeEvidence.RuntimeReportPath);
            Assert.IsTrue(P0DreamRouteBatch86RuntimeEvidence.DoesNotAutoGenerateManualGateEvidence());
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            P0DreamRouteBatch86ScreenshotTarget target = P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets[0];
            P0DreamRouteBatch86RuntimeEvidence.SetBeforeCaptureResolutionOverride(t => "override " + t.ResolutionLabel);

            Assert.AreEqual("override " + target.ResolutionLabel, P0DreamRouteBatch86RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));

            P0DreamRouteBatch86RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            Assert.AreEqual("none", P0DreamRouteBatch86RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightTokensButKeepsManualGates()
        {
            string semanticsReview = P0DreamRouteBatch86RuntimeEvidence.BuildTextReplacementNodePathSemanticsReviewMarkdown(
                "layout summary",
                P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets);
            string clickReview = P0DreamRouteBatch86RuntimeEvidence.BuildRouteChoiceClickTargetDensityReviewMarkdown(
                "click summary",
                P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets);
            string report = P0DreamRouteBatch86RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0DreamRouteBatch86RuntimeEvidenceState.Passed,
                "summary",
                "Candidate frame draw audit passed for 1920x1080: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1365x768: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1280x720: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1024x768: 6/6 candidate textures drawn; fallback=0\n"
                + "Route state semantics visible: dreamEntry=1 branch=1 boss=1 compact=1\n"
                + "Boss gate scale visible: bossGate=1\n"
                + "Chinese route labels and rewards visible: yes\n"
                + "Route-choice click targets visible: yes\n",
                new[]
                {
                    P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[0],
                    P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[1],
                    P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[2],
                    P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[3]
                },
                P0DreamRouteBatch86RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Unity-rendered text replacement: pass", semanticsReview);
            StringAssert.Contains("Route-card click targets: pass", clickReview);
            StringAssert.Contains(P0DreamRouteBatch86UnityPreflight.CandidateFrameDrawToken, report);
            StringAssert.Contains(P0DreamRouteBatch86UnityPreflight.RouteChoiceClickTargetsVisibleToken, report);
            StringAssert.Contains("Remaining manual gates", report);
            StringAssert.DoesNotContain("Formal install approved: yes", report);
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0DreamRouteBatch86UnityPreflightReport report = P0DreamRouteBatch86UnityPreflight.Evaluate(
                P0DreamRouteBatch86CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_dream_route_route_boss_gate_frame_360x260_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0DreamRouteBatch86UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
        {
            return EvaluateWithSyntheticEvidence(readText, AlwaysUsefulVisualContent);
        }

        private static P0DreamRouteBatch86UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            return EvaluateWithSyntheticEvidence(readText, readVisualContent, configureExistingPaths: null);
        }

        private static P0DreamRouteBatch86UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent,
            Action<HashSet<string>> configureExistingPaths)
        {
            IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates =
                P0DreamRouteBatch86CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = BuildSyntheticExistingPaths(candidates);
            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);
            if (configureExistingPaths != null)
            {
                configureExistingPaths(existingPaths);
            }

            return P0DreamRouteBatch86UnityPreflight.Evaluate(
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
                return "# Batch 86 Scene Binding Console Clean Report\n\n"
                    + "Runtime scene/presenter binding: yes\n"
                    + "Console clean: yes\n"
                    + "Unity log reviewed: yes\n"
                    + "Batch 86 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 86 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-26\n";
            }

            if (path.EndsWith("text_replacement_node_path_semantics_review.md", StringComparison.Ordinal))
            {
                return "# Batch 86 Text Replacement Node Path Semantics Review\n\n"
                    + "Review result: pass\n"
                    + "Unity-rendered text replacement: pass\n"
                    + "Node/path semantics: pass\n"
                    + "Boss gate scale: pass\n";
            }

            if (path.EndsWith("route_choice_click_target_density_review.md", StringComparison.Ordinal))
            {
                return "# Batch 86 Route Choice Click Target Density Review\n\n"
                    + "Review result: pass\n"
                    + "Route-card click targets: pass\n"
                    + "1024x768 route-card density: pass\n"
                    + "Path connector occlusion: pass\n";
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 86 dream-route runtime evidence passed: synthetic clean log\n";
            }

            if (path == P0DreamRouteBatch86UnityPreflight.RuntimeEvidenceReportPath)
            {
                return BuildRuntimeEvidenceReportText(includeAllScreenshots: true);
            }

            return "# Batch 86 Review\n\nReview result: pass\n";
        }

        private static HashSet<string> BuildSyntheticExistingPaths(IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates)
        {
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            existingPaths.Add(P0DreamRouteBatch86UnityPreflight.RuntimeEvidenceReportPath);
            return existingPaths;
        }

        private static string BuildRuntimeEvidenceReportText(bool includeAllScreenshots)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 86 Dream Route Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: passed");
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.CompleteScreenshotEvidenceToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.DreamRouteSurfaceCapturedToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.CandidateFrameDrawToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.NoCandidateTextureFallbackToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.RouteStateSemanticsVisibleToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.BossGateScaleVisibleToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.ChineseRouteTextVisibleToken);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.RouteChoiceClickTargetsVisibleToken);
            builder.AppendLine("- Formal install allowed: no");
            builder.AppendLine();
            builder.AppendLine("## Screenshots");

            int screenshotCount = 0;
            for (int i = 0; i < P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                string path = P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[i];
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

            foreach (P0DreamRouteBatch86CandidateAsset candidate in P0DreamRouteBatch86CandidateCatalog.CreateCandidates())
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

