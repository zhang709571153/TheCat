using System;
using System.Collections.Generic;

namespace TheCat.Gameplay
{
    public static class P0CatRoomHotspotIds
    {
        public const string Bed = "bed";
        public const string Feeder = "feeder";
        public const string LitterBox = "litter_box";
        public const string DreamEntrance = "dream_entrance";
    }

    public static class P0CatRoomActionIds
    {
        public const string UseBed = "cat_room_bed_feedback";
        public const string FeedCats = "cat_room_feeder_feedback";
        public const string CleanLitter = "cat_room_litter_feedback";
        public const string EnterDream = "enter_dream";
        public const string ReturnMainMenu = "return_main_menu";
    }

    public enum P0CatRoomReturnReason
    {
        FreshStart = 0,
        BattleVictory = 1,
        BattleDefeat = 2,
        RouteCleared = 3,
        RouteFailed = 4
    }

    public readonly struct P0CatRoomState
    {
        public P0CatRoomState(
            float ownerSleep01,
            float teamCatHealth01,
            float teamHunger01,
            float teamPoop01,
            int fishTreats,
            int dreamShards,
            bool hasActiveRun,
            bool dreamEntryUnlocked,
            P0CatRoomReturnReason returnReason,
            string returnDetail)
        {
            OwnerSleep01 = Clamp01(ownerSleep01);
            TeamCatHealth01 = Clamp01(teamCatHealth01);
            TeamHunger01 = Clamp01(teamHunger01);
            TeamPoop01 = Clamp01(teamPoop01);
            FishTreats = Math.Max(0, fishTreats);
            DreamShards = Math.Max(0, dreamShards);
            HasActiveRun = hasActiveRun;
            DreamEntryUnlocked = dreamEntryUnlocked;
            ReturnReason = returnReason;
            ReturnDetail = returnDetail ?? string.Empty;
        }

        public float OwnerSleep01 { get; }

        public float TeamCatHealth01 { get; }

        public float TeamHunger01 { get; }

        public float TeamPoop01 { get; }

        public int FishTreats { get; }

        public int DreamShards { get; }

        public bool HasActiveRun { get; }

        public bool DreamEntryUnlocked { get; }

        public P0CatRoomReturnReason ReturnReason { get; }

        public string ReturnDetail { get; }

        public static P0CatRoomState CreateFreshStart()
        {
            return new P0CatRoomState(1f, 1f, 0.85f, 0.1f, 0, 0, false, true, P0CatRoomReturnReason.FreshStart, string.Empty);
        }

        public static P0CatRoomState CreateReturn(
            P0CatRoomReturnReason returnReason,
            string returnDetail,
            int fishTreats = 0,
            int dreamShards = 0)
        {
            switch (returnReason)
            {
                case P0CatRoomReturnReason.BattleVictory:
                case P0CatRoomReturnReason.RouteCleared:
                    return new P0CatRoomState(0.78f, 0.86f, 0.65f, 0.25f, fishTreats, dreamShards, false, true, returnReason, returnDetail);
                case P0CatRoomReturnReason.BattleDefeat:
                case P0CatRoomReturnReason.RouteFailed:
                    return new P0CatRoomState(0.28f, 0.55f, 0.32f, 0.58f, fishTreats, dreamShards, false, true, returnReason, returnDetail);
                case P0CatRoomReturnReason.FreshStart:
                    return CreateFreshStart();
                default:
                    throw new ArgumentOutOfRangeException(nameof(returnReason), returnReason, "Unknown cat-room return reason.");
            }
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }

    public readonly struct P0CatRoomValueRow
    {
        public P0CatRoomValueRow(string valueId, string label, float value01, string statusLabel)
        {
            ValueId = valueId ?? string.Empty;
            Label = label ?? string.Empty;
            Value01 = Clamp01(value01);
            StatusLabel = statusLabel ?? string.Empty;
        }

        public string ValueId { get; }

        public string Label { get; }

        public float Value01 { get; }

        public string StatusLabel { get; }

        public string BuildSummary()
        {
            return Label + " " + FormatPercent(Value01) + " " + StatusLabel;
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }

        private static string FormatPercent(float value)
        {
            return ((int)Math.Round(value * 100f)) + "%";
        }
    }

    public readonly struct P0CatRoomHotspot
    {
        public P0CatRoomHotspot(
            string hotspotId,
            string actionId,
            string label,
            string feedbackLine,
            bool isEnabled,
            bool isFeedbackOnly)
        {
            HotspotId = hotspotId ?? string.Empty;
            ActionId = actionId ?? string.Empty;
            Label = label ?? string.Empty;
            FeedbackLine = feedbackLine ?? string.Empty;
            IsEnabled = isEnabled;
            IsFeedbackOnly = isFeedbackOnly;
        }

        public string HotspotId { get; }

        public string ActionId { get; }

        public string Label { get; }

        public string FeedbackLine { get; }

        public bool IsEnabled { get; }

        public bool IsFeedbackOnly { get; }

        public string BuildSummary()
        {
            return Label + " -> " + FeedbackLine;
        }
    }

    public readonly struct P0CatRoomResourceRow
    {
        public P0CatRoomResourceRow(string resourceId, string label, string valueLabel)
        {
            ResourceId = resourceId ?? string.Empty;
            Label = label ?? string.Empty;
            ValueLabel = valueLabel ?? string.Empty;
        }

        public string ResourceId { get; }

        public string Label { get; }

        public string ValueLabel { get; }

        public string BuildSummary()
        {
            return Label + " " + ValueLabel;
        }
    }

    public readonly struct P0CatRoomAction
    {
        public P0CatRoomAction(string actionId, string label, string targetSceneName, bool isEnabled, string detail)
        {
            ActionId = actionId ?? string.Empty;
            Label = label ?? string.Empty;
            TargetSceneName = targetSceneName ?? string.Empty;
            IsEnabled = isEnabled;
            Detail = detail ?? string.Empty;
        }

        public string ActionId { get; }

        public string Label { get; }

        public string TargetSceneName { get; }

        public bool IsEnabled { get; }

        public string Detail { get; }

        public string BuildButtonLabel()
        {
            return IsEnabled ? Label : Label + "（未解锁）";
        }
    }

    public sealed class P0CatRoomSurface
    {
        public P0CatRoomSurface(
            string title,
            string subtitle,
            string returnFeedbackLabel,
            P0UiShellSurface uiShell,
            IEnumerable<P0CatRoomValueRow> valueRows,
            IEnumerable<P0CatRoomResourceRow> resourceRows,
            IEnumerable<P0CatRoomHotspot> hotspots,
            IEnumerable<P0CatRoomAction> actions)
        {
            Title = title ?? string.Empty;
            Subtitle = subtitle ?? string.Empty;
            ReturnFeedbackLabel = returnFeedbackLabel ?? string.Empty;
            UiShell = uiShell ?? P0UiShellPresenter.BuildSurface();
            ValueRows = valueRows == null
                ? Array.Empty<P0CatRoomValueRow>()
                : new List<P0CatRoomValueRow>(valueRows).AsReadOnly();
            ResourceRows = resourceRows == null
                ? Array.Empty<P0CatRoomResourceRow>()
                : new List<P0CatRoomResourceRow>(resourceRows).AsReadOnly();
            Hotspots = hotspots == null
                ? Array.Empty<P0CatRoomHotspot>()
                : new List<P0CatRoomHotspot>(hotspots).AsReadOnly();
            Actions = actions == null
                ? Array.Empty<P0CatRoomAction>()
                : new List<P0CatRoomAction>(actions).AsReadOnly();
        }

        public string Title { get; }

        public string Subtitle { get; }

        public string ReturnFeedbackLabel { get; }

        public P0UiShellSurface UiShell { get; }

        public IReadOnlyList<P0CatRoomValueRow> ValueRows { get; }

        public IReadOnlyList<P0CatRoomResourceRow> ResourceRows { get; }

        public IReadOnlyList<P0CatRoomHotspot> Hotspots { get; }

        public IReadOnlyList<P0CatRoomAction> Actions { get; }

        public bool TryGetHotspot(string hotspotId, out P0CatRoomHotspot hotspot)
        {
            for (int i = 0; i < Hotspots.Count; i++)
            {
                if (Hotspots[i].HotspotId == hotspotId)
                {
                    hotspot = Hotspots[i];
                    return true;
                }
            }

            hotspot = default(P0CatRoomHotspot);
            return false;
        }

        public bool TryGetAction(string actionId, out P0CatRoomAction action)
        {
            for (int i = 0; i < Actions.Count; i++)
            {
                if (Actions[i].ActionId == actionId)
                {
                    action = Actions[i];
                    return true;
                }
            }

            action = default(P0CatRoomAction);
            return false;
        }
    }

    public static class P0CatRoomPresenter
    {
        public static P0CatRoomSurface BuildSurface(P0CatRoomState state)
        {
            return new P0CatRoomSurface(
                "猫房",
                "梦境前的准备间",
                BuildReturnFeedbackLabel(state),
                P0UiShellPresenter.BuildSurface(),
                BuildValueRows(state),
                BuildResourceRows(state),
                BuildHotspots(state),
                BuildActions(state));
        }

        public static bool HasP0CatRoomSurface(P0CatRoomSurface surface)
        {
            if (surface == null
                || string.IsNullOrWhiteSpace(surface.Title)
                || string.IsNullOrWhiteSpace(surface.Subtitle)
                || string.IsNullOrWhiteSpace(surface.ReturnFeedbackLabel)
                || !P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell)
                || !HasRequiredValueRows(surface))
            {
                return false;
            }

            return HasFeedbackHotspot(surface, P0CatRoomHotspotIds.Bed, P0CatRoomActionIds.UseBed)
                && HasFeedbackHotspot(surface, P0CatRoomHotspotIds.Feeder, P0CatRoomActionIds.FeedCats)
                && HasFeedbackHotspot(surface, P0CatRoomHotspotIds.LitterBox, P0CatRoomActionIds.CleanLitter)
                && surface.TryGetHotspot(P0CatRoomHotspotIds.DreamEntrance, out P0CatRoomHotspot dreamEntrance)
                && !dreamEntrance.IsFeedbackOnly
                && dreamEntrance.ActionId == P0CatRoomActionIds.EnterDream
                && surface.TryGetAction(P0CatRoomActionIds.EnterDream, out P0CatRoomAction enterDream)
                && dreamEntrance.IsEnabled == enterDream.IsEnabled
                && enterDream.TargetSceneName == P0SceneFlow.RouteMapSceneName
                && surface.TryGetAction(P0CatRoomActionIds.ReturnMainMenu, out P0CatRoomAction returnMainMenu)
                && returnMainMenu.IsEnabled
                && returnMainMenu.TargetSceneName == P0SceneFlow.MainMenuSceneName;
        }

        public static string BuildCompactSummary(P0CatRoomSurface surface)
        {
            if (surface == null)
            {
                return "猫房: missing";
            }

            return "猫房: values "
                + surface.ValueRows.Count
                + " resources "
                + surface.ResourceRows.Count
                + " hotspots "
                + surface.Hotspots.Count
                + " actions "
                + surface.Actions.Count
                + " feedback "
                + surface.ReturnFeedbackLabel;
        }

        private static IReadOnlyList<P0CatRoomValueRow> BuildValueRows(P0CatRoomState state)
        {
            return new[]
            {
                new P0CatRoomValueRow("owner_sleep", "主人睡眠", state.OwnerSleep01, BuildSleepStatus(state.OwnerSleep01)),
                new P0CatRoomValueRow("team_cat_health", "猫队HP", state.TeamCatHealth01, BuildCatStatus(state.TeamCatHealth01)),
                new P0CatRoomValueRow("team_hunger", "饱肚度", state.TeamHunger01, BuildFullnessStatus(state.TeamHunger01)),
                new P0CatRoomValueRow("team_poop", "便便压力", state.TeamPoop01, BuildPressureStatus(state.TeamPoop01))
            };
        }

        private static IReadOnlyList<P0CatRoomResourceRow> BuildResourceRows(P0CatRoomState state)
        {
            return new[]
            {
                new P0CatRoomResourceRow("fish_treats", "鱼干", state.FishTreats.ToString()),
                new P0CatRoomResourceRow("dream_shards", "梦屑", state.DreamShards.ToString()),
                new P0CatRoomResourceRow("active_run", "梦境进度", state.HasActiveRun ? "进行中" : "待开始")
            };
        }

        private static IReadOnlyList<P0CatRoomHotspot> BuildHotspots(P0CatRoomState state)
        {
            return new[]
            {
                new P0CatRoomHotspot(P0CatRoomHotspotIds.Bed, P0CatRoomActionIds.UseBed, "床铺", BuildBedFeedback(state), true, true),
                new P0CatRoomHotspot(P0CatRoomHotspotIds.Feeder, P0CatRoomActionIds.FeedCats, "食盆", BuildFeederFeedback(state), true, true),
                new P0CatRoomHotspot(P0CatRoomHotspotIds.LitterBox, P0CatRoomActionIds.CleanLitter, "猫砂盆", BuildLitterFeedback(state), true, true),
                new P0CatRoomHotspot(P0CatRoomHotspotIds.DreamEntrance, P0CatRoomActionIds.EnterDream, "梦境入口", "卧室梦境可进入；埃及梦境仍在占位验证。", state.DreamEntryUnlocked, false)
            };
        }

        private static IReadOnlyList<P0CatRoomAction> BuildActions(P0CatRoomState state)
        {
            return new[]
            {
                new P0CatRoomAction(P0CatRoomActionIds.EnterDream, "进入卧室梦境", P0SceneFlow.RouteMapSceneName, state.DreamEntryUnlocked, "守护中心床，进入当前Demo可玩路线"),
                new P0CatRoomAction(P0CatRoomActionIds.ReturnMainMenu, "返回标题", P0SceneFlow.MainMenuSceneName, true, "回到主菜单")
            };
        }

        private static bool HasFeedbackHotspot(P0CatRoomSurface surface, string hotspotId, string actionId)
        {
            return surface.TryGetHotspot(hotspotId, out P0CatRoomHotspot hotspot)
                && hotspot.IsEnabled
                && hotspot.IsFeedbackOnly
                && hotspot.ActionId == actionId
                && !string.IsNullOrWhiteSpace(hotspot.FeedbackLine);
        }

        private static bool HasRequiredValueRows(P0CatRoomSurface surface)
        {
            return HasValueRow(surface, "owner_sleep")
                && HasValueRow(surface, "team_cat_health")
                && HasValueRow(surface, "team_hunger")
                && HasValueRow(surface, "team_poop");
        }

        private static bool HasValueRow(P0CatRoomSurface surface, string valueId)
        {
            for (int i = 0; i < surface.ValueRows.Count; i++)
            {
                if (surface.ValueRows[i].ValueId == valueId
                    && !string.IsNullOrWhiteSpace(surface.ValueRows[i].Label)
                    && !string.IsNullOrWhiteSpace(surface.ValueRows[i].StatusLabel))
                {
                    return true;
                }
            }

            return false;
        }

        private static string BuildReturnFeedbackLabel(P0CatRoomState state)
        {
            string label;
            switch (state.ReturnReason)
            {
                case P0CatRoomReturnReason.BattleVictory:
                    label = "梦境胜利，猫房记录了新的成长。";
                    break;
                case P0CatRoomReturnReason.BattleDefeat:
                    label = "梦境失败，先回猫房整理状态。";
                    break;
                case P0CatRoomReturnReason.RouteCleared:
                    label = "本轮梦境完成，奖励回收到猫房。";
                    break;
                case P0CatRoomReturnReason.RouteFailed:
                    label = "本轮梦境中断，猫房保留复盘提示。";
                    break;
                case P0CatRoomReturnReason.FreshStart:
                    label = "摸摸猫头，准备进入今晚的梦境。";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state.ReturnReason), state.ReturnReason, "Unknown cat-room return reason.");
            }

            return string.IsNullOrWhiteSpace(state.ReturnDetail) ? label : label + " " + state.ReturnDetail;
        }

        private static string BuildBedFeedback(P0CatRoomState state)
        {
            return state.OwnerSleep01 < 0.35f ? "床铺有些凌乱，今晚要先稳住主人的睡眠。" : "床铺已经整理好，可以承接下一场梦。";
        }

        private static string BuildFeederFeedback(P0CatRoomState state)
        {
            return state.TeamHunger01 < 0.4f ? "食盆提醒猫队先补充一点鱼干。" : "食盆状态稳定，猫队可以出发。";
        }

        private static string BuildLitterFeedback(P0CatRoomState state)
        {
            return state.TeamPoop01 > 0.55f ? "猫砂盆需要清理，避免下一场压力堆高。" : "猫砂盆已经清理，房间保持安静。";
        }

        private static string BuildSleepStatus(float value)
        {
            return value < 0.35f ? "危险" : value < 0.7f ? "偏低" : "稳定";
        }

        private static string BuildCatStatus(float value)
        {
            return value < 0.4f ? "需要照看" : value < 0.75f ? "可继续" : "精神很好";
        }

        private static string BuildPressureStatus(float value)
        {
            return value > 0.7f ? "偏高" : value > 0.35f ? "注意" : "稳定";
        }

        private static string BuildFullnessStatus(float value)
        {
            return value < 0.35f ? "偏低" : value < 0.7f ? "注意" : "充足";
        }
    }
}
