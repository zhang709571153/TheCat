using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0ResultSettlementBatch84CandidateAsset
    {
        public P0ResultSettlementBatch84CandidateAsset(
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

    public static class P0ResultSettlementBatch84CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 7;
        public const string BatchSlug = "batch_84_result_settlement_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/result_settlement/batch_84_result_settlement_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/ResultSettlement";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_result_settlement_batch84_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch84 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_result_settlement_panel_frame_960x540_candidate_v001",
                    "result_settlement_panel",
                    "panel_frame",
                    "960x540",
                    "large textless result/settlement panel frame"),
                Candidate(
                    "thecat_ui_result_settlement_reward_row_frame_760x96_candidate_v001",
                    "result_settlement_reward_row",
                    "reward_row_frame",
                    "760x96",
                    "textless reward row for icon plus Unity-rendered labels"),
                Candidate(
                    "thecat_ui_result_settlement_stat_chip_frame_256x72_candidate_v001",
                    "result_settlement_stat_chip",
                    "stat_chip_frame",
                    "256x72",
                    "compact stat chip frame"),
                Candidate(
                    "thecat_ui_result_settlement_action_button_frame_384x96_candidate_v001",
                    "result_settlement_action_button",
                    "action_button_frame",
                    "384x96",
                    "textless result/settlement action button frame"),
                Candidate(
                    "thecat_ui_result_settlement_outcome_divider_640x32_candidate_v001",
                    "result_settlement_divider",
                    "outcome_divider",
                    "640x32",
                    "thin outcome section divider"),
                Candidate(
                    "thecat_ui_result_settlement_success_stamp_ring_256x256_candidate_v001",
                    "result_settlement_stamp",
                    "success_stamp_ring",
                    "256x256",
                    "symbolic success outcome stamp ring"),
                Candidate(
                    "thecat_ui_result_settlement_failure_stamp_ring_256x256_candidate_v001",
                    "result_settlement_stamp",
                    "failure_stamp_ring",
                    "256x256",
                    "symbolic failure outcome stamp ring")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0ResultSettlementBatch84CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch84." + candidate.ComponentId + "." + candidate.VariantId,
                    "result_settlement_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0ResultSettlementBatch84CandidateAsset Candidate(
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
                "Batch 84 result/settlement candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity victory/defeat/settlement screenshots, text replacement, 1024x768 crowding, binding, Console checks, and review approval.");
            return new P0ResultSettlementBatch84CandidateAsset(entry, componentId, variantId, sourcePath);
        }

        private static IReadOnlyList<string> ReferenceAssets()
        {
            return new[]
            {
                P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId,
                P0VisualAssetCatalog.DreamGlassPanelId,
                P0VisualAssetCatalog.PrimaryButtonId,
                P0VisualAssetCatalog.BattleResultVictoryBannerId,
                P0VisualAssetCatalog.BattleResultDefeatBannerId,
                P0VisualAssetCatalog.SettlementRunClearedBannerId,
                P0VisualAssetCatalog.SettlementRunFailedBannerId,
                P0VisualAssetCatalog.DreamShardRewardIconId,
                P0VisualAssetCatalog.FishTreatRewardIconId,
                P0VisualAssetCatalog.OwnerSleepIconId,
                P0VisualAssetCatalog.CatHpIconId
            };
        }

        private static IReadOnlyList<string> SourceLockAssets()
        {
            return new[]
            {
                "batch_84_result_settlement_preflight_candidates",
                "p0_result_settlement_surface",
                "p0_asset_batch_08_ui_shell"
            };
        }
    }
}
