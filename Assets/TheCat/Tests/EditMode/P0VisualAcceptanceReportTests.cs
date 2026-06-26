using System;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0VisualAcceptanceReportTests
    {
        [Test]
        public void EvaluateCurrent_DemoReadyButFinalVisualAcceptanceBlockedByFormalInstallReview()
        {
            P0VisualAcceptanceReport report = P0VisualAcceptance.EvaluateCurrent();

            Assert.IsTrue(report.IsArchitectureReadyForSystematicAssetProduction, report.BuildMarkdown());
            Assert.IsTrue(report.IsP0DemoVisualEvidenceReady, report.BuildMarkdown());
            Assert.IsFalse(report.IsFinalP0VisualAcceptanceReady, report.BuildMarkdown());
            Assert.IsTrue(report.IsStarterCatFormalImportBlocked, report.BuildMarkdown());
            Assert.IsNotNull(report.ChineseUiScaleEvidence);
            Assert.IsTrue(report.ChineseUiScaleEvidence.IsReady, report.ChineseUiScaleEvidence.BuildDetailedSummary());
            Assert.IsNotNull(report.ScreenshotFileEvidence);
            Assert.IsTrue(report.ScreenshotFileEvidence.IsComplete, report.ScreenshotFileEvidence.BuildDetailedSummary());
            Assert.IsTrue(report.HasCompletePlayModeEvidence, report.BuildMarkdown());
            Assert.IsNotNull(report.PlayModeReportFileEvidence);
            Assert.IsTrue(report.PlayModeReportFileEvidence.IsComplete, report.PlayModeReportFileEvidence.BuildDetailedSummary());

            string markdown = report.BuildMarkdown();
            StringAssert.Contains("Architecture ready for systematic asset production: yes", markdown);
            StringAssert.Contains("Current P0 demo visual evidence ready: yes", markdown);
            StringAssert.Contains("Final P0 visual acceptance ready: no", markdown);
            StringAssert.Contains("P0 demo visual evidence is ready", markdown);
            StringAssert.Contains("Play Mode report file evidence", markdown);
            StringAssert.Contains("Chinese UI Scale Evidence Packet", markdown);
            StringAssert.Contains(P0ChineseUiScaleEvidencePacket.BatchSlug, markdown);
            StringAssert.Contains(P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName, markdown);
            StringAssert.Contains(P0PlayModeScreenshotSmoke.BattleWorldVisualsCaptureFileName, markdown);
            StringAssert.Contains("p0_runtime_visual_contact_sheet_2026-06-14.png", markdown);
            StringAssert.Contains("colored three-view turnarounds", markdown);
            StringAssert.Contains("cat.combat.saiban", markdown);
        }

        [Test]
        public void Evaluate_WithApprovedEvidenceMarksFinalVisualAcceptanceReady()
        {
            P0PlayModeScreenshotFileEvidenceReport screenshotEvidence = CreateCompleteScreenshotEvidence();
            P0VisualAcceptanceReport report = P0VisualAcceptance.Evaluate(
                P0PlayableReadiness.EvaluatePrototypeBuild(),
                P0AssetProductionReadiness.EvaluateP0OfflineReadiness(),
                P0AssetReviewPacket.EvaluateP0Packet(),
                P0RuntimeVisualBindingCoverage.Evaluate(P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true),
                P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket(),
                screenshotEvidence,
                CreateAllPassedPlayModeEvidence(),
                CreateApprovedStarterCatFormalImport(screenshotEvidence));

            Assert.IsTrue(report.IsArchitectureReadyForSystematicAssetProduction, report.BuildMarkdown());
            Assert.IsTrue(report.IsP0DemoVisualEvidenceReady, report.BuildMarkdown());
            Assert.IsTrue(report.IsFinalP0VisualAcceptanceReady, report.BuildMarkdown());
            Assert.IsFalse(report.IsStarterCatFormalImportBlocked, report.BuildMarkdown());
            StringAssert.Contains("Final P0 visual acceptance ready: yes", report.BuildMarkdown());
            StringAssert.Contains("Starter cat formal import allowed: yes", report.BuildMarkdown());
        }

        private static P0PlayModeEvidenceReport CreateAllPassedPlayModeEvidence()
        {
            return P0PlayModeEvidenceChecklist.Evaluate(new P0PlayModeEvidenceSnapshot(
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
                "Defeat flow smoke passed with failed cat-room return verified.",
                routeFlowCatRoomReturnVerified: true,
                routeFlowRestNestNextBattleRecoveryVerified: true,
                routeFlowDreamEventCatnipNextBattleModifierVerified: true,
                routeFlowShopBedPatchNextBattleSleepVerified: true,
                defeatFlowFailedCatRoomReturnVerified: true));
        }

        private static P0PlayModeScreenshotFileEvidenceReport CreateCompleteScreenshotEvidence()
        {
            return P0PlayModeScreenshotFileEvidence.Evaluate(
                "screens",
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                _ => true,
                _ => P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames);
        }

        private static P0StarterCatFormalImportReadinessReport CreateApprovedStarterCatFormalImport(
            P0PlayModeScreenshotFileEvidenceReport screenshotEvidence)
        {
            return P0StarterCatFormalImportReadiness.Evaluate(
                P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05(),
                P0StarterCatAssetProductionSpec.EvaluateP0Spec(),
                screenshotEvidence,
                P0StarterCatFormalImportReadiness.ReviewNotePaths,
                ApprovedNoteReader);
        }

        private static bool ApprovedNoteReader(string path, out string text, out string error)
        {
            text = "Recommendation: approved for Unity import." + Environment.NewLine
                + "Result: active-cat Play Mode screenshot approved." + Environment.NewLine
                + "Review basis: colored turnaround contact sheet comparison passed.";
            error = string.Empty;
            return true;
        }
    }
}
