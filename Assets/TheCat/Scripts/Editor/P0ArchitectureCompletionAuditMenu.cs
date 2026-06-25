using System;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0ArchitectureCompletionAuditMenu
    {
        private const string MenuPath = "TheCat/P0/Run Architecture Completion Audit";

        [MenuItem(MenuPath, false, 87)]
        private static void RunArchitectureCompletionAudit()
        {
            try
            {
                P0ArchitectureCompletionAuditReport report = P0ArchitectureCompletionAudit.EvaluateCurrentProject();
                LogReport(report);

                string title = report.IsFinalP0PlayableComplete
                    ? "P0 Architecture Complete"
                    : report.IsReadyForSystematicAssetProduction
                        ? "P0 Architecture Ready, Unity Evidence Pending"
                        : "P0 Architecture Audit Failed";

                EditorUtility.DisplayDialog(title, report.BuildSummary(), "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Architecture Audit Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        private static void LogReport(P0ArchitectureCompletionAuditReport report)
        {
            string log = "[TheCat] P0 Architecture Completion Audit: " + report.BuildDetailedSummary();
            if (report.HasBlockingFailures)
            {
                Debug.LogError(log);
                return;
            }

            if (report.IsFinalP0PlayableComplete)
            {
                Debug.Log(log);
                return;
            }

            Debug.LogWarning(log);
        }
    }
}
