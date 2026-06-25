using System;
using TheCat.Roguelite;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0GrayboxTelemetryMenu
    {
        [MenuItem("TheCat/P0/Run Golden Path Telemetry", false, 103)]
        private static void RunGoldenPathTelemetry()
        {
            try
            {
                LogReport(P0GrayboxTelemetry.EvaluateGoldenPath());
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Graybox Telemetry Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        [MenuItem("TheCat/P0/Log Current Run Telemetry", false, 104)]
        private static void LogCurrentRunTelemetry()
        {
            if (P0RunSession.CurrentRun == null)
            {
                Debug.LogWarning("[TheCat] No current P0 run is available for telemetry.");
                return;
            }

            LogReport(P0GrayboxTelemetry.Evaluate(P0RunSession.CurrentRun));
        }

        [MenuItem("TheCat/P0/Log Current Run Telemetry", true)]
        private static bool CanLogCurrentRunTelemetry()
        {
            return P0RunSession.CurrentRun != null;
        }

        private static void LogReport(P0GrayboxTelemetryReport report)
        {
            string log = "[TheCat] P0 Graybox Telemetry: " + report.BuildDetailedSummary();
            if (report.IsUsable)
            {
                Debug.Log(log);
            }
            else
            {
                Debug.LogError(log);
            }

            EditorUtility.DisplayDialog(
                report.IsUsable ? "P0 Graybox Telemetry Captured" : "P0 Graybox Telemetry Needs Attention",
                report.BuildSummary(),
                "OK");
        }
    }
}
