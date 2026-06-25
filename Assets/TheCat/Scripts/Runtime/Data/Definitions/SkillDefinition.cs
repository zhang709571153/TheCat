using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public sealed class SkillDefinition
    {
        public SkillDefinition(
            string id,
            string ownerCatId,
            SkillSlot slot,
            float cooldownSeconds,
            float hungerCost,
            SkillTargetingMode targetingMode,
            IReadOnlyList<SkillEffectDefinition> effects)
        {
            RequireText(id, nameof(id));
            RequireText(ownerCatId, nameof(ownerCatId));
            RequireNonNegative(cooldownSeconds, nameof(cooldownSeconds));
            RequireNonNegative(hungerCost, nameof(hungerCost));

            Id = id;
            OwnerCatId = ownerCatId;
            Slot = slot;
            CooldownSeconds = cooldownSeconds;
            HungerCost = hungerCost;
            TargetingMode = targetingMode;
            Effects = effects == null ? Array.Empty<SkillEffectDefinition>() : new List<SkillEffectDefinition>(effects).AsReadOnly();
        }

        public string Id { get; }

        public string OwnerCatId { get; }

        public SkillSlot Slot { get; }

        public float CooldownSeconds { get; }

        public float HungerCost { get; }

        public SkillTargetingMode TargetingMode { get; }

        public IReadOnlyList<SkillEffectDefinition> Effects { get; }

        private static void RequireText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", name);
            }
        }

        private static void RequireNonNegative(float value, string name)
        {
            if (value < 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }
        }
    }
}
