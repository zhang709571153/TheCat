using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public sealed class P0GoldenPathSimulationOptions
    {
        public P0GoldenPathSimulationOptions(
            float tickDeltaSeconds = 0.5f,
            int maxTicksPerBattle = 512,
            float damagePerSweep = 9999f,
            float bossMinimumObservationSeconds = 8.25f,
            bool useAssistedOpeningActions = true)
        {
            TickDeltaSeconds = RequirePositive(tickDeltaSeconds, nameof(tickDeltaSeconds));
            if (maxTicksPerBattle <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxTicksPerBattle), maxTicksPerBattle, "Max ticks must be greater than zero.");
            }

            MaxTicksPerBattle = maxTicksPerBattle;
            DamagePerSweep = RequirePositive(damagePerSweep, nameof(damagePerSweep));
            if (bossMinimumObservationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(bossMinimumObservationSeconds), bossMinimumObservationSeconds, "Boss observation seconds must not be negative.");
            }

            BossMinimumObservationSeconds = bossMinimumObservationSeconds;
            UseAssistedOpeningActions = useAssistedOpeningActions;
        }

        public static P0GoldenPathSimulationOptions Default => new P0GoldenPathSimulationOptions();

        public float TickDeltaSeconds { get; }

        public int MaxTicksPerBattle { get; }

        public float DamagePerSweep { get; }

        public float BossMinimumObservationSeconds { get; }

        public bool UseAssistedOpeningActions { get; }

        private static float RequirePositive(float value, string name)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be greater than zero.");
            }

            return value;
        }
    }

    public readonly struct P0GoldenPathBattleReport
    {
        public P0GoldenPathBattleReport(
            RouteNodeDefinition node,
            string waveId,
            BattleOutcome outcome,
            float durationSeconds,
            float ownerSleepCurrent,
            float ownerSleepMax,
            float teamPoop,
            float teamHunger,
            int bossSummonsTriggered,
            int bossThrowsTriggered,
            int bedPressureHits,
            int bossThrowPressureHits,
            int enemySleepPressureEvents,
            float enemySleepDamageIncoming,
            float enemySleepDamageTaken,
            float enemySleepDamageAbsorbed,
            int catPressureEvents,
            float catDamageIncoming,
            float catDamageTaken,
            float catDamageAbsorbed,
            int catHealEvents,
            float catHealingApplied,
            int catShieldEvents,
            float catShieldApplied,
            int catSwitchAttempts,
            int catSwitchesSucceeded,
            int autoTargetAttempts,
            int autoTargetsAcquired,
            int skillTargetAttempts,
            int skillTargetsAcquired,
            int skillCastAttempts,
            int skillCastsSucceeded,
            int interactionAttempts,
            int interactionSuccesses,
            RouteBattleReward reward)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            WaveId = string.IsNullOrWhiteSpace(waveId) ? string.Empty : waveId;
            Outcome = outcome;
            DurationSeconds = durationSeconds;
            OwnerSleepCurrent = ownerSleepCurrent;
            OwnerSleepMax = ownerSleepMax;
            TeamPoop = teamPoop;
            TeamHunger = teamHunger;
            BossSummonsTriggered = bossSummonsTriggered;
            BossThrowsTriggered = bossThrowsTriggered;
            BedPressureHits = bedPressureHits;
            BossThrowPressureHits = bossThrowPressureHits;
            EnemySleepPressureEvents = enemySleepPressureEvents;
            EnemySleepDamageIncoming = enemySleepDamageIncoming;
            EnemySleepDamageTaken = enemySleepDamageTaken;
            EnemySleepDamageAbsorbed = enemySleepDamageAbsorbed;
            CatPressureEvents = catPressureEvents;
            CatDamageIncoming = catDamageIncoming;
            CatDamageTaken = catDamageTaken;
            CatDamageAbsorbed = catDamageAbsorbed;
            CatHealEvents = catHealEvents;
            CatHealingApplied = catHealingApplied;
            CatShieldEvents = catShieldEvents;
            CatShieldApplied = catShieldApplied;
            CatSwitchAttempts = catSwitchAttempts;
            CatSwitchesSucceeded = catSwitchesSucceeded;
            AutoTargetAttempts = autoTargetAttempts;
            AutoTargetsAcquired = autoTargetsAcquired;
            SkillTargetAttempts = skillTargetAttempts;
            SkillTargetsAcquired = skillTargetsAcquired;
            SkillCastAttempts = skillCastAttempts;
            SkillCastsSucceeded = skillCastsSucceeded;
            InteractionAttempts = interactionAttempts;
            InteractionSuccesses = interactionSuccesses;
            Reward = reward;
        }

        public RouteNodeDefinition Node { get; }

        public string NodeId => Node.Id;

        public RouteNodeType NodeType => Node.NodeType;

        public string WaveId { get; }

        public BattleOutcome Outcome { get; }

        public float DurationSeconds { get; }

        public float OwnerSleepCurrent { get; }

        public float OwnerSleepMax { get; }

        public float TeamPoop { get; }

        public float TeamHunger { get; }

        public int BossSummonsTriggered { get; }

        public int BossThrowsTriggered { get; }

        public int BedPressureHits { get; }

        public int BossThrowPressureHits { get; }

        public int EnemySleepPressureEvents { get; }

        public float EnemySleepDamageIncoming { get; }

        public float EnemySleepDamageTaken { get; }

        public float EnemySleepDamageAbsorbed { get; }

        public int CatPressureEvents { get; }

        public float CatDamageIncoming { get; }

        public float CatDamageTaken { get; }

        public float CatDamageAbsorbed { get; }

        public int CatHealEvents { get; }

        public float CatHealingApplied { get; }

        public int CatShieldEvents { get; }

        public float CatShieldApplied { get; }

        public int CatSwitchAttempts { get; }

        public int CatSwitchesSucceeded { get; }

        public int AutoTargetAttempts { get; }

        public int AutoTargetsAcquired { get; }

        public int SkillTargetAttempts { get; }

        public int SkillTargetsAcquired { get; }

        public int SkillCastAttempts { get; }

        public int SkillCastsSucceeded { get; }

        public int InteractionAttempts { get; }

        public int InteractionSuccesses { get; }

        public RouteBattleReward Reward { get; }

        public bool IsBossBattle => NodeId == P0RouteCatalog.BossNodeId;

        public bool BossBehaviorObserved => BossSummonsTriggered > 0 && BossThrowsTriggered > 0;

        public string BuildSummary()
        {
            return NodeId + " " + Outcome
                + " wave " + WaveId
                + " time " + DurationSeconds.ToString("0.0") + "s"
                + " sleep " + OwnerSleepCurrent.ToString("0.#") + "/" + OwnerSleepMax.ToString("0.#")
                + " poop " + TeamPoop.ToString("0.#")
                + " hunger " + TeamHunger.ToString("0.#")
                + " boss summon " + BossSummonsTriggered
                + " throw " + BossThrowsTriggered
                + " pressure " + EnemySleepPressureEvents
                + " sleep " + EnemySleepDamageTaken.ToString("0.#") + "/" + EnemySleepDamageIncoming.ToString("0.#")
                + " absorbed " + EnemySleepDamageAbsorbed.ToString("0.#")
                + " catVitals pressure " + CatPressureEvents
                + " damage " + CatDamageTaken.ToString("0.#") + "/" + CatDamageIncoming.ToString("0.#")
                + " heal " + CatHealEvents + "/" + CatHealingApplied.ToString("0.#")
                + " shield " + CatShieldEvents + "/" + CatShieldApplied.ToString("0.#")
                + " switches " + CatSwitchesSucceeded + "/" + CatSwitchAttempts
                + " targets auto " + AutoTargetsAcquired + "/" + AutoTargetAttempts
                + " skill " + SkillTargetsAcquired + "/" + SkillTargetAttempts
                + " skills " + SkillCastsSucceeded + "/" + SkillCastAttempts
                + " interactions " + InteractionSuccesses + "/" + InteractionAttempts
                + " reward " + Reward.BuildSummary();
        }
    }

    public sealed class P0GoldenPathReport
    {
        public P0GoldenPathReport(RunProgressionState run, IReadOnlyList<P0GoldenPathBattleReport> battleReports)
        {
            Run = run ?? throw new ArgumentNullException(nameof(run));
            BattleReports = new List<P0GoldenPathBattleReport>(
                battleReports ?? Array.Empty<P0GoldenPathBattleReport>()).AsReadOnly();
            Settlement = new P0RunSettlementSummary(run);
        }

        public RunProgressionState Run { get; }

        public IReadOnlyList<P0GoldenPathBattleReport> BattleReports { get; }

        public P0RunSettlementSummary Settlement { get; }

        public bool IsCleared => Settlement.IsCleared;

        public int BattleCount => BattleReports.Count;

        public bool BossBattleCleared
        {
            get
            {
                P0GoldenPathBattleReport report;
                return TryGetBossBattleReport(out report) && report.Outcome == BattleOutcome.Victory;
            }
        }

        public bool BossBehaviorObserved
        {
            get
            {
                P0GoldenPathBattleReport report;
                return TryGetBossBattleReport(out report) && report.BossBehaviorObserved;
            }
        }

        public bool TryGetBossBattleReport(out P0GoldenPathBattleReport report)
        {
            for (int i = 0; i < BattleReports.Count; i++)
            {
                if (BattleReports[i].IsBossBattle)
                {
                    report = BattleReports[i];
                    return true;
                }
            }

            report = default(P0GoldenPathBattleReport);
            return false;
        }

        public string BuildSummary()
        {
            return Settlement.ResultLabel
                + " nodes " + Settlement.CompletedNodes + "/" + Settlement.TotalLayers
                + " battles " + Settlement.BattleSuccesses + "/" + BattleCount
                + " boss " + (BossBehaviorObserved ? "observed" : "not observed")
                + " shards " + Settlement.DreamShards
                + " fish " + Settlement.FishTreats;
        }
    }

    public static class P0GoldenPathSimulator
    {
        public static P0GoldenPathReport SimulateDefaultRun()
        {
            return SimulateRun(P0RunSession.CreateDefaultStarterCatIds(), P0GoldenPathSimulationOptions.Default);
        }

        public static P0GoldenPathReport SimulateRun(
            IEnumerable<string> starterCatIds,
            P0GoldenPathSimulationOptions options = null)
        {
            P0GoldenPathSimulationOptions activeOptions = options ?? P0GoldenPathSimulationOptions.Default;
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.NormalizeStarterCatIds(starterCatIds));
            List<P0GoldenPathBattleReport> battleReports = new List<P0GoldenPathBattleReport>();

            int guard = run.Route.Route.LayerCount + 1;
            while (!run.Route.IsComplete)
            {
                if (guard <= 0)
                {
                    throw new InvalidOperationException("Golden path route guard exhausted before the route completed.");
                }

                guard--;
                RouteNodeDefinition node = run.Route.CurrentNode;
                if (RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    battleReports.Add(SimulateBattleNode(run, node, activeOptions));
                    continue;
                }

                RouteNodeResolution resolution = RouteNodeResolver.ResolveCurrentNode(run);
                if (resolution.ResolutionType != RouteNodeResolutionType.PlaceholderResolved
                    && resolution.ResolutionType != RouteNodeResolutionType.RouteCleared)
                {
                    throw new InvalidOperationException("Golden path could not resolve route node " + node.Id + ": " + resolution.Message);
                }
            }

            return new P0GoldenPathReport(run, battleReports);
        }

        private static P0GoldenPathBattleReport SimulateBattleNode(
            RunProgressionState run,
            RouteNodeDefinition node,
            P0GoldenPathSimulationOptions options)
        {
            WaveDefinition wave = P0PrototypeCatalog.CreateWaveForContentId(node.ContentId);
            RunPendingBattleModifierSnapshot pendingModifiers = run.PendingBattleModifiers.Consume();
            BattleModifierSet battleModifiers = pendingModifiers.ApplyTo(P0BlessingCatalog.CreateBattleModifiers(run.Blessings));
            P0Tuning tuning = pendingModifiers.ApplyTo(P0Tuning.Default);

            BattleSimulationConfig config = new BattleSimulationConfig(
                wave,
                P0PrototypeCatalog.CreateCoreEnemies(),
                tuning,
                run.CoreValues.OwnerSleepCurrent,
                run.CoreValues.OwnerSleepMax,
                run.CoreValues.OwnerSleepBaseMax,
                run.CoreValues.TeamPoop,
                run.CoreValues.TeamHunger,
                P0PrototypeCatalog.CreateStatusTags(),
                battleModifiers);
            BattleSimulation battle = new BattleSimulation(config, run.Metrics);

            int ticks = 0;
            bool assistedOpeningApplied = false;
            while (battle.Outcome == BattleOutcome.InProgress)
            {
                if (ticks >= options.MaxTicksPerBattle)
                {
                    throw new InvalidOperationException("Golden path battle guard exhausted at node " + node.Id + ".");
                }

                ticks++;
                battle.Tick(options.TickDeltaSeconds);
                if (options.UseAssistedOpeningActions
                    && !assistedOpeningApplied
                    && battle.ActiveEnemies.Count > 0)
                {
                    ApplyAssistedOpeningActions(battle);
                    assistedOpeningApplied = true;
                }

                bool preserveBoss = node.NodeType == RouteNodeType.Boss
                    && battle.BattleTimeSeconds < options.BossMinimumObservationSeconds;
                ClearActiveEnemies(battle, options.DamagePerSweep, preserveBoss);
            }

            run.CoreValues.Capture(battle.OwnerSleep, battle.TeamPoop, battle.TeamHunger);

            RouteBattleReward reward = RouteBattleReward.None;
            NodeResult result = battle.Outcome == BattleOutcome.Victory ? NodeResult.Success : NodeResult.Failure;
            run.Route.CompleteCurrentNode(result);
            if (result == NodeResult.Success)
            {
                reward = P0RouteRewardResolver.ApplyBattleReward(node, run);
            }

            return new P0GoldenPathBattleReport(
                node,
                wave.Id,
                battle.Outcome,
                battle.BattleTimeSeconds,
                battle.OwnerSleep.Current,
                battle.OwnerSleep.Max,
                battle.TeamPoop.Current,
                battle.TeamHunger.Current,
                battle.BossSummonsTriggered,
                battle.BossThrowsTriggered,
                battle.NodeMetrics.BedPressureHits,
                battle.NodeMetrics.BossThrowPressureHits,
                battle.NodeMetrics.EnemySleepPressureEvents,
                battle.NodeMetrics.EnemySleepDamageIncoming,
                battle.NodeMetrics.EnemySleepDamageTaken,
                battle.NodeMetrics.EnemySleepDamageAbsorbed,
                battle.NodeMetrics.CatPressureEvents,
                battle.NodeMetrics.CatDamageIncoming,
                battle.NodeMetrics.CatDamageTaken,
                battle.NodeMetrics.CatDamageAbsorbed,
                battle.NodeMetrics.CatHealEvents,
                battle.NodeMetrics.CatHealingApplied,
                battle.NodeMetrics.CatShieldEvents,
                battle.NodeMetrics.CatShieldApplied,
                battle.NodeMetrics.CatSwitchAttempts,
                battle.NodeMetrics.CatSwitchesSucceeded,
                battle.NodeMetrics.AutoTargetAttempts,
                battle.NodeMetrics.AutoTargetsAcquired,
                battle.NodeMetrics.SkillTargetAttempts,
                battle.NodeMetrics.SkillTargetsAcquired,
                battle.NodeMetrics.SkillCastAttempts,
                battle.NodeMetrics.SkillCastsSucceeded,
                battle.NodeMetrics.InteractionAttempts,
                battle.NodeMetrics.InteractionSuccesses,
                reward);
        }

        private static void ApplyAssistedOpeningActions(BattleSimulation battle)
        {
            if (battle == null || battle.Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            IReadOnlyList<CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            battle.NodeMetrics.RecordAutoTargetAcquired();
            CastIfPresent(battle, "saiban_oath_shield", CreateCatState(cats, P0PrototypeCatalog.SaibanId));
            battle.NodeMetrics.RecordCatSwitchSuccess();
            battle.NodeMetrics.RecordSkillTargetAcquired();
            CastIfPresent(battle, "nephthys_quicksand_trap", CreateCatState(cats, P0PrototypeCatalog.NephthysId));
            battle.NodeMetrics.RecordCatSwitchSuccess();
            CastIfPresent(battle, "suzune_sleep_bell", CreateCatState(cats, P0PrototypeCatalog.SuzuneId));
            battle.RecordBedCareUse();
            battle.RecordLitterBoxUse();
            battle.RecordFeederUse();
        }

        private static void CastIfPresent(BattleSimulation battle, string skillId, CatBattleState caster)
        {
            SkillDefinition skill = GetStarterSkill(skillId);
            if (skill != null)
            {
                battle.CastSkill(skill, caster);
            }
        }

        private static SkillDefinition GetStarterSkill(string skillId)
        {
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Id == skillId)
                {
                    return skills[i];
                }
            }

            return null;
        }

        private static CatBattleState CreateCatState(IReadOnlyList<CatDefinition> cats, string catId)
        {
            if (cats != null)
            {
                for (int i = 0; i < cats.Count; i++)
                {
                    if (cats[i].Id == catId)
                    {
                        return new CatBattleState(cats[i]);
                    }
                }
            }

            throw new InvalidOperationException("Missing starter cat for golden path assisted action: " + catId);
        }

        private static void ClearActiveEnemies(BattleSimulation battle, float damage, bool preserveBoss)
        {
            for (int i = battle.ActiveEnemies.Count - 1; i >= 0; i--)
            {
                BattleEnemyState enemy = battle.ActiveEnemies[i];
                if (preserveBoss && enemy.Definition.BehaviorType == EnemyBehaviorType.BossCallTyrant)
                {
                    continue;
                }

                battle.ApplyDamageToEnemy(enemy, damage);
            }
        }
    }
}
