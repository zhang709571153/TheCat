using NUnit.Framework;
using TheCat.Data;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0GrayboxTelemetryTests
    {
        [Test]
        public void EvaluateGoldenPath_ReportsP0GrayboxMetrics()
        {
            P0GrayboxTelemetryReport report = P0GrayboxTelemetry.EvaluateGoldenPath();

            Assert.IsTrue(report.IsUsable, report.BuildDetailedSummary());
            Assert.AreEqual(5, report.Summary.NodeCount);
            Assert.AreEqual(5, report.CompletedNodeCount);
            Assert.AreEqual(5, report.Summary.SuccessCount);
            Assert.AreEqual(0, report.Summary.FailureCount);
            Assert.Greater(report.Summary.DurationSeconds, 0f);
            Assert.AreEqual(report.Summary.PoopIncidents, report.Settlement.PoopIncidents);
            Assert.AreEqual(report.Summary.SleepMaxLost, report.Settlement.SleepMaxLost);
            Assert.AreEqual(report.Summary.LitterBoxUses, report.Settlement.LitterBoxUses);
            Assert.AreEqual(report.Summary.FeederUses, report.Settlement.FeederUses);
            Assert.AreEqual(report.Summary.WeakIncidents, report.Settlement.WeakIncidents);
            Assert.AreEqual(report.Summary.BedPressureHits, report.Settlement.BedPressureHits);
            Assert.AreEqual(report.Summary.BossThrowPressureHits, report.Settlement.BossThrowPressureHits);
            Assert.AreEqual(report.Summary.EnemySleepPressureEvents, report.Settlement.EnemySleepPressureEvents);
            Assert.AreEqual(report.Summary.EnemySleepDamageIncoming, report.Settlement.EnemySleepDamageIncoming);
            Assert.AreEqual(report.Summary.EnemySleepDamageTaken, report.Settlement.EnemySleepDamageTaken);
            Assert.AreEqual(report.Summary.CatPressureEvents, report.Settlement.CatPressureEvents);
            Assert.AreEqual(report.Summary.CatDamageIncoming, report.Settlement.CatDamageIncoming);
            Assert.AreEqual(report.Summary.CatDamageTaken, report.Settlement.CatDamageTaken);
            Assert.AreEqual(report.Summary.CatHealEvents, report.Settlement.CatHealEvents);
            Assert.AreEqual(report.Summary.CatHealingApplied, report.Settlement.CatHealingApplied);
            Assert.AreEqual(report.Summary.CatShieldEvents, report.Settlement.CatShieldEvents);
            Assert.AreEqual(report.Summary.CatShieldApplied, report.Settlement.CatShieldApplied);
            Assert.AreEqual(report.Summary.CatSwitchAttempts, report.Settlement.CatSwitchAttempts);
            Assert.AreEqual(report.Summary.CatSwitchesSucceeded, report.Settlement.CatSwitchesSucceeded);
            Assert.AreEqual(report.Summary.AutoTargetAttempts, report.Settlement.AutoTargetAttempts);
            Assert.AreEqual(report.Summary.AutoTargetsAcquired, report.Settlement.AutoTargetsAcquired);
            Assert.AreEqual(report.Summary.SkillTargetAttempts, report.Settlement.SkillTargetAttempts);
            Assert.AreEqual(report.Summary.SkillTargetsAcquired, report.Settlement.SkillTargetsAcquired);
            Assert.AreEqual(report.Summary.SkillCastAttempts, report.Settlement.SkillCastAttempts);
            Assert.AreEqual(report.Summary.SkillCastsSucceeded, report.Settlement.SkillCastsSucceeded);
            Assert.AreEqual(report.Summary.InteractionAttempts, report.Settlement.InteractionAttempts);
            Assert.AreEqual(report.Summary.InteractionSuccesses, report.Settlement.InteractionSuccesses);
            Assert.Greater(report.Summary.EnemySleepPressureEvents, 0);
            Assert.Greater(report.Summary.EnemySleepDamageIncoming, 0f);
            Assert.Greater(report.Summary.CatHealEvents, 0);
            Assert.Greater(report.Summary.CatHealingApplied, 0f);
            Assert.Greater(report.Summary.CatShieldEvents, 0);
            Assert.Greater(report.Summary.CatShieldApplied, 0f);
            Assert.Greater(report.Summary.CatSwitchesSucceeded, 0);
            Assert.Greater(report.Summary.AutoTargetsAcquired, 0);
            Assert.Greater(report.Summary.SkillTargetsAcquired, 0);
            Assert.Greater(report.Summary.SkillCastsSucceeded, 0);
            Assert.Greater(report.Summary.InteractionSuccesses, 0);
            StringAssert.Contains("poop", report.BuildSummary());
            StringAssert.Contains("litter", report.BuildSummary());
            StringAssert.Contains("feeder", report.BuildSummary());
            StringAssert.Contains("weak", report.BuildSummary());
            StringAssert.Contains("cat pressure", report.BuildSummary());
            StringAssert.Contains("pressure", report.BuildSummary());
            StringAssert.Contains("switches", report.BuildSummary());
            StringAssert.Contains("targets", report.BuildSummary());
            StringAssert.Contains("skills", report.BuildSummary());
            StringAssert.Contains("interactions", report.BuildSummary());
        }

        [Test]
        public void Evaluate_NullRunFailsTelemetry()
        {
            P0GrayboxTelemetryReport report = P0GrayboxTelemetry.Evaluate((RunProgressionState)null);

            Assert.IsFalse(report.IsUsable);
            Assert.Greater(report.FailureCount, 0);
            StringAssert.Contains("missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RunWithoutBattleMetricsFailsCoverage()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());

            P0GrayboxTelemetryReport report = P0GrayboxTelemetry.Evaluate(run);

            Assert.IsFalse(report.IsUsable);
            Assert.AreEqual(0, report.Summary.NodeCount);
            StringAssert.Contains("No battle node telemetry", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_InProgressNodeWarnsButKeepsTelemetryUsable()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            NodeMetrics node = run.Metrics.BeginNode(1, "layer_01_defense", 100f);
            node.RecordLitterBoxUse();

            P0GrayboxTelemetryReport report = P0GrayboxTelemetry.Evaluate(run);

            Assert.IsFalse(report.IsUsable);
            Assert.Greater(report.WarningCount, 0);
            StringAssert.Contains("still in progress", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_FailedNodeRecordsWarningAndTotals()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            NodeMetrics node = run.Metrics.BeginNode(1, "layer_01_defense", 100f);
            node.RecordPoopIncident();
            node.RecordSleepMaxLoss(10f);
            node.RecordWeakIncident();
            node.Complete(NodeResult.Failure, 45f, 0f);

            P0GrayboxTelemetryReport report = P0GrayboxTelemetry.Evaluate(run);

            Assert.IsTrue(report.IsUsable, report.BuildDetailedSummary());
            Assert.AreEqual(1, report.Summary.FailureCount);
            Assert.AreEqual(1, report.Summary.PoopIncidents);
            Assert.AreEqual(10f, report.Summary.SleepMaxLost);
            Assert.AreEqual(1, report.Summary.WeakIncidents);
            Assert.Greater(report.WarningCount, 0);
            StringAssert.Contains("failed battle", report.BuildDetailedSummary());
        }

        [Test]
        public void NodeRow_BuildSummaryIncludesExplicitGreyboxMetrics()
        {
            NodeMetrics node = new NodeMetrics(2, "elite_test", 80f);
            node.RecordLitterBoxUse();
            node.RecordFeederUse();
            node.RecordBedCareUse();
            node.RecordPoopIncident();
            node.RecordSleepMaxLoss(5f);
            node.RecordWeakIncident();
            node.RecordCatPressure(12f, 5f);
            node.RecordCatHeal(18f);
            node.RecordCatShield(22f);
            node.RecordBedPressure(10f, 6f);
            node.RecordBossThrowPressure(4f, 0f);
            node.RecordCatSwitchSuccess();
            node.RecordCatSwitchBlockedByWeak();
            node.RecordAutoTargetAcquired();
            node.RecordSkillTargetAcquired();
            node.RecordSkillTargetMissed();
            node.RecordSkillCastSuccess();
            node.RecordSkillCastBlockedByCooldown();
            node.RecordSkillCastBlockedByTarget();
            node.RecordSkillCastBlockedByMissingDefinition();
            node.RecordInteractionBlockedByRange();
            node.Complete(NodeResult.Success, 60f, 70f);

            P0GrayboxTelemetryNodeRow row = new P0GrayboxTelemetryNodeRow(node);
            string summary = row.BuildSummary();

            StringAssert.Contains("time 60.0s", summary);
            StringAssert.Contains("sleep -10.0", summary);
            StringAssert.Contains("poop 1", summary);
            StringAssert.Contains("maxLost 5", summary);
            StringAssert.Contains("litter 1", summary);
            StringAssert.Contains("feeder 1", summary);
            StringAssert.Contains("bed 1", summary);
            StringAssert.Contains("weak 1", summary);
            StringAssert.Contains("catPressure 1", summary);
            StringAssert.Contains("catDamage 5/12", summary);
            StringAssert.Contains("catAbsorbed 7", summary);
            StringAssert.Contains("catHeal 1/18", summary);
            StringAssert.Contains("catShield 1/22", summary);
            StringAssert.Contains("enemyPressure 2", summary);
            StringAssert.Contains("bedHits 1", summary);
            StringAssert.Contains("bossThrows 1", summary);
            StringAssert.Contains("sleep 6/14", summary);
            StringAssert.Contains("absorbed 8", summary);
            StringAssert.Contains("switches 1/2", summary);
            StringAssert.Contains("switchBlocks weak 1", summary);
            StringAssert.Contains("targets auto 1/1 skill 1/2", summary);
            StringAssert.Contains("skills 1/4", summary);
            StringAssert.Contains("skillBlocks cd 1 target 1 missing 1", summary);
            StringAssert.Contains("interactions 3/4", summary);
            StringAssert.Contains("rangeBlocks 1", summary);
        }
    }
}
