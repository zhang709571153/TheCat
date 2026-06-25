using System;
using System.IO;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0BedroomInteractableBatch54UnityPreflightTests
    {
        [Test]
        public void EvaluateCurrentPreflight_ReadyForUnityReviewButBlocksFormalInstall()
        {
            P0BedroomInteractableBatch54UnityPreflightReport report = P0BedroomInteractableBatch54UnityPreflight.EvaluateCurrentPreflight();

            Assert.IsTrue(report.IsReadyForUnityPreflight, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.UnityEditorValidationReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.QueueEntryReadyForUnityReview, report.BuildDetailedSummary());
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedManifestRowCount, report.ManifestRowCount);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedSubjectCount, report.SubjectCount);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedAlphaCandidateCount, report.AlphaCandidateCount);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedManifestRowCount, report.CandidateOutsideAssetsCount);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedManifestRowCount, report.CandidateNoMetaCount);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedManifestRowCount, report.CandidateRecommendationLockedCount);
            Assert.AreEqual(3, report.CurrentUnitySpriteCount);
            Assert.AreEqual(3, report.CurrentUnitySpriteMetaCount);
            Assert.AreEqual(2, report.SourceLockCount);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedRuntimeBindingCount, report.RuntimeBindingCount);
            Assert.AreEqual(0, report.UnityEvidenceCount);
            Assert.AreEqual(6, report.UnityEvidenceRequiredCount);
            Assert.AreEqual(6, report.BlockingItems.Count);
            Assert.AreEqual(P0BedroomInteractableBatch54UnityPreflight.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
        }

        [Test]
        public void BuildMarkdown_ListsSelectedCandidatesAndBlockedUnityEvidence()
        {
            P0BedroomInteractableBatch54UnityPreflightReport report = P0BedroomInteractableBatch54UnityPreflight.EvaluateCurrentPreflight();

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Batch 54 Bedroom Interactable Unity Preflight", markdown);
            StringAssert.Contains("Ready for Unity review: yes", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("bed", markdown);
            StringAssert.Contains("litter_box", markdown);
            StringAssert.Contains("feeder", markdown);
            StringAssert.Contains("candidate_review_only_do_not_import", markdown);
            StringAssert.Contains("runtime_scale_screenshot_sheet.md", markdown);
            StringAssert.Contains("Assets/TheCat/Art/Scenes/BedroomDream", markdown);
            StringAssert.Contains("design/development/asset_candidates", markdown);
        }

        [Test]
        public void Evaluate_CandidateInsideAssetsFailsPreflight()
        {
            string manifest = ReadCurrentManifest().Replace(
                "design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/thecat_prop_bed_batch54_interactable_alpha_1024_candidate_v001.png",
                "Assets/TheCat/Art/Scenes/BedroomDream/unsafe_bedroom_candidate.png",
                StringComparison.Ordinal);

            P0BedroomInteractableBatch54UnityPreflightReport report = P0BedroomInteractableBatch54UnityPreflight.Evaluate(
                P0BedroomInteractableBatch54UnityPreflight.DefaultManifestPath,
                _ => manifest,
                path => !path.EndsWith(".meta", StringComparison.Ordinal) && File.Exists(ResolveProjectPath(path)),
                unityEditorValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("review-only handling", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingCurrentUnitySpriteFailsPreflight()
        {
            P0BedroomInteractableBatch54UnityPreflightReport report = P0BedroomInteractableBatch54UnityPreflight.Evaluate(
                P0BedroomInteractableBatch54UnityPreflight.DefaultManifestPath,
                _ => ReadCurrentManifest(),
                path => !path.EndsWith("thecat_prop_feeder_sprite_256_v001.png", StringComparison.Ordinal)
                    && File.Exists(ResolveProjectPath(path)),
                unityEditorValidationReady: false);

            Assert.IsFalse(report.IsReadyForUnityPreflight);
            StringAssert.Contains("Current BedroomDream prop sprites or meta files are missing", report.BuildDetailedSummary());
        }

        private static string ReadCurrentManifest()
        {
            return File.ReadAllText(ResolveProjectPath(P0BedroomInteractableBatch54UnityPreflight.DefaultManifestPath));
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
