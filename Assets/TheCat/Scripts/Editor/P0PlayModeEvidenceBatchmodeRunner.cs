using System;
using System.IO;
using System.Text;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public static class P0PlayModeEvidenceBatchmodeRunner
    {
        private const double EnterPlayModeTimeoutSeconds = 45d;
        private const double AcceptanceSmokeTimeoutSeconds = 260d;
        private const string OutputFolder = "design/development/unity_batchmode";
        private const string ReportPath = OutputFolder + "/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md";
        private const string FullReportPath = OutputFolder + "/P0_FULL_ACCEPTANCE_REPORT.md";
        private const string CommandLineEntryPoint = "TheCat.EditorTools.P0PlayModeEvidenceBatchmodeRunner.RunPlayModeAcceptanceSmokeForBatchmode";

        private static double stageStartedAt;
        private static bool smokeStarted;
        private static bool exitRequested;
        private static bool hasStarted;

        [InitializeOnLoadMethod]
        private static void StartFromCommandLineAfterReload()
        {
            if (!IsCommandLineEntryPointRequested())
            {
                return;
            }

            EditorApplication.delayCall -= RunPlayModeAcceptanceSmokeForBatchmode;
            EditorApplication.delayCall += RunPlayModeAcceptanceSmokeForBatchmode;
        }

        public static void RunPlayModeAcceptanceSmokeForBatchmode()
        {
            try
            {
                if (hasStarted)
                {
                    return;
                }

                hasStarted = true;
                Debug.Log("[TheCat] P0 Play Mode evidence batchmode runner invoked.");
                ResetState();
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
                EditorApplication.update += Update;

                if (EditorApplication.isPlaying)
                {
                    StartSmoke();
                    return;
                }

                stageStartedAt = EditorApplication.timeSinceStartup;
                Debug.Log("[TheCat] Entering Play Mode for P0 acceptance smoke.");
                EditorApplication.EnterPlaymode();
            }
            catch (Exception exception)
            {
                ExitWithException(exception);
            }
        }

        private static void ResetState()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.update -= Update;
            stageStartedAt = EditorApplication.timeSinceStartup;
            smokeStarted = false;
            exitRequested = false;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            Debug.Log("[TheCat] Play Mode state changed: " + change + ".");
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                StartSmoke();
            }
        }

        private static void Update()
        {
            if (exitRequested)
            {
                return;
            }

            try
            {
                double elapsed = EditorApplication.timeSinceStartup - stageStartedAt;
                if (!smokeStarted)
                {
                    if (elapsed > EnterPlayModeTimeoutSeconds)
                    {
                        ExitWithFailure("Timed out entering Play Mode before starting P0 acceptance smoke.");
                    }

                    return;
                }

                if (P0PlayModeAcceptanceSmoke.IsFinished)
                {
                    ExitWithSmokeResult();
                    return;
                }

                if (elapsed > AcceptanceSmokeTimeoutSeconds)
                {
                    ExitWithFailure("Timed out waiting for P0 Play Mode acceptance smoke to finish.");
                }
            }
            catch (Exception exception)
            {
                ExitWithException(exception);
            }
        }

        private static void StartSmoke()
        {
            if (exitRequested || smokeStarted)
            {
                return;
            }

            stageStartedAt = EditorApplication.timeSinceStartup;
            smokeStarted = true;

            string screenshotDirectory = ToProjectPath(P0PlayModeScreenshotFileEvidence.DefaultScreenshotDirectory);
            Debug.Log("[TheCat] Starting P0 Play Mode acceptance smoke. Screenshot output: " + screenshotDirectory);
            bool started = P0PlayModeAcceptanceSmoke.StartDefaultAcceptanceSmoke(screenshotDirectory);
            if (!started)
            {
                ExitWithFailure("Could not start P0 Play Mode acceptance smoke: " + P0PlayModeAcceptanceSmoke.LastSummary);
            }
        }

        private static void ExitWithSmokeResult()
        {
            P0PlayModeEvidenceReport evidence = P0PlayModeEvidenceChecklist.EvaluateCurrent();
            bool passed = P0PlayModeAcceptanceSmoke.State == P0PlayModeAcceptanceSmokeState.Passed
                && evidence.IsComplete;
            string title = "P0 Play Mode Acceptance Smoke Report";
            string markdown = BuildReportMarkdown(title, passed, null);
            WriteReport(ReportPath, markdown);
            WriteFullAcceptanceDeferredReport(evidence);
            CleanupAndExit(passed, P0PlayModeAcceptanceSmoke.LastSummary);
        }

        private static void ExitWithFailure(string message)
        {
            string title = "P0 Play Mode Acceptance Smoke Report";
            string markdown = BuildReportMarkdown(title, false, message);
            WriteReport(ReportPath, markdown);
            CleanupAndExit(false, message);
        }

        private static void ExitWithException(Exception exception)
        {
            string title = "P0 Play Mode Acceptance Smoke Report";
            string markdown = BuildReportMarkdown(
                title,
                false,
                exception.GetType().Name + ": " + exception.Message + Environment.NewLine + exception);
            WriteReport(ReportPath, markdown);
            Debug.LogException(exception);
            CleanupAndExit(false, exception.Message);
        }

        private static string BuildReportMarkdown(
            string title,
            bool passed,
            string failureMessage)
        {
            P0PlayModeEvidenceReport evidence = P0PlayModeEvidenceChecklist.EvaluateCurrent();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# " + title);
            builder.AppendLine();
            builder.AppendLine("- Result: " + (passed ? "passed" : "failed"));
            builder.AppendLine("- Smoke state: " + P0PlayModeAcceptanceSmoke.State);
            builder.AppendLine("- Screenshot output: " + P0PlayModeScreenshotSmoke.LastOutputDirectory);
            builder.AppendLine("- Evidence summary: " + evidence.BuildSummary());
            if (!string.IsNullOrWhiteSpace(failureMessage))
            {
                builder.AppendLine("- Failure: " + failureMessage.Replace(Environment.NewLine, " "));
            }

            builder.AppendLine();
            AppendSection(builder, "Acceptance Smoke", P0PlayModeAcceptanceSmoke.LastDetailedLog);
            AppendSection(builder, "Screenshot Smoke", P0PlayModeScreenshotSmoke.LastDetailedLog);
            AppendSection(builder, "Route Flow Smoke", P0PlayModeRouteFlowSmoke.LastDetailedLog);
            AppendSection(builder, "Defeat Flow Smoke", P0PlayModeDefeatFlowSmoke.LastDetailedLog);
            AppendSection(builder, "Play Mode Evidence", evidence.BuildDetailedSummary());
            return builder.ToString();
        }

        private static void WriteFullAcceptanceDeferredReport(P0PlayModeEvidenceReport evidence)
        {
            WriteReport(
                FullReportPath,
                "# P0 Full Acceptance Report" + Environment.NewLine
                + Environment.NewLine
                + "- Result: not evaluated by Play Mode smoke runner" + Environment.NewLine
                + "- Reason: offline scene/import gates require Edit Mode, while smoke completion state is only authoritative inside this Play Mode process." + Environment.NewLine
                + "- Current Play Mode evidence: " + evidence.BuildSummary() + Environment.NewLine
                + "- Source report: " + ReportPath + Environment.NewLine
                + Environment.NewLine
                + "Use this Play Mode report together with the offline acceptance report until a persisted Play Mode evidence merge is implemented." + Environment.NewLine);
        }

        private static void AppendSection(StringBuilder builder, string title, string detail)
        {
            builder.AppendLine("## " + title);
            builder.AppendLine();
            builder.AppendLine("```text");
            builder.AppendLine(string.IsNullOrWhiteSpace(detail) ? "(empty)" : detail);
            builder.AppendLine("```");
            builder.AppendLine();
        }

        private static void CleanupAndExit(bool passed, string message)
        {
            if (exitRequested)
            {
                return;
            }

            exitRequested = true;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.update -= Update;

            string logMessage = "[TheCat] P0 Play Mode acceptance batchmode " + (passed ? "passed: " : "failed: ") + message;
            if (passed)
            {
                Debug.Log(logMessage);
            }
            else
            {
                Debug.LogError(logMessage);
            }

            EditorApplication.Exit(passed ? 0 : 1);
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

        private static bool IsCommandLineEntryPointRequested()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], CommandLineEntryPoint, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class P0PlayModeEgyptEntryEvidenceBatchmodeRunner
    {
        private const double EnterPlayModeTimeoutSeconds = 45d;
        private const double EgyptEntrySmokeTimeoutSeconds = 90d;
        private const string OutputFolder = "design/development/unity_batchmode";
        private const string ReportPath = OutputFolder + "/P0_EGYPT_ENTRY_SMOKE_REPORT.md";
        private const string CommandLineEntryPoint = "TheCat.EditorTools.P0PlayModeEgyptEntryEvidenceBatchmodeRunner.RunEgyptEntrySmokeForBatchmode";

        private static double stageStartedAt;
        private static bool smokeStarted;
        private static bool exitRequested;
        private static bool hasStarted;

        [InitializeOnLoadMethod]
        private static void StartFromCommandLineAfterReload()
        {
            if (!IsCommandLineEntryPointRequested())
            {
                return;
            }

            EditorApplication.delayCall -= RunEgyptEntrySmokeForBatchmode;
            EditorApplication.delayCall += RunEgyptEntrySmokeForBatchmode;
        }

        public static void RunEgyptEntrySmokeForBatchmode()
        {
            try
            {
                if (hasStarted)
                {
                    return;
                }

                hasStarted = true;
                Debug.Log("[TheCat] P0 Egypt entry evidence batchmode runner invoked.");
                ResetState();
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
                EditorApplication.update += Update;

                if (EditorApplication.isPlaying)
                {
                    StartSmoke();
                    return;
                }

                stageStartedAt = EditorApplication.timeSinceStartup;
                Debug.Log("[TheCat] Entering Play Mode for P0 Egypt entry smoke.");
                EditorApplication.EnterPlaymode();
            }
            catch (Exception exception)
            {
                ExitWithException(exception);
            }
        }

        private static void ResetState()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.update -= Update;
            stageStartedAt = EditorApplication.timeSinceStartup;
            smokeStarted = false;
            exitRequested = false;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            Debug.Log("[TheCat] Play Mode state changed for Egypt entry smoke: " + change + ".");
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                StartSmoke();
            }
        }

        private static void Update()
        {
            if (exitRequested)
            {
                return;
            }

            try
            {
                double elapsed = EditorApplication.timeSinceStartup - stageStartedAt;
                if (!smokeStarted)
                {
                    if (elapsed > EnterPlayModeTimeoutSeconds)
                    {
                        ExitWithFailure("Timed out entering Play Mode before starting P0 Egypt entry smoke.");
                    }

                    return;
                }

                if (P0PlayModeEgyptEntrySmoke.IsFinished)
                {
                    ExitWithSmokeResult();
                    return;
                }

                if (elapsed > EgyptEntrySmokeTimeoutSeconds)
                {
                    ExitWithFailure("Timed out waiting for P0 Egypt entry smoke to finish.");
                }
            }
            catch (Exception exception)
            {
                ExitWithException(exception);
            }
        }

        private static void StartSmoke()
        {
            if (exitRequested || smokeStarted)
            {
                return;
            }

            stageStartedAt = EditorApplication.timeSinceStartup;
            smokeStarted = true;

            string screenshotDirectory = ToProjectPath(P0PlayModeEgyptEntrySmoke.DefaultScreenshotDirectory);
            Debug.Log("[TheCat] Starting P0 Egypt entry smoke. Screenshot output: " + screenshotDirectory);
            bool started = P0PlayModeEgyptEntrySmoke.StartDefaultEgyptEntrySmoke(screenshotDirectory);
            if (!started)
            {
                ExitWithFailure("Could not start P0 Egypt entry smoke: " + P0PlayModeEgyptEntrySmoke.LastSummary);
            }
        }

        private static void ExitWithSmokeResult()
        {
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence = BuildScreenshotFileEvidence();
            bool passed = P0PlayModeEgyptEntrySmoke.State == P0PlayModeEgyptEntrySmokeState.Passed
                && P0PlayModeEgyptEntrySmoke.CapturedPaths.Count == P0PlayModeEgyptEntrySmoke.ExpectedCaptureCount
                && screenshotFileEvidence.IsComplete;
            string markdown = BuildReportMarkdown(passed, null, screenshotFileEvidence);
            WriteReport(ReportPath, markdown);
            CleanupAndExit(passed, P0PlayModeEgyptEntrySmoke.LastSummary);
        }

        private static void ExitWithFailure(string message)
        {
            string markdown = BuildReportMarkdown(false, message, BuildScreenshotFileEvidence());
            WriteReport(ReportPath, markdown);
            CleanupAndExit(false, message);
        }

        private static void ExitWithException(Exception exception)
        {
            string markdown = BuildReportMarkdown(
                false,
                exception.GetType().Name + ": " + exception.Message + Environment.NewLine + exception,
                BuildScreenshotFileEvidence());
            WriteReport(ReportPath, markdown);
            Debug.LogException(exception);
            CleanupAndExit(false, exception.Message);
        }

        private static P0PlayModeScreenshotFileEvidenceReport BuildScreenshotFileEvidence()
        {
            string screenshotDirectory = string.IsNullOrWhiteSpace(P0PlayModeEgyptEntrySmoke.LastOutputDirectory)
                ? ToProjectPath(P0PlayModeEgyptEntrySmoke.DefaultScreenshotDirectory)
                : P0PlayModeEgyptEntrySmoke.LastOutputDirectory;
            return P0PlayModeScreenshotFileEvidence.Evaluate(
                screenshotDirectory,
                P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames,
                null,
                null,
                null);
        }

        private static string BuildReportMarkdown(
            bool passed,
            string failureMessage,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence)
        {
            P0PlayModeScreenshotFileEvidenceReport fileEvidence = screenshotFileEvidence ?? BuildScreenshotFileEvidence();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# P0 Egypt Entry Smoke Report");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (passed ? "passed" : "failed"));
            builder.AppendLine("- Smoke state: " + P0PlayModeEgyptEntrySmoke.State);
            builder.AppendLine("- Screenshot output: " + P0PlayModeEgyptEntrySmoke.LastOutputDirectory);
            builder.AppendLine("- Capture count: " + P0PlayModeEgyptEntrySmoke.CapturedPaths.Count + "/" + P0PlayModeEgyptEntrySmoke.ExpectedCaptureCount);
            builder.AppendLine("- Screenshot file evidence: " + fileEvidence.BuildSummary());
            builder.AppendLine("- Evidence target: cat-room `enter_egypt_dream` -> Egypt dream route map layer 1 -> Egypt-context battle HUD -> first battle result -> route-map return.");
            if (!string.IsNullOrWhiteSpace(failureMessage))
            {
                builder.AppendLine("- Failure: " + failureMessage.Replace(Environment.NewLine, " "));
            }

            builder.AppendLine();
            builder.AppendLine("## Expected Screenshots");
            builder.AppendLine();
            for (int i = 0; i < P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames.Count; i++)
            {
                builder.AppendLine("- " + P0PlayModeEgyptEntrySmoke.ExpectedCaptureFileNames[i]);
            }

            builder.AppendLine();
            AppendSection(builder, "Egypt Entry Smoke", P0PlayModeEgyptEntrySmoke.LastDetailedLog);
            AppendSection(builder, "Screenshot File Evidence", fileEvidence.BuildDetailedSummary());
            return builder.ToString();
        }

        private static void AppendSection(StringBuilder builder, string title, string detail)
        {
            builder.AppendLine("## " + title);
            builder.AppendLine();
            builder.AppendLine("```text");
            builder.AppendLine(string.IsNullOrWhiteSpace(detail) ? "(empty)" : detail);
            builder.AppendLine("```");
            builder.AppendLine();
        }

        private static void CleanupAndExit(bool passed, string message)
        {
            if (exitRequested)
            {
                return;
            }

            exitRequested = true;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.update -= Update;

            string logMessage = "[TheCat] P0 Egypt entry batchmode " + (passed ? "passed: " : "failed: ") + message;
            if (passed)
            {
                Debug.Log(logMessage);
            }
            else
            {
                Debug.LogError(logMessage);
            }

            EditorApplication.Exit(passed ? 0 : 1);
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

        private static bool IsCommandLineEntryPointRequested()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], CommandLineEntryPoint, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
