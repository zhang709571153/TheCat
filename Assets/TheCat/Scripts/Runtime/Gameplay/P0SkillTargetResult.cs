using TheCat.Combat;

namespace TheCat.Gameplay
{
    public readonly struct P0SkillTargetResult
    {
        public P0SkillTargetResult(
            bool requiresEnemyTarget,
            BattleEnemyState enemy,
            float distance,
            float range)
        {
            RequiresEnemyTarget = requiresEnemyTarget;
            Enemy = enemy;
            Distance = distance;
            Range = range;
        }

        public bool RequiresEnemyTarget { get; }

        public BattleEnemyState Enemy { get; }

        public float Distance { get; }

        public float Range { get; }

        public bool HasEnemyTarget => Enemy != null;

        public bool CanCast => !RequiresEnemyTarget || HasEnemyTarget;
    }
}
