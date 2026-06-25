using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0RuntimeSettingsCoverageTests
    {
        [Test]
        public void Presenter_BuildsReadableRuntimeSettingsSummary()
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();

            P0RuntimeSettingsPresentation live = P0RuntimeSettingsPresenter.Build(settings);
            settings.TogglePause();
            P0RuntimeSettingsPresentation paused = P0RuntimeSettingsPresenter.Build(settings);

            Assert.IsFalse(live.IsPaused);
            Assert.IsTrue(paused.IsPaused);
            Assert.IsFalse(string.IsNullOrWhiteSpace(live.PauseButtonLabel));
            Assert.IsFalse(string.IsNullOrWhiteSpace(paused.PauseButtonLabel));
            Assert.AreNotEqual(live.PauseButtonLabel, paused.PauseButtonLabel);
            Assert.IsTrue(P0RuntimeSettingsPresenter.HasP0RuntimeSettingsSurface(live));
            Assert.AreEqual(4, live.Actions.Count);
            Assert.IsTrue(live.TryGetAction(P0RuntimeSettingsActionIds.SpeedNormal, out P0RuntimeSettingsAction normal));
            Assert.IsTrue(normal.IsCurrent);
            Assert.IsFalse(normal.IsEnabled);
            Assert.IsTrue(live.TryGetAction(P0RuntimeSettingsActionIds.SpeedFast, out P0RuntimeSettingsAction fast));
            Assert.IsTrue(fast.IsEnabled);
            Assert.AreEqual(P0InputCommand.SpeedFast, fast.Command);
            StringAssert.Contains("P/Esc", live.BuildSummary());
            StringAssert.Contains("F1/F2/F3", live.BuildSummary());
            StringAssert.Contains("4", live.BuildSummary());
        }

        [Test]
        public void Presenter_ActionSurfaceMovesCurrentSpeedAndResumeLabel()
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();
            settings.SetBattleSpeed(1.5f);

            P0RuntimeSettingsPresentation fast = P0RuntimeSettingsPresenter.Build(settings);
            settings.TogglePause();
            P0RuntimeSettingsPresentation paused = P0RuntimeSettingsPresenter.Build(settings);

            Assert.IsTrue(fast.TryGetAction(P0RuntimeSettingsActionIds.SpeedFast, out P0RuntimeSettingsAction fastAction));
            Assert.IsTrue(fastAction.IsCurrent);
            Assert.IsFalse(fastAction.IsEnabled);
            Assert.IsTrue(fast.TryGetAction(P0RuntimeSettingsActionIds.SpeedNormal, out P0RuntimeSettingsAction normalAction));
            Assert.IsTrue(normalAction.IsEnabled);
            Assert.IsTrue(paused.TryGetAction(P0RuntimeSettingsActionIds.TogglePause, out P0RuntimeSettingsAction pauseAction));
            Assert.AreEqual(P0InputCommand.TogglePause, pauseAction.Command);
            Assert.IsTrue(pauseAction.IsEnabled);
            StringAssert.Contains("1", P0RuntimeSettingsPresenter.BuildCompactSummary(fast));
        }

        [Test]
        public void Presenter_BuildsPauseSettingsSurfaceWithRestartConfirmation()
        {
            P0RuntimeSettings settings = new P0RuntimeSettings();

            P0PauseSettingsSurface closed = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(settings, false);
            P0PauseSettingsSurface open = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(settings, true);
            settings.TogglePause();
            P0PauseSettingsSurface paused = P0RuntimeSettingsPresenter.BuildPauseSettingsSurface(settings, false);

            Assert.IsTrue(P0RuntimeSettingsPresenter.HasP0PauseSettingsSurface(closed));
            Assert.AreEqual("暂停 / 设置", closed.TitleLabel);
            Assert.AreEqual(3, closed.Sections.Count);
            Assert.AreEqual(3, closed.KeyHints.Count);
            Assert.AreEqual(6, closed.Actions.Count);
            Assert.IsTrue(closed.TryGetAction(P0RuntimeSettingsActionIds.RequestRestart, out P0RuntimeSettingsAction request));
            Assert.AreEqual(P0InputCommand.RestartRun, request.Command);
            Assert.IsTrue(request.RequiresConfirmation);
            Assert.IsTrue(request.IsEnabled);
            Assert.IsTrue(closed.TryGetAction(P0RuntimeSettingsActionIds.ConfirmRestart, out P0RuntimeSettingsAction closedConfirm));
            Assert.IsFalse(closedConfirm.IsEnabled);
            Assert.IsTrue(open.TryGetAction(P0RuntimeSettingsActionIds.ConfirmRestart, out P0RuntimeSettingsAction openConfirm));
            Assert.IsTrue(openConfirm.IsEnabled);
            Assert.IsTrue(paused.RuntimeSettings.IsPaused);
            Assert.AreEqual(0f, settings.ApplyToDelta(1.5f));
            StringAssert.Contains("再次确认", open.RestartConfirmationDetail);
            StringAssert.Contains("P/Esc", closed.KeyHints[0]);
            StringAssert.Contains("F1/F2/F3", closed.KeyHints[1]);
            StringAssert.Contains("N", closed.KeyHints[2]);
            Assert.IsFalse(closed.BuildSummary().Contains("batch_85"));
            Assert.IsFalse(closed.BuildSummary().Contains(".png"));
        }

        [Test]
        public void Presenter_BuildsFullSettingsSurfaceWithTabsAndRows()
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

            Assert.IsTrue(P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(combat));
            Assert.IsTrue(P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(controls));
            Assert.AreEqual(2, combat.Tabs.Count);
            Assert.AreEqual(6, combat.OptionRows.Count);
            Assert.IsTrue(combat.TryGetOptionRow(P0FullSettingsIds.PauseRow, out P0FullSettingsOptionRow pause));
            Assert.AreEqual("toggle", pause.ControlKind);
            Assert.AreEqual(combat.PauseSurface.RuntimeSettings.StatusLabel, pause.ValueLabel);
            Assert.IsTrue(combat.TryGetOptionRow(P0FullSettingsIds.SpeedFastRow, out P0FullSettingsOptionRow fast));
            Assert.AreEqual("F3", fast.ShortcutLabel);
            StringAssert.Contains("1.5", fast.ValueLabel);
            Assert.IsTrue(combat.TryGetOptionRow(P0FullSettingsIds.RestartRow, out P0FullSettingsOptionRow restart));
            Assert.AreEqual("needs-confirm", restart.ValueLabel);
            Assert.AreNotEqual(combat.PauseSurface.RuntimeSettings.SpeedLabel, restart.ValueLabel);
            Assert.IsTrue(controls.TryGetOptionRow(P0FullSettingsIds.ConfirmRestartRow, out P0FullSettingsOptionRow confirm));
            Assert.IsTrue(confirm.IsEnabled);
            Assert.AreEqual("confirm-open", confirm.ValueLabel);
            Assert.AreNotEqual(controls.PauseSurface.RuntimeSettings.SpeedLabel, confirm.ValueLabel);
            Assert.IsFalse(combat.BuildSummary().Contains("batch_85"));
            Assert.IsFalse(combat.BuildSummary().Contains(".png"));
        }

        [Test]
        public void Presenter_RejectsCandidateTokensInFullSettingsRows()
        {
            P0FullSettingsSurface valid = P0RuntimeSettingsPresenter.BuildFullSettingsSurface(
                new P0RuntimeSettings(),
                P0FullSettingsIds.CombatTab,
                false);
            List<P0FullSettingsOptionRow> rows = new List<P0FullSettingsOptionRow>(valid.OptionRows);
            rows[0] = new P0FullSettingsOptionRow(
                P0FullSettingsIds.PauseRow,
                "Assets/TheCat/Art/UI",
                "batch_85_settings_pause_preflight_2026-06-25",
                "toggle",
                "P/Esc",
                true,
                false,
                "candidate_v001.png.meta");
            P0FullSettingsSurface withCandidatePath = new P0FullSettingsSurface(
                valid.TitleLabel,
                valid.SubtitleLabel,
                valid.ActiveTabId,
                valid.PauseSurface,
                valid.Tabs,
                rows.AsReadOnly(),
                valid.ModalTitle,
                valid.ModalDetail);

            Assert.IsTrue(P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(valid));
            Assert.IsFalse(P0RuntimeSettingsPresenter.HasP0FullSettingsSurface(withCandidatePath));
        }

        [Test]
        public void EvaluatePrototypeSettings_CompletesRuntimeSettingsCoverage()
        {
            P0RuntimeSettingsCoverageReport report = P0RuntimeSettingsCoverage.EvaluatePrototypeSettings();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0RuntimeSettingsCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("runtime settings coverage complete", report.BuildSummary());
            StringAssert.Contains("Speed presets", report.BuildDetailedSummary());
            StringAssert.Contains("P/Esc", report.BuildDetailedSummary());
            StringAssert.Contains("Restart confirmation", report.BuildDetailedSummary());
            StringAssert.Contains("Full settings screen hook", report.BuildDetailedSummary());
            StringAssert.Contains("Batch 85 boundary", report.BuildDetailedSummary());
        }
    }
}
