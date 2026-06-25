using NUnit.Framework;
using TheCat.Data;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class P0RouteMapCommandRouterTests
    {
        [Test]
        public void ContinueRoute_OnBattleNodeRequestsBattleWithoutAdvancing()
        {
            RunProgressionState run = CreateRun();

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.IsTrue(result.ShouldLoadBattle);
            Assert.AreEqual(P0RouteMapCommandAction.StartBattle, result.Action);
            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, result.SelectedNodeId);
            Assert.AreEqual(0, run.Route.CompletedCount);
            Assert.AreEqual(1, run.Route.CurrentLayer);
        }

        [Test]
        public void ContinueRoute_OnNonBattleNodeAppliesDefaultChoiceAndAdvances()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ResolveRewardChoice, result.Action);
            Assert.AreEqual("layer_02_dream_event", result.SelectedNodeId);
            Assert.AreEqual("dream_event_clear_notifications", result.SelectedChoiceId);
            Assert.AreEqual(2, run.Wallet.FishTreats);
            Assert.AreEqual(1, run.DreamEventsResolved);
            Assert.AreEqual(3, run.Route.CurrentLayer);
        }

        [Test]
        public void NumberCommand_SelectsCurrentLayerOptionBeforeResolvingReward()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.SelectCat2);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.SelectRouteOption, result.Action);
            Assert.AreEqual("layer_02_shop_early", result.SelectedNodeId);
            Assert.AreEqual("layer_02_shop_early", run.Route.CurrentNode.Id);
            Assert.AreEqual(1, run.Route.CompletedCount);
        }

        [Test]
        public void NumberCommand_OnExplicitNonBattleNodeResolvesMatchingRewardChoice()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.Route.SelectCurrentNode("layer_02_dream_event");
            run.CoreValues.Capture(40f, 100f, 100f, 0f, 100f);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.SelectCat3);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ResolveRewardChoice, result.Action);
            Assert.AreEqual("layer_02_dream_event", result.SelectedNodeId);
            Assert.AreEqual("dream_event_mark_all_read", result.SelectedChoiceId);
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(52f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(3, run.Route.CurrentLayer);
        }

        [Test]
        public void ContinueRoute_WithPendingCatUpgradeDoesNotEnterNode()
        {
            RunProgressionState run = CreateRun();
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.IsFalse(result.ShouldLoadBattle);
            Assert.AreEqual(P0RouteMapCommandAction.None, result.Action);
            Assert.IsTrue(run.CatUpgrades.HasPendingUpgrade);
            Assert.AreEqual(0, run.Route.CompletedCount);
        }

        [Test]
        public void NumberCommand_WithPendingCatUpgradeResolvesUpgradeBeforeBranchSelection()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.SelectCat2);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ResolveCatUpgradeChoice, result.Action);
            StringAssert.StartsWith("cat_upgrade_", result.SelectedChoiceId);
            StringAssert.Contains("（", result.Message);
            StringAssert.Contains("）", result.Message);
            Assert.IsFalse(run.CatUpgrades.HasPendingUpgrade);
            Assert.AreEqual("layer_02_dream_event", run.Route.CurrentNode.Id);
            Assert.IsFalse(run.Route.IsCurrentNodeExplicitlySelected);
        }

        [Test]
        public void RestartRunCommandRequestsNewRunWithoutMutatingCurrentRoute()
        {
            RunProgressionState run = CreateRun();

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.RestartRun);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.StartNewRun, result.Action);
            Assert.AreEqual(0, run.Route.CompletedCount);
            Assert.AreEqual(1, run.Route.CurrentLayer);
        }

        [Test]
        public void UnsupportedBattleCommandIsIgnoredOnRouteMap()
        {
            RunProgressionState run = CreateRun();

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.Skill1);

            Assert.IsFalse(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.None, result.Action);
            Assert.AreEqual(0, run.Route.CompletedCount);
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
        }
    }
}
