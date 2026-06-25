using System;

namespace TheCat.Gameplay
{
    public sealed class P0RuntimeSettings
    {
        private const float MinimumBattleSpeed = 0.25f;
        private const float MaximumBattleSpeed = 2f;

        public bool IsPaused { get; private set; }

        public float BattleSpeedMultiplier { get; private set; } = 1f;

        public float ApplyToDelta(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            return IsPaused ? 0f : deltaSeconds * BattleSpeedMultiplier;
        }

        public void SetPaused(bool paused)
        {
            IsPaused = paused;
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
        }

        public void SetBattleSpeed(float multiplier)
        {
            if (multiplier < MinimumBattleSpeed || multiplier > MaximumBattleSpeed)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier), multiplier, "Battle speed is outside the P0 supported range.");
            }

            BattleSpeedMultiplier = multiplier;
        }

        public void Reset()
        {
            IsPaused = false;
            BattleSpeedMultiplier = 1f;
        }
    }
}
