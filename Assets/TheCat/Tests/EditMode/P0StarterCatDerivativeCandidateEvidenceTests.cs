using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StarterCatDerivativeCandidateEvidenceTests
    {
        [Test]
        public void EvaluateBatch05_CurrentCandidateFilesAreReviewReady()
        {
            P0StarterCatDerivativeCandidateEvidenceReport report = P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05();

            Assert.IsTrue(report.IsReviewReady, report.BuildSummary());
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedEvidenceFileCount, report.ExpectedFileCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedEvidenceFileCount, report.ExistingFileCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedCandidatePngCount, report.CandidatePngCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNoteCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewSheetCount, report.ReviewSheetCount);
            Assert.IsTrue(report.HasCandidateOutsideAssets);
            Assert.IsTrue(report.HasTurnaroundConformanceReviewNotes);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNoteConformanceSpecMentionCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNoteFrontAnchorSectionCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNoteSideAnchorSectionCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNoteBackAnchorSectionCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNotePaletteAnchorSectionCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNotePropCostumeAnchorSectionCount);
            Assert.AreEqual(P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount, report.ReviewNoteProhibitedDriftSectionCount);
        }

        [Test]
        public void Evaluate_MissingCandidateBlocksReviewReady()
        {
            IReadOnlyList<string> expected = P0StarterCatDerivativeCandidateEvidence.CreateBatch05ExpectedFiles();
            string missing = expected[2];

            P0StarterCatDerivativeCandidateEvidenceReport report = P0StarterCatDerivativeCandidateEvidence.Evaluate(
                expected,
                path => !string.Equals(path, missing, StringComparison.Ordinal));

            Assert.IsFalse(report.IsReviewReady);
            Assert.AreEqual(1, report.MissingFileCount);
            CollectionAssert.Contains(report.MissingFiles, missing);
        }

        [Test]
        public void Evaluate_CandidatePathInsideAssetsBlocksReviewReady()
        {
            List<string> expected = new List<string>(P0StarterCatDerivativeCandidateEvidence.CreateBatch05ExpectedFiles());
            expected[2] = "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_hud_avatar_256_candidate_v001.png";

            P0StarterCatDerivativeCandidateEvidenceReport report = P0StarterCatDerivativeCandidateEvidence.Evaluate(
                expected,
                _ => true);

            Assert.IsFalse(report.IsReviewReady);
            Assert.IsFalse(report.HasCandidateOutsideAssets);
        }

        [Test]
        public void Evaluate_ReviewNotesWithoutTurnaroundConformanceSectionsBlockReviewReady()
        {
            IReadOnlyList<string> expected = P0StarterCatDerivativeCandidateEvidence.CreateBatch05ExpectedFiles();

            P0StarterCatDerivativeCandidateEvidenceReport report = P0StarterCatDerivativeCandidateEvidence.Evaluate(
                expected,
                _ => true,
                ReviewNoteWithoutConformanceSections);

            Assert.IsFalse(report.IsReviewReady);
            Assert.IsFalse(report.HasTurnaroundConformanceReviewNotes);
            Assert.AreEqual(0, report.ReviewNoteConformanceSpecMentionCount);
            Assert.AreEqual(0, report.ReviewNoteFrontAnchorSectionCount);
            Assert.AreEqual(0, report.ReviewNoteSideAnchorSectionCount);
            Assert.AreEqual(0, report.ReviewNoteBackAnchorSectionCount);
            StringAssert.Contains("conformance notes ready: no", report.BuildSummary());
        }

        private static bool ReviewNoteWithoutConformanceSections(string path, out string text, out string error)
        {
            text = "Recommendation: candidate review only; do not import into Unity yet.\n"
                + "Reason: " + P0StarterCatFormalImportReadiness.FormalImportBlockPhrase + ".\n"
                + "Import gate: compare against the colored turnaround contact sheet.";
            error = string.Empty;
            return true;
        }
    }
}
