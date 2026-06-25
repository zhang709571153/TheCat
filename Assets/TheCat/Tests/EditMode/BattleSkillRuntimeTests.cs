using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class BattleSkillRuntimeTests
    {
        [Test]
        public void CastSkill_SlowReducesEnemyAdvanceRate()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.Tick(0.1f);
            BattleEnemyState enemy = battle.ActiveEnemies[0];
            float timeBeforeSlow = enemy.TimeToBedSeconds;

            SkillCastResult result = battle.CastSkill(GetSkill("nephthys_quicksand_trap"));
            battle.Tick(1f);

            Assert.AreEqual(1, result.StatusApplications);
            Assert.IsTrue(enemy.Statuses.Has(StatusTagIds.Slow));
            Assert.Greater(enemy.TimeToBedSeconds, timeBeforeSlow - 1f);
        }

        [Test]
        public void CastSkill_MarkIncreasesDamageTaken()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.Tick(0.1f);
            BattleEnemyState enemy = battle.ActiveEnemies[0];

            battle.CastSkill(GetSkill("nephthys_royal_mark"));
            float hpBeforeDamage = enemy.CurrentHp;
            battle.ApplyDamageToNearestEnemy(10f);

            Assert.AreEqual(hpBeforeDamage - 12.5f, enemy.CurrentHp, 0.001f);
        }

        [Test]
        public void CastSkill_TargetOverrideAppliesDamageToSelectedEnemy()
        {
            BattleSimulation battle = CreateTwoEnemyBattle();
            battle.Tick(0.1f);
            BattleEnemyState first = battle.ActiveEnemies[0];
            BattleEnemyState second = battle.ActiveEnemies[1];
            CatBattleState saiban = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));

            SkillCastResult result = battle.CastSkill(GetSkill("saiban_sword_sweep"), saiban, second);

            Assert.IsTrue(result.HadEnemyTarget);
            Assert.AreEqual(first.Definition.MaxHp, first.CurrentHp, 0.001f);
            Assert.Less(second.CurrentHp, second.Definition.MaxHp);
        }

        [Test]
        public void CastSkill_TargetOverrideAppliesStatusToSelectedEnemy()
        {
            BattleSimulation battle = CreateTwoEnemyBattle();
            battle.Tick(0.1f);
            BattleEnemyState first = battle.ActiveEnemies[0];
            BattleEnemyState second = battle.ActiveEnemies[1];

            SkillCastResult result = battle.CastSkill(GetSkill("nephthys_royal_mark"), null, second);

            Assert.AreEqual(1, result.StatusApplications);
            Assert.IsFalse(first.Statuses.Has(StatusTagIds.Mark));
            Assert.IsTrue(second.Statuses.Has(StatusTagIds.Mark));
        }

        [Test]
        public void CastSkill_InvalidTargetOverrideFallsBackToNearestEnemy()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.Tick(0.1f);
            BattleEnemyState active = battle.ActiveEnemies[0];
            BattleEnemyState inactive = new BattleEnemyState(99, GetEnemy(P0PrototypeCatalog.BlackMudNightmareId), 4f);

            SkillCastResult result = battle.CastSkill(GetSkill("nephthys_royal_mark"), null, inactive);

            Assert.AreEqual(1, result.StatusApplications);
            Assert.IsTrue(active.Statuses.Has(StatusTagIds.Mark));
        }

        [Test]
        public void NephthysPassive_IncreasesHerDamageAgainstControlledOrMarkedTargets()
        {
            BattleSimulation slowBattle = CreateLayerOneBattle();
            slowBattle.Tick(0.1f);
            BattleEnemyState slowedEnemy = slowBattle.ActiveEnemies[0];
            CatBattleState nephthys = new CatBattleState(GetCat(P0PrototypeCatalog.NephthysId));

            slowBattle.CastSkill(GetSkill("nephthys_quicksand_trap"), nephthys);
            float hpBeforeNephthysDamage = slowedEnemy.CurrentHp;
            slowBattle.ApplyDamageToNearestEnemy(8f, nephthys);

            Assert.AreEqual(
                hpBeforeNephthysDamage - 8f * BattleSimulation.NephthysControlledTargetDamageMultiplier,
                slowedEnemy.CurrentHp,
                0.001f);

            BattleSimulation markBattle = CreateLayerOneBattle();
            markBattle.Tick(0.1f);
            BattleEnemyState markedEnemy = markBattle.ActiveEnemies[0];
            CatBattleState saiban = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));

            markBattle.CastSkill(GetSkill("nephthys_royal_mark"), nephthys);
            float hpBeforeSaibanDamage = markedEnemy.CurrentHp;
            markBattle.ApplyDamageToNearestEnemy(8f, saiban);

            Assert.AreEqual(hpBeforeSaibanDamage - 8f * markedEnemy.DamageTakenMultiplier, markedEnemy.CurrentHp, 0.001f);
        }

        [Test]
        public void CastSkill_KnockbackPushesEnemyAwayFromBed()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.Tick(0.1f);
            BattleEnemyState enemy = battle.ActiveEnemies[0];
            float timeBeforeKnockback = enemy.TimeToBedSeconds;

            SkillCastResult result = battle.CastSkill(GetSkill("saiban_sword_sweep"));

            Assert.AreEqual(1, result.KnockbacksApplied);
            Assert.Greater(enemy.TimeToBedSeconds, timeBeforeKnockback);
            Assert.IsTrue(enemy.Statuses.Has(StatusTagIds.Knockback));
        }

        [Test]
        public void CastSkill_ShieldAbsorbsCatDamage()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            CatBattleState saiban = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));

            SkillCastResult result = battle.CastSkill(GetSkill("saiban_oath_shield"), saiban);
            float hpBeforeDamage = saiban.Vital.CurrentHp;
            float unabsorbedFirstHit = saiban.ApplyDamage(20f);
            float unabsorbedSecondHit = saiban.ApplyDamage(20f);

            Assert.AreEqual(35f, result.ShieldApplied, 0.001f);
            Assert.AreEqual(0f, unabsorbedFirstHit, 0.001f);
            Assert.AreEqual(5f, unabsorbedSecondHit, 0.001f);
            Assert.AreEqual(hpBeforeDamage - 5f, saiban.Vital.CurrentHp, 0.001f);
        }

        [Test]
        public void SaibanShieldPassive_AppliesBedShieldAndAbsorbsBedDamage()
        {
            BattleSimulation battle = CreateFastBedPressureBattle();
            battle.Tick(0.1f);
            CatBattleState saiban = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));

            SkillCastResult result = battle.CastSkill(GetSkill("saiban_oath_shield"), saiban);

            Assert.AreEqual(35f * BattleSimulation.SaibanBedShieldRatio, result.BedShieldApplied, 0.001f);
            Assert.AreEqual(2, result.StatusApplications);
            Assert.IsTrue(battle.BedStatuses.TryGet(StatusTagIds.Shield, out StatusEffectState bedShield));
            Assert.AreEqual(result.BedShieldApplied, bedShield.Magnitude, 0.001f);

            battle.Tick(0.75f);

            Assert.AreEqual(100f, battle.OwnerSleep.Current, 0.001f);
            Assert.IsTrue(battle.BedStatuses.TryGet(StatusTagIds.Shield, out bedShield));
            Assert.AreEqual(result.BedShieldApplied - 10f, bedShield.Magnitude, 0.001f);
        }

        [Test]
        public void CastSkill_SleepStableRestoresOwnerSleep()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            CatBattleState suzune = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId));
            battle.OwnerSleep.ApplyDamage(25f);

            SkillCastResult result = battle.CastSkill(GetSkill("suzune_sleep_bell"), suzune);

            Assert.AreEqual(10f, result.OwnerSleepRestored, 0.001f);
            Assert.AreEqual(85f, battle.OwnerSleep.Current, 0.001f);
            Assert.AreEqual(1, result.StatusApplications);
            Assert.IsTrue(battle.BedStatuses.TryGet(StatusTagIds.SleepStable, out StatusEffectState sleepStable));
            Assert.AreEqual(8f, sleepStable.RemainingSeconds, 0.001f);

            battle.Tick(8.1f);

            Assert.IsFalse(battle.BedStatuses.Has(StatusTagIds.SleepStable));
        }

        [Test]
        public void SuzuneSleepBell_ExtendsActivePoopCountdown()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            CatBattleState suzune = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId));
            battle.TeamPoop.Tick(400f, P0Tuning.Default, isDigesting: false, layer: 1);
            battle.TeamPoop.Tick(25f, P0Tuning.Default, isDigesting: false, layer: 1);
            float countdownBeforeSkill = battle.TeamPoop.CountdownRemainingSeconds;

            SkillCastResult result = battle.CastSkill(GetSkill("suzune_sleep_bell"), suzune);

            Assert.AreEqual(BattleSimulation.SuzuneSleepBellPoopCountdownExtensionSeconds, result.PoopCountdownExtendedSeconds, 0.001f);
            Assert.AreEqual(
                countdownBeforeSkill + BattleSimulation.SuzuneSleepBellPoopCountdownExtensionSeconds,
                battle.TeamPoop.CountdownRemainingSeconds,
                0.001f);
        }

        [Test]
        public void SuzuneSleepBell_DoesNotExtendInactivePoopCountdown()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            CatBattleState suzune = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId));

            SkillCastResult result = battle.CastSkill(GetSkill("suzune_sleep_bell"), suzune);

            Assert.AreEqual(0f, result.PoopCountdownExtendedSeconds);
            Assert.AreEqual(0f, battle.TeamPoop.CountdownRemainingSeconds);
        }

        [Test]
        public void AuthorityBlessings_ImproveMatchingSkillEffects()
        {
            RunBlessingInventory inventory = CreateBlessingInventory(
                P0BlessingCatalog.SaibanBedlineId,
                P0BlessingCatalog.NephthysSandglassId,
                P0BlessingCatalog.SuzuneLullabyId);
            BattleSimulation battle = CreateLayerOneBattle(P0BlessingCatalog.CreateBattleModifiers(inventory));
            battle.Tick(0.1f);
            BattleEnemyState enemy = battle.ActiveEnemies[0];
            CatBattleState saiban = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));
            CatBattleState suzune = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId));
            suzune.ApplyDamage(40f);
            battle.OwnerSleep.ApplyDamage(40f);

            SkillCastResult shield = battle.CastSkill(GetSkill("saiban_oath_shield"), saiban);
            battle.CastSkill(GetSkill("nephthys_quicksand_trap"));
            SkillCastResult lullaby = battle.CastSkill(GetSkill("suzune_sleep_bell"), suzune);

            Assert.AreEqual(43.75f, shield.ShieldApplied, 0.001f);
            Assert.IsTrue(enemy.Statuses.TryGet(StatusTagIds.Slow, out StatusEffectState slow));
            Assert.AreEqual(GetStatus(StatusTagIds.Slow).BaseDurationSeconds * 1.25f, slow.RemainingSeconds, 0.001f);
            Assert.AreEqual(12f, lullaby.OwnerSleepRestored, 0.001f);
            Assert.AreEqual(24f, lullaby.CatHealingApplied, 0.001f);
        }

        [Test]
        public void CatUpgradeRuntimeSkill_CastsThroughBattleSimulation()
        {
            BattleSimulation battle = CreateLayerOneBattle();
            battle.Tick(0.1f);
            BattleEnemyState enemy = battle.ActiveEnemies[0];
            float timeBeforeSkill = enemy.TimeToBedSeconds;
            SkillDefinition skill = P0CatUpgradeRuntimeCatalog.CreateUpgradeSkillDefinitions()
                .Single(candidate => candidate.Id == P0CatUpgradeRuntimeCatalog.SaibanBedlineInterceptSkillId);
            CatBattleState saiban = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));

            SkillCastResult result = battle.CastSkill(skill, saiban, enemy);

            Assert.AreEqual(P0CatUpgradeRuntimeCatalog.SaibanBedlineInterceptSkillId, result.SkillId);
            Assert.IsTrue(result.HadEnemyTarget);
            Assert.Greater(result.DamageApplied, 0f);
            Assert.AreEqual(1, result.KnockbacksApplied);
            Assert.Greater(enemy.TimeToBedSeconds, timeBeforeSkill);
        }

        private static BattleSimulation CreateLayerOneBattle(BattleModifierSet modifiers = null)
        {
            BattleSimulationConfig config = new BattleSimulationConfig(
                P0PrototypeCatalog.CreateLayerOneWave(),
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default,
                statusTags: P0PrototypeCatalog.CreateStatusTags(),
                modifiers: modifiers);

            return new BattleSimulation(config, new RunMetrics());
        }

        private static BattleSimulation CreateFastBedPressureBattle()
        {
            const string enemyId = "fast_bed_pressure";
            EnemyDefinition enemy = new EnemyDefinition(
                enemyId,
                "Fast Bed Pressure",
                EnemyBehaviorType.MoveToBed,
                maxHp: 30f,
                moveSpeed: 10f,
                playerDamage: 0f,
                bedDamage: 10f,
                canBeKnockedBack: true,
                slowResponseMultiplier: 1f);
            WaveDefinition wave = new WaveDefinition(
                layer: 1,
                id: "fast_bed_pressure_wave",
                targetDurationSeconds: 5f,
                spawnGroups: new[] { new SpawnGroupDefinition(enemyId, 1, 0f, 0f, "center") });
            BattleSimulationConfig config = new BattleSimulationConfig(
                wave,
                new[] { enemy },
                P0Tuning.Default,
                statusTags: P0PrototypeCatalog.CreateStatusTags());

            return new BattleSimulation(config, new RunMetrics());
        }

        private static BattleSimulation CreateTwoEnemyBattle()
        {
            WaveDefinition wave = new WaveDefinition(
                layer: 1,
                id: "two_enemy_skill_target_test",
                targetDurationSeconds: 20f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(P0PrototypeCatalog.BlackMudNightmareId, 1, 0f, 0f, "north"),
                    new SpawnGroupDefinition(P0PrototypeCatalog.ColdLightShadowId, 1, 0f, 0f, "east")
                });
            BattleSimulationConfig config = new BattleSimulationConfig(
                wave,
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0Tuning.Default,
                statusTags: P0PrototypeCatalog.CreateStatusTags());

            return new BattleSimulation(config, new RunMetrics());
        }

        private static SkillDefinition GetSkill(string skillId)
        {
            IReadOnlyList<SkillDefinition> skills = P0PrototypeCatalog.CreateStarterSkills();
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

        private static CatDefinition GetCat(string catId)
        {
            IReadOnlyList<CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i];
                }
            }

            Assert.Fail("Missing cat: " + catId);
            return null;
        }

        private static EnemyDefinition GetEnemy(string enemyId)
        {
            IReadOnlyList<EnemyDefinition> enemies = P0PrototypeCatalog.CreateCoreEnemies();
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

        private static StatusTagDefinition GetStatus(string statusTagId)
        {
            IReadOnlyList<StatusTagDefinition> statuses = P0PrototypeCatalog.CreateStatusTags();
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i].Id == statusTagId)
                {
                    return statuses[i];
                }
            }

            Assert.Fail("Missing status: " + statusTagId);
            return default(StatusTagDefinition);
        }

        private static RunBlessingInventory CreateBlessingInventory(params string[] blessingIds)
        {
            RunBlessingInventory inventory = new RunBlessingInventory();
            IReadOnlyList<AuthorityBlessingDefinition> blessings = P0BlessingCatalog.CreateAuthorityBlessings();
            for (int i = 0; i < blessingIds.Length; i++)
            {
                for (int j = 0; j < blessings.Count; j++)
                {
                    if (blessings[j].Id == blessingIds[i])
                    {
                        inventory.Add(blessings[j]);
                        break;
                    }
                }
            }

            return inventory;
        }
    }
}
