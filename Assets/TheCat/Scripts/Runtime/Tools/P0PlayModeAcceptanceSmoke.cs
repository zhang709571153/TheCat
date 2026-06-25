using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheCat.Tools
{
    public enum P0PlayModeAcceptanceSmokeState
    {
        Idle,
        Running,
        Passed,
        Failed
    }

    public static class P0PlayModeAcceptanceSmoke
    {
        private const string RunnerObjectName = "__TheCat_P0PlayModeAcceptanceSmoke";
        public const int ExpectedEvidenceCheckCount = 8;

        private static readonly string[] expectedEvidenceCheckIds =
        {
            P0PlayModeEvidenceChecklist.ScreenshotCapturePlanCheckId,
            P0PlayModeEvidenceChecklist.RuntimeVisualScreenshotPlanCheckId,
            P0PlayModeEvidenceChecklist.RuntimeVisualContactSheetCheckId,
            P0PlayModeEvidenceChecklist.ScreenshotFileEvidenceCheckId,
            P0PlayModeEvidenceChecklist.UnityRuntimeValidationPlanCheckId,
            P0PlayModeEvidenceChecklist.ScreenshotSmokeCheckId,
            P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId,
            P0PlayModeEvidenceChecklist.DefeatFlowSmokeCheckId
        };

        private static P0PlayModeAcceptanceSmokeRunner activeRunner;

        public static P0PlayModeAcceptanceSmokeState State { get; private set; }

        public static string LastSummary { get; private set; } = "P0 play mode acceptance smoke has not run.";

        public static string LastDetailedLog { get; private set; } = string.Empty;

        public static IReadOnlyList<string> ExpectedEvidenceCheckIds => Array.AsReadOnly(expectedEvidenceCheckIds);

        public static bool IsRunning => State == P0PlayModeAcceptanceSmokeState.Running;

        public static bool IsFinished => State == P0PlayModeAcceptanceSmokeState.Passed
            || State == P0PlayModeAcceptanceSmokeState.Failed;

        public static bool HasP0AcceptanceSequencePlan()
        {
            return expectedEvidenceCheckIds.Length == ExpectedEvidenceCheckCount
                && expectedEvidenceCheckIds[0] == P0PlayModeEvidenceChecklist.ScreenshotCapturePlanCheckId
                && expectedEvidenceCheckIds[1] == P0PlayModeEvidenceChecklist.RuntimeVisualScreenshotPlanCheckId
                && expectedEvidenceCheckIds[2] == P0PlayModeEvidenceChecklist.RuntimeVisualContactSheetCheckId
                && expectedEvidenceCheckIds[3] == P0PlayModeEvidenceChecklist.ScreenshotFileEvidenceCheckId
                && expectedEvidenceCheckIds[4] == P0PlayModeEvidenceChecklist.UnityRuntimeValidationPlanCheckId
                && expectedEvidenceCheckIds[5] == P0PlayModeEvidenceChecklist.ScreenshotSmokeCheckId
                && expectedEvidenceCheckIds[6] == P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId
                && expectedEvidenceCheckIds[7] == P0PlayModeEvidenceChecklist.DefeatFlowSmokeCheckId;
        }

        public static bool StartDefaultAcceptanceSmoke(string outputDirectory = null)
        {
            if (!Application.isPlaying)
            {
                Complete(
                    P0PlayModeAcceptanceSmokeState.Failed,
                    "P0 play mode acceptance smoke requires Play Mode.",
                    "StartDefaultAcceptanceSmoke was called outside Play Mode.");
                return false;
            }

            if (activeRunner != null)
            {
                UnityEngine.Object.Destroy(activeRunner.gameObject);
                activeRunner = null;
            }

            State = P0PlayModeAcceptanceSmokeState.Running;
            LastSummary = "P0 play mode acceptance smoke running.";
            LastDetailedLog = LastSummary;

            GameObject runnerObject = new GameObject(RunnerObjectName);
            UnityEngine.Object.DontDestroyOnLoad(runnerObject);
            activeRunner = runnerObject.AddComponent<P0PlayModeAcceptanceSmokeRunner>();
            activeRunner.Begin(outputDirectory);
            return true;
        }

        internal static void Complete(
            P0PlayModeAcceptanceSmokeState state,
            string summary,
            string detailedLog)
        {
            State = state;
            LastSummary = string.IsNullOrWhiteSpace(summary) ? state.ToString() : summary;
            LastDetailedLog = detailedLog ?? string.Empty;
            activeRunner = null;
        }
    }

    internal sealed class P0PlayModeAcceptanceSmokeRunner : MonoBehaviour
    {
        private const float ScreenshotSmokeTimeoutSeconds = 120f;
        private const float DefeatSmokeTimeoutSeconds = 45f;

        private readonly List<string> lines = new List<string>();

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
            failed = false;
            Add("Started P0 Play Mode acceptance smoke sequence.");

            if (!P0PlayModeScreenshotSmoke.StartDefaultScreenshotSmoke(outputDirectory))
            {
                Fail("Could not start screenshot smoke: " + P0PlayModeScreenshotSmoke.LastSummary);
                yield break;
            }

            yield return WaitForScreenshotSmoke();
            if (failed)
            {
                yield break;
            }

            if (P0PlayModeScreenshotSmoke.State != P0PlayModeScreenshotSmokeState.Passed)
            {
                Fail("Screenshot smoke did not pass: " + P0PlayModeScreenshotSmoke.LastSummary);
                yield break;
            }

            Add("Screenshot smoke passed: " + P0PlayModeScreenshotSmoke.LastSummary);

            if (P0PlayModeRouteFlowSmoke.State != P0PlayModeRouteFlowSmokeState.Passed)
            {
                Fail("Route-flow smoke did not pass during screenshot smoke: " + P0PlayModeRouteFlowSmoke.LastSummary);
                yield break;
            }

            Add("Route-flow smoke passed: " + P0PlayModeRouteFlowSmoke.LastSummary);

            if (!P0PlayModeDefeatFlowSmoke.StartDefaultDefeatSmoke())
            {
                Fail("Could not start defeat-flow smoke: " + P0PlayModeDefeatFlowSmoke.LastSummary);
                yield break;
            }

            yield return WaitForDefeatSmoke();
            if (failed)
            {
                yield break;
            }

            if (P0PlayModeDefeatFlowSmoke.State != P0PlayModeDefeatFlowSmokeState.Passed)
            {
                Fail("Defeat-flow smoke did not pass: " + P0PlayModeDefeatFlowSmoke.LastSummary);
                yield break;
            }

            Add("Defeat-flow smoke passed: " + P0PlayModeDefeatFlowSmoke.LastSummary);

            P0PlayModeEvidenceReport evidence = P0PlayModeEvidenceChecklist.EvaluateCurrent();
            Add("Evidence gate summary: " + evidence.BuildSummary());
            if (!evidence.IsComplete)
            {
                Add(evidence.BuildDetailedSummary());
                Fail("Play Mode evidence gate is not complete: " + evidence.BuildSummary());
                yield break;
            }

            string summary = "P0 play mode acceptance smoke passed: "
                + evidence.PassedCount
                + " evidence check(s), "
                + evidence.WarningCount
                + " warning(s).";
            Add(summary);
            Complete(P0PlayModeAcceptanceSmokeState.Passed, summary);
        }

        private IEnumerator WaitForScreenshotSmoke()
        {
            float start = Time.realtimeSinceStartup;
            while (!P0PlayModeScreenshotSmoke.IsFinished)
            {
                if (Time.realtimeSinceStartup - start > ScreenshotSmokeTimeoutSeconds)
                {
                    Fail("Timed out waiting for screenshot smoke.");
                    yield break;
                }

                yield return null;
            }
        }

        private IEnumerator WaitForDefeatSmoke()
        {
            float start = Time.realtimeSinceStartup;
            while (!P0PlayModeDefeatFlowSmoke.IsFinished)
            {
                if (Time.realtimeSinceStartup - start > DefeatSmokeTimeoutSeconds)
                {
                    Fail("Timed out waiting for defeat-flow smoke.");
                    yield break;
                }

                yield return null;
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
            Complete(P0PlayModeAcceptanceSmokeState.Failed, message);
        }

        private void Complete(P0PlayModeAcceptanceSmokeState state, string summary)
        {
            P0PlayModeAcceptanceSmoke.Complete(state, summary, string.Join("\n", lines));
            Destroy(gameObject);
        }
    }
}
