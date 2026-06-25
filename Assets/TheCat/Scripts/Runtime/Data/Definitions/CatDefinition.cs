using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public sealed class CatDefinition
    {
        public CatDefinition(
            string id,
            string displayName,
            CatRole role,
            string authorityId,
            string attributeId,
            float maxHp,
            float physicalDefense,
            float magicalDefense,
            float moveSpeedMultiplier,
            IReadOnlyList<string> skillIds)
            : this(
                id,
                displayName,
                role,
                authorityId,
                attributeId,
                maxHp,
                physicalDefense,
                magicalDefense,
                moveSpeedMultiplier,
                skillIds,
                default(P0VisualAssetReference))
        {
        }

        public CatDefinition(
            string id,
            string displayName,
            CatRole role,
            string authorityId,
            string attributeId,
            float maxHp,
            float physicalDefense,
            float magicalDefense,
            float moveSpeedMultiplier,
            IReadOnlyList<string> skillIds,
            P0VisualAssetReference combatSprite)
        {
            RequireText(id, nameof(id));
            RequireText(displayName, nameof(displayName));
            RequireText(authorityId, nameof(authorityId));
            RequireText(attributeId, nameof(attributeId));
            RequirePositive(maxHp, nameof(maxHp));
            RequireNonNegative(physicalDefense, nameof(physicalDefense));
            RequireNonNegative(magicalDefense, nameof(magicalDefense));
            RequirePositive(moveSpeedMultiplier, nameof(moveSpeedMultiplier));

            Id = id;
            DisplayName = displayName;
            Role = role;
            AuthorityId = authorityId;
            AttributeId = attributeId;
            MaxHp = maxHp;
            PhysicalDefense = physicalDefense;
            MagicalDefense = magicalDefense;
            MoveSpeedMultiplier = moveSpeedMultiplier;
            SkillIds = skillIds == null ? Array.Empty<string>() : new List<string>(skillIds).AsReadOnly();
            CombatSprite = combatSprite;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public CatRole Role { get; }

        public string AuthorityId { get; }

        public string AttributeId { get; }

        public float MaxHp { get; }

        public float PhysicalDefense { get; }

        public float MagicalDefense { get; }

        public float MoveSpeedMultiplier { get; }

        public IReadOnlyList<string> SkillIds { get; }

        public P0VisualAssetReference CombatSprite { get; }

        private static void RequireText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", name);
            }
        }

        private static void RequirePositive(float value, string name)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be greater than zero.");
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
