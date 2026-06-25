using System;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0StatusTagCoverageMenu
    {
        private const string MenuPath = "TheCat/P0/Log Status Tag Coverage";

        [MenuItem(MenuPath, false, 102)]
        private static void LogStatusTagCoverage()
        {
            try
            {
                P0StatusTagCoverageReport report = P0StatusTagCoverage.EvaluatePrototypeCatalog();
                LogReport(report);

                EditorUtility.DisplayDialog(
                    report.IsComplete ? "P0 Status Tags Covered" : "P0 Status Tags Need Attention",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Status Tag Coverage Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        private static void LogReport(P0StatusTagCoverageReport report)
        {
            string log = "[TheCat] P0 Status Tag Coverage: " + report.BuildDetailedSummary();
            if (report.IsComplete)
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
