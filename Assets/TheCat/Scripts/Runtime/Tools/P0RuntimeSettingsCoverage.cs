using System;
using System.Collections.Generic;
using TheCat.Gameplay;
using TheCat.Inputs;

namespace TheCat.Tools
{
    public enum P0RuntimeSettingsCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0RuntimeSettingsCoverageIssue
    {
        public P0RuntimeSettingsCoverageIssue(P0RuntimeSettingsCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0RuntimeSettingsCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0RuntimeSettingsCoverageReport
    {
        private readonly List<P0RuntimeSettingsCoverageIssue> issues = new List<P0RuntimeSettingsCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0RuntimeSettingsCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0RuntimeSettingsCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0RuntimeSettingsCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0RuntimeSettingsCoverageSeverity severity, string message)
        {
            issues.Add(new P0RuntimeSettingsCoverageIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 runtime settings coverage complete for " + coveredChecks.Count + " check(s)."
                : "P0 runtime settings coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " covered check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0RuntimeSettingsCoverageSeverity severity)
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

    public static class P0RuntimeSettingsCoverage
    {
        public const int ExpectedCoveredCheckCount = 13;

        public static P0RuntimeSettingsCoverageReport EvaluatePrototypeSettings()
        {
            P0RuntimeSettingsCoverageReport report = new P0RuntimeSettingsCoverageReport();

            EvaluateDefaultPresentation(report);
            EvaluatePauseDelta(report);
            EvaluateSpeedPresets(report);
            EvaluateInvalidSpeedGuards(report);
            EvaluateKeyboardBindings(report);
            EvaluateActionSurface(report);
            EvaluateActiveSpeedGating(report);
            EvaluatePauseOverlayContract(report);
            EvaluateSettingsSurfaceContract(report);
            EvaluateFullSettingsScreenHook(report);
            EvaluateRestartConfirmationContract(report);
            EvaluateKeyboardAndButtonParity(report);
            EvaluateBatch85Boundary(report);

            return report;
        }

        private static void EvaluateDefaultPresentation(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            P0RuntimeSettingsPresentation presentation = P0RuntimeSettingsPresenter.Build(settings);
            string summary = presentation.BuildSummary();

            Require(
                report,
                !presentation.IsPaused
                && presentation.PauseButtonLabel == "暂停"
                && P0RuntimeSettingsPresenter.HasP0RuntimeSettingsSurface(presentation)
                && summary.Contains("运行设置：实时")
                && summary.Contains("速度 1 倍")
                && summary.Contains("暂停键 P/Esc")
                && summary.Contains("速度键 F1/F2/F3")
                && summary.Contains("操作 4"),
                "Runtime settings default presentation exposes live state, 1 倍 speed, and keyboard hints.",
                "Runtime settings default presentation is missing status, speed, or key hints.");
        }

        private static void EvaluatePauseDelta(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            settings.TogglePause();
            P0RuntimeSettingsPresentation presentation = P0RuntimeSettingsPresenter.Build(settings);

            Require(
                report,
                settings.IsPaused
                && settings.ApplyToDelta(2f) == 0f
                && presentation.PauseButtonLabel == "继续"
                && presentation.BuildSummary().Contains("已暂停"),
                "Pause toggles the runtime to paused, stops battle delta, and swaps the button to Resume.",
                "Pause did not stop delta or update the runtime settings presentation.");
        }

        private static void EvaluateSpeedPresets(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            settings.SetBattleSpeed(0.5f);
            bool half = settings.ApplyToDelta(2f) == 1f && P0RuntimeSettingsPresenter.Build(settings).SpeedLabel == "0.5 倍";
            settings.SetBattleSpeed(1f);
            bool normal = settings.ApplyToDelta(2f) == 2f && P0RuntimeSettingsPresenter.Build(settings).SpeedLabel == "1 倍";
            settings.SetBattleSpeed(1.5f);
            bool fast = settings.ApplyToDelta(2f) == 3f && P0RuntimeSettingsPresenter.Build(settings).SpeedLabel == "1.5 倍";
            settings.Reset();

            Require(
                report,
                half && normal && fast && settings.BattleSpeedMultiplier == 1f && !settings.IsPaused,
                "Speed presets 0.5 倍, 1 倍, and 1.5 倍 scale delta and Reset restores live 1 倍.",
                "Runtime speed presets or reset behavior failed.");
        }

        private static void EvaluateInvalidSpeedGuards(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            bool rejectedLow = ThrowsArgumentOutOfRange(() => settings.SetBattleSpeed(0f));
            bool rejectedHigh = ThrowsArgumentOutOfRange(() => settings.SetBattleSpeed(3f));
            bool rejectedNegativeDelta = ThrowsArgumentOutOfRange(() => settings.ApplyToDelta(-0.1f));

            Require(
                report,
                rejectedLow && rejectedHigh && rejectedNegativeDelta,
                "Runtime settings reject unsupported speed values and negative delta.",
                "Runtime settings guard rails did not reject invalid values.");
        }

        private static void EvaluateKeyboardBindings(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettingsPresentation presentation = P0RuntimeSettingsPresenter.Build(new P0RuntimeSettings());
            bool hasPause = Enum.IsDefined(typeof(P0InputCommand), P0InputCommand.TogglePause)
                && presentation.PauseShortcutLabel == "P/Esc";
            bool hasHalf = Enum.IsDefined(typeof(P0InputCommand), P0InputCommand.SpeedHalf)
                && presentation.SpeedShortcutLabel.Contains("F1");
            bool hasNormal = Enum.IsDefined(typeof(P0InputCommand), P0InputCommand.SpeedNormal)
                && presentation.SpeedShortcutLabel.Contains("F2");
            bool hasFast = Enum.IsDefined(typeof(P0InputCommand), P0InputCommand.SpeedFast)
                && presentation.SpeedShortcutLabel.Contains("F3");

            Require(
                report,
                hasPause && hasHalf && hasNormal && hasFast,
                "Pause and speed commands expose P/Esc and F1/F2/F3 runtime shortcut labels.",
                "Pause or speed runtime shortcut label is missing.");
        }

        private static void EvaluateActionSurface(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettingsPresentation presentation = P0RuntimeSettingsPresenter.Build(new P0RuntimeSettings());

            Require(
                report,
                presentation.TryGetAction(P0RuntimeSettingsActionIds.TogglePause, out P0RuntimeSettingsAction pause)
                && pause.IsEnabled
                && pause.Label == "暂停"
                && pause.ShortcutLabel == "P/Esc"
                && presentation.TryGetAction(P0RuntimeSettingsActionIds.SpeedHalf, out P0RuntimeSettingsAction half)
                && half.IsEnabled
                && half.TargetSpeedMultiplier == 0.5f
                && presentation.TryGetAction(P0RuntimeSettingsActionIds.SpeedNormal, out P0RuntimeSettingsAction normal)
                && !normal.IsEnabled
                && normal.IsCurrent
                && presentation.TryGetAction(P0RuntimeSettingsActionIds.SpeedFast, out P0RuntimeSettingsAction fast)
                && fast.IsEnabled
                && fast.ShortcutLabel == "F3",
                "Runtime settings action surface exposes pause plus 0.5 倍/1 倍/1.5 倍 speed buttons with current speed disabled.",
                "Runtime settings action surface did not expose the required pause and speed actions.");
        }

        private static void EvaluateActiveSpeedGating(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            settings.SetBattleSpeed(1.5f);
            P0RuntimeSettingsPresentation fastPresentation = P0RuntimeSettingsPresenter.Build(settings);
            settings.TogglePause();
            P0RuntimeSettingsPresentation pausedPresentation = P0RuntimeSettingsPresenter.Build(settings);

            Require(
                report,
                fastPresentation.TryGetAction(P0RuntimeSettingsActionIds.SpeedNormal, out P0RuntimeSettingsAction normal)
                && normal.IsEnabled
                && fastPresentation.TryGetAction(P0RuntimeSettingsActionIds.SpeedFast, out P0RuntimeSettingsAction fast)
                && fast.IsCurrent
                && !fast.IsEnabled
                && pausedPresentation.TryGetAction(P0RuntimeSettingsActionIds.TogglePause, out P0RuntimeSettingsAction resume)
                && resume.Label == "继续"
                && resume.IsEnabled
                && pausedPresentation.IsPaused,
                "Runtime settings action surface moves the disabled current-speed button and swaps Pause to Resume when paused.",
                "Runtime settings action surface did not update current speed or paused action labels.");
        }

        private static void EvaluatePauseOverlayContract(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            settings.TogglePause();
            P0PauseSettingsSurface surface = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(settings, false);

            Require(
                report,
                settings.ApplyToDelta(2f) == 0f
                && surface.RuntimeSettings.IsPaused
                && P0RuntimeSettingsPresenter.HasP0PauseSettingsSurface(surface)
                && surface.TryGetAction(P0RuntimeSettingsActionIds.TogglePause, out P0RuntimeSettingsAction resume)
                && resume.Command == P0InputCommand.TogglePause
                && !string.IsNullOrWhiteSpace(resume.Label)
                && resume.Label == surface.RuntimeSettings.PauseButtonLabel
                && resume.IsEnabled
                && surface.BuildSummary().Contains("暂停 / 设置"),
                "Pause overlay contract exposes Resume, P/Esc hint, speed controls, and paused delta freeze.",
                "Pause overlay contract did not expose resume, hints, speed controls, or delta freeze.");
        }

        private static void EvaluateSettingsSurfaceContract(P0RuntimeSettingsCoverageReport report)
        {
            P0PauseSettingsSurface surface = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(new P0RuntimeSettings(), false);
            string summary = surface.BuildSummary();

            Require(
                report,
                P0RuntimeSettingsPresenter.HasP0PauseSettingsSurface(surface)
                && surface.Sections.Count == 3
                && surface.KeyHints.Count == 3
                && summary.Contains("速度")
                && summary.Contains("确认 False")
                && !summary.Contains("音乐")
                && !summary.Contains("音效")
                && !summary.Contains("画面"),
                "Settings surface contract limits D2 settings to pause, speed, key hints, and restart confirmation.",
                "Settings surface contract drifted into unsupported settings or lost required sections.");
        }

        private static void EvaluateFullSettingsScreenHook(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            P0FullSettingsSurface combat = P0RuntimeSettingsPresenter.BuildFullSettingsSurface(
                settings,
                P0FullSettingsIds.CombatTab,
                false);
            P0FullSettingsSurface controls = P0RuntimeSettingsPresenter.BuildFullSettingsSurface(
                settings,
                P0FullSettingsIds.ControlsTab,
                true);
            List<P0FullSettingsOptionRow> badRows = new List<P0FullSettingsOptionRow>(combat.OptionRows);
            badRows[0] = new P0FullSettingsOptionRow(
                P0FullSettingsIds.PauseRow,
                "Assets/TheCat/Art/UI",
                "batch_85_settings_pause_preflight_2026-06-25",
                "toggle",
                "P/Esc",
                true,
                false,
                "candidate_v001.png.meta");
            P0FullSettingsSurface badSurface = new P0FullSettingsSurface(
                combat.TitleLabel,
                combat.SubtitleLabel,
                combat.ActiveTabId,
                combat.PauseSurface,
                combat.Tabs,
                badRows.AsReadOnly(),
                combat.ModalTitle,
                combat.ModalDetail);

            Require(
                report,
                P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(combat)
                && P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(controls)
                && combat.Tabs.Count == 2
                && combat.OptionRows.Count == 6
                && controls.TryGetOptionRow(P0FullSettingsIds.ConfirmRestartRow, out P0FullSettingsOptionRow confirm)
                && confirm.IsEnabled
                && !string.IsNullOrWhiteSpace(controls.ModalTitle)
                && !combat.BuildSummary().Contains("batch_85")
                && !combat.BuildSummary().Contains(".png")
                && !P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(badSurface),
                "Full settings screen hook exposes tabs, option rows, restart modal, and deep Batch 85 candidate-only boundary.",
                "Full settings screen hook is missing tabs, rows, modal, or Batch 85 boundary.");
        }

        private static void EvaluateRestartConfirmationContract(P0RuntimeSettingsCoverageReport report)
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            P0PauseSettingsSurface closed = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(settings, false);
            P0PauseSettingsSurface open = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(settings, true);

            Require(
                report,
                closed.TryGetAction(P0RuntimeSettingsActionIds.RequestRestart, out P0RuntimeSettingsAction requestClosed)
                && requestClosed.IsEnabled
                && requestClosed.RequiresConfirmation
                && closed.TryGetAction(P0RuntimeSettingsActionIds.ConfirmRestart, out P0RuntimeSettingsAction confirmClosed)
                && !confirmClosed.IsEnabled
                && open.TryGetAction(P0RuntimeSettingsActionIds.RequestRestart, out P0RuntimeSettingsAction requestOpen)
                && !requestOpen.IsEnabled
                && open.TryGetAction(P0RuntimeSettingsActionIds.ConfirmRestart, out P0RuntimeSettingsAction confirmOpen)
                && confirmOpen.IsEnabled
                && !confirmOpen.RequiresConfirmation
                && open.RestartConfirmationDetail.Contains("再次确认"),
                "Restart confirmation contract blocks direct restart until the confirmation surface is open.",
                "Restart confirmation contract allows restart without confirmation or hides the confirm copy.");
        }

        private static void EvaluateKeyboardAndButtonParity(P0RuntimeSettingsCoverageReport report)
        {
            P0PauseSettingsSurface surface = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(new P0RuntimeSettings(), false);

            Require(
                report,
                ActionUsesBinding(surface, P0RuntimeSettingsActionIds.TogglePause, P0InputCommand.TogglePause)
                && ActionUsesBinding(surface, P0RuntimeSettingsActionIds.SpeedHalf, P0InputCommand.SpeedHalf)
                && ActionUsesBinding(surface, P0RuntimeSettingsActionIds.SpeedNormal, P0InputCommand.SpeedNormal)
                && ActionUsesBinding(surface, P0RuntimeSettingsActionIds.SpeedFast, P0InputCommand.SpeedFast)
                && ActionUsesBinding(surface, P0RuntimeSettingsActionIds.RequestRestart, P0InputCommand.RestartRun),
                "Keyboard and button parity keeps pause, speed, and restart actions on the shared input map.",
                "Keyboard and button parity failed for pause, speed, or restart actions.");
        }

        private static void EvaluateBatch85Boundary(P0RuntimeSettingsCoverageReport report)
        {
            P0PauseSettingsSurface surface = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(new P0RuntimeSettings(), true);
            bool actionTextIsCodeSideOnly = true;
            for (int i = 0; i < surface.Actions.Count; i++)
            {
                string summary = surface.Actions[i].BuildSummary();
                actionTextIsCodeSideOnly &= !summary.Contains(".png")
                    && !summary.Contains(".meta")
                    && !summary.Contains("Sprite")
                    && !summary.Contains("batch_85");
            }

            Require(
                report,
                actionTextIsCodeSideOnly
                && !surface.BuildSummary().Contains(".png")
                && !surface.BuildSummary().Contains("batch_85"),
                "Batch 85 boundary stays candidate-only: D2 pause/settings acceptance has no runtime asset binding requirement.",
                "Batch 85 candidate assets leaked into runtime settings acceptance.");
        }

        private static void Require(
            P0RuntimeSettingsCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0RuntimeSettingsCoverageSeverity.Failure, failureMessage);
        }

        private static bool ThrowsArgumentOutOfRange(Action action)
        {
            try
            {
                action();
                return false;
            }
            catch (ArgumentOutOfRangeException)
            {
                return true;
            }
        }

        private static bool ActionUsesBinding(
            P0PauseSettingsSurface surface,
            string actionId,
            P0InputCommand command)
        {
            return surface.TryGetAction(actionId, out P0RuntimeSettingsAction action)
                && action.Command == command
                && P0KeyboardInputMap.TryGetBinding(command, out P0InputBinding binding)
                && action.ShortcutLabel == BuildBindingLabel(binding);
        }

        private static string BuildBindingLabel(P0InputBinding binding)
        {
            return binding.HasSecondaryKey
                ? binding.PrimaryKeyLabel + "/" + binding.SecondaryKeyLabel
                : binding.PrimaryKeyLabel;
        }
    }
}
