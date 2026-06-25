using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0UnityRuntimeValidationPlanTests
    {
        [Test]
        public void EvaluateCurrentPlan_IsReadyForUnityRuntimeAcceptance()
        {
            P0UnityRuntimeValidationPlanReport report = P0UnityRuntimeValidationPlan.EvaluateCurrentPlan();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0UnityRuntimeValidationPlan.ExpectedStepCount, report.StepCount);
            Assert.AreEqual(P0UnityRuntimeValidationPlan.ExpectedScreenshotStepCount, report.ScreenshotStepCount);
            Assert.AreEqual(P0PlayModeScreenshotSmoke.ExpectedCaptureCount, report.ExpectedScreenshotMatchCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.ActiveCatScreenshotStepCount);
            Assert.AreEqual(P0UnityRuntimeValidationPlan.ExpectedSmokeStepCount, report.SmokeStepCount);
            Assert.AreEqual(P0UnityRuntimeValidationPlan.ExpectedConsoleStepCount, report.ConsoleStepCount);
            Assert.AreEqual(P0UnityRuntimeValidationPlan.ExpectedUnityBindingStepCount, report.UnityBindingStepCount);
            Assert.AreEqual(P0UnityRuntimeValidationPlan.ExpectedReviewStepCount, report.ReviewStepCount);
            Assert.AreEqual(P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount, report.ChineseUiScaleCaptureRows);
            Assert.AreEqual(0, report.DuplicateStepIdCount);
            StringAssert.Contains("colored-turnaround", report.BuildMarkdown());
            StringAssert.Contains("20 Batch 75 surface/resolution rows", report.BuildMarkdown());
            StringAssert.Contains("TheCat/P0/Start Play Mode Acceptance Smoke", report.BuildMarkdown());
            StringAssert.Contains("design/development/screenshots/p0-playmode-smoke", report.BuildMarkdown());
            StringAssert.Contains("Final P0 visual acceptance still requires Unity Play Mode screenshots", report.BuildMarkdown());
        }

        [Test]
        public void Evaluate_MissingActiveCatScreenshotStepFails()
        {
            List<P0UnityRuntimeValidationStep> steps = new List<P0UnityRuntimeValidationStep>(
                P0UnityRuntimeValidationPlan.CreateP0Steps());
            RemoveStep(steps, "screenshot.active_cat_suzune");

            P0UnityRuntimeValidationPlanReport report = P0UnityRuntimeValidationPlan.Evaluate(
                steps,
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                P0StarterCatFormalImportReadiness.ActiveCatScreenshotFileNames,
                P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket());

            Assert.IsFalse(report.IsReady);
            Assert.Less(report.ActiveCatScreenshotStepCount, P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            StringAssert.Contains("missing active-cat turnaround screenshot checks", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_DuplicateStepIdFails()
        {
            List<P0UnityRuntimeValidationStep> steps = new List<P0UnityRuntimeValidationStep>(
                P0UnityRuntimeValidationPlan.CreateP0Steps());
            steps.Add(steps[0]);

            P0UnityRuntimeValidationPlanReport report = P0UnityRuntimeValidationPlan.Evaluate(
                steps,
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                P0StarterCatFormalImportReadiness.ActiveCatScreenshotFileNames,
                P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket());

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.DuplicateStepIdCount);
            StringAssert.Contains("duplicate step ids", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingChineseUiScaleEvidenceFails()
        {
            P0UnityRuntimeValidationPlanReport report = P0UnityRuntimeValidationPlan.Evaluate(
                P0UnityRuntimeValidationPlan.CreateP0Steps(),
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                P0StarterCatFormalImportReadiness.ActiveCatScreenshotFileNames,
                null);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.ChineseUiScaleEvidenceReady);
            StringAssert.Contains("Chinese UI scale matrix", report.BuildDetailedSummary());
        }

        private static void RemoveStep(List<P0UnityRuntimeValidationStep> steps, string stepId)
        {
            for (int i = steps.Count - 1; i >= 0; i--)
            {
                if (steps[i].StepId == stepId)
                {
                    steps.RemoveAt(i);
                }
            }
        }
    }
}
