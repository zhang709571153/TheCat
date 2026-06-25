using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0AssetProductionReadinessSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetProductionReadinessIssue
    {
        public P0AssetProductionReadinessIssue(P0AssetProductionReadinessSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetProductionReadinessSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetProductionReadinessReport
    {
        private readonly List<P0AssetProductionReadinessIssue> issues = new List<P0AssetProductionReadinessIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0AssetProductionReadinessIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int ReviewAssetCount { get; private set; }

        public int RuntimeBindingCount { get; private set; }

        public int ResolvedRuntimeTextureCount { get; private set; }

        public int SourceLockedEntryCount { get; private set; }

        public int StarterCatLockCount { get; private set; }

        public int StarterCatVisualChecklistCount { get; private set; }

        public int StarterCatTurnaroundConformanceSpecCount { get; private set; }

        public int StarterCatTurnaroundViewAnchorCount { get; private set; }

        public int StarterCatProductionSpecCount { get; private set; }

        public bool StarterCatSourceLockPacketReady { get; private set; }

        public bool StarterCatRuntimeCombatSpriteAuditReady { get; private set; }

        public int StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount { get; private set; }

        public bool StarterCatStrictCandidateReady { get; private set; }

        public int StarterCatStrictCandidateCount { get; private set; }

        public bool StarterCatContactSheetPresent { get; private set; }

        public P0StarterCatFormalImportState StarterCatFormalImportState { get; private set; } = P0StarterCatFormalImportState.Invalid;

        public bool StarterCatFormalImportGateValid { get; private set; }

        public bool StarterCatFormalImportAllowed { get; private set; }

        public int StarterCatFormalReviewNoteCount { get; private set; }

        public int StarterCatActiveScreenshotCount { get; private set; }

        public bool AssetProductionQueueReady { get; private set; }

        public int AssetProductionQueueCount { get; private set; }

        public int AssetProductionQueueCodexRunnableCount { get; private set; }

        public int AssetProductionQueueCandidatePackCompletePendingUnityReviewCount { get; private set; }

        public int AssetProductionQueueUnityBlockedCount { get; private set; }

        public bool RuntimeVisualContactSheetPresent { get; private set; }

        public int FailureCount => Count(P0AssetProductionReadinessSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0AssetProductionReadiness.ExpectedCoveredCheckCount;

        public void SetCounts(
            int reviewAssetCount,
            int runtimeBindingCount,
            int resolvedRuntimeTextureCount,
            int sourceLockedEntryCount,
            int starterCatLockCount,
            int starterCatVisualChecklistCount,
            int starterCatTurnaroundConformanceSpecCount,
            int starterCatTurnaroundViewAnchorCount,
            int starterCatProductionSpecCount,
            bool starterCatSourceLockPacketReady,
            bool starterCatContactSheetPresent,
            bool runtimeVisualContactSheetPresent)
        {
            ReviewAssetCount = reviewAssetCount;
            RuntimeBindingCount = runtimeBindingCount;
            ResolvedRuntimeTextureCount = resolvedRuntimeTextureCount;
            SourceLockedEntryCount = sourceLockedEntryCount;
            StarterCatLockCount = starterCatLockCount;
            StarterCatVisualChecklistCount = starterCatVisualChecklistCount;
            StarterCatTurnaroundConformanceSpecCount = starterCatTurnaroundConformanceSpecCount;
            StarterCatTurnaroundViewAnchorCount = starterCatTurnaroundViewAnchorCount;
            StarterCatProductionSpecCount = starterCatProductionSpecCount;
            StarterCatSourceLockPacketReady = starterCatSourceLockPacketReady;
            StarterCatContactSheetPresent = starterCatContactSheetPresent;
            RuntimeVisualContactSheetPresent = runtimeVisualContactSheetPresent;
        }

        public void SetStarterCatFormalImport(P0StarterCatFormalImportReadinessReport formalImport)
        {
            if (formalImport == null)
            {
                StarterCatFormalImportState = P0StarterCatFormalImportState.Invalid;
                StarterCatFormalImportGateValid = false;
                StarterCatFormalImportAllowed = false;
                StarterCatFormalReviewNoteCount = 0;
                StarterCatActiveScreenshotCount = 0;
                return;
            }

            StarterCatFormalImportState = formalImport.State;
            StarterCatFormalImportGateValid = formalImport.IsGateValid;
            StarterCatFormalImportAllowed = formalImport.IsImportAllowed;
            StarterCatFormalReviewNoteCount = formalImport.ReviewNoteCount;
            StarterCatActiveScreenshotCount = formalImport.ActiveCatScreenshotCount;
        }

        public void SetStarterCatStrictCandidate(P0StarterCatStrictCandidateEvidenceReport strictCandidates)
        {
            StarterCatStrictCandidateReady = strictCandidates != null && strictCandidates.IsReady;
            StarterCatStrictCandidateCount = strictCandidates == null ? 0 : strictCandidates.CandidateCount;
        }

        public void SetStarterCatRuntimeCombatSpriteAudit(P0StarterCatRuntimeCombatSpriteAuditReport audit)
        {
            StarterCatRuntimeCombatSpriteAuditReady = audit != null && audit.IsReady;
            StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount = audit == null ? 0 : audit.RuntimeSpriteCount;
        }

        public void SetAssetProductionQueue(P0AssetProductionQueueCoverageReport queue)
        {
            AssetProductionQueueReady = queue != null && queue.IsReady;
            AssetProductionQueueCount = queue == null ? 0 : queue.QueueCount;
            AssetProductionQueueCodexRunnableCount = queue == null ? 0 : queue.CodexRunnableCount;
            AssetProductionQueueCandidatePackCompletePendingUnityReviewCount = queue == null ? 0 : queue.CandidatePackCompletePendingUnityReviewCount;
            AssetProductionQueueUnityBlockedCount = queue == null ? 0 : queue.UnityBlockedCount;
        }

        public void AddIssue(P0AssetProductionReadinessSeverity severity, string message)
        {
            issues.Add(new P0AssetProductionReadinessIssue(severity, message));
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
                ? "P0 offline asset production readiness passed for " + ReviewAssetCount + " review asset(s), " + RuntimeBindingCount + " runtime binding(s), and " + StarterCatLockCount + " starter cat lock(s)."
                : "P0 offline asset production readiness has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Review assets: " + ReviewAssetCount,
                "Runtime bindings: " + RuntimeBindingCount,
                "Resolved runtime textures: " + ResolvedRuntimeTextureCount,
                "Source-locked entries: " + SourceLockedEntryCount,
                "Starter cat locks: " + StarterCatLockCount,
                "Starter cat visual checklist cats: " + StarterCatVisualChecklistCount,
                "Starter cat turnaround conformance spec cats: " + StarterCatTurnaroundConformanceSpecCount,
                "Starter cat turnaround view anchors: " + StarterCatTurnaroundViewAnchorCount,
                "Starter cat asset-production spec cats: " + StarterCatProductionSpecCount,
                "Starter cat source-lock packet ready: " + (StarterCatSourceLockPacketReady ? "yes" : "no"),
                "Starter cat runtime combat sprite audit ready: " + (StarterCatRuntimeCombatSpriteAuditReady ? "yes" : "no"),
                "Starter cat runtime combat sprites: " + StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount,
                "Starter cat strict candidates ready: " + (StarterCatStrictCandidateReady ? "yes" : "no"),
                "Starter cat strict candidates: " + StarterCatStrictCandidateCount + "/" + P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount,
                "Starter cat contact sheet present: " + (StarterCatContactSheetPresent ? "yes" : "no"),
                "Starter cat formal import gate valid: " + (StarterCatFormalImportGateValid ? "yes" : "no"),
                "Starter cat formal import state: " + StarterCatFormalImportState,
                "Starter cat formal import allowed: " + (StarterCatFormalImportAllowed ? "yes" : "no"),
                "Starter cat formal review notes: " + StarterCatFormalReviewNoteCount,
                "Starter cat active screenshots: " + StarterCatActiveScreenshotCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount,
                "Asset production queue ready: " + (AssetProductionQueueReady ? "yes" : "no"),
                "Asset production queue items: " + AssetProductionQueueCount,
                "Asset production queue Codex-runnable items: " + AssetProductionQueueCodexRunnableCount,
                "Asset production queue completed candidate packs pending Unity review: " + AssetProductionQueueCandidatePackCompletePendingUnityReviewCount,
                "Asset production queue Unity-blocked items: " + AssetProductionQueueUnityBlockedCount,
                "Runtime visual contact sheet present: " + (RuntimeVisualContactSheetPresent ? "yes" : "no")
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

        private int Count(P0AssetProductionReadinessSeverity severity)
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

    public enum P0AssetProductionNextBatchGateSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetProductionNextBatchGateIssue
    {
        public P0AssetProductionNextBatchGateIssue(P0AssetProductionNextBatchGateSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetProductionNextBatchGateSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetProductionNextBatchGateReport
    {
        private readonly List<P0AssetProductionNextBatchGateIssue> issues = new List<P0AssetProductionNextBatchGateIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0AssetProductionQueueEntry> pendingUnityReviewEntries = new List<P0AssetProductionQueueEntry>();

        public IReadOnlyList<P0AssetProductionNextBatchGateIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0AssetProductionQueueEntry> PendingUnityReviewEntries => pendingUnityReviewEntries.AsReadOnly();

        public bool OfflineReadinessReady { get; private set; }

        public bool AssetProductionQueueReady { get; private set; }

        public bool UnityValidationChecklistReady { get; private set; }

        public bool StarterCatStrictIdentityReady { get; private set; }

        public bool StarterCatFormalImportGateValid { get; private set; }

        public bool StarterCatFormalImportAllowed { get; private set; }

        public P0StarterCatFormalImportState StarterCatFormalImportState { get; private set; } = P0StarterCatFormalImportState.Invalid;

        public int QueueItemCount { get; private set; }

        public int CodexRunnableCount { get; private set; }

        public int CandidatePacksPendingUnityReviewCount { get; private set; }

        public int UnityBlockedCount { get; private set; }

        public string RecommendedNextLane { get; private set; } = string.Empty;

        public bool CanStartNewCodexCandidateBatch { get; private set; }

        public bool StarterCatBodyImportAllowed { get; private set; }

        public bool RequiresUnityReviewBeforeInstall { get; private set; }

        public int FailureCount => Count(P0AssetProductionNextBatchGateSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0AssetProductionNextBatchGate.ExpectedCoveredCheckCount;

        public void SetSnapshot(
            bool offlineReadinessReady,
            bool assetProductionQueueReady,
            bool unityValidationChecklistReady,
            bool starterCatStrictIdentityReady,
            bool starterCatFormalImportGateValid,
            bool starterCatFormalImportAllowed,
            P0StarterCatFormalImportState starterCatFormalImportState,
            int queueItemCount,
            int codexRunnableCount,
            int candidatePacksPendingUnityReviewCount,
            int unityBlockedCount,
            string recommendedNextLane,
            bool canStartNewCodexCandidateBatch,
            bool starterCatBodyImportAllowed,
            bool requiresUnityReviewBeforeInstall,
            IReadOnlyList<P0AssetProductionQueueEntry> queueEntries)
        {
            OfflineReadinessReady = offlineReadinessReady;
            AssetProductionQueueReady = assetProductionQueueReady;
            UnityValidationChecklistReady = unityValidationChecklistReady;
            StarterCatStrictIdentityReady = starterCatStrictIdentityReady;
            StarterCatFormalImportGateValid = starterCatFormalImportGateValid;
            StarterCatFormalImportAllowed = starterCatFormalImportAllowed;
            StarterCatFormalImportState = starterCatFormalImportState;
            QueueItemCount = queueItemCount;
            CodexRunnableCount = codexRunnableCount;
            CandidatePacksPendingUnityReviewCount = candidatePacksPendingUnityReviewCount;
            UnityBlockedCount = unityBlockedCount;
            RecommendedNextLane = recommendedNextLane ?? string.Empty;
            CanStartNewCodexCandidateBatch = canStartNewCodexCandidateBatch;
            StarterCatBodyImportAllowed = starterCatBodyImportAllowed;
            RequiresUnityReviewBeforeInstall = requiresUnityReviewBeforeInstall;

            pendingUnityReviewEntries.Clear();
            if (queueEntries == null)
            {
                return;
            }

            for (int i = 0; i < queueEntries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queueEntries[i];
                if (entry.IsCandidatePackCompletePendingUnityReview || entry.IsUnityBlocked)
                {
                    pendingUnityReviewEntries.Add(entry);
                }
            }
        }

        public void AddIssue(P0AssetProductionNextBatchGateSeverity severity, string message)
        {
            issues.Add(new P0AssetProductionNextBatchGateIssue(severity, message));
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
                return "P0 asset production next-batch gate has " + FailureCount + " failure(s).";
            }

            return "P0 asset production next-batch gate allows a new scoped Codex candidate/spec batch while keeping starter-cat body import blocked.";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Next Codex candidate batch allowed: " + (CanStartNewCodexCandidateBatch ? "yes" : "no"),
                "Allowed lane: " + RecommendedNextLane,
                "Starter-cat body import allowed: " + (StarterCatBodyImportAllowed ? "yes" : "no"),
                "Unity review required before install: " + (RequiresUnityReviewBeforeInstall ? "yes" : "no"),
                "Offline readiness ready: " + (OfflineReadinessReady ? "yes" : "no"),
                "Asset production queue ready: " + (AssetProductionQueueReady ? "yes" : "no"),
                "Unity validation checklist ready: " + (UnityValidationChecklistReady ? "yes" : "no"),
                "Starter-cat strict identity ready: " + (StarterCatStrictIdentityReady ? "yes" : "no"),
                "Starter-cat formal import gate valid: " + (StarterCatFormalImportGateValid ? "yes" : "no"),
                "Starter-cat formal import state: " + StarterCatFormalImportState,
                "Starter-cat formal import allowed: " + (StarterCatFormalImportAllowed ? "yes" : "no"),
                "Queue items: " + QueueItemCount,
                "Codex-runnable queue items: " + CodexRunnableCount,
                "Candidate packs pending Unity review: " + CandidatePacksPendingUnityReviewCount,
                "Unity-blocked queue items: " + UnityBlockedCount,
                "Pending Unity review entries: " + pendingUnityReviewEntries.Count
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
            builder.AppendLine("# P0 Asset Production Next Batch Gate");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Decision");
            builder.AppendLine();
            builder.AppendLine("- Next Codex candidate batch allowed: " + (CanStartNewCodexCandidateBatch ? "yes" : "no"));
            builder.AppendLine("- Allowed lane: `" + RecommendedNextLane + "`");
            builder.AppendLine("- Starter-cat body import allowed: " + (StarterCatBodyImportAllowed ? "yes" : "no"));
            builder.AppendLine("- Unity review required before install: " + (RequiresUnityReviewBeforeInstall ? "yes" : "no"));
            builder.AppendLine("- Cat-body policy: `" + P0AssetProductionNextBatchGate.StarterCatBodyPolicy + "`");
            builder.AppendLine();
            builder.AppendLine("## Current Gates");
            builder.AppendLine();
            builder.AppendLine("- Offline readiness ready: " + (OfflineReadinessReady ? "yes" : "no"));
            builder.AppendLine("- Asset production queue ready: " + (AssetProductionQueueReady ? "yes" : "no"));
            builder.AppendLine("- Unity validation checklist ready: " + (UnityValidationChecklistReady ? "yes" : "no"));
            builder.AppendLine("- Starter-cat strict identity ready: " + (StarterCatStrictIdentityReady ? "yes" : "no"));
            builder.AppendLine("- Starter-cat formal import gate valid: " + (StarterCatFormalImportGateValid ? "yes" : "no"));
            builder.AppendLine("- Starter-cat formal import state: `" + StarterCatFormalImportState + "`");
            builder.AppendLine("- Starter-cat formal import allowed: " + (StarterCatFormalImportAllowed ? "yes" : "no"));
            builder.AppendLine();
            builder.AppendLine("## Queue Snapshot");
            builder.AppendLine();
            builder.AppendLine("- Queue items: " + QueueItemCount);
            builder.AppendLine("- Codex-runnable queue items: " + CodexRunnableCount);
            builder.AppendLine("- Candidate packs pending Unity review: " + CandidatePacksPendingUnityReviewCount);
            builder.AppendLine("- Unity-blocked queue items: " + UnityBlockedCount);
            builder.AppendLine();
            builder.AppendLine("## Pending Unity Review");
            builder.AppendLine();
            builder.AppendLine("| priority | queue | phase | state | next action |");
            builder.AppendLine("| --- | --- | --- | --- | --- |");

            for (int i = 0; i < pendingUnityReviewEntries.Count; i++)
            {
                P0AssetProductionQueueEntry entry = pendingUnityReviewEntries[i];
                builder.Append("| ");
                builder.Append(entry.Priority);
                builder.Append(" | ");
                builder.Append(EscapeTable(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(entry.Phase);
                builder.Append(" | ");
                builder.Append(entry.State);
                builder.Append(" | ");
                builder.Append(EscapeTable(entry.NextAction));
                builder.AppendLine(" |");
            }

            if (issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                builder.AppendLine();
                for (int i = 0; i < issues.Count; i++)
                {
                    builder.AppendLine("- [" + issues[i].Severity + "] " + issues[i].Message);
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

        private int Count(P0AssetProductionNextBatchGateSeverity severity)
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

    public static class P0AssetProductionNextBatchGate
    {
        public const int ExpectedCoveredCheckCount = 8;
        public const string RecommendedLaneNewScopedCandidateOrSpecBatchOutsideAssets = "NewScopedCandidateOrSpecBatchOutsideAssets";
        public const string StarterCatBodyPolicy = "CatBodyGenerationReviewOnlyUntilActiveScreenshots";

        public static P0AssetProductionNextBatchGateReport EvaluateCurrentGate()
        {
            P0AssetUnityValidationChecklistReport unityValidationChecklist = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
            return Evaluate(
                P0AssetProductionReadiness.EvaluateP0OfflineReadiness(),
                P0AssetProductionQueueCoverage.EvaluateP0Queue(),
                unityValidationChecklist,
                P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates(),
                P0StarterCatFormalImportReadiness.EvaluateCurrentGate());
        }

        public static P0AssetProductionNextBatchGateReport Evaluate(
            P0AssetProductionReadinessReport offlineReadiness,
            P0AssetProductionQueueCoverageReport assetProductionQueue,
            P0AssetUnityValidationChecklistReport unityValidationChecklist,
            P0StarterCatStrictCandidateEvidenceReport starterCatStrictCandidates,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            P0AssetProductionNextBatchGateReport report = new P0AssetProductionNextBatchGateReport();

            bool offlineReady = offlineReadiness != null && offlineReadiness.IsReady;
            bool queueReady = assetProductionQueue != null && assetProductionQueue.IsReady;
            bool checklistReady = unityValidationChecklist != null && unityValidationChecklist.IsReadyForUnityValidation;
            bool strictIdentityReady = starterCatStrictCandidates != null
                && starterCatStrictCandidates.IsReady
                && starterCatStrictCandidates.SourceTurnaroundExactPathCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && starterCatStrictCandidates.Batch47SpecManifestLockCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && starterCatStrictCandidates.Batch47SpecJsonIdentityLockCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount;
            bool formalGateValid = starterCatFormalImport != null && starterCatFormalImport.IsGateValid;
            bool formalImportAllowed = starterCatFormalImport != null && starterCatFormalImport.IsImportAllowed;
            bool formalImportBlocked = formalGateValid
                && starterCatFormalImport.State == P0StarterCatFormalImportState.Blocked
                && !formalImportAllowed;
            bool noCurrentCodexRunnableItems = assetProductionQueue != null
                && assetProductionQueue.CodexRunnableCount == P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount;
            bool queueCountsMatchCurrentGate = assetProductionQueue != null
                && assetProductionQueue.QueueCount == P0AssetProductionQueueCatalog.ExpectedP0QueueCount
                && assetProductionQueue.CandidatePackCompletePendingUnityReviewCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount
                && assetProductionQueue.UnityBlockedCount == P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount;
            bool requiresUnityReviewBeforeInstall = assetProductionQueue != null
                && (assetProductionQueue.CandidatePackCompletePendingUnityReviewCount > 0
                    || assetProductionQueue.UnityBlockedCount > 0
                    || !formalImportAllowed);
            bool canStartNewCodexCandidateBatch = offlineReady
                && queueReady
                && checklistReady
                && strictIdentityReady
                && formalImportBlocked
                && noCurrentCodexRunnableItems
                && queueCountsMatchCurrentGate
                && requiresUnityReviewBeforeInstall;

            report.SetSnapshot(
                offlineReady,
                queueReady,
                checklistReady,
                strictIdentityReady,
                formalGateValid,
                formalImportAllowed,
                starterCatFormalImport == null ? P0StarterCatFormalImportState.Invalid : starterCatFormalImport.State,
                assetProductionQueue == null ? 0 : assetProductionQueue.QueueCount,
                assetProductionQueue == null ? 0 : assetProductionQueue.CodexRunnableCount,
                assetProductionQueue == null ? 0 : assetProductionQueue.CandidatePackCompletePendingUnityReviewCount,
                assetProductionQueue == null ? 0 : assetProductionQueue.UnityBlockedCount,
                RecommendedLaneNewScopedCandidateOrSpecBatchOutsideAssets,
                canStartNewCodexCandidateBatch,
                formalImportAllowed,
                requiresUnityReviewBeforeInstall,
                unityValidationChecklist == null ? null : unityValidationChecklist.Entries);

            Require(
                report,
                offlineReady,
                "P0 offline asset readiness remains green before starting another asset batch.",
                "P0 offline asset readiness is not ready.");

            Require(
                report,
                queueReady,
                "Asset production queue is complete and separates Codex candidate work from Unity validation.",
                "Asset production queue is not ready.");

            Require(
                report,
                checklistReady,
                "Unity validation checklist covers every pending queue item before formal install.",
                "Unity validation checklist is not ready.");

            Require(
                report,
                strictIdentityReady,
                "Starter-cat strict candidates pin exact colored-turnaround source paths and Batch 47 identity locks.",
                "Starter-cat strict candidates are missing exact colored-turnaround identity locks.");

            Require(
                report,
                formalImportBlocked,
                "Starter-cat body import remains blocked until active-cat screenshot review explicitly approves it.",
                "Starter-cat body import must remain blocked until active-cat screenshots and explicit approvals are real.");

            Require(
                report,
                noCurrentCodexRunnableItems,
                "Current queue has no open Codex-runnable rows; any next batch must be newly scoped.",
                "Current queue still has Codex-runnable rows; finish or reclassify them before opening a new batch.");

            Require(
                report,
                queueCountsMatchCurrentGate,
                "Current queue counts match the expected candidate-pending and Unity-blocked split.",
                "Current queue counts do not match the expected candidate-pending and Unity-blocked split.");

            Require(
                report,
                canStartNewCodexCandidateBatch
                    && report.RecommendedNextLane == RecommendedLaneNewScopedCandidateOrSpecBatchOutsideAssets
                    && report.RequiresUnityReviewBeforeInstall
                    && !report.StarterCatBodyImportAllowed,
                "Next asset work may start as a scoped Codex candidate/spec batch outside Assets; formal install remains Unity-gated.",
                "Next asset work is not safe to start as a scoped outside-Assets Codex batch.");

            return report;
        }

        private static void Require(
            P0AssetProductionNextBatchGateReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetProductionNextBatchGateSeverity.Failure, failureMessage);
        }
    }

    public static class P0AssetProductionReadiness
    {
        public const int ExpectedCoveredCheckCount = 19;
        public const string StarterCatTurnaroundContactSheetPath = "design/development/asset_review/p0_starter_cat_turnaround_review_2026-06-14.png";
        public const string RuntimeVisualContactSheetPath = "design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.png";

        public static P0AssetProductionReadinessReport EvaluateP0OfflineReadiness()
        {
            return Evaluate(
                P0AssetManifestCoverage.EvaluateP0Manifest(),
                P0AssetGenerationBatchCoverage.EvaluateP0Batches(),
                P0AssetImportReadiness.EvaluateP0Manifest(),
                P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest(),
                P0RuntimeVisualBindingCoverage.EvaluateP0Bindings(),
                P0AssetReviewPacket.EvaluateP0Packet(),
                P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks(),
                P0StarterCatVisualConsistencyChecklist.EvaluateP0Checklist(),
                P0StarterCatTurnaroundConformanceSpec.EvaluateP0Spec(),
                P0StarterCatAssetProductionSpec.EvaluateP0Spec(),
                P0StarterCatProductionPromptReadiness.EvaluateCurrentPrompts(),
                P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket(),
                P0StarterCatRuntimeCombatSpriteAuditEvidence.EvaluateCurrentAudit(),
                P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates(),
                P0StarterCatFormalImportReadiness.EvaluateCurrentGate(),
                P0AssetProductionQueueCoverage.EvaluateP0Queue(),
                P0HardReferenceSourceLocks.EvaluateP0Locks(),
                DefaultFileExists);
        }

        public static P0AssetProductionReadinessReport Evaluate(
            P0AssetManifestCoverageReport manifest,
            P0AssetGenerationBatchCoverageReport assetBatches,
            P0AssetImportReadinessReport importReadiness,
            P0AssetMetaImportSettingsReport metaReadiness,
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings,
            P0AssetReviewPacketReport assetReviewPacket,
            P0StarterCatTurnaroundSourceLockReport starterCatSourceLocks,
            P0StarterCatVisualConsistencyReport starterCatVisualConsistency,
            P0StarterCatTurnaroundConformanceSpecReport starterCatTurnaroundConformanceSpec,
            P0StarterCatAssetProductionSpecReport starterCatAssetProductionSpec,
            P0StarterCatProductionPromptReadinessReport starterCatProductionPromptReadiness,
            P0StarterCatSourceLockPacketReport starterCatSourceLockPacket,
            P0StarterCatRuntimeCombatSpriteAuditReport starterCatRuntimeCombatSpriteAudit,
            P0StarterCatStrictCandidateEvidenceReport starterCatStrictCandidates,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport,
            P0AssetProductionQueueCoverageReport assetProductionQueue,
            P0HardReferenceSourceLockReport hardReferenceSourceLocks,
            Func<string, bool> fileExists)
        {
            P0AssetProductionReadinessReport report = new P0AssetProductionReadinessReport();
            Func<string, bool> exists = fileExists ?? File.Exists;
            bool contactSheetPresent = exists(StarterCatTurnaroundContactSheetPath);
            bool runtimeVisualContactSheetPresent = exists(RuntimeVisualContactSheetPath);

            report.SetCounts(
                assetReviewPacket == null ? 0 : assetReviewPacket.ReviewAssetCount,
                runtimeVisualBindings == null ? 0 : runtimeVisualBindings.BindingCount,
                runtimeVisualBindings == null ? 0 : runtimeVisualBindings.ResolvedTextureCount,
                assetReviewPacket == null ? 0 : assetReviewPacket.SourceLockedEntryCount,
                starterCatSourceLocks == null ? 0 : starterCatSourceLocks.LockCount,
                starterCatVisualConsistency == null ? 0 : starterCatVisualConsistency.ChecklistCount,
                starterCatTurnaroundConformanceSpec == null ? 0 : starterCatTurnaroundConformanceSpec.SpecCount,
                starterCatTurnaroundConformanceSpec == null ? 0 : starterCatTurnaroundConformanceSpec.FrontViewAnchorCount + starterCatTurnaroundConformanceSpec.SideViewAnchorCount + starterCatTurnaroundConformanceSpec.BackViewAnchorCount,
                starterCatAssetProductionSpec == null ? 0 : starterCatAssetProductionSpec.SpecCount,
                starterCatSourceLockPacket != null && starterCatSourceLockPacket.IsReady,
                contactSheetPresent,
                runtimeVisualContactSheetPresent);
            report.SetStarterCatStrictCandidate(starterCatStrictCandidates);
            report.SetStarterCatRuntimeCombatSpriteAudit(starterCatRuntimeCombatSpriteAudit);
            report.SetStarterCatFormalImport(starterCatFormalImport);
            report.SetAssetProductionQueue(assetProductionQueue);

            Require(
                report,
                manifest != null && manifest.IsComplete,
                "Asset manifest coverage is complete for the current P0 asset plan.",
                "Asset manifest coverage is incomplete or missing.");

            Require(
                report,
                assetBatches != null && assetBatches.IsComplete,
                "Asset generation batches are defined and ordered for the current P0 plan.",
                "Asset generation batch coverage is incomplete or missing.");

            Require(
                report,
                importReadiness != null
                    && importReadiness.IsReady
                    && importReadiness.WorkspaceFileRequiredCount == importReadiness.ExistingWorkspaceFileCount
                    && importReadiness.PlannedAssetCount == 0,
                "All generated/imported P0 asset files exist and no planned manifest rows remain.",
                "P0 asset import readiness is incomplete, missing files, or still contains planned rows.");

            Require(
                report,
                metaReadiness != null
                    && metaReadiness.IsReady
                    && metaReadiness.RequiredMetaCount == metaReadiness.SettingsMatchedCount,
                "All required P0 asset meta files have import settings matched.",
                "P0 asset meta import settings are incomplete or mismatched.");

            Require(
                report,
                runtimeVisualBindings != null
                    && runtimeVisualBindings.IsComplete
                    && runtimeVisualBindings.BindingCount == P0VisualAssetCatalog.P0RuntimeVisualBindingCount
                    && runtimeVisualBindings.ResolvedTextureCount == P0VisualAssetCatalog.P0RuntimeVisualBindingCount,
                "The " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " P0 runtime visual bindings are complete and resolve to textures.",
                "The " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " P0 runtime visual bindings are incomplete or unresolved.");

            Require(
                report,
                assetReviewPacket != null
                    && assetReviewPacket.IsReady
                    && assetReviewPacket.ReviewAssetCount == P0AssetManifestCatalog.P0ManifestAssetCount
                    && assetReviewPacket.RuntimeBoundEntryCount == P0VisualAssetCatalog.P0RuntimeVisualBindingCount
                    && assetReviewPacket.StarterCatEntryCount == P0AssetReviewPacket.ExpectedStarterCatReviewEntryCount,
                "The asset review packet covers " + P0AssetManifestCatalog.P0ManifestAssetCount + " review assets, " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " runtime bindings, and " + P0AssetReviewPacket.ExpectedStarterCatReviewEntryCount + " starter cat review entries.",
                "The asset review packet is incomplete or has stale counts.");

            Require(
                report,
                starterCatSourceLocks != null
                    && starterCatSourceLocks.IsReady
                    && starterCatSourceLocks.LockCount == 3
                    && starterCatSourceLocks.SourceHashMatchedCount == 3
                    && starterCatSourceLocks.SpriteHashMatchedCount == 3
                    && starterCatSourceLocks.ManifestMatchedCount == 3,
                "Starter cat source locks pin Saiban, Nephthys, and Suzune to colored turnarounds.",
                "Starter cat colored-turnaround source locks are incomplete or stale.");

            Require(
                report,
                starterCatVisualConsistency != null
                    && starterCatVisualConsistency.IsReady
                    && starterCatVisualConsistency.ChecklistCount == 3
                    && starterCatVisualConsistency.RequiredTraitCount >= 15
                    && starterCatVisualConsistency.ScreenshotPlanMatchedCount == 3
                    && starterCatVisualConsistency.SourceLockMatchedCount == 3,
                "Starter cat visual consistency checklist binds three active-cat screenshots to colored-turnaround traits.",
                "Starter cat visual consistency checklist is incomplete or stale.");

            Require(
                report,
                starterCatTurnaroundConformanceSpec != null
                    && starterCatTurnaroundConformanceSpec.IsReady
                    && starterCatTurnaroundConformanceSpec.SpecCount == 3
                    && starterCatTurnaroundConformanceSpec.SourceLockMatchedCount == 3
                    && starterCatTurnaroundConformanceSpec.ExistingSourceFileCount == 3
                    && starterCatTurnaroundConformanceSpec.FrontViewAnchorCount >= 9
                    && starterCatTurnaroundConformanceSpec.SideViewAnchorCount >= 9
                    && starterCatTurnaroundConformanceSpec.BackViewAnchorCount >= 9
                    && starterCatTurnaroundConformanceSpec.PaletteAnchorCount >= 9
                    && starterCatTurnaroundConformanceSpec.PropAndCostumeAnchorCount >= 9
                    && starterCatTurnaroundConformanceSpec.ProhibitedDriftRuleCount >= 12,
                "Starter cat turnaround conformance spec pins front, side, back, palette, prop, and drift anchors to the colored three-view turnarounds.",
                "Starter cat turnaround conformance spec is incomplete or stale.");

            Require(
                report,
                starterCatAssetProductionSpec != null
                    && starterCatAssetProductionSpec.IsReady
                    && starterCatAssetProductionSpec.SpecCount == 3
                    && starterCatAssetProductionSpec.SourceLockMatchedCount == 3
                    && starterCatAssetProductionSpec.RequiredEvidenceCount >= 24
                    && starterCatAssetProductionSpec.PromptClauseCount >= 21
                    && starterCatAssetProductionSpec.RejectionRuleCount >= 12,
                "Starter cat asset-production specs bind allowed derivatives, required evidence, prompt clauses, and rejection rules to colored turnarounds.",
                "Starter cat asset-production specs are incomplete or stale.");

            Require(
                report,
                starterCatProductionPromptReadiness != null
                    && starterCatProductionPromptReadiness.IsReady
                    && starterCatProductionPromptReadiness.PromptCount == P0StarterCatProductionPromptReadiness.ExpectedPromptCount
                    && starterCatProductionPromptReadiness.MojibakePathMentionCount == 0
                    && starterCatProductionPromptReadiness.SourceTurnaroundPathMentionCount >= P0StarterCatProductionPromptReadiness.ExpectedPromptCount * 3,
                "Starter cat production prompts pin real colored-turnaround source paths and block formal import until active screenshots pass.",
                "Starter cat production prompt readiness is incomplete or stale.");

            Require(
                report,
                starterCatSourceLockPacket != null
                    && starterCatSourceLockPacket.IsReady
                    && starterCatSourceLockPacket.CatEntryCount == 3
                    && starterCatSourceLockPacket.SourceHashMentionCount == 3
                    && starterCatSourceLockPacket.SpriteHashMentionCount == 3,
                "Starter cat source-lock packet records turnaround hashes, locked sprite hashes, screenshots, and candidate review sheets.",
                "Starter cat source-lock packet is incomplete or stale.");

            Require(
                report,
                starterCatRuntimeCombatSpriteAudit != null
                    && starterCatRuntimeCombatSpriteAudit.IsReady
                    && starterCatRuntimeCombatSpriteAudit.RuntimeSpriteCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount
                    && starterCatRuntimeCombatSpriteAudit.SourceTurnaroundPathMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount
                    && starterCatRuntimeCombatSpriteAudit.RuntimeBindingMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeBindingMentionCount,
                "Starter cat runtime combat sprite audit ties current runtime-bound sprites to exact colored-turnaround source paths and runtime binding ids.",
                "Starter cat runtime combat sprite audit is incomplete or stale.");

            Require(
                report,
                starterCatStrictCandidates != null
                    && starterCatStrictCandidates.IsReady
                    && starterCatStrictCandidates.CandidateCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount,
                "Starter cat strict candidate evidence records Batch 49/50/51 candidates and blocks import until active-cat screenshots pass.",
                "Starter cat strict candidate evidence is incomplete or stale.");

            Require(
                report,
                starterCatFormalImport != null
                    && starterCatFormalImport.IsGateValid
                    && starterCatFormalImport.ReviewNoteCount == P0StarterCatFormalImportReadiness.ExpectedStarterCatCount
                    && (starterCatFormalImport.State == P0StarterCatFormalImportState.Blocked || starterCatFormalImport.IsImportAllowed),
                "Starter cat formal import gate has an explicit blocked-or-approved decision tied to review notes and active screenshots.",
                "Starter cat formal import gate is invalid or missing.");

            Require(
                report,
                assetProductionQueue != null
                    && assetProductionQueue.IsReady
                    && assetProductionQueue.QueueCount == P0AssetProductionQueueCatalog.ExpectedP0QueueCount
                    && assetProductionQueue.CodexRunnableCount == P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount
                    && assetProductionQueue.CandidatePackCompletePendingUnityReviewCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount
                    && assetProductionQueue.UnityBlockedCount == P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount,
                "Asset production queue separates Codex candidate production from Unity validation and formal install work.",
                "Asset production queue is incomplete or stale.");

            Require(
                report,
                hardReferenceSourceLocks != null && hardReferenceSourceLocks.IsReady,
                "Hard reference source locks are ready for source-sensitive P0 assets.",
                "Hard reference source locks are incomplete or missing.");

            Require(
                report,
                contactSheetPresent,
                "Starter cat turnaround review contact sheet is present for human visual comparison.",
                "Starter cat turnaround review contact sheet is missing: " + StarterCatTurnaroundContactSheetPath + ".");

            Require(
                report,
                runtimeVisualContactSheetPresent,
                "Runtime visual contact sheet is present for " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + "-slot offline asset review.",
                "Runtime visual contact sheet is missing: " + RuntimeVisualContactSheetPath + ".");

            return report;
        }

        private static void Require(
            P0AssetProductionReadinessReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetProductionReadinessSeverity.Failure, failureMessage);
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
    }
}
