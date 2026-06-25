using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public enum P0StatusHudTargetKind
    {
        Bed,
        Enemy,
        Cat
    }

    public readonly struct P0StatusHudIconEntry
    {
        public P0StatusHudIconEntry(
            string statusTagId,
            P0VisualAssetReference iconAsset,
            P0VisualAssetReference compactIconAsset)
        {
            StatusTagId = statusTagId ?? string.Empty;
            IconAsset = iconAsset;
            CompactIconAsset = compactIconAsset;
        }

        public string StatusTagId { get; }

        public P0VisualAssetReference IconAsset { get; }

        public P0VisualAssetReference CompactIconAsset { get; }

        public bool IsReady => !string.IsNullOrWhiteSpace(StatusTagId)
            && IconAsset.HasAsset
            && CompactIconAsset.HasAsset;
    }

    public readonly struct P0StatusHudEntry
    {
        public P0StatusHudEntry(
            string targetId,
            string targetLabel,
            P0StatusHudTargetKind targetKind,
            P0StatusIndicatorState indicator,
            IEnumerable<string> statusTagIds,
            IEnumerable<P0StatusHudIconEntry> statusIcons,
            string responseSummary,
            float movementRateMultiplier,
            float damageTakenMultiplier,
            float timeToBedSeconds,
            float shieldAmount)
        {
            TargetId = targetId ?? string.Empty;
            TargetLabel = targetLabel ?? string.Empty;
            TargetKind = targetKind;
            Indicator = indicator;
            StatusTagIds = statusTagIds == null
                ? Array.Empty<string>()
                : new List<string>(statusTagIds).AsReadOnly();
            StatusIcons = statusIcons == null
                ? Array.Empty<P0StatusHudIconEntry>()
                : new List<P0StatusHudIconEntry>(statusIcons).AsReadOnly();
            ResponseSummary = responseSummary ?? string.Empty;
            MovementRateMultiplier = movementRateMultiplier;
            DamageTakenMultiplier = damageTakenMultiplier;
            TimeToBedSeconds = timeToBedSeconds;
            ShieldAmount = shieldAmount;
        }

        public string TargetId { get; }

        public string TargetLabel { get; }

        public P0StatusHudTargetKind TargetKind { get; }

        public P0StatusIndicatorState Indicator { get; }

        public IReadOnlyList<string> StatusTagIds { get; }

        public IReadOnlyList<P0StatusHudIconEntry> StatusIcons { get; }

        public string ResponseSummary { get; }

        public float MovementRateMultiplier { get; }

        public float DamageTakenMultiplier { get; }

        public float TimeToBedSeconds { get; }

        public float ShieldAmount { get; }

        public bool HasStatuses => Indicator.HasStatuses && StatusTagIds.Count > 0;

        public bool HasStatusIcons => HasStatuses && StatusIcons.Count == StatusTagIds.Count && AllStatusIconsReady();

        public bool HasTag(string statusTagId)
        {
            if (string.IsNullOrWhiteSpace(statusTagId))
            {
                return false;
            }

            for (int i = 0; i < StatusTagIds.Count; i++)
            {
                if (StatusTagIds[i] == statusTagId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasIconFor(string statusTagId)
        {
            if (string.IsNullOrWhiteSpace(statusTagId))
            {
                return false;
            }

            for (int i = 0; i < StatusIcons.Count; i++)
            {
                if (StatusIcons[i].StatusTagId == statusTagId && StatusIcons[i].IconAsset.HasAsset)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasCompactIconFor(string statusTagId)
        {
            if (string.IsNullOrWhiteSpace(statusTagId))
            {
                return false;
            }

            for (int i = 0; i < StatusIcons.Count; i++)
            {
                if (StatusIcons[i].StatusTagId == statusTagId && StatusIcons[i].CompactIconAsset.HasAsset)
                {
                    return true;
                }
            }

            return false;
        }

        private bool AllStatusIconsReady()
        {
            for (int i = 0; i < StatusIcons.Count; i++)
            {
                if (!StatusIcons[i].IsReady)
                {
                    return false;
                }
            }

            return StatusIcons.Count > 0;
        }

        public string BuildSummary()
        {
            string statusText = Indicator.Text.Replace("\r\n", " | ").Replace("\n", " | ");
            return FormatTargetKind(TargetKind)
                + " "
                + TargetLabel
                + "："
                + statusText
                + "，响应："
                + ResponseSummary;
        }

        public string BuildIconSummary()
        {
            return FormatTargetKind(TargetKind)
                + " "
                + TargetLabel
                + " 图标 "
                + StatusIcons.Count
                + "/"
                + StatusTagIds.Count;
        }

        private static string FormatTargetKind(P0StatusHudTargetKind kind)
        {
            switch (kind)
            {
                case P0StatusHudTargetKind.Bed:
                    return "床";
                case P0StatusHudTargetKind.Enemy:
                    return "敌人";
                case P0StatusHudTargetKind.Cat:
                    return "猫";
                default:
                    return "目标";
            }
        }
    }

    public static class P0StatusHudPresenter
    {
        public static IReadOnlyList<P0StatusHudEntry> BuildEntries(
            BattleSimulation battle,
            IReadOnlyList<CatBattleState> cats)
        {
            List<P0StatusHudEntry> entries = new List<P0StatusHudEntry>();
            if (battle == null)
            {
                return entries.AsReadOnly();
            }

            AddIfVisible(entries, BuildBedEntry(battle.BedStatuses));

            for (int i = 0; i < battle.ActiveEnemies.Count; i++)
            {
                AddIfVisible(entries, BuildEnemyEntry(battle.ActiveEnemies[i]));
            }

            if (cats != null)
            {
                for (int i = 0; i < cats.Count; i++)
                {
                    AddIfVisible(entries, BuildCatEntry(cats[i]));
                }
            }

            return entries.AsReadOnly();
        }

        public static IReadOnlyList<P0StatusHudEntry> BuildPrototypeEntries()
        {
            StatusTagDefinition sleepStable = GetPrototypeStatus(StatusTagIds.SleepStable);
            StatusTagDefinition slow = GetPrototypeStatus(StatusTagIds.Slow);
            StatusTagDefinition knockback = GetPrototypeStatus(StatusTagIds.Knockback);
            StatusTagDefinition mark = GetPrototypeStatus(StatusTagIds.Mark);
            StatusTagDefinition shield = GetPrototypeStatus(StatusTagIds.Shield);

            StatusEffectCollection bedStatuses = new StatusEffectCollection();
            bedStatuses.Apply(sleepStable);
            bedStatuses.Apply(shield, 18f);

            BattleEnemyState enemy = new BattleEnemyState(
                1,
                GetPrototypeEnemy(P0PrototypeCatalog.BlackMudNightmareId),
                3f);
            enemy.ApplyStatus(slow);
            enemy.ApplyStatus(knockback);
            enemy.ApplyStatus(mark);
            enemy.ApplyKnockback(2f);

            CatBattleState cat = new CatBattleState(GetPrototypeCat(P0PrototypeCatalog.SaibanId));
            cat.ApplyStatus(shield, 25f);

            return new[]
            {
                BuildBedEntry(bedStatuses),
                BuildEnemyEntry(enemy),
                BuildCatEntry(cat)
            };
        }

        public static P0StatusHudEntry BuildBedEntry(StatusEffectCollection statuses, string label = "床")
        {
            return new P0StatusHudEntry(
                "bed",
                label,
                P0StatusHudTargetKind.Bed,
                P0StatusIndicatorPresenter.Build(statuses, label),
                CollectStatusIds(statuses),
                CollectStatusIcons(statuses),
                DescribeBedResponse(statuses),
                1f,
                1f,
                0f,
                GetMagnitude(statuses, StatusTagIds.Shield));
        }

        public static P0StatusHudEntry BuildEnemyEntry(BattleEnemyState enemy)
        {
            if (enemy == null)
            {
                return default(P0StatusHudEntry);
            }

            string label = enemy.Definition.DisplayName + " #" + enemy.InstanceId;
            return new P0StatusHudEntry(
                enemy.InstanceId.ToString(),
                label,
                P0StatusHudTargetKind.Enemy,
                P0StatusIndicatorPresenter.Build(enemy.Statuses, label),
                CollectStatusIds(enemy.Statuses),
                CollectStatusIcons(enemy.Statuses),
                DescribeEnemyResponse(enemy),
                enemy.MovementRateMultiplier,
                enemy.DamageTakenMultiplier,
                enemy.TimeToBedSeconds,
                0f);
        }

        public static P0StatusHudEntry BuildCatEntry(CatBattleState cat)
        {
            if (cat == null)
            {
                return default(P0StatusHudEntry);
            }

            return new P0StatusHudEntry(
                cat.Definition.Id,
                cat.Definition.DisplayName,
                P0StatusHudTargetKind.Cat,
                P0StatusIndicatorPresenter.Build(cat.Statuses, cat.Definition.DisplayName),
                CollectStatusIds(cat.Statuses),
                CollectStatusIcons(cat.Statuses),
                DescribeCatResponse(cat),
                1f,
                1f,
                0f,
                GetMagnitude(cat.Statuses, StatusTagIds.Shield));
        }

        public static bool HasP0StatusHudEntries(IReadOnlyList<P0StatusHudEntry> entries)
        {
            return HasKind(entries, P0StatusHudTargetKind.Bed)
                && HasKind(entries, P0StatusHudTargetKind.Enemy)
                && HasKind(entries, P0StatusHudTargetKind.Cat)
                && HasTag(entries, StatusTagIds.SleepStable)
                && HasTag(entries, StatusTagIds.Slow)
                && HasTag(entries, StatusTagIds.Knockback)
                && HasTag(entries, StatusTagIds.Mark)
                && HasTag(entries, StatusTagIds.Shield);
        }

        public static string BuildCompactSummary(IReadOnlyList<P0StatusHudEntry> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                return "状态 HUD：空";
            }

            HashSet<string> uniqueTags = new HashSet<string>();
            int bedCount = 0;
            int enemyCount = 0;
            int catCount = 0;
            int shieldCount = 0;
            int responseCount = 0;

            for (int i = 0; i < entries.Count; i++)
            {
                if (!entries[i].HasStatuses)
                {
                    continue;
                }

                switch (entries[i].TargetKind)
                {
                    case P0StatusHudTargetKind.Bed:
                        bedCount++;
                        break;
                    case P0StatusHudTargetKind.Enemy:
                        enemyCount++;
                        break;
                    case P0StatusHudTargetKind.Cat:
                        catCount++;
                        break;
                }

                if (entries[i].HasTag(StatusTagIds.Shield))
                {
                    shieldCount++;
                }

                if (!string.IsNullOrWhiteSpace(entries[i].ResponseSummary))
                {
                    responseCount++;
                }

                for (int j = 0; j < entries[i].StatusTagIds.Count; j++)
                {
                    uniqueTags.Add(entries[i].StatusTagIds[j]);
                }
            }

            return "状态 HUD："
                + entries.Count
                + " 床 "
                + bedCount
                + " 敌人 "
                + enemyCount
                + " 猫 "
                + catCount
                + " 标签 "
                + uniqueTags.Count
                + " 护盾 "
                + shieldCount
                + " 图标 "
                + CountIconReadyEntries(entries)
                + " 响应 "
                + responseCount;
        }

        private static void AddIfVisible(List<P0StatusHudEntry> entries, P0StatusHudEntry entry)
        {
            if (entry.HasStatuses)
            {
                entries.Add(entry);
            }
        }

        private static IReadOnlyList<string> CollectStatusIds(StatusEffectCollection statuses)
        {
            if (statuses == null || statuses.Count == 0)
            {
                return Array.Empty<string>();
            }

            List<string> ids = new List<string>();
            foreach (StatusEffectState effect in statuses.ActiveEffects)
            {
                if (effect != null && !string.IsNullOrWhiteSpace(effect.Id))
                {
                    ids.Add(effect.Id);
                }
            }

            ids.Sort(StringComparer.Ordinal);
            return ids.AsReadOnly();
        }

        private static IReadOnlyList<P0StatusHudIconEntry> CollectStatusIcons(StatusEffectCollection statuses)
        {
            IReadOnlyList<string> ids = CollectStatusIds(statuses);
            if (ids.Count == 0)
            {
                return Array.Empty<P0StatusHudIconEntry>();
            }

            List<P0StatusHudIconEntry> icons = new List<P0StatusHudIconEntry>();
            for (int i = 0; i < ids.Count; i++)
            {
                icons.Add(new P0StatusHudIconEntry(
                    ids[i],
                    P0VisualAssetCatalog.GetStatusIcon(ids[i]),
                    P0VisualAssetCatalog.GetCompactStatusIcon(ids[i])));
            }

            return icons.AsReadOnly();
        }

        private static int CountIconReadyEntries(IReadOnlyList<P0StatusHudEntry> entries)
        {
            int count = 0;
            if (entries == null)
            {
                return count;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].HasStatusIcons)
                {
                    count++;
                }
            }

            return count;
        }

        private static string DescribeBedResponse(StatusEffectCollection statuses)
        {
            List<string> responses = new List<string>();
            if (HasStatus(statuses, StatusTagIds.SleepStable))
            {
                responses.Add("主人睡眠稳定");
            }

            float shield = GetMagnitude(statuses, StatusTagIds.Shield);
            if (shield > 0f)
            {
                responses.Add("床护盾吸收 " + shield.ToString("0.#") + " 睡眠伤害");
            }

            return responses.Count == 0 ? string.Empty : string.Join("; ", responses);
        }

        private static string DescribeEnemyResponse(BattleEnemyState enemy)
        {
            List<string> responses = new List<string>();
            if (enemy.Statuses.Has(StatusTagIds.Slow))
            {
                responses.Add("移动 " + enemy.MovementRateMultiplier.ToString("0.##") + " 倍");
            }

            if (enemy.Statuses.Has(StatusTagIds.Knockback))
            {
                responses.Add(enemy.Definition.CanBeKnockedBack
                    ? "击退生效，压床倒计时 " + enemy.TimeToBedSeconds.ToString("0.0") + "s"
                    : "抵抗击退");
            }

            if (enemy.Statuses.Has(StatusTagIds.Mark))
            {
                responses.Add("承伤 " + enemy.DamageTakenMultiplier.ToString("0.##") + " 倍");
            }

            return responses.Count == 0 ? string.Empty : string.Join("; ", responses);
        }

        private static string DescribeCatResponse(CatBattleState cat)
        {
            float shield = GetMagnitude(cat.Statuses, StatusTagIds.Shield);
            return shield > 0f
                ? "猫护盾先于生命吸收 " + shield.ToString("0.#") + " 伤害"
                : string.Empty;
        }

        private static string FormatTargetKind(P0StatusHudTargetKind kind)
        {
            switch (kind)
            {
                case P0StatusHudTargetKind.Bed:
                    return "床";
                case P0StatusHudTargetKind.Enemy:
                    return "敌人";
                case P0StatusHudTargetKind.Cat:
                    return "猫";
                default:
                    return "目标";
            }
        }

        private static bool HasKind(IReadOnlyList<P0StatusHudEntry> entries, P0StatusHudTargetKind kind)
        {
            if (entries == null)
            {
                return false;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].TargetKind == kind && entries[i].HasStatuses)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasTag(IReadOnlyList<P0StatusHudEntry> entries, string statusTagId)
        {
            if (entries == null)
            {
                return false;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].HasTag(statusTagId))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasStatus(StatusEffectCollection statuses, string statusTagId)
        {
            return statuses != null && statuses.Has(statusTagId);
        }

        private static float GetMagnitude(StatusEffectCollection statuses, string statusTagId)
        {
            if (statuses == null || !statuses.TryGet(statusTagId, out StatusEffectState effect))
            {
                return 0f;
            }

            return effect.Magnitude;
        }

        private static StatusTagDefinition GetPrototypeStatus(string statusId)
        {
            IReadOnlyList<StatusTagDefinition> statuses = P0PrototypeCatalog.CreateStatusTags();
            for (int i = 0; i < statuses.Count; i++)
            {
                if (statuses[i].Id == statusId)
                {
                    return statuses[i];
                }
            }

            throw new InvalidOperationException("Missing status: " + statusId);
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

        private static CatDefinition GetPrototypeCat(string catId)
        {
            IReadOnlyList<CatDefinition> cats = P0PrototypeCatalog.CreateStarterCats();
            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i];
                }
            }

            throw new InvalidOperationException("Missing cat: " + catId);
        }
    }
}
