using System;
using System.Collections.Generic;
using System.Text;

namespace TheCat.Tools
{
    public enum P0UnityRuntimeValidationStepCategory
    {
        Screenshot,
        Smoke,
        Console,
        UnityBinding,
        Review
    }

    public enum P0UnityRuntimeValidationPlanSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0UnityRuntimeValidationPlanIssue
    {
        public P0UnityRuntimeValidationPlanIssue(P0UnityRuntimeValidationPlanSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0UnityRuntimeValidationPlanSeverity Severity { get; }

        public string Message { get; }
    }

    public enum P0UnityConsoleLogSignalKind
    {
        ProjectFailure,
        KnownEnvironmentNoise,
        UnknownBlocking
    }

    public readonly struct P0UnityConsoleLogSignal
    {
        public P0UnityConsoleLogSignal(P0UnityConsoleLogSignalKind kind, string token, string line, int lineNumber)
        {
            Kind = kind;
            Token = token ?? string.Empty;
            Line = line ?? string.Empty;
            LineNumber = lineNumber;
        }

        public P0UnityConsoleLogSignalKind Kind { get; }

        public string Token { get; }

        public string Line { get; }

        public int LineNumber { get; }

        public string BuildSummary()
        {
            return Kind + " line " + LineNumber + " token `" + Token + "`: " + Line;
        }
    }

    public sealed class P0UnityConsoleLogClassifierReport
    {
        private readonly List<P0UnityConsoleLogSignal> signals = new List<P0UnityConsoleLogSignal>();

        public IReadOnlyList<P0UnityConsoleLogSignal> Signals => signals.AsReadOnly();

        public int ScannedLineCount { get; private set; }

        public int ProjectFailureTokenCount => Count(P0UnityConsoleLogSignalKind.ProjectFailure);

        public int KnownEnvironmentNoiseCount => Count(P0UnityConsoleLogSignalKind.KnownEnvironmentNoise);

        public int UnknownBlockingTokenCount => Count(P0UnityConsoleLogSignalKind.UnknownBlocking);

        public bool StrictClean => ProjectFailureTokenCount == 0
            && KnownEnvironmentNoiseCount == 0
            && UnknownBlockingTokenCount == 0;

        public bool ProjectOwnedClean => ProjectFailureTokenCount == 0
            && UnknownBlockingTokenCount == 0;

        public bool HasKnownEnvironmentNoise => KnownEnvironmentNoiseCount > 0;

        public bool HasProjectFailureTokens => ProjectFailureTokenCount > 0;

        public bool HasUnknownBlockingTokens => UnknownBlockingTokenCount > 0;

        public string FirstSignalToken => signals.Count == 0 ? string.Empty : signals[0].Token;

        public string FirstSignalSummary => signals.Count == 0 ? string.Empty : signals[0].BuildSummary();

        public void SetScannedLineCount(int scannedLineCount)
        {
            ScannedLineCount = Math.Max(0, scannedLineCount);
        }

        public void AddSignal(P0UnityConsoleLogSignal signal)
        {
            signals.Add(signal);
        }

        public string BuildSummary()
        {
            if (StrictClean)
            {
                return "Unity Console log is strict clean across " + ScannedLineCount + " scanned line(s).";
            }

            if (ProjectOwnedClean)
            {
                return "Unity Console log is project-owned clean with " + KnownEnvironmentNoiseCount + " known environment noise signal(s).";
            }

            return "Unity Console log has " + ProjectFailureTokenCount + " project failure token(s), "
                + UnknownBlockingTokenCount + " unknown blocker token(s), and "
                + KnownEnvironmentNoiseCount + " known environment noise signal(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Strict clean: " + YesNo(StrictClean),
                "Project-owned clean: " + YesNo(ProjectOwnedClean),
                "Project failure tokens: " + ProjectFailureTokenCount,
                "Unknown blocker tokens: " + UnknownBlockingTokenCount,
                "Known environment noise: " + KnownEnvironmentNoiseCount
            };

            for (int i = 0; i < signals.Count; i++)
            {
                lines.Add("- " + signals[i].BuildSummary());
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0UnityConsoleLogSignalKind kind)
        {
            int count = 0;
            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i].Kind == kind)
                {
                    count++;
                }
            }

            return count;
        }

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }
    }

    public static class P0UnityConsoleLogClassifier
    {
        private static readonly string[] ProjectPassTokens =
        {
            "[TheCat] P0 batchmode acceptance passed",
            "[TheCat] P0 Play Mode acceptance batchmode passed",
            "P0 Play Mode acceptance batchmode passed",
            "runtime evidence passed:"
        };

        private static readonly string[] ProjectFailureTokens =
        {
            "[TheCat] P0 batchmode acceptance failed",
            "P0 batchmode acceptance failed",
            "[TheCat] P0 Play Mode acceptance batchmode failed",
            "P0 Play Mode acceptance batchmode failed",
            "runtime evidence failed",
            "ReadPixels capture failed",
            "Candidate frame draw audit failed",
            "Resolution override failed"
        };

        private static readonly string[] KnownEnvironmentNoiseTokens =
        {
            "[Licensing::",
            "Licensing Client",
            "Unity.Licensing",
            "d3d12: upload buffer was too small",
            "D3D12",
            "D3D",
            "Unity AI",
            "Unity.AI.Tracing",
            "Unity.AI",
            "AI Assistant",
            "AiEditor",
            "##utp:{\"type\":\"MemoryLeaks\"",
            "\"type\":\"MemoryLeaks\"",
            "MemoryLeaks",
            "StackAllocator",
            "ALLOC_TEMP",
            "TLS Allocator",
            "JobTempAlloc",
            "Package Manager",
            "AcceleratorClient"
        };

        private static readonly string[] UnknownBlockingTokens =
        {
            "] Error:",
            " Error:",
            "[Error]",
            "Exception:",
            "Exception",
            "FAILED:",
            "Assertion failed",
            "Test run failed",
            "compilation failed",
            "Build failed"
        };

        public static P0UnityConsoleLogClassifierReport Classify(string logContent)
        {
            P0UnityConsoleLogClassifierReport report = new P0UnityConsoleLogClassifierReport();
            if (string.IsNullOrWhiteSpace(logContent))
            {
                report.SetScannedLineCount(0);
                return report;
            }

            string[] lines = NormalizeLineEndings(logContent).Split('\n');
            int scannedLineCount = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                scannedLineCount++;
                ClassifyLine(report, line.Trim(), i + 1);
            }

            report.SetScannedLineCount(scannedLineCount);
            return report;
        }

        private static void ClassifyLine(P0UnityConsoleLogClassifierReport report, string line, int lineNumber)
        {
            if (ContainsAny(line, ProjectPassTokens))
            {
                return;
            }

            string projectFailureToken = FindToken(line, ProjectFailureTokens);
            if (!string.IsNullOrEmpty(projectFailureToken))
            {
                report.AddSignal(new P0UnityConsoleLogSignal(
                    P0UnityConsoleLogSignalKind.ProjectFailure,
                    projectFailureToken,
                    line,
                    lineNumber));
                return;
            }

            string environmentNoiseToken = FindToken(line, KnownEnvironmentNoiseTokens);
            if (!string.IsNullOrEmpty(environmentNoiseToken))
            {
                report.AddSignal(new P0UnityConsoleLogSignal(
                    P0UnityConsoleLogSignalKind.KnownEnvironmentNoise,
                    environmentNoiseToken,
                    line,
                    lineNumber));
                return;
            }

            string unknownBlockingToken = FindToken(line, UnknownBlockingTokens);
            if (!string.IsNullOrEmpty(unknownBlockingToken))
            {
                report.AddSignal(new P0UnityConsoleLogSignal(
                    P0UnityConsoleLogSignalKind.UnknownBlocking,
                    unknownBlockingToken,
                    line,
                    lineNumber));
            }
        }

        private static string NormalizeLineEndings(string text)
        {
            return (text ?? string.Empty).Replace("\r\n", "\n").Replace('\r', '\n');
        }

        private static bool ContainsAny(string text, IReadOnlyList<string> tokens)
        {
            return !string.IsNullOrEmpty(FindToken(text, tokens));
        }

        private static string FindToken(string text, IReadOnlyList<string> tokens)
        {
            if (string.IsNullOrEmpty(text) || tokens == null)
            {
                return string.Empty;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (text.IndexOf(tokens[i], StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return tokens[i];
                }
            }

            return string.Empty;
        }
    }

    public readonly struct P0UnityRuntimeValidationStep
    {
        public P0UnityRuntimeValidationStep(
            string stepId,
            string displayName,
            P0UnityRuntimeValidationStepCategory category,
            string requiredOutput,
            string screenshotFileName,
            string validationNotes)
        {
            StepId = stepId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Category = category;
            RequiredOutput = requiredOutput ?? string.Empty;
            ScreenshotFileName = screenshotFileName ?? string.Empty;
            ValidationNotes = validationNotes ?? string.Empty;
        }

        public string StepId { get; }

        public string DisplayName { get; }

        public P0UnityRuntimeValidationStepCategory Category { get; }

        public string RequiredOutput { get; }

        public string ScreenshotFileName { get; }

        public string ValidationNotes { get; }

        public bool IsScreenshot => Category == P0UnityRuntimeValidationStepCategory.Screenshot;

        public bool IsActiveCatScreenshot =>
            ScreenshotFileName == P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName
            || ScreenshotFileName == P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName
            || ScreenshotFileName == P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName;

        public string BuildSummary()
        {
            return StepId + " [" + Category + "] -> " + RequiredOutput;
        }
    }

    public sealed class P0UnityRuntimeValidationPlanReport
    {
        private readonly List<P0UnityRuntimeValidationPlanIssue> issues =
            new List<P0UnityRuntimeValidationPlanIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0UnityRuntimeValidationStep> steps =
            new List<P0UnityRuntimeValidationStep>();

        public IReadOnlyList<P0UnityRuntimeValidationPlanIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0UnityRuntimeValidationStep> Steps => steps.AsReadOnly();

        public int StepCount => steps.Count;

        public int ScreenshotStepCount { get; private set; }

        public int ExpectedScreenshotMatchCount { get; private set; }

        public int ActiveCatScreenshotStepCount { get; private set; }

        public int SmokeStepCount { get; private set; }

        public int ConsoleStepCount { get; private set; }

        public int UnityBindingStepCount { get; private set; }

        public int ReviewStepCount { get; private set; }

        public int DuplicateStepIdCount { get; private set; }

        public bool HasRouteFlowSmokeStep { get; private set; }

        public bool HasDefeatFlowSmokeStep { get; private set; }

        public bool HasStarterCatTurnaroundReviewStep { get; private set; }

        public bool HasChineseUiScaleMatrixStep { get; private set; }

        public bool ChineseUiScaleEvidenceReady { get; private set; }

        public int ChineseUiScaleCaptureRows { get; private set; }

        public int FailureCount => Count(P0UnityRuntimeValidationPlanSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0UnityRuntimeValidationPlan.ExpectedCoveredCheckCount
            && StepCount == P0UnityRuntimeValidationPlan.ExpectedStepCount
            && ScreenshotStepCount == P0UnityRuntimeValidationPlan.ExpectedScreenshotStepCount
            && ExpectedScreenshotMatchCount == P0PlayModeScreenshotSmoke.ExpectedCaptureCount
            && ActiveCatScreenshotStepCount == P0StarterCatFormalImportReadiness.ExpectedStarterCatCount
            && SmokeStepCount == P0UnityRuntimeValidationPlan.ExpectedSmokeStepCount
            && ConsoleStepCount == P0UnityRuntimeValidationPlan.ExpectedConsoleStepCount
            && UnityBindingStepCount == P0UnityRuntimeValidationPlan.ExpectedUnityBindingStepCount
            && ReviewStepCount == P0UnityRuntimeValidationPlan.ExpectedReviewStepCount
            && DuplicateStepIdCount == 0
            && HasRouteFlowSmokeStep
            && HasDefeatFlowSmokeStep
            && HasStarterCatTurnaroundReviewStep
            && HasChineseUiScaleMatrixStep
            && ChineseUiScaleEvidenceReady
            && ChineseUiScaleCaptureRows == P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount;

        public void AddStep(P0UnityRuntimeValidationStep step)
        {
            steps.Add(step);
        }

        public void SetCounts(
            int screenshotStepCount,
            int expectedScreenshotMatchCount,
            int activeCatScreenshotStepCount,
            int smokeStepCount,
            int consoleStepCount,
            int unityBindingStepCount,
            int reviewStepCount,
            int duplicateStepIdCount,
            bool hasRouteFlowSmokeStep,
            bool hasDefeatFlowSmokeStep,
            bool hasStarterCatTurnaroundReviewStep,
            bool hasChineseUiScaleMatrixStep,
            bool chineseUiScaleEvidenceReady,
            int chineseUiScaleCaptureRows)
        {
            ScreenshotStepCount = screenshotStepCount;
            ExpectedScreenshotMatchCount = expectedScreenshotMatchCount;
            ActiveCatScreenshotStepCount = activeCatScreenshotStepCount;
            SmokeStepCount = smokeStepCount;
            ConsoleStepCount = consoleStepCount;
            UnityBindingStepCount = unityBindingStepCount;
            ReviewStepCount = reviewStepCount;
            DuplicateStepIdCount = duplicateStepIdCount;
            HasRouteFlowSmokeStep = hasRouteFlowSmokeStep;
            HasDefeatFlowSmokeStep = hasDefeatFlowSmokeStep;
            HasStarterCatTurnaroundReviewStep = hasStarterCatTurnaroundReviewStep;
            HasChineseUiScaleMatrixStep = hasChineseUiScaleMatrixStep;
            ChineseUiScaleEvidenceReady = chineseUiScaleEvidenceReady;
            ChineseUiScaleCaptureRows = chineseUiScaleCaptureRows;
        }

        public void AddIssue(P0UnityRuntimeValidationPlanSeverity severity, string message)
        {
            issues.Add(new P0UnityRuntimeValidationPlanIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsReady
                ? "P0 Unity runtime validation plan ready for " + StepCount + " step(s), " + ScreenshotStepCount + " screenshot(s), and " + ChineseUiScaleCaptureRows + " Chinese UI scale capture row(s)."
                : "P0 Unity runtime validation plan has " + FailureCount + " failure(s) across " + StepCount + " step(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Steps: " + StepCount,
                "Screenshot steps: " + ScreenshotStepCount,
                "Screenshot matches: " + ExpectedScreenshotMatchCount + "/" + P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Active-cat screenshot steps: " + ActiveCatScreenshotStepCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount,
                "Smoke steps: " + SmokeStepCount,
                "Console steps: " + ConsoleStepCount,
                "Unity binding steps: " + UnityBindingStepCount,
                "Review steps: " + ReviewStepCount,
                "Duplicate step ids: " + DuplicateStepIdCount,
                "Chinese UI scale rows: " + ChineseUiScaleCaptureRows + "/" + P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# P0 Unity Runtime Validation Plan");
            builder.AppendLine();
            builder.AppendLine("- Ready: " + YesNo(IsReady));
            builder.AppendLine("- Screenshot steps: " + ScreenshotStepCount + "/" + P0PlayModeScreenshotSmoke.ExpectedCaptureCount);
            builder.AppendLine("- Active-cat turnaround screenshots: " + ActiveCatScreenshotStepCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount);
            builder.AppendLine("- Starter-cat colored-turnaround lock: required before any formal body-art replacement.");
            builder.AppendLine("- Chinese UI scale rows: " + ChineseUiScaleCaptureRows + "/" + P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount);
            builder.AppendLine("- Output evidence folder: `design/development/screenshots/p0-playmode-smoke`");
            builder.AppendLine("- Editor menu: `TheCat/P0/Start Play Mode Acceptance Smoke`");
            builder.AppendLine("- Report menu: `TheCat/P0/Write P0 Unity Runtime Validation Plan`");
            builder.AppendLine();
            builder.AppendLine("This report is an execution plan. Final P0 visual acceptance still requires Unity Play Mode screenshots, a clean Console, scene/prefab binding inspection, Sprite import checks, active-cat turnaround review, and the Batch 75 Chinese UI scale matrix.");
            builder.AppendLine();
            builder.AppendLine("| step | category | output | notes |");
            builder.AppendLine("| --- | --- | --- | --- |");
            for (int i = 0; i < steps.Count; i++)
            {
                builder.Append("| ");
                builder.Append(Escape(steps[i].StepId));
                builder.Append(" | ");
                builder.Append(steps[i].Category);
                builder.Append(" | ");
                builder.Append(Escape(steps[i].RequiredOutput));
                builder.Append(" | ");
                builder.Append(Escape(steps[i].ValidationNotes));
                builder.AppendLine(" |");
            }

            return builder.ToString();
        }

        private int Count(P0UnityRuntimeValidationPlanSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("|", "/");
        }
    }

    public static class P0UnityRuntimeValidationPlan
    {
        public const int ExpectedStepCount = 18;
        public const int ExpectedScreenshotStepCount = 11;
        public const int ExpectedSmokeStepCount = 2;
        public const int ExpectedConsoleStepCount = 1;
        public const int ExpectedUnityBindingStepCount = 2;
        public const int ExpectedReviewStepCount = 2;
        public const int ExpectedCoveredCheckCount = 8;

        public const string RouteFlowSmokeStepId = "smoke.route_flow";
        public const string DefeatFlowSmokeStepId = "smoke.defeat_flow";
        public const string ConsoleCleanStepId = "console.clean";
        public const string ScenePrefabBindingStepId = "unity.scene_prefab_bindings";
        public const string SpriteImportSettingsStepId = "unity.sprite_import_settings";
        public const string StarterCatTurnaroundReviewStepId = "review.starter_cat_turnaround";
        public const string ChineseUiScaleMatrixStepId = "review.chinese_ui_scale_matrix";

        public static P0UnityRuntimeValidationPlanReport EvaluateCurrentPlan()
        {
            return Evaluate(
                CreateP0Steps(),
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                P0StarterCatFormalImportReadiness.ActiveCatScreenshotFileNames,
                P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket());
        }

        public static P0UnityRuntimeValidationPlanReport Evaluate(
            IReadOnlyList<P0UnityRuntimeValidationStep> steps,
            IReadOnlyList<string> expectedScreenshotFileNames,
            IReadOnlyList<string> expectedActiveCatScreenshotFileNames,
            P0ChineseUiScaleEvidencePacketReport chineseUiScaleEvidence)
        {
            P0UnityRuntimeValidationPlanReport report = new P0UnityRuntimeValidationPlanReport();
            IReadOnlyList<P0UnityRuntimeValidationStep> validationSteps = steps ?? Array.Empty<P0UnityRuntimeValidationStep>();
            for (int i = 0; i < validationSteps.Count; i++)
            {
                report.AddStep(validationSteps[i]);
            }

            int screenshotStepCount = 0;
            int expectedScreenshotMatchCount = 0;
            int activeCatScreenshotStepCount = 0;
            int smokeStepCount = 0;
            int consoleStepCount = 0;
            int unityBindingStepCount = 0;
            int reviewStepCount = 0;
            int duplicateStepIdCount = CountDuplicateStepIds(validationSteps);
            bool hasRouteFlowSmokeStep = false;
            bool hasDefeatFlowSmokeStep = false;
            bool hasStarterCatTurnaroundReviewStep = false;
            bool hasChineseUiScaleMatrixStep = false;

            HashSet<string> screenshotFiles = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < validationSteps.Count; i++)
            {
                P0UnityRuntimeValidationStep step = validationSteps[i];
                if (step.IsScreenshot)
                {
                    screenshotStepCount++;
                    if (!string.IsNullOrWhiteSpace(step.ScreenshotFileName))
                    {
                        screenshotFiles.Add(step.ScreenshotFileName);
                    }
                }

                if (step.IsActiveCatScreenshot)
                {
                    activeCatScreenshotStepCount++;
                }

                if (step.Category == P0UnityRuntimeValidationStepCategory.Smoke)
                {
                    smokeStepCount++;
                }
                else if (step.Category == P0UnityRuntimeValidationStepCategory.Console)
                {
                    consoleStepCount++;
                }
                else if (step.Category == P0UnityRuntimeValidationStepCategory.UnityBinding)
                {
                    unityBindingStepCount++;
                }
                else if (step.Category == P0UnityRuntimeValidationStepCategory.Review)
                {
                    reviewStepCount++;
                }

                if (step.StepId == RouteFlowSmokeStepId)
                {
                    hasRouteFlowSmokeStep = true;
                }
                else if (step.StepId == DefeatFlowSmokeStepId)
                {
                    hasDefeatFlowSmokeStep = true;
                }
                else if (step.StepId == StarterCatTurnaroundReviewStepId)
                {
                    hasStarterCatTurnaroundReviewStep = true;
                }
                else if (step.StepId == ChineseUiScaleMatrixStepId)
                {
                    hasChineseUiScaleMatrixStep = true;
                }
            }

            expectedScreenshotMatchCount = CountExpectedValues(screenshotFiles, expectedScreenshotFileNames);
            int expectedActiveCatMatches = CountExpectedValues(screenshotFiles, expectedActiveCatScreenshotFileNames);
            bool chineseUiScaleEvidenceReady = chineseUiScaleEvidence != null && chineseUiScaleEvidence.IsReady;
            int chineseUiScaleRows = chineseUiScaleEvidence == null ? 0 : chineseUiScaleEvidence.CaptureMatrixRowCount;

            report.SetCounts(
                screenshotStepCount,
                expectedScreenshotMatchCount,
                activeCatScreenshotStepCount,
                smokeStepCount,
                consoleStepCount,
                unityBindingStepCount,
                reviewStepCount,
                duplicateStepIdCount,
                hasRouteFlowSmokeStep,
                hasDefeatFlowSmokeStep,
                hasStarterCatTurnaroundReviewStep,
                hasChineseUiScaleMatrixStep,
                chineseUiScaleEvidenceReady,
                chineseUiScaleRows);

            Require(
                report,
                validationSteps.Count == ExpectedStepCount,
                "Runtime validation plan has the expected 18 Unity-side acceptance steps.",
                "Runtime validation plan step count is stale.");

            Require(
                report,
                duplicateStepIdCount == 0,
                "Runtime validation plan has unique step ids.",
                "Runtime validation plan contains duplicate step ids.");

            Require(
                report,
                screenshotStepCount == ExpectedScreenshotStepCount
                && expectedScreenshotMatchCount == P0PlayModeScreenshotSmoke.ExpectedCaptureCount,
                "Runtime validation plan covers all 11 Play Mode screenshot files in the screenshot smoke.",
                "Runtime validation plan is missing one or more Play Mode screenshot files.");

            Require(
                report,
                activeCatScreenshotStepCount == P0StarterCatFormalImportReadiness.ExpectedStarterCatCount
                && expectedActiveCatMatches == P0StarterCatFormalImportReadiness.ExpectedStarterCatCount,
                "Runtime validation plan includes Saiban, Nephthys, and Suzune active-cat screenshot checks for colored-turnaround comparison.",
                "Runtime validation plan is missing active-cat turnaround screenshot checks.");

            Require(
                report,
                smokeStepCount == ExpectedSmokeStepCount
                && hasRouteFlowSmokeStep
                && hasDefeatFlowSmokeStep,
                "Runtime validation plan includes route-flow and defeat-flow Play Mode smoke checks.",
                "Runtime validation plan is missing route-flow or defeat-flow smoke checks.");

            Require(
                report,
                consoleStepCount == ExpectedConsoleStepCount,
                "Runtime validation plan requires a clean Unity Console pass.",
                "Runtime validation plan is missing a clean Console check.");

            Require(
                report,
                unityBindingStepCount == ExpectedUnityBindingStepCount,
                "Runtime validation plan requires scene/prefab binding and Sprite import settings checks.",
                "Runtime validation plan is missing scene, prefab, or Sprite import validation.");

            Require(
                report,
                reviewStepCount == ExpectedReviewStepCount
                && hasStarterCatTurnaroundReviewStep
                && hasChineseUiScaleMatrixStep
                && chineseUiScaleEvidenceReady
                && chineseUiScaleRows == P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount,
                "Runtime validation plan links starter-cat turnaround review and the 20-row Chinese UI scale screenshot matrix.",
                "Runtime validation plan is missing starter-cat review, Chinese UI scale matrix, or Batch 75 evidence readiness.");

            return report;
        }

        public static IReadOnlyList<P0UnityRuntimeValidationStep> CreateP0Steps()
        {
            return Array.AsReadOnly(new[]
            {
                Screenshot("screenshot.main_menu", "Main Menu", P0PlayModeScreenshotSmoke.MainMenuCaptureFileName, "Confirm Chinese-facing start UI, character-selection affordance, and no overlap."),
                Screenshot("screenshot.cat_room", "Cat Room", P0PlayModeScreenshotSmoke.CatRoomCaptureFileName, "Confirm cat-room values, bed/feeder/litter/dream entrance affordances, and no overlap before entering the route."),
                Screenshot("screenshot.route_map", "Route Map Layer 1", P0PlayModeScreenshotSmoke.RouteMapCaptureFileName, "Confirm 10-layer route entry, node readability, route-choice controls, and no clipped Chinese labels."),
                Screenshot("screenshot.battle_hud", "Battle HUD Layer 1", P0PlayModeScreenshotSmoke.BattleHudCaptureFileName, "Confirm owner sleep, cat HP, poop, hunger, skills, enemy cards, and interaction hints remain readable."),
                Screenshot("screenshot.active_cat_saiban", "Active Cat Saiban", P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName, "Compare Saiban against the locked colored three-view turnaround before any formal body-art import."),
                Screenshot("screenshot.active_cat_nephthys", "Active Cat Nephthys", P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName, "Compare Nephthys against the locked colored three-view turnaround before any formal body-art import."),
                Screenshot("screenshot.active_cat_suzune", "Active Cat Suzune", P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName, "Compare Suzune against the locked colored three-view turnaround before any formal body-art import."),
                Screenshot("screenshot.battle_world_visuals", "Battle World Visuals", P0PlayModeScreenshotSmoke.BattleWorldVisualsCaptureFileName, "Confirm bed, litter box, feeder, bedroom dream background, and enemy readability in the battle scene."),
                Screenshot("screenshot.call_tyrant_warning_vfx", "Call Tyrant Warning VFX", P0PlayModeScreenshotSmoke.CallTyrantWarningVfxCaptureFileName, "Confirm Boss summon and app-throw warning readability without obscuring HUD or bed state."),
                Screenshot("screenshot.battle_result", "Battle Result Layer 1", P0PlayModeScreenshotSmoke.BattleResultCaptureFileName, "Confirm victory/defeat result surface, rewards, restart/continue controls, and Chinese labels."),
                Screenshot("screenshot.settlement", "Run Settlement", P0PlayModeScreenshotSmoke.SettlementCaptureFileName, "Confirm run-cleared/failed summary, route metrics, rewards, and follow-up controls."),
                new P0UnityRuntimeValidationStep(RouteFlowSmokeStepId, "Route Flow Smoke", P0UnityRuntimeValidationStepCategory.Smoke, "P0PlayModeRouteFlowSmoke.State == Passed", string.Empty, "Verify main menu to route map to battle to post-battle route flow and final settlement cat-room return in Play Mode."),
                new P0UnityRuntimeValidationStep(DefeatFlowSmokeStepId, "Defeat Flow Smoke", P0UnityRuntimeValidationStepCategory.Smoke, "P0PlayModeDefeatFlowSmoke.State == Passed", string.Empty, "Verify forced defeat reaches result and settlement flow without Console errors."),
                new P0UnityRuntimeValidationStep(ConsoleCleanStepId, "Clean Unity Console", P0UnityRuntimeValidationStepCategory.Console, "Unity Console has zero Error logs after acceptance smoke", string.Empty, "Use Unity MCP or manual Editor Console capture after the Play Mode run."),
                new P0UnityRuntimeValidationStep(ScenePrefabBindingStepId, "Scene And Prefab Bindings", P0UnityRuntimeValidationStepCategory.UnityBinding, "P0 scenes load with required controllers, SpriteRenderers, HUD presenters, and route/battle bindings", string.Empty, "Inspect MainMenu, RouteMap, GrayboxBattle, runtime visual views, and validation-only assets."),
                new P0UnityRuntimeValidationStep(SpriteImportSettingsStepId, "Sprite Import Settings", P0UnityRuntimeValidationStepCategory.UnityBinding, "Runtime sprites use P0 import settings, alpha, no unwanted mipmaps, and expected max texture sizes", string.Empty, "Keep candidate-only folders out of Assets until formal install gates approve them."),
                new P0UnityRuntimeValidationStep(StarterCatTurnaroundReviewStepId, "Starter Cat Turnaround Review", P0UnityRuntimeValidationStepCategory.Review, "3 active-cat screenshots approved against locked colored three-view turnarounds", string.Empty, "Do not replace Saiban, Nephthys, or Suzune body art until all three active-cat comparisons pass."),
                new P0UnityRuntimeValidationStep(ChineseUiScaleMatrixStepId, "Chinese UI Scale Matrix", P0UnityRuntimeValidationStepCategory.Review, "20 Batch 75 surface/resolution rows filled with screenshots and Console notes", string.Empty, "Use 1024x768, 1280x720, 1600x900, and 1920x1080 for five P0 UI surfaces.")
            });
        }

        private static P0UnityRuntimeValidationStep Screenshot(
            string stepId,
            string displayName,
            string fileName,
            string notes)
        {
            return new P0UnityRuntimeValidationStep(
                stepId,
                displayName,
                P0UnityRuntimeValidationStepCategory.Screenshot,
                fileName,
                fileName,
                notes);
        }

        private static int CountExpectedValues(HashSet<string> actual, IReadOnlyList<string> expected)
        {
            if (actual == null || expected == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < expected.Count; i++)
            {
                if (actual.Contains(expected[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountDuplicateStepIds(IReadOnlyList<P0UnityRuntimeValidationStep> steps)
        {
            if (steps == null || steps.Count == 0)
            {
                return 0;
            }

            HashSet<string> seen = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> duplicates = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < steps.Count; i++)
            {
                string id = steps[i].StepId;
                if (string.IsNullOrWhiteSpace(id))
                {
                    duplicates.Add(string.Empty);
                }
                else if (!seen.Add(id))
                {
                    duplicates.Add(id);
                }
            }

            return duplicates.Count;
        }

        private static void Require(
            P0UnityRuntimeValidationPlanReport report,
            bool condition,
            string coveredMessage,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredMessage);
            }
            else
            {
                report.AddIssue(P0UnityRuntimeValidationPlanSeverity.Failure, failureMessage);
            }
        }
    }
}
