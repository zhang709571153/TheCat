using System;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0MainMenuCoverageTests
    {
        [Test]
        public void BuildSurface_DefaultSelectionContainsStarterTrioAndRouteActions()
        {
            P0MainMenuSurface surface = BuildSurface(P0RunSession.CreateDefaultStarterCatIds());

            Assert.IsTrue(P0MainMenuPresenter.HasP0MainMenuSurface(surface));
            Assert.AreEqual(3, surface.StarterCards.Count);
            Assert.AreEqual(3, surface.SelectedStarterCount);
            Assert.AreEqual(10, surface.RouteRows.Count);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.EnterCatRoom, out P0MainMenuAction catRoom));
            Assert.IsTrue(catRoom.IsEnabled);
            Assert.AreEqual(P0SceneFlow.CatRoomSceneName, catRoom.TargetSceneName);
            Assert.AreEqual(P0MainMenuActionCategory.PlayerPrimary, catRoom.ActionCategory);
            StringAssert.Contains("猫房", catRoom.Detail);
            StringAssert.Contains("卧室梦境", catRoom.Detail);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.StartSelectedRoute, out P0MainMenuAction selected));
            Assert.IsTrue(selected.IsEnabled);
            Assert.AreEqual(P0SceneFlow.RouteMapSceneName, selected.TargetSceneName);
            Assert.AreEqual(P0MainMenuActionCategory.DevRouteHelper, selected.ActionCategory);
            StringAssert.Contains("灰盒", selected.Label);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.QuickBattle, out P0MainMenuAction quickBattle));
            Assert.IsTrue(quickBattle.IsEnabled);
            Assert.AreEqual(P0SceneFlow.GrayboxBattleSceneName, quickBattle.TargetSceneName);
            Assert.AreEqual(P0MainMenuActionCategory.GrayboxBattleHelper, quickBattle.ActionCategory);
            StringAssert.Contains("调试", quickBattle.Label);
            Assert.IsTrue(P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell));
            Assert.AreEqual(P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId, surface.UiShell.MainMenuBackground.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TitleLogoId, surface.UiShell.TitleLogo.AssetId);
            StringAssert.Contains("猫房", surface.PlayerPathLabel);
            StringAssert.Contains("卧室梦境", surface.PlayerPathLabel);
            StringAssert.Contains("守护中心床", surface.PlayerPathLabel);
            StringAssert.Contains("塞班", surface.StarterCards[0].BuildSelectionLabel());
            StringAssert.Contains("银誓护盾", surface.StarterCards[0].BuildSkillPreview());
            StringAssert.Contains("已准备", surface.StarterCards[0].BuildCharacterSelectSummary());
            Assert.AreEqual("已加入猫队", surface.StarterCards[0].SelectionStateLabel);
            Assert.AreEqual("已准备", surface.StarterCards[0].ReadyBadgeLabel);
            Assert.AreEqual(P0VisualAssetCatalog.SaibanHudAvatarId, surface.StarterCards[0].HudAvatar.AssetId);
            Assert.IsFalse(surface.StarterCards[0].BuildSelectionLabel().Contains("silver_oath_sun_sword"));
            Assert.IsFalse(surface.StarterCards[0].BuildCharacterSelectSummary().Contains("silver_oath_sun_sword"));
            StringAssert.Contains("silver_oath_sun_sword", surface.StarterCards[0].BuildDesignPreview());
            StringAssert.Contains("bed", surface.StarterCards[0].SignatureLine);
        }

        [Test]
        public void BuildSurface_EmptySelectionDisablesSelectedAndQuickStart()
        {
            P0MainMenuSurface surface = BuildSurface(Array.Empty<string>());

            Assert.IsFalse(surface.HasAnyStarterSelected);
            Assert.AreEqual(0, surface.SelectedStarterCount);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.StartSelectedRoute, out P0MainMenuAction selected));
            Assert.IsFalse(selected.IsEnabled);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.EnterCatRoom, out P0MainMenuAction catRoom));
            Assert.IsFalse(catRoom.IsEnabled);
            Assert.AreEqual(P0MainMenuActionCategory.PlayerPrimary, catRoom.ActionCategory);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.QuickBattle, out P0MainMenuAction quickBattle));
            Assert.IsFalse(quickBattle.IsEnabled);
            Assert.AreEqual(P0MainMenuActionCategory.GrayboxBattleHelper, quickBattle.ActionCategory);
            Assert.IsTrue(surface.TryGetAction(P0MainMenuActionIds.StartDefaultRoute, out P0MainMenuAction defaultStart));
            Assert.IsTrue(defaultStart.IsEnabled);
            Assert.AreEqual(P0MainMenuActionCategory.DevRouteHelper, defaultStart.ActionCategory);
            Assert.AreEqual("待选择", surface.StarterCards[0].SelectionStateLabel);
            Assert.AreEqual("未选择", surface.StarterCards[0].ReadyBadgeLabel);
        }

        [Test]
        public void BuildSurface_RoutePreviewShowsBranchesAndBoss()
        {
            P0MainMenuSurface surface = BuildSurface(P0RunSession.CreateDefaultStarterCatIds());

            bool hasBranch = false;
            bool hasBoss = false;
            for (int i = 0; i < surface.RouteRows.Count; i++)
            {
                hasBranch |= surface.RouteRows[i].OptionCount > 1;
                hasBoss |= surface.RouteRows[i].HasBoss;
            }

            Assert.IsTrue(hasBranch);
            Assert.IsTrue(hasBoss);
            StringAssert.Contains("来电暴君首领", surface.RouteRows[9].PreviewLabel);
            StringAssert.Contains("第 10 层", surface.RouteRows[9].BuildSummary());
        }

        [Test]
        public void EvaluatePrototypeSurface_CompletesMainMenuGate()
        {
            P0MainMenuCoverageReport report = P0MainMenuCoverage.EvaluatePrototypeSurface();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0MainMenuCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("main menu coverage complete", report.BuildSummary());
            StringAssert.Contains("quick battle", report.BuildDetailedSummary());
            StringAssert.Contains("design signature", report.BuildDetailedSummary());
            StringAssert.Contains("Character-select contract", report.BuildDetailedSummary());
            StringAssert.Contains("player-primary", report.BuildDetailedSummary());
            StringAssert.Contains("UI shell", report.BuildDetailedSummary());
        }

        private static P0MainMenuSurface BuildSurface(System.Collections.Generic.IReadOnlyList<string> selectedStarterIds)
        {
            return P0MainMenuPresenter.BuildSurface(
                P0PrototypeCatalog.CreateStarterCats(),
                selectedStarterIds,
                P0RouteCatalog.CreateTenLayerRoute(),
                "Ready");
        }
    }
}
