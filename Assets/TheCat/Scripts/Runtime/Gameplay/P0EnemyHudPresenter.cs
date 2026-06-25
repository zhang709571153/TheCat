using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public readonly struct P0EnemyHudCard
    {
        public P0EnemyHudCard(
            int instanceId,
            string enemyId,
            string displayName,
            string behaviorToken,
            string threatToken,
            string targetToken,
            string priorityToken,
            float currentHp,
            float maxHp,
            float timeToBedSeconds,
            float bedDamage,
            float catDamage,
            float pressureRange,
            bool isBoss,
            bool isPressureSource,
            bool canBeKnockedBack,
            float movementRateMultiplier,
            float damageTakenMultiplier,
            string warningText,
            string statusSummary,
            float bossSummonRemainingSeconds,
            float bossThrowRemainingSeconds,
            string counterHint)
        {
            InstanceId = instanceId;
            EnemyId = enemyId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            BehaviorToken = behaviorToken ?? string.Empty;
            ThreatToken = threatToken ?? string.Empty;
            TargetToken = targetToken ?? string.Empty;
            PriorityToken = priorityToken ?? string.Empty;
            CurrentHp = currentHp;
            MaxHp = maxHp;
            TimeToBedSeconds = timeToBedSeconds;
            BedDamage = bedDamage;
            CatDamage = catDamage;
            PressureRange = pressureRange;
            IsBoss = isBoss;
            IsPressureSource = isPressureSource;
            CanBeKnockedBack = canBeKnockedBack;
            MovementRateMultiplier = movementRateMultiplier;
            DamageTakenMultiplier = damageTakenMultiplier;
            WarningText = warningText ?? string.Empty;
            StatusSummary = statusSummary ?? string.Empty;
            BossSummonRemainingSeconds = bossSummonRemainingSeconds;
            BossThrowRemainingSeconds = bossThrowRemainingSeconds;
            CounterHint = counterHint ?? string.Empty;
        }

        public int InstanceId { get; }

        public string EnemyId { get; }

        public string DisplayName { get; }

        public string BehaviorToken { get; }

        public string ThreatToken { get; }

        public string TargetToken { get; }

        public string PriorityToken { get; }

        public float CurrentHp { get; }

        public float MaxHp { get; }

        public float HpRatio => MaxHp <= 0f ? 0f : Clamp01(CurrentHp / MaxHp);

        public float TimeToBedSeconds { get; }

        public float BedDamage { get; }

        public float CatDamage { get; }

        public float PressureRange { get; }

        public bool IsBoss { get; }

        public bool IsPressureSource { get; }

        public bool CanBeKnockedBack { get; }

        public float MovementRateMultiplier { get; }

        public float DamageTakenMultiplier { get; }

        public string WarningText { get; }

        public string StatusSummary { get; }

        public float BossSummonRemainingSeconds { get; }

        public float BossThrowRemainingSeconds { get; }

        public string CounterHint { get; }

        public bool HasWarning => !string.IsNullOrWhiteSpace(WarningText);

        public string BuildSummary()
        {
            string pressure = IsPressureSource ? " 压力源" : string.Empty;
            string warning = HasWarning ? " 预警 " + WarningText : " 预警 无";
            string status = string.IsNullOrWhiteSpace(StatusSummary) ? string.Empty : " 状态 " + StatusSummary;
            return PriorityToken
                + pressure
                + " "
                + DisplayName
                + " "
                + ThreatToken
                + " 目标 "
                + TargetToken
                + " 生命 "
                + CurrentHp.ToString("0")
                + "/"
                + MaxHp.ToString("0")
                + " 床 "
                + BedDamage.ToString("0.#")
                + " 猫 "
                + CatDamage.ToString("0.#")
                + warning
                + status
                + " 应对 "
                + CounterHint;
        }

        public string BuildButtonLabel()
        {
            return DisplayName
                + " ["
                + PriorityToken
                + "] "
                + ThreatToken
                + "\n生命 "
                + CurrentHp.ToString("0")
                + "/"
                + MaxHp.ToString("0")
                + "  目标 "
                + TargetToken
                + "  床 "
                + BedDamage.ToString("0.#")
                + "  猫 "
                + CatDamage.ToString("0.#")
                + "\n"
                + (HasWarning ? WarningText : CounterHint);
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }

    public static class P0EnemyHudPresenter
    {
        public static IReadOnlyList<P0EnemyHudCard> BuildCards(
            IReadOnlyList<BattleEnemyState> enemies,
            P0EnemyPressureResult pressure = default(P0EnemyPressureResult))
        {
            List<P0EnemyHudCard> cards = new List<P0EnemyHudCard>();
            if (enemies == null)
            {
                return cards.AsReadOnly();
            }

            int pressureInstanceId = pressure.HasEnemy ? pressure.Enemy.InstanceId : 0;
            for (int i = 0; i < enemies.Count; i++)
            {
                BattleEnemyState enemy = enemies[i];
                if (enemy == null || !enemy.IsAlive)
                {
                    continue;
                }

                cards.Add(BuildCard(enemy, enemy.InstanceId == pressureInstanceId));
            }

            cards.Sort(CompareCards);
            return cards.AsReadOnly();
        }

        public static IReadOnlyList<P0EnemyHudCard> BuildPrototypeCards()
        {
            BattleEnemyState mud = new BattleEnemyState(
                1,
                GetPrototypeEnemy(P0PrototypeCatalog.BlackMudNightmareId),
                EnemyWarningFormatter.BedWarningThresholdSeconds);
            BattleEnemyState coldLight = new BattleEnemyState(
                2,
                GetPrototypeEnemy(P0PrototypeCatalog.ColdLightShadowId),
                EnemyWarningFormatter.RangedPressureWarningThresholdSeconds);
            BattleEnemyState boss = new BattleEnemyState(
                3,
                GetPrototypeEnemy(P0PrototypeCatalog.CallTyrantId),
                8f);
            boss.DebugSetBossTimers(
                EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                EnemyWarningFormatter.BossThrowWarningThresholdSeconds);

            return BuildCards(
                new[] { mud, coldLight, boss },
                new P0EnemyPressureResult(
                    coldLight,
                    2.5f,
                    P0EnemyPressureResolver.GetPressureRange(coldLight.Definition.BehaviorType),
                    P0EnemyPressureResolver.GetDamageMultiplier(coldLight.Definition.BehaviorType)));
        }

        public static P0EnemyHudCard BuildCard(BattleEnemyState enemy, bool isPressureSource = false)
        {
            if (enemy == null)
            {
                return default(P0EnemyHudCard);
            }

            return new P0EnemyHudCard(
                enemy.InstanceId,
                enemy.Definition.Id,
                enemy.Definition.DisplayName,
                GetBehaviorToken(enemy.Definition.BehaviorType),
                GetThreatToken(enemy.Definition),
                GetTargetToken(enemy.Definition.BehaviorType),
                GetPriorityToken(enemy, isPressureSource),
                enemy.CurrentHp,
                enemy.Definition.MaxHp,
                enemy.TimeToBedSeconds,
                enemy.Definition.BedDamage,
                enemy.Definition.PlayerDamage,
                P0EnemyPressureResolver.GetPressureRange(enemy.Definition.BehaviorType),
                enemy.Definition.BehaviorType == EnemyBehaviorType.BossCallTyrant,
                isPressureSource,
                enemy.Definition.CanBeKnockedBack,
                enemy.MovementRateMultiplier,
                enemy.DamageTakenMultiplier,
                EnemyWarningFormatter.Format(enemy),
                StatusDisplayFormatter.FormatCollection(enemy.Statuses),
                enemy.BossSummonRemainingSeconds,
                enemy.BossThrowRemainingSeconds,
                GetCounterHint(enemy.Definition));
        }

        public static bool HasP0EnemyHudCards(IReadOnlyList<P0EnemyHudCard> cards)
        {
            return HasEnemy(cards, P0PrototypeCatalog.BlackMudNightmareId)
                && HasEnemy(cards, P0PrototypeCatalog.ColdLightShadowId)
                && HasEnemy(cards, P0PrototypeCatalog.CallTyrantId)
                && HasThreat(cards, "压床")
                && HasThreat(cards, "远程压制")
                && HasThreat(cards, "首领机制");
        }

        public static string BuildCompactSummary(IReadOnlyList<P0EnemyHudCard> cards)
        {
            if (cards == null || cards.Count == 0)
            {
                return "敌人 HUD：空";
            }

            int bossCount = 0;
            int warningCount = 0;
            int pressureCount = 0;
            int bedPressureCount = 0;
            int rangedPressureCount = 0;

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].IsBoss)
                {
                    bossCount++;
                }

                if (cards[i].HasWarning)
                {
                    warningCount++;
                }

                if (cards[i].IsPressureSource)
                {
                    pressureCount++;
                }

                if (cards[i].ThreatToken == "压床")
                {
                    bedPressureCount++;
                }

                if (cards[i].ThreatToken == "远程压制")
                {
                    rangedPressureCount++;
                }
            }

            return "敌人 HUD："
                + cards.Count
                + " 首领 "
                + bossCount
                + " 预警 "
                + warningCount
                + " 压力源 "
                + pressureCount
                + " 压床 "
                + bedPressureCount
                + " 远程 "
                + rangedPressureCount;
        }

        private static string GetBehaviorToken(EnemyBehaviorType behaviorType)
        {
            switch (behaviorType)
            {
                case EnemyBehaviorType.MoveToBed:
                    return "近程压床";
                case EnemyBehaviorType.Charger:
                    return "冲锋路线";
                case EnemyBehaviorType.RangedHarasser:
                    return "远程骚扰";
                case EnemyBehaviorType.FlyingAttachment:
                    return "飞虫附着";
                case EnemyBehaviorType.EliteJumpSlam:
                    return "跳砸精英";
                case EnemyBehaviorType.BossCallTyrant:
                    return "首领行动";
                default:
                    return "未知";
            }
        }

        private static string GetThreatToken(EnemyDefinition definition)
        {
            if (definition.Id == P0PrototypeCatalog.BlackMudNightmareId)
            {
                return "压床";
            }

            if (definition.Id == P0PrototypeCatalog.ColdLightShadowId)
            {
                return "远程压制";
            }

            if (definition.Id == P0PrototypeCatalog.CallTyrantId)
            {
                return "首领机制";
            }

            switch (definition.BehaviorType)
            {
                case EnemyBehaviorType.MoveToBed:
                    return "压床";
                case EnemyBehaviorType.RangedHarasser:
                    return "远程压制";
                case EnemyBehaviorType.BossCallTyrant:
                    return "首领机制";
                default:
                    return "特殊压力";
            }
        }

        private static string GetTargetToken(EnemyBehaviorType behaviorType)
        {
            switch (behaviorType)
            {
                case EnemyBehaviorType.MoveToBed:
                case EnemyBehaviorType.Charger:
                case EnemyBehaviorType.FlyingAttachment:
                    return "床";
                case EnemyBehaviorType.RangedHarasser:
                case EnemyBehaviorType.EliteJumpSlam:
                    return "猫";
                case EnemyBehaviorType.BossCallTyrant:
                    return "床+猫";
                default:
                    return "未知";
            }
        }

        private static string GetPriorityToken(BattleEnemyState enemy, bool isPressureSource)
        {
            if (enemy.Definition.BehaviorType == EnemyBehaviorType.BossCallTyrant)
            {
                if (enemy.BossThrowRemainingSeconds <= EnemyWarningFormatter.BossThrowWarningThresholdSeconds)
                {
                    return "危急";
                }

                return "首领";
            }

            if (enemy.TimeToBedSeconds <= EnemyWarningFormatter.BedWarningThresholdSeconds)
            {
                return "危急";
            }

            if (isPressureSource || !string.IsNullOrWhiteSpace(EnemyWarningFormatter.Format(enemy)))
            {
                return "高";
            }

            return "普通";
        }

        private static string GetCounterHint(EnemyDefinition definition)
        {
            if (definition.Id == P0PrototypeCatalog.BlackMudNightmareId)
            {
                return "压床前拦截";
            }

            if (definition.Id == P0PrototypeCatalog.ColdLightShadowId)
            {
                return "贴近处理或切换集火";
            }

            if (definition.Id == P0PrototypeCatalog.CallTyrantId)
            {
                return "盯住召唤和投掷计时";
            }

            if (definition.CanBeKnockedBack)
            {
                return "缓速或击退";
            }

            return "集火并开护盾";
        }

        private static bool HasEnemy(IReadOnlyList<P0EnemyHudCard> cards, string enemyId)
        {
            if (cards == null)
            {
                return false;
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].EnemyId == enemyId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasThreat(IReadOnlyList<P0EnemyHudCard> cards, string threatToken)
        {
            if (cards == null)
            {
                return false;
            }

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].ThreatToken == threatToken)
                {
                    return true;
                }
            }

            return false;
        }

        private static int CompareCards(P0EnemyHudCard left, P0EnemyHudCard right)
        {
            int priority = GetPrioritySort(left.PriorityToken).CompareTo(GetPrioritySort(right.PriorityToken));
            if (priority != 0)
            {
                return priority;
            }

            int time = left.TimeToBedSeconds.CompareTo(right.TimeToBedSeconds);
            if (time != 0)
            {
                return time;
            }

            return left.InstanceId.CompareTo(right.InstanceId);
        }

        private static int GetPrioritySort(string priorityToken)
        {
            switch (priorityToken)
            {
                case "危急":
                    return 0;
                case "首领":
                    return 1;
                case "高":
                    return 2;
                default:
                    return 3;
            }
        }

        private static EnemyDefinition GetPrototypeEnemy(string enemyId)
        {
            IReadOnlyList<EnemyDefinition> enemies = P0PrototypeCatalog.CreateCoreEnemies();
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].Id == enemyId)
                {
                    return enemies[i];
                }
            }

            throw new InvalidOperationException("Missing enemy: " + enemyId);
        }
    }
}
