using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public readonly struct P0SettingsPauseBatch85CandidateAsset
    {
        public P0SettingsPauseBatch85CandidateAsset(
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

    public static class P0SettingsPauseBatch85CandidateCatalog
    {
        public const int ExpectedCandidateSpriteCount = 6;
        public const string BatchSlug = "batch_85_settings_pause_preflight_2026-06-25";
        public const string CandidateDirectory = "design/development/asset_candidates/ui/settings_screen/batch_85_settings_pause_preflight_2026-06-25";
        public const string SourceSpriteDirectory = CandidateDirectory + "/sprites";
        public const string UnityImportRoot = "Assets/TheCat/Art/UI/SettingsPause";
        public const string AgentPromptPath = CandidateDirectory + "/thecat_ui_settings_pause_batch85_agent_review_prompt.md";
        public const string CandidateStatus = P0AssetManifestStatus.Imported;
        public const string SourceAuthority = "Batch85 candidate-backed Unity preflight; not formal runtime binding";

        public static IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> CreateCandidates()
        {
            return new[]
            {
                Candidate(
                    "thecat_ui_settings_pause_screen_panel_frame_960x640_candidate_v001",
                    "settings_pause_panel",
                    "screen_panel_frame",
                    "960x640",
                    "large textless settings/pause screen panel frame"),
                Candidate(
                    "thecat_ui_settings_pause_tab_bar_frame_760x80_candidate_v001",
                    "settings_pause_tabs",
                    "tab_bar_frame",
                    "760x80",
                    "textless segmented settings tab bar frame"),
                Candidate(
                    "thecat_ui_settings_pause_option_row_frame_840x96_candidate_v001",
                    "settings_pause_option_row",
                    "option_row_frame",
                    "840x96",
                    "textless option row frame with icon and control wells"),
                Candidate(
                    "thecat_ui_settings_pause_confirm_modal_frame_720x420_candidate_v001",
                    "settings_pause_modal",
                    "confirm_modal_frame",
                    "720x420",
                    "restart confirmation modal frame"),
                Candidate(
                    "thecat_ui_settings_pause_key_hint_chip_frame_256x72_candidate_v001",
                    "settings_pause_hint",
                    "key_hint_chip_frame",
                    "256x72",
                    "keyboard and controller hint chip frame"),
                Candidate(
                    "thecat_ui_settings_pause_settings_section_divider_640x24_candidate_v001",
                    "settings_pause_divider",
                    "settings_section_divider",
                    "640x24",
                    "thin settings section divider")
            };
        }

        public static IReadOnlyList<P0AssetManifestEntry> CreateUnityPreflightManifest()
        {
            IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates = CreateCandidates();
            P0AssetManifestEntry[] entries = new P0AssetManifestEntry[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                entries[i] = candidates[i].ManifestEntry;
            }

            return entries;
        }

        public static P0VisualAssetBinding[] CreateUnityPreflightBindings()
        {
            IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates = CreateCandidates();
            P0VisualAssetBinding[] bindings = new P0VisualAssetBinding[candidates.Count];
            for (int i = 0; i < candidates.Count; i++)
            {
                P0SettingsPauseBatch85CandidateAsset candidate = candidates[i];
                P0AssetManifestEntry entry = candidate.ManifestEntry;
                P0VisualAssetReference asset = new P0VisualAssetReference(
                    entry.AssetId,
                    entry.UnityImportPath,
                    entry.AssetType,
                    entry.Status,
                    entry.ReferenceAssetIds,
                    entry.SourceLockIds);
                bindings[i] = new P0VisualAssetBinding(
                    "batch85." + candidate.ComponentId + "." + candidate.VariantId,
                    "settings_pause_preflight",
                    candidate.ComponentId,
                    candidate.VariantId,
                    asset,
                    SourceAuthority);
            }

            return bindings;
        }

        private static P0SettingsPauseBatch85CandidateAsset Candidate(
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
                "Batch 85 settings/pause candidate imported for Unity preflight only; "
                + note
                + ". Formal install remains blocked by Unity settings/pause screenshots, text replacement, 1024x768 key-hint semantics, click targets, binding, Console checks, and review approval.");
            return new P0SettingsPauseBatch85CandidateAsset(entry, componentId, variantId, sourcePath);
        }

        private static IReadOnlyList<string> ReferenceAssets()
        {
            return new[]
            {
                P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId,
                P0VisualAssetCatalog.TitleLogoId,
                P0VisualAssetCatalog.DreamGlassPanelId,
                P0VisualAssetCatalog.PrimaryButtonId,
                P0VisualAssetCatalog.OwnerSleepIconId,
                P0VisualAssetCatalog.CatHpIconId,
                P0VisualAssetCatalog.TeamPoopIconId,
                P0VisualAssetCatalog.TeamHungerIconId
            };
        }

        private static IReadOnlyList<string> SourceLockAssets()
        {
            return new[]
            {
                "batch_85_settings_pause_preflight_candidates",
                "batch_78_settings_control_candidates",
                "batch_79_system_icon_candidates",
                "p0_settings_pause_surface",
                "p0_asset_batch_08_ui_shell"
            };
        }
    }
}

