using System;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public static class P0SettingsPauseBatch85UnityPreflightRunner
    {
        private const string MenuPath = "TheCat/P0/Batch 85 Settings Pause Unity Preflight";

        [MenuItem(MenuPath, false, 92)]
        private static void RunFromMenu()
        {
            P0SettingsPauseBatch85UnityPreflightReport report = RunPreflight(writeReport: true);
            EditorUtility.DisplayDialog(
                report.IsReadyForUnityPreflight
                    ? "Batch 85 Settings Pause Preflight Ready"
                    : "Batch 85 Settings Pause Preflight Needs Attention",
                report.BuildSummary(),
                "OK");
        }

        public static void RunForBatchmode()
        {
            try
            {
                P0SettingsPauseBatch85UnityPreflightReport report = RunPreflight(writeReport: true);
                EditorApplication.Exit(report.IsReadyForUnityPreflight ? 0 : 1);
            }
            catch (Exception exception)
            {
                WriteReport(
                    "# Batch 85 Settings Pause Unity Preflight"
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

        public static P0SettingsPauseBatch85UnityPreflightReport RunPreflight(bool writeReport)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            P0AssetImportApplyReport applyReport = P0AssetImportSettingsApplier.Apply(
                P0SettingsPauseBatch85CandidateCatalog.CreateUnityPreflightManifest(),
                reimportChanged: true);
            LogApplyReport(applyReport);

            P0AssetImportSettingsReport validationReport = P0AssetImportSettingsValidator.Validate(
                P0SettingsPauseBatch85CandidateCatalog.CreateUnityPreflightManifest());
            LogValidationReport(validationReport);

            P0SettingsPauseBatch85UnityPreflightReport report =
                P0SettingsPauseBatch85UnityPreflight.EvaluateCurrentPreflight(
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
            string message = "[TheCat] Batch 85 import apply: " + report.BuildDetailedLog();
            if (report.IsApplied)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogValidationReport(P0AssetImportSettingsReport report)
        {
            string message = "[TheCat] Batch 85 import validation: " + report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogPreflightReport(P0SettingsPauseBatch85UnityPreflightReport report)
        {
            string message = "[TheCat] Batch 85 settings/pause preflight: " + report.BuildDetailedSummary();
            if (report.IsReadyForUnityPreflight)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void WriteReport(string markdown)
        {
            string path = ToProjectPath(P0SettingsPauseBatch85UnityPreflight.ReportPath);
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

