using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0ChineseUiCoverageTests
    {
        [Test]
        public void EvaluatePrototypeUi_CompletesChineseUiAndResponsiveLayoutGate()
        {
            P0ChineseUiCoverageReport report = P0ChineseUiCoverage.EvaluatePrototypeUi();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0ChineseUiCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("Chinese UI coverage complete", report.BuildSummary());
            StringAssert.Contains("Main menu surface", report.BuildDetailedSummary());
            StringAssert.Contains("Responsive IMGUI helpers", report.BuildDetailedSummary());
        }

        [Test]
        public void Report_WithManualFailureIsIncomplete()
        {
            P0ChineseUiCoverageReport report = new P0ChineseUiCoverageReport();
            report.AddIssue(P0ChineseUiCoverageSeverity.Failure, "Injected legacy UI token.");

            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(1, report.FailureCount);
            StringAssert.Contains("Injected legacy UI token", report.BuildDetailedSummary());
        }
    }
}
