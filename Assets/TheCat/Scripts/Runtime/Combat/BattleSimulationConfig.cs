using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;

namespace TheCat.Combat
{
    public sealed class BattleSimulationConfig
    {
        public BattleSimulationConfig(
            WaveDefinition wave,
            IEnumerable<EnemyDefinition> enemies,
            P0Tuning tuning,
            float startingSleep = OwnerSleepState.DefaultStart,
            float startingSleepMax = OwnerSleepState.DefaultBaseMax,
            float startingSleepBaseMax = OwnerSleepState.DefaultBaseMax,
            float startingPoop = 0f,
            float startingHunger = TeamHungerGauge.MaxValue,
            IEnumerable<StatusTagDefinition> statusTags = null,
            BattleModifierSet modifiers = null)
        {
            Wave = wave ?? throw new ArgumentNullException(nameof(wave));
            Tuning = tuning;
            ValidateSleepBounds(startingSleepMax, startingSleepBaseMax);
            StartingSleepBaseMax = startingSleepBaseMax;
            StartingSleepMax = startingSleepMax;
            StartingSleep = Clamp(startingSleep, 0f, StartingSleepMax);
            StartingPoop = Clamp(startingPoop, 0f, TeamPoopGauge.MaxValue);
            StartingHunger = Clamp(startingHunger, 0f, TeamHungerGauge.MaxValue);
            Modifiers = modifiers ?? BattleModifierSet.Neutral;

            EnemiesById = new Dictionary<string, EnemyDefinition>();
            if (enemies != null)
            {
                foreach (EnemyDefinition enemy in enemies)
                {
                    EnemiesById[enemy.Id] = enemy;
                }
            }

            StatusesById = new Dictionary<string, StatusTagDefinition>();
            if (statusTags != null)
            {
                foreach (StatusTagDefinition statusTag in statusTags)
                {
                    StatusesById[statusTag.Id] = statusTag;
                }
            }

            ValidateWaveReferences();
        }

        public WaveDefinition Wave { get; }

        public Dictionary<string, EnemyDefinition> EnemiesById { get; }

        public Dictionary<string, StatusTagDefinition> StatusesById { get; }

        public P0Tuning Tuning { get; }

        public float StartingSleep { get; }

        public float StartingSleepMax { get; }

        public float StartingSleepBaseMax { get; }

        public float StartingPoop { get; }

        public float StartingHunger { get; }

        public BattleModifierSet Modifiers { get; }

        private static void ValidateSleepBounds(float max, float baseMax)
        {
            if (baseMax <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(baseMax), baseMax, "Base max must be greater than zero.");
            }

            if (max <= 0f || max > baseMax)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max must be greater than zero and not exceed base max.");
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

        private void ValidateWaveReferences()
        {
            for (int i = 0; i < Wave.SpawnGroups.Count; i++)
            {
                string enemyId = Wave.SpawnGroups[i].EnemyId;
                if (!EnemiesById.ContainsKey(enemyId))
                {
                    throw new ArgumentException("Wave references an unknown enemy id: " + enemyId, nameof(Wave));
                }
            }
        }
    }
}
