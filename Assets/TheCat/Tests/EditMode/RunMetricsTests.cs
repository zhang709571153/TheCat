using NUnit.Framework;
using TheCat.Data;

namespace TheCat.Tests
{
    public sealed class RunMetricsTests
    {
        [Test]
        public void Summary_AggregatesNodeMetrics()
        {
            RunMetrics runMetrics = new RunMetrics();
            NodeMetrics first = runMetrics.BeginNode(1, "layer_01_defense", 100f);
            first.RecordLitterBoxUse();
            first.RecordFeederUse();
            first.RecordBedCareUse();
            first.RecordCatPressure(12f, 5f);
            first.RecordCatHeal(18f);
            first.RecordCatShield(22f);
            first.RecordBedPressure(10f, 7f);
            first.RecordBossThrowPressure(4f, 0f);
            first.RecordCatSwitchSuccess();
            first.RecordCatSwitchBlockedByWeak();
            first.RecordAutoTargetAcquired();
            first.RecordSkillTargetAcquired();
            first.RecordSkillTargetMissed();
            first.RecordSkillCastSuccess();
            first.RecordSkillCastBlockedByCooldown();
            first.RecordSkillCastBlockedByTarget();
            first.RecordSkillCastBlockedByMissingDefinition();
            first.RecordInteractionBlockedByRange();
            first.Complete(NodeResult.Success, 60f, 92f);

            NodeMetrics second = runMetrics.BeginNode(2, "layer_02_elite", 92f);
            second.RecordPoopIncident();
            second.RecordSleepMaxLoss(10f);
            second.RecordWeakIncident();
            second.Complete(NodeResult.Failure, 75f, 0f);

            RunMetricsSummary summary = runMetrics.GetSummary();

            Assert.AreEqual(2, summary.NodeCount);
            Assert.AreEqual(1, summary.SuccessCount);
            Assert.AreEqual(1, summary.FailureCount);
            Assert.AreEqual(135f, summary.DurationSeconds);
            Assert.AreEqual(-100f, summary.SleepDelta);
            Assert.AreEqual(1, summary.LitterBoxUses);
            Assert.AreEqual(1, summary.FeederUses);
            Assert.AreEqual(1, summary.BedCareUses);
            Assert.AreEqual(1, summary.PoopIncidents);
            Assert.AreEqual(10f, summary.SleepMaxLost);
            Assert.AreEqual(1, summary.WeakIncidents);
            Assert.AreEqual(1, summary.CatPressureEvents);
            Assert.AreEqual(12f, summary.CatDamageIncoming);
            Assert.AreEqual(5f, summary.CatDamageTaken);
            Assert.AreEqual(7f, summary.CatDamageAbsorbed);
            Assert.AreEqual(1, summary.CatHealEvents);
            Assert.AreEqual(18f, summary.CatHealingApplied);
            Assert.AreEqual(1, summary.CatShieldEvents);
            Assert.AreEqual(22f, summary.CatShieldApplied);
            Assert.AreEqual(1, summary.BedPressureHits);
            Assert.AreEqual(1, summary.BossThrowPressureHits);
            Assert.AreEqual(2, summary.EnemySleepPressureEvents);
            Assert.AreEqual(14f, summary.EnemySleepDamageIncoming);
            Assert.AreEqual(7f, summary.EnemySleepDamageTaken);
            Assert.AreEqual(7f, summary.EnemySleepDamageAbsorbed);
            Assert.AreEqual(2, summary.CatSwitchAttempts);
            Assert.AreEqual(1, summary.CatSwitchesSucceeded);
            Assert.AreEqual(1, summary.CatSwitchesBlockedByWeak);
            Assert.AreEqual(1, summary.AutoTargetAttempts);
            Assert.AreEqual(1, summary.AutoTargetsAcquired);
            Assert.AreEqual(2, summary.SkillTargetAttempts);
            Assert.AreEqual(1, summary.SkillTargetsAcquired);
            Assert.AreEqual(4, summary.SkillCastAttempts);
            Assert.AreEqual(1, summary.SkillCastsSucceeded);
            Assert.AreEqual(1, summary.SkillCastsBlockedByCooldown);
            Assert.AreEqual(1, summary.SkillCastsBlockedByTarget);
            Assert.AreEqual(1, summary.SkillCastsBlockedByMissingDefinition);
            Assert.AreEqual(4, summary.InteractionAttempts);
            Assert.AreEqual(3, summary.InteractionSuccesses);
            Assert.AreEqual(1, summary.InteractionBlockedByRange);
        }
    }
}
