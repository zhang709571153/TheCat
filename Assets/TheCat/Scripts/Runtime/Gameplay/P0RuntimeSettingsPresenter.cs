using System;
using System.Collections.Generic;
using TheCat.Inputs;

namespace TheCat.Gameplay
{
    public static class P0RuntimeSettingsActionIds
    {
        public const string TogglePause = "toggle_pause";
        public const string SpeedHalf = "speed_half";
        public const string SpeedNormal = "speed_normal";
        public const string SpeedFast = "speed_fast";
        public const string RequestRestart = "request_restart";
        public const string ConfirmRestart = "confirm_restart";
    }

    public readonly struct P0RuntimeSettingsAction
    {
        public P0RuntimeSettingsAction(
            string actionId,
            string label,
            bool isEnabled,
            bool isCurrent,
            P0InputCommand command,
            float targetSpeedMultiplier,
            string shortcutLabel,
            string detail,
            bool requiresConfirmation = false)
        {
            ActionId = actionId ?? string.Empty;
            Label = label ?? string.Empty;
            IsEnabled = isEnabled;
            IsCurrent = isCurrent;
            Command = command;
            TargetSpeedMultiplier = Math.Max(0f, targetSpeedMultiplier);
            ShortcutLabel = shortcutLabel ?? string.Empty;
            Detail = detail ?? string.Empty;
            RequiresConfirmation = requiresConfirmation;
        }

        public string ActionId { get; }

        public string Label { get; }

        public bool IsEnabled { get; }

        public bool IsCurrent { get; }

        public P0InputCommand Command { get; }

        public float TargetSpeedMultiplier { get; }

        public string ShortcutLabel { get; }

        public string Detail { get; }

        public bool RequiresConfirmation { get; }

        public string BuildSummary()
        {
            return Label
                + " 可用 "
                + IsEnabled
                + " 当前 "
                + IsCurrent
                + " 按键 "
                + ShortcutLabel
                + " "
                + Detail
                + " 确认 "
                + RequiresConfirmation;
        }
    }

    public readonly struct P0PauseSettingsSurface
    {
        public P0PauseSettingsSurface(
            string titleLabel,
            string subtitleLabel,
            bool isRestartConfirmationOpen,
            string restartConfirmationTitle,
            string restartConfirmationDetail,
            P0RuntimeSettingsPresentation runtimeSettings,
            IReadOnlyList<string> sections,
            IReadOnlyList<string> keyHints,
            IReadOnlyList<P0RuntimeSettingsAction> actions)
        {
            TitleLabel = titleLabel ?? string.Empty;
            SubtitleLabel = subtitleLabel ?? string.Empty;
            IsRestartConfirmationOpen = isRestartConfirmationOpen;
            RestartConfirmationTitle = restartConfirmationTitle ?? string.Empty;
            RestartConfirmationDetail = restartConfirmationDetail ?? string.Empty;
            RuntimeSettings = runtimeSettings;
            Sections = sections ?? Array.Empty<string>();
            KeyHints = keyHints ?? Array.Empty<string>();
            Actions = actions ?? Array.Empty<P0RuntimeSettingsAction>();
        }

        public string TitleLabel { get; }

        public string SubtitleLabel { get; }

        public bool IsRestartConfirmationOpen { get; }

        public string RestartConfirmationTitle { get; }

        public string RestartConfirmationDetail { get; }

        public P0RuntimeSettingsPresentation RuntimeSettings { get; }

        public IReadOnlyList<string> Sections { get; }

        public IReadOnlyList<string> KeyHints { get; }

        public IReadOnlyList<P0RuntimeSettingsAction> Actions { get; }

        public bool TryGetAction(string actionId, out P0RuntimeSettingsAction action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionId == actionId)
                {
                    action = Actions[i];
                    return true;
                }
            }

            action = default(P0RuntimeSettingsAction);
            return false;
        }

        public string BuildSummary()
        {
            return TitleLabel
                + " "
                + SubtitleLabel
                + " 状态 "
                + RuntimeSettings.StatusLabel
                + " 速度 "
                + RuntimeSettings.SpeedLabel
                + " 分区 "
                + Sections.Count
                + " 提示 "
                + KeyHints.Count
                + " 操作 "
                + Actions.Count
                + " 确认 "
                + IsRestartConfirmationOpen;
        }
    }

    public static class P0FullSettingsIds
    {
        public const string CombatTab = "tab_combat";
        public const string ControlsTab = "tab_controls";
        public const string PauseRow = "row_pause";
        public const string SpeedHalfRow = "row_speed_half";
        public const string SpeedNormalRow = "row_speed_normal";
        public const string SpeedFastRow = "row_speed_fast";
        public const string RestartRow = "row_restart";
        public const string ConfirmRestartRow = "row_confirm_restart";
    }

    public readonly struct P0FullSettingsTab
    {
        public P0FullSettingsTab(string tabId, string label, bool isSelected, string detail)
        {
            TabId = tabId ?? string.Empty;
            Label = label ?? string.Empty;
            IsSelected = isSelected;
            Detail = detail ?? string.Empty;
        }

        public string TabId { get; }

        public string Label { get; }

        public bool IsSelected { get; }

        public string Detail { get; }
    }

    public readonly struct P0FullSettingsOptionRow
    {
        public P0FullSettingsOptionRow(
            string rowId,
            string label,
            string valueLabel,
            string controlKind,
            string shortcutLabel,
            bool isEnabled,
            bool isCurrent,
            string detail)
        {
            RowId = rowId ?? string.Empty;
            Label = label ?? string.Empty;
            ValueLabel = valueLabel ?? string.Empty;
            ControlKind = controlKind ?? string.Empty;
            ShortcutLabel = shortcutLabel ?? string.Empty;
            IsEnabled = isEnabled;
            IsCurrent = isCurrent;
            Detail = detail ?? string.Empty;
        }

        public string RowId { get; }

        public string Label { get; }

        public string ValueLabel { get; }

        public string ControlKind { get; }

        public string ShortcutLabel { get; }

        public bool IsEnabled { get; }

        public bool IsCurrent { get; }

        public string Detail { get; }

        public string BuildSummary()
        {
            return RowId
                + " "
                + Label
                + " "
                + ValueLabel
                + " "
                + ControlKind
                + " "
                + ShortcutLabel
                + " enabled "
                + IsEnabled
                + " current "
                + IsCurrent
                + " "
                + Detail;
        }
    }

    public readonly struct P0FullSettingsSurface
    {
        public P0FullSettingsSurface(
            string titleLabel,
            string subtitleLabel,
            string activeTabId,
            P0PauseSettingsSurface pauseSurface,
            IReadOnlyList<P0FullSettingsTab> tabs,
            IReadOnlyList<P0FullSettingsOptionRow> optionRows,
            string modalTitle,
            string modalDetail)
        {
            TitleLabel = titleLabel ?? string.Empty;
            SubtitleLabel = subtitleLabel ?? string.Empty;
            ActiveTabId = activeTabId ?? string.Empty;
            PauseSurface = pauseSurface;
            Tabs = tabs ?? Array.Empty<P0FullSettingsTab>();
            OptionRows = optionRows ?? Array.Empty<P0FullSettingsOptionRow>();
            ModalTitle = modalTitle ?? string.Empty;
            ModalDetail = modalDetail ?? string.Empty;
        }

        public string TitleLabel { get; }

        public string SubtitleLabel { get; }

        public string ActiveTabId { get; }

        public P0PauseSettingsSurface PauseSurface { get; }

        public IReadOnlyList<P0FullSettingsTab> Tabs { get; }

        public IReadOnlyList<P0FullSettingsOptionRow> OptionRows { get; }

        public string ModalTitle { get; }

        public string ModalDetail { get; }

        public bool TryGetOptionRow(string rowId, out P0FullSettingsOptionRow row)
        {
            for (int i = 0; i < OptionRows.Count; i++)
            {
                if (OptionRows[i].RowId == rowId)
                {
                    row = OptionRows[i];
                    return true;
                }
            }

            row = default(P0FullSettingsOptionRow);
            return false;
        }

        public string BuildSummary()
        {
            return TitleLabel
                + " activeTab "
                + ActiveTabId
                + " tabs "
                + Tabs.Count
                + " rows "
                + OptionRows.Count
                + " modal "
                + (!string.IsNullOrWhiteSpace(ModalTitle))
                + " pause "
                + PauseSurface.RuntimeSettings.StatusLabel;
        }
    }

    public readonly struct P0RuntimeSettingsPresentation
    {
        public P0RuntimeSettingsPresentation(
            bool isPaused,
            float speedMultiplier,
            string statusLabel,
            string speedLabel,
            string pauseButtonLabel,
            string pauseShortcutLabel,
            string speedShortcutLabel,
            IReadOnlyList<P0RuntimeSettingsAction> actions)
        {
            IsPaused = isPaused;
            SpeedMultiplier = speedMultiplier;
            StatusLabel = statusLabel ?? string.Empty;
            SpeedLabel = speedLabel ?? string.Empty;
            PauseButtonLabel = pauseButtonLabel ?? string.Empty;
            PauseShortcutLabel = pauseShortcutLabel ?? string.Empty;
            SpeedShortcutLabel = speedShortcutLabel ?? string.Empty;
            Actions = actions ?? Array.Empty<P0RuntimeSettingsAction>();
        }

        public bool IsPaused { get; }

        public float SpeedMultiplier { get; }

        public string StatusLabel { get; }

        public string SpeedLabel { get; }

        public string PauseButtonLabel { get; }

        public string PauseShortcutLabel { get; }

        public string SpeedShortcutLabel { get; }

        public IReadOnlyList<P0RuntimeSettingsAction> Actions { get; }

        public bool TryGetAction(string actionId, out P0RuntimeSettingsAction action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionId == actionId)
                {
                    action = Actions[i];
                    return true;
                }
            }

            action = default(P0RuntimeSettingsAction);
            return false;
        }

        public string BuildSummary()
        {
            return "运行设置："
                + StatusLabel
                + " 速度 "
                + SpeedLabel
                + " 暂停键 "
                + PauseShortcutLabel
                + " 速度键 "
                + SpeedShortcutLabel
                + " 操作 "
                + Actions.Count;
        }
    }

    public static class P0RuntimeSettingsPresenter
    {
        public static P0RuntimeSettingsPresentation Build(P0RuntimeSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            return new P0RuntimeSettingsPresentation(
                settings.IsPaused,
                settings.BattleSpeedMultiplier,
                settings.IsPaused ? "已暂停" : "实时",
                FormatSpeed(settings.BattleSpeedMultiplier),
                settings.IsPaused ? "继续" : "暂停",
                GetInputLabel(P0InputCommand.TogglePause),
                GetInputLabel(P0InputCommand.SpeedHalf)
                    + "/"
                    + GetInputLabel(P0InputCommand.SpeedNormal)
                    + "/"
                    + GetInputLabel(P0InputCommand.SpeedFast),
                BuildActions(settings));
        }

        public static string FormatSpeed(float speedMultiplier)
        {
            return speedMultiplier.ToString("0.##") + " 倍";
        }

        public static bool HasP0RuntimeSettingsSurface(P0RuntimeSettingsPresentation presentation)
        {
            if (string.IsNullOrWhiteSpace(presentation.StatusLabel)
                || string.IsNullOrWhiteSpace(presentation.SpeedLabel)
                || presentation.Actions.Count < 4)
            {
                return false;
            }

            if (!presentation.TryGetAction(P0RuntimeSettingsActionIds.TogglePause, out P0RuntimeSettingsAction pause)
                || pause.Command != P0InputCommand.TogglePause
                || !pause.IsEnabled
                || string.IsNullOrWhiteSpace(pause.ShortcutLabel))
            {
                return false;
            }

            return HasSpeedAction(
                    presentation,
                    P0RuntimeSettingsActionIds.SpeedHalf,
                    P0InputCommand.SpeedHalf,
                    0.5f)
                && HasSpeedAction(
                    presentation,
                    P0RuntimeSettingsActionIds.SpeedNormal,
                    P0InputCommand.SpeedNormal,
                    1f)
                && HasSpeedAction(
                    presentation,
                    P0RuntimeSettingsActionIds.SpeedFast,
                    P0InputCommand.SpeedFast,
                    1.5f)
                && CountCurrentSpeedActions(presentation) == 1;
        }

        public static P0PauseSettingsSurface BuildPauseSettingsSurface(
            P0RuntimeSettings settings,
            bool isRestartConfirmationOpen)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            P0RuntimeSettingsPresentation runtime = Build(settings);
            List<P0RuntimeSettingsAction> actions = new List<P0RuntimeSettingsAction>(runtime.Actions);
            actions.Add(new P0RuntimeSettingsAction(
                P0RuntimeSettingsActionIds.RequestRestart,
                "请求重开",
                !isRestartConfirmationOpen,
                false,
                P0InputCommand.RestartRun,
                settings.BattleSpeedMultiplier,
                GetInputLabel(P0InputCommand.RestartRun),
                "打开重开确认，不直接结束本轮",
                requiresConfirmation: true));
            actions.Add(new P0RuntimeSettingsAction(
                P0RuntimeSettingsActionIds.ConfirmRestart,
                "确认重开",
                isRestartConfirmationOpen,
                false,
                P0InputCommand.RestartRun,
                settings.BattleSpeedMultiplier,
                GetInputLabel(P0InputCommand.RestartRun),
                "确认后才开始新一轮",
                requiresConfirmation: false));

            return new P0PauseSettingsSurface(
                "暂停 / 设置",
                "战斗可暂停，速度可调整；重开需要二次确认。",
                isRestartConfirmationOpen,
                "确认重开本轮？",
                "当前战斗进度会被放弃；再次确认才会开始新一轮。",
                runtime,
                new[]
                {
                    "战斗节奏：暂停 / 继续",
                    "速度：0.5x / 1x / 1.5x",
                    "重开：先确认，再执行"
                },
                new[]
                {
                    "P/Esc 暂停或继续",
                    "F1/F2/F3 调整速度",
                    "N 打开或确认重开"
                },
                actions.AsReadOnly());
        }

        public static P0FullSettingsSurface BuildFullSettingsSurface(
            P0RuntimeSettings settings,
            string activeTabId,
            bool isRestartConfirmationOpen)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            string selectedTab = string.IsNullOrWhiteSpace(activeTabId)
                ? P0FullSettingsIds.CombatTab
                : activeTabId;
            P0PauseSettingsSurface pause = BuildPauseSettingsSurface(settings, isRestartConfirmationOpen);
            return new P0FullSettingsSurface(
                "设置",
                "只显示当前已实现的战斗暂停、速度、按键和重开确认。",
                selectedTab,
                pause,
                BuildFullSettingsTabs(selectedTab),
                BuildFullSettingsOptionRows(pause),
                isRestartConfirmationOpen ? pause.RestartConfirmationTitle : string.Empty,
                isRestartConfirmationOpen ? pause.RestartConfirmationDetail : string.Empty);
        }

        public static bool HasP0FullSettingsSurface(P0FullSettingsSurface surface)
        {
            if (string.IsNullOrWhiteSpace(surface.TitleLabel)
                || string.IsNullOrWhiteSpace(surface.SubtitleLabel)
                || string.IsNullOrWhiteSpace(surface.ActiveTabId)
                || surface.Tabs.Count < 2
                || surface.OptionRows.Count < 6
                || CountSelectedTabs(surface) != 1
                || !P0RuntimeSettingsPresenter.HasP0PauseSettingsSurface(surface.PauseSurface))
            {
                return false;
            }

            return HasOptionRow(surface, P0FullSettingsIds.PauseRow, "toggle")
                && HasOptionRow(surface, P0FullSettingsIds.SpeedHalfRow, "stepper")
                && HasOptionRow(surface, P0FullSettingsIds.SpeedNormalRow, "stepper")
                && HasOptionRow(surface, P0FullSettingsIds.SpeedFastRow, "stepper")
                && HasOptionRow(surface, P0FullSettingsIds.RestartRow, "button")
                && HasOptionRow(surface, P0FullSettingsIds.ConfirmRestartRow, "modal_button")
                && !ContainsForbiddenCandidateText(surface);
        }

        public static bool HasP0PauseSettingsSurface(P0PauseSettingsSurface surface)
        {
            if (string.IsNullOrWhiteSpace(surface.TitleLabel)
                || string.IsNullOrWhiteSpace(surface.SubtitleLabel)
                || surface.Sections.Count < 3
                || surface.KeyHints.Count < 3
                || !HasP0RuntimeSettingsSurface(surface.RuntimeSettings))
            {
                return false;
            }

            return surface.TryGetAction(P0RuntimeSettingsActionIds.RequestRestart, out P0RuntimeSettingsAction requestRestart)
                && requestRestart.Command == P0InputCommand.RestartRun
                && requestRestart.RequiresConfirmation
                && surface.TryGetAction(P0RuntimeSettingsActionIds.ConfirmRestart, out P0RuntimeSettingsAction confirmRestart)
                && confirmRestart.Command == P0InputCommand.RestartRun
                && confirmRestart.IsEnabled == surface.IsRestartConfirmationOpen
                && !confirmRestart.RequiresConfirmation
                && KeyHintsContain(surface, "P/Esc")
                && KeyHintsContain(surface, "F1/F2/F3")
                && KeyHintsContain(surface, "N");
        }

        public static string BuildCompactSummary(P0RuntimeSettingsPresentation presentation)
        {
            int enabled = 0;
            int current = 0;
            for (int i = 0; i < presentation.Actions.Count; i++)
            {
                enabled += presentation.Actions[i].IsEnabled ? 1 : 0;
                current += presentation.Actions[i].IsCurrent ? 1 : 0;
            }

            return "运行设置界面："
                + presentation.StatusLabel
                + " 速度 "
                + presentation.SpeedLabel
                + " 操作 "
                + presentation.Actions.Count
                + " 可用 "
                + enabled
                + " 当前 "
                + current
                + " 暂停键 "
                + presentation.PauseShortcutLabel
                + " 速度键 "
                + presentation.SpeedShortcutLabel;
        }

        private static IReadOnlyList<P0RuntimeSettingsAction> BuildActions(P0RuntimeSettings settings)
        {
            return new[]
            {
                new P0RuntimeSettingsAction(
                    P0RuntimeSettingsActionIds.TogglePause,
                    settings.IsPaused ? "继续" : "暂停",
                    true,
                    false,
                    P0InputCommand.TogglePause,
                    settings.BattleSpeedMultiplier,
                    GetInputLabel(P0InputCommand.TogglePause),
                    settings.IsPaused ? "继续战斗更新" : "暂停战斗更新"),
                BuildSpeedAction(
                    P0RuntimeSettingsActionIds.SpeedHalf,
                    P0InputCommand.SpeedHalf,
                    0.5f,
                    settings.BattleSpeedMultiplier),
                BuildSpeedAction(
                    P0RuntimeSettingsActionIds.SpeedNormal,
                    P0InputCommand.SpeedNormal,
                    1f,
                    settings.BattleSpeedMultiplier),
                BuildSpeedAction(
                    P0RuntimeSettingsActionIds.SpeedFast,
                    P0InputCommand.SpeedFast,
                    1.5f,
                    settings.BattleSpeedMultiplier)
            };
        }

        private static IReadOnlyList<P0FullSettingsTab> BuildFullSettingsTabs(string selectedTab)
        {
            return new[]
            {
                new P0FullSettingsTab(
                    P0FullSettingsIds.CombatTab,
                    "战斗",
                    selectedTab == P0FullSettingsIds.CombatTab,
                    "暂停、速度和当前战斗节奏。"),
                new P0FullSettingsTab(
                    P0FullSettingsIds.ControlsTab,
                    "按键",
                    selectedTab == P0FullSettingsIds.ControlsTab,
                    "P/Esc、F1/F2/F3、N 与按钮保持一致。")
            };
        }

        private static IReadOnlyList<P0FullSettingsOptionRow> BuildFullSettingsOptionRows(P0PauseSettingsSurface surface)
        {
            List<P0FullSettingsOptionRow> rows = new List<P0FullSettingsOptionRow>();
            AddActionRow(rows, surface, P0RuntimeSettingsActionIds.TogglePause, P0FullSettingsIds.PauseRow, "toggle");
            AddActionRow(rows, surface, P0RuntimeSettingsActionIds.SpeedHalf, P0FullSettingsIds.SpeedHalfRow, "stepper");
            AddActionRow(rows, surface, P0RuntimeSettingsActionIds.SpeedNormal, P0FullSettingsIds.SpeedNormalRow, "stepper");
            AddActionRow(rows, surface, P0RuntimeSettingsActionIds.SpeedFast, P0FullSettingsIds.SpeedFastRow, "stepper");
            AddActionRow(rows, surface, P0RuntimeSettingsActionIds.RequestRestart, P0FullSettingsIds.RestartRow, "button");
            AddActionRow(rows, surface, P0RuntimeSettingsActionIds.ConfirmRestart, P0FullSettingsIds.ConfirmRestartRow, "modal_button");
            return rows.AsReadOnly();
        }

        private static void AddActionRow(
            List<P0FullSettingsOptionRow> rows,
            P0PauseSettingsSurface surface,
            string actionId,
            string rowId,
            string controlKind)
        {
            if (!surface.TryGetAction(actionId, out P0RuntimeSettingsAction action))
            {
                return;
            }

            rows.Add(new P0FullSettingsOptionRow(
                rowId,
                action.Label,
                BuildFullSettingsRowValue(surface, action),
                controlKind,
                action.ShortcutLabel,
                action.IsEnabled,
                action.IsCurrent,
                action.Detail));
        }

        private static bool HasOptionRow(P0FullSettingsSurface surface, string rowId, string controlKind)
        {
            return surface.TryGetOptionRow(rowId, out P0FullSettingsOptionRow row)
                && row.ControlKind == controlKind
                && !string.IsNullOrWhiteSpace(row.Label)
                && !string.IsNullOrWhiteSpace(row.ShortcutLabel);
        }

        private static int CountSelectedTabs(P0FullSettingsSurface surface)
        {
            int count = 0;
            for (int i = 0; i < surface.Tabs.Count; i++)
            {
                count += surface.Tabs[i].IsSelected ? 1 : 0;
            }

            return count;
        }

        private static string BuildFullSettingsRowValue(P0PauseSettingsSurface surface, P0RuntimeSettingsAction action)
        {
            if (IsSpeedCommand(action.Command))
            {
                return FormatSpeed(action.TargetSpeedMultiplier);
            }

            if (action.Command == P0InputCommand.TogglePause)
            {
                return surface.RuntimeSettings.StatusLabel;
            }

            if (action.ActionId == P0RuntimeSettingsActionIds.RequestRestart)
            {
                return action.RequiresConfirmation ? "needs-confirm" : "ready";
            }

            if (action.ActionId == P0RuntimeSettingsActionIds.ConfirmRestart)
            {
                return action.IsEnabled ? "confirm-open" : "confirm-locked";
            }

            return action.IsEnabled ? "enabled" : "disabled";
        }

        private static bool ContainsForbiddenCandidateText(P0FullSettingsSurface surface)
        {
            if (ContainsForbiddenCandidateText(surface.TitleLabel)
                || ContainsForbiddenCandidateText(surface.SubtitleLabel)
                || ContainsForbiddenCandidateText(surface.ActiveTabId)
                || ContainsForbiddenCandidateText(surface.ModalTitle)
                || ContainsForbiddenCandidateText(surface.ModalDetail)
                || ContainsForbiddenCandidateText(surface.BuildSummary())
                || ContainsForbiddenCandidateText(surface.PauseSurface))
            {
                return true;
            }

            for (int i = 0; i < surface.Tabs.Count; i++)
            {
                if (ContainsForbiddenCandidateText(surface.Tabs[i]))
                {
                    return true;
                }
            }

            for (int i = 0; i < surface.OptionRows.Count; i++)
            {
                if (ContainsForbiddenCandidateText(surface.OptionRows[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsForbiddenCandidateText(P0FullSettingsTab tab)
        {
            return ContainsForbiddenCandidateText(tab.TabId)
                || ContainsForbiddenCandidateText(tab.Label)
                || ContainsForbiddenCandidateText(tab.Detail);
        }

        private static bool ContainsForbiddenCandidateText(P0FullSettingsOptionRow row)
        {
            return ContainsForbiddenCandidateText(row.RowId)
                || ContainsForbiddenCandidateText(row.Label)
                || ContainsForbiddenCandidateText(row.ValueLabel)
                || ContainsForbiddenCandidateText(row.ControlKind)
                || ContainsForbiddenCandidateText(row.ShortcutLabel)
                || ContainsForbiddenCandidateText(row.Detail)
                || ContainsForbiddenCandidateText(row.BuildSummary());
        }

        private static bool ContainsForbiddenCandidateText(P0PauseSettingsSurface surface)
        {
            if (ContainsForbiddenCandidateText(surface.TitleLabel)
                || ContainsForbiddenCandidateText(surface.SubtitleLabel)
                || ContainsForbiddenCandidateText(surface.RestartConfirmationTitle)
                || ContainsForbiddenCandidateText(surface.RestartConfirmationDetail)
                || ContainsForbiddenCandidateText(surface.BuildSummary())
                || ContainsForbiddenCandidateText(surface.RuntimeSettings))
            {
                return true;
            }

            for (int i = 0; i < surface.Sections.Count; i++)
            {
                if (ContainsForbiddenCandidateText(surface.Sections[i]))
                {
                    return true;
                }
            }

            for (int i = 0; i < surface.KeyHints.Count; i++)
            {
                if (ContainsForbiddenCandidateText(surface.KeyHints[i]))
                {
                    return true;
                }
            }

            for (int i = 0; i < surface.Actions.Count; i++)
            {
                if (ContainsForbiddenCandidateText(surface.Actions[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsForbiddenCandidateText(P0RuntimeSettingsPresentation presentation)
        {
            if (ContainsForbiddenCandidateText(presentation.StatusLabel)
                || ContainsForbiddenCandidateText(presentation.SpeedLabel)
                || ContainsForbiddenCandidateText(presentation.PauseButtonLabel)
                || ContainsForbiddenCandidateText(presentation.PauseShortcutLabel)
                || ContainsForbiddenCandidateText(presentation.SpeedShortcutLabel)
                || ContainsForbiddenCandidateText(presentation.BuildSummary()))
            {
                return true;
            }

            for (int i = 0; i < presentation.Actions.Count; i++)
            {
                if (ContainsForbiddenCandidateText(presentation.Actions[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsForbiddenCandidateText(P0RuntimeSettingsAction action)
        {
            return ContainsForbiddenCandidateText(action.ActionId)
                || ContainsForbiddenCandidateText(action.Label)
                || ContainsForbiddenCandidateText(action.ShortcutLabel)
                || ContainsForbiddenCandidateText(action.Detail)
                || ContainsForbiddenCandidateText(action.BuildSummary());
        }

        private static bool ContainsForbiddenCandidateText(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.IndexOf(".png", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf(".meta", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("Assets/", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("Assets\\", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("asset_candidates", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("candidate_v", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("batch_85", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("batch85", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("settings_pause_preflight", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static P0RuntimeSettingsAction BuildSpeedAction(
            string actionId,
            P0InputCommand command,
            float targetSpeed,
            float currentSpeed)
        {
            bool isCurrent = Math.Abs(targetSpeed - currentSpeed) < 0.001f;
            return new P0RuntimeSettingsAction(
                actionId,
                FormatSpeed(targetSpeed),
                !isCurrent,
                isCurrent,
                command,
                targetSpeed,
                GetInputLabel(command),
                isCurrent ? "当前战斗速度" : "设置战斗速度");
        }

        private static bool HasSpeedAction(
            P0RuntimeSettingsPresentation presentation,
            string actionId,
            P0InputCommand command,
            float targetSpeed)
        {
            return presentation.TryGetAction(actionId, out P0RuntimeSettingsAction action)
                && action.Command == command
                && Math.Abs(action.TargetSpeedMultiplier - targetSpeed) < 0.001f
                && !string.IsNullOrWhiteSpace(action.Label)
                && !string.IsNullOrWhiteSpace(action.ShortcutLabel);
        }

        private static bool IsSpeedCommand(P0InputCommand command)
        {
            return command == P0InputCommand.SpeedHalf
                || command == P0InputCommand.SpeedNormal
                || command == P0InputCommand.SpeedFast;
        }

        private static int CountCurrentSpeedActions(P0RuntimeSettingsPresentation presentation)
        {
            int count = 0;
            for (int i = 0; i < presentation.Actions.Count; i++)
            {
                P0RuntimeSettingsAction action = presentation.Actions[i];
                if (IsSpeedCommand(action.Command))
                {
                    count += action.IsCurrent ? 1 : 0;
                }
            }

            return count;
        }

        private static string GetInputLabel(P0InputCommand command)
        {
            if (P0KeyboardInputMap.TryGetBinding(command, out P0InputBinding binding))
            {
                return binding.HasSecondaryKey
                    ? binding.PrimaryKeyLabel + "/" + binding.SecondaryKeyLabel
                    : binding.PrimaryKeyLabel;
            }

            switch (command)
            {
                case P0InputCommand.TogglePause:
                    return "P/Esc";
                case P0InputCommand.SpeedHalf:
                    return "F1";
                case P0InputCommand.SpeedNormal:
                    return "F2";
                case P0InputCommand.SpeedFast:
                    return "F3";
                case P0InputCommand.RestartRun:
                    return "N";
                default:
                    return "?";
            }
        }

        private static bool KeyHintsContain(P0PauseSettingsSurface surface, string value)
        {
            for (int i = 0; i < surface.KeyHints.Count; i++)
            {
                if ((surface.KeyHints[i] ?? string.Empty).Contains(value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
