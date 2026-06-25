using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0PlayModeRouteFlowSmokeMenu
    {
        private const string StartMenuPath = "TheCat/P0/Start Play Mode Route Flow Smoke";
        private const string LogMenuPath = "TheCat/P0/Log Play Mode Route Flow Smoke";

        [MenuItem(StartMenuPath, false, 86)]
        private static void StartPlayModeRouteFlowSmoke()
        {
            bool started = P0PlayModeRouteFlowSmoke.StartDefaultRouteSmoke();
            if (started)
            {
                Debug.Log("[TheCat] Started P0 Play Mode Route Flow Smoke.");
                return;
            }

            Debug.LogError("[TheCat] Could not start P0 Play Mode Route Flow Smoke: " + P0PlayModeRouteFlowSmoke.LastSummary);
        }

        [MenuItem(StartMenuPath, true)]
        private static bool CanStartPlayModeRouteFlowSmoke()
        {
            return EditorApplication.isPlaying && !P0PlayModeRouteFlowSmoke.IsRunning;
        }

        [MenuItem(LogMenuPath, false, 86)]
        private static void LogPlayModeRouteFlowSmoke()
        {
            string log = "[TheCat] P0 Play Mode Route Flow Smoke: " + P0PlayModeRouteFlowSmoke.LastDetailedLog;
            if (P0PlayModeRouteFlowSmoke.State == P0PlayModeRouteFlowSmokeState.Failed)
            {
                Debug.LogError(log);
                return;
            }

            Debug.Log(log);
        }
    }
}
