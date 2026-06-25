using System;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0GoldenPathSimulatorTests
    {
        [Test]
        public void SimulateDefaultRun_CompletesTenLayerRouteThroughBossWithRealBattles()
        {
            P0GoldenPathReport report = P0GoldenPathSimulator.SimulateDefaultRun();

            Assert.IsTrue(report.IsCleared, report.BuildSummary());
            Assert.IsFalse(report.Settlement.IsFailed);
            Assert.AreEqual(10, report.Settlement.CompletedNodes);
            Assert.AreEqual(10, report.Settlement.TotalLayers);
            Assert.AreEqual(5, report.BattleCount);
            Assert.AreEqual(5, report.Settlement.BattleSuccesses);
            Assert.AreEqual(0, report.Settlement.BattleFailures);
            Assert.IsTrue(report.BossBattleCleared);
            Assert.IsTrue(report.BossBehaviorObserved);
            Assert.Greater(report.Settlement.DreamShards, 0);
            Assert.Greater(report.Settlement.FishTreats, 0);
            Assert.AreEqual(1, report.Run.DreamEventsResolved);
            Assert.AreEqual(1, report.Run.ShopPurchases);
            Assert.AreEqual(1, report.Run.RestNestUses);
            Assert.AreEqual(1, report.Run.Blessings.Count);
            Assert.IsTrue(report.Run.Roster.HasCat(P0RouteRewardResolver.PreviewPartnerId));
            Assert.Greater(report.Settlement.OwnerSleepCurrent, 0f);
            Assert.Greater(report.Settlement.EnemySleepPressureEvents, 0);
            Assert.Greater(report.Settlement.EnemySleepDamageIncoming, 0f);
            Assert.Greater(report.Settlement.CatHealEvents, 0);
            Assert.Greater(report.Settlement.CatHealingApplied, 0f);
            Assert.Greater(report.Settlement.CatShieldEvents, 0);
            Assert.Greater(report.Settlement.CatShieldApplied, 0f);
            Assert.Greater(report.Settlement.CatSwitchAttempts, 0);
            Assert.Greater(report.Settlement.CatSwitchesSucceeded, 0);
            Assert.Greater(report.Settlement.AutoTargetAttempts, 0);
            Assert.Greater(report.Settlement.AutoTargetsAcquired, 0);
            Assert.Greater(report.Settlement.SkillTargetAttempts, 0);
            Assert.Greater(report.Settlement.SkillTargetsAcquired, 0);
            Assert.Greater(report.Settlement.SkillCastAttempts, 0);
            Assert.Greater(report.Settlement.SkillCastsSucceeded, 0);
            Assert.Greater(report.Settlement.InteractionAttempts, 0);
            Assert.Greater(report.Settlement.InteractionSuccesses, 0);
        }

        [Test]
        public void SimulateDefaultRun_RecordsVictoryReportsForEveryBattleNode()
        {
            P0GoldenPathReport report = P0GoldenPathSimulator.SimulateDefaultRun();

            Assert.AreEqual(P0RouteCatalog.LayerOneDefenseId, report.BattleReports[0].NodeId);
            for (int i = 0; i < report.BattleReports.Count; i++)
            {
                P0GoldenPathBattleReport battleReport = report.BattleReports[i];
                Assert.AreEqual(BattleOutcome.Victory, battleReport.Outcome, battleReport.BuildSummary());
                Assert.Greater(battleReport.DurationSeconds, 0f, battleReport.BuildSummary());
                Assert.IsNotEmpty(battleReport.WaveId);
                Assert.Greater(battleReport.OwnerSleepCurrent, 0f, battleReport.BuildSummary());
                StringAssert.Contains("pressure", battleReport.BuildSummary());
                StringAssert.Contains("catVitals", battleReport.BuildSummary());
                Assert.Greater(battleReport.CatHealEvents, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.CatHealingApplied, 0f, battleReport.BuildSummary());
                Assert.Greater(battleReport.CatShieldEvents, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.CatShieldApplied, 0f, battleReport.BuildSummary());
                Assert.Greater(battleReport.CatSwitchAttempts, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.CatSwitchesSucceeded, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.AutoTargetAttempts, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.AutoTargetsAcquired, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.SkillTargetAttempts, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.SkillTargetsAcquired, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.SkillCastAttempts, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.SkillCastsSucceeded, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.InteractionAttempts, 0, battleReport.BuildSummary());
                Assert.Greater(battleReport.InteractionSuccesses, 0, battleReport.BuildSummary());
                StringAssert.Contains("switches", battleReport.BuildSummary());
                StringAssert.Contains("targets", battleReport.BuildSummary());
                StringAssert.Contains("skills", battleReport.BuildSummary());
                StringAssert.Contains("interactions", battleReport.BuildSummary());
            }
        }

        [Test]
        public void SimulateRun_WithAssistedOpeningActionsDisabled_LeavesActionTelemetryEmpty()
        {
            P0GoldenPathReport report = P0GoldenPathSimulator.SimulateRun(
                P0RunSession.CreateDefaultStarterCatIds(),
                new P0GoldenPathSimulationOptions(useAssistedOpeningActions: false));

            Assert.IsTrue(report.IsCleared, report.BuildSummary());
            Assert.AreEqual(0, report.Settlement.CatSwitchAttempts);
            Assert.AreEqual(0, report.Settlement.CatSwitchesSucceeded);
            Assert.AreEqual(0, report.Settlement.AutoTargetAttempts);
            Assert.AreEqual(0, report.Settlement.AutoTargetsAcquired);
            Assert.AreEqual(0, report.Settlement.SkillTargetAttempts);
            Assert.AreEqual(0, report.Settlement.SkillTargetsAcquired);
            Assert.AreEqual(0, report.Settlement.SkillCastAttempts);
            Assert.AreEqual(0, report.Settlement.SkillCastsSucceeded);
            Assert.AreEqual(0, report.Settlement.CatHealEvents);
            Assert.AreEqual(0f, report.Settlement.CatHealingApplied);
            Assert.AreEqual(0, report.Settlement.CatShieldEvents);
            Assert.AreEqual(0f, report.Settlement.CatShieldApplied);
            Assert.AreEqual(0, report.Settlement.InteractionAttempts);
            Assert.AreEqual(0, report.Settlement.InteractionSuccesses);
        }

        [Test]
        public void SimulateDefaultRun_BossReportShowsSummonAndThrowPressure()
        {
            P0GoldenPathReport report = P0GoldenPathSimulator.SimulateDefaultRun();

            Assert.IsTrue(report.TryGetBossBattleReport(out P0GoldenPathBattleReport bossReport));
            Assert.AreEqual(P0RouteCatalog.BossNodeId, bossReport.NodeId);
            Assert.AreEqual("boss_call_tyrant", bossReport.WaveId);
            Assert.GreaterOrEqual(bossReport.BossSummonsTriggered, 1);
            Assert.GreaterOrEqual(bossReport.BossThrowsTriggered, 1);
            Assert.GreaterOrEqual(bossReport.BossThrowPressureHits, 1);
            Assert.GreaterOrEqual(bossReport.EnemySleepPressureEvents, 1);
            Assert.Greater(bossReport.EnemySleepDamageIncoming, 0f);
            Assert.AreEqual(5, bossReport.Reward.DreamShards);
            Assert.AreEqual(3, bossReport.Reward.FishTreats);
        }

        [Test]
        public void Acceptance_DefaultRunHasNoBlockingFailures()
        {
            P0GoldenPathReport report = P0GoldenPathSimulator.SimulateDefaultRun();

            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(report);

            Assert.IsTrue(acceptance.IsAccepted, acceptance.BuildDetailedSummary());
            Assert.AreEqual(0, acceptance.FailureCount);
            StringAssert.Contains("P0 golden path accepted", acceptance.BuildSummary());
            StringAssert.Contains("Enemy pressure", acceptance.BuildDetailedSummary());
            StringAssert.Contains("Cat vitality", acceptance.BuildDetailedSummary());
            StringAssert.Contains("shields 5/", acceptance.BuildDetailedSummary());
            StringAssert.Contains("Action telemetry", acceptance.BuildDetailedSummary());
            StringAssert.Contains("switches 10/10", acceptance.BuildDetailedSummary());
            StringAssert.Contains("targets auto 5/5 skill 5/5", acceptance.BuildDetailedSummary());
            StringAssert.Contains("skills 15/15", acceptance.BuildDetailedSummary());
            StringAssert.Contains("interactions 15/15", acceptance.BuildDetailedSummary());
        }

        [Test]
        public void Acceptance_RejectsClearedRunWithoutActionTelemetry()
        {
            P0GoldenPathReport report = P0GoldenPathSimulator.SimulateRun(
                P0RunSession.CreateDefaultStarterCatIds(),
                new P0GoldenPathSimulationOptions(useAssistedOpeningActions: false));

            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(report);

            Assert.IsFalse(acceptance.IsAccepted);
            Assert.Greater(acceptance.FailureCount, 0);
            StringAssert.Contains("cat switch telemetry", acceptance.BuildDetailedSummary());
            StringAssert.Contains("auto target telemetry", acceptance.BuildDetailedSummary());
            StringAssert.Contains("skill target telemetry", acceptance.BuildDetailedSummary());
            StringAssert.Contains("skill action telemetry", acceptance.BuildDetailedSummary());
            StringAssert.Contains("interaction telemetry", acceptance.BuildDetailedSummary());
        }

        [Test]
        public void Acceptance_RejectsIncompleteReport()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                P0RunSession.CreateDefaultStarterCatIds());
            P0GoldenPathReport report = new P0GoldenPathReport(run, Array.Empty<P0GoldenPathBattleReport>());

            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(report);

            Assert.IsFalse(acceptance.IsAccepted);
            Assert.Greater(acceptance.FailureCount, 0);
            StringAssert.Contains("rejected", acceptance.BuildSummary());
        }

        [Test]
        public void AcceptanceProfile_RejectsInvalidThresholds()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(expectedTotalLayers: 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(expectedBattleCount: 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumOwnerSleep: -1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(warningTotalDurationSeconds: 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumEnemySleepPressureEvents: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumEnemySleepDamageIncoming: -1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumCatShieldEvents: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumCatSwitchesPerBattle: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumAutoTargetsPerBattle: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumSkillTargetsPerBattle: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumSkillCastsPerBattle: -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathAcceptanceProfile(minimumInteractionsPerBattle: -1));
        }

        [Test]
        public void Options_RejectInvalidSimulationParameters()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathSimulationOptions(tickDeltaSeconds: 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathSimulationOptions(maxTicksPerBattle: 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathSimulationOptions(damagePerSweep: 0f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0GoldenPathSimulationOptions(bossMinimumObservationSeconds: -0.1f));
        }
    }
}
