using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
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
            Assert.AreEqual(2, surface.DreamChoices.Count);
            Assert.AreEqual(4, surface.Hotspots.Count);
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.Bed, out P0CatRoomHotspot bed));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.Feeder, out P0CatRoomHotspot feeder));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.LitterBox, out P0CatRoomHotspot litter));
            Assert.IsTrue(surface.TryGetHotspot(P0CatRoomHotspotIds.DreamEntrance, out P0CatRoomHotspot dreamEntrance));
            Assert.IsTrue(bed.IsFeedbackOnly);
            Assert.IsTrue(feeder.IsFeedbackOnly);
            Assert.IsTrue(litter.IsFeedbackOnly);
            Assert.IsFalse(dreamEntrance.IsFeedbackOnly);
            StringAssert.Contains("卧室", dreamEntrance.FeedbackLine);
            StringAssert.Contains("埃及", dreamEntrance.FeedbackLine);
            StringAssert.Contains("共享现有路线和战斗", dreamEntrance.FeedbackLine);
            Assert.IsTrue(surface.TryGetDreamChoice(P0DreamMapCatalog.BedroomDreamMapId, out P0CatRoomDreamChoice bedroom));
            Assert.AreEqual(P0CatRoomActionIds.EnterDream, bedroom.ActionId);
            Assert.IsTrue(bedroom.IsPlayable);
            Assert.IsTrue(bedroom.IsEnabled);
            StringAssert.Contains("中心床", bedroom.TargetLabel);
            Assert.IsTrue(surface.TryGetDreamChoice(P0DreamMapCatalog.EgyptDreamMapId, out P0CatRoomDreamChoice egypt));
            Assert.AreEqual(P0CatRoomActionIds.EnterEgyptDream, egypt.ActionId);
            Assert.IsTrue(egypt.IsPlayable);
            Assert.IsTrue(egypt.IsEnabled);
            Assert.AreEqual("P0可进入", egypt.StatusLabel);
            StringAssert.Contains("共享卧室战斗规则", egypt.Detail);
            Assert.IsTrue(surface.TryGetAction(P0CatRoomActionIds.EnterDream, out P0CatRoomAction enterDream));
            Assert.IsTrue(enterDream.IsEnabled);
            Assert.AreEqual(P0SceneFlow.RouteMapSceneName, enterDream.TargetSceneName);
            Assert.AreEqual("进入卧室梦境", enterDream.Label);
            StringAssert.Contains("守护中心床", enterDream.Detail);
            Assert.IsTrue(surface.TryGetAction(P0CatRoomActionIds.EnterEgyptDream, out P0CatRoomAction egyptDream));
            Assert.IsTrue(egyptDream.IsEnabled);
            Assert.AreEqual(P0SceneFlow.RouteMapSceneName, egyptDream.TargetSceneName);
            Assert.AreEqual("进入埃及梦境", egyptDream.Label);
            StringAssert.Contains("月砂祭坛", egyptDream.Detail);
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
            Assert.IsTrue(surface.TryGetDreamChoice(P0DreamMapCatalog.BedroomDreamMapId, out P0CatRoomDreamChoice bedroom));
            Assert.IsTrue(surface.TryGetDreamChoice(P0DreamMapCatalog.EgyptDreamMapId, out P0CatRoomDreamChoice egypt));
            Assert.IsFalse(dreamEntrance.IsEnabled);
            Assert.IsFalse(enterDream.IsEnabled);
            Assert.IsFalse(bedroom.IsEnabled);
            Assert.IsFalse(egypt.IsEnabled);
            Assert.AreEqual(P0CatRoomActionIds.EnterEgyptDream, egypt.ActionId);
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

        [Test]
        public void CatRoomSession_RouteReturnCarriesClearedStateAndClosesRun()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            while (!run.Route.IsComplete)
            {
                var node = run.Route.CurrentNode;
                if (RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    P0RouteRewardResolver.ApplyBattleReward(node, run);
                    run.Route.CompleteCurrentNode(NodeResult.Success);
                }
                else
                {
                    RouteNodeResolver.ResolveCurrentNode(run);
                }
            }

            P0CatRoomSession.RecordRouteReturn(run);
            P0CatRoomSurface surface = P0CatRoomPresenter.BuildSurface(P0CatRoomSession.CurrentState);

            Assert.AreEqual(P0CatRoomReturnReason.RouteCleared, P0CatRoomSession.CurrentState.ReturnReason);
            Assert.IsFalse(P0CatRoomSession.CurrentState.HasActiveRun);
            StringAssert.Contains("本轮梦境完成", surface.ReturnFeedbackLabel);
            Assert.AreEqual("结束", surface.ResourceRows[2].ValueLabel);
        }
    }
}
