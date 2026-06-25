using System;
using System.Collections.Generic;
using TheCat.Data;

namespace TheCat.Combat
{
    public sealed class StatusEffectCollection
    {
        private readonly Dictionary<string, StatusEffectState> effectsById = new Dictionary<string, StatusEffectState>();

        public IReadOnlyCollection<StatusEffectState> ActiveEffects => effectsById.Values;

        public int Count => effectsById.Count;

        public bool Has(string statusTagId)
        {
            return !string.IsNullOrWhiteSpace(statusTagId) && effectsById.ContainsKey(statusTagId);
        }

        public bool TryGet(string statusTagId, out StatusEffectState effect)
        {
            if (string.IsNullOrWhiteSpace(statusTagId))
            {
                effect = null;
                return false;
            }

            return effectsById.TryGetValue(statusTagId, out effect);
        }

        public StatusEffectState Apply(
            StatusTagDefinition definition,
            float magnitudeOverride = -1f,
            float durationOverrideSeconds = -1f)
        {
            if (string.IsNullOrWhiteSpace(definition.Id))
            {
                throw new ArgumentException("Status definition id is required.", nameof(definition));
            }

            float magnitude = magnitudeOverride >= 0f ? magnitudeOverride : definition.Magnitude;
            float duration = durationOverrideSeconds >= 0f ? durationOverrideSeconds : definition.BaseDurationSeconds;
            if (magnitude < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(magnitudeOverride), magnitudeOverride, "Magnitude must not be negative.");
            }

            if (duration < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(durationOverrideSeconds), durationOverrideSeconds, "Duration must not be negative.");
            }

            if (!effectsById.TryGetValue(definition.Id, out StatusEffectState existing))
            {
                StatusEffectState created = new StatusEffectState(definition, magnitude, duration);
                effectsById.Add(definition.Id, created);
                return created;
            }

            switch (definition.StackPolicy)
            {
                case StatusStackPolicy.RefreshDuration:
                    existing.Refresh(magnitude, duration);
                    break;
                case StatusStackPolicy.HighestMagnitude:
                    existing.UseHighest(magnitude, duration);
                    break;
                case StatusStackPolicy.AddStack:
                    existing.AddStack(magnitude, duration);
                    break;
                case StatusStackPolicy.Unique:
                    existing.UseHighest(magnitude, duration);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(definition), definition.StackPolicy, "Unknown status stack policy.");
            }

            return existing;
        }

        public void Tick(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            List<string> expiredIds = null;
            foreach (KeyValuePair<string, StatusEffectState> pair in effectsById)
            {
                pair.Value.Tick(deltaSeconds);
                if (pair.Value.IsExpired)
                {
                    if (expiredIds == null)
                    {
                        expiredIds = new List<string>();
                    }

                    expiredIds.Add(pair.Key);
                }
            }

            if (expiredIds == null)
            {
                return;
            }

            for (int i = 0; i < expiredIds.Count; i++)
            {
                effectsById.Remove(expiredIds[i]);
            }
        }

        public float AbsorbDamage(string shieldStatusTagId, float damage)
        {
            if (damage < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), damage, "Damage must not be negative.");
            }

            if (!TryGet(shieldStatusTagId, out StatusEffectState shield))
            {
                return damage;
            }

            float absorbed = shield.ConsumeMagnitude(damage);
            if (shield.IsExpired)
            {
                effectsById.Remove(shieldStatusTagId);
            }

            return damage - absorbed;
        }
    }
}
