namespace TheCat.Data
{
    public readonly struct RunMetricsSummary
    {
        public RunMetricsSummary(
            int nodeCount,
            int successCount,
            int failureCount,
            float durationSeconds,
            float sleepDelta,
            int litterBoxUses,
            int feederUses,
            int bedCareUses,
            int poopIncidents,
            float sleepMaxLost,
            int weakIncidents,
            int catPressureEvents,
            float catDamageIncoming,
            float catDamageTaken,
            float catDamageAbsorbed,
            int catHealEvents,
            float catHealingApplied,
            int catShieldEvents,
            float catShieldApplied,
            int bedPressureHits,
            int bossThrowPressureHits,
            int enemySleepPressureEvents,
            float enemySleepDamageIncoming,
            float enemySleepDamageTaken,
            float enemySleepDamageAbsorbed,
            int catSwitchAttempts,
            int catSwitchesSucceeded,
            int catSwitchesBlockedByWeak,
            int autoTargetAttempts,
            int autoTargetsAcquired,
            int skillTargetAttempts,
            int skillTargetsAcquired,
            int skillCastAttempts,
            int skillCastsSucceeded,
            int skillCastsBlockedByCooldown,
            int skillCastsBlockedByTarget,
            int skillCastsBlockedByMissingDefinition,
            int interactionAttempts,
            int interactionSuccesses,
            int interactionBlockedByRange)
        {
            NodeCount = nodeCount;
            SuccessCount = successCount;
            FailureCount = failureCount;
            DurationSeconds = durationSeconds;
            SleepDelta = sleepDelta;
            LitterBoxUses = litterBoxUses;
            FeederUses = feederUses;
            BedCareUses = bedCareUses;
            PoopIncidents = poopIncidents;
            SleepMaxLost = sleepMaxLost;
            WeakIncidents = weakIncidents;
            CatPressureEvents = catPressureEvents;
            CatDamageIncoming = catDamageIncoming;
            CatDamageTaken = catDamageTaken;
            CatDamageAbsorbed = catDamageAbsorbed;
            CatHealEvents = catHealEvents;
            CatHealingApplied = catHealingApplied;
            CatShieldEvents = catShieldEvents;
            CatShieldApplied = catShieldApplied;
            BedPressureHits = bedPressureHits;
            BossThrowPressureHits = bossThrowPressureHits;
            EnemySleepPressureEvents = enemySleepPressureEvents;
            EnemySleepDamageIncoming = enemySleepDamageIncoming;
            EnemySleepDamageTaken = enemySleepDamageTaken;
            EnemySleepDamageAbsorbed = enemySleepDamageAbsorbed;
            CatSwitchAttempts = catSwitchAttempts;
            CatSwitchesSucceeded = catSwitchesSucceeded;
            CatSwitchesBlockedByWeak = catSwitchesBlockedByWeak;
            AutoTargetAttempts = autoTargetAttempts;
            AutoTargetsAcquired = autoTargetsAcquired;
            SkillTargetAttempts = skillTargetAttempts;
            SkillTargetsAcquired = skillTargetsAcquired;
            SkillCastAttempts = skillCastAttempts;
            SkillCastsSucceeded = skillCastsSucceeded;
            SkillCastsBlockedByCooldown = skillCastsBlockedByCooldown;
            SkillCastsBlockedByTarget = skillCastsBlockedByTarget;
            SkillCastsBlockedByMissingDefinition = skillCastsBlockedByMissingDefinition;
            InteractionAttempts = interactionAttempts;
            InteractionSuccesses = interactionSuccesses;
            InteractionBlockedByRange = interactionBlockedByRange;
        }

        public int NodeCount { get; }

        public int SuccessCount { get; }

        public int FailureCount { get; }

        public float DurationSeconds { get; }

        public float SleepDelta { get; }

        public int LitterBoxUses { get; }

        public int FeederUses { get; }

        public int BedCareUses { get; }

        public int PoopIncidents { get; }

        public float SleepMaxLost { get; }

        public int WeakIncidents { get; }

        public int CatPressureEvents { get; }

        public float CatDamageIncoming { get; }

        public float CatDamageTaken { get; }

        public float CatDamageAbsorbed { get; }

        public int CatHealEvents { get; }

        public float CatHealingApplied { get; }

        public int CatShieldEvents { get; }

        public float CatShieldApplied { get; }

        public int BedPressureHits { get; }

        public int BossThrowPressureHits { get; }

        public int EnemySleepPressureEvents { get; }

        public float EnemySleepDamageIncoming { get; }

        public float EnemySleepDamageTaken { get; }

        public float EnemySleepDamageAbsorbed { get; }

        public int CatSwitchAttempts { get; }

        public int CatSwitchesSucceeded { get; }

        public int CatSwitchesBlockedByWeak { get; }

        public int AutoTargetAttempts { get; }

        public int AutoTargetsAcquired { get; }

        public int SkillTargetAttempts { get; }

        public int SkillTargetsAcquired { get; }

        public int SkillCastAttempts { get; }

        public int SkillCastsSucceeded { get; }

        public int SkillCastsBlockedByCooldown { get; }

        public int SkillCastsBlockedByTarget { get; }

        public int SkillCastsBlockedByMissingDefinition { get; }

        public int InteractionAttempts { get; }

        public int InteractionSuccesses { get; }

        public int InteractionBlockedByRange { get; }
    }
}
