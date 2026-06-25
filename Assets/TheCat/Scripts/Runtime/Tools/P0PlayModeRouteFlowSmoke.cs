using System.Collections;
using System.Collections.Generic;
using TheCat.Combat;
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

        public static bool IsFinished => State == P0PlayModeRouteFlowSmokeState.Passed
            || State == P0PlayModeRouteFlowSmokeState.Failed;

        public static bool StartDefaultRouteSmoke()
        {
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

            GameObject runnerObject = new GameObject(RunnerObjectName);
            Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0PlayModeRouteFlowSmokeRunner>();
            activeRunner.Begin();
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
            activeRunner = null;
        }
    }

    internal sealed class P0PlayModeRouteFlowSmokeRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 8f;
        private const int RouteGuard = 24;
        private const int MaxBattleTicks = 1600;
        private const float BattleTickDeltaSeconds = 0.25f;
        private const int StarterCatCount = 3;
        private const int P0SkillSlotCount = 3;
        private const int MaxPendingCatUpgradeSelections = 8;

        private readonly List<string> lines = new List<string>();

        private bool failed;
        private int battleCount;
        private bool bossCleared;
        private bool bossObserved;

        public void Begin()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            lines.Clear();
            failed = false;
            battleCount = 0;
            bossCleared = false;
            bossObserved = false;

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

            Add("Settlement rows verified: " + P0SettlementPresenter.BuildCompactSummary(settlement) + ".");
            Add("Settlement action telemetry verified: " + actionSummary + ".");

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

            int ticks = 0;
            while (battleController.Battle.Outcome == BattleOutcome.InProgress && ticks < MaxBattleTicks)
            {
                DriveBattlePlayer(battleController, ticks);
                battleController.AdvanceGraybox(BattleTickDeltaSeconds);
                ticks++;
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

            routeMap.ExecuteInputCommand(P0InputCommand.ContinueRoute);
            yield return null;

            if (P0RunSession.CurrentRoute.CompletedCount <= completedBefore)
            {
                Fail("Reward node " + node.Id + " did not advance route completion.");
                yield break;
            }

            Add("Reward " + node.Id
                + " resolved route "
                + P0RunSession.CurrentRoute.CompletedCount
                + "/"
                + P0RunSession.CurrentRoute.Route.LayerCount
                + ".");
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
