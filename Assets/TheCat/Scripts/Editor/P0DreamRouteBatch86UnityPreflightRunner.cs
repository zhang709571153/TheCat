using System;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public static class P0DreamRouteBatch86UnityPreflightRunner
    {
        private const string MenuPath = "TheCat/P0/Batch 86 Dream Route Unity Preflight";

        [MenuItem(MenuPath, false, 92)]
        private static void RunFromMenu()
        {
            P0DreamRouteBatch86UnityPreflightReport report = RunPreflight(writeReport: true);
            EditorUtility.DisplayDialog(
                report.IsReadyForUnityPreflight
                    ? "Batch 86 Dream Route Preflight Ready"
                    : "Batch 86 Dream Route Preflight Needs Attention",
                report.BuildSummary(),
                "OK");
        }

        public static void RunForBatchmode()
        {
            try
            {
                P0DreamRouteBatch86UnityPreflightReport report = RunPreflight(writeReport: true);
                EditorApplication.Exit(report.IsReadyForUnityPreflight ? 0 : 1);
            }
            catch (Exception exception)
            {
                WriteReport(
                    "# Batch 86 Dream Route Unity Preflight"
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

        public static P0DreamRouteBatch86UnityPreflightReport RunPreflight(bool writeReport)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            P0AssetImportApplyReport applyReport = P0AssetImportSettingsApplier.Apply(
                P0DreamRouteBatch86CandidateCatalog.CreateUnityPreflightManifest(),
                reimportChanged: true);
            LogApplyReport(applyReport);

            P0AssetImportSettingsReport validationReport = P0AssetImportSettingsValidator.Validate(
                P0DreamRouteBatch86CandidateCatalog.CreateUnityPreflightManifest());
            LogValidationReport(validationReport);

            P0DreamRouteBatch86UnityPreflightReport report =
                P0DreamRouteBatch86UnityPreflight.EvaluateCurrentPreflight(
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
            string message = "[TheCat] Batch 86 import apply: " + report.BuildDetailedLog();
            if (report.IsApplied)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogValidationReport(P0AssetImportSettingsReport report)
        {
            string message = "[TheCat] Batch 86 import validation: " + report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogPreflightReport(P0DreamRouteBatch86UnityPreflightReport report)
        {
            string message = "[TheCat] Batch 86 dream-route preflight: " + report.BuildDetailedSummary();
            if (report.IsReadyForUnityPreflight)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void WriteReport(string markdown)
        {
            string path = ToProjectPath(P0DreamRouteBatch86UnityPreflight.ReportPath);
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

