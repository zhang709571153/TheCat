using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public sealed class P0AssetGenerationBatchDefinition
    {
        public P0AssetGenerationBatchDefinition(
            string batchId,
            string displayName,
            string executionPromptPath,
            IReadOnlyList<string> assetIds,
            IReadOnlyList<string> requiredAssetIds)
        {
            RequireText(batchId, nameof(batchId));
            RequireText(displayName, nameof(displayName));
            RequireText(executionPromptPath, nameof(executionPromptPath));

            BatchId = batchId;
            DisplayName = displayName;
            ExecutionPromptPath = executionPromptPath;
            AssetIds = assetIds == null
                ? Array.Empty<string>()
                : new List<string>(assetIds).AsReadOnly();
            RequiredAssetIds = requiredAssetIds == null
                ? Array.Empty<string>()
                : new List<string>(requiredAssetIds).AsReadOnly();
        }

        public string BatchId { get; }

        public string DisplayName { get; }

        public string ExecutionPromptPath { get; }

        public IReadOnlyList<string> AssetIds { get; }

        public IReadOnlyList<string> RequiredAssetIds { get; }

        public string BuildSummary()
        {
            return BatchId + " " + DisplayName + " -> " + AssetIds.Count + " asset(s)";
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
