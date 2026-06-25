using System;
using System.Collections.Generic;

namespace TheCat.Data
{
    public sealed class RunMetrics
    {
        private readonly List<NodeMetrics> nodes = new List<NodeMetrics>();

        public IReadOnlyList<NodeMetrics> Nodes => nodes;

        public NodeMetrics BeginNode(int layer, string nodeId, float startingSleep)
        {
            NodeMetrics metrics = new NodeMetrics(layer, nodeId, startingSleep);
            nodes.Add(metrics);
            return metrics;
        }

        public RunMetricsSummary GetSummary()
        {
            int successes = 0;
            int failures = 0;
            float duration = 0f;
            float sleepDelta = 0f;
            int litterBoxUses = 0;
            int feederUses = 0;
            int bedCareUses = 0;
            int poopIncidents = 0;
            float sleepMaxLost = 0f;
            int weakIncidents = 0;
            int catPressureEvents = 0;
            float catDamageIncoming = 0f;
            float catDamageTaken = 0f;
            float catDamageAbsorbed = 0f;
            int catHealEvents = 0;
            float catHealingApplied = 0f;
            int catShieldEvents = 0;
            float catShieldApplied = 0f;
            int bedPressureHits = 0;
            int bossThrowPressureHits = 0;
            int enemySleepPressureEvents = 0;
            float enemySleepDamageIncoming = 0f;
            float enemySleepDamageTaken = 0f;
            float enemySleepDamageAbsorbed = 0f;
            int catSwitchAttempts = 0;
            int catSwitchesSucceeded = 0;
            int catSwitchesBlockedByWeak = 0;
            int autoTargetAttempts = 0;
            int autoTargetsAcquired = 0;
            int skillTargetAttempts = 0;
            int skillTargetsAcquired = 0;
            int skillCastAttempts = 0;
            int skillCastsSucceeded = 0;
            int skillCastsBlockedByCooldown = 0;
            int skillCastsBlockedByTarget = 0;
            int skillCastsBlockedByMissingDefinition = 0;
            int interactionAttempts = 0;
            int interactionSuccesses = 0;
            int interactionBlockedByRange = 0;

            for (int i = 0; i < nodes.Count; i++)
            {
                NodeMetrics node = nodes[i];
                if (node.Result == NodeResult.Success)
                {
                    successes++;
                }
                else if (node.Result == NodeResult.Failure)
                {
                    failures++;
                }

                duration += node.DurationSeconds;
                sleepDelta += node.SleepDelta;
                litterBoxUses += node.LitterBoxUses;
                feederUses += node.FeederUses;
                bedCareUses += node.BedCareUses;
                poopIncidents += node.PoopIncidents;
                sleepMaxLost += node.SleepMaxLost;
                weakIncidents += node.WeakIncidents;
                catPressureEvents += node.CatPressureEvents;
                catDamageIncoming += node.CatDamageIncoming;
                catDamageTaken += node.CatDamageTaken;
                catDamageAbsorbed += node.CatDamageAbsorbed;
                catHealEvents += node.CatHealEvents;
                catHealingApplied += node.CatHealingApplied;
                catShieldEvents += node.CatShieldEvents;
                catShieldApplied += node.CatShieldApplied;
                bedPressureHits += node.BedPressureHits;
                bossThrowPressureHits += node.BossThrowPressureHits;
                enemySleepPressureEvents += node.EnemySleepPressureEvents;
                enemySleepDamageIncoming += node.EnemySleepDamageIncoming;
                enemySleepDamageTaken += node.EnemySleepDamageTaken;
                enemySleepDamageAbsorbed += node.EnemySleepDamageAbsorbed;
                catSwitchAttempts += node.CatSwitchAttempts;
                catSwitchesSucceeded += node.CatSwitchesSucceeded;
                catSwitchesBlockedByWeak += node.CatSwitchesBlockedByWeak;
                autoTargetAttempts += node.AutoTargetAttempts;
                autoTargetsAcquired += node.AutoTargetsAcquired;
                skillTargetAttempts += node.SkillTargetAttempts;
                skillTargetsAcquired += node.SkillTargetsAcquired;
                skillCastAttempts += node.SkillCastAttempts;
                skillCastsSucceeded += node.SkillCastsSucceeded;
                skillCastsBlockedByCooldown += node.SkillCastsBlockedByCooldown;
                skillCastsBlockedByTarget += node.SkillCastsBlockedByTarget;
                skillCastsBlockedByMissingDefinition += node.SkillCastsBlockedByMissingDefinition;
                interactionAttempts += node.InteractionAttempts;
                interactionSuccesses += node.InteractionSuccesses;
                interactionBlockedByRange += node.InteractionBlockedByRange;
            }

            return new RunMetricsSummary(
                nodes.Count,
                successes,
                failures,
                duration,
                sleepDelta,
                litterBoxUses,
                feederUses,
                bedCareUses,
                poopIncidents,
                sleepMaxLost,
                weakIncidents,
                catPressureEvents,
                catDamageIncoming,
                catDamageTaken,
                catDamageAbsorbed,
                catHealEvents,
                catHealingApplied,
                catShieldEvents,
                catShieldApplied,
                bedPressureHits,
                bossThrowPressureHits,
                enemySleepPressureEvents,
                enemySleepDamageIncoming,
                enemySleepDamageTaken,
                enemySleepDamageAbsorbed,
                catSwitchAttempts,
                catSwitchesSucceeded,
                catSwitchesBlockedByWeak,
                autoTargetAttempts,
                autoTargetsAcquired,
                skillTargetAttempts,
                skillTargetsAcquired,
                skillCastAttempts,
                skillCastsSucceeded,
                skillCastsBlockedByCooldown,
                skillCastsBlockedByTarget,
                skillCastsBlockedByMissingDefinition,
                interactionAttempts,
                interactionSuccesses,
                interactionBlockedByRange);
        }

        public NodeMetrics GetLatestNode()
        {
            if (nodes.Count == 0)
            {
                throw new InvalidOperationException("No node metrics have been recorded.");
            }

            return nodes[nodes.Count - 1];
        }
    }
}
