using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0PlayModeDefeatFlowSmokeMenu
    {
        private const string StartMenuPath = "TheCat/P0/Start Play Mode Defeat Flow Smoke";
        private const string LogMenuPath = "TheCat/P0/Log Play Mode Defeat Flow Smoke";

        [MenuItem(StartMenuPath, false, 86)]
        private static void StartPlayModeDefeatFlowSmoke()
        {
            bool started = P0PlayModeDefeatFlowSmoke.StartDefaultDefeatSmoke();
            if (started)
            {
                Debug.Log("[TheCat] Started P0 Play Mode Defeat Flow Smoke.");
                return;
            }

            Debug.LogError("[TheCat] Could not start P0 Play Mode Defeat Flow Smoke: " + P0PlayModeDefeatFlowSmoke.LastSummary);
        }

        [MenuItem(StartMenuPath, true)]
        private static bool CanStartPlayModeDefeatFlowSmoke()
        {
            return EditorApplication.isPlaying && !P0PlayModeDefeatFlowSmoke.IsRunning;
        }

        [MenuItem(LogMenuPath, false, 86)]
        private static void LogPlayModeDefeatFlowSmoke()
        {
            string log = "[TheCat] P0 Play Mode Defeat Flow Smoke: " + P0PlayModeDefeatFlowSmoke.LastDetailedLog;
            if (P0PlayModeDefeatFlowSmoke.State == P0PlayModeDefeatFlowSmokeState.Failed)
            {
                Debug.LogError(log);
                return;
            }

            Debug.Log(log);
        }
    }
}
