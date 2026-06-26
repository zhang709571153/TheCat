using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0SkillSelectionBatch89CandidateAsset
    {
        public P0SkillSelectionBatch89CandidateAsset(
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

    public static class P0SkillSelectionBatch89CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 8;
        public const string BatchSlug = "batch_89_skill_selection_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/skill_selection/batch_89_skill_selection_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/SkillSelection";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_skill_selection_batch89_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch89 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_skill_selection_skill_selection_panel_frame_1180x640_candidate_v001",
                    "skill_selection_panel",
                    "skill_selection_panel_frame",
                    "1180x640",
                    "main skill-selection panel frame"),
                Candidate(
                    "thecat_ui_skill_selection_skill_choice_card_selected_420x240_candidate_v001",
                    "skill_selection_card",
                    "skill_choice_card_selected",
                    "420x240",
                    "selected skill choice card"),
                Candidate(
                    "thecat_ui_skill_selection_skill_choice_card_ready_420x240_candidate_v001",
                    "skill_selection_card",
                    "skill_choice_card_ready",
                    "420x240",
                    "ready skill choice card"),
                Candidate(
                    "thecat_ui_skill_selection_skill_choice_card_disabled_420x240_candidate_v001",
                    "skill_selection_card",
                    "skill_choice_card_disabled",
                    "420x240",
                    "disabled or unavailable skill choice card"),
                Candidate(
                    "thecat_ui_skill_selection_skill_choice_card_locked_420x240_candidate_v001",
                    "skill_selection_card",
                    "skill_choice_card_locked",
                    "420x240",
                    "locked skill choice card"),
                Candidate(
                    "thecat_ui_skill_selection_skill_detail_panel_frame_760x320_candidate_v001",
                    "skill_selection_detail",
                    "skill_detail_panel_frame",
                    "760x320",
                    "skill detail panel frame"),
                Candidate(
                    "thecat_ui_skill_selection_skill_cost_cooldown_strip_420x96_candidate_v001",
                    "skill_selection_state_strip",
                    "skill_cost_cooldown_strip",
                    "420x96",
                    "cost, cooldown, low-resource, and no-target state strip"),
                Candidate(
                    "thecat_ui_skill_selection_skill_confirm_button_frame_420x112_candidate_v001",
                    "skill_selection_confirm",
                    "skill_confirm_button_frame",
                    "420x112",
                    "confirm skill choice button frame")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0SkillSelectionBatch89CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch89." + candidate.ComponentId + "." + candidate.VariantId,
                    "skill_selection_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0SkillSelectionBatch89CandidateAsset Candidate(
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
                "Batch 89 skill-selection candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity screenshots, state semantics, click-target proof, Console checks, and review approval.");
            return new P0SkillSelectionBatch89CandidateAsset(entry, componentId, variantId, sourcePath);
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
                "batch_80_starter_skill_icon_motifs",
                "batch_81_skill_slot_frame_candidates",
                "batch_82_common_ui_state_candidates"
            };
        }
    }
}
