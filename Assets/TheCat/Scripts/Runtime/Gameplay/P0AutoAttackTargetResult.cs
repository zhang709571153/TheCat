using TheCat.Combat;

namespace TheCat.Gameplay
{
    public readonly struct P0AutoAttackTargetResult
    {
        public P0AutoAttackTargetResult(BattleEnemyState enemy, float distance, float range)
        {
            Enemy = enemy;
            Distance = distance;
            Range = range;
        }

        public BattleEnemyState Enemy { get; }

        public float Distance { get; }

        public float Range { get; }

        public bool HasTarget => Enemy != null;
    }
}
