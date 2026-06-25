using System;
using TheCat.Combat;
using TheCat.Data;

namespace TheCat.Roguelite
{
    public sealed class RunPendingBattleModifiers
    {
        public RunPendingBattleModifiers()
        {
            Clear();
        }

        public float SkillDamageMultiplier { get; private set; }

        public float PoopGrowthMultiplier { get; private set; }

        public float ShieldMultiplier { get; private set; }

        public float EnemyStatusDurationMultiplier { get; private set; }

        public float OwnerSleepRestoreMultiplier { get; private set; }

        public float CatHealMultiplier { get; private set; }

        public int SourceCount { get; private set; }

        public bool HasPending => SourceCount > 0;

        public void Add(
            float skillDamageMultiplier = 1f,
            float poopGrowthMultiplier = 1f,
            float shieldMultiplier = 1f,
            float enemyStatusDurationMultiplier = 1f,
            float ownerSleepRestoreMultiplier = 1f,
            float catHealMultiplier = 1f)
        {
            SkillDamageMultiplier *= RequirePositive(skillDamageMultiplier, nameof(skillDamageMultiplier));
            PoopGrowthMultiplier *= RequirePositive(poopGrowthMultiplier, nameof(poopGrowthMultiplier));
            ShieldMultiplier *= RequirePositive(shieldMultiplier, nameof(shieldMultiplier));
            EnemyStatusDurationMultiplier *= RequirePositive(enemyStatusDurationMultiplier, nameof(enemyStatusDurationMultiplier));
            OwnerSleepRestoreMultiplier *= RequirePositive(ownerSleepRestoreMultiplier, nameof(ownerSleepRestoreMultiplier));
            CatHealMultiplier *= RequirePositive(catHealMultiplier, nameof(catHealMultiplier));
            SourceCount++;
        }

        public RunPendingBattleModifierSnapshot Consume()
        {
            RunPendingBattleModifierSnapshot snapshot = new RunPendingBattleModifierSnapshot(
                SkillDamageMultiplier,
                PoopGrowthMultiplier,
                ShieldMultiplier,
                EnemyStatusDurationMultiplier,
                OwnerSleepRestoreMultiplier,
                CatHealMultiplier,
                SourceCount);

            Clear();
            return snapshot;
        }

        public string BuildSummary()
        {
            if (!HasPending)
            {
                return "无";
            }

            return "下一战技能 " + SkillDamageMultiplier.ToString("0.##") + " 倍"
                + " 屎意 " + PoopGrowthMultiplier.ToString("0.##") + " 倍"
                + " 护盾 " + ShieldMultiplier.ToString("0.##") + " 倍"
                + " 状态 " + EnemyStatusDurationMultiplier.ToString("0.##") + " 倍"
                + " 睡眠 " + OwnerSleepRestoreMultiplier.ToString("0.##") + " 倍"
                + " 治疗 " + CatHealMultiplier.ToString("0.##") + " 倍"
                + " 来源 " + SourceCount;
        }

        public string BuildDiagnosticSummary()
        {
            if (!HasPending)
            {
                return "none";
            }

            return "next skill x" + SkillDamageMultiplier.ToString("0.##")
                + " poop x" + PoopGrowthMultiplier.ToString("0.##")
                + " shield x" + ShieldMultiplier.ToString("0.##")
                + " status x" + EnemyStatusDurationMultiplier.ToString("0.##")
                + " sleep x" + OwnerSleepRestoreMultiplier.ToString("0.##")
                + " heal x" + CatHealMultiplier.ToString("0.##")
                + " sources " + SourceCount;
        }

        private void Clear()
        {
            SkillDamageMultiplier = 1f;
            PoopGrowthMultiplier = 1f;
            ShieldMultiplier = 1f;
            EnemyStatusDurationMultiplier = 1f;
            OwnerSleepRestoreMultiplier = 1f;
            CatHealMultiplier = 1f;
            SourceCount = 0;
        }

        private static float RequirePositive(float value, string name)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Multiplier must be greater than zero.");
            }

            return value;
        }
    }

    public readonly struct RunPendingBattleModifierSnapshot
    {
        public RunPendingBattleModifierSnapshot(
            float skillDamageMultiplier,
            float poopGrowthMultiplier,
            float shieldMultiplier,
            float enemyStatusDurationMultiplier,
            float ownerSleepRestoreMultiplier,
            float catHealMultiplier,
            int sourceCount)
        {
            SkillDamageMultiplier = RequirePositive(skillDamageMultiplier, nameof(skillDamageMultiplier));
            PoopGrowthMultiplier = RequirePositive(poopGrowthMultiplier, nameof(poopGrowthMultiplier));
            ShieldMultiplier = RequirePositive(shieldMultiplier, nameof(shieldMultiplier));
            EnemyStatusDurationMultiplier = RequirePositive(enemyStatusDurationMultiplier, nameof(enemyStatusDurationMultiplier));
            OwnerSleepRestoreMultiplier = RequirePositive(ownerSleepRestoreMultiplier, nameof(ownerSleepRestoreMultiplier));
            CatHealMultiplier = RequirePositive(catHealMultiplier, nameof(catHealMultiplier));

            if (sourceCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceCount), sourceCount, "Source count must not be negative.");
            }

            SourceCount = sourceCount;
        }

        public float SkillDamageMultiplier { get; }

        public float PoopGrowthMultiplier { get; }

        public float ShieldMultiplier { get; }

        public float EnemyStatusDurationMultiplier { get; }

        public float OwnerSleepRestoreMultiplier { get; }

        public float CatHealMultiplier { get; }

        public int SourceCount { get; }

        public bool HasPending => SourceCount > 0;

        public BattleModifierSet ApplyTo(BattleModifierSet modifiers)
        {
            BattleModifierSet source = modifiers ?? BattleModifierSet.Neutral;
            if (!HasPending)
            {
                return source;
            }

            return new BattleModifierSet(
                skillDamageMultiplier: source.SkillDamageMultiplier * SkillDamageMultiplier,
                shieldMultiplier: source.ShieldMultiplier * ShieldMultiplier,
                knockbackMultiplier: source.KnockbackMultiplier,
                enemyStatusDurationMultiplier: source.EnemyStatusDurationMultiplier * EnemyStatusDurationMultiplier,
                ownerSleepRestoreMultiplier: source.OwnerSleepRestoreMultiplier * OwnerSleepRestoreMultiplier,
                catHealMultiplier: source.CatHealMultiplier * CatHealMultiplier);
        }

        public P0Tuning ApplyTo(P0Tuning tuning)
        {
            if (!HasPending)
            {
                return tuning;
            }

            return tuning.With(
                poopNaturalGrowthPerSecond: tuning.PoopNaturalGrowthPerSecond * PoopGrowthMultiplier);
        }

        private static float RequirePositive(float value, string name)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Multiplier must be greater than zero.");
            }

            return value;
        }
    }
}
