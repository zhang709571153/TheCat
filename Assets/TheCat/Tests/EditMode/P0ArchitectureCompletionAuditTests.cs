using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0ArchitectureCompletionAuditTests
    {
        [Test]
        public void EvaluateCurrentProject_ReportsDemoReadyWhileFinalRuntimeRemainsPending()
        {
            P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.EvaluateCurrentProject();

            Assert.IsFalse(report.HasBlockingFailures, report.BuildDetailedSummary());
            Assert.IsTrue(report.HasPlayableArchitecture, report.BuildDetailedSummary());
            Assert.IsTrue(report.IsReadyForSystematicAssetProduction, report.BuildDetailedSummary());
            Assert.IsTrue(report.IsP0DemoReleaseReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsFinalP0PlayableComplete, report.BuildDetailedSummary());
            Assert.IsTrue(report.RequiresUnityRuntimeValidation, report.BuildDetailedSummary());
            Assert.Greater(report.PendingUnityValidationGateCount, 0, report.BuildDetailedSummary());
            Assert.IsNotNull(report.ChineseUiScaleValidation);
            Assert.IsTrue(report.ChineseUiScaleValidation.IsReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.ChineseUiScaleValidationGateId, out P0ArchitectureCompletionAuditGate uiScaleGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.Passed, uiScaleGate.State);
            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.PlayModeEvidenceGateId, out P0ArchitectureCompletionAuditGate playModeGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.Passed, playModeGate.State);
            StringAssert.Contains("report file evidence complete", playModeGate.Message);
            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.FinalP0RuntimeGateId, out P0ArchitectureCompletionAuditGate runtimeGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.PendingUnityValidation, runtimeGate.State);
            Assert.AreEqual(10, report.TotalRouteLayers);
            Assert.AreEqual(10, report.CompletedRouteLayers);
            Assert.AreEqual(5, report.BattleCount);
        }

        [Test]
        public void EvaluateCurrentProject_DemoReadyDoesNotRequireStarterCatFormalImportApproval()
        {
            P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.EvaluateCurrentProject();

            Assert.IsTrue(report.IsP0DemoReleaseReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.StarterCatFormalImportGateId, out P0ArchitectureCompletionAuditGate starterCatGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.PendingUnityValidation, starterCatGate.State);
            Assert.IsNotNull(report.StarterCatFormalImport);
            Assert.IsTrue(report.StarterCatFormalImport.IsGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.StarterCatFormalImport.IsImportAllowed, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsFinalP0PlayableComplete, report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentProject_KeepsCodexCandidateProductionSeparateFromUnityInstall()
        {
            P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.EvaluateCurrentProject();

            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.QueueCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount, report.CodexRunnableAssetQueueCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.CandidatePacksPendingUnityReviewCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, report.UnityBlockedAssetQueueCount);
            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.CodexUnityBoundaryGateId, out P0ArchitectureCompletionAuditGate boundaryGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.Passed, boundaryGate.State);
            StringAssert.Contains("Unity validation", boundaryGate.Message);
        }

        [Test]
        public void EvaluateCurrentProject_KeepsStarterCatFormalImportPendingUnityValidation()
        {
            P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.EvaluateCurrentProject();

            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.StarterCatFormalImportGateId, out P0ArchitectureCompletionAuditGate starterCatGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.PendingUnityValidation, starterCatGate.State);
            Assert.IsNotNull(report.StarterCatFormalImport);
            Assert.IsTrue(report.StarterCatFormalImport.IsGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.StarterCatFormalImport.IsImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.StarterCatFormalImport.ActiveCatScreenshotCount);
        }

        [Test]
        public void BuildMarkdown_StatesAssetProductionBoundaryAndCatTurnaroundRule()
        {
            P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.EvaluateCurrentProject();

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Ready for systematic Codex-side asset production: yes", markdown);
            StringAssert.Contains("Current P0 demo release/readiness: yes", markdown);
            StringAssert.Contains("Final P0 Unity runtime complete: no", markdown);
            StringAssert.Contains("Codex owns candidate production", markdown);
            StringAssert.Contains("Unity owns formal install", markdown);
            StringAssert.Contains("Play Mode report file evidence complete", markdown);
            StringAssert.Contains("Chinese UI scale validation", markdown);
            StringAssert.Contains("locked colored three-view turnarounds", markdown);
        }

        [Test]
        public void Evaluate_MissingChineseUiScaleValidationBlocksArchitectureReadiness()
        {
            P0GoldenPathReport goldenPath = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport goldenPathAcceptance = P0GoldenPathAcceptance.Evaluate(goldenPath);
            P0VisualAcceptanceReport visualAcceptance = P0VisualAcceptance.EvaluateCurrent();

            P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.Evaluate(
                goldenPath,
                goldenPathAcceptance,
                P0PlayableReadiness.EvaluatePrototypeBuild(),
                P0CodeSmokeSuite.EvaluatePrototypeBuild(),
                null,
                visualAcceptance,
                visualAcceptance.AssetProductionReadiness,
                P0AssetProductionQueueCoverage.EvaluateP0Queue(),
                visualAcceptance.ScreenshotFileEvidence,
                visualAcceptance.PlayModeEvidence,
                visualAcceptance.StarterCatFormalImport);

            Assert.IsTrue(report.HasBlockingFailures, report.BuildDetailedSummary());
            Assert.IsFalse(report.HasPlayableArchitecture, report.BuildDetailedSummary());
            Assert.IsFalse(report.IsReadyForSystematicAssetProduction, report.BuildDetailedSummary());
            Assert.IsTrue(report.TryGetGate(P0ArchitectureCompletionAudit.ChineseUiScaleValidationGateId, out P0ArchitectureCompletionAuditGate uiScaleGate));
            Assert.AreEqual(P0ArchitectureCompletionAuditGateState.Failed, uiScaleGate.State);
            StringAssert.Contains("missing", uiScaleGate.Message);
        }
    }
}
