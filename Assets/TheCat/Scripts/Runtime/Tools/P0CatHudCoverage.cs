using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tools
{
    public enum P0CatHudCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0CatHudCoverageIssue
    {
        public P0CatHudCoverageIssue(P0CatHudCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0CatHudCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0CatHudCoverageReport
    {
        private readonly List<P0CatHudCoverageIssue> issues = new List<P0CatHudCoverageIssue>();
        private readonly List<string> coveredCards = new List<string>();

        public IReadOnlyList<P0CatHudCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredCards => coveredCards.AsReadOnly();

        public int FailureCount => Count(P0CatHudCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredCards.Count >= P0CatHudCoverage.ExpectedCoveredCardCount;

        public void AddIssue(P0CatHudCoverageSeverity severity, string message)
        {
            issues.Add(new P0CatHudCoverageIssue(severity, message));
        }

        public void AddCoveredCard(string card)
        {
            if (!string.IsNullOrWhiteSpace(card))
            {
                coveredCards.Add(card);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 cat HUD coverage complete for " + coveredCards.Count + " card check(s)."
                : "P0 cat HUD coverage has " + FailureCount + " failure(s) across " + coveredCards.Count + " card check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredCards.Count; i++)
            {
                lines.Add("- " + coveredCards[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0CatHudCoverageSeverity severity)
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

    public static class P0CatHudCoverage
    {
        public const int ExpectedCoveredCardCount = 5;

        public static P0CatHudCoverageReport EvaluatePrototypeCards()
        {
            P0CatHudCoverageReport report = new P0CatHudCoverageReport();

            EvaluateStarterRoster(report);
            EvaluateHpStates(report);
            EvaluateShieldStatus(report);
            EvaluateCooldownState(report);
            EvaluateCompactSummary(report);

            return report;
        }

        private static void EvaluateStarterRoster(P0CatHudCoverageReport report)
        {
            IReadOnlyList<P0CatHudCard> cards = P0CatHudPresenter.BuildCards(CreateStarterCatStates(), 0, NoCooldown);

            Require(
                report,
                P0CatHudPresenter.HasP0CatHudCards(cards)
                && cards[0].IsActive
                && cards[0].SlotState == "当前"
                && cards[0].RoleToken == "DEF"
                && cards[1].RoleToken == "CTRL"
                && cards[2].RoleToken == "HEAL"
                && HasAvatar(cards[0], P0VisualAssetCatalog.SaibanHudAvatarId, "saiban_turnaround_colored")
                && HasAvatar(cards[1], P0VisualAssetCatalog.NephthysHudAvatarId, "nephthys_turnaround_colored")
                && HasAvatar(cards[2], P0VisualAssetCatalog.SuzuneHudAvatarId, "suzune_turnaround_colored"),
                "Starter cat HUD cards cover active Saiban, role tokens, and source-locked HUD avatars.",
                "Starter cat HUD cards did not expose the required P0 roster roles or source-locked HUD avatars.");
        }

        private static void EvaluateHpStates(P0CatHudCoverageReport report)
        {
            CatBattleState critical = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId), currentHp: 44f);
            CatBattleState weak = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId), currentHp: 0f, weakRemainingSeconds: 12f);
            P0CatHudCard criticalCard = P0CatHudPresenter.BuildCard(critical, false, NoCooldown);
            P0CatHudCard weakCard = P0CatHudPresenter.BuildCard(weak, false, NoCooldown);

            Require(
                report,
                criticalCard.HpStateToken == "危急"
                && criticalCard.HpRatio > 0.19f
                && criticalCard.HpRatio < 0.21f
                && weakCard.HpStateToken == "虚弱"
                && weakCard.SlotState == "虚弱"
                && !weakCard.CanSwitch
                && weakCard.BuildSummary().Contains("虚弱 12.0s"),
                "Cat HUD cards distinguish critical HP from weak state and disable weak switching.",
                "Cat HUD cards did not distinguish critical HP and weak state.");
        }

        private static void EvaluateShieldStatus(P0CatHudCoverageReport report)
        {
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));
            cat.ApplyStatus(GetStatus(StatusTagIds.Shield), 25f);
            P0CatHudCard card = P0CatHudPresenter.BuildCard(cat, true, NoCooldown);

            Require(
                report,
                card.HasShield
                && card.ShieldAmount == 25f
                && card.StatusSummary.Contains("护盾")
                && card.BuildButtonLabel().Contains("护盾"),
                "Cat HUD card surfaces active shield amount and status text.",
                "Cat HUD card did not surface active shield state.");
        }

        private static void EvaluateCooldownState(P0CatHudCoverageReport report)
        {
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.NephthysId));
            P0CatHudCard card = P0CatHudPresenter.BuildCard(cat, false, id => id == "nephthys_royal_mark" ? 5.5f : 1.25f);

            Require(
                report,
                card.SkillCount == 3
                && card.MaxCooldownSeconds == 5.5f
                && card.CooldownLabel == "冷却 5.5s"
                && card.BuildButtonLabel().Contains("冷却 5.5s"),
                "Cat HUD card exposes skill count and highest cooldown state.",
                "Cat HUD card did not expose skill cooldown state.");
        }

        private static void EvaluateCompactSummary(P0CatHudCoverageReport report)
        {
            List<CatBattleState> cats = new List<CatBattleState>(CreateStarterCatStates());
            cats[0].ApplyStatus(GetStatus(StatusTagIds.Shield), 10f);
            IReadOnlyList<P0CatHudCard> cards = P0CatHudPresenter.BuildCards(cats, 1, id => id == "suzune_sleep_bell" ? 3f : 0f);
            string summary = P0CatHudPresenter.BuildCompactSummary(cards);

            Require(
                report,
                summary.Contains("3 当前 1")
                && summary.Contains("护盾 1")
                && summary.Contains("冷却 1"),
                "Cat HUD compact summary reports card count, active cat, shield, and cooldown totals.",
                "Cat HUD compact summary did not report required totals.");
        }

        private static IReadOnlyList<CatBattleState> CreateStarterCatStates()
        {
            IReadOnlyList<CatDefinition> definitions = P0PrototypeCatalog.CreateStarterCats();
            List<CatBattleState> cats = new List<CatBattleState>();
            for (int i = 0; i < definitions.Count; i++)
            {
                cats.Add(new CatBattleState(definitions[i]));
            }

            return cats.AsReadOnly();
        }

        private static float NoCooldown(string skillId)
        {
            return 0f;
        }

        private static CatDefinition GetCat(string catId)
        {
            IReadOnlyList<CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i];
                }
            }

            throw new InvalidOperationException("Missing cat: " + catId);
        }

        private static bool HasAvatar(P0CatHudCard card, string assetId, string sourceLockId)
        {
            return card.HudAvatar.AssetId == assetId
                && card.HudAvatar.AssetType == "avatar_icon"
                && card.PrimaryHudIcon.AssetId == assetId
                && Contains(card.HudAvatar.SourceLockIds, sourceLockId);
        }

        private static bool Contains(IReadOnlyList<string> values, string value)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == value)
                {
                    return true;
                }
            }

            return false;
        }

        private static StatusTagDefinition GetStatus(string statusId)
        {
            IReadOnlyList<StatusTagDefinition> statuses = P0PrototypeCatalog.CreateStatusTags();
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i].Id == statusId)
                {
                    return statuses[i];
                }
            }

            throw new InvalidOperationException("Missing status: " + statusId);
        }

        private static void Require(
            P0CatHudCoverageReport report,
            bool condition,
            string coveredCard,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCard(coveredCard);
                return;
            }

            report.AddIssue(P0CatHudCoverageSeverity.Failure, failureMessage);
        }
    }
}
