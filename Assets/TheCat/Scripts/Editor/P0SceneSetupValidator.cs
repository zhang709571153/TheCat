using System;
using System.Collections.Generic;
using System.Linq;
using TheCat.Gameplay;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.EditorTools
{
    public enum P0SceneValidationSeverity
    {
        Info,
        Warning,
        Error
    }

    public readonly struct P0SceneValidationIssue
    {
        public P0SceneValidationIssue(P0SceneValidationSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0SceneValidationSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0SceneValidationReport
    {
        private readonly List<P0SceneValidationIssue> issues = new List<P0SceneValidationIssue>();

        public IReadOnlyList<P0SceneValidationIssue> Issues => issues.AsReadOnly();

        public int ErrorCount => Count(P0SceneValidationSeverity.Error);

        public int WarningCount => Count(P0SceneValidationSeverity.Warning);

        public bool IsValid => ErrorCount == 0;

        public void Add(P0SceneValidationSeverity severity, string message)
        {
            issues.Add(new P0SceneValidationIssue(severity, message));
        }

        public string BuildSummary()
        {
            return IsValid
                ? "P0 scene setup valid with " + WarningCount + " warning(s)."
                : "P0 scene setup has " + ErrorCount + " error(s) and " + WarningCount + " warning(s).";
        }

        public string BuildDetailedLog()
        {
            if (issues.Count == 0)
            {
                return BuildSummary();
            }

            List<string> lines = new List<string>
            {
                BuildSummary()
            };
            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0SceneValidationSeverity severity)
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
    }

    public static class P0SceneSetupValidator
    {
        private const string SceneFolder = "Assets/TheCat/Scenes/";
        private const string MenuPath = "TheCat/P0/Validate P0 Scene Setup";

        private static readonly P0SceneExpectation[] ExpectedScenes =
        {
            new P0SceneExpectation(
                MainMenuController.MainMenuSceneName,
                SceneFolder + MainMenuController.MainMenuSceneName + ".unity",
                "P0MainMenuRoot",
                typeof(MainMenuController)),
            new P0SceneExpectation(
                CatRoomController.CatRoomSceneName,
                SceneFolder + CatRoomController.CatRoomSceneName + ".unity",
                "P0CatRoomRoot",
                typeof(CatRoomController)),
            new P0SceneExpectation(
                RouteMapController.RouteMapSceneName,
                SceneFolder + RouteMapController.RouteMapSceneName + ".unity",
                "P0RouteMapRoot",
                typeof(RouteMapController)),
            new P0SceneExpectation(
                MainMenuController.GrayboxBattleSceneName,
                SceneFolder + MainMenuController.GrayboxBattleSceneName + ".unity",
                "P0GrayboxBattleRoot",
                typeof(GrayboxBattleController))
        };

        [MenuItem(MenuPath, false, 90)]
        private static void ValidateP0SceneSetup()
        {
            P0SceneValidationReport report = Validate(deepSceneInspection: true);
            string log = report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log("[TheCat] " + log);
            }
            else
            {
                Debug.LogError("[TheCat] " + log);
            }

            EditorUtility.DisplayDialog(
                report.IsValid ? "P0 Scene Setup Valid" : "P0 Scene Setup Needs Attention",
                report.BuildSummary(),
                "OK");
        }

        public static void ValidateP0SceneSetupForBatchmode()
        {
            P0SceneValidationReport report = Validate(deepSceneInspection: true);
            string log = report.BuildDetailedLog();
            if (!report.IsValid)
            {
                throw new InvalidOperationException(log);
            }

            Debug.Log("[TheCat] " + log);
        }

        public static P0SceneValidationReport Validate(bool deepSceneInspection)
        {
            P0SceneValidationReport report = new P0SceneValidationReport();
            ValidateSceneAssets(report);
            ValidateBuildSettings(report);
            if (deepSceneInspection)
            {
                ValidateSceneContents(report);
            }

            return report;
        }

        private static void ValidateSceneAssets(P0SceneValidationReport report)
        {
            for (int i = 0; i < ExpectedScenes.Length; i++)
            {
                P0SceneExpectation expectation = ExpectedScenes[i];
                SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(expectation.Path);
                if (sceneAsset == null)
                {
                    report.Add(P0SceneValidationSeverity.Error, "Missing scene asset: " + expectation.Path);
                    continue;
                }

                report.Add(P0SceneValidationSeverity.Info, "Found scene asset: " + expectation.Path);
            }
        }

        private static void ValidateBuildSettings(P0SceneValidationReport report)
        {
            List<string> enabledPaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToList();

            for (int i = 0; i < ExpectedScenes.Length; i++)
            {
                P0SceneExpectation expectation = ExpectedScenes[i];
                if (!enabledPaths.Contains(expectation.Path))
                {
                    report.Add(P0SceneValidationSeverity.Error, "Build Settings missing enabled scene: " + expectation.Path);
                }
            }

            for (int i = 0; i < ExpectedScenes.Length; i++)
            {
                if (enabledPaths.Count <= i)
                {
                    report.Add(P0SceneValidationSeverity.Error, "Build Settings has fewer enabled scenes than the P0 flow requires.");
                    return;
                }

                if (enabledPaths[i] != ExpectedScenes[i].Path)
                {
                    report.Add(
                        P0SceneValidationSeverity.Error,
                        "Build Settings scene " + i + " should be " + ExpectedScenes[i].Path + " but is " + enabledPaths[i] + ".");
                }
            }

            report.Add(P0SceneValidationSeverity.Info, "Build Settings P0 scene order is main menu, cat room, route map, graybox battle.");
        }

        private static void ValidateSceneContents(P0SceneValidationReport report)
        {
            if (HasDirtyOpenScenes())
            {
                report.Add(P0SceneValidationSeverity.Warning, "Deep scene validation skipped because at least one open scene has unsaved changes.");
                return;
            }

            SceneSetup[] previousSetup = EditorSceneManager.GetSceneManagerSetup();
            try
            {
                for (int i = 0; i < ExpectedScenes.Length; i++)
                {
                    ValidateSingleSceneContents(ExpectedScenes[i], report);
                }
            }
            finally
            {
                if (previousSetup.Length > 0)
                {
                    EditorSceneManager.RestoreSceneManagerSetup(previousSetup);
                }
            }
        }

        private static void ValidateSingleSceneContents(P0SceneExpectation expectation, P0SceneValidationReport report)
        {
            Scene scene = EditorSceneManager.OpenScene(expectation.Path, OpenSceneMode.Single);
            if (!scene.IsValid() || !scene.isLoaded)
            {
                report.Add(P0SceneValidationSeverity.Error, "Could not load scene for validation: " + expectation.Path);
                return;
            }

            GameObject root = FindRoot(scene, expectation.RootName);
            if (root == null)
            {
                report.Add(P0SceneValidationSeverity.Error, expectation.SceneName + " is missing root object " + expectation.RootName + ".");
                return;
            }

            if (root.GetComponentInChildren(expectation.ControllerType, includeInactive: true) == null)
            {
                report.Add(
                    P0SceneValidationSeverity.Error,
                    expectation.SceneName + " root is missing controller " + expectation.ControllerType.Name + ".");
                return;
            }

            report.Add(
                P0SceneValidationSeverity.Info,
                expectation.SceneName + " has root " + expectation.RootName + " and controller " + expectation.ControllerType.Name + ".");
        }

        private static GameObject FindRoot(Scene scene, string rootName)
        {
            GameObject[] roots = scene.GetRootGameObjects();
            for (int i = 0; i < roots.Length; i++)
            {
                if (roots[i].name == rootName)
                {
                    return roots[i];
                }
            }

            return null;
        }

        private static bool HasDirtyOpenScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).isDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private readonly struct P0SceneExpectation
        {
            public P0SceneExpectation(string sceneName, string path, string rootName, Type controllerType)
            {
                SceneName = sceneName;
                Path = path;
                RootName = rootName;
                ControllerType = controllerType;
            }

            public string SceneName { get; }

            public string Path { get; }

            public string RootName { get; }

            public Type ControllerType { get; }
        }
    }
}
