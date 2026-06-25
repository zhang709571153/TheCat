using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public sealed class P0AssetProductionQueueEntry
    {
        public P0AssetProductionQueueEntry(
            string queueId,
            int priority,
            string displayName,
            string subjectGroup,
            P0AssetProductionQueuePhase phase,
            P0AssetProductionQueueState state,
            string executionPromptPath,
            string candidateDirectory,
            string unityImportRoot,
            IReadOnlyList<string> relatedBatchSlugs,
            IReadOnlyList<string> sourceLockIds,
            IReadOnlyList<string> requiredEvidence,
            IReadOnlyList<string> forbiddenWriteRoots,
            string nextAction)
        {
            RequireText(queueId, nameof(queueId));
            RequireText(displayName, nameof(displayName));
            RequireText(subjectGroup, nameof(subjectGroup));
            RequireText(executionPromptPath, nameof(executionPromptPath));
            RequireText(candidateDirectory, nameof(candidateDirectory));
            RequireText(unityImportRoot, nameof(unityImportRoot));
            RequireText(nextAction, nameof(nextAction));

            QueueId = queueId;
            Priority = priority;
            DisplayName = displayName;
            SubjectGroup = subjectGroup;
            Phase = phase;
            State = state;
            ExecutionPromptPath = executionPromptPath;
            CandidateDirectory = candidateDirectory;
            UnityImportRoot = unityImportRoot;
            RelatedBatchSlugs = Copy(relatedBatchSlugs);
            SourceLockIds = Copy(sourceLockIds);
            RequiredEvidence = Copy(requiredEvidence);
            ForbiddenWriteRoots = Copy(forbiddenWriteRoots);
            NextAction = nextAction;
        }

        public string QueueId { get; }

        public int Priority { get; }

        public string DisplayName { get; }

        public string SubjectGroup { get; }

        public P0AssetProductionQueuePhase Phase { get; }

        public P0AssetProductionQueueState State { get; }

        public string ExecutionPromptPath { get; }

        public string CandidateDirectory { get; }

        public string UnityImportRoot { get; }

        public IReadOnlyList<string> RelatedBatchSlugs { get; }

        public IReadOnlyList<string> SourceLockIds { get; }

        public IReadOnlyList<string> RequiredEvidence { get; }

        public IReadOnlyList<string> ForbiddenWriteRoots { get; }

        public string NextAction { get; }

        public bool IsCodexRunnable => Phase == P0AssetProductionQueuePhase.CodexCandidateProduction
            && State == P0AssetProductionQueueState.ReadyForCodexCandidateProduction;

        public bool IsCandidatePackCompletePendingUnityReview => State == P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview;

        public bool IsUnityBlocked => State == P0AssetProductionQueueState.BlockedByUnityValidation;

        public string BuildSummary()
        {
            return QueueId + " [" + Priority + "] " + DisplayName + " -> " + State;
        }

        private static IReadOnlyList<string> Copy(IReadOnlyList<string> values)
        {
            return values == null
                ? Array.Empty<string>()
                : new List<string>(values).AsReadOnly();
        }

        private static void RequireText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", name);
            }
        }
    }
}
