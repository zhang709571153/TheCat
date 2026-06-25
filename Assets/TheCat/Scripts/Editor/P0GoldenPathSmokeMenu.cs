using System;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0GoldenPathSmokeMenu
    {
        private const string MenuPath = "TheCat/P0/Run Golden Path Smoke";
        private static P0GoldenPathReport lastReport;

        [MenuItem(MenuPath, false, 100)]
        private static void RunGoldenPathSmoke()
        {
            try
            {
                lastReport = P0GoldenPathSimulator.SimulateDefaultRun();
                LogReport(lastReport);
                P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(lastReport);
                LogAcceptance(acceptance);

                string title = acceptance.IsAccepted
                    ? "P0 Golden Path Accepted"
                    : "P0 Golden Path Needs Attention";
                EditorUtility.DisplayDialog(title, lastReport.BuildSummary() + "\n" + acceptance.BuildSummary(), "OK");
            }
            catch (Exception exception)
            {
                lastReport = null;
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Golden Path Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        [MenuItem("TheCat/P0/Log Last Golden Path Report", false, 101)]
        private static void LogLastGoldenPathReport()
        {
            if (lastReport == null)
            {
                Debug.LogWarning("[TheCat] No golden path report has been generated in this editor session.");
                return;
            }

            LogReport(lastReport);
        }

        [MenuItem("TheCat/P0/Log Last Golden Path Report", true)]
        private static bool CanLogLastGoldenPathReport()
        {
            return lastReport != null;
        }

        private static void LogReport(P0GoldenPathReport report)
        {
            Debug.Log("[TheCat] P0 Golden Path: " + report.BuildSummary());
            for (int i = 0; i < report.BattleReports.Count; i++)
            {
                Debug.Log("[TheCat] P0 Golden Path Battle " + (i + 1) + ": "
                    + report.BattleReports[i].BuildSummary());
            }
        }

        private static void LogAcceptance(P0GoldenPathAcceptanceReport acceptance)
        {
            string log = "[TheCat] P0 Golden Path Acceptance: " + acceptance.BuildDetailedSummary();
            if (acceptance.IsAccepted)
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
