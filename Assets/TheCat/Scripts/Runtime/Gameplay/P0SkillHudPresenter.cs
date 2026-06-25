using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Gameplay
{
    public readonly struct P0SkillHudColor
    {
        public P0SkillHudColor(float r, float g, float b, float a)
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

    public readonly struct P0SkillHudCard
    {
        public P0SkillHudCard(
            string skillId,
            string displayName,
            string slotToken,
            string statusToken,
            string statusLabel,
            string detail,
            bool isEnabled,
            bool isCoolingDown,
            bool hasTargetIssue,
            bool isLowHunger,
            float cooldownSeconds,
            float cooldownRatio,
            float hungerCost,
            float currentHunger,
            float hungerAfterCast,
            string targetLabel,
            bool requiresEnemyTarget,
            bool hasEnemyTarget,
            P0SkillHudColor accentColor,
            P0SkillHudColor cooldownFillColor,
            P0VisualAssetReference statusVisualAsset,
            P0VisualAssetReference hungerCostVisualAsset,
            P0VisualAssetReference targetReticleAsset)
        {
            SkillId = skillId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            SlotToken = slotToken ?? string.Empty;
            StatusToken = statusToken ?? string.Empty;
            StatusLabel = statusLabel ?? string.Empty;
            Detail = detail ?? string.Empty;
            IsEnabled = isEnabled;
            IsCoolingDown = isCoolingDown;
            HasTargetIssue = hasTargetIssue;
            IsLowHunger = isLowHunger;
            CooldownSeconds = Math.Max(0f, cooldownSeconds);
            CooldownRatio = Clamp01(cooldownRatio);
            HungerCost = Math.Max(0f, hungerCost);
            CurrentHunger = Math.Max(0f, currentHunger);
            HungerAfterCast = Math.Max(0f, hungerAfterCast);
            TargetLabel = AppendTargetDiagnosticLabel(targetLabel ?? string.Empty, requiresEnemyTarget, hasEnemyTarget);
            RequiresEnemyTarget = requiresEnemyTarget;
            HasEnemyTarget = hasEnemyTarget;
            AccentColor = accentColor;
            CooldownFillColor = cooldownFillColor;
            StatusVisualAsset = statusVisualAsset;
            HungerCostVisualAsset = hungerCostVisualAsset;
            TargetReticleAsset = targetReticleAsset;
        }

        public string SkillId { get; }

        public string DisplayName { get; }

        public string SlotToken { get; }

        public string StatusToken { get; }

        public string StatusLabel { get; }

        public string Detail { get; }

        public bool IsEnabled { get; }

        public bool IsCoolingDown { get; }

        public bool HasTargetIssue { get; }

        public bool IsLowHunger { get; }

        public float CooldownSeconds { get; }

        public float CooldownRatio { get; }

        public float HungerCost { get; }

        public float CurrentHunger { get; }

        public float HungerAfterCast { get; }

        public string TargetLabel { get; }

        public bool RequiresEnemyTarget { get; }

        public bool HasEnemyTarget { get; }

        public P0SkillHudColor AccentColor { get; }

        public P0SkillHudColor CooldownFillColor { get; }

        public P0VisualAssetReference StatusVisualAsset { get; }

        public P0VisualAssetReference HungerCostVisualAsset { get; }

        public P0VisualAssetReference TargetReticleAsset { get; }

        public string BuildButtonLabel()
        {
            string label = SlotToken + " " + DisplayName + "\n" + StatusLabel;
            if (!string.IsNullOrWhiteSpace(TargetLabel))
            {
                label += "\n" + TargetLabel;
            }

            if (!string.IsNullOrWhiteSpace(Detail))
            {
                label += "\n" + Detail;
            }

            return label;
        }

        public string BuildSummary()
        {
            return SlotToken
                + " "
                + DisplayName
                + " "
                + StatusToken
                + " 冷却 "
                + CooldownSeconds.ToString("0.0")
                + "s 比例 "
                + CooldownRatio.ToString("0.00")
                + " 饱肚 "
                + CurrentHunger.ToString("0")
                + "->"
                + HungerAfterCast.ToString("0")
                + " 目标 "
                + TargetLabel
                + " | 饱肚 "
                + CurrentHunger.ToString("0")
                + "->"
                + HungerAfterCast.ToString("0");
        }

        private static string AppendTargetDiagnosticLabel(
            string label,
            bool requiresEnemyTarget,
            bool hasEnemyTarget)
        {
            if (!requiresEnemyTarget || !hasEnemyTarget || string.IsNullOrWhiteSpace(label) || label.Contains("Target"))
            {
                return label;
            }

            return label + " | 目标";
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

    public readonly struct P0SkillSelectionChoiceCard
    {
        public P0SkillSelectionChoiceCard(
            string slotLabel,
            string choiceId,
            string catLabel,
            string stageLabel,
            string skillLabel,
            string intentLabel,
            string summary,
            string stateToken,
            string stateLabel,
            bool canConfirm,
            SkillDefinition unlockedSkill)
        {
            SlotLabel = slotLabel ?? string.Empty;
            ChoiceId = choiceId ?? string.Empty;
            CatLabel = catLabel ?? string.Empty;
            StageLabel = stageLabel ?? string.Empty;
            SkillLabel = skillLabel ?? string.Empty;
            IntentLabel = intentLabel ?? string.Empty;
            Summary = summary ?? string.Empty;
            StateToken = stateToken ?? string.Empty;
            StateLabel = stateLabel ?? string.Empty;
            CanConfirm = canConfirm;
            UnlockedSkill = unlockedSkill;
        }

        public string SlotLabel { get; }

        public string ChoiceId { get; }

        public string CatLabel { get; }

        public string StageLabel { get; }

        public string SkillLabel { get; }

        public string IntentLabel { get; }

        public string Summary { get; }

        public string StateToken { get; }

        public string StateLabel { get; }

        public bool CanConfirm { get; }

        public SkillDefinition UnlockedSkill { get; }

        public bool HasRuntimeSkill => UnlockedSkill != null;

        public string BuildButtonLabel()
        {
            string selectedPrefix = StateToken == "selected" ? "> " : string.Empty;
            string intent = string.IsNullOrWhiteSpace(IntentLabel)
                ? string.Empty
                : " | " + IntentLabel;
            return selectedPrefix
                + SlotLabel
                + ". "
                + CatLabel
                + " | "
                + StageLabel
                + " | "
                + SkillLabel
                + "\n"
                + StateLabel
                + intent
                + "\n"
                + Summary;
        }

        public string BuildSummary()
        {
            return SlotLabel
                + " "
                + CatLabel
                + " "
                + StageLabel
                + " "
                + SkillLabel
                + " state "
                + StateToken
                + " confirm "
                + CanConfirm
                + " skill "
                + HasRuntimeSkill;
        }
    }

    public readonly struct P0SkillSelectionSurface
    {
        public P0SkillSelectionSurface(
            string titleLabel,
            string subtitleLabel,
            string detailLabel,
            string confirmActionLabel,
            bool hasPendingUpgrade,
            bool hasSelectedChoice,
            bool canConfirmSelection,
            IReadOnlyList<P0SkillSelectionChoiceCard> choices)
        {
            TitleLabel = titleLabel ?? string.Empty;
            SubtitleLabel = subtitleLabel ?? string.Empty;
            DetailLabel = detailLabel ?? string.Empty;
            ConfirmActionLabel = confirmActionLabel ?? string.Empty;
            HasPendingUpgrade = hasPendingUpgrade;
            HasSelectedChoice = hasSelectedChoice;
            CanConfirmSelection = canConfirmSelection;
            Choices = choices ?? Array.Empty<P0SkillSelectionChoiceCard>();
        }

        public string TitleLabel { get; }

        public string SubtitleLabel { get; }

        public string DetailLabel { get; }

        public string ConfirmActionLabel { get; }

        public bool HasPendingUpgrade { get; }

        public bool HasSelectedChoice { get; }

        public bool CanConfirmSelection { get; }

        public IReadOnlyList<P0SkillSelectionChoiceCard> Choices { get; }

        public bool TryGetChoice(string choiceId, out P0SkillSelectionChoiceCard choice)
        {
            for (int i = 0; i < Choices.Count; i++)
            {
                if (Choices[i].ChoiceId == choiceId)
                {
                    choice = Choices[i];
                    return true;
                }
            }

            choice = default(P0SkillSelectionChoiceCard);
            return false;
        }

        public string BuildSummary()
        {
            return TitleLabel
                + " choices "
                + Choices.Count
                + " pending "
                + HasPendingUpgrade
                + " selected "
                + HasSelectedChoice
                + " confirm "
                + CanConfirmSelection
                + " "
                + DetailLabel;
        }
    }

    public static class P0SkillSelectionPresenter
    {
        public static P0SkillSelectionSurface BuildSurface(
            RunProgressionState run,
            string selectedChoiceId)
        {
            if (run == null || !run.CatUpgrades.HasPendingUpgrade)
            {
                return new P0SkillSelectionSurface(
                    "技能选择",
                    "等待猫队获得下一次升级。",
                    "当前没有待确认的猫咪升级。",
                    "等待升级",
                    false,
                    false,
                    false,
                    Array.Empty<P0SkillSelectionChoiceCard>());
            }

            IReadOnlyList<CatUpgradeCandidate> offer = run.CatUpgrades.CreateCurrentOffer(run.Roster);
            bool hasSelected = HasChoice(offer, selectedChoiceId);
            List<P0SkillSelectionChoiceCard> cards = new List<P0SkillSelectionChoiceCard>();
            P0SkillSelectionChoiceCard selectedCard = default(P0SkillSelectionChoiceCard);
            for (int i = 0; i < offer.Count; i++)
            {
                P0SkillSelectionChoiceCard card = BuildChoiceCard(
                    offer[i],
                    (i + 1).ToString(),
                    hasSelected,
                    selectedChoiceId);
                if (card.StateToken == "selected")
                {
                    selectedCard = card;
                }

                cards.Add(card);
            }

            string detail = hasSelected
                ? selectedCard.CatLabel + " | " + selectedCard.StageLabel + " | " + selectedCard.SkillLabel + " | " + selectedCard.Summary
                : "选择一个候选后，再确认猫咪升级。";

            return new P0SkillSelectionSurface(
                "技能选择",
                "从本轮候选中选择一个技能或升级，确认后才写入路线状态。",
                detail,
                hasSelected ? "确认技能选择" : "先选择技能",
                true,
                hasSelected,
                hasSelected,
                cards.AsReadOnly());
        }

        public static P0SkillSelectionChoiceCard BuildLockedPreview(string catId, CatUpgradeStage stage)
        {
            CatPresentation cat = P0CatPresenter.Describe(catId);
            return new P0SkillSelectionChoiceCard(
                "-",
                string.Empty,
                cat.ShortLabel,
                P0CatUpgradeCatalog.GetStageLabel(stage),
                "未解锁",
                "前置不足",
                "需要先完成前置猫咪升级。",
                "locked",
                "未解锁",
                false,
                null);
        }

        public static bool HasP0SkillSelectionSurface(P0SkillSelectionSurface surface)
        {
            if (string.IsNullOrWhiteSpace(surface.TitleLabel)
                || string.IsNullOrWhiteSpace(surface.SubtitleLabel)
                || string.IsNullOrWhiteSpace(surface.DetailLabel)
                || surface.Choices.Count < 3)
            {
                return false;
            }

            bool hasReady = false;
            bool hasSelected = false;
            bool hasDisabled = false;
            bool hasRuntimeSkill = false;
            for (int i = 0; i < surface.Choices.Count; i++)
            {
                P0SkillSelectionChoiceCard card = surface.Choices[i];
                if (string.IsNullOrWhiteSpace(card.BuildButtonLabel()) || ContainsInternalToken(card.BuildButtonLabel()))
                {
                    return false;
                }

                hasReady |= card.StateToken == "ready";
                hasSelected |= card.StateToken == "selected";
                hasDisabled |= card.StateToken == "disabled";
                hasRuntimeSkill |= card.HasRuntimeSkill;
            }

            return surface.HasPendingUpgrade
                && (!surface.HasSelectedChoice || (hasSelected && hasDisabled && surface.CanConfirmSelection))
                && (hasReady || hasRuntimeSkill);
        }

        public static SkillDefinition ResolveUnlockedSkill(CatUpgradeCandidate candidate)
        {
            if (!candidate.IsValid || candidate.Stage == CatUpgradeStage.Passive)
            {
                return null;
            }

            SkillDefinition[] definitions = P0CatUpgradeRuntimeCatalog.CreateUpgradeSkillDefinitions();
            for (int i = 0; i < definitions.Length; i++)
            {
                if (P0CatUpgradeRuntimeCatalog.GetUnlockingUpgradeId(definitions[i].Id) == candidate.Id)
                {
                    return definitions[i];
                }
            }

            return null;
        }

        private static P0SkillSelectionChoiceCard BuildChoiceCard(
            CatUpgradeCandidate choice,
            string slotLabel,
            bool hasSelected,
            string selectedChoiceId)
        {
            CatPresentation cat = P0CatPresenter.Describe(choice.CatId);
            SkillDefinition skill = ResolveUnlockedSkill(choice);
            string skillLabel = skill == null
                ? choice.DisplayName
                : P0SkillPresenter.Describe(skill).DisplayName;
            bool selected = hasSelected && choice.Id == selectedChoiceId;
            string stateToken = selected ? "selected" : hasSelected ? "disabled" : "ready";
            return new P0SkillSelectionChoiceCard(
                slotLabel,
                choice.Id,
                cat.ShortLabel,
                choice.StageLabel,
                skillLabel,
                choice.PlayerIntent,
                choice.BuildSummary(),
                stateToken,
                BuildStateLabel(stateToken),
                selected,
                skill);
        }

        private static string BuildStateLabel(string stateToken)
        {
            switch (stateToken)
            {
                case "selected":
                    return "已选择";
                case "disabled":
                    return "待确认其他选择";
                case "locked":
                    return "未解锁";
                default:
                    return "可选择";
            }
        }

        private static bool HasChoice(IReadOnlyList<CatUpgradeCandidate> offer, string choiceId)
        {
            if (string.IsNullOrWhiteSpace(choiceId))
            {
                return false;
            }

            for (int i = 0; i < offer.Count; i++)
            {
                if (offer[i].Id == choiceId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsInternalToken(string text)
        {
            return (text ?? string.Empty).Contains("cat_upgrade")
                || (text ?? string.Empty).Contains("saiban")
                || (text ?? string.Empty).Contains("nephthys")
                || (text ?? string.Empty).Contains("suzune")
                || (text ?? string.Empty).Contains("_");
        }
    }

    public static class P0SkillHudPresenter
    {
        public static P0SkillHudCard BuildCard(
            SkillDefinition skill,
            P0BattleActionAffordance affordance,
            float cooldownSeconds,
            float currentHunger,
            P0SkillTargetResult target)
        {
            if (skill == null)
            {
                return new P0SkillHudCard(
                    "missing_skill",
                    "缺失技能",
                    "缺失",
                    "缺失",
                    "缺少定义",
                    "检查原型目录",
                    false,
                    false,
                    true,
                    false,
                    0f,
                    0f,
                    0f,
                    currentHunger,
                    currentHunger,
                    "无技能目标",
                    false,
                    false,
                    new P0SkillHudColor(0.85f, 0.2f, 0.2f, 1f),
                    new P0SkillHudColor(0.85f, 0.2f, 0.2f, 1f),
                    default(P0VisualAssetReference),
                    default(P0VisualAssetReference),
                    default(P0VisualAssetReference));
            }

            SkillPresentation presentation = P0SkillPresenter.Describe(skill);
            float safeCooldown = Math.Max(0f, cooldownSeconds);
            float cooldownRatio = skill.CooldownSeconds <= 0f ? 0f : safeCooldown / skill.CooldownSeconds;
            bool coolingDown = safeCooldown > 0f;
            bool targetIssue = target.RequiresEnemyTarget && !target.HasEnemyTarget;
            bool lowHunger = currentHunger < skill.HungerCost;
            string statusToken = BuildStatusToken(affordance, coolingDown, targetIssue, lowHunger);
            string statusLabel = string.IsNullOrWhiteSpace(affordance.Status)
                ? statusToken
                : affordance.Status;

            return new P0SkillHudCard(
                skill.Id,
                presentation.DisplayName,
                BuildSlotToken(skill.Slot),
                statusToken,
                statusLabel,
                presentation.EffectHint,
                affordance.IsEnabled,
                coolingDown,
                targetIssue,
                lowHunger,
                safeCooldown,
                cooldownRatio,
                skill.HungerCost,
                currentHunger,
                Math.Max(0f, currentHunger - skill.HungerCost),
                BuildTargetLabel(skill, target),
                target.RequiresEnemyTarget,
                target.HasEnemyTarget,
                BuildAccentColor(skill.Slot, statusToken),
                BuildCooldownFillColor(statusToken),
                P0VisualAssetCatalog.GetSkillHudStatusFeedback(statusToken),
                skill.HungerCost > 0f ? P0VisualAssetCatalog.GetSkillHudHungerCostChip() : default(P0VisualAssetReference),
                target.HasEnemyTarget ? P0VisualAssetCatalog.GetAutoTargetReticle() : default(P0VisualAssetReference));
        }

        public static bool HasP0SkillHudCards(IReadOnlyList<P0SkillHudCard> cards)
        {
            if (cards == null || cards.Count < 3)
            {
                return false;
            }

            bool hasReady = false;
            bool hasSlot1 = false;
            bool hasSlot2 = false;
            bool hasUltimate = false;
            for (int i = 0; i < cards.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(cards[i].BuildButtonLabel()))
                {
                    return false;
                }

                hasReady |= cards[i].StatusToken == "ready" || cards[i].StatusToken == "low_hunger";
                hasSlot1 |= cards[i].SlotToken == "S1";
                hasSlot2 |= cards[i].SlotToken == "S2";
                hasUltimate |= cards[i].SlotToken == "ULT";
            }

            return hasReady && hasSlot1 && hasSlot2 && hasUltimate;
        }

        public static string BuildCompactSummary(IReadOnlyList<P0SkillHudCard> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                return "技能 HUD：空";
            }

            int enabled = 0;
            int cooldown = 0;
            int targetIssue = 0;
            int lowHunger = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                enabled += cards[i].IsEnabled ? 1 : 0;
                cooldown += cards[i].IsCoolingDown ? 1 : 0;
                targetIssue += cards[i].HasTargetIssue ? 1 : 0;
                lowHunger += cards[i].IsLowHunger ? 1 : 0;
            }

            return "技能 HUD："
                + cards.Count
                + " 可用 "
                + enabled
                + " 冷却 "
                + cooldown
                + " 目标问题 "
                + targetIssue
                + " 低饱肚 "
                + lowHunger;
        }

        private static string BuildStatusToken(
            P0BattleActionAffordance affordance,
            bool coolingDown,
            bool targetIssue,
            bool lowHunger)
        {
            if (coolingDown)
            {
                return "cooldown";
            }

            if (targetIssue)
            {
                return "no_target";
            }

            if (lowHunger)
            {
                return "low_hunger";
            }

            return affordance.IsEnabled ? "ready" : "disabled";
        }

        private static string BuildTargetLabel(SkillDefinition skill, P0SkillTargetResult target)
        {
            if (target.RequiresEnemyTarget)
            {
                if (target.HasEnemyTarget)
                {
                    return "目标 " + target.Enemy.Definition.DisplayName + " " + target.Distance.ToString("0.0") + "m";
                }

                return "无目标 <= " + target.Range.ToString("0.0") + "m";
            }

            return P0SkillTargetResolver.RequiresEnemyTarget(skill) ? "无目标" : "自身 / 区域";
        }

        private static string BuildSlotToken(SkillSlot slot)
        {
            switch (slot)
            {
                case SkillSlot.SmallSkill1:
                    return "S1";
                case SkillSlot.SmallSkill2:
                    return "S2";
                case SkillSlot.SmallSkill3:
                    return "S3";
                case SkillSlot.SmallSkill4:
                    return "S4";
                case SkillSlot.Ultimate1:
                case SkillSlot.Ultimate2:
                    return "ULT";
                default:
                    return "SK";
            }
        }

        private static P0SkillHudColor BuildAccentColor(SkillSlot slot, string statusToken)
        {
            if (statusToken == "cooldown")
            {
                return new P0SkillHudColor(0.55f, 0.6f, 0.72f, 1f);
            }

            if (statusToken == "no_target")
            {
                return new P0SkillHudColor(1f, 0.45f, 0.25f, 1f);
            }

            if (statusToken == "low_hunger")
            {
                return new P0SkillHudColor(1f, 0.72f, 0.24f, 1f);
            }

            switch (slot)
            {
                case SkillSlot.Ultimate1:
                    return new P0SkillHudColor(0.85f, 0.62f, 1f, 1f);
                case SkillSlot.SmallSkill2:
                    return new P0SkillHudColor(0.46f, 0.78f, 1f, 1f);
                case SkillSlot.SmallSkill1:
                default:
                    return new P0SkillHudColor(0.38f, 0.9f, 0.58f, 1f);
            }
        }

        private static P0SkillHudColor BuildCooldownFillColor(string statusToken)
        {
            if (statusToken == "cooldown")
            {
                return new P0SkillHudColor(0.55f, 0.6f, 0.72f, 1f);
            }

            if (statusToken == "no_target")
            {
                return new P0SkillHudColor(1f, 0.45f, 0.25f, 1f);
            }

            if (statusToken == "low_hunger")
            {
                return new P0SkillHudColor(1f, 0.72f, 0.24f, 1f);
            }

            return new P0SkillHudColor(0.35f, 0.86f, 0.48f, 1f);
        }
    }
}
