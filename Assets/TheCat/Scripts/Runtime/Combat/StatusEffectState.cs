using System;
using TheCat.Data;

namespace TheCat.Combat
{
    public sealed class StatusEffectState
    {
        internal StatusEffectState(StatusTagDefinition definition, float magnitude, float remainingSeconds)
        {
            Definition = definition;
            Magnitude = magnitude;
            RemainingSeconds = remainingSeconds;
            StackCount = 1;
        }

        public StatusTagDefinition Definition { get; }

        public string Id => Definition.Id;

        public float Magnitude { get; private set; }

        public float RemainingSeconds { get; private set; }

        public int StackCount { get; private set; }

        public bool IsExpired => RemainingSeconds <= 0f || Magnitude <= 0f;

        internal void Tick(float deltaSeconds)
        {
            RemainingSeconds = Math.Max(0f, RemainingSeconds - deltaSeconds);
        }

        internal void Refresh(float magnitude, float remainingSeconds)
        {
            Magnitude = magnitude;
            RemainingSeconds = remainingSeconds;
        }

        internal void UseHighest(float magnitude, float remainingSeconds)
        {
            Magnitude = Math.Max(Magnitude, magnitude);
            RemainingSeconds = Math.Max(RemainingSeconds, remainingSeconds);
        }

        internal void AddStack(float magnitude, float remainingSeconds)
        {
            Magnitude += magnitude;
            RemainingSeconds = Math.Max(RemainingSeconds, remainingSeconds);
            StackCount++;
        }

        internal float ConsumeMagnitude(float amount)
        {
            float consumed = Math.Min(Magnitude, amount);
            Magnitude -= consumed;
            return consumed;
        }
    }
}
