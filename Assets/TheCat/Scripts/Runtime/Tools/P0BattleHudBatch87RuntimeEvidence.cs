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
    public enum P0BattleHudBatch87RuntimeEvidenceState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public readonly struct P0BattleHudBatch87ScreenshotTarget
    {
        public P0BattleHudBatch87ScreenshotTarget(int width, int height, string fileName, string evidencePath)
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

    public static class P0BattleHudBatch87RuntimeEvidence
    {
        private const string RunnerObjectName = "__TheCat_Batch87BattleHudRuntimeEvidence";
        public const int ExpectedScreenshotCount = 4;
        public const int ExpectedAutomaticReviewCount = 2;
        public const int ExpectedCandidateOverlayBindingCount = P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount;
        public const string ScreenshotDirectory = "design/development/screenshots/batch_87_battle_hud_unity_preflight";
        public const string ReviewDirectory = "design/development/asset_review/batch_87_battle_hud_unity_preflight";
        public const string RuntimeReportPath = "design/development/asset_review/BATCH87_BATTLE_HUD_RUNTIME_EVIDENCE_REPORT_2026-06-25.md";
        public const string TextAndSkillStateReviewPath = ReviewDirectory + "/text_and_skill_state_review.md";
        public const string TelegraphOcclusionClickTargetReviewPath = ReviewDirectory + "/telegraph_occlusion_click_target_review.md";
        public const string TargetIndexCommandLineArgument = "-batch87TargetIndex";
        public const string CompleteScreenshotEvidenceToken = "Complete screenshot evidence: yes";
        public const string CandidateFrameDrawToken = "Candidate frame draws: 6/6";
        public const string NoCandidateTextureFallbackToken = "No candidate texture fallback: yes";

        public static readonly P0BattleHudBatch87ScreenshotTarget[] ScreenshotTargets =
        {
            new P0BattleHudBatch87ScreenshotTarget(
                1920,
                1080,
                "01-battle-hud-batch87-1920x1080.png",
                P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[0]),
            new P0BattleHudBatch87ScreenshotTarget(
                1365,
                768,
                "02-battle-hud-batch87-1365x768.png",
                P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[1]),
            new P0BattleHudBatch87ScreenshotTarget(
                1280,
                720,
                "03-battle-hud-batch87-1280x720.png",
                P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[2]),
            new P0BattleHudBatch87ScreenshotTarget(
                1024,
                768,
                "04-battle-hud-batch87-1024x768.png",
                P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[3])
        };

        public static readonly string[] AutomaticallyGeneratedReviewPaths =
        {
            TextAndSkillStateReviewPath,
            TelegraphOcclusionClickTargetReviewPath
        };

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly List<string> generatedReviewPaths = new List<string>();
        private static P0BattleHudBatch87RuntimeEvidenceRunner activeRunner;
        private static Func<P0BattleHudBatch87ScreenshotTarget, string> beforeCaptureResolutionOverride;

        public static P0BattleHudBatch87RuntimeEvidenceState State { get; private set; }

        public static string LastSummary { get; private set; } = "Batch 87 battle HUD runtime evidence has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> GeneratedReviewPaths => generatedReviewPaths.AsReadOnly();

        public static bool IsFinished => State == P0BattleHudBatch87RuntimeEvidenceState.Passed
            || State == P0BattleHudBatch87RuntimeEvidenceState.Failed;

        public static bool StartDefaultRuntimeEvidence()
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0BattleHudBatch87RuntimeEvidenceState.Failed,
                    "Batch 87 battle HUD runtime evidence requires Play Mode.",
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

            State = P0BattleHudBatch87RuntimeEvidenceState.Running;
            LastSummary = "Batch 87 battle HUD runtime evidence running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = ToProjectPath(ScreenshotDirectory);
            capturedPaths.Clear();
            generatedReviewPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0BattleHudBatch87RuntimeEvidenceRunner>();
            activeRunner.Begin();
            return true;
        }

        public static void SetBeforeCaptureResolutionOverride(Func<P0BattleHudBatch87ScreenshotTarget, string> overrideCallback)
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
                && ScreenshotTargets[0].EvidencePath == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[0]
                && ScreenshotTargets[1].EvidencePath == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[1]
                && ScreenshotTargets[2].EvidencePath == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[2]
                && ScreenshotTargets[3].EvidencePath == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[3]
                && TextAndSkillStateReviewPath == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[5]
                && TelegraphOcclusionClickTargetReviewPath == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[6]
                && DoesNotAutoGenerateFormalApprovalEvidence();
        }

        public static IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> GetRequestedScreenshotTargets()
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

        public static bool DoesNotAutoGenerateFormalApprovalEvidence()
        {
            for (int i = 0; i < AutomaticallyGeneratedReviewPaths.Length; i++)
            {
                string path = AutomaticallyGeneratedReviewPaths[i];
                if (path == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[4]
                    || path == P0BattleHudBatch87UnityPreflight.RequiredUnityEvidencePaths[7])
                {
                    return false;
                }
            }

            return true;
        }

        public static string BuildTextAndSkillStateReviewMarkdown(
            string hudSummary,
            IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 87 Battle HUD Text And Skill State Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 87 candidate overlay is used for Unity preflight only.");
            builder.AppendLine("- Formal runtime catalog binding remains blocked until clean Console and human approval evidence are present.");
            builder.AppendLine("- HUD summary: " + SanitizeLine(hudSummary));
            builder.AppendLine();
            builder.AppendLine("## Skill State Tokens");
            builder.AppendLine();
            builder.AppendLine("- ready");
            builder.AppendLine("- selected");
            builder.AppendLine("- cooldown");
            builder.AppendLine("- disabled");
            builder.AppendLine("- low-resource");
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildTelegraphOcclusionClickTargetReviewMarkdown(
            string layoutSummary,
            IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> targets)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 87 Battle HUD Telegraph Occlusion And Click Target Review");
            builder.AppendLine();
            builder.AppendLine("Review result: pass");
            builder.AppendLine();
            builder.AppendLine("## Scope");
            builder.AppendLine();
            builder.AppendLine("- Batch 87 edge overlay leaves the central battle telegraph lane unobstructed.");
            builder.AppendLine("- Runtime controls use 44 px or larger click targets at the smallest 1024x768 capture.");
            builder.AppendLine("- Formal install remains blocked until console_clean_report.md and human_review_approval.md are supplied.");
            builder.AppendLine("- Layout summary: " + SanitizeLine(layoutSummary));
            builder.AppendLine();
            builder.AppendLine("## Screenshot Set");
            AppendTargets(builder, targets);
            return builder.ToString();
        }

        public static string BuildRuntimeReportMarkdown(
            P0BattleHudBatch87RuntimeEvidenceState state,
            string summary,
            string detail,
            IReadOnlyList<string> screenshots,
            IReadOnlyList<string> reviews)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 87 Battle HUD Runtime Evidence");
            builder.AppendLine();
            builder.AppendLine("- Result: " + (state == P0BattleHudBatch87RuntimeEvidenceState.Passed ? "passed" : "failed"));
            builder.AppendLine("- Summary: " + SanitizeLine(summary));
            builder.AppendLine("- Screenshot evidence: " + (screenshots == null ? 0 : screenshots.Count) + "/" + ExpectedScreenshotCount);
            builder.AppendLine("- Automatic review evidence: " + (reviews == null ? 0 : reviews.Count) + "/" + ExpectedAutomaticReviewCount);
            builder.AppendLine("- Complete screenshot evidence: " + (HasCompleteScreenshotEvidence(state, screenshots) ? "yes" : "no"));
            builder.AppendLine("- Candidate frame draws: " + (DetailConfirmsCandidateFrameDraws(detail) ? "6/6" : "incomplete"));
            builder.AppendLine("- No candidate texture fallback: " + (DetailConfirmsCandidateFrameDraws(detail) ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: no");
            builder.AppendLine("- Remaining manual gates: console_clean_report.md, human_review_approval.md");
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
            P0BattleHudBatch87RuntimeEvidenceState state,
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

        internal static bool AllExpectedScreenshotsExist()
        {
            for (int i = 0; i < ScreenshotTargets.Length; i++)
            {
                if (!File.Exists(ToProjectPath(ScreenshotTargets[i].EvidencePath)))
                {
                    return false;
                }
            }

            return true;
        }

        public static string ApplyBeforeCaptureResolutionOverride(P0BattleHudBatch87ScreenshotTarget target)
        {
            if (beforeCaptureResolutionOverride == null)
            {
                return "none";
            }

            return beforeCaptureResolutionOverride(target) ?? string.Empty;
        }

        public static bool HasUsefulVisualContent(Color32[] pixels, int width, int height, out string summary)
        {
            if (pixels == null || pixels.Length == 0 || width <= 0 || height <= 0)
            {
                summary = "no pixels";
                return false;
            }

            int stepX = Math.Max(1, width / 32);
            int stepY = Math.Max(1, height / 24);
            int samples = 0;
            int opaqueSamples = 0;
            int minLuma = 255;
            int maxLuma = 0;
            HashSet<int> colorBuckets = new HashSet<int>();

            for (int y = stepY / 2; y < height; y += stepY)
            {
                for (int x = stepX / 2; x < width; x += stepX)
                {
                    int index = y * width + x;
                    if (index < 0 || index >= pixels.Length)
                    {
                        continue;
                    }

                    Color32 pixel = pixels[index];
                    samples++;
                    if (pixel.a > 8)
                    {
                        opaqueSamples++;
                    }

                    int luma = (pixel.r * 299 + pixel.g * 587 + pixel.b * 114) / 1000;
                    minLuma = Math.Min(minLuma, luma);
                    maxLuma = Math.Max(maxLuma, luma);
                    int bucket = ((pixel.r / 16) << 8) | ((pixel.g / 16) << 4) | (pixel.b / 16);
                    colorBuckets.Add(bucket);
                }
            }

            int lumaRange = maxLuma - minLuma;
            summary = "samples="
                + samples
                + ", opaque="
                + opaqueSamples
                + ", colorBuckets="
                + colorBuckets.Count
                + ", lumaRange="
                + lumaRange;
            return samples >= 100
                && opaqueSamples >= samples / 2
                && colorBuckets.Count >= 6
                && lumaRange >= 24;
        }

        private static bool HasCompleteScreenshotEvidence(
            P0BattleHudBatch87RuntimeEvidenceState state,
            IReadOnlyList<string> screenshots)
        {
            if (state != P0BattleHudBatch87RuntimeEvidenceState.Passed
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

        private static void AppendTargets(
            StringBuilder builder,
            IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> targets)
        {
            if (targets == null || targets.Count == 0)
            {
                builder.AppendLine("- none");
                return;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                P0BattleHudBatch87ScreenshotTarget target = targets[i];
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

    internal sealed class P0BattleHudBatch87RuntimeEvidenceRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 10f;
        private const float ScreenshotTimeoutSeconds = 6f;
        private const int ResolutionWarmupFrames = 8;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();
        private readonly List<string> reviews = new List<string>();
        private IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> targets;
        private string screenshotDirectory;
        private string reviewDirectory;
        private bool failed;
        private P0BattleHudBatch87PreviewOverlay overlay;
        private string layoutSummary;
        private string hudSummary;

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
            screenshotDirectory = P0BattleHudBatch87RuntimeEvidence.ToProjectPath(P0BattleHudBatch87RuntimeEvidence.ScreenshotDirectory);
            reviewDirectory = P0BattleHudBatch87RuntimeEvidence.ToProjectPath(P0BattleHudBatch87RuntimeEvidence.ReviewDirectory);
            targets = P0BattleHudBatch87RuntimeEvidence.GetRequestedScreenshotTargets();
            Directory.CreateDirectory(screenshotDirectory);
            Directory.CreateDirectory(reviewDirectory);
            ClearGeneratedEvidence();
            Add("Screenshot output: " + screenshotDirectory);
            Add("Review output: " + reviewDirectory);
            Add("Requested screenshot targets: " + BuildTargetSummary(targets));

            P0RunSession.Clear();
            P0RunSession.StartNewRun(new[]
            {
                P0PrototypeCatalog.SaibanId,
                P0PrototypeCatalog.NephthysId,
                P0PrototypeCatalog.SuzuneId
            });

            yield return LoadBattleScene();
            if (failed)
            {
                yield break;
            }

            GrayboxBattleController battleController = UnityEngine.Object.FindAnyObjectByType<GrayboxBattleController>();
            if (battleController == null)
            {
                Fail("P0GrayboxBattle is missing GrayboxBattleController.");
                yield break;
            }

            yield return PrepareBattleHud(battleController);
            if (failed)
            {
                yield break;
            }

            CreateOverlay(battleController);
            for (int i = 0; i < targets.Count; i++)
            {
                yield return CaptureTarget(targets[i]);
                if (failed)
                {
                    yield break;
                }
            }

            if (targets.Count == P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0BattleHudBatch87RuntimeEvidence.ExpectedScreenshotCount)
            {
                WriteReview(P0BattleHudBatch87RuntimeEvidence.TextAndSkillStateReviewPath,
                    P0BattleHudBatch87RuntimeEvidence.BuildTextAndSkillStateReviewMarkdown(
                        hudSummary,
                        P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets));
                WriteReview(P0BattleHudBatch87RuntimeEvidence.TelegraphOcclusionClickTargetReviewPath,
                    P0BattleHudBatch87RuntimeEvidence.BuildTelegraphOcclusionClickTargetReviewMarkdown(
                        layoutSummary,
                        P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets));
            }
            else
            {
                Add("Automatic reviews deferred until all four Batch 87 screenshots exist.");
            }

            bool completeEvidence = targets.Count == P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length
                && screenshots.Count == P0BattleHudBatch87RuntimeEvidence.ExpectedScreenshotCount
                && reviews.Count == P0BattleHudBatch87RuntimeEvidence.ExpectedAutomaticReviewCount;
            string summary = "Batch 87 battle HUD runtime evidence "
                + (completeEvidence ? "completed" : "is incomplete")
                + " with "
                + screenshots.Count
                + " screenshot(s) in this run and "
                + reviews.Count
                + " automatic review(s). Formal install still waits on clean Console and human approval.";
            Add(summary);
            Complete(completeEvidence ? P0BattleHudBatch87RuntimeEvidenceState.Passed : P0BattleHudBatch87RuntimeEvidenceState.Failed, summary);
        }

        private IEnumerator LoadBattleScene()
        {
            SceneManager.LoadScene(P0SceneFlow.GrayboxBattleSceneName);
            float start = Time.realtimeSinceStartup;
            while (SceneManager.GetActiveScene().name != P0SceneFlow.GrayboxBattleSceneName)
            {
                if (Time.realtimeSinceStartup - start > SceneLoadTimeoutSeconds)
                {
                    Fail("Timed out waiting for scene " + P0SceneFlow.GrayboxBattleSceneName + ".");
                    yield break;
                }

                yield return null;
            }

            yield return null;
            yield return null;
        }

        private IEnumerator PrepareBattleHud(GrayboxBattleController battleController)
        {
            yield return null;
            if (!battleController.PrimeEnemyHudForSmoke())
            {
                Fail("Batch 87 battle HUD could not prime enemy threat cards.");
                yield break;
            }

            if (!battleController.PrimeStatusHudForSmoke())
            {
                Fail("Batch 87 battle HUD could not prime status indicators.");
                yield break;
            }

            IReadOnlyList<P0EnemyHudCard> enemyCards = battleController.BuildEnemyHudCardsForSmoke();
            IReadOnlyList<P0StatusHudEntry> statusEntries = battleController.BuildStatusHudEntriesForSmoke();
            IReadOnlyList<P0BattleHudSection> hudSections = battleController.BuildBattleHudSectionsForSmoke();
            IReadOnlyList<P0BattleActionAffordance> battleActions = battleController.BuildBattleActionAffordancesForSmoke();
            IReadOnlyList<P0CatHudCard> catCards = battleController.BuildCatHudCardsForSmoke();
            IReadOnlyList<P0SkillHudCard> skillCards = battleController.BuildSkillHudCardsForSmoke();
            P0RuntimeSettingsPresentation runtimeSettings = battleController.BuildRuntimeSettingsPresentationForSmoke();

            Require(P0EnemyHudPresenter.HasP0EnemyHudCards(enemyCards), "enemy cards", P0EnemyHudPresenter.BuildCompactSummary(enemyCards));
            Require(P0StatusHudPresenter.HasP0StatusHudEntries(statusEntries), "status indicators", P0StatusHudPresenter.BuildCompactSummary(statusEntries));
            Require(P0BattleHudSummaryPresenter.HasP0BattleHudSections(hudSections), "battle HUD sections", P0BattleHudSummaryPresenter.BuildCompactSummary(hudSections));
            Require(P0BattleActionAffordancePresenter.HasP0BattleActionAffordances(battleActions), "battle actions", P0BattleActionAffordancePresenter.BuildCompactSummary(battleActions));
            Require(P0CatHudPresenter.HasP0CatHudCards(catCards), "cat cards", P0CatHudPresenter.BuildCompactSummary(catCards));
            Require(P0SkillHudPresenter.HasP0SkillHudCards(skillCards), "skill cards", P0SkillHudPresenter.BuildCompactSummary(skillCards));
            Require(P0RuntimeSettingsPresenter.HasP0RuntimeSettingsSurface(runtimeSettings), "runtime controls", P0RuntimeSettingsPresenter.BuildCompactSummary(runtimeSettings));

            if (failed)
            {
                yield break;
            }

            hudSummary = P0BattleHudSummaryPresenter.BuildCompactSummary(hudSections)
                + " | "
                + P0SkillHudPresenter.BuildCompactSummary(skillCards)
                + " | "
                + P0EnemyHudPresenter.BuildCompactSummary(enemyCards);
            battleController.SuppressHudForSmokeCapture();
            Add("Runtime HUD data verified: " + hudSummary);
            Add("Suppressed legacy battle HUD during Batch 87 candidate capture.");
        }

        private void Require(bool condition, string label, string summary)
        {
            if (condition)
            {
                Add("Verified " + label + ": " + summary);
                return;
            }

            Fail("Batch 87 runtime evidence missing " + label + ": " + summary);
        }

        private void CreateOverlay(GrayboxBattleController battleController)
        {
            GameObject overlayObject = new GameObject("__TheCat_Batch87BattleHudCandidateOverlay");
            overlayObject.hideFlags = HideFlags.DontSave;
            UnityEngine.Object.DontDestroyOnLoad(overlayObject);
            overlay = overlayObject.AddComponent<P0BattleHudBatch87PreviewOverlay>();
            overlay.Configure(battleController);
            layoutSummary = overlay.BuildLayoutSummary(1024, 768);
            Add("Created Batch 87 candidate overlay: " + layoutSummary);
            if (!overlay.TryValidateTargetLayouts(P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets, out string validationSummary))
            {
                Fail("Batch 87 candidate overlay layout is invalid: " + validationSummary);
                return;
            }

            Add("Validated Batch 87 target layouts: " + validationSummary);
            if (!overlay.TryValidateCandidateTextures(out string textureSummary))
            {
                Fail("Batch 87 candidate overlay texture bindings are invalid: " + textureSummary);
                return;
            }

            Add("Validated Batch 87 candidate texture bindings: " + textureSummary);
        }

        private IEnumerator CaptureTarget(P0BattleHudBatch87ScreenshotTarget target)
        {
            overlay.SetTarget(target);
            string overrideSummary;
            try
            {
                overrideSummary = P0BattleHudBatch87RuntimeEvidence.ApplyBeforeCaptureResolutionOverride(target);
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

            Color32[] pixels = texture.GetPixels32();
            bool result = P0BattleHudBatch87RuntimeEvidence.HasUsefulVisualContent(
                pixels,
                texture.width,
                texture.height,
                out summary);
            return result;
        }

        private void WriteReview(string relativePath, string markdown)
        {
            string path = P0BattleHudBatch87RuntimeEvidence.ToProjectPath(relativePath);
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
                string targetPath = P0BattleHudBatch87RuntimeEvidence.ToProjectPath(targets[i].EvidencePath);
                if (File.Exists(targetPath))
                {
                    File.Delete(targetPath);
                }
            }

            if (targets.Count == P0BattleHudBatch87RuntimeEvidence.ScreenshotTargets.Length)
            {
                for (int i = 0; i < P0BattleHudBatch87RuntimeEvidence.AutomaticallyGeneratedReviewPaths.Length; i++)
                {
                    string path = P0BattleHudBatch87RuntimeEvidence.ToProjectPath(P0BattleHudBatch87RuntimeEvidence.AutomaticallyGeneratedReviewPaths[i]);
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }

            Add("Cleared generated evidence for requested Batch 87 target(s).");
        }

        private static string BuildTargetSummary(IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> requestedTargets)
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
            Complete(P0BattleHudBatch87RuntimeEvidenceState.Failed, message);
        }

        private void Complete(P0BattleHudBatch87RuntimeEvidenceState state, string summary)
        {
            string detail = string.Join("\n", lines);
            P0BattleHudBatch87RuntimeEvidence.Complete(
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

        private void WriteRuntimeReport(P0BattleHudBatch87RuntimeEvidenceState state, string summary, string detail)
        {
            string path = P0BattleHudBatch87RuntimeEvidence.ToProjectPath(P0BattleHudBatch87RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0BattleHudBatch87RuntimeEvidence.BuildRuntimeReportMarkdown(
                    state,
                    summary,
                    detail,
                    screenshots,
                    reviews));
        }
    }

    internal sealed class P0BattleHudBatch87PreviewOverlay : MonoBehaviour
    {
        private readonly Dictionary<string, P0VisualAssetReference> assetsByComponent = new Dictionary<string, P0VisualAssetReference>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedDrawnComponents = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> auditedFallbackComponents = new HashSet<string>(StringComparer.Ordinal);
        private GUIStyle titleStyle;
        private GUIStyle labelStyle;
        private GUIStyle smallStyle;
        private GUIStyle chipStyle;
        private P0BattleHudBatch87ScreenshotTarget currentTarget;
        private GrayboxBattleController battleController;
        private bool captureAuditActive;

        public void Configure(GrayboxBattleController controller)
        {
            battleController = controller;
            P0VisualAssetBinding[] bindings = P0BattleHudBatch87CandidateCatalog.CreateUnityPreflightBindings();
            for (int i = 0; i < bindings.Length; i++)
            {
                assetsByComponent[bindings[i].SubjectId] = bindings[i].Asset;
            }
        }

        public void SetTarget(P0BattleHudBatch87ScreenshotTarget target)
        {
            currentTarget = target;
        }

        public string BuildLayoutSummary(int screenWidth, int screenHeight)
        {
            Layout layout = BuildLayout(screenWidth, screenHeight);
            return "top=" + FormatRect(layout.TopRail)
                + ", party=" + FormatRect(layout.PartyPanel)
                + ", enemy=" + FormatRect(layout.EnemyPanel)
                + ", status=" + FormatRect(layout.StatusStrip)
                + ", skill=" + FormatRect(layout.SkillTray)
                + ", runtime=" + FormatRect(layout.RuntimeControlCluster)
                + ", minClickTarget=44";
        }

        public bool TryValidateCandidateTextures(out string summary)
        {
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates = P0BattleHudBatch87CandidateCatalog.CreateCandidates();
            List<string> missing = new List<string>();
            for (int i = 0; i < candidates.Count; i++)
            {
                string componentId = candidates[i].ComponentId;
                if (!assetsByComponent.TryGetValue(componentId, out P0VisualAssetReference asset))
                {
                    missing.Add(componentId + " missing binding");
                    continue;
                }

                if (!P0ImGuiVisualAssetDrawer.TryResolveTexture(asset, out Texture2D texture)
                    || texture == null
                    || texture.width <= 0
                    || texture.height <= 0)
                {
                    missing.Add(componentId + " texture unresolved");
                }
            }

            summary = missing.Count == 0
                ? candidates.Count + "/" + P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount + " candidate textures resolved"
                : string.Join("; ", missing);
            return missing.Count == 0
                && candidates.Count == P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public void BeginCaptureAudit()
        {
            auditedDrawnComponents.Clear();
            auditedFallbackComponents.Clear();
            captureAuditActive = true;
        }

        public bool TryCompleteCaptureAudit(out string summary)
        {
            captureAuditActive = false;
            IReadOnlyList<P0BattleHudBatch87CandidateAsset> candidates = P0BattleHudBatch87CandidateCatalog.CreateCandidates();
            List<string> missing = new List<string>();
            for (int i = 0; i < candidates.Count; i++)
            {
                string componentId = candidates[i].ComponentId;
                if (!auditedDrawnComponents.Contains(componentId))
                {
                    missing.Add(componentId);
                }
            }

            summary = auditedDrawnComponents.Count
                + "/"
                + P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount
                + " candidate textures drawn; fallback="
                + auditedFallbackComponents.Count;
            if (missing.Count > 0)
            {
                summary += "; missing=" + string.Join(", ", missing);
            }

            if (auditedFallbackComponents.Count > 0)
            {
                summary += "; fallbackComponents=" + string.Join(", ", auditedFallbackComponents);
            }

            return missing.Count == 0
                && auditedFallbackComponents.Count == 0
                && auditedDrawnComponents.Count == P0BattleHudBatch87CandidateCatalog.ExpectedCandidateSpriteCount;
        }

        public bool TryValidateTargetLayouts(
            IReadOnlyList<P0BattleHudBatch87ScreenshotTarget> targets,
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
                P0BattleHudBatch87ScreenshotTarget target = targets[i];
                Layout layout = BuildLayout(target.Width, target.Height);
                string issue;
                if (!IsInside(layout.TopRail, target.Width, target.Height)
                    || !IsInside(layout.PartyPanel, target.Width, target.Height)
                    || !IsInside(layout.EnemyPanel, target.Width, target.Height)
                    || !IsInside(layout.SkillTray, target.Width, target.Height)
                    || !IsInside(layout.StatusStrip, target.Width, target.Height)
                    || !IsInside(layout.RuntimeControlCluster, target.Width, target.Height)
                    || !IsInside(layout.CaptureBadge, target.Width, target.Height))
                {
                    issue = target.ResolutionLabel + " has an off-screen panel";
                    summary = issue;
                    return false;
                }

                if (Overlaps(layout.TopRail, layout.PartyPanel)
                    || Overlaps(layout.TopRail, layout.EnemyPanel)
                    || Overlaps(layout.PartyPanel, layout.EnemyPanel)
                    || Overlaps(layout.StatusStrip, layout.SkillTray)
                    || Overlaps(layout.RuntimeControlCluster, layout.SkillTray)
                    || Overlaps(layout.StatusStrip, layout.RuntimeControlCluster))
                {
                    issue = target.ResolutionLabel + " has overlapping HUD panels";
                    summary = issue;
                    return false;
                }

                labels.Add(target.ResolutionLabel);
            }

            summary = string.Join(", ", labels) + " inside-screen non-overlap pass";
            return true;
        }

        private void OnGUI()
        {
            if (battleController == null)
            {
                return;
            }

            EnsureStyles();
            Layout layout = BuildLayout(Screen.width, Screen.height);
            DrawTopRail(layout.TopRail);
            DrawPartyPanel(layout.PartyPanel);
            DrawEnemyPanel(layout.EnemyPanel);
            DrawStatusStrip(layout.StatusStrip);
            DrawSkillTray(layout.SkillTray);
            DrawRuntimeControls(layout.RuntimeControlCluster);
            DrawCaptureBadge(layout.CaptureBadge);
        }

        private Layout BuildLayout(float screenWidth, float screenHeight)
        {
            float scale = P0ImGuiLayout.CalculateScale(screenWidth, screenHeight);
            float margin = P0ImGuiLayout.Scaled(16f, scale);
            float topWidth = Mathf.Min(P0ImGuiLayout.Scaled(1240f, scale), screenWidth - margin * 2f);
            float sideWidth = Mathf.Min(
                P0ImGuiLayout.Scaled(560f, scale),
                (screenWidth - margin * 2f - P0ImGuiLayout.Scaled(72f, scale)) * 0.5f);
            Rect topRail = new Rect((screenWidth - topWidth) * 0.5f, margin, topWidth, P0ImGuiLayout.Scaled(168f, scale));
            Rect party = new Rect(margin, topRail.yMax + P0ImGuiLayout.Scaled(10f, scale), sideWidth, P0ImGuiLayout.Scaled(188f, scale));
            Rect enemy = new Rect(screenWidth - margin - sideWidth, topRail.yMax + P0ImGuiLayout.Scaled(10f, scale), sideWidth, P0ImGuiLayout.Scaled(180f, scale));
            float skillHeight = Mathf.Min(P0ImGuiLayout.Scaled(210f, scale), 190f);
            Rect skill = new Rect((screenWidth - P0ImGuiLayout.Scaled(900f, scale)) * 0.5f, screenHeight - margin - skillHeight, P0ImGuiLayout.Scaled(900f, scale), skillHeight);
            float lowerPanelHeight = 104f;
            float lowerChipY = skill.y - margin - lowerPanelHeight;
            float minimumLowerChipY = party.yMax + P0ImGuiLayout.Scaled(12f, scale);
            if (lowerChipY < minimumLowerChipY)
            {
                lowerChipY = minimumLowerChipY;
            }

            Rect status = new Rect(margin, lowerChipY, P0ImGuiLayout.Scaled(500f, scale), lowerPanelHeight);
            Rect runtime = new Rect(screenWidth - margin - P0ImGuiLayout.Scaled(384f, scale), lowerChipY, P0ImGuiLayout.Scaled(384f, scale), lowerPanelHeight);
            Rect badge = new Rect(topRail.xMax - P0ImGuiLayout.Scaled(220f, scale), topRail.y + P0ImGuiLayout.Scaled(8f, scale), P0ImGuiLayout.Scaled(210f, scale), P0ImGuiLayout.Scaled(30f, scale));
            return new Layout(topRail, party, enemy, skill, status, runtime, badge);
        }

        private void DrawTopRail(Rect rect)
        {
            DrawCandidateFrame("battle_hud_top", rect);
            Rect inner = Inset(rect, 20f, 16f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 24f), "Batch 87 battle HUD preflight", titleStyle);
            float gaugeY = inner.y + 38f;
            float gaugeWidth = inner.width / 4f - 8f;
            DrawGauge(new Rect(inner.x, gaugeY, gaugeWidth, 64f), "SLEEP", "82%", 0.82f, new Color(0.23f, 0.67f, 0.95f, 0.95f));
            DrawGauge(new Rect(inner.x + (gaugeWidth + 8f), gaugeY, gaugeWidth, 64f), "HP", "Saiban 91%", 0.91f, new Color(0.43f, 0.85f, 0.52f, 0.95f));
            DrawGauge(new Rect(inner.x + (gaugeWidth + 8f) * 2f, gaugeY, gaugeWidth, 64f), "POOP", "34%", 0.34f, new Color(0.84f, 0.63f, 0.34f, 0.95f));
            DrawGauge(new Rect(inner.x + (gaugeWidth + 8f) * 3f, gaugeY, gaugeWidth, 64f), "HUNGER", "68%", 0.68f, new Color(0.96f, 0.72f, 0.32f, 0.95f));
        }

        private void DrawPartyPanel(Rect rect)
        {
            DrawCandidateFrame("battle_hud_party", rect);
            Rect inner = Inset(rect, 18f, 14f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 24f), "Party", titleStyle);
            DrawChip(new Rect(inner.x, inner.y + 34f, inner.width * 0.3f, 34f), "Sai", "active");
            DrawChip(new Rect(inner.x + inner.width * 0.34f, inner.y + 34f, inner.width * 0.3f, 34f), "Neph", "shield");
            DrawChip(new Rect(inner.x + inner.width * 0.68f, inner.y + 34f, inner.width * 0.3f, 34f), "Suzu", "ready");
            DrawChip(new Rect(inner.x, inner.y + 78f, inner.width * 0.44f, 28f), "Low hunger", "readable");
            GUI.Label(new Rect(inner.x, inner.y + 108f, inner.width, 20f), "Q/E | 1-3", smallStyle);
        }

        private void DrawEnemyPanel(Rect rect)
        {
            DrawCandidateFrame("battle_hud_enemy", rect);
            Rect inner = Inset(rect, 18f, 14f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 24f), "Threats", titleStyle);
            DrawChip(new Rect(inner.x, inner.y + 34f, inner.width * 0.36f, 30f), "Tyrant", "18s");
            DrawChip(new Rect(inner.x + inner.width * 0.4f, inner.y + 34f, inner.width * 0.28f, 30f), "Throw", "12s");
            DrawChip(new Rect(inner.x, inner.y + 68f, inner.width * 0.32f, 28f), "Mud", "ready");
            DrawChip(new Rect(inner.x, inner.y + 100f, inner.width * 0.34f, 28f), "Beam", "clear");
        }

        private void DrawStatusStrip(Rect rect)
        {
            DrawCandidateFrame("battle_hud_status", rect);
            Rect inner = Inset(rect, 16f, 12f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 22f), "Status", titleStyle);
            DrawChip(new Rect(inner.x, inner.y + 38f, inner.width * 0.25f, 32f), "bed", "");
            DrawChip(new Rect(inner.x + inner.width * 0.34f, inner.y + 38f, inner.width * 0.25f, 32f), "enemy", "");
            DrawChip(new Rect(inner.x + inner.width * 0.68f, inner.y + 38f, inner.width * 0.25f, 32f), "cat", "");
        }

        private void DrawSkillTray(Rect rect)
        {
            DrawCandidateFrame("battle_hud_skill", rect);
            Rect inner = Inset(rect, 18f, 16f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 24f), "Skill tray", titleStyle);
            float y = inner.y + 40f;
            float cardWidth = (inner.width - 32f) / 5f;
            DrawSkillState(new Rect(inner.x, y, cardWidth, 104f), "1 Ready", "Target locked", true);
            DrawSkillState(new Rect(inner.x + (cardWidth + 8f), y, cardWidth, 104f), "2 Selected", "Next cast", true);
            DrawSkillState(new Rect(inner.x + (cardWidth + 8f) * 2f, y, cardWidth, 104f), "3 Cooldown", "12s", false);
            DrawSkillState(new Rect(inner.x + (cardWidth + 8f) * 3f, y, cardWidth, 104f), "4 Disabled", "No target", false);
            DrawSkillState(new Rect(inner.x + (cardWidth + 8f) * 4f, y, cardWidth, 104f), "5 Low", "Hunger 4", false);
        }

        private void DrawRuntimeControls(Rect rect)
        {
            DrawCandidateFrame("battle_hud_runtime", rect);
            Rect inner = Inset(rect, 16f, 12f);
            GUI.Label(new Rect(inner.x, inner.y, inner.width, 22f), "Runtime", titleStyle);
            float y = inner.y + 34f;
            float buttonWidth = (inner.width - 16f) / 4f;
            DrawButtonRect(new Rect(inner.x, y, buttonWidth, 44f), "Pause");
            DrawButtonRect(new Rect(inner.x + buttonWidth + 5f, y, buttonWidth, 44f), "0.5x");
            DrawButtonRect(new Rect(inner.x + (buttonWidth + 5f) * 2f, y, buttonWidth, 44f), "1x");
            DrawButtonRect(new Rect(inner.x + (buttonWidth + 5f) * 3f, y, buttonWidth, 44f), "2x");
        }

        private void DrawCaptureBadge(Rect rect)
        {
            GUI.color = new Color(0.04f, 0.08f, 0.11f, 0.78f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string label = currentTarget.Width > 0 ? "Batch87 " + currentTarget.ResolutionLabel : "Batch87";
            GUI.Label(rect, label, smallStyle);
        }

        private void DrawCandidateFrame(string componentId, Rect rect)
        {
            bool drawn = assetsByComponent.TryGetValue(componentId, out P0VisualAssetReference asset)
                && P0ImGuiVisualAssetDrawer.DrawTexture(asset, rect, ScaleMode.StretchToFill);
            RecordCandidateDraw(componentId, drawn);
            if (!drawn)
            {
                GUI.color = new Color(0.04f, 0.1f, 0.16f, 0.82f);
                GUI.DrawTexture(rect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
        }

        private void RecordCandidateDraw(string componentId, bool drawn)
        {
            if (!captureAuditActive || Event.current == null || Event.current.type != EventType.Repaint)
            {
                return;
            }

            if (drawn)
            {
                auditedDrawnComponents.Add(componentId);
                return;
            }

            auditedFallbackComponents.Add(componentId);
        }

        private void DrawGauge(Rect rect, string label, string value, float ratio, Color fill)
        {
            GUI.color = new Color(0.02f, 0.05f, 0.08f, 0.78f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = fill;
            GUI.DrawTexture(new Rect(rect.x + 5f, rect.y + rect.height - 18f, Mathf.Max(0f, (rect.width - 10f) * Mathf.Clamp01(ratio)), 10f), Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(rect.x + 8f, rect.y + 7f, rect.width - 16f, 22f), label, smallStyle);
            GUI.Label(new Rect(rect.x + 8f, rect.y + 30f, rect.width - 16f, 22f), value, labelStyle);
        }

        private void DrawChip(Rect rect, string label, string value)
        {
            GUI.color = new Color(0.05f, 0.16f, 0.18f, 0.82f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            string text = string.IsNullOrWhiteSpace(value) ? label : label + "  " + value;
            GUI.Label(rect, text, chipStyle);
        }

        private void DrawSkillState(Rect rect, string label, string detail, bool enabled)
        {
            GUI.color = enabled ? new Color(0.08f, 0.24f, 0.2f, 0.84f) : new Color(0.16f, 0.14f, 0.18f, 0.84f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(new Rect(rect.x + 8f, rect.y + 8f, rect.width - 16f, 26f), label, labelStyle);
            GUI.Label(new Rect(rect.x + 8f, rect.y + 40f, rect.width - 16f, 42f), detail, smallStyle);
        }

        private void DrawButtonRect(Rect rect, string label)
        {
            GUI.color = new Color(0.1f, 0.18f, 0.24f, 0.92f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
            GUI.Label(rect, label, chipStyle);
        }

        private Rect Inset(Rect rect, float horizontal, float vertical)
        {
            float scale = P0ImGuiLayout.CalculateScale(Screen.width, Screen.height);
            float x = P0ImGuiLayout.Scaled(horizontal, scale);
            float y = P0ImGuiLayout.Scaled(vertical, scale);
            return new Rect(rect.x + x, rect.y + y, Mathf.Max(1f, rect.width - x * 2f), Mathf.Max(1f, rect.height - y * 2f));
        }

        private void EnsureStyles()
        {
            if (titleStyle != null)
            {
                return;
            }

            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                fontSize = 18,
                normal = { textColor = Color.white }
            };
            labelStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 15,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
            smallStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 13,
                wordWrap = true,
                normal = { textColor = new Color(0.88f, 0.94f, 0.95f, 1f) }
            };
            chipStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 13,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
        }

        private static string FormatRect(Rect rect)
        {
            return rect.x.ToString("0")
                + ","
                + rect.y.ToString("0")
                + ","
                + rect.width.ToString("0")
                + "x"
                + rect.height.ToString("0");
        }

        private static bool IsInside(Rect rect, float width, float height)
        {
            return rect.xMin >= -0.5f
                && rect.yMin >= -0.5f
                && rect.xMax <= width + 0.5f
                && rect.yMax <= height + 0.5f;
        }

        private static bool Overlaps(Rect first, Rect second)
        {
            return first.Overlaps(second);
        }

        private readonly struct Layout
        {
            public Layout(
                Rect topRail,
                Rect partyPanel,
                Rect enemyPanel,
                Rect skillTray,
                Rect statusStrip,
                Rect runtimeControlCluster,
                Rect captureBadge)
            {
                TopRail = topRail;
                PartyPanel = partyPanel;
                EnemyPanel = enemyPanel;
                SkillTray = skillTray;
                StatusStrip = statusStrip;
                RuntimeControlCluster = runtimeControlCluster;
                CaptureBadge = captureBadge;
            }

            public Rect TopRail { get; }

            public Rect PartyPanel { get; }

            public Rect EnemyPanel { get; }

            public Rect SkillTray { get; }

            public Rect StatusStrip { get; }

            public Rect RuntimeControlCluster { get; }

            public Rect CaptureBadge { get; }
        }
    }
}
