using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0RouteMapSurfaceCoverageTests
    {
        [Test]
        public void BuildSurface_DefaultRouteShowsCurrentBattleAndActions()
        {
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(CreateRun(), "就绪");

            Assert.IsTrue(P0RouteMapPresenter.HasP0RouteMapSurface(surface));
            Assert.AreEqual("进度：0/10", surface.ProgressLabel);
            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, surface.CurrentNode.NodeId);
            Assert.IsTrue(surface.CurrentNode.RequiresBattle);
            Assert.AreEqual(P0VisualAssetCatalog.DefenseRouteNodeIconId, surface.CurrentNode.VisualAsset.AssetId);
            Assert.AreEqual(10, surface.LayerRows.Count);
            Assert.IsTrue(surface.TryGetAction(P0RouteMapActionIds.EnterCurrentNode, out P0RouteMapAction enter));
            Assert.AreEqual(P0SceneFlow.GrayboxBattleSceneName, enter.TargetSceneName);
            Assert.IsTrue(P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell));
            Assert.AreEqual(P0VisualAssetCatalog.DreamGlassPanelId, surface.UiShell.DreamGlassPanel.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PrimaryButtonId, surface.UiShell.PrimaryButton.AssetId);
            StringAssert.Contains("资源", surface.SummaryRows[0]);
            Assert.AreEqual("未来", surface.LayerRows[1].StateToken);
            StringAssert.Contains("未知分支 x2", surface.LayerRows[1].OptionPreview);
            StringAssert.DoesNotContain("柔雨窗台", surface.LayerRows[1].OptionPreview);
            StringAssert.DoesNotContain("午夜猫粮商店", surface.LayerRows[1].OptionPreview);
            Assert.IsTrue(Contains(surface.SummaryRows, "旧梦地图：未持有"));
        }

        [Test]
        public void BuildSurface_ShowsDreamMapContextForEgyptPlaceholder()
        {
            DreamMapDefinition egypt = P0DreamMapCatalog.GetEgyptDreamMap();
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateEgyptPlaceholderRoute(),
                P0RunSession.CreateDefaultStarterCatIds());

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "埃及梦境登记。");

            Assert.IsTrue(Contains(surface.SummaryRows, "梦境主题：埃及梦境 / 月砂遗迹 / 守护 月砂祭坛 / 占位"));
            Assert.IsTrue(Contains(surface.SummaryRows, egypt.DisplayName));
            Assert.IsTrue(Contains(surface.SummaryRows, egypt.ThemeLabel));
            Assert.IsTrue(Contains(surface.SummaryRows, egypt.DefenseTargetLabel));
            Assert.IsTrue(egypt.IsPlaceholder);
            Assert.AreEqual(P0DreamMapCatalog.EgyptDreamMapId, run.DreamMap.Id);
            Assert.IsTrue(P0RouteMapPresenter.HasP0RouteMapSurface(surface));
        }

        [Test]
        public void BuildSurface_OldDreamMapRevealsOnlyNextFutureLayer()
        {
            RunProgressionState run = CreateRun();
            run.EventItems.Add(RunEventItemInventory.OldDreamMapId);

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "预览路线。");

            Assert.AreEqual("旧梦地图预览", surface.LayerRows[1].StateToken);
            StringAssert.Contains("柔雨窗台", surface.LayerRows[1].OptionPreview);
            StringAssert.Contains("午夜猫粮商店", surface.LayerRows[1].OptionPreview);
            Assert.AreEqual("未来", surface.LayerRows[2].StateToken);
            StringAssert.Contains("未知分支 x2", surface.LayerRows[2].OptionPreview);
            StringAssert.DoesNotContain("冷光影子", surface.LayerRows[2].OptionPreview);
            Assert.IsTrue(Contains(surface.SummaryRows, "事件道具：旧梦地图 x1"));
            Assert.IsTrue(Contains(surface.SummaryRows, "旧梦地图：第 2 层预览 柔雨窗台 / 午夜猫粮商店"));
        }

        [Test]
        public void BuildSurface_PendingCatUpgradeShowsChoicesAndPawStampReroll()
        {
            RunProgressionState run = CreateRun();
            run.EventItems.Add(RunEventItemInventory.PawStampId);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "猫咪升级。");

            Assert.AreEqual("梦境路线", surface.Title);
            Assert.GreaterOrEqual(surface.CatUpgradeChoices.Count, 3);
            Assert.AreEqual("1", surface.CatUpgradeChoices[0].SlotLabel);
            AssertCatUpgradeChoicesArePlayerFacing(surface);
            Assert.IsTrue(surface.CanRerollCatUpgrade);
            Assert.IsTrue(Contains(surface.SummaryRows, "猫咪经验：0/3，待选择猫咪升级"));
            Assert.IsTrue(Contains(surface.SummaryRows, "猫爪印章：可消耗 1 枚，刷新这批猫咪升级候选"));
            AssertPendingCatUpgradeVisibleTextHasNoDeveloperTokens(surface);
            Assert.IsTrue(surface.TryGetAction(P0RouteMapActionIds.EnterCurrentNode, out P0RouteMapAction enter));
            Assert.IsFalse(enter.IsEnabled);
        }

        [Test]
        public void BuildSurface_BranchAndRewardRowsArePlayerFacing()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.Route.SelectCurrentNode("layer_02_dream_event");

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "请选择奖励。");

            Assert.AreEqual(2, surface.RouteOptions.Count);
            Assert.AreEqual("1", surface.RouteOptions[0].SlotLabel);
            Assert.IsTrue(surface.RouteOptions[0].BuildButtonLabel().Contains("柔雨窗台"));
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventRouteNodeIconId, surface.RouteOptions[0].VisualAsset.AssetId);
            Assert.GreaterOrEqual(surface.RewardChoices.Count, 3);
            Assert.AreEqual("1", surface.RewardChoices[0].SlotLabel);
            StringAssert.DoesNotContain("_", surface.RewardChoices[0].Summary);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, surface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventRouteCardFrameId, surface.RewardChoices[0].FrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardGainBadgeId, surface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventClearNotificationsCardId, surface.RewardChoices[0].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventModifierChoiceIconId, surface.RewardChoices[1].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardRiskBadgeId, surface.RewardChoices[1].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventCatnipResidueCardId, surface.RewardChoices[1].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventMarkAllReadCardId, surface.RewardChoices[2].ItemCardAsset.AssetId);
        }

        [Test]
        public void BuildSurface_UnreadRedDotDreamEventUsesContentSpecificRewardCards()
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, "layer_05_dream_event");

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择梦境事件。");

            Assert.GreaterOrEqual(surface.RewardChoices.Count, 3);
            Assert.AreEqual("dream_event_red_dot_cleanup", surface.RewardChoices[0].ChoiceId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, surface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventRouteCardFrameId, surface.RewardChoices[0].FrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardGainBadgeId, surface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventClearNotificationsCardId, surface.RewardChoices[0].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventCatnipResidueCardId, surface.RewardChoices[1].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventMarkAllReadCardId, surface.RewardChoices[2].ItemCardAsset.AssetId);
            StringAssert.DoesNotContain("_", surface.RewardChoices[0].Summary);
        }

        [Test]
        public void BuildSurface_RestNestNodeUsesRecoveryChoiceCard()
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, "layer_07_rest_nest");

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择休整。");

            Assert.GreaterOrEqual(surface.RewardChoices.Count, 1);
            Assert.AreEqual("rest_nest_recovery", surface.RewardChoices[0].ChoiceId);
            Assert.AreEqual(P0VisualAssetCatalog.RestSupplyChoiceIconId, surface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RestNestRouteCardFrameId, surface.RewardChoices[0].FrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardRecoveryBadgeId, surface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RestNestRecoveryCardId, surface.RewardChoices[0].ItemCardAsset.AssetId);
            StringAssert.Contains("猫生命", surface.RewardChoices[0].Summary);
            StringAssert.DoesNotContain("HP", surface.RewardChoices[0].Summary);
        }

        [Test]
        public void BuildSurface_PartnerNodeUsesSpecificChoiceCards()
        {
            RunProgressionState recruitRun = CreateRun();
            MoveToNode(recruitRun, "layer_03_partner");

            P0RouteMapSurface recruitSurface = P0RouteMapPresenter.BuildSurface(recruitRun, "选择伙伴。");

            Assert.GreaterOrEqual(recruitSurface.RewardChoices.Count, 1);
            Assert.AreEqual("partner_shadowmaru_preview", recruitSurface.RewardChoices[0].ChoiceId);
            Assert.AreEqual(P0VisualAssetCatalog.PartnerRecruitChoiceIconId, recruitSurface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PartnerRouteCardFrameId, recruitSurface.RewardChoices[0].FrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardGainBadgeId, recruitSurface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PartnerShadowmaruPreviewCardId, recruitSurface.RewardChoices[0].ItemCardAsset.AssetId);

            RunProgressionState duplicateRun = CreateRun();
            duplicateRun.Roster.AddCat(P0RouteRewardResolver.PreviewPartnerId);
            MoveToNode(duplicateRun, "layer_03_partner");

            P0RouteMapSurface duplicateSurface = P0RouteMapPresenter.BuildSurface(duplicateRun, "选择伙伴。");

            Assert.GreaterOrEqual(duplicateSurface.RewardChoices.Count, 1);
            Assert.AreEqual("partner_preview_duplicate_supply", duplicateSurface.RewardChoices[0].ChoiceId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, duplicateSurface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PartnerRouteCardFrameId, duplicateSurface.RewardChoices[0].FrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardGainBadgeId, duplicateSurface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PartnerDuplicateSupplyCardId, duplicateSurface.RewardChoices[0].ItemCardAsset.AssetId);
        }

        [Test]
        public void BuildSurface_NonBattleNodesResolveRewardCardFrames()
        {
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventRouteCardFrameId, RewardFrameForNode("layer_02_dream_event"));
            Assert.AreEqual(P0VisualAssetCatalog.ShopRouteCardFrameId, RewardFrameForNode("layer_02_shop_early"));
            Assert.AreEqual(P0VisualAssetCatalog.PartnerRouteCardFrameId, RewardFrameForNode("layer_03_partner"));
            Assert.AreEqual(P0VisualAssetCatalog.BlessingRouteCardFrameId, RewardFrameForNode("layer_07_blessing"));
            Assert.AreEqual(P0VisualAssetCatalog.RestNestRouteCardFrameId, RewardFrameForNode("layer_07_rest_nest"));
        }

        [Test]
        public void BuildSurface_NonBattleNodesResolveSummaryBanners()
        {
            Assert.AreEqual(P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId, SummaryBannerForNode("layer_02_shop_early"));
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId, SummaryBannerForNode("layer_02_dream_event"));
            Assert.AreEqual(P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId, SummaryBannerForNode("layer_07_rest_nest"));
            Assert.IsFalse(P0RouteMapPresenter.BuildSurface(CreateRun(), "就绪").CurrentNode.SummaryBannerAsset.HasAsset);
        }

        [Test]
        public void BuildSurface_BlessingNodeUsesSpecificAuthoritySealAssets()
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, "layer_07_blessing");

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择祝福。");

            Assert.GreaterOrEqual(surface.RewardChoices.Count, 3);
            Assert.AreEqual("誓约床线", surface.RewardChoices[0].DisplayName);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityOathBedlineSealId, surface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityDominionSandglassSealId, surface.RewardChoices[1].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityRhythmLullabySealId, surface.RewardChoices[2].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.BlessingRouteCardFrameId, surface.RewardChoices[0].FrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardGainBadgeId, surface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityOathBedlineCardId, surface.RewardChoices[0].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityDominionSandglassCardId, surface.RewardChoices[1].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityRhythmLullabyCardId, surface.RewardChoices[2].ItemCardAsset.AssetId);
        }

        [Test]
        public void BuildSurface_BlessingUpgradeChoicesReuseAuthorityChoiceCards()
        {
            RunProgressionState run = CreateRun();
            var blessings = P0BlessingCatalog.CreateAuthorityBlessings();
            run.Blessings.Add(blessings[0]);
            run.Blessings.Add(blessings[1]);
            run.Blessings.Add(blessings[2]);
            MoveToNode(run, "layer_07_blessing");

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择祝福升级。");

            Assert.GreaterOrEqual(surface.RewardChoices.Count, 3);
            Assert.AreEqual("blessing_upgrade_authority_oath_bedline", surface.RewardChoices[0].ChoiceId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityOathBedlineSealId, surface.RewardChoices[0].VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RouteRewardUpgradeBadgeId, surface.RewardChoices[0].DetailBadgeAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityOathBedlineCardId, surface.RewardChoices[0].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityDominionSandglassCardId, surface.RewardChoices[1].ItemCardAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityRhythmLullabyCardId, surface.RewardChoices[2].ItemCardAsset.AssetId);
        }

        [Test]
        public void BuildSurface_FailedRunShowsFailedSettlementRows()
        {
            RunProgressionState run = CreateFailedRun();

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "路线失败。");

            Assert.IsTrue(surface.IsRouteComplete);
            Assert.IsFalse(surface.IsRouteCleared);
            Assert.AreEqual("失败", surface.StatusLabel);
            Assert.AreEqual("进度：1/10", surface.ProgressLabel);
            Assert.IsTrue(Contains(surface.SettlementRows, "结算：路线失败"));
            Assert.IsTrue(Contains(surface.SettlementRows, "路线：1/10 节点"));
            Assert.IsTrue(Contains(surface.SettlementRows, "战斗：0胜 / 1负"));
            Assert.IsTrue(Contains(surface.SettlementRows, "最终核心："));
            Assert.IsTrue(surface.TryGetAction(P0RouteMapActionIds.NewRun, out P0RouteMapAction restart));
            Assert.IsTrue(restart.IsEnabled);
        }

        [Test]
        public void BuildSurface_BossNodeExposesRouteIconAsset()
        {
            RunProgressionState run = CreateRun();
            AdvanceToBoss(run);

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "首领将至。");

            Assert.AreEqual(P0RouteCatalog.BossNodeId, surface.CurrentNode.NodeId);
            Assert.AreEqual(P0VisualAssetCatalog.BossRouteNodeIconId, surface.CurrentNode.VisualAsset.AssetId);
            Assert.IsTrue(surface.CurrentNode.VisualAsset.RequiresWorkspaceFile);
            Assert.IsTrue(Contains(surface.CurrentNode.VisualAsset.SourceLockIds, "call_tyrant_concept"));
        }

        [Test]
        public void EvaluatePrototypeRouteMap_CompletesRouteMapSurfaceGate()
        {
            P0RouteMapSurfaceCoverageReport report = P0RouteMapSurfaceCoverage.EvaluatePrototypeRouteMap();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0RouteMapSurfaceCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("route map surface coverage complete", report.BuildSummary());
            StringAssert.Contains("Egypt placeholder surface", report.BuildDetailedSummary());
            StringAssert.Contains("settlement surface", report.BuildDetailedSummary());
            StringAssert.Contains("UI shell", report.BuildDetailedSummary());
            StringAssert.Contains("route choice icons", report.BuildDetailedSummary());
            StringAssert.Contains("reward card frames", report.BuildDetailedSummary());
            StringAssert.Contains("detail badges", report.BuildDetailedSummary());
            StringAssert.Contains("authority blessing seal", report.BuildDetailedSummary());
            StringAssert.Contains("summary banners", report.BuildDetailedSummary());
            StringAssert.Contains("dream-event surface", report.BuildDetailedSummary());
            StringAssert.Contains("RestNest surface", report.BuildDetailedSummary());
            StringAssert.Contains("partner surface", report.BuildDetailedSummary());
            StringAssert.Contains("blessing surface", report.BuildDetailedSummary());
            StringAssert.Contains("outcome banner assets", report.BuildDetailedSummary());
            StringAssert.Contains("old dream map", report.BuildDetailedSummary());
            StringAssert.Contains("cat upgrade", report.BuildDetailedSummary());
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
        }

        private static RunProgressionState CreateFailedRun()
        {
            RunProgressionState run = CreateRun();
            run.CoreValues.Capture(0f, 100f, 100f, 45f, 18f);
            run.CatVitals.Capture("saiban", 220f, 70f, 0f);
            run.CatVitals.Capture("nephthys", 160f, 30f, 0f);
            run.CatVitals.Capture("suzune", 120f, 90f, 0f);

            NodeMetrics metrics = run.Metrics.BeginNode(
                run.Route.CurrentLayer,
                run.Route.CurrentNode.Id,
                run.CoreValues.OwnerSleepMax);
            metrics.RecordBedPressure(24f, 4f);
            metrics.RecordCatPressure(18f, 6f);
            metrics.RecordAutoTargetAcquired();
            metrics.RecordSkillTargetAcquired();
            metrics.RecordSkillCastSuccess();
            metrics.RecordInteractionBlockedByRange();
            metrics.Complete(NodeResult.Failure, 18f, 0f);
            run.Route.CompleteCurrentNode(NodeResult.Failure);
            return run;
        }

        private static void AdvanceToBoss(RunProgressionState run)
        {
            int safety = 0;
            while (!run.Route.IsComplete
                && run.Route.CurrentNode.NodeType != RouteNodeType.Boss
                && safety < 20)
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

                safety++;
            }
        }

        private static string RewardFrameForNode(string nodeId)
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, nodeId);
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "请选择奖励。");
            return surface.RewardChoices.Count == 0
                ? string.Empty
                : surface.RewardChoices[0].FrameAsset.AssetId;
        }

        private static string SummaryBannerForNode(string nodeId)
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, nodeId);
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "当前节点。");
            return surface.CurrentNode.SummaryBannerAsset.AssetId;
        }

        private static void MoveToNode(RunProgressionState run, string nodeId)
        {
            int safety = 0;
            while (!run.Route.IsComplete && safety < 20)
            {
                for (int i = 0; i < run.Route.CurrentLayerOptions.Count; i++)
                {
                    if (run.Route.CurrentLayerOptions[i].Id == nodeId)
                    {
                        run.Route.SelectCurrentNode(nodeId);
                        return;
                    }
                }

                RouteNodeDefinition node = run.Route.CurrentNode;
                if (node == null)
                {
                    return;
                }

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
        }

        private static bool Contains(System.Collections.Generic.IReadOnlyList<string> rows, string expected)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Contains(expected))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AssertCatUpgradeChoicesArePlayerFacing(P0RouteMapSurface surface)
        {
            for (int i = 0; i < surface.CatUpgradeChoices.Count; i++)
            {
                P0RouteMapCatUpgradeChoiceCard choice = surface.CatUpgradeChoices[i];
                Assert.IsNotEmpty(choice.CatLabel, "cat label " + i);
                Assert.IsNotEmpty(choice.StageLabel, "stage label " + i);
                Assert.IsNotEmpty(choice.DisplayName, "display name " + i);
                Assert.IsNotEmpty(choice.IntentLabel, "intent label " + i);
                Assert.IsNotEmpty(choice.Summary, "summary " + i);

                string visibleText = choice.BuildButtonLabel() + " " + choice.Summary;
                StringAssert.Contains(choice.IntentLabel, visibleText, "intent appears " + i);
                StringAssert.DoesNotContain("_", visibleText, "internal underscore " + i);
                StringAssert.DoesNotContain(" / ", visibleText, "debug slash separator " + i);
                StringAssert.DoesNotContain(" | ", visibleText, "debug pipe separator " + i);
                StringAssert.DoesNotContain(" - ", visibleText, "debug dash separator " + i);
                StringAssert.DoesNotContain("cat_upgrade", visibleText, "internal upgrade id " + i);
                StringAssert.DoesNotContain("saiban", visibleText, "internal saiban id " + i);
                StringAssert.DoesNotContain("nephthys", visibleText, "internal nephthys id " + i);
                StringAssert.DoesNotContain("suzune", visibleText, "internal suzune id " + i);
                StringAssert.DoesNotContain("placeholder", visibleText, "placeholder token " + i);
            }
        }

        private static void AssertPendingCatUpgradeVisibleTextHasNoDeveloperTokens(P0RouteMapSurface surface)
        {
            string text = surface.Title
                + " " + surface.ProgressLabel
                + " " + surface.CurrentNode.Detail;
            for (int i = 0; i < surface.SummaryRows.Count; i++)
            {
                text += " " + surface.SummaryRows[i];
            }

            for (int i = 0; i < surface.CatUpgradeChoices.Count; i++)
            {
                text += " " + surface.CatUpgradeChoices[i].BuildButtonLabel();
                text += " " + surface.CatUpgradeChoices[i].Summary;
            }

            StringAssert.DoesNotContain("P0", text);
            StringAssert.DoesNotContain("HP", text);
            StringAssert.DoesNotContain("cat_upgrade", text);
            StringAssert.DoesNotContain("saiban", text);
            StringAssert.DoesNotContain("nephthys", text);
            StringAssert.DoesNotContain("suzune", text);
            StringAssert.DoesNotContain("placeholder", text);
        }
    }
}
