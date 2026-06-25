using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0EnemyPressureResolver
    {
        public const float MeleePressureRange = 0.95f;
        public const float ChargePressureRange = 1.35f;
        public const float RangedPressureRange = 4.25f;
        public const float FlyingAttachPressureRange = 2.1f;
        public const float JumpSlamPressureRange = 2.6f;
        public const float BossPressureRange = 5.2f;

        public static P0EnemyPressureResult FindBestPressureSource(
            IReadOnlyList<BattleEnemyState> enemies,
            Vector2 catPosition,
            Func<BattleEnemyState, Vector2> enemyPositionResolver)
        {
            if (enemyPositionResolver == null)
            {
                throw new ArgumentNullException(nameof(enemyPositionResolver));
            }

            if (enemies == null || enemies.Count == 0)
            {
                return default(P0EnemyPressureResult);
            }

            P0EnemyPressureResult best = default(P0EnemyPressureResult);
            float bestScore = float.MaxValue;
            for (int i = 0; i < enemies.Count; i++)
            {
                BattleEnemyState enemy = enemies[i];
                if (enemy == null || !enemy.IsAlive || enemy.Definition.PlayerDamage <= 0f)
                {
                    continue;
                }

                float range = GetPressureRange(enemy.Definition.BehaviorType);
                Vector2 enemyPosition = enemyPositionResolver(enemy);
                float distance = Vector2.Distance(catPosition, enemyPosition);
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
                best = new P0EnemyPressureResult(
                    enemy,
                    distance,
                    range,
                    GetDamageMultiplier(enemy.Definition.BehaviorType));
            }

            return best;
        }

        public static float GetPressureRange(EnemyBehaviorType behaviorType)
        {
            switch (behaviorType)
            {
                case EnemyBehaviorType.MoveToBed:
                    return MeleePressureRange;
                case EnemyBehaviorType.Charger:
                    return ChargePressureRange;
                case EnemyBehaviorType.RangedHarasser:
                    return RangedPressureRange;
                case EnemyBehaviorType.FlyingAttachment:
                    return FlyingAttachPressureRange;
                case EnemyBehaviorType.EliteJumpSlam:
                    return JumpSlamPressureRange;
                case EnemyBehaviorType.BossCallTyrant:
                    return BossPressureRange;
                default:
                    return MeleePressureRange;
            }
        }

        public static float GetDamageMultiplier(EnemyBehaviorType behaviorType)
        {
            switch (behaviorType)
            {
                case EnemyBehaviorType.Charger:
                    return 1.15f;
                case EnemyBehaviorType.RangedHarasser:
                    return 0.8f;
                case EnemyBehaviorType.FlyingAttachment:
                    return 0.65f;
                case EnemyBehaviorType.EliteJumpSlam:
                    return 1.25f;
                case EnemyBehaviorType.BossCallTyrant:
                    return 1f;
                default:
                    return 1f;
            }
        }
    }
}
