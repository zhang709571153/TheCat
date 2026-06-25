using System;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0SkillIndicatorPresenter
    {
        public static P0SkillIndicatorState Build(
            SkillDefinition skill,
            float cooldownSeconds,
            Vector2 origin,
            P0SkillTargetResult target,
            Func<BattleEnemyState, Vector2> targetPositionResolver)
        {
            if (skill == null)
            {
                return new P0SkillIndicatorState(
                    string.Empty,
                    string.Empty,
                    0f,
                    false,
                    string.Empty,
                    origin,
                    Vector2.zero,
                    0f,
                    0f);
            }

            bool requiresEnemyTarget = target.RequiresEnemyTarget || P0SkillTargetResolver.RequiresEnemyTarget(skill);
            float range = target.Range > 0f
                ? target.Range
                : requiresEnemyTarget ? P0SkillTargetResolver.GetTargetingRange(skill.TargetingMode) : 0f;
            string targetDisplayName = string.Empty;
            Vector2 targetPosition = Vector2.zero;
            float distance = target.Distance;
            if (target.HasEnemyTarget)
            {
                targetDisplayName = target.Enemy.Definition.DisplayName;
                if (targetPositionResolver != null)
                {
                    targetPosition = targetPositionResolver(target.Enemy);
                    distance = Vector2.Distance(origin, targetPosition);
                }
            }

            return new P0SkillIndicatorState(
                skill.Id,
                P0SkillPresenter.Describe(skill).DisplayName,
                cooldownSeconds,
                requiresEnemyTarget,
                targetDisplayName,
                origin,
                targetPosition,
                distance,
                range);
        }
    }
}
