using System;
using TheCat.Data;

namespace TheCat.Data.CoreValues
{
    public sealed class TeamHungerGauge
    {
        public const float MaxValue = 100f;
        public const float NaturalDrainPerSecond = 0.25f;
        public const float SmallSkillCost = 3f;
        public const float UltimateCost = 8f;
        public const float FeederRestoreAmount = 50f;
        public const float DigestionDurationSeconds = 45f;

        public TeamHungerGauge(float current = MaxValue)
        {
            Current = Clamp(current, 0f, MaxValue);
        }

        public float Current { get; private set; }

        public float DigestionRemainingSeconds { get; private set; }

        public bool IsDigesting => DigestionRemainingSeconds > 0f;

        public HungerStage Stage
        {
            get
            {
                if (Current <= 9f)
                {
                    return HungerStage.Empty;
                }

                if (Current <= 39f)
                {
                    return HungerStage.Starving;
                }

                if (Current <= 69f)
                {
                    return HungerStage.Hungry;
                }

                return HungerStage.Full;
            }
        }

        public float DamageMultiplier
        {
            get
            {
                switch (Stage)
                {
                    case HungerStage.Hungry:
                        return 0.9f;
                    case HungerStage.Starving:
                        return 0.8f;
                    case HungerStage.Empty:
                        return 0.65f;
                    default:
                        return 1f;
                }
            }
        }

        public void Tick(float deltaSeconds, P0Tuning tuning)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            Current = Clamp(Current - NaturalDrainPerSecond * tuning.HungerDrainMultiplier * deltaSeconds, 0f, MaxValue);
            DigestionRemainingSeconds = Math.Max(0f, DigestionRemainingSeconds - deltaSeconds);
        }

        public void SpendForSmallSkill()
        {
            Spend(SmallSkillCost);
        }

        public void SpendForUltimate()
        {
            Spend(UltimateCost);
        }

        public void Spend(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Spend amount must not be negative.");
            }

            Current = Clamp(Current - amount, 0f, MaxValue);
        }

        public void UseFeeder()
        {
            Current = Clamp(Current + FeederRestoreAmount, 0f, MaxValue);
            DigestionRemainingSeconds = DigestionDurationSeconds;
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
