using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0StatusIndicatorPresenter
    {
        public static P0StatusIndicatorState Build(StatusEffectCollection statuses, string ownerLabel = "")
        {
            if (statuses == null || statuses.Count == 0)
            {
                return default(P0StatusIndicatorState);
            }

            List<StatusEffectState> effects = new List<StatusEffectState>(statuses.ActiveEffects);
            effects.Sort(CompareStatusPriority);
            List<string> lines = new List<string>();
            if (!string.IsNullOrWhiteSpace(ownerLabel))
            {
                lines.Add(ownerLabel);
            }

            for (int i = 0; i < effects.Count; i++)
            {
                lines.Add(StatusDisplayFormatter.Format(effects[i]));
            }

            Color accentColor = effects.Count == 0
                ? Color.white
                : GetAccentColor(effects[0].Id);
            return new P0StatusIndicatorState(string.Join("\n", lines), accentColor, effects.Count);
        }

        public static Color GetAccentColor(string statusTagId)
        {
            switch (statusTagId)
            {
                case StatusTagIds.SleepStable:
                    return new Color(0.45f, 0.75f, 1f, 0.95f);
                case StatusTagIds.Slow:
                    return new Color(0.35f, 0.9f, 1f, 0.95f);
                case StatusTagIds.Knockback:
                    return new Color(0.9f, 0.92f, 1f, 0.95f);
                case StatusTagIds.Mark:
                    return new Color(0.9f, 0.45f, 1f, 0.95f);
                case StatusTagIds.Shield:
                    return new Color(1f, 0.85f, 0.25f, 0.95f);
                default:
                    return new Color(1f, 1f, 1f, 0.9f);
            }
        }

        private static int CompareStatusPriority(StatusEffectState left, StatusEffectState right)
        {
            int leftPriority = GetPriority(left == null ? string.Empty : left.Id);
            int rightPriority = GetPriority(right == null ? string.Empty : right.Id);
            int comparison = leftPriority.CompareTo(rightPriority);
            if (comparison != 0)
            {
                return comparison;
            }

            string leftId = left == null ? string.Empty : left.Id;
            string rightId = right == null ? string.Empty : right.Id;
            return string.CompareOrdinal(leftId, rightId);
        }

        private static int GetPriority(string statusTagId)
        {
            switch (statusTagId)
            {
                case StatusTagIds.Shield:
                    return 0;
                case StatusTagIds.SleepStable:
                    return 1;
                case StatusTagIds.Mark:
                    return 2;
                case StatusTagIds.Slow:
                    return 3;
                case StatusTagIds.Knockback:
                    return 4;
                default:
                    return 99;
            }
        }
    }
}
