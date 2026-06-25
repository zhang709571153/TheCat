using UnityEngine;

namespace TheCat.Gameplay
{
    public readonly struct P0SkillIndicatorState
    {
        public P0SkillIndicatorState(
            string skillId,
            string skillDisplayName,
            float cooldownSeconds,
            bool requiresEnemyTarget,
            string targetDisplayName,
            Vector2 origin,
            Vector2 targetPosition,
            float distance,
            float range)
        {
            SkillId = skillId ?? string.Empty;
            SkillDisplayName = skillDisplayName ?? string.Empty;
            CooldownSeconds = cooldownSeconds;
            RequiresEnemyTarget = requiresEnemyTarget;
            TargetDisplayName = targetDisplayName ?? string.Empty;
            Origin = origin;
            TargetPosition = targetPosition;
            Distance = distance;
            Range = range;
        }

        public string SkillId { get; }

        public string SkillDisplayName { get; }

        public float CooldownSeconds { get; }

        public bool RequiresEnemyTarget { get; }

        public string TargetDisplayName { get; }

        public Vector2 Origin { get; }

        public Vector2 TargetPosition { get; }

        public float Distance { get; }

        public float Range { get; }

        public bool HasSkill => !string.IsNullOrWhiteSpace(SkillId);

        public bool IsCoolingDown => CooldownSeconds > 0f;

        public bool HasTarget => !string.IsNullOrWhiteSpace(TargetDisplayName);

        public bool ShowsRange => RequiresEnemyTarget && Range > 0f;

        public bool ShowsTarget => RequiresEnemyTarget && HasTarget;

        public bool CanCast => HasSkill && !IsCoolingDown && (!RequiresEnemyTarget || HasTarget);

        public string BuildSummary()
        {
            if (!HasSkill)
            {
                return "技能指示：缺少技能";
            }

            string summary = "技能指示：" + SkillDisplayName + " ";
            if (IsCoolingDown)
            {
                summary += "冷却 " + CooldownSeconds.ToString("0.0") + "s | ";
            }

            if (!RequiresEnemyTarget)
            {
                return summary + "不需要敌人目标";
            }

            if (HasTarget)
            {
                return summary
                    + "目标 "
                    + TargetDisplayName
                    + " "
                    + Distance.ToString("0.0")
                    + "/"
                    + Range.ToString("0.0")
                    + "m";
            }

            return summary + "无目标 <= " + Range.ToString("0.0") + "m";
        }
    }
}
