using System;
using System.IO;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0AssetSystematicProductionPlanMenu
    {
        [MenuItem(P0AssetSystematicProductionPlan.EditorMenuPath, false, 96)]
        private static void WriteP0SystematicAssetProductionPlan()
        {
            try
            {
                P0AssetSystematicProductionPlanReport report = P0AssetSystematicProductionPlan.EvaluateCurrentPlan();
                string outputPath = ToProjectPath(P0AssetSystematicProductionPlan.ReportOutputPath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllText(outputPath, report.BuildMarkdown());

                string log = "[TheCat] P0 Systematic Asset Production Plan written to "
                    + outputPath
                    + Environment.NewLine
                    + report.BuildDetailedSummary();

                if (report.IsReady)
                {
                    Debug.LogWarning(log + Environment.NewLine + "Starter-cat body art remains locked until active-cat screenshot approval.");
                }
                else
                {
                    Debug.LogError(log);
                }

                EditorUtility.DisplayDialog(
                    report.IsReady
                        ? "P0 Asset Plan Ready"
                        : "P0 Asset Plan Failed",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Asset Plan Failed",
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
