using System;

namespace TheCat.Roguelite
{
    public readonly struct RunCatVitalSnapshot
    {
        public RunCatVitalSnapshot(string catId, float maxHp, float currentHp, float weakRemainingSeconds)
        {
            if (string.IsNullOrWhiteSpace(catId))
            {
                throw new ArgumentException("Cat id is required.", nameof(catId));
            }

            if (maxHp <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(maxHp), maxHp, "Max HP must be greater than zero.");
            }

            if (weakRemainingSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(weakRemainingSeconds), weakRemainingSeconds, "Weak time must not be negative.");
            }

            CatId = catId;
            MaxHp = maxHp;
            CurrentHp = Clamp(currentHp, 0f, maxHp);
            WeakRemainingSeconds = weakRemainingSeconds;
        }

        public string CatId { get; }

        public float MaxHp { get; }

        public float CurrentHp { get; }

        public float WeakRemainingSeconds { get; }

        public bool IsWeak => WeakRemainingSeconds > 0f;

        public float HpRatio => CurrentHp / MaxHp;

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
