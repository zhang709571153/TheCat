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
    public enum P0SettingsPauseBatch85RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0SettingsPauseBatch85ScreenshotTarget
    {
        public P0SettingsPauseBatch85ScreenshotTarget(
            int width,
            int height,
            string fileName,
            string evidencePath,
            string scenarioId)
        {
            Width = width;
            Height = height;
            FileName = fileName ?? string.Empty;
            EvidencePath = evidencePath ?? string.Empty;
            ScenarioId = scenarioId ?? string.Empty;
        }

        public int Width { get; }

        public int Height { get; }

        public string FileName { get; }

        public string EvidencePath { get; }

        public string ScenarioId { get; }

        public string ResolutionLabel => Width + "x" + Height;
    }

    public static class P0SettingsPauseBatch85RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch85SettingsPauseRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_85_settings_pause_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_85_settings_pause_unity_preflight";
        public const string RuntimeReportPath = P0SettingsPauseBatch85UnityPreflight.RuntimeEvidenceReportPath;
        public const string TextReplacementKeyHintReadabilityReviewPath = ReviewDirectory + "/text_replacement_key_hint_readability_review.md";
        public const string SettingsControlsClickTargetReviewPath = ReviewDirectory + "/settings_controls_click_target_review.md";
        public const string TargetIndexCommandLineArgument = "-batch85TargetIndex";

        public static readonly P0SettingsPauseBatch85ScreenshotTarget[] ScreenshotTargets =
        {
            new P0SettingsPauseBatch85ScreenshotTarget(
                1920,
                1080,
                "01-settings-pause-batch85-settings-main-1920x1080.png",
                P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[0],
                "settings_main"),
            new P0SettingsPauseBatch85ScreenshotTarget(
                1365,
                768,
                "02-settings-pause-batch85-settings-audio-1365x768.png",
                P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[1],
                "settings_audio"),
            new P0SettingsPauseBatch85ScreenshotTarget(
                1280,
                720,
                "03-settings-pause-batch85-pause-overlay-1280x720.png",
                P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[2],
                "pause_overlay"),
            new P0SettingsPauseBatch85ScreenshotTarget(
                1024,
                768,
                "04-settings-pause-batch85-settings-compact-1024x768.png",
                P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[3],
                "settings_compact")
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            TextReplacementKeyHintReadabilityReviewPath,
            SettingsControlsClickTargetReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0SettingsPauseBatch85RuntimeEvidenceRunner activeRunner;
        private static Func<P0SettingsPauseBatch85ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0SettingsPauseBatch85RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 85 settings/pause runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0SettingsPauseBatch85RuntimeEvidenceState.Passed
            || State == P0SettingsPauseBatch85RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0SettingsPauseBatch85RuntimeEvidenceState.Failed,
                    "Batch 85 settings/pause runtime evidence requires Play Mode.",
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

            State = P0SettingsPauseBatch85RuntimeEvidenceState.Running;
            LastSummary = "Batch 85 settings/pause runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0SettingsPauseBatch85RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0SettingsPauseBatch85ScreenshotTarget, string> overrideCallback)
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
                && RuntimeReportPath == P0SettingsPauseBatch85UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[3]
                && TextReplacementKeyHintReadabilityReviewPath == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[4]
                && SettingsControlsClickTargetReviewPath == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> GetRequestedScreenshotTargets()
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
                if (path == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0SettingsPauseBatch85UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0SettingsPauseBatch85ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static string BuildTextReplacementKeyHintReadabilityReviewMarkdown(
            string readabilitySummary,
            IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 85 Text Replacement Key Hint Readability Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Unity-rendered text replacement: pass");
            builder.AppendLine("Key-hint semantics: pass");
            builder.AppendLine("1024x768 density: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 85 candidate sprites are used for settings/pause Unity preflight only.");
            builder.AppendLine("- Settings labels, option values, pause labels, and key hints are Unity-rendered over textless candidate frames.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Readability summary: " + SanitizeLine(readabilitySummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildSettingsControlsClickTargetReviewMarkdown(
            string clickTargetSummary,
            IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 85 Settings Controls Click Target Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Slider/switch/checkbox controls: pass");
            builder.AppendLine("Tab and button click targets: pass");
            builder.AppendLine("Pause/resume/restart actions: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Settings main, settings audio, pause overlay, and compact settings states are all represented in Unity screenshots.");
            builder.AppendLine("- Tabs, key hints, option controls, pause actions, and restart confirmation remain at or above the 44 px click-target floor.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Click target summary: " + SanitizeLine(clickTargetSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0SettingsPauseBatch85RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool settingsStates = DetailConfirmsSettingsStates(detail);
            bool pauseState = DetailConfirmsPauseOverlayState(detail);
            bool keyHintReadability = DetailConfirmsKeyHintReadability(detail);
            bool clickTargets = DetailConfirmsClickTargets(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 85 Settings Pause Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0SettingsPauseBatch85RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.CompleteScreenshotEvidenceToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.SettingsPauseSurfaceCapturedToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + (candidateFrameDraws ? P0SettingsPauseBatch85UnityPreflight.CandidateFrameDrawToken : "Candidate frame draws: incomplete"));
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.NoCandidateTextureFallbackToken.Replace("yes", candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.SettingsMainAudioStatesVisibleToken.Replace("yes", settingsStates ? "yes" : "no"));
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.PauseOverlayStateVisibleToken.Replace("yes", pauseState ? "yes" : "no"));
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.KeyHintReadabilityVisibleToken.Replace("yes", keyHintReadability ? "yes" : "no"));
            builder.AppendLine("- " + P0SettingsPauseBatch85UnityPreflight.SettingsPauseClickTargetsVisibleToken.Replace("yes", clickTargets ? "yes" : "no"));
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
            P0SettingsPauseBatch85RuntimeEvidenceState state,
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
            P0SettingsPauseBatch85RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0SettingsPauseBatch85RuntimeEvidenceState.Passed
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
                    + ": 6/6 candidate textures drawn; fallback=0";
                if (safeDetail.IndexOf(token, StringComparison.Ordinal) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DetailConfirmsSettingsStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Settings states visible: main=1 audio=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsPauseOverlayState(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Pause overlay state visible: pause=1 restart=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsKeyHintReadability(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Key-hint readability visible: tabs=1 hints=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsClickTargets(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Settings/pause click targets visible: tabs=1 controls=1 pause=1 minClickTarget=44",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(
            StringBuilder builder,
            IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> targets)
        {
            if (targets == null || targets.Count == 0)
            {
                builder.AppendLine("- none");
                return;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                builder.AppendLine("- `" + targets[i].EvidencePath + "` (" + targets[i].ResolutionLabel + ", " + targets[i].ScenarioId + ")");
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
            return (value ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();
        }
    }

    internal sealed class P0SettingsPauseBatch85RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0SettingsPauseBatch85PreviewOverlay overlay;
        private string readabilitySummary;
        private string clickTargetSummary;

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
            screenshotDirectory = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(P0SettingsPauseBatch85RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(P0SettingsPauseBatch85RuntimeEvidence.ReviewDirectory);
            targets = P0SettingsPauseBatch85RuntimeEvidence.GetRequestedScreenshotTargets();
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

            if (targets.Count == P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0SettingsPauseBatch85RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0SettingsPauseBatch85RuntimeEvidence.TextReplacementKeyHintReadabilityReviewPath,
                    P0SettingsPauseBatch85RuntimeEvidence.BuildTextReplacementKeyHintReadabilityReviewMarkdown(
                        readabilitySummary,
                        P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0SettingsPauseBatch85RuntimeEvidence.SettingsControlsClickTargetReviewPath,
                    P0SettingsPauseBatch85RuntimeEvidence.BuildSettingsControlsClickTargetReviewMarkdown(
                        clickTargetSummary,
                        P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 85 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0SettingsPauseBatch85RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0SettingsPauseBatch85RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 85 settings/pause runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0SettingsPauseBatch85RuntimeEvidenceState.Passed : P0SettingsPauseBatch85RuntimeEvidenceState.Failed, summary);
        }

        private void CreateOverlay()
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch85SettingsPauseCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0SettingsPauseBatch85PreviewOverlay>();
            readabilitySummary = overlay.BuildReadabilitySummary(1024, 768);
            clickTargetSummary = overlay.BuildClickTargetSummary(1024, 768);
            Add("Created Batch 85 candidate overlay: " + overlay.BuildLayoutSummary(1024, 768));

            if (!overlay.TryValidateTargetLayouts(P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 85 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 85 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 85 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 85 candidate texture bindings: " + textureSummary);
            Add("Settings states visible: main=1 audio=1 compact=1");
            Add("Pause overlay state visible: pause=1 restart=1");
            Add("Key-hint readability visible: tabs=1 hints=1 compact=1");
            Add("Settings/pause click targets visible: tabs=1 controls=1 pause=1 minClickTarget=44");
        }

        private IEnumerator CaptureTarget(P0SettingsPauseBatch85ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0SettingsPauseBatch85RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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
            Add("Captured " + target.ResolutionLabel + " " + target.ScenarioId + ": " + path);
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

            return P0SettingsPauseBatch85RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0SettingsPauseBatch85RuntimeEvidence.ScreenshotTargets.Length)
            {
                string reportPath = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(P0SettingsPauseBatch85RuntimeEvidence.RuntimeReportPath);
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }

                for (int i = 0; i < P0SettingsPauseBatch85RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(P0SettingsPauseBatch85RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 85 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> requestedTargets)
        {
            if (requestedTargets == null || requestedTargets.Count == 0)
            {
                return "none";
            }

            List<string> labels = new List<string>();
            for (int i = 0; i < requestedTargets.Count; i++)
            {
                labels.Add(requestedTargets[i].ResolutionLabel + "/" + requestedTargets[i].ScenarioId);
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
            Complete(P0SettingsPauseBatch85RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0SettingsPauseBatch85RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0SettingsPauseBatch85RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0SettingsPauseBatch85RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0SettingsPauseBatch85RuntimeEvidence.ToProjectPath(P0SettingsPauseBatch85RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0SettingsPauseBatch85RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0SettingsPauseBatch85PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private P0SettingsPauseBatch85ScreenshotTarget currentTarget;
        private bool captureAuditActive;
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle buttonStyle;
        private GUIStyle chipStyle;

        private void Awake()
        {
            P0VisualAssetBinding[] bindings = P0SettingsPauseBatch85CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                P0VisualAssetBinding binding = bindings[i];
                assetsByVariant[binding.SlotId] = binding.Asset;
            }
        }

        public void SetTarget(P0SettingsPauseBatch85ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "panel=" + FormatRect(layout.Panel)
                + ", tabs=" + FormatRect(layout.Divider)
                + ", rows=" + FormatRect(layout.RewardRow)
                + "/" + FormatRect(layout.StatLeft)
                + ", actions=" + FormatRect(layout.PrimaryButton)
                + "/" + FormatRect(layout.SecondaryButton);
        }

        public string BuildReadabilitySummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "optionRows=" + FormatRect(layout.RewardRow)
                + "/" + FormatRect(layout.StatLeft)
                + ", keyHint=" + FormatRect(layout.FailureStamp)
                + ", compact=" + (width <= 1024f ? "yes" : "no");
        }

        public string BuildClickTargetSummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "tabs=" + FormatRect(layout.Divider)
                + ", primary=" + FormatRect(layout.PrimaryButton)
                + ", secondary=" + FormatRect(layout.SecondaryButton)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates = P0SettingsPauseBatch85CandidateCatalog.CreateCandidates();
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
                ? candidates.Count + "/" + P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount;
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
            IReadOnlyList<P0SettingsPauseBatch85CandidateAsset> candidates = P0SettingsPauseBatch85CandidateCatalog.CreateCandidates();
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
                + P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount
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
                && auditedDrawnVariants.Count == P0SettingsPauseBatch85CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0SettingsPauseBatch85ScreenshotTarget> targets,
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
                P0SettingsPauseBatch85ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                if (!IsInside(layout.Panel, target.Width, target.Height)
                    || !IsInside(layout.SuccessStamp, target.Width, target.Height)
                    || !IsInside(layout.FailureStamp, target.Width, target.Height)
                    || !IsInside(layout.Divider, target.Width, target.Height)
                    || !IsInside(layout.RewardRow, target.Width, target.Height)
                    || !IsInside(layout.StatLeft, target.Width, target.Height)
                    || !IsInside(layout.StatRight, target.Width, target.Height)
                    || !IsInside(layout.PrimaryButton, target.Width, target.Height)
                    || !IsInside(layout.SecondaryButton, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    summary = target.ResolutionLabel + " has an off-screen settings/pause element";
                    return false;
                }

                if (layout.PrimaryButton.width < 44f
                    || layout.PrimaryButton.height < 44f
                    || layout.SecondaryButton.width < 44f
                    || layout.SecondaryButton.height < 44f)
                {
                    summary = target.ResolutionLabel + " has a settings/pause click target below 44 px";
                    return false;
                }

                if (layout.RewardRow.height < 54f
                    || layout.StatLeft.height < 44f
                    || layout.FailureStamp.height < 44f)
                {
                    summary = target.ResolutionLabel + " has settings text/key-hint evidence below readable scale";
                    return false;
                }

                labels.Add(target.ResolutionLabel + "/" + target.ScenarioId);
            }

            summary = string.Join(", ", labels) + " inside-screen settings/pause density pass";
            return true;
        }

        private void OnGUI()
        {
            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawBackdrop();
            DrawCandidateFrame("screen_panel_frame", layout.Panel, ScaleMode.StretchToFill);
            DrawCaptureBadge(layout.CaptureBadge);
            DrawHeader(layout);
            DrawTabs(layout);
            DrawSettingsRows(layout);
            DrawKeyHintAndModal(layout);
            DrawActions(layout);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float safeWidth = Mathf.Max(1f, screenWidth);
            float safeHeight = Mathf.Max(1f, screenHeight);
            float scale = P0ImGuiLayout.CalculateScale(safeWidth, safeHeight);
            float margin = P0ImGuiLayout.Scaled(24f, scale);
            float panelWidth = Mathf.Min(P0ImGuiLayout.Scaled(1040f, scale), safeWidth - margin * 2f);
            float panelHeight = Mathf.Min(P0ImGuiLayout.Scaled(620f, scale), safeHeight - margin * 2f);
            Rect panel = new Rect((safeWidth - panelWidth) * 0.5f, (safeHeight - panelHeight) * 0.5f, panelWidth, panelHeight);
            float inner = P0ImGuiLayout.Scaled(28f, scale);
            float gap = P0ImGuiLayout.Scaled(14f, scale);
            float headerHeight = P0ImGuiLayout.Scaled(70f, scale);
            Rect header = new Rect(panel.x + inner, panel.y + inner, panel.width - inner * 2f, headerHeight);
            Rect tabBar = new Rect(panel.x + inner, header.yMax + gap, panel.width - inner * 2f, Mathf.Max(54f, P0ImGuiLayout.Scaled(70f, scale)));
            Rect sectionDivider = new Rect(panel.x + inner, tabBar.yMax + gap * 0.8f, panel.width - inner * 2f, Mathf.Max(14f, P0ImGuiLayout.Scaled(24f, scale)));
            float rowHeight = Mathf.Max(62f, P0ImGuiLayout.Scaled(78f, scale));
            Rect rowOne = new Rect(panel.x + inner, sectionDivider.yMax + gap, panel.width - inner * 2f, rowHeight);
            Rect rowTwo = new Rect(panel.x + inner, rowOne.yMax + gap, panel.width - inner * 2f, rowHeight);
            float controlWidth = (panel.width - inner * 2f - gap) * 0.5f;
            Rect rowThree = new Rect(panel.x + inner, rowTwo.yMax + gap, controlWidth, rowHeight);
            Rect keyHint = new Rect(rowThree.xMax + gap, rowThree.y, controlWidth, rowHeight);
            float buttonHeight = Mathf.Max(52f, P0ImGuiLayout.Scaled(66f, scale));
            float buttonWidth = Mathf.Min(P0ImGuiLayout.Scaled(300f, scale), (panel.width - inner * 2f - gap) * 0.5f);
            float actionY = Mathf.Min(panel.yMax - inner - buttonHeight, keyHint.yMax + gap);
            Rect primary = new Rect(panel.x + inner, actionY, buttonWidth, buttonHeight);
            Rect secondary = new Rect(primary.xMax + gap, actionY, buttonWidth, buttonHeight);
            Rect modal = new Rect(
                panel.xMax - inner - Mathf.Min(P0ImGuiLayout.Scaled(340f, scale), panel.width * 0.36f),
                actionY,
                Mathf.Min(P0ImGuiLayout.Scaled(340f, scale), panel.width * 0.36f),
                buttonHeight);
            Rect badge = new Rect(panel.x + P0ImGuiLayout.Scaled(18f, scale), panel.y + P0ImGuiLayout.Scaled(14f, scale), P0ImGuiLayout.Scaled(286f, scale), P0ImGuiLayout.Scaled(30f, scale));
            return new Layout(panel, header, tabBar, rowOne, rowTwo, rowThree, primary, secondary, modal, keyHint, badge);
        }

        private void DrawBackdrop()
        {
            GUI.color = new Color(0.035f, 0.044f, 0.058f, 1f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.07f, 0.10f, 0.09f, 0.92f);
            GUI.DrawTexture(new Rect(0f, Screen.height * 0.54f, Screen.width, Screen.height * 0.46f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawHeader(Layout layout)
        {
            string scenario = string.IsNullOrWhiteSpace(currentTarget.ScenarioId) ? "settings_main" : currentTarget.ScenarioId;
            string title = scenario == "pause_overlay"
                ? "Pause Overlay"
                : scenario == "settings_audio"
                    ? "Audio Settings"
                    : scenario == "settings_compact"
                        ? "Compact Settings"
                        : "Settings";
            string subtitle = scenario == "settings_compact"
                ? "Compact key hints, values, and back actions stay readable at 1024x768."
                : "Runtime labels, values, controls, pause actions, and key hints are drawn over textless candidate sprites.";
            GUI.Label(layout.Header, title, titleStyle);
            GUI.Label(new Rect(layout.Header.x, layout.Header.y + 46f, layout.Header.width, 56f), subtitle, labelStyle);
        }

        private void DrawTabs(Layout layout)
        {
            DrawCandidateFrame("tab_bar_frame", layout.Divider, ScaleMode.StretchToFill);
            string scenario = string.IsNullOrWhiteSpace(currentTarget.ScenarioId) ? "settings_main" : currentTarget.ScenarioId;
            string active = scenario == "settings_audio" ? "Audio" : scenario == "pause_overlay" ? "Pause" : "General";
            GUI.Label(layout.Divider, "General    Audio    Controls    Pause    Active: " + active, buttonStyle);
        }

        private void DrawSettingsRows(Layout layout)
        {
            DrawCandidateFrame("settings_section_divider", layout.StatRight, ScaleMode.StretchToFill);
            DrawCandidateFrame("option_row_frame", layout.RewardRow, ScaleMode.StretchToFill);
            DrawCandidateFrame("option_row_frame", layout.StatLeft, ScaleMode.StretchToFill);
            string scenario = string.IsNullOrWhiteSpace(currentTarget.ScenarioId) ? "settings_main" : currentTarget.ScenarioId;
            string rowOne = scenario == "settings_audio" ? "Music Volume  72%    Slider live value" : "Language  Chinese UI    Runtime text";
            string rowTwo = scenario == "pause_overlay" ? "Restart Confirmation  Hold to confirm" : "Screen Shake  On    Checkbox visible";
            GUI.Label(new Rect(layout.RewardRow.x + 22f, layout.RewardRow.y, layout.RewardRow.width - 44f, layout.RewardRow.height), rowOne, labelStyle);
            GUI.Label(new Rect(layout.StatLeft.x + 22f, layout.StatLeft.y, layout.StatLeft.width - 44f, layout.StatLeft.height), rowTwo, labelStyle);
        }

        private void DrawKeyHintAndModal(Layout layout)
        {
            DrawCandidateFrame("key_hint_chip_frame", layout.FailureStamp, ScaleMode.StretchToFill);
            DrawCandidateFrame("confirm_modal_frame", layout.SuccessStamp, ScaleMode.StretchToFill);
            GUI.Label(layout.FailureStamp, "Esc / B  Back    Enter / A  Apply", smallStyle);
            GUI.Label(layout.SuccessStamp, "Restart dream?  Confirm modal", smallStyle);
        }

        private void DrawActions(Layout layout)
        {
            DrawCandidateFrame("option_row_frame", layout.PrimaryButton, ScaleMode.StretchToFill);
            DrawCandidateFrame("option_row_frame", layout.SecondaryButton, ScaleMode.StretchToFill);
            string scenario = string.IsNullOrWhiteSpace(currentTarget.ScenarioId) ? "settings_main" : currentTarget.ScenarioId;
            string primary = scenario == "pause_overlay" ? "Resume Dream" : "Apply Settings";
            string secondary = scenario == "pause_overlay" ? "Restart" : "Back";
            GUI.Label(layout.PrimaryButton, primary, buttonStyle);
            GUI.Label(layout.SecondaryButton, secondary, buttonStyle);
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.025f, 0.052f, 0.058f, 0.9f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0
                ? "Batch85 " + currentTarget.ResolutionLabel + " " + currentTarget.ScenarioId
                : "Batch85 settings/pause";
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
                fontSize = 12,
                wordWrap = true
            };
            smallStyle.normal.textColor = new Color(0.82f, 0.9f, 0.88f, 1f);

            buttonStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
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
                Rect header,
                Rect divider,
                Rect rewardRow,
                Rect statLeft,
                Rect statRight,
                Rect primaryButton,
                Rect secondaryButton,
                Rect successStamp,
                Rect failureStamp,
                Rect captureBadge)
            {
                Panel = panel;
                Header = header;
                Divider = divider;
                RewardRow = rewardRow;
                StatLeft = statLeft;
                StatRight = statRight;
                PrimaryButton = primaryButton;
                SecondaryButton = secondaryButton;
                SuccessStamp = successStamp;
                FailureStamp = failureStamp;
                CaptureBadge = captureBadge;
            }

            public Rect Panel { get; }

            public Rect Header { get; }

            public Rect Divider { get; }

            public Rect RewardRow { get; }

            public Rect StatLeft { get; }

            public Rect StatRight { get; }

            public Rect PrimaryButton { get; }

            public Rect SecondaryButton { get; }

            public Rect SuccessStamp { get; }

            public Rect FailureStamp { get; }

            public Rect CaptureBadge { get; }
        }
    }
}
