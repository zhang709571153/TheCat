using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetUnityValidationChecklistTests
    {
        [Test]
        public void EvaluateCurrentQueue_BuildsUnityValidationChecklist()
        {
            P0AssetUnityValidationChecklistReport report = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();

            Assert.IsTrue(report.IsReadyForUnityValidation, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.QueueItemCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.CandidateReviewItemCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, report.UnityBlockedItemCount);
            Assert.AreEqual(2, report.ActiveScreenshotValidationItemCount);
            Assert.AreEqual(2, report.InstalledAssetValidationItemCount);
            Assert.AreEqual(1, report.FormalInstallDecisionItemCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.CandidateNoMetaPolicyCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.ConsoleGateItemCount);
            Assert.AreEqual(P0AssetUnityValidationChecklist.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
        }

        [Test]
        public void BuildMarkdown_ListsPendingUnityGates()
        {
            P0AssetUnityValidationChecklistReport report = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("P0 Asset Unity Validation Checklist", markdown);
            StringAssert.Contains("Starter Cat Active Screenshot Validation", markdown);
            StringAssert.Contains("Core Enemy Active Screenshot Validation", markdown);
            StringAssert.Contains("Bedroom Interactable Candidate Pack", markdown);
            StringAssert.Contains("Cat Room Preflight Candidate Pack", markdown);
            StringAssert.Contains("Character Select Preflight Candidate Pack", markdown);
            StringAssert.Contains("Battle HUD Preflight Candidate Pack", markdown);
            StringAssert.Contains("Skill Selection Preflight Candidate Pack", markdown);
            StringAssert.Contains("Runtime Control Icon Candidate Pack", markdown);
            StringAssert.Contains("Runtime Control Panel Candidate Pack", markdown);
            StringAssert.Contains("Secondary Enemy Warning Candidate Pack", markdown);
            StringAssert.Contains("Route Map Readability Candidate Pack", markdown);
            StringAssert.Contains("Bedroom Interaction Affordance Candidate Pack", markdown);
            StringAssert.Contains("Loading Start Preflight Candidate Pack", markdown);
            StringAssert.Contains("Result Settlement Preflight Candidate Pack", markdown);
            StringAssert.Contains("Settings Pause Preflight Candidate Pack", markdown);
            StringAssert.Contains("Dream Route Preflight Candidate Pack", markdown);
            StringAssert.Contains("Starter Skill VFX Installed Unity Validation", markdown);
            StringAssert.Contains("Skill HUD Feedback Installed Unity Validation", markdown);
            StringAssert.Contains("Formal Install Decision Packet", markdown);
            StringAssert.Contains("Refresh the Unity AssetDatabase", markdown);
            StringAssert.Contains("Check Console logs", markdown);
            StringAssert.Contains("colored three-view turnarounds", markdown);
            StringAssert.Contains("batch_63_runtime_control_panel_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_88_character_select_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_87_battle_hud_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_89_skill_selection_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_90_cat_room_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_64_secondary_enemy_warning_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_65_route_map_readability_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_67_bedroom_interaction_affordance_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_83_loading_start_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_84_result_settlement_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_85_settings_pause_preflight_2026-06-25", markdown);
            StringAssert.Contains("batch_86_dream_route_preflight_2026-06-25", markdown);
        }

        [Test]
        public void EvaluateCurrentFile_AcceptsGeneratedChecklistArtifact()
        {
            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.EvaluateCurrentFile();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.FileExists);
            Assert.IsTrue(report.Batch65Mentioned);
            Assert.IsTrue(report.Batch67Mentioned);
            Assert.IsTrue(report.Batch83Mentioned);
            Assert.IsTrue(report.Batch84Mentioned);
            Assert.IsTrue(report.Batch85Mentioned);
            Assert.IsTrue(report.Batch86Mentioned);
            Assert.IsTrue(report.Batch87Mentioned);
            Assert.IsTrue(report.Batch88Mentioned);
            Assert.IsTrue(report.Batch89Mentioned);
            Assert.IsTrue(report.Batch90Mentioned);
            Assert.IsTrue(report.StarterCatTurnaroundGateMentioned);
            Assert.AreEqual(P0AssetUnityValidationChecklistFileEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch65FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_65_route_map_readability_candidates_2026-06-15",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch65Mentioned);
            StringAssert.Contains("Batch 65", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch67FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_67_bedroom_interaction_affordance_candidates_2026-06-15",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch67Mentioned);
            StringAssert.Contains("Batch 67", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch83FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_83_loading_start_preflight_2026-06-25",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch83Mentioned);
            StringAssert.Contains("Batch 83", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch84FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_84_result_settlement_preflight_2026-06-25",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch84Mentioned);
            StringAssert.Contains("Batch 84", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch85FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_85_settings_pause_preflight_2026-06-25",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch85Mentioned);
            StringAssert.Contains("Batch 85", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch86FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_86_dream_route_preflight_2026-06-25",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch86Mentioned);
            StringAssert.Contains("Batch 86", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch87FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_87_battle_hud_preflight_2026-06-25",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch87Mentioned);
            StringAssert.Contains("Batch 87", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateFileEvidence_MissingBatch88FailsChecklist()
        {
            P0AssetUnityValidationChecklistReport checklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            string markdown = checklist.BuildMarkdown().Replace(
                "batch_88_character_select_preflight_2026-06-25",
                "batch_removed_for_regression_test",
                System.StringComparison.Ordinal);

            P0AssetUnityValidationChecklistFileEvidenceReport report = P0AssetUnityValidationChecklistFileEvidence.Evaluate(
                P0AssetUnityValidationChecklistFileEvidence.DefaultChecklistPath,
                checklist,
                _ => true,
                _ => markdown);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.Batch88Mentioned);
            StringAssert.Contains("Batch 88", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingCandidateDirectoryFailsChecklist()
        {
            P0AssetUnityValidationChecklistReport report = P0AssetUnityValidationChecklist.Evaluate(
                P0AssetProductionQueueCatalog.CreateP0Queue(),
                path => !path.Contains("batch_63_runtime_control_panel_candidates_2026-06-15", System.StringComparison.Ordinal));

            Assert.IsFalse(report.IsReadyForUnityValidation);
            StringAssert.Contains("candidate directory is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_CandidateWithoutNoMetaPolicyFailsChecklist()
        {
            P0AssetProductionQueueEntry[] queue = CreateQueueArray();
            int targetIndex = FindQueueIndex(queue, P0AssetProductionQueueCatalog.RuntimeControlIconCandidateQueueId);
            P0AssetProductionQueueEntry entry = queue[targetIndex];
            P0AssetProductionQueueEntry mutated = new P0AssetProductionQueueEntry(
                entry.QueueId,
                entry.Priority,
                entry.DisplayName,
                entry.SubjectGroup,
                entry.Phase,
                entry.State,
                entry.ExecutionPromptPath,
                entry.CandidateDirectory,
                entry.UnityImportRoot,
                entry.RelatedBatchSlugs,
                entry.SourceLockIds,
                new[] { "runtime_control_icons_batch62_manifest.csv", "thecat_ui_runtime_controls_batch62_review_sheet.png", "runtime_control_icons_batch62_candidate_review.md" },
                entry.ForbiddenWriteRoots,
                entry.NextAction);

            queue[targetIndex] = mutated;
            P0AssetUnityValidationChecklistReport report = P0AssetUnityValidationChecklist.Evaluate(queue, _ => true);

            Assert.IsFalse(report.IsReadyForUnityValidation);
            StringAssert.Contains("no-meta policy", report.BuildDetailedSummary());
        }

        private static P0AssetProductionQueueEntry[] CreateQueueArray()
        {
            System.Collections.Generic.IReadOnlyList<P0AssetProductionQueueEntry> source = P0AssetProductionQueueCatalog.CreateP0Queue();
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
