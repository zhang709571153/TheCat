using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetProductionQueueCoverageTests
    {
        [Test]
        public void EvaluateP0Queue_CurrentQueueSeparatesCodexAndUnityWork()
        {
            P0AssetProductionQueueCoverageReport report = P0AssetProductionQueueCoverage.EvaluateP0Queue();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.QueueCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount, report.CodexRunnableCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.CandidatePackCompletePendingUnityReviewCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, report.UnityBlockedCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.ExistingPromptCount);
            Assert.AreEqual(P0AssetProductionQueueCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("candidate packs pending Unity review", report.BuildDetailedSummary());
            StringAssert.Contains("Unity validation", report.BuildDetailedSummary());
        }

        [Test]
        public void Catalog_PrioritizesStarterCatValidationBeforeFormalInstall()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();

            Assert.AreEqual(P0AssetProductionQueueCatalog.StarterCatActiveValidationQueueId, queue[0].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.BlockedByUnityValidation, queue[0].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId, queue[2].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[2].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.StarterSkillVfxCandidateQueueId, queue[3].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.BlockedByUnityValidation, queue[3].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.SkillHudFeedbackCandidateQueueId, queue[4].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.BlockedByUnityValidation, queue[4].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.CatRoomPreflightCandidateQueueId, queue[5].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[5].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.CharacterSelectPreflightCandidateQueueId, queue[6].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[6].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.BattleHudPreflightCandidateQueueId, queue[7].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[7].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.SkillSelectionPreflightCandidateQueueId, queue[8].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[8].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.RuntimeControlIconCandidateQueueId, queue[9].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[9].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.RuntimeControlPanelCandidateQueueId, queue[10].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[10].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.SecondaryEnemyWarningCandidateQueueId, queue[11].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[11].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.RouteMapReadabilityCandidateQueueId, queue[12].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[12].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.BedroomInteractionAffordanceCandidateQueueId, queue[13].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[13].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.LoadingStartPreflightCandidateQueueId, queue[14].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[14].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ResultSettlementPreflightCandidateQueueId, queue[15].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[15].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.SettingsPausePreflightCandidateQueueId, queue[16].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[16].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.DreamRoutePreflightCandidateQueueId, queue[17].QueueId);
            Assert.AreEqual(P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview, queue[17].State);
            Assert.AreEqual(P0AssetProductionQueueCatalog.FormalInstallDecisionQueueId, queue[queue.Count - 1].QueueId);
            Assert.AreEqual(P0AssetProductionQueuePhase.FormalUnityInstall, queue[queue.Count - 1].Phase);
            Assert.Greater(queue[queue.Count - 1].Priority, queue[0].Priority);
        }

        [Test]
        public void Evaluate_MissingPromptFailsQueueCoverage()
        {
            P0AssetProductionQueueCoverageReport report = P0AssetProductionQueueCoverage.Evaluate(
                P0AssetProductionQueueCatalog.CreateP0Queue(),
                path => !path.EndsWith("p0_asset_batch_52_starter_cat_active_validation.md", System.StringComparison.Ordinal));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("prompt is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_CandidateDirectoryInsideAssetsFailsQueueCoverage()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> queue = MutateFirstEntryCandidateDirectory("Assets/TheCat/Art/Characters/Sprites");

            P0AssetProductionQueueCoverageReport report = P0AssetProductionQueueCoverage.Evaluate(queue, _ => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("candidate directory must stay under design/development/asset_candidates", report.BuildDetailedSummary());
        }

        private static IReadOnlyList<P0AssetProductionQueueEntry> MutateFirstEntryCandidateDirectory(string candidateDirectory)
        {
            List<P0AssetProductionQueueEntry> result = new List<P0AssetProductionQueueEntry>();
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();
            for (int i = 0; i < queue.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queue[i];
                result.Add(i == 0
                    ? new P0AssetProductionQueueEntry(
                        entry.QueueId,
                        entry.Priority,
                        entry.DisplayName,
                        entry.SubjectGroup,
                        entry.Phase,
                        entry.State,
                        entry.ExecutionPromptPath,
                        candidateDirectory,
                        entry.UnityImportRoot,
                        entry.RelatedBatchSlugs,
                        entry.SourceLockIds,
                        entry.RequiredEvidence,
                        entry.ForbiddenWriteRoots,
                        entry.NextAction)
                    : entry);
            }

            return result;
        }
    }
}
