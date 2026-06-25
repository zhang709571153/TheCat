using System;

namespace TheCat.Data.Definitions
{
    public sealed class SpawnGroupDefinition
    {
        public SpawnGroupDefinition(string enemyId, int count, float startDelaySeconds, float intervalSeconds, string spawnGateId)
        {
            if (string.IsNullOrWhiteSpace(enemyId))
            {
                throw new ArgumentException("Enemy id is required.", nameof(enemyId));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
            }

            if (startDelaySeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(startDelaySeconds), startDelaySeconds, "Start delay must not be negative.");
            }

            if (intervalSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(intervalSeconds), intervalSeconds, "Interval must not be negative.");
            }

            EnemyId = enemyId;
            Count = count;
            StartDelaySeconds = startDelaySeconds;
            IntervalSeconds = intervalSeconds;
            SpawnGateId = spawnGateId ?? string.Empty;
        }

        public string EnemyId { get; }

        public int Count { get; }

        public float StartDelaySeconds { get; }

        public float IntervalSeconds { get; }

        public string SpawnGateId { get; }
    }
}
