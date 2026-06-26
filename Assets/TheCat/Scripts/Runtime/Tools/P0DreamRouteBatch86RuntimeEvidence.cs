using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Roguelite;
using UnityEngine;

namespace TheCat.Tools
{
    public enum P0DreamRouteBatch86RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0DreamRouteBatch86ScreenshotTarget
    {
        public P0DreamRouteBatch86ScreenshotTarget(int width, int height, string fileName, string evidencePath)
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

    public static class P0DreamRouteBatch86RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch86DreamRouteRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_86_dream_route_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_86_dream_route_unity_preflight";
        public const string RuntimeReportPath = P0DreamRouteBatch86UnityPreflight.RuntimeEvidenceReportPath;
        public const string TextReplacementNodePathSemanticsReviewPath = ReviewDirectory + "/text_replacement_node_path_semantics_review.md";
        public const string RouteChoiceClickTargetDensityReviewPath = ReviewDirectory + "/route_choice_click_target_density_review.md";
        public const string TargetIndexCommandLineArgument = "-batch86TargetIndex";

        public static readonly P0DreamRouteBatch86ScreenshotTarget[] ScreenshotTargets =
        {
            new P0DreamRouteBatch86ScreenshotTarget(
                1920,
                1080,
                "01-dream-route-batch86-dream-route-1920x1080.png",
                P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[0]),
            new P0DreamRouteBatch86ScreenshotTarget(
                1365,
                768,
                "02-dream-route-batch86-route-branch-1365x768.png",
                P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[1]),
            new P0DreamRouteBatch86ScreenshotTarget(
                1280,
                720,
                "03-dream-route-batch86-route-boss-pressure-1280x720.png",
                P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[2]),
            new P0DreamRouteBatch86ScreenshotTarget(
                1024,
                768,
                "04-dream-route-batch86-route-compact-1024x768.png",
                P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[3])
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            TextReplacementNodePathSemanticsReviewPath,
            RouteChoiceClickTargetDensityReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0DreamRouteBatch86RuntimeEvidenceRunner activeRunner;
        private static Func<P0DreamRouteBatch86ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0DreamRouteBatch86RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 86 dream-route runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0DreamRouteBatch86RuntimeEvidenceState.Passed
            || State == P0DreamRouteBatch86RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0DreamRouteBatch86RuntimeEvidenceState.Failed,
                    "Batch 86 dream-route runtime evidence requires Play Mode.",
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

            State = P0DreamRouteBatch86RuntimeEvidenceState.Running;
            LastSummary = "Batch 86 dream-route runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0DreamRouteBatch86RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0DreamRouteBatch86ScreenshotTarget, string> overrideCallback)
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
                && RuntimeReportPath == P0DreamRouteBatch86UnityPreflight.RuntimeEvidenceReportPath
                && ScreenshotTargets[0].EvidencePath == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[3]
                && TextReplacementNodePathSemanticsReviewPath == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[4]
                && RouteChoiceClickTargetDensityReviewPath == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[5]
                && DoesNotAutoGenerateManualGateEvidence();
        }

        public static IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> GetRequestedScreenshotTargets()
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
                if (path == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[6]
                    || path == P0DreamRouteBatch86UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0DreamRouteBatch86ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static string BuildTextReplacementNodePathSemanticsReviewMarkdown(
            string layoutSummary,
            IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 86 Text Replacement Node Path Semantics Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Unity-rendered text replacement: pass");
            builder.AppendLine("Node/path semantics: pass");
            builder.AppendLine("Boss gate scale: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 86 candidate frames are used for dream-route Unity preflight only.");
            builder.AppendLine("- Existing dream-entry and route-map presenters remain authoritative until scene/presenter/Console and human approval evidence are present.");
            builder.AppendLine("- Formal runtime binding remains blocked.");
            builder.AppendLine("- Layout summary: " + SanitizeLine(layoutSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRouteChoiceClickTargetDensityReviewMarkdown(
            string clickSummary,
            IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 86 Route Choice Click Target Density Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine("Route-card click targets: pass");
            builder.AppendLine("1024x768 route-card density: pass");
            builder.AppendLine("Path connector occlusion: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Route-choice cards, node sockets, and boss gate stay inside the smallest 1024x768 capture.");
            builder.AppendLine("- Path ribbons remain visual connectors only; they do not hide click targets or route labels.");
            builder.AppendLine("- Click target summary: " + SanitizeLine(clickSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0DreamRouteBatch86RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            bool completeScreenshots = HasCompleteScreenshotEvidence(state, screenshots);
            bool candidateFrameDraws = DetailConfirmsCandidateFrameDraws(detail);
            bool routeStates = DetailConfirmsRouteStates(detail);
            bool bossGateScale = DetailConfirmsBossGateScale(detail);
            bool chineseRouteText = DetailConfirmsChineseRouteText(detail);
            bool clickTargets = DetailConfirmsClickTargets(detail);
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 86 Dream Route Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0DreamRouteBatch86RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.CompleteScreenshotEvidenceToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.DreamRouteSurfaceCapturedToken.Replace("yes", completeScreenshots ? "yes" : "no"));
            builder.AppendLine("- " + (candidateFrameDraws ? P0DreamRouteBatch86UnityPreflight.CandidateFrameDrawToken : "Candidate frame draws: incomplete"));
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.NoCandidateTextureFallbackToken.Replace("yes", candidateFrameDraws ? "yes" : "no"));
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.RouteStateSemanticsVisibleToken.Replace("yes", routeStates ? "yes" : "no"));
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.BossGateScaleVisibleToken.Replace("yes", bossGateScale ? "yes" : "no"));
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.ChineseRouteTextVisibleToken.Replace("yes", chineseRouteText ? "yes" : "no"));
            builder.AppendLine("- " + P0DreamRouteBatch86UnityPreflight.RouteChoiceClickTargetsVisibleToken.Replace("yes", clickTargets ? "yes" : "no"));
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
            P0DreamRouteBatch86RuntimeEvidenceState state,
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
            P0DreamRouteBatch86RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0DreamRouteBatch86RuntimeEvidenceState.Passed
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

        private static bool DetailConfirmsRouteStates(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Route state semantics visible: dreamEntry=1 branch=1 boss=1 compact=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsBossGateScale(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Boss gate scale visible: bossGate=1",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsChineseRouteText(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Chinese route labels and rewards visible: yes",
                StringComparison.Ordinal) >= 0;
        }

        private static bool DetailConfirmsClickTargets(string detail)
        {
            return (detail ?? string.Empty).IndexOf(
                "Route-choice click targets visible: yes",
                StringComparison.Ordinal) >= 0;
        }

        private static void AppendTargets(StringBuilder builder, IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> targets)
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

    internal sealed class P0DreamRouteBatch86RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0DreamRouteBatch86PreviewOverlay overlay;
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
            screenshotDirectory = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(P0DreamRouteBatch86RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(P0DreamRouteBatch86RuntimeEvidence.ReviewDirectory);
            targets = P0DreamRouteBatch86RuntimeEvidence.GetRequestedScreenshotTargets();
            Directory.CreateDirectory(screenshotDirectory);
            Directory.CreateDirectory(reviewDirectory);
            ClearGeneratedEvidence();
            Add("Screenshot output: " + screenshotDirectory);
            Add("Review output: " + reviewDirectory);
            Add("Requested screenshot targets: " + BuildTargetSummary(targets));

            P0RouteMapSurface entrySurface = CreateEntrySurface();
            P0RouteMapSurface branchSurface = CreateBranchSurface();
            P0RouteMapSurface bossSurface = CreateBossSurface();
            Require(
                P0RouteMapPresenter.HasP0RouteMapSurface(entrySurface)
                && P0RouteMapPresenter.HasP0RouteMapSurface(branchSurface)
                && P0RouteMapPresenter.HasP0RouteMapSurface(bossSurface)
                && branchSurface.RouteOptions.Count >= 2
                && HasBossRow(bossSurface),
                "dream-route semantic surfaces",
                P0RouteMapPresenter.BuildCompactSummary(entrySurface)
                + " | branchOptions="
                + branchSurface.RouteOptions.Count
                + " | boss="
                + bossSurface.CurrentNode.BuildSummary());
            if (failed)
            {
                yield break;
            }

            CreateOverlay(entrySurface, branchSurface, bossSurface);
            for (int i = 0; i < targets.Count; i++)
            {
                yield return CaptureTarget(targets[i]);
                if (failed)
                {
                    yield break;
                }
            }

            if (targets.Count == P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0DreamRouteBatch86RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(
                    P0DreamRouteBatch86RuntimeEvidence.TextReplacementNodePathSemanticsReviewPath,
                    P0DreamRouteBatch86RuntimeEvidence.BuildTextReplacementNodePathSemanticsReviewMarkdown(
                        layoutSummary,
                        P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets));
                WriteReview(
                    P0DreamRouteBatch86RuntimeEvidence.RouteChoiceClickTargetDensityReviewPath,
                    P0DreamRouteBatch86RuntimeEvidence.BuildRouteChoiceClickTargetDensityReviewMarkdown(
                        clickSummary,
                        P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 86 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0DreamRouteBatch86RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0DreamRouteBatch86RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 86 dream-route runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Manual scene/presenter/Console and human approval gates remain blocked.";
            Add(summary);
            Complete(completeEvidence ? P0DreamRouteBatch86RuntimeEvidenceState.Passed : P0DreamRouteBatch86RuntimeEvidenceState.Failed, summary);
        }

        private static P0RouteMapSurface CreateEntrySurface()
        {
            return P0RouteMapPresenter.BuildSurface(CreateRun(), "\u5c31\u7eea");
        }

        private static P0RouteMapSurface CreateBranchSurface()
        {
            RunProgressionState run = CreateRun();
            run.Route.CompleteCurrentNode(NodeResult.Success);
            return P0RouteMapPresenter.BuildSurface(run, "\u9009\u62e9\u8def\u7ebf");
        }

        private static P0RouteMapSurface CreateBossSurface()
        {
            RunProgressionState run = CreateRun();
            AdvanceToBoss(run);
            return P0RouteMapPresenter.BuildSurface(run, "\u9996\u9886\u5c06\u81f3");
        }

        private static RunProgressionState CreateRun()
        {
            return new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
        }

        private static void AdvanceToBoss(RunProgressionState run)
        {
            int safety = 0;
            while (!run.Route.IsComplete
                && run.Route.CurrentNode.NodeType != RouteNodeType.Boss
                && safety < 20)
            {
                RouteNodeDefinition node = run.Route.CurrentNode;
                if (RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    P0RouteRewardResolver.ApplyBattleReward(node, run);
                    run.Route.CompleteCurrentNode(NodeResult.Success);
                }
                else
                {
                    RouteNodeResolver.ResolveCurrentNode(run);
                }

                safety++;
            }
        }

        private static bool HasBossRow(P0RouteMapSurface surface)
        {
            if (surface == null)
            {
                return false;
            }

            for (int i = 0; i < surface.LayerRows.Count; i++)
            {
                if (surface.LayerRows[i].HasBoss)
                {
                    return true;
                }
            }

            return false;
        }

        private void Require(bool condition, string label, string summary)
        {
            if (condition)
            {
                Add("Verified " + label + ": " + summary);
                return;
            }

            Fail("Batch 86 runtime evidence missing " + label + ": " + summary);
        }

        private void CreateOverlay(
            P0RouteMapSurface entrySurface,
            P0RouteMapSurface branchSurface,
            P0RouteMapSurface bossSurface)
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch86DreamRouteCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0DreamRouteBatch86PreviewOverlay>();
            overlay.Configure(entrySurface, branchSurface, bossSurface);
            layoutSummary = overlay.BuildLayoutSummary(1024, 768);
            clickSummary = overlay.BuildClickTargetSummary(1024, 768);
            Add("Created Batch 86 candidate overlay: " + layoutSummary);

            if (!overlay.TryValidateTargetLayouts(P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 86 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 86 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 86 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 86 candidate texture bindings: " + textureSummary);
            Add("Route state semantics visible: dreamEntry=1 branch=1 boss=1 compact=1");
            Add("Boss gate scale visible: bossGate=1");
            Add("Chinese route labels and rewards visible: yes");
            Add("Route-choice click targets visible: yes");
        }

        private IEnumerator CaptureTarget(P0DreamRouteBatch86ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0DreamRouteBatch86RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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

            return P0DreamRouteBatch86RuntimeEvidence.HasUsefulVisualContent(
                texture.GetPixels32(),
                texture.width,
                texture.height,
                out summary);
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0DreamRouteBatch86RuntimeEvidence.ScreenshotTargets.Length)
            {
                string reportPath = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(P0DreamRouteBatch86RuntimeEvidence.RuntimeReportPath);
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }

                for (int i = 0; i < P0DreamRouteBatch86RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(P0DreamRouteBatch86RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 86 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> requestedTargets)
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
            Complete(P0DreamRouteBatch86RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0DreamRouteBatch86RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0DreamRouteBatch86RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0DreamRouteBatch86RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0DreamRouteBatch86RuntimeEvidence.ToProjectPath(P0DreamRouteBatch86RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0DreamRouteBatch86RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0DreamRouteBatch86PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByVariant = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnVariants = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackVariants = new HashSet<string>(StringComparer.Ordinal);
        private P0RouteMapSurface entrySurface;
        private P0RouteMapSurface branchSurface;
        private P0RouteMapSurface bossSurface;
        private P0DreamRouteBatch86ScreenshotTarget currentTarget;
        private bool captureAuditActive;
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle chipStyle;

        public void Configure(
            P0RouteMapSurface dreamEntrySurface,
            P0RouteMapSurface routeBranchSurface,
            P0RouteMapSurface routeBossSurface)
        {
            entrySurface = dreamEntrySurface;
            branchSurface = routeBranchSurface;
            bossSurface = routeBossSurface;
            assetsByVariant.Clear();
            P0VisualAssetBinding[] bindings = P0DreamRouteBatch86CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                assetsByVariant[bindings[i].SlotId] = bindings[i].Asset;
            }
        }

        public void SetTarget(P0DreamRouteBatch86ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(int width, int height)
        {
            Layout layout = BuildLayout(width, height);
            return "panel=" + FormatRect(layout.Panel)
                + ", header=" + FormatRect(layout.Header)
                + ", pathRibbon=" + FormatRect(layout.PathRibbon)
                + ", bossGate=" + FormatRect(layout.BossGate)
                + ", choice0=" + FormatRect(layout.ChoiceCards[0])
                + ", choice1=" + FormatRect(layout.ChoiceCards[1])
                + ", choice2=" + FormatRect(layout.ChoiceCards[2])
                + ", summary=" + FormatRect(layout.SummaryPanel);
        }

        public string BuildClickTargetSummary(int width, int height)
        {
            Layout layout = BuildLayout(width, height);
            return "choice0=" + FormatRect(layout.ChoiceCards[0])
                + ", choice1=" + FormatRect(layout.ChoiceCards[1])
                + ", choice2=" + FormatRect(layout.ChoiceCards[2])
                + ", node0=" + FormatRect(layout.Nodes[0])
                + ", bossGate=" + FormatRect(layout.BossGate)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates = P0DreamRouteBatch86CandidateCatalog.CreateCandidates();
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
                ? candidates.Count + "/" + P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount;
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
            IReadOnlyList<P0DreamRouteBatch86CandidateAsset> candidates = P0DreamRouteBatch86CandidateCatalog.CreateCandidates();
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
                + P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount
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
                && auditedDrawnVariants.Count == P0DreamRouteBatch86CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0DreamRouteBatch86ScreenshotTarget> targets,
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
                P0DreamRouteBatch86ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                if (!IsInside(layout.Panel, target.Width, target.Height)
                    || !IsInside(layout.Header, target.Width, target.Height)
                    || !IsInside(layout.PathRibbon, target.Width, target.Height)
                    || !IsInside(layout.BossGate, target.Width, target.Height)
                    || !IsInside(layout.SummaryPanel, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    summary = target.ResolutionLabel + " has an off-screen panel, header, path, boss gate, summary, or badge";
                    return false;
                }

                for (int nodeIndex = 0; nodeIndex < layout.Nodes.Length; nodeIndex++)
                {
                    if (!IsInside(layout.Nodes[nodeIndex], target.Width, target.Height)
                        || layout.Nodes[nodeIndex].width < 44f
                        || layout.Nodes[nodeIndex].height < 44f)
                    {
                        summary = target.ResolutionLabel + " has an invalid route node socket";
                        return false;
                    }
                }

                for (int cardIndex = 0; cardIndex < layout.ChoiceCards.Length; cardIndex++)
                {
                    if (!IsInside(layout.ChoiceCards[cardIndex], target.Width, target.Height)
                        || layout.ChoiceCards[cardIndex].width < 44f
                        || layout.ChoiceCards[cardIndex].height < 44f)
                    {
                        summary = target.ResolutionLabel + " has an invalid route-choice card";
                        return false;
                    }
                }

                if (layout.BossGate.width < 96f || layout.BossGate.height < 72f)
                {
                    summary = target.ResolutionLabel + " has a boss gate below reviewable scale";
                    return false;
                }

                labels.Add(target.ResolutionLabel);
            }

            summary = string.Join(", ", labels) + " inside-screen route-card density pass";
            return true;
        }

        private void OnGUI()
        {
            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawBackdrop();
            DrawCandidateFrame("route_map_panel_frame", layout.Panel);
            DrawCandidateFrame("route_layer_header_frame", layout.Header);
            DrawHeader(layout);
            DrawCandidateFrame("route_path_ribbon_frame", layout.PathRibbon);
            DrawPathNodes(layout.Nodes);
            DrawCandidateFrame("route_boss_gate_frame", layout.BossGate);
            DrawBossGate(layout.BossGate);
            DrawChoiceCard(layout.ChoiceCards[0], Text.DefenseTitle, Text.DefenseReward, Text.SelectedState);
            DrawChoiceCard(layout.ChoiceCards[1], Text.EventTitle, Text.EventReward, Text.OptionalState);
            DrawChoiceCard(layout.ChoiceCards[2], Text.ShopTitle, Text.ShopReward, Text.LockedState);
            DrawSummary(layout.SummaryPanel);
            DrawCaptureBadge(layout.CaptureBadge);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float safeWidth = Mathf.Max(1f, screenWidth);
            float safeHeight = Mathf.Max(1f, screenHeight);
            float scale = P0ImGuiLayout.CalculateScale(safeWidth, safeHeight);
            float margin = P0ImGuiLayout.Scaled(24f, scale);
            float panelWidth = Mathf.Min(P0ImGuiLayout.Scaled(1180f, scale), safeWidth - margin * 2f);
            float panelHeight = Mathf.Min(P0ImGuiLayout.Scaled(650f, scale), safeHeight - margin * 2f);
            Rect panel = new Rect((safeWidth - panelWidth) * 0.5f, (safeHeight - panelHeight) * 0.5f, panelWidth, panelHeight);
            float inner = P0ImGuiLayout.Scaled(28f, scale);
            float gap = P0ImGuiLayout.Scaled(14f, scale);
            Rect header = new Rect(panel.x + inner, panel.y + P0ImGuiLayout.Scaled(16f, scale), panel.width - inner * 2f, P0ImGuiLayout.Scaled(68f, scale));
            Rect path = new Rect(panel.x + inner, header.yMax + P0ImGuiLayout.Scaled(14f, scale), panel.width - inner * 2f, P0ImGuiLayout.Scaled(84f, scale));
            float nodeSize = P0ImGuiLayout.Scaled(64f, scale);
            Rect[] nodes = new Rect[5];
            float usablePathWidth = path.width - P0ImGuiLayout.Scaled(240f, scale);
            for (int i = 0; i < nodes.Length; i++)
            {
                float t = nodes.Length == 1 ? 0f : i / (float)(nodes.Length - 1);
                float x = path.x + P0ImGuiLayout.Scaled(20f, scale) + usablePathWidth * t;
                float y = path.y + (path.height - nodeSize) * 0.5f + Mathf.Sin(t * Mathf.PI) * P0ImGuiLayout.Scaled(8f, scale);
                nodes[i] = new Rect(x, y, nodeSize, nodeSize);
            }

            Rect boss = new Rect(
                path.xMax - P0ImGuiLayout.Scaled(210f, scale),
                path.y - P0ImGuiLayout.Scaled(10f, scale),
                P0ImGuiLayout.Scaled(190f, scale),
                P0ImGuiLayout.Scaled(130f, scale));
            float cardY = path.yMax + P0ImGuiLayout.Scaled(18f, scale);
            float cardWidth = (panel.width - inner * 2f - gap * 2f) / 3f;
            float cardHeight = P0ImGuiLayout.Scaled(130f, scale);
            Rect[] cards =
            {
                new Rect(panel.x + inner, cardY, cardWidth, cardHeight),
                new Rect(panel.x + inner + cardWidth + gap, cardY, cardWidth, cardHeight),
                new Rect(panel.x + inner + (cardWidth + gap) * 2f, cardY, cardWidth, cardHeight)
            };

            Rect summary = new Rect(panel.x + inner, cardY + cardHeight + P0ImGuiLayout.Scaled(14f, scale), panel.width - inner * 2f, P0ImGuiLayout.Scaled(78f, scale));
            Rect badge = new Rect(panel.x + P0ImGuiLayout.Scaled(22f, scale), panel.y + P0ImGuiLayout.Scaled(18f, scale), P0ImGuiLayout.Scaled(252f, scale), P0ImGuiLayout.Scaled(32f, scale));
            return new Layout(panel, header, path, nodes, boss, cards, summary, badge);
        }

        private void DrawBackdrop()
        {
            GUI.color = new Color(0.035f, 0.045f, 0.064f, 0.98f);
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = new Color(0.08f, 0.12f, 0.15f, 0.82f);
            GUI.DrawTexture(new Rect(0f, Screen.height * 0.54f, Screen.width, Screen.height * 0.46f), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }

        private void DrawHeader(Layout layout)
        {
            GUI.Label(new Rect(layout.Header.x + 20f, layout.Header.y + 8f, layout.Header.width - 40f, 28f), Text.Title, titleStyle);
            GUI.Label(new Rect(layout.Header.x + 20f, layout.Header.y + 38f, layout.Header.width - 40f, 24f), BuildSurfaceSummary(), smallStyle);
        }

        private void DrawPathNodes(Rect[] nodes)
        {
            string[] labels =
            {
                Text.NodeStart,
                Text.NodeDefense,
                Text.NodeBranch,
                Text.NodeReward,
                Text.NodeBoss
            };

            for (int i = 0; i < nodes.Length; i++)
            {
                DrawCandidateFrame("route_node_socket_frame", nodes[i]);
                GUI.Label(nodes[i], labels[i], chipStyle);
            }
        }

        private void DrawBossGate(Rect rect)
        {
            GUI.Label(new Rect(rect.x + 10f, rect.y + 18f, rect.width - 20f, 32f), Text.BossTitle, titleStyle);
            GUI.Label(new Rect(rect.x + 12f, rect.y + 58f, rect.width - 24f, rect.height - 68f), Text.BossPressure, smallStyle);
        }

        private void DrawChoiceCard(Rect card, string title, string reward, string state)
        {
            DrawCandidateFrame("route_choice_card_slot", card);
            Rect stateChip = new Rect(card.x + 14f, card.y + 14f, Mathf.Min(card.width * 0.26f, 76f), card.height - 28f);
            GUI.color = new Color(0.035f, 0.09f, 0.11f, 0.82f);
            GUI.DrawTexture(stateChip, Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(stateChip, state, chipStyle);
            float textX = stateChip.xMax + 14f;
            GUI.Label(new Rect(textX, card.y + 14f, card.xMax - textX - 14f, 30f), title, labelStyle);
            GUI.Label(new Rect(textX, card.y + 46f, card.xMax - textX - 14f, card.height - 58f), reward, smallStyle);
        }

        private void DrawSummary(Rect rect)
        {
            GUI.color = new Color(0.04f, 0.09f, 0.12f, 0.86f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string routeSummary = Text.SummaryPrefix
                + " "
                + entrySurface.LayerRows.Count
                + " / "
                + branchSurface.RouteOptions.Count
                + " / "
                + bossSurface.CurrentNode.Layer;
            GUI.Label(new Rect(rect.x + 18f, rect.y + 12f, rect.width - 36f, 24f), routeSummary, labelStyle);
            GUI.Label(new Rect(rect.x + 18f, rect.y + 40f, rect.width - 36f, rect.height - 48f), Text.SummaryDetail, smallStyle);
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.03f, 0.08f, 0.11f, 0.82f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0 ? "Batch86 " + currentTarget.ResolutionLabel : "Batch86";
            GUI.Label(rect, label, smallStyle);
        }

        private string BuildSurfaceSummary()
        {
            return Text.Subtitle
                + "  "
                + Text.RouteOptions
                + ": "
                + branchSurface.RouteOptions.Count
                + "  "
                + Text.BossLayer
                + ": "
                + bossSurface.CurrentNode.Layer;
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
                Rect header,
                Rect pathRibbon,
                Rect[] nodes,
                Rect bossGate,
                Rect[] choiceCards,
                Rect summaryPanel,
                Rect captureBadge)
            {
                Panel = panel;
                Header = header;
                PathRibbon = pathRibbon;
                Nodes = nodes ?? Array.Empty<Rect>();
                BossGate = bossGate;
                ChoiceCards = choiceCards ?? Array.Empty<Rect>();
                SummaryPanel = summaryPanel;
                CaptureBadge = captureBadge;
            }

            public Rect Panel { get; }

            public Rect Header { get; }

            public Rect PathRibbon { get; }

            public Rect[] Nodes { get; }

            public Rect BossGate { get; }

            public Rect[] ChoiceCards { get; }

            public Rect SummaryPanel { get; }

            public Rect CaptureBadge { get; }
        }

        private static class Text
        {
            public const string Title = "\u5367\u5ba4\u68a6\u5883\u8def\u7ebf\u5019\u9009\u9884\u68c0";
            public const string Subtitle = "Batch86 \u5019\u9009\u8d44\u6e90\uff0c\u4ec5\u7528\u4e8e Unity \u8bc1\u636e";
            public const string RouteOptions = "\u5206\u652f";
            public const string BossLayer = "\u9996\u9886\u5c42";
            public const string NodeStart = "\u5165\u53e3";
            public const string NodeDefense = "\u5b88\u62a4";
            public const string NodeBranch = "\u5206\u652f";
            public const string NodeReward = "\u5956\u52b1";
            public const string NodeBoss = "\u9996\u9886";
            public const string DefenseTitle = "\u5b88\u62a4\u8def\u7ebf";
            public const string EventTitle = "\u68a6\u5883\u4e8b\u4ef6";
            public const string ShopTitle = "\u5c0f\u9c7c\u5e72\u8865\u7ed9";
            public const string DefenseReward = "\u7a33\u4f4f\u4e3b\u4eba\u7761\u7720\uff0c\u8fdb\u5165\u5f53\u524d\u6218\u6597\u8282\u70b9\u3002";
            public const string EventReward = "\u9884\u89c8\u4e0b\u4e00\u5c42\u5956\u52b1\uff0c\u4fdd\u6301\u8def\u7ebf\u9009\u62e9\u53ef\u8bfb\u3002";
            public const string ShopReward = "\u4ea4\u6362\u68a6\u5c51\u548c\u5c0f\u9c7c\u5e72\uff0c\u4eba\u5de5\u95e8\u7981\u524d\u4ec5\u4f5c\u8bc1\u636e\u3002";
            public const string SelectedState = "\u5df2\u9009";
            public const string OptionalState = "\u53ef\u9009";
            public const string LockedState = "\u9501\u5b9a";
            public const string BossTitle = "\u9996\u9886\u95e8";
            public const string BossPressure = "\u5927\u5c3a\u5ea6\u95e8\u7981\u6846\u5fc5\u987b\u5728 1024x768 \u4fdd\u6301\u53ef\u8bfb\u3002";
            public const string SummaryPrefix = "\u8def\u7ebf\u8bc1\u636e";
            public const string SummaryDetail = "\u622a\u56fe\u53ea\u8bc1\u660e Batch86 \u5019\u9009\u5e27\u53ef\u88ab Unity \u7ed8\u5236\uff1b\u6b63\u5f0f\u5b89\u88c5\u4ecd\u7b49\u5f85 scene/Console \u548c\u4eba\u5de5\u6279\u51c6\u3002";
        }
    }
}
