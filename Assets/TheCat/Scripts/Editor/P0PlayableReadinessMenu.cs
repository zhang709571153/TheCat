using System;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0PlayableReadinessMenu
    {
        private const string MenuPath = "TheCat/P0/Run Playable Readiness";

        [MenuItem(MenuPath, false, 89)]
        private static void RunPlayableReadiness()
        {
            try
            {
                P0PlayableReadinessReport report = P0PlayableReadiness.EvaluatePrototypeBuild();
                LogReport(report);

                EditorUtility.DisplayDialog(
                    report.IsReady ? "P0 Playable Readiness Passed" : "P0 Playable Readiness Failed",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Playable Readiness Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        private static void LogReport(P0PlayableReadinessReport report)
        {
            string log = "[TheCat] P0 Playable Readiness: " + report.BuildDetailedSummary();
            if (report.IsReady)
            {
                Debug.Log(log);
            }
            else
            {
                Debug.LogError(log);
            }
        }
    }
}
