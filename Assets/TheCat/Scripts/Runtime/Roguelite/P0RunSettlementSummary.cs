using TheCat.Data;

namespace TheCat.Roguelite
{
    public readonly struct P0RunSettlementSummary
    {
        public P0RunSettlementSummary(RunProgressionState run)
        {
            RunRouteState route = run == null ? null : run.Route;
            RunMetricsSummary metrics = run == null ? default(RunMetricsSummary) : run.Metrics.GetSummary();

            IsCleared = route != null && route.IsCleared;
            IsFailed = route != null && route.IsFailed;
            CompletedNodes = route == null ? 0 : route.CompletedCount;
            TotalLayers = route == null ? 0 : route.Route.LayerCount;
            BattleNodeCount = metrics.NodeCount;
            BattleSuccesses = metrics.SuccessCount;
            BattleFailures = metrics.FailureCount;
            DurationSeconds = metrics.DurationSeconds;
            SleepDelta = metrics.SleepDelta;
            LitterBoxUses = metrics.LitterBoxUses;
            FeederUses = metrics.FeederUses;
            BedCareUses = metrics.BedCareUses;
            PoopIncidents = metrics.PoopIncidents;
            SleepMaxLost = metrics.SleepMaxLost;
            WeakIncidents = metrics.WeakIncidents;
            CatPressureEvents = metrics.CatPressureEvents;
            CatDamageIncoming = metrics.CatDamageIncoming;
            CatDamageTaken = metrics.CatDamageTaken;
            CatDamageAbsorbed = metrics.CatDamageAbsorbed;
            CatHealEvents = metrics.CatHealEvents;
            CatHealingApplied = metrics.CatHealingApplied;
            CatShieldEvents = metrics.CatShieldEvents;
            CatShieldApplied = metrics.CatShieldApplied;
            BedPressureHits = metrics.BedPressureHits;
            BossThrowPressureHits = metrics.BossThrowPressureHits;
            EnemySleepPressureEvents = metrics.EnemySleepPressureEvents;
            EnemySleepDamageIncoming = metrics.EnemySleepDamageIncoming;
            EnemySleepDamageTaken = metrics.EnemySleepDamageTaken;
            EnemySleepDamageAbsorbed = metrics.EnemySleepDamageAbsorbed;
            CatSwitchAttempts = metrics.CatSwitchAttempts;
            CatSwitchesSucceeded = metrics.CatSwitchesSucceeded;
            CatSwitchesBlockedByWeak = metrics.CatSwitchesBlockedByWeak;
            AutoTargetAttempts = metrics.AutoTargetAttempts;
            AutoTargetsAcquired = metrics.AutoTargetsAcquired;
            SkillTargetAttempts = metrics.SkillTargetAttempts;
            SkillTargetsAcquired = metrics.SkillTargetsAcquired;
            SkillCastAttempts = metrics.SkillCastAttempts;
            SkillCastsSucceeded = metrics.SkillCastsSucceeded;
            SkillCastsBlockedByCooldown = metrics.SkillCastsBlockedByCooldown;
            SkillCastsBlockedByTarget = metrics.SkillCastsBlockedByTarget;
            SkillCastsBlockedByMissingDefinition = metrics.SkillCastsBlockedByMissingDefinition;
            InteractionAttempts = metrics.InteractionAttempts;
            InteractionSuccesses = metrics.InteractionSuccesses;
            InteractionBlockedByRange = metrics.InteractionBlockedByRange;
            DreamShards = run == null ? 0 : run.Wallet.DreamShards;
            FishTreats = run == null ? 0 : run.Wallet.FishTreats;
            RosterCount = run == null ? 0 : run.Roster.Count;
            BlessingCount = run == null ? 0 : run.Blessings.Count;
            BlessingTotalLevel = run == null ? 0 : run.Blessings.TotalLevel;
            DreamEventsResolved = run == null ? 0 : run.DreamEventsResolved;
            ShopPurchases = run == null ? 0 : run.ShopPurchases;
            RestNestUses = run == null ? 0 : run.RestNestUses;
            OwnerSleepCurrent = run == null ? 0f : run.CoreValues.OwnerSleepCurrent;
            OwnerSleepMax = run == null ? 0f : run.CoreValues.OwnerSleepMax;
            TeamPoop = run == null ? 0f : run.CoreValues.TeamPoop;
            TeamHunger = run == null ? 0f : run.CoreValues.TeamHunger;
            CatVitalCount = run == null ? 0 : run.CatVitals.Count;
            WeakCatCount = run == null ? 0 : run.CatVitals.CountWeakCats();
            LowestCatHpRatio = run == null ? 1f : run.CatVitals.GetLowestHpRatio();
        }

        public bool IsCleared { get; }

        public bool IsFailed { get; }

        public int CompletedNodes { get; }

        public int TotalLayers { get; }

        public int BattleNodeCount { get; }

        public int BattleSuccesses { get; }

        public int BattleFailures { get; }

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

        public int DreamShards { get; }

        public int FishTreats { get; }

        public int RosterCount { get; }

        public int BlessingCount { get; }

        public int BlessingTotalLevel { get; }

        public int DreamEventsResolved { get; }

        public int ShopPurchases { get; }

        public int RestNestUses { get; }

        public float OwnerSleepCurrent { get; }

        public float OwnerSleepMax { get; }

        public float TeamPoop { get; }

        public float TeamHunger { get; }

        public int CatVitalCount { get; }

        public int WeakCatCount { get; }

        public float LowestCatHpRatio { get; }

        public string ResultLabel
        {
            get
            {
                if (IsCleared)
                {
                    return "路线通关";
                }

                return IsFailed ? "路线失败" : "路线进行中";
            }
        }
    }
}
