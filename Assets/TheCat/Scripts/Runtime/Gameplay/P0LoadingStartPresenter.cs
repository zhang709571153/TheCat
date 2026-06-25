using System;
using System.Collections.Generic;

namespace TheCat.Gameplay
{
    public readonly struct P0LoadingStartSurface
    {
        public P0LoadingStartSurface(
            string titleLabel,
            string targetSceneName,
            string targetLabel,
            string stateLabel,
            float progress01,
            string progressLabel,
            string spinnerLabel,
            IReadOnlyList<string> detailRows,
            bool hasScreenshotHook)
        {
            TitleLabel = titleLabel ?? string.Empty;
            TargetSceneName = targetSceneName ?? string.Empty;
            TargetLabel = targetLabel ?? string.Empty;
            StateLabel = stateLabel ?? string.Empty;
            Progress01 = Clamp01(progress01);
            ProgressLabel = progressLabel ?? string.Empty;
            SpinnerLabel = spinnerLabel ?? string.Empty;
            DetailRows = detailRows ?? Array.Empty<string>();
            HasScreenshotHook = hasScreenshotHook;
        }

        public string TitleLabel { get; }

        public string TargetSceneName { get; }

        public string TargetLabel { get; }

        public string StateLabel { get; }

        public float Progress01 { get; }

        public string ProgressLabel { get; }

        public string SpinnerLabel { get; }

        public IReadOnlyList<string> DetailRows { get; }

        public bool HasScreenshotHook { get; }

        public string BuildStatusLine()
        {
            return TitleLabel
                + " -> "
                + TargetLabel
                + " "
                + ProgressLabel
                + " "
                + StateLabel;
        }

        public string BuildSummary()
        {
            return "Loading start surface target "
                + TargetSceneName
                + " progress "
                + ProgressLabel
                + " spinner "
                + SpinnerLabel
                + " rows "
                + DetailRows.Count
                + " screenshotHook "
                + HasScreenshotHook;
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

    public static class P0LoadingStartPresenter
    {
        public static P0LoadingStartSurface BuildRunStartSurface(
            P0RunStartMode startMode,
            float progress01)
        {
            string targetSceneName = P0SceneFlow.GetStartSceneName(startMode);
            return BuildSceneTransitionSurface(
                targetSceneName,
                BuildTargetLabel(startMode, targetSceneName),
                progress01,
                hasScreenshotHook: true);
        }

        public static P0LoadingStartSurface BuildSceneTransitionSurface(
            string targetSceneName,
            string targetLabel,
            float progress01,
            bool hasScreenshotHook)
        {
            float progress = Clamp01(progress01);
            return new P0LoadingStartSurface(
                "加载 / 开始",
                targetSceneName,
                targetLabel,
                progress >= 1f ? "准备完成" : "正在准备梦境入口",
                progress,
                FormatProgress(progress),
                "月牙旋转",
                new[]
                {
                    "保持当前猫队选择",
                    "进入前不导入候选素材",
                    "截图钩子用于 Batch 83 验证"
                },
                hasScreenshotHook);
        }

        public static bool HasP0LoadingStartSurface(P0LoadingStartSurface surface)
        {
            if (string.IsNullOrWhiteSpace(surface.TitleLabel)
                || string.IsNullOrWhiteSpace(surface.TargetSceneName)
                || string.IsNullOrWhiteSpace(surface.TargetLabel)
                || string.IsNullOrWhiteSpace(surface.StateLabel)
                || string.IsNullOrWhiteSpace(surface.ProgressLabel)
                || string.IsNullOrWhiteSpace(surface.SpinnerLabel)
                || surface.DetailRows.Count < 3
                || !surface.HasScreenshotHook)
            {
                return false;
            }

            return surface.Progress01 >= 0f
                && surface.Progress01 <= 1f
                && !ContainsForbiddenCandidateText(surface);
        }

        public static string BuildCompactSummary(P0LoadingStartSurface surface)
        {
            return surface.BuildSummary();
        }

        private static string BuildTargetLabel(P0RunStartMode startMode, string targetSceneName)
        {
            switch (startMode)
            {
                case P0RunStartMode.CatRoom:
                    return "猫房准备";
                case P0RunStartMode.RouteMap:
                    return "卧室梦境路线";
                case P0RunStartMode.QuickBattle:
                    return "卧室守床战斗";
                default:
                    return string.IsNullOrWhiteSpace(targetSceneName) ? "未知入口" : targetSceneName;
            }
        }

        private static string FormatProgress(float progress01)
        {
            int percent = (int)Math.Round(Clamp01(progress01) * 100f);
            return percent + "%";
        }

        private static bool ContainsForbiddenCandidateText(P0LoadingStartSurface surface)
        {
            if (ContainsForbiddenCandidateText(surface.TitleLabel)
                || ContainsForbiddenCandidateText(surface.TargetSceneName)
                || ContainsForbiddenCandidateText(surface.TargetLabel)
                || ContainsForbiddenCandidateText(surface.StateLabel)
                || ContainsForbiddenCandidateText(surface.ProgressLabel)
                || ContainsForbiddenCandidateText(surface.SpinnerLabel)
                || ContainsForbiddenCandidateText(surface.BuildStatusLine())
                || ContainsForbiddenCandidateText(surface.BuildSummary()))
            {
                return true;
            }

            for (int i = 0; i < surface.DetailRows.Count; i++)
            {
                if (ContainsForbiddenCandidateText(surface.DetailRows[i]))
                {
                    return true;
                }
            }

            return false;
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
                || value.IndexOf("batch_83", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("batch83", StringComparison.OrdinalIgnoreCase) >= 0
                || value.IndexOf("loading_start_preflight", StringComparison.OrdinalIgnoreCase) >= 0;
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
}
