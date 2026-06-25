using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;

namespace TheCat.Combat
{
    public sealed class BattleSimulation
    {
        private const string BlackMudNightmareId = "black_mud_nightmare";
        private const float BaseDistanceToBed = 8f;
        private const float CallTyrantSummonIntervalSeconds = 8f;
        private const float CallTyrantThrowIntervalSeconds = 6f;
        private const float CallTyrantThrowSleepDamage = 4f;
        public const float BedCareSleepRestoreAmount = 8f;
        public const float BedCareHungerCost = 6f;
        public const float SaibanBedShieldRatio = 0.35f;
        public const float NephthysControlledTargetDamageMultiplier = 1.25f;
        public const float SuzuneSleepBellPoopCountdownExtensionSeconds = 8f;
        private const string SuzuneSleepBellSkillId = "suzune_sleep_bell";

        private readonly BattleSimulationConfig config;
        private readonly List<SpawnScheduleState> spawnSchedules = new List<SpawnScheduleState>();
        private readonly List<BattleEnemyState> activeEnemies = new List<BattleEnemyState>();
        private int nextEnemyInstanceId = 1;

        public BattleSimulation(BattleSimulationConfig config, RunMetrics runMetrics)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            RunMetrics = runMetrics ?? throw new ArgumentNullException(nameof(runMetrics));

            OwnerSleep = new OwnerSleepState(config.StartingSleep, config.StartingSleepMax, config.StartingSleepBaseMax);
            TeamPoop = new TeamPoopGauge(config.StartingPoop);
            TeamHunger = new TeamHungerGauge(config.StartingHunger);
            BedStatuses = new StatusEffectCollection();
            NodeMetrics = RunMetrics.BeginNode(config.Wave.Layer, config.Wave.Id, OwnerSleep.Current);

            for (int i = 0; i < config.Wave.SpawnGroups.Count; i++)
            {
                spawnSchedules.Add(new SpawnScheduleState(config.Wave.SpawnGroups[i]));
            }
        }

        public OwnerSleepState OwnerSleep { get; }

        public TeamPoopGauge TeamPoop { get; }

        public TeamHungerGauge TeamHunger { get; }

        public StatusEffectCollection BedStatuses { get; }

        public RunMetrics RunMetrics { get; }

        public NodeMetrics NodeMetrics { get; }

        public float BattleTimeSeconds { get; private set; }

        public BattleOutcome Outcome { get; private set; }

        public int BossSummonsTriggered { get; private set; }

        public int BossThrowsTriggered { get; private set; }

        public IReadOnlyList<BattleEnemyState> ActiveEnemies => activeEnemies.AsReadOnly();

        public void Tick(float deltaSeconds)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            if (Outcome != BattleOutcome.InProgress)
            {
                return;
            }

            BattleTimeSeconds += deltaSeconds;
            BedStatuses.Tick(deltaSeconds);
            TeamHunger.Tick(deltaSeconds, config.Tuning);
            TeamPoop.Tick(deltaSeconds, config.Tuning, TeamHunger.IsDigesting, config.Wave.Layer);
            SpawnDueEnemies();
            AdvanceEnemies(deltaSeconds);
            TickBossBehaviors(deltaSeconds);
            ConsumePoopIncidents();
            UpdateOutcome();
        }

        public bool ApplyDamageToNearestEnemy(float damage)
        {
            return ApplyDamageToNearestEnemy(damage, null);
        }

        public bool ApplyDamageToNearestEnemy(float damage, CatBattleState attacker)
        {
            if (damage < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), damage, "Damage must not be negative.");
            }

            BattleEnemyState target = GetNearestEnemyToBed();
            if (target == null)
            {
                return false;
            }

            return ApplyDamageToEnemy(target, damage, attacker);
        }

        public bool ApplyDamageToEnemy(BattleEnemyState target, float damage, CatBattleState attacker = null)
        {
            if (damage < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damage), damage, "Damage must not be negative.");
            }

            if (target == null || !target.IsAlive || !activeEnemies.Contains(target))
            {
                return false;
            }

            target.ApplyDamage(CalculateFinalEnemyDamage(damage, attacker, target));
            if (!target.IsAlive)
            {
                activeEnemies.Remove(target);
            }

            UpdateOutcome();
            return true;
        }

        public SkillCastResult CastSkill(SkillDefinition skill, CatBattleState caster = null)
        {
            return CastSkill(skill, caster, null);
        }

        public SkillCastResult CastSkill(SkillDefinition skill, CatBattleState caster, BattleEnemyState targetOverride)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            TeamHunger.Spend(skill.HungerCost);
            SkillCastResult result = new SkillCastResult(skill.Id, skill.HungerCost);

            for (int i = 0; i < skill.Effects.Count; i++)
            {
                ApplySkillEffect(skill, skill.Effects[i], caster, targetOverride, result);
            }

            NodeMetrics.RecordSkillCastSuccess();
            NodeMetrics.RecordCatHeal(result.CatHealingApplied);
            NodeMetrics.RecordCatShield(result.ShieldApplied);
            UpdateOutcome();
            return result;
        }

        public void RecordLitterBoxUse()
        {
            TeamPoop.UseLitterBox(config.Tuning);
            NodeMetrics.RecordLitterBoxUse();
        }

        public void RecordFeederUse()
        {
            TeamHunger.UseFeeder();
            NodeMetrics.RecordFeederUse();
        }

        public float RecordBedCareUse()
        {
            TeamHunger.Spend(BedCareHungerCost);
            float before = OwnerSleep.Current;
            OwnerSleep.Restore(BedCareSleepRestoreAmount);
            NodeMetrics.RecordBedCareUse();
            return OwnerSleep.Current - before;
        }

        public BattleEnemyState DebugSpawnEnemy(string enemyId, float timeToBedSeconds = -1f, string spawnGateId = "center")
        {
            if (string.IsNullOrWhiteSpace(enemyId))
            {
                throw new ArgumentException("Enemy id is required.", nameof(enemyId));
            }

            if (!config.EnemiesById.TryGetValue(enemyId, out EnemyDefinition enemy))
            {
                throw new InvalidOperationException("Missing enemy definition: " + enemyId);
            }

            float timeToBed = timeToBedSeconds >= 0f ? timeToBedSeconds : CalculateTimeToBed(enemy);
            BattleEnemyState state = new BattleEnemyState(
                nextEnemyInstanceId++,
                enemy,
                timeToBed,
                spawnGateId);
            activeEnemies.Add(state);
            return state;
        }

        public bool DebugApplyStatusToEnemy(
            BattleEnemyState target,
            string statusTagId,
            float magnitudeOverride = -1f,
            float durationOverrideSeconds = -1f)
        {
            if (target == null || !target.IsAlive || !activeEnemies.Contains(target))
            {
                return false;
            }

            target.ApplyStatus(GetStatusDefinition(statusTagId), magnitudeOverride, durationOverrideSeconds);
            return true;
        }

        public void DebugApplyBedStatus(
            string statusTagId,
            float magnitudeOverride = -1f,
            float durationOverrideSeconds = -1f)
        {
            BedStatuses.Apply(GetStatusDefinition(statusTagId), magnitudeOverride, durationOverrideSeconds);
        }

        public void DebugDamageOwnerSleep(float amount)
        {
            OwnerSleep.ApplyDamage(amount);
            UpdateOutcome();
        }

        public void DebugSpendHunger(float amount)
        {
            TeamHunger.Spend(amount);
        }

        public void DebugForcePoopCountdown()
        {
            TeamPoop.Tick(9999f, config.Tuning, TeamHunger.IsDigesting, config.Wave.Layer);
        }

        private void SpawnDueEnemies()
        {
            for (int i = 0; i < spawnSchedules.Count; i++)
            {
                SpawnScheduleState schedule = spawnSchedules[i];
                while (schedule.CanSpawn(BattleTimeSeconds))
                {
                    EnemyDefinition enemy = config.EnemiesById[schedule.Definition.EnemyId];
                    activeEnemies.Add(new BattleEnemyState(nextEnemyInstanceId++, enemy, CalculateTimeToBed(enemy), schedule.Definition.SpawnGateId));
                    schedule.MarkSpawned();
                }
            }
        }

        private void TickBossBehaviors(float deltaSeconds)
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                BattleEnemyState enemy = activeEnemies[i];
                if (enemy.Definition.BehaviorType != EnemyBehaviorType.BossCallTyrant)
                {
                    continue;
                }

                if (enemy.TickBossSummon(deltaSeconds, CallTyrantSummonIntervalSeconds))
                {
                    SpawnBossSummon(enemy);
                }

                if (enemy.TickBossThrow(deltaSeconds, CallTyrantThrowIntervalSeconds))
                {
                    BossThrowsTriggered++;
                    ApplyDamageToOwnerSleep(CallTyrantThrowSleepDamage, isBossThrow: true);
                }
            }
        }

        private void SpawnBossSummon(BattleEnemyState boss)
        {
            if (!config.EnemiesById.TryGetValue(BlackMudNightmareId, out EnemyDefinition summonDefinition))
            {
                return;
            }

            string gate = string.IsNullOrWhiteSpace(boss.SpawnGateId) ? "north" : boss.SpawnGateId;
            activeEnemies.Add(new BattleEnemyState(nextEnemyInstanceId++, summonDefinition, CalculateTimeToBed(summonDefinition), gate));
            BossSummonsTriggered++;
        }

        private void ApplySkillEffect(
            SkillDefinition skill,
            SkillEffectDefinition effect,
            CatBattleState caster,
            BattleEnemyState targetOverride,
            SkillCastResult result)
        {
            switch (effect.EffectType)
            {
                case SkillEffectType.Damage:
                    ApplySkillDamage(effect.Magnitude * config.Modifiers.SkillDamageMultiplier, caster, targetOverride, result);
                    break;
                case SkillEffectType.HealCat:
                    if (caster != null)
                    {
                        float healAmount = effect.Magnitude * config.Modifiers.CatHealMultiplier;
                        caster.Heal(healAmount);
                        result.RecordCatHeal(healAmount);
                    }

                    break;
                case SkillEffectType.RestoreOwnerSleep:
                    float restoreAmount = effect.Magnitude * config.Modifiers.OwnerSleepRestoreMultiplier;
                    OwnerSleep.Restore(restoreAmount);
                    result.RecordOwnerSleepRestore(restoreAmount);
                    ApplyBedStatusIfPresent(effect, result);
                    ApplySuzunePoopCountdownRelief(skill, caster, effect, result);
                    break;
                case SkillEffectType.ApplyStatus:
                    ApplyStatusToEnemy(effect, targetOverride, result);
                    break;
                case SkillEffectType.Shield:
                    if (caster != null)
                    {
                        StatusTagDefinition shieldDefinition = GetStatusDefinition(effect.StatusTagId);
                        float shieldAmount = effect.Magnitude * config.Modifiers.ShieldMultiplier;
                        caster.ApplyStatus(shieldDefinition, shieldAmount);
                        result.RecordShield(shieldAmount);
                        result.RecordStatusApplication();
                        ApplySaibanBedShieldPassive(caster, shieldDefinition, shieldAmount, result);
                    }

                    break;
                case SkillEffectType.Knockback:
                    ApplyKnockbackToEnemy(effect, targetOverride, result);
                    break;
                case SkillEffectType.SpawnSummon:
                    result.RecordSummon();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effect), effect.EffectType, "Unknown skill effect type.");
            }
        }

        private void ApplySkillDamage(float damage, CatBattleState caster, BattleEnemyState targetOverride, SkillCastResult result)
        {
            BattleEnemyState target = GetTargetEnemy(targetOverride);
            if (target == null)
            {
                return;
            }

            result.RecordEnemyTarget();
            float finalDamage = CalculateFinalEnemyDamage(damage, caster, target);
            target.ApplyDamage(finalDamage);
            result.RecordDamage(finalDamage);
            if (!target.IsAlive)
            {
                activeEnemies.Remove(target);
            }
        }

        private float CalculateFinalEnemyDamage(float damage, CatBattleState attacker, BattleEnemyState target)
        {
            return damage
                * TeamHunger.DamageMultiplier
                * target.DamageTakenMultiplier
                * GetAttackerDamageMultiplier(attacker, target);
        }

        private static float GetAttackerDamageMultiplier(CatBattleState attacker, BattleEnemyState target)
        {
            if (attacker == null || target == null)
            {
                return 1f;
            }

            if (attacker.Definition.Id == P0PrototypeCatalog.NephthysId && IsControlledOrMarked(target))
            {
                return NephthysControlledTargetDamageMultiplier;
            }

            return 1f;
        }

        private static bool IsControlledOrMarked(BattleEnemyState target)
        {
            return target.Statuses.Has(StatusTagIds.Slow) || target.Statuses.Has(StatusTagIds.Mark);
        }

        private void ApplyStatusToEnemy(SkillEffectDefinition effect, BattleEnemyState targetOverride, SkillCastResult result)
        {
            BattleEnemyState target = GetTargetEnemy(targetOverride);
            if (target == null)
            {
                return;
            }

            StatusTagDefinition definition = GetStatusDefinition(effect.StatusTagId);
            float duration = definition.BaseDurationSeconds * config.Modifiers.EnemyStatusDurationMultiplier;
            target.ApplyStatus(definition, effect.Magnitude, duration);
            result.RecordEnemyTarget();
            result.RecordStatusApplication();
        }

        private void ApplyKnockbackToEnemy(SkillEffectDefinition effect, BattleEnemyState targetOverride, SkillCastResult result)
        {
            BattleEnemyState target = GetTargetEnemy(targetOverride);
            if (target == null)
            {
                return;
            }

            bool applied = target.ApplyKnockback(effect.Magnitude * config.Modifiers.KnockbackMultiplier);
            if (!string.IsNullOrWhiteSpace(effect.StatusTagId))
            {
                StatusTagDefinition definition = GetStatusDefinition(effect.StatusTagId);
                float duration = definition.BaseDurationSeconds * config.Modifiers.EnemyStatusDurationMultiplier;
                target.ApplyStatus(definition, effect.Magnitude, duration);
                result.RecordStatusApplication();
            }

            result.RecordEnemyTarget();
            if (applied)
            {
                result.RecordKnockback();
            }
        }

        private void ApplyBedStatusIfPresent(SkillEffectDefinition effect, SkillCastResult result)
        {
            if (string.IsNullOrWhiteSpace(effect.StatusTagId))
            {
                return;
            }

            BedStatuses.Apply(GetStatusDefinition(effect.StatusTagId));
            result.RecordStatusApplication();
        }

        private BattleEnemyState GetTargetEnemy(BattleEnemyState targetOverride)
        {
            if (targetOverride != null && targetOverride.IsAlive && activeEnemies.Contains(targetOverride))
            {
                return targetOverride;
            }

            return GetNearestEnemyToBed();
        }

        private void ApplySuzunePoopCountdownRelief(
            SkillDefinition skill,
            CatBattleState caster,
            SkillEffectDefinition effect,
            SkillCastResult result)
        {
            if (skill.Id != SuzuneSleepBellSkillId
                || caster == null
                || caster.Definition.Id != P0PrototypeCatalog.SuzuneId
                || effect.StatusTagId != StatusTagIds.SleepStable)
            {
                return;
            }

            float extendedSeconds = TeamPoop.ExtendCountdown(SuzuneSleepBellPoopCountdownExtensionSeconds);
            if (extendedSeconds > 0f)
            {
                result.RecordPoopCountdownExtension(extendedSeconds);
            }
        }

        private StatusTagDefinition GetStatusDefinition(string statusTagId)
        {
            if (string.IsNullOrWhiteSpace(statusTagId))
            {
                throw new ArgumentException("Skill effect requires a status tag id.", nameof(statusTagId));
            }

            if (!config.StatusesById.TryGetValue(statusTagId, out StatusTagDefinition definition))
            {
                throw new InvalidOperationException("Missing status tag definition: " + statusTagId);
            }

            return definition;
        }

        private void AdvanceEnemies(float deltaSeconds)
        {
            for (int i = activeEnemies.Count - 1; i >= 0; i--)
            {
                BattleEnemyState enemy = activeEnemies[i];
                enemy.Advance(deltaSeconds);
                if (enemy.HasReachedBed)
                {
                    ApplyDamageToOwnerSleep(enemy.Definition.BedDamage, isBedContact: true);
                    activeEnemies.RemoveAt(i);
                }
            }
        }

        private void ApplyDamageToOwnerSleep(
            float baseDamage,
            bool isBedContact = false,
            bool isBossThrow = false)
        {
            float incomingDamage = baseDamage * config.Tuning.OwnerSleepDamageMultiplier;
            float unabsorbedDamage = BedStatuses.AbsorbDamage(StatusTagIds.Shield, incomingDamage);
            OwnerSleep.ApplyDamage(unabsorbedDamage);
            if (isBossThrow)
            {
                NodeMetrics.RecordBossThrowPressure(incomingDamage, unabsorbedDamage);
            }
            else if (isBedContact)
            {
                NodeMetrics.RecordBedPressure(incomingDamage, unabsorbedDamage);
            }
        }

        private void ApplySaibanBedShieldPassive(
            CatBattleState caster,
            StatusTagDefinition shieldDefinition,
            float shieldAmount,
            SkillCastResult result)
        {
            if (caster.Definition.Id != P0PrototypeCatalog.SaibanId || shieldDefinition.Id != StatusTagIds.Shield)
            {
                return;
            }

            float bedShieldAmount = shieldAmount * SaibanBedShieldRatio;
            BedStatuses.Apply(shieldDefinition, bedShieldAmount);
            result.RecordBedShield(bedShieldAmount);
            result.RecordStatusApplication();
        }

        private void ConsumePoopIncidents()
        {
            while (TeamPoop.TryConsumeIncident())
            {
                NodeMetrics.RecordPoopIncident();
                float sleepMaxLost = OwnerSleep.ApplyMaxPenalty(config.Tuning.PoopSleepMaxPenalty);
                NodeMetrics.RecordSleepMaxLoss(sleepMaxLost);
            }
        }

        private void UpdateOutcome()
        {
            if (OwnerSleep.IsFailed)
            {
                Outcome = BattleOutcome.Defeat;
                NodeMetrics.Complete(NodeResult.Failure, BattleTimeSeconds, OwnerSleep.Current);
                return;
            }

            if (AllSpawnsComplete() && activeEnemies.Count == 0)
            {
                Outcome = BattleOutcome.Victory;
                NodeMetrics.Complete(NodeResult.Success, BattleTimeSeconds, OwnerSleep.Current);
            }
        }

        private bool AllSpawnsComplete()
        {
            for (int i = 0; i < spawnSchedules.Count; i++)
            {
                if (!spawnSchedules[i].IsComplete)
                {
                    return false;
                }
            }

            return true;
        }

        private BattleEnemyState GetNearestEnemyToBed()
        {
            BattleEnemyState nearest = null;
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                BattleEnemyState enemy = activeEnemies[i];
                if (nearest == null || enemy.TimeToBedSeconds < nearest.TimeToBedSeconds)
                {
                    nearest = enemy;
                }
            }

            return nearest;
        }

        private static float CalculateTimeToBed(EnemyDefinition enemy)
        {
            if (enemy.MoveSpeed <= 0f)
            {
                return float.MaxValue;
            }

            return BaseDistanceToBed / enemy.MoveSpeed;
        }
    }
}
