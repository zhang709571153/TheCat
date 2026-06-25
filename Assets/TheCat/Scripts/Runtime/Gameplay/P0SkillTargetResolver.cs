using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0SkillTargetResolver
    {
        public const float DirectionalRange = 2.35f;
        public const float AreaAtTargetRange = 4.15f;
        public const float AutoNearestRange = 5.25f;

        public static P0SkillTargetResult Resolve(
            SkillDefinition skill,
            IReadOnlyList<BattleEnemyState> enemies,
            Vector2 catPosition,
            Func<BattleEnemyState, Vector2> enemyPositionResolver)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            if (enemyPositionResolver == null)
            {
                throw new ArgumentNullException(nameof(enemyPositionResolver));
            }

            bool requiresEnemyTarget = RequiresEnemyTarget(skill);
            if (!requiresEnemyTarget)
            {
                return new P0SkillTargetResult(false, null, 0f, 0f);
            }

            if (enemies == null || enemies.Count == 0)
            {
                return new P0SkillTargetResult(true, null, 0f, GetTargetingRange(skill.TargetingMode));
            }

            float range = GetTargetingRange(skill.TargetingMode);
            BattleEnemyState bestEnemy = null;
            float bestDistance = 0f;
            float bestScore = float.MaxValue;
            for (int i = 0; i < enemies.Count; i++)
            {
                BattleEnemyState enemy = enemies[i];
                if (enemy == null || !enemy.IsAlive)
                {
                    continue;
                }

                float distance = Vector2.Distance(catPosition, enemyPositionResolver(enemy));
                if (distance > range)
                {
                    continue;
                }

                float score = distance / Mathf.Max(0.01f, range);
                if (score >= bestScore)
                {
                    continue;
                }

                bestScore = score;
                bestDistance = distance;
                bestEnemy = enemy;
            }

            return new P0SkillTargetResult(true, bestEnemy, bestDistance, range);
        }

        public static bool RequiresEnemyTarget(SkillDefinition skill)
        {
            if (skill == null)
            {
                throw new ArgumentNullException(nameof(skill));
            }

            for (int i = 0; i < skill.Effects.Count; i++)
            {
                switch (skill.Effects[i].EffectType)
                {
                    case SkillEffectType.Damage:
                    case SkillEffectType.ApplyStatus:
                    case SkillEffectType.Knockback:
                        return true;
                }
            }

            return false;
        }

        public static float GetTargetingRange(SkillTargetingMode targetingMode)
        {
            switch (targetingMode)
            {
                case SkillTargetingMode.Directional:
                    return DirectionalRange;
                case SkillTargetingMode.AreaAtTarget:
                    return AreaAtTargetRange;
                case SkillTargetingMode.AutoNearestEnemy:
                    return AutoNearestRange;
                default:
                    return AutoNearestRange;
            }
        }
    }
}
