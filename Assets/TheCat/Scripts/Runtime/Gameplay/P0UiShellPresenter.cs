using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public sealed class P0UiShellSurface
    {
        public P0UiShellSurface(
            P0VisualAssetReference mainMenuBackground,
            P0VisualAssetReference titleLogo,
            P0VisualAssetReference dreamGlassPanel,
            P0VisualAssetReference primaryButton,
            P0VisualAssetReference fishTreatRewardIcon,
            P0VisualAssetReference dreamShardRewardIcon)
        {
            MainMenuBackground = mainMenuBackground;
            TitleLogo = titleLogo;
            DreamGlassPanel = dreamGlassPanel;
            PrimaryButton = primaryButton;
            FishTreatRewardIcon = fishTreatRewardIcon;
            DreamShardRewardIcon = dreamShardRewardIcon;
        }

        public P0VisualAssetReference MainMenuBackground { get; }

        public P0VisualAssetReference TitleLogo { get; }

        public P0VisualAssetReference DreamGlassPanel { get; }

        public P0VisualAssetReference PrimaryButton { get; }

        public P0VisualAssetReference FishTreatRewardIcon { get; }

        public P0VisualAssetReference DreamShardRewardIcon { get; }

        public int AssetCount
        {
            get
            {
                int count = 0;
                count += MainMenuBackground.HasAsset ? 1 : 0;
                count += TitleLogo.HasAsset ? 1 : 0;
                count += DreamGlassPanel.HasAsset ? 1 : 0;
                count += PrimaryButton.HasAsset ? 1 : 0;
                count += FishTreatRewardIcon.HasAsset ? 1 : 0;
                count += DreamShardRewardIcon.HasAsset ? 1 : 0;
                return count;
            }
        }

        public string BuildSummary()
        {
            return "UI shell assets "
                + AssetCount
                + "/"
                + P0UiShellPresenter.ExpectedUiShellAssetCount
                + " background "
                + MainMenuBackground.AssetId
                + " logo "
                + TitleLogo.AssetId
                + " panel "
                + DreamGlassPanel.AssetId
                + " button "
                + PrimaryButton.AssetId;
        }
    }

    public static class P0UiShellPresenter
    {
        public const int ExpectedUiShellAssetCount = 6;

        public static P0UiShellSurface BuildSurface()
        {
            return new P0UiShellSurface(
                P0VisualAssetCatalog.GetMainMenuBackground(),
                P0VisualAssetCatalog.GetTitleLogo(),
                P0VisualAssetCatalog.GetDreamGlassPanel(),
                P0VisualAssetCatalog.GetPrimaryButton(),
                P0VisualAssetCatalog.GetRewardIcon("fish_treats"),
                P0VisualAssetCatalog.GetRewardIcon("dream_shards"));
        }

        public static bool HasP0UiShellSurface(P0UiShellSurface surface)
        {
            return surface != null
                && surface.AssetCount == ExpectedUiShellAssetCount
                && surface.MainMenuBackground.AssetId == P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId
                && surface.TitleLogo.AssetId == P0VisualAssetCatalog.TitleLogoId
                && surface.DreamGlassPanel.AssetId == P0VisualAssetCatalog.DreamGlassPanelId
                && surface.PrimaryButton.AssetId == P0VisualAssetCatalog.PrimaryButtonId
                && surface.FishTreatRewardIcon.AssetId == P0VisualAssetCatalog.FishTreatRewardIconId
                && surface.DreamShardRewardIcon.AssetId == P0VisualAssetCatalog.DreamShardRewardIconId;
        }
    }
}
