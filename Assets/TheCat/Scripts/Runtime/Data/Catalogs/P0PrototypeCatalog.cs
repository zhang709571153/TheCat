using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Definitions;

namespace TheCat.Data.Catalogs
{
    public static class P0PrototypeCatalog
    {
        public const string SaibanId = "saiban";
        public const string NephthysId = "nephthys";
        public const string SuzuneId = "suzune";

        public const string BlackMudNightmareId = "black_mud_nightmare";
        public const string DreamRailToyTrainId = "dream_rail_toy_train";
        public const string ColdLightShadowId = "cold_light_shadow";
        public const string RedEyeAlarmId = "red_eye_alarm";
        public const string UnreadRedDotFlyerId = "unread_red_dot_flyer";
        public const string FallingDreamTeddyId = "falling_dream_teddy";
        public const string CallTyrantId = "call_tyrant";

        public static IReadOnlyList<CatDefinition> CreateStarterCats()
        {
            return new[]
            {
                new CatDefinition(
                    SaibanId,
                    "塞班",
                    CatRole.Defender,
                    AuthorityIds.Oath,
                    AttributeIds.Sun,
                    maxHp: 220f,
                    physicalDefense: 18f,
                    magicalDefense: 12f,
                    moveSpeedMultiplier: 0.85f,
                    skillIds: new[] { "saiban_oath_shield", "saiban_sword_sweep", "saiban_sun_charge" },
                    combatSprite: P0VisualAssetCatalog.GetStarterCatCombatSprite(SaibanId)),
                new CatDefinition(
                    NephthysId,
                    "奈芙蒂斯",
                    CatRole.Controller,
                    AuthorityIds.Dominion,
                    AttributeIds.Earth,
                    maxHp: 110f,
                    physicalDefense: 6f,
                    magicalDefense: 10f,
                    moveSpeedMultiplier: 1f,
                    skillIds: new[] { "nephthys_moon_sand_obelisk", "nephthys_quicksand_trap", "nephthys_royal_mark" },
                    combatSprite: P0VisualAssetCatalog.GetStarterCatCombatSprite(NephthysId)),
                new CatDefinition(
                    SuzuneId,
                    "铃音",
                    CatRole.Healer,
                    AuthorityIds.Rhythm,
                    AttributeIds.Moon,
                    maxHp: 130f,
                    physicalDefense: 9f,
                    magicalDefense: 14f,
                    moveSpeedMultiplier: 0.9f,
                    skillIds: new[] { "suzune_sleep_bell", "suzune_healing_bell", "suzune_moon_torii" },
                    combatSprite: P0VisualAssetCatalog.GetStarterCatCombatSprite(SuzuneId))
            };
        }

        public static IReadOnlyList<SkillDefinition> CreateStarterSkills()
        {
            return new[]
            {
                new SkillDefinition(
                    "saiban_oath_shield",
                    SaibanId,
                    SkillSlot.SmallSkill1,
                    cooldownSeconds: 8f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.Self,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.Shield, 35f, "shield") }),
                new SkillDefinition(
                    "saiban_sword_sweep",
                    SaibanId,
                    SkillSlot.SmallSkill2,
                    cooldownSeconds: 6f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.Directional,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Damage, 24f),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 3f, "knockback")
                    }),
                new SkillDefinition(
                    "saiban_sun_charge",
                    SaibanId,
                    SkillSlot.Ultimate1,
                    cooldownSeconds: 36f,
                    hungerCost: 8f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.Damage, 60f),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 5f, "knockback"),
                        new SkillEffectDefinition(SkillEffectType.Shield, 20f, "shield")
                    }),
                new SkillDefinition(
                    "nephthys_moon_sand_obelisk",
                    NephthysId,
                    SkillSlot.SmallSkill1,
                    cooldownSeconds: 10f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.SpawnSummon, 1f),
                        new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.35f, "slow")
                    }),
                new SkillDefinition(
                    "nephthys_quicksand_trap",
                    NephthysId,
                    SkillSlot.SmallSkill2,
                    cooldownSeconds: 12f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.AreaAtTarget,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.5f, "slow") }),
                new SkillDefinition(
                    "nephthys_royal_mark",
                    NephthysId,
                    SkillSlot.SmallSkill3,
                    cooldownSeconds: 9f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.AutoNearestEnemy,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.ApplyStatus, 0.25f, "mark") }),
                new SkillDefinition(
                    "suzune_sleep_bell",
                    SuzuneId,
                    SkillSlot.SmallSkill1,
                    cooldownSeconds: 8f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.BedZone,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.RestoreOwnerSleep, 10f, "sleep_stable"),
                        new SkillEffectDefinition(SkillEffectType.HealCat, 20f)
                    }),
                new SkillDefinition(
                    "suzune_healing_bell",
                    SuzuneId,
                    SkillSlot.SmallSkill2,
                    cooldownSeconds: 10f,
                    hungerCost: 3f,
                    targetingMode: SkillTargetingMode.Self,
                    effects: new[] { new SkillEffectDefinition(SkillEffectType.HealCat, 35f) }),
                new SkillDefinition(
                    "suzune_moon_torii",
                    SuzuneId,
                    SkillSlot.Ultimate1,
                    cooldownSeconds: 45f,
                    hungerCost: 8f,
                    targetingMode: SkillTargetingMode.BedZone,
                    effects: new[]
                    {
                        new SkillEffectDefinition(SkillEffectType.RestoreOwnerSleep, 30f, "sleep_stable"),
                        new SkillEffectDefinition(SkillEffectType.Knockback, 4f, "knockback")
                    })
            };
        }

        public static IReadOnlyList<StatusTagDefinition> CreateStatusTags()
        {
            return new[]
            {
                new StatusTagDefinition(
                    StatusTagIds.SleepStable,
                    "安眠",
                    StatusTargetType.BedZone,
                    baseDurationSeconds: 8f,
                    magnitude: 1f,
                    stackPolicy: StatusStackPolicy.RefreshDuration,
                    visualToken: "soft_blue_note"),
                new StatusTagDefinition(
                    StatusTagIds.Slow,
                    "缓速",
                    StatusTargetType.Enemy,
                    baseDurationSeconds: 4f,
                    magnitude: 0.35f,
                    stackPolicy: StatusStackPolicy.HighestMagnitude,
                    visualToken: "moon_sand"),
                new StatusTagDefinition(
                    StatusTagIds.Knockback,
                    "击退",
                    StatusTargetType.Enemy,
                    baseDurationSeconds: 0.25f,
                    magnitude: 1f,
                    stackPolicy: StatusStackPolicy.RefreshDuration,
                    visualToken: "silver_impact"),
                new StatusTagDefinition(
                    StatusTagIds.Mark,
                    "标记",
                    StatusTargetType.Enemy,
                    baseDurationSeconds: 5f,
                    magnitude: 0.25f,
                    stackPolicy: StatusStackPolicy.RefreshDuration,
                    visualToken: "royal_eye"),
                new StatusTagDefinition(
                    StatusTagIds.Shield,
                    "护盾",
                    StatusTargetType.Cat,
                    baseDurationSeconds: 6f,
                    magnitude: 35f,
                    stackPolicy: StatusStackPolicy.HighestMagnitude,
                    visualToken: "oath_edge")
            };
        }

        public static IReadOnlyList<EnemyDefinition> CreateCoreEnemies()
        {
            return new[]
            {
                new EnemyDefinition(
                    BlackMudNightmareId,
                    "黑泥梦魇",
                    EnemyBehaviorType.MoveToBed,
                    maxHp: 30f,
                    moveSpeed: 1.1f,
                    playerDamage: 12f,
                    bedDamage: 3f,
                    canBeKnockedBack: true,
                    slowResponseMultiplier: 1f),
                new EnemyDefinition(
                    DreamRailToyTrainId,
                    "梦轨小火车",
                    EnemyBehaviorType.Charger,
                    maxHp: 45f,
                    moveSpeed: 2.25f,
                    playerDamage: 18f,
                    bedDamage: 8f,
                    canBeKnockedBack: true,
                    slowResponseMultiplier: 0.7f),
                new EnemyDefinition(
                    ColdLightShadowId,
                    "冷光灯影",
                    EnemyBehaviorType.RangedHarasser,
                    maxHp: 70f,
                    moveSpeed: 0.75f,
                    playerDamage: 25f,
                    bedDamage: 5f,
                    canBeKnockedBack: true,
                    slowResponseMultiplier: 1f),
                new EnemyDefinition(
                    RedEyeAlarmId,
                    "红眼闹铃",
                    EnemyBehaviorType.RangedHarasser,
                    maxHp: 120f,
                    moveSpeed: 0.55f,
                    playerDamage: 28f,
                    bedDamage: 6f,
                    canBeKnockedBack: true,
                    slowResponseMultiplier: 0.8f),
                new EnemyDefinition(
                    UnreadRedDotFlyerId,
                    "未读红点小飞虫",
                    EnemyBehaviorType.FlyingAttachment,
                    maxHp: 18f,
                    moveSpeed: 1.7f,
                    playerDamage: 8f,
                    bedDamage: 1.5f,
                    canBeKnockedBack: true,
                    slowResponseMultiplier: 0.9f),
                new EnemyDefinition(
                    FallingDreamTeddyId,
                    "坠梦玩具熊",
                    EnemyBehaviorType.EliteJumpSlam,
                    maxHp: 260f,
                    moveSpeed: 0.45f,
                    playerDamage: 36f,
                    bedDamage: 9f,
                    canBeKnockedBack: false,
                    slowResponseMultiplier: 0.5f),
                new EnemyDefinition(
                    CallTyrantId,
                    "来电暴君",
                    EnemyBehaviorType.BossCallTyrant,
                    maxHp: 800f,
                    moveSpeed: 0.35f,
                    playerDamage: 40f,
                    bedDamage: 10f,
                    canBeKnockedBack: false,
                    slowResponseMultiplier: 0.35f)
            };
        }

        public static WaveDefinition CreateLayerOneWave()
        {
            return new WaveDefinition(
                layer: 1,
                id: "layer_01_defense",
                targetDurationSeconds: 60f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(BlackMudNightmareId, 6, 0f, 5f, "north"),
                    new SpawnGroupDefinition(BlackMudNightmareId, 4, 15f, 6f, "east")
                });
        }

        public static WaveDefinition CreateLayerSixDefenseWave()
        {
            return new WaveDefinition(
                layer: 6,
                id: "layer_06_defense",
                targetDurationSeconds: 75f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(BlackMudNightmareId, 8, 0f, 4f, "north"),
                    new SpawnGroupDefinition(ColdLightShadowId, 3, 12f, 12f, "east"),
                    new SpawnGroupDefinition(DreamRailToyTrainId, 3, 18f, 10f, "center"),
                    new SpawnGroupDefinition(BlackMudNightmareId, 5, 24f, 5f, "center")
                });
        }

        public static WaveDefinition CreateColdLightEliteWave(int layer = 3, string id = "elite_cold_light_shadow")
        {
            return new WaveDefinition(
                layer,
                id,
                targetDurationSeconds: 70f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(ColdLightShadowId, 2, 0f, 10f, "east"),
                    new SpawnGroupDefinition(BlackMudNightmareId, 4, 6f, 6f, "north")
                });
        }

        public static WaveDefinition CreateRedEyeAlarmEliteWave()
        {
            return new WaveDefinition(
                layer: 9,
                id: "elite_red_eye_alarm",
                targetDurationSeconds: 85f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(RedEyeAlarmId, 1, 0f, 0f, "center"),
                    new SpawnGroupDefinition(UnreadRedDotFlyerId, 6, 4f, 3f, "east"),
                    new SpawnGroupDefinition(BlackMudNightmareId, 5, 16f, 5f, "north"),
                    new SpawnGroupDefinition(UnreadRedDotFlyerId, 5, 28f, 3f, "north")
                });
        }

        public static WaveDefinition CreateFallingDreamTeddyEliteWave()
        {
            return new WaveDefinition(
                layer: 9,
                id: "elite_falling_dream_teddy",
                targetDurationSeconds: 95f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(FallingDreamTeddyId, 1, 0f, 0f, "center"),
                    new SpawnGroupDefinition(DreamRailToyTrainId, 3, 6f, 11f, "north"),
                    new SpawnGroupDefinition(UnreadRedDotFlyerId, 6, 18f, 3f, "east"),
                    new SpawnGroupDefinition(BlackMudNightmareId, 4, 30f, 5f, "north")
                });
        }

        public static WaveDefinition CreateCallTyrantBossWave()
        {
            return new WaveDefinition(
                layer: 10,
                id: "boss_call_tyrant",
                targetDurationSeconds: 120f,
                spawnGroups: new[]
                {
                    new SpawnGroupDefinition(CallTyrantId, 1, 0f, 0f, "center"),
                    new SpawnGroupDefinition(BlackMudNightmareId, 4, 10f, 9f, "north"),
                    new SpawnGroupDefinition(ColdLightShadowId, 2, 20f, 18f, "east")
                });
        }

        public static WaveDefinition CreateWaveForContentId(string contentId)
        {
            switch (contentId)
            {
                case "layer_06_defense":
                    return CreateLayerSixDefenseWave();
                case "elite_cold_light_shadow":
                    return CreateColdLightEliteWave(3, contentId);
                case "elite_red_eye_alarm":
                    return CreateRedEyeAlarmEliteWave();
                case "elite_falling_dream_teddy":
                    return CreateFallingDreamTeddyEliteWave();
                case "boss_call_tyrant":
                    return CreateCallTyrantBossWave();
                case "layer_01_defense":
                default:
                    return CreateLayerOneWave();
            }
        }
    }
}
