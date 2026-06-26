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
        public void ContinueRoute_OnEgyptSharedRouteRequestsBattleWithoutRebindingBedroom()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateEgyptPlayableRoute(),
                new[] { "saiban", "nephthys", "suzune" });

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.IsTrue(result.ShouldLoadBattle);
            Assert.AreEqual(P0RouteMapCommandAction.StartBattle, result.Action);
            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, result.SelectedNodeId);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, run.DreamMap.Id);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, run.Route.Route.DreamMap.Id);
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
        public void NumberCommand_OnExplicitDreamEventSelectsCatnipResidueModifier()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.Route.SelectCurrentNode("layer_02_dream_event");

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.SelectCat2);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ResolveRewardChoice, result.Action);
            Assert.AreEqual("layer_02_dream_event", result.SelectedNodeId);
            Assert.AreEqual("dream_event_catnip_residue", result.SelectedChoiceId);
            Assert.AreEqual(1, run.DreamEventsResolved);
            Assert.IsTrue(run.PendingBattleModifiers.HasPending);
            Assert.AreEqual(1.2f, run.PendingBattleModifiers.SkillDamageMultiplier, 0.001f);
            Assert.AreEqual(1.5f, run.PendingBattleModifiers.PoopGrowthMultiplier, 0.001f);
            Assert.AreEqual(3, run.Route.CurrentLayer);
        }

        [Test]
        public void ContinueRoute_OnLayerFiveShopBuysBedPatchByDefault()
        {
            RunProgressionState run = CreateRun();
            AdvanceToLayerFiveShop(run);
            run.CoreValues.Capture(60f, 100f, 100f, 0f, 100f);

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ResolveRewardChoice, result.Action);
            Assert.AreEqual("layer_05_shop", result.SelectedNodeId);
            Assert.AreEqual("shop_bed_patch", result.SelectedChoiceId);
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(80f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(1, run.ShopPurchases);
            Assert.AreEqual(6, run.Route.CurrentLayer);
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
        public void ContinueRoute_OnCompletedRouteRequestsCatRoomReturn()
        {
            RunProgressionState run = CreateClearedRun();

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ReturnCatRoom, result.Action);
            Assert.IsFalse(result.ShouldLoadBattle);
            Assert.IsTrue(run.Route.IsComplete);
            Assert.IsTrue(run.Route.IsCleared);
        }

        [Test]
        public void ContinueRoute_OnFailedRouteRequestsCatRoomReturn()
        {
            RunProgressionState run = CreateFailedRun();

            P0RouteMapCommandResult result = P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);

            Assert.IsTrue(result.IsHandled);
            Assert.AreEqual(P0RouteMapCommandAction.ReturnCatRoom, result.Action);
            Assert.IsFalse(result.ShouldLoadBattle);
            Assert.IsTrue(run.Route.IsComplete);
            Assert.IsTrue(run.Route.IsFailed);
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

        private static RunProgressionState CreateClearedRun()
        {
            RunProgressionState run = CreateRun();
            int safety = 0;
            while (!run.Route.IsComplete && safety < 20)
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

                safety++;
            }

            return run;
        }

        private static void AdvanceToLayerFiveShop(RunProgressionState run)
        {
            run.Route.CompleteCurrentNode(NodeResult.Success);
            P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);
            run.Route.CompleteCurrentNode(NodeResult.Success);
            P0RouteMapCommandRouter.Execute(run, P0InputCommand.ContinueRoute);
            Assert.AreEqual(5, run.Route.CurrentLayer);
            Assert.AreEqual(3, run.Wallet.FishTreats);
            Assert.IsTrue(run.Route.SelectCurrentNode("layer_05_shop"));
        }

        private static RunProgressionState CreateFailedRun()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Failure);
            return run;
        }
    }
}
