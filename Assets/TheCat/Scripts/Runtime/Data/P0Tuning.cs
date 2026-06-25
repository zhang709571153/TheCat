using System;

namespace TheCat.Data
{
    public readonly struct P0Tuning
    {
        public P0Tuning(
            float ownerSleepDamageMultiplier,
            int poopSleepMaxPenalty,
            float poopNaturalGrowthPerSecond,
            float digestionPoopMultiplier,
            float litterBoxPoopReduction,
            float hungerDrainMultiplier,
            float layerDifficultyMultiplier)
        {
            OwnerSleepDamageMultiplier = ownerSleepDamageMultiplier;
            PoopSleepMaxPenalty = poopSleepMaxPenalty;
            PoopNaturalGrowthPerSecond = poopNaturalGrowthPerSecond;
            DigestionPoopMultiplier = digestionPoopMultiplier;
            LitterBoxPoopReduction = litterBoxPoopReduction;
            HungerDrainMultiplier = hungerDrainMultiplier;
            LayerDifficultyMultiplier = layerDifficultyMultiplier;

            Validate();
        }

        public float OwnerSleepDamageMultiplier { get; }

        public int PoopSleepMaxPenalty { get; }

        public float PoopNaturalGrowthPerSecond { get; }

        public float DigestionPoopMultiplier { get; }

        public float LitterBoxPoopReduction { get; }

        public float HungerDrainMultiplier { get; }

        public float LayerDifficultyMultiplier { get; }

        public static P0Tuning Default => new P0Tuning(
            ownerSleepDamageMultiplier: 1f,
            poopSleepMaxPenalty: 10,
            poopNaturalGrowthPerSecond: 0.3f,
            digestionPoopMultiplier: 2f,
            litterBoxPoopReduction: 60f,
            hungerDrainMultiplier: 1f,
            layerDifficultyMultiplier: 1f);

        public P0Tuning With(
            float? ownerSleepDamageMultiplier = null,
            int? poopSleepMaxPenalty = null,
            float? poopNaturalGrowthPerSecond = null,
            float? digestionPoopMultiplier = null,
            float? litterBoxPoopReduction = null,
            float? hungerDrainMultiplier = null,
            float? layerDifficultyMultiplier = null)
        {
            return new P0Tuning(
                ownerSleepDamageMultiplier ?? OwnerSleepDamageMultiplier,
                poopSleepMaxPenalty ?? PoopSleepMaxPenalty,
                poopNaturalGrowthPerSecond ?? PoopNaturalGrowthPerSecond,
                digestionPoopMultiplier ?? DigestionPoopMultiplier,
                litterBoxPoopReduction ?? LitterBoxPoopReduction,
                hungerDrainMultiplier ?? HungerDrainMultiplier,
                layerDifficultyMultiplier ?? LayerDifficultyMultiplier);
        }

        private void Validate()
        {
            RequirePositive(nameof(OwnerSleepDamageMultiplier), OwnerSleepDamageMultiplier);
            RequireNonNegative(nameof(PoopSleepMaxPenalty), PoopSleepMaxPenalty);
            RequirePositive(nameof(PoopNaturalGrowthPerSecond), PoopNaturalGrowthPerSecond);
            RequirePositive(nameof(DigestionPoopMultiplier), DigestionPoopMultiplier);
            RequirePositive(nameof(LitterBoxPoopReduction), LitterBoxPoopReduction);
            RequirePositive(nameof(HungerDrainMultiplier), HungerDrainMultiplier);
            RequirePositive(nameof(LayerDifficultyMultiplier), LayerDifficultyMultiplier);
        }

        private static void RequirePositive(string name, float value)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be greater than zero.");
            }
        }

        private static void RequireNonNegative(string name, int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }
        }
    }
}
