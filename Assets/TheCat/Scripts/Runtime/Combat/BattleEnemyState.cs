using System;
using TheCat.Data;
using TheCat.Data.Definitions;

namespace TheCat.Combat
{
    public sealed class BattleEnemyState
    {
        private float bossSummonRemainingSeconds;
        private float bossThrowRemainingSeconds;

        public BattleEnemyState(int instanceId, EnemyDefinition definition, float timeToBedSeconds, string spawnGateId = "")
        {
            if (instanceId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(instanceId), instanceId, "Instance id must be greater than zero.");
            }

            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            InstanceId = instanceId;
            SpawnGateId = spawnGateId ?? string.Empty;
            CurrentHp = definition.MaxHp;
            TimeToBedSeconds = Math.Max(0f, timeToBedSeconds);
            Statuses = new StatusEffectCollection();
            bossSummonRemainingSeconds = definition.BehaviorType == EnemyBehaviorType.BossCallTyrant ? 8f : float.MaxValue;
            bossThrowRemainingSeconds = definition.BehaviorType == EnemyBehaviorType.BossCallTyrant ? 5f : float.MaxValue;
        }

        public int InstanceId { get; }

        public EnemyDefinition Definition { get; }

        public string SpawnGateId { get; }

        public float CurrentHp { get; private set; }

        public float TimeToBedSeconds { get; private set; }

        public StatusEffectCollection Statuses { get; }

        public float BossSummonRemainingSeconds => bossSummonRemainingSeconds;

        public float BossThrowRemainingSeconds => bossThrowRemainingSeconds;

        public bool IsAlive => CurrentHp > 0f;

        public bool HasReachedBed => TimeToBedSeconds <= 0f;

        public float DamageTakenMultiplier
        {
            get
            {
                if (!Statuses.TryGet(StatusTagIds.Mark, out StatusEffectState mark))
                {
                    return 1f;
                }

                return 1f + mark.Magnitude;
            }
        }

        public float MovementRateMultiplier
        {
            get
            {
                if (!Statuses.TryGet(StatusTagIds.Slow, out StatusEffectState slow))
                {
                    return 1f;
                }

                float reduction = slow.Magnitude * Definition.SlowResponseMultiplier;
                return Clamp(1f - reduction, 0.1f, 1f);
            }
        }

        public void ApplyDamage(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Damage must not be negative.");
            }

            CurrentHp = Math.Max(0f, CurrentHp - amount);
        }

        public void ApplyStatus(StatusTagDefinition definition, float magnitudeOverride = -1f, float durationOverrideSeconds = -1f)
        {
            Statuses.Apply(definition, magnitudeOverride, durationOverrideSeconds);
        }

        public bool ApplyKnockback(float timeAddedSeconds)
        {
            if (timeAddedSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(timeAddedSeconds), timeAddedSeconds, "Knockback amount must not be negative.");
            }

            if (!Definition.CanBeKnockedBack || timeAddedSeconds <= 0f)
            {
                return false;
            }

            TimeToBedSeconds += timeAddedSeconds;
            return true;
        }

        public void Advance(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            TimeToBedSeconds = Math.Max(0f, TimeToBedSeconds - deltaSeconds * MovementRateMultiplier);
            Statuses.Tick(deltaSeconds);
        }

        public bool TickBossSummon(float deltaSeconds, float resetSeconds)
        {
            return TickBossTimer(deltaSeconds, resetSeconds, ref bossSummonRemainingSeconds);
        }

        public bool TickBossThrow(float deltaSeconds, float resetSeconds)
        {
            return TickBossTimer(deltaSeconds, resetSeconds, ref bossThrowRemainingSeconds);
        }

        public void DebugSetBossTimers(float summonRemainingSeconds, float throwRemainingSeconds)
        {
            if (summonRemainingSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(summonRemainingSeconds), summonRemainingSeconds, "Summon timer must not be negative.");
            }

            if (throwRemainingSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(throwRemainingSeconds), throwRemainingSeconds, "Throw timer must not be negative.");
            }

            if (Definition.BehaviorType != EnemyBehaviorType.BossCallTyrant)
            {
                return;
            }

            bossSummonRemainingSeconds = summonRemainingSeconds;
            bossThrowRemainingSeconds = throwRemainingSeconds;
        }

        private static bool TickBossTimer(float deltaSeconds, float resetSeconds, ref float remainingSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            if (resetSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(resetSeconds), resetSeconds, "Reset seconds must be greater than zero.");
            }

            if (float.IsPositiveInfinity(remainingSeconds) || remainingSeconds == float.MaxValue)
            {
                return false;
            }

            remainingSeconds -= deltaSeconds;
            if (remainingSeconds > 0f)
            {
                return false;
            }

            remainingSeconds += resetSeconds;
            if (remainingSeconds <= 0f)
            {
                remainingSeconds = resetSeconds;
            }

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
