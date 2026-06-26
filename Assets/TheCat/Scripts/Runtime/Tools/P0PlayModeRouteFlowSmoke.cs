using System.Collections;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Roguelite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Tools
{
    public enum P0PlayModeRouteFlowSmokeState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public static class P0PlayModeRouteFlowSmoke
    {
        private const string RunnerObjectName = "__TheCat_P0PlayModeRouteFlowSmoke";

        private static P0PlayModeRouteFlowSmokeRunner activeRunner;

        public static P0PlayModeRouteFlowSmokeState State { get; private set; }

        public static string LastSummary { get; private set; } = "P0 play mode route flow smoke has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static bool IsRunning => State == P0PlayModeRouteFlowSmokeState.Running;

        public static bool IsSettlementReadyForScreenshot { get; private set; }

        public static bool LastRunRestNestNextBattleRecoveryVerified { get; private set; }

        public static bool LastRunDreamEventCatnipNextBattleModifierVerified { get; private set; }

        public static bool LastRunShopBedPatchNextBattleSleepVerified { get; private set; }

        public static bool LastRunCatRoomReturnVerified { get; private set; }

        public static bool IsFinished => State == P0PlayModeRouteFlowSmokeState.Passed
            || State == P0PlayModeRouteFlowSmokeState.Failed;

        public static bool StartDefaultRouteSmoke()
        {
            return StartDefaultRouteSmoke(waitAtSettlementForScreenshot: false);
        }

        public static bool StartDefaultRouteSmokeForSettlementScreenshot()
        {
            return StartDefaultRouteSmoke(waitAtSettlementForScreenshot: true);
        }

        public static void ContinueAfterSettlementScreenshot()
        {
            if (activeRunner != null)
            {
                activeRunner.ReleaseSettlementScreenshotPause();
            }
        }

        private static bool StartDefaultRouteSmoke(bool waitAtSettlementForScreenshot)
        {
            LastRunRestNestNextBattleRecoveryVerified = false;
            LastRunDreamEventCatnipNextBattleModifierVerified = false;
            LastRunShopBedPatchNextBattleSleepVerified = false;
            LastRunCatRoomReturnVerified = false;

            if (!Application.isPlaying)
            {
                Complete(
                    P0PlayModeRouteFlowSmokeState.Failed,
                    "P0 play mode route flow smoke requires Play Mode.",
                    "StartDefaultRouteSmoke was called outside Play Mode.");
                return false;
            }

            if (activeRunner != null)
            {
                Object.Destroy(activeRunner.gameObject);
                activeRunner = null;
            }

            State = P0PlayModeRouteFlowSmokeState.Running;
            LastSummary = "P0 play mode route flow smoke running.";
            LastDetailedLog = LastSummary;
            IsSettlementReadyForScreenshot = false;

            GameObject runnerObject = new GameObject(RunnerObjectName);
            Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0PlayModeRouteFlowSmokeRunner>();
            activeRunner.Begin(waitAtSettlementForScreenshot);
            return true;
        }

        internal static void Complete(
            P0PlayModeRouteFlowSmokeState state,
            string summary,
            string detailedLog)
        {
            State = state;
            LastSummary = string.IsNullOrWhiteSpace(summary) ? state.ToString() : summary;
            LastDetailedLog = detailedLog ?? string.Empty;
            IsSettlementReadyForScreenshot = false;
            activeRunner = null;
        }

        internal static void MarkSettlementReadyForScreenshot(string summary)
        {
            IsSettlementReadyForScreenshot = true;
            LastSummary = string.IsNullOrWhiteSpace(summary) ? LastSummary : summary;
            LastDetailedLog = string.IsNullOrWhiteSpace(summary) ? LastDetailedLog : summary + "\n" + LastDetailedLog;
        }

        internal static void MarkRestNestNextBattleRecoveryVerified()
        {
            LastRunRestNestNextBattleRecoveryVerified = true;
        }

        internal static void MarkDreamEventCatnipNextBattleModifierVerified()
        {
            LastRunDreamEventCatnipNextBattleModifierVerified = true;
        }

        internal static void MarkShopBedPatchNextBattleSleepVerified()
        {
            LastRunShopBedPatchNextBattleSleepVerified = true;
        }

        internal static void MarkCatRoomReturnVerified()
        {
            LastRunCatRoomReturnVerified = true;
        }
    }

    internal sealed class P0PlayModeRouteFlowSmokeRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 8f;
        private const int RouteGuard = 24;
        private const int MaxBattleTicks = 1600;
        private const float BattleTickDeltaSeconds = 0.25f;
        private const int BattleTicksPerFrame = 16;
        private const int StarterCatCount = 3;
        private const int P0SkillSlotCount = 3;
        private const int MaxPendingCatUpgradeSelections = 8;
        private const float RestNestSmokeWeakSeconds = 12f;
        private const string DreamEventCatnipResidueChoiceId = "dream_event_catnip_residue";
        private const float DreamEventCatnipSkillDamageMultiplier = 1.2f;
        private const float DreamEventCatnipPoopGrowthMultiplier = 1.5f;
        private const string ShopBedPatchChoiceId = "shop_bed_patch";
        private const int ShopBedPatchCost = 3;
        private const float ShopSmokeOwnerSleepStart = 60f;
        private const float ShopBedPatchSleepRestore = 20f;
        private const float ModifierTolerance = 0.001f;

        private readonly List<string> lines = new List<string>();

        private bool failed;
        private int battleCount;
        private bool bossCleared;
        private bool bossObserved;
        private bool waitAtSettlementForScreenshot;
        private bool settlementScreenshotReleased;
        private bool restNestRecoveryPendingBattleVerification;
        private bool restNestNextBattleRecoveryVerified;
        private bool dreamEventModifierPendingBattleVerification;
        private bool dreamEventNextBattleModifierVerified;
        private bool shopBedPatchPendingBattleVerification;
        private bool shopBedPatchNextBattleSleepVerified;
        private string restNestRecoveryCatId;
        private float restNestRecoverySafeHp;
        private int shopBedPatchFishBefore;
        private int shopBedPatchPurchasesBefore;
        private float shopBedPatchExpectedStartingSleep;

        public void Begin(bool waitAtSettlementForScreenshot)
        {
            this.waitAtSettlementForScreenshot = waitAtSettlementForScreenshot;
            settlementScreenshotReleased = false;
            StartCoroutine(Run());
        }

        public void ReleaseSettlementScreenshotPause()
        {
            settlementScreenshotReleased = true;
        }

        private IEnumerator Run()
        {
            lines.Clear();
            failed = false;
            battleCount = 0;
            bossCleared = false;
            bossObserved = false;
            restNestRecoveryPendingBattleVerification = false;
            restNestNextBattleRecoveryVerified = false;
            dreamEventModifierPendingBattleVerification = false;
            dreamEventNextBattleModifierVerified = false;
            shopBedPatchPendingBattleVerification = false;
            shopBedPatchNextBattleSleepVerified = false;
            restNestRecoveryCatId = string.Empty;
            restNestRecoverySafeHp = 0f;
            shopBedPatchFishBefore = 0;
            shopBedPatchPurchasesBefore = 0;
            shopBedPatchExpectedStartingSleep = 0f;

            P0RunSession.Clear();
            yield return LoadScene(P0SceneFlow.MainMenuSceneName);
            if (failed)
            {
                yield break;
            }

            MainMenuController mainMenu = Object.FindAnyObjectByType<MainMenuController>();
            if (mainMenu == null)
            {
                Fail("P0MainMenu is missing MainMenuController.");
                yield break;
            }

            mainMenu.StartP0Run();
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            Add("Started default route from P0MainMenu.");

            int guard = RouteGuard;
            while (P0RunSession.CurrentRoute != null && !P0RunSession.CurrentRoute.IsComplete)
            {
                if (guard <= 0)
                {
                    Fail("Route guard exhausted before route completion.");
                    yield break;
                }

                guard--;
                RouteNodeDefinition node = P0RunSession.CurrentRoute.CurrentNode;
                if (node == null)
                {
                    Fail("Current route node is missing before route completion.");
                    yield break;
                }

                if (RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    yield return ResolveBattleNode(node);
                }
                else
                {
                    yield return ResolveRewardNode(node);
                }

                if (failed)
                {
                    yield break;
                }
            }

            P0RunSettlementSummary settlement = new P0RunSettlementSummary(P0RunSession.CurrentRun);
            if (!settlement.IsCleared)
            {
                Fail("Route smoke ended without clearing the route: " + settlement.ResultLabel + ".");
                yield break;
            }

            if (settlement.CompletedNodes != settlement.TotalLayers || settlement.TotalLayers != 10)
            {
                Fail("Route smoke settlement node count is " + settlement.CompletedNodes + "/" + settlement.TotalLayers + ".");
                yield break;
            }

            if (settlement.BattleSuccesses != battleCount || settlement.BattleFailures != 0)
            {
                Fail("Route smoke battle summary mismatch: success "
                    + settlement.BattleSuccesses
                    + " failures "
                    + settlement.BattleFailures
                    + " counted "
                    + battleCount
                    + ".");
                yield break;
            }

            if (!bossCleared)
            {
                Fail("Route smoke did not clear the P0 Boss node.");
                yield break;
            }

            if (!P0SettlementPresenter.HasP0ClearedSettlementRows(settlement))
            {
                Fail("Route smoke cleared the route but settlement rows are incomplete: "
                    + P0SettlementPresenter.BuildCompactSummary(settlement)
                    + ".");
                yield break;
            }

            string actionSummary = P0SettlementPresenter.BuildActionTelemetrySummary(settlement);
            if (!P0SettlementPresenter.HasP0ActionTelemetry(settlement))
            {
                Fail("Route smoke cleared the route but action telemetry is incomplete: "
                    + actionSummary
                    + ".");
                yield break;
            }

            if (!restNestNextBattleRecoveryVerified)
            {
                Fail("Route smoke did not verify RestNest next-battle recovery.");
                yield break;
            }

            if (!dreamEventNextBattleModifierVerified)
            {
                Fail("Route smoke did not verify DreamEvent next-battle modifier.");
                yield break;
            }

            if (!shopBedPatchNextBattleSleepVerified)
            {
                Fail("Route smoke did not verify Shop bed-patch next-battle sleep.");
                yield break;
            }

            Add("Settlement rows verified: " + P0SettlementPresenter.BuildCompactSummary(settlement) + ".");
            Add("Settlement action telemetry verified: " + actionSummary + ".");
            if (waitAtSettlementForScreenshot)
            {
                string settlementReady = "P0 play mode route flow settlement ready for screenshot: "
                    + P0SettlementPresenter.BuildCompactSummary(settlement)
                    + ".";
                Add(settlementReady);
                P0PlayModeRouteFlowSmoke.MarkSettlementReadyForScreenshot(settlementReady);
                while (!settlementScreenshotReleased && !failed)
                {
                    yield return null;
                }
            }

            yield return ReturnSettlementToCatRoom(settlement);
            if (failed)
            {
                yield break;
            }

            string summary = "P0 play mode route flow smoke passed: nodes "
                + settlement.CompletedNodes
                + "/"
                + settlement.TotalLayers
                + ", battles "
                + settlement.BattleSuccesses
                + ", boss "
                + (bossObserved ? "observed" : "cleared")
                + ", fish "
                + settlement.FishTreats
                + ", shards "
                + settlement.DreamShards
                + ", "
                + actionSummary
                + ", RestNest next-battle recovery verified"
                + ", DreamEvent catnip next-battle modifier verified"
                + ", Shop bed-patch next-battle sleep verified"
                + ", cat-room return verified"
                + ".";
            Add(summary);
            Complete(P0PlayModeRouteFlowSmokeState.Passed, summary);
        }

        private IEnumerator ResolveBattleNode(RouteNodeDefinition node)
        {
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            RouteMapController routeMap = Object.FindAnyObjectByType<RouteMapController>();
            if (routeMap == null)
            {
                Fail("RouteMapController missing before battle node " + node.Id + ".");
                yield break;
            }

            int completedBefore = P0RunSession.CurrentRoute.CompletedCount;
            yield return ResolvePendingCatUpgrades(routeMap, "battle node " + node.Id);
            if (failed)
            {
                yield break;
            }

            routeMap.ExecuteInputCommand(P0InputCommand.ContinueRoute);
            yield return WaitForScene(P0SceneFlow.GrayboxBattleSceneName);
            if (failed)
            {
                yield break;
            }

            GrayboxBattleController battleController = Object.FindAnyObjectByType<GrayboxBattleController>();
            if (battleController == null || battleController.Battle == null)
            {
                Fail("GrayboxBattleController or BattleSimulation missing for node " + node.Id + ".");
                yield break;
            }

            if (!VerifyRestNestNextBattleRecovery(battleController, node))
            {
                yield break;
            }

            if (!VerifyDreamEventNextBattleModifier(battleController, node))
            {
                yield break;
            }

            if (!VerifyShopBedPatchNextBattleSleep(battleController, node))
            {
                yield break;
            }

            int ticks = 0;
            while (battleController.Battle.Outcome == BattleOutcome.InProgress && ticks < MaxBattleTicks)
            {
                for (int frameTick = 0;
                     frameTick < BattleTicksPerFrame
                     && battleController.Battle.Outcome == BattleOutcome.InProgress
                     && ticks < MaxBattleTicks;
                     frameTick++)
                {
                    DriveBattlePlayer(battleController, ticks);
                    battleController.AdvanceGraybox(BattleTickDeltaSeconds);
                    ticks++;
                }

                yield return null;
            }

            if (battleController.Battle.Outcome != BattleOutcome.Victory)
            {
                Fail("Battle node " + node.Id + " ended as " + battleController.Battle.Outcome + " after " + ticks + " smoke tick(s).");
                yield break;
            }

            battleCount++;
            if (node.Id == P0RouteCatalog.BossNodeId)
            {
                bossCleared = true;
                bossObserved = battleController.Battle.BossSummonsTriggered > 0
                    && battleController.Battle.BossThrowsTriggered > 0;
            }

            if (P0RunSession.CurrentRoute.CompletedCount <= completedBefore)
            {
                Fail("Battle node " + node.Id + " did not advance route completion.");
                yield break;
            }

            P0BattleResultSurface resultSurface = battleController.BuildBattleResultSurfaceForSmoke();
            if (!P0BattleResultPresenter.HasP0BattleResultSurface(resultSurface))
            {
                Fail("Battle node " + node.Id + " produced incomplete result surface: "
                    + P0BattleResultPresenter.BuildCompactSummary(resultSurface)
                    + ".");
                yield break;
            }

            Add("Battle " + node.Id
                + " -> "
                + battleController.Battle.Outcome
                + " ticks "
                + ticks
                + " route "
                + P0RunSession.CurrentRoute.CompletedCount
                + "/"
                + P0RunSession.CurrentRoute.Route.LayerCount
                + ".");
            Add("Battle result surface verified: "
                + P0BattleResultPresenter.BuildCompactSummary(resultSurface)
                + ".");

            battleController.ContinueRoute();
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
        }

        private IEnumerator ResolveRewardNode(RouteNodeDefinition node)
        {
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            RouteMapController routeMap = Object.FindAnyObjectByType<RouteMapController>();
            if (routeMap == null)
            {
                Fail("RouteMapController missing before reward node " + node.Id + ".");
                yield break;
            }

            int completedBefore = P0RunSession.CurrentRoute.CompletedCount;
            yield return ResolvePendingCatUpgrades(routeMap, "reward node " + node.Id);
            if (failed)
            {
                yield break;
            }

            if (node.NodeType == RouteNodeType.RestNest)
            {
                SeedRestNestRecoveryForSmoke(node);
            }

            if (ShouldVerifyShopBedPatch(node) && !SeedShopBedPatchForSmoke(node))
            {
                yield break;
            }

            if (ShouldForceDreamEventCatnip(node))
            {
                yield return ResolveDreamEventCatnipReward(routeMap, node, completedBefore);
            }
            else
            {
                routeMap.ExecuteInputCommand(P0InputCommand.ContinueRoute);
            }

            yield return null;

            if (P0RunSession.CurrentRoute.CompletedCount <= completedBefore)
            {
                Fail("Reward node " + node.Id + " did not advance route completion.");
                yield break;
            }

            if (node.NodeType == RouteNodeType.RestNest && !CaptureRestNestRecoveryAfterReward(node))
            {
                yield break;
            }

            if (node.NodeType == RouteNodeType.Shop && !CaptureShopBedPatchAfterReward(node))
            {
                yield break;
            }

            if (node.NodeType == RouteNodeType.Shop && shopBedPatchPendingBattleVerification)
            {
                Add("Shop bed patch queued next-battle sleep verification after reward "
                    + node.Id
                    + ".");
            }

            if (node.NodeType == RouteNodeType.DreamEvent && dreamEventModifierPendingBattleVerification)
            {
                Add("DreamEvent catnip residue queued next-battle modifier after reward "
                    + node.Id
                    + ".");
            }

            Add("Reward " + node.Id
                + " resolved route "
                + P0RunSession.CurrentRoute.CompletedCount
                + "/"
                + P0RunSession.CurrentRoute.Route.LayerCount
                + ".");
        }

        private bool ShouldVerifyShopBedPatch(RouteNodeDefinition node)
        {
            return node != null
                && node.NodeType == RouteNodeType.Shop
                && node.ContentId == P0RouteCatalog.MidnightKibbleShopContentId
                && !shopBedPatchPendingBattleVerification
                && !shopBedPatchNextBattleSleepVerified;
        }

        private bool SeedShopBedPatchForSmoke(RouteNodeDefinition node)
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null)
            {
                Fail("Shop bed-patch smoke is missing run state before " + node.Id + ".");
                return false;
            }

            RouteRewardChoice choice = FindChoiceById(
                P0RouteRewardResolver.CreatePlaceholderChoices(node, run),
                ShopBedPatchChoiceId);
            if (choice == null
                || choice.FishTreatsCost != ShopBedPatchCost
                || choice.OwnerSleepRestored != ShopBedPatchSleepRestore)
            {
                Fail("Shop bed-patch smoke could not find expected "
                    + ShopBedPatchChoiceId
                    + " choice before "
                    + node.Id
                    + ".");
                return false;
            }

            if (run.Wallet.FishTreats < choice.FishTreatsCost)
            {
                Fail("Shop bed-patch smoke reached "
                    + node.Id
                    + " with only "
                    + run.Wallet.FishTreats
                    + "/"
                    + choice.FishTreatsCost
                    + " fish treat(s).");
                return false;
            }

            shopBedPatchFishBefore = run.Wallet.FishTreats;
            shopBedPatchPurchasesBefore = run.ShopPurchases;
            shopBedPatchExpectedStartingSleep = ShopSmokeOwnerSleepStart + choice.OwnerSleepRestored;
            if (shopBedPatchExpectedStartingSleep > run.CoreValues.OwnerSleepMax)
            {
                shopBedPatchExpectedStartingSleep = run.CoreValues.OwnerSleepMax;
            }

            run.CoreValues.Capture(
                ShopSmokeOwnerSleepStart,
                run.CoreValues.OwnerSleepMax,
                run.CoreValues.OwnerSleepBaseMax,
                run.CoreValues.TeamPoop,
                run.CoreValues.TeamHunger);
            Add("Seeded Shop bed-patch smoke state before "
                + node.Id
                + ": sleep "
                + ShopSmokeOwnerSleepStart.ToString("0")
                + "/"
                + run.CoreValues.OwnerSleepMax.ToString("0")
                + ", fish "
                + shopBedPatchFishBefore
                + ".");
            return true;
        }

        private bool CaptureShopBedPatchAfterReward(RouteNodeDefinition node)
        {
            if (!ShouldVerifyShopBedPatch(node))
            {
                return true;
            }

            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null)
            {
                Fail("Shop bed-patch smoke is missing run state after " + node.Id + ".");
                return false;
            }

            int expectedFish = shopBedPatchFishBefore - ShopBedPatchCost;
            bool fishSpent = run.Wallet.FishTreats == expectedFish;
            bool sleepRestored = Approximately(run.CoreValues.OwnerSleepCurrent, shopBedPatchExpectedStartingSleep);
            bool purchaseRecorded = run.ShopPurchases == shopBedPatchPurchasesBefore + 1;
            if (!fishSpent || !sleepRestored || !purchaseRecorded)
            {
                Fail("Shop bed patch did not apply expected route state after "
                    + node.Id
                    + ": fish "
                    + run.Wallet.FishTreats
                    + " expected "
                    + expectedFish
                    + ", sleep "
                    + run.CoreValues.OwnerSleepCurrent.ToString("0.0")
                    + " expected "
                    + shopBedPatchExpectedStartingSleep.ToString("0.0")
                    + ", shop purchases "
                    + run.ShopPurchases
                    + " expected "
                    + (shopBedPatchPurchasesBefore + 1)
                    + ".");
                return false;
            }

            shopBedPatchPendingBattleVerification = true;
            Add("Shop bed-patch state verified after reward "
                + node.Id
                + ": sleep "
                + run.CoreValues.OwnerSleepCurrent.ToString("0")
                + ", fish "
                + run.Wallet.FishTreats
                + ".");
            return true;
        }

        private bool ShouldForceDreamEventCatnip(RouteNodeDefinition node)
        {
            return node != null
                && node.NodeType == RouteNodeType.DreamEvent
                && node.ContentId == P0RouteCatalog.SoftRainWindowEventContentId
                && !dreamEventModifierPendingBattleVerification
                && !dreamEventNextBattleModifierVerified;
        }

        private IEnumerator ResolveDreamEventCatnipReward(
            RouteMapController routeMap,
            RouteNodeDefinition node,
            int completedBefore)
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null || run.Route == null)
            {
                Fail("DreamEvent reward smoke is missing run state before " + node.Id + ".");
                yield break;
            }

            if (!run.Route.SelectCurrentNode(node.Id))
            {
                Fail("DreamEvent reward smoke could not lock current node " + node.Id + ".");
                yield break;
            }

            P0RouteMapCommandResult result = routeMap.ExecuteInputCommand(P0InputCommand.SelectCat2);
            if (!result.IsHandled
                || result.Action != P0RouteMapCommandAction.ResolveRewardChoice
                || result.SelectedNodeId != node.Id
                || result.SelectedChoiceId != DreamEventCatnipResidueChoiceId)
            {
                Fail("DreamEvent reward smoke selected "
                    + result.SelectedChoiceId
                    + " on "
                    + result.SelectedNodeId
                    + " instead of "
                    + DreamEventCatnipResidueChoiceId
                    + " on "
                    + node.Id
                    + ".");
                yield break;
            }

            yield return null;

            if (P0RunSession.CurrentRoute.CompletedCount <= completedBefore)
            {
                yield break;
            }

            if (!run.PendingBattleModifiers.HasPending
                || run.PendingBattleModifiers.SourceCount != 1
                || !Approximately(run.PendingBattleModifiers.SkillDamageMultiplier, DreamEventCatnipSkillDamageMultiplier)
                || !Approximately(run.PendingBattleModifiers.PoopGrowthMultiplier, DreamEventCatnipPoopGrowthMultiplier))
            {
                Fail("DreamEvent "
                    + DreamEventCatnipResidueChoiceId
                    + " did not queue expected next-battle modifier after "
                    + node.Id
                    + ": "
                    + run.PendingBattleModifiers.BuildDiagnosticSummary()
                    + ".");
                yield break;
            }

            dreamEventModifierPendingBattleVerification = true;
        }

        private void SeedRestNestRecoveryForSmoke(RouteNodeDefinition node)
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null || run.Roster.CatIds.Count == 0)
            {
                return;
            }

            restNestRecoveryCatId = run.Roster.CatIds[0];
            float maxHp = FindStarterCatMaxHp(restNestRecoveryCatId);
            float lowHp = maxHp * 0.2f;
            restNestRecoverySafeHp = maxHp * RunCatVitals.RestNestHpSafeRatio;
            run.CatVitals.Capture(restNestRecoveryCatId, maxHp, lowHp, RestNestSmokeWeakSeconds);
            Add("Seeded RestNest recovery smoke state before "
                + node.Id
                + ": "
                + restNestRecoveryCatId
                + " hp "
                + lowHp.ToString("0")
                + "/"
                + maxHp.ToString("0")
                + " weak "
                + RestNestSmokeWeakSeconds.ToString("0")
                + "s.");
        }

        private bool CaptureRestNestRecoveryAfterReward(RouteNodeDefinition node)
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null || string.IsNullOrWhiteSpace(restNestRecoveryCatId))
            {
                Fail("RestNest recovery smoke state is missing after " + node.Id + ".");
                return false;
            }

            if (!run.CatVitals.TryGet(restNestRecoveryCatId, out RunCatVitalSnapshot snapshot))
            {
                Fail("RestNest recovery did not preserve cat vital snapshot for " + restNestRecoveryCatId + ".");
                return false;
            }

            if (snapshot.CurrentHp + 0.001f < restNestRecoverySafeHp || snapshot.IsWeak)
            {
                Fail("RestNest recovery did not restore cat HP/weak state for "
                    + restNestRecoveryCatId
                    + ": hp "
                    + snapshot.CurrentHp.ToString("0.0")
                    + " safe "
                    + restNestRecoverySafeHp.ToString("0.0")
                    + " weak "
                    + snapshot.IsWeak
                    + ".");
                return false;
            }

            restNestRecoveryPendingBattleVerification = true;
            Add("RestNest recovery state verified after reward "
                + node.Id
                + ": "
                + restNestRecoveryCatId
                + " hp "
                + snapshot.CurrentHp.ToString("0")
                + "/"
                + snapshot.MaxHp.ToString("0")
                + ".");
            return true;
        }

        private bool VerifyRestNestNextBattleRecovery(GrayboxBattleController battleController, RouteNodeDefinition node)
        {
            if (!restNestRecoveryPendingBattleVerification)
            {
                return true;
            }

            IReadOnlyList<P0CatHudCard> cards = battleController.BuildCatHudCardsForSmoke();
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].CatId != restNestRecoveryCatId)
                {
                    continue;
                }

                if (cards[i].CurrentHp + 0.001f < restNestRecoverySafeHp || cards[i].IsWeak)
                {
                    Fail("RestNest next-battle cat HUD did not start recovered after "
                        + node.Id
                        + ": "
                        + cards[i].BuildSummary()
                        + ".");
                    return false;
                }

                restNestRecoveryPendingBattleVerification = false;
                restNestNextBattleRecoveryVerified = true;
                P0PlayModeRouteFlowSmoke.MarkRestNestNextBattleRecoveryVerified();
                Add("RestNest next-battle recovery verified at "
                    + node.Id
                    + ": "
                    + cards[i].BuildSummary()
                    + ".");
                return true;
            }

            Fail("RestNest next-battle cat HUD is missing " + restNestRecoveryCatId + " after " + node.Id + ".");
            return false;
        }

        private bool VerifyDreamEventNextBattleModifier(GrayboxBattleController battleController, RouteNodeDefinition node)
        {
            if (!dreamEventModifierPendingBattleVerification)
            {
                return true;
            }

            P0BattleModifierEvidence evidence = battleController.BuildBattleModifierEvidenceForSmoke();
            float expectedPoopGrowth = P0Tuning.Default.PoopNaturalGrowthPerSecond * DreamEventCatnipPoopGrowthMultiplier;
            if (!evidence.HasPendingSource
                || evidence.SourceCount != 1
                || !Approximately(evidence.SkillDamageMultiplier, DreamEventCatnipSkillDamageMultiplier)
                || !Approximately(evidence.PoopNaturalGrowthPerSecond, expectedPoopGrowth))
            {
                Fail("DreamEvent catnip next-battle modifier did not apply to battle "
                    + node.Id
                    + ": "
                    + evidence.BuildDiagnosticSummary()
                    + ".");
                return false;
            }

            RunProgressionState run = P0RunSession.CurrentRun;
            if (run != null && run.PendingBattleModifiers.HasPending)
            {
                Fail("DreamEvent catnip next-battle modifier was not consumed at battle "
                    + node.Id
                    + ": "
                    + run.PendingBattleModifiers.BuildDiagnosticSummary()
                    + ".");
                return false;
            }

            dreamEventModifierPendingBattleVerification = false;
            dreamEventNextBattleModifierVerified = true;
            P0PlayModeRouteFlowSmoke.MarkDreamEventCatnipNextBattleModifierVerified();
            Add("DreamEvent catnip next-battle modifier verified at "
                + node.Id
                + ": "
                + evidence.BuildDiagnosticSummary()
                + ".");
            return true;
        }

        private bool VerifyShopBedPatchNextBattleSleep(GrayboxBattleController battleController, RouteNodeDefinition node)
        {
            if (!shopBedPatchPendingBattleVerification)
            {
                return true;
            }

            if (battleController.Battle == null
                || !Approximately(battleController.Battle.OwnerSleep.Current, shopBedPatchExpectedStartingSleep))
            {
                string current = battleController.Battle == null
                    ? "missing battle"
                    : battleController.Battle.OwnerSleep.Current.ToString("0.0");
                Fail("Shop bed-patch next-battle sleep did not carry into battle "
                    + node.Id
                    + ": "
                    + current
                    + " expected "
                    + shopBedPatchExpectedStartingSleep.ToString("0.0")
                    + ".");
                return false;
            }

            shopBedPatchPendingBattleVerification = false;
            shopBedPatchNextBattleSleepVerified = true;
            P0PlayModeRouteFlowSmoke.MarkShopBedPatchNextBattleSleepVerified();
            Add("Shop bed-patch next-battle sleep verified at "
                + node.Id
                + ": sleep "
                + battleController.Battle.OwnerSleep.Current.ToString("0")
                + ".");
            return true;
        }

        private static RouteRewardChoice FindChoiceById(IReadOnlyList<RouteRewardChoice> choices, string choiceId)
        {
            if (choices == null || string.IsNullOrWhiteSpace(choiceId))
            {
                return null;
            }

            for (int i = 0; i < choices.Count; i++)
            {
                if (choices[i] != null && choices[i].Id == choiceId)
                {
                    return choices[i];
                }
            }

            return null;
        }

        private static bool Approximately(float actual, float expected)
        {
            return Mathf.Abs(actual - expected) <= ModifierTolerance;
        }

        private static float FindStarterCatMaxHp(string catId)
        {
            IReadOnlyList<TheCat.Data.Definitions.CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i].MaxHp;
                }
            }

            return 200f;
        }

        private IEnumerator ReturnSettlementToCatRoom(P0RunSettlementSummary settlement)
        {
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            RouteMapController routeMap = Object.FindAnyObjectByType<RouteMapController>();
            if (routeMap == null)
            {
                Fail("RouteMapController missing before settlement cat-room return.");
                yield break;
            }

            P0RouteMapSurface surface = routeMap.BuildRouteMapSurfaceForSmoke();
            if (!surface.IsRouteComplete
                || !surface.TryGetAction(P0RouteMapActionIds.ReturnCatRoom, out P0RouteMapAction returnCatRoom)
                || !returnCatRoom.IsEnabled
                || returnCatRoom.TargetSceneName != P0SceneFlow.CatRoomSceneName)
            {
                Fail("Route settlement surface is missing enabled cat-room return action after "
                    + P0SettlementPresenter.BuildCompactSummary(settlement)
                    + ".");
                yield break;
            }

            routeMap.ExecuteInputCommand(P0InputCommand.ContinueRoute);
            yield return WaitForScene(P0SceneFlow.CatRoomSceneName);
            if (failed)
            {
                yield break;
            }

            CatRoomController catRoom = Object.FindAnyObjectByType<CatRoomController>();
            if (catRoom == null)
            {
                Fail("CatRoomController missing after settlement cat-room return.");
                yield break;
            }

            P0CatRoomSurface catRoomSurface = catRoom.BuildCatRoomSurfaceForSmoke();
            if (!P0CatRoomPresenter.HasP0CatRoomSurface(catRoomSurface)
                || P0CatRoomSession.CurrentState.ReturnReason != P0CatRoomReturnReason.RouteCleared
                || P0CatRoomSession.CurrentState.HasActiveRun)
            {
                Fail("Cat-room return state is incomplete after route settlement: "
                    + P0CatRoomPresenter.BuildCompactSummary(catRoomSurface)
                    + ".");
                yield break;
            }

            Add("Route settlement cat-room return verified: "
                + catRoomSurface.ReturnFeedbackLabel
                + ".");
            P0PlayModeRouteFlowSmoke.MarkCatRoomReturnVerified();
        }

        private IEnumerator ResolvePendingCatUpgrades(RouteMapController routeMap, string context)
        {
            RunProgressionState run = P0RunSession.CurrentRun;
            if (run == null || !run.CatUpgrades.HasPendingUpgrade)
            {
                yield break;
            }

            int guard = MaxPendingCatUpgradeSelections;
            while (run.CatUpgrades.HasPendingUpgrade)
            {
                if (guard <= 0)
                {
                    Fail("Pending cat upgrade guard exhausted before " + context + ".");
                    yield break;
                }

                guard--;
                int learnedBefore = run.CatUpgrades.LearnedUpgradeCount;
                routeMap.ExecuteInputCommand(P0InputCommand.SelectCat1);
                yield return null;

                if (run.CatUpgrades.LearnedUpgradeCount <= learnedBefore)
                {
                    Fail("Pending cat upgrade did not resolve before "
                        + context
                        + ".");
                    yield break;
                }

                Add("Resolved pending cat upgrade before "
                    + context
                    + ": "
                    + run.CatUpgrades.LastResolvedSummary
                    + ".");
            }
        }

        private void DriveBattlePlayer(GrayboxBattleController battleController, int tick)
        {
            int catIndex = (tick / P0SkillSlotCount) % StarterCatCount;
            int skillSlot = tick % P0SkillSlotCount;
            battleController.SelectCat(catIndex);
            battleController.CastSkillBySlot(skillSlot);

            if (tick % 24 == 0)
            {
                battleController.UseBedCare();
            }

            if (tick % 40 == 0)
            {
                battleController.UseLitterBox();
            }

            if (tick % 48 == 0)
            {
                battleController.UseFeeder();
            }
        }

        private IEnumerator LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield return WaitForScene(sceneName);
        }

        private IEnumerator WaitForScene(string sceneName)
        {
            float start = Time.realtimeSinceStartup;
            while (SceneManager.GetActiveScene().name != sceneName)
            {
                if (Time.realtimeSinceStartup - start > SceneLoadTimeoutSeconds)
                {
                    Fail("Timed out waiting for scene " + sceneName + "; active scene is " + SceneManager.GetActiveScene().name + ".");
                    yield break;
                }

                yield return null;
            }

            yield return null;
        }

        private void Add(string line)
        {
            lines.Add(line);
        }

        private void Fail(string message)
        {
            failed = true;
            Add("FAILED: " + message);
            Complete(P0PlayModeRouteFlowSmokeState.Failed, message);
        }

        private void Complete(P0PlayModeRouteFlowSmokeState state, string summary)
        {
            P0PlayModeRouteFlowSmoke.Complete(state, summary, string.Join("\n", lines));
            Destroy(gameObject);
        }
    }
}
