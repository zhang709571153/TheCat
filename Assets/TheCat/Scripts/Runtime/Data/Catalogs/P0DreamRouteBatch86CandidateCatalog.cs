using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0DreamRouteBatch86CandidateAsset
    {
        public P0DreamRouteBatch86CandidateAsset(
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

    public static class P0DreamRouteBatch86CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 6;
        public const string BatchSlug = "batch_86_dream_route_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/dream_route/batch_86_dream_route_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/DreamRoute";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_dream_route_batch86_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch86 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0DreamRouteBatch86CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_dream_route_route_map_panel_frame_1120x640_candidate_v001",
                    "dream_route_panel",
                    "route_map_panel_frame",
                    "1120x640",
                    "large textless dream-route map panel frame"),
                Candidate(
                    "thecat_ui_dream_route_route_layer_header_frame_760x96_candidate_v001",
                    "dream_route_header",
                    "route_layer_header_frame",
                    "760x96",
                    "textless route layer header frame"),
                Candidate(
                    "thecat_ui_dream_route_route_node_socket_frame_192x192_candidate_v001",
                    "dream_route_node",
                    "route_node_socket_frame",
                    "192x192",
                    "route node socket frame for icon overlays"),
                Candidate(
                    "thecat_ui_dream_route_route_choice_card_slot_440x220_candidate_v001",
                    "dream_route_choice",
                    "route_choice_card_slot",
                    "440x220",
                    "route choice card slot frame"),
                Candidate(
                    "thecat_ui_dream_route_route_path_ribbon_frame_640x96_candidate_v001",
                    "dream_route_path",
                    "route_path_ribbon_frame",
                    "640x96",
                    "route path ribbon frame"),
                Candidate(
                    "thecat_ui_dream_route_route_boss_gate_frame_360x260_candidate_v001",
                    "dream_route_boss",
                    "route_boss_gate_frame",
                    "360x260",
                    "boss gate pressure frame")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0DreamRouteBatch86CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch86." + candidate.ComponentId + "." + candidate.VariantId,
                    "dream_route_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0DreamRouteBatch86CandidateAsset Candidate(
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
                "Batch 86 dream-route candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity dream-entry and route-map screenshots, text replacement, node/path semantics, 1024x768 route-card density, boss gate scale, route-card click targets, binding, Console checks, and review approval.");
            return new P0DreamRouteBatch86CandidateAsset(entry, componentId, variantId, sourcePath);
        }

        private static IReadOnlyList<string> ReferenceAssets()
        {
            return new[]
            {
                P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId,
                P0VisualAssetCatalog.TitleLogoId,
                P0VisualAssetCatalog.DreamGlassPanelId,
                P0VisualAssetCatalog.PrimaryButtonId,
                P0VisualAssetCatalog.PartnerRouteCardFrameId,
                P0VisualAssetCatalog.ShopRouteCardFrameId,
                P0VisualAssetCatalog.BlessingRouteCardFrameId,
                P0VisualAssetCatalog.DreamEventRouteCardFrameId,
                P0VisualAssetCatalog.RestNestRouteCardFrameId,
                P0VisualAssetCatalog.BossRouteNodeIconId,
                P0VisualAssetCatalog.DefenseRouteNodeIconId,
                P0VisualAssetCatalog.EliteRouteNodeIconId,
                P0VisualAssetCatalog.PartnerRouteNodeIconId,
                P0VisualAssetCatalog.ShopRouteNodeIconId,
                P0VisualAssetCatalog.DreamEventRouteNodeIconId,
                P0VisualAssetCatalog.BlessingRouteNodeIconId,
                P0VisualAssetCatalog.RestNestRouteNodeIconId
            };
        }

        private static IReadOnlyList<string> SourceLockAssets()
        {
            return new[]
            {
                "batch_86_dream_route_preflight_candidates",
                "batch_65_route_map_readability_candidates",
                "p0_route_map_surface",
                "p0_dream_entry_surface",
                "p0_asset_batch_08_ui_shell"
            };
        }
    }
}

