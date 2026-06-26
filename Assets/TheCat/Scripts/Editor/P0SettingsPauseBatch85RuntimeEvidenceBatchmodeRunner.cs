using System;
using System.IO;
using System.Reflection;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public static class P0SettingsPauseBatch85RuntimeEvidenceBatchmodeRunner
    {
        private const double EnterPlayModeTimeoutSeconds = 45d;
        private const double RuntimeEvidenceTimeoutSeconds = 160d;
        private const string CommandLineEntryPoint = "TheCat.EditorTools.P0SettingsPauseBatch85RuntimeEvidenceBatchmodeRunner.RunForBatchmode";

        private static double stageStartedAt;
        private static bool evidenceStarted;
        private static bool exitRequested;
        private static bool hasStarted;

        [InitializeOnLoadMethod]
        private static void StartFromCommandLineAfterReload()
        {
            if (!IsCommandLineEntryPointRequested())
            {
                return;
            }

            EditorApplication.delayCall -= RunForBatchmode;
            EditorApplication.delayCall += RunForBatchmode;
        }

        public static void RunForBatchmode()
        {
            try
            {
                if (hasStarted)
                {
                    return;
                }

                hasStarted = true;
                Debug.Log("[TheCat] Batch 85 settings/pause runtime evidence runner invoked.");
                ResetState();
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
                EditorApplication.update += Update;

                if (EditorApplication.isPlaying)
                {
                    StartEvidence();
                    return;
                }

                stageStartedAt = EditorApplication.timeSinceStartup;
                Debug.Log("[TheCat] Entering Play Mode for Batch 85 settings/pause runtime evidence.");
                EditorApplication.EnterPlaymode();
            }
            catch (Exception exception)
            {
                ExitWithException(exception);
            }
        }

        private static void ResetState()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.update -= Update;
            P0SettingsPauseBatch85RuntimeEvidence.ClearBeforeCaptureResolutionOverride();
            stageStartedAt = EditorApplication.timeSinceStartup;
            evidenceStarted = false;
            exitRequested = false;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            Debug.Log("[TheCat] Batch 85 runtime evidence Play Mode state changed: " + change + ".");
            if (change == PlayModeStateChange.EnteredPlayMode)
            {
                StartEvidence();
            }
        }

        private static void Update()
        {
            if (exitRequested)
            {
                return;
            }

            try
            {
                double elapsed = EditorApplication.timeSinceStartup - stageStartedAt;
                if (!evidenceStarted)
                {
                    if (elapsed > EnterPlayModeTimeoutSeconds)
                    {
                        ExitWithFailure("Timed out entering Play Mode before starting Batch 85 runtime evidence.");
                    }

                    return;
                }

                if (P0SettingsPauseBatch85RuntimeEvidence.IsFinished)
                {
                    bool passed = P0SettingsPauseBatch85RuntimeEvidence.State == P0SettingsPauseBatch85RuntimeEvidenceState.Passed
                        && P0SettingsPauseBatch85RuntimeEvidence.CapturedPaths.Count == P0SettingsPauseBatch85RuntimeEvidence.ExpectedScreenshotCount
                        && P0SettingsPauseBatch85RuntimeEvidence.GeneratedReviewPaths.Count == P0SettingsPauseBatch85RuntimeEvidence.ExpectedAutomaticReviewCount;
                    CleanupAndExit(passed, P0SettingsPauseBatch85RuntimeEvidence.LastSummary);
                    return;
                }

                if (elapsed > RuntimeEvidenceTimeoutSeconds)
                {
                    ExitWithFailure("Timed out waiting for Batch 85 settings/pause runtime evidence to finish.");
                }
            }
            catch (Exception exception)
            {
                ExitWithException(exception);
            }
        }

        private static void StartEvidence()
        {
            if (exitRequested || evidenceStarted)
            {
                return;
            }

            stageStartedAt = EditorApplication.timeSinceStartup;
            evidenceStarted = true;
            Debug.Log("[TheCat] Starting Batch 85 settings/pause runtime evidence.");
            P0SettingsPauseBatch85RuntimeEvidence.SetBeforeCaptureResolutionOverride(P0SettingsPauseBatch85GameViewSizeUtility.ApplyTargetSize);
            bool started = P0SettingsPauseBatch85RuntimeEvidence.StartDefaultRuntimeEvidence();
            if (!started)
            {
                ExitWithFailure("Could not start Batch 85 runtime evidence: " + P0SettingsPauseBatch85RuntimeEvidence.LastSummary);
            }
        }

        private static void ExitWithFailure(string message)
        {
            WriteFailureReport(message);
            CleanupAndExit(false, message);
        }

        private static void ExitWithException(Exception exception)
        {
            WriteFailureReport(exception.GetType().Name + ": " + exception.Message + Environment.NewLine + exception);
            Debug.LogException(exception);
            CleanupAndExit(false, exception.Message);
        }

        private static void CleanupAndExit(bool passed, string message)
        {
            if (exitRequested)
            {
                return;
            }

            exitRequested = true;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.update -= Update;
            P0SettingsPauseBatch85RuntimeEvidence.ClearBeforeCaptureResolutionOverride();

            string logMessage = "[TheCat] Batch 85 settings/pause runtime evidence " + (passed ? "passed: " : "failed: ") + message;
            if (passed)
            {
                Debug.Log(logMessage);
            }
            else
            {
                Debug.LogError(logMessage);
            }

            EditorApplication.Exit(passed ? 0 : 1);
        }

        private static void WriteFailureReport(string message)
        {
            string path = ToProjectPath(P0SettingsPauseBatch85RuntimeEvidence.RuntimeReportPath);
            string directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(
                path,
                P0SettingsPauseBatch85RuntimeEvidence.BuildRuntimeReportMarkdown(
                    P0SettingsPauseBatch85RuntimeEvidenceState.Failed,
                    message,
                    message,
                    P0SettingsPauseBatch85RuntimeEvidence.CapturedPaths,
                    P0SettingsPauseBatch85RuntimeEvidence.GeneratedReviewPaths));
        }

        private static string ToProjectPath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }

            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            return Path.Combine(projectRoot, path.Replace('/', Path.DirectorySeparatorChar));
        }

        private static bool IsCommandLineEntryPointRequested()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], CommandLineEntryPoint, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }

    internal static class P0SettingsPauseBatch85GameViewSizeUtility
    {
        private const string CustomSizePrefix = "Batch85 ";

        public static string ApplyTargetSize(P0SettingsPauseBatch85ScreenshotTarget target)
        {
            try
            {
                int index = EnsureCustomGameViewSize(target.Width, target.Height, CustomSizePrefix + target.ResolutionLabel);
                string selectedSummary = SelectGameViewSize(index);
                Screen.SetResolution(target.Width, target.Height, FullScreenMode.Windowed);
                return "GameView custom size index "
                    + index
                    + " selected for "
                    + target.ResolutionLabel
                    + "; "
                    + selectedSummary;
            }
            catch (Exception exception)
            {
                return "GameView size override unavailable for "
                    + target.ResolutionLabel
                    + ": "
                    + exception.GetType().Name
                    + ": "
                    + exception.Message;
            }
        }

        private static int EnsureCustomGameViewSize(int width, int height, string label)
        {
            object group = GetGameViewSizeGroup();
            MethodInfo getTotalCount = RequireMethod(group.GetType(), "GetTotalCount");
            MethodInfo getGameViewSize = RequireMethod(group.GetType(), "GetGameViewSize");
            int totalCount = Convert.ToInt32(getTotalCount.Invoke(group, null));

            for (int i = 0; i < totalCount; i++)
            {
                object size = getGameViewSize.Invoke(group, new object[] { i });
                if (TryReadDimension(size, "width", out int existingWidth)
                    && TryReadDimension(size, "height", out int existingHeight)
                    && existingWidth == width
                    && existingHeight == height)
                {
                    return i;
                }
            }

            Type sizeType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSize");
            Type sizeKindType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizeType");
            if (sizeType == null || sizeKindType == null)
            {
                throw new InvalidOperationException("UnityEditor.GameViewSize reflection type is unavailable.");
            }

            object fixedResolution = Enum.Parse(sizeKindType, "FixedResolution");
            ConstructorInfo constructor = sizeType.GetConstructor(new[] { sizeKindType, typeof(int), typeof(int), typeof(string) });
            if (constructor == null)
            {
                throw new InvalidOperationException("UnityEditor.GameViewSize constructor is unavailable.");
            }

            object customSize = constructor.Invoke(new[] { fixedResolution, width, height, label });
            RequireMethod(group.GetType(), "AddCustomSize").Invoke(group, new[] { customSize });
            return totalCount;
        }

        private static object GetGameViewSizeGroup()
        {
            Assembly editorAssembly = typeof(Editor).Assembly;
            Type gameViewSizesType = editorAssembly.GetType("UnityEditor.GameViewSizes");
            Type groupType = editorAssembly.GetType("UnityEditor.GameViewSizeGroupType");
            Type singletonOpenType = editorAssembly.GetType("UnityEditor.ScriptableSingleton`1");
            if (gameViewSizesType == null || groupType == null || singletonOpenType == null)
            {
                throw new InvalidOperationException("Unity GameView size reflection types are unavailable.");
            }

            Type singletonType = singletonOpenType.MakeGenericType(gameViewSizesType);
            PropertyInfo instanceProperty = singletonType.GetProperty("instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            object instance = instanceProperty == null ? null : instanceProperty.GetValue(null, null);
            if (instance == null)
            {
                throw new InvalidOperationException("Unity GameViewSizes singleton is unavailable.");
            }

            object standaloneGroup = Enum.Parse(groupType, "Standalone");
            return RequireMethod(gameViewSizesType, "GetGroup").Invoke(instance, new[] { standaloneGroup });
        }

        private static string SelectGameViewSize(int index)
        {
            Type gameViewType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            if (gameViewType == null)
            {
                throw new InvalidOperationException("UnityEditor.GameView reflection type is unavailable.");
            }

            EditorWindow gameView = EditorWindow.GetWindow(gameViewType);
            if (gameView == null)
            {
                throw new InvalidOperationException("Unity GameView window is unavailable.");
            }

            PropertyInfo selectedSizeIndex = gameViewType.GetProperty(
                "selectedSizeIndex",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (selectedSizeIndex != null && selectedSizeIndex.CanWrite)
            {
                selectedSizeIndex.SetValue(gameView, index, null);
                gameView.Repaint();
                return "selectedSizeIndex property set";
            }

            MethodInfo callback = gameViewType.GetMethod(
                "SizeSelectionCallback",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (callback != null)
            {
                ParameterInfo[] parameters = callback.GetParameters();
                if (parameters.Length == 2)
                {
                    callback.Invoke(gameView, new object[] { index, null });
                    gameView.Repaint();
                    return "SizeSelectionCallback(index, null) invoked";
                }

                if (parameters.Length == 1)
                {
                    callback.Invoke(gameView, new object[] { index });
                    gameView.Repaint();
                    return "SizeSelectionCallback(index) invoked";
                }
            }

            throw new InvalidOperationException("Unity GameView size selection API is unavailable.");
        }

        private static bool TryReadDimension(object size, string name, out int value)
        {
            value = 0;
            if (size == null)
            {
                return false;
            }

            Type type = size.GetType();
            PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
            {
                value = Convert.ToInt32(property.GetValue(size, null));
                return true;
            }

            FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null)
            {
                value = Convert.ToInt32(field.GetValue(size));
                return true;
            }

            return false;
        }

        private static MethodInfo RequireMethod(Type type, string name)
        {
            MethodInfo method = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
            {
                throw new InvalidOperationException(type.FullName + "." + name + " is unavailable.");
            }

            return method;
        }
    }
}
