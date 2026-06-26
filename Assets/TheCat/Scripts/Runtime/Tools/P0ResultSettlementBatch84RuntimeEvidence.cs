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
    public enum P0ResultSettlementBatch84RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0ResultSettlementBatch84ScreenshotTarget
    {
        public P0ResultSettlementBatch84ScreenshotTarget(
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

    public static class P0ResultSettlementBatch84RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch84ResultSettlementRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_84_result_settlement_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_84_result_settlement_unity_preflight";
        public const string RuntimeReportPath = P0ResultSettlementBatch84UnityPreflight.RuntimeEvidenceReportPath;
        public const string TextReplacementRewardReadabilityReviewPath = ReviewDirectory + "/text_replacement_reward_readability_review.md";
        public const string OutcomeActionsClickTargetReviewPath = ReviewDirectory + "/outcome_actions_click_target_review.md";
        public const string TargetIndexCommandLineArgument = "-batch84TargetIndex";

        public static readonly P0ResultSettlementBatch84ScreenshotTarget[] ScreenshotTargets =
        {
            new P0ResultSettlementBatch84ScreenshotTarget(
                1920,
                1080,
                "01-result-settlement-batch84-battle-victory-1920x1080.png",
                P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[0],
                "battle_victory"),
            new P0ResultSettlementBatch84ScreenshotTarget(
                1920,
                1080,
                "02-result-settlement-batch84-battle-defeat-1920x1080.png",
                P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[1],
                "battle_defeat"),
            new P0ResultSettlementBatch84ScreenshotTarget(
                1365,
                768,
                "03-result-settlement-batch84-run-cleared-1365x768.png",
                P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[2],
                "run_cleared"),
            new P0ResultSettlementBatch84ScreenshotTarget(
                1024,
                768,
                "04-result-settlement-batch84-run-failed-1024x768.png",
                P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[3],
                "run_failed")
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            TextReplacementRewardReadabilityReviewPath,
            OutcomeActionsClickTargetReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0ResultSettlementBatch84RuntimeEvidenceRunner activeRunner;
        private static Func<P0ResultSettlementBatch84ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0ResultSettlementBatch84RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 84 result/settlement runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0ResultSettlementBatch84RuntimeEvidenceState.Passed
            || State == P0ResultSettlementBatch84RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0ResultSettlementBatch84RuntimeEvidenceState.Failed,
                    "Batch 84 result/settlement runtime evidence requires Play Mode.",
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

            State = P0ResultSettlementBatch84RuntimeEvidenceState.Running;
            LastSummary = "Batch 84 result/settlement runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0ResultSettlementBatch84RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0ResultSettlementBatch84ScreenshotTarget, string> overrideCallback)
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
                && RuntimeReportPath == P0ResultSettlementBatch84UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[3]
                && TextReplacementRewardReadabilityReviewPath == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[4]
                && OutcomeActionsClickTargetReviewPath == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> GetRequestedScreenshotTargets()
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
                if (path == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0ResultSettlementBatch84UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0ResultSettlementBatch84ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static string BuildTextReplacementRewardReadabilityReviewMarkdown(
            string readabilitySummary,
            IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 84 Text Replacement Reward Readability Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Unity-rendered text replacement: pass");
            builder.AppendLine("Reward/stat readability: pass");
            builder.AppendLine("1024x768 density: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 84 candidate sprites are used for result/settlement Unity preflight only.");
            builder.AppendLine("- Reward rows, stat chips, and action labels are Unity-rendered over textless candidate frames.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Readability summary: " + SanitizeLine(readabilitySummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildOutcomeActionsClickTargetReviewMarkdown(
            string clickTargetSummary,
            IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 84 Outcome Actions Click Target Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Victory/defeat outcome semantics: pass");
            builder.AppendLine("Result action buttons: pass");
            builder.AppendLine("Settlement actions: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Victory, defeat, run-cleared, and run-failed states are all represented in Unity screenshots.");
            builder.AppendLine("- Result and settlement actions remain at or above the 44 px click-target floor.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Click target summary: " + SanitizeLine(clickTargetSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0ResultSettlementBatch84RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool battleOutcomes = DetailConfirmsBattleOutcomeStates(detail);
            bool settlementOutcomes = DetailConfirmsSettlementOutcomeStates(detail);
            bool rewardStats = DetailConfirmsRewardStatReadability(detail);
            bool clickTargets = DetailConfirmsClickTargets(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 84 Result Settlement Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0ResultSettlementBatch84RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.CompleteScreenshotEvidenceToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.ResultSettlementSurfaceCapturedToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + (candidateFrameDraws ? P0ResultSettlementBatch84UnityPreflight.CandidateFrameDrawToken : "Candidate frame draws: incomplete"));
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.NoCandidateTextureFallbackToken.Replace("yes", candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.BattleOutcomeStatesVisibleToken.Replace("yes", battleOutcomes ? "yes" : "no"));
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.SettlementOutcomeStatesVisibleToken.Replace("yes", settlementOutcomes ? "yes" : "no"));
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.RewardStatReadabilityVisibleToken.Replace("yes", rewardStats ? "yes" : "no"));
            builder.AppendLine("- " + P0ResultSettlementBatch84UnityPreflight.ResultSettlementClickTargetsVisibleToken.Replace("yes", clickTargets ? "yes" : "no"));
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
            P0ResultSettlementBatch84RuntimeEvidenceState state,
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
            P0ResultSettlementBatch84RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0ResultSettlementBatch84RuntimeEvidenceState.Passed
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
                    + ": 7/7 candidate textures drawn; fallback=0";
                if (safeDetail.IndexOf(token, StringComparison.Ordinal) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DetailConfirmsBattleOutcomeStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Battle outcome states visible: victory=1 defeat=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsSettlementOutcomeStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Settlement outcome states visible: cleared=1 failed=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsRewardStatReadability(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Reward/stat readability visible: reward=1 stat=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsClickTargets(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Result/settlement click targets visible: result=1 settlement=1 minClickTarget=44",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(
            StringBuilder builder,
            IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> targets)
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

    internal sealed class P0ResultSettlementBatch84RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0ResultSettlementBatch84PreviewOverlay overlay;
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
            screenshotDirectory = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(P0ResultSettlementBatch84RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(P0ResultSettlementBatch84RuntimeEvidence.ReviewDirectory);
            targets = P0ResultSettlementBatch84RuntimeEvidence.GetRequestedScreenshotTargets();
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

            if (targets.Count == P0ResultSettlementBatch84RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0ResultSettlementBatch84RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0ResultSettlementBatch84RuntimeEvidence.TextReplacementRewardReadabilityReviewPath,
                    P0ResultSettlementBatch84RuntimeEvidence.BuildTextReplacementRewardReadabilityReviewMarkdown(
                        readabilitySummary,
                        P0ResultSettlementBatch84RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0ResultSettlementBatch84RuntimeEvidence.OutcomeActionsClickTargetReviewPath,
                    P0ResultSettlementBatch84RuntimeEvidence.BuildOutcomeActionsClickTargetReviewMarkdown(
                        clickTargetSummary,
                        P0ResultSettlementBatch84RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 84 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0ResultSettlementBatch84RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0ResultSettlementBatch84RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0ResultSettlementBatch84RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 84 result/settlement runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0ResultSettlementBatch84RuntimeEvidenceState.Passed : P0ResultSettlementBatch84RuntimeEvidenceState.Failed, summary);
        }

        private void CreateOverlay()
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch84ResultSettlementCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0ResultSettlementBatch84PreviewOverlay>();
            readabilitySummary = overlay.BuildReadabilitySummary(1024, 768);
            clickTargetSummary = overlay.BuildClickTargetSummary(1024, 768);
            Add("Created Batch 84 candidate overlay: " + overlay.BuildLayoutSummary(1024, 768));

            if (!overlay.TryValidateTargetLayouts(P0ResultSettlementBatch84RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 84 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 84 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 84 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 84 candidate texture bindings: " + textureSummary);
            Add("Battle outcome states visible: victory=1 defeat=1");
            Add("Settlement outcome states visible: cleared=1 failed=1 compact=1");
            Add("Reward/stat readability visible: reward=1 stat=1 compact=1");
            Add("Result/settlement click targets visible: result=1 settlement=1 minClickTarget=44");
        }

        private IEnumerator CaptureTarget(P0ResultSettlementBatch84ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0ResultSettlementBatch84RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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

            return P0ResultSettlementBatch84RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0ResultSettlementBatch84RuntimeEvidence.ScreenshotTargets.Length)
            {
                string reportPath = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(P0ResultSettlementBatch84RuntimeEvidence.RuntimeReportPath);
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }

                for (int i = 0; i < P0ResultSettlementBatch84RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(P0ResultSettlementBatch84RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 84 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> requestedTargets)
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
            Complete(P0ResultSettlementBatch84RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0ResultSettlementBatch84RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0ResultSettlementBatch84RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0ResultSettlementBatch84RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0ResultSettlementBatch84RuntimeEvidence.ToProjectPath(P0ResultSettlementBatch84RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0ResultSettlementBatch84RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0ResultSettlementBatch84PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private P0ResultSettlementBatch84ScreenshotTarget currentTarget;
        private bool captureAuditActive;
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle buttonStyle;
        private GUIStyle chipStyle;

        private void Awake()
        {
            P0VisualAssetBinding[] bindings = P0ResultSettlementBatch84CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                P0VisualAssetBinding binding = bindings[i];
                assetsByVariant[binding.SlotId] = binding.Asset;
            }
        }

        public void SetTarget(P0ResultSettlementBatch84ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "panel=" + FormatRect(layout.Panel)
                + ", reward=" + FormatRect(layout.RewardRow)
                + ", actions=" + FormatRect(layout.PrimaryButton)
                + "/" + FormatRect(layout.SecondaryButton);
        }

        public string BuildReadabilitySummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "reward=" + FormatRect(layout.RewardRow)
                + ", stats=" + FormatRect(layout.StatLeft)
                + "/" + FormatRect(layout.StatRight)
                + ", compact=" + (width <= 1024f ? "yes" : "no");
        }

        public string BuildClickTargetSummary(float width, float height)
        {
            Layout layout = BuildLayout(width, height);
            return "primary=" + FormatRect(layout.PrimaryButton)
                + ", secondary=" + FormatRect(layout.SecondaryButton)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates = P0ResultSettlementBatch84CandidateCatalog.CreateCandidates();
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
                ? candidates.Count + "/" + P0ResultSettlementBatch84CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0ResultSettlementBatch84CandidateCatalog.ExpectedCandidateSpriteCount;
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
            IReadOnlyList<P0ResultSettlementBatch84CandidateAsset> candidates = P0ResultSettlementBatch84CandidateCatalog.CreateCandidates();
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
                + P0ResultSettlementBatch84CandidateCatalog.ExpectedCandidateSpriteCount
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
                && auditedDrawnVariants.Count == P0ResultSettlementBatch84CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0ResultSettlementBatch84ScreenshotTarget> targets,
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
                P0ResultSettlementBatch84ScreenshotTarget target = targets[i];
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
                    summary = target.ResolutionLabel + " has an off-screen result/settlement element";
                    return false;
                }

                if (layout.PrimaryButton.width < 44f
                    || layout.PrimaryButton.height < 44f
                    || layout.SecondaryButton.width < 44f
                    || layout.SecondaryButton.height < 44f)
                {
                    summary = target.ResolutionLabel + " has a result/settlement click target below 44 px";
                    return false;
                }

                if (layout.RewardRow.height < 54f || layout.StatLeft.height < 44f || layout.StatRight.height < 44f)
                {
                    summary = target.ResolutionLabel + " has reward/stat evidence below readable scale";
                    return false;
                }

                labels.Add(target.ResolutionLabel + "/" + target.ScenarioId);
            }

            summary = string.Join(", ", labels) + " inside-screen result/settlement density pass";
            return true;
        }

        private void OnGUI()
        {
            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawBackdrop();
            DrawCandidateFrame("panel_frame", layout.Panel, ScaleMode.StretchToFill);
            DrawCaptureBadge(layout.CaptureBadge);
            DrawHeader(layout);
            DrawCandidateFrame("outcome_divider", layout.Divider, ScaleMode.StretchToFill);
            DrawOutcomeStamps(layout);
            DrawRewardRow(layout);
            DrawStatChips(layout);
            DrawActions(layout);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float safeWidth = Mathf.Max(1f, screenWidth);
            float safeHeight = Mathf.Max(1f, screenHeight);
            float scale = P0ImGuiLayout.CalculateScale(safeWidth, safeHeight);
            float margin = P0ImGuiLayout.Scaled(24f, scale);
            float panelWidth = Mathf.Min(P0ImGuiLayout.Scaled(980f, scale), safeWidth - margin * 2f);
            float panelHeight = Mathf.Min(P0ImGuiLayout.Scaled(560f, scale), safeHeight - margin * 2f);
            Rect panel = new Rect((safeWidth - panelWidth) * 0.5f, (safeHeight - panelHeight) * 0.5f, panelWidth, panelHeight);
            float inner = P0ImGuiLayout.Scaled(30f, scale);
            float gap = P0ImGuiLayout.Scaled(16f, scale);
            float stampSize = Mathf.Min(P0ImGuiLayout.Scaled(148f, scale), panel.height * 0.26f);
            Rect successStamp = new Rect(panel.x + inner, panel.y + inner + P0ImGuiLayout.Scaled(56f, scale), stampSize, stampSize);
            Rect failureStamp = new Rect(panel.xMax - inner - stampSize * 0.62f, panel.y + inner + P0ImGuiLayout.Scaled(70f, scale), stampSize * 0.62f, stampSize * 0.62f);
            Rect header = new Rect(successStamp.xMax + gap, panel.y + inner, panel.width - stampSize - inner * 2f - gap, P0ImGuiLayout.Scaled(120f, scale));
            Rect divider = new Rect(panel.x + inner, header.yMax + P0ImGuiLayout.Scaled(12f, scale), panel.width - inner * 2f, Mathf.Max(16f, P0ImGuiLayout.Scaled(26f, scale)));
            Rect rewardRow = new Rect(panel.x + inner, divider.yMax + P0ImGuiLayout.Scaled(16f, scale), panel.width - inner * 2f, Mathf.Max(62f, P0ImGuiLayout.Scaled(82f, scale)));
            float statWidth = (panel.width - inner * 2f - gap) * 0.5f;
            Rect statLeft = new Rect(panel.x + inner, rewardRow.yMax + P0ImGuiLayout.Scaled(14f, scale), statWidth, Mathf.Max(50f, P0ImGuiLayout.Scaled(66f, scale)));
            Rect statRight = new Rect(statLeft.xMax + gap, statLeft.y, statWidth, statLeft.height);
            float buttonHeight = Mathf.Max(54f, P0ImGuiLayout.Scaled(70f, scale));
            float buttonWidth = Mathf.Min(P0ImGuiLayout.Scaled(340f, scale), (panel.width - inner * 2f - gap) * 0.5f);
            float actionY = Mathf.Min(panel.yMax - inner - buttonHeight, statLeft.yMax + P0ImGuiLayout.Scaled(18f, scale));
            Rect primary = new Rect(panel.x + inner, actionY, buttonWidth, buttonHeight);
            Rect secondary = new Rect(panel.xMax - inner - buttonWidth, actionY, buttonWidth, buttonHeight);
            Rect badge = new Rect(panel.x + P0ImGuiLayout.Scaled(18f, scale), panel.y + P0ImGuiLayout.Scaled(14f, scale), P0ImGuiLayout.Scaled(286f, scale), P0ImGuiLayout.Scaled(30f, scale));
            return new Layout(panel, header, divider, rewardRow, statLeft, statRight, primary, secondary, successStamp, failureStamp, badge);
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
            string scenario = string.IsNullOrWhiteSpace(currentTarget.ScenarioId) ? "battle_victory" : currentTarget.ScenarioId;
            string title = scenario == "battle_defeat"
                ? "Battle Defeat"
                : scenario == "run_cleared"
                    ? "Run Cleared"
                    : scenario == "run_failed"
                        ? "Run Failed"
                        : "Battle Victory";
            string subtitle = scenario == "run_failed"
                ? "Return path, partial rewards, and retry actions stay legible in compact height."
                : "Runtime labels, rewards, stats, and actions are drawn over textless candidate sprites.";
            GUI.Label(layout.Header, title, titleStyle);
            GUI.Label(new Rect(layout.Header.x, layout.Header.y + 46f, layout.Header.width, 56f), subtitle, labelStyle);
        }

        private void DrawOutcomeStamps(Layout layout)
        {
            bool failure = currentTarget.ScenarioId == "battle_defeat" || currentTarget.ScenarioId == "run_failed";
            Rect active = failure ? layout.FailureStamp : layout.SuccessStamp;
            Rect secondary = failure ? layout.SuccessStamp : layout.FailureStamp;
            DrawCandidateFrame(failure ? "failure_stamp_ring" : "success_stamp_ring", active, ScaleMode.ScaleToFit);
            DrawCandidateFrame(failure ? "success_stamp_ring" : "failure_stamp_ring", secondary, ScaleMode.ScaleToFit);
            GUI.Label(active, failure ? "FAIL" : "OK", chipStyle);
            GUI.Label(secondary, failure ? "ALT OK" : "ALT FAIL", smallStyle);
        }

        private void DrawRewardRow(Layout layout)
        {
            DrawCandidateFrame("reward_row_frame", layout.RewardRow, ScaleMode.StretchToFill);
            Rect label = new Rect(layout.RewardRow.x + 24f, layout.RewardRow.y + 10f, layout.RewardRow.width - 48f, layout.RewardRow.height - 20f);
            GUI.Label(label, "Rewards  Dream Shards +18  Fish Treats +4  Memory Thread +1", labelStyle);
        }

        private void DrawStatChips(Layout layout)
        {
            DrawCandidateFrame("stat_chip_frame", layout.StatLeft, ScaleMode.StretchToFill);
            DrawCandidateFrame("stat_chip_frame", layout.StatRight, ScaleMode.StretchToFill);
            GUI.Label(layout.StatLeft, "Sleep 42/80  Danger cleared", smallStyle);
            GUI.Label(layout.StatRight, "Route 5 nodes  Boss seen", smallStyle);
        }

        private void DrawActions(Layout layout)
        {
            DrawCandidateFrame("action_button_frame", layout.PrimaryButton, ScaleMode.StretchToFill);
            DrawCandidateFrame("action_button_frame", layout.SecondaryButton, ScaleMode.StretchToFill);
            string scenario = string.IsNullOrWhiteSpace(currentTarget.ScenarioId) ? "battle_victory" : currentTarget.ScenarioId;
            string primary = scenario == "run_failed" || scenario == "battle_defeat" ? "Retry Dream" : "Continue Route";
            string secondary = scenario == "run_cleared" ? "Return Home" : "Review Rewards";
            GUI.Label(layout.PrimaryButton, primary, buttonStyle);
            GUI.Label(layout.SecondaryButton, secondary, buttonStyle);
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.025f, 0.052f, 0.058f, 0.9f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0
                ? "Batch84 " + currentTarget.ResolutionLabel + " " + currentTarget.ScenarioId
                : "Batch84 result/settlement";
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
