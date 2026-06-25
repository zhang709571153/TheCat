using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0AutoAttackTargetResolver
    {
        public const float DefenderAttackRange = 1.45f;
        public const float ControllerAttackRange = 3.75f;
        public const float HealerAttackRange = 2.65f;

        public static P0AutoAttackTargetResult FindBestTarget(
            IReadOnlyList<BattleEnemyState> enemies,
            Vector2 catPosition,
            CatBattleState attacker,
            Func<BattleEnemyState, Vector2> enemyPositionResolver)
        {
            if (enemyPositionResolver == null)
            {
                throw new ArgumentNullException(nameof(enemyPositionResolver));
            }

            if (attacker == null || enemies == null || enemies.Count == 0)
            {
                return default(P0AutoAttackTargetResult);
            }

            float range = GetAttackRange(attacker.Definition.Role);
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

            return bestEnemy == null
                ? default(P0AutoAttackTargetResult)
                : new P0AutoAttackTargetResult(bestEnemy, bestDistance, range);
        }

        public static float GetAttackRange(CatRole role)
        {
            switch (role)
            {
                case CatRole.Defender:
                    return DefenderAttackRange;
                case CatRole.Controller:
                    return ControllerAttackRange;
                case CatRole.Healer:
                    return HealerAttackRange;
                default:
                    return HealerAttackRange;
            }
        }
    }
}
