using NUnit.Framework;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0PlayModeScreenshotSmokeTests
    {
        [Test]
        public void CapturePlan_IncludesRuntimeVisualAcceptanceBeforeBattleResult()
        {
            Assert.IsTrue(P0PlayModeScreenshotSmoke.HasP0ScreenshotCapturePlan());
            Assert.IsTrue(P0PlayModeScreenshotSmoke.HasRuntimeVisualScreenshotCapturePlan());
            Assert.AreEqual(P0PlayModeScreenshotSmoke.ExpectedCaptureCount, P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames.Count);
            Assert.AreEqual("01-main-menu.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[0]);
            Assert.AreEqual("02-cat-room.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[1]);
            Assert.AreEqual("03-route-map-layer1.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[2]);
            Assert.AreEqual("04-battle-hud-layer1.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[3]);
            Assert.AreEqual("05-active-cat-saiban.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[4]);
            Assert.AreEqual("06-active-cat-nephthys.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[5]);
            Assert.AreEqual("07-active-cat-suzune.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[6]);
            Assert.AreEqual("08-battle-world-visuals.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[7]);
            Assert.AreEqual("09-call-tyrant-warning-vfx.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[8]);
            Assert.AreEqual("10-battle-result-layer1.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[9]);
            Assert.AreEqual("11-settlement.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[10]);
            Assert.AreEqual(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingCount, P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds.Count);
            Assert.AreEqual("background.bedroom_dream", P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds[0]);
            Assert.AreEqual("cat.combat.saiban", P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds[1]);
            Assert.AreEqual("cat.combat.nephthys", P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds[2]);
            Assert.AreEqual("cat.combat.suzune", P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds[3]);
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "cat.avatar.saiban");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "cat.avatar.nephthys");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "cat.avatar.suzune");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "enemy.combat.call_tyrant");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "warning.call_tyrant");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "warning.call_tyrant_app_throw");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "warning.call_tyrant_summon_portal");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "battle_hud.interaction_range_ripple");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "skill_vfx.saiban_bedline");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "route_reward_detail.upgrade");
            CollectionAssert.Contains(P0PlayModeScreenshotSmoke.ExpectedRuntimeVisualBindingIds, "blessing_detail.rhythm_lullaby");
            Assert.AreEqual(P0AssetProductionReadiness.RuntimeVisualContactSheetPath, P0PlayModeScreenshotSmoke.RuntimeVisualContactSheetPath);
        }

        [Test]
        public void LoadingStartSurface_HasScreenshotHookWithoutChangingCaptureCount()
        {
            P0LoadingStartSurface surface = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, 0.35f);

            Assert.IsTrue(P0LoadingStartPresenter.HasP0LoadingStartSurface(surface));
            Assert.IsTrue(surface.HasScreenshotHook);
            Assert.AreEqual(11, P0PlayModeScreenshotSmoke.ExpectedCaptureCount);
            Assert.AreEqual("01-main-menu.png", P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames[0]);
            StringAssert.Contains("35%", surface.BuildStatusLine());
        }

        [Test]
        public void EgyptEntrySmoke_UsesDedicatedFiveScreenshotEvidencePlan()
        {
            Assert.IsTrue(P0PlayModeEgyptEntrySmoke.HasEgyptEntryCapturePlan());
            Assert.AreEqual(5, P0PlayModeEgyptEntrySmoke.ExpectedCaptureCount);
            Assert.AreEqual(5, P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames.Count);
            Assert.AreEqual("design/development/screenshots/p0-egypt-entry-smoke", P0PlayModeEgyptEntrySmoke.DefaultScreenshotDirectory);
            Assert.AreEqual("01-cat-room-egypt-entry.png", P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames[0]);
            Assert.AreEqual("02-egypt-route-map-layer1.png", P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames[1]);
            Assert.AreEqual("03-egypt-battle-hud-layer1.png", P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames[2]);
            Assert.AreEqual("04-egypt-battle-result-layer1.png", P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames[3]);
            Assert.AreEqual("05-egypt-route-map-after-layer1.png", P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames[4]);
            CollectionAssert.DoesNotContain(
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                P0PlayModeEgyptEntrySmoke.RouteMapCaptureFileName);
        }
    }
}
