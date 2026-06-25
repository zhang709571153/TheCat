using System;

namespace TheCat.Data.Definitions
{
    public readonly struct P0VisualAssetBinding
    {
        public P0VisualAssetBinding(
            string bindingId,
            string surfaceId,
            string subjectId,
            string slotId,
            P0VisualAssetReference asset,
            string sourceAuthority)
        {
            BindingId = bindingId ?? string.Empty;
            SurfaceId = surfaceId ?? string.Empty;
            SubjectId = subjectId ?? string.Empty;
            SlotId = slotId ?? string.Empty;
            Asset = asset;
            SourceAuthority = sourceAuthority ?? string.Empty;
        }

        public string BindingId { get; }

        public string SurfaceId { get; }

        public string SubjectId { get; }

        public string SlotId { get; }

        public P0VisualAssetReference Asset { get; }

        public string SourceAuthority { get; }

        public bool IsReady => !string.IsNullOrWhiteSpace(BindingId)
            && !string.IsNullOrWhiteSpace(SurfaceId)
            && !string.IsNullOrWhiteSpace(SubjectId)
            && !string.IsNullOrWhiteSpace(SlotId)
            && Asset.HasAsset;

        public string BuildSummary()
        {
            return IsReady
                ? BindingId + " " + SurfaceId + "/" + SlotId + " -> " + Asset.AssetId + " [" + SourceAuthority + "]"
                : "visual binding: incomplete";
        }
    }
}
