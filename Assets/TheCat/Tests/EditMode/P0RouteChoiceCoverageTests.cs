using NUnit.Framework;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0RouteChoiceCoverageTests
    {
        [Test]
        public void EvaluatePrototypeRoute_CoversAllNonBattleP0Nodes()
        {
            P0RouteChoiceCoverageReport report = P0RouteChoiceCoverage.EvaluatePrototypeRoute();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(10, report.Rows.Count);
            Assert.IsTrue(report.TryGetRow("layer_02_dream_event", out P0RouteChoiceCoverageRow dreamEvent));
            Assert.AreEqual(RouteNodeType.DreamEvent, dreamEvent.NodeType);
            Assert.GreaterOrEqual(dreamEvent.ChoiceCount, 3);
            Assert.IsTrue(report.TryGetRow("layer_05_dream_event", out P0RouteChoiceCoverageRow redDotDreamEvent));
            Assert.AreEqual(RouteNodeType.DreamEvent, redDotDreamEvent.NodeType);
            Assert.AreEqual("dream_event_red_dot_cleanup", redDotDreamEvent.DefaultChoiceId);
            Assert.AreNotEqual(dreamEvent.DefaultChoiceId, redDotDreamEvent.DefaultChoiceId);
            Assert.IsTrue(report.TryGetRow("layer_03_partner", out P0RouteChoiceCoverageRow partner));
            Assert.AreEqual(RouteNodeType.Partner, partner.NodeType);
            Assert.IsTrue(report.TryGetRow("layer_05_shop", out P0RouteChoiceCoverageRow shop));
            Assert.AreEqual(RouteNodeType.Shop, shop.NodeType);
            Assert.Greater(shop.ChoiceCount, 0);
            Assert.IsTrue(report.TryGetRow("layer_07_blessing", out P0RouteChoiceCoverageRow blessing));
            Assert.AreEqual(RouteNodeType.BlessingOffering, blessing.NodeType);
            Assert.IsTrue(report.TryGetRow("layer_08_rest_nest", out P0RouteChoiceCoverageRow rest));
            Assert.AreEqual(RouteNodeType.RestNest, rest.NodeType);
        }

        [Test]
        public void EvaluatePrototypeRoute_DefaultChoiceSummariesArePlayerFacing()
        {
            P0RouteChoiceCoverageReport report = P0RouteChoiceCoverage.EvaluatePrototypeRoute();

            for (int i = 0; i < report.Rows.Count; i++)
            {
                P0RouteChoiceCoverageRow row = report.Rows[i];

                Assert.IsNotEmpty(row.DefaultChoiceId, row.NodeId);
                Assert.IsNotEmpty(row.DefaultChoiceSummary, row.NodeId);
                StringAssert.DoesNotContain("_", row.DefaultChoiceSummary, row.NodeId);
            }
        }

        [Test]
        public void Evaluate_NullRouteFailsCoverage()
        {
            P0RouteChoiceCoverageReport report = P0RouteChoiceCoverage.Evaluate(null);

            Assert.IsFalse(report.IsComplete);
            Assert.Greater(report.FailureCount, 0);
            StringAssert.Contains("missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RouteWithoutNonBattleNodesFailsCoverage()
        {
            RouteDefinition route = new RouteDefinition(
                "combat_only",
                new[]
                {
                    new[] { new RouteNodeDefinition(1, "defense", RouteNodeType.Defense, "layer_01_defense") },
                    new[] { new RouteNodeDefinition(2, P0RouteCatalog.BossNodeId, RouteNodeType.Boss, "boss_call_tyrant") }
                });

            P0RouteChoiceCoverageReport report = P0RouteChoiceCoverage.Evaluate(route);

            Assert.IsFalse(report.IsComplete);
            StringAssert.Contains("No non-battle route choice nodes", report.BuildDetailedSummary());
        }

        [Test]
        public void Row_BuildSummaryIncludesNodeTypeAndDefaultChoice()
        {
            P0RouteChoiceCoverageRow row = new P0RouteChoiceCoverageRow(
                "layer_02_dream_event",
                RouteNodeType.DreamEvent,
                3,
                "dream_event_clear_notifications",
                "Clear Red Dots +2 fish");

            string summary = row.BuildSummary();

            StringAssert.Contains("layer_02_dream_event", summary);
            StringAssert.Contains("DreamEvent", summary);
            StringAssert.Contains("choices 3", summary);
            StringAssert.Contains("Clear Red Dots", summary);
        }
    }
}
