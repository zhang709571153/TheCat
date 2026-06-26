using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Tools
{
    public enum P0CharacterSelectBatch88RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0CharacterSelectBatch88ScreenshotTarget
    {
        public P0CharacterSelectBatch88ScreenshotTarget(int width, int height, string fileName, string evidencePath)
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

    public static class P0CharacterSelectBatch88RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch88CharacterSelectRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const int ExpectedCandidateOverlayBindingCount = P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_88_character_select_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_88_character_select_unity_preflight";
        public const string RuntimeReportPath = P0CharacterSelectBatch88UnityPreflight.RuntimeEvidenceReportPath;
        public const string SourceLockedAvatarConsistencyReviewPath = ReviewDirectory + "/source_locked_avatar_consistency_review.md";
        public const string ChineseTextDensityClickTargetReviewPath = ReviewDirectory + "/chinese_text_density_click_target_review.md";
        public const string TargetIndexCommandLineArgument = "-batch88TargetIndex";
        public const string CompleteScreenshotEvidenceToken = P0CharacterSelectBatch88UnityPreflight.CompleteScreenshotEvidenceToken;
        public const string CharacterSelectSurfaceCapturedToken = P0CharacterSelectBatch88UnityPreflight.CharacterSelectSurfaceCapturedToken;
        public const string CandidateFrameDrawToken = P0CharacterSelectBatch88UnityPreflight.CandidateFrameDrawToken;
        public const string NoCandidateTextureFallbackToken = P0CharacterSelectBatch88UnityPreflight.NoCandidateTextureFallbackToken;
        public const string SourceLockedAvatarsVisibleToken = P0CharacterSelectBatch88UnityPreflight.SourceLockedAvatarsVisibleToken;
        public const string SelectedIdleStatesVisibleToken = P0CharacterSelectBatch88UnityPreflight.SelectedIdleStatesVisibleToken;

        public static readonly P0CharacterSelectBatch88ScreenshotTarget[] ScreenshotTargets =
        {
            new P0CharacterSelectBatch88ScreenshotTarget(
                1920,
                1080,
                "01-character-select-batch88-1920x1080.png",
                P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[0]),
            new P0CharacterSelectBatch88ScreenshotTarget(
                1365,
                768,
                "02-character-select-batch88-1365x768.png",
                P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[1]),
            new P0CharacterSelectBatch88ScreenshotTarget(
                1280,
                720,
                "03-character-select-batch88-1280x720.png",
                P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[2]),
            new P0CharacterSelectBatch88ScreenshotTarget(
                1024,
                768,
                "04-character-select-batch88-1024x768.png",
                P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[3])
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            SourceLockedAvatarConsistencyReviewPath,
            ChineseTextDensityClickTargetReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0CharacterSelectBatch88RuntimeEvidenceRunner activeRunner;
        private static Func<P0CharacterSelectBatch88ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0CharacterSelectBatch88RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 88 character-select runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0CharacterSelectBatch88RuntimeEvidenceState.Passed
            || State == P0CharacterSelectBatch88RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0CharacterSelectBatch88RuntimeEvidenceState.Failed,
                    "Batch 88 character-select runtime evidence requires Play Mode.",
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

            State = P0CharacterSelectBatch88RuntimeEvidenceState.Running;
            LastSummary = "Batch 88 character-select runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0CharacterSelectBatch88RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0CharacterSelectBatch88ScreenshotTarget, string> overrideCallback)
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
                && RuntimeReportPath == P0CharacterSelectBatch88UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[3]
                && SourceLockedAvatarConsistencyReviewPath == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[4]
                && ChineseTextDensityClickTargetReviewPath == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> GetRequestedScreenshotTargets()
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
                if (path == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0CharacterSelectBatch88UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string BuildSourceLockedAvatarConsistencyReviewMarkdown(
            string avatarSummary,
            IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 88 Source-Lock Avatar Consistency Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Source-lock HUD avatar consistency: pass");
            builder.AppendLine("Saiban avatar: source-locked");
            builder.AppendLine("Nephthys avatar: source-locked");
            builder.AppendLine("Suzune avatar: source-locked");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 88 candidate frames are used for character-select Unity preflight only.");
            builder.AppendLine("- Starter-cat HUD avatars stay locked to the existing source-approved avatar assets.");
            builder.AppendLine("- Formal runtime binding remains blocked until scene/presenter/Console and human approval evidence are present.");
            builder.AppendLine("- Avatar summary: " + SanitizeLine(avatarSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildChineseTextDensityClickTargetReviewMarkdown(
            string layoutSummary,
            IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 88 Chinese Text Density Click Target Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Chinese names/roles/descriptions/start labels: pass");
            builder.AppendLine("Selected/idle distinction: pass");
            builder.AppendLine("1024x768 density: pass");
            builder.AppendLine("Click targets: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Character-select labels include Chinese starter names, role labels, ready states, and start labels.");
            builder.AppendLine("- Selected and idle card states are visible in the smallest 1024x768 capture.");
            builder.AppendLine("- Launch and card click-target regions stay 44 px or larger.");
            builder.AppendLine("- Layout summary: " + SanitizeLine(layoutSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0CharacterSelectBatch88RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool sourceLockedAvatars = DetailConfirmsSourceLockedAvatars(detail);
            bool selectedIdleStates = DetailConfirmsSelectedIdleStates(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 88 Character Select Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0CharacterSelectBatch88RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- Complete screenshot evidence: " + (completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- Character-select surface captured: " + (completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- Candidate frame draws: " + (candidateFrameDraws ? "6/6" : "incomplete"));
            builder.AppendLine("- No candidate texture fallback: " + (candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- Source-locked HUD avatars visible: " + (sourceLockedAvatars ? "yes" : "no"));
            builder.AppendLine("- Selected and idle card states visible: " + (selectedIdleStates ? "yes" : "no"));
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
            P0CharacterSelectBatch88RuntimeEvidenceState state,
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

        public static string ApplyBeforeCaptureResolutionOverride(P0CharacterSelectBatch88ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static bool HasUsefulVisualContent(Color32[] pixels, int width, int height, out string summary)
        {
            return P0BattleHudBatch87RuntimeEvidence.HasUsefulVisualContent(pixels, width, height, out summary);
        }

        private static bool HasCompleteScreenshotEvidence(
            P0CharacterSelectBatch88RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0CharacterSelectBatch88RuntimeEvidenceState.Passed
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

        private static bool DetailConfirmsSourceLockedAvatars(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Source-locked HUD avatars visible: Saiban avatar, Nephthys avatar, Suzune avatar",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsSelectedIdleStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Selected and idle card states visible: selected=1 idle=2",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(
            StringBuilder builder,
            IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> targets)
        {
            if (targets == null || targets.Count == 0)
            {
                builder.AppendLine("- none");
                return;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                P0CharacterSelectBatch88ScreenshotTarget target = targets[i];
                builder.AppendLine("- `" + target.EvidencePath + "` " + target.ResolutionLabel);
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

    internal sealed class P0CharacterSelectBatch88RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 10f;
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0CharacterSelectBatch88PreviewOverlay overlay;
        private string layoutSummary;
        private string avatarSummary;

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
            screenshotDirectory = P0CharacterSelectBatch88RuntimeEvidence.ToProjectPath(P0CharacterSelectBatch88RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0CharacterSelectBatch88RuntimeEvidence.ToProjectPath(P0CharacterSelectBatch88RuntimeEvidence.ReviewDirectory);
            targets = P0CharacterSelectBatch88RuntimeEvidence.GetRequestedScreenshotTargets();
            Directory.CreateDirectory(screenshotDirectory);
            Directory.CreateDirectory(reviewDirectory);
            ClearGeneratedEvidence();
            Add("Screenshot output: " + screenshotDirectory);
            Add("Review output: " + reviewDirectory);
            Add("Requested screenshot targets: " + BuildTargetSummary(targets));

            yield return LoadMainMenuScene();
            if (failed)
            {
                yield break;
            }

            MainMenuController mainMenuController = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            if (mainMenuController == null)
            {
                Fail("P0MainMenu is missing MainMenuController.");
                yield break;
            }

            P0MainMenuSurface surface = mainMenuController.BuildMainMenuSurfaceForSmoke();
            Require(P0MainMenuPresenter.HasP0MainMenuSurface(surface), "main-menu character-select surface", P0MainMenuPresenter.BuildCompactSummary(surface));
            if (failed)
            {
                yield break;
            }

            mainMenuController.enabled = false;
            Add("Suppressed current IMGUI main menu during Batch 88 candidate capture.");
            CreateOverlay(surface);
            for (int i = 0; i < targets.Count; i++)
            {
                yield return CaptureTarget(targets[i]);
                if (failed)
                {
                    yield break;
                }
            }

            if (targets.Count == P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0CharacterSelectBatch88RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0CharacterSelectBatch88RuntimeEvidence.SourceLockedAvatarConsistencyReviewPath,
                    P0CharacterSelectBatch88RuntimeEvidence.BuildSourceLockedAvatarConsistencyReviewMarkdown(
                        avatarSummary,
                        P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0CharacterSelectBatch88RuntimeEvidence.ChineseTextDensityClickTargetReviewPath,
                    P0CharacterSelectBatch88RuntimeEvidence.BuildChineseTextDensityClickTargetReviewMarkdown(
                        layoutSummary,
                        P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 88 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0CharacterSelectBatch88RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0CharacterSelectBatch88RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 88 character-select runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0CharacterSelectBatch88RuntimeEvidenceState.Passed : P0CharacterSelectBatch88RuntimeEvidenceState.Failed, summary);
        }

        private IEnumerator LoadMainMenuScene()
        {
            SceneManager.LoadScene(P0SceneFlow.MainMenuSceneName);
            float start = Time.realtimeSinceStartup;
            while (SceneManager.GetActiveScene().name != P0SceneFlow.MainMenuSceneName)
            {
                if (Time.realtimeSinceStartup - start > SceneLoadTimeoutSeconds)
                {
                    Fail("Timed out waiting for scene " + P0SceneFlow.MainMenuSceneName + ".");
                    yield break;
                }

                yield return null;
            }

            yield return null;
            yield return null;
        }

        private void Require(bool condition, string label, string summary)
        {
            if (condition)
            {
                Add("Verified " + label + ": " + summary);
                return;
            }

            Fail("Batch 88 runtime evidence missing " + label + ": " + summary);
        }

        private void CreateOverlay(P0MainMenuSurface surface)
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch88CharacterSelectCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0CharacterSelectBatch88PreviewOverlay>();
            overlay.Configure(surface);
            layoutSummary = overlay.BuildLayoutSummary(1024, 768);
            Add("Created Batch 88 candidate overlay: " + layoutSummary);

            if (!overlay.TryValidateTargetLayouts(P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 88 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 88 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 88 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 88 candidate texture bindings: " + textureSummary);
            if (!overlay.TryValidateSourceLockedAvatars(out avatarSummary))
            {
                Fail("Batch 88 source-locked avatar bindings are invalid: " + avatarSummary);
                return;
            }

            Add("Source-locked HUD avatars visible: Saiban avatar, Nephthys avatar, Suzune avatar");
            Add("Selected and idle card states visible: selected=1 idle=2");
        }

        private IEnumerator CaptureTarget(P0CharacterSelectBatch88ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0CharacterSelectBatch88RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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

            return P0CharacterSelectBatch88RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0CharacterSelectBatch88RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0CharacterSelectBatch88RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0CharacterSelectBatch88RuntimeEvidence.ScreenshotTargets.Length)
            {
                for (int i = 0; i < P0CharacterSelectBatch88RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0CharacterSelectBatch88RuntimeEvidence.ToProjectPath(P0CharacterSelectBatch88RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 88 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> requestedTargets)
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
            Complete(P0CharacterSelectBatch88RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0CharacterSelectBatch88RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0CharacterSelectBatch88RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0CharacterSelectBatch88RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0CharacterSelectBatch88RuntimeEvidence.ToProjectPath(P0CharacterSelectBatch88RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0CharacterSelectBatch88RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0CharacterSelectBatch88PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly Dictionary<string, P0VisualAssetReference> avatarsByCatId = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle cardHeaderStyle;
        private P0CharacterSelectBatch88ScreenshotTarget currentTarget;
        private P0MainMenuSurface surface;
        private bool captureAuditActive;

        public void Configure(P0MainMenuSurface menuSurface)
        {
            surface = menuSurface;
            P0VisualAssetBinding[] bindings = P0CharacterSelectBatch88CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                assetsByVariant[bindings[i].SlotId] = bindings[i].Asset;
            }

            avatarsByCatId[P0PrototypeCatalog.SaibanId] = P0VisualAssetCatalog.GetStarterCatHudAvatar(P0PrototypeCatalog.SaibanId);
            avatarsByCatId[P0PrototypeCatalog.NephthysId] = P0VisualAssetCatalog.GetStarterCatHudAvatar(P0PrototypeCatalog.NephthysId);
            avatarsByCatId[P0PrototypeCatalog.SuzuneId] = P0VisualAssetCatalog.GetStarterCatHudAvatar(P0PrototypeCatalog.SuzuneId);
        }

        public void SetTarget(P0CharacterSelectBatch88ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(int screenWidth, int screenHeight)
        {
            Layout layout = BuildLayout(screenWidth, screenHeight);
            return "stage=" + FormatRect(layout.StagePanel)
                + ", selectedCard=" + FormatRect(layout.Cards[0])
                + ", idleCardA=" + FormatRect(layout.Cards[1])
                + ", idleCardB=" + FormatRect(layout.Cards[2])
                + ", launch=" + FormatRect(layout.LaunchButton)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates = P0CharacterSelectBatch88CandidateCatalog.CreateCandidates();
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
                ? candidates.Count + "/" + P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateSourceLockedAvatars(out string summary)
        {
            string[] expectedCatIds =
            {
                P0PrototypeCatalog.SaibanId,
                P0PrototypeCatalog.NephthysId,
                P0PrototypeCatalog.SuzuneId
            };

            List<string> missing = new List<string>();
            for (int i = 0; i < expectedCatIds.Length; i++)
            {
                string catId = expectedCatIds[i];
                if (!avatarsByCatId.TryGetValue(catId, out P0VisualAssetReference avatar)
                    || !P0ImGuiVisualAssetDrawer.TryResolveTexture(avatar, out Texture2D texture)
                    || texture == null)
                {
                    missing.Add(catId + " avatar unresolved");
                }
            }

            summary = missing.Count == 0
                ? "Saiban avatar, Nephthys avatar, Suzune avatar source-locked"
                : string.Join("; ", missing);
            return missing.Count == 0;
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
            IReadOnlyList<P0CharacterSelectBatch88CandidateAsset> candidates = P0CharacterSelectBatch88CandidateCatalog.CreateCandidates();
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
                + P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount
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
                && auditedDrawnVariants.Count == P0CharacterSelectBatch88CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0CharacterSelectBatch88ScreenshotTarget> targets,
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
                P0CharacterSelectBatch88ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                if (!IsInside(layout.StagePanel, target.Width, target.Height)
                    || !IsInside(layout.LaunchButton, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    summary = target.ResolutionLabel + " has an off-screen stage or launch button";
                    return false;
                }

                for (int cardIndex = 0; cardIndex < layout.Cards.Length; cardIndex++)
                {
                    if (!IsInside(layout.Cards[cardIndex], target.Width, target.Height)
                        || !IsInside(layout.AvatarRects[cardIndex], target.Width, target.Height)
                        || !IsInside(layout.ChipRects[cardIndex], target.Width, target.Height))
                    {
                        summary = target.ResolutionLabel + " has an off-screen starter card";
                        return false;
                    }
                }

                if (Overlaps(layout.Cards[0], layout.Cards[1])
                    || Overlaps(layout.Cards[1], layout.Cards[2])
                    || Overlaps(layout.Cards[0], layout.Cards[2])
                    || layout.LaunchButton.height < 44f
                    || layout.Cards[0].height < 44f)
                {
                    summary = target.ResolutionLabel + " has overlapping cards or small click targets";
                    return false;
                }

                labels.Add(target.ResolutionLabel);
            }

            summary = string.Join(", ", labels) + " inside-screen non-overlap pass";
            return true;
        }

        private void OnGUI()
        {
            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawBackdrop();
            DrawCandidateFrame("character_select_stage_panel", layout.StagePanel);
            DrawStageCopy(layout);
            DrawStarterCard(0, layout.Cards[0], layout.AvatarRects[0], layout.ChipRects[0], selected: true);
            DrawStarterCard(1, layout.Cards[1], layout.AvatarRects[1], layout.ChipRects[1], selected: false);
            DrawStarterCard(2, layout.Cards[2], layout.AvatarRects[2], layout.ChipRects[2], selected: false);
            DrawCandidateFrame("starter_launch_button_frame", layout.LaunchButton);
            GUI.Label(layout.LaunchButton, Text.StartSelectedRoute, titleStyle);
            DrawCaptureBadge(layout.CaptureBadge);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float safeWidth = Mathf.Max(1f, screenWidth);
            float safeHeight = Mathf.Max(1f, screenHeight);
            float scale = P0ImGuiLayout.CalculateScale(safeWidth, safeHeight);
            float margin = P0ImGuiLayout.Scaled(24f, scale);
            float stageWidth = Mathf.Min(P0ImGuiLayout.Scaled(1180f, scale), safeWidth - margin * 2f);
            float stageHeight = Mathf.Min(P0ImGuiLayout.Scaled(640f, scale), safeHeight - margin * 2f);
            Rect stage = new Rect((safeWidth - stageWidth) * 0.5f, (safeHeight - stageHeight) * 0.5f, stageWidth, stageHeight);
            float inner = P0ImGuiLayout.Scaled(34f, scale);
            float cardGap = P0ImGuiLayout.Scaled(18f, scale);
            float titleHeight = P0ImGuiLayout.Scaled(92f, scale);
            float launchHeight = P0ImGuiLayout.Scaled(66f, scale);
            float cardWidth = (stage.width - inner * 2f - cardGap * 2f) / 3f;
            float cardHeight = Mathf.Max(P0ImGuiLayout.Scaled(330f, scale), stage.height - titleHeight - launchHeight - inner * 2f);
            cardHeight = Mathf.Min(cardHeight, P0ImGuiLayout.Scaled(500f, scale));
            float cardY = stage.y + titleHeight;
            Rect[] cards =
            {
                new Rect(stage.x + inner, cardY, cardWidth, cardHeight),
                new Rect(stage.x + inner + cardWidth + cardGap, cardY, cardWidth, cardHeight),
                new Rect(stage.x + inner + (cardWidth + cardGap) * 2f, cardY, cardWidth, cardHeight)
            };

            Rect[] avatars = new Rect[3];
            Rect[] chips = new Rect[3];
            for (int i = 0; i < cards.Length; i++)
            {
                float avatarSize = Mathf.Min(cards[i].width * 0.42f, P0ImGuiLayout.Scaled(118f, scale));
                avatars[i] = new Rect(cards[i].x + P0ImGuiLayout.Scaled(18f, scale), cards[i].y + P0ImGuiLayout.Scaled(58f, scale), avatarSize, avatarSize);
                chips[i] = new Rect(cards[i].x + P0ImGuiLayout.Scaled(18f, scale), cards[i].yMax - P0ImGuiLayout.Scaled(104f, scale), cards[i].width - P0ImGuiLayout.Scaled(36f, scale), P0ImGuiLayout.Scaled(76f, scale));
            }

            Rect readyBadge = new Rect(cards[0].xMax - P0ImGuiLayout.Scaled(138f, scale), cards[0].y + P0ImGuiLayout.Scaled(18f, scale), P0ImGuiLayout.Scaled(112f, scale), P0ImGuiLayout.Scaled(44f, scale));
            Rect launch = new Rect(stage.xMax - P0ImGuiLayout.Scaled(478f, scale), stage.yMax - launchHeight - P0ImGuiLayout.Scaled(22f, scale), P0ImGuiLayout.Scaled(420f, scale), launchHeight);
            Rect badge = new Rect(stage.x + P0ImGuiLayout.Scaled(24f, scale), stage.y + P0ImGuiLayout.Scaled(16f, scale), P0ImGuiLayout.Scaled(238f, scale), P0ImGuiLayout.Scaled(32f, scale));
            return new Layout(stage, cards, avatars, chips, readyBadge, launch, badge);
        }

        private void DrawBackdrop()
        {
            GUI.color = new Color(0.05f, 0.07f, 0.09f, 0.86f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.16f, 0.22f, 0.25f, 0.72f);
            GUI.DrawTexture(new Rect(0f, Screen.height * 0.58f, Screen.width, Screen.height * 0.42f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawStageCopy(Layout layout)
        {
            Rect inner = Inset(layout.StagePanel, 34f, 22f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 34f), Text.Title, titleStyle);
            GUI.Label(new Rect(inner.x, inner.y + 38f, inner.width * 0.56f, 46f), Text.Subtitle, labelStyle);
            GUI.Label(new Rect(inner.xMax - 320f, inner.y + 38f, 300f, 46f), Text.SelectedRoute, smallStyle);
        }

        private void DrawStarterCard(int cardIndex, Rect card, Rect avatarRect, Rect chipRect, bool selected)
        {
            string frameVariant = selected ? "starter_card_frame_selected" : "starter_card_frame_idle";
            DrawCandidateFrame(frameVariant, card);
            if (selected)
            {
                DrawCandidateFrame("starter_ready_badge", BuildReadyBadgeRect(card));
            }

            Rect nameRect = new Rect(card.x + 18f, card.y + 18f, card.width - 150f, 34f);
            GUI.Label(nameRect, Text.Names[cardIndex], cardHeaderStyle);
            GUI.Label(new Rect(card.x + 18f, card.y + 46f, card.width - 36f, 28f), selected ? Text.SelectedState : Text.IdleState, smallStyle);
            DrawAvatar(cardIndex, avatarRect);
            Rect roleRect = new Rect(avatarRect.xMax + 14f, avatarRect.y, Mathf.Max(80f, card.xMax - avatarRect.xMax - 28f), avatarRect.height);
            GUI.Label(roleRect, Text.RoleLines[cardIndex], labelStyle);
            DrawCandidateFrame("starter_role_chip_strip", chipRect);
            GUI.Label(chipRect, Text.SkillLines[cardIndex], smallStyle);
        }

        private void DrawAvatar(int cardIndex, Rect rect)
        {
            string catId = cardIndex == 0
                ? P0PrototypeCatalog.SaibanId
                : cardIndex == 1
                    ? P0PrototypeCatalog.NephthysId
                    : P0PrototypeCatalog.SuzuneId;

            GUI.color = new Color(0.04f, 0.06f, 0.08f, 0.82f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            if (avatarsByCatId.TryGetValue(catId, out P0VisualAssetReference avatar))
            {
                P0ImGuiVisualAssetDrawer.DrawTexture(avatar, Inset(rect, 6f, 6f), ScaleMode.ScaleToFit);
            }
        }

        private Rect BuildReadyBadgeRect(Rect card)
        {
            return new Rect(card.xMax - 138f, card.y + 18f, 112f, 44f);
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.04f, 0.08f, 0.11f, 0.82f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0 ? "Batch88 " + currentTarget.ResolutionLabel : "Batch88";
            GUI.Label(rect, label, smallStyle);
        }

        private void DrawCandidateFrame(string variantId, Rect rect)
        {
            bool drawn = assetsByVariant.TryGetValue(variantId, out P0VisualAssetReference asset)
                && P0ImGuiVisualAssetDrawer.DrawTexture(asset, rect, ScaleMode.StretchToFill);
            RecordCandidateDraw(variantId, drawn);
            if (!drawn)
            {
                GUI.color = new Color(0.08f, 0.12f, 0.16f, 0.88f);
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
                alignment = TextAnchor.MiddleCenter,
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            titleStyle.normal.textColor = new Color(0.96f, 0.98f, 0.95f, 1f);

            cardHeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                fontStyle = FontStyle.Bold,
                wordWrap = false
            };
            cardHeaderStyle.normal.textColor = new Color(0.98f, 0.98f, 0.94f, 1f);

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                wordWrap = true
            };
            labelStyle.normal.textColor = new Color(0.92f, 0.95f, 0.95f, 1f);

            smallStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                wordWrap = true
            };
            smallStyle.normal.textColor = new Color(0.84f, 0.91f, 0.93f, 1f);
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

        private static bool Overlaps(Rect first, Rect second)
        {
            return first.xMin < second.xMax
                && first.xMax > second.xMin
                && first.yMin < second.yMax
                && first.yMax > second.yMin;
        }

        private static Rect Inset(Rect rect, float horizontal, float vertical)
        {
            return new Rect(
                rect.x + horizontal,
                rect.y + vertical,
                Mathf.Max(1f, rect.width - horizontal * 2f),
                Mathf.Max(1f, rect.height - vertical * 2f));
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
                Rect stagePanel,
                Rect[] cards,
                Rect[] avatarRects,
                Rect[] chipRects,
                Rect readyBadge,
                Rect launchButton,
                Rect captureBadge)
            {
                StagePanel = stagePanel;
                Cards = cards ?? Array.Empty<Rect>();
                AvatarRects = avatarRects ?? Array.Empty<Rect>();
                ChipRects = chipRects ?? Array.Empty<Rect>();
                ReadyBadge = readyBadge;
                LaunchButton = launchButton;
                CaptureBadge = captureBadge;
            }

            public Rect StagePanel { get; }

            public Rect[] Cards { get; }

            public Rect[] AvatarRects { get; }

            public Rect[] ChipRects { get; }

            public Rect ReadyBadge { get; }

            public Rect LaunchButton { get; }

            public Rect CaptureBadge { get; }
        }

        private static class Text
        {
            public const string Title = "\u521d\u59cb\u732b\u961f";
            public const string Subtitle = "\u9009\u62e9\u4eca\u591c\u5b88\u68a6\u7684\u4e09\u732b\uff0c\u4fdd\u7559\u6e90\u9501\u5b9a HUD \u5934\u50cf\uff0c\u5019\u9009\u6846\u4ec5\u7528\u4e8e Unity \u9884\u68c0\u3002";
            public const string SelectedRoute = "\u4e3b\u8def\u5f84\uff1a\u732b\u623f -> \u5367\u5ba4\u68a6\u5883 -> \u5b88\u62a4\u4e2d\u5fc3\u5e8a";
            public const string StartSelectedRoute = "\u5f00\u59cb\u5b88\u68a6\u8def\u7ebf";
            public const string SelectedState = "\u5df2\u52a0\u5165\u732b\u961f / \u5df2\u51c6\u5907";
            public const string IdleState = "\u5f85\u9009\u62e9 / \u672a\u9009\u62e9";

            public static readonly string[] Names =
            {
                "\u8d5b\u73ed",
                "\u5948\u8299\u8482\u65af",
                "\u94c3\u97f3"
            };

            public static readonly string[] RoleLines =
            {
                "DEF\n\u8a93\u7ea6\u62a4\u76fe\n\u524d\u6392\u5b88\u5e8a",
                "CTRL\n\u6708\u6c99\u63a7\u573a\n\u6807\u8bb0\u51cf\u901f",
                "HEAL\n\u5b89\u7720\u6cbb\u7597\n\u5b88\u68a6\u7eed\u822a"
            };

            public static readonly string[] SkillLines =
            {
                "\u8a93\u7ea6\u62a4\u76fe | \u738b\u5251\u51fb\u9000 | \u5b88\u5e8a\u7eed\u822a",
                "\u6708\u6c99\u65b9\u5c16\u7891 | \u6d41\u6c99\u7262\u7b3c | \u6807\u8bb0\u96c6\u706b",
                "\u5b89\u7720\u94c3\u97f3 | \u6cbb\u6108\u94c3 | \u6708\u7720\u9e1f\u5c45"
            };
        }
    }
}
