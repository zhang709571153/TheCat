using System;
using System.IO;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0VisualAcceptanceReportMenu
    {
        private const string MenuPath = "TheCat/P0/Write P0 Visual Acceptance Report";
        private const string OutputPath = "design/development/asset_review/P0_VISUAL_ACCEPTANCE_REPORT.md";

        [MenuItem(MenuPath, false, 94)]
        private static void WriteP0VisualAcceptanceReport()
        {
            try
            {
                P0VisualAcceptanceReport report = P0VisualAcceptance.EvaluateCurrent();
                string outputPath = ToProjectPath(OutputPath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                File.WriteAllText(outputPath, report.BuildMarkdown());

                string log = "[TheCat] P0 Visual Acceptance Report written to " + outputPath + Environment.NewLine + report.BuildSummary();
                if (report.IsArchitectureReadyForSystematicAssetProduction)
                {
                    Debug.Log(log);
                }
                else
                {
                    Debug.LogError(log);
                }

                EditorUtility.DisplayDialog(
                    report.IsArchitectureReadyForSystematicAssetProduction
                        ? "P0 Visual Architecture Ready"
                        : "P0 Visual Architecture Needs Attention",
                    report.BuildSummary(),
                    "OK");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog(
                    "P0 Visual Acceptance Report Failed",
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
