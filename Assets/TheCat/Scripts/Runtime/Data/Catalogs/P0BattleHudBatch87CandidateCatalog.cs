using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0BattleHudBatch87CandidateAsset
    {
        public P0BattleHudBatch87CandidateAsset(
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

    public static class P0BattleHudBatch87CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 6;
        public const string BatchSlug = "batch_87_battle_hud_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/battle_hud/batch_87_battle_hud_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/BattleHUD";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_battle_hud_batch87_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch87 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0BattleHudBatch87CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_battle_hud_battle_top_resource_rail_frame_1240x144_candidate_v001",
                    "battle_hud_top",
                    "battle_top_resource_rail_frame",
                    "1240x144",
                    "top resource rail for HP, sleep, poop, and hunger gauges"),
                Candidate(
                    "thecat_ui_battle_hud_battle_cat_party_panel_520x188_candidate_v001",
                    "battle_hud_party",
                    "battle_cat_party_panel",
                    "520x188",
                    "cat party avatar and status frame"),
                Candidate(
                    "thecat_ui_battle_hud_battle_enemy_status_panel_520x156_candidate_v001",
                    "battle_hud_enemy",
                    "battle_enemy_status_panel",
                    "520x156",
                    "enemy target and warning status frame"),
                Candidate(
                    "thecat_ui_battle_hud_battle_skill_tray_frame_900x180_candidate_v001",
                    "battle_hud_skill",
                    "battle_skill_tray_frame",
                    "900x180",
                    "bottom skill tray frame"),
                Candidate(
                    "thecat_ui_battle_hud_battle_status_chip_strip_480x96_candidate_v001",
                    "battle_hud_status",
                    "battle_status_chip_strip",
                    "480x96",
                    "status chip strip frame"),
                Candidate(
                    "thecat_ui_battle_hud_battle_runtime_control_cluster_360x96_candidate_v001",
                    "battle_hud_runtime",
                    "battle_runtime_control_cluster",
                    "360x96",
                    "pause, speed, and restart cluster frame")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0BattleHudBatch87CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch87." + candidate.ComponentId,
                    "battle_hud_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0BattleHudBatch87CandidateAsset Candidate(
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
                Empty(),
                unityPath,
                size,
                CandidateStatus,
                "Batch 87 battle HUD candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by runtime screenshots, Console checks, and review approval.");
            return new P0BattleHudBatch87CandidateAsset(entry, componentId, variantId, sourcePath);
        }

        private static IReadOnlyList<string> ReferenceAssets()
        {
            return new[]
            {
                P0VisualAssetCatalog.DreamGlassPanelId,
                P0VisualAssetCatalog.PrimaryButtonId,
                P0VisualAssetCatalog.OwnerSleepGaugeFrameId,
                P0VisualAssetCatalog.OwnerSleepGaugeFillId,
                P0VisualAssetCatalog.CatHpGaugeFrameId,
                P0VisualAssetCatalog.CatHpGaugeFillId,
                P0VisualAssetCatalog.TeamPoopGaugeFrameId,
                P0VisualAssetCatalog.TeamPoopGaugeFillId,
                P0VisualAssetCatalog.TeamHungerGaugeFrameId,
                P0VisualAssetCatalog.TeamHungerGaugeFillId,
                P0VisualAssetCatalog.SkillReadyFrameId,
                P0VisualAssetCatalog.SkillCooldownOverlayId,
                P0VisualAssetCatalog.SkillNoTargetMarkerId,
                P0VisualAssetCatalog.SkillHungerCostChipId
            };
        }

        private static IReadOnlyList<string> Empty()
        {
            return new string[0];
        }
    }
}
