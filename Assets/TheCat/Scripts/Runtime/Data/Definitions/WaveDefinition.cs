using System;
using System.Collections.Generic;

namespace TheCat.Data.Definitions
{
    public sealed class WaveDefinition
    {
        public WaveDefinition(int layer, string id, float targetDurationSeconds, IReadOnlyList<SpawnGroupDefinition> spawnGroups)
        {
            if (layer <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(layer), layer, "Layer must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Wave id is required.", nameof(id));
            }

            if (targetDurationSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(targetDurationSeconds), targetDurationSeconds, "Target duration must be greater than zero.");
            }

            Layer = layer;
            Id = id;
            TargetDurationSeconds = targetDurationSeconds;
            SpawnGroups = spawnGroups == null ? Array.Empty<SpawnGroupDefinition>() : new List<SpawnGroupDefinition>(spawnGroups).AsReadOnly();
        }

        public int Layer { get; }

        public string Id { get; }

        public float TargetDurationSeconds { get; }

        public IReadOnlyList<SpawnGroupDefinition> SpawnGroups { get; }
    }
}
