using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public readonly struct P0BatchmodeGateResult
    {
        public P0BatchmodeGateResult(string title, bool passed, string detail)
        {
            Title = title ?? string.Empty;
            Passed = passed;
            Detail = detail ?? string.Empty;
        }

        public string Title { get; }

        public bool Passed { get; }

        public string Detail { get; }
    }

    public sealed class P0BatchmodeAcceptanceReport
    {
        private readonly List<P0BatchmodeGateResult> gates = new List<P0BatchmodeGateResult>();

        public IReadOnlyList<P0BatchmodeGateResult> Gates => gates.AsReadOnly();

        public int FailureCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < gates.Count; i++)
                {
                    if (!gates[i].Passed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool IsPassed => FailureCount == 0;

        public void AddGate(string title, bool passed, string detail)
        {
            gates.Add(new P0BatchmodeGateResult(title, passed, detail));
        }

        public string BuildSummary()
        {
            return IsPassed
                ? "P0 batchmode acceptance passed " + gates.Count + " gate(s)."
                : "P0 batchmode acceptance failed " + FailureCount + " of " + gates.Count + " gate(s).";
        }

        public string BuildMarkdown(string title)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# " + title);
            builder.AppendLine();
            builder.AppendLine("- Result: " + (IsPassed ? "passed" : "failed"));
            builder.AppendLine("- Gate count: " + gates.Count);
            builder.AppendLine("- Failure count: " + FailureCount);
            builder.AppendLine();

            for (int i = 0; i < gates.Count; i++)
            {
                P0BatchmodeGateResult gate = gates[i];
                builder.AppendLine("## " + gate.Title);
                builder.AppendLine();
                builder.AppendLine("- Passed: " + (gate.Passed ? "yes" : "no"));
                builder.AppendLine();
                builder.AppendLine("```text");
                builder.AppendLine(gate.Detail);
                builder.AppendLine("```");
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }

    public static class P0BatchmodeAcceptanceRunner
    {
        private const string OutputFolder = "design/development/unity_batchmode";
        private const string OfflineReportPath = OutputFolder + "/P0_OFFLINE_ACCEPTANCE_REPORT.md";
        private const string FullReportPath = OutputFolder + "/P0_FULL_ACCEPTANCE_REPORT.md";
        private const string AssetReviewPacketPath = "design/development/asset_review/P0_RUNTIME_VISUAL_REVIEW_PACKET.md";
        private const string FormalInstallMatrixPath = "design/development/asset_review/P0_FORMAL_INSTALL_BLOCKER_EVIDENCE_MATRIX_2026-06-26.md";

        public static void RunOfflineP0GatesForBatchmode()
        {
            RunAndExit(includePlayModeEvidence: false, reportPath: OfflineReportPath, title: "P0 Offline Acceptance Report");
        }

        public static void RunFullP0AcceptanceForBatchmode()
        {
            RunAndExit(includePlayModeEvidence: true, reportPath: FullReportPath, title: "P0 Full Acceptance Report");
        }

        public static P0BatchmodeAcceptanceReport EvaluateOfflineP0Gates()
        {
            return Evaluate(includePlayModeEvidence: false);
        }

        public static P0BatchmodeAcceptanceReport EvaluateFullP0Acceptance()
        {
            return Evaluate(includePlayModeEvidence: true);
        }

        public static P0BatchmodeAcceptanceReport Evaluate(bool includePlayModeEvidence)
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

            P0BatchmodeAcceptanceReport report = new P0BatchmodeAcceptanceReport();

            P0CodeSmokeSuiteReport codeSmoke = P0CodeSmokeSuite.EvaluatePrototypeBuild();
            report.AddGate("P0 Code Smoke Suite", codeSmoke.IsPassed, codeSmoke.BuildDetailedSummary());

            P0PlayableReadinessReport readiness = P0PlayableReadiness.EvaluatePrototypeBuild();
            report.AddGate("P0 Playable Readiness", readiness.IsReady, readiness.BuildDetailedSummary());

            P0SceneValidationReport sceneSetup = P0SceneSetupValidator.Validate(deepSceneInspection: true);
            report.AddGate("P0 Scene Setup", sceneSetup.IsValid, sceneSetup.BuildDetailedLog());

            P0AssetImportSettingsReport assetImports = P0AssetImportSettingsValidator.ValidateP0Manifest(refreshAssetDatabase: false);
            report.AddGate("P0 Asset Imports", assetImports.IsValid, assetImports.BuildDetailedLog());

            P0AssetReviewPacketReport assetReview = P0AssetReviewPacket.EvaluateP0Packet();
            report.AddGate("P0 Asset Review Packet", assetReview.IsReady, assetReview.BuildDetailedSummary());
            WriteAssetReviewPacket(assetReview);

            P0AssetProductionReadinessReport assetProduction = P0AssetProductionReadiness.EvaluateP0OfflineReadiness();
            report.AddGate("P0 Offline Asset Production Readiness", assetProduction.IsReady, assetProduction.BuildDetailedSummary());

            P0FormalInstallGateReport formalInstallGate = P0FormalInstallGate.EvaluateCurrentGate();
            report.AddGate("P0 Formal Install Gate Matrix", formalInstallGate.IsGateValid && formalInstallGate.IsFormalInstallBlocked, formalInstallGate.BuildDetailedSummary());
            WriteReport(FormalInstallMatrixPath, formalInstallGate.BuildMarkdown());

            if (includePlayModeEvidence)
            {
                P0PlayModeEvidenceReport playModeEvidence = P0PlayModeEvidenceChecklist.EvaluateCurrent();
                report.AddGate("P0 Play Mode Evidence", playModeEvidence.IsComplete, playModeEvidence.BuildDetailedSummary());
            }

            return report;
        }

        private static void RunAndExit(bool includePlayModeEvidence, string reportPath, string title)
        {
            try
            {
                P0BatchmodeAcceptanceReport report = Evaluate(includePlayModeEvidence);
                WriteReport(reportPath, report.BuildMarkdown(title));
                LogReport(report);
                EditorApplication.Exit(report.IsPassed ? 0 : 1);
            }
            catch (Exception exception)
            {
                WriteReport(
                    reportPath,
                    "# " + title + Environment.NewLine
                    + Environment.NewLine
                    + "- Result: exception" + Environment.NewLine
                    + "- Exception: " + exception.GetType().Name + ": " + exception.Message + Environment.NewLine
                    + Environment.NewLine
                    + "```text" + Environment.NewLine
                    + exception + Environment.NewLine
                    + "```" + Environment.NewLine);
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }

        private static void WriteAssetReviewPacket(P0AssetReviewPacketReport assetReview)
        {
            WriteReport(AssetReviewPacketPath, assetReview.BuildMarkdown());
        }

        private static void WriteReport(string path, string content)
        {
            string outputPath = ToProjectPath(path);
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(outputPath, content ?? string.Empty);
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

        private static void LogReport(P0BatchmodeAcceptanceReport report)
        {
            string message = "[TheCat] " + report.BuildSummary();
            if (report.IsPassed)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }
    }
}
