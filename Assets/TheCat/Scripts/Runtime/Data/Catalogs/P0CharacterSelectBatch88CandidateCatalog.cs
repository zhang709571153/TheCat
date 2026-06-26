using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0CharacterSelectBatch88CandidateAsset
    {
        public P0CharacterSelectBatch88CandidateAsset(
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

    public static class P0CharacterSelectBatch88CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 6;
        public const string BatchSlug = "batch_88_character_select_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/character_select/batch_88_character_select_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/CharacterSelect";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_character_select_batch88_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch88 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_character_select_character_select_stage_panel_1180x640_candidate_v001",
                    "character_select_shell",
                    "character_select_stage_panel",
                    "1180x640",
                    "wide stage panel for three starter-card layout"),
                Candidate(
                    "thecat_ui_character_select_starter_card_frame_selected_360x500_candidate_v001",
                    "character_select_card",
                    "starter_card_frame_selected",
                    "360x500",
                    "selected starter-card frame"),
                Candidate(
                    "thecat_ui_character_select_starter_card_frame_idle_360x500_candidate_v001",
                    "character_select_card",
                    "starter_card_frame_idle",
                    "360x500",
                    "idle starter-card frame"),
                Candidate(
                    "thecat_ui_character_select_starter_role_chip_strip_360x96_candidate_v001",
                    "character_select_chip",
                    "starter_role_chip_strip",
                    "360x96",
                    "role and skill chip strip"),
                Candidate(
                    "thecat_ui_character_select_starter_ready_badge_220x80_candidate_v001",
                    "character_select_badge",
                    "starter_ready_badge",
                    "220x80",
                    "ready-state badge"),
                Candidate(
                    "thecat_ui_character_select_starter_launch_button_frame_420x112_candidate_v001",
                    "character_select_button",
                    "starter_launch_button_frame",
                    "420x112",
                    "start-selected-route button frame")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0CharacterSelectBatch88CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch88." + candidate.ComponentId + "." + candidate.VariantId,
                    "character_select_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0CharacterSelectBatch88CandidateAsset Candidate(
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
                "Batch 88 character-select candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity screenshots, Console checks, click-target proof, and review approval.");
            return new P0CharacterSelectBatch88CandidateAsset(entry, componentId, variantId, sourcePath);
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
                P0VisualAssetCatalog.SaibanHudAvatarId,
                P0VisualAssetCatalog.NephthysHudAvatarId,
                P0VisualAssetCatalog.SuzuneHudAvatarId
            };
        }
    }
}
