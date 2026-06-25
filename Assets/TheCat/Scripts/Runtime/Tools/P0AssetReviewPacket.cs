using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0AssetReviewPacketSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetReviewPacketIssue
    {
        public P0AssetReviewPacketIssue(P0AssetReviewPacketSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetReviewPacketSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetReviewPacketEntry
    {
        public P0AssetReviewPacketEntry(
            string assetId,
            string subjectId,
            string assetType,
            string unityImportPath,
            IReadOnlyList<string> sourceLockIds,
            IReadOnlyList<string> sourcePaths,
            IReadOnlyList<string> runtimeBindingIds,
            string consistencyNotes)
        {
            AssetId = assetId ?? string.Empty;
            SubjectId = subjectId ?? string.Empty;
            AssetType = assetType ?? string.Empty;
            UnityImportPath = unityImportPath ?? string.Empty;
            SourceLockIds = sourceLockIds == null
                ? Array.Empty<string>()
                : new List<string>(sourceLockIds).AsReadOnly();
            SourcePaths = sourcePaths == null
                ? Array.Empty<string>()
                : new List<string>(sourcePaths).AsReadOnly();
            RuntimeBindingIds = runtimeBindingIds == null
                ? Array.Empty<string>()
                : new List<string>(runtimeBindingIds).AsReadOnly();
            ConsistencyNotes = consistencyNotes ?? string.Empty;
        }

        public string AssetId { get; }

        public string SubjectId { get; }

        public string AssetType { get; }

        public string UnityImportPath { get; }

        public IReadOnlyList<string> SourceLockIds { get; }

        public IReadOnlyList<string> SourcePaths { get; }

        public IReadOnlyList<string> RuntimeBindingIds { get; }

        public string ConsistencyNotes { get; }

        public bool IsStarterCat => IsStarterCatId(SubjectId);

        public bool IsSourceLocked => SourceLockIds.Count > 0;

        public bool IsRuntimeBound => RuntimeBindingIds.Count > 0;

        private static bool IsStarterCatId(string subjectId)
        {
            return subjectId == P0PrototypeCatalog.SaibanId
                || subjectId == P0PrototypeCatalog.NephthysId
                || subjectId == P0PrototypeCatalog.SuzuneId;
        }
    }

    public sealed class P0AssetReviewPacketReport
    {
        private readonly List<P0AssetReviewPacketIssue> issues = new List<P0AssetReviewPacketIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0AssetReviewPacketEntry> entries = new List<P0AssetReviewPacketEntry>();

        public IReadOnlyList<P0AssetReviewPacketIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0AssetReviewPacketEntry> Entries => entries.AsReadOnly();

        public int ReviewAssetCount { get; private set; }

        public int ExistingWorkspaceFileCount { get; private set; }

        public int SourceLockedEntryCount { get; private set; }

        public int StarterCatEntryCount { get; private set; }

        public int StarterCatVisualChecklistCount { get; private set; }

        public int StarterCatVisualTraitCount { get; private set; }

        public int StarterCatTurnaroundConformanceSpecCount { get; private set; }

        public int StarterCatTurnaroundViewAnchorCount { get; private set; }

        public int StarterCatProductionSpecCount { get; private set; }

        public int StarterCatProductionAllowedTypeCount { get; private set; }

        public bool StarterCatSourceLockPacketReady { get; private set; }

        public bool StarterCatTurnaroundComparisonAuditReady { get; private set; }

        public int StarterCatTurnaroundComparisonAuditArtifactCount { get; private set; }

        public int StarterCatTurnaroundComparisonAuditManifestRowCount { get; private set; }

        public bool StarterCatReferencePlateReady { get; private set; }

        public int StarterCatReferencePlateArtifactCount { get; private set; }

        public int StarterCatReferencePlateCount { get; private set; }

        public bool StarterCatUnityReferenceInstallReady { get; private set; }

        public int StarterCatUnityReferenceInstallArtifactCount { get; private set; }

        public int StarterCatUnityReferenceInstallAssetCount { get; private set; }

        public bool StarterCatRuntimeCombatSpriteAuditReady { get; private set; }

        public int StarterCatRuntimeCombatSpriteAuditArtifactCount { get; private set; }

        public int StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount { get; private set; }

        public bool StarterCatStrictCandidateReady { get; private set; }

        public int StarterCatStrictCandidateCount { get; private set; }

        public P0StarterCatFormalImportState StarterCatFormalImportState { get; private set; } = P0StarterCatFormalImportState.Invalid;

        public bool StarterCatFormalImportGateValid { get; private set; }

        public bool StarterCatFormalImportAllowed { get; private set; }

        public int StarterCatFormalReviewNoteCount { get; private set; }

        public int StarterCatFormalBlockNoteCount { get; private set; }

        public int StarterCatFormalApprovalNoteCount { get; private set; }

        public int StarterCatActiveScreenshotCount { get; private set; }

        public bool AssetProductionQueueReady { get; private set; }

        public int AssetProductionQueueCount { get; private set; }

        public int AssetProductionQueueCodexRunnableCount { get; private set; }

        public int AssetProductionQueueCandidatePackCompletePendingUnityReviewCount { get; private set; }

        public int AssetProductionQueueUnityBlockedCount { get; private set; }

        public int RuntimeBoundEntryCount { get; private set; }

        public int FailureCount => Count(P0AssetReviewPacketSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0AssetReviewPacket.ExpectedCoveredCheckCount;

        public void SetCounts(
            int reviewAssetCount,
            int existingWorkspaceFileCount,
            int sourceLockedEntryCount,
            int starterCatEntryCount,
            int starterCatVisualChecklistCount,
            int starterCatVisualTraitCount,
            int starterCatTurnaroundConformanceSpecCount,
            int starterCatTurnaroundViewAnchorCount,
            int starterCatProductionSpecCount,
            int starterCatProductionAllowedTypeCount,
            bool starterCatSourceLockPacketReady,
            int runtimeBoundEntryCount)
        {
            ReviewAssetCount = reviewAssetCount;
            ExistingWorkspaceFileCount = existingWorkspaceFileCount;
            SourceLockedEntryCount = sourceLockedEntryCount;
            StarterCatEntryCount = starterCatEntryCount;
            StarterCatVisualChecklistCount = starterCatVisualChecklistCount;
            StarterCatVisualTraitCount = starterCatVisualTraitCount;
            StarterCatTurnaroundConformanceSpecCount = starterCatTurnaroundConformanceSpecCount;
            StarterCatTurnaroundViewAnchorCount = starterCatTurnaroundViewAnchorCount;
            StarterCatProductionSpecCount = starterCatProductionSpecCount;
            StarterCatProductionAllowedTypeCount = starterCatProductionAllowedTypeCount;
            StarterCatSourceLockPacketReady = starterCatSourceLockPacketReady;
            RuntimeBoundEntryCount = runtimeBoundEntryCount;
        }

        public void SetStarterCatTurnaroundComparisonAudit(P0StarterCatTurnaroundComparisonAuditReport audit)
        {
            StarterCatTurnaroundComparisonAuditReady = audit != null && audit.IsReady;
            StarterCatTurnaroundComparisonAuditArtifactCount = audit == null ? 0 : audit.ArtifactCount;
            StarterCatTurnaroundComparisonAuditManifestRowCount = audit == null ? 0 : audit.ManifestRowCount;
        }

        public void SetStarterCatReferencePlates(P0StarterCatReferencePlateReport plates)
        {
            StarterCatReferencePlateReady = plates != null && plates.IsReady;
            StarterCatReferencePlateArtifactCount = plates == null ? 0 : plates.ArtifactCount;
            StarterCatReferencePlateCount = plates == null ? 0 : plates.PlateCount;
        }

        public void SetStarterCatUnityReferenceInstall(P0StarterCatUnityReferenceInstallReport install)
        {
            StarterCatUnityReferenceInstallReady = install != null && install.IsReady;
            StarterCatUnityReferenceInstallArtifactCount = install == null ? 0 : install.ArtifactCount;
            StarterCatUnityReferenceInstallAssetCount = install == null ? 0 : install.InstalledAssetCount;
        }

        public void SetStarterCatRuntimeCombatSpriteAudit(P0StarterCatRuntimeCombatSpriteAuditReport audit)
        {
            StarterCatRuntimeCombatSpriteAuditReady = audit != null && audit.IsReady;
            StarterCatRuntimeCombatSpriteAuditArtifactCount = audit == null ? 0 : audit.ArtifactCount;
            StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount = audit == null ? 0 : audit.RuntimeSpriteCount;
        }

        public void SetStarterCatFormalImport(P0StarterCatFormalImportReadinessReport formalImport)
        {
            if (formalImport == null)
            {
                StarterCatFormalImportState = P0StarterCatFormalImportState.Invalid;
                StarterCatFormalImportGateValid = false;
                StarterCatFormalImportAllowed = false;
                StarterCatFormalReviewNoteCount = 0;
                StarterCatFormalBlockNoteCount = 0;
                StarterCatFormalApprovalNoteCount = 0;
                StarterCatActiveScreenshotCount = 0;
                return;
            }

            StarterCatFormalImportState = formalImport.State;
            StarterCatFormalImportGateValid = formalImport.IsGateValid;
            StarterCatFormalImportAllowed = formalImport.IsImportAllowed;
            StarterCatFormalReviewNoteCount = formalImport.ReviewNoteCount;
            StarterCatFormalBlockNoteCount = formalImport.ExplicitBlockNoteCount;
            StarterCatFormalApprovalNoteCount = formalImport.ExplicitApprovalNoteCount;
            StarterCatActiveScreenshotCount = formalImport.ActiveCatScreenshotCount;
        }

        public void SetStarterCatStrictCandidate(P0StarterCatStrictCandidateEvidenceReport strictCandidates)
        {
            StarterCatStrictCandidateReady = strictCandidates != null && strictCandidates.IsReady;
            StarterCatStrictCandidateCount = strictCandidates == null ? 0 : strictCandidates.CandidateCount;
        }

        public void SetAssetProductionQueue(P0AssetProductionQueueCoverageReport queue)
        {
            AssetProductionQueueReady = queue != null && queue.IsReady;
            AssetProductionQueueCount = queue == null ? 0 : queue.QueueCount;
            AssetProductionQueueCodexRunnableCount = queue == null ? 0 : queue.CodexRunnableCount;
            AssetProductionQueueCandidatePackCompletePendingUnityReviewCount = queue == null ? 0 : queue.CandidatePackCompletePendingUnityReviewCount;
            AssetProductionQueueUnityBlockedCount = queue == null ? 0 : queue.UnityBlockedCount;
        }

        public void AddEntry(P0AssetReviewPacketEntry entry)
        {
            if (entry != null)
            {
                entries.Add(entry);
            }
        }

        public void AddIssue(P0AssetReviewPacketSeverity severity, string message)
        {
            issues.Add(new P0AssetReviewPacketIssue(severity, message));
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
                ? "P0 asset review packet ready for " + ReviewAssetCount + " asset(s), " + StarterCatEntryCount + " starter cat(s), and " + RuntimeBoundEntryCount + " runtime-bound asset(s)."
                : "P0 asset review packet has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Review assets: " + ReviewAssetCount,
                "Existing workspace files: " + ExistingWorkspaceFileCount,
                "Source-locked entries: " + SourceLockedEntryCount,
                "Starter cat entries: " + StarterCatEntryCount,
                "Starter cat visual checklist cats: " + StarterCatVisualChecklistCount,
                "Starter cat visual traits: " + StarterCatVisualTraitCount,
                "Starter cat turnaround conformance spec cats: " + StarterCatTurnaroundConformanceSpecCount,
                "Starter cat turnaround view anchors: " + StarterCatTurnaroundViewAnchorCount,
                "Starter cat asset-production spec cats: " + StarterCatProductionSpecCount,
                "Starter cat allowed derivative asset types: " + StarterCatProductionAllowedTypeCount,
                "Starter cat source-lock packet ready: " + (StarterCatSourceLockPacketReady ? "yes" : "no"),
                "Starter cat turnaround comparison audit ready: " + (StarterCatTurnaroundComparisonAuditReady ? "yes" : "no"),
                "Starter cat turnaround comparison audit artifacts: " + StarterCatTurnaroundComparisonAuditArtifactCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount,
                "Starter cat turnaround comparison audit manifest rows: " + StarterCatTurnaroundComparisonAuditManifestRowCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount,
                "Starter cat reference plates ready: " + (StarterCatReferencePlateReady ? "yes" : "no"),
                "Starter cat reference plate artifacts: " + StarterCatReferencePlateArtifactCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedArtifactCount,
                "Starter cat reference plates: " + StarterCatReferencePlateCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedPlateCount,
                "Starter cat Unity reference install ready: " + (StarterCatUnityReferenceInstallReady ? "yes" : "no"),
                "Starter cat Unity reference install artifacts: " + StarterCatUnityReferenceInstallArtifactCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount,
                "Starter cat Unity reference install assets: " + StarterCatUnityReferenceInstallAssetCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount,
                "Starter cat runtime combat sprite audit ready: " + (StarterCatRuntimeCombatSpriteAuditReady ? "yes" : "no"),
                "Starter cat runtime combat sprite audit artifacts: " + StarterCatRuntimeCombatSpriteAuditArtifactCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount,
                "Starter cat runtime combat sprites: " + StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount,
                "Starter cat strict candidates ready: " + (StarterCatStrictCandidateReady ? "yes" : "no"),
                "Starter cat strict candidates: " + StarterCatStrictCandidateCount + "/" + P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount,
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
                "Runtime-bound entries: " + RuntimeBoundEntryCount
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
            builder.AppendLine("# P0 Asset Review Packet");
            builder.AppendLine();
            builder.AppendLine("## Summary");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (IsReady ? "yes" : "no"));
            builder.AppendLine("- Review assets: " + ReviewAssetCount);
            builder.AppendLine("- Existing workspace files: " + ExistingWorkspaceFileCount);
            builder.AppendLine("- Source-locked entries: " + SourceLockedEntryCount);
            builder.AppendLine("- Starter cat entries: " + StarterCatEntryCount);
            builder.AppendLine("- Starter cat visual checklist cats: " + StarterCatVisualChecklistCount);
            builder.AppendLine("- Starter cat visual traits: " + StarterCatVisualTraitCount);
            builder.AppendLine("- Starter cat turnaround conformance spec cats: " + StarterCatTurnaroundConformanceSpecCount);
            builder.AppendLine("- Starter cat turnaround view anchors: " + StarterCatTurnaroundViewAnchorCount);
            builder.AppendLine("- Starter cat asset-production spec cats: " + StarterCatProductionSpecCount);
            builder.AppendLine("- Starter cat allowed derivative asset types: " + StarterCatProductionAllowedTypeCount);
            builder.AppendLine("- Starter cat source-lock packet ready: " + (StarterCatSourceLockPacketReady ? "yes" : "no"));
            builder.AppendLine("- Starter cat turnaround comparison audit ready: " + (StarterCatTurnaroundComparisonAuditReady ? "yes" : "no"));
            builder.AppendLine("- Starter cat turnaround comparison audit artifacts: " + StarterCatTurnaroundComparisonAuditArtifactCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Starter cat turnaround comparison audit manifest rows: " + StarterCatTurnaroundComparisonAuditManifestRowCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Starter cat reference plates ready: " + (StarterCatReferencePlateReady ? "yes" : "no"));
            builder.AppendLine("- Starter cat reference plate artifacts: " + StarterCatReferencePlateArtifactCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Starter cat reference plates: " + StarterCatReferencePlateCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedPlateCount);
            builder.AppendLine("- Starter cat Unity reference install ready: " + (StarterCatUnityReferenceInstallReady ? "yes" : "no"));
            builder.AppendLine("- Starter cat Unity reference install artifacts: " + StarterCatUnityReferenceInstallArtifactCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Starter cat Unity reference install assets: " + StarterCatUnityReferenceInstallAssetCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount);
            builder.AppendLine("- Starter cat runtime combat sprite audit ready: " + (StarterCatRuntimeCombatSpriteAuditReady ? "yes" : "no"));
            builder.AppendLine("- Starter cat runtime combat sprite audit artifacts: " + StarterCatRuntimeCombatSpriteAuditArtifactCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Starter cat runtime combat sprites: " + StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount);
            builder.AppendLine("- Starter cat strict candidates ready: " + (StarterCatStrictCandidateReady ? "yes" : "no"));
            builder.AppendLine("- Starter cat strict candidates: " + StarterCatStrictCandidateCount + "/" + P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Starter cat formal import gate valid: " + (StarterCatFormalImportGateValid ? "yes" : "no"));
            builder.AppendLine("- Starter cat formal import state: " + StarterCatFormalImportState);
            builder.AppendLine("- Starter cat formal import allowed: " + (StarterCatFormalImportAllowed ? "yes" : "no"));
            builder.AppendLine("- Starter cat formal review notes: " + StarterCatFormalReviewNoteCount);
            builder.AppendLine("- Starter cat active screenshots: " + StarterCatActiveScreenshotCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            builder.AppendLine("- Asset production queue ready: " + (AssetProductionQueueReady ? "yes" : "no"));
            builder.AppendLine("- Asset production queue items: " + AssetProductionQueueCount);
            builder.AppendLine("- Asset production queue Codex-runnable items: " + AssetProductionQueueCodexRunnableCount);
            builder.AppendLine("- Asset production queue completed candidate packs pending Unity review: " + AssetProductionQueueCandidatePackCompletePendingUnityReviewCount);
            builder.AppendLine("- Asset production queue Unity-blocked items: " + AssetProductionQueueUnityBlockedCount);
            builder.AppendLine("- Runtime-bound entries: " + RuntimeBoundEntryCount);
            builder.AppendLine();

            builder.AppendLine("## Gate Checks");
            builder.AppendLine();
            for (int i = 0; i < coveredChecks.Count; i++)
            {
                builder.AppendLine("- " + coveredChecks[i]);
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

            AppendEntryTable(builder, "Starter Cat Turnaround Review", true);
            AppendStarterCatVisualChecklist(builder);
            AppendStarterCatTurnaroundConformanceSpec(builder);
            AppendStarterCatSourceLockPacketEvidence(builder);
            AppendStarterCatTurnaroundComparisonAudit(builder);
            AppendStarterCatReferencePlates(builder);
            AppendStarterCatUnityReferenceInstall(builder);
            AppendStarterCatRuntimeCombatSpriteAudit(builder);
            AppendStarterCatAssetProductionSpec(builder);
            AppendStarterCatProductionPromptReadiness(builder);
            AppendStarterCatDerivativeCandidateEvidence(builder);
            AppendStarterCatStrictCandidateEvidence(builder);
            AppendStarterCatFormalImportReadiness(builder);
            AppendAssetProductionQueue(builder);
            AppendPlayModeScreenshotFileEvidence(builder);
            AppendRuntimeVisualContactSheetEvidence(builder);
            AppendEntryTable(builder, "Runtime Visual Binding Review", false);
            AppendFullManifestTable(builder);
            return builder.ToString();
        }

        private static void AppendStarterCatVisualChecklist(StringBuilder builder)
        {
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist = P0StarterCatVisualConsistencyChecklist.CreateP0Checklist();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Visual Consistency Checklist");
            builder.AppendLine();
            builder.AppendLine("| cat | source lock | active screenshot | required colored-turnaround traits | drift blocker |");
            builder.AppendLine("| --- | --- | --- | --- | --- |");
            for (int i = 0; i < checklist.Count; i++)
            {
                P0StarterCatVisualConsistencyChecklistEntry entry = checklist[i];
                builder.Append("| ");
                builder.Append(Escape(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(Escape(entry.SourceLockId));
                builder.Append(" | ");
                builder.Append(Escape(entry.PlayModeScreenshotFileName));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.RequiredTraits)));
                builder.Append(" | ");
                builder.Append(Escape(entry.ProhibitedDriftRule));
                builder.AppendLine(" |");
            }
        }

        private static void AppendStarterCatAssetProductionSpec(StringBuilder builder)
        {
            IReadOnlyList<P0StarterCatAssetProductionSpecEntry> specs = P0StarterCatAssetProductionSpec.CreateP0Spec();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Asset Production Spec");
            builder.AppendLine();
            builder.AppendLine("| cat | source lock | candidate directory | approved import directory | allowed derivative asset types | required evidence | rejection rules |");
            builder.AppendLine("| --- | --- | --- | --- | --- | --- | --- |");
            for (int i = 0; i < specs.Count; i++)
            {
                P0StarterCatAssetProductionSpecEntry entry = specs[i];
                builder.Append("| ");
                builder.Append(Escape(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(Escape(entry.SourceLockId));
                builder.Append(" | ");
                builder.Append(Escape(entry.CandidateDirectory));
                builder.Append(" | ");
                builder.Append(Escape(entry.ApprovedImportDirectory));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.AllowedDerivativeAssetTypes)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.RequiredEvidence)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.RejectionRules)));
                builder.AppendLine(" |");
            }
        }

        private static void AppendStarterCatProductionPromptReadiness(StringBuilder builder)
        {
            P0StarterCatProductionPromptReadinessReport prompts = P0StarterCatProductionPromptReadiness.EvaluateCurrentPrompts();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Production Prompt Readiness");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (prompts.IsReady ? "yes" : "no"));
            builder.AppendLine("- Prompt files: " + prompts.PromptCount + "/" + P0StarterCatProductionPromptReadiness.ExpectedPromptCount);
            builder.AppendLine("- Real design-root mentions: " + prompts.DesignRootMentionCount + "/" + P0StarterCatProductionPromptReadiness.ExpectedPromptCount);
            builder.AppendLine("- Source turnaround path mentions: " + prompts.SourceTurnaroundPathMentionCount);
            builder.AppendLine("- Candidate policy mentions: " + prompts.CandidatePolicyMentionCount + "/" + P0StarterCatProductionPromptReadiness.ExpectedPromptCount);
            builder.AppendLine("- Formal import block mentions: " + prompts.FormalImportBlockMentionCount + "/" + P0StarterCatProductionPromptReadiness.ExpectedPromptCount);
            builder.AppendLine("- One-cat-at-a-time mentions: " + prompts.OneCatAtATimeMentionCount);
            builder.AppendLine("- Mojibake path mentions: " + prompts.MojibakePathMentionCount);
            builder.AppendLine("- Batch 28 prompt: `design/development/agent_prompts/p0_asset_batch_28_starter_cat_strict_reference_pack.md`");
            builder.AppendLine("- Batch 29 prompt: `design/development/agent_prompts/p0_asset_batch_29_saiban_strict_turnaround_derivatives.md`");
            builder.AppendLine("- Batch 30 prompt: `design/development/agent_prompts/p0_asset_batch_30_saiban_ai_refinement_candidate.md`");
            builder.AppendLine("- Batch 31 prompt: `design/development/agent_prompts/p0_asset_batch_31_saiban_cutout_candidate.md`");
            builder.AppendLine("- Batch 32 prompt: `design/development/agent_prompts/p0_asset_batch_32_nephthys_strict_turnaround_derivatives.md`");
            builder.AppendLine("- Batch 33 prompt: `design/development/agent_prompts/p0_asset_batch_33_suzune_strict_turnaround_derivatives.md`");
            builder.AppendLine("- Batch 34 prompt: `design/development/agent_prompts/p0_asset_batch_34_suzune_ai_refinement_candidate.md`");
            builder.AppendLine("- Batch 35 prompt: `design/development/agent_prompts/p0_asset_batch_35_suzune_cutout_candidate.md`");
            builder.AppendLine("- Batch 36 prompt: `design/development/agent_prompts/p0_asset_batch_36_nephthys_ai_refinement_candidate.md`");
            builder.AppendLine("- Batch 37 prompt: `design/development/agent_prompts/p0_asset_batch_37_nephthys_cutout_candidate.md`");
            builder.AppendLine("- Real design root: `" + Escape(P0StarterCatProductionPromptReadiness.DesignRoot) + "`");
        }

        private static void AppendStarterCatTurnaroundConformanceSpec(StringBuilder builder)
        {
            IReadOnlyList<P0StarterCatTurnaroundConformanceSpecEntry> specs = P0StarterCatTurnaroundConformanceSpec.CreateP0Spec();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Turnaround Conformance Spec");
            builder.AppendLine();
            builder.AppendLine("| cat | source lock | front view anchors | side view anchors | back view anchors | palette anchors | prop/costume anchors | prohibited drift |");
            builder.AppendLine("| --- | --- | --- | --- | --- | --- | --- | --- |");
            for (int i = 0; i < specs.Count; i++)
            {
                P0StarterCatTurnaroundConformanceSpecEntry entry = specs[i];
                builder.Append("| ");
                builder.Append(Escape(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(Escape(entry.SourceLockId));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.FrontViewAnchors)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.SideViewAnchors)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.BackViewAnchors)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.PaletteAnchors)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.PropAndCostumeAnchors)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.ProhibitedDriftRules)));
                builder.AppendLine(" |");
            }
        }

        private static void AppendStarterCatSourceLockPacketEvidence(StringBuilder builder)
        {
            P0StarterCatSourceLockPacketReport packet = P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Source-Lock Packet Evidence");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (packet.IsReady ? "yes" : "no"));
            builder.AppendLine("- Markdown packet: `" + Escape(P0StarterCatSourceLockPacketEvidence.PacketMarkdownPath) + "`");
            builder.AppendLine("- CSV packet: `" + Escape(P0StarterCatSourceLockPacketEvidence.PacketCsvPath) + "`");
            builder.AppendLine("- Cat entries: " + packet.CatEntryCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Source hash mentions: " + packet.SourceHashMentionCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Sprite hash mentions: " + packet.SpriteHashMentionCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Active screenshots: " + packet.ActiveScreenshotMentionCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Candidate review sheets: " + packet.CandidateReviewSheetMentionCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Core source-lock documents: " + packet.CoreDocumentCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount);
            builder.AppendLine("- Core source-lock documents with exact source paths: " + packet.CoreDocumentSourcePathMentionCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount);
            builder.AppendLine("- Core source-lock documents with import block: " + packet.CoreDocumentImportBlockMentionCount + "/" + P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount);
            builder.AppendLine("- Core source-lock document mojibake mentions: " + packet.CoreDocumentMojibakeMentionCount);
        }

        private static void AppendStarterCatTurnaroundComparisonAudit(StringBuilder builder)
        {
            P0StarterCatTurnaroundComparisonAuditReport audit = P0StarterCatTurnaroundComparisonAuditEvidence.EvaluateCurrentAudit();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Turnaround Runtime Comparison Audit");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (audit.IsReady ? "yes" : "no"));
            builder.AppendLine("- Import status: audit-only; do not import into Unity until active-cat Play Mode screenshots pass source-lock review");
            builder.AppendLine("- Batch directory: `" + Escape(P0StarterCatTurnaroundComparisonAuditEvidence.BatchDirectory) + "`");
            builder.AppendLine("- Manifest: `" + Escape(P0StarterCatTurnaroundComparisonAuditEvidence.ManifestPath) + "`");
            builder.AppendLine("- Review sheet: `" + Escape(P0StarterCatTurnaroundComparisonAuditEvidence.ReviewSheetPath) + "`");
            builder.AppendLine("- Artifacts: " + audit.ArtifactCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Manifest rows: " + audit.ManifestRowCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Source locks: " + audit.SourceLockMentionCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Exact colored-turnaround source paths: " + audit.SourcePathMentionCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Current Unity combat sprites: " + audit.SpritePathMentionCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Active-cat screenshot targets: " + audit.ActiveScreenshotMentionCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Audit-only recommendations: " + audit.RecommendationMentionCount + "/" + P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Unity .meta files in candidate batch: " + audit.MetaFileCount);
        }

        private static void AppendStarterCatReferencePlates(StringBuilder builder)
        {
            P0StarterCatReferencePlateReport plates = P0StarterCatReferencePlateEvidence.EvaluateCurrentPlates();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Source Turnaround Reference Plates");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (plates.IsReady ? "yes" : "no"));
            builder.AppendLine("- Import status: reference-only; do not import into Unity until active-cat Play Mode screenshots pass source-lock review");
            builder.AppendLine("- Batch directory: `" + Escape(P0StarterCatReferencePlateEvidence.BatchDirectory) + "`");
            builder.AppendLine("- Manifest: `" + Escape(P0StarterCatReferencePlateEvidence.ManifestPath) + "`");
            builder.AppendLine("- Review sheet: `" + Escape(P0StarterCatReferencePlateEvidence.ReviewSheetPath) + "`");
            builder.AppendLine("- Artifacts: " + plates.ArtifactCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Reference plates: " + plates.PlateCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedPlateCount);
            builder.AppendLine("- Manifest rows: " + plates.ManifestRowCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedPlateCount);
            builder.AppendLine("- Source locks: " + plates.SourceLockMentionCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Exact colored-turnaround source paths: " + plates.SourcePathMentionCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Front/side/back view rows: " + plates.ViewRowCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedPlateCount);
            builder.AppendLine("- Import blocks: " + plates.ImportBlockCount + "/" + P0StarterCatReferencePlateEvidence.ExpectedPlateCount);
            builder.AppendLine("- Unity .meta files in candidate batch: " + plates.MetaFileCount);
        }

        private static void AppendStarterCatUnityReferenceInstall(StringBuilder builder)
        {
            P0StarterCatUnityReferenceInstallReport install = P0StarterCatUnityReferenceInstallEvidence.EvaluateCurrentInstall();
            builder.AppendLine();
            builder.AppendLine("## Batch 71-73 Starter Cat Unity Reference Installs");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (install.IsReady ? "yes" : "no"));
            builder.AppendLine("- Import status: installed debug references; not runtime-bound; formal cat body-art import remains blocked");
            IReadOnlyList<P0StarterCatUnityReferenceInstallEntry> installs = P0StarterCatUnityReferenceInstallEvidence.CreateP0Installs();
            for (int i = 0; i < installs.Count; i++)
            {
                P0StarterCatUnityReferenceInstallEntry entry = installs[i];
                builder.AppendLine("- " + Escape(entry.DisplayName) + " batch directory: `" + Escape(entry.BatchDirectory) + "`");
                builder.AppendLine("- " + Escape(entry.DisplayName) + " installed atlas: `" + Escape(entry.InstalledAtlasPath) + "`");
                builder.AppendLine("- " + Escape(entry.DisplayName) + " manifest: `" + Escape(entry.ManifestPath) + "`");
                builder.AppendLine("- " + Escape(entry.DisplayName) + " review sheet: `" + Escape(entry.ReviewSheetPath) + "`");
            }

            builder.AppendLine("- Artifacts: " + install.ArtifactCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Installed debug reference assets: " + install.InstalledAssetCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount);
            builder.AppendLine("- Reference plate links: " + install.ReferencePlateMentionCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedReferencePlateMentionCount);
            builder.AppendLine("- Import setting tokens: " + install.ImportSettingTokenCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedImportSettingTokenCount);
            builder.AppendLine("- Runtime replacement block mentions: " + install.RuntimeBlockMentionCount + "/" + P0StarterCatUnityReferenceInstallEvidence.ExpectedRuntimeBlockMentionCount);
        }

        private static void AppendStarterCatRuntimeCombatSpriteAudit(StringBuilder builder)
        {
            P0StarterCatRuntimeCombatSpriteAuditReport audit = P0StarterCatRuntimeCombatSpriteAuditEvidence.EvaluateCurrentAudit();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Runtime Combat Sprite Source Audit");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (audit.IsReady ? "yes" : "no"));
            builder.AppendLine("- Import status: runtime-bound sprites; no Unity sprite replacement; formal cat body-art import remains blocked");
            builder.AppendLine("- Batch directory: `" + Escape(P0StarterCatRuntimeCombatSpriteAuditEvidence.BatchDirectory) + "`");
            builder.AppendLine("- Manifest: `" + Escape(P0StarterCatRuntimeCombatSpriteAuditEvidence.ManifestPath) + "`");
            builder.AppendLine("- Review sheet: `" + Escape(P0StarterCatRuntimeCombatSpriteAuditEvidence.ReviewSheetPath) + "`");
            builder.AppendLine("- Review note: `" + Escape(P0StarterCatRuntimeCombatSpriteAuditEvidence.ReviewNotePath) + "`");
            builder.AppendLine("- Evidence class: `P0StarterCatRuntimeCombatSpriteAuditEvidence`");
            builder.AppendLine("- Artifacts: " + audit.ArtifactCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount);
            builder.AppendLine("- Manifest rows: " + audit.ManifestRowCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Runtime sprites: " + audit.RuntimeSpriteCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount);
            builder.AppendLine("- Source locks: " + audit.SourceLockMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Exact colored-turnaround source paths: " + audit.SourceTurnaroundPathMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Front reference plates: " + audit.FrontPlateMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Runtime binding ids: " + audit.RuntimeBindingMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeBindingMentionCount);
            builder.AppendLine("- Import-block clauses: " + audit.ImportBlockMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedImportBlockMentionCount);
        }

        private static void AppendStarterCatDerivativeCandidateEvidence(StringBuilder builder)
        {
            P0StarterCatDerivativeCandidateEvidenceReport evidence = P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Derivative Candidate Evidence");
            builder.AppendLine();
            builder.AppendLine("- Review ready: " + (evidence.IsReviewReady ? "yes" : "no"));
            builder.AppendLine("- Import status: blocked until active-cat Play Mode screenshots exist and pass source-lock review");
            builder.AppendLine("- Batch directory: `" + Escape(evidence.BatchDirectory) + "`");
            builder.AppendLine("- Existing evidence files: " + evidence.ExistingFileCount + "/" + evidence.ExpectedFileCount);
            builder.AppendLine("- Candidate PNG files: " + evidence.CandidatePngCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedCandidatePngCount);
            builder.AppendLine("- Review notes: " + evidence.ReviewNoteCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount);
            builder.AppendLine("- Review sheets: " + evidence.ReviewSheetCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewSheetCount);
            builder.AppendLine("- Candidate outputs outside `Assets`: " + (evidence.HasCandidateOutsideAssets ? "yes" : "no"));
            builder.AppendLine("- Turnaround conformance review notes: " + (evidence.HasTurnaroundConformanceReviewNotes ? "yes" : "no"));
            builder.AppendLine("- Conformance spec mentions: " + evidence.ReviewNoteConformanceSpecMentionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount);
            builder.AppendLine("- Front/side/back anchor sections: " + evidence.ReviewNoteFrontAnchorSectionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
                + ", " + evidence.ReviewNoteSideAnchorSectionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
                + ", " + evidence.ReviewNoteBackAnchorSectionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount);
            builder.AppendLine("- Palette, prop/costume, prohibited-drift sections: " + evidence.ReviewNotePaletteAnchorSectionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
                + ", " + evidence.ReviewNotePropCostumeAnchorSectionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
                + ", " + evidence.ReviewNoteProhibitedDriftSectionCount + "/" + P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount);
            AppendFileList(builder, "Missing Starter Cat Candidate Evidence", evidence.MissingFiles);
        }

        private static void AppendStarterCatStrictCandidateEvidence(StringBuilder builder)
        {
            P0StarterCatStrictCandidateEvidenceReport evidence = P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Strict Candidate Evidence");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (evidence.IsReady ? "yes" : "no"));
            builder.AppendLine("- Import status: candidate review only; do not import into Unity until active-cat screenshot review approves it");
            builder.AppendLine("- Candidates: " + evidence.CandidateCount + "/" + P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount);
            builder.AppendLine("- Manifests: " + evidence.ManifestCount);
            builder.AppendLine("- Alpha candidates: " + evidence.AlphaCandidateCount);
            builder.AppendLine("- Review sheets: " + evidence.ReviewSheetCount);
            builder.AppendLine("- Review notes: " + evidence.ReviewNoteCount);
            builder.AppendLine("- Agent prompts: " + evidence.AgentPromptCount);
            builder.AppendLine("- Explicit block notes: " + evidence.ExplicitBlockNoteCount);
            builder.AppendLine("- Active screenshot mentions: " + evidence.ActiveScreenshotMentionCount);
            builder.AppendLine("- Exact source turnaround paths: " + evidence.SourceTurnaroundExactPathCount);
            builder.AppendLine("- Batch 47 spec manifest locks: " + evidence.Batch47SpecManifestLockCount);
            builder.AppendLine("- Batch 47 JSON identity locks: " + evidence.Batch47SpecJsonIdentityLockCount);
            builder.AppendLine();
            builder.AppendLine("| cat | batch | alpha candidate | review sheet | active screenshot gate |");
            builder.AppendLine("| --- | --- | --- | --- | --- |");

            for (int i = 0; i < evidence.Entries.Count; i++)
            {
                P0StarterCatStrictCandidateEntry entry = evidence.Entries[i];
                builder.Append("| ");
                builder.Append(Escape(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(Escape(entry.BatchSlug));
                builder.Append(" | ");
                builder.Append(Escape(entry.AlphaCandidatePath));
                builder.Append(" | ");
                builder.Append(Escape(entry.ReviewSheetPath));
                builder.Append(" | ");
                builder.Append(Escape(entry.ActiveScreenshotFileName));
                builder.AppendLine(" |");
            }
        }

        private static void AppendStarterCatFormalImportReadiness(StringBuilder builder)
        {
            P0StarterCatFormalImportReadinessReport formalImport = P0StarterCatFormalImportReadiness.EvaluateCurrentGate();
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Formal Import Readiness");
            builder.AppendLine();
            builder.AppendLine("- Gate valid: " + (formalImport.IsGateValid ? "yes" : "no"));
            builder.AppendLine("- Import allowed: " + (formalImport.IsImportAllowed ? "yes" : "no"));
            builder.AppendLine("- State: " + formalImport.State);
            builder.AppendLine("- Review notes: " + formalImport.ReviewNoteCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            builder.AppendLine("- Explicit block notes: " + formalImport.ExplicitBlockNoteCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            builder.AppendLine("- Explicit approval notes: " + formalImport.ExplicitApprovalNoteCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            builder.AppendLine("- Active-cat screenshots: " + formalImport.ActiveCatScreenshotCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            builder.AppendLine("- Summary: " + Escape(formalImport.BuildSummary()));
        }

        private static void AppendAssetProductionQueue(StringBuilder builder)
        {
            P0AssetProductionQueueCoverageReport coverage = P0AssetProductionQueueCoverage.EvaluateP0Queue();
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();
            builder.AppendLine();
            builder.AppendLine("## Asset Production Queue");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + (coverage.IsReady ? "yes" : "no"));
            builder.AppendLine("- Queue items: " + coverage.QueueCount);
            builder.AppendLine("- Codex-runnable items: " + coverage.CodexRunnableCount);
            builder.AppendLine("- Completed candidate packs pending Unity review: " + coverage.CandidatePackCompletePendingUnityReviewCount);
            builder.AppendLine("- Unity-blocked items: " + coverage.UnityBlockedCount);
            builder.AppendLine("- Existing prompts: " + coverage.ExistingPromptCount);
            builder.AppendLine();
            builder.AppendLine("| priority | queue | phase | state | prompt | next action |");
            builder.AppendLine("| --- | --- | --- | --- | --- | --- |");

            for (int i = 0; i < queue.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queue[i];
                builder.Append("| ");
                builder.Append(entry.Priority);
                builder.Append(" | ");
                builder.Append(Escape(entry.DisplayName));
                builder.Append(" | ");
                builder.Append(entry.Phase);
                builder.Append(" | ");
                builder.Append(entry.State);
                builder.Append(" | ");
                builder.Append(Escape(entry.ExecutionPromptPath));
                builder.Append(" | ");
                builder.Append(Escape(entry.NextAction));
                builder.AppendLine(" |");
            }
        }

        private static void AppendPlayModeScreenshotFileEvidence(StringBuilder builder)
        {
            P0PlayModeScreenshotFileEvidenceReport evidence = P0PlayModeScreenshotFileEvidence.EvaluateP0Directory();
            builder.AppendLine();
            builder.AppendLine("## Play Mode Screenshot File Evidence");
            builder.AppendLine();
            builder.AppendLine("- Complete: " + (evidence.IsComplete ? "yes" : "no"));
            builder.AppendLine("- Directory: `" + Escape(evidence.ScreenshotDirectory) + "`");
            builder.AppendLine("- Existing expected captures: " + evidence.ExistingExpectedFileCount + "/" + evidence.ExpectedFileCount);
            builder.AppendLine("- Missing expected captures: " + evidence.MissingExpectedFileCount);
            builder.AppendLine("- Unexpected PNG files: " + evidence.UnexpectedPngFileCount);
            AppendFileList(builder, "Existing Expected Captures", evidence.ExistingExpectedFiles);
            AppendFileList(builder, "Missing Expected Captures", evidence.MissingExpectedFiles);
            AppendFileList(builder, "Unexpected PNG Files", evidence.UnexpectedPngFiles);
        }

        private static void AppendRuntimeVisualContactSheetEvidence(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("## Runtime Visual Contact Sheet Evidence");
            builder.AppendLine();
            builder.AppendLine("- Present: " + (File.Exists(P0AssetProductionReadiness.RuntimeVisualContactSheetPath) ? "yes" : "no"));
            builder.AppendLine("- Contact sheet: `" + Escape(P0AssetProductionReadiness.RuntimeVisualContactSheetPath) + "`");
            builder.AppendLine("- Review note: `design/development/asset_review/p0_runtime_visual_contact_sheet_2026-06-14.md`");
            builder.AppendLine("- Scope: " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " runtime visual bindings, with starter cats shown only as locked current sprites.");
        }

        private static void AppendFileList(StringBuilder builder, string title, IReadOnlyList<string> fileNames)
        {
            if (fileNames == null || fileNames.Count == 0)
            {
                return;
            }

            builder.AppendLine();
            builder.AppendLine("### " + title);
            builder.AppendLine();
            for (int i = 0; i < fileNames.Count; i++)
            {
                builder.AppendLine("- `" + Escape(fileNames[i]) + "`");
            }
        }

        private void AppendEntryTable(StringBuilder builder, string title, bool starterCatsOnly)
        {
            builder.AppendLine();
            builder.AppendLine("## " + title);
            builder.AppendLine();
            builder.AppendLine("| subject | asset | source locks | source paths | runtime bindings | notes |");
            builder.AppendLine("| --- | --- | --- | --- | --- | --- |");
            for (int i = 0; i < entries.Count; i++)
            {
                P0AssetReviewPacketEntry entry = entries[i];
                if (starterCatsOnly && !entry.IsStarterCat)
                {
                    continue;
                }

                if (!starterCatsOnly && !entry.IsRuntimeBound)
                {
                    continue;
                }

                AppendEntryRow(builder, entry);
            }
        }

        private void AppendFullManifestTable(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("## Full Review Manifest");
            builder.AppendLine();
            builder.AppendLine("| subject | type | asset | path | source locks | runtime bindings |");
            builder.AppendLine("| --- | --- | --- | --- | --- | --- |");
            for (int i = 0; i < entries.Count; i++)
            {
                P0AssetReviewPacketEntry entry = entries[i];
                builder.Append("| ");
                builder.Append(Escape(entry.SubjectId));
                builder.Append(" | ");
                builder.Append(Escape(entry.AssetType));
                builder.Append(" | ");
                builder.Append(Escape(entry.AssetId));
                builder.Append(" | ");
                builder.Append(Escape(entry.UnityImportPath));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.SourceLockIds)));
                builder.Append(" | ");
                builder.Append(Escape(Join(entry.RuntimeBindingIds)));
                builder.AppendLine(" |");
            }
        }

        private static void AppendEntryRow(StringBuilder builder, P0AssetReviewPacketEntry entry)
        {
            builder.Append("| ");
            builder.Append(Escape(entry.SubjectId));
            builder.Append(" | ");
            builder.Append(Escape(entry.AssetId));
            builder.Append(" | ");
            builder.Append(Escape(Join(entry.SourceLockIds)));
            builder.Append(" | ");
            builder.Append(Escape(Join(entry.SourcePaths)));
            builder.Append(" | ");
            builder.Append(Escape(Join(entry.RuntimeBindingIds)));
            builder.Append(" | ");
            builder.Append(Escape(entry.ConsistencyNotes));
            builder.AppendLine(" |");
        }

        private static string Join(IReadOnlyList<string> values)
        {
            if (values == null || values.Count == 0)
            {
                return "-";
            }

            return string.Join("<br>", values);
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("|", "/");
        }

        private int Count(P0AssetReviewPacketSeverity severity)
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

    public static class P0AssetReviewPacket
    {
        public const int ExpectedCoveredCheckCount = 17;
        public const int ExpectedStarterCatReviewEntryCount = 6;

        public static P0AssetReviewPacketReport EvaluateP0Packet()
        {
            return Evaluate(
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                P0HardReferenceSourceLocks.CreateP0Locks(),
                P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                File.Exists);
        }

        public static P0AssetReviewPacketReport Evaluate(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<P0HardReferenceSourceLockEntry> sourceLocks,
            IReadOnlyList<P0VisualAssetBinding> runtimeBindings,
            Func<string, bool> fileExists)
        {
            P0AssetReviewPacketReport report = new P0AssetReviewPacketReport();
            Func<string, bool> exists = fileExists ?? File.Exists;
            Dictionary<string, P0HardReferenceSourceLockEntry> sourceLockById = BuildSourceLockMap(sourceLocks, report);
            Dictionary<string, List<string>> runtimeBindingsByAssetId = BuildRuntimeBindingMap(runtimeBindings);
            int reviewAssetCount = 0;
            int existingWorkspaceFileCount = 0;
            int sourceLockedEntryCount = 0;
            int starterCatEntryCount = 0;
            P0StarterCatVisualConsistencyReport starterCatVisualChecklist = P0StarterCatVisualConsistencyChecklist.Evaluate(
                P0StarterCatVisualConsistencyChecklist.CreateP0Checklist(),
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                exists);
            P0StarterCatTurnaroundConformanceSpecReport starterCatTurnaroundConformanceSpec = P0StarterCatTurnaroundConformanceSpec.EvaluateP0Spec();
            P0StarterCatAssetProductionSpecReport starterCatProductionSpec = P0StarterCatAssetProductionSpec.EvaluateP0Spec();
            P0StarterCatSourceLockPacketReport starterCatSourceLockPacket = P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket();
            P0StarterCatTurnaroundComparisonAuditReport starterCatTurnaroundComparisonAudit = P0StarterCatTurnaroundComparisonAuditEvidence.EvaluateCurrentAudit();
            P0StarterCatReferencePlateReport starterCatReferencePlates = P0StarterCatReferencePlateEvidence.EvaluateCurrentPlates();
            P0StarterCatUnityReferenceInstallReport starterCatUnityReferenceInstall = P0StarterCatUnityReferenceInstallEvidence.EvaluateCurrentInstall();
            P0StarterCatRuntimeCombatSpriteAuditReport starterCatRuntimeCombatSpriteAudit = P0StarterCatRuntimeCombatSpriteAuditEvidence.EvaluateCurrentAudit();
            P0StarterCatStrictCandidateEvidenceReport starterCatStrictCandidates = P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates();
            P0StarterCatFormalImportReadinessReport starterCatFormalImport = P0StarterCatFormalImportReadiness.EvaluateCurrentGate();
            P0AssetProductionQueueCoverageReport assetProductionQueue = P0AssetProductionQueueCoverage.EvaluateP0Queue();
            int runtimeBoundEntryCount = 0;
            bool allWorkspaceFilesExist = true;
            bool allSourceLocksResolve = true;

            if (manifest == null)
            {
                report.AddIssue(P0AssetReviewPacketSeverity.Failure, "Asset manifest is missing.");
                report.SetCounts(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, false, 0);
                return report;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (!P0AssetManifestStatus.RequiresWorkspaceFile(entry.Status))
                {
                    continue;
                }

                reviewAssetCount++;
                if (exists(entry.UnityImportPath))
                {
                    existingWorkspaceFileCount++;
                }
                else
                {
                    allWorkspaceFilesExist = false;
                    report.AddIssue(P0AssetReviewPacketSeverity.Failure, entry.AssetId + " workspace file is missing: " + entry.UnityImportPath);
                }

                List<string> sourcePaths = ResolveSourcePaths(entry, sourceLockById, report, ref allSourceLocksResolve);
                List<string> bindingIds = ResolveRuntimeBindings(entry.AssetId, runtimeBindingsByAssetId);
                P0AssetReviewPacketEntry reviewEntry = new P0AssetReviewPacketEntry(
                    entry.AssetId,
                    entry.SubjectId,
                    entry.AssetType,
                    entry.UnityImportPath,
                    entry.SourceLockIds,
                    sourcePaths,
                    bindingIds,
                    entry.ConsistencyNotes);
                report.AddEntry(reviewEntry);

                if (reviewEntry.IsSourceLocked)
                {
                    sourceLockedEntryCount++;
                }

                if (reviewEntry.IsStarterCat)
                {
                    starterCatEntryCount++;
                }

                if (reviewEntry.IsRuntimeBound)
                {
                    runtimeBoundEntryCount++;
                }
            }

            report.SetCounts(
                reviewAssetCount,
                existingWorkspaceFileCount,
                sourceLockedEntryCount,
                starterCatEntryCount,
                starterCatVisualChecklist.ChecklistCount,
                starterCatVisualChecklist.RequiredTraitCount,
                starterCatTurnaroundConformanceSpec.SpecCount,
                starterCatTurnaroundConformanceSpec.FrontViewAnchorCount + starterCatTurnaroundConformanceSpec.SideViewAnchorCount + starterCatTurnaroundConformanceSpec.BackViewAnchorCount,
                starterCatProductionSpec.SpecCount,
                starterCatProductionSpec.AllowedDerivativeAssetTypeCount,
                starterCatSourceLockPacket.IsReady,
                runtimeBoundEntryCount);
            report.SetStarterCatStrictCandidate(starterCatStrictCandidates);
            report.SetStarterCatTurnaroundComparisonAudit(starterCatTurnaroundComparisonAudit);
            report.SetStarterCatReferencePlates(starterCatReferencePlates);
            report.SetStarterCatUnityReferenceInstall(starterCatUnityReferenceInstall);
            report.SetStarterCatRuntimeCombatSpriteAudit(starterCatRuntimeCombatSpriteAudit);
            report.SetStarterCatFormalImport(starterCatFormalImport);
            report.SetAssetProductionQueue(assetProductionQueue);
            Require(report, reviewAssetCount > 0 && report.Entries.Count == reviewAssetCount, "Every generated/imported manifest asset has a review packet row.", "Review packet does not cover every generated/imported manifest asset.");
            Require(report, allWorkspaceFilesExist && existingWorkspaceFileCount == reviewAssetCount, "Every review packet asset exists at its workspace path.", "One or more review packet assets are missing from the workspace.");
            Require(report, allSourceLocksResolve, "Every manifest source-lock id resolves to a locked source file path.", "One or more manifest source-lock ids are unknown.");
            Require(report, sourceLockedEntryCount >= 11, "Source-sensitive P0 cats, enemies, Boss, and bedroom props are source-locked for review.", "Source-sensitive P0 review entries lost source-lock coverage.");
            Require(report, StarterCatReviewReady(report.Entries), "Starter cat review rows bind colored turnarounds, runtime slots, and hard-reference notes.", "Starter cat review rows are missing colored-turnaround evidence.");
            Require(report, StarterCatVisualChecklistReady(starterCatVisualChecklist), "Starter cat visual checklist binds source locks, active screenshots, and colored-turnaround traits for review.", "Starter cat visual checklist is incomplete or stale.");
            Require(report, StarterCatTurnaroundConformanceSpecReady(starterCatTurnaroundConformanceSpec), "Starter cat turnaround conformance spec pins front, side, back, palette, prop, and drift anchors to the colored three-view turnarounds.", "Starter cat turnaround conformance spec is incomplete or stale.");
            Require(report, StarterCatProductionSpecReady(starterCatProductionSpec), "Starter cat asset-production specs define allowed derivatives, required evidence, strict prompt clauses, and rejection rules.", "Starter cat asset-production specs are incomplete or stale.");
            Require(report, StarterCatSourceLockPacketReady(starterCatSourceLockPacket), "Starter cat source-lock packet records turnaround hashes, locked sprite hashes, screenshots, and candidate review sheets.", "Starter cat source-lock packet is incomplete or stale.");
            Require(report, StarterCatTurnaroundComparisonAuditReady(starterCatTurnaroundComparisonAudit), "Starter cat turnaround comparison audit ties colored three-view sources to current Unity sprites and blocks import pending active-cat screenshots.", "Starter cat turnaround comparison audit is incomplete or stale.");
            Require(report, StarterCatReferencePlateReady(starterCatReferencePlates), "Starter cat source-turnaround reference plates provide front, side, and back hard visual inputs for future Codex image generation.", "Starter cat source-turnaround reference plates are incomplete or stale.");
            Require(report, StarterCatUnityReferenceInstallReady(starterCatUnityReferenceInstall), "Starter cat Unity reference atlases are installed as source-derived debug references without replacing runtime combat art.", "Starter cat Unity reference installs are incomplete or unsafe.");
            Require(report, StarterCatRuntimeCombatSpriteAuditReady(starterCatRuntimeCombatSpriteAudit), "Starter cat runtime combat sprite audit binds current runtime sprites to source locks, Batch 70 front plates, and runtime binding ids.", "Starter cat runtime combat sprite audit is incomplete or stale.");
            Require(report, StarterCatStrictCandidateReady(starterCatStrictCandidates), "Starter cat strict candidate evidence records Batch 49/50/51 candidates and keeps Unity import blocked until active-cat screenshots pass.", "Starter cat strict candidate evidence is incomplete or stale.");
            Require(report, StarterCatFormalImportReady(starterCatFormalImport), "Starter cat formal import gate has an explicit blocked-or-approved decision tied to review notes and active screenshots.", "Starter cat formal import gate is invalid or missing.");
            Require(report, AssetProductionQueueReady(assetProductionQueue), "Asset production queue separates Codex candidate production from Unity validation and formal install work.", "Asset production queue is incomplete or stale.");
            Require(report, RuntimeBindingReviewReady(report.Entries, runtimeBindings), "Every runtime visual binding is represented in the asset review packet.", "Runtime visual bindings are not fully represented in the asset review packet.");
            return report;
        }

        private static bool StarterCatVisualChecklistReady(P0StarterCatVisualConsistencyReport report)
        {
            return report != null
                && report.IsReady
                && report.ChecklistCount == 3
                && report.RequiredTraitCount >= 15
                && report.ScreenshotPlanMatchedCount == 3
                && report.SourceLockMatchedCount == 3;
        }

        private static bool StarterCatProductionSpecReady(P0StarterCatAssetProductionSpecReport report)
        {
            return report != null
                && report.IsReady
                && report.SpecCount == 3
                && report.SourceLockMatchedCount == 3
                && report.AllowedDerivativeAssetTypeCount >= 12
                && report.RequiredEvidenceCount >= 24
                && report.PromptClauseCount >= 21
                && report.RejectionRuleCount >= 12;
        }

        private static bool StarterCatTurnaroundConformanceSpecReady(P0StarterCatTurnaroundConformanceSpecReport report)
        {
            return report != null
                && report.IsReady
                && report.SpecCount == 3
                && report.SourceLockMatchedCount == 3
                && report.ExistingSourceFileCount == 3
                && report.FrontViewAnchorCount >= 9
                && report.SideViewAnchorCount >= 9
                && report.BackViewAnchorCount >= 9
                && report.PaletteAnchorCount >= 9
                && report.PropAndCostumeAnchorCount >= 9
                && report.ProhibitedDriftRuleCount >= 12;
        }

        private static bool StarterCatSourceLockPacketReady(P0StarterCatSourceLockPacketReport report)
        {
            return report != null
                && report.IsReady
                && report.CatEntryCount == 3
                && report.SourceHashMentionCount == 3
                && report.SpriteHashMentionCount == 3
                && report.ActiveScreenshotMentionCount == 3
                && report.CandidateReviewSheetMentionCount == 3;
        }

        private static bool StarterCatTurnaroundComparisonAuditReady(P0StarterCatTurnaroundComparisonAuditReport report)
        {
            return report != null
                && report.IsReady
                && report.ArtifactCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount
                && report.ManifestRowCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount
                && report.SourceLockMentionCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount
                && report.SourcePathMentionCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount
                && report.SpritePathMentionCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount
                && report.ActiveScreenshotMentionCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount
                && report.RecommendationMentionCount == P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount
                && report.MetaFileCount == 0;
        }

        private static bool StarterCatReferencePlateReady(P0StarterCatReferencePlateReport report)
        {
            return report != null
                && report.IsReady
                && report.ArtifactCount == P0StarterCatReferencePlateEvidence.ExpectedArtifactCount
                && report.PlateCount == P0StarterCatReferencePlateEvidence.ExpectedPlateCount
                && report.ManifestRowCount == P0StarterCatReferencePlateEvidence.ExpectedPlateCount
                && report.SourceLockMentionCount == P0StarterCatReferencePlateEvidence.ExpectedStarterCatCount
                && report.SourcePathMentionCount == P0StarterCatReferencePlateEvidence.ExpectedStarterCatCount
                && report.ViewRowCount == P0StarterCatReferencePlateEvidence.ExpectedPlateCount
                && report.ImportBlockCount == P0StarterCatReferencePlateEvidence.ExpectedPlateCount
                && report.MetaFileCount == 0;
        }

        private static bool StarterCatUnityReferenceInstallReady(P0StarterCatUnityReferenceInstallReport report)
        {
            return report != null
                && report.IsReady
                && report.ArtifactCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount
                && report.InstalledAssetCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount
                && report.ManifestRowCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedManifestRowCount
                && report.SourceLockMentionCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedSourceLockMentionCount
                && report.ReferencePlateMentionCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedReferencePlateMentionCount
                && report.ImportSettingTokenCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedImportSettingTokenCount
                && report.RuntimeBlockMentionCount == P0StarterCatUnityReferenceInstallEvidence.ExpectedRuntimeBlockMentionCount;
        }

        private static bool StarterCatRuntimeCombatSpriteAuditReady(P0StarterCatRuntimeCombatSpriteAuditReport report)
        {
            return report != null
                && report.IsReady
                && report.ArtifactCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount
                && report.ManifestRowCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount
                && report.RuntimeSpriteCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount
                && report.RuntimeSpriteMetaCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount
                && report.SourceLockMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount
                && report.SourceTurnaroundPathMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount
                && report.FrontPlateMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount
                && report.RuntimeBindingMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeBindingMentionCount
                && report.ImportBlockMentionCount == P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedImportBlockMentionCount;
        }

        private static bool StarterCatFormalImportReady(P0StarterCatFormalImportReadinessReport report)
        {
            return report != null
                && report.IsGateValid
                && report.ReviewNoteCount == P0StarterCatFormalImportReadiness.ExpectedStarterCatCount
                && (report.State == P0StarterCatFormalImportState.Blocked || report.IsImportAllowed);
        }

        private static bool StarterCatStrictCandidateReady(P0StarterCatStrictCandidateEvidenceReport report)
        {
            return report != null
                && report.IsReady
                && report.CandidateCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.ManifestCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.AlphaCandidateCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.ReviewSheetCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.ReviewNoteCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.AgentPromptCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.ExplicitBlockNoteCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount
                && report.ActiveScreenshotMentionCount == P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount;
        }

        private static bool AssetProductionQueueReady(P0AssetProductionQueueCoverageReport report)
        {
            return report != null
                && report.IsReady
                && report.QueueCount == P0AssetProductionQueueCatalog.ExpectedP0QueueCount
                && report.CodexRunnableCount == P0AssetProductionQueueCatalog.ExpectedCodexRunnableCount
                && report.CandidatePackCompletePendingUnityReviewCount == P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount
                && report.UnityBlockedCount == P0AssetProductionQueueCatalog.ExpectedUnityBlockedCount;
        }

        private static Dictionary<string, P0HardReferenceSourceLockEntry> BuildSourceLockMap(
            IReadOnlyList<P0HardReferenceSourceLockEntry> sourceLocks,
            P0AssetReviewPacketReport report)
        {
            Dictionary<string, P0HardReferenceSourceLockEntry> result = new Dictionary<string, P0HardReferenceSourceLockEntry>(StringComparer.Ordinal);
            if (sourceLocks == null)
            {
                report.AddIssue(P0AssetReviewPacketSeverity.Failure, "Hard reference source locks are missing.");
                return result;
            }

            for (int i = 0; i < sourceLocks.Count; i++)
            {
                P0HardReferenceSourceLockEntry current = sourceLocks[i];
                if (!result.ContainsKey(current.LockId))
                {
                    result.Add(current.LockId, current);
                }
            }

            return result;
        }

        private static Dictionary<string, List<string>> BuildRuntimeBindingMap(IReadOnlyList<P0VisualAssetBinding> runtimeBindings)
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            if (runtimeBindings == null)
            {
                return result;
            }

            for (int i = 0; i < runtimeBindings.Count; i++)
            {
                P0VisualAssetBinding binding = runtimeBindings[i];
                string assetId = binding.Asset.AssetId;
                if (string.IsNullOrWhiteSpace(assetId))
                {
                    continue;
                }

                if (!result.TryGetValue(assetId, out List<string> bindings))
                {
                    bindings = new List<string>();
                    result.Add(assetId, bindings);
                }

                bindings.Add(binding.BindingId);
            }

            return result;
        }

        private static List<string> ResolveSourcePaths(
            P0AssetManifestEntry entry,
            Dictionary<string, P0HardReferenceSourceLockEntry> sourceLockById,
            P0AssetReviewPacketReport report,
            ref bool allSourceLocksResolve)
        {
            List<string> sourcePaths = new List<string>();
            for (int i = 0; i < entry.SourceLockIds.Count; i++)
            {
                string lockId = entry.SourceLockIds[i];
                if (sourceLockById.TryGetValue(lockId, out P0HardReferenceSourceLockEntry sourceLock))
                {
                    sourcePaths.Add(sourceLock.Path);
                    continue;
                }

                allSourceLocksResolve = false;
                report.AddIssue(P0AssetReviewPacketSeverity.Failure, entry.AssetId + " references unknown source lock " + lockId + ".");
            }

            return sourcePaths;
        }

        private static List<string> ResolveRuntimeBindings(
            string assetId,
            Dictionary<string, List<string>> runtimeBindingsByAssetId)
        {
            if (runtimeBindingsByAssetId.TryGetValue(assetId, out List<string> bindings))
            {
                return new List<string>(bindings);
            }

            return new List<string>();
        }

        private static bool StarterCatReviewReady(IReadOnlyList<P0AssetReviewPacketEntry> entries)
        {
            return StarterCatEntryReady(entries, P0PrototypeCatalog.SaibanId, "saiban_turnaround_colored")
                && StarterCatEntryReady(entries, P0PrototypeCatalog.NephthysId, "nephthys_turnaround_colored")
                && StarterCatEntryReady(entries, P0PrototypeCatalog.SuzuneId, "suzune_turnaround_colored");
        }

        private static bool StarterCatEntryReady(
            IReadOnlyList<P0AssetReviewPacketEntry> entries,
            string catId,
            string expectedSourceLockId)
        {
            P0AssetReviewPacketEntry entry = FindBySubject(entries, catId);
            return entry != null
                && Contains(entry.SourceLockIds, expectedSourceLockId)
                && entry.SourcePaths.Count > 0
                && entry.RuntimeBindingIds.Count > 0
                && ContainsText(entry.ConsistencyNotes, "colored turnaround")
                && ContainsText(entry.ConsistencyNotes, "hard reference");
        }

        private static bool RuntimeBindingReviewReady(
            IReadOnlyList<P0AssetReviewPacketEntry> entries,
            IReadOnlyList<P0VisualAssetBinding> runtimeBindings)
        {
            if (runtimeBindings == null || runtimeBindings.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < runtimeBindings.Count; i++)
            {
                P0VisualAssetBinding binding = runtimeBindings[i];
                P0AssetReviewPacketEntry entry = FindByAssetId(entries, binding.Asset.AssetId);
                if (entry == null || !Contains(entry.RuntimeBindingIds, binding.BindingId))
                {
                    return false;
                }
            }

            return true;
        }

        private static P0AssetReviewPacketEntry FindBySubject(IReadOnlyList<P0AssetReviewPacketEntry> entries, string subjectId)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].SubjectId == subjectId)
                {
                    return entries[i];
                }
            }

            return null;
        }

        private static P0AssetReviewPacketEntry FindByAssetId(IReadOnlyList<P0AssetReviewPacketEntry> entries, string assetId)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].AssetId == assetId)
                {
                    return entries[i];
                }
            }

            return null;
        }

        private static bool Contains(IReadOnlyList<string> values, string expected)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsText(string value, string token)
        {
            return value != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0AssetReviewPacketReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetReviewPacketSeverity.Failure, failureMessage);
        }
    }
}
