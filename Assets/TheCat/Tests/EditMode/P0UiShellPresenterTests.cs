using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0UiShellPresenterTests
    {
        [Test]
        public void BuildSurface_ExposesCurrentP0UiShellAssetBindings()
        {
            P0UiShellSurface surface = P0UiShellPresenter.BuildSurface();

            Assert.IsTrue(P0UiShellPresenter.HasP0UiShellSurface(surface));
            Assert.AreEqual(P0UiShellPresenter.ExpectedUiShellAssetCount, surface.AssetCount);
            Assert.AreEqual(P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId, surface.MainMenuBackground.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TitleLogoId, surface.TitleLogo.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamGlassPanelId, surface.DreamGlassPanel.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PrimaryButtonId, surface.PrimaryButton.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, surface.FishTreatRewardIcon.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamShardRewardIconId, surface.DreamShardRewardIcon.AssetId);
            StringAssert.Contains("UI shell assets 6/6", surface.BuildSummary());
        }
    }
}
