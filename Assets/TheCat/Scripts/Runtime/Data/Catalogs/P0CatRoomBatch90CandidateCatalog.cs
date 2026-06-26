using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0CatRoomBatch90CandidateAsset
    {
        public P0CatRoomBatch90CandidateAsset(
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

    public static class P0CatRoomBatch90CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 6;
        public const string BatchSlug = "batch_90_cat_room_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/cat_room/batch_90_cat_room_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/CatRoom";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_cat_room_batch90_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch90 candidate-backed Unity cat-room preflight; not formal runtime binding";

        public static IReadOnlyList<P0CatRoomBatch90CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_cat_room_cat_room_stage_panel_frame_1180x640_candidate_v001",
                    "cat_room_stage_panel",
                    "cat_room_stage_panel_frame",
                    "1180x640",
                    "wide cat-room stage panel frame"),
                Candidate(
                    "thecat_ui_cat_room_cat_room_status_rail_frame_960x120_candidate_v001",
                    "cat_room_status_rail",
                    "cat_room_status_rail_frame",
                    "960x120",
                    "status rail frame for hunger, sleep, litter, mood, and HP readouts"),
                Candidate(
                    "thecat_ui_cat_room_cat_room_interaction_card_slot_440x180_candidate_v001",
                    "cat_room_interaction_card",
                    "cat_room_interaction_card_slot",
                    "440x180",
                    "interaction card slot for bed, feeder, litter, and dream entrance actions"),
                Candidate(
                    "thecat_ui_cat_room_cat_room_prop_hotspot_frame_256x256_candidate_v001",
                    "cat_room_prop_hotspot",
                    "cat_room_prop_hotspot_frame",
                    "256x256",
                    "prop hotspot frame for room object focus and click-target proof"),
                Candidate(
                    "thecat_ui_cat_room_cat_room_dream_entrance_button_frame_420x112_candidate_v001",
                    "cat_room_dream_entrance",
                    "cat_room_dream_entrance_button_frame",
                    "420x112",
                    "dream entrance button frame"),
                Candidate(
                    "thecat_ui_cat_room_cat_room_hover_disabled_prompt_chip_360x96_candidate_v001",
                    "cat_room_prompt_chip",
                    "cat_room_hover_disabled_prompt_chip",
                    "360x96",
                    "hover, disabled, blocked, and range prompt chip")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0CatRoomBatch90CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch90." + candidate.ComponentId + "." + candidate.VariantId,
                    "cat_room_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0CatRoomBatch90CandidateAsset Candidate(
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
                "Batch 90 cat-room candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity screenshots, bed/feeder/litter/dream entrance state proof, click-target validation, Console checks, and review approval.");
            return new P0CatRoomBatch90CandidateAsset(entry, componentId, variantId, sourcePath);
        }

        private static IReadOnlyList<string> ReferenceAssets()
        {
            return new[]
            {
                P0VisualAssetCatalog.BedroomDreamBattleBackgroundId,
                P0VisualAssetCatalog.DreamGlassPanelId,
                P0VisualAssetCatalog.PrimaryButtonId,
                P0VisualAssetCatalog.TeamHungerIconId,
                P0VisualAssetCatalog.OwnerSleepIconId,
                P0VisualAssetCatalog.TeamPoopIconId,
                P0VisualAssetCatalog.CatHpIconId
            };
        }

        private static IReadOnlyList<string> SourceLockAssets()
        {
            return new[]
            {
                "batch_54_bedroom_interactable_candidates_2026-06-15",
                "batch_67_bedroom_interaction_affordance_candidates_2026-06-15",
                "batch_82_common_ui_state_candidates",
                "p0_cat_room_surface"
            };
        }
    }
}
