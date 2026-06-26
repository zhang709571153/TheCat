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
    public sealed class P0SettingsPauseBatch85UnityPreflightTests
    {
        private const string SyntheticRuntimeEvidenceLogPath = "Logs/batch85_settings_pause_runtime_evidence_synthetic.log";

        [Test]
        public void EvaluateCurrentPreflight_CandidateFilesReadyButNeedsEditorImportValidation()
        {
            P0SettingsPauseBatch85UnityPreflightReport report =
                P0SettingsPauseBatch85UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsFalse(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.CandidatePreflightChecksReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorImportValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.CandidateCount);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidatePngCount);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.SourceCandidateNoMetaCount);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidatePngCount);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.ImportedCandidateMetaCount);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.DimensionMatchedCount);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, report.UnityPreflightBindingCount);
            Assert.AreEqual(0, report.FormalRuntimeBindingLeakCount);
            Assert.Less(report.UnityEvidenceCount, P0SettingsPauseBatch85UnityPreflight.ExpectedUnityEvidenceRequiredCount);
            Assert.AreEqual(P0SettingsPauseBatch85UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceRequiredCount);
            StringAssert.Contains("Unity editor import validation has not passed", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentPreflight_EditorImportValidationStillRequiresRuntimeEvidence()
        {
            P0SettingsPauseBatch85UnityPreflightReport report =
                P0SettingsPauseBatch85UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

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
            IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates =
                P0SettingsPauseBatch85CandidateCatalog.CreateCandidates();
            P0VisualAssetBinding[] preflightBindings =
                P0SettingsPauseBatch85CandidateCatalog.CreateUnityPreflightBindings();
            P0VisualAssetBinding[] runtimeBindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            HashSet<string> preflightBindingIds = new HashSet<string>(StringComparer.Ordinal);

            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, candidates.Count);
            Assert.AreEqual(P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount, preflightBindings.Length);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                Assert.AreEqual(P0AssetManifestStatus.Imported, entry.Status);
                StringAssert.StartsWith("Assets/TheCat/Art/UI/SettingsPause/", entry.UnityImportPath);
                StringAssert.StartsWith(P0SettingsPauseBatch85CandidateCatalog.SourceSpriteDirectory + "/", candidates[i].SourceCandidatePath);
                Assert.IsTrue(preflightBindingIds.Add(preflightBindings[i].BindingId), preflightBindings[i].BindingId);
                StringAssert.Contains(candidates[i].VariantId, preflightBindings[i].BindingId);
                AssertNoRuntimeBinding(runtimeBindings, entry.AssetId, entry.UnityImportPath);
            }
        }

        [Test]
        public void BuildMarkdown_ListsUnityImportsAndBlockedEvidence()
        {
            P0SettingsPauseBatch85UnityPreflightReport report =
                P0SettingsPauseBatch85UnityPreflight.EvaluateCurrentPreflight(unityEditorImportValidationReady: true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 85 Settings Pause Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity preflight: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Assets/TheCat/Art/UI/SettingsPause", markdown);
            StringAssert.Contains("key_hint_chip_frame", markdown);
            StringAssert.Contains("batch_85_settings_pause_unity_preflight", markdown);
            StringAssert.Contains("P0VisualAssetCatalog.P0RuntimeVisualBindingCount", markdown);
        }

        [Test]
        public void Evaluate_AllEvidenceWithSignedReviewsAllowsFormalInstall()
        {
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(BuildValidEvidenceText);

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsTrue(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0SettingsPauseBatch85UnityPreflight.ExpectedUnityEvidenceRequiredCount, report.UnityEvidenceCount);
            Assert.AreEqual(0, report.BlockingItems.Count, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ScreenshotEvidenceWithoutRuntimeReportDoesNotAllowFormalInstall()
        {
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(
                BuildValidEvidenceText,
                AlwaysUsefulVisualContent,
                paths => paths.Remove(P0SettingsPauseBatch85UnityPreflight.RuntimeEvidenceReportPath));

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            StringAssert.Contains("runtime evidence report", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_BlankScreenshotEvidenceDoesNotAllowFormalInstall()
        {
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(
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
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path == SyntheticRuntimeEvidenceLogPath)
                {
                    return "[TheCat] Batch 85 settings/pause runtime evidence passed: synthetic\nLicensing Client Error: dirty log\n";
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
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 85 Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 85 Human Approval\n\nFormal install approved: yes\n";
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
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("text_replacement_key_hint_readability_review.md", StringComparison.Ordinal)
                    || path.EndsWith("settings_controls_click_target_review.md", StringComparison.Ordinal))
                {
                    return "# Batch 85 Review\n\nReview result: pass\n";
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
            P0SettingsPauseBatch85UnityPreflightReport report = EvaluateWithSyntheticEvidence(path =>
            {
                if (path.EndsWith("scene_binding_console_clean_report.md", StringComparison.Ordinal))
                {
                    return "# Batch 85 Scene Binding Console Clean Report\n\nConsole clean: yes\n";
                }

                if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
                {
                    return "# Batch 85 Human Approval\n\nReviewer: pending manual review\n";
                }

                return BuildValidEvidenceText(path);
            });

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(
                P0SettingsPauseBatch85UnityPreflight.ExpectedUnityEvidenceRequiredCount - 2,
                report.UnityEvidenceCount,
                report.BuildDetailedSummary());
            StringAssert.Contains("runtime screenshots/reviews passed", report.BuildSummary());
            StringAssert.Contains("scene/Console and human approval gates", report.BuildSummary());
        }

        [Test]
        public void RuntimeEvidencePlan_GeneratesScreenshotsAndReviewsWithoutManualGates()
        {
            Assert.IsTrue(P0SettingsPauseBatch85RuntimeEvidence.HasExpectedRuntimeEvidencePlan());
            Assert.AreEqual(P0SettingsPauseBatch85RuntimeEvidence.ExpectedScreenshotCount, P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets.Length);
            Assert.AreEqual(P0SettingsPauseBatch85RuntimeEvidence.ExpectedAutomaticReviewCount, P0SettingsPauseBatch85RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length);
            Assert.AreEqual(P0SettingsPauseBatch85UnityPreflight.RuntimeEvidenceReportPath, P0SettingsPauseBatch85RuntimeEvidence.RuntimeReportPath);
            Assert.IsTrue(P0SettingsPauseBatch85RuntimeEvidence.DoesNotAutoGenerateManualGateEvidence());
        }

        [Test]
        public void RuntimeEvidenceResolutionOverride_CanBeConfiguredAndClearedForEditorRunner()
        {
            P0SettingsPauseBatch85ScreenshotTarget target = P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets[0];
            P0SettingsPauseBatch85RuntimeEvidence.SetBeforeCaptureResolutionOverride(t => "override " + t.ResolutionLabel);

            Assert.AreEqual("override " + target.ResolutionLabel, P0SettingsPauseBatch85RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));

            P0SettingsPauseBatch85RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            Assert.AreEqual("none", P0SettingsPauseBatch85RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target));
        }

        [Test]
        public void RuntimeEvidenceReviewMarkdown_HasPreflightTokensButKeepsManualGates()
        {
            string textReview = P0SettingsPauseBatch85RuntimeEvidence.BuildTextReplacementKeyHintReadabilityReviewMarkdown(
                "readability summary",
                P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets);
            string controlsReview = P0SettingsPauseBatch85RuntimeEvidence.BuildSettingsControlsClickTargetReviewMarkdown(
                "click summary",
                P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets);
            string report = P0SettingsPauseBatch85RuntimeEvidence.BuildRuntimeReportMarkdown(
                P0SettingsPauseBatch85RuntimeEvidenceState.Passed,
                "summary",
                "Candidate frame draw audit passed for 1920x1080: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1365x768: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1280x720: 6/6 candidate textures drawn; fallback=0\n"
                + "Candidate frame draw audit passed for 1024x768: 6/6 candidate textures drawn; fallback=0\n"
                + "Settings states visible: main=1 audio=1 compact=1\n"
                + "Pause overlay state visible: pause=1 restart=1\n"
                + "Key-hint readability visible: tabs=1 hints=1 compact=1\n"
                + "Settings/pause click targets visible: tabs=1 controls=1 pause=1 minClickTarget=44\n",
                new[]
                {
                    P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[0],
                    P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[1],
                    P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[2],
                    P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[3]
                },
                P0SettingsPauseBatch85RuntimeEvidence.AutomaticallyGeneratedReviewPaths);

            StringAssert.Contains("Unity-rendered text replacement: pass", textReview);
            StringAssert.Contains("Key-hint semantics: pass", textReview);
            StringAssert.Contains("Slider/switch/checkbox controls: pass", controlsReview);
            StringAssert.Contains(P0SettingsPauseBatch85UnityPreflight.CandidateFrameDrawToken, report);
            StringAssert.Contains(P0SettingsPauseBatch85UnityPreflight.SettingsPauseClickTargetsVisibleToken, report);
            StringAssert.Contains("Remaining manual gates", report);
            StringAssert.DoesNotContain("Formal install approved: yes", report);
        }

        [Test]
        public void Evaluate_MissingImportedCandidateFailsPreflight()
        {
            P0SettingsPauseBatch85UnityPreflightReport report = P0SettingsPauseBatch85UnityPreflight.Evaluate(
                P0SettingsPauseBatch85CandidateCatalog.CreateCandidates(),
                path => !path.EndsWith("thecat_ui_settings_pause_settings_section_divider_640x24_candidate_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                ReadExpectedDimensions,
                unityEditorImportValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Unity preflight imports are incomplete", report.BuildDetailedSummary());
        }

        private static P0SettingsPauseBatch85UnityPreflightReport EvaluateWithSyntheticEvidence(Func<string, string> readText)
        {
            return EvaluateWithSyntheticEvidence(readText, AlwaysUsefulVisualContent);
        }

        private static P0SettingsPauseBatch85UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent)
        {
            return EvaluateWithSyntheticEvidence(readText, readVisualContent, configureExistingPaths: null);
        }

        private static P0SettingsPauseBatch85UnityPreflightReport EvaluateWithSyntheticEvidence(
            Func<string, string> readText,
            P0AssetPngVisualContentReader readVisualContent,
            Action<HashSet<string>> configureExistingPaths)
        {
            IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates =
                P0SettingsPauseBatch85CandidateCatalog.CreateCandidates();
            HashSet<string> existingPaths = BuildSyntheticExistingPaths(candidates);
            existingPaths.Add(SyntheticRuntimeEvidenceLogPath);
            if (configureExistingPaths != null)
            {
                configureExistingPaths(existingPaths);
            }

            return P0SettingsPauseBatch85UnityPreflight.Evaluate(
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
                return "# Batch 85 Scene Binding Console Clean Report\n\n"
                    + "Runtime scene/presenter binding: yes\n"
                    + "Console clean: yes\n"
                    + "Unity log reviewed: yes\n"
                    + "Batch 85 runtime evidence log: " + SyntheticRuntimeEvidenceLogPath + "\n";
            }

            if (path.EndsWith("human_review_approval.md", StringComparison.Ordinal))
            {
                return "# Batch 85 Human Approval\n\nFormal install approved: yes\nReviewer: L1 human gate\nApproval date: 2026-06-26\n";
            }

            if (path.EndsWith("text_replacement_key_hint_readability_review.md", StringComparison.Ordinal))
            {
                return "# Batch 85 Text Replacement Key Hint Readability Review\n\n"
                    + "Review result: pass\n"
                    + "Unity-rendered text replacement: pass\n"
                    + "Key-hint semantics: pass\n"
                    + "1024x768 density: pass\n";
            }

            if (path.EndsWith("settings_controls_click_target_review.md", StringComparison.Ordinal))
            {
                return "# Batch 85 Settings Controls Click Target Review\n\n"
                    + "Review result: pass\n"
                    + "Slider/switch/checkbox controls: pass\n"
                    + "Tab and button click targets: pass\n"
                    + "Pause/resume/restart actions: pass\n";
            }

            if (path == SyntheticRuntimeEvidenceLogPath)
            {
                return "[TheCat] Batch 85 settings/pause runtime evidence passed: synthetic clean log\n";
            }

            if (path == P0SettingsPauseBatch85UnityPreflight.RuntimeEvidenceReportPath)
            {
                return BuildRuntimeEvidenceReportText();
            }

            return "# Batch 85 Review\n\nReview result: pass\n";
        }

        private static HashSet<string> BuildSyntheticExistingPaths(IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates)
        {
            HashSet<string> existingPaths = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < candidates.Count; i++)
            {
                P0AssetManifestEntry entry = candidates[i].ManifestEntry;
                existingPaths.Add(candidates[i].SourceCandidatePath);
                existingPaths.Add(entry.UnityImportPath);
                existingPaths.Add(entry.UnityImportPath + ".meta");
            }

            for (int i = 0; i < P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                existingPaths.Add(P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[i]);
            }

            existingPaths.Add(P0SettingsPauseBatch85UnityPreflight.RuntimeEvidenceReportPath);
            return existingPaths;
        }

        private static string BuildRuntimeEvidenceReportText()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 85 Settings Pause Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: passed");
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.CompleteScreenshotEvidenceToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.SettingsPauseSurfaceCapturedToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.CandidateFrameDrawToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.NoCandidateTextureFallbackToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.SettingsMainAudioStatesVisibleToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.PauseOverlayStateVisibleToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.KeyHintReadabilityVisibleToken);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.SettingsPauseClickTargetsVisibleToken);
            builder.AppendLine("- Formal install allowed: no");
            builder.AppendLine();
            builder.AppendLine("## Screenshots");

            for (int i = 0; i < P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths.Length; i++)
            {
                string path = P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[i];
                if (path.EndsWith(".png", StringComparison.Ordinal))
                {
                    builder.AppendLine("- `" + path + "`");
                }
            }

            return builder.ToString();
        }

        private static bool AlwaysUsefulVisualContent(string path, out string summary, out string error)
        {
            summary = "synthetic useful visual content";
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

            foreach (P0SettingsPauseBatch85CandidateAsset candidate in P0SettingsPauseBatch85CandidateCatalog.CreateCandidates())
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

