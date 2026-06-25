using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;

namespace TheCat.Tests
{
    public sealed class BattleSimulationTests
    {
        [Test]
        public void Tick_SpawnsLayerOneEnemies()
        {
            BattleSimulation battle = CreateLayerOneBattle();

            battle.Tick(0.1f);

            Assert.AreEqual(1, battle.ActiveEnemies.Count);
            Assert.AreEqual(BattleOutcome.InProgress, battle.Outcome);
        }

        [Test]
        public void Tick_SpawnedEnemiesCarrySpawnGateId()
        {
            BattleSimulation battle = CreateLayerOneBattle();

            battle.Tick(0.1f);

            Assert.AreEqual("north", battle.ActiveEnemies[0].SpawnGateId);
        }

        [Test]
        public void ApplyDamageToNearestEnemy_CanClearWaveAndWin()
        {
            BattleSimulation battle = CreateLayerOneBattle();

            for (int i = 0; i < 20 && battle.Outcome == BattleOutcome.InProgress; i++)
            {
                battle.Tick(5f);
                while (battle.ApplyDamageToNearestEnemy(999f))
                {
                }
            }

            Assert.AreEqual(BattleOutcome.Victory, battle.Outcome);
            Assert.AreEqual(1, battle.RunMetrics.GetSummary().SuccessCount);
        }

        [Test]
        public void ApplyDamageToEnemy_DamagesSpecificActiveEnemyOnly()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateColdLightEliteWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default);
            BattleSimulation battle = new BattleSimulation(config, new RunMetrics());
            battle.Tick(0.1f);
            battle.Tick(6f);

            BattleEnemyState first = battle.ActiveEnemies[0];
            BattleEnemyState second = battle.ActiveEnemies[1];

            bool applied = battle.ApplyDamageToEnemy(second, 10f);

            Assert.IsTrue(applied);
            Assert.AreEqual(first.Definition.MaxHp, first.CurrentHp, 0.001f);
            Assert.AreEqual(second.Definition.MaxHp - 10f, second.CurrentHp, 0.001f);
        }

        [Test]
        public void ApplyDamageToEnemy_RejectsInactiveTarget()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            BattleEnemyState inactive = new BattleEnemyState(99, GetEnemy(P0PrototypeCatalog.BlackMudNightmareId), 4f);

            Assert.IsFalse(battle.ApplyDamageToEnemy(inactive, 10f));
            Assert.Throws<System.ArgumentOutOfRangeException>(() => battle.ApplyDamageToEnemy(inactive, -1f));
        }

        [Test]
        public void EnemiesReachingBed_ReduceSleep()
        {
            BattleSimulation battle = CreateLayerOneBattle();

            battle.Tick(10f);

            Assert.Less(battle.OwnerSleep.Current, 100f);
            Assert.AreEqual(1, battle.NodeMetrics.BedPressureHits);
            Assert.AreEqual(1, battle.NodeMetrics.EnemySleepPressureEvents);
            Assert.Greater(battle.NodeMetrics.EnemySleepDamageIncoming, 0f);
            Assert.Greater(battle.NodeMetrics.EnemySleepDamageTaken, 0f);
        }

        [Test]
        public void Interactions_RecordMetricsAndAffectCoreValues()
        {
            BattleSimulation battle = CreateLayerOneBattle();

            battle.TeamPoop.Tick(180f, P0Tuning.Default, isDigesting: false, layer: 1);
            battle.RecordLitterBoxUse();
            battle.TeamHunger.Spend(60f);
            battle.RecordFeederUse();
            battle.OwnerSleep.ApplyDamage(20f);
            float sleepBeforeBedCare = battle.OwnerSleep.Current;
            float hungerBeforeBedCare = battle.TeamHunger.Current;
            float restored = battle.RecordBedCareUse();

            Assert.AreEqual(1, battle.NodeMetrics.LitterBoxUses);
            Assert.AreEqual(1, battle.NodeMetrics.FeederUses);
            Assert.AreEqual(1, battle.NodeMetrics.BedCareUses);
            Assert.AreEqual(3, battle.NodeMetrics.InteractionAttempts);
            Assert.AreEqual(3, battle.NodeMetrics.InteractionSuccesses);
            Assert.Less(battle.TeamPoop.Current, 100f);
            Assert.IsTrue(battle.TeamHunger.IsDigesting);
            Assert.Greater(restored, 0f);
            Assert.Greater(battle.OwnerSleep.Current, sleepBeforeBedCare);
            Assert.Less(battle.TeamHunger.Current, hungerBeforeBedCare);
        }

        [Test]
        public void CastSkill_RecordsSkillCastTelemetry()
        {
            BattleSimulation battle = CreateLayerOneBattleWithStatuses();

            battle.CastSkill(GetSkill("saiban_oath_shield"), new CatBattleState(P0PrototypeCatalog.CreateStarterCats()[0]));

            Assert.AreEqual(1, battle.NodeMetrics.SkillCastAttempts);
            Assert.AreEqual(1, battle.NodeMetrics.SkillCastsSucceeded);
            Assert.AreEqual(1, battle.NodeMetrics.CatShieldEvents);
            Assert.AreEqual(35f, battle.NodeMetrics.CatShieldApplied, 0.001f);
        }

        [Test]
        public void BattleSimulation_UsesConfiguredStartingCoreValues()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateLayerOneWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default,
                startingSleep: 45f,
                startingSleepMax: 80f,
                startingSleepBaseMax: 100f,
                startingPoop: 70f,
                startingHunger: 35f);

            BattleSimulation battle = new BattleSimulation(config, new RunMetrics());

            Assert.AreEqual(45f, battle.OwnerSleep.Current, 0.001f);
            Assert.AreEqual(80f, battle.OwnerSleep.Max, 0.001f);
            Assert.AreEqual(100f, battle.OwnerSleep.BaseMax, 0.001f);
            Assert.AreEqual(70f, battle.TeamPoop.Current, 0.001f);
            Assert.AreEqual(35f, battle.TeamHunger.Current, 0.001f);
            Assert.AreEqual(45f, battle.NodeMetrics.StartingSleep, 0.001f);
        }

        [Test]
        public void BossWave_CallTyrantSummonsAndThrows()
        {
            BattleSimulation battle = CreateBossBattle();

            battle.Tick(8.1f);

            Assert.GreaterOrEqual(battle.BossSummonsTriggered, 1);
            Assert.GreaterOrEqual(battle.BossThrowsTriggered, 1);
            Assert.IsTrue(battle.ActiveEnemies.Any(enemy => enemy.Definition.Id == P0PrototypeCatalog.CallTyrantId));
            Assert.IsTrue(battle.ActiveEnemies.Any(enemy => enemy.Definition.Id == P0PrototypeCatalog.BlackMudNightmareId));
            Assert.Less(battle.OwnerSleep.Current, 100f);
            Assert.GreaterOrEqual(battle.NodeMetrics.BossThrowPressureHits, 1);
            Assert.GreaterOrEqual(battle.NodeMetrics.EnemySleepPressureEvents, 1);
            Assert.Greater(battle.NodeMetrics.EnemySleepDamageIncoming, 0f);
        }

        [Test]
        public void PoopIncident_RecordsSleepMaxLoss()
        {
            BattleSimulation battle = CreateLayerOneBattle();

            battle.TeamPoop.Tick(400f, P0Tuning.Default, isDigesting: false, layer: 1);
            battle.TeamPoop.Tick(TeamPoopGauge.EarlyLayerCountdownSeconds, P0Tuning.Default, isDigesting: false, layer: 1);
            battle.Tick(0f);

            Assert.AreEqual(1, battle.NodeMetrics.PoopIncidents);
            Assert.AreEqual(P0Tuning.Default.PoopSleepMaxPenalty, battle.NodeMetrics.SleepMaxLost, 0.001f);
            Assert.AreEqual(OwnerSleepState.DefaultBaseMax - P0Tuning.Default.PoopSleepMaxPenalty, battle.OwnerSleep.Max, 0.001f);
        }

        [Test]
        public void EnemyWarningFormatter_NearBedShowsBedWarning()
        {
            BattleEnemyState enemy = new BattleEnemyState(
                1,
                GetEnemy(P0PrototypeCatalog.BlackMudNightmareId),
                EnemyWarningFormatter.BedWarningThresholdSeconds);

            string warning = EnemyWarningFormatter.Format(enemy);

            StringAssert.Contains("压床", warning);
            StringAssert.Contains("2.5s", warning);
            StringAssert.DoesNotContain("_", warning);
        }

        [Test]
        public void EnemyWarningFormatter_BossCountdownsShowWarnings()
        {
            BattleEnemyState boss = new BattleEnemyState(
                1,
                GetEnemy(P0PrototypeCatalog.CallTyrantId),
                10f);

            boss.TickBossSummon(6.1f, 8f);
            boss.TickBossThrow(3.1f, 6f);

            string warning = EnemyWarningFormatter.Format(boss);

            StringAssert.Contains("首领召唤", warning);
            StringAssert.Contains("首领投掷", warning);
            StringAssert.DoesNotContain("_", warning);
        }

        [Test]
        public void EnemyWarningFormatter_BehaviorWarningsShowRangedAndFlyingPressure()
        {
            BattleEnemyState train = new BattleEnemyState(
                3,
                GetEnemy(P0PrototypeCatalog.DreamRailToyTrainId),
                EnemyWarningFormatter.ChargeWarningThresholdSeconds);
            BattleEnemyState alarm = new BattleEnemyState(
                1,
                GetEnemy(P0PrototypeCatalog.RedEyeAlarmId),
                EnemyWarningFormatter.RangedPressureWarningThresholdSeconds);
            BattleEnemyState flyer = new BattleEnemyState(
                2,
                GetEnemy(P0PrototypeCatalog.UnreadRedDotFlyerId),
                EnemyWarningFormatter.FlyingAttachWarningThresholdSeconds);
            BattleEnemyState teddy = new BattleEnemyState(
                4,
                GetEnemy(P0PrototypeCatalog.FallingDreamTeddyId),
                EnemyWarningFormatter.JumpSlamWarningThresholdSeconds);

            string trainWarning = EnemyWarningFormatter.Format(train);
            string alarmWarning = EnemyWarningFormatter.Format(alarm);
            string flyerWarning = EnemyWarningFormatter.Format(flyer);
            string teddyWarning = EnemyWarningFormatter.Format(teddy);

            StringAssert.Contains("冲锋路线", trainWarning);
            StringAssert.Contains("远程压制", alarmWarning);
            StringAssert.Contains("飞虫附着", flyerWarning);
            StringAssert.Contains("跳砸", teddyWarning);
            StringAssert.DoesNotContain("_", trainWarning);
            StringAssert.DoesNotContain("_", alarmWarning);
            StringAssert.DoesNotContain("_", flyerWarning);
            StringAssert.DoesNotContain("_", teddyWarning);
        }

        [Test]
        public void RedEyeAlarmEliteWave_SpawnsAlarmAndUnreadFlyers()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateRedEyeAlarmEliteWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default);
            BattleSimulation battle = new BattleSimulation(config, new RunMetrics());

            battle.Tick(4.1f);

            Assert.IsTrue(battle.ActiveEnemies.Any(enemy => enemy.Definition.Id == P0PrototypeCatalog.RedEyeAlarmId));
            Assert.IsTrue(battle.ActiveEnemies.Any(enemy => enemy.Definition.Id == P0PrototypeCatalog.UnreadRedDotFlyerId));
        }

        [Test]
        public void LayerSixWave_SpawnsDreamRailToyTrain()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateLayerSixDefenseWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default);
            BattleSimulation battle = new BattleSimulation(config, new RunMetrics());

            battle.Tick(18.1f);

            Assert.IsTrue(battle.ActiveEnemies.Any(enemy => enemy.Definition.Id == P0PrototypeCatalog.DreamRailToyTrainId));
        }

        [Test]
        public void FallingDreamTeddyEliteWave_SpawnsTeddy()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateFallingDreamTeddyEliteWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default);
            BattleSimulation battle = new BattleSimulation(config, new RunMetrics());

            battle.Tick(0.1f);

            Assert.IsTrue(battle.ActiveEnemies.Any(enemy => enemy.Definition.Id == P0PrototypeCatalog.FallingDreamTeddyId));
        }

        [Test]
        public void DebugSpawnEnemy_SpawnsRequestedEnemyForSmokeValidation()
        {
            BattleSimulation battle = CreateLayerOneBattleWithStatuses();

            BattleEnemyState enemy = battle.DebugSpawnEnemy(
                P0PrototypeCatalog.UnreadRedDotFlyerId,
                3f,
                "east");

            Assert.AreSame(enemy, battle.ActiveEnemies.Last());
            Assert.AreEqual(P0PrototypeCatalog.UnreadRedDotFlyerId, enemy.Definition.Id);
            Assert.AreEqual(3f, enemy.TimeToBedSeconds, 0.001f);
            Assert.AreEqual("east", enemy.SpawnGateId);
        }

        [Test]
        public void DebugStatusSmokeMethods_ApplyEnemyAndBedStatuses()
        {
            BattleSimulation battle = CreateLayerOneBattleWithStatuses();
            BattleEnemyState enemy = battle.DebugSpawnEnemy(P0PrototypeCatalog.ColdLightShadowId, 5f);

            bool applied = battle.DebugApplyStatusToEnemy(enemy, StatusTagIds.Slow);
            battle.DebugApplyBedStatus(StatusTagIds.SleepStable);

            Assert.IsTrue(applied);
            Assert.IsTrue(enemy.Statuses.Has(StatusTagIds.Slow));
            Assert.IsTrue(battle.BedStatuses.Has(StatusTagIds.SleepStable));
        }

        [Test]
        public void DebugCoreValueSmokeMethods_AdjustSleepHungerAndPoop()
        {
            BattleSimulation battle = CreateLayerOneBattleWithStatuses();

            battle.DebugDamageOwnerSleep(35f);
            battle.DebugSpendHunger(35f);
            battle.DebugForcePoopCountdown();

            Assert.AreEqual(65f, battle.OwnerSleep.Current, 0.001f);
            Assert.AreEqual(65f, battle.TeamHunger.Current, 0.001f);
            Assert.IsTrue(battle.TeamPoop.IsCountdownActive);
        }

        [Test]
        public void DebugSetBossTimers_PrimesBossWarningText()
        {
            BattleEnemyState boss = new BattleEnemyState(
                1,
                GetEnemy(P0PrototypeCatalog.CallTyrantId),
                10f);

            boss.DebugSetBossTimers(
                EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                EnemyWarningFormatter.BossThrowWarningThresholdSeconds);

            string warning = EnemyWarningFormatter.Format(boss);

            StringAssert.Contains("首领召唤", warning);
            StringAssert.Contains("首领投掷", warning);
        }

        private static BattleSimulation CreateLayerOneBattle()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateLayerOneWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default);

            return new BattleSimulation(config, new RunMetrics());
        }

        private static BattleSimulation CreateLayerOneBattleWithStatuses()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateLayerOneWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default,
                statusTags: P0PrototypeCatalog.CreateStatusTags());

            return new BattleSimulation(config, new RunMetrics());
        }

        private static BattleSimulation CreateBossBattle()
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateCallTyrantBossWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default);

            return new BattleSimulation(config, new RunMetrics());
        }

        private static TheCat.Data.Definitions.EnemyDefinition GetEnemy(string enemyId)
        {
            var enemies = P0PrototypeCatalog.CreateCoreEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Id == enemyId)
                {
                    return enemies[i];
                }
            }

            Assert.Fail("Missing enemy: " + enemyId);
            return null;
        }

        private static TheCat.Data.Definitions.SkillDefinition GetSkill(string skillId)
        {
            var skills = P0PrototypeCatalog.CreateStarterSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Id == skillId)
                {
                    return skills[i];
                }
            }

            Assert.Fail("Missing skill: " + skillId);
            return null;
        }
    }
}
