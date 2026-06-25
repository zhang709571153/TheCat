using System;
using System.IO;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0AssetUnityValidationChecklistMenu
    {
        private const string MenuPath = "TheCat/P0/Write P0 Asset Unity Validation Checklist";
        private const string OutputPath = "design/development/asset_review/P0_ASSET_UNITY_VALIDATION_CHECKLIST.md";

        [MenuItem(MenuPath, false, 94)]
        private static void WriteP0AssetUnityValidationChecklist()
        {
            try
            {
                P0AssetUnityValidationChecklistReport report = P0AssetUnityValidationChecklist.EvaluateCurrentQueue();
                string outputPath = ToProjectPath(OutputPath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllText(outputPath, report.BuildMarkdown());

                string log = "[TheCat] P0 Asset Unity Validation Checklist written to " + outputPath + Environment.NewLine + report.BuildDetailedSummary();
                if (report.IsReadyForUnityValidation)
                {
                    Debug.Log(log);
                }
                else
                {
                    Debug.LogError(log);
                }

                EditorUtility.DisplayDialog(
                    report.IsReadyForUnityValidation ? "P0 Asset Unity Checklist Ready" : "P0 Asset Unity Checklist Failed",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Asset Unity Checklist Failed",
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
