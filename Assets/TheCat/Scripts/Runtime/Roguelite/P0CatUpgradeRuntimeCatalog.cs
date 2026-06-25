using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Roguelite
{
    public static class P0CatUpgradeRuntimeCatalog
    {
        public const string SaibanOathShieldFocusSkillId = "saiban_oath_shield_focus";
        public const string SaibanSwordSweepArcSkillId = "saiban_sword_sweep_arc";
        public const string SaibanBedlineInterceptSkillId = "saiban_bedline_intercept";
        public const string SaibanOathCounterSkillId = "saiban_oath_counter";
        public const string SaibanSunCrownSkillId = "saiban_sun_crown";
        public const string SaibanOathDomainSkillId = "saiban_oath_domain";

        public const string NephthysMoonSandFocusSkillId = "nephthys_moon_sand_focus";
        public const string NephthysQuicksandPrisonSkillId = "nephthys_quicksand_prison";
        public const string NephthysRoyalCommandSkillId = "nephthys_royal_command";
        public const string NephthysSandSentinelSkillId = "nephthys_sand_sentinel";
        public const string NephthysSandThroneSkillId = "nephthys_sand_throne";
        public const string NephthysEclipseObeliskSkillId = "nephthys_eclipse_obelisk";

        public const string SuzuneSleepBellFocusSkillId = "suzune_sleep_bell_focus";
        public const string SuzuneHealingBellBloomSkillId = "suzune_healing_bell_bloom";
        public const string SuzuneMoonToriiGuardSkillId = "suzune_moon_torii_guard";
        public const string SuzuneDreamChimeSkillId = "suzune_dream_chime";
        public const string SuzuneMoonSleepDomainSkillId = "suzune_moon_sleep_domain";
        public const string SuzuneKaguraCleanseSkillId = "suzune_kagura_cleanse";

        public static CatDefinition ApplyRunUpgrades(CatDefinition cat, RunCatUpgradeState upgrades)
        {
            if (cat == null)
            {
                throw new ArgumentNullException(nameof(cat));
            }

            if (upgrades == null)
            {
                return cat;
            }

            float maxHp = cat.MaxHp;
            float physicalDefense = cat.PhysicalDefense;
            float magicalDefense = cat.MagicalDefense;
            float moveSpeedMultiplier = cat.MoveSpeedMultiplier;
            ApplyPassiveBonuses(cat.Id, upgrades, ref maxHp, ref physicalDefense, ref magicalDefense, ref moveSpeedMultiplier);

            List<string> skillIds = new List<string>(cat.SkillIds);
            AddUnlockedSkillIds(cat.Id, upgrades, skillIds);

            return new CatDefinition(
                cat.Id,
                cat.DisplayName,
                cat.Role,
                cat.AuthorityId,
                cat.AttributeId,
                maxHp,
                physicalDefense,
                magicalDefense,
                moveSpeedMultiplier,
                skillIds,
                cat.CombatSprite);
        }

        public static IReadOnlyList<SkillDefinition> CreateSelectedSkillDefinitions(RunCatUpgradeState upgrades)
        {
            if (upgrades == null)
            {
                return Array.Empty<SkillDefinition>();
            }

            SkillDefinition[] definitions = CreateUpgradeSkillDefinitions();
            List<SkillDefinition> selected = new List<SkillDefinition>();
            for (int i = 0; i < definitions.Length; i++)
            {
                string upgradeId = GetUnlockingUpgradeId(definitions[i].Id);
                if (upgrades.HasSelectedUpgrade(upgradeId))
                {
                    selected.Add(definitions[i]);
                }
            }

            return selected.AsReadOnly();
        }

        public static SkillDefinition[] CreateUpgradeSkillDefinitions()
        {
            return new[]
            {
                new SkillDefinition(
                    SaibanOathShieldFocusSkillId,
                    P0PrototypeCatalog.SaibanId,
                    SkillSlot.SmallSkill3,
                    cooldownSeconds: 7f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.Self,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.Shield, 50f, StatusTagIds.Shield) }),
                new SkillDefinition(
                    SaibanSwordSweepArcSkillId,
                    P0PrototypeCatalog.SaibanId,
                    SkillSlot.SmallSkill3,
                    cooldownSeconds: 7f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.Directional,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Damage, 32f),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 4f, StatusTagIds.Knockback)
                    }),
                new SkillDefinition(
                    SaibanBedlineInterceptSkillId,
                    P0PrototypeCatalog.SaibanId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 8f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Damage, 18f),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 6f, StatusTagIds.Knockback)
                    }),
                new SkillDefinition(
                    SaibanOathCounterSkillId,
                    P0PrototypeCatalog.SaibanId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 9f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Shield, 25f, StatusTagIds.Shield),
                        new SkillEffectDefinition(SkillEffectType.Damage, 22f)
                    }),
                new SkillDefinition(
                    SaibanSunCrownSkillId,
                    P0PrototypeCatalog.SaibanId,
                    SkillSlot.Ultimate2,
                    cooldownSeconds: 42f,
                    hungerCost: 9f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Damage, 78f),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 6f, StatusTagIds.Knockback),
                        new SkillEffectDefinition(SkillEffectType.Shield, 30f, StatusTagIds.Shield)
                    }),
                new SkillDefinition(
                    SaibanOathDomainSkillId,
                    P0PrototypeCatalog.SaibanId,
                    SkillSlot.Ultimate2,
                    cooldownSeconds: 44f,
                    hungerCost: 9f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Shield, 65f, StatusTagIds.Shield),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 4f, StatusTagIds.Knockback)
                    }),
                new SkillDefinition(
                    NephthysMoonSandFocusSkillId,
                    P0PrototypeCatalog.NephthysId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 10f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.SpawnSummon, 1f),
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.45f, StatusTagIds.Slow)
                    }),
                new SkillDefinition(
                    NephthysQuicksandPrisonSkillId,
                    P0PrototypeCatalog.NephthysId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 11f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.65f, StatusTagIds.Slow) }),
                new SkillDefinition(
                    NephthysRoyalCommandSkillId,
                    P0PrototypeCatalog.NephthysId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 9f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.35f, StatusTagIds.Mark),
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.25f, StatusTagIds.Slow)
                    }),
                new SkillDefinition(
                    NephthysSandSentinelSkillId,
                    P0PrototypeCatalog.NephthysId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 12f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.SpawnSummon, 1f),
                        new SkillEffectDefinition(SkillEffectType.Damage, 16f),
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.25f, StatusTagIds.Slow)
                    }),
                new SkillDefinition(
                    NephthysSandThroneSkillId,
                    P0PrototypeCatalog.NephthysId,
                    SkillSlot.Ultimate1,
                    cooldownSeconds: 42f,
                    hungerCost: 9f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.75f, StatusTagIds.Slow),
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.35f, StatusTagIds.Mark)
                    }),
                new SkillDefinition(
                    NephthysEclipseObeliskSkillId,
                    P0PrototypeCatalog.NephthysId,
                    SkillSlot.Ultimate2,
                    cooldownSeconds: 45f,
                    hungerCost: 9f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.SpawnSummon, 2f),
                        new SkillEffectDefinition(SkillEffectType.Damage, 30f),
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.45f, StatusTagIds.Slow)
                    }),
                new SkillDefinition(
                    SuzuneSleepBellFocusSkillId,
                    P0PrototypeCatalog.SuzuneId,
                    SkillSlot.SmallSkill3,
                    cooldownSeconds: 9f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.BedZone,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.RestoreOwnerSleep, 18f, StatusTagIds.SleepStable),
                        new SkillEffectDefinition(SkillEffectType.HealCat, 25f)
                    }),
                new SkillDefinition(
                    SuzuneHealingBellBloomSkillId,
                    P0PrototypeCatalog.SuzuneId,
                    SkillSlot.SmallSkill3,
                    cooldownSeconds: 9f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.Self,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.HealCat, 55f) }),
                new SkillDefinition(
                    SuzuneMoonToriiGuardSkillId,
                    P0PrototypeCatalog.SuzuneId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 12f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.BedZone,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.RestoreOwnerSleep, 22f, StatusTagIds.SleepStable),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 5f, StatusTagIds.Knockback)
                    }),
                new SkillDefinition(
                    SuzuneDreamChimeSkillId,
                    P0PrototypeCatalog.SuzuneId,
                    SkillSlot.SmallSkill4,
                    cooldownSeconds: 8f,
                    hungerCost: 4f,
                    targetingMode: SkillTargetingMode.Self,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.HealCat, 20f),
                        new SkillEffectDefinition(SkillEffectType.Shield, 18f, StatusTagIds.Shield)
                    }),
                new SkillDefinition(
                    SuzuneMoonSleepDomainSkillId,
                    P0PrototypeCatalog.SuzuneId,
                    SkillSlot.Ultimate2,
                    cooldownSeconds: 45f,
                    hungerCost: 9f,
                    targetingMode: SkillTargetingMode.BedZone,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.RestoreOwnerSleep, 45f, StatusTagIds.SleepStable),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 5f, StatusTagIds.Knockback)
                    }),
                new SkillDefinition(
                    SuzuneKaguraCleanseSkillId,
                    P0PrototypeCatalog.SuzuneId,
                    SkillSlot.Ultimate2,
                    cooldownSeconds: 44f,
                    hungerCost: 9f,
                    targetingMode: SkillTargetingMode.Self,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.RestoreOwnerSleep, 25f, StatusTagIds.SleepStable),
                        new SkillEffectDefinition(SkillEffectType.HealCat, 45f),
                        new SkillEffectDefinition(SkillEffectType.Shield, 25f, StatusTagIds.Shield)
                    })
            };
        }

        public static string GetUnlockingUpgradeId(string skillId)
        {
            switch (skillId)
            {
                case SaibanOathShieldFocusSkillId:
                    return P0CatUpgradeCatalog.SaibanSmallOathShieldFocusId;
                case SaibanSwordSweepArcSkillId:
                    return P0CatUpgradeCatalog.SaibanSmallSwordSweepArcId;
                case SaibanBedlineInterceptSkillId:
                    return P0CatUpgradeCatalog.SaibanSmallBedlineInterceptId;
                case SaibanOathCounterSkillId:
                    return P0CatUpgradeCatalog.SaibanSmallOathCounterId;
                case SaibanSunCrownSkillId:
                    return P0CatUpgradeCatalog.SaibanUltimateSunCrownId;
                case SaibanOathDomainSkillId:
                    return P0CatUpgradeCatalog.SaibanUltimateOathDomainId;
                case NephthysMoonSandFocusSkillId:
                    return P0CatUpgradeCatalog.NephthysSmallMoonSandId;
                case NephthysQuicksandPrisonSkillId:
                    return P0CatUpgradeCatalog.NephthysSmallQuicksandId;
                case NephthysRoyalCommandSkillId:
                    return P0CatUpgradeCatalog.NephthysSmallRoyalMarkId;
                case NephthysSandSentinelSkillId:
                    return P0CatUpgradeCatalog.NephthysSmallSandSentinelId;
                case NephthysSandThroneSkillId:
                    return P0CatUpgradeCatalog.NephthysUltimateSandThroneId;
                case NephthysEclipseObeliskSkillId:
                    return P0CatUpgradeCatalog.NephthysUltimateEclipseObeliskId;
                case SuzuneSleepBellFocusSkillId:
                    return P0CatUpgradeCatalog.SuzuneSmallSleepBellId;
                case SuzuneHealingBellBloomSkillId:
                    return P0CatUpgradeCatalog.SuzuneSmallHealingBellId;
                case SuzuneMoonToriiGuardSkillId:
                    return P0CatUpgradeCatalog.SuzuneSmallMoonToriiId;
                case SuzuneDreamChimeSkillId:
                    return P0CatUpgradeCatalog.SuzuneSmallDreamChimeId;
                case SuzuneMoonSleepDomainSkillId:
                    return P0CatUpgradeCatalog.SuzuneUltimateMoonSleepId;
                case SuzuneKaguraCleanseSkillId:
                    return P0CatUpgradeCatalog.SuzuneUltimateKaguraCleanseId;
                default:
                    return string.Empty;
            }
        }

        private static void ApplyPassiveBonuses(
            string catId,
            RunCatUpgradeState upgrades,
            ref float maxHp,
            ref float physicalDefense,
            ref float magicalDefense,
            ref float moveSpeedMultiplier)
        {
            if (catId == P0PrototypeCatalog.SaibanId)
            {
                if (upgrades.HasSelectedUpgrade(P0CatUpgradeCatalog.SaibanPassiveOathReflowId))
                {
                    maxHp += 20f;
                    magicalDefense += 3f;
                }

                if (upgrades.HasSelectedUpgrade(P0CatUpgradeCatalog.SaibanPassiveBedlineGuardId))
                {
                    maxHp += 15f;
                    physicalDefense += 6f;
                }
            }
            else if (catId == P0PrototypeCatalog.NephthysId)
            {
                if (upgrades.HasSelectedUpgrade(P0CatUpgradeCatalog.NephthysPassiveSandEyeId))
                {
                    maxHp += 15f;
                    magicalDefense += 3f;
                }

                if (upgrades.HasSelectedUpgrade(P0CatUpgradeCatalog.NephthysPassiveRoyalOverseerId))
                {
                    physicalDefense += 2f;
                    moveSpeedMultiplier += 0.08f;
                }
            }
            else if (catId == P0PrototypeCatalog.SuzuneId)
            {
                if (upgrades.HasSelectedUpgrade(P0CatUpgradeCatalog.SuzunePassiveLingeringBellId))
                {
                    maxHp += 18f;
                    physicalDefense += 3f;
                }

                if (upgrades.HasSelectedUpgrade(P0CatUpgradeCatalog.SuzunePassiveSleepPrayerId))
                {
                    maxHp += 10f;
                    magicalDefense += 4f;
                }
            }
        }

        private static void AddUnlockedSkillIds(string catId, RunCatUpgradeState upgrades, List<string> skillIds)
        {
            if (catId == P0PrototypeCatalog.SaibanId)
            {
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SaibanSmallOathShieldFocusId, SaibanOathShieldFocusSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SaibanSmallSwordSweepArcId, SaibanSwordSweepArcSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SaibanSmallBedlineInterceptId, SaibanBedlineInterceptSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SaibanSmallOathCounterId, SaibanOathCounterSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SaibanUltimateSunCrownId, SaibanSunCrownSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SaibanUltimateOathDomainId, SaibanOathDomainSkillId);
            }
            else if (catId == P0PrototypeCatalog.NephthysId)
            {
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.NephthysSmallMoonSandId, NephthysMoonSandFocusSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.NephthysSmallQuicksandId, NephthysQuicksandPrisonSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.NephthysSmallRoyalMarkId, NephthysRoyalCommandSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.NephthysSmallSandSentinelId, NephthysSandSentinelSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.NephthysUltimateSandThroneId, NephthysSandThroneSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.NephthysUltimateEclipseObeliskId, NephthysEclipseObeliskSkillId);
            }
            else if (catId == P0PrototypeCatalog.SuzuneId)
            {
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SuzuneSmallSleepBellId, SuzuneSleepBellFocusSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SuzuneSmallHealingBellId, SuzuneHealingBellBloomSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SuzuneSmallMoonToriiId, SuzuneMoonToriiGuardSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SuzuneSmallDreamChimeId, SuzuneDreamChimeSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SuzuneUltimateMoonSleepId, SuzuneMoonSleepDomainSkillId);
                AddSkillIfSelected(upgrades, skillIds, P0CatUpgradeCatalog.SuzuneUltimateKaguraCleanseId, SuzuneKaguraCleanseSkillId);
            }
        }

        private static void AddSkillIfSelected(
            RunCatUpgradeState upgrades,
            List<string> skillIds,
            string upgradeId,
            string skillId)
        {
            if (!upgrades.HasSelectedUpgrade(upgradeId) || skillIds.Contains(skillId))
            {
                return;
            }

            skillIds.Add(skillId);
        }
    }
}
