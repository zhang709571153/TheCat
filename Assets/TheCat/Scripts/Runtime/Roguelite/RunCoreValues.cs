using System;
using TheCat.Data.CoreValues;

namespace TheCat.Roguelite
{
    public sealed class RunCoreValues
    {
        public const float RestNestOwnerSleepRestoreAmount = 25f;
        public const float RestNestPoopReductionAmount = 30f;
        public const float RestNestHungerSafeLine = 80f;

        public RunCoreValues(
            float ownerSleepCurrent = OwnerSleepState.DefaultStart,
            float ownerSleepMax = OwnerSleepState.DefaultBaseMax,
            float ownerSleepBaseMax = OwnerSleepState.DefaultBaseMax,
            float teamPoop = 0f,
            float teamHunger = TeamHungerGauge.MaxValue)
        {
            Capture(ownerSleepCurrent, ownerSleepMax, ownerSleepBaseMax, teamPoop, teamHunger);
        }

        public float OwnerSleepCurrent { get; private set; }

        public float OwnerSleepMax { get; private set; }

        public float OwnerSleepBaseMax { get; private set; }

        public float TeamPoop { get; private set; }

        public float TeamHunger { get; private set; }

        public void Capture(OwnerSleepState ownerSleep, TeamPoopGauge teamPoop, TeamHungerGauge teamHunger)
        {
            if (ownerSleep == null)
            {
                throw new ArgumentNullException(nameof(ownerSleep));
            }

            if (teamPoop == null)
            {
                throw new ArgumentNullException(nameof(teamPoop));
            }

            if (teamHunger == null)
            {
                throw new ArgumentNullException(nameof(teamHunger));
            }

            Capture(ownerSleep.Current, ownerSleep.Max, ownerSleep.BaseMax, teamPoop.Current, teamHunger.Current);
        }

        public void Capture(
            float ownerSleepCurrent,
            float ownerSleepMax,
            float ownerSleepBaseMax,
            float teamPoop,
            float teamHunger)
        {
            if (ownerSleepBaseMax <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(ownerSleepBaseMax), ownerSleepBaseMax, "Base max must be greater than zero.");
            }

            if (ownerSleepMax <= 0f || ownerSleepMax > ownerSleepBaseMax)
            {
                throw new ArgumentOutOfRangeException(nameof(ownerSleepMax), ownerSleepMax, "Max must be greater than zero and not exceed base max.");
            }

            OwnerSleepBaseMax = ownerSleepBaseMax;
            OwnerSleepMax = ownerSleepMax;
            OwnerSleepCurrent = Clamp(ownerSleepCurrent, 0f, OwnerSleepMax);
            TeamPoop = Clamp(teamPoop, 0f, TeamPoopGauge.MaxValue);
            TeamHunger = Clamp(teamHunger, 0f, TeamHungerGauge.MaxValue);
        }

        public void ApplyRestNestRecovery(
            float ownerSleepRestoreAmount = RestNestOwnerSleepRestoreAmount,
            float poopReductionAmount = RestNestPoopReductionAmount,
            float hungerSafeLine = RestNestHungerSafeLine)
        {
            RequireNonNegative(ownerSleepRestoreAmount, nameof(ownerSleepRestoreAmount));
            RequireNonNegative(poopReductionAmount, nameof(poopReductionAmount));
            if (hungerSafeLine < 0f || hungerSafeLine > TeamHungerGauge.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(hungerSafeLine), hungerSafeLine, "Hunger safe line must fit the hunger gauge.");
            }

            OwnerSleepCurrent = Clamp(OwnerSleepCurrent + ownerSleepRestoreAmount, 0f, OwnerSleepMax);
            TeamPoop = Clamp(TeamPoop - poopReductionAmount, 0f, TeamPoopGauge.MaxValue);
            if (TeamHunger < hungerSafeLine)
            {
                TeamHunger = hungerSafeLine;
            }
        }

        public void RestoreOwnerSleep(float amount)
        {
            RequireNonNegative(amount, nameof(amount));
            OwnerSleepCurrent = Clamp(OwnerSleepCurrent + amount, 0f, OwnerSleepMax);
        }

        public void DamageOwnerSleep(float amount)
        {
            RequireNonNegative(amount, nameof(amount));
            OwnerSleepCurrent = Clamp(OwnerSleepCurrent - amount, 0f, OwnerSleepMax);
        }

        public void ReducePoop(float amount)
        {
            RequireNonNegative(amount, nameof(amount));
            TeamPoop = Clamp(TeamPoop - amount, 0f, TeamPoopGauge.MaxValue);
        }

        public void IncreasePoop(float amount)
        {
            RequireNonNegative(amount, nameof(amount));
            TeamPoop = Clamp(TeamPoop + amount, 0f, TeamPoopGauge.MaxValue);
        }

        public void RestoreHungerToSafeLine(float safeLine)
        {
            RequireNonNegative(safeLine, nameof(safeLine));
            if (safeLine > TeamHungerGauge.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(safeLine), safeLine, "Hunger safe line must fit the hunger gauge.");
            }

            if (TeamHunger < safeLine)
            {
                TeamHunger = safeLine;
            }
        }

        private static void RequireNonNegative(float value, string name)
        {
            if (value < 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }
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
