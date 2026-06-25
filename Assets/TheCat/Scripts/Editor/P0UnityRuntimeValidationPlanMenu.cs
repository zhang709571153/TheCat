using System;
using System.IO;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0UnityRuntimeValidationPlanMenu
    {
        private const string MenuPath = "TheCat/P0/Write P0 Unity Runtime Validation Plan";
        private const string OutputPath = "design/development/asset_review/P0_UNITY_RUNTIME_VALIDATION_PLAN.md";

        [MenuItem(MenuPath, false, 95)]
        private static void WriteP0UnityRuntimeValidationPlan()
        {
            try
            {
                P0UnityRuntimeValidationPlanReport report = P0UnityRuntimeValidationPlan.EvaluateCurrentPlan();
                string outputPath = ToProjectPath(OutputPath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllText(outputPath, report.BuildMarkdown());

                string log = "[TheCat] P0 Unity Runtime Validation Plan written to "
                    + outputPath
                    + Environment.NewLine
                    + report.BuildDetailedSummary();

                if (report.IsReady)
                {
                    Debug.LogWarning(log + Environment.NewLine + "Unity execution is still required before final P0 acceptance.");
                }
                else
                {
                    Debug.LogError(log);
                }

                EditorUtility.DisplayDialog(
                    report.IsReady
                        ? "P0 Runtime Validation Plan Ready"
                        : "P0 Runtime Validation Plan Failed",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Runtime Validation Plan Failed",
                    exception.GetType().Name + ": " + exception.Message,
                    "OK");
            }
        }

        private static string ToProjectPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(projectRoot, path.Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
