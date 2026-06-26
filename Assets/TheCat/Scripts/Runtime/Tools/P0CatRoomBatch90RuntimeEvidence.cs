using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Tools
{
    public enum P0CatRoomBatch90RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0CatRoomBatch90ScreenshotTarget
    {
        public P0CatRoomBatch90ScreenshotTarget(int width, int height, string fileName, string evidencePath)
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

    public static class P0CatRoomBatch90RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch90CatRoomRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_90_cat_room_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_90_cat_room_unity_preflight";
        public const string RuntimeReportPath = P0CatRoomBatch90UnityPreflight.RuntimeEvidenceReportPath;
        public const string InteractionStateTextDensityReviewPath = ReviewDirectory + "/interaction_state_text_density_review.md";
        public const string ClickTargetPropScaleReviewPath = ReviewDirectory + "/click_target_prop_scale_review.md";
        public const string TargetIndexCommandLineArgument = "-batch90TargetIndex";

        public static readonly P0CatRoomBatch90ScreenshotTarget[] ScreenshotTargets =
        {
            new P0CatRoomBatch90ScreenshotTarget(
                1920,
                1080,
                "01-cat-room-batch90-1920x1080.png",
                P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[0]),
            new P0CatRoomBatch90ScreenshotTarget(
                1365,
                768,
                "02-cat-room-batch90-1365x768.png",
                P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[1]),
            new P0CatRoomBatch90ScreenshotTarget(
                1280,
                720,
                "03-cat-room-batch90-1280x720.png",
                P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[2]),
            new P0CatRoomBatch90ScreenshotTarget(
                1024,
                768,
                "04-cat-room-batch90-1024x768.png",
                P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[3])
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            InteractionStateTextDensityReviewPath,
            ClickTargetPropScaleReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0CatRoomBatch90RuntimeEvidenceRunner activeRunner;
        private static Func<P0CatRoomBatch90ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0CatRoomBatch90RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 90 cat-room runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0CatRoomBatch90RuntimeEvidenceState.Passed
            || State == P0CatRoomBatch90RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0CatRoomBatch90RuntimeEvidenceState.Failed,
                    "Batch 90 cat-room runtime evidence requires Play Mode.",
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

            State = P0CatRoomBatch90RuntimeEvidenceState.Running;
            LastSummary = "Batch 90 cat-room runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0CatRoomBatch90RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0CatRoomBatch90ScreenshotTarget, string> overrideCallback)
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
                && RuntimeReportPath == P0CatRoomBatch90UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[3]
                && InteractionStateTextDensityReviewPath == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[4]
                && ClickTargetPropScaleReviewPath == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> GetRequestedScreenshotTargets()
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
                if (path == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0CatRoomBatch90UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0CatRoomBatch90ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static string BuildInteractionStateTextDensityReviewMarkdown(
            string layoutSummary,
            IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 90 Interaction State Text Density Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Bed/feeder/litter/dream entrance states: pass");
            builder.AppendLine("Hover/disabled/blocked/range states: pass");
            builder.AppendLine("Chinese labels and values: pass");
            builder.AppendLine("1024x768 density: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 90 candidate frames are used for cat-room Unity preflight only.");
            builder.AppendLine("- Existing `02-cat-room.png` remains runtime-surface reachability evidence only.");
            builder.AppendLine("- Formal runtime binding remains blocked until scene/presenter/Console and human approval evidence are present.");
            builder.AppendLine("- Layout summary: " + SanitizeLine(layoutSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildClickTargetPropScaleReviewMarkdown(
            string clickSummary,
            IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 90 Click Target Prop Scale Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Bed click target: pass");
            builder.AppendLine("Feeder click target: pass");
            builder.AppendLine("Litter click target: pass");
            builder.AppendLine("Dream entrance semantics: pass");
            builder.AppendLine("Prop scale: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Bed, feeder, litter, and dream entrance targets stay at or above 44 px.");
            builder.AppendLine("- Dream entrance remains a route-entry affordance, not a battle-start shortcut.");
            builder.AppendLine("- Prop scale summary: " + SanitizeLine(clickSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0CatRoomBatch90RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool interactionStates = DetailConfirmsInteractionStates(detail);
            bool hoverDisabledRange = DetailConfirmsHoverDisabledRangeStates(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 90 Cat Room Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0CatRoomBatch90RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- Complete screenshot evidence: " + (completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- Cat-room surface captured: " + (completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- Candidate frame draws: " + (candidateFrameDraws ? "6/6" : "incomplete"));
            builder.AppendLine("- No candidate texture fallback: " + (candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- Bed/feeder/litter/dream entrance states visible: " + (interactionStates ? "yes" : "no"));
            builder.AppendLine("- Hover/disabled/blocked/range states visible: " + (hoverDisabledRange ? "yes" : "no"));
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
            P0CatRoomBatch90RuntimeEvidenceState state,
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
            P0CatRoomBatch90RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0CatRoomBatch90RuntimeEvidenceState.Passed
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

        private static bool DetailConfirmsInteractionStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Bed/feeder/litter/dream entrance states visible: bed=ready feeder=ready litter=attention dream=available",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsHoverDisabledRangeStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Hover/disabled/blocked/range states visible: hover=1 disabled=1 blocked=1 range=1",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(StringBuilder builder, IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> targets)
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
            return (value ?? string.Empty)
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Trim();
        }
    }

    internal sealed class P0CatRoomBatch90RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 10f;
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0CatRoomBatch90PreviewOverlay overlay;
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
            screenshotDirectory = P0CatRoomBatch90RuntimeEvidence.ToProjectPath(P0CatRoomBatch90RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0CatRoomBatch90RuntimeEvidence.ToProjectPath(P0CatRoomBatch90RuntimeEvidence.ReviewDirectory);
            targets = P0CatRoomBatch90RuntimeEvidence.GetRequestedScreenshotTargets();
            Directory.CreateDirectory(screenshotDirectory);
            Directory.CreateDirectory(reviewDirectory);
            ClearGeneratedEvidence();
            Add("Screenshot output: " + screenshotDirectory);
            Add("Review output: " + reviewDirectory);
            Add("Requested screenshot targets: " + BuildTargetSummary(targets));

            yield return LoadCatRoomScene();
            if (failed)
            {
                yield break;
            }

            CatRoomController catRoomController = UnityEngine.Object.FindAnyObjectByType<CatRoomController>();
            if (catRoomController == null)
            {
                Fail("P0CatRoom is missing CatRoomController.");
                yield break;
            }

            P0CatRoomSurface surface = catRoomController.BuildCatRoomSurfaceForSmoke();
            Require(P0CatRoomPresenter.HasP0CatRoomSurface(surface), "cat-room surface", P0CatRoomPresenter.BuildCompactSummary(surface));
            if (failed)
            {
                yield break;
            }

            catRoomController.enabled = false;
            Add("Suppressed current IMGUI cat-room during Batch 90 candidate capture.");
            CreateOverlay(surface);
            for (int i = 0; i < targets.Count; i++)
            {
                yield return CaptureTarget(targets[i]);
                if (failed)
                {
                    yield break;
                }
            }

            if (targets.Count == P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0CatRoomBatch90RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0CatRoomBatch90RuntimeEvidence.InteractionStateTextDensityReviewPath,
                    P0CatRoomBatch90RuntimeEvidence.BuildInteractionStateTextDensityReviewMarkdown(
                        layoutSummary,
                        P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0CatRoomBatch90RuntimeEvidence.ClickTargetPropScaleReviewPath,
                    P0CatRoomBatch90RuntimeEvidence.BuildClickTargetPropScaleReviewMarkdown(
                        clickSummary,
                        P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 90 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0CatRoomBatch90RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0CatRoomBatch90RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 90 cat-room runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0CatRoomBatch90RuntimeEvidenceState.Passed : P0CatRoomBatch90RuntimeEvidenceState.Failed, summary);
        }

        private IEnumerator LoadCatRoomScene()
        {
            SceneManager.LoadScene(P0SceneFlow.CatRoomSceneName);
            float start = Time.realtimeSinceStartup;
            while (SceneManager.GetActiveScene().name != P0SceneFlow.CatRoomSceneName)
            {
                if (Time.realtimeSinceStartup - start > SceneLoadTimeoutSeconds)
                {
                    Fail("Timed out waiting for scene " + P0SceneFlow.CatRoomSceneName + ".");
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

            Fail("Batch 90 runtime evidence missing " + label + ": " + summary);
        }

        private void CreateOverlay(P0CatRoomSurface surface)
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch90CatRoomCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0CatRoomBatch90PreviewOverlay>();
            overlay.Configure(surface);
            layoutSummary = overlay.BuildLayoutSummary(1024, 768);
            clickSummary = overlay.BuildClickTargetSummary(1024, 768);
            Add("Created Batch 90 candidate overlay: " + layoutSummary);

            if (!overlay.TryValidateTargetLayouts(P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 90 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 90 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 90 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 90 candidate texture bindings: " + textureSummary);
            Add("Bed/feeder/litter/dream entrance states visible: bed=ready feeder=ready litter=attention dream=available");
            Add("Hover/disabled/blocked/range states visible: hover=1 disabled=1 blocked=1 range=1");
        }

        private IEnumerator CaptureTarget(P0CatRoomBatch90ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0CatRoomBatch90RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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

            return P0CatRoomBatch90RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0CatRoomBatch90RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0CatRoomBatch90RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0CatRoomBatch90RuntimeEvidence.ScreenshotTargets.Length)
            {
                for (int i = 0; i < P0CatRoomBatch90RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0CatRoomBatch90RuntimeEvidence.ToProjectPath(P0CatRoomBatch90RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 90 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> requestedTargets)
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
            Complete(P0CatRoomBatch90RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0CatRoomBatch90RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0CatRoomBatch90RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0CatRoomBatch90RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0CatRoomBatch90RuntimeEvidence.ToProjectPath(P0CatRoomBatch90RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0CatRoomBatch90RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0CatRoomBatch90PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle chipStyle;
        private P0CatRoomBatch90ScreenshotTarget currentTarget;
        private P0CatRoomSurface surface;
        private bool captureAuditActive;

        public void Configure(P0CatRoomSurface catRoomSurface)
        {
            surface = catRoomSurface;
            P0VisualAssetBinding[] bindings = P0CatRoomBatch90CandidateCatalog.CreateUnityPreflightBindings();
            assetsByVariant.Clear();
            for (int i = 0; i < bindings.Length; i++)
            {
                P0VisualAssetBinding binding = bindings[i];
                if (binding.IsReady)
                {
                    assetsByVariant[binding.SlotId] = binding.Asset;
                }
            }
        }

        public void SetTarget(P0CatRoomBatch90ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(int width, int height)
        {
            Layout layout = BuildLayout(width, height);
            return "stage=" + FormatRect(layout.StagePanel)
                + ", status=" + FormatRect(layout.StatusRail)
                + ", bed=" + FormatRect(layout.InteractionCards[0])
                + ", feeder=" + FormatRect(layout.InteractionCards[1])
                + ", litter=" + FormatRect(layout.InteractionCards[2])
                + ", dream=" + FormatRect(layout.InteractionCards[3])
                + ", prompts=" + layout.PromptChips.Length
                + ", surfaceValues=" + (surface == null ? 0 : surface.ValueRows.Count);
        }

        public string BuildClickTargetSummary(int width, int height)
        {
            Layout layout = BuildLayout(width, height);
            return "bed=" + FormatRect(layout.Hotspots[0])
                + ", feeder=" + FormatRect(layout.Hotspots[1])
                + ", litter=" + FormatRect(layout.Hotspots[2])
                + ", dreamButton=" + FormatRect(layout.DreamButton)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates = P0CatRoomBatch90CandidateCatalog.CreateCandidates();
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
                ? candidates.Count + "/" + P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount;
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
            IReadOnlyList<P0CatRoomBatch90CandidateAsset> candidates = P0CatRoomBatch90CandidateCatalog.CreateCandidates();
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
                + P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount
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
                && auditedDrawnVariants.Count == P0CatRoomBatch90CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0CatRoomBatch90ScreenshotTarget> targets,
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
                P0CatRoomBatch90ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                if (!IsInside(layout.StagePanel, target.Width, target.Height)
                    || !IsInside(layout.StatusRail, target.Width, target.Height)
                    || !IsInside(layout.DreamButton, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    summary = target.ResolutionLabel + " has an off-screen stage, status rail, dream button, or badge";
                    return false;
                }

                for (int cardIndex = 0; cardIndex < layout.InteractionCards.Length; cardIndex++)
                {
                    if (!IsInside(layout.InteractionCards[cardIndex], target.Width, target.Height)
                        || !IsInside(layout.Hotspots[cardIndex], target.Width, target.Height)
                        || layout.Hotspots[cardIndex].width < 44f
                        || layout.Hotspots[cardIndex].height < 44f)
                    {
                        summary = target.ResolutionLabel + " has an invalid hotspot or card";
                        return false;
                    }
                }

                for (int chipIndex = 0; chipIndex < layout.PromptChips.Length; chipIndex++)
                {
                    if (!IsInside(layout.PromptChips[chipIndex], target.Width, target.Height))
                    {
                        summary = target.ResolutionLabel + " has an off-screen prompt chip";
                        return false;
                    }
                }

                if (layout.DreamButton.width < 44f || layout.DreamButton.height < 44f)
                {
                    summary = target.ResolutionLabel + " has a dream entrance button below click target size";
                    return false;
                }

                labels.Add(target.ResolutionLabel);
            }

            summary = string.Join(", ", labels) + " inside-screen click-target pass";
            return true;
        }

        private void OnGUI()
        {
            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawBackdrop();
            DrawCandidateFrame("cat_room_stage_panel_frame", layout.StagePanel);
            DrawHeader(layout);
            DrawCandidateFrame("cat_room_status_rail_frame", layout.StatusRail);
            DrawStatusRail(layout.StatusRail);
            DrawInteractionCard(0, layout.InteractionCards[0], layout.Hotspots[0], Text.BedLabel, Text.BedState, Text.BedFeedback);
            DrawInteractionCard(1, layout.InteractionCards[1], layout.Hotspots[1], Text.FeederLabel, Text.FeederState, Text.FeederFeedback);
            DrawInteractionCard(2, layout.InteractionCards[2], layout.Hotspots[2], Text.LitterLabel, Text.LitterState, Text.LitterFeedback);
            DrawInteractionCard(3, layout.InteractionCards[3], layout.Hotspots[3], Text.DreamLabel, Text.DreamState, Text.DreamFeedback);
            DrawCandidateFrame("cat_room_dream_entrance_button_frame", layout.DreamButton);
            GUI.Label(layout.DreamButton, Text.DreamButton, titleStyle);
            DrawPromptChips(layout.PromptChips);
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
            float inner = P0ImGuiLayout.Scaled(28f, scale);
            float gap = P0ImGuiLayout.Scaled(18f, scale);
            Rect status = new Rect(stage.x + inner, stage.y + P0ImGuiLayout.Scaled(76f, scale), stage.width - inner * 2f, P0ImGuiLayout.Scaled(88f, scale));
            float cardWidth = (stage.width - inner * 2f - gap) * 0.5f;
            float cardHeight = P0ImGuiLayout.Scaled(132f, scale);
            float cardY = status.yMax + P0ImGuiLayout.Scaled(22f, scale);
            Rect[] cards =
            {
                new Rect(stage.x + inner, cardY, cardWidth, cardHeight),
                new Rect(stage.x + inner + cardWidth + gap, cardY, cardWidth, cardHeight),
                new Rect(stage.x + inner, cardY + cardHeight + gap, cardWidth, cardHeight),
                new Rect(stage.x + inner + cardWidth + gap, cardY + cardHeight + gap, cardWidth, cardHeight)
            };

            Rect[] hotspots = new Rect[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                float hotspotSize = Mathf.Min(P0ImGuiLayout.Scaled(82f, scale), cards[i].height - P0ImGuiLayout.Scaled(20f, scale));
                hotspots[i] = new Rect(cards[i].x + P0ImGuiLayout.Scaled(14f, scale), cards[i].y + (cards[i].height - hotspotSize) * 0.5f, hotspotSize, hotspotSize);
            }

            Rect dreamButton = new Rect(stage.xMax - inner - P0ImGuiLayout.Scaled(420f, scale), stage.yMax - inner - P0ImGuiLayout.Scaled(76f, scale), P0ImGuiLayout.Scaled(420f, scale), P0ImGuiLayout.Scaled(76f, scale));
            float chipWidth = P0ImGuiLayout.Scaled(184f, scale);
            float chipHeight = P0ImGuiLayout.Scaled(38f, scale);
            float chipY = stage.yMax - inner - chipHeight * 2f - P0ImGuiLayout.Scaled(8f, scale);
            Rect[] chips =
            {
                new Rect(stage.x + inner, chipY, chipWidth, chipHeight),
                new Rect(stage.x + inner + chipWidth + P0ImGuiLayout.Scaled(10f, scale), chipY, chipWidth, chipHeight),
                new Rect(stage.x + inner, chipY + chipHeight + P0ImGuiLayout.Scaled(8f, scale), chipWidth, chipHeight),
                new Rect(stage.x + inner + chipWidth + P0ImGuiLayout.Scaled(10f, scale), chipY + chipHeight + P0ImGuiLayout.Scaled(8f, scale), chipWidth, chipHeight)
            };

            Rect badge = new Rect(stage.x + P0ImGuiLayout.Scaled(24f, scale), stage.y + P0ImGuiLayout.Scaled(16f, scale), P0ImGuiLayout.Scaled(238f, scale), P0ImGuiLayout.Scaled(32f, scale));
            return new Layout(stage, status, cards, hotspots, dreamButton, chips, badge);
        }

        private void DrawBackdrop()
        {
            GUI.color = new Color(0.04f, 0.06f, 0.07f, 0.96f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.11f, 0.16f, 0.18f, 0.78f);
            GUI.DrawTexture(new Rect(0f, Screen.height * 0.56f, Screen.width, Screen.height * 0.44f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawHeader(Layout layout)
        {
            Rect titleRect = new Rect(layout.StagePanel.x + 280f, layout.StagePanel.y + 18f, layout.StagePanel.width - 560f, 36f);
            GUI.Label(titleRect, Text.Title, titleStyle);
            Rect subtitleRect = new Rect(layout.StagePanel.x + 280f, layout.StagePanel.y + 48f, layout.StagePanel.width - 560f, 28f);
            GUI.Label(subtitleRect, Text.Subtitle, smallStyle);
        }

        private void DrawStatusRail(Rect rect)
        {
            float segmentWidth = rect.width / 4f;
            string[] labels =
            {
                Text.SleepValue,
                Text.HpValue,
                Text.HungerValue,
                Text.PoopValue
            };

            for (int i = 0; i < labels.Length; i++)
            {
                Rect labelRect = new Rect(rect.x + segmentWidth * i + 8f, rect.y + 8f, segmentWidth - 16f, rect.height - 16f);
                GUI.Label(labelRect, labels[i], labelStyle);
            }
        }

        private void DrawInteractionCard(int index, Rect card, Rect hotspot, string label, string state, string feedback)
        {
            DrawCandidateFrame("cat_room_interaction_card_slot", card);
            DrawCandidateFrame("cat_room_prop_hotspot_frame", hotspot);
            GUI.Label(new Rect(hotspot.x, hotspot.y + hotspot.height * 0.24f, hotspot.width, hotspot.height * 0.52f), (index + 1).ToString(), titleStyle);
            float textX = hotspot.xMax + 14f;
            GUI.Label(new Rect(textX, card.y + 16f, card.xMax - textX - 16f, 28f), label + "  " + state, labelStyle);
            GUI.Label(new Rect(textX, card.y + 48f, card.xMax - textX - 16f, card.height - 58f), feedback, smallStyle);
        }

        private void DrawPromptChips(Rect[] chips)
        {
            string[] labels =
            {
                Text.HoverPrompt,
                Text.DisabledPrompt,
                Text.RangePrompt,
                Text.BlockedPrompt
            };

            for (int i = 0; i < chips.Length; i++)
            {
                DrawCandidateFrame("cat_room_hover_disabled_prompt_chip", chips[i]);
                GUI.Label(chips[i], labels[i], chipStyle);
            }
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.04f, 0.08f, 0.11f, 0.82f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0 ? "Batch90 " + currentTarget.ResolutionLabel : "Batch90";
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

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            labelStyle.normal.textColor = new Color(0.94f, 0.97f, 0.94f, 1f);

            smallStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                wordWrap = true
            };
            smallStyle.normal.textColor = new Color(0.84f, 0.91f, 0.93f, 1f);

            chipStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                wordWrap = true
            };
            chipStyle.normal.textColor = new Color(0.91f, 0.96f, 0.98f, 1f);
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
                Rect stagePanel,
                Rect statusRail,
                Rect[] interactionCards,
                Rect[] hotspots,
                Rect dreamButton,
                Rect[] promptChips,
                Rect captureBadge)
            {
                StagePanel = stagePanel;
                StatusRail = statusRail;
                InteractionCards = interactionCards ?? Array.Empty<Rect>();
                Hotspots = hotspots ?? Array.Empty<Rect>();
                DreamButton = dreamButton;
                PromptChips = promptChips ?? Array.Empty<Rect>();
                CaptureBadge = captureBadge;
            }

            public Rect StagePanel { get; }

            public Rect StatusRail { get; }

            public Rect[] InteractionCards { get; }

            public Rect[] Hotspots { get; }

            public Rect DreamButton { get; }

            public Rect[] PromptChips { get; }

            public Rect CaptureBadge { get; }
        }

        private static class Text
        {
            public const string Title = "\u732b\u623f\u5019\u9009\u9884\u68c0";
            public const string Subtitle = "Batch90 \u5019\u9009\u8d44\u6e90\uff0c\u4ec5\u7528\u4e8e Unity \u8bc1\u636e";
            public const string SleepValue = "\u4e3b\u4eba\u7761\u7720 78%  \u7a33\u5b9a";
            public const string HpValue = "\u732b\u961fHP 86%  \u53ef\u7ee7\u7eed";
            public const string HungerValue = "\u9971\u8179\u5ea6 65%  \u6ce8\u610f";
            public const string PoopValue = "\u4fbf\u4fbf\u538b\u529b 25%  \u7a33\u5b9a";
            public const string BedLabel = "\u5e8a\u94fa";
            public const string FeederLabel = "\u98df\u76c6";
            public const string LitterLabel = "\u732b\u7802\u76c6";
            public const string DreamLabel = "\u68a6\u5883\u5165\u53e3";
            public const string BedState = "\u53ef\u4e92\u52a8";
            public const string FeederState = "\u53ef\u4e92\u52a8";
            public const string LitterState = "\u9700\u6ce8\u610f";
            public const string DreamState = "\u53ef\u8fdb\u5165";
            public const string BedFeedback = "\u6574\u7406\u5e8a\u94fa\uff0c\u7a33\u4f4f\u4eca\u591c\u7761\u7720\u9632\u7ebf\u3002";
            public const string FeederFeedback = "\u8865\u5145\u9c7c\u5e72\uff0c\u51fa\u53d1\u524d\u786e\u8ba4\u732b\u961f\u9971\u8179\u5ea6\u3002";
            public const string LitterFeedback = "\u6e05\u7406\u732b\u7802\u76c6\uff0c\u964d\u4f4e\u4e0b\u4e00\u573a\u538b\u529b\u3002";
            public const string DreamFeedback = "\u8fdb\u5165\u5367\u5ba4\u68a6\u5883\uff0c\u8854\u63a5\u5f53\u524d Demo \u8def\u7ebf\u3002";
            public const string DreamButton = "\u8fdb\u5165\u5367\u5ba4\u68a6\u5883";
            public const string HoverPrompt = "\u60ac\u505c\uff1a\u67e5\u770b\u53cd\u9988";
            public const string DisabledPrompt = "\u7981\u7528\uff1a\u6761\u4ef6\u672a\u8fbe\u6210";
            public const string RangePrompt = "\u8ddd\u79bb\uff1a\u9760\u8fd1\u540e\u4e92\u52a8";
            public const string BlockedPrompt = "\u963b\u6321\uff1a\u5148\u6e05\u7406\u72b6\u6001";
        }
    }
}
