using NUnit.Framework;
using TheCat.Combat;
using TheCat.Core;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class GameStateMachineTests
    {
        [Test]
        public void TryChangeState_AllowsExpectedBootPath()
        {
            GameStateMachine stateMachine = new GameStateMachine();

            Assert.IsTrue(stateMachine.TryChangeState(GameState.MainMenu));
            Assert.IsTrue(stateMachine.TryChangeState(GameState.CharacterSelect));
            Assert.IsTrue(stateMachine.TryChangeState(GameState.RouteMap));
            Assert.IsTrue(stateMachine.TryChangeState(GameState.BattleLoading));
            Assert.IsTrue(stateMachine.TryChangeState(GameState.Battle));

            Assert.AreEqual(GameState.Battle, stateMachine.CurrentState);
        }

        [Test]
        public void TryChangeState_BlocksInvalidJump()
        {
            GameStateMachine stateMachine = new GameStateMachine();

            Assert.IsFalse(stateMachine.TryChangeState(GameState.Battle));
            Assert.AreEqual(GameState.Boot, stateMachine.CurrentState);
        }

        [Test]
        public void StateChange_PublishesEvent()
        {
            EventBus bus = new EventBus();
            GameStateChangedEvent received = default;
            bus.Subscribe<GameStateChangedEvent>(evt => received = evt);

            GameStateMachine stateMachine = new GameStateMachine(bus);
            stateMachine.TryChangeState(GameState.MainMenu);

            Assert.AreEqual(GameState.Boot, received.PreviousState);
            Assert.AreEqual(GameState.MainMenu, received.NewState);
        }

        [Test]
        public void P0RuntimeSettings_PauseStopsDeltaAndSpeedScalesDelta()
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();

            settings.SetBattleSpeed(1.5f);

            Assert.AreEqual(3f, settings.ApplyToDelta(2f), 0.001f);

            settings.SetPaused(true);

            Assert.AreEqual(0f, settings.ApplyToDelta(2f), 0.001f);
        }

        [Test]
        public void P0RuntimeSettings_RejectsUnsupportedSpeed()
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();

            Assert.Throws<System.ArgumentOutOfRangeException>(() => settings.SetBattleSpeed(0f));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => settings.SetBattleSpeed(3f));
        }

        [Test]
        public void P0SceneFlow_StartRunUsesRouteMapAndQuickBattleUsesBattleScene()
        {
            Assert.AreEqual("P0RouteMap", P0SceneFlow.GetStartSceneName(P0RunStartMode.RouteMap));
            Assert.AreEqual("P0GrayboxBattle", P0SceneFlow.GetStartSceneName(P0RunStartMode.QuickBattle));
            Assert.AreEqual("P0CatRoom", P0SceneFlow.GetStartSceneName(P0RunStartMode.CatRoom));
            Assert.AreEqual(P0SceneFlow.MainMenuSceneName, MainMenuController.MainMenuSceneName);
            Assert.AreEqual(P0SceneFlow.CatRoomSceneName, CatRoomController.CatRoomSceneName);
            Assert.AreEqual(P0SceneFlow.RouteMapSceneName, RouteMapController.RouteMapSceneName);
            Assert.AreEqual(P0SceneFlow.GrayboxBattleSceneName, MainMenuController.GrayboxBattleSceneName);
        }

        [Test]
        public void P0SceneFlow_PostBattleReturnsRouteMapOnlyAfterOutcome()
        {
            Assert.AreEqual("P0RouteMap", P0SceneFlow.GetPostBattleSceneName(BattleOutcome.Victory));
            Assert.AreEqual("P0RouteMap", P0SceneFlow.GetPostBattleSceneName(BattleOutcome.Defeat));
            Assert.Throws<System.InvalidOperationException>(() => P0SceneFlow.GetPostBattleSceneName(BattleOutcome.InProgress));
        }
    }
}
