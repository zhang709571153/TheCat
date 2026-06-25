using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public readonly struct P0VisualAssetReference
    {
        private readonly IReadOnlyList<string> referenceAssetIds;
        private readonly IReadOnlyList<string> sourceLockIds;

        public P0VisualAssetReference(
            string assetId,
            string unityImportPath,
            string assetType,
            string status,
            IReadOnlyList<string> referenceAssetIds,
            IReadOnlyList<string> sourceLockIds)
        {
            AssetId = assetId ?? string.Empty;
            UnityImportPath = unityImportPath ?? string.Empty;
            AssetType = assetType ?? string.Empty;
            Status = status ?? string.Empty;
            this.referenceAssetIds = referenceAssetIds == null
                ? Array.Empty<string>()
                : new List<string>(referenceAssetIds).AsReadOnly();
            this.sourceLockIds = sourceLockIds == null
                ? Array.Empty<string>()
                : new List<string>(sourceLockIds).AsReadOnly();
        }

        public string AssetId { get; }

        public string UnityImportPath { get; }

        public string AssetType { get; }

        public string Status { get; }

        public IReadOnlyList<string> ReferenceAssetIds => referenceAssetIds ?? Array.Empty<string>();

        public IReadOnlyList<string> SourceLockIds => sourceLockIds ?? Array.Empty<string>();

        public bool HasAsset => !string.IsNullOrWhiteSpace(AssetId)
            && !string.IsNullOrWhiteSpace(UnityImportPath);

        public bool RequiresWorkspaceFile => P0AssetManifestStatus.RequiresWorkspaceFile(Status);

        public string BuildSummary()
        {
            return HasAsset
                ? AssetId + " " + AssetType + " " + Status + " -> " + UnityImportPath
                : "visual asset: none";
        }
    }
}
