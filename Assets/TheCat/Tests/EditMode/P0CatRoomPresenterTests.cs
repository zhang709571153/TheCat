using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class P0CatRoomPresenterTests
    {
        [Test]
        public void BuildSurface_FreshStartContainsHubHotspotsAndDreamAction()
        {
            P0CatRoomSurface surface = P0CatRoomPresenter.BuildSurface(P0CatRoomState.CreateFreshStart());

            Assert.IsTrue(P0CatRoomPresenter.HasP0CatRoomSurface(surface), P0CatRoomPresenter.BuildCompactSummary(surface));
            Assert.AreEqual(4, surface.ValueRows.Count);
            Assert.AreEqual("饱肚度", surface.ValueRows[2].Label);
            Assert.AreEqual("充足", surface.ValueRows[2].StatusLabel);
            Assert.AreEqual(3, surface.ResourceRows.Count);
            Assert.AreEqual(4, surface.Hotspots.Count);
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.Bed, out P0CatRoomHotspot bed));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.Feeder, out P0CatRoomHotspot feeder));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.LitterBox, out P0CatRoomHotspot litter));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.DreamEntrance, out P0CatRoomHotspot dreamEntrance));
            Assert.IsTrue(bed.IsFeedbackOnly);
            Assert.IsTrue(feeder.IsFeedbackOnly);
            Assert.IsTrue(litter.IsFeedbackOnly);
            Assert.IsFalse(dreamEntrance.IsFeedbackOnly);
            StringAssert.Contains("卧室梦境可进入", dreamEntrance.FeedbackLine);
            StringAssert.Contains("埃及梦境仍在占位验证", dreamEntrance.FeedbackLine);
            Assert.IsTrue(surface.TryGetAction(P0CatRoomActionIds.EnterDream, out P0CatRoomAction enterDream));
            Assert.IsTrue(enterDream.IsEnabled);
            Assert.AreEqual(P0SceneFlow.RouteMapSceneName, enterDream.TargetSceneName);
            Assert.AreEqual("进入卧室梦境", enterDream.Label);
            StringAssert.Contains("守护中心床", enterDream.Detail);
        }

        [Test]
        public void BuildSurface_ReturnReasonsProduceDistinctFeedback()
        {
            P0CatRoomSurface victory = P0CatRoomPresenter.BuildSurface(
                P0CatRoomState.CreateReturn(P0CatRoomReturnReason.BattleVictory, "fish +3", fishTreats: 3));
            P0CatRoomSurface defeat = P0CatRoomPresenter.BuildSurface(
                P0CatRoomState.CreateReturn(P0CatRoomReturnReason.BattleDefeat, "sleep broke"));
            P0CatRoomSurface cleared = P0CatRoomPresenter.BuildSurface(
                P0CatRoomState.CreateReturn(P0CatRoomReturnReason.RouteCleared, "boss cleared", dreamShards: 1));

            StringAssert.Contains("梦境胜利", victory.ReturnFeedbackLabel);
            StringAssert.Contains("梦境失败", defeat.ReturnFeedbackLabel);
            StringAssert.Contains("本轮梦境完成", cleared.ReturnFeedbackLabel);
            StringAssert.Contains("3", victory.ResourceRows[0].ValueLabel);
            StringAssert.Contains("1", cleared.ResourceRows[1].ValueLabel);
            Assert.AreNotEqual(victory.ReturnFeedbackLabel, defeat.ReturnFeedbackLabel);
            Assert.AreNotEqual(victory.ReturnFeedbackLabel, cleared.ReturnFeedbackLabel);
            Assert.IsTrue(P0CatRoomPresenter.HasP0CatRoomSurface(victory));
            Assert.IsTrue(P0CatRoomPresenter.HasP0CatRoomSurface(defeat));
            Assert.IsTrue(P0CatRoomPresenter.HasP0CatRoomSurface(cleared));
        }

        [Test]
        public void SceneFlow_ExposesCatRoomWithoutReplacingCurrentRouteAndBattleFlow()
        {
            Assert.AreEqual("P0CatRoom", P0SceneFlow.CatRoomSceneName);
            Assert.AreEqual(P0SceneFlow.CatRoomSceneName, P0SceneFlow.GetStartSceneName(P0RunStartMode.CatRoom));
            Assert.AreEqual(P0SceneFlow.RouteMapSceneName, P0SceneFlow.GetStartSceneName(P0RunStartMode.RouteMap));
            Assert.AreEqual(P0SceneFlow.GrayboxBattleSceneName, P0SceneFlow.GetStartSceneName(P0RunStartMode.QuickBattle));
            Assert.AreEqual(P0SceneFlow.CatRoomSceneName, P0SceneFlow.GetCatRoomReturnSceneName(P0CatRoomReturnReason.RouteCleared));
        }

        [Test]
        public void GameStateMachine_AllowsMenuToCatRoomToRoutePath()
        {
            GameStateMachine stateMachine = new GameStateMachine();

            Assert.IsTrue(stateMachine.TryChangeState(GameState.MainMenu));
            Assert.IsTrue(stateMachine.TryChangeState(GameState.CatRoom));
            Assert.IsTrue(stateMachine.TryChangeState(GameState.RouteMap));

            Assert.AreEqual(GameState.RouteMap, stateMachine.CurrentState);
            Assert.IsTrue(GameStateMachine.CanTransition(GameState.Result, GameState.CatRoom));
            Assert.IsFalse(GameStateMachine.CanTransition(GameState.CatRoom, GameState.BattleLoading));
        }

        [Test]
        public void HubFeedbackHotspotsDoNotOwnDreamNavigation()
        {
            P0CatRoomState state = new P0CatRoomState(
                0.2f,
                0.55f,
                0.8f,
                0.75f,
                1,
                0,
                false,
                true,
                P0CatRoomReturnReason.RouteFailed,
                "review");
            P0CatRoomSurface surface = P0CatRoomPresenter.BuildSurface(state);

            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.Bed, out P0CatRoomHotspot bed));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.Feeder, out P0CatRoomHotspot feeder));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.LitterBox, out P0CatRoomHotspot litter));
            Assert.AreNotEqual(P0CatRoomActionIds.EnterDream, bed.ActionId);
            Assert.AreNotEqual(P0CatRoomActionIds.EnterDream, feeder.ActionId);
            Assert.AreNotEqual(P0CatRoomActionIds.EnterDream, litter.ActionId);
            StringAssert.Contains("床铺", bed.BuildSummary());
            StringAssert.Contains("食盆", feeder.BuildSummary());
            StringAssert.Contains("猫砂盆", litter.BuildSummary());
        }

        [Test]
        public void BuildSurface_LockedDreamEntryKeepsNavigationStateConsistent()
        {
            P0CatRoomState state = new P0CatRoomState(
                1f,
                1f,
                0.1f,
                0.1f,
                0,
                0,
                true,
                false,
                P0CatRoomReturnReason.FreshStart,
                "等待选择");

            P0CatRoomSurface surface = P0CatRoomPresenter.BuildSurface(state);

            Assert.IsTrue(P0CatRoomPresenter.HasP0CatRoomSurface(surface));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.DreamEntrance, out P0CatRoomHotspot dreamEntrance));
            Assert.IsTrue(surface.TryGetAction(P0CatRoomActionIds.EnterDream, out P0CatRoomAction enterDream));
            Assert.IsFalse(dreamEntrance.IsEnabled);
            Assert.IsFalse(enterDream.IsEnabled);
            StringAssert.Contains("进行中", surface.ResourceRows[2].ValueLabel);
            StringAssert.Contains("未解锁", enterDream.BuildButtonLabel());
        }

        [Test]
        public void CatRoomSession_BattleReturnCarriesRunResourcesAndActiveState()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.Wallet.AddFishTreats(4);
            run.Wallet.AddDreamShards(2);

            P0CatRoomSession.RecordBattleReturn(BattleOutcome.Victory, run, null);
            P0CatRoomSurface surface = P0CatRoomPresenter.BuildSurface(P0CatRoomSession.CurrentState);

            StringAssert.Contains("梦境胜利", surface.ReturnFeedbackLabel);
            Assert.IsTrue(P0CatRoomSession.CurrentState.HasActiveRun);
            Assert.AreEqual("4", surface.ResourceRows[0].ValueLabel);
            Assert.AreEqual("2", surface.ResourceRows[1].ValueLabel);
            Assert.AreEqual("进行中", surface.ResourceRows[2].ValueLabel);
        }
    }
}
