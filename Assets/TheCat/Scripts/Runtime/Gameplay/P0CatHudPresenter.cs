using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public readonly struct P0CatHudColor
    {
        public P0CatHudColor(float r, float g, float b, float a)
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

    public readonly struct P0CatHudCard
    {
        public P0CatHudCard(
            string catId,
            string displayName,
            string title,
            string portraitToken,
            P0VisualAssetReference combatSprite,
            P0VisualAssetReference hudAvatar,
            string roleToken,
            string roleLabel,
            string slotState,
            bool isActive,
            bool canSwitch,
            bool isWeak,
            float weakRemainingSeconds,
            float currentHp,
            float maxHp,
            float hpRatio,
            string hpStateToken,
            string hpLabel,
            P0VisualAssetReference hpIcon,
            P0VisualAssetReference hpGaugeFrameAsset,
            P0VisualAssetReference hpGaugeFillAsset,
            bool hasShield,
            float shieldAmount,
            string statusSummary,
            int skillCount,
            float maxCooldownSeconds,
            string cooldownLabel,
            P0CatHudColor accentColor,
            P0CatHudColor hpFillColor)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            Title = title ?? string.Empty;
            PortraitToken = portraitToken ?? string.Empty;
            CombatSprite = combatSprite;
            HudAvatar = hudAvatar;
            RoleToken = roleToken ?? string.Empty;
            RoleLabel = roleLabel ?? string.Empty;
            SlotState = slotState ?? string.Empty;
            IsActive = isActive;
            CanSwitch = canSwitch;
            IsWeak = isWeak;
            WeakRemainingSeconds = Math.Max(0f, weakRemainingSeconds);
            CurrentHp = Math.Max(0f, currentHp);
            MaxHp = Math.Max(0.01f, maxHp);
            HpRatio = Clamp01(hpRatio);
            HpStateToken = hpStateToken ?? string.Empty;
            HpLabel = hpLabel ?? string.Empty;
            HpIcon = hpIcon;
            HpGaugeFrameAsset = hpGaugeFrameAsset;
            HpGaugeFillAsset = hpGaugeFillAsset;
            HasShield = hasShield;
            ShieldAmount = Math.Max(0f, shieldAmount);
            StatusSummary = statusSummary ?? string.Empty;
            SkillCount = Math.Max(0, skillCount);
            MaxCooldownSeconds = Math.Max(0f, maxCooldownSeconds);
            CooldownLabel = cooldownLabel ?? string.Empty;
            AccentColor = accentColor;
            HpFillColor = hpFillColor;
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string Title { get; }

        public string PortraitToken { get; }

        public P0VisualAssetReference CombatSprite { get; }

        public P0VisualAssetReference HudAvatar { get; }

        public P0VisualAssetReference PrimaryHudIcon => HudAvatar.HasAsset ? HudAvatar : CombatSprite;

        public string RoleToken { get; }

        public string RoleLabel { get; }

        public string SlotState { get; }

        public bool IsActive { get; }

        public bool CanSwitch { get; }

        public bool IsWeak { get; }

        public float WeakRemainingSeconds { get; }

        public float CurrentHp { get; }

        public float MaxHp { get; }

        public float HpRatio { get; }

        public string HpStateToken { get; }

        public string HpLabel { get; }

        public P0VisualAssetReference HpIcon { get; }

        public P0VisualAssetReference HpGaugeFrameAsset { get; }

        public P0VisualAssetReference HpGaugeFillAsset { get; }

        public bool HasShield { get; }

        public float ShieldAmount { get; }

        public string StatusSummary { get; }

        public int SkillCount { get; }

        public float MaxCooldownSeconds { get; }

        public string CooldownLabel { get; }

        public P0CatHudColor AccentColor { get; }

        public P0CatHudColor HpFillColor { get; }

        public string BuildButtonLabel()
        {
            string label = SlotState
                + " [" + PortraitToken + "] "
                + DisplayName
                + "  " + RoleLabel
                + "\n"
                + HpLabel
                + "  " + HpStateToken;
            if (!string.IsNullOrWhiteSpace(CooldownLabel))
            {
                label += "\n" + CooldownLabel;
            }

            if (!string.IsNullOrWhiteSpace(StatusSummary))
            {
                label += "\n" + StatusSummary;
            }

            return label;
        }

        public string BuildSummary()
        {
            string summary = SlotState
                + " "
                + DisplayName
                + " "
                + RoleLabel
                + " 生命 "
                + CurrentHp.ToString("0")
                + "/"
                + MaxHp.ToString("0")
                + " "
                + HpStateToken
                + " 技能 "
                + SkillCount
                + " "
                + CooldownLabel;
            if (HasShield)
            {
                summary += " 护盾 " + ShieldAmount.ToString("0.#");
            }

            if (IsWeak)
            {
                summary += " 虚弱 " + WeakRemainingSeconds.ToString("0.0") + "s";
            }

            return summary;
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

    public static class P0CatHudPresenter
    {
        public static IReadOnlyList<P0CatHudCard> BuildCards(
            IReadOnlyList<CatBattleState> cats,
            int activeCatIndex,
            Func<string, float> cooldownProvider)
        {
            if (cats == null || cats.Count == 0)
            {
                return Array.Empty<P0CatHudCard>();
            }

            List<P0CatHudCard> cards = new List<P0CatHudCard>();
            for (int i = 0; i < cats.Count; i++)
            {
                cards.Add(BuildCard(cats[i], i == activeCatIndex, cooldownProvider));
            }

            return cards.AsReadOnly();
        }

        public static P0CatHudCard BuildCard(
            CatBattleState cat,
            bool isActive,
            Func<string, float> cooldownProvider)
        {
            if (cat == null)
            {
                return default(P0CatHudCard);
            }

            CatDefinition definition = cat.Definition;
            CatPresentation presentation = P0CatPresenter.Describe(definition);
            float hpRatio = definition.MaxHp <= 0f ? 0f : cat.Vital.CurrentHp / definition.MaxHp;
            string hpState = BuildHpStateToken(cat.Vital.IsWeak, hpRatio);
            bool hasShield = cat.Statuses.TryGet(StatusTagIds.Shield, out StatusEffectState shield);
            float maxCooldown = GetMaxCooldown(definition, cooldownProvider);
            string statusSummary = BuildStatusSummary(cat, hasShield, shield);

            return new P0CatHudCard(
                definition.Id,
                presentation.DisplayName,
                presentation.Title,
                BuildPortraitToken(presentation.DisplayName),
                definition.CombatSprite,
                P0VisualAssetCatalog.GetStarterCatHudAvatar(definition.Id),
                BuildRoleToken(definition.Role),
                BuildRoleLabel(definition.Role),
                cat.Vital.IsWeak ? "虚弱" : isActive ? "当前" : "候补",
                isActive,
                cat.Vital.CanSwitchTo && !isActive,
                cat.Vital.IsWeak,
                cat.Vital.WeakRemainingSeconds,
                cat.Vital.CurrentHp,
                definition.MaxHp,
                hpRatio,
                hpState,
                presentation.BuildVitalLabel(cat.Vital.CurrentHp, definition.MaxHp, cat.Vital.IsWeak, cat.Vital.WeakRemainingSeconds),
                P0VisualAssetCatalog.GetCatHpIcon(),
                P0VisualAssetCatalog.GetCoreGaugeFrame("cat_hp"),
                P0VisualAssetCatalog.GetCoreGaugeFill("cat_hp"),
                hasShield,
                hasShield ? shield.Magnitude : 0f,
                statusSummary,
                definition.SkillIds.Count,
                maxCooldown,
                BuildCooldownLabel(maxCooldown),
                BuildRoleAccent(definition.Role),
                BuildHpColor(hpState));
        }

        public static bool HasP0CatHudCards(IReadOnlyList<P0CatHudCard> cards)
        {
            if (cards == null || cards.Count < 3)
            {
                return false;
            }

            bool hasActive = false;
            bool hasDefender = false;
            bool hasController = false;
            bool hasHealer = false;
            for (int i = 0; i < cards.Count; i++)
            {
                hasActive |= cards[i].IsActive;
                hasDefender |= cards[i].RoleToken == "DEF";
                hasController |= cards[i].RoleToken == "CTRL";
                hasHealer |= cards[i].RoleToken == "HEAL";
            }

            return hasActive && hasDefender && hasController && hasHealer;
        }

        public static string BuildCompactSummary(IReadOnlyList<P0CatHudCard> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                return "猫 HUD：空";
            }

            int active = 0;
            int weak = 0;
            int shield = 0;
            int cooldown = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                active += cards[i].IsActive ? 1 : 0;
                weak += cards[i].IsWeak ? 1 : 0;
                shield += cards[i].HasShield ? 1 : 0;
                cooldown += cards[i].MaxCooldownSeconds > 0f ? 1 : 0;
            }

            return "猫 HUD："
                + cards.Count
                + " 当前 "
                + active
                + " 虚弱 "
                + weak
                + " 护盾 "
                + shield
                + " 冷却 "
                + cooldown;
        }

        private static float GetMaxCooldown(CatDefinition definition, Func<string, float> cooldownProvider)
        {
            if (definition == null || cooldownProvider == null)
            {
                return 0f;
            }

            float maxCooldown = 0f;
            for (int i = 0; i < definition.SkillIds.Count; i++)
            {
                maxCooldown = Math.Max(maxCooldown, cooldownProvider(definition.SkillIds[i]));
            }

            return maxCooldown;
        }

        private static string BuildStatusSummary(CatBattleState cat, bool hasShield, StatusEffectState shield)
        {
            string statusText = StatusDisplayFormatter.FormatCollection(cat.Statuses);
            if (hasShield && string.IsNullOrWhiteSpace(statusText))
            {
                statusText = "护盾 " + shield.Magnitude.ToString("0.#");
            }

            return statusText;
        }

        private static string BuildCooldownLabel(float maxCooldown)
        {
            return maxCooldown > 0f
                ? "冷却 " + maxCooldown.ToString("0.0") + "s"
                : "技能就绪";
        }

        private static string BuildHpStateToken(bool isWeak, float hpRatio)
        {
            if (isWeak)
            {
                return "虚弱";
            }

            if (hpRatio <= 0.3f)
            {
                return "危急";
            }

            if (hpRatio <= 0.6f)
            {
                return "受伤";
            }

            return "健康";
        }

        private static string BuildPortraitToken(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                return "CAT";
            }

            string compact = displayName.Trim().Replace(" ", string.Empty);
            return compact.Length <= 3
                ? compact.ToUpperInvariant()
                : compact.Substring(0, 3).ToUpperInvariant();
        }

        private static string BuildRoleToken(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return "DEF";
                case CatRole.Controller:
                    return "CTRL";
                case CatRole.Healer:
                    return "HEAL";
                case CatRole.Assassin:
                    return "ASN";
                case CatRole.Commander:
                    return "CMD";
                case CatRole.Counter:
                    return "CTR";
                default:
                    return "CAT";
            }
        }

        private static string BuildRoleLabel(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return "防守";
                case CatRole.Controller:
                    return "控场";
                case CatRole.Healer:
                    return "治疗";
                case CatRole.Assassin:
                    return "刺杀";
                case CatRole.Commander:
                    return "指挥";
                case CatRole.Counter:
                    return "反制";
                default:
                    return "专精";
            }
        }

        private static P0CatHudColor BuildRoleAccent(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return new P0CatHudColor(1f, 0.74f, 0.25f, 1f);
                case CatRole.Controller:
                    return new P0CatHudColor(0.52f, 0.66f, 1f, 1f);
                case CatRole.Healer:
                    return new P0CatHudColor(0.34f, 0.9f, 0.58f, 1f);
                default:
                    return new P0CatHudColor(0.8f, 0.8f, 0.85f, 1f);
            }
        }

        private static P0CatHudColor BuildHpColor(string hpState)
        {
            switch (hpState)
            {
                case "虚弱":
                    return new P0CatHudColor(0.55f, 0.55f, 0.62f, 1f);
                case "危急":
                    return new P0CatHudColor(1f, 0.25f, 0.22f, 1f);
                case "受伤":
                    return new P0CatHudColor(1f, 0.68f, 0.24f, 1f);
                case "健康":
                default:
                    return new P0CatHudColor(0.35f, 0.88f, 0.44f, 1f);
            }
        }
    }
}
