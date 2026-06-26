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

        [Test]
        public void Classify_CleanProjectPassLog_IsStrictAndProjectOwnedClean()
        {
            P0UnityConsoleLogClassifierReport report = P0UnityConsoleLogClassifier.Classify(
                "[TheCat] P0 batchmode acceptance passed 7 gate(s).\n"
                + "[TheCat] P0 Play Mode acceptance batchmode passed.\n"
                + "Defeat flow smoke passed with failed cat-room return verified.\n");

            Assert.IsTrue(report.StrictClean, report.BuildDetailedSummary());
            Assert.IsTrue(report.ProjectOwnedClean, report.BuildDetailedSummary());
            Assert.AreEqual(0, report.ProjectFailureTokenCount);
            Assert.AreEqual(0, report.UnknownBlockingTokenCount);
            Assert.AreEqual(0, report.KnownEnvironmentNoiseCount);
        }

        [Test]
        public void Classify_KnownEnvironmentNoise_IsProjectOwnedCleanButNotStrictClean()
        {
            P0UnityConsoleLogClassifierReport report = P0UnityConsoleLogClassifier.Classify(
                "[TheCat] P0 batchmode acceptance passed 7 gate(s).\n"
                + "[Licensing::Client] Error: noisy entitlement line\n"
                + "Unity.AI.Tracing.ConsoleSink:LogToConsole (string,string,System.Exception)\n"
                + "##utp:{\"type\":\"MemoryLeaks\",\"phase\":\"Immediate\"}\n");

            Assert.IsFalse(report.StrictClean);
            Assert.IsTrue(report.ProjectOwnedClean, report.BuildDetailedSummary());
            Assert.AreEqual(0, report.ProjectFailureTokenCount);
            Assert.AreEqual(0, report.UnknownBlockingTokenCount);
            Assert.AreEqual(3, report.KnownEnvironmentNoiseCount);
            StringAssert.Contains("KnownEnvironmentNoise", report.BuildDetailedSummary());
        }

        [Test]
        public void Classify_UnknownError_BlocksProjectOwnedClean()
        {
            P0UnityConsoleLogClassifierReport report = P0UnityConsoleLogClassifier.Classify(
                "SomePlugin Error: missing scene binding\n");

            Assert.IsFalse(report.StrictClean);
            Assert.IsFalse(report.ProjectOwnedClean);
            Assert.AreEqual(0, report.ProjectFailureTokenCount);
            Assert.AreEqual(1, report.UnknownBlockingTokenCount);
            Assert.AreEqual(0, report.KnownEnvironmentNoiseCount);
            StringAssert.Contains("UnknownBlocking", report.BuildDetailedSummary());
        }

        [Test]
        public void Classify_ProjectFailureToken_BlocksProjectOwnedClean()
        {
            P0UnityConsoleLogClassifierReport report = P0UnityConsoleLogClassifier.Classify(
                "[TheCat] P0 batchmode acceptance failed 1 of 7 gate(s).\n");

            Assert.IsFalse(report.StrictClean);
            Assert.IsFalse(report.ProjectOwnedClean);
            Assert.AreEqual(1, report.ProjectFailureTokenCount);
            Assert.AreEqual(0, report.UnknownBlockingTokenCount);
            StringAssert.Contains("ProjectFailure", report.BuildDetailedSummary());
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
