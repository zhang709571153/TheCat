using System;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public static class P0BattleHudBatch87UnityPreflightRunner
    {
        private const string MenuPath = "TheCat/P0/Batch 87 Battle HUD Unity Preflight";

        [MenuItem(MenuPath, false, 93)]
        private static void RunFromMenu()
        {
            P0BattleHudBatch87UnityPreflightReport report = RunPreflight(writeReport: true);
            EditorUtility.DisplayDialog(
                report.IsReadyForUnityPreflight
                    ? "Batch 87 Battle HUD Preflight Ready"
                    : "Batch 87 Battle HUD Preflight Needs Attention",
                report.BuildSummary(),
                "OK");
        }

        public static void RunForBatchmode()
        {
            try
            {
                P0BattleHudBatch87UnityPreflightReport report = RunPreflight(writeReport: true);
                EditorApplication.Exit(report.IsReadyForUnityPreflight ? 0 : 1);
            }
            catch (Exception exception)
            {
                WriteReport(
                    "# Batch 87 Battle HUD Unity Preflight"
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

        public static P0BattleHudBatch87UnityPreflightReport RunPreflight(bool writeReport)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            P0AssetImportApplyReport applyReport = P0AssetImportSettingsApplier.Apply(
                P0BattleHudBatch87CandidateCatalog.CreateUnityPreflightManifest(),
                reimportChanged: true);
            LogApplyReport(applyReport);

            P0AssetImportSettingsReport validationReport = P0AssetImportSettingsValidator.Validate(
                P0BattleHudBatch87CandidateCatalog.CreateUnityPreflightManifest());
            LogValidationReport(validationReport);

            P0BattleHudBatch87UnityPreflightReport report =
                P0BattleHudBatch87UnityPreflight.EvaluateCurrentPreflight(
                    unityEditorImportValidationReady: applyReport.IsApplied && validationReport.IsValid);
            LogPreflightReport(report);

            if (writeReport)
            {
                WriteReport(P0BattleHudBatch87UnityPreflight.ReportPath, report.BuildMarkdown());
                WriteReport(P0BattleHudBatch87UnityPreflight.HumanReviewRequestPath, report.BuildHumanReviewRequestMarkdown());
                WriteReport(P0BattleHudBatch87UnityPreflight.FormalRuntimeBindingDecisionRequestPath, report.BuildFormalRuntimeBindingDecisionRequestMarkdown());
            }

            return report;
        }

        private static void LogApplyReport(P0AssetImportApplyReport report)
        {
            string message = "[TheCat] Batch 87 import apply: " + report.BuildDetailedLog();
            if (report.IsApplied)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogValidationReport(P0AssetImportSettingsReport report)
        {
            string message = "[TheCat] Batch 87 import validation: " + report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogPreflightReport(P0BattleHudBatch87UnityPreflightReport report)
        {
            string message = "[TheCat] Batch 87 battle HUD preflight: " + report.BuildDetailedSummary();
            if (report.IsReadyForUnityPreflight)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void WriteReport(string markdown)
        {
            WriteReport(P0BattleHudBatch87UnityPreflight.ReportPath, markdown);
        }

        private static void WriteReport(string relativePath, string markdown)
        {
            string path = ToProjectPath(relativePath);
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
