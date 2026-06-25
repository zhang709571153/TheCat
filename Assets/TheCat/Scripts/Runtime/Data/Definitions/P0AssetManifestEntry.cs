using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public sealed class P0AssetManifestEntry
    {
        public P0AssetManifestEntry(
            string assetId,
            string subjectId,
            string assetType,
            string priority,
            string sourcePromptPath,
            IReadOnlyList<string> referenceAssetIds,
            string unityImportPath,
            string size,
            string status,
            string consistencyNotes)
            : this(
                assetId,
                subjectId,
                assetType,
                priority,
                sourcePromptPath,
                referenceAssetIds,
                Array.Empty<string>(),
                unityImportPath,
                size,
                status,
                consistencyNotes)
        {
        }

        public P0AssetManifestEntry(
            string assetId,
            string subjectId,
            string assetType,
            string priority,
            string sourcePromptPath,
            IReadOnlyList<string> referenceAssetIds,
            IReadOnlyList<string> sourceLockIds,
            string unityImportPath,
            string size,
            string status,
            string consistencyNotes)
        {
            RequireText(assetId, nameof(assetId));
            RequireText(subjectId, nameof(subjectId));
            RequireText(assetType, nameof(assetType));
            RequireText(priority, nameof(priority));
            RequireText(sourcePromptPath, nameof(sourcePromptPath));
            RequireText(unityImportPath, nameof(unityImportPath));
            RequireText(size, nameof(size));
            RequireText(status, nameof(status));
            RequireText(consistencyNotes, nameof(consistencyNotes));

            AssetId = assetId;
            SubjectId = subjectId;
            AssetType = assetType;
            Priority = priority;
            SourcePromptPath = sourcePromptPath;
            ReferenceAssetIds = referenceAssetIds == null
                ? Array.Empty<string>()
                : new List<string>(referenceAssetIds).AsReadOnly();
            SourceLockIds = sourceLockIds == null
                ? Array.Empty<string>()
                : new List<string>(sourceLockIds).AsReadOnly();
            UnityImportPath = unityImportPath;
            Size = size;
            Status = status;
            ConsistencyNotes = consistencyNotes;
        }

        public string AssetId { get; }

        public string SubjectId { get; }

        public string AssetType { get; }

        public string Priority { get; }

        public string SourcePromptPath { get; }

        public IReadOnlyList<string> ReferenceAssetIds { get; }

        public IReadOnlyList<string> SourceLockIds { get; }

        public string UnityImportPath { get; }

        public string Size { get; }

        public string Status { get; }

        public string ConsistencyNotes { get; }

        public string BuildSummary()
        {
            return AssetId
                + " "
                + SubjectId
                + " "
                + AssetType
                + " "
                + Priority
                + " "
                + Status
                + " -> "
                + UnityImportPath;
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
