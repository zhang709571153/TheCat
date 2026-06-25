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
    public enum P0PlayModeDefeatFlowSmokeState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public static class P0PlayModeDefeatFlowSmoke
    {
        private const string RunnerObjectName = "__TheCat_P0PlayModeDefeatFlowSmoke";

        private static P0PlayModeDefeatFlowSmokeRunner activeRunner;

        public static P0PlayModeDefeatFlowSmokeState State { get; private set; }

        public static string LastSummary { get; private set; } = "P0 play mode defeat flow smoke has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static bool IsRunning => State == P0PlayModeDefeatFlowSmokeState.Running;

        public static bool IsFinished => State == P0PlayModeDefeatFlowSmokeState.Passed
            || State == P0PlayModeDefeatFlowSmokeState.Failed;

        public static bool StartDefaultDefeatSmoke()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0PlayModeDefeatFlowSmokeState.Failed,
                    "P0 play mode defeat flow smoke requires Play Mode.",
                    "StartDefaultDefeatSmoke was called outside Play Mode.");
                return false;
            }

            if (activeRunner != null)
            {
                Object.Destroy(activeRunner.gameObject);
                activeRunner = null;
            }

            State = P0PlayModeDefeatFlowSmokeState.Running;
            LastSummary = "P0 play mode defeat flow smoke running.";
            LastDetailedLog = LastSummary;

            GameObject runnerObject = new GameObject(RunnerObjectName);
            Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0PlayModeDefeatFlowSmokeRunner>();
            activeRunner.Begin();
            return true;
        }

        internal static void Complete(
            P0PlayModeDefeatFlowSmokeState state,
            string summary,
            string detailedLog)
        {
            State = state;
            LastSummary = string.IsNullOrWhiteSpace(summary) ? state.ToString() : summary;
            LastDetailedLog = detailedLog ?? string.Empty;
            activeRunner = null;
        }
    }

    internal sealed class P0PlayModeDefeatFlowSmokeRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 8f;
        private const float ForcedSleepDamage = 9999f;

        private readonly List<string> lines = new List<string>();

        private bool failed;

        public void Begin()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            lines.Clear();
            failed = false;

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

            RouteMapController routeMap = Object.FindAnyObjectByType<RouteMapController>();
            if (routeMap == null)
            {
                Fail("P0RouteMap is missing RouteMapController before defeat battle.");
                yield break;
            }

            int completedBefore = P0RunSession.CurrentRoute == null ? -1 : P0RunSession.CurrentRoute.CompletedCount;
            routeMap.ExecuteInputCommand(P0InputCommand.ContinueRoute);
            yield return WaitForScene(P0SceneFlow.GrayboxBattleSceneName);
            if (failed)
            {
                yield break;
            }

            GrayboxBattleController battleController = Object.FindAnyObjectByType<GrayboxBattleController>();
            if (battleController == null || battleController.Battle == null)
            {
                Fail("GrayboxBattleController or BattleSimulation missing for forced defeat.");
                yield break;
            }

            battleController.Battle.DebugDamageOwnerSleep(ForcedSleepDamage);
            battleController.AdvanceGraybox(0f);
            yield return null;

            if (battleController.Battle.Outcome != BattleOutcome.Defeat)
            {
                Fail("Forced defeat expected Defeat but got " + battleController.Battle.Outcome + ".");
                yield break;
            }

            if (P0RunSession.CurrentRoute == null
                || !P0RunSession.CurrentRoute.IsFailed
                || P0RunSession.CurrentRoute.CompletedCount <= completedBefore)
            {
                Fail("Forced defeat did not record a failed route node.");
                yield break;
            }

            P0BattleResultSurface resultSurface = battleController.BuildBattleResultSurfaceForSmoke();
            if (!P0BattleResultPresenter.HasP0BattleResultSurface(resultSurface)
                || resultSurface.Outcome != BattleOutcome.Defeat)
            {
                Fail("Forced defeat produced incomplete battle result surface: "
                    + P0BattleResultPresenter.BuildCompactSummary(resultSurface)
                    + ".");
                yield break;
            }

            if (!resultSurface.TryGetAction(P0BattleResultActionIds.ContinueRoute, out P0BattleResultAction continueRoute)
                || !continueRoute.IsEnabled
                || continueRoute.TargetSceneName != P0SceneFlow.RouteMapSceneName)
            {
                Fail("Forced defeat result surface does not expose enabled route continuation.");
                yield break;
            }

            Add("Defeat battle result surface verified: "
                + P0BattleResultPresenter.BuildCompactSummary(resultSurface)
                + ".");

            battleController.ContinueRoute();
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            RouteMapController failedRouteMap = Object.FindAnyObjectByType<RouteMapController>();
            if (failedRouteMap == null)
            {
                Fail("P0RouteMap is missing RouteMapController after forced defeat.");
                yield break;
            }

            P0RouteMapSurface routeSurface = failedRouteMap.BuildRouteMapSurfaceForSmoke();
            P0RunSettlementSummary settlement = new P0RunSettlementSummary(P0RunSession.CurrentRun);
            if (!routeSurface.IsRouteComplete
                || routeSurface.IsRouteCleared
                || routeSurface.StatusLabel != "失败"
                || routeSurface.ProgressLabel != "进度：1/10"
                || !P0SettlementPresenter.HasP0FailedSettlementRows(settlement))
            {
                Fail("Forced defeat route map settlement surface is incomplete: "
                    + P0RouteMapPresenter.BuildCompactSummary(routeSurface)
                    + " | "
                    + P0SettlementPresenter.BuildCompactSummary(settlement)
                    + ".");
                yield break;
            }

            if (!routeSurface.TryGetAction(P0RouteMapActionIds.NewRun, out P0RouteMapAction newRun)
                || !newRun.IsEnabled)
            {
                Fail("Forced defeat settlement does not expose enabled New Run action.");
                yield break;
            }

            Add("Failed settlement rows verified: "
                + P0SettlementPresenter.BuildCompactSummary(settlement)
                + ".");

            string summary = "P0 play mode defeat flow smoke passed: "
                + P0SettlementPresenter.BuildCompactSummary(settlement)
                + ".";
            Add(summary);
            Complete(P0PlayModeDefeatFlowSmokeState.Passed, summary);
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
            Complete(P0PlayModeDefeatFlowSmokeState.Failed, message);
        }

        private void Complete(P0PlayModeDefeatFlowSmokeState state, string summary)
        {
            P0PlayModeDefeatFlowSmoke.Complete(state, summary, string.Join("\n", lines));
            Destroy(gameObject);
        }
    }
}
