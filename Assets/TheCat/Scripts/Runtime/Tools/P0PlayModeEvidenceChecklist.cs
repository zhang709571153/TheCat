using System;
using System.Collections.Generic;
using System.IO;

namespace TheCat.Tools
{
    public enum P0PlayModeEvidenceState
    {
        Passed,
        Warning,
        Failed
    }

    public readonly struct P0PlayModeEvidenceCheck
    {
        public P0PlayModeEvidenceCheck(string checkId, string displayName, P0PlayModeEvidenceState state, string message)
        {
            CheckId = checkId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            State = state;
            Message = message ?? string.Empty;
        }

        public string CheckId { get; }

        public string DisplayName { get; }

        public P0PlayModeEvidenceState State { get; }

        public string Message { get; }

        public string BuildSummary()
        {
            return DisplayName + ": " + State + " - " + Message;
        }
    }

    public sealed class P0PlayModeEvidenceReport
    {
        private readonly List<P0PlayModeEvidenceCheck> checks = new List<P0PlayModeEvidenceCheck>();

        public IReadOnlyList<P0PlayModeEvidenceCheck> Checks => checks.AsReadOnly();

        public int FailureCount => Count(P0PlayModeEvidenceState.Failed);

        public int WarningCount => Count(P0PlayModeEvidenceState.Warning);

        public int PassedCount => Count(P0PlayModeEvidenceState.Passed);

        public bool HasBlockingFailures => FailureCount > 0;

        public bool HasPendingWarnings => WarningCount > 0;

        public bool IsUsable => FailureCount == 0;

        public bool IsComplete => FailureCount == 0 && WarningCount == 0;

        public void AddCheck(string checkId, string displayName, P0PlayModeEvidenceState state, string message)
        {
            checks.Add(new P0PlayModeEvidenceCheck(checkId, displayName, state, message));
        }

        public bool TryGetCheck(string checkId, out P0PlayModeEvidenceCheck check)
        {
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].CheckId == checkId)
                {
                    check = checks[i];
                    return true;
                }
            }

            check = default(P0PlayModeEvidenceCheck);
            return false;
        }

        public string BuildSummary()
        {
            return HasBlockingFailures
                ? "P0 Play Mode evidence has " + FailureCount + " failure(s), " + WarningCount + " pending warning(s), and " + PassedCount + " passed check(s)."
                : "P0 Play Mode evidence has no failures, " + WarningCount + " pending warning(s), and " + PassedCount + " passed check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < checks.Count; i++)
            {
                lines.Add("[" + checks[i].State + "] " + checks[i].BuildSummary());
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0PlayModeEvidenceState state)
        {
            int count = 0;
            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].State == state)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public readonly struct P0PlayModeEvidenceSnapshot
    {
        public P0PlayModeEvidenceSnapshot(
            bool hasScreenshotCapturePlan,
            bool hasRuntimeVisualScreenshotPlan,
            bool hasRuntimeVisualContactSheet,
            bool hasScreenshotFileEvidence,
            string screenshotFileEvidenceSummary,
            bool hasUnityRuntimeValidationPlan,
            string unityRuntimeValidationPlanSummary,
            P0PlayModeScreenshotSmokeState screenshotState,
            int screenshotCaptureCount,
            string screenshotSummary,
            P0PlayModeRouteFlowSmokeState routeFlowState,
            string routeFlowSummary,
            P0PlayModeDefeatFlowSmokeState defeatFlowState,
            string defeatFlowSummary)
        {
            HasScreenshotCapturePlan = hasScreenshotCapturePlan;
            HasRuntimeVisualScreenshotPlan = hasRuntimeVisualScreenshotPlan;
            HasRuntimeVisualContactSheet = hasRuntimeVisualContactSheet;
            HasScreenshotFileEvidence = hasScreenshotFileEvidence;
            ScreenshotFileEvidenceSummary = screenshotFileEvidenceSummary ?? string.Empty;
            HasUnityRuntimeValidationPlan = hasUnityRuntimeValidationPlan;
            UnityRuntimeValidationPlanSummary = unityRuntimeValidationPlanSummary ?? string.Empty;
            ScreenshotState = screenshotState;
            ScreenshotCaptureCount = screenshotCaptureCount;
            ScreenshotSummary = screenshotSummary ?? string.Empty;
            RouteFlowState = routeFlowState;
            RouteFlowSummary = routeFlowSummary ?? string.Empty;
            DefeatFlowState = defeatFlowState;
            DefeatFlowSummary = defeatFlowSummary ?? string.Empty;
        }

        public bool HasScreenshotCapturePlan { get; }

        public bool HasRuntimeVisualScreenshotPlan { get; }

        public bool HasRuntimeVisualContactSheet { get; }

        public bool HasScreenshotFileEvidence { get; }

        public string ScreenshotFileEvidenceSummary { get; }

        public bool HasUnityRuntimeValidationPlan { get; }

        public string UnityRuntimeValidationPlanSummary { get; }

        public P0PlayModeScreenshotSmokeState ScreenshotState { get; }

        public int ScreenshotCaptureCount { get; }

        public string ScreenshotSummary { get; }

        public P0PlayModeRouteFlowSmokeState RouteFlowState { get; }

        public string RouteFlowSummary { get; }

        public P0PlayModeDefeatFlowSmokeState DefeatFlowState { get; }

        public string DefeatFlowSummary { get; }
    }

    public static class P0PlayModeEvidenceChecklist
    {
        public const string ScreenshotCapturePlanCheckId = "screenshot_capture_plan";
        public const string RuntimeVisualScreenshotPlanCheckId = "runtime_visual_screenshot_plan";
        public const string RuntimeVisualContactSheetCheckId = "runtime_visual_contact_sheet";
        public const string ScreenshotFileEvidenceCheckId = "screenshot_file_evidence";
        public const string UnityRuntimeValidationPlanCheckId = "unity_runtime_validation_plan";
        public const string ScreenshotSmokeCheckId = "screenshot_smoke";
        public const string RouteFlowSmokeCheckId = "route_flow_smoke";
        public const string DefeatFlowSmokeCheckId = "defeat_flow_smoke";

        public static P0PlayModeEvidenceSnapshot CreateCurrentSnapshot()
        {
            P0PlayModeScreenshotFileEvidenceReport screenshotFiles = P0PlayModeScreenshotFileEvidence.EvaluateP0Directory();
            P0UnityRuntimeValidationPlanReport runtimeValidationPlan = P0UnityRuntimeValidationPlan.EvaluateCurrentPlan();
            return new P0PlayModeEvidenceSnapshot(
                P0PlayModeScreenshotSmoke.HasP0ScreenshotCapturePlan(),
                P0PlayModeScreenshotSmoke.HasRuntimeVisualScreenshotCapturePlan(),
                File.Exists(P0PlayModeScreenshotSmoke.RuntimeVisualContactSheetPath),
                screenshotFiles.IsComplete,
                screenshotFiles.BuildSummary(),
                runtimeValidationPlan.IsReady,
                runtimeValidationPlan.BuildSummary(),
                P0PlayModeScreenshotSmoke.State,
                P0PlayModeScreenshotSmoke.CapturedPaths.Count,
                P0PlayModeScreenshotSmoke.LastSummary,
                P0PlayModeRouteFlowSmoke.State,
                P0PlayModeRouteFlowSmoke.LastSummary,
                P0PlayModeDefeatFlowSmoke.State,
                P0PlayModeDefeatFlowSmoke.LastSummary);
        }

        public static P0PlayModeEvidenceReport EvaluateCurrent()
        {
            return Evaluate(CreateCurrentSnapshot());
        }

        public static P0PlayModeEvidenceReport Evaluate(P0PlayModeEvidenceSnapshot snapshot)
        {
            P0PlayModeEvidenceReport report = new P0PlayModeEvidenceReport();
            EvaluateScreenshotPlan(snapshot, report);
            EvaluateRuntimeVisualScreenshotPlan(snapshot, report);
            EvaluateRuntimeVisualContactSheet(snapshot, report);
            EvaluateScreenshotFileEvidence(snapshot, report);
            EvaluateUnityRuntimeValidationPlan(snapshot, report);
            EvaluateScreenshotSmoke(snapshot, report);
            EvaluateRouteFlowSmoke(snapshot, report);
            EvaluateDefeatFlowSmoke(snapshot, report);
            return report;
        }

        private static void EvaluateScreenshotPlan(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            report.AddCheck(
                ScreenshotCapturePlanCheckId,
                "Screenshot Capture Plan",
                snapshot.HasScreenshotCapturePlan ? P0PlayModeEvidenceState.Passed : P0PlayModeEvidenceState.Failed,
                snapshot.HasScreenshotCapturePlan
                    ? "Expected 11-capture plan is registered."
                    : "Expected screenshot capture plan is missing or incomplete.");
        }

        private static void EvaluateRuntimeVisualScreenshotPlan(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            report.AddCheck(
                RuntimeVisualScreenshotPlanCheckId,
                "Runtime Visual Screenshot Plan",
                snapshot.HasRuntimeVisualScreenshotPlan ? P0PlayModeEvidenceState.Passed : P0PlayModeEvidenceState.Failed,
                snapshot.HasRuntimeVisualScreenshotPlan
                    ? "Active cat, battle-world, and Call Tyrant warning VFX screenshots are registered."
                    : "Runtime visual screenshot evidence is missing active cat, battle-world, or Call Tyrant warning VFX captures.");
        }

        private static void EvaluateRuntimeVisualContactSheet(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            report.AddCheck(
                RuntimeVisualContactSheetCheckId,
                "Runtime Visual Contact Sheet",
                snapshot.HasRuntimeVisualContactSheet ? P0PlayModeEvidenceState.Passed : P0PlayModeEvidenceState.Failed,
                snapshot.HasRuntimeVisualContactSheet
                    ? "Runtime visual contact sheet is present for screenshot comparison: " + P0PlayModeScreenshotSmoke.RuntimeVisualContactSheetPath + "."
                    : "Runtime visual contact sheet is missing before Play Mode screenshot comparison: " + P0PlayModeScreenshotSmoke.RuntimeVisualContactSheetPath + ".");
        }

        private static void EvaluateScreenshotFileEvidence(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            report.AddCheck(
                ScreenshotFileEvidenceCheckId,
                "Screenshot File Evidence",
                snapshot.HasScreenshotFileEvidence ? P0PlayModeEvidenceState.Passed : P0PlayModeEvidenceState.Failed,
                snapshot.HasScreenshotFileEvidence
                    ? snapshot.ScreenshotFileEvidenceSummary
                    : "Screenshot file evidence is incomplete: " + snapshot.ScreenshotFileEvidenceSummary);
        }

        private static void EvaluateUnityRuntimeValidationPlan(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            report.AddCheck(
                UnityRuntimeValidationPlanCheckId,
                "Unity Runtime Validation Plan",
                snapshot.HasUnityRuntimeValidationPlan ? P0PlayModeEvidenceState.Passed : P0PlayModeEvidenceState.Failed,
                snapshot.HasUnityRuntimeValidationPlan
                    ? snapshot.UnityRuntimeValidationPlanSummary
                    : "Unity runtime validation plan is incomplete: " + snapshot.UnityRuntimeValidationPlanSummary);
        }

        private static void EvaluateScreenshotSmoke(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            if (snapshot.ScreenshotState == P0PlayModeScreenshotSmokeState.Passed)
            {
                bool hasAllCaptures = snapshot.ScreenshotCaptureCount >= P0PlayModeScreenshotSmoke.ExpectedCaptureCount;
                report.AddCheck(
                    ScreenshotSmokeCheckId,
                    "Screenshot Smoke",
                    hasAllCaptures ? P0PlayModeEvidenceState.Passed : P0PlayModeEvidenceState.Failed,
                    hasAllCaptures
                        ? snapshot.ScreenshotSummary
                        : "Screenshot smoke passed but captured only " + snapshot.ScreenshotCaptureCount + "/" + P0PlayModeScreenshotSmoke.ExpectedCaptureCount + " screenshot(s).");
                return;
            }

            AddPlayModeSmokeState(
                report,
                ScreenshotSmokeCheckId,
                "Screenshot Smoke",
                snapshot.ScreenshotState.ToString(),
                snapshot.ScreenshotSummary);
        }

        private static void EvaluateRouteFlowSmoke(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            if (snapshot.RouteFlowState == P0PlayModeRouteFlowSmokeState.Passed)
            {
                report.AddCheck(
                    RouteFlowSmokeCheckId,
                    "Route Flow Smoke",
                    P0PlayModeEvidenceState.Passed,
                    snapshot.RouteFlowSummary);
                return;
            }

            AddPlayModeSmokeState(
                report,
                RouteFlowSmokeCheckId,
                "Route Flow Smoke",
                snapshot.RouteFlowState.ToString(),
                snapshot.RouteFlowSummary);
        }

        private static void EvaluateDefeatFlowSmoke(
            P0PlayModeEvidenceSnapshot snapshot,
            P0PlayModeEvidenceReport report)
        {
            if (snapshot.DefeatFlowState == P0PlayModeDefeatFlowSmokeState.Passed)
            {
                report.AddCheck(
                    DefeatFlowSmokeCheckId,
                    "Defeat Flow Smoke",
                    P0PlayModeEvidenceState.Passed,
                    snapshot.DefeatFlowSummary);
                return;
            }

            AddPlayModeSmokeState(
                report,
                DefeatFlowSmokeCheckId,
                "Defeat Flow Smoke",
                snapshot.DefeatFlowState.ToString(),
                snapshot.DefeatFlowSummary);
        }

        private static void AddPlayModeSmokeState(
            P0PlayModeEvidenceReport report,
            string checkId,
            string displayName,
            string state,
            string summary)
        {
            bool failed = state == "Failed";
            report.AddCheck(
                checkId,
                displayName,
                failed ? P0PlayModeEvidenceState.Failed : P0PlayModeEvidenceState.Warning,
                failed
                    ? "Smoke failed: " + summary
                    : "Smoke is pending or not complete yet (" + state + "): " + summary);
        }
    }
}
