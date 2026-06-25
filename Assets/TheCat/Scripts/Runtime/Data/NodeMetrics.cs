using System;

namespace TheCat.Data
{
    public sealed class NodeMetrics
    {
        public NodeMetrics(int layer, string nodeId, float startingSleep)
        {
            if (layer <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(layer), layer, "Layer must be greater than zero.");
            }

            if (string.IsNullOrWhiteSpace(nodeId))
            {
                throw new ArgumentException("Node id is required.", nameof(nodeId));
            }

            Layer = layer;
            NodeId = nodeId;
            StartingSleep = startingSleep;
            EndingSleep = startingSleep;
            Result = NodeResult.Unknown;
        }

        public int Layer { get; }

        public string NodeId { get; }

        public float StartingSleep { get; }

        public float EndingSleep { get; private set; }

        public float SleepDelta => EndingSleep - StartingSleep;

        public float DurationSeconds { get; private set; }

        public int LitterBoxUses { get; private set; }

        public int FeederUses { get; private set; }

        public int BedCareUses { get; private set; }

        public int PoopIncidents { get; private set; }

        public float SleepMaxLost { get; private set; }

        public int WeakIncidents { get; private set; }

        public int CatPressureEvents { get; private set; }

        public float CatDamageIncoming { get; private set; }

        public float CatDamageTaken { get; private set; }

        public float CatDamageAbsorbed { get; private set; }

        public int CatHealEvents { get; private set; }

        public float CatHealingApplied { get; private set; }

        public int CatShieldEvents { get; private set; }

        public float CatShieldApplied { get; private set; }

        public int BedPressureHits { get; private set; }

        public int BossThrowPressureHits { get; private set; }

        public int EnemySleepPressureEvents { get; private set; }

        public float EnemySleepDamageIncoming { get; private set; }

        public float EnemySleepDamageTaken { get; private set; }

        public float EnemySleepDamageAbsorbed { get; private set; }

        public int CatSwitchAttempts { get; private set; }

        public int CatSwitchesSucceeded { get; private set; }

        public int CatSwitchesBlockedByWeak { get; private set; }

        public int AutoTargetAttempts { get; private set; }

        public int AutoTargetsAcquired { get; private set; }

        public int SkillTargetAttempts { get; private set; }

        public int SkillTargetsAcquired { get; private set; }

        public int SkillCastAttempts { get; private set; }

        public int SkillCastsSucceeded { get; private set; }

        public int SkillCastsBlockedByCooldown { get; private set; }

        public int SkillCastsBlockedByTarget { get; private set; }

        public int SkillCastsBlockedByMissingDefinition { get; private set; }

        public int InteractionAttempts { get; private set; }

        public int InteractionSuccesses { get; private set; }

        public int InteractionBlockedByRange { get; private set; }

        public NodeResult Result { get; private set; }

        public bool IsComplete => Result != NodeResult.Unknown;

        public void RecordLitterBoxUse()
        {
            LitterBoxUses++;
            RecordInteractionSuccess();
        }

        public void RecordFeederUse()
        {
            FeederUses++;
            RecordInteractionSuccess();
        }

        public void RecordBedCareUse()
        {
            BedCareUses++;
            RecordInteractionSuccess();
        }

        public void RecordPoopIncident()
        {
            PoopIncidents++;
        }

        public void RecordSleepMaxLoss(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Sleep max loss must not be negative.");
            }

            SleepMaxLost += amount;
        }

        public void RecordWeakIncident()
        {
            WeakIncidents++;
        }

        public void RecordCatPressure(float incomingDamage, float damageTaken)
        {
            if (incomingDamage < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(incomingDamage), incomingDamage, "Incoming damage must not be negative.");
            }

            if (damageTaken < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damageTaken), damageTaken, "Damage taken must not be negative.");
            }

            CatPressureEvents++;
            CatDamageIncoming += incomingDamage;
            CatDamageTaken += damageTaken;
            CatDamageAbsorbed += Math.Max(0f, incomingDamage - damageTaken);
        }

        public void RecordCatHeal(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Cat healing amount must not be negative.");
            }

            if (amount <= 0f)
            {
                return;
            }

            CatHealEvents++;
            CatHealingApplied += amount;
        }

        public void RecordCatShield(float amount)
        {
            if (amount < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Cat shield amount must not be negative.");
            }

            if (amount <= 0f)
            {
                return;
            }

            CatShieldEvents++;
            CatShieldApplied += amount;
        }

        public void RecordBedPressure(float incomingDamage, float damageTaken)
        {
            BedPressureHits++;
            RecordEnemySleepPressure(incomingDamage, damageTaken);
        }

        public void RecordBossThrowPressure(float incomingDamage, float damageTaken)
        {
            BossThrowPressureHits++;
            RecordEnemySleepPressure(incomingDamage, damageTaken);
        }

        public void RecordCatSwitchSuccess()
        {
            CatSwitchAttempts++;
            CatSwitchesSucceeded++;
        }

        public void RecordCatSwitchBlockedByWeak()
        {
            CatSwitchAttempts++;
            CatSwitchesBlockedByWeak++;
        }

        public void RecordAutoTargetAcquired()
        {
            AutoTargetAttempts++;
            AutoTargetsAcquired++;
        }

        public void RecordAutoTargetMissed()
        {
            AutoTargetAttempts++;
        }

        public void RecordSkillTargetAcquired()
        {
            SkillTargetAttempts++;
            SkillTargetsAcquired++;
        }

        public void RecordSkillTargetMissed()
        {
            SkillTargetAttempts++;
        }

        public void RecordSkillCastSuccess()
        {
            SkillCastAttempts++;
            SkillCastsSucceeded++;
        }

        public void RecordSkillCastBlockedByCooldown()
        {
            SkillCastAttempts++;
            SkillCastsBlockedByCooldown++;
        }

        public void RecordSkillCastBlockedByTarget()
        {
            SkillCastAttempts++;
            SkillCastsBlockedByTarget++;
        }

        public void RecordSkillCastBlockedByMissingDefinition()
        {
            SkillCastAttempts++;
            SkillCastsBlockedByMissingDefinition++;
        }

        public void RecordInteractionBlockedByRange()
        {
            InteractionAttempts++;
            InteractionBlockedByRange++;
        }

        private void RecordInteractionSuccess()
        {
            InteractionAttempts++;
            InteractionSuccesses++;
        }

        private void RecordEnemySleepPressure(float incomingDamage, float damageTaken)
        {
            if (incomingDamage < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(incomingDamage), incomingDamage, "Incoming damage must not be negative.");
            }

            if (damageTaken < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damageTaken), damageTaken, "Damage taken must not be negative.");
            }

            EnemySleepPressureEvents++;
            EnemySleepDamageIncoming += incomingDamage;
            EnemySleepDamageTaken += damageTaken;
            EnemySleepDamageAbsorbed += Math.Max(0f, incomingDamage - damageTaken);
        }

        public void Complete(NodeResult result, float durationSeconds, float endingSleep)
        {
            if (result == NodeResult.Unknown)
            {
                throw new ArgumentException("Completed node metrics require a success or failure result.", nameof(result));
            }

            if (durationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(durationSeconds), durationSeconds, "Duration must not be negative.");
            }

            Result = result;
            DurationSeconds = durationSeconds;
            EndingSleep = endingSleep;
        }
    }
}
