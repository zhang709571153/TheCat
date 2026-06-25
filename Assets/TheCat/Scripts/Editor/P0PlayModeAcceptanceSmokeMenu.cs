using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0PlayModeAcceptanceSmokeMenu
    {
        private const string StartMenuPath = "TheCat/P0/Start Play Mode Acceptance Smoke";
        private const string LogMenuPath = "TheCat/P0/Log Play Mode Acceptance Smoke";

        [MenuItem(StartMenuPath, false, 84)]
        private static void StartPlayModeAcceptanceSmoke()
        {
            bool started = P0PlayModeAcceptanceSmoke.StartDefaultAcceptanceSmoke();
            if (started)
            {
                Debug.Log("[TheCat] Started P0 Play Mode Acceptance Smoke.");
                return;
            }

            Debug.LogError("[TheCat] Could not start P0 Play Mode Acceptance Smoke: " + P0PlayModeAcceptanceSmoke.LastSummary);
        }

        [MenuItem(StartMenuPath, true)]
        private static bool CanStartPlayModeAcceptanceSmoke()
        {
            return EditorApplication.isPlaying
                && !P0PlayModeAcceptanceSmoke.IsRunning
                && !P0PlayModeScreenshotSmoke.IsRunning
                && !P0PlayModeRouteFlowSmoke.IsRunning
                && !P0PlayModeDefeatFlowSmoke.IsRunning;
        }

        [MenuItem(LogMenuPath, false, 84)]
        private static void LogPlayModeAcceptanceSmoke()
        {
            string log = "[TheCat] P0 Play Mode Acceptance Smoke: " + P0PlayModeAcceptanceSmoke.LastDetailedLog;
            if (P0PlayModeAcceptanceSmoke.State == P0PlayModeAcceptanceSmokeState.Failed)
            {
                Debug.LogError(log);
                return;
            }

            Debug.Log(log);
        }
    }
}
