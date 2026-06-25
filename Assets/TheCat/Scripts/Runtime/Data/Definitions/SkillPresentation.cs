using System;

namespace TheCat.Data.Definitions
{
    public readonly struct SkillPresentation
    {
        public SkillPresentation(
            string skillId,
            string displayName,
            string roleHint,
            string effectHint,
            string voiceLine)
        {
            if (string.IsNullOrWhiteSpace(skillId))
            {
                throw new ArgumentException("Skill id is required.", nameof(skillId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Display name is required.", nameof(displayName));
            }

            SkillId = skillId;
            DisplayName = displayName;
            RoleHint = roleHint ?? string.Empty;
            EffectHint = effectHint ?? string.Empty;
            VoiceLine = voiceLine ?? string.Empty;
        }

        public string SkillId { get; }

        public string DisplayName { get; }

        public string RoleHint { get; }

        public string EffectHint { get; }

        public string VoiceLine { get; }

        public string BuildSummary(SkillDefinition skill)
        {
            string summary = DisplayName;
            if (skill != null)
            {
                summary += " 冷却 " + skill.CooldownSeconds.ToString("0.#") + "s";
                summary += " 消耗 " + skill.HungerCost.ToString("0.#") + " 饱肚度";
            }

            if (!string.IsNullOrWhiteSpace(EffectHint))
            {
                summary += " | " + EffectHint;
            }

            return summary;
        }
    }
}
