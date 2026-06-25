using NUnit.Framework;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class P0BattleStartContextTests
    {
        [Test]
        public void Create_LayerOneBattleUsesCurrentRouteNode()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());

            P0BattleStartContext context = P0BattleStartContext.Create(run);

            Assert.IsTrue(context.IsRouteBattle);
            Assert.IsTrue(context.ShouldCompleteRouteNode);
            Assert.IsTrue(context.ShouldPersistRunState);
            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, context.DreamMap.Id);
            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, context.Node.Id);
            Assert.AreEqual("layer_01_defense", context.Wave.Id);
            StringAssert.Contains("已开始", context.StartMessage);
        }

        [Test]
        public void Create_EliteAndBossNodesResolveTheirCombatWaves()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            while (run.Route.CurrentLayer < 9)
            {
                run.Route.CompleteCurrentNode(TheCat.Data.NodeResult.Success);
            }

            P0BattleStartContext elite = P0BattleStartContext.Create(run);

            Assert.IsTrue(elite.IsRouteBattle);
            Assert.AreEqual("elite_red_eye_alarm", elite.Wave.Id);

            run.Route.CompleteCurrentNode(TheCat.Data.NodeResult.Success);
            P0BattleStartContext boss = P0BattleStartContext.Create(run);

            Assert.IsTrue(boss.IsRouteBattle);
            Assert.AreEqual("boss_call_tyrant", boss.Wave.Id);
            Assert.AreEqual(P0RouteCatalog.BossNodeId, boss.Node.Id);
        }

        [Test]
        public void Create_NonBattleRouteNodeCreatesStandaloneBattleWithoutRouteCompletion()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.Route.CompleteCurrentNode(TheCat.Data.NodeResult.Success);

            P0BattleStartContext context = P0BattleStartContext.Create(run);

            Assert.IsFalse(context.IsRouteBattle);
            Assert.IsFalse(context.ShouldCompleteRouteNode);
            Assert.IsFalse(context.ShouldPersistRunState);
            Assert.IsTrue(context.HasNode);
            Assert.AreEqual(RouteNodeType.DreamEvent, context.Node.NodeType);
            Assert.AreEqual("layer_01_defense", context.Wave.Id);
            StringAssert.Contains("不是战斗节点", context.StartMessage);
        }

        [Test]
        public void Create_EgyptMapContextDoesNotForkCombatWaveRules()
        {
            RunProgressionState bedroomRun = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            RunProgressionState egyptRun = new RunProgressionState(
                P0RouteCatalog.CreateEgyptPlaceholderRoute(),
                P0RunSession.CreateDefaultStarterCatIds());

            P0BattleStartContext bedroom = P0BattleStartContext.Create(bedroomRun);
            P0BattleStartContext egypt = P0BattleStartContext.Create(egyptRun);

            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, bedroom.DreamMap.Id);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, egypt.DreamMap.Id);
            Assert.AreEqual(bedroom.Wave.Id, egypt.Wave.Id);
            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, egypt.Node.Id);
            StringAssert.Contains("埃及梦境", egypt.StartMessage);
        }

        [Test]
        public void BuildSummary_ReportsRouteCompletionDecision()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());

            string summary = P0BattleStartContext.Create(run).BuildSummary();

            StringAssert.Contains("Route battle", summary);
            StringAssert.Contains("completeRoute True", summary);
            StringAssert.Contains("persistRun True", summary);
            StringAssert.Contains("layer_01_defense", summary);
            StringAssert.Contains(P0DreamMapCatalog.BedroomDreamMapId, summary);
        }

        [Test]
        public void BuildSummary_ReportsStandaloneDoesNotPersistRunState()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.Route.CompleteCurrentNode(TheCat.Data.NodeResult.Success);

            string summary = P0BattleStartContext.Create(run).BuildSummary();

            StringAssert.Contains("Standalone battle", summary);
            StringAssert.Contains("completeRoute False", summary);
            StringAssert.Contains("persistRun False", summary);
        }
    }
}
