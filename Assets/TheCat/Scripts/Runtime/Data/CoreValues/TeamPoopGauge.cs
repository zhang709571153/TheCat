using System;
using TheCat.Data;

namespace TheCat.Data.CoreValues
{
    public sealed class TeamPoopGauge
    {
        public const float MaxValue = 100f;
        public const float ResetAfterIncident = 20f;
        public const float EarlyLayerCountdownSeconds = 30f;
        public const float StandardCountdownSeconds = 20f;

        private int pendingIncidents;

        public TeamPoopGauge(float current = 0f)
        {
            Current = Clamp(current, 0f, MaxValue);
        }

        public float Current { get; private set; }

        public float CountdownRemainingSeconds { get; private set; }

        public bool IsCountdownActive => CountdownRemainingSeconds > 0f;

        public PoopStage Stage
        {
            get
            {
                if (Current >= 86f)
                {
                    return PoopStage.Critical;
                }

                if (Current >= 61f)
                {
                    return PoopStage.High;
                }

                if (Current >= 31f)
                {
                    return PoopStage.Medium;
                }

                return PoopStage.Normal;
            }
        }

        public void Tick(float deltaSeconds, P0Tuning tuning, bool isDigesting, int layer)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            if (layer <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(layer), layer, "Layer must be greater than zero.");
            }

            if (IsCountdownActive)
            {
                CountdownRemainingSeconds = Math.Max(0f, CountdownRemainingSeconds - deltaSeconds);
                if (CountdownRemainingSeconds <= 0f)
                {
                    pendingIncidents++;
                    Current = ResetAfterIncident;
                }

                return;
            }

            float multiplier = isDigesting ? tuning.DigestionPoopMultiplier : 1f;
            Current = Clamp(Current + tuning.PoopNaturalGrowthPerSecond * multiplier * deltaSeconds, 0f, MaxValue);
            if (Current >= MaxValue)
            {
                CountdownRemainingSeconds = layer <= 3 ? EarlyLayerCountdownSeconds : StandardCountdownSeconds;
            }
        }

        public void UseLitterBox(P0Tuning tuning)
        {
            Current = Clamp(Current - tuning.LitterBoxPoopReduction, 0f, MaxValue);
            CountdownRemainingSeconds = 0f;
        }

        public float ExtendCountdown(float seconds)
        {
            if (seconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), seconds, "Countdown extension must not be negative.");
            }

            if (!IsCountdownActive || seconds <= 0f)
            {
                return 0f;
            }

            CountdownRemainingSeconds += seconds;
            return seconds;
        }

        public bool TryConsumeIncident()
        {
            if (pendingIncidents <= 0)
            {
                return false;
            }

            pendingIncidents--;
            return true;
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
