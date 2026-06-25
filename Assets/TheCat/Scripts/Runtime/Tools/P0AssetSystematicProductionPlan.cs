using System;
using System.Collections.Generic;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public sealed class P0AssetSystematicProductionPlanReport
    {
        private readonly List<string> issues = new List<string>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0AssetProductionQueueEntry> recommendedReviewEntries = new List<P0AssetProductionQueueEntry>();
        private readonly List<P0AssetProductionQueueEntry> protectedCatBodyEntries = new List<P0AssetProductionQueueEntry>();

        public IReadOnlyList<string> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0AssetProductionQueueEntry> RecommendedReviewEntries => recommendedReviewEntries.AsReadOnly();

        public IReadOnlyList<P0AssetProductionQueueEntry> ProtectedCatBodyEntries => protectedCatBodyEntries.AsReadOnly();

        public bool NextBatchGateReady { get; private set; }

        public bool CanContinueSystematicProduction { get; private set; }

        public bool StarterCatBodyLaneLocked { get; private set; }

        public bool RequiresUnityReviewBeforeInstall { get; private set; }

        public int QueueItemCount { get; private set; }

        public int NonCatCandidateReviewCount { get; private set; }

        public int ProtectedCatBodyEntryCount { get; private set; }

        public int UnityInstallBlockedCount { get; private set; }

        public bool FirstRecommendedLanePreflightReady { get; private set; }

        public bool FirstRecommendedLaneFormalInstallAllowed { get; private set; }

        public int FirstRecommendedLaneBlockingItemCount { get; private set; }

        public string RecommendedWorkMode { get; private set; } = string.Empty;

        public string RecommendedFirstQueueId { get; private set; } = string.Empty;

        public int FailureCount => issues.Count;

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0AssetSystematicProductionPlan.ExpectedCoveredCheckCount;

        public void SetSnapshot(
            bool nextBatchGateReady,
            bool canContinueSystematicProduction,
            bool starterCatBodyLaneLocked,
            bool requiresUnityReviewBeforeInstall,
            int queueItemCount,
            int nonCatCandidateReviewCount,
            int protectedCatBodyEntryCount,
            int unityInstallBlockedCount,
            bool firstRecommendedLanePreflightReady,
            bool firstRecommendedLaneFormalInstallAllowed,
            int firstRecommendedLaneBlockingItemCount,
            string recommendedWorkMode,
            string recommendedFirstQueueId)
        {
            NextBatchGateReady = nextBatchGateReady;
            CanContinueSystematicProduction = canContinueSystematicProduction;
            StarterCatBodyLaneLocked = starterCatBodyLaneLocked;
            RequiresUnityReviewBeforeInstall = requiresUnityReviewBeforeInstall;
            QueueItemCount = queueItemCount;
            NonCatCandidateReviewCount = nonCatCandidateReviewCount;
            ProtectedCatBodyEntryCount = protectedCatBodyEntryCount;
            UnityInstallBlockedCount = unityInstallBlockedCount;
            FirstRecommendedLanePreflightReady = firstRecommendedLanePreflightReady;
            FirstRecommendedLaneFormalInstallAllowed = firstRecommendedLaneFormalInstallAllowed;
            FirstRecommendedLaneBlockingItemCount = firstRecommendedLaneBlockingItemCount;
            RecommendedWorkMode = recommendedWorkMode ?? string.Empty;
            RecommendedFirstQueueId = recommendedFirstQueueId ?? string.Empty;
        }

        public void AddRecommendedReviewEntry(P0AssetProductionQueueEntry entry)
        {
            if (entry != null)
            {
                recommendedReviewEntries.Add(entry);
            }
        }

        public void AddProtectedCatBodyEntry(P0AssetProductionQueueEntry entry)
        {
            if (entry != null)
            {
                protectedCatBodyEntries.Add(entry);
            }
        }

        public void AddIssue(string issue)
        {
            if (!string.IsNullOrWhiteSpace(issue))
            {
                issues.Add(issue);
            }
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
            if (!IsReady)
            {
                return "P0 systematic asset production plan has " + FailureCount + " failure(s).";
            }

            return "P0 systematic asset production may continue with non-cat candidate review while starter-cat body assets stay locked.";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Next batch gate ready: " + (NextBatchGateReady ? "yes" : "no"),
                "Can continue systematic production: " + (CanContinueSystematicProduction ? "yes" : "no"),
                "Starter-cat body lane locked: " + (StarterCatBodyLaneLocked ? "yes" : "no"),
                "Unity review required before install: " + (RequiresUnityReviewBeforeInstall ? "yes" : "no"),
                "Queue items: " + QueueItemCount,
                "Non-cat candidate review items: " + NonCatCandidateReviewCount,
                "Protected starter-cat body entries: " + ProtectedCatBodyEntryCount,
                "Unity install blocked items: " + UnityInstallBlockedCount,
                "First recommended lane preflight ready: " + (FirstRecommendedLanePreflightReady ? "yes" : "no"),
                "First recommended lane formal install allowed: " + (FirstRecommendedLaneFormalInstallAllowed ? "yes" : "no"),
                "First recommended lane blocking items: " + FirstRecommendedLaneBlockingItemCount,
                "Recommended work mode: " + RecommendedWorkMode,
                "Recommended first queue id: " + RecommendedFirstQueueId
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[Failure] " + issues[i]);
            }

            return string.Join(Environment.NewLine, lines);
        }

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# P0 Systematic Asset Production Plan");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Decision");
            builder.AppendLine();
            builder.AppendLine("- Editor menu: `" + P0AssetSystematicProductionPlan.EditorMenuPath + "`");
            builder.AppendLine("- Report output: `" + P0AssetSystematicProductionPlan.ReportOutputPath + "`");
            builder.AppendLine("- Recommended work mode: `" + RecommendedWorkMode + "`");
            builder.AppendLine("- Can continue systematic production: " + (CanContinueSystematicProduction ? "yes" : "no"));
            builder.AppendLine("- Starter-cat body lane locked: " + (StarterCatBodyLaneLocked ? "yes" : "no"));
            builder.AppendLine("- Unity review required before install: " + (RequiresUnityReviewBeforeInstall ? "yes" : "no"));
            builder.AppendLine("- First recommended lane preflight ready: " + (FirstRecommendedLanePreflightReady ? "yes" : "no"));
            builder.AppendLine("- First recommended lane formal install allowed: " + (FirstRecommendedLaneFormalInstallAllowed ? "yes" : "no"));
            builder.AppendLine("- Cat-body policy: `" + P0AssetSystematicProductionPlan.StarterCatLockPolicy + "`");
            builder.AppendLine("- Cat-body generation rule: do not generate, crop, recolor, import, or runtime-bind starter-cat body art until active-cat Play Mode screenshots are approved against the locked colored three-view turnarounds.");
            builder.AppendLine();
            builder.AppendLine("## Recommended Review Order");
            builder.AppendLine();
            builder.AppendLine("| priority | queue | subject | candidate directory | install root |");
            builder.AppendLine("| --- | --- | --- | --- | --- |");

            for (int i = 0; i < recommendedReviewEntries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = recommendedReviewEntries[i];
                builder.Append("| ");
                builder.Append(entry.Priority);
                builder.Append(" | ");
                builder.Append(EscapeTable(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(EscapeTable(entry.SubjectGroup));
                builder.Append(" | `");
                builder.Append(EscapeTable(entry.CandidateDirectory));
                builder.Append("` | `");
                builder.Append(EscapeTable(entry.UnityImportRoot));
                builder.AppendLine("` |");
            }

            builder.AppendLine();
            builder.AppendLine("## Locked Cat Body Entries");
            builder.AppendLine();
            builder.AppendLine("| priority | queue | state | next action |");
            builder.AppendLine("| --- | --- | --- | --- |");

            for (int i = 0; i < protectedCatBodyEntries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = protectedCatBodyEntries[i];
                builder.Append("| ");
                builder.Append(entry.Priority);
                builder.Append(" | ");
                builder.Append(EscapeTable(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(entry.State);
                builder.Append(" | ");
                builder.Append(EscapeTable(entry.NextAction));
                builder.AppendLine(" |");
            }

            builder.AppendLine();
            builder.AppendLine("## Do Not Modify Without Formal Approval");
            builder.AppendLine();
            builder.AppendLine("- `Assets/TheCat/Art/Characters/Sprites`");
            builder.AppendLine("- starter-cat runtime sprite bindings");
            builder.AppendLine("- colored three-view source-lock paths");
            builder.AppendLine("- existing Unity `.meta` files for installed art");

            if (issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                builder.AppendLine();
                for (int i = 0; i < issues.Count; i++)
                {
                    builder.AppendLine("- [Failure] " + issues[i]);
                }
            }

            return builder.ToString();
        }

        private static string EscapeTable(string value)
        {
            return (value ?? string.Empty)
                .Replace("|", "\\|")
                .Replace("\r", " ")
                .Replace("\n", " ");
        }
    }

    public static class P0AssetSystematicProductionPlan
    {
        public const int ExpectedCoveredCheckCount = 8;
        public const string EditorMenuPath = "TheCat/P0/Write P0 Systematic Asset Production Plan";
        public const string ReportOutputPath = "design/development/asset_review/P0_SYSTEMATIC_ASSET_PRODUCTION_PLAN.md";
        public const string RecommendedWorkModeReviewExistingNonCatCandidatePacks = "ReviewExistingNonCatCandidatePacksBeforeNewImageGeneration";
        public const string StarterCatLockPolicy = "DocumentColoredThreeViewLockedNoCatBodyImportUntilActiveScreenshotApproval";

        public static P0AssetSystematicProductionPlanReport EvaluateCurrentPlan()
        {
            return Evaluate(
                P0AssetProductionNextBatchGate.EvaluateCurrentGate(),
                P0AssetProductionQueueCatalog.CreateP0Queue(),
                P0BedroomInteractableBatch54UnityPreflight.EvaluateCurrentPreflight());
        }

        public static P0AssetSystematicProductionPlanReport Evaluate(
            P0AssetProductionNextBatchGateReport nextBatchGate,
            IReadOnlyList<P0AssetProductionQueueEntry> queue)
        {
            return Evaluate(
                nextBatchGate,
                queue,
                P0BedroomInteractableBatch54UnityPreflight.EvaluateCurrentPreflight());
        }

        public static P0AssetSystematicProductionPlanReport Evaluate(
            P0AssetProductionNextBatchGateReport nextBatchGate,
            IReadOnlyList<P0AssetProductionQueueEntry> queue,
            P0BedroomInteractableBatch54UnityPreflightReport firstLanePreflight)
        {
            P0AssetSystematicProductionPlanReport report = new P0AssetSystematicProductionPlanReport();
            IReadOnlyList<P0AssetProductionQueueEntry> entries = queue ?? Array.Empty<P0AssetProductionQueueEntry>();

            int nonCatCandidateReviewCount = 0;
            int protectedCatBodyEntryCount = 0;
            int unityInstallBlockedCount = 0;
            bool allRecommendedEntriesAreNonCat = true;
            bool allRecommendedCandidatesStayOutsideAssets = true;

            for (int i = 0; i < entries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = entries[i];
                if (entry.IsUnityBlocked)
                {
                    unityInstallBlockedCount++;
                }

                if (IsStarterCatBodyProtectedEntry(entry))
                {
                    protectedCatBodyEntryCount++;
                    report.AddProtectedCatBodyEntry(entry);
                    continue;
                }

                if (IsRecommendedNonCatCandidateReviewEntry(entry))
                {
                    nonCatCandidateReviewCount++;
                    report.AddRecommendedReviewEntry(entry);
                    if (IsStarterCatBodyProtectedEntry(entry))
                    {
                        allRecommendedEntriesAreNonCat = false;
                    }

                    if (!IsDesignAssetCandidateDirectory(entry.CandidateDirectory))
                    {
                        allRecommendedCandidatesStayOutsideAssets = false;
                    }
                }
            }

            string recommendedFirstQueueId = report.RecommendedReviewEntries.Count == 0
                ? string.Empty
                : report.RecommendedReviewEntries[0].QueueId;
            bool nextBatchGateReady = nextBatchGate != null && nextBatchGate.IsReady;
            bool starterCatBodyLaneLocked = nextBatchGate != null
                && !nextBatchGate.StarterCatBodyImportAllowed
                && protectedCatBodyEntryCount >= 2;
            bool requiresUnityReviewBeforeInstall = nextBatchGate != null && nextBatchGate.RequiresUnityReviewBeforeInstall;
            bool firstLanePreflightReady = firstLanePreflight != null && firstLanePreflight.IsReadyForUnityPreflight;
            bool firstLaneFormalInstallAllowed = firstLanePreflight != null && firstLanePreflight.FormalInstallAllowed;
            int firstLaneBlockingItemCount = firstLanePreflight == null ? 0 : firstLanePreflight.BlockingItems.Count;
            bool canContinueSystematicProduction = nextBatchGateReady
                && starterCatBodyLaneLocked
                && requiresUnityReviewBeforeInstall
                && nonCatCandidateReviewCount > 0
                && allRecommendedEntriesAreNonCat
                && allRecommendedCandidatesStayOutsideAssets
                && firstLanePreflightReady
                && !firstLaneFormalInstallAllowed;

            report.SetSnapshot(
                nextBatchGateReady,
                canContinueSystematicProduction,
                starterCatBodyLaneLocked,
                requiresUnityReviewBeforeInstall,
                entries.Count,
                nonCatCandidateReviewCount,
                protectedCatBodyEntryCount,
                unityInstallBlockedCount,
                firstLanePreflightReady,
                firstLaneFormalInstallAllowed,
                firstLaneBlockingItemCount,
                RecommendedWorkModeReviewExistingNonCatCandidatePacks,
                recommendedFirstQueueId);

            Require(report, nextBatchGateReady, "Next-batch gate is green before extending asset work.", "Next-batch gate is not ready.");
            Require(report, starterCatBodyLaneLocked, "Starter-cat body entries stay locked behind document colored three-view review and active screenshots.", "Starter-cat body entries are not locked.");
            Require(report, requiresUnityReviewBeforeInstall, "Formal Unity install remains blocked until Unity review evidence exists.", "Unity install is not gated.");
            Require(report, nonCatCandidateReviewCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, "All completed Codex candidate packs are queued for non-cat Unity review.", "Non-cat candidate review queue is incomplete.");
            Require(report, recommendedFirstQueueId == P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId, "Bedroom interactables are the first recommended non-cat review lane.", "First recommended asset lane is not the bedroom interactable review.");
            Require(report, allRecommendedEntriesAreNonCat && allRecommendedCandidatesStayOutsideAssets, "Recommended review entries exclude starter-cat body lanes and keep candidates outside Assets.", "Recommended review entries include unsafe cat-body or Assets writes.");
            Require(report, firstLanePreflightReady && !firstLaneFormalInstallAllowed && firstLaneBlockingItemCount > 0, "First recommended bedroom-interactable lane has an offline Unity preflight and remains blocked from formal install.", "First recommended lane is missing a safe preflight or is prematurely installable.");
            Require(report, canContinueSystematicProduction && report.RecommendedWorkMode == RecommendedWorkModeReviewExistingNonCatCandidatePacks, "Continue by reviewing existing non-cat candidate packs before generating new images.", "Systematic asset production mode is not safe.");

            return report;
        }

        private static bool IsRecommendedNonCatCandidateReviewEntry(P0AssetProductionQueueEntry entry)
        {
            return entry != null
                && entry.IsCandidatePackCompletePendingUnityReview
                && !IsStarterCatBodyProtectedEntry(entry);
        }

        private static bool IsStarterCatBodyProtectedEntry(P0AssetProductionQueueEntry entry)
        {
            if (entry == null)
            {
                return false;
            }

            return ContainsOrdinal(entry.QueueId, "starter_cat")
                || ContainsOrdinal(entry.SubjectGroup, "starter_cats")
                || ContainsOrdinal(entry.UnityImportRoot, "Characters/Sprites")
                || ContainsOrdinal(entry.NextAction, "starter-cat")
                || ContainsToken(entry.SourceLockIds, "starter_cat")
                || ContainsToken(entry.RelatedBatchSlugs, "saiban")
                || ContainsToken(entry.RelatedBatchSlugs, "nephthys")
                || ContainsToken(entry.RelatedBatchSlugs, "suzune");
        }

        private static bool IsDesignAssetCandidateDirectory(string path)
        {
            return ContainsOrdinal(path, "design/development/asset_candidates/")
                || ContainsOrdinal(path, "design\\development\\asset_candidates\\");
        }

        private static bool ContainsToken(IReadOnlyList<string> values, string token)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (ContainsOrdinal(values[i], token))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsOrdinal(string value, string token)
        {
            return (value ?? string.Empty).IndexOf(token ?? string.Empty, StringComparison.Ordinal) >= 0;
        }

        private static void Require(
            P0AssetSystematicProductionPlanReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(failureMessage);
        }
    }
}
