using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0LoadingStartBatch83CandidateAsset
    {
        public P0LoadingStartBatch83CandidateAsset(
            P0AssetManifestEntry manifestEntry,
            string componentId,
            string variantId,
            string sourceCandidatePath)
        {
            ManifestEntry = manifestEntry;
            ComponentId = componentId ?? string.Empty;
            VariantId = variantId ?? string.Empty;
            SourceCandidatePath = sourceCandidatePath ?? string.Empty;
        }

        public P0AssetManifestEntry ManifestEntry { get; }

        public string ComponentId { get; }

        public string VariantId { get; }

        public string SourceCandidatePath { get; }
    }

    public static class P0LoadingStartBatch83CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 4;
        public const string BatchSlug = "batch_83_loading_start_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/loading_start/batch_83_loading_start_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/LoadingStart";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_loading_start_batch83_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch83 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0LoadingStartBatch83CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_loading_progress_frame_640x48_candidate_v001",
                    "loading_progress",
                    "progress_frame",
                    "640x48",
                    "textless loading progress frame"),
                Candidate(
                    "thecat_ui_loading_progress_fill_640x48_candidate_v001",
                    "loading_progress",
                    "progress_fill",
                    "640x48",
                    "textless loading progress fill strip"),
                Candidate(
                    "thecat_ui_loading_spinner_crescent_128x128_candidate_v001",
                    "loading_spinner",
                    "spinner_crescent",
                    "128x128",
                    "sleep-crescent loading spinner symbol"),
                Candidate(
                    "thecat_ui_loading_dot_sequence_384x64_candidate_v001",
                    "loading_pulse",
                    "dot_sequence",
                    "384x64",
                    "five-step loading dot sequence")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0LoadingStartBatch83CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch83." + candidate.ComponentId + "." + candidate.VariantId,
                    "loading_start_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0LoadingStartBatch83CandidateAsset Candidate(
            string assetId,
            string componentId,
            string variantId,
            string size,
            string note)
        {
            string fileName = assetId + ".png";
            string sourcePath = SourceSpriteDirectory + "/" + fileName;
            string unityPath = UnityImportRoot + "/" + fileName;
            P0AssetManifestEntry entry = new P0AssetManifestEntry(
                assetId,
                componentId,
                "sprite",
                "P0",
                AgentPromptPath,
                ReferenceAssets(),
                SourceLockAssets(),
                unityPath,
                size,
                CandidateStatus,
                "Batch 83 loading/start candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity screenshots, spinner/state semantics, binding, Console checks, and review approval.");
            return new P0LoadingStartBatch83CandidateAsset(entry, componentId, variantId, sourcePath);
        }

        private static IReadOnlyList<string> ReferenceAssets()
        {
            return new[]
            {
                P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId,
                P0VisualAssetCatalog.DreamGlassPanelId,
                P0VisualAssetCatalog.PrimaryButtonId,
                P0VisualAssetCatalog.TitleLogoId
            };
        }

        private static IReadOnlyList<string> SourceLockAssets()
        {
            return new[]
            {
                "batch_83_loading_start_preflight_candidates",
                "batch_82_common_ui_state_candidates",
                "p0_loading_start_surface"
            };
        }
    }
}
