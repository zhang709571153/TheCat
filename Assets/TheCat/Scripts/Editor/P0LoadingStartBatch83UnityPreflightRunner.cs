using System;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public static class P0LoadingStartBatch83UnityPreflightRunner
    {
        private const string MenuPath = "TheCat/P0/Batch 83 Loading Start Unity Preflight";

        [MenuItem(MenuPath, false, 91)]
        private static void RunFromMenu()
        {
            P0LoadingStartBatch83UnityPreflightReport report = RunPreflight(writeReport: true);
            EditorUtility.DisplayDialog(
                report.IsReadyForUnityPreflight
                    ? "Batch 83 Loading Start Preflight Ready"
                    : "Batch 83 Loading Start Preflight Needs Attention",
                report.BuildSummary(),
                "OK");
        }

        public static void RunForBatchmode()
        {
            try
            {
                P0LoadingStartBatch83UnityPreflightReport report = RunPreflight(writeReport: true);
                EditorApplication.Exit(report.IsReadyForUnityPreflight ? 0 : 1);
            }
            catch (Exception exception)
            {
                WriteReport(
                    "# Batch 83 Loading Start Unity Preflight"
                    + Environment.NewLine
                    + Environment.NewLine
                    + "- Result: exception"
                    + Environment.NewLine
                    + "- Exception: " + exception.GetType().Name + ": " + exception.Message
                    + Environment.NewLine
                    + Environment.NewLine
                    + "```text"
                    + Environment.NewLine
                    + exception
                    + Environment.NewLine
                    + "```"
                    + Environment.NewLine);
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }

        public static P0LoadingStartBatch83UnityPreflightReport RunPreflight(bool writeReport)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            P0AssetImportApplyReport applyReport = P0AssetImportSettingsApplier.Apply(
                P0LoadingStartBatch83CandidateCatalog.CreateUnityPreflightManifest(),
                reimportChanged: true);
            LogApplyReport(applyReport);

            P0AssetImportSettingsReport validationReport = P0AssetImportSettingsValidator.Validate(
                P0LoadingStartBatch83CandidateCatalog.CreateUnityPreflightManifest());
            LogValidationReport(validationReport);

            P0LoadingStartBatch83UnityPreflightReport report =
                P0LoadingStartBatch83UnityPreflight.EvaluateCurrentPreflight(
                    unityEditorImportValidationReady: applyReport.IsApplied && validationReport.IsValid);
            LogPreflightReport(report);

            if (writeReport)
            {
                WriteReport(report.BuildMarkdown());
            }

            return report;
        }

        private static void LogApplyReport(P0AssetImportApplyReport report)
        {
            string message = "[TheCat] Batch 83 import apply: " + report.BuildDetailedLog();
            if (report.IsApplied)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogValidationReport(P0AssetImportSettingsReport report)
        {
            string message = "[TheCat] Batch 83 import validation: " + report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogPreflightReport(P0LoadingStartBatch83UnityPreflightReport report)
        {
            string message = "[TheCat] Batch 83 loading/start preflight: " + report.BuildDetailedSummary();
            if (report.IsReadyForUnityPreflight)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void WriteReport(string markdown)
        {
            string path = ToProjectPath(P0LoadingStartBatch83UnityPreflight.ReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, markdown ?? string.Empty);
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
