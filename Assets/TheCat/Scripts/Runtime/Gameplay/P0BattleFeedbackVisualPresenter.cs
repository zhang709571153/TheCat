using System;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public readonly struct P0BattleFeedbackColor
    {
        public P0BattleFeedbackColor(float r, float g, float b, float a)
        {
            R = Clamp01(r);
            G = Clamp01(g);
            B = Clamp01(b);
            A = Clamp01(a);
        }

        public float R { get; }

        public float G { get; }

        public float B { get; }

        public float A { get; }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }

    public readonly struct P0BattleFeedbackVisualState
    {
        public P0BattleFeedbackVisualState(
            P0BattleFeedback feedback,
            bool hasVisual,
            string accentToken,
            P0BattleFeedbackColor accentColor,
            P0BattleFeedbackColor backgroundColor,
            P0BattleFeedbackColor textColor,
            P0VisualAssetReference visualAsset,
            float durationSeconds,
            float ageSeconds,
            float remainingSeconds,
            float progress01,
            float pulseFill01,
            float pulseAlpha)
        {
            Feedback = feedback;
            HasVisual = hasVisual;
            AccentToken = accentToken ?? string.Empty;
            AccentColor = accentColor;
            BackgroundColor = backgroundColor;
            TextColor = textColor;
            VisualAsset = visualAsset;
            DurationSeconds = Math.Max(0f, durationSeconds);
            AgeSeconds = Math.Max(0f, ageSeconds);
            RemainingSeconds = Math.Max(0f, remainingSeconds);
            Progress01 = Clamp01(progress01);
            PulseFill01 = Clamp01(pulseFill01);
            PulseAlpha = Clamp01(pulseAlpha);
        }

        public P0BattleFeedback Feedback { get; }

        public bool HasVisual { get; }

        public string AccentToken { get; }

        public P0BattleFeedbackColor AccentColor { get; }

        public P0BattleFeedbackColor BackgroundColor { get; }

        public P0BattleFeedbackColor TextColor { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public float DurationSeconds { get; }

        public float AgeSeconds { get; }

        public float RemainingSeconds { get; }

        public float Progress01 { get; }

        public float PulseFill01 { get; }

        public float PulseAlpha { get; }

        public string BuildTitleLabel()
        {
            if (!HasVisual)
            {
                return "反馈";
            }

            return FormatLevel(Feedback.Level)
                + " "
                + FormatKind(Feedback.Kind)
                + " - "
                + Feedback.Title;
        }

        public string BuildSummary()
        {
            if (!HasVisual)
            {
                return "反馈视觉：无";
            }

            return "反馈视觉："
                + FormatLevel(Feedback.Level)
                + " "
                + FormatKind(Feedback.Kind)
                + " 强调 "
                + FormatAccentToken(AccentToken)
                + " 资产 "
                + (VisualAsset.HasAsset ? VisualAsset.AssetId : "none")
                + " 进度 "
                + Progress01.ToString("0.00")
                + " 剩余 "
                + RemainingSeconds.ToString("0.00")
                + "s 填充 "
                + PulseFill01.ToString("0.00")
                + " alpha "
                + PulseAlpha.ToString("0.00");
        }

        private static string FormatKind(P0BattleFeedbackKind kind)
        {
            switch (kind)
            {
                case P0BattleFeedbackKind.SkillCast:
                    return "技能释放";
                case P0BattleFeedbackKind.SkillBlocked:
                    return "技能受阻";
                case P0BattleFeedbackKind.InteractionSuccess:
                    return "交互成功";
                case P0BattleFeedbackKind.InteractionBlocked:
                    return "交互受阻";
                case P0BattleFeedbackKind.CatSwitch:
                    return "猫咪切换";
                case P0BattleFeedbackKind.CatPressure:
                    return "猫咪受压";
                case P0BattleFeedbackKind.CatWeak:
                    return "猫咪虚弱";
                case P0BattleFeedbackKind.RuntimeSettings:
                    return "运行设置";
                case P0BattleFeedbackKind.BattleResult:
                    return "战斗结果";
                case P0BattleFeedbackKind.None:
                default:
                    return "无";
            }
        }

        private static string FormatLevel(P0BattleFeedbackLevel level)
        {
            switch (level)
            {
                case P0BattleFeedbackLevel.Positive:
                    return "正向";
                case P0BattleFeedbackLevel.Warning:
                    return "警告";
                case P0BattleFeedbackLevel.Critical:
                    return "危急";
                case P0BattleFeedbackLevel.Result:
                    return "结果";
                case P0BattleFeedbackLevel.Info:
                default:
                    return "提示";
            }
        }

        private static string FormatAccentToken(string token)
        {
            switch (token)
            {
                case "green":
                    return "绿色";
                case "amber":
                    return "琥珀";
                case "red":
                    return "红色";
                case "violet":
                    return "紫色";
                case "blue":
                default:
                    return "蓝色";
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

    public static class P0BattleFeedbackVisualPresenter
    {
        private static readonly P0BattleFeedbackColor Text = new P0BattleFeedbackColor(0.96f, 0.97f, 1f, 1f);

        public static P0BattleFeedbackVisualState Build(P0BattleFeedback feedback, float ageSeconds)
        {
            if (!feedback.HasFeedback)
            {
                return default(P0BattleFeedbackVisualState);
            }

            float safeAge = Math.Max(0f, ageSeconds);
            float duration = Math.Max(0.01f, feedback.PulseSeconds);
            float progress = Clamp01(safeAge / duration);
            float pulseFill = 1f - progress;
            float pulseAlpha = pulseFill * feedback.Intensity;
            P0BattleFeedbackColor accent = GetAccentColor(feedback.Level);
            P0BattleFeedbackColor background = BuildBackgroundColor(accent, pulseAlpha);

            return new P0BattleFeedbackVisualState(
                feedback,
                hasVisual: true,
                GetAccentToken(feedback.Level),
                accent,
                background,
                Text,
                GetVisualAsset(feedback),
                feedback.PulseSeconds,
                safeAge,
                Math.Max(0f, feedback.PulseSeconds - safeAge),
                progress,
                pulseFill,
                pulseAlpha);
        }

        private static P0VisualAssetReference GetVisualAsset(P0BattleFeedback feedback)
        {
            switch (feedback.Kind)
            {
                case P0BattleFeedbackKind.SkillCast:
                    P0VisualAssetReference starterSkillVfx = GetStarterSkillVisualAsset(feedback.Title);
                    if (starterSkillVfx.HasAsset)
                    {
                        return starterSkillVfx;
                    }

                    if (ContainsAnyToken(feedback.Detail, "床护盾", "护盾", "bed shield", "shield"))
                    {
                        return P0VisualAssetCatalog.GetBedShieldPulseVfx();
                    }

                    if (ContainsAnyToken(feedback.Detail, "睡眠 +", "sleep +"))
                    {
                        return P0VisualAssetCatalog.GetSleepStableWaveVfx();
                    }

                    if (ContainsAnyToken(feedback.Detail, "状态", "目标", "status", "target"))
                    {
                        return P0VisualAssetCatalog.GetEnemyMarkRingVfx();
                    }

                    return P0VisualAssetCatalog.GetHitSparkVfx();
                case P0BattleFeedbackKind.InteractionSuccess:
                    if (ContainsAnyToken(feedback.Title, "猫砂盆", "Litter"))
                    {
                        return P0VisualAssetCatalog.GetLitterCleanseVfx();
                    }

                    if (ContainsAnyToken(feedback.Title, "喂食器", "Feeder"))
                    {
                        return P0VisualAssetCatalog.GetFeederKibbleVfx();
                    }

                    if (ContainsAnyToken(feedback.Title, "床", "Bed") || ContainsAnyToken(feedback.Detail, "睡眠 +", "sleep +"))
                    {
                        return P0VisualAssetCatalog.GetSleepStableWaveVfx();
                    }

                    return P0VisualAssetCatalog.GetHitSparkVfx();
                case P0BattleFeedbackKind.InteractionBlocked:
                case P0BattleFeedbackKind.SkillBlocked:
                    return P0VisualAssetCatalog.GetEnemyMarkRingVfx();
                case P0BattleFeedbackKind.CatPressure:
                case P0BattleFeedbackKind.CatWeak:
                    return P0VisualAssetCatalog.GetHitSparkVfx();
                case P0BattleFeedbackKind.BattleResult:
                    return feedback.Level == P0BattleFeedbackLevel.Result
                        ? P0VisualAssetCatalog.GetSleepStableWaveVfx()
                        : P0VisualAssetCatalog.GetHitSparkVfx();
                default:
                    return default(P0VisualAssetReference);
            }
        }

        private static P0VisualAssetReference GetStarterSkillVisualAsset(string title)
        {
            if (ContainsToken(title, "誓约银盾")
                || ContainsToken(title, "圆盾冲刺")
                || ContainsToken(title, "圆盾冲锋")
                || ContainsToken(title, "冠冕审判")
                || ContainsToken(title, "Silver Oath Shield")
                || ContainsToken(title, "Round Shield Rush")
                || ContainsToken(title, "Crown Judgement"))
            {
                return P0VisualAssetCatalog.GetStarterSkillVfx("saiban_oath_shield");
            }

            if (ContainsToken(title, "月砂方尖碑")
                || ContainsToken(title, "流沙陷阱")
                || ContainsToken(title, "王权标记")
                || ContainsToken(title, "Moon-Sand Obelisk")
                || ContainsToken(title, "Quicksand Trap")
                || ContainsToken(title, "Royal Mark"))
            {
                return P0VisualAssetCatalog.GetStarterSkillVfx("nephthys_moon_sand_obelisk");
            }

            if (ContainsToken(title, "安眠铃")
                || ContainsToken(title, "冰花祈愿")
                || ContainsToken(title, "月下鸟居印")
                || ContainsToken(title, "Sleep Bell")
                || ContainsToken(title, "Ice Blossom Prayer")
                || ContainsToken(title, "Moon Torii Seal"))
            {
                return P0VisualAssetCatalog.GetStarterSkillVfx("suzune_sleep_bell");
            }

            return default(P0VisualAssetReference);
        }

        private static P0BattleFeedbackColor BuildBackgroundColor(P0BattleFeedbackColor accent, float pulseAlpha)
        {
            float tint = 0.16f + 0.12f * pulseAlpha;
            return new P0BattleFeedbackColor(
                Math.Min(1f, accent.R * tint),
                Math.Min(1f, accent.G * tint),
                Math.Min(1f, accent.B * tint),
                0.34f + 0.36f * pulseAlpha);
        }

        private static string GetAccentToken(P0BattleFeedbackLevel level)
        {
            switch (level)
            {
                case P0BattleFeedbackLevel.Positive:
                    return "green";
                case P0BattleFeedbackLevel.Warning:
                    return "amber";
                case P0BattleFeedbackLevel.Critical:
                    return "red";
                case P0BattleFeedbackLevel.Result:
                    return "violet";
                case P0BattleFeedbackLevel.Info:
                default:
                    return "blue";
            }
        }

        private static P0BattleFeedbackColor GetAccentColor(P0BattleFeedbackLevel level)
        {
            switch (level)
            {
                case P0BattleFeedbackLevel.Positive:
                    return new P0BattleFeedbackColor(0.28f, 0.88f, 0.46f, 1f);
                case P0BattleFeedbackLevel.Warning:
                    return new P0BattleFeedbackColor(1f, 0.69f, 0.22f, 1f);
                case P0BattleFeedbackLevel.Critical:
                    return new P0BattleFeedbackColor(1f, 0.28f, 0.24f, 1f);
                case P0BattleFeedbackLevel.Result:
                    return new P0BattleFeedbackColor(0.78f, 0.58f, 1f, 1f);
                case P0BattleFeedbackLevel.Info:
                default:
                    return new P0BattleFeedbackColor(0.38f, 0.68f, 1f, 1f);
            }
        }

        private static bool ContainsToken(string value, string token)
        {
            return !string.IsNullOrWhiteSpace(value)
                && !string.IsNullOrWhiteSpace(token)
                && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool ContainsAnyToken(string value, params string[] tokens)
        {
            if (tokens == null)
            {
                return false;
            }

            for (int i = 0; i < tokens.Length; i++)
            {
                if (ContainsToken(value, tokens[i]))
                {
                    return true;
                }
            }

            return false;
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
