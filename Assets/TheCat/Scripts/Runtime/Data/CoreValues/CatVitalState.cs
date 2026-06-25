using System;

namespace TheCat.Data.CoreValues
{
    public sealed class CatVitalState
    {
        public const float DefaultWeakDurationSeconds = 20f;
        public const float DefaultWeakRecoveryRatio = 0.3f;

        public CatVitalState(string catId, float maxHp, float currentHp = -1f, float weakRemainingSeconds = 0f)
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
            CurrentHp = currentHp < 0f ? maxHp : Clamp(currentHp, 0f, maxHp);
            WeakRemainingSeconds = weakRemainingSeconds;
        }

        public string CatId { get; }

        public float MaxHp { get; }

        public float CurrentHp { get; private set; }

        public float WeakRemainingSeconds { get; private set; }

        public bool IsWeak => WeakRemainingSeconds > 0f;

        public bool CanSwitchTo => !IsWeak;

        public void ApplyDamage(float amount, float weakDurationSeconds = DefaultWeakDurationSeconds)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Damage must not be negative.");
            }

            if (weakDurationSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(weakDurationSeconds), weakDurationSeconds, "Weak duration must be greater than zero.");
            }

            if (IsWeak)
            {
                return;
            }

            CurrentHp = Clamp(CurrentHp - amount, 0f, MaxHp);
            if (CurrentHp <= 0f)
            {
                WeakRemainingSeconds = weakDurationSeconds;
            }
        }

        public void Heal(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Heal amount must not be negative.");
            }

            if (IsWeak)
            {
                return;
            }

            CurrentHp = Clamp(CurrentHp + amount, 0f, MaxHp);
        }

        public void RestoreToAtLeast(float targetHp, bool clearWeak = true)
        {
            if (targetHp < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(targetHp), targetHp, "Target HP must not be negative.");
            }

            if (clearWeak)
            {
                WeakRemainingSeconds = 0f;
            }

            if (CurrentHp < targetHp)
            {
                CurrentHp = Clamp(targetHp, 0f, MaxHp);
            }
        }

        public void Tick(float deltaSeconds, float recoveryRatio = DefaultWeakRecoveryRatio)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            if (recoveryRatio <= 0f || recoveryRatio > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(recoveryRatio), recoveryRatio, "Recovery ratio must be between zero and one.");
            }

            if (!IsWeak)
            {
                return;
            }

            WeakRemainingSeconds = Math.Max(0f, WeakRemainingSeconds - deltaSeconds);
            if (WeakRemainingSeconds <= 0f)
            {
                CurrentHp = MaxHp * recoveryRatio;
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
