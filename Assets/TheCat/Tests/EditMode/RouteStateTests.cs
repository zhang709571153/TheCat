using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class RouteStateTests
    {
        [Test]
        public void P0RouteCatalog_CreateTenLayerRoute_CoversRequiredNodeTypes()
        {
            RouteDefinition route = P0RouteCatalog.CreateTenLayerRoute();

            Assert.AreEqual(10, route.LayerCount);
            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, route.DreamMap.Id);
            Assert.IsTrue(route.DreamMap.IsPlayableInP0);
            Assert.AreEqual(RouteNodeType.Defense, route.Nodes[0].NodeType);
            Assert.AreEqual(RouteNodeType.Boss, route.Nodes[9].NodeType);
            Assert.AreEqual(P0RouteCatalog.BossNodeId, route.Nodes[9].Id);
            Assert.AreEqual(10, route.LayerOptions.Count);
            Assert.Greater(route.GetLayerOptions(2).Count, 1);
            Assert.IsTrue(route.GetLayerOptions(8).Any(node => node.ContentId == "elite_falling_dream_teddy"));
            Assert.IsTrue(route.GetLayerOptions(9).Any(node => node.ContentId == "shop_midnight_kibble"));
            Assert.IsTrue(route.Nodes.Any(node => node.NodeType == RouteNodeType.Elite));
            Assert.IsTrue(route.Nodes.Any(node => node.NodeType == RouteNodeType.Partner));
            Assert.IsTrue(route.Nodes.Any(node => node.NodeType == RouteNodeType.Shop));
            Assert.IsTrue(route.Nodes.Any(node => node.NodeType == RouteNodeType.DreamEvent));
            Assert.IsTrue(route.Nodes.Any(node => node.NodeType == RouteNodeType.BlessingOffering));
            Assert.IsTrue(route.Nodes.Any(node => node.NodeType == RouteNodeType.RestNest));
        }

        [Test]
        public void P0DreamMapCatalog_DefinesBedroomAndEgyptP0Contexts()
        {
            DreamMapDefinition bedroom = P0DreamMapCatalog.GetBedroomDreamMap();
            DreamMapDefinition egypt = P0DreamMapCatalog.GetEgyptDreamMap();

            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, bedroom.Id);
            Assert.AreEqual("卧室梦境", bedroom.DisplayName);
            Assert.AreEqual("中心床", bedroom.DefenseTargetLabel);
            Assert.IsTrue(bedroom.IsPlayableInP0);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, egypt.Id);
            Assert.AreEqual("埃及梦境", egypt.DisplayName);
            Assert.AreEqual("月砂遗迹", egypt.ThemeLabel);
            Assert.AreEqual("月砂祭坛", egypt.DefenseTargetLabel);
            Assert.IsTrue(egypt.IsPlayableInP0);
            Assert.IsFalse(egypt.IsPlaceholder);
            Assert.IsTrue(P0DreamMapCatalog.IsKnownMapId(egypt.Id));
            StringAssert.Contains("P0可玩", egypt.BuildSummary());
        }

        [Test]
        public void P0RouteCatalog_EgyptPlayableRouteSharesRouteShapeAndCombatContent()
        {
            RouteDefinition bedroom = P0RouteCatalog.CreateTenLayerRoute();
            RouteDefinition egypt = P0RouteCatalog.CreateEgyptPlayableRoute();

            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, bedroom.DreamMap.Id);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, egypt.DreamMap.Id);
            Assert.AreEqual(bedroom.LayerCount, egypt.LayerCount);
            Assert.AreEqual(bedroom.Nodes[0].ContentId, egypt.Nodes[0].ContentId);
            Assert.AreEqual(bedroom.Nodes[bedroom.Nodes.Count - 1].ContentId, egypt.Nodes[egypt.Nodes.Count - 1].ContentId);
        }

        [Test]
        public void P0RunSession_StartNewRunCanBindDreamMapContext()
        {
            P0RunSession.Clear();

            P0RunSession.StartNewRun(P0DreamMapCatalog.GetEgyptDreamMap());

            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, P0RunSession.CurrentRun.DreamMap.Id);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, P0RunSession.CurrentRoute.Route.DreamMap.Id);
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_EnsureBedroomDreamRunPreservesActiveBedroomRun()
        {
            P0RunSession.Clear();
            RunRouteState started = P0RunSession.StartNewRun(new[] { "suzune", "saiban" });

            RunRouteState preserved = P0RunSession.EnsureBedroomDreamRun();

            Assert.AreSame(started, preserved);
            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, P0RunSession.CurrentRun.DreamMap.Id);
            Assert.AreEqual(2, P0RunSession.CurrentRun.Roster.Count);
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("suzune"));
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("saiban"));
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_EnsureBedroomDreamRunConvertsEgyptRunWithoutLosingRoster()
        {
            P0RunSession.Clear();
            RunRouteState egypt = P0RunSession.StartNewRun(
                new[] { "suzune", "saiban" },
                P0DreamMapCatalog.GetEgyptDreamMap());

            RunRouteState bedroom = P0RunSession.EnsureBedroomDreamRun();

            Assert.AreNotSame(egypt, bedroom);
            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, P0RunSession.CurrentRun.DreamMap.Id);
            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, P0RunSession.CurrentRoute.Route.DreamMap.Id);
            Assert.AreEqual(2, P0RunSession.CurrentRun.Roster.Count);
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("suzune"));
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("saiban"));
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_EnsureEgyptDreamRunPreservesActiveEgyptRun()
        {
            P0RunSession.Clear();
            RunRouteState started = P0RunSession.StartNewRun(
                new[] { "suzune", "saiban" },
                P0DreamMapCatalog.GetEgyptDreamMap());

            RunRouteState preserved = P0RunSession.EnsureEgyptDreamRun();

            Assert.AreSame(started, preserved);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, P0RunSession.CurrentRun.DreamMap.Id);
            Assert.AreEqual(2, P0RunSession.CurrentRun.Roster.Count);
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("suzune"));
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("saiban"));
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_EnsureEgyptDreamRunConvertsBedroomRunWithoutChangingBedroomHelper()
        {
            P0RunSession.Clear();
            RunRouteState bedroom = P0RunSession.StartNewRun(new[] { "suzune", "saiban" });

            RunRouteState egypt = P0RunSession.EnsureEgyptDreamRun();

            Assert.AreNotSame(bedroom, egypt);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, P0RunSession.CurrentRun.DreamMap.Id);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, P0RunSession.CurrentRoute.Route.DreamMap.Id);
            Assert.AreEqual(2, P0RunSession.CurrentRun.Roster.Count);
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("suzune"));
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("saiban"));

            RunRouteState bedroomAgain = P0RunSession.EnsureBedroomDreamRun();

            Assert.AreNotSame(egypt, bedroomAgain);
            Assert.AreEqual(P0DreamMapCatalog.BedroomDreamMapId, P0RunSession.CurrentRun.DreamMap.Id);
            P0RunSession.Clear();
        }

        [Test]
        public void RunRouteState_SuccessAdvancesToNextNode()
        {
            RunRouteState state = new RunRouteState(P0RouteCatalog.CreateTenLayerRoute());

            state.CompleteCurrentNode(NodeResult.Success);

            Assert.AreEqual(1, state.CompletedCount);
            Assert.AreEqual(2, state.CurrentNode.Layer);
            Assert.IsFalse(state.IsComplete);
        }

        [Test]
        public void RunRouteState_SelectCurrentNode_UsesAlternativeForCompletion()
        {
            RouteDefinition route = new RouteDefinition(
                "test_branching",
                new[]
                {
                    new[]
                    {
                        new RouteNodeDefinition(1, "default_defense", RouteNodeType.Defense, "layer_01_defense"),
                        new RouteNodeDefinition(1, "event_choice", RouteNodeType.DreamEvent, "event_unread_red_dot_rain")
                    },
                    new[]
                    {
                        new RouteNodeDefinition(2, "boss", RouteNodeType.Boss, "boss_call_tyrant")
                    }
                });
            RunRouteState state = new RunRouteState(route);

            Assert.AreEqual("default_defense", state.CurrentNode.Id);
            Assert.AreEqual(2, state.CurrentLayerOptions.Count);
            Assert.IsFalse(state.IsCurrentNodeExplicitlySelected);

            bool selected = state.SelectCurrentNode("event_choice");
            state.CompleteCurrentNode(NodeResult.Success);

            Assert.IsTrue(selected);
            Assert.AreEqual("event_choice", state.Completions[0].Node.Id);
            Assert.AreEqual(2, state.CurrentLayer);
        }

        [Test]
        public void P0RouteNodePresenter_DescribesEveryRouteOptionWithPlayerFacingHints()
        {
            RouteDefinition route = P0RouteCatalog.CreateTenLayerRoute();

            for (int layer = 1; layer <= route.LayerCount; layer++)
            {
                foreach (RouteNodeDefinition node in route.GetLayerOptions(layer))
                {
                    RouteNodePresentation presentation = P0RouteNodePresenter.Describe(node);

                    Assert.IsNotEmpty(presentation.Title, node.Id);
                    Assert.IsNotEmpty(presentation.RiskHint, node.Id);
                    Assert.IsNotEmpty(presentation.RewardHint, node.Id);
                    Assert.AreEqual(RouteNodeResolver.RequiresBattle(node.NodeType), presentation.RequiresBattle, node.Id);
                    StringAssert.DoesNotContain(node.ContentId, presentation.BuildCompactLabel());
                }
            }
        }

        [Test]
        public void P0RouteNodePresenter_BuildRouteChoiceLabelMarksSelectedNode()
        {
            RouteNodeDefinition node = P0RouteCatalog.CreateTenLayerRoute().GetLayerOptions(1)[0];

            string selectedLabel = P0RouteNodePresenter.BuildRouteChoiceLabel(node, null, isSelected: true);
            string unselectedLabel = P0RouteNodePresenter.BuildRouteChoiceLabel(node, null, isSelected: false);

            StringAssert.StartsWith("> 1. 卧室门槛", selectedLabel);
            StringAssert.StartsWith("1. 卧室门槛", unselectedLabel);
            StringAssert.DoesNotContain(P0RouteCatalog.LayerOneDefenseId, selectedLabel);
        }

        [Test]
        public void P0RouteNodePresenter_AdaptsRewardHintsToRunState()
        {
            RouteNodeDefinition partnerNode = new RouteNodeDefinition(1, "partner", RouteNodeType.Partner, "partner_shadowmaru_preview");
            RouteNodeDefinition shopNode = new RouteNodeDefinition(1, "shop", RouteNodeType.Shop, "shop_midnight_kibble");
            RouteNodeDefinition blessingNode = new RouteNodeDefinition(1, "blessing", RouteNodeType.BlessingOffering, "authority_blessing");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });

            Assert.AreEqual("预览伙伴 + 小鱼干", P0RouteNodePresenter.Describe(partnerNode, run).RewardHint);
            run.Roster.AddCat(P0RouteRewardResolver.PreviewPartnerId);
            Assert.AreEqual("重复伙伴转资源", P0RouteNodePresenter.Describe(partnerNode, run).RewardHint);
            Assert.AreEqual("领取试吃", P0RouteNodePresenter.Describe(shopNode, run).RewardHint);

            foreach (AuthorityBlessingDefinition blessing in P0BlessingCatalog.CreateAuthorityBlessings())
            {
                run.Blessings.Add(blessing);
                run.Blessings.Upgrade(blessing.Id);
                run.Blessings.Upgrade(blessing.Id);
            }

            Assert.AreEqual("收集残余梦尘", P0RouteNodePresenter.Describe(blessingNode, run).RewardHint);
        }

        [Test]
        public void RunRouteState_FailureEndsRoute()
        {
            RunRouteState state = new RunRouteState(P0RouteCatalog.CreateTenLayerRoute());

            state.CompleteCurrentNode(NodeResult.Failure);

            Assert.IsTrue(state.IsFailed);
            Assert.IsTrue(state.IsComplete);
            Assert.IsNull(state.CurrentNode);
        }

        [Test]
        public void P0RunSession_EnsureAnyRun_PreservesCompletedRoute()
        {
            P0RunSession.Clear();
            RunRouteState started = P0RunSession.StartNewRun();
            started.CompleteCurrentNode(NodeResult.Failure);

            RunRouteState preserved = P0RunSession.EnsureAnyRun();

            Assert.AreSame(started, preserved);
            Assert.IsTrue(preserved.IsFailed);
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_StartNewRun_CreatesProgressionWithStarterRoster()
        {
            P0RunSession.Clear();

            P0RunSession.StartNewRun();

            Assert.IsNotNull(P0RunSession.CurrentRun);
            Assert.AreEqual(3, P0RunSession.CurrentRun.Roster.Count);
            Assert.AreSame(P0RunSession.CurrentRun.Route, P0RunSession.CurrentRoute);
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_StartNewRun_ClearsLastCompletionReport()
        {
            P0RunSession.Clear();
            P0RunSession.StartNewRun();
            P0RunSession.CompleteCurrentNode(NodeResult.Success);

            Assert.IsNotNull(P0RunSession.LastCompletionReport);

            P0RunSession.StartNewRun();

            Assert.IsNull(P0RunSession.LastCompletionReport);
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_CompleteCurrentBattleNode_RecordsRewardReport()
        {
            P0RunSession.Clear();
            P0RunSession.StartNewRun();

            RunNodeCompletionReport report = P0RunSession.CompleteCurrentNode(NodeResult.Success);

            Assert.IsNotNull(report);
            Assert.AreSame(report, P0RunSession.LastCompletionReport);
            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, report.CompletedNode.Id);
            Assert.AreEqual(NodeResult.Success, report.Result);
            Assert.AreEqual(0, report.BattleReward.DreamShards);
            Assert.AreEqual(1, report.BattleReward.FishTreats);
            Assert.AreEqual(RunCatUpgradeState.DefenseExperience, report.BattleReward.TeamExperience);
            Assert.AreEqual(1, P0RunSession.CurrentRun.Wallet.FishTreats);
            Assert.AreEqual(RunCatUpgradeState.DefenseExperience, P0RunSession.CurrentRun.CatUpgrades.TeamExperience);
            Assert.AreEqual(2, P0RunSession.CurrentRoute.CurrentLayer);
            Assert.IsNotNull(report.NextNode);
            StringAssert.Contains("卧室门槛", report.BuildSummary());
            StringAssert.Contains("+1 小鱼干", report.BuildSummary());
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_CompleteBossNode_ReportMarksClearedAndRewards()
        {
            P0RunSession.Clear();
            P0RunSession.StartNewRun();
            while (P0RunSession.CurrentRoute.CurrentLayer < 10)
            {
                P0RunSession.CompleteCurrentNode(NodeResult.Success);
            }

            RunNodeCompletionReport report = P0RunSession.CompleteCurrentNode(NodeResult.Success);

            Assert.IsTrue(P0RunSession.CurrentRoute.IsCleared);
            Assert.IsTrue(report.IsRouteCleared);
            Assert.AreEqual(P0RouteCatalog.BossNodeId, report.CompletedNode.Id);
            Assert.AreEqual(5, report.BattleReward.DreamShards);
            Assert.AreEqual(3, report.BattleReward.FishTreats);
            Assert.AreEqual(RunCatUpgradeState.BossExperience, report.BattleReward.TeamExperience);
            StringAssert.Contains("路线通关", report.BuildSummary());
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_StartNewRunWithSelection_UsesSelectedStarterRoster()
        {
            P0RunSession.Clear();

            P0RunSession.StartNewRun(new[] { "suzune", "saiban" });

            Assert.AreEqual(2, P0RunSession.CurrentRun.Roster.Count);
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("suzune"));
            Assert.IsTrue(P0RunSession.CurrentRun.Roster.HasCat("saiban"));
            Assert.IsFalse(P0RunSession.CurrentRun.Roster.HasCat("nephthys"));
            Assert.AreEqual("suzune", P0RunSession.CurrentRun.Roster.CatIds[0]);
            P0RunSession.Clear();
        }

        [Test]
        public void P0RunSession_NormalizeStarterCatIds_FallsBackToDefaultTrio()
        {
            var starters = P0RunSession.NormalizeStarterCatIds(new[] { "unknown", "saiban", "saiban" });

            Assert.AreEqual(1, starters.Count);
            Assert.AreEqual("saiban", starters[0]);

            var fallback = P0RunSession.NormalizeStarterCatIds(new[] { "unknown" });

            Assert.AreEqual(3, fallback.Count);
            Assert.AreEqual("saiban", fallback[0]);
            Assert.AreEqual("nephthys", fallback[1]);
            Assert.AreEqual("suzune", fallback[2]);
        }

        [Test]
        public void RunProgressionState_MetricsAccumulateAcrossBattleNodes()
        {
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            NodeMetrics first = run.Metrics.BeginNode(1, "layer_01_defense", 100f);
            first.RecordLitterBoxUse();
            first.RecordBedCareUse();
            first.RecordBedPressure(10f, 8f);
            first.Complete(NodeResult.Success, 12f, 94f);
            NodeMetrics second = run.Metrics.BeginNode(3, "elite_cold_light_shadow", 94f);
            second.RecordFeederUse();
            second.RecordSleepMaxLoss(10f);
            second.RecordWeakIncident();
            second.RecordBossThrowPressure(4f, 0f);
            second.Complete(NodeResult.Failure, 18f, 60f);

            P0RunSettlementSummary summary = new P0RunSettlementSummary(run);

            Assert.AreEqual(2, summary.BattleNodeCount);
            Assert.AreEqual(1, summary.BattleSuccesses);
            Assert.AreEqual(1, summary.BattleFailures);
            Assert.AreEqual(30f, summary.DurationSeconds, 0.001f);
            Assert.AreEqual(-40f, summary.SleepDelta, 0.001f);
            Assert.AreEqual(1, summary.BedCareUses);
            Assert.AreEqual(1, summary.LitterBoxUses);
            Assert.AreEqual(1, summary.FeederUses);
            Assert.AreEqual(10f, summary.SleepMaxLost, 0.001f);
            Assert.AreEqual(1, summary.WeakIncidents);
            Assert.AreEqual(1, summary.BedPressureHits);
            Assert.AreEqual(1, summary.BossThrowPressureHits);
            Assert.AreEqual(2, summary.EnemySleepPressureEvents);
            Assert.AreEqual(14f, summary.EnemySleepDamageIncoming, 0.001f);
            Assert.AreEqual(8f, summary.EnemySleepDamageTaken, 0.001f);
            Assert.AreEqual(6f, summary.EnemySleepDamageAbsorbed, 0.001f);
        }

        [Test]
        public void P0RunSettlementSummary_ReportsRouteResultAndRunState()
        {
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban", "suzune" });
            run.Wallet.AddDreamShards(3);
            run.Wallet.AddFishTreats(2);
            run.Blessings.Add(P0BlessingCatalog.CreateAuthorityBlessings()[0]);
            run.CoreValues.Capture(73f, 90f, 100f, 41f, 82f);
            run.CatVitals.Capture("saiban", 220f, 110f, 0f);
            run.CatVitals.Capture("suzune", 130f, 0f, 12f);
            while (!run.Route.IsComplete)
            {
                run.Route.CompleteCurrentNode(NodeResult.Success);
            }

            P0RunSettlementSummary summary = new P0RunSettlementSummary(run);

            Assert.IsTrue(summary.IsCleared);
            Assert.AreEqual("路线通关", summary.ResultLabel);
            Assert.AreEqual(10, summary.CompletedNodes);
            Assert.AreEqual(10, summary.TotalLayers);
            Assert.AreEqual(3, summary.DreamShards);
            Assert.AreEqual(2, summary.FishTreats);
            Assert.AreEqual(2, summary.RosterCount);
            Assert.AreEqual(1, summary.BlessingCount);
            Assert.AreEqual(73f, summary.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(90f, summary.OwnerSleepMax, 0.001f);
            Assert.AreEqual(41f, summary.TeamPoop, 0.001f);
            Assert.AreEqual(82f, summary.TeamHunger, 0.001f);
            Assert.AreEqual(2, summary.CatVitalCount);
            Assert.AreEqual(1, summary.WeakCatCount);
            Assert.AreEqual(0f, summary.LowestCatHpRatio, 0.001f);
        }

        [Test]
        public void RouteNodeResolver_PlaceholderNodeAdvancesRoute()
        {
            RunRouteState state = new RunRouteState(P0RouteCatalog.CreateTenLayerRoute());
            state.CompleteCurrentNode(NodeResult.Success);

            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(state);

            Assert.AreEqual(RouteNodeResolutionType.PlaceholderResolved, resolution.ResolutionType);
            Assert.AreEqual(RouteNodeType.DreamEvent, resolution.Node.NodeType);
            Assert.AreEqual(3, state.CurrentNode.Layer);
        }

        [Test]
        public void RouteNodeResolver_ProgressionDreamEventAddsFishTreats()
        {
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.Route.CompleteCurrentNode(NodeResult.Success);

            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run);

            Assert.AreEqual(RouteNodeResolutionType.PlaceholderResolved, resolution.ResolutionType);
            Assert.AreEqual(0, run.Wallet.DreamShards);
            Assert.AreEqual(2, run.Wallet.FishTreats);
            Assert.AreEqual(1, run.DreamEventsResolved);
            Assert.AreEqual(3, run.Route.CurrentNode.Layer);
        }

        [Test]
        public void RouteNodeResolver_BossNodeRequestsBattle()
        {
            RunRouteState state = new RunRouteState(P0RouteCatalog.CreateTenLayerRoute());
            for (int i = 0; i < 9; i++)
            {
                state.CompleteCurrentNode(NodeResult.Success);
            }

            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(state);

            Assert.AreEqual(RouteNodeResolutionType.NeedsBattle, resolution.ResolutionType);
            Assert.AreEqual(RouteNodeType.Boss, resolution.Node.NodeType);
            Assert.IsTrue(resolution.ShouldLoadBattle);
        }

        [Test]
        public void RouteNodeResolver_ProgressionAppliesPartnerShopBlessingAndRestRewards()
        {
            RouteDefinition route = new RouteDefinition(
                "test_rewards",
                new[]
                {
                    new RouteNodeDefinition(1, "partner", RouteNodeType.Partner, "partner_shadowmaru_preview"),
                    new RouteNodeDefinition(2, "shop", RouteNodeType.Shop, "shop_midnight_kibble"),
                    new RouteNodeDefinition(3, "blessing", RouteNodeType.BlessingOffering, "authority_blessing"),
                    new RouteNodeDefinition(4, "rest", RouteNodeType.RestNest, "rest_nest")
                });
            RunProgressionState run = new RunProgressionState(route, new[] { "saiban", "nephthys", "suzune" });
            run.Wallet.AddDreamShards(1);
            run.CoreValues.Capture(40f, 80f, 100f, 70f, 20f);
            run.CatVitals.Capture("saiban", 220f, 50f, 8f);
            run.CatVitals.Capture("nephthys", 110f, 100f, 0f);

            RouteNodeResolver.ResolveCurrentNode(run);
            RouteNodeResolver.ResolveCurrentNode(run);
            RouteNodeResolver.ResolveCurrentNode(run);
            RouteNodeResolver.ResolveCurrentNode(run);

            Assert.IsTrue(run.Roster.HasCat(P0RouteRewardResolver.PreviewPartnerId));
            Assert.AreEqual(4, run.Roster.Count);
            Assert.AreEqual(1, run.Wallet.DreamShards);
            Assert.AreEqual(2, run.Wallet.FishTreats);
            Assert.AreEqual(1, run.ShopPurchases);
            Assert.AreEqual(1, run.Blessings.Count);
            Assert.AreEqual(1, run.RestNestUses);
            Assert.AreEqual(65f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(40f, run.CoreValues.TeamPoop, 0.001f);
            Assert.AreEqual(80f, run.CoreValues.TeamHunger, 0.001f);
            Assert.IsTrue(run.CatVitals.TryGet("saiban", out RunCatVitalSnapshot saiban));
            Assert.AreEqual(154f, saiban.CurrentHp, 0.001f);
            Assert.IsFalse(saiban.IsWeak);
            Assert.IsTrue(run.Route.IsCleared);
        }

        [Test]
        public void P0RouteRewardResolver_RestNestChoiceReportsAndAppliesCoreRecovery()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "rest", RouteNodeType.RestNest, "rest_nest");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.CoreValues.Capture(60f, 70f, 100f, 25f, 95f);
            run.CatVitals.Capture("saiban", 200f, 20f, 10f);

            RouteRewardChoice choice = P0RouteRewardResolver.GetDefaultPlaceholderChoice(node, run);
            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            StringAssert.Contains("sleep +25", choice.BuildDiagnosticSummary());
            StringAssert.Contains("poop -30", choice.BuildDiagnosticSummary());
            StringAssert.Contains("hunger >=80", choice.BuildDiagnosticSummary());
            StringAssert.Contains("cat hp >=70%", choice.BuildDiagnosticSummary());
            Assert.AreEqual(70f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(0f, run.CoreValues.TeamPoop, 0.001f);
            Assert.AreEqual(95f, run.CoreValues.TeamHunger, 0.001f);
            Assert.IsTrue(run.CatVitals.TryGet("saiban", out RunCatVitalSnapshot saiban));
            Assert.AreEqual(140f, saiban.CurrentHp, 0.001f);
            Assert.IsFalse(saiban.IsWeak);
            Assert.AreEqual(1, run.RestNestUses);
        }

        [Test]
        public void P0RouteRewardResolver_DreamEventRewardsVaryByContentId()
        {
            RouteNodeDefinition softRainNode = new RouteNodeDefinition(1, "soft_rain", RouteNodeType.DreamEvent, P0RouteCatalog.SoftRainWindowEventContentId);
            RouteNodeDefinition redDotNode = new RouteNodeDefinition(1, "red_dot", RouteNodeType.DreamEvent, P0RouteCatalog.UnreadRedDotRainEventContentId);
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });

            var softRainChoices = P0RouteRewardResolver.CreatePlaceholderChoices(softRainNode, run);
            var redDotChoices = P0RouteRewardResolver.CreatePlaceholderChoices(redDotNode, run);

            Assert.AreEqual("dream_event_clear_notifications", softRainChoices[0].Id);
            Assert.AreEqual("dream_event_red_dot_cleanup", redDotChoices[0].Id);
            Assert.AreNotEqual(softRainChoices[0].Id, redDotChoices[0].Id);
            Assert.AreEqual(2, softRainChoices[0].FishTreatsGained);
            Assert.AreEqual(3, redDotChoices[0].FishTreatsGained);
            StringAssert.DoesNotContain("_", redDotChoices[0].BuildSummary());
        }

        [Test]
        public void P0RouteRewardResolver_DreamEventCanQueueNextBattlePressure()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "event", RouteNodeType.DreamEvent, P0RouteCatalog.UnreadRedDotRainEventContentId);
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.Id == "dream_event_red_dot_ignore");

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.AreEqual(RouteRewardChoiceType.GainEventItem, choice.ChoiceType);
            Assert.AreEqual(RunEventItemInventory.BlankWishTagId, choice.EventItemId);
            Assert.AreEqual(1, choice.EventItemCount);
            StringAssert.Contains("item blank_wish_tag +1", choice.BuildDiagnosticSummary());
            StringAssert.Contains("next poop +75%", choice.BuildDiagnosticSummary());
            Assert.IsTrue(run.PendingBattleModifiers.HasPending);
            Assert.AreEqual(1, run.EventItems.Count(RunEventItemInventory.BlankWishTagId));

            RunPendingBattleModifierSnapshot snapshot = run.PendingBattleModifiers.Consume();
            BattleModifierSet modifiers = snapshot.ApplyTo(BattleModifierSet.Neutral);
            P0Tuning tuning = snapshot.ApplyTo(P0Tuning.Default);

            Assert.AreEqual(1f, modifiers.SkillDamageMultiplier, 0.001f);
            Assert.AreEqual(0.525f, tuning.PoopNaturalGrowthPerSecond, 0.001f);
            Assert.IsFalse(run.PendingBattleModifiers.HasPending);
            Assert.AreEqual(1, run.DreamEventsResolved);
        }

        [Test]
        public void P0RouteRewardResolver_FadedFishBagBoostsAndConsumesNextEventFishReward()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "event", RouteNodeType.DreamEvent, P0RouteCatalog.UnreadRedDotRainEventContentId);
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.EventItems.Add(RunEventItemInventory.FadedFishBagId);

            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.Id == "dream_event_red_dot_cleanup");

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.AreEqual(4, choice.FishTreatsGained);
            Assert.AreEqual(3, choice.BaseFishTreatsGained);
            StringAssert.Contains("base fish +3", choice.BuildDiagnosticSummary());
            Assert.AreEqual(4, run.Wallet.FishTreats);
            Assert.AreEqual(0, run.EventItems.Count(RunEventItemInventory.FadedFishBagId));
            Assert.AreEqual(1, run.DreamEventsResolved);
        }

        [Test]
        public void P0RouteRewardResolver_BlankWishTagAddsExtraEventOptionAndConsumesOnResolve()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "event", RouteNodeType.DreamEvent, P0RouteCatalog.SoftRainWindowEventContentId);
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.EventItems.Add(RunEventItemInventory.BlankWishTagId);

            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.Id == "dream_event_blank_wish_extra");

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.AreEqual(RouteRewardChoiceType.GainEventItem, choice.ChoiceType);
            Assert.AreEqual(RunEventItemInventory.FadedFishBagId, choice.EventItemId);
            Assert.AreEqual(0, run.EventItems.Count(RunEventItemInventory.BlankWishTagId));
            Assert.AreEqual(1, run.EventItems.Count(RunEventItemInventory.FadedFishBagId));
            Assert.AreEqual(1, run.DreamEventsResolved);
        }

        [Test]
        public void P0RouteRewardResolver_ShopSupplySpendsFishAndRestoresCoreValue()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "shop", RouteNodeType.Shop, "shop_midnight_kibble");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.Wallet.AddFishTreats(3);
            run.CoreValues.Capture(35f, 100f, 100f, 20f, 50f);
            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.Id == "shop_bed_patch");

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            StringAssert.Contains("cost 3 fish", choice.BuildDiagnosticSummary());
            StringAssert.Contains("sleep +20", choice.BuildDiagnosticSummary());
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(55f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(1, run.ShopPurchases);
        }

        [Test]
        public void P0RouteRewardResolver_FoldedCouponDiscountsAndConsumesNextShopPurchase()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "shop", RouteNodeType.Shop, "shop_midnight_kibble");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.Wallet.AddFishTreats(2);
            run.EventItems.Add(RunEventItemInventory.FoldedCouponId);
            run.CoreValues.Capture(35f, 100f, 100f, 20f, 50f);

            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.Id == "shop_bed_patch");

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.AreEqual(2, choice.FishTreatsCost);
            Assert.AreEqual(3, choice.BaseFishTreatsCost);
            StringAssert.Contains("base fish cost 3", choice.BuildDiagnosticSummary());
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(0, run.EventItems.Count(RunEventItemInventory.FoldedCouponId));
            Assert.AreEqual(55f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(1, run.ShopPurchases);
        }

        [Test]
        public void RunCatUpgradeState_PendingOfferUsesJoinedRosterAndRecordsSelection()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "suzune" });

            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);

            Assert.IsTrue(run.CatUpgrades.HasPendingUpgrade);
            Assert.AreEqual(0, run.CatUpgrades.TeamExperience);
            var offer = run.CatUpgrades.CreateCurrentOffer(run.Roster);
            Assert.AreEqual(4, offer.Count);
            Assert.IsTrue(offer.Any(candidate => candidate.CatId == "saiban"));
            Assert.IsTrue(offer.Any(candidate => candidate.CatId == "suzune"));
            Assert.IsFalse(offer.Any(candidate => candidate.CatId == "nephthys"));
            Assert.AreEqual(CatUpgradeStage.Passive, offer[0].Stage);
            for (int i = 0; i < offer.Count; i++)
            {
                Assert.IsNotEmpty(offer[i].PlayerIntent, offer[i].Id);
                StringAssert.Contains(offer[i].PlayerIntent, offer[i].BuildSummary(), offer[i].Id);
                StringAssert.DoesNotContain(" | ", offer[i].BuildSummary(), offer[i].Id);
                StringAssert.DoesNotContain(" - ", offer[i].BuildSummary(), offer[i].Id);
            }

            bool selected = run.CatUpgrades.TrySelect(offer[0].Id, run.Roster, out CatUpgradeCandidate selectedCandidate);

            Assert.IsTrue(selected);
            Assert.IsFalse(run.CatUpgrades.HasPendingUpgrade);
            Assert.AreEqual(1, run.CatUpgrades.GetUpgradeCount(selectedCandidate.CatId));
            Assert.AreEqual(1, run.CatUpgrades.LearnedUpgradeCount);
            StringAssert.Contains(selectedCandidate.DisplayName, run.CatUpgrades.LastResolvedSummary);
        }

        [Test]
        public void P0CatUpgradeCatalog_StarterPoolsMatchTwoFourTwoDesign()
        {
            string[] catIds =
            {
                P0PrototypeCatalog.SaibanId,
                P0PrototypeCatalog.NephthysId,
                P0PrototypeCatalog.SuzuneId
            };

            for (int i = 0; i < catIds.Length; i++)
            {
                Assert.AreEqual(2, P0CatUpgradeCatalog.GetStageCandidateCount(catIds[i], CatUpgradeStage.Passive), catIds[i]);
                Assert.AreEqual(4, P0CatUpgradeCatalog.GetStageCandidateCount(catIds[i], CatUpgradeStage.SmallSkill), catIds[i]);
                Assert.AreEqual(2, P0CatUpgradeCatalog.GetStageCandidateCount(catIds[i], CatUpgradeStage.Ultimate), catIds[i]);
            }
        }

        [Test]
        public void P0CatUpgradeRuntimeCatalog_SelectedUpgradesChangeBattleLoadout()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { P0PrototypeCatalog.SaibanId });
            CatDefinition baseSaiban = P0PrototypeCatalog.CreateStarterCats()
                .Single(cat => cat.Id == P0PrototypeCatalog.SaibanId);

            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            Assert.IsTrue(run.CatUpgrades.TrySelect(P0CatUpgradeCatalog.SaibanPassiveBedlineGuardId, run.Roster, out _));
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            Assert.IsTrue(run.CatUpgrades.TrySelect(P0CatUpgradeCatalog.SaibanSmallBedlineInterceptId, run.Roster, out _));
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            Assert.IsTrue(run.CatUpgrades.TrySelect(P0CatUpgradeCatalog.SaibanUltimateOathDomainId, run.Roster, out _));

            CatDefinition upgradedSaiban = P0CatUpgradeRuntimeCatalog.ApplyRunUpgrades(baseSaiban, run.CatUpgrades);
            var selectedSkills = P0CatUpgradeRuntimeCatalog.CreateSelectedSkillDefinitions(run.CatUpgrades);

            Assert.Greater(upgradedSaiban.MaxHp, baseSaiban.MaxHp);
            Assert.Greater(upgradedSaiban.PhysicalDefense, baseSaiban.PhysicalDefense);
            CollectionAssert.Contains(upgradedSaiban.SkillIds, P0CatUpgradeRuntimeCatalog.SaibanBedlineInterceptSkillId);
            CollectionAssert.Contains(upgradedSaiban.SkillIds, P0CatUpgradeRuntimeCatalog.SaibanOathDomainSkillId);
            Assert.IsTrue(selectedSkills.Any(skill => skill.Id == P0CatUpgradeRuntimeCatalog.SaibanBedlineInterceptSkillId && skill.Slot == SkillSlot.SmallSkill4));
            Assert.IsTrue(selectedSkills.Any(skill => skill.Id == P0CatUpgradeRuntimeCatalog.SaibanOathDomainSkillId && skill.Slot == SkillSlot.Ultimate2));
        }

        [Test]
        public void RunCatUpgradeState_PawStampRerollsCurrentOfferAndConsumesStamp()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
            run.EventItems.Add(RunEventItemInventory.PawStampId);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            string firstSaibanOffer = run.CatUpgrades.CreateCurrentOffer(run.Roster)[0].Id;

            bool rerolled = run.CatUpgrades.TryRerollWithPawStamp(run.EventItems);

            Assert.IsTrue(rerolled);
            Assert.AreEqual(0, run.EventItems.Count(RunEventItemInventory.PawStampId));
            Assert.IsFalse(run.CatUpgrades.CanRerollWithPawStamp(run.EventItems));
            Assert.AreNotEqual(firstSaibanOffer, run.CatUpgrades.CreateCurrentOffer(run.Roster)[0].Id);
        }

        [Test]
        public void P0RouteRewardResolver_PreviewAndApplyBattleRewardStayConsistent()
        {
            RouteNodeDefinition eliteNode = new RouteNodeDefinition(3, "elite", RouteNodeType.Elite, "elite_cold_light_shadow");
            RouteNodeDefinition bossNode = new RouteNodeDefinition(10, "boss", RouteNodeType.Boss, "boss_call_tyrant");
            RouteNodeDefinition eventNode = new RouteNodeDefinition(2, "event", RouteNodeType.DreamEvent, "event_soft_rain_window");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });

            RouteBattleReward elitePreview = P0RouteRewardResolver.PreviewBattleReward(eliteNode);
            RouteBattleReward eliteApplied = P0RouteRewardResolver.ApplyBattleReward(eliteNode, run);
            RouteBattleReward bossPreview = P0RouteRewardResolver.PreviewBattleReward(bossNode);
            RouteBattleReward eventPreview = P0RouteRewardResolver.PreviewBattleReward(eventNode);

            Assert.AreEqual(2, elitePreview.DreamShards);
            Assert.AreEqual(1, elitePreview.FishTreats);
            Assert.AreEqual(RunCatUpgradeState.EliteExperience, elitePreview.TeamExperience);
            Assert.AreEqual(elitePreview.DreamShards, eliteApplied.DreamShards);
            Assert.AreEqual(elitePreview.FishTreats, eliteApplied.FishTreats);
            Assert.AreEqual(elitePreview.TeamExperience, eliteApplied.TeamExperience);
            Assert.AreEqual(2, run.Wallet.DreamShards);
            Assert.AreEqual(1, run.Wallet.FishTreats);
            Assert.AreEqual(RunCatUpgradeState.EliteExperience, run.CatUpgrades.TeamExperience);
            Assert.AreEqual(5, bossPreview.DreamShards);
            Assert.AreEqual(3, bossPreview.FishTreats);
            Assert.AreEqual(RunCatUpgradeState.BossExperience, bossPreview.TeamExperience);
            Assert.IsFalse(eventPreview.HasReward);
            Assert.AreEqual(0, eventPreview.TeamExperience);
            Assert.AreEqual("无奖励", eventPreview.BuildSummary());
        }

        [Test]
        public void P0RouteRewardResolver_BlessingChoicesCanSelectSpecificAuthority()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "blessing", RouteNodeType.BlessingOffering, "authority_blessing");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban", "nephthys", "suzune" });
            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.AuthorityBlessing != null && candidate.AuthorityBlessing.Id == P0BlessingCatalog.SuzuneLullabyId);

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.IsTrue(run.Blessings.Has(P0BlessingCatalog.SuzuneLullabyId));
            Assert.AreEqual(1, run.PlaceholderRewardsResolved);
        }

        [Test]
        public void P0RouteRewardResolver_BlessingOfferingUpgradesWhenAllAuthorityBlessingsClaimed()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "blessing", RouteNodeType.BlessingOffering, "authority_blessing");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban", "nephthys", "suzune" });
            foreach (AuthorityBlessingDefinition blessing in P0BlessingCatalog.CreateAuthorityBlessings())
            {
                run.Blessings.Add(blessing);
            }

            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.AuthorityBlessingUpgradeId == P0BlessingCatalog.SaibanBedlineId);

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.AreEqual(RouteRewardChoiceType.UpgradeAuthorityBlessing, choice.ChoiceType);
            Assert.AreEqual(2, run.Blessings.GetLevel(P0BlessingCatalog.SaibanBedlineId));
            Assert.AreEqual(4, run.Blessings.TotalLevel);
            Assert.AreEqual(1, run.PlaceholderRewardsResolved);
            StringAssert.Contains("升级 誓约床线", choice.BuildSummary());
            StringAssert.DoesNotContain(P0BlessingCatalog.SaibanBedlineId, choice.BuildSummary());
        }

        [Test]
        public void P0RouteRewardResolver_ShopCanUpgradeOwnedAuthorityBlessing()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "shop", RouteNodeType.Shop, "shop_midnight_kibble");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            AuthorityBlessingDefinition blessing = P0BlessingCatalog.CreateAuthorityBlessings()
                .Single(candidate => candidate.Id == P0BlessingCatalog.SaibanBedlineId);
            run.Blessings.Add(blessing);
            run.Wallet.AddFishTreats(P0RouteRewardResolver.ShopAuthorityBlessingUpgradeCost);

            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(node, run)
                .Single(candidate => candidate.AuthorityBlessingUpgradeId == P0BlessingCatalog.SaibanBedlineId);

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(2, run.Blessings.GetLevel(P0BlessingCatalog.SaibanBedlineId));
            Assert.AreEqual(1, run.ShopPurchases);
            StringAssert.Contains("等级2", choice.DisplayName);
            StringAssert.Contains("升级 誓约床线", choice.BuildSummary());
            StringAssert.DoesNotContain(P0BlessingCatalog.SaibanBedlineId, choice.BuildSummary());
        }

        [Test]
        public void P0BlessingCatalog_GetAuthorityBlessingDisplayName_MapsKnownIds()
        {
            Assert.AreEqual("誓约床线", P0BlessingCatalog.GetAuthorityBlessingDisplayName(P0BlessingCatalog.SaibanBedlineId));
            Assert.AreEqual("月砂支配", P0BlessingCatalog.GetAuthorityBlessingDisplayName(P0BlessingCatalog.NephthysSandglassId));
            Assert.AreEqual("摇篮曲韵律", P0BlessingCatalog.GetAuthorityBlessingDisplayName(P0BlessingCatalog.SuzuneLullabyId));
            Assert.AreEqual("未知祝福", P0BlessingCatalog.GetAuthorityBlessingDisplayName("future_blessing"));
        }

        [Test]
        public void P0RouteRewardResolver_InvalidShopChoiceDoesNotSpendOrRecord()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "shop", RouteNodeType.Shop, "shop_midnight_kibble");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            RouteRewardChoice paidChoice = new RouteRewardChoice(
                "paid",
                "Paid",
                "Costs a shard.",
                RouteRewardChoiceType.PurchaseFishTreats,
                fishTreatsGained: 3,
                dreamShardsCost: 1);

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, paidChoice);

            Assert.IsFalse(applied);
            Assert.AreEqual(0, run.Wallet.DreamShards);
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(0, run.ShopPurchases);
        }

        [Test]
        public void P0RouteRewardResolver_DuplicatePartnerChoiceIsRejected()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "partner", RouteNodeType.Partner, "partner_shadowmaru_preview");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            RouteRewardChoice choice = P0RouteRewardResolver.GetDefaultPlaceholderChoice(node, run);

            bool firstApplied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);
            bool secondApplied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(firstApplied);
            Assert.IsFalse(secondApplied);
            Assert.AreEqual(2, run.Roster.Count);
            Assert.AreEqual(1, run.PlaceholderRewardsResolved);
        }

        [Test]
        public void P0RouteRewardResolver_PartnerRewardSummaryUsesPlayerFacingCatName()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "partner", RouteNodeType.Partner, "partner_shadowmaru_preview");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });

            RouteRewardChoice choice = P0RouteRewardResolver.GetDefaultPlaceholderChoice(node, run);
            string summary = choice.BuildSummary();

            StringAssert.Contains("影丸", summary);
            StringAssert.DoesNotContain(P0RouteRewardResolver.PreviewPartnerId, summary);
        }

        [Test]
        public void P0RouteRewardResolver_PartnerRecruitQueuesNextBattleSupport()
        {
            RouteNodeDefinition node = new RouteNodeDefinition(1, "partner", RouteNodeType.Partner, "partner_shadowmaru_preview");
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            RouteRewardChoice choice = P0RouteRewardResolver.GetDefaultPlaceholderChoice(node, run);

            bool applied = P0RouteRewardResolver.ApplyPlaceholderChoice(node, run, choice);

            Assert.IsTrue(applied);
            Assert.IsTrue(run.Roster.HasCat(P0RouteRewardResolver.PreviewPartnerId));
            Assert.IsTrue(run.PendingBattleModifiers.HasPending);
            StringAssert.Contains("next shield +10%", choice.BuildDiagnosticSummary());
            StringAssert.Contains("next status +10%", choice.BuildDiagnosticSummary());
            StringAssert.Contains("next sleep +10%", choice.BuildDiagnosticSummary());
            StringAssert.Contains("next heal +10%", choice.BuildDiagnosticSummary());

            RunPendingBattleModifierSnapshot snapshot = run.PendingBattleModifiers.Consume();
            BattleModifierSet modifiers = snapshot.ApplyTo(BattleModifierSet.Neutral);
            P0Tuning tuning = snapshot.ApplyTo(P0Tuning.Default);

            Assert.AreEqual(1f, modifiers.SkillDamageMultiplier, 0.001f);
            Assert.AreEqual(1.1f, modifiers.ShieldMultiplier, 0.001f);
            Assert.AreEqual(1.1f, modifiers.EnemyStatusDurationMultiplier, 0.001f);
            Assert.AreEqual(1.1f, modifiers.OwnerSleepRestoreMultiplier, 0.001f);
            Assert.AreEqual(1.1f, modifiers.CatHealMultiplier, 0.001f);
            Assert.AreEqual(P0Tuning.Default.PoopNaturalGrowthPerSecond, tuning.PoopNaturalGrowthPerSecond, 0.001f);
            Assert.IsFalse(run.PendingBattleModifiers.HasPending);
        }

        [Test]
        public void RouteNodeResolver_SelectedRewardChoiceAdvancesNonCombatNode()
        {
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.CoreValues.Capture(40f, 100f, 100f, 0f, 100f);
            RouteRewardChoice choice = P0RouteRewardResolver
                .CreatePlaceholderChoices(run.Route.CurrentNode, run)
                .Single(candidate => candidate.Id == "dream_event_mark_all_read");

            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run, choice);

            Assert.AreEqual(RouteNodeResolutionType.PlaceholderResolved, resolution.ResolutionType);
            Assert.AreEqual(0, run.Wallet.DreamShards);
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(52f, run.CoreValues.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(1, run.DreamEventsResolved);
            Assert.AreEqual(3, run.Route.CurrentNode.Layer);
        }

        [Test]
        public void RouteNodeResolver_InvalidRewardChoiceDoesNotAdvanceRoute()
        {
            RouteDefinition route = new RouteDefinition(
                "test_invalid_choice",
                new[]
                {
                    new RouteNodeDefinition(1, "shop", RouteNodeType.Shop, "shop_midnight_kibble"),
                    new RouteNodeDefinition(2, "boss", RouteNodeType.Boss, "boss_call_tyrant")
                });
            RunProgressionState run = new RunProgressionState(route, new[] { "saiban" });
            RouteRewardChoice paidChoice = new RouteRewardChoice(
                "paid",
                "Paid",
                "Costs a shard.",
                RouteRewardChoiceType.PurchaseFishTreats,
                fishTreatsGained: 3,
                dreamShardsCost: 1);

            RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run, paidChoice);

            Assert.AreEqual(RouteNodeResolutionType.InvalidChoice, resolution.ResolutionType);
            Assert.AreEqual(1, run.Route.CurrentNode.Layer);
            Assert.AreEqual(0, run.Wallet.FishTreats);
            Assert.AreEqual(0, run.ShopPurchases);
        }

        [Test]
        public void P0BlessingCatalog_CreateAuthorityBlessings_CoversThreeStarterCats()
        {
            var blessings = P0BlessingCatalog.CreateAuthorityBlessings();

            Assert.AreEqual(3, blessings.Count);
            Assert.IsTrue(blessings.Any(blessing => blessing.OwnerCatId == "saiban"));
            Assert.IsTrue(blessings.Any(blessing => blessing.OwnerCatId == "nephthys"));
            Assert.IsTrue(blessings.Any(blessing => blessing.OwnerCatId == "suzune"));
        }

        [Test]
        public void RunBlessingInventory_UpgradesBlessingLevelsAndCaps()
        {
            AuthorityBlessingDefinition blessing = P0BlessingCatalog.CreateAuthorityBlessings()[0];
            RunBlessingInventory inventory = new RunBlessingInventory();

            Assert.AreEqual(0, inventory.GetLevel(blessing.Id));
            Assert.IsFalse(inventory.CanUpgrade(blessing.Id));
            Assert.IsTrue(inventory.Add(blessing));
            Assert.IsFalse(inventory.Add(blessing));
            Assert.AreEqual(1, inventory.GetLevel(blessing.Id));
            Assert.AreEqual(1, inventory.TotalLevel);
            Assert.IsTrue(inventory.CanUpgrade(blessing.Id));

            Assert.IsTrue(inventory.Upgrade(blessing.Id));
            Assert.IsTrue(inventory.Upgrade(blessing.Id));
            Assert.IsFalse(inventory.Upgrade(blessing.Id));

            Assert.AreEqual(RunBlessingInventory.MaxLevel, inventory.GetLevel(blessing.Id));
            Assert.AreEqual(RunBlessingInventory.MaxLevel, inventory.TotalLevel);
            StringAssert.Contains("等级3", inventory.BuildSummary());
        }

        [Test]
        public void P0BlessingCatalog_CreateBattleModifiers_ConvertsAuthorityBlessings()
        {
            RunBlessingInventory inventory = new RunBlessingInventory();
            foreach (AuthorityBlessingDefinition blessing in P0BlessingCatalog.CreateAuthorityBlessings())
            {
                inventory.Add(blessing);
            }

            BattleModifierSet modifiers = P0BlessingCatalog.CreateBattleModifiers(inventory);

            Assert.AreEqual(1.25f, modifiers.ShieldMultiplier, 0.001f);
            Assert.AreEqual(1.15f, modifiers.KnockbackMultiplier, 0.001f);
            Assert.AreEqual(1.25f, modifiers.EnemyStatusDurationMultiplier, 0.001f);
            Assert.AreEqual(1.2f, modifiers.OwnerSleepRestoreMultiplier, 0.001f);
            Assert.AreEqual(1.2f, modifiers.CatHealMultiplier, 0.001f);
        }

        [Test]
        public void P0BlessingCatalog_CreateBattleModifiers_ScalesAuthorityBlessingLevels()
        {
            AuthorityBlessingDefinition saibanBlessing = P0BlessingCatalog.CreateAuthorityBlessings()
                .Single(blessing => blessing.Id == P0BlessingCatalog.SaibanBedlineId);
            RunBlessingInventory inventory = new RunBlessingInventory();
            inventory.Add(saibanBlessing);
            inventory.Upgrade(saibanBlessing.Id);

            BattleModifierSet modifiers = P0BlessingCatalog.CreateBattleModifiers(inventory);

            Assert.AreEqual(1.5f, modifiers.ShieldMultiplier, 0.001f);
            Assert.AreEqual(1.3f, modifiers.KnockbackMultiplier, 0.001f);
            Assert.AreEqual(1f, modifiers.EnemyStatusDurationMultiplier, 0.001f);
            Assert.AreEqual(1f, modifiers.OwnerSleepRestoreMultiplier, 0.001f);
            Assert.AreEqual(1f, modifiers.CatHealMultiplier, 0.001f);
        }

        [Test]
        public void RouteNodeResolver_ProgressionCanResolveTenLayerRouteToBossClear()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });

            while (!run.Route.IsComplete)
            {
                RouteNodeDefinition node = run.Route.CurrentNode;
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

            Assert.IsTrue(run.Route.IsCleared);
            Assert.AreEqual(10, run.Route.CompletedCount);
            Assert.AreEqual(1, run.DreamEventsResolved);
            Assert.AreEqual(1, run.ShopPurchases);
            Assert.AreEqual(1, run.RestNestUses);
            Assert.AreEqual(1, run.Blessings.Count);
            Assert.IsTrue(run.Roster.HasCat(P0RouteRewardResolver.PreviewPartnerId));
            Assert.Greater(run.Wallet.DreamShards, 0);
            Assert.Greater(run.Wallet.FishTreats, 0);
        }
    }
}
