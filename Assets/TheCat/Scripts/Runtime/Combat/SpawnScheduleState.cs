using TheCat.Data.Definitions;

namespace TheCat.Combat
{
    internal sealed class SpawnScheduleState
    {
        public SpawnScheduleState(SpawnGroupDefinition definition)
        {
            Definition = definition;
            NextSpawnTimeSeconds = definition.StartDelaySeconds;
        }

        public SpawnGroupDefinition Definition { get; }

        public int SpawnedCount { get; private set; }

        public float NextSpawnTimeSeconds { get; private set; }

        public bool IsComplete => SpawnedCount >= Definition.Count;

        public bool CanSpawn(float battleTimeSeconds)
        {
            return !IsComplete && battleTimeSeconds >= NextSpawnTimeSeconds;
        }

        public void MarkSpawned()
        {
            SpawnedCount++;
            NextSpawnTimeSeconds += Definition.IntervalSeconds;
        }
    }
}
