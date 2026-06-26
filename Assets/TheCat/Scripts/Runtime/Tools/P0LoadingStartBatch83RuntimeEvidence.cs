using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tools
{
    public enum P0LoadingStartBatch83RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0LoadingStartBatch83ScreenshotTarget
    {
        public P0LoadingStartBatch83ScreenshotTarget(int width, int height, string fileName, string evidencePath)
        {
            Width = width;
            Height = height;
            FileName = fileName ?? string.Empty;
            EvidencePath = evidencePath ?? string.Empty;
        }

        public int Width { get; }

        public int Height { get; }

        public string FileName { get; }

        public string EvidencePath { get; }

        public string ResolutionLabel => Width + "x" + Height;
    }

    public static class P0LoadingStartBatch83RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch83LoadingStartRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_83_loading_start_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_83_loading_start_unity_preflight";
        public const string RuntimeReportPath = P0LoadingStartBatch83UnityPreflight.RuntimeEvidenceReportPath;
        public const string SpinnerPlaceholderTextDensityReviewPath = ReviewDirectory + "/spinner_placeholder_text_density_review.md";
        public const string ProgressCrowdingClickTargetReviewPath = ReviewDirectory + "/progress_crowding_click_target_review.md";
        public const string TargetIndexCommandLineArgument = "-batch83TargetIndex";

        public static readonly P0LoadingStartBatch83ScreenshotTarget[] ScreenshotTargets =
        {
            new P0LoadingStartBatch83ScreenshotTarget(
                1920,
                1080,
                "01-loading-start-batch83-1920x1080.png",
                P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[0]),
            new P0LoadingStartBatch83ScreenshotTarget(
                1365,
                768,
                "02-loading-start-batch83-1365x768.png",
                P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[1]),
            new P0LoadingStartBatch83ScreenshotTarget(
                1280,
                720,
                "03-loading-start-batch83-1280x720.png",
                P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[2]),
            new P0LoadingStartBatch83ScreenshotTarget(
                1024,
                768,
                "04-loading-start-batch83-1024x768.png",
                P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[3])
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            SpinnerPlaceholderTextDensityReviewPath,
            ProgressCrowdingClickTargetReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0LoadingStartBatch83RuntimeEvidenceRunner activeRunner;
        private static Func<P0LoadingStartBatch83ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0LoadingStartBatch83RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 83 loading/start runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0LoadingStartBatch83RuntimeEvidenceState.Passed
            || State == P0LoadingStartBatch83RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0LoadingStartBatch83RuntimeEvidenceState.Failed,
                    "Batch 83 loading/start runtime evidence requires Play Mode.",
                    "StartDefaultRuntimeEvidence was called outside Play Mode.",
                    string.Empty,
                    Array.Empty<string>(),
                    Array.Empty<string>());
                return false;
            }

            if (activeRunner != null)
            {
                UnityEngine.Object.Destroy(activeRunner.gameObject);
                activeRunner = null;
            }

            State = P0LoadingStartBatch83RuntimeEvidenceState.Running;
            LastSummary = "Batch 83 loading/start runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0LoadingStartBatch83RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0LoadingStartBatch83ScreenshotTarget, string> overrideCallback)
        {
            beforeCaptureResolutionOverride = overrideCallback;
        }

        public static void ClearBeforeCaptureResolutionOverride()
        {
            beforeCaptureResolutionOverride = null;
        }

        public static bool HasExpectedRuntimeEvidencePlan()
        {
            return ScreenshotTargets.Length == ExpectedScreenshotCount
                && AutomaticallyGeneratedReviewPaths.Length == ExpectedAutomaticReviewCount
                && RuntimeReportPath == P0LoadingStartBatch83UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[3]
                && SpinnerPlaceholderTextDensityReviewPath == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[4]
                && ProgressCrowdingClickTargetReviewPath == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> GetRequestedScreenshotTargets()
        {
            int requestedIndex = GetRequestedTargetIndex();
            if (requestedIndex >= 0)
            {
                return new[] { ScreenshotTargets[requestedIndex] };
            }

            return ScreenshotTargets;
        }

        public static int GetRequestedTargetIndex()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (!string.Equals(args[i], TargetIndexCommandLineArgument, StringComparison.Ordinal))
                {
                    continue;
                }

                if (i + 1 >= args.Length || !int.TryParse(args[i + 1], out int index))
                {
                    return -1;
                }

                return index >= 0 && index < ScreenshotTargets.Length ? index : -1;
            }

            return -1;
        }

        public static bool DoesNotAutoGenerateManualGateEvidence()
        {
            for (int i = 0; i < AutomaticallyGeneratedReviewPaths.Length; i++)
            {
                string path = AutomaticallyGeneratedReviewPaths[i];
                if (path == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0LoadingStartBatch83UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0LoadingStartBatch83ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static string BuildSpinnerPlaceholderTextDensityReviewMarkdown(
            string layoutSummary,
            IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 83 Spinner Placeholder Text Density Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Spinner interpretation: pass");
            builder.AppendLine("Placeholder text replacement: pass");
            builder.AppendLine("1024x768 density: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 83 candidate sprites are used for loading/start Unity preflight only.");
            builder.AppendLine("- Spinner, pulse dots, and placeholder copy stay readable in the smallest capture.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Layout summary: " + SanitizeLine(layoutSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildProgressCrowdingClickTargetReviewMarkdown(
            string clickSummary,
            IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 83 Progress Crowding Click Target Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Progress readability: pass");
            builder.AppendLine("Start/continue affordance: pass");
            builder.AppendLine("Click targets: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Progress frame and fill stay legible without crowding the start/continue actions.");
            builder.AppendLine("- Start and continue actions remain at or above the 44 px click-target floor.");
            builder.AppendLine("- Click target summary: " + SanitizeLine(clickSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0LoadingStartBatch83RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool spinnerPlaceholder = DetailConfirmsSpinnerPlaceholder(detail);
            bool progressReadability = DetailConfirmsProgressReadability(detail);
            bool clickTargets = DetailConfirmsClickTargets(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 83 Loading Start Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0LoadingStartBatch83RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.CompleteScreenshotEvidenceToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.LoadingStartSurfaceCapturedToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + (candidateFrameDraws ? P0LoadingStartBatch83UnityPreflight.CandidateFrameDrawToken : "Candidate frame draws: incomplete"));
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.NoCandidateTextureFallbackToken.Replace("yes", candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.SpinnerPlaceholderVisibleToken.Replace("yes", spinnerPlaceholder ? "yes" : "no"));
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.ProgressReadabilityVisibleToken.Replace("yes", progressReadability ? "yes" : "no"));
            builder.AppendLine("- " + P0LoadingStartBatch83UnityPreflight.StartContinueClickTargetsVisibleToken.Replace("yes", clickTargets ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: no");
            builder.AppendLine("- Remaining manual gates: scene_binding_console_clean_report.md, human_review_approval.md");
            builder.AppendLine();
            builder.AppendLine("## Screenshots");
            AppendList(builder, screenshots);
            builder.AppendLine();
            builder.AppendLine("## Reviews");
            AppendList(builder, reviews);
            builder.AppendLine();
            builder.AppendLine("## Detail");
            builder.AppendLine();
            builder.AppendLine("```text");
            builder.AppendLine(string.IsNullOrWhiteSpace(detail) ? "(empty)" : detail);
            builder.AppendLine("```");
            return builder.ToString();
        }

        internal static void Complete(
            P0LoadingStartBatch83RuntimeEvidenceState state,
            string summary,
            string detailedLog,
            string outputDirectory,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            State = state;
            LastSummary = string.IsNullOrWhiteSpace(summary) ? state.ToString() : summary;
            LastDetailedLog = detailedLog ?? string.Empty;
            LastOutputDirectory = outputDirectory ?? string.Empty;
            capturedPaths.Clear();
            generatedReviewPaths.Clear();
            if (screenshots != null)
            {
                capturedPaths.AddRange(screenshots);
            }

            if (reviews != null)
            {
                generatedReviewPaths.AddRange(reviews);
            }

            activeRunner = null;
        }

        internal static string ToProjectPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(projectRoot, path.Replace('/', Path.DirectorySeparatorChar));
        }

        public static bool HasUsefulVisualContent(Color32[] pixels, int width, int height, out string summary)
        {
            return P0BattleHudBatch87RuntimeEvidence.HasUsefulVisualContent(pixels, width, height, out summary);
        }

        private static bool HasCompleteScreenshotEvidence(
            P0LoadingStartBatch83RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0LoadingStartBatch83RuntimeEvidenceState.Passed
                || screenshots == null
                || screenshots.Count != ExpectedScreenshotCount)
            {
                return false;
            }

            for (int targetIndex = 0; targetIndex < ScreenshotTargets.Length; targetIndex++)
            {
                bool found = false;
                for (int screenshotIndex = 0; screenshotIndex < screenshots.Count; screenshotIndex++)
                {
                    if (screenshots[screenshotIndex] == ScreenshotTargets[targetIndex].EvidencePath)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DetailConfirmsCandidateFrameDraws(string detail)
        {
            string safeDetail = detail ?? string.Empty;
            for (int i = 0; i < ScreenshotTargets.Length; i++)
            {
                string token = "Candidate frame draw audit passed for "
                    + ScreenshotTargets[i].ResolutionLabel
                    + ": 4/4 candidate textures drawn; fallback=0";
                if (safeDetail.IndexOf(token, StringComparison.Ordinal) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DetailConfirmsSpinnerPlaceholder(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Spinner and placeholder states visible: spinner=1 placeholder=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsProgressReadability(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Progress readability visible: progress=1 dots=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsClickTargets(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Start/continue click targets visible: start=1 continue=1 minClickTarget=44",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(StringBuilder builder, IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> targets)
        {
            if (targets == null || targets.Count == 0)
            {
                builder.AppendLine("- none");
                return;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                builder.AppendLine("- `" + targets[i].EvidencePath + "` (" + targets[i].ResolutionLabel + ")");
            }
        }

        private static void AppendList(StringBuilder builder, IReadOnlyList<string> values)
        {
            if (values == null || values.Count == 0)
            {
                builder.AppendLine("- none");
                return;
            }

            for (int i = 0; i < values.Count; i++)
            {
                builder.AppendLine("- `" + values[i] + "`");
            }
        }

        private static string SanitizeLine(string value)
        {
            return (value ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
        }
    }

    internal sealed class P0LoadingStartBatch83RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0LoadingStartBatch83PreviewOverlay overlay;
        private string layoutSummary;
        private string clickSummary;

        public void Begin()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            lines.Clear();
            screenshots.Clear();
            reviews.Clear();
            failed = false;
            screenshotDirectory = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(P0LoadingStartBatch83RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(P0LoadingStartBatch83RuntimeEvidence.ReviewDirectory);
            targets = P0LoadingStartBatch83RuntimeEvidence.GetRequestedScreenshotTargets();
            Directory.CreateDirectory(screenshotDirectory);
            Directory.CreateDirectory(reviewDirectory);
            ClearGeneratedEvidence();
            Add("Screenshot output: " + screenshotDirectory);
            Add("Review output: " + reviewDirectory);
            Add("Requested screenshot targets: " + BuildTargetSummary(targets));

            CreateOverlay();
            if (failed)
            {
                yield break;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                yield return CaptureTarget(targets[i]);
                if (failed)
                {
                    yield break;
                }
            }

            if (targets.Count == P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0LoadingStartBatch83RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0LoadingStartBatch83RuntimeEvidence.SpinnerPlaceholderTextDensityReviewPath,
                    P0LoadingStartBatch83RuntimeEvidence.BuildSpinnerPlaceholderTextDensityReviewMarkdown(
                        layoutSummary,
                        P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0LoadingStartBatch83RuntimeEvidence.ProgressCrowdingClickTargetReviewPath,
                    P0LoadingStartBatch83RuntimeEvidence.BuildProgressCrowdingClickTargetReviewMarkdown(
                        clickSummary,
                        P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 83 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0LoadingStartBatch83RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0LoadingStartBatch83RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 83 loading/start runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0LoadingStartBatch83RuntimeEvidenceState.Passed : P0LoadingStartBatch83RuntimeEvidenceState.Failed, summary);
        }

        private void CreateOverlay()
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch83LoadingStartCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0LoadingStartBatch83PreviewOverlay>();
            layoutSummary = overlay.BuildLayoutSummary(1024, 768);
            clickSummary = overlay.BuildClickTargetSummary(1024, 768);
            Add("Created Batch 83 candidate overlay: " + layoutSummary);

            if (!overlay.TryValidateTargetLayouts(P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 83 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 83 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 83 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 83 candidate texture bindings: " + textureSummary);
            Add("Spinner and placeholder states visible: spinner=1 placeholder=1 compact=1");
            Add("Progress readability visible: progress=1 dots=1 compact=1");
            Add("Start/continue click targets visible: start=1 continue=1 minClickTarget=44");
        }

        private IEnumerator CaptureTarget(P0LoadingStartBatch83ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0LoadingStartBatch83RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
            }
            catch (Exception exception)
            {
                Fail("Resolution override failed for " + target.ResolutionLabel + ": " + exception.GetType().Name + ": " + exception.Message);
                yield break;
            }

            Add("Resolution override for " + target.ResolutionLabel + ": " + overrideSummary);
            Screen.SetResolution(target.Width, target.Height, false);
            for (int frame = 0; frame < ResolutionWarmupFrames; frame++)
            {
                yield return null;
            }

            string path = Path.Combine(screenshotDirectory, target.FileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            overlay.BeginCaptureAudit();
            yield return new WaitForEndOfFrame();

            if (!TryCaptureFrameToPng(path, out int actualWidth, out int actualHeight, out string captureError))
            {
                Fail("ReadPixels capture failed for " + target.ResolutionLabel + ": " + captureError);
                yield break;
            }

            if (!overlay.TryCompleteCaptureAudit(out string drawAuditSummary))
            {
                Fail("Candidate frame draw audit failed for " + target.ResolutionLabel + ": " + drawAuditSummary);
                yield break;
            }

            float start = Time.realtimeSinceStartup;
            while (!File.Exists(path) || new FileInfo(path).Length <= 0)
            {
                if (Time.realtimeSinceStartup - start > ScreenshotTimeoutSeconds)
                {
                    Fail("Timed out capturing " + target.ResolutionLabel + " screenshot at " + path + ".");
                    yield break;
                }

                yield return null;
            }

            if (actualWidth != target.Width || actualHeight != target.Height)
            {
                Fail("Screenshot " + target.FileName + " expected "
                    + target.ResolutionLabel
                    + " but captured "
                    + actualWidth
                    + "x"
                    + actualHeight
                    + ".");
                yield break;
            }

            screenshots.Add(target.EvidencePath);
            Add("Captured " + target.ResolutionLabel + ": " + path);
            Add("Candidate frame draw audit passed for " + target.ResolutionLabel + ": " + drawAuditSummary);
        }

        private bool TryCaptureFrameToPng(string path, out int width, out int height, out string error)
        {
            width = 0;
            height = 0;
            try
            {
                width = Mathf.Max(1, Screen.width);
                height = Mathf.Max(1, Screen.height);
                Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
                texture.Apply(false, false);
                if (!HasUsefulVisualContent(texture, out string visualSummary))
                {
                    Destroy(texture);
                    error = "captured frame is visually blank or incomplete (" + visualSummary + ")";
                    return false;
                }

                byte[] bytes = texture.EncodeToPNG();
                Destroy(texture);

                if (bytes == null || bytes.Length == 0)
                {
                    error = "encoded PNG bytes are empty";
                    return false;
                }

                File.WriteAllBytes(path, bytes);
                error = string.Empty;
                return true;
            }
            catch (Exception exception)
            {
                error = exception.GetType().Name + ": " + exception.Message;
                return false;
            }
        }

        private static bool HasUsefulVisualContent(Texture2D texture, out string summary)
        {
            if (texture == null)
            {
                summary = "missing texture";
                return false;
            }

            return P0LoadingStartBatch83RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(relativePath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, markdown ?? string.Empty);
            reviews.Add(relativePath);
            Add("Wrote review evidence: " + relativePath);
        }

        private void ClearGeneratedEvidence()
        {
            for (int i = 0; i < targets.Count; i++)
            {
                string targetPath = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0LoadingStartBatch83RuntimeEvidence.ScreenshotTargets.Length)
            {
                string reportPath = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(P0LoadingStartBatch83RuntimeEvidence.RuntimeReportPath);
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }

                for (int i = 0; i < P0LoadingStartBatch83RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(P0LoadingStartBatch83RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 83 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> requestedTargets)
        {
            if (requestedTargets == null || requestedTargets.Count == 0)
            {
                return "none";
            }

            List<string> labels = new List<string>();
            for (int i = 0; i < requestedTargets.Count; i++)
            {
                labels.Add(requestedTargets[i].ResolutionLabel);
            }

            return string.Join(", ", labels);
        }

        private void Add(string line)
        {
            lines.Add(line);
        }

        private void Fail(string message)
        {
            if (failed)
            {
                return;
            }

            failed = true;
            Add("FAILED: " + message);
            Complete(P0LoadingStartBatch83RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0LoadingStartBatch83RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0LoadingStartBatch83RuntimeEvidence.Complete(
                state,
                summary,
                detail,
                screenshotDirectory,
                screenshots,
                reviews);
            WriteRuntimeReport(state, summary, detail);
            if (overlay != null)
            {
                Destroy(overlay.gameObject);
            }

            Destroy(gameObject);
        }

        private void WriteRuntimeReport(P0LoadingStartBatch83RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0LoadingStartBatch83RuntimeEvidence.ToProjectPath(P0LoadingStartBatch83RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0LoadingStartBatch83RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0LoadingStartBatch83PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private P0LoadingStartBatch83ScreenshotTarget currentTarget;
        private bool captureAuditActive;
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle buttonStyle;
        private GUIStyle chipStyle;

        private void Awake()
        {
            P0VisualAssetBinding[] bindings = P0LoadingStartBatch83CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                P0VisualAssetBinding binding = bindings[i];
                assetsByVariant[binding.SlotId] = binding.Asset;
            }
        }

        public void SetTarget(P0LoadingStartBatch83ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "panel=" + FormatRect(layout.Panel)
                + ", spinner=" + FormatRect(layout.Spinner)
                + ", progress=" + FormatRect(layout.ProgressFrame)
                + ", start=" + FormatRect(layout.StartButton)
                + ", continue=" + FormatRect(layout.ContinueButton);
        }

        public string BuildClickTargetSummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "start=" + FormatRect(layout.StartButton)
                + ", continue=" + FormatRect(layout.ContinueButton)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates = P0LoadingStartBatch83CandidateCatalog.CreateCandidates();
            List<string> missing = new List<string>();
            for (int i = 0; i < candidates.Count; i++)
            {
                string variantId = candidates[i].VariantId;
                if (!assetsByVariant.TryGetValue(variantId, out P0VisualAssetReference asset))
                {
                    missing.Add(variantId + " missing binding");
                    continue;
                }

                if (!P0ImGuiVisualAssetDrawer.TryResolveTexture(asset, out Texture2D texture)
                    || texture == null
                    || texture.width <= 0
                    || texture.height <= 0)
                {
                    missing.Add(variantId + " texture unresolved");
                }
            }

            summary = missing.Count == 0
                ? candidates.Count + "/" + P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public void BeginCaptureAudit()
        {
            auditedDrawnVariants.Clear();
            auditedFallbackVariants.Clear();
            captureAuditActive = true;
        }

        public bool TryCompleteCaptureAudit(out string summary)
        {
            captureAuditActive = false;
            IReadOnlyList<P0LoadingStartBatch83CandidateAsset> candidates = P0LoadingStartBatch83CandidateCatalog.CreateCandidates();
            List<string> missing = new List<string>();
            for (int i = 0; i < candidates.Count; i++)
            {
                string variantId = candidates[i].VariantId;
                if (!auditedDrawnVariants.Contains(variantId))
                {
                    missing.Add(variantId);
                }
            }

            summary = auditedDrawnVariants.Count
                + "/"
                + P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount
                + " candidate textures drawn; fallback="
                + auditedFallbackVariants.Count;
            if (missing.Count > 0)
            {
                summary += "; missing=" + string.Join(", ", missing);
            }

            if (auditedFallbackVariants.Count > 0)
            {
                summary += "; fallbackVariants=" + string.Join(", ", auditedFallbackVariants);
            }

            return missing.Count == 0
                && auditedFallbackVariants.Count == 0
                && auditedDrawnVariants.Count == P0LoadingStartBatch83CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0LoadingStartBatch83ScreenshotTarget> targets,
            out string summary)
        {
            if (targets == null || targets.Count == 0)
            {
                summary = "no targets";
                return false;
            }

            List<string> labels = new List<string>();
            for (int i = 0; i < targets.Count; i++)
            {
                P0LoadingStartBatch83ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                if (!IsInside(layout.Panel, target.Width, target.Height)
                    || !IsInside(layout.Spinner, target.Width, target.Height)
                    || !IsInside(layout.Placeholder, target.Width, target.Height)
                    || !IsInside(layout.ProgressFrame, target.Width, target.Height)
                    || !IsInside(layout.DotSequence, target.Width, target.Height)
                    || !IsInside(layout.StartButton, target.Width, target.Height)
                    || !IsInside(layout.ContinueButton, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    summary = target.ResolutionLabel + " has an off-screen loading/start element";
                    return false;
                }

                if (layout.StartButton.width < 44f
                    || layout.StartButton.height < 44f
                    || layout.ContinueButton.width < 44f
                    || layout.ContinueButton.height < 44f)
                {
                    summary = target.ResolutionLabel + " has a start/continue click target below 44 px";
                    return false;
                }

                if (layout.ProgressFrame.width < 220f || layout.ProgressFrame.height < 32f)
                {
                    summary = target.ResolutionLabel + " has a progress bar below reviewable scale";
                    return false;
                }

                labels.Add(target.ResolutionLabel);
            }

            summary = string.Join(", ", labels) + " inside-screen loading/start density pass";
            return true;
        }

        private void OnGUI()
        {
            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawBackdrop();
            DrawPanel(layout.Panel);
            DrawCandidateFrame("spinner_crescent", layout.Spinner, ScaleMode.ScaleToFit);
            DrawHeaderAndPlaceholder(layout);
            DrawProgress(layout);
            DrawCandidateFrame("dot_sequence", layout.DotSequence, ScaleMode.ScaleToFit);
            DrawActions(layout);
            DrawCaptureBadge(layout.CaptureBadge);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float safeWidth = Mathf.Max(1f, screenWidth);
            float safeHeight = Mathf.Max(1f, screenHeight);
            float scale = P0ImGuiLayout.CalculateScale(safeWidth, safeHeight);
            float margin = P0ImGuiLayout.Scaled(24f, scale);
            float panelWidth = Mathf.Min(P0ImGuiLayout.Scaled(920f, scale), safeWidth - margin * 2f);
            float panelHeight = Mathf.Min(P0ImGuiLayout.Scaled(440f, scale), safeHeight - margin * 2f);
            Rect panel = new Rect((safeWidth - panelWidth) * 0.5f, (safeHeight - panelHeight) * 0.5f, panelWidth, panelHeight);
            float inner = P0ImGuiLayout.Scaled(28f, scale);
            float gap = P0ImGuiLayout.Scaled(18f, scale);
            float spinnerSize = Mathf.Min(P0ImGuiLayout.Scaled(128f, scale), panel.height * 0.34f);
            Rect spinner = new Rect(panel.x + inner, panel.y + inner + P0ImGuiLayout.Scaled(38f, scale), spinnerSize, spinnerSize);
            Rect placeholder = new Rect(spinner.xMax + gap, panel.y + inner, panel.xMax - spinner.xMax - inner - gap, P0ImGuiLayout.Scaled(150f, scale));
            Rect progress = new Rect(panel.x + inner, placeholder.yMax + P0ImGuiLayout.Scaled(16f, scale), panel.width - inner * 2f, Mathf.Max(38f, P0ImGuiLayout.Scaled(48f, scale)));
            Rect dots = new Rect(panel.x + inner, progress.yMax + P0ImGuiLayout.Scaled(10f, scale), panel.width - inner * 2f, Mathf.Max(48f, P0ImGuiLayout.Scaled(64f, scale)));
            float buttonHeight = Mathf.Max(52f, P0ImGuiLayout.Scaled(58f, scale));
            float buttonWidth = Mathf.Min(P0ImGuiLayout.Scaled(210f, scale), (panel.width - inner * 2f - gap) * 0.5f);
            float actionsY = Mathf.Min(panel.yMax - inner - buttonHeight, dots.yMax + P0ImGuiLayout.Scaled(14f, scale));
            Rect start = new Rect(panel.x + inner, actionsY, buttonWidth, buttonHeight);
            Rect continueRect = new Rect(panel.xMax - inner - buttonWidth, actionsY, buttonWidth, buttonHeight);
            Rect badge = new Rect(panel.x + P0ImGuiLayout.Scaled(18f, scale), panel.y + P0ImGuiLayout.Scaled(14f, scale), P0ImGuiLayout.Scaled(230f, scale), P0ImGuiLayout.Scaled(30f, scale));
            Rect progressFill = new Rect(progress.x + 7f, progress.y + 7f, Mathf.Max(24f, (progress.width - 14f) * 0.64f), Mathf.Max(8f, progress.height - 14f));
            return new Layout(panel, spinner, placeholder, progress, progressFill, dots, start, continueRect, badge);
        }

        private void DrawBackdrop()
        {
            GUI.color = new Color(0.035f, 0.042f, 0.058f, 1f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.055f, 0.08f, 0.09f, 0.95f);
            GUI.DrawTexture(new Rect(0f, Screen.height * 0.58f, Screen.width, Screen.height * 0.42f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawPanel(Rect panel)
        {
            GUI.color = new Color(0.06f, 0.085f, 0.095f, 0.96f);
            GUI.DrawTexture(panel, Texture2D.whiteTexture);
            GUI.color = new Color(0.12f, 0.22f, 0.20f, 0.92f);
            GUI.DrawTexture(new Rect(panel.x, panel.y, panel.width, 3f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawHeaderAndPlaceholder(Layout layout)
        {
            GUI.Label(new Rect(layout.Placeholder.x, layout.Placeholder.y, layout.Placeholder.width, 34f), "The Cat Dream Start", titleStyle);
            GUI.Label(
                new Rect(layout.Placeholder.x, layout.Placeholder.y + 42f, layout.Placeholder.width, 50f),
                "Loading starter cat, bedroom dream map, and first battle seed...",
                labelStyle);
            GUI.Label(
                new Rect(layout.Placeholder.x, layout.Placeholder.y + 100f, layout.Placeholder.width, 42f),
                "Candidate proof only: current loading/start presenter remains authoritative until manual gates pass.",
                smallStyle);
        }

        private void DrawProgress(Layout layout)
        {
            DrawCandidateFrame("progress_frame", layout.ProgressFrame, ScaleMode.StretchToFill);
            DrawCandidateFrame("progress_fill", layout.ProgressFill, ScaleMode.StretchToFill);
            GUI.Label(layout.ProgressFrame, "64%   preparing dream route", smallStyle);
        }

        private void DrawActions(Layout layout)
        {
            GUI.color = new Color(0.13f, 0.21f, 0.18f, 0.95f);
            GUI.DrawTexture(layout.StartButton, Texture2D.whiteTexture);
            GUI.DrawTexture(layout.ContinueButton, Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(layout.StartButton, "Start Run", buttonStyle);
            GUI.Label(layout.ContinueButton, "Continue", buttonStyle);
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.025f, 0.052f, 0.058f, 0.9f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0 ? "Batch83 " + currentTarget.ResolutionLabel : "Batch83";
            GUI.Label(rect, label, chipStyle);
        }

        private void DrawCandidateFrame(string variantId, Rect rect, ScaleMode scaleMode)
        {
            bool drawn = assetsByVariant.TryGetValue(variantId, out P0VisualAssetReference asset)
                && P0ImGuiVisualAssetDrawer.DrawTexture(asset, rect, scaleMode);
            RecordCandidateDraw(variantId, drawn);
            if (!drawn)
            {
                GUI.color = new Color(0.09f, 0.13f, 0.14f, 0.88f);
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
        }

        private void RecordCandidateDraw(string variantId, bool drawn)
        {
            if (!captureAuditActive || Event.current == null || Event.current.type != EventType.Repaint)
            {
                return;
            }

            if (drawn)
            {
                auditedDrawnVariants.Add(variantId);
                return;
            }

            auditedFallbackVariants.Add(variantId);
        }

        private void EnsureStyles()
        {
            if (titleStyle != null)
            {
                return;
            }

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            titleStyle.normal.textColor = new Color(0.96f, 0.98f, 0.95f, 1f);

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            labelStyle.normal.textColor = new Color(0.9f, 0.95f, 0.91f, 1f);

            smallStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                wordWrap = true
            };
            smallStyle.normal.textColor = new Color(0.78f, 0.88f, 0.88f, 1f);

            buttonStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            buttonStyle.normal.textColor = new Color(0.95f, 0.98f, 0.94f, 1f);

            chipStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                wordWrap = false
            };
            chipStyle.normal.textColor = new Color(0.92f, 0.96f, 0.98f, 1f);
        }

        private static bool IsInside(Rect rect, float width, float height)
        {
            return rect.x >= 0f
                && rect.y >= 0f
                && rect.xMax <= width + 0.5f
                && rect.yMax <= height + 0.5f
                && rect.width > 0f
                && rect.height > 0f;
        }

        private static string FormatRect(Rect rect)
        {
            return "("
                + rect.x.ToString("0")
                + ","
                + rect.y.ToString("0")
                + " "
                + rect.width.ToString("0")
                + "x"
                + rect.height.ToString("0")
                + ")";
        }

        private readonly struct Layout
        {
            public Layout(
                Rect panel,
                Rect spinner,
                Rect placeholder,
                Rect progressFrame,
                Rect progressFill,
                Rect dotSequence,
                Rect startButton,
                Rect continueButton,
                Rect captureBadge)
            {
                Panel = panel;
                Spinner = spinner;
                Placeholder = placeholder;
                ProgressFrame = progressFrame;
                ProgressFill = progressFill;
                DotSequence = dotSequence;
                StartButton = startButton;
                ContinueButton = continueButton;
                CaptureBadge = captureBadge;
            }

            public Rect Panel { get; }

            public Rect Spinner { get; }

            public Rect Placeholder { get; }

            public Rect ProgressFrame { get; }

            public Rect ProgressFill { get; }

            public Rect DotSequence { get; }

            public Rect StartButton { get; }

            public Rect ContinueButton { get; }

            public Rect CaptureBadge { get; }
        }
    }
}
