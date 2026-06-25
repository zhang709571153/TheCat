using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetSystematicProductionPlanTests
    {
        [Test]
        public void EvaluateCurrentPlan_RecommendsNonCatReviewBeforeMoreImageGeneration()
        {
            P0AssetSystematicProductionPlanReport report = P0AssetSystematicProductionPlan.EvaluateCurrentPlan();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.CanContinueSystematicProduction, report.BuildDetailedSummary());
            Assert.IsTrue(report.StarterCatBodyLaneLocked, report.BuildDetailedSummary());
            Assert.IsTrue(report.RequiresUnityReviewBeforeInstall, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.QueueItemCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.NonCatCandidateReviewCount);
            Assert.GreaterOrEqual(report.ProtectedCatBodyEntryCount, 2);
            Assert.AreEqual(P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId, report.RecommendedFirstQueueId);
            Assert.AreEqual(P0AssetSystematicProductionPlan.RecommendedWorkModeReviewExistingNonCatCandidatePacks, report.RecommendedWorkMode);
            Assert.IsTrue(report.FirstRecommendedLanePreflightReady, report.BuildDetailedSummary());
            Assert.IsFalse(report.FirstRecommendedLaneFormalInstallAllowed, report.BuildDetailedSummary());
            Assert.Greater(report.FirstRecommendedLaneBlockingItemCount, 0);
            Assert.AreEqual(P0AssetSystematicProductionPlan.ExpectedCoveredCheckCount, report.CoveredChecks.Count);

            for (int i = 0; i < report.RecommendedReviewEntries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = report.RecommendedReviewEntries[i];
                Assert.AreNotEqual("starter_cats", entry.SubjectGroup);
                StringAssert.DoesNotContain("Characters/Sprites", entry.UnityImportRoot);
                StringAssert.Contains("design/development/asset_candidates", entry.CandidateDirectory);
            }
        }

        [Test]
        public void BuildMarkdown_ListsReviewOrderAndCatBodyLocks()
        {
            P0AssetSystematicProductionPlanReport report = P0AssetSystematicProductionPlan.EvaluateCurrentPlan();

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("P0 Systematic Asset Production Plan", markdown);
            StringAssert.Contains(P0AssetSystematicProductionPlan.EditorMenuPath, markdown);
            StringAssert.Contains(P0AssetSystematicProductionPlan.ReportOutputPath, markdown);
            StringAssert.Contains("ReviewExistingNonCatCandidatePacksBeforeNewImageGeneration", markdown);
            StringAssert.Contains("Starter-cat body lane locked: yes", markdown);
            StringAssert.Contains("First recommended lane preflight ready: yes", markdown);
            StringAssert.Contains("First recommended lane formal install allowed: no", markdown);
            StringAssert.Contains("do not generate, crop, recolor, import, or runtime-bind starter-cat body art", markdown);
            StringAssert.Contains("Bedroom Interactable Candidate Pack", markdown);
            StringAssert.Contains("Cat Room Preflight Candidate Pack", markdown);
            StringAssert.Contains("Character Select Preflight Candidate Pack", markdown);
            StringAssert.Contains("Battle HUD Preflight Candidate Pack", markdown);
            StringAssert.Contains("Skill Selection Preflight Candidate Pack", markdown);
            StringAssert.Contains("Runtime Control Icon Candidate Pack", markdown);
            StringAssert.Contains("Route Map Readability Candidate Pack", markdown);
            StringAssert.Contains("Loading Start Preflight Candidate Pack", markdown);
            StringAssert.Contains("Result Settlement Preflight Candidate Pack", markdown);
            StringAssert.Contains("Settings Pause Preflight Candidate Pack", markdown);
            StringAssert.Contains("Dream Route Preflight Candidate Pack", markdown);
            StringAssert.Contains("Assets/TheCat/Art/Characters/Sprites", markdown);
            StringAssert.Contains("colored three-view source-lock paths", markdown);
        }

        [Test]
        public void Evaluate_StarterCatCandidateReviewFailsSystematicPlan()
        {
            P0AssetProductionQueueEntry[] queue = CreateQueueArray();
            int targetIndex = FindQueueIndex(queue, P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId);
            P0AssetProductionQueueEntry entry = queue[targetIndex];
            queue[targetIndex] = new P0AssetProductionQueueEntry(
                entry.QueueId,
                entry.Priority,
                entry.DisplayName,
                "starter_cats",
                entry.Phase,
                entry.State,
                entry.ExecutionPromptPath,
                entry.CandidateDirectory,
                "Assets/TheCat/Art/Characters/Sprites",
                entry.RelatedBatchSlugs,
                new[] { "starter_cat_source_locks" },
                entry.RequiredEvidence,
                entry.ForbiddenWriteRoots,
                entry.NextAction);

            P0AssetSystematicProductionPlanReport report = P0AssetSystematicProductionPlan.Evaluate(
                P0AssetProductionNextBatchGate.EvaluateCurrentGate(),
                queue);

            Assert.IsFalse(report.IsReady);
            Assert.AreNotEqual(P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId, report.RecommendedFirstQueueId);
            StringAssert.Contains("Non-cat candidate review queue is incomplete", report.BuildDetailedSummary());
        }

        private static P0AssetProductionQueueEntry[] CreateQueueArray()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> source = P0AssetProductionQueueCatalog.CreateP0Queue();
            P0AssetProductionQueueEntry[] queue = new P0AssetProductionQueueEntry[source.Count];
            for (int i = 0; i < queue.Length; i++)
            {
                queue[i] = source[i];
            }

            return queue;
        }

        private static int FindQueueIndex(P0AssetProductionQueueEntry[] queue, string queueId)
        {
            for (int i = 0; i < queue.Length; i++)
            {
                if (queue[i].QueueId == queueId)
                {
                    return i;
                }
            }

            Assert.Fail("Missing queue item: " + queueId);
            return -1;
        }
    }
}
