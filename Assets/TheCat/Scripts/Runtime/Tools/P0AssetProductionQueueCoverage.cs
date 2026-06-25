using System;
using System.Collections.Generic;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0AssetProductionQueueCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetProductionQueueCoverageIssue
    {
        public P0AssetProductionQueueCoverageIssue(P0AssetProductionQueueCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetProductionQueueCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetProductionQueueCoverageReport
    {
        private readonly List<P0AssetProductionQueueCoverageIssue> issues = new List<P0AssetProductionQueueCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0AssetProductionQueueCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int QueueCount { get; private set; }

        public int CodexRunnableCount { get; private set; }

        public int CandidatePackCompletePendingUnityReviewCount { get; private set; }

        public int UnityBlockedCount { get; private set; }

        public int ExistingPromptCount { get; private set; }

        public int CandidateDirectoryPolicyCount { get; private set; }

        public int ForbiddenWriteRootCount { get; private set; }

        public int RequiredEvidenceCount { get; private set; }

        public int FailureCount => Count(P0AssetProductionQueueCoverageSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0AssetProductionQueueCoverage.ExpectedCoveredCheckCount;

        public void SetCounts(
            int queueCount,
            int codexRunnableCount,
            int candidatePackCompletePendingUnityReviewCount,
            int unityBlockedCount,
            int existingPromptCount,
            int candidateDirectoryPolicyCount,
            int forbiddenWriteRootCount,
            int requiredEvidenceCount)
        {
            QueueCount = queueCount;
            CodexRunnableCount = codexRunnableCount;
            CandidatePackCompletePendingUnityReviewCount = candidatePackCompletePendingUnityReviewCount;
            UnityBlockedCount = unityBlockedCount;
            ExistingPromptCount = existingPromptCount;
            CandidateDirectoryPolicyCount = candidateDirectoryPolicyCount;
            ForbiddenWriteRootCount = forbiddenWriteRootCount;
            RequiredEvidenceCount = requiredEvidenceCount;
        }

        public void AddIssue(P0AssetProductionQueueCoverageSeverity severity, string message)
        {
            issues.Add(new P0AssetProductionQueueCoverageIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsReady
                ? "P0 asset production queue is ready with " + QueueCount + " queued item(s)."
                : "P0 asset production queue has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Queue items: " + QueueCount,
                "Codex-runnable items: " + CodexRunnableCount,
                "Candidate packs complete pending Unity review: " + CandidatePackCompletePendingUnityReviewCount,
                "Unity-blocked items: " + UnityBlockedCount,
                "Existing prompts: " + ExistingPromptCount,
                "Candidate directory policies: " + CandidateDirectoryPolicyCount,
                "Forbidden write roots: " + ForbiddenWriteRootCount,
                "Required evidence items: " + RequiredEvidenceCount
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0AssetProductionQueueCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0AssetProductionQueueCoverage
    {
        public const int ExpectedCoveredCheckCount = 9;

        public static P0AssetProductionQueueCoverageReport EvaluateP0Queue()
        {
            return Evaluate(P0AssetProductionQueueCatalog.CreateP0Queue(), DefaultFileExists);
        }

        public static P0AssetProductionQueueCoverageReport Evaluate(
            IReadOnlyList<P0AssetProductionQueueEntry> queue,
            Func<string, bool> fileExists)
        {
            P0AssetProductionQueueCoverageReport report = new P0AssetProductionQueueCoverageReport();
            IReadOnlyList<P0AssetProductionQueueEntry> entries = queue ?? Array.Empty<P0AssetProductionQueueEntry>();
            Func<string, bool> exists = fileExists ?? DefaultFileExists;

            int codexRunnableCount = 0;
            int candidatePackCompletePendingUnityReviewCount = 0;
            int unityBlockedCount = 0;
            int existingPromptCount = 0;
            int candidateDirectoryPolicyCount = 0;
            int forbiddenWriteRootCount = 0;
            int requiredEvidenceCount = 0;
            bool allPromptPathsValid = entries.Count > 0;
            bool allPromptFilesExist = entries.Count > 0;
            bool allCandidateDirectoriesOutsideAssets = entries.Count > 0;
            bool allUnityImportRootsInsideAssets = entries.Count > 0;
            bool allForbiddenRootsProtectUnityImports = entries.Count > 0;
            bool allRequiredEvidencePresent = entries.Count > 0;

            for (int i = 0; i < entries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = entries[i];

                if (entry.IsCodexRunnable)
                {
                    codexRunnableCount++;
                }

                if (entry.IsCandidatePackCompletePendingUnityReview)
                {
                    candidatePackCompletePendingUnityReviewCount++;
                }

                if (entry.IsUnityBlocked)
                {
                    unityBlockedCount++;
                }

                if (!IsPromptPath(entry.ExecutionPromptPath))
                {
                    allPromptPathsValid = false;
                    report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, entry.QueueId + " has invalid prompt path: " + entry.ExecutionPromptPath);
                }
                else if (exists(entry.ExecutionPromptPath))
                {
                    existingPromptCount++;
                }
                else
                {
                    allPromptFilesExist = false;
                    report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, entry.QueueId + " prompt is missing: " + entry.ExecutionPromptPath);
                }

                if (IsCandidateDirectory(entry.CandidateDirectory))
                {
                    candidateDirectoryPolicyCount++;
                }
                else
                {
                    allCandidateDirectoriesOutsideAssets = false;
                    report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, entry.QueueId + " candidate directory must stay under design/development/asset_candidates.");
                }

                if (!StartsWithAssets(entry.UnityImportRoot))
                {
                    allUnityImportRootsInsideAssets = false;
                    report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, entry.QueueId + " Unity import root must be inside Assets.");
                }

                forbiddenWriteRootCount += entry.ForbiddenWriteRoots.Count;
                if (!ProtectsUnityImportRoot(entry))
                {
                    allForbiddenRootsProtectUnityImports = false;
                    report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, entry.QueueId + " forbidden write roots do not protect its Unity import root.");
                }

                requiredEvidenceCount += entry.RequiredEvidence.Count;
                if (entry.RequiredEvidence.Count < 3)
                {
                    allRequiredEvidencePresent = false;
                    report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, entry.QueueId + " has too little required evidence.");
                }
            }

            report.SetCounts(
                entries.Count,
                codexRunnableCount,
                candidatePackCompletePendingUnityReviewCount,
                unityBlockedCount,
                existingPromptCount,
                candidateDirectoryPolicyCount,
                forbiddenWriteRootCount,
                requiredEvidenceCount);

            Require(report, entries.Count == P0AssetProductionQueueCatalog.ExpectedP0QueueCount, "P0 asset production queue has the expected starter-cat, enemy, prop, VFX, skill-HUD, cat-room preflight, character-select preflight, battle-HUD preflight, skill-selection preflight, runtime-control icon, runtime-control panel, secondary enemy warning, route-map readability, bedroom interaction affordance, loading/start preflight, result/settlement preflight, settings/pause preflight, dream-route preflight, and install decision items.", "P0 asset production queue count is incomplete.");
            Require(report, PrioritiesAreUniqueAndSorted(entries), "P0 asset production queue priorities are unique and sorted.", "P0 asset production queue priorities are missing, duplicated, or out of order.");
            Require(report, codexRunnableCount == P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount, "Queue has no remaining Codex-runnable candidate packs after Batch 54, Batch 55/61, Batch 57/60, Batch 62, Batch 63, and Batch 64 completion.", "Codex-runnable asset production queue items are stale.");
            Require(report, candidatePackCompletePendingUnityReviewCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, "Queue records Batch 54 prop, Batch 62 runtime-control icon, Batch 63 runtime-control panel, Batch 64 secondary enemy warning, Batch 65 route-map readability, Batch 67 bedroom interaction affordance, Batch 83 loading/start preflight, Batch 84 result/settlement preflight, Batch 85 settings/pause preflight, Batch 86 dream-route preflight, Batch 88 character-select preflight, Batch 87 battle HUD preflight, Batch 89 skill-selection preflight, and Batch 90 cat-room preflight candidate packs pending Unity review.", "Completed candidate-pack queue items are incomplete.");
            Require(report, unityBlockedCount == P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, "Queue keeps starter-cat, core-enemy, installed starter-skill VFX, installed skill-HUD feedback, and formal-install work blocked behind Unity validation.", "Unity-blocked asset production queue items are incomplete.");
            Require(report, allPromptPathsValid && allPromptFilesExist && existingPromptCount == entries.Count, "Every queue item has an existing scoped agent prompt.", "One or more asset production queue prompts are missing or invalid.");
            Require(report, allCandidateDirectoriesOutsideAssets && allUnityImportRootsInsideAssets, "Queue separates Codex candidate directories from Unity import roots.", "Asset production queue mixes candidates and Unity import roots.");
            Require(report, allForbiddenRootsProtectUnityImports && forbiddenWriteRootCount >= entries.Count, "Every queue item declares forbidden Unity write roots before formal approval.", "Asset production queue is missing forbidden write-root protections.");
            Require(report, allRequiredEvidencePresent && requiredEvidenceCount >= entries.Count * 3 && HasRequiredP0Gates(entries), "Queue records required evidence for starter cats, core enemies, Codex candidates, and formal install decisions.", "Asset production queue is missing required P0 gate evidence.");
            return report;
        }

        private static bool HasRequiredP0Gates(IReadOnlyList<P0AssetProductionQueueEntry> entries)
        {
            P0AssetProductionQueueEntry starterCats = Find(entries, P0AssetProductionQueueCatalog.StarterCatActiveValidationQueueId);
            P0AssetProductionQueueEntry enemies = Find(entries, P0AssetProductionQueueCatalog.CoreEnemyActiveValidationQueueId);
            P0AssetProductionQueueEntry bedroom = Find(entries, P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId);
            P0AssetProductionQueueEntry starterSkillVfx = Find(entries, P0AssetProductionQueueCatalog.StarterSkillVfxCandidateQueueId);
            P0AssetProductionQueueEntry skillHud = Find(entries, P0AssetProductionQueueCatalog.SkillHudFeedbackCandidateQueueId);
            P0AssetProductionQueueEntry catRoom = Find(entries, P0AssetProductionQueueCatalog.CatRoomPreflightCandidateQueueId);
            P0AssetProductionQueueEntry characterSelect = Find(entries, P0AssetProductionQueueCatalog.CharacterSelectPreflightCandidateQueueId);
            P0AssetProductionQueueEntry battleHud = Find(entries, P0AssetProductionQueueCatalog.BattleHudPreflightCandidateQueueId);
            P0AssetProductionQueueEntry skillSelection = Find(entries, P0AssetProductionQueueCatalog.SkillSelectionPreflightCandidateQueueId);
            P0AssetProductionQueueEntry runtimeControls = Find(entries, P0AssetProductionQueueCatalog.RuntimeControlIconCandidateQueueId);
            P0AssetProductionQueueEntry runtimePanels = Find(entries, P0AssetProductionQueueCatalog.RuntimeControlPanelCandidateQueueId);
            P0AssetProductionQueueEntry secondaryWarnings = Find(entries, P0AssetProductionQueueCatalog.SecondaryEnemyWarningCandidateQueueId);
            P0AssetProductionQueueEntry routeMap = Find(entries, P0AssetProductionQueueCatalog.RouteMapReadabilityCandidateQueueId);
            P0AssetProductionQueueEntry bedroomInteraction = Find(entries, P0AssetProductionQueueCatalog.BedroomInteractionAffordanceCandidateQueueId);
            P0AssetProductionQueueEntry loadingStart = Find(entries, P0AssetProductionQueueCatalog.LoadingStartPreflightCandidateQueueId);
            P0AssetProductionQueueEntry resultSettlement = Find(entries, P0AssetProductionQueueCatalog.ResultSettlementPreflightCandidateQueueId);
            P0AssetProductionQueueEntry settingsPause = Find(entries, P0AssetProductionQueueCatalog.SettingsPausePreflightCandidateQueueId);
            P0AssetProductionQueueEntry dreamRoute = Find(entries, P0AssetProductionQueueCatalog.DreamRoutePreflightCandidateQueueId);
            P0AssetProductionQueueEntry install = Find(entries, P0AssetProductionQueueCatalog.FormalInstallDecisionQueueId);

            return starterCats != null
                && Contains(starterCats.RelatedBatchSlugs, "batch_49_saiban_low_drift_refinement_2026-06-15")
                && Contains(starterCats.RelatedBatchSlugs, "batch_50_nephthys_strict_ai_generation_candidate_2026-06-15")
                && Contains(starterCats.RelatedBatchSlugs, "batch_51_suzune_strict_ai_generation_candidate_2026-06-15")
                && Contains(starterCats.RequiredEvidence, P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName)
                && Contains(starterCats.RequiredEvidence, P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName)
                && Contains(starterCats.RequiredEvidence, P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName)
                && enemies != null
                && Contains(enemies.RelatedBatchSlugs, "batch_40_black_mud_cutout_candidate_2026-06-15")
                && Contains(enemies.RelatedBatchSlugs, "batch_42_cold_light_cutout_candidate_2026-06-15")
                && Contains(enemies.RelatedBatchSlugs, "batch_44_call_tyrant_cutout_candidate_2026-06-15")
                && bedroom != null
                && bedroom.IsCandidatePackCompletePendingUnityReview
                && Contains(bedroom.RelatedBatchSlugs, "batch_54_bedroom_interactable_candidates_2026-06-15")
                && Contains(bedroom.RequiredEvidence, "bedroom_interactables_batch54_manifest.csv")
                && starterSkillVfx != null
                && starterSkillVfx.IsUnityBlocked
                && Contains(starterSkillVfx.RelatedBatchSlugs, "batch_55_starter_skill_vfx_candidates_2026-06-15")
                && Contains(starterSkillVfx.RelatedBatchSlugs, "batch_61_starter_skill_vfx_install_2026-06-15")
                && Contains(starterSkillVfx.RequiredEvidence, "starter_skill_vfx_batch61_install_manifest.csv")
                && skillHud != null
                && skillHud.IsUnityBlocked
                && Contains(skillHud.RelatedBatchSlugs, "batch_57_skill_hud_feedback_candidates_2026-06-15")
                && Contains(skillHud.RelatedBatchSlugs, "batch_60_skill_hud_feedback_install_2026-06-15")
                && Contains(skillHud.RequiredEvidence, "skill_hud_feedback_batch60_install_manifest.csv")
                && catRoom != null
                && catRoom.IsCandidatePackCompletePendingUnityReview
                && Contains(catRoom.RelatedBatchSlugs, "batch_90_cat_room_preflight_2026-06-25")
                && Contains(catRoom.RequiredEvidence, "thecat_ui_cat_room_batch90_manifest.csv")
                && characterSelect != null
                && characterSelect.IsCandidatePackCompletePendingUnityReview
                && Contains(characterSelect.RelatedBatchSlugs, "batch_88_character_select_preflight_2026-06-25")
                && Contains(characterSelect.RequiredEvidence, "thecat_ui_character_select_batch88_manifest.csv")
                && battleHud != null
                && battleHud.IsCandidatePackCompletePendingUnityReview
                && Contains(battleHud.RelatedBatchSlugs, "batch_87_battle_hud_preflight_2026-06-25")
                && Contains(battleHud.RequiredEvidence, "thecat_ui_battle_hud_batch87_manifest.csv")
                && skillSelection != null
                && skillSelection.IsCandidatePackCompletePendingUnityReview
                && Contains(skillSelection.RelatedBatchSlugs, "batch_89_skill_selection_preflight_2026-06-25")
                && Contains(skillSelection.RequiredEvidence, "thecat_ui_skill_selection_batch89_manifest.csv")
                && runtimeControls != null
                && runtimeControls.IsCandidatePackCompletePendingUnityReview
                && Contains(runtimeControls.RelatedBatchSlugs, "batch_62_runtime_control_icon_candidates_2026-06-15")
                && Contains(runtimeControls.RequiredEvidence, "runtime_control_icons_batch62_manifest.csv")
                && runtimePanels != null
                && runtimePanels.IsCandidatePackCompletePendingUnityReview
                && Contains(runtimePanels.RelatedBatchSlugs, "batch_63_runtime_control_panel_candidates_2026-06-15")
                && Contains(runtimePanels.RequiredEvidence, "runtime_control_panels_batch63_manifest.csv")
                && secondaryWarnings != null
                && secondaryWarnings.IsCandidatePackCompletePendingUnityReview
                && Contains(secondaryWarnings.RelatedBatchSlugs, "batch_64_secondary_enemy_warning_candidates_2026-06-15")
                && Contains(secondaryWarnings.RequiredEvidence, "secondary_enemy_warning_batch64_manifest.csv")
                && routeMap != null
                && routeMap.IsCandidatePackCompletePendingUnityReview
                && Contains(routeMap.RelatedBatchSlugs, "batch_65_route_map_readability_candidates_2026-06-15")
                && Contains(routeMap.RequiredEvidence, "route_map_readability_batch65_manifest.csv")
                && bedroomInteraction != null
                && bedroomInteraction.IsCandidatePackCompletePendingUnityReview
                && Contains(bedroomInteraction.RelatedBatchSlugs, "batch_67_bedroom_interaction_affordance_candidates_2026-06-15")
                && Contains(bedroomInteraction.RequiredEvidence, "bedroom_interaction_affordance_batch67_manifest.csv")
                && loadingStart != null
                && loadingStart.IsCandidatePackCompletePendingUnityReview
                && Contains(loadingStart.RelatedBatchSlugs, "batch_83_loading_start_preflight_2026-06-25")
                && Contains(loadingStart.RequiredEvidence, "thecat_ui_loading_start_batch83_manifest.csv")
                && resultSettlement != null
                && resultSettlement.IsCandidatePackCompletePendingUnityReview
                && Contains(resultSettlement.RelatedBatchSlugs, "batch_84_result_settlement_preflight_2026-06-25")
                && Contains(resultSettlement.RequiredEvidence, "thecat_ui_result_settlement_batch84_manifest.csv")
                && settingsPause != null
                && settingsPause.IsCandidatePackCompletePendingUnityReview
                && Contains(settingsPause.RelatedBatchSlugs, "batch_85_settings_pause_preflight_2026-06-25")
                && Contains(settingsPause.RequiredEvidence, "thecat_ui_settings_pause_batch85_manifest.csv")
                && dreamRoute != null
                && dreamRoute.IsCandidatePackCompletePendingUnityReview
                && Contains(dreamRoute.RelatedBatchSlugs, "batch_86_dream_route_preflight_2026-06-25")
                && Contains(dreamRoute.RequiredEvidence, "thecat_ui_dream_route_batch86_manifest.csv")
                && install != null
                && install.Phase == P0AssetProductionQueuePhase.FormalUnityInstall
                && Contains(install.RelatedBatchSlugs, "batch_54_bedroom_interactable_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_55_starter_skill_vfx_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_57_skill_hud_feedback_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_61_starter_skill_vfx_install_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_60_skill_hud_feedback_install_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_62_runtime_control_icon_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_63_runtime_control_panel_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_64_secondary_enemy_warning_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_65_route_map_readability_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_67_bedroom_interaction_affordance_candidates_2026-06-15")
                && Contains(install.RelatedBatchSlugs, "batch_83_loading_start_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_84_result_settlement_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_85_settings_pause_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_86_dream_route_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_88_character_select_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_87_battle_hud_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_89_skill_selection_preflight_2026-06-25")
                && Contains(install.RelatedBatchSlugs, "batch_90_cat_room_preflight_2026-06-25");
        }

        private static bool PrioritiesAreUniqueAndSorted(IReadOnlyList<P0AssetProductionQueueEntry> entries)
        {
            HashSet<int> priorities = new HashSet<int>();
            int last = int.MinValue;
            for (int i = 0; i < entries.Count; i++)
            {
                int current = entries[i].Priority;
                if (current <= last || !priorities.Add(current))
                {
                    return false;
                }

                last = current;
            }

            return true;
        }

        private static bool ProtectsUnityImportRoot(P0AssetProductionQueueEntry entry)
        {
            for (int i = 0; i < entry.ForbiddenWriteRoots.Count; i++)
            {
                string root = entry.ForbiddenWriteRoots[i];
                if (entry.UnityImportRoot.StartsWith(root, StringComparison.Ordinal)
                    || root.StartsWith(entry.UnityImportRoot, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static P0AssetProductionQueueEntry Find(
            IReadOnlyList<P0AssetProductionQueueEntry> entries,
            string queueId)
        {
            if (entries == null)
            {
                return null;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].QueueId == queueId)
                {
                    return entries[i];
                }
            }

            return null;
        }

        private static bool Contains(IReadOnlyList<string> values, string token)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == token)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsPromptPath(string path)
        {
            return path != null
                && path.StartsWith("design/development/agent_prompts/", StringComparison.Ordinal)
                && path.EndsWith(".md", StringComparison.Ordinal);
        }

        private static bool IsCandidateDirectory(string path)
        {
            return path != null
                && path.StartsWith("design/development/asset_candidates/", StringComparison.Ordinal)
                && !StartsWithAssets(path);
        }

        private static bool StartsWithAssets(string path)
        {
            return path != null && path.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase);
        }

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (File.Exists(path))
            {
                return true;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return true;
                }

                current = current.Parent;
            }

            return false;
        }

        private static void Require(
            P0AssetProductionQueueCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetProductionQueueCoverageSeverity.Failure, failureMessage);
        }
    }
}
