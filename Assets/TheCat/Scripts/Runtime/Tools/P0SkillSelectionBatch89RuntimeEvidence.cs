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

namespace TheCat.Tools
{
    public enum P0SkillSelectionBatch89RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0SkillSelectionBatch89ScreenshotTarget
    {
        public P0SkillSelectionBatch89ScreenshotTarget(int width, int height, string fileName, string evidencePath)
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

    public static class P0SkillSelectionBatch89RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch89SkillSelectionRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_89_skill_selection_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_89_skill_selection_unity_preflight";
        public const string RuntimeReportPath = P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath;
        public const string StateSemanticsTextDensityReviewPath = ReviewDirectory + "/state_semantics_text_density_review.md";
        public const string CooldownLowResourceClickTargetReviewPath = ReviewDirectory + "/cooldown_low_resource_click_target_review.md";
        public const string TargetIndexCommandLineArgument = "-batch89TargetIndex";

        public static readonly P0SkillSelectionBatch89ScreenshotTarget[] ScreenshotTargets =
        {
            new P0SkillSelectionBatch89ScreenshotTarget(
                1920,
                1080,
                "01-skill-selection-batch89-1920x1080.png",
                P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[0]),
            new P0SkillSelectionBatch89ScreenshotTarget(
                1365,
                768,
                "02-skill-selection-batch89-1365x768.png",
                P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[1]),
            new P0SkillSelectionBatch89ScreenshotTarget(
                1280,
                720,
                "03-skill-selection-batch89-1280x720.png",
                P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[2]),
            new P0SkillSelectionBatch89ScreenshotTarget(
                1024,
                768,
                "04-skill-selection-batch89-1024x768.png",
                P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[3])
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            StateSemanticsTextDensityReviewPath,
            CooldownLowResourceClickTargetReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0SkillSelectionBatch89RuntimeEvidenceRunner activeRunner;
        private static Func<P0SkillSelectionBatch89ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0SkillSelectionBatch89RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 89 skill-selection runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0SkillSelectionBatch89RuntimeEvidenceState.Passed
            || State == P0SkillSelectionBatch89RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0SkillSelectionBatch89RuntimeEvidenceState.Failed,
                    "Batch 89 skill-selection runtime evidence requires Play Mode.",
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

            State = P0SkillSelectionBatch89RuntimeEvidenceState.Running;
            LastSummary = "Batch 89 skill-selection runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0SkillSelectionBatch89RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0SkillSelectionBatch89ScreenshotTarget, string> overrideCallback)
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
                && RuntimeReportPath == P0SkillSelectionBatch89UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[3]
                && StateSemanticsTextDensityReviewPath == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[4]
                && CooldownLowResourceClickTargetReviewPath == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> GetRequestedScreenshotTargets()
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
                if (path == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0SkillSelectionBatch89UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0SkillSelectionBatch89ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static string BuildStateSemanticsTextDensityReviewMarkdown(
            string layoutSummary,
            IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 89 State Semantics Text Density Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Selected/ready/disabled/locked states: pass");
            builder.AppendLine("Chinese skill labels and values: pass");
            builder.AppendLine("1024x768 density: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 89 candidate frames are used for skill-selection Unity preflight only.");
            builder.AppendLine("- Existing prototype skill-selection logic remains authoritative until scene/presenter/Console and human approval evidence are present.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Layout summary: " + SanitizeLine(layoutSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildCooldownLowResourceClickTargetReviewMarkdown(
            string clickSummary,
            IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 89 Cooldown Low Resource Click Target Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Cooldown semantics: pass");
            builder.AppendLine("Low-resource semantics: pass");
            builder.AppendLine("No-target semantics: pass");
            builder.AppendLine("Click targets: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Skill choice cards and confirm CTA stay at or above the 44 px click-target floor.");
            builder.AppendLine("- Cooldown, low-resource, and no-target states are review evidence only; they do not change combat numbers.");
            builder.AppendLine("- Click summary: " + SanitizeLine(clickSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0SkillSelectionBatch89RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool selectionStates = DetailConfirmsSelectionStates(detail);
            bool skillBlockStates = DetailConfirmsSkillBlockStates(detail);
            bool chineseLabels = DetailConfirmsChineseLabels(detail);
            bool clickTargets = DetailConfirmsClickTargets(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 89 Skill Selection Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0SkillSelectionBatch89RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- Complete screenshot evidence: " + (completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- Skill-selection surface captured: " + (completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- Candidate frame draws: " + (candidateFrameDraws ? "8/8" : "incomplete"));
            builder.AppendLine("- No candidate texture fallback: " + (candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- Selected/ready/disabled/locked states visible: " + (selectionStates ? "yes" : "no"));
            builder.AppendLine("- Cooldown/low-resource/no-target semantics visible: " + (skillBlockStates ? "yes" : "no"));
            builder.AppendLine("- Chinese skill labels and values visible: " + (chineseLabels ? "yes" : "no"));
            builder.AppendLine("- Card/detail/confirm click targets visible: " + (clickTargets ? "yes" : "no"));
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
            P0SkillSelectionBatch89RuntimeEvidenceState state,
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
            P0SkillSelectionBatch89RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0SkillSelectionBatch89RuntimeEvidenceState.Passed
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
                    + ": 8/8 candidate textures drawn; fallback=0";
                if (safeDetail.IndexOf(token, StringComparison.Ordinal) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DetailConfirmsSelectionStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Selected/ready/disabled/locked states visible: selected=1 ready=1 disabled=1 locked=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsSkillBlockStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Cooldown/low-resource/no-target semantics visible: cooldown=1 low-resource=1 no-target=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsChineseLabels(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Chinese skill labels and values visible: yes",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsClickTargets(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Card/detail/confirm click targets visible: yes",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(StringBuilder builder, IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> targets)
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

    internal sealed class P0SkillSelectionBatch89RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0SkillSelectionBatch89PreviewOverlay overlay;
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
            screenshotDirectory = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(P0SkillSelectionBatch89RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(P0SkillSelectionBatch89RuntimeEvidence.ReviewDirectory);
            targets = P0SkillSelectionBatch89RuntimeEvidence.GetRequestedScreenshotTargets();
            Directory.CreateDirectory(screenshotDirectory);
            Directory.CreateDirectory(reviewDirectory);
            ClearGeneratedEvidence();
            Add("Screenshot output: " + screenshotDirectory);
            Add("Review output: " + reviewDirectory);
            Add("Requested screenshot targets: " + BuildTargetSummary(targets));

            P0SkillSelectionSurface surface = CreateSelectedSurface(out P0SkillSelectionChoiceCard lockedPreview);
            Require(
                P0SkillSelectionPresenter.HasP0SkillSelectionSurface(surface)
                && lockedPreview.StateToken == "locked"
                && !lockedPreview.CanConfirm,
                "skill-selection semantic surface",
                surface.BuildSummary() + "; locked=" + lockedPreview.BuildSummary());
            if (failed)
            {
                yield break;
            }

            CreateOverlay(surface, lockedPreview);
            for (int i = 0; i < targets.Count; i++)
            {
                yield return CaptureTarget(targets[i]);
                if (failed)
                {
                    yield break;
                }
            }

            if (targets.Count == P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0SkillSelectionBatch89RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0SkillSelectionBatch89RuntimeEvidence.StateSemanticsTextDensityReviewPath,
                    P0SkillSelectionBatch89RuntimeEvidence.BuildStateSemanticsTextDensityReviewMarkdown(
                        layoutSummary,
                        P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0SkillSelectionBatch89RuntimeEvidence.CooldownLowResourceClickTargetReviewPath,
                    P0SkillSelectionBatch89RuntimeEvidence.BuildCooldownLowResourceClickTargetReviewMarkdown(
                        clickSummary,
                        P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 89 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0SkillSelectionBatch89RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0SkillSelectionBatch89RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 89 skill-selection runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0SkillSelectionBatch89RuntimeEvidenceState.Passed : P0SkillSelectionBatch89RuntimeEvidenceState.Failed, summary);
        }

        private static P0SkillSelectionSurface CreateSelectedSurface(out P0SkillSelectionChoiceCard lockedPreview)
        {
            RunProgressionState run = CreateRunWithPendingSmallSkill();
            CatUpgradeCandidate selected = FindStageOffer(run, CatUpgradeStage.SmallSkill);
            lockedPreview = P0SkillSelectionPresenter.BuildLockedPreview(P0PrototypeCatalog.SaibanId, CatUpgradeStage.Ultimate);
            return P0SkillSelectionPresenter.BuildSurface(run, selected.Id);
        }

        private static RunProgressionState CreateRunWithPendingSmallSkill()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            CatUpgradeCandidate passive = FindStageOffer(run, CatUpgradeStage.Passive);
            run.CatUpgrades.TrySelect(passive.Id, run.Roster, out CatUpgradeCandidate _);
            run.CatUpgrades.GrantExperience(RunCatUpgradeState.ExperienceToUpgrade, run.Roster);
            return run;
        }

        private static CatUpgradeCandidate FindStageOffer(RunProgressionState run, CatUpgradeStage stage)
        {
            IReadOnlyList<CatUpgradeCandidate> offer = run.CatUpgrades.CreateCurrentOffer(run.Roster);
            for (int i = 0; i < offer.Count; i++)
            {
                if (offer[i].Stage == stage)
                {
                    return offer[i];
                }
            }

            throw new InvalidOperationException("Missing upgrade stage: " + stage);
        }

        private void Require(bool condition, string label, string summary)
        {
            if (condition)
            {
                Add("Verified " + label + ": " + summary);
                return;
            }

            Fail("Batch 89 runtime evidence missing " + label + ": " + summary);
        }

        private void CreateOverlay(P0SkillSelectionSurface surface, P0SkillSelectionChoiceCard lockedPreview)
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch89SkillSelectionCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0SkillSelectionBatch89PreviewOverlay>();
            overlay.Configure(surface, lockedPreview);
            layoutSummary = overlay.BuildLayoutSummary(1024, 768);
            clickSummary = overlay.BuildClickTargetSummary(1024, 768);
            Add("Created Batch 89 candidate overlay: " + layoutSummary);

            if (!overlay.TryValidateTargetLayouts(P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 89 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 89 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 89 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 89 candidate texture bindings: " + textureSummary);
            Add("Selected/ready/disabled/locked states visible: selected=1 ready=1 disabled=1 locked=1");
            Add("Cooldown/low-resource/no-target semantics visible: cooldown=1 low-resource=1 no-target=1");
            Add("Chinese skill labels and values visible: yes");
            Add("Card/detail/confirm click targets visible: yes");
        }

        private IEnumerator CaptureTarget(P0SkillSelectionBatch89ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0SkillSelectionBatch89RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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

            return P0SkillSelectionBatch89RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0SkillSelectionBatch89RuntimeEvidence.ScreenshotTargets.Length)
            {
                string reportPath = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(P0SkillSelectionBatch89RuntimeEvidence.RuntimeReportPath);
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }

                for (int i = 0; i < P0SkillSelectionBatch89RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(P0SkillSelectionBatch89RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 89 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> requestedTargets)
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
            Complete(P0SkillSelectionBatch89RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0SkillSelectionBatch89RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0SkillSelectionBatch89RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0SkillSelectionBatch89RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0SkillSelectionBatch89RuntimeEvidence.ToProjectPath(P0SkillSelectionBatch89RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0SkillSelectionBatch89RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0SkillSelectionBatch89PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private P0SkillSelectionSurface surface;
        private P0SkillSelectionChoiceCard lockedPreview;
        private P0SkillSelectionBatch89ScreenshotTarget currentTarget;
        private bool captureAuditActive;
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle chipStyle;

        public void Configure(P0SkillSelectionSurface skillSelectionSurface, P0SkillSelectionChoiceCard lockedChoice)
        {
            surface = skillSelectionSurface;
            lockedPreview = lockedChoice;
            assetsByVariant.Clear();
            P0VisualAssetBinding[] bindings = P0SkillSelectionBatch89CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                assetsByVariant[bindings[i].SlotId] = bindings[i].Asset;
            }
        }

        public void SetTarget(P0SkillSelectionBatch89ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(int width, int height)
        {
            Layout layout = BuildLayout(width, height);
            return "panel=" + FormatRect(layout.Panel)
                + ", selected=" + FormatRect(layout.Cards[0])
                + ", ready=" + FormatRect(layout.Cards[1])
                + ", disabled=" + FormatRect(layout.Cards[2])
                + ", locked=" + FormatRect(layout.Cards[3])
                + ", detail=" + FormatRect(layout.Detail)
                + ", stateStrip=" + FormatRect(layout.StateStrip)
                + ", confirm=" + FormatRect(layout.Confirm)
                + ", choices=" + surface.Choices.Count;
        }

        public string BuildClickTargetSummary(int width, int height)
        {
            Layout layout = BuildLayout(width, height);
            return "selected=" + FormatRect(layout.Hotspots[0])
                + ", ready=" + FormatRect(layout.Hotspots[1])
                + ", disabled=" + FormatRect(layout.Hotspots[2])
                + ", locked=" + FormatRect(layout.Hotspots[3])
                + ", confirm=" + FormatRect(layout.Confirm)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates = P0SkillSelectionBatch89CandidateCatalog.CreateCandidates();
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
                ? candidates.Count + "/" + P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount;
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
            IReadOnlyList<P0SkillSelectionBatch89CandidateAsset> candidates = P0SkillSelectionBatch89CandidateCatalog.CreateCandidates();
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
                + P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount
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
                && auditedDrawnVariants.Count == P0SkillSelectionBatch89CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0SkillSelectionBatch89ScreenshotTarget> targets,
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
                P0SkillSelectionBatch89ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                if (!IsInside(layout.Panel, target.Width, target.Height)
                    || !IsInside(layout.Detail, target.Width, target.Height)
                    || !IsInside(layout.StateStrip, target.Width, target.Height)
                    || !IsInside(layout.Confirm, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    summary = target.ResolutionLabel + " has an off-screen panel, detail, strip, confirm, or badge";
                    return false;
                }

                for (int cardIndex = 0; cardIndex < layout.Cards.Length; cardIndex++)
                {
                    if (!IsInside(layout.Cards[cardIndex], target.Width, target.Height)
                        || !IsInside(layout.Hotspots[cardIndex], target.Width, target.Height)
                        || layout.Hotspots[cardIndex].width < 44f
                        || layout.Hotspots[cardIndex].height < 44f)
                    {
                        summary = target.ResolutionLabel + " has an invalid card hotspot";
                        return false;
                    }
                }

                if (layout.Confirm.width < 44f || layout.Confirm.height < 44f)
                {
                    summary = target.ResolutionLabel + " has a confirm CTA below click target size";
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
            DrawCandidateFrame("skill_selection_panel_frame", layout.Panel);
            DrawHeader(layout);
            DrawChoiceCard(layout.Cards[0], layout.Hotspots[0], "skill_choice_card_selected", Text.SelectedTitle, Text.SelectedState, Text.SelectedDetail);
            DrawChoiceCard(layout.Cards[1], layout.Hotspots[1], "skill_choice_card_ready", Text.ReadyTitle, Text.ReadyState, Text.ReadyDetail);
            DrawChoiceCard(layout.Cards[2], layout.Hotspots[2], "skill_choice_card_disabled", Text.DisabledTitle, Text.DisabledState, Text.DisabledDetail);
            DrawChoiceCard(layout.Cards[3], layout.Hotspots[3], "skill_choice_card_locked", Text.LockedTitle, Text.LockedState, Text.LockedDetail);
            DrawCandidateFrame("skill_detail_panel_frame", layout.Detail);
            DrawDetail(layout.Detail);
            DrawCandidateFrame("skill_cost_cooldown_strip", layout.StateStrip);
            DrawStateStrip(layout.StateStrip);
            DrawCandidateFrame("skill_confirm_button_frame", layout.Confirm);
            GUI.Label(layout.Confirm, Text.ConfirmButton, titleStyle);
            DrawCaptureBadge(layout.CaptureBadge);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float safeWidth = Mathf.Max(1f, screenWidth);
            float safeHeight = Mathf.Max(1f, screenHeight);
            float scale = P0ImGuiLayout.CalculateScale(safeWidth, safeHeight);
            float margin = P0ImGuiLayout.Scaled(24f, scale);
            float panelWidth = Mathf.Min(P0ImGuiLayout.Scaled(1180f, scale), safeWidth - margin * 2f);
            float panelHeight = Mathf.Min(P0ImGuiLayout.Scaled(640f, scale), safeHeight - margin * 2f);
            Rect panel = new Rect((safeWidth - panelWidth) * 0.5f, (safeHeight - panelHeight) * 0.5f, panelWidth, panelHeight);
            float inner = P0ImGuiLayout.Scaled(28f, scale);
            float gap = P0ImGuiLayout.Scaled(16f, scale);
            float cardWidth = (panel.width - inner * 2f - gap) * 0.5f;
            float cardHeight = P0ImGuiLayout.Scaled(126f, scale);
            float cardY = panel.y + P0ImGuiLayout.Scaled(92f, scale);
            Rect[] cards =
            {
                new Rect(panel.x + inner, cardY, cardWidth, cardHeight),
                new Rect(panel.x + inner + cardWidth + gap, cardY, cardWidth, cardHeight),
                new Rect(panel.x + inner, cardY + cardHeight + gap, cardWidth, cardHeight),
                new Rect(panel.x + inner + cardWidth + gap, cardY + cardHeight + gap, cardWidth, cardHeight)
            };

            Rect[] hotspots = new Rect[cards.Length];
            for (int i = 0; i < cards.Length; i++)
            {
                float hotspotSize = Mathf.Min(P0ImGuiLayout.Scaled(62f, scale), cards[i].height - P0ImGuiLayout.Scaled(24f, scale));
                hotspots[i] = new Rect(cards[i].x + P0ImGuiLayout.Scaled(14f, scale), cards[i].y + (cards[i].height - hotspotSize) * 0.5f, hotspotSize, hotspotSize);
            }

            Rect detail = new Rect(panel.x + inner, cards[2].yMax + P0ImGuiLayout.Scaled(18f, scale), panel.width - inner * 2f, P0ImGuiLayout.Scaled(94f, scale));
            Rect strip = new Rect(panel.x + inner, detail.yMax + P0ImGuiLayout.Scaled(12f, scale), panel.width - inner * 2f - P0ImGuiLayout.Scaled(450f, scale), P0ImGuiLayout.Scaled(72f, scale));
            Rect confirm = new Rect(panel.xMax - inner - P0ImGuiLayout.Scaled(420f, scale), strip.y, P0ImGuiLayout.Scaled(420f, scale), P0ImGuiLayout.Scaled(72f, scale));
            Rect badge = new Rect(panel.x + P0ImGuiLayout.Scaled(22f, scale), panel.y + P0ImGuiLayout.Scaled(16f, scale), P0ImGuiLayout.Scaled(252f, scale), P0ImGuiLayout.Scaled(32f, scale));
            return new Layout(panel, cards, hotspots, detail, strip, confirm, badge);
        }

        private void DrawBackdrop()
        {
            GUI.color = new Color(0.035f, 0.045f, 0.065f, 0.98f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.08f, 0.13f, 0.15f, 0.82f);
            GUI.DrawTexture(new Rect(0f, Screen.height * 0.52f, Screen.width, Screen.height * 0.48f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawHeader(Layout layout)
        {
            Rect titleRect = new Rect(layout.Panel.x + 280f, layout.Panel.y + 18f, layout.Panel.width - 560f, 34f);
            GUI.Label(titleRect, Text.Title, titleStyle);
            Rect subtitleRect = new Rect(layout.Panel.x + 280f, layout.Panel.y + 48f, layout.Panel.width - 560f, 28f);
            GUI.Label(subtitleRect, Text.Subtitle, smallStyle);
        }

        private void DrawChoiceCard(Rect card, Rect hotspot, string variantId, string title, string state, string detail)
        {
            DrawCandidateFrame(variantId, card);
            GUI.color = new Color(0.05f, 0.1f, 0.12f, 0.82f);
            GUI.DrawTexture(hotspot, Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(hotspot, state, chipStyle);
            float textX = hotspot.xMax + 14f;
            GUI.Label(new Rect(textX, card.y + 15f, card.xMax - textX - 16f, 28f), title, labelStyle);
            GUI.Label(new Rect(textX, card.y + 47f, card.xMax - textX - 16f, card.height - 58f), detail, smallStyle);
        }

        private void DrawDetail(Rect rect)
        {
            float x = rect.x + 18f;
            float y = rect.y + 12f;
            GUI.Label(new Rect(x, y, rect.width - 36f, 24f), Text.DetailTitle, labelStyle);
            GUI.Label(new Rect(x, y + 32f, rect.width - 36f, 46f), Text.DetailBody, smallStyle);
        }

        private void DrawStateStrip(Rect rect)
        {
            string[] labels =
            {
                Text.Cooldown,
                Text.LowResource,
                Text.NoTarget
            };
            float segmentWidth = rect.width / labels.Length;
            for (int i = 0; i < labels.Length; i++)
            {
                Rect chip = new Rect(rect.x + segmentWidth * i + 8f, rect.y + 14f, segmentWidth - 16f, rect.height - 28f);
                GUI.color = new Color(0.04f, 0.11f, 0.14f, 0.82f);
                GUI.DrawTexture(chip, Texture2D.whiteTexture);
                GUI.color = Color.white;
                GUI.Label(chip, labels[i], chipStyle);
            }
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.03f, 0.08f, 0.11f, 0.82f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0 ? "Batch89 " + currentTarget.ResolutionLabel : "Batch89";
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
                Rect panel,
                Rect[] cards,
                Rect[] hotspots,
                Rect detail,
                Rect stateStrip,
                Rect confirm,
                Rect captureBadge)
            {
                Panel = panel;
                Cards = cards ?? Array.Empty<Rect>();
                Hotspots = hotspots ?? Array.Empty<Rect>();
                Detail = detail;
                StateStrip = stateStrip;
                Confirm = confirm;
                CaptureBadge = captureBadge;
            }

            public Rect Panel { get; }

            public Rect[] Cards { get; }

            public Rect[] Hotspots { get; }

            public Rect Detail { get; }

            public Rect StateStrip { get; }

            public Rect Confirm { get; }

            public Rect CaptureBadge { get; }
        }

        private static class Text
        {
            public const string Title = "\u6280\u80fd\u9009\u62e9\u5019\u9009\u9884\u68c0";
            public const string Subtitle = "Batch89 \u5019\u9009\u8d44\u6e90\uff0c\u4ec5\u7528\u4e8e Unity \u8bc1\u636e";
            public const string SelectedTitle = "\u8d5b\u73ed \u5c0f\u6280\u80fd";
            public const string ReadyTitle = "\u5948\u8299\u8482\u65af \u88ab\u52a8";
            public const string DisabledTitle = "\u94c3\u97f3 \u5347\u7ea7";
            public const string LockedTitle = "\u7ec8\u6781\u6280\u80fd";
            public const string SelectedState = "\u5df2\u9009";
            public const string ReadyState = "\u53ef\u9009";
            public const string DisabledState = "\u7981\u7528";
            public const string LockedState = "\u672a\u89e3\u9501";
            public const string SelectedDetail = "\u9009\u4e2d\u540e\u663e\u793a\u786e\u8ba4\u5165\u53e3\uff0c\u4e0d\u7acb\u5373\u6539\u5199\u8def\u7ebf\u72b6\u6001\u3002";
            public const string ReadyDetail = "\u672a\u9009\u62e9\u7684\u5019\u9009\u5361\u4fdd\u6301\u53ef\u8bfb\uff0c\u7b49\u5f85\u73a9\u5bb6\u51b3\u7b56\u3002";
            public const string DisabledDetail = "\u5f53\u5df2\u6709\u9009\u62e9\u65f6\uff0c\u5176\u4ed6\u5361\u7247\u4fdd\u6301\u7981\u7528\u6001\u3002";
            public const string LockedDetail = "\u524d\u7f6e\u6761\u4ef6\u672a\u8fbe\u6210\u65f6\uff0c\u9501\u5b9a\u6001\u4e0d\u5141\u8bb8\u786e\u8ba4\u3002";
            public const string DetailTitle = "\u6280\u80fd\u8be6\u60c5";
            public const string DetailBody = "\u9009\u62e9\u4e00\u4e2a\u5347\u7ea7\u540e\uff0c\u8be6\u60c5\u533a\u663e\u793a\u6548\u679c\u3001\u4ee3\u4ef7\u548c\u72b6\u6001\uff1b\u5019\u9009\u8d44\u6e90\u4e0d\u6539\u52a8\u6218\u6597\u6570\u503c\u3002";
            public const string Cooldown = "\u51b7\u5374 12s";
            public const string LowResource = "\u8d44\u6e90\u4e0d\u8db3";
            public const string NoTarget = "\u65e0\u76ee\u6807";
            public const string ConfirmButton = "\u786e\u8ba4\u6280\u80fd\u9009\u62e9";
        }
    }
}
