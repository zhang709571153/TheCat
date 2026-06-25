using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0RouteMapSurfaceCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0RouteMapSurfaceCoverageIssue
    {
        public P0RouteMapSurfaceCoverageIssue(P0RouteMapSurfaceCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0RouteMapSurfaceCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0RouteMapSurfaceCoverageReport
    {
        private readonly List<P0RouteMapSurfaceCoverageIssue> issues = new List<P0RouteMapSurfaceCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0RouteMapSurfaceCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0RouteMapSurfaceCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0RouteMapSurfaceCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0RouteMapSurfaceCoverageSeverity severity, string message)
        {
            issues.Add(new P0RouteMapSurfaceCoverageIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 route map surface coverage complete for " + coveredChecks.Count + " surface check(s)."
                : "P0 route map surface coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " surface check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0RouteMapSurfaceCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0RouteMapSurfaceCoverage
    {
        public const int ExpectedCoveredCheckCount = 21;

        public static P0RouteMapSurfaceCoverageReport EvaluatePrototypeRouteMap()
        {
            P0RouteMapSurfaceCoverageReport report = new P0RouteMapSurfaceCoverageReport();

            EvaluateLayerOneSurface(report);
            EvaluateEgyptPlaceholderSurface(report);
            EvaluateBranchSurface(report);
            EvaluateRewardChoiceSurface(report);
            EvaluateOldDreamMapPreviewSurface(report);
            EvaluateCatUpgradeSurface(report);
            EvaluateRewardCardFrameSurface(report);
            EvaluateNonBattleSummaryBannerSurface(report);
            EvaluateShopItemCardSurface(report);
            EvaluateDreamEventChoiceCardSurface(report);
            EvaluateRestNestRecoveryCardSurface(report);
            EvaluatePartnerChoiceCardSurface(report);
            EvaluateAuthorityBlessingSealSurface(report);
            EvaluateAuthorityBlessingChoiceCardSurface(report);
            EvaluateSettlementSurface(report);
            EvaluateFailedSettlementSurface(report);
            EvaluateSettlementOutcomeBannerSurface(report);
            EvaluateActionSurface(report);
            EvaluateBossVisualAssetSurface(report);
            EvaluateUiShellSurface(report);
            EvaluateCompactSummary(report);

            return report;
        }

        private static void EvaluateLayerOneSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "就绪");

            Require(
                report,
                P0RouteMapPresenter.HasP0RouteMapSurface(surface)
                && surface.ProgressLabel == "进度：0/10"
                && surface.CurrentNode.NodeId == P0RouteCatalog.LayerOneDefenseId
                && surface.CurrentNode.RequiresBattle
                && surface.SummaryRows.Count >= 6,
                "Route map layer-one surface exposes progress, battle current node, route rows, and run summary rows.",
                "Route map layer-one surface is incomplete.");
        }

        private static void EvaluateEgyptPlaceholderSurface(P0RouteMapSurfaceCoverageReport report)
        {
            DreamMapDefinition egypt = P0DreamMapCatalog.GetEgyptDreamMap();
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateEgyptPlaceholderRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "埃及梦境占位验证。");

            Require(
                report,
                egypt.IsPlaceholder
                && P0RouteMapPresenter.HasP0RouteMapSurface(surface)
                && Contains(surface.SummaryRows, egypt.DisplayName)
                && Contains(surface.SummaryRows, egypt.ThemeLabel)
                && Contains(surface.SummaryRows, egypt.DefenseTargetLabel)
                && surface.TryGetAction(P0RouteMapActionIds.EnterCurrentNode, out P0RouteMapAction enter)
                && enter.IsEnabled
                && enter.TargetSceneName == P0SceneFlow.GrayboxBattleSceneName,
                "Route map Egypt placeholder surface exposes map theme, defense target, and current shared battle entry.",
                "Route map Egypt placeholder surface did not expose map context or shared battle entry.");
        }

        private static void EvaluateBranchSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择路线。");

            Require(
                report,
                surface.RouteOptions.Count == 2
                && surface.RouteOptions[0].IsSelected
                && !surface.RouteOptions[1].IsSelected
                && surface.LayerRows[1].HasBranch
                && surface.LayerRows[1].IsCurrent,
                "Route map branch surface exposes current-layer option buttons and selected route marker.",
                "Route map branch surface did not expose branch choices correctly.");
        }

        private static void EvaluateRewardChoiceSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            run.Route.SelectCurrentNode("layer_02_dream_event");
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "请选择奖励。");

            Require(
                report,
                surface.RewardChoices.Count >= 3
                && surface.RewardChoices[0].SlotLabel == "1"
                && !string.IsNullOrWhiteSpace(surface.RewardChoices[0].Summary)
                && !surface.RewardChoices[0].Summary.Contains("_")
                && surface.RewardChoices[0].VisualAsset.AssetId == P0VisualAssetCatalog.FishTreatRewardIconId
                && surface.RewardChoices[0].FrameAsset.AssetId == P0VisualAssetCatalog.DreamEventRouteCardFrameId
                && surface.RewardChoices[0].DetailBadgeAsset.AssetId == P0VisualAssetCatalog.RouteRewardGainBadgeId
                && surface.RewardChoices[1].VisualAsset.AssetId == P0VisualAssetCatalog.DreamEventModifierChoiceIconId
                && surface.RewardChoices[1].DetailBadgeAsset.AssetId == P0VisualAssetCatalog.RouteRewardRiskBadgeId,
                "Route map reward surface exposes player-facing numbered reward choices with route choice icons, card frames, and detail badges.",
                "Route map reward surface did not expose player-facing reward choices, route choice icons, card frames, or detail badges.");
        }

        private static void EvaluateOldDreamMapPreviewSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState hiddenRun = CreateRun();
            P0RouteMapSurface hiddenSurface = P0RouteMapPresenter.BuildSurface(hiddenRun, "就绪");

            RunProgressionState previewRun = CreateRun();
            previewRun.EventItems.Add(RunEventItemInventory.OldDreamMapId);
            P0RouteMapSurface previewSurface = P0RouteMapPresenter.BuildSurface(previewRun, "旧梦地图预览。");

            Require(
                report,
                hiddenSurface.LayerRows[1].StateToken == "未来"
                && hiddenSurface.LayerRows[1].OptionPreview.Contains("未知分支 x2")
                && !hiddenSurface.LayerRows[1].OptionPreview.Contains("柔雨窗台")
                && previewSurface.LayerRows[1].StateToken == "旧梦地图预览"
                && previewSurface.LayerRows[1].OptionPreview.Contains("柔雨窗台")
                && previewSurface.LayerRows[1].OptionPreview.Contains("午夜猫粮商店")
                && previewSurface.LayerRows[2].StateToken == "未来"
                && previewSurface.LayerRows[2].OptionPreview.Contains("未知分支 x2")
                && Contains(previewSurface.SummaryRows, "旧梦地图：第 2 层预览 柔雨窗台 / 午夜猫粮商店"),
                "Route map old dream map surface hides future route details by default and reveals only the next layer while held.",
                "Route map old dream map surface leaked future routes or failed to reveal the next-layer preview.");
        }

        private static void EvaluateCatUpgradeSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            run.EventItems.Add(RunEventItemInventory.PawStampId);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "猫咪升级。");

            Require(
                report,
                surface.Title == "梦境路线"
                && surface.CatUpgradeChoices.Count >= 3
                && surface.CatUpgradeChoices[0].SlotLabel == "1"
                && CatUpgradeChoicesArePlayerFacing(surface)
                && surface.CanRerollCatUpgrade
                && Contains(surface.SummaryRows, "猫咪经验：0/3，待选择猫咪升级")
                && Contains(surface.SummaryRows, "猫爪印章：可消耗 1 枚，刷新这批猫咪升级候选")
                && PendingCatUpgradeVisibleTextHasNoDeveloperTokens(surface)
                && surface.TryGetAction(P0RouteMapActionIds.EnterCurrentNode, out P0RouteMapAction enter)
                && !enter.IsEnabled,
                "Route map cat upgrade surface exposes joined-cat upgrade choices, pending-state gating, and paw-stamp reroll affordance.",
                "Route map cat upgrade surface did not expose choices, reroll affordance, or pending-state gating.");
        }

        private static void EvaluateRewardCardFrameSurface(P0RouteMapSurfaceCoverageReport report)
        {
            Require(
                report,
                RewardFrameForNode("layer_02_dream_event") == P0VisualAssetCatalog.DreamEventRouteCardFrameId
                && RewardFrameForNode("layer_02_shop_early") == P0VisualAssetCatalog.ShopRouteCardFrameId
                && RewardFrameForNode("layer_03_partner") == P0VisualAssetCatalog.PartnerRouteCardFrameId
                && RewardFrameForNode("layer_07_blessing") == P0VisualAssetCatalog.BlessingRouteCardFrameId
                && RewardFrameForNode("layer_07_rest_nest") == P0VisualAssetCatalog.RestNestRouteCardFrameId,
                "Route map reward card frames cover partner, shop, blessing, dream-event, and rest-nest nodes.",
                "Route map reward card frames are missing for one or more non-battle route nodes.");
        }

        private static void EvaluateNonBattleSummaryBannerSurface(P0RouteMapSurfaceCoverageReport report)
        {
            Require(
                report,
                SummaryBannerForNode("layer_02_shop_early") == P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId
                && SummaryBannerForNode("layer_02_dream_event") == P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId
                && SummaryBannerForNode("layer_07_rest_nest") == P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId,
                "Route map current-node surface exposes non-battle node summary banners for shop, dream event, and rest nest nodes.",
                "Route map current-node surface is missing one or more non-battle node summary banners.");
        }

        private static void EvaluateShopItemCardSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            run.Wallet.AddFishTreats(10);
            MoveToNode(run, "layer_02_shop_early");
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择商店物品。");

            Require(
                report,
                ChoiceHasItemCard(surface, "shop_bed_patch", P0VisualAssetCatalog.ShopBedPatchCardId)
                && ChoiceHasItemCard(surface, "shop_litter_sachet", P0VisualAssetCatalog.ShopLitterSachetCardId)
                && ChoiceHasItemCard(surface, "shop_late_kibble", P0VisualAssetCatalog.ShopLateKibbleCardId)
                && ChoiceHasItemCard(surface, "shop_free_sample", P0VisualAssetCatalog.ShopFreeSampleCardId),
                "Route map shop surface exposes specific item-card assets for bed patch, litter sachet, late kibble, and free sample choices.",
                "Route map shop surface is missing one or more specific shop item-card assets.");
        }

        private static void EvaluateDreamEventChoiceCardSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, "layer_02_dream_event");
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择梦境事件。");

            Require(
                report,
                ChoiceHasItemCard(surface, "dream_event_clear_notifications", P0VisualAssetCatalog.DreamEventClearNotificationsCardId)
                && ChoiceHasItemCard(surface, "dream_event_catnip_residue", P0VisualAssetCatalog.DreamEventCatnipResidueCardId)
                && ChoiceHasItemCard(surface, "dream_event_mark_all_read", P0VisualAssetCatalog.DreamEventMarkAllReadCardId),
                "Route map dream-event surface exposes specific choice-card assets for clear notifications, catnip residue, and mark all read choices.",
                "Route map dream-event surface is missing one or more specific dream-event choice-card assets.");
        }

        private static void EvaluateRestNestRecoveryCardSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, "layer_07_rest_nest");
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择休整。");

            Require(
                report,
                ChoiceHasItemCard(surface, "rest_nest_recovery", P0VisualAssetCatalog.RestNestRecoveryCardId),
                "Route map RestNest surface exposes the specific recovery choice-card asset for rest_nest_recovery.",
                "Route map RestNest surface is missing the specific recovery choice-card asset.");
        }

        private static void EvaluatePartnerChoiceCardSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState recruitRun = CreateRun();
            MoveToNode(recruitRun, "layer_03_partner");
            P0RouteMapSurface recruitSurface = P0RouteMapPresenter.BuildSurface(recruitRun, "选择伙伴。");

            RunProgressionState duplicateRun = CreateRun();
            duplicateRun.Roster.AddCat(P0RouteRewardResolver.PreviewPartnerId);
            MoveToNode(duplicateRun, "layer_03_partner");
            P0RouteMapSurface duplicateSurface = P0RouteMapPresenter.BuildSurface(duplicateRun, "选择伙伴。");

            Require(
                report,
                ChoiceHasItemCard(recruitSurface, "partner_shadowmaru_preview", P0VisualAssetCatalog.PartnerShadowmaruPreviewCardId)
                && ChoiceHasItemCard(duplicateSurface, "partner_preview_duplicate_supply", P0VisualAssetCatalog.PartnerDuplicateSupplyCardId),
                "Route map partner surface exposes specific choice-card assets for Shadowmaru preview and duplicate-supply fallback states.",
                "Route map partner surface is missing one or more specific partner choice-card assets.");
        }

        private static void EvaluateAuthorityBlessingSealSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            MoveToNode(run, "layer_07_blessing");
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "选择祝福。");

            Require(
                report,
                surface.RewardChoices.Count >= 3
                && surface.RewardChoices[0].DisplayName == "誓约床线"
                && surface.RewardChoices[0].VisualAsset.AssetId == P0VisualAssetCatalog.AuthorityOathBedlineSealId
                && surface.RewardChoices[1].VisualAsset.AssetId == P0VisualAssetCatalog.AuthorityDominionSandglassSealId
                && surface.RewardChoices[2].VisualAsset.AssetId == P0VisualAssetCatalog.AuthorityRhythmLullabySealId
                && surface.RewardChoices[0].FrameAsset.AssetId == P0VisualAssetCatalog.BlessingRouteCardFrameId
                && surface.RewardChoices[0].DetailBadgeAsset.AssetId == P0VisualAssetCatalog.RouteRewardGainBadgeId,
                "Route map blessing reward choices consume the three specific P0 authority blessing seal assets.",
                "Route map blessing reward choices did not resolve specific authority blessing seal assets.");
        }

        private static void EvaluateAuthorityBlessingChoiceCardSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState gainRun = CreateRun();
            MoveToNode(gainRun, "layer_07_blessing");
            P0RouteMapSurface gainSurface = P0RouteMapPresenter.BuildSurface(gainRun, "选择祝福。");

            RunProgressionState upgradeRun = CreateRun();
            IReadOnlyList<AuthorityBlessingDefinition> blessings = P0BlessingCatalog.CreateAuthorityBlessings();
            upgradeRun.Blessings.Add(blessings[0]);
            upgradeRun.Blessings.Add(blessings[1]);
            upgradeRun.Blessings.Add(blessings[2]);
            MoveToNode(upgradeRun, "layer_07_blessing");
            P0RouteMapSurface upgradeSurface = P0RouteMapPresenter.BuildSurface(upgradeRun, "选择祝福升级。");

            Require(
                report,
                ChoiceHasItemCard(gainSurface, "blessing_authority_oath_bedline", P0VisualAssetCatalog.AuthorityOathBedlineCardId)
                && ChoiceHasItemCard(gainSurface, "blessing_authority_dominion_sandglass", P0VisualAssetCatalog.AuthorityDominionSandglassCardId)
                && ChoiceHasItemCard(gainSurface, "blessing_authority_rhythm_lullaby", P0VisualAssetCatalog.AuthorityRhythmLullabyCardId)
                && ChoiceHasItemCard(upgradeSurface, "blessing_upgrade_authority_oath_bedline", P0VisualAssetCatalog.AuthorityOathBedlineCardId)
                && ChoiceHasItemCard(upgradeSurface, "blessing_upgrade_authority_dominion_sandglass", P0VisualAssetCatalog.AuthorityDominionSandglassCardId)
                && ChoiceHasItemCard(upgradeSurface, "blessing_upgrade_authority_rhythm_lullaby", P0VisualAssetCatalog.AuthorityRhythmLullabyCardId),
                "Route map blessing surface exposes specific choice-card assets for authority blessing gains and upgrade choices.",
                "Route map blessing surface is missing one or more specific authority blessing choice-card assets.");
        }

        private static void EvaluateSettlementSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
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

            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "路线通关。");

            Require(
                report,
                surface.IsRouteComplete
                && surface.IsRouteCleared
                && surface.SettlementRows.Count > 0
                && Contains(surface.SettlementRows, "结算：路线通关")
                && Contains(surface.SettlementRows, "路线：10/10 节点"),
                "Route map settlement surface exposes cleared settlement rows after Boss completion.",
                "Route map settlement surface did not expose cleared settlement rows.");
        }

        private static void EvaluateFailedSettlementSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateFailedRun();
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "路线失败。");
            P0RunSettlementSummary summary = new P0RunSettlementSummary(run);

            Require(
                report,
                surface.IsRouteComplete
                && !surface.IsRouteCleared
                && surface.StatusLabel == "失败"
                && P0SettlementPresenter.HasP0FailedSettlementRows(summary)
                && Contains(surface.SettlementRows, "结算：路线失败")
                && Contains(surface.SettlementRows, "路线：1/10 节点")
                && Contains(surface.SettlementRows, "战斗：0胜 / 1负"),
                "Route map failed-settlement surface exposes failed run rows after owner sleep collapse.",
                "Route map failed-settlement surface did not expose failed run rows.");
        }

        private static void EvaluateSettlementOutcomeBannerSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState clearedRun = CreateRun();
            while (!clearedRun.Route.IsComplete)
            {
                RouteNodeDefinition node = clearedRun.Route.CurrentNode;
                if (RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    P0RouteRewardResolver.ApplyBattleReward(node, clearedRun);
                    clearedRun.Route.CompleteCurrentNode(NodeResult.Success);
                }
                else
                {
                    RouteNodeResolver.ResolveCurrentNode(clearedRun);
                }
            }

            P0RouteMapSurface clearedSurface = P0RouteMapPresenter.BuildSurface(clearedRun, "路线通关。");
            P0RouteMapSurface failedSurface = P0RouteMapPresenter.BuildSurface(CreateFailedRun(), "路线失败。");

            Require(
                report,
                clearedSurface.SettlementOutcomeBannerAsset.AssetId == P0VisualAssetCatalog.SettlementRunClearedBannerId
                && failedSurface.SettlementOutcomeBannerAsset.AssetId == P0VisualAssetCatalog.SettlementRunFailedBannerId,
                "Route map settlement surface exposes specific cleared and failed outcome banner assets.",
                "Route map settlement surface is missing one or more outcome banner assets.");
        }

        private static void EvaluateActionSurface(P0RouteMapSurfaceCoverageReport report)
        {
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(CreateRun(), "就绪");

            Require(
                report,
                surface.TryGetAction(P0RouteMapActionIds.EnterCurrentNode, out P0RouteMapAction enter)
                && enter.IsEnabled
                && enter.TargetSceneName == P0SceneFlow.GrayboxBattleSceneName
                && surface.TryGetAction(P0RouteMapActionIds.NewRun, out P0RouteMapAction restart)
                && restart.IsEnabled
                && restart.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && surface.TryGetAction(P0RouteMapActionIds.MainMenu, out P0RouteMapAction mainMenu)
                && mainMenu.IsEnabled
                && mainMenu.TargetSceneName == P0SceneFlow.MainMenuSceneName,
                "Route map action surface exposes enter, new-run, and main-menu actions with target scenes.",
                "Route map action surface is missing required actions or target scenes.");
        }

        private static void EvaluateBossVisualAssetSurface(P0RouteMapSurfaceCoverageReport report)
        {
            RunProgressionState run = CreateRun();
            AdvanceToBoss(run);
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(run, "首领将至。");

            Require(
                report,
                surface.CurrentNode.NodeId == P0RouteCatalog.BossNodeId
                && surface.CurrentNode.VisualAsset.AssetId == P0VisualAssetCatalog.BossRouteNodeIconId
                && surface.CurrentNode.VisualAsset.RequiresWorkspaceFile
                && Contains(surface.CurrentNode.VisualAsset.SourceLockIds, "call_tyrant_concept"),
                "Route map Boss surface exposes the generated Call Tyrant route-node icon asset.",
                "Route map Boss surface is missing the Call Tyrant route-node icon asset.");
        }

        private static void EvaluateUiShellSurface(P0RouteMapSurfaceCoverageReport report)
        {
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(CreateRun(), "就绪");

            Require(
                report,
                P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell)
                && surface.UiShell.DreamGlassPanel.AssetId == P0VisualAssetCatalog.DreamGlassPanelId
                && surface.UiShell.PrimaryButton.AssetId == P0VisualAssetCatalog.PrimaryButtonId
                && surface.UiShell.FishTreatRewardIcon.AssetId == P0VisualAssetCatalog.FishTreatRewardIconId
                && surface.UiShell.DreamShardRewardIcon.AssetId == P0VisualAssetCatalog.DreamShardRewardIconId,
                "Route map UI shell consumes the Batch 08 panel, button, and settlement reward icon shell assets.",
                "Route map UI shell assets are missing from the surface.");
        }

        private static void EvaluateCompactSummary(P0RouteMapSurfaceCoverageReport report)
        {
            P0RouteMapSurface surface = P0RouteMapPresenter.BuildSurface(CreateRun(), "就绪");
            string summary = P0RouteMapPresenter.BuildCompactSummary(surface);

            Require(
                report,
                summary.Contains("层数 10")
                && summary.Contains("进度 进度：0/10")
                && summary.Contains("分支")
                && summary.Contains("首领层 1")
                && summary.Contains("操作 3")
                && summary.Contains("uiShell 6"),
                "Route map compact summary reports layers, progress, branches, Boss row, and actions.",
                "Route map compact summary is missing required totals.");
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
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

        private static bool ChoiceHasItemCard(P0RouteMapSurface surface, string choiceId, string assetId)
        {
            if (surface == null)
            {
                return false;
            }

            for (int i = 0; i < surface.RewardChoices.Count; i++)
            {
                if (surface.RewardChoices[i].ChoiceId == choiceId
                    && surface.RewardChoices[i].ItemCardAsset.AssetId == assetId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CatUpgradeChoicesArePlayerFacing(P0RouteMapSurface surface)
        {
            if (surface == null || surface.CatUpgradeChoices.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < surface.CatUpgradeChoices.Count; i++)
            {
                P0RouteMapCatUpgradeChoiceCard choice = surface.CatUpgradeChoices[i];
                if (string.IsNullOrWhiteSpace(choice.CatLabel)
                    || string.IsNullOrWhiteSpace(choice.StageLabel)
                    || string.IsNullOrWhiteSpace(choice.DisplayName)
                    || string.IsNullOrWhiteSpace(choice.IntentLabel)
                    || string.IsNullOrWhiteSpace(choice.Summary))
                {
                    return false;
                }

                string visibleText = choice.BuildButtonLabel() + " " + choice.Summary;
                if (!visibleText.Contains(choice.IntentLabel)
                    || visibleText.Contains("_")
                    || visibleText.Contains(" / ")
                    || visibleText.Contains(" | ")
                    || visibleText.Contains(" - ")
                    || visibleText.Contains("cat_upgrade")
                    || visibleText.Contains("saiban")
                    || visibleText.Contains("nephthys")
                    || visibleText.Contains("suzune")
                    || visibleText.Contains("placeholder"))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool PendingCatUpgradeVisibleTextHasNoDeveloperTokens(P0RouteMapSurface surface)
        {
            if (surface == null)
            {
                return false;
            }

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

            return !text.Contains("P0")
                && !text.Contains("HP")
                && !text.Contains("cat_upgrade")
                && !text.Contains("saiban")
                && !text.Contains("nephthys")
                && !text.Contains("suzune")
                && !text.Contains("placeholder");
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

        private static bool Contains(IReadOnlyList<string> rows, string expected)
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

        private static void Require(
            P0RouteMapSurfaceCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0RouteMapSurfaceCoverageSeverity.Failure, failureMessage);
        }
    }
}
