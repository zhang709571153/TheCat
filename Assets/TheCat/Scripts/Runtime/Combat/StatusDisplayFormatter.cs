using System.Text;

namespace TheCat.Combat
{
    public static class StatusDisplayFormatter
    {
        public static string Format(StatusEffectState effect)
        {
            if (effect == null)
            {
                return string.Empty;
            }

            string displayName = string.IsNullOrWhiteSpace(effect.Definition.DisplayName)
                ? effect.Id
                : effect.Definition.DisplayName;
            return displayName
                + " 强度 "
                + effect.Magnitude.ToString("0.##")
                + " "
                + effect.RemainingSeconds.ToString("0.0")
                + "s";
        }

        public static string FormatCollection(StatusEffectCollection statuses)
        {
            if (statuses == null || statuses.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach (StatusEffectState effect in statuses.ActiveEffects)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" | ");
                }

                builder.Append(Format(effect));
            }

            return builder.ToString();
        }
    }
}
