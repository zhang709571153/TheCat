using System;

namespace TheCat.Combat
{
    public sealed class BattleModifierSet
    {
        public static readonly BattleModifierSet Neutral = new BattleModifierSet();

        public BattleModifierSet(
            float skillDamageMultiplier = 1f,
            float shieldMultiplier = 1f,
            float knockbackMultiplier = 1f,
            float enemyStatusDurationMultiplier = 1f,
            float ownerSleepRestoreMultiplier = 1f,
            float catHealMultiplier = 1f)
        {
            SkillDamageMultiplier = RequirePositive(skillDamageMultiplier, nameof(skillDamageMultiplier));
            ShieldMultiplier = RequirePositive(shieldMultiplier, nameof(shieldMultiplier));
            KnockbackMultiplier = RequirePositive(knockbackMultiplier, nameof(knockbackMultiplier));
            EnemyStatusDurationMultiplier = RequirePositive(enemyStatusDurationMultiplier, nameof(enemyStatusDurationMultiplier));
            OwnerSleepRestoreMultiplier = RequirePositive(ownerSleepRestoreMultiplier, nameof(ownerSleepRestoreMultiplier));
            CatHealMultiplier = RequirePositive(catHealMultiplier, nameof(catHealMultiplier));
        }

        public float SkillDamageMultiplier { get; }

        public float ShieldMultiplier { get; }

        public float KnockbackMultiplier { get; }

        public float EnemyStatusDurationMultiplier { get; }

        public float OwnerSleepRestoreMultiplier { get; }

        public float CatHealMultiplier { get; }

        private static float RequirePositive(float value, string name)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Multiplier must be greater than zero.");
            }

            return value;
        }
    }
}
