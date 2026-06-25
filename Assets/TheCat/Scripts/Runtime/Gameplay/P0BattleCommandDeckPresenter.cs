using System;
using System.Collections.Generic;

namespace TheCat.Gameplay
{
    public readonly struct P0BattleCommandDeck
    {
        public P0BattleCommandDeck(
            string title,
            IReadOnlyList<string> lines,
            int catCount,
            int skillCount,
            int interactionCount,
            int enabledActionCount)
        {
            Title = title ?? string.Empty;
            Lines = lines ?? Array.Empty<string>();
            CatCount = Math.Max(0, catCount);
            SkillCount = Math.Max(0, skillCount);
            InteractionCount = Math.Max(0, interactionCount);
            EnabledActionCount = Math.Max(0, enabledActionCount);
        }

        public string Title { get; }

        public IReadOnlyList<string> Lines { get; }

        public int CatCount { get; }

        public int SkillCount { get; }

        public int InteractionCount { get; }

        public int EnabledActionCount { get; }

        public string BuildSummary()
        {
            return Title
                + "：猫 "
                + CatCount
                + " 技能 "
                + SkillCount
                + " 交互 "
                + InteractionCount
                + " 可用 "
                + EnabledActionCount
                + " | "
                + string.Join("；", Lines);
        }

        public string BuildCompactPlayerLine()
        {
            return Title + "：" + string.Join(" | ", Lines);
        }
    }

    public static class P0BattleCommandDeckPresenter
    {
        public static P0BattleCommandDeck BuildDeck(
            IReadOnlyList<P0CatHudCard> catCards,
            IReadOnlyList<P0SkillHudCard> skillCards,
            IReadOnlyList<P0BattleActionAffordance> interactions)
        {
            List<string> lines = new List<string>();

            P0CatHudCard activeCat = SelectActiveCat(catCards);
            if (!string.IsNullOrWhiteSpace(activeCat.DisplayName))
            {
                lines.Add("上阵：" + BuildCatLine(activeCat));
            }

            P0SkillHudCard skill = SelectPrimarySkill(skillCards);
            if (!string.IsNullOrWhiteSpace(skill.DisplayName))
            {
                lines.Add("主技能：" + BuildSkillLine(skill));
            }

            P0BattleActionAffordance interaction = SelectPrimaryInteraction(interactions);
            if (!string.IsNullOrWhiteSpace(interaction.Title))
            {
                lines.Add("交互：" + BuildInteractionLine(interaction));
            }

            return new P0BattleCommandDeck(
                "当前行动",
                lines.AsReadOnly(),
                Count(catCards),
                Count(skillCards),
                Count(interactions),
                CountEnabled(skillCards, interactions));
        }

        public static bool HasP0BattleCommandDeck(P0BattleCommandDeck deck)
        {
            if (string.IsNullOrWhiteSpace(deck.Title)
                || deck.Lines == null
                || deck.Lines.Count < 3
                || deck.CatCount < 3
                || deck.SkillCount < 3
                || deck.InteractionCount < 3)
            {
                return false;
            }

            for (int i = 0; i < deck.Lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(deck.Lines[i])
                    || deck.Lines[i].Contains("Target")
                    || deck.Lines[i].Contains("hunger")
                    || deck.Lines[i].Contains("\n")
                    || deck.Lines[i].Contains("缺失技能")
                    || deck.Lines[i].Contains("缺少技能定义"))
                {
                    return false;
                }
            }

            return true;
        }

        private static P0CatHudCard SelectActiveCat(IReadOnlyList<P0CatHudCard> catCards)
        {
            if (catCards == null || catCards.Count == 0)
            {
                return default(P0CatHudCard);
            }

            for (int i = 0; i < catCards.Count; i++)
            {
                if (catCards[i].IsActive)
                {
                    return catCards[i];
                }
            }

            return catCards[0];
        }

        private static P0SkillHudCard SelectPrimarySkill(IReadOnlyList<P0SkillHudCard> skillCards)
        {
            if (skillCards == null || skillCards.Count == 0)
            {
                return default(P0SkillHudCard);
            }

            for (int i = 0; i < skillCards.Count; i++)
            {
                if (skillCards[i].IsEnabled)
                {
                    return skillCards[i];
                }
            }

            for (int i = 0; i < skillCards.Count; i++)
            {
                if (!skillCards[i].IsCoolingDown)
                {
                    return skillCards[i];
                }
            }

            return skillCards[0];
        }

        private static P0BattleActionAffordance SelectPrimaryInteraction(IReadOnlyList<P0BattleActionAffordance> interactions)
        {
            if (interactions == null || interactions.Count == 0)
            {
                return default(P0BattleActionAffordance);
            }

            for (int i = 0; i < interactions.Count; i++)
            {
                if (interactions[i].IsEnabled)
                {
                    return interactions[i];
                }
            }

            return interactions[0];
        }

        private static string BuildCatLine(P0CatHudCard card)
        {
            return card.DisplayName
                + " 生命 "
                + card.CurrentHp.ToString("0")
                + "/"
                + card.MaxHp.ToString("0")
                + " "
                + card.HpStateToken;
        }

        private static string BuildSkillLine(P0SkillHudCard card)
        {
            string line = card.SlotToken
                + " "
                + card.DisplayName
                + " "
                + card.StatusLabel;
            string target = card.HasTargetIssue ? FormatPlayerTargetLabel(card.TargetLabel) : string.Empty;
            if (!string.IsNullOrWhiteSpace(target))
            {
                line += " / " + target;
            }

            return line;
        }

        private static string BuildInteractionLine(P0BattleActionAffordance interaction)
        {
            string line = interaction.Title;
            if (!string.IsNullOrWhiteSpace(interaction.Status))
            {
                line += " " + interaction.Status;
            }

            return line;
        }

        private static string FormatPlayerTargetLabel(string targetLabel)
        {
            if (string.IsNullOrWhiteSpace(targetLabel))
            {
                return string.Empty;
            }

            int diagnosticIndex = targetLabel.IndexOf(" | ", StringComparison.Ordinal);
            return diagnosticIndex < 0 ? targetLabel : targetLabel.Substring(0, diagnosticIndex);
        }

        private static int Count<T>(IReadOnlyList<T> items)
        {
            return items == null ? 0 : items.Count;
        }

        private static int CountEnabled(
            IReadOnlyList<P0SkillHudCard> skillCards,
            IReadOnlyList<P0BattleActionAffordance> interactions)
        {
            int count = 0;
            if (skillCards != null)
            {
                for (int i = 0; i < skillCards.Count; i++)
                {
                    count += skillCards[i].IsEnabled ? 1 : 0;
                }
            }

            if (interactions != null)
            {
                for (int i = 0; i < interactions.Count; i++)
                {
                    count += interactions[i].IsEnabled ? 1 : 0;
                }
            }

            return count;
        }
    }
}
