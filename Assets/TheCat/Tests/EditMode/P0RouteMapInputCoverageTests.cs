using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0RouteMapInputCoverageTests
    {
        [Test]
        public void EvaluatePrototypeRouteMap_CoversP0RouteInputActions()
        {
            P0RouteMapInputCoverageReport report = P0RouteMapInputCoverage.EvaluatePrototypeRouteMap();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0RouteMapInputCoverage.ExpectedCoveredActionCount, report.CoveredActions.Count);
            StringAssert.Contains("Enter applies the default", report.BuildDetailedSummary());
            StringAssert.Contains("Number keys select", report.BuildDetailedSummary());
        }

        [Test]
        public void Report_WithFailureIsIncomplete()
        {
            P0RouteMapInputCoverageReport report = P0RouteMapInputCoverage.EvaluatePrototypeRouteMap();

            report.AddIssue(P0RouteMapInputCoverageSeverity.Failure, "Injected route map input failure.");

            Assert.IsFalse(report.IsComplete);
            StringAssert.Contains("failure", report.BuildSummary());
        }
    }
}
