using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0ChineseUiScaleValidationPlanTests
    {
        [Test]
        public void EvaluateCurrentPlan_ReadiesChineseUiScaleValidation()
        {
            P0ChineseUiScaleValidationReport report = P0ChineseUiScaleValidationPlan.EvaluateCurrentPlan();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0ChineseUiScaleValidationPlan.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(P0ChineseUiScaleValidationPlan.ExpectedSurfaceCount, report.Surfaces.Count);
            Assert.AreEqual(P0ChineseUiScaleValidationPlan.ExpectedResolutionCount, report.Resolutions.Count);
            Assert.AreEqual(P0ChineseUiScaleValidationPlan.ExpectedAcceptanceCheckCount, report.AcceptanceChecks.Count);
        }

        [Test]
        public void BuildCaptureMatrix_ListsEverySurfaceAtEveryResolution()
        {
            IReadOnlyList<P0ChineseUiScaleSurface> surfaces = P0ChineseUiScaleValidationPlan.CreateP0Surfaces();
            IReadOnlyList<P0ChineseUiScaleResolution> resolutions = P0ChineseUiScaleValidationPlan.CreateP0ResolutionMatrix();

            string matrix = P0ChineseUiScaleValidationPlan.BuildCaptureMatrix(surfaces, resolutions);

            StringAssert.Contains("surface,resolution,evidence", matrix);
            StringAssert.Contains(P0ChineseUiScaleValidationPlan.MainMenuSurfaceId + ",1024x768", matrix);
            StringAssert.Contains(P0ChineseUiScaleValidationPlan.BattleHudSurfaceId + ",1920x1080", matrix);
            StringAssert.Contains("\u4e3b\u83dc\u5355", matrix);
        }

        [Test]
        public void Evaluate_MissingChineseUiCoverageFails()
        {
            P0ChineseUiScaleValidationReport report = P0ChineseUiScaleValidationPlan.Evaluate(
                P0ChineseUiScaleValidationPlan.CreateP0Surfaces(),
                P0ChineseUiScaleValidationPlan.CreateP0ResolutionMatrix(),
                P0ChineseUiScaleValidationPlan.CreateP0AcceptanceChecks(),
                null);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("Chinese UI scale validation requires", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingSurfaceFails()
        {
            List<P0ChineseUiScaleSurface> surfaces = new List<P0ChineseUiScaleSurface>(
                P0ChineseUiScaleValidationPlan.CreateP0Surfaces());
            surfaces.RemoveAt(surfaces.Count - 1);

            P0ChineseUiScaleValidationReport report = P0ChineseUiScaleValidationPlan.Evaluate(
                surfaces,
                P0ChineseUiScaleValidationPlan.CreateP0ResolutionMatrix(),
                P0ChineseUiScaleValidationPlan.CreateP0AcceptanceChecks(),
                P0ChineseUiCoverage.EvaluatePrototypeUi());

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("surface matrix", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingAcceptanceGateFails()
        {
            List<P0ChineseUiScaleAcceptanceCheck> checks = new List<P0ChineseUiScaleAcceptanceCheck>(
                P0ChineseUiScaleValidationPlan.CreateP0AcceptanceChecks());
            checks.RemoveAt(checks.Count - 1);

            P0ChineseUiScaleValidationReport report = P0ChineseUiScaleValidationPlan.Evaluate(
                P0ChineseUiScaleValidationPlan.CreateP0Surfaces(),
                P0ChineseUiScaleValidationPlan.CreateP0ResolutionMatrix(),
                checks,
                P0ChineseUiCoverage.EvaluatePrototypeUi());

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("acceptance checklist", report.BuildDetailedSummary());
        }
    }
}
