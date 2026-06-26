using System;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StarterCatFormalImportReadinessTests
    {
        [Test]
        public void EvaluateCurrentGate_CurrentBatchIsExplicitlyBlockedUntilActiveCatScreenshotsAreReviewed()
        {
            P0StarterCatFormalImportReadinessReport report = P0StarterCatFormalImportReadiness.EvaluateCurrentGate();

            Assert.IsTrue(report.IsGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportState.Blocked, report.State);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.StarterCatCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.ReviewNoteCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.ExplicitBlockNoteCount);
            Assert.AreEqual(0, report.ExplicitApprovalNoteCount);
            Assert.LessOrEqual(report.ActiveCatScreenshotCount, P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("explicitly blocked", report.BuildSummary());
        }

        [Test]
        public void Evaluate_BlockedNotesWithActiveCatScreenshotsStayBlockedUntilApproval()
        {
            P0StarterCatFormalImportReadinessReport report = EvaluateWith(
                ScreenshotEvidenceWithActiveCats(true),
                BlockedNoteReader);

            Assert.IsTrue(report.IsGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportState.Blocked, report.State);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.ActiveCatScreenshotCount);
            StringAssert.Contains("explicitly blocked", report.BuildSummary());
            StringAssert.Contains("until explicit approval notes replace the block notes", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_LegacyBlockedNotesNoLongerCreateValidGate()
        {
            P0StarterCatFormalImportReadinessReport report = EvaluateWith(
                ScreenshotEvidenceWithActiveCats(true),
                LegacyBlockedNoteReader);

            Assert.IsFalse(report.IsGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportState.Invalid, report.State);
            Assert.AreEqual(0, report.ExplicitBlockNoteCount);
            StringAssert.Contains("review notes must be consistently blocked or approved", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ApprovedNotesWithoutActiveCatScreenshotsFailsGate()
        {
            P0StarterCatFormalImportReadinessReport report = EvaluateWith(
                ScreenshotEvidenceWithActiveCats(false),
                ApprovedNoteReader);

            Assert.IsFalse(report.IsGateValid);
            Assert.IsFalse(report.IsImportAllowed);
            Assert.AreEqual(P0StarterCatFormalImportState.Invalid, report.State);
            StringAssert.Contains("Formal starter cat import approval requires all three active-cat Play Mode screenshots", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ApprovedNotesWithActiveCatScreenshotsAllowsImport()
        {
            P0StarterCatFormalImportReadinessReport report = EvaluateWith(
                ScreenshotEvidenceWithActiveCats(true),
                ApprovedNoteReader);

            Assert.IsTrue(report.IsGateValid, report.BuildDetailedSummary());
            Assert.IsTrue(report.IsImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportState.Approved, report.State);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.ActiveCatScreenshotCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.ExplicitApprovalNoteCount);
        }

        [Test]
        public void Evaluate_MixedReviewDecisionsFailsGate()
        {
            P0StarterCatFormalImportReadinessReport report = EvaluateWith(
                ScreenshotEvidenceWithActiveCats(false),
                MixedDecisionReader);

            Assert.IsFalse(report.IsGateValid);
            Assert.AreEqual(P0StarterCatFormalImportState.Invalid, report.State);
            StringAssert.Contains("review notes must be consistently blocked or approved", report.BuildDetailedSummary());
        }

        private static P0StarterCatFormalImportReadinessReport EvaluateWith(
            P0PlayModeScreenshotFileEvidenceReport screenshotEvidence,
            P0StarterCatReviewTextReader reader)
        {
            return P0StarterCatFormalImportReadiness.Evaluate(
                P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05(),
                P0StarterCatAssetProductionSpec.EvaluateP0Spec(),
                screenshotEvidence,
                P0StarterCatFormalImportReadiness.ReviewNotePaths,
                reader);
        }

        private static P0PlayModeScreenshotFileEvidenceReport ScreenshotEvidenceWithActiveCats(bool hasActiveCatScreenshots)
        {
            return P0PlayModeScreenshotFileEvidence.Evaluate(
                "screens",
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                path => hasActiveCatScreenshots && IsActiveCatScreenshotPath(path),
                _ => Array.Empty<string>());
        }

        private static bool IsActiveCatScreenshotPath(string path)
        {
            for (int i = 0; i < P0StarterCatFormalImportReadiness.ActiveCatScreenshotFileNames.Length; i++)
            {
                if ((path ?? string.Empty).EndsWith(P0StarterCatFormalImportReadiness.ActiveCatScreenshotFileNames[i], StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool BlockedNoteReader(string path, out string text, out string error)
        {
            text = "Recommendation: candidate review only; do not import into Unity yet.\n"
                + "Reason: " + P0StarterCatFormalImportReadiness.FormalImportBlockPhrase + ".\n"
                + "Import gate: compare against the colored turnaround contact sheet.";
            error = string.Empty;
            return true;
        }

        private static bool LegacyBlockedNoteReader(string path, out string text, out string error)
        {
            text = "Recommendation: candidate review only; do not import into Unity yet.\n"
                + "Reason: active-cat Play Mode screenshots are still missing.\n"
                + "Import gate: compare against the colored turnaround contact sheet.";
            error = string.Empty;
            return true;
        }

        private static bool ApprovedNoteReader(string path, out string text, out string error)
        {
            text = "Recommendation: approved for Unity import.\n"
                + "Result: active-cat Play Mode screenshot approved.\n"
                + "Review basis: colored turnaround contact sheet comparison passed.";
            error = string.Empty;
            return true;
        }

        private static bool MixedDecisionReader(string path, out string text, out string error)
        {
            error = string.Empty;
            if ((path ?? string.Empty).IndexOf("saiban", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                text = "Recommendation: candidate review only; do not import into Unity yet.\n"
                    + "Reason: " + P0StarterCatFormalImportReadiness.FormalImportBlockPhrase + ".\n"
                    + "Import gate: compare against the colored turnaround contact sheet.";
                return true;
            }

            text = "Recommendation: approved for Unity import.\n"
                + "Result: active-cat Play Mode screenshot approved.\n"
                + "Review basis: colored turnaround contact sheet comparison passed.";
            return true;
        }
    }
}
