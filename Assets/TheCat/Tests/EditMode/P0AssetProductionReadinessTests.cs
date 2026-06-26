using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetProductionReadinessTests
    {
        [Test]
        public void EvaluateP0OfflineReadiness_PassesCurrentAssetProductionGate()
        {
            P0AssetProductionReadinessReport report = P0AssetProductionReadiness.EvaluateP0OfflineReadiness();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.ReviewAssetCount);
            Assert.AreEqual(P0VisualAssetCatalog.P0RuntimeVisualBindingCount, report.RuntimeBindingCount);
            Assert.AreEqual(P0VisualAssetCatalog.P0RuntimeVisualBindingCount, report.ResolvedRuntimeTextureCount);
            Assert.AreEqual(P0HardReferenceSourceLocks.ExpectedManifestLinkedAssetCount, report.SourceLockedEntryCount);
            Assert.AreEqual(3, report.StarterCatLockCount);
            Assert.AreEqual(3, report.StarterCatVisualChecklistCount);
            Assert.AreEqual(3, report.StarterCatTurnaroundConformanceSpecCount);
            Assert.AreEqual(27, report.StarterCatTurnaroundViewAnchorCount);
            Assert.AreEqual(3, report.StarterCatProductionSpecCount);
            Assert.IsTrue(report.StarterCatSourceLockPacketReady);
            Assert.IsTrue(report.StarterCatRuntimeCombatSpriteAuditReady, report.BuildDetailedSummary());
            Assert.AreEqual(3, report.StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount);
            Assert.IsTrue(report.StarterCatStrictCandidateReady, report.BuildDetailedSummary());
            Assert.AreEqual(3, report.StarterCatStrictCandidateCount);
            Assert.IsTrue(report.StarterCatContactSheetPresent);
            Assert.IsTrue(report.RuntimeVisualContactSheetPresent);
            Assert.IsTrue(report.StarterCatFormalImportGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.StarterCatFormalImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportState.Blocked, report.StarterCatFormalImportState);
            Assert.AreEqual(3, report.StarterCatFormalReviewNoteCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.StarterCatActiveScreenshotCount);
            Assert.IsTrue(report.AssetProductionQueueReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.AssetProductionQueueCount);
            Assert.AreEqual(0, report.AssetProductionQueueCodexRunnableCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.AssetProductionQueueCandidatePackCompletePendingUnityReviewCount);
            Assert.AreEqual(5, report.AssetProductionQueueUnityBlockedCount);
            Assert.AreEqual(P0AssetProductionReadiness.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("Starter cat production prompts pin real colored-turnaround source paths", report.BuildDetailedSummary());
            StringAssert.Contains("Asset production queue separates Codex candidate production", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingStarterCatContactSheetFailsProductionGate()
        {
            P0AssetProductionReadinessReport report = EvaluateWith(
                P0RuntimeVisualBindingCoverage.Evaluate(P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true),
                path => path != P0AssetProductionReadiness.StarterCatTurnaroundContactSheetPath && File.Exists(path));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("Starter cat turnaround review contact sheet is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingRuntimeVisualContactSheetFailsProductionGate()
        {
            P0AssetProductionReadinessReport report = EvaluateWith(
                P0RuntimeVisualBindingCoverage.Evaluate(P0VisualAssetCatalog.CreateP0RuntimeBindings(), _ => true),
                path => path != P0AssetProductionReadiness.RuntimeVisualContactSheetPath && File.Exists(path));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("Runtime visual contact sheet is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_StaleRuntimeVisualBindingCountFailsProductionGate()
        {
            List<P0VisualAssetBinding> bindings = new List<P0VisualAssetBinding>(P0VisualAssetCatalog.CreateP0RuntimeBindings());
            bindings.RemoveAt(bindings.Count - 1);
            P0RuntimeVisualBindingCoverageReport runtimeVisuals = P0RuntimeVisualBindingCoverage.Evaluate(bindings, _ => true);

            P0AssetProductionReadinessReport report = EvaluateWith(runtimeVisuals, File.Exists);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains(P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " P0 runtime visual bindings are incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentNextBatchGate_AllowsScopedOutsideAssetsWorkButBlocksStarterCatBodyImport()
        {
            P0AssetProductionNextBatchGateReport report = P0AssetProductionNextBatchGate.EvaluateCurrentGate();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.CanStartNewCodexCandidateBatch, report.BuildDetailedSummary());
            Assert.IsFalse(report.StarterCatBodyImportAllowed, report.BuildDetailedSummary());
            Assert.IsTrue(report.RequiresUnityReviewBeforeInstall, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionNextBatchGate.RecommendedLaneNewScopedCandidateOrSpecBatchOutsideAssets, report.RecommendedNextLane);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.QueueItemCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount, report.CodexRunnableCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.CandidatePacksPendingUnityReviewCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, report.UnityBlockedCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.PendingUnityReviewEntries.Count);
            Assert.AreEqual(P0AssetProductionNextBatchGate.ExpectedCoveredCheckCount, report.CoveredChecks.Count);

            string markdown = report.BuildMarkdown();
            StringAssert.Contains("P0 Asset Production Next Batch Gate", markdown);
            StringAssert.Contains("Next Codex candidate batch allowed: yes", markdown);
            StringAssert.Contains("Allowed lane: `" + P0AssetProductionNextBatchGate.RecommendedLaneNewScopedCandidateOrSpecBatchOutsideAssets + "`", markdown);
            StringAssert.Contains("Starter-cat body import allowed: no", markdown);
            StringAssert.Contains("Unity review required before install: yes", markdown);
            StringAssert.Contains("Starter Cat Active Screenshot Validation", markdown);
            StringAssert.Contains("Formal Install Decision Packet", markdown);
        }

        [Test]
        public void EvaluateCurrentFormalInstallGate_ReportsRuntimeEvidenceSixOfEightButBlocksFormalInstall()
        {
            P0FormalInstallGateReport report = P0FormalInstallGate.EvaluateCurrentGate();

            Assert.IsTrue(report.IsGateValid, report.BuildDetailedSummary());
            Assert.IsTrue(report.IsFormalInstallBlocked, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.QueueItemCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.CandidatePackCompletePendingUnityReviewCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, report.UnityBlockedCount);
            Assert.AreEqual(P0FormalInstallGate.ExpectedRuntimeEvidenceSixOfEightCount, report.RuntimeEvidenceSixOfEightCount);
            Assert.GreaterOrEqual(report.SceneOrConsoleGateCount, report.RuntimeEvidenceSixOfEightCount);
            Assert.GreaterOrEqual(report.HumanApprovalGateCount, report.RuntimeEvidenceSixOfEightCount);
            Assert.AreEqual(0, report.ReadyForFormalInstallCount);
            Assert.AreEqual(P0StarterCatFormalImportState.Blocked, report.StarterCatFormalImportState);
            Assert.IsFalse(report.StarterCatFormalImportAllowed);
            Assert.IsTrue(report.SharedConsoleClassifierContractReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0FormalInstallGate.ExpectedCoveredCheckCount, report.CoveredChecks.Count);

            string markdown = report.BuildMarkdown();
            StringAssert.Contains("P0 Formal Install Gate Matrix", markdown);
            StringAssert.Contains("Formal install allowed: no", markdown);
            StringAssert.Contains("Formal install blocked: yes", markdown);
            StringAssert.Contains("Battle HUD Preflight Candidate Pack", markdown);
            StringAssert.Contains("Cat Room Preflight Candidate Pack", markdown);
            StringAssert.Contains("Runtime evidence 6/8 items: " + P0FormalInstallGate.ExpectedRuntimeEvidenceSixOfEightCount, markdown);
            StringAssert.Contains("Shared Console classifier: active strict-clean contract", markdown);
            StringAssert.Contains("known environment noise is classified but still blocks formal clean-Console approval", markdown);
        }

        [Test]
        public void EvaluateFormalInstallGate_RuntimeEvidenceLaneWithoutHumanApprovalFailsMatrix()
        {
            List<P0AssetProductionQueueEntry> queue = new List<P0AssetProductionQueueEntry>(P0AssetProductionQueueCatalog.CreateP0Queue());
            int batch87Index = queue.FindIndex(entry => entry.QueueId == P0AssetProductionQueueCatalog.BattleHudPreflightCandidateQueueId);
            Assert.GreaterOrEqual(batch87Index, 0);

            P0AssetProductionQueueEntry batch87 = queue[batch87Index];
            queue[batch87Index] = new P0AssetProductionQueueEntry(
                batch87.QueueId,
                batch87.Priority,
                batch87.DisplayName,
                batch87.SubjectGroup,
                batch87.Phase,
                batch87.State,
                batch87.ExecutionPromptPath,
                batch87.CandidateDirectory,
                batch87.UnityImportRoot,
                batch87.RelatedBatchSlugs,
                batch87.SourceLockIds,
                RemoveHumanApprovalEvidence(batch87.RequiredEvidence),
                batch87.ForbiddenWriteRoots,
                "Batch 87 battle HUD is candidate-backed runtime evidence 6/8 with candidate imports under Assets/TheCat/Art/UI/BattleHUD; keep it review-only until clean Console checks and formal runtime binding decision approve a formal install.");

            P0FormalInstallGateReport report = P0FormalInstallGate.Evaluate(
                queue,
                P0StarterCatFormalImportReadiness.EvaluateCurrentGate());

            Assert.IsFalse(report.IsGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.FormalInstallAllowed);
            StringAssert.Contains("lacks a human-approval blocker", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateNextBatchGate_ApprovedStarterCatImportFailsCurrentPreReviewGate()
        {
            P0StarterCatFormalImportReadinessReport approvedImport = CreateApprovedStarterCatFormalImport();

            P0AssetProductionNextBatchGateReport report = P0AssetProductionNextBatchGate.Evaluate(
                P0AssetProductionReadiness.EvaluateP0OfflineReadiness(),
                P0AssetProductionQueueCoverage.EvaluateP0Queue(),
                P0AssetUnityValidationChecklist.EvaluateCurrentQueue(),
                P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates(),
                approvedImport);

            Assert.IsFalse(report.IsReady);
            Assert.IsTrue(report.StarterCatBodyImportAllowed);
            StringAssert.Contains("must remain blocked", report.BuildDetailedSummary());
        }

        private static P0AssetProductionReadinessReport EvaluateWith(
            P0RuntimeVisualBindingCoverageReport runtimeVisuals,
            System.Func<string, bool> fileExists)
        {
            return P0AssetProductionReadiness.Evaluate(
                P0AssetManifestCoverage.EvaluateP0Manifest(),
                P0AssetGenerationBatchCoverage.EvaluateP0Batches(),
                P0AssetImportReadiness.EvaluateP0Manifest(),
                P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest(),
                runtimeVisuals,
                P0AssetReviewPacket.Evaluate(
                    P0AssetManifestCatalog.CreateP0PlannedManifest(),
                    P0HardReferenceSourceLocks.CreateP0Locks(),
                    P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                    _ => true),
                P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks(),
                P0StarterCatVisualConsistencyChecklist.Evaluate(
                    P0StarterCatVisualConsistencyChecklist.CreateP0Checklist(),
                    P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                    fileExists),
                P0StarterCatTurnaroundConformanceSpec.Evaluate(
                    P0StarterCatTurnaroundConformanceSpec.CreateP0Spec(),
                    P0StarterCatVisualConsistencyChecklist.CreateP0Checklist(),
                    fileExists),
                P0StarterCatAssetProductionSpec.EvaluateP0Spec(),
                P0StarterCatProductionPromptReadiness.EvaluateCurrentPrompts(),
                P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket(),
                P0StarterCatRuntimeCombatSpriteAuditEvidence.EvaluateCurrentAudit(),
                P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates(),
                P0StarterCatFormalImportReadiness.EvaluateCurrentGate(),
                P0AssetProductionQueueCoverage.EvaluateP0Queue(),
                P0HardReferenceSourceLocks.EvaluateP0Locks(),
                fileExists);
        }

        private static P0StarterCatFormalImportReadinessReport CreateApprovedStarterCatFormalImport()
        {
            P0StarterCatFormalImportReadinessReport report = new P0StarterCatFormalImportReadinessReport();
            report.SetCounts(
                P0StarterCatFormalImportReadiness.ExpectedStarterCatCount,
                P0StarterCatFormalImportReadiness.ExpectedStarterCatCount,
                0,
                P0StarterCatFormalImportReadiness.ExpectedStarterCatCount,
                P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            for (int i = 0; i < P0StarterCatFormalImportReadiness.ExpectedCoveredCheckCount; i++)
            {
                report.AddCoveredCheck("approved-formal-import-test-check-" + i);
            }

            report.SetState(P0StarterCatFormalImportState.Approved);
            return report;
        }

        private static IReadOnlyList<string> RemoveHumanApprovalEvidence(IReadOnlyList<string> evidence)
        {
            List<string> filtered = new List<string>();
            for (int i = 0; i < evidence.Count; i++)
            {
                string value = evidence[i];
                if (value != null
                    && (value.IndexOf("human approval", System.StringComparison.OrdinalIgnoreCase) >= 0
                        || value.IndexOf("explicit human", System.StringComparison.OrdinalIgnoreCase) >= 0
                        || value.IndexOf("human review", System.StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    continue;
                }

                filtered.Add(value);
            }

            return filtered;
        }
    }
}
