using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0PlayModeScreenshotSmokeMenu
    {
        private const string StartMenuPath = "TheCat/P0/Start Play Mode Screenshot Smoke";
        private const string LogMenuPath = "TheCat/P0/Log Play Mode Screenshot Smoke";

        [MenuItem(StartMenuPath, false, 85)]
        private static void StartPlayModeScreenshotSmoke()
        {
            bool started = P0PlayModeScreenshotSmoke.StartDefaultScreenshotSmoke();
            if (started)
            {
                Debug.Log("[TheCat] Started P0 Play Mode Screenshot Smoke.");
                return;
            }

            Debug.LogError("[TheCat] Could not start P0 Play Mode Screenshot Smoke: " + P0PlayModeScreenshotSmoke.LastSummary);
        }

        [MenuItem(StartMenuPath, true)]
        private static bool CanStartPlayModeScreenshotSmoke()
        {
            return EditorApplication.isPlaying && !P0PlayModeScreenshotSmoke.IsRunning;
        }

        [MenuItem(LogMenuPath, false, 85)]
        private static void LogPlayModeScreenshotSmoke()
        {
            string log = "[TheCat] P0 Play Mode Screenshot Smoke: " + P0PlayModeScreenshotSmoke.LastDetailedLog;
            if (P0PlayModeScreenshotSmoke.State == P0PlayModeScreenshotSmokeState.Failed)
            {
                Debug.LogError(log);
                return;
            }

            Debug.Log(log);
        }
    }
}
