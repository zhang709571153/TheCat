using System;

namespace TheCat.Data.Definitions
{
    public sealed class SkillEffectDefinition
    {
        public SkillEffectDefinition(SkillEffectType effectType, float magnitude, string statusTagId = "")
        {
            if (magnitude < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(magnitude), magnitude, "Magnitude must not be negative.");
            }

            EffectType = effectType;
            Magnitude = magnitude;
            StatusTagId = statusTagId ?? string.Empty;
        }

        public SkillEffectType EffectType { get; }

        public float Magnitude { get; }

        public string StatusTagId { get; }
    }
}
