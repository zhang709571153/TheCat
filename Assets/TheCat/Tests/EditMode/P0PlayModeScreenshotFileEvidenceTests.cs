using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0PlayModeScreenshotFileEvidenceTests
    {
        [Test]
        public void Evaluate_AllExpectedFilesCompleteEvidence()
        {
            IReadOnlyList<string> expected = P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames;
            P0PlayModeScreenshotFileEvidenceReport report = P0PlayModeScreenshotFileEvidence.Evaluate(
                P0PlayModeScreenshotFileEvidence.DefaultScreenshotDirectory,
                expected,
                path => ContainsPath(expected, path),
                _ => expected);

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0PlayModeScreenshotSmoke.ExpectedCaptureCount, report.ExpectedFileCount);
            Assert.AreEqual(P0PlayModeScreenshotSmoke.ExpectedCaptureCount, report.ExistingExpectedFileCount);
            Assert.AreEqual(0, report.MissingExpectedFileCount);
            Assert.AreEqual(0, report.UnexpectedPngFileCount);
        }

        [Test]
        public void Evaluate_MissingRuntimeVisualCapturesAreReported()
        {
            string[] existing =
            {
                "01-main-menu.png",
                "02-cat-room.png",
                "03-route-map-layer1.png",
                "04-settlement.png"
            };

            P0PlayModeScreenshotFileEvidenceReport report = P0PlayModeScreenshotFileEvidence.Evaluate(
                P0PlayModeScreenshotFileEvidence.DefaultScreenshotDirectory,
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                path => ContainsPath(existing, path),
                _ => existing);

            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(3, report.ExistingExpectedFileCount);
            Assert.AreEqual(8, report.MissingExpectedFileCount);
            Assert.AreEqual(1, report.UnexpectedPngFileCount);
            StringAssert.Contains("05-active-cat-saiban.png", report.BuildDetailedSummary());
            StringAssert.Contains("08-battle-world-visuals.png", report.BuildDetailedSummary());
            StringAssert.Contains("04-settlement.png", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_UnusableExpectedCaptureFailsEvidence()
        {
            IReadOnlyList<string> expected = P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames;
            P0PlayModeScreenshotFileEvidenceReport report = P0PlayModeScreenshotFileEvidence.Evaluate(
                P0PlayModeScreenshotFileEvidence.DefaultScreenshotDirectory,
                expected,
                path => ContainsPath(expected, path),
                path => !path.EndsWith("09-call-tyrant-warning-vfx.png", StringComparison.Ordinal),
                _ => expected);

            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(P0PlayModeScreenshotSmoke.ExpectedCaptureCount, report.ExistingExpectedFileCount);
            Assert.AreEqual(0, report.MissingExpectedFileCount);
            Assert.AreEqual(1, report.UnusableExpectedFileCount);
            StringAssert.Contains("09-call-tyrant-warning-vfx.png", report.BuildDetailedSummary());
        }

        private static bool ContainsPath(IReadOnlyList<string> fileNames, string path)
        {
            if (fileNames == null)
            {
                return false;
            }

            for (int i = 0; i < fileNames.Count; i++)
            {
                if (path.EndsWith(fileNames[i], StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
