using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Roguelite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Tools
{
    public enum P0PlayModeScreenshotSmokeState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public static class P0PlayModeScreenshotSmoke
    {
        private const string RunnerObjectName = "__TheCat_P0PlayModeScreenshotSmoke";
        public const int ExpectedCaptureCount = 11;
        public const string MainMenuCaptureFileName = "01-main-menu.png";
        public const string CatRoomCaptureFileName = "02-cat-room.png";
        public const string RouteMapCaptureFileName = "03-route-map-layer1.png";
        public const string BattleHudCaptureFileName = "04-battle-hud-layer1.png";
        public const string ActiveCatSaibanCaptureFileName = "05-active-cat-saiban.png";
        public const string ActiveCatNephthysCaptureFileName = "06-active-cat-nephthys.png";
        public const string ActiveCatSuzuneCaptureFileName = "07-active-cat-suzune.png";
        public const string BattleWorldVisualsCaptureFileName = "08-battle-world-visuals.png";
        public const string CallTyrantWarningVfxCaptureFileName = "09-call-tyrant-warning-vfx.png";
        public const string BattleResultCaptureFileName = "10-battle-result-layer1.png";
        public const string SettlementCaptureFileName = "11-settlement.png";
        public const string RuntimeVisualContactSheetPath = P0AssetProductionReadiness.RuntimeVisualContactSheetPath;
        public const int ExpectedRuntimeVisualBindingCount = P0VisualAssetCatalog.P0RuntimeVisualBindingCount;

        private static readonly List<string> capturedPaths = new List<string>();
        private static readonly string[] expectedCaptureFileNames =
        {
            MainMenuCaptureFileName,
            CatRoomCaptureFileName,
            RouteMapCaptureFileName,
            BattleHudCaptureFileName,
            ActiveCatSaibanCaptureFileName,
            ActiveCatNephthysCaptureFileName,
            ActiveCatSuzuneCaptureFileName,
            BattleWorldVisualsCaptureFileName,
            CallTyrantWarningVfxCaptureFileName,
            BattleResultCaptureFileName,
            SettlementCaptureFileName
        };

        private static readonly string[] criticalRuntimeVisualBindingIds =
        {
            "background.bedroom_dream",
            "cat.combat.saiban",
            "cat.combat.nephthys",
            "cat.combat.suzune",
            "cat.avatar.saiban",
            "cat.avatar.nephthys",
            "cat.avatar.suzune",
            "enemy.combat.black_mud",
            "enemy.combat.cold_light",
            "enemy.combat.call_tyrant",
            "enemy.anim.black_mud_move",
            "enemy.anim.cold_light_cast",
            "enemy.anim.call_tyrant_boss_pattern",
            "prop.bed",
            "prop.litter_box",
            "prop.feeder",
            "warning.call_tyrant",
            "warning.black_mud_bed_claw",
            "warning.cold_light_beam",
            "warning.call_tyrant_app_throw",
            "warning.call_tyrant_summon_portal",
            "feedback.hit_spark",
            "feedback.bed_shield_pulse",
            "feedback.sleep_stable_wave",
            "feedback.litter_cleanse",
            "feedback.feeder_kibble",
            "feedback.enemy_mark_ring"
        };

        private static P0PlayModeScreenshotSmokeRunner activeRunner;

        public static P0PlayModeScreenshotSmokeState State { get; private set; }

        public static string LastSummary { get; private set; } = "P0 play mode screenshot smoke has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static string LastOutputDirectory { get; private set; } = string.Empty;

        public static IReadOnlyList<string> CapturedPaths => capturedPaths.AsReadOnly();

        public static IReadOnlyList<string> ExpectedCaptureFileNames => Array.AsReadOnly(expectedCaptureFileNames);

        public static IReadOnlyList<string> ExpectedRuntimeVisualBindingIds => Array.AsReadOnly(BuildRuntimeVisualBindingIds());

        public static bool IsRunning => State == P0PlayModeScreenshotSmokeState.Running;

        public static bool IsFinished => State == P0PlayModeScreenshotSmokeState.Passed
            || State == P0PlayModeScreenshotSmokeState.Failed;

        public static bool HasP0ScreenshotCapturePlan()
        {
            return expectedCaptureFileNames.Length == ExpectedCaptureCount
                && expectedCaptureFileNames[0] == MainMenuCaptureFileName
                && expectedCaptureFileNames[1] == CatRoomCaptureFileName
                && expectedCaptureFileNames[2] == RouteMapCaptureFileName
                && expectedCaptureFileNames[3] == BattleHudCaptureFileName
                && expectedCaptureFileNames[4] == ActiveCatSaibanCaptureFileName
                && expectedCaptureFileNames[5] == ActiveCatNephthysCaptureFileName
                && expectedCaptureFileNames[6] == ActiveCatSuzuneCaptureFileName
                && expectedCaptureFileNames[7] == BattleWorldVisualsCaptureFileName
                && expectedCaptureFileNames[8] == CallTyrantWarningVfxCaptureFileName
                && expectedCaptureFileNames[9] == BattleResultCaptureFileName
                && expectedCaptureFileNames[10] == SettlementCaptureFileName;
        }

        public static bool HasRuntimeVisualScreenshotCapturePlan()
        {
            return HasP0ScreenshotCapturePlan()
                && expectedCaptureFileNames[4] == ActiveCatSaibanCaptureFileName
                && expectedCaptureFileNames[5] == ActiveCatNephthysCaptureFileName
                && expectedCaptureFileNames[6] == ActiveCatSuzuneCaptureFileName
                && expectedCaptureFileNames[7] == BattleWorldVisualsCaptureFileName
                && expectedCaptureFileNames[8] == CallTyrantWarningVfxCaptureFileName
                && RuntimeVisualContactSheetPath == P0AssetProductionReadiness.RuntimeVisualContactSheetPath
                && RuntimeBindingsMatchCatalog();
        }

        public static string DefaultOutputDirectory
        {
            get
            {
                return Path.GetFullPath(Path.Combine(
                    Application.dataPath,
                    "..",
                    "design",
                    "development",
                    "screenshots",
                    "p0-playmode-smoke"));
            }
        }

        public static bool StartDefaultScreenshotSmoke(string outputDirectory = null)
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0PlayModeScreenshotSmokeState.Failed,
                    "P0 play mode screenshot smoke requires Play Mode.",
                    "StartDefaultScreenshotSmoke was called outside Play Mode.",
                    null,
                    Array.Empty<string>());
                return false;
            }

            if (activeRunner != null)
            {
                UnityEngine.Object.Destroy(activeRunner.gameObject);
                activeRunner = null;
            }

            State = P0PlayModeScreenshotSmokeState.Running;
            LastSummary = "P0 play mode screenshot smoke running.";
            LastDetailedLog = LastSummary;
            LastOutputDirectory = string.IsNullOrWhiteSpace(outputDirectory)
                ? DefaultOutputDirectory
                : Path.GetFullPath(outputDirectory);
            capturedPaths.Clear();

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0PlayModeScreenshotSmokeRunner>();
            activeRunner.Begin(LastOutputDirectory);
            return true;
        }

        internal static void Complete(
            P0PlayModeScreenshotSmokeState state,
            string summary,
            string detailedLog,
            string outputDirectory,
            IReadOnlyList<string> paths)
        {
            State = state;
            LastSummary = string.IsNullOrWhiteSpace(summary) ? state.ToString() : summary;
            LastDetailedLog = detailedLog ?? string.Empty;
            LastOutputDirectory = outputDirectory ?? string.Empty;
            capturedPaths.Clear();
            if (paths != null)
            {
                capturedPaths.AddRange(paths);
            }

            activeRunner = null;
        }

        private static bool RuntimeBindingsMatchCatalog()
        {
            P0VisualAssetBinding[] bindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            if (bindings.Length != ExpectedRuntimeVisualBindingCount)
            {
                return false;
            }

            return ContainsAllBindingIds(bindings, criticalRuntimeVisualBindingIds);
        }

        private static string[] BuildRuntimeVisualBindingIds()
        {
            P0VisualAssetBinding[] bindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            string[] bindingIds = new string[bindings.Length];
            for (int i = 0; i < bindings.Length; i++)
            {
                bindingIds[i] = bindings[i].BindingId;
            }

            return bindingIds;
        }

        private static bool ContainsAllBindingIds(
            P0VisualAssetBinding[] bindings,
            IReadOnlyList<string> requiredBindingIds)
        {
            for (int i = 0; i < requiredBindingIds.Count; i++)
            {
                if (!ContainsBindingId(bindings, requiredBindingIds[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ContainsBindingId(P0VisualAssetBinding[] bindings, string bindingId)
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                if (bindings[i].BindingId == bindingId)
                {
                    return true;
                }
            }

            return false;
        }
    }

    internal sealed class P0PlayModeScreenshotSmokeRunner : MonoBehaviour
    {
        private const float SceneLoadTimeoutSeconds = 8f;
        private const float FullRouteTimeoutSeconds = 45f;
        private const float ScreenshotTimeoutSeconds = 5f;
        private const int MaxBattleResultTicks = 1600;
        private const float BattleResultTickDeltaSeconds = 0.25f;
        private const int BattleResultTicksPerFrame = 16;
        private const int StarterCatCount = 3;
        private const int P0SkillSlotCount = 3;

        private readonly List<string> lines = new List<string>();
        private readonly List<string> screenshots = new List<string>();

        private string outputDirectory;
        private bool failed;

        public void Begin(string outputDirectoryPath)
        {
            outputDirectory = outputDirectoryPath;
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            lines.Clear();
            screenshots.Clear();
            failed = false;

            if (string.IsNullOrWhiteSpace(outputDirectory))
            {
                Fail("Screenshot output directory is missing.");
                yield break;
            }

            Directory.CreateDirectory(outputDirectory);
            ClearExistingPngEvidence();
            Add("Screenshot output: " + outputDirectory);

            P0RunSession.Clear();
            yield return LoadScene(P0SceneFlow.MainMenuSceneName);
            if (failed)
            {
                yield break;
            }

            yield return Capture(P0PlayModeScreenshotSmoke.MainMenuCaptureFileName, "main menu");
            if (failed)
            {
                yield break;
            }

            MainMenuController mainMenu = UnityEngine.Object.FindAnyObjectByType<MainMenuController>();
            if (mainMenu == null)
            {
                Fail("P0MainMenu is missing MainMenuController.");
                yield break;
            }

            P0MainMenuSurface mainMenuSurface = mainMenu.BuildMainMenuSurfaceForSmoke();
            if (!P0MainMenuPresenter.HasP0MainMenuSurface(mainMenuSurface))
            {
                Fail("P0MainMenu is missing required start surface: " + P0MainMenuPresenter.BuildCompactSummary(mainMenuSurface));
                yield break;
            }

            Add("Main menu start surface verified: " + P0MainMenuPresenter.BuildCompactSummary(mainMenuSurface));

            P0LoadingStartSurface loadingStartSurface = mainMenu.BuildLoadingStartSurfaceForSmoke(P0RunStartMode.CatRoom);
            if (!P0LoadingStartPresenter.HasP0LoadingStartSurface(loadingStartSurface))
            {
                Fail("P0 loading/start surface is missing required screenshot hook: "
                    + P0LoadingStartPresenter.BuildCompactSummary(loadingStartSurface));
                yield break;
            }

            Add("Loading/start surface hook verified: "
                + P0LoadingStartPresenter.BuildCompactSummary(loadingStartSurface));

            P0RunSession.StartNewRun(new[]
            {
                P0PrototypeCatalog.SaibanId,
                P0PrototypeCatalog.NephthysId,
                P0PrototypeCatalog.SuzuneId
            });
            P0CatRoomSession.RecordFreshStart(P0RunSession.CurrentRun);
            yield return LoadScene(P0SceneFlow.CatRoomSceneName);
            if (failed)
            {
                yield break;
            }

            CatRoomController catRoom = UnityEngine.Object.FindAnyObjectByType<CatRoomController>();
            if (catRoom == null)
            {
                Fail("P0CatRoom is missing CatRoomController.");
                yield break;
            }

            P0CatRoomSurface catRoomSurface = catRoom.BuildCatRoomSurfaceForSmoke();
            if (!P0CatRoomPresenter.HasP0CatRoomSurface(catRoomSurface))
            {
                Fail("P0CatRoom is missing required cat-room surface: " + P0CatRoomPresenter.BuildCompactSummary(catRoomSurface));
                yield break;
            }

            Add("Cat room surface verified: " + P0CatRoomPresenter.BuildCompactSummary(catRoomSurface));
            yield return Capture(P0PlayModeScreenshotSmoke.CatRoomCaptureFileName, "cat room");
            if (failed)
            {
                yield break;
            }

            catRoom.EnterDream();
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            yield return Capture(P0PlayModeScreenshotSmoke.RouteMapCaptureFileName, "route map layer 1");
            if (failed)
            {
                yield break;
            }

            RouteMapController routeMap = UnityEngine.Object.FindAnyObjectByType<RouteMapController>();
            if (routeMap == null)
            {
                Fail("P0RouteMap is missing RouteMapController.");
                yield break;
            }

            P0RouteMapSurface routeMapSurface = routeMap.BuildRouteMapSurfaceForSmoke();
            if (!P0RouteMapPresenter.HasP0RouteMapSurface(routeMapSurface))
            {
                Fail("P0RouteMap is missing required route map surface: " + P0RouteMapPresenter.BuildCompactSummary(routeMapSurface));
                yield break;
            }

            Add("Route map surface verified: " + P0RouteMapPresenter.BuildCompactSummary(routeMapSurface));

            routeMap.ExecuteInputCommand(P0InputCommand.ContinueRoute);
            yield return WaitForScene(P0SceneFlow.GrayboxBattleSceneName);
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

            if (!battleController.PrimeEnemyHudForSmoke())
            {
                Fail("P0 battle HUD could not prime required enemy threat cards.");
                yield break;
            }

            IReadOnlyList<P0EnemyHudCard> enemyHudCards = battleController.BuildEnemyHudCardsForSmoke();
            if (!P0EnemyHudPresenter.HasP0EnemyHudCards(enemyHudCards))
            {
                Fail("P0 battle HUD is missing required enemy threat cards: " + P0EnemyHudPresenter.BuildCompactSummary(enemyHudCards));
                yield break;
            }

            Add("Battle HUD enemy cards verified: " + P0EnemyHudPresenter.BuildCompactSummary(enemyHudCards));

            if (!battleController.PrimeStatusHudForSmoke())
            {
                Fail("P0 battle HUD could not prime required status tag indicators.");
                yield break;
            }

            IReadOnlyList<P0StatusHudEntry> statusHudEntries = battleController.BuildStatusHudEntriesForSmoke();
            if (!P0StatusHudPresenter.HasP0StatusHudEntries(statusHudEntries))
            {
                Fail("P0 battle HUD is missing required status indicators: " + P0StatusHudPresenter.BuildCompactSummary(statusHudEntries));
                yield break;
            }

            Add("Battle HUD status indicators verified: " + P0StatusHudPresenter.BuildCompactSummary(statusHudEntries));

            P0RuntimeSettingsPresentation runtimeSettings = battleController.BuildRuntimeSettingsPresentationForSmoke();
            if (!P0RuntimeSettingsPresenter.HasP0RuntimeSettingsSurface(runtimeSettings))
            {
                Fail("P0 battle HUD is missing required runtime settings controls: " + P0RuntimeSettingsPresenter.BuildCompactSummary(runtimeSettings));
                yield break;
            }

            Add("Battle HUD runtime settings verified: " + P0RuntimeSettingsPresenter.BuildCompactSummary(runtimeSettings));

            IReadOnlyList<P0BattleHudSection> battleHudSections = battleController.BuildBattleHudSectionsForSmoke();
            if (!P0BattleHudSummaryPresenter.HasP0BattleHudSections(battleHudSections))
            {
                Fail("P0 battle HUD is missing required demo sections: " + P0BattleHudSummaryPresenter.BuildCompactSummary(battleHudSections));
                yield break;
            }

            Add("Battle HUD sections verified: " + P0BattleHudSummaryPresenter.BuildCompactSummary(battleHudSections));

            IReadOnlyList<P0BattleActionAffordance> battleActions = battleController.BuildBattleActionAffordancesForSmoke();
            if (!P0BattleActionAffordancePresenter.HasP0BattleActionAffordances(battleActions))
            {
                Fail("P0 battle HUD is missing required action affordances: " + P0BattleActionAffordancePresenter.BuildCompactSummary(battleActions));
                yield break;
            }

            Add("Battle HUD actions verified: " + P0BattleActionAffordancePresenter.BuildCompactSummary(battleActions));

            IReadOnlyList<P0CatHudCard> catHudCards = battleController.BuildCatHudCardsForSmoke();
            if (!P0CatHudPresenter.HasP0CatHudCards(catHudCards))
            {
                Fail("P0 battle HUD is missing required cat cards: " + P0CatHudPresenter.BuildCompactSummary(catHudCards));
                yield break;
            }

            Add("Battle HUD cat cards verified: " + P0CatHudPresenter.BuildCompactSummary(catHudCards));

            IReadOnlyList<P0SkillHudCard> skillHudCards = battleController.BuildSkillHudCardsForSmoke();
            if (!P0SkillHudPresenter.HasP0SkillHudCards(skillHudCards))
            {
                Fail("P0 battle HUD is missing required skill cards: " + P0SkillHudPresenter.BuildCompactSummary(skillHudCards));
                yield break;
            }

            Add("Battle HUD skill cards verified: " + P0SkillHudPresenter.BuildCompactSummary(skillHudCards));

            battleController.CollapseDiagnosticsHudForSmoke();
            yield return Capture(P0PlayModeScreenshotSmoke.BattleHudCaptureFileName, "battle HUD layer 1");
            if (failed)
            {
                yield break;
            }

            yield return CaptureRuntimeVisualAcceptanceScreenshots(battleController);
            if (failed)
            {
                yield break;
            }

            yield return ResolveBattleResultScreenshot(battleController);
            if (failed)
            {
                yield break;
            }

            if (!P0PlayModeRouteFlowSmoke.StartDefaultRouteSmoke())
            {
                Fail("Could not start full route smoke for settlement screenshot: " + P0PlayModeRouteFlowSmoke.LastSummary);
                yield break;
            }

            yield return WaitForRouteFlowSmoke();
            if (failed)
            {
                yield break;
            }

            if (P0PlayModeRouteFlowSmoke.State != P0PlayModeRouteFlowSmokeState.Passed)
            {
                Fail("Full route smoke did not pass before settlement screenshot: " + P0PlayModeRouteFlowSmoke.LastSummary);
                yield break;
            }

            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
            if (failed)
            {
                yield break;
            }

            yield return Capture(P0PlayModeScreenshotSmoke.SettlementCaptureFileName, "settlement");
            if (failed)
            {
                yield break;
            }

            string summary = "P0 play mode screenshot smoke passed with "
                + screenshots.Count
                + " screenshot(s) in "
                + outputDirectory
                + ".";
            Add(summary);
            Complete(P0PlayModeScreenshotSmokeState.Passed, summary);
        }

        private IEnumerator LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
            yield return WaitForScene(sceneName);
        }

        private IEnumerator WaitForScene(string sceneName)
        {
            float start = Time.realtimeSinceStartup;
            while (SceneManager.GetActiveScene().name != sceneName)
            {
                if (Time.realtimeSinceStartup - start > SceneLoadTimeoutSeconds)
                {
                    Fail("Timed out waiting for scene " + sceneName + "; active scene is " + SceneManager.GetActiveScene().name + ".");
                    yield break;
                }

                yield return null;
            }

            yield return null;
            yield return null;
        }

        private IEnumerator WaitForRouteFlowSmoke()
        {
            float start = Time.realtimeSinceStartup;
            while (!P0PlayModeRouteFlowSmoke.IsFinished)
            {
                if (Time.realtimeSinceStartup - start > FullRouteTimeoutSeconds)
                {
                    Fail("Timed out waiting for full route smoke before settlement screenshot.");
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator Capture(string fileName, string label)
        {
            string path = Path.Combine(outputDirectory, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            if (Application.isBatchMode)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }

            if (!TryCaptureFrameToPng(path, out string captureError))
            {
                Add("ReadPixels capture failed for " + label + ": " + captureError + ". Falling back to ScreenCapture.");
                ScreenCapture.CaptureScreenshot(path);
            }

            float start = Time.realtimeSinceStartup;
            while (!File.Exists(path) || new FileInfo(path).Length <= 0)
            {
                if (Time.realtimeSinceStartup - start > ScreenshotTimeoutSeconds)
                {
                    Fail("Timed out capturing " + label + " screenshot at " + path + ".");
                    yield break;
                }

                yield return null;
            }

            screenshots.Add(path);
            Add("Captured " + label + ": " + path);
        }

        private void ClearExistingPngEvidence()
        {
            string[] files = Directory.GetFiles(outputDirectory, "*.png", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }

            if (files.Length > 0)
            {
                Add("Cleared " + files.Length + " existing screenshot PNG(s).");
            }
        }

        private bool TryCaptureFrameToPng(string path, out string error)
        {
            try
            {
                int width = Mathf.Max(1, Screen.width);
                int height = Mathf.Max(1, Screen.height);
                Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
                texture.Apply(false, false);
                byte[] bytes = texture.EncodeToPNG();
                Destroy(texture);

                if (bytes == null || bytes.Length == 0)
                {
                    error = "encoded PNG bytes are empty";
                    return false;
                }

                File.WriteAllBytes(path, bytes);
                error = string.Empty;
                return File.Exists(path) && new FileInfo(path).Length > 0;
            }
            catch (Exception exception)
            {
                error = exception.GetType().Name + ": " + exception.Message;
                return false;
            }
        }

        private void Add(string line)
        {
            lines.Add(line);
        }

        private void Fail(string message)
        {
            failed = true;
            Add("FAILED: " + message);
            Complete(P0PlayModeScreenshotSmokeState.Failed, message);
        }

        private void Complete(P0PlayModeScreenshotSmokeState state, string summary)
        {
            P0PlayModeScreenshotSmoke.Complete(
                state,
                summary,
                string.Join("\n", lines),
                outputDirectory,
                screenshots);
            Destroy(gameObject);
        }

        private IEnumerator CaptureRuntimeVisualAcceptanceScreenshots(GrayboxBattleController battleController)
        {
            P0RuntimeVisualBindingCoverageReport runtimeVisuals = P0RuntimeVisualBindingCoverage.EvaluateP0Bindings();
            if (!runtimeVisuals.IsComplete)
            {
                Fail("P0 runtime visual bindings are not ready before Play Mode screenshot acceptance: "
                    + runtimeVisuals.BuildDetailedSummary());
                yield break;
            }

            Add("Runtime visual bindings verified before Play Mode screenshots: " + runtimeVisuals.BuildSummary());

            yield return CaptureActiveCatVisual(battleController, 0, P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName, "active cat Saiban visual");
            if (failed)
            {
                yield break;
            }

            yield return CaptureActiveCatVisual(battleController, 1, P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName, "active cat Nephthys visual");
            if (failed)
            {
                yield break;
            }

            yield return CaptureActiveCatVisual(battleController, 2, P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName, "active cat Suzune visual");
            if (failed)
            {
                yield break;
            }

            if (!battleController.PrimeEnemyHudForSmoke())
            {
                Fail("P0 battle world visuals could not prime required enemy assets.");
                yield break;
            }

            battleController.AdvanceGraybox(0.1f);
            yield return null;
            yield return Capture(P0PlayModeScreenshotSmoke.BattleWorldVisualsCaptureFileName, "battle world visual bindings");
            if (failed)
            {
                yield break;
            }

            if (!battleController.PrimeEnemyHudForSmoke())
            {
                Fail("P0 Call Tyrant warning VFX could not prime required warning state.");
                yield break;
            }

            battleController.AdvanceGraybox(0f);
            yield return null;
            yield return Capture(P0PlayModeScreenshotSmoke.CallTyrantWarningVfxCaptureFileName, "Call Tyrant warning VFX");
        }

        private IEnumerator CaptureActiveCatVisual(
            GrayboxBattleController battleController,
            int catIndex,
            string fileName,
            string label)
        {
            battleController.SelectCat(catIndex);
            battleController.AdvanceGraybox(0f);
            yield return null;
            yield return Capture(fileName, label);
        }

        private IEnumerator ResolveBattleResultScreenshot(GrayboxBattleController battleController)
        {
            if (battleController == null || battleController.Battle == null)
            {
                Fail("Cannot capture battle result because battle controller state is missing.");
                yield break;
            }

            int ticks = 0;
            while (battleController.Battle.Outcome == BattleOutcome.InProgress && ticks < MaxBattleResultTicks)
            {
                for (int frameTick = 0;
                     frameTick < BattleResultTicksPerFrame
                     && battleController.Battle.Outcome == BattleOutcome.InProgress
                     && ticks < MaxBattleResultTicks;
                     frameTick++)
                {
                    DriveBattlePlayer(battleController, ticks);
                    battleController.AdvanceGraybox(BattleResultTickDeltaSeconds);
                    ticks++;
                }

                yield return null;
            }

            if (battleController.Battle.Outcome != BattleOutcome.Victory)
            {
                Fail("First battle result screenshot expected victory but got "
                    + battleController.Battle.Outcome
                    + " after "
                    + ticks
                    + " tick(s).");
                yield break;
            }

            P0BattleResultSurface resultSurface = battleController.BuildBattleResultSurfaceForSmoke();
            if (!P0BattleResultPresenter.HasP0BattleResultSurface(resultSurface))
            {
                Fail("First battle result surface is incomplete before screenshot: "
                    + P0BattleResultPresenter.BuildCompactSummary(resultSurface)
                    + ".");
                yield break;
            }

            Add("Battle result screenshot surface verified: "
                + P0BattleResultPresenter.BuildCompactSummary(resultSurface)
                + ".");
            yield return Capture(P0PlayModeScreenshotSmoke.BattleResultCaptureFileName, "battle result layer 1");
            if (failed)
            {
                yield break;
            }

            battleController.ContinueRoute();
            yield return WaitForScene(P0SceneFlow.RouteMapSceneName);
        }

        private void DriveBattlePlayer(GrayboxBattleController battleController, int tick)
        {
            int catIndex = (tick / P0SkillSlotCount) % StarterCatCount;
            int skillSlot = tick % P0SkillSlotCount;
            battleController.SelectCat(catIndex);
            battleController.CastSkillBySlot(skillSlot);

            if (tick % 24 == 0)
            {
                battleController.UseBedCare();
            }

            if (tick % 40 == 0)
            {
                battleController.UseLitterBox();
            }

            if (tick % 48 == 0)
            {
                battleController.UseFeeder();
            }
        }
    }
}
