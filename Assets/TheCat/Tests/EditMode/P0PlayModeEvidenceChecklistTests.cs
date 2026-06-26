using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0PlayModeEvidenceChecklistTests
    {
        [Test]
        public void Evaluate_AllPassedCompletesEvidenceChecklist()
        {
            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(CreateAllPassedSnapshot());

            Assert.IsTrue(report.IsUsable, report.BuildDetailedSummary());
            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.IsFalse(report.HasBlockingFailures);
            Assert.IsFalse(report.HasPendingWarnings);
            Assert.AreEqual(0, report.FailureCount);
            Assert.AreEqual(0, report.WarningCount);
            Assert.AreEqual(8, report.PassedCount);
            Assert.AreEqual(8, report.Checks.Count);
            StringAssert.Contains("Runtime Visual Screenshot Plan: Passed", report.BuildDetailedSummary());
            StringAssert.Contains("Runtime Visual Contact Sheet: Passed", report.BuildDetailedSummary());
            StringAssert.Contains("Screenshot File Evidence: Passed", report.BuildDetailedSummary());
            StringAssert.Contains("Unity Runtime Validation Plan: Passed", report.BuildDetailedSummary());
            StringAssert.Contains("Screenshot Smoke: Passed", report.BuildDetailedSummary());
            StringAssert.Contains("Route Flow Smoke: Passed", report.BuildDetailedSummary());
            StringAssert.Contains("Defeat Flow Smoke: Passed", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_PendingSmokeWarnsWithoutBlocking()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Idle,
                0,
                "Screenshot smoke has not run.",
                P0PlayModeRouteFlowSmokeState.Running,
                "Route flow smoke running.",
                P0PlayModeDefeatFlowSmokeState.Idle,
                "Defeat flow smoke has not run.");

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsTrue(report.IsUsable, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsComplete, report.BuildDetailedSummary());
            Assert.IsTrue(report.HasPendingWarnings);
            Assert.AreEqual(0, report.FailureCount);
            Assert.AreEqual(3, report.WarningCount);
            Assert.AreEqual(5, report.PassedCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId, out P0PlayModeEvidenceCheck routeFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Warning, routeFlow.State);
            StringAssert.Contains("pending or not complete", routeFlow.Message);
        }

        [Test]
        public void Evaluate_FailedDefeatSmokeBlocksEvidenceChecklist()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed with RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, Shop bed-patch next-battle sleep verified, and cat-room return verified.",
                P0PlayModeDefeatFlowSmokeState.Failed,
                "Forced defeat result surface missing.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: true);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsFalse(report.IsComplete);
            Assert.IsTrue(report.HasBlockingFailures);
            Assert.AreEqual(1, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.DefeatFlowSmokeCheckId, out P0PlayModeEvidenceCheck defeatFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, defeatFlow.State);
            StringAssert.Contains("Smoke failed", defeatFlow.Message);
        }

        [Test]
        public void Evaluate_RouteSmokeWithoutCatRoomReturnEvidenceFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed.",
                P0PlayModeDefeatFlowSmokeState.Passed,
                "Defeat flow smoke passed.",
                routeFlowCatRoomReturnVerified: false,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: true,
                defeatFlowFailedCatRoomReturnVerified: true);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId, out P0PlayModeEvidenceCheck routeFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, routeFlow.State);
            StringAssert.Contains("without final cat-room return evidence", routeFlow.Message);
        }

        [Test]
        public void Evaluate_RouteSmokeWithoutRestNestRecoveryEvidenceFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed with cat-room return verified.",
                P0PlayModeDefeatFlowSmokeState.Passed,
                "Defeat flow smoke passed with failed cat-room return verified.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: false,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: true,
                defeatFlowFailedCatRoomReturnVerified: true);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId, out P0PlayModeEvidenceCheck routeFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, routeFlow.State);
            StringAssert.Contains("without RestNest next-battle recovery evidence", routeFlow.Message);
        }

        [Test]
        public void Evaluate_BadScreenshotPlanFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = CreateAllPassedSnapshot(false);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(1, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.ScreenshotCapturePlanCheckId, out P0PlayModeEvidenceCheck plan));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, plan.State);
            StringAssert.Contains("missing or incomplete", plan.Message);
        }

        [Test]
        public void Evaluate_BadRuntimeVisualScreenshotPlanFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = CreateAllPassedSnapshot(true, false);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(1, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RuntimeVisualScreenshotPlanCheckId, out P0PlayModeEvidenceCheck plan));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, plan.State);
            StringAssert.Contains("Runtime visual screenshot evidence is missing", plan.Message);
        }

        [Test]
        public void Evaluate_MissingRuntimeVisualContactSheetFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = CreateAllPassedSnapshot(true, true, false);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(1, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RuntimeVisualContactSheetCheckId, out P0PlayModeEvidenceCheck contactSheet));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, contactSheet.State);
            StringAssert.Contains("Runtime visual contact sheet is missing", contactSheet.Message);
        }

        [Test]
        public void Evaluate_MissingScreenshotFileEvidenceFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = CreateAllPassedSnapshot(true, true, true, false);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(1, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.ScreenshotFileEvidenceCheckId, out P0PlayModeEvidenceCheck screenshotFiles));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, screenshotFiles.State);
            StringAssert.Contains("Screenshot file evidence is incomplete", screenshotFiles.Message);
        }

        [Test]
        public void Evaluate_MissingUnityRuntimeValidationPlanFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = CreateAllPassedSnapshot(
                true,
                true,
                true,
                true,
                false);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsFalse(report.IsComplete);
            Assert.AreEqual(1, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.UnityRuntimeValidationPlanCheckId, out P0PlayModeEvidenceCheck runtimePlan));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, runtimePlan.State);
            StringAssert.Contains("Unity runtime validation plan is incomplete", runtimePlan.Message);
        }

        private static P0PlayModeEvidenceSnapshot CreateAllPassedSnapshot(
            bool hasScreenshotPlan = true,
            bool hasRuntimeVisualScreenshotPlan = true,
            bool hasRuntimeVisualContactSheet = true,
            bool hasScreenshotFileEvidence = true,
            bool hasUnityRuntimeValidationPlan = true)
        {
            return new P0PlayModeEvidenceSnapshot(
                hasScreenshotPlan,
                hasRuntimeVisualScreenshotPlan,
                hasRuntimeVisualContactSheet,
                hasScreenshotFileEvidence,
                hasScreenshotFileEvidence
                    ? "Screenshot file evidence complete."
                    : "3/11 expected capture(s), 8 missing, 1 unexpected PNG file.",
                hasUnityRuntimeValidationPlan,
                hasUnityRuntimeValidationPlan
                    ? "Unity runtime validation plan ready."
                    : "Runtime validation plan missing active-cat or Chinese UI scale checks.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed with RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, Shop bed-patch next-battle sleep verified, and cat-room return verified.",
                P0PlayModeDefeatFlowSmokeState.Passed,
                "Defeat flow smoke passed with failed cat-room return verified.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: true,
                defeatFlowFailedCatRoomReturnVerified: true);
        }

        [Test]
        public void Evaluate_RouteSmokeWithoutDreamEventModifierEvidenceFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed with RestNest next-battle recovery verified and cat-room return verified.",
                P0PlayModeDefeatFlowSmokeState.Passed,
                "Defeat flow smoke passed with failed cat-room return verified.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: false,
                routeFlowShopBedPatchNextBattleSleepVerified: true,
                defeatFlowFailedCatRoomReturnVerified: true);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId, out P0PlayModeEvidenceCheck routeFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, routeFlow.State);
            StringAssert.Contains("without DreamEvent catnip next-battle modifier evidence", routeFlow.Message);
        }

        [Test]
        public void Evaluate_RouteSmokeWithoutShopBedPatchSleepEvidenceFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed with RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, and cat-room return verified.",
                P0PlayModeDefeatFlowSmokeState.Passed,
                "Defeat flow smoke passed with failed cat-room return verified.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: false,
                defeatFlowFailedCatRoomReturnVerified: true);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId, out P0PlayModeEvidenceCheck routeFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, routeFlow.State);
            StringAssert.Contains("without Shop bed-patch next-battle sleep evidence", routeFlow.Message);
        }

        [Test]
        public void Evaluate_DefeatSmokeWithoutCatRoomReturnEvidenceFails()
        {
            P0PlayModeEvidenceSnapshot snapshot = new P0PlayModeEvidenceSnapshot(
                true,
                true,
                true,
                true,
                "Screenshot file evidence complete.",
                true,
                "Unity runtime validation plan ready.",
                P0PlayModeScreenshotSmokeState.Passed,
                P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Screenshot smoke passed.",
                P0PlayModeRouteFlowSmokeState.Passed,
                "Route flow smoke passed with RestNest next-battle recovery verified, DreamEvent catnip next-battle modifier verified, Shop bed-patch next-battle sleep verified, and cat-room return verified.",
                P0PlayModeDefeatFlowSmokeState.Passed,
                "Defeat flow smoke passed.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: true,
                defeatFlowFailedCatRoomReturnVerified: false);

            P0PlayModeEvidenceReport report = P0PlayModeEvidenceChecklist.Evaluate(snapshot);

            Assert.IsFalse(report.IsUsable);
            Assert.IsTrue(report.TryGetCheck(P0PlayModeEvidenceChecklist.DefeatFlowSmokeCheckId, out P0PlayModeEvidenceCheck defeatFlow));
            Assert.AreEqual(P0PlayModeEvidenceState.Failed, defeatFlow.State);
            StringAssert.Contains("without failed cat-room return evidence", defeatFlow.Message);
        }
    }
}
