using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public delegate WaveDefinition P0WaveResolver(string contentId);

    public enum P0PlayableReadinessState
    {
        Passed,
        Warning,
        Failed
    }

    public readonly struct P0PlayableReadinessCheck
    {
        public P0PlayableReadinessCheck(string checkId, string displayName, P0PlayableReadinessState state, string message)
        {
            CheckId = checkId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            State = state;
            Message = message ?? string.Empty;
        }

        public string CheckId { get; }

        public string DisplayName { get; }

        public P0PlayableReadinessState State { get; }

        public string Message { get; }

        public string BuildSummary()
        {
            return DisplayName + ": " + State + " - " + Message;
        }
    }

    public sealed class P0PlayableReadinessReport
    {
        private readonly List<P0PlayableReadinessCheck> checks = new List<P0PlayableReadinessCheck>();

        public IReadOnlyList<P0PlayableReadinessCheck> Checks => checks.AsReadOnly();

        public int FailureCount => Count(P0PlayableReadinessState.Failed);

        public int WarningCount => Count(P0PlayableReadinessState.Warning);

        public bool IsReady => FailureCount == 0;

        public void AddPassed(string checkId, string displayName, string message)
        {
            checks.Add(new P0PlayableReadinessCheck(checkId, displayName, P0PlayableReadinessState.Passed, message));
        }

        public void AddWarning(string checkId, string displayName, string message)
        {
            checks.Add(new P0PlayableReadinessCheck(checkId, displayName, P0PlayableReadinessState.Warning, message));
        }

        public void AddFailure(string checkId, string displayName, string message)
        {
            checks.Add(new P0PlayableReadinessCheck(checkId, displayName, P0PlayableReadinessState.Failed, message));
        }

        public bool TryGetCheck(string checkId, out P0PlayableReadinessCheck check)
        {
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].CheckId == checkId)
                {
                    check = checks[i];
                    return true;
                }
            }

            check = default(P0PlayableReadinessCheck);
            return false;
        }

        public string BuildSummary()
        {
            return IsReady
                ? "P0 playable readiness passed with " + WarningCount + " warning(s)."
                : "P0 playable readiness failed with " + FailureCount + " failure(s) and " + WarningCount + " warning(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < checks.Count; i++)
            {
                lines.Add("[" + checks[i].State + "] " + checks[i].BuildSummary());
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0PlayableReadinessState state)
        {
            int count = 0;
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].State == state)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public sealed class P0PlayableReadinessContext
    {
        public P0PlayableReadinessContext(
            RouteDefinition route,
            IReadOnlyList<CatDefinition> starterCats,
            IReadOnlyList<SkillDefinition> starterSkills,
            IReadOnlyList<EnemyDefinition> coreEnemies,
            IReadOnlyList<StatusTagDefinition> statusTags,
            P0WaveResolver waveResolver,
            P0GoldenPathAcceptanceReport goldenPathAcceptance = null,
            P0StatusTagCoverageReport statusTagCoverage = null)
        {
            Route = route;
            StarterCats = starterCats ?? Array.Empty<CatDefinition>();
            StarterSkills = starterSkills ?? Array.Empty<SkillDefinition>();
            CoreEnemies = coreEnemies ?? Array.Empty<EnemyDefinition>();
            StatusTags = statusTags ?? Array.Empty<StatusTagDefinition>();
            WaveResolver = waveResolver;
            GoldenPathAcceptance = goldenPathAcceptance;
            StatusTagCoverage = statusTagCoverage;
        }

        public RouteDefinition Route { get; }

        public IReadOnlyList<CatDefinition> StarterCats { get; }

        public IReadOnlyList<SkillDefinition> StarterSkills { get; }

        public IReadOnlyList<EnemyDefinition> CoreEnemies { get; }

        public IReadOnlyList<StatusTagDefinition> StatusTags { get; }

        public P0WaveResolver WaveResolver { get; }

        public P0GoldenPathAcceptanceReport GoldenPathAcceptance { get; }

        public P0StatusTagCoverageReport StatusTagCoverage { get; }
    }

    public static class P0PlayableReadiness
    {
        public const string SceneFlowCheckId = "scene_flow";
        public const string EntryCharacterSelectCheckId = "entry_character_select";
        public const string LoadingStartSurfaceCheckId = "loading_start_surface";
        public const string RuntimeSettingsAcceptanceCheckId = "runtime_settings_acceptance";
        public const string SkillSelectionAcceptanceCheckId = "skill_selection_acceptance";
        public const string BattleReadabilityAcceptanceCheckId = "battle_readability_acceptance";
        public const string StarterTrioCheckId = "starter_trio";
        public const string StarterSkillsCheckId = "starter_skills";
        public const string CoreEnemiesCheckId = "core_enemies";
        public const string RouteStructureCheckId = "route_structure";
        public const string DreamMapCheckId = "dream_maps";
        public const string EgyptReadinessCheckId = "egypt_readiness";
        public const string BattleWavesCheckId = "battle_waves";
        public const string StatusTagsCheckId = "status_tags";
        public const string GoldenPathCheckId = "golden_path";

        public static P0PlayableReadinessReport EvaluatePrototypeBuild()
        {
            P0GoldenPathReport goldenPath = P0GoldenPathSimulator.SimulateDefaultRun();
            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(goldenPath);
            return Evaluate(new P0PlayableReadinessContext(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0PrototypeCatalog.CreateStarterCats(),
                P0PrototypeCatalog.CreateStarterSkills(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0PrototypeCatalog.CreateStatusTags(),
                P0PrototypeCatalog.CreateWaveForContentId,
                acceptance));
        }

        public static P0PlayableReadinessReport Evaluate(P0PlayableReadinessContext context)
        {
            P0PlayableReadinessReport report = new P0PlayableReadinessReport();
            if (context == null)
            {
                report.AddFailure("context", "Readiness Context", "Readiness context is missing.");
                return report;
            }

            EvaluateSceneFlow(report);
            EvaluateEntryCharacterSelect(report);
            EvaluateLoadingStartSurface(report);
            EvaluateRuntimeSettingsAcceptance(report);
            EvaluateSkillSelectionAcceptance(report);
            EvaluateBattleReadabilityAcceptance(report);
            EvaluateStarterTrio(context, report);
            EvaluateStarterSkills(context, report);
            EvaluateCoreEnemies(context, report);
            EvaluateRouteStructure(context, report);
            EvaluateDreamMaps(context, report);
            EvaluateEgyptReadiness(context, report);
            EvaluateBattleWaves(context, report);
            EvaluateStatusTags(context, report);
            EvaluateGoldenPath(context, report);
            return report;
        }

        private static void EvaluateSceneFlow(P0PlayableReadinessReport report)
        {
            bool sceneNamesPresent = !string.IsNullOrWhiteSpace(P0SceneFlow.MainMenuSceneName)
                && !string.IsNullOrWhiteSpace(P0SceneFlow.RouteMapSceneName)
                && !string.IsNullOrWhiteSpace(P0SceneFlow.GrayboxBattleSceneName);
            bool sceneNamesDistinct = P0SceneFlow.MainMenuSceneName != P0SceneFlow.RouteMapSceneName
                && P0SceneFlow.MainMenuSceneName != P0SceneFlow.GrayboxBattleSceneName
                && P0SceneFlow.RouteMapSceneName != P0SceneFlow.GrayboxBattleSceneName;
            bool routeStart = P0SceneFlow.GetStartSceneName(P0RunStartMode.RouteMap) == P0SceneFlow.RouteMapSceneName;
            bool quickBattleStart = P0SceneFlow.GetStartSceneName(P0RunStartMode.QuickBattle) == P0SceneFlow.GrayboxBattleSceneName;
            bool postBattle = P0SceneFlow.GetPostBattleSceneName(BattleOutcome.Victory) == P0SceneFlow.RouteMapSceneName
                && P0SceneFlow.GetPostBattleSceneName(BattleOutcome.Defeat) == P0SceneFlow.RouteMapSceneName;

            if (sceneNamesPresent && sceneNamesDistinct && routeStart && quickBattleStart && postBattle)
            {
                report.AddPassed(
                    SceneFlowCheckId,
                    "Scene Flow",
                    "Main menu, route map, quick battle, and post-battle scene routing are aligned.");
                return;
            }

            report.AddFailure(
                SceneFlowCheckId,
                "Scene Flow",
                "P0 scene names or start/post-battle routing are not aligned.");
        }

        private static void EvaluateEntryCharacterSelect(P0PlayableReadinessReport report)
        {
            IReadOnlyList<string> defaultStarters = P0RunSession.CreateDefaultStarterCatIds();
            P0MainMenuSurface mainMenu = P0MainMenuPresenter.BuildSurface(
                P0PrototypeCatalog.CreateStarterCats(),
                defaultStarters,
                P0RouteCatalog.CreateTenLayerRoute(),
                "Ready");
            P0MainMenuSurface emptySelection = P0MainMenuPresenter.BuildSurface(
                P0PrototypeCatalog.CreateStarterCats(),
                Array.Empty<string>(),
                P0RouteCatalog.CreateTenLayerRoute(),
                "Ready");
            IReadOnlyList<string> selectedStarters = P0RunSession.NormalizeStarterCatIds(new[]
            {
                P0PrototypeCatalog.SaibanId,
                P0PrototypeCatalog.SuzuneId
            });
            RunProgressionState selectedRun = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(P0DreamMapCatalog.GetBedroomDreamMap()),
                selectedStarters);
            P0CatRoomSurface catRoom = P0CatRoomPresenter.BuildSurface(P0CatRoomState.CreateFreshStart());

            bool mainMenuContract = P0MainMenuPresenter.HasP0MainMenuSurface(mainMenu)
                && mainMenu.TryGetAction(P0MainMenuActionIds.EnterCatRoom, out P0MainMenuAction catRoomAction)
                && catRoomAction.IsEnabled
                && catRoomAction.TargetSceneName == P0SceneFlow.CatRoomSceneName
                && catRoomAction.ActionCategory == P0MainMenuActionCategory.PlayerPrimary
                && mainMenu.TryGetAction(P0MainMenuActionIds.StartSelectedRoute, out P0MainMenuAction selectedRoute)
                && selectedRoute.ActionCategory == P0MainMenuActionCategory.DevRouteHelper
                && mainMenu.TryGetAction(P0MainMenuActionIds.StartDefaultRoute, out P0MainMenuAction defaultRoute)
                && defaultRoute.ActionCategory == P0MainMenuActionCategory.DevRouteHelper
                && mainMenu.TryGetAction(P0MainMenuActionIds.QuickBattle, out P0MainMenuAction quickBattle)
                && quickBattle.ActionCategory == P0MainMenuActionCategory.GrayboxBattleHelper;
            bool emptySelectionGated = emptySelection.TryGetAction(P0MainMenuActionIds.EnterCatRoom, out P0MainMenuAction emptyCatRoom)
                && !emptyCatRoom.IsEnabled
                && emptySelection.TryGetAction(P0MainMenuActionIds.StartSelectedRoute, out P0MainMenuAction emptySelectedRoute)
                && !emptySelectedRoute.IsEnabled
                && emptySelection.TryGetAction(P0MainMenuActionIds.QuickBattle, out P0MainMenuAction emptyQuickBattle)
                && !emptyQuickBattle.IsEnabled;
            bool rosterHandoff = selectedRun.DreamMap.Id == P0DreamMapCatalog.BedroomDreamMapId
                && selectedRun.Roster.Count == 2
                && selectedRun.Roster.HasCat(P0PrototypeCatalog.SaibanId)
                && selectedRun.Roster.HasCat(P0PrototypeCatalog.SuzuneId)
                && !selectedRun.Roster.HasCat(P0PrototypeCatalog.NephthysId);
            bool catRoomDreamEntry = P0CatRoomPresenter.HasP0CatRoomSurface(catRoom)
                && catRoom.TryGetAction(P0CatRoomActionIds.EnterDream, out P0CatRoomAction enterDream)
                && enterDream.IsEnabled
                && enterDream.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && enterDream.Detail.Contains("守护中心床")
                && catRoom.TryGetHotspot(P0CatRoomHotspotIds.DreamEntrance, out P0CatRoomHotspot entrance)
                && entrance.FeedbackLine.Contains("卧室梦境")
                && entrance.FeedbackLine.Contains("埃及梦境");

            if (mainMenuContract && emptySelectionGated && rosterHandoff && catRoomDreamEntry)
            {
                report.AddPassed(
                    EntryCharacterSelectCheckId,
                    "Entry Character Select",
                    "Main menu selects a starter roster, enters cat-room preparation as the only player-primary CTA, then gates bedroom dream entry through the cat room.");
                return;
            }

            report.AddFailure(
                EntryCharacterSelectCheckId,
                "Entry Character Select",
                "D1 entry contract must keep cat room as the player-primary path, demote shortcuts to graybox helpers, and preserve selected-roster bedroom dream handoff.");
        }

        private static void EvaluateRuntimeSettingsAcceptance(P0PlayableReadinessReport report)
        {
            P0RuntimeSettingsCoverageReport coverage = P0RuntimeSettingsCoverage.EvaluatePrototypeSettings();
            if (coverage.IsComplete)
            {
                report.AddPassed(
                    RuntimeSettingsAcceptanceCheckId,
                    "Pause Settings Acceptance",
                    "D2/H1 pause/settings contract covers overlay, full settings hook, speed controls, shared key/button bindings, and restart confirmation.");
                return;
            }

            report.AddFailure(
                RuntimeSettingsAcceptanceCheckId,
                "Pause Settings Acceptance",
                coverage.BuildDetailedSummary());
        }

        private static void EvaluateLoadingStartSurface(P0PlayableReadinessReport report)
        {
            P0LoadingStartCoverageReport coverage = P0LoadingStartCoverage.EvaluatePrototypeLoadingStart();
            if (coverage.IsComplete)
            {
                report.AddPassed(
                    LoadingStartSurfaceCheckId,
                    "Loading Start Surface",
                    "H1 loading/start contract exposes target scene, progress, spinner, screenshot hook, and Batch 83 candidate-only boundary.");
                return;
            }

            report.AddFailure(
                LoadingStartSurfaceCheckId,
                "Loading Start Surface",
                coverage.BuildDetailedSummary());
        }

        private static void EvaluateSkillSelectionAcceptance(P0PlayableReadinessReport report)
        {
            P0SkillSelectionAcceptanceReport coverage = P0SkillSelectionAcceptanceCoverage.EvaluatePrototypeSkillSelection();
            if (coverage.IsComplete)
            {
                report.AddPassed(
                    SkillSelectionAcceptanceCheckId,
                    "Skill Selection Acceptance",
                    "D2 skill-selection contract covers ready, selected, disabled, locked, confirm, and runtime skill mapping states.");
                return;
            }

            report.AddFailure(
                SkillSelectionAcceptanceCheckId,
                "Skill Selection Acceptance",
                coverage.BuildDetailedSummary());
        }

        private static void EvaluateBattleReadabilityAcceptance(P0PlayableReadinessReport report)
        {
            P0BattleReadabilityCoverageReport coverage = P0BattleReadabilityCoverage.EvaluatePrototypeBattleBrief();
            if (coverage.IsComplete)
            {
                report.AddPassed(
                    BattleReadabilityAcceptanceCheckId,
                    "Battle Readability Acceptance",
                    "E1 battle brief keeps priority, action, compact threat, result actions, and candidate-only asset boundaries visible.");
                return;
            }

            report.AddFailure(
                BattleReadabilityAcceptanceCheckId,
                "Battle Readability Acceptance",
                coverage.BuildDetailedSummary());
        }

        private static void EvaluateStarterTrio(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            bool hasSaiban = HasCat(context.StarterCats, P0PrototypeCatalog.SaibanId, CatRole.Defender);
            bool hasNephthys = HasCat(context.StarterCats, P0PrototypeCatalog.NephthysId, CatRole.Controller);
            bool hasSuzune = HasCat(context.StarterCats, P0PrototypeCatalog.SuzuneId, CatRole.Healer);
            bool allPlayable = true;
            for (int i = 0; i < context.StarterCats.Count; i++)
            {
                CatDefinition cat = context.StarterCats[i];
                if (cat == null || cat.MaxHp <= 0f || cat.SkillIds.Count == 0)
                {
                    allPlayable = false;
                    break;
                }
            }

            if (context.StarterCats.Count == 3 && hasSaiban && hasNephthys && hasSuzune && allPlayable)
            {
                report.AddPassed(
                    StarterTrioCheckId,
                    "Starter Trio",
                    "Saiban, Nephthys, and Suzune cover defender, controller, and healer roles.");
                return;
            }

            report.AddFailure(
                StarterTrioCheckId,
                "Starter Trio",
                "P0 starter cats must include playable Saiban, Nephthys, and Suzune role coverage.");
        }

        private static void EvaluateStarterSkills(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            List<string> catIds = new List<string>();
            List<string> skillIds = new List<string>();
            for (int i = 0; i < context.StarterCats.Count; i++)
            {
                if (context.StarterCats[i] == null)
                {
                    continue;
                }

                catIds.Add(context.StarterCats[i].Id);
                for (int j = 0; j < context.StarterCats[i].SkillIds.Count; j++)
                {
                    skillIds.Add(context.StarterCats[i].SkillIds[j]);
                }
            }

            bool allSkillOwnersKnown = true;
            bool allCatSkillsDefined = true;
            for (int i = 0; i < context.StarterSkills.Count; i++)
            {
                SkillDefinition skill = context.StarterSkills[i];
                if (skill == null || !Contains(catIds, skill.OwnerCatId))
                {
                    allSkillOwnersKnown = false;
                    break;
                }
            }

            for (int i = 0; i < skillIds.Count; i++)
            {
                if (!HasSkill(context.StarterSkills, skillIds[i]))
                {
                    allCatSkillsDefined = false;
                    break;
                }
            }

            if (context.StarterSkills.Count >= 9 && allSkillOwnersKnown && allCatSkillsDefined)
            {
                report.AddPassed(
                    StarterSkillsCheckId,
                    "Starter Skills",
                    "All starter skill ids resolve to P0 cat-owned skill definitions.");
                return;
            }

            report.AddFailure(
                StarterSkillsCheckId,
                "Starter Skills",
                "Starter skill definitions are missing, underfilled, or mapped to unknown cats.");
        }

        private static void EvaluateCoreEnemies(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            bool hasMud = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.BlackMudNightmareId, EnemyBehaviorType.MoveToBed);
            bool hasTrain = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.DreamRailToyTrainId, EnemyBehaviorType.Charger);
            bool hasCold = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.ColdLightShadowId, EnemyBehaviorType.RangedHarasser);
            bool hasAlarm = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.RedEyeAlarmId, EnemyBehaviorType.RangedHarasser);
            bool hasFlyer = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.UnreadRedDotFlyerId, EnemyBehaviorType.FlyingAttachment);
            bool hasTeddy = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.FallingDreamTeddyId, EnemyBehaviorType.EliteJumpSlam);
            bool hasBoss = HasEnemy(context.CoreEnemies, P0PrototypeCatalog.CallTyrantId, EnemyBehaviorType.BossCallTyrant);

            if (hasMud && hasTrain && hasCold && hasAlarm && hasFlyer && hasTeddy && hasBoss)
            {
                report.AddPassed(
                    CoreEnemiesCheckId,
                    "Core Enemies",
                    "Core enemy roster covers P0 pressure, ranged, elite, flyer, and Call Tyrant boss roles.");
                return;
            }

            report.AddFailure(
                CoreEnemiesCheckId,
                "Core Enemies",
                "Core enemy roster is missing required P0 enemies or behavior roles.");
        }

        private static void EvaluateRouteStructure(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            RouteDefinition route = context.Route;
            if (route == null)
            {
                report.AddFailure(RouteStructureCheckId, "Route Structure", "P0 route definition is missing.");
                return;
            }

            bool hasTenLayers = route.LayerCount == 10;
            bool startsWithDefense = route.LayerCount > 0 && route.Nodes[0].NodeType == RouteNodeType.Defense;
            bool endsWithBoss = route.LayerCount >= 10
                && route.Nodes[route.Nodes.Count - 1].NodeType == RouteNodeType.Boss
                && route.Nodes[route.Nodes.Count - 1].Id == P0RouteCatalog.BossNodeId;
            bool hasRequiredTypes = HasNodeType(route, RouteNodeType.Defense)
                && HasNodeType(route, RouteNodeType.Elite)
                && HasNodeType(route, RouteNodeType.Partner)
                && HasNodeType(route, RouteNodeType.Shop)
                && HasNodeType(route, RouteNodeType.DreamEvent)
                && HasNodeType(route, RouteNodeType.BlessingOffering)
                && HasNodeType(route, RouteNodeType.RestNest)
                && HasNodeType(route, RouteNodeType.Boss);

            if (hasTenLayers && startsWithDefense && endsWithBoss && hasRequiredTypes)
            {
                report.AddPassed(
                    RouteStructureCheckId,
                    "Route Structure",
                    "Ten-layer route covers required P0 node types and ends at Call Tyrant.");
                return;
            }

            report.AddFailure(
                RouteStructureCheckId,
                "Route Structure",
                "P0 route must be ten layers, start with defense, include required node types, and end at Call Tyrant.");
        }

        private static void EvaluateDreamMaps(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            RouteDefinition route = context.Route;
            IReadOnlyList<DreamMapDefinition> maps = P0DreamMapCatalog.CreateP0DreamMaps();
            bool hasBedroom = false;
            bool hasEgypt = false;
            for (int i = 0; i < maps.Count; i++)
            {
                DreamMapDefinition map = maps[i];
                if (map == null)
                {
                    continue;
                }

                hasBedroom |= map.Id == P0DreamMapCatalog.BedroomDreamMapId
                    && map.IsPlayableInP0
                    && !string.IsNullOrWhiteSpace(map.DefenseTargetLabel);
                hasEgypt |= map.Id == P0DreamMapCatalog.EgyptDreamMapId
                    && map.IsPlaceholder
                    && !string.IsNullOrWhiteSpace(map.ThemeLabel);
            }

            bool routeCarriesMap = route != null
                && route.DreamMap != null
                && P0DreamMapCatalog.IsKnownMapId(route.DreamMap.Id);

            if (hasBedroom && hasEgypt && routeCarriesMap)
            {
                report.AddPassed(
                    DreamMapCheckId,
                    "Dream Maps",
                    "Bedroom is the playable map and Egypt is registered as a P0 placeholder context.");
                return;
            }

            report.AddFailure(
                DreamMapCheckId,
                "Dream Maps",
                "P0 must register bedroom and Egypt dream-map contexts before claiming the live-source map split.");
        }

        private static void EvaluateEgyptReadiness(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            DreamMapDefinition bedroom = P0DreamMapCatalog.GetBedroomDreamMap();
            DreamMapDefinition egypt = P0DreamMapCatalog.GetEgyptDreamMap();
            RouteDefinition bedroomRoute = P0RouteCatalog.CreateTenLayerRoute(bedroom);
            RouteDefinition egyptRoute = P0RouteCatalog.CreateEgyptPlaceholderRoute();
            RunProgressionState bedroomRun = new RunProgressionState(
                bedroomRoute,
                P0RunSession.CreateDefaultStarterCatIds());
            RunProgressionState egyptRun = new RunProgressionState(
                egyptRoute,
                P0RunSession.CreateDefaultStarterCatIds());
            P0RouteMapSurface egyptSurface = P0RouteMapPresenter.BuildSurface(
                egyptRun,
                "埃及梦境占位验证。");
            P0BattleStartContext bedroomStart = P0BattleStartContext.Create(bedroomRun);
            P0BattleStartContext egyptStart = P0BattleStartContext.Create(egyptRun);

            bool catalogState = egypt.Id == P0DreamMapCatalog.EgyptDreamMapId
                && egypt.IsPlaceholder
                && !egypt.IsPlayableInP0
                && !string.IsNullOrWhiteSpace(egypt.DisplayName)
                && !string.IsNullOrWhiteSpace(egypt.ThemeLabel)
                && !string.IsNullOrWhiteSpace(egypt.DefenseTargetLabel)
                && egypt.DisplayName != bedroom.DisplayName
                && egypt.DefenseTargetLabel != bedroom.DefenseTargetLabel;
            bool routeState = egyptRoute.DreamMap.Id == egypt.Id
                && bedroomRoute.DreamMap.Id == bedroom.Id
                && RouteShapesMatch(bedroomRoute, egyptRoute)
                && CombatContentIdsMatch(bedroomRoute, egyptRoute);
            bool surfaceState = P0RouteMapPresenter.HasP0RouteMapSurface(egyptSurface)
                && SummaryRowsContain(egyptSurface.SummaryRows, egypt.DisplayName)
                && SummaryRowsContain(egyptSurface.SummaryRows, egypt.ThemeLabel)
                && SummaryRowsContain(egyptSurface.SummaryRows, egypt.DefenseTargetLabel);
            bool battleState = egyptStart.DreamMap.Id == egypt.Id
                && bedroomStart.Wave.Id == egyptStart.Wave.Id
                && bedroomStart.Node.ContentId == egyptStart.Node.ContentId
                && egyptStart.ShouldCompleteRouteNode
                && egyptStart.ShouldPersistRunState;

            if (catalogState && routeState && surfaceState && battleState)
            {
                report.AddPassed(
                    EgyptReadinessCheckId,
                    "Egypt Readiness",
                    "Egypt is registered as a placeholder UI/readiness target while sharing the current route and combat content.");
                return;
            }

            report.AddFailure(
                EgyptReadinessCheckId,
                "Egypt Readiness",
                "Egypt must remain a registered placeholder with visible route-map context and no separate combat fork.");
        }

        private static void EvaluateBattleWaves(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            if (context.Route == null || context.WaveResolver == null)
            {
                report.AddFailure(BattleWavesCheckId, "Battle Waves", "Route or wave resolver is missing.");
                return;
            }

            bool allCombatNodesResolve = true;
            bool allSpawnsKnownEnemies = true;
            bool bossWaveContainsBoss = false;
            for (int i = 0; i < context.Route.Nodes.Count; i++)
            {
                RouteNodeDefinition node = context.Route.Nodes[i];
                if (!RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    continue;
                }

                WaveDefinition wave = context.WaveResolver(node.ContentId);
                if (wave == null || wave.SpawnGroups.Count == 0 || wave.TargetDurationSeconds <= 0f)
                {
                    allCombatNodesResolve = false;
                    continue;
                }

                for (int j = 0; j < wave.SpawnGroups.Count; j++)
                {
                    SpawnGroupDefinition group = wave.SpawnGroups[j];
                    if (!HasEnemyId(context.CoreEnemies, group.EnemyId))
                    {
                        allSpawnsKnownEnemies = false;
                    }

                    if (node.Id == P0RouteCatalog.BossNodeId && group.EnemyId == P0PrototypeCatalog.CallTyrantId)
                    {
                        bossWaveContainsBoss = true;
                    }
                }
            }

            if (allCombatNodesResolve && allSpawnsKnownEnemies && bossWaveContainsBoss)
            {
                report.AddPassed(
                    BattleWavesCheckId,
                    "Battle Waves",
                    "All combat route nodes resolve to waves with known enemy spawns and a Call Tyrant boss wave.");
                return;
            }

            report.AddFailure(
                BattleWavesCheckId,
                "Battle Waves",
                "One or more combat route nodes has no valid wave, unknown enemy spawn, or missing boss spawn.");
        }

        private static void EvaluateStatusTags(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            P0StatusTagCoverageReport coverage = context.StatusTagCoverage
                ?? P0StatusTagCoverage.Evaluate(context.StatusTags, context.StarterSkills);
            if (coverage.IsComplete)
            {
                report.AddPassed(StatusTagsCheckId, "Status Tags", coverage.BuildSummary());
                return;
            }

            report.AddFailure(StatusTagsCheckId, "Status Tags", coverage.BuildDetailedSummary());
        }

        private static void EvaluateGoldenPath(P0PlayableReadinessContext context, P0PlayableReadinessReport report)
        {
            if (context.GoldenPathAcceptance == null)
            {
                report.AddFailure(GoldenPathCheckId, "Golden Path", "Golden path acceptance report is missing.");
                return;
            }

            if (!context.GoldenPathAcceptance.IsAccepted)
            {
                report.AddFailure(GoldenPathCheckId, "Golden Path", context.GoldenPathAcceptance.BuildDetailedSummary());
                return;
            }

            if (context.GoldenPathAcceptance.WarningCount > 0)
            {
                report.AddWarning(GoldenPathCheckId, "Golden Path", context.GoldenPathAcceptance.BuildDetailedSummary());
                return;
            }

            report.AddPassed(GoldenPathCheckId, "Golden Path", context.GoldenPathAcceptance.BuildSummary());
        }

        private static bool HasCat(IReadOnlyList<CatDefinition> cats, string catId, CatRole role)
        {
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i] != null && cats[i].Id == catId && cats[i].Role == role)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasSkill(IReadOnlyList<SkillDefinition> skills, string skillId)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i] != null && skills[i].Id == skillId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasEnemy(IReadOnlyList<EnemyDefinition> enemies, string enemyId, EnemyBehaviorType behaviorType)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null && enemies[i].Id == enemyId && enemies[i].BehaviorType == behaviorType)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasEnemyId(IReadOnlyList<EnemyDefinition> enemies, string enemyId)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null && enemies[i].Id == enemyId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasNodeType(RouteDefinition route, RouteNodeType nodeType)
        {
            for (int i = 0; i < route.Nodes.Count; i++)
            {
                if (route.Nodes[i].NodeType == nodeType)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool Contains(IReadOnlyList<string> values, string value)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == value)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool RouteShapesMatch(RouteDefinition left, RouteDefinition right)
        {
            if (left == null || right == null || left.LayerCount != right.LayerCount)
            {
                return false;
            }

            for (int layer = 1; layer <= left.LayerCount; layer++)
            {
                IReadOnlyList<RouteNodeDefinition> leftOptions = left.GetLayerOptions(layer);
                IReadOnlyList<RouteNodeDefinition> rightOptions = right.GetLayerOptions(layer);
                if (leftOptions.Count != rightOptions.Count)
                {
                    return false;
                }

                for (int i = 0; i < leftOptions.Count; i++)
                {
                    if (leftOptions[i].Id != rightOptions[i].Id
                        || leftOptions[i].NodeType != rightOptions[i].NodeType)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool CombatContentIdsMatch(RouteDefinition left, RouteDefinition right)
        {
            if (left == null || right == null || left.LayerCount != right.LayerCount)
            {
                return false;
            }

            for (int layer = 1; layer <= left.LayerCount; layer++)
            {
                IReadOnlyList<RouteNodeDefinition> leftOptions = left.GetLayerOptions(layer);
                IReadOnlyList<RouteNodeDefinition> rightOptions = right.GetLayerOptions(layer);
                for (int i = 0; i < leftOptions.Count; i++)
                {
                    bool leftBattle = RouteNodeResolver.RequiresBattle(leftOptions[i].NodeType);
                    bool rightBattle = RouteNodeResolver.RequiresBattle(rightOptions[i].NodeType);
                    if (leftBattle != rightBattle)
                    {
                        return false;
                    }

                    if (leftBattle && leftOptions[i].ContentId != rightOptions[i].ContentId)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool SummaryRowsContain(IReadOnlyList<string> rows, string value)
        {
            if (rows == null || string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            for (int i = 0; i < rows.Count; i++)
            {
                if ((rows[i] ?? string.Empty).Contains(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
