using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0AssetUnityValidationChecklistSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetUnityValidationChecklistIssue
    {
        public P0AssetUnityValidationChecklistIssue(P0AssetUnityValidationChecklistSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetUnityValidationChecklistSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetUnityValidationChecklistReport
    {
        private readonly List<P0AssetUnityValidationChecklistIssue> issues = new List<P0AssetUnityValidationChecklistIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0AssetProductionQueueEntry> entries = new List<P0AssetProductionQueueEntry>();

        public IReadOnlyList<P0AssetUnityValidationChecklistIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0AssetProductionQueueEntry> Entries => entries.AsReadOnly();

        public int QueueItemCount { get; private set; }

        public int CandidateReviewItemCount { get; private set; }

        public int UnityBlockedItemCount { get; private set; }

        public int ActiveScreenshotValidationItemCount { get; private set; }

        public int InstalledAssetValidationItemCount { get; private set; }

        public int FormalInstallDecisionItemCount { get; private set; }

        public int CandidateNoMetaPolicyCount { get; private set; }

        public int ConsoleGateItemCount { get; private set; }

        public int FailureCount => Count(P0AssetUnityValidationChecklistSeverity.Failure);

        public bool IsReadyForUnityValidation => FailureCount == 0
            && coveredChecks.Count >= P0AssetUnityValidationChecklist.ExpectedCoveredCheckCount;

        public void SetCounts(
            IReadOnlyList<P0AssetProductionQueueEntry> queue,
            int candidateReviewItemCount,
            int unityBlockedItemCount,
            int activeScreenshotValidationItemCount,
            int installedAssetValidationItemCount,
            int formalInstallDecisionItemCount,
            int candidateNoMetaPolicyCount,
            int consoleGateItemCount)
        {
            entries.Clear();
            if (queue != null)
            {
                for (int i = 0; i < queue.Count; i++)
                {
                    entries.Add(queue[i]);
                }
            }

            QueueItemCount = entries.Count;
            CandidateReviewItemCount = candidateReviewItemCount;
            UnityBlockedItemCount = unityBlockedItemCount;
            ActiveScreenshotValidationItemCount = activeScreenshotValidationItemCount;
            InstalledAssetValidationItemCount = installedAssetValidationItemCount;
            FormalInstallDecisionItemCount = formalInstallDecisionItemCount;
            CandidateNoMetaPolicyCount = candidateNoMetaPolicyCount;
            ConsoleGateItemCount = consoleGateItemCount;
        }

        public void AddIssue(P0AssetUnityValidationChecklistSeverity severity, string message)
        {
            issues.Add(new P0AssetUnityValidationChecklistIssue(severity, message));
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
            return IsReadyForUnityValidation
                ? "P0 asset Unity validation checklist is ready for " + QueueItemCount + " queue item(s)."
                : "P0 asset Unity validation checklist has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Queue items: " + QueueItemCount,
                "Candidate review items: " + CandidateReviewItemCount,
                "Unity-blocked items: " + UnityBlockedItemCount,
                "Active screenshot validation items: " + ActiveScreenshotValidationItemCount,
                "Installed asset validation items: " + InstalledAssetValidationItemCount,
                "Formal install decision items: " + FormalInstallDecisionItemCount,
                "Candidate no-meta policy items: " + CandidateNoMetaPolicyCount,
                "Console-gated items: " + ConsoleGateItemCount
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

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# P0 Asset Unity Validation Checklist");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Counts");
            builder.AppendLine();
            builder.AppendLine("- Queue items: " + QueueItemCount);
            builder.AppendLine("- Candidate review items: " + CandidateReviewItemCount);
            builder.AppendLine("- Unity-blocked items: " + UnityBlockedItemCount);
            builder.AppendLine("- Active screenshot validation items: " + ActiveScreenshotValidationItemCount);
            builder.AppendLine("- Installed asset validation items: " + InstalledAssetValidationItemCount);
            builder.AppendLine("- Formal install decision items: " + FormalInstallDecisionItemCount);
            builder.AppendLine("- Candidate no-meta policy items: " + CandidateNoMetaPolicyCount);
            builder.AppendLine("- Console-gated items: " + ConsoleGateItemCount);
            builder.AppendLine();
            builder.AppendLine("## Validation Order");
            builder.AppendLine();
            builder.AppendLine("1. Refresh the Unity AssetDatabase.");
            builder.AppendLine("2. Inspect Sprite import settings for any already-installed assets.");
            builder.AppendLine("3. Confirm candidate-only folders have no Unity `.meta` files.");
            builder.AppendLine("4. Capture active-cat, active-enemy, cat-room, character-select, battle HUD, skill-selection, skill VFX, runtime-control, secondary-warning, loading/start, result/settlement, settings/pause, dream-route/route-map, and bedroom interaction screenshots.");
            builder.AppendLine("5. Compare starter-cat screenshots against locked colored three-view turnarounds.");
            builder.AppendLine("6. Check scene and prefab references for every approved install target.");
            builder.AppendLine("7. Check Console logs after each validation pass.");
            builder.AppendLine("8. Write formal install decisions only for rows that pass the matching Unity evidence gate.");
            builder.AppendLine();
            builder.AppendLine("## Queue Items");
            builder.AppendLine();

            for (int i = 0; i < entries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = entries[i];
                builder.AppendLine("### " + entry.DisplayName);
                builder.AppendLine();
                builder.AppendLine("- Queue id: `" + entry.QueueId + "`");
                builder.AppendLine("- Phase: `" + entry.Phase + "`");
                builder.AppendLine("- State: `" + entry.State + "`");
                builder.AppendLine("- Candidate directory: `" + entry.CandidateDirectory + "`");
                builder.AppendLine("- Unity import root: `" + entry.UnityImportRoot + "`");
                builder.AppendLine("- Required evidence: " + JoinInline(entry.RequiredEvidence));
                builder.AppendLine("- Next action: " + entry.NextAction);
                builder.AppendLine();
            }

            if (issues.Count > 0)
            {
                builder.AppendLine("## Issues");
                builder.AppendLine();
                for (int i = 0; i < issues.Count; i++)
                {
                    builder.AppendLine("- [" + issues[i].Severity + "] " + issues[i].Message);
                }
            }

            return builder.ToString();
        }

        private int Count(P0AssetUnityValidationChecklistSeverity severity)
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

        private static string JoinInline(IReadOnlyList<string> values)
        {
            if (values == null || values.Count == 0)
            {
                return "(none)";
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < values.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append("; ");
                }

                builder.Append('`');
                builder.Append(values[i]);
                builder.Append('`');
            }

            return builder.ToString();
        }
    }

    public sealed class P0AssetUnityValidationChecklistFileEvidenceReport
    {
        private readonly List<P0AssetUnityValidationChecklistIssue> issues = new List<P0AssetUnityValidationChecklistIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public P0AssetUnityValidationChecklistFileEvidenceReport(string checklistPath)
        {
            ChecklistPath = checklistPath ?? string.Empty;
        }

        public string ChecklistPath { get; }

        public IReadOnlyList<P0AssetUnityValidationChecklistIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public bool FileExists { get; private set; }

        public bool SummaryMentioned { get; private set; }

        public bool QueueItemCountMentioned { get; private set; }

        public bool CandidateReviewItemCountMentioned { get; private set; }

        public bool UnityBlockedItemCountMentioned { get; private set; }

        public bool Batch65Mentioned { get; private set; }

        public bool Batch67Mentioned { get; private set; }

        public bool Batch83Mentioned { get; private set; }

        public bool Batch84Mentioned { get; private set; }

        public bool Batch85Mentioned { get; private set; }

        public bool Batch86Mentioned { get; private set; }

        public bool Batch87Mentioned { get; private set; }

        public bool Batch88Mentioned { get; private set; }

        public bool Batch89Mentioned { get; private set; }

        public bool Batch90Mentioned { get; private set; }

        public bool NoMetaPolicyMentioned { get; private set; }

        public bool ConsoleGateMentioned { get; private set; }

        public bool StarterCatTurnaroundGateMentioned { get; private set; }

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0AssetUnityValidationChecklistFileEvidence.ExpectedCoveredCheckCount;

        public int FailureCount => Count(P0AssetUnityValidationChecklistSeverity.Failure);

        public void MarkFileExists(bool value)
        {
            FileExists = value;
        }

        public void MarkSummaryMentioned(bool value)
        {
            SummaryMentioned = value;
        }

        public void MarkQueueItemCountMentioned(bool value)
        {
            QueueItemCountMentioned = value;
        }

        public void MarkCandidateReviewItemCountMentioned(bool value)
        {
            CandidateReviewItemCountMentioned = value;
        }

        public void MarkUnityBlockedItemCountMentioned(bool value)
        {
            UnityBlockedItemCountMentioned = value;
        }

        public void MarkBatch65Mentioned(bool value)
        {
            Batch65Mentioned = value;
        }

        public void MarkBatch67Mentioned(bool value)
        {
            Batch67Mentioned = value;
        }

        public void MarkBatch83Mentioned(bool value)
        {
            Batch83Mentioned = value;
        }

        public void MarkBatch84Mentioned(bool value)
        {
            Batch84Mentioned = value;
        }

        public void MarkBatch85Mentioned(bool value)
        {
            Batch85Mentioned = value;
        }

        public void MarkBatch86Mentioned(bool value)
        {
            Batch86Mentioned = value;
        }

        public void MarkBatch87Mentioned(bool value)
        {
            Batch87Mentioned = value;
        }

        public void MarkBatch88Mentioned(bool value)
        {
            Batch88Mentioned = value;
        }

        public void MarkBatch89Mentioned(bool value)
        {
            Batch89Mentioned = value;
        }

        public void MarkBatch90Mentioned(bool value)
        {
            Batch90Mentioned = value;
        }

        public void MarkNoMetaPolicyMentioned(bool value)
        {
            NoMetaPolicyMentioned = value;
        }

        public void MarkConsoleGateMentioned(bool value)
        {
            ConsoleGateMentioned = value;
        }

        public void MarkStarterCatTurnaroundGateMentioned(bool value)
        {
            StarterCatTurnaroundGateMentioned = value;
        }

        public void AddIssue(P0AssetUnityValidationChecklistSeverity severity, string message)
        {
            issues.Add(new P0AssetUnityValidationChecklistIssue(severity, message));
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
                ? "P0 asset Unity validation checklist file evidence is ready: " + ChecklistPath
                : "P0 asset Unity validation checklist file evidence has " + FailureCount + " failure(s): " + ChecklistPath;
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "File exists: " + FileExists,
                "Summary mentioned: " + SummaryMentioned,
                "Queue item count mentioned: " + QueueItemCountMentioned,
                "Candidate review count mentioned: " + CandidateReviewItemCountMentioned,
                "Unity-blocked count mentioned: " + UnityBlockedItemCountMentioned,
                "Batch 65 mentioned: " + Batch65Mentioned,
                "Batch 67 mentioned: " + Batch67Mentioned,
                "Batch 83 mentioned: " + Batch83Mentioned,
                "Batch 84 mentioned: " + Batch84Mentioned,
                "Batch 85 mentioned: " + Batch85Mentioned,
                "Batch 86 mentioned: " + Batch86Mentioned,
                "Batch 87 mentioned: " + Batch87Mentioned,
                "Batch 88 mentioned: " + Batch88Mentioned,
                "Batch 89 mentioned: " + Batch89Mentioned,
                "Batch 90 mentioned: " + Batch90Mentioned,
                "No-meta policy mentioned: " + NoMetaPolicyMentioned,
                "Console gate mentioned: " + ConsoleGateMentioned,
                "Starter-cat turnaround gate mentioned: " + StarterCatTurnaroundGateMentioned
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

        private int Count(P0AssetUnityValidationChecklistSeverity severity)
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

    public static class P0AssetUnityValidationChecklist
    {
        public const int ExpectedCoveredCheckCount = 8;

        public static P0AssetUnityValidationChecklistReport EvaluateCurrentQueue()
        {
            return Evaluate(P0AssetProductionQueueCatalog.CreateP0Queue(), DefaultDirectoryExists);
        }

        public static P0AssetUnityValidationChecklistReport Evaluate(
            IReadOnlyList<P0AssetProductionQueueEntry> queue,
            Func<string, bool> directoryExists)
        {
            P0AssetUnityValidationChecklistReport report = new P0AssetUnityValidationChecklistReport();
            IReadOnlyList<P0AssetProductionQueueEntry> entries = queue ?? Array.Empty<P0AssetProductionQueueEntry>();
            Func<string, bool> exists = directoryExists ?? DefaultDirectoryExists;

            int candidateReviewItemCount = 0;
            int unityBlockedItemCount = 0;
            int activeScreenshotValidationItemCount = 0;
            int installedAssetValidationItemCount = 0;
            int formalInstallDecisionItemCount = 0;
            int candidateNoMetaPolicyCount = 0;
            int consoleGateItemCount = 0;
            bool allCandidateDirectoriesExist = entries.Count > 0;
            bool allCandidateItemsProtectMetaPolicy = true;
            bool allItemsHaveUnityImportRoots = entries.Count > 0;

            for (int i = 0; i < entries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = entries[i];
                if (entry.IsCandidatePackCompletePendingUnityReview)
                {
                    candidateReviewItemCount++;
                    if (HasNoMetaPolicy(entry))
                    {
                        candidateNoMetaPolicyCount++;
                    }
                    else
                    {
                        allCandidateItemsProtectMetaPolicy = false;
                        report.AddIssue(P0AssetUnityValidationChecklistSeverity.Failure, entry.QueueId + " candidate item does not state the no-meta policy.");
                    }
                }

                if (entry.IsUnityBlocked)
                {
                    unityBlockedItemCount++;
                }

                if (RequiresActiveScreenshot(entry))
                {
                    activeScreenshotValidationItemCount++;
                }

                if (RequiresInstalledAssetValidation(entry))
                {
                    installedAssetValidationItemCount++;
                }

                if (entry.Phase == P0AssetProductionQueuePhase.FormalUnityInstall)
                {
                    formalInstallDecisionItemCount++;
                }

                if (HasConsoleGate(entry))
                {
                    consoleGateItemCount++;
                }

                if (string.IsNullOrWhiteSpace(entry.UnityImportRoot) || !entry.UnityImportRoot.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase))
                {
                    allItemsHaveUnityImportRoots = false;
                    report.AddIssue(P0AssetUnityValidationChecklistSeverity.Failure, entry.QueueId + " has an invalid Unity import root.");
                }

                if (string.IsNullOrWhiteSpace(entry.CandidateDirectory) || !exists(entry.CandidateDirectory))
                {
                    allCandidateDirectoriesExist = false;
                    report.AddIssue(P0AssetUnityValidationChecklistSeverity.Failure, entry.QueueId + " candidate directory is missing: " + entry.CandidateDirectory);
                }
            }

            report.SetCounts(
                entries,
                candidateReviewItemCount,
                unityBlockedItemCount,
                activeScreenshotValidationItemCount,
                installedAssetValidationItemCount,
                formalInstallDecisionItemCount,
                candidateNoMetaPolicyCount,
                consoleGateItemCount);

            Require(report, entries.Count == P0AssetProductionQueueCatalog.ExpectedP0QueueCount, "Checklist covers the current P0 asset production queue.", "Checklist queue count is stale.");
            Require(report, candidateReviewItemCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, "Checklist covers all candidate packs pending Unity review.", "Candidate review checklist count is stale.");
            Require(report, unityBlockedItemCount == P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount, "Checklist covers all Unity-blocked validation and install items.", "Unity-blocked checklist count is stale.");
            Require(report, activeScreenshotValidationItemCount >= 2, "Checklist includes active-cat and active-enemy screenshot validation gates.", "Active screenshot validation gates are missing.");
            Require(report, installedAssetValidationItemCount >= 2, "Checklist includes installed skill-HUD and starter-skill VFX validation gates.", "Installed asset validation gates are missing.");
            Require(report, formalInstallDecisionItemCount == 1, "Checklist includes the formal install decision gate.", "Formal install decision gate is missing.");
            Require(report, allCandidateItemsProtectMetaPolicy && candidateNoMetaPolicyCount == candidateReviewItemCount, "Checklist keeps candidate-only packs outside Unity meta/import state.", "Candidate no-meta policies are incomplete.");
            Require(report, allCandidateDirectoriesExist && allItemsHaveUnityImportRoots && consoleGateItemCount == entries.Count, "Checklist has directories, Unity import roots, and Console gates for every queue item.", "Checklist is missing directories, Unity import roots, or Console gates.");
            return report;
        }

        private static bool RequiresActiveScreenshot(P0AssetProductionQueueEntry entry)
        {
            return ContainsAny(entry.RequiredEvidence, "active")
                || Contains(entry.DisplayName, "Active");
        }

        private static bool RequiresInstalledAssetValidation(P0AssetProductionQueueEntry entry)
        {
            return Contains(entry.DisplayName, "Installed")
                || Contains(entry.NextAction, "installed");
        }

        private static bool HasNoMetaPolicy(P0AssetProductionQueueEntry entry)
        {
            return ContainsAny(entry.RequiredEvidence, "no Unity meta", "no Unity .meta", ".meta files");
        }

        private static bool HasConsoleGate(P0AssetProductionQueueEntry entry)
        {
            return ContainsAny(entry.RequiredEvidence, "Console")
                || Contains(entry.NextAction, "Console");
        }

        private static bool ContainsAny(IReadOnlyList<string> values, params string[] tokens)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                for (int tokenIndex = 0; tokenIndex < tokens.Length; tokenIndex++)
                {
                    if (Contains(values[i], tokens[tokenIndex]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool Contains(string value, string token)
        {
            return !string.IsNullOrEmpty(value)
                && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultDirectoryExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (Directory.Exists(path))
            {
                return true;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (Directory.Exists(candidate))
                {
                    return true;
                }

                current = current.Parent;
            }

            return false;
        }

        private static void Require(
            P0AssetUnityValidationChecklistReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetUnityValidationChecklistSeverity.Failure, failureMessage);
        }
    }

    public static class P0AssetUnityValidationChecklistFileEvidence
    {
        public const int ExpectedCoveredCheckCount = 19;
        public const string DefaultChecklistPath = "design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md";

        public static P0AssetUnityValidationChecklistFileEvidenceReport EvaluateCurrentFile()
        {
            return Evaluate(DefaultChecklistPath, P0AssetUnityValidationChecklist.EvaluateCurrentQueue(), DefaultFileExists, DefaultReadAllText);
        }

        public static P0AssetUnityValidationChecklistFileEvidenceReport Evaluate(
            string checklistPath,
            P0AssetUnityValidationChecklistReport expectedChecklist,
            Func<string, bool> fileExists,
            Func<string, string> readAllText)
        {
            P0AssetUnityValidationChecklistFileEvidenceReport report = new P0AssetUnityValidationChecklistFileEvidenceReport(checklistPath);
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            Func<string, string> read = readAllText ?? DefaultReadAllText;
            P0AssetUnityValidationChecklistReport expected = expectedChecklist ?? P0AssetUnityValidationChecklist.EvaluateCurrentQueue();

            bool hasFile = exists(checklistPath);
            report.MarkFileExists(hasFile);
            Require(report, hasFile, "Checklist markdown file exists at the asset-review path.", "Checklist markdown file is missing.");
            if (!hasFile)
            {
                return report;
            }

            string text = read(checklistPath) ?? string.Empty;
            bool summaryMentioned = Contains(text, expected.BuildSummary());
            bool queueItemCountMentioned = Contains(text, "Queue items: " + expected.QueueItemCount);
            bool candidateReviewItemCountMentioned = Contains(text, "Candidate review items: " + expected.CandidateReviewItemCount);
            bool unityBlockedItemCountMentioned = Contains(text, "Unity-blocked items: " + expected.UnityBlockedItemCount);
            bool batch65Mentioned = Contains(text, "batch_65_route_map_readability_candidates_2026-06-15")
                && Contains(text, P0AssetProductionQueueCatalog.RouteMapReadabilityCandidateQueueId)
                && Contains(text, "Route Map Readability Candidate Pack");
            bool batch67Mentioned = Contains(text, "batch_67_bedroom_interaction_affordance_candidates_2026-06-15")
                && Contains(text, P0AssetProductionQueueCatalog.BedroomInteractionAffordanceCandidateQueueId)
                && Contains(text, "Bedroom Interaction Affordance Candidate Pack");
            bool batch83Mentioned = Contains(text, "batch_83_loading_start_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.LoadingStartPreflightCandidateQueueId)
                && Contains(text, "Loading Start Preflight Candidate Pack");
            bool batch84Mentioned = Contains(text, "batch_84_result_settlement_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.ResultSettlementPreflightCandidateQueueId)
                && Contains(text, "Result Settlement Preflight Candidate Pack");
            bool batch85Mentioned = Contains(text, "batch_85_settings_pause_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.SettingsPausePreflightCandidateQueueId)
                && Contains(text, "Settings Pause Preflight Candidate Pack");
            bool batch86Mentioned = Contains(text, "batch_86_dream_route_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.DreamRoutePreflightCandidateQueueId)
                && Contains(text, "Dream Route Preflight Candidate Pack");
            bool batch87Mentioned = Contains(text, "batch_87_battle_hud_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.BattleHudPreflightCandidateQueueId)
                && Contains(text, "Battle HUD Preflight Candidate Pack");
            bool batch88Mentioned = Contains(text, "batch_88_character_select_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.CharacterSelectPreflightCandidateQueueId)
                && Contains(text, "Character Select Preflight Candidate Pack");
            bool batch89Mentioned = Contains(text, "batch_89_skill_selection_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.SkillSelectionPreflightCandidateQueueId)
                && Contains(text, "Skill Selection Preflight Candidate Pack");
            bool batch90Mentioned = Contains(text, "batch_90_cat_room_preflight_2026-06-25")
                && Contains(text, P0AssetProductionQueueCatalog.CatRoomPreflightCandidateQueueId)
                && Contains(text, "Cat Room Preflight Candidate Pack");
            bool noMetaPolicyMentioned = Contains(text, "no Unity meta files")
                || Contains(text, "no Unity `.meta` files")
                || Contains(text, "no Unity .meta files");
            bool consoleGateMentioned = Contains(text, "Check Console logs")
                && Contains(text, "Console-gated items: " + expected.ConsoleGateItemCount);
            bool starterCatTurnaroundGateMentioned = Contains(text, "colored three-view turnarounds")
                && Contains(text, "Starter Cat Active Screenshot Validation");

            report.MarkSummaryMentioned(summaryMentioned);
            report.MarkQueueItemCountMentioned(queueItemCountMentioned);
            report.MarkCandidateReviewItemCountMentioned(candidateReviewItemCountMentioned);
            report.MarkUnityBlockedItemCountMentioned(unityBlockedItemCountMentioned);
            report.MarkBatch65Mentioned(batch65Mentioned);
            report.MarkBatch67Mentioned(batch67Mentioned);
            report.MarkBatch83Mentioned(batch83Mentioned);
            report.MarkBatch84Mentioned(batch84Mentioned);
            report.MarkBatch85Mentioned(batch85Mentioned);
            report.MarkBatch86Mentioned(batch86Mentioned);
            report.MarkBatch87Mentioned(batch87Mentioned);
            report.MarkBatch88Mentioned(batch88Mentioned);
            report.MarkBatch89Mentioned(batch89Mentioned);
            report.MarkBatch90Mentioned(batch90Mentioned);
            report.MarkNoMetaPolicyMentioned(noMetaPolicyMentioned);
            report.MarkConsoleGateMentioned(consoleGateMentioned);
            report.MarkStarterCatTurnaroundGateMentioned(starterCatTurnaroundGateMentioned);

            Require(report, summaryMentioned, "Checklist summary matches the current queue evaluation.", "Checklist summary is missing or stale.");
            Require(report, queueItemCountMentioned, "Checklist records the current queue count.", "Checklist queue count is missing or stale.");
            Require(report, candidateReviewItemCountMentioned, "Checklist records the current candidate-review count.", "Checklist candidate-review count is missing or stale.");
            Require(report, unityBlockedItemCountMentioned, "Checklist records the current Unity-blocked count.", "Checklist Unity-blocked count is missing or stale.");
            Require(report, batch65Mentioned, "Checklist covers Batch 65 route-map readability.", "Checklist is missing Batch 65 route-map readability coverage.");
            Require(report, batch67Mentioned, "Checklist covers Batch 67 bedroom interaction affordances.", "Checklist is missing Batch 67 bedroom interaction affordance coverage.");
            Require(report, batch83Mentioned, "Checklist covers Batch 83 loading/start preflight.", "Checklist is missing Batch 83 loading/start preflight coverage.");
            Require(report, batch84Mentioned, "Checklist covers Batch 84 result/settlement preflight.", "Checklist is missing Batch 84 result/settlement preflight coverage.");
            Require(report, batch85Mentioned, "Checklist covers Batch 85 settings/pause preflight.", "Checklist is missing Batch 85 settings/pause preflight coverage.");
            Require(report, batch86Mentioned, "Checklist covers Batch 86 dream-route preflight.", "Checklist is missing Batch 86 dream-route preflight coverage.");
            Require(report, batch87Mentioned, "Checklist covers Batch 87 battle HUD preflight.", "Checklist is missing Batch 87 battle HUD preflight coverage.");
            Require(report, batch88Mentioned, "Checklist covers Batch 88 character-select preflight.", "Checklist is missing Batch 88 character-select preflight coverage.");
            Require(report, batch89Mentioned, "Checklist covers Batch 89 skill-selection preflight.", "Checklist is missing Batch 89 skill-selection preflight coverage.");
            Require(report, batch90Mentioned, "Checklist covers Batch 90 cat-room preflight.", "Checklist is missing Batch 90 cat-room preflight coverage.");
            Require(report, noMetaPolicyMentioned, "Checklist preserves candidate-only no-meta policy.", "Checklist is missing the no-meta policy.");
            Require(report, consoleGateMentioned, "Checklist preserves Console validation gates.", "Checklist is missing Console validation gates.");
            Require(report, starterCatTurnaroundGateMentioned, "Checklist preserves starter-cat colored-turnaround validation.", "Checklist is missing starter-cat colored-turnaround validation.");
            Require(report, expected.IsReadyForUnityValidation, "Checklist was generated from a ready queue evaluation.", "Expected queue evaluation is not ready.");
            return report;
        }

        private static bool DefaultFileExists(string path)
        {
            return File.Exists(ResolveProjectPath(path));
        }

        private static string DefaultReadAllText(string path)
        {
            return File.ReadAllText(ResolveProjectPath(path));
        }

        private static string ResolveProjectPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path))
            {
                return path;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return path.Replace('/', Path.DirectorySeparatorChar);
        }

        private static bool Contains(string value, string token)
        {
            return !string.IsNullOrEmpty(value)
                && !string.IsNullOrEmpty(token)
                && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0AssetUnityValidationChecklistFileEvidenceReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetUnityValidationChecklistSeverity.Failure, failureMessage);
        }
    }
}
