using System;

namespace TheCat.Data.CoreValues
{
    public sealed class OwnerSleepState
    {
        public const float DefaultBaseMax = 100f;
        public const float DefaultStart = 100f;
        public const float DefaultMinimumMax = 50f;

        public OwnerSleepState(float current = DefaultStart, float max = DefaultBaseMax, float baseMax = DefaultBaseMax)
        {
            if (baseMax <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(baseMax), baseMax, "Base max must be greater than zero.");
            }

            if (max <= 0f || max > baseMax)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max must be greater than zero and not exceed base max.");
            }

            BaseMax = baseMax;
            Max = max;
            Current = Clamp(current, 0f, max);
        }

        public float Current { get; private set; }

        public float Max { get; private set; }

        public float BaseMax { get; }

        public bool IsFailed => Current <= 0f;

        public OwnerSleepStage Stage
        {
            get
            {
                if (IsFailed)
                {
                    return OwnerSleepStage.Failed;
                }

                float ratio = Current / Max;
                if (ratio <= 0.15f)
                {
                    return OwnerSleepStage.Critical;
                }

                if (ratio <= 0.3f)
                {
                    return OwnerSleepStage.Danger;
                }

                if (ratio <= 0.6f)
                {
                    return OwnerSleepStage.Uneasy;
                }

                return OwnerSleepStage.Stable;
            }
        }

        public void ApplyDamage(float amount, float multiplier = 1f)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Damage must not be negative.");
            }

            if (multiplier < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier), multiplier, "Multiplier must not be negative.");
            }

            Current = Clamp(Current - amount * multiplier, 0f, Max);
        }

        public void Restore(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Restore amount must not be negative.");
            }

            Current = Clamp(Current + amount, 0f, Max);
        }

        public float ApplyMaxPenalty(float amount, float minimumMax = DefaultMinimumMax)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Penalty must not be negative.");
            }

            if (minimumMax <= 0f || minimumMax > BaseMax)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumMax), minimumMax, "Minimum max must be greater than zero and not exceed base max.");
            }

            float oldMax = Max;
            Max = Clamp(Max - amount, minimumMax, BaseMax);
            Current = Clamp(Current, 0f, Max);
            return oldMax - Max;
        }

        private static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                return min;
            }

            return value > max ? max : value;
        }
    }
}
