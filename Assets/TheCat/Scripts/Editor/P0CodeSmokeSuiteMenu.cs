using System;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0CodeSmokeSuiteMenu
    {
        private const string MenuPath = "TheCat/P0/Run Code Smoke Suite";

        [MenuItem(MenuPath, false, 88)]
        private static void RunCodeSmokeSuite()
        {
            try
            {
                P0CodeSmokeSuiteReport report = P0CodeSmokeSuite.EvaluatePrototypeBuild();
                LogReport(report);

                EditorUtility.DisplayDialog(
                    report.IsPassed ? "P0 Code Smoke Suite Passed" : "P0 Code Smoke Suite Failed",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Code Smoke Suite Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        private static void LogReport(P0CodeSmokeSuiteReport report)
        {
            string log = "[TheCat] P0 Code Smoke Suite: " + report.BuildDetailedSummary();
            if (report.IsPassed)
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
