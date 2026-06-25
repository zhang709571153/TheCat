using System;

namespace TheCat.Data.Definitions
{
    public sealed class EnemyDefinition
    {
        public EnemyDefinition(
            string id,
            string displayName,
            EnemyBehaviorType behaviorType,
            float maxHp,
            float moveSpeed,
            float playerDamage,
            float bedDamage,
            bool canBeKnockedBack,
            float slowResponseMultiplier)
        {
            RequireText(id, nameof(id));
            RequireText(displayName, nameof(displayName));
            RequirePositive(maxHp, nameof(maxHp));
            RequireNonNegative(moveSpeed, nameof(moveSpeed));
            RequireNonNegative(playerDamage, nameof(playerDamage));
            RequireNonNegative(bedDamage, nameof(bedDamage));
            RequirePositive(slowResponseMultiplier, nameof(slowResponseMultiplier));

            Id = id;
            DisplayName = displayName;
            BehaviorType = behaviorType;
            MaxHp = maxHp;
            MoveSpeed = moveSpeed;
            PlayerDamage = playerDamage;
            BedDamage = bedDamage;
            CanBeKnockedBack = canBeKnockedBack;
            SlowResponseMultiplier = slowResponseMultiplier;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public EnemyBehaviorType BehaviorType { get; }

        public float MaxHp { get; }

        public float MoveSpeed { get; }

        public float PlayerDamage { get; }

        public float BedDamage { get; }

        public bool CanBeKnockedBack { get; }

        public float SlowResponseMultiplier { get; }

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
