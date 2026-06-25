using TheCat.Combat;

namespace TheCat.Gameplay
{
    public readonly struct P0EnemyPressureResult
    {
        public P0EnemyPressureResult(
            BattleEnemyState enemy,
            float distance,
            float range,
            float damageMultiplier)
        {
            Enemy = enemy;
            Distance = distance;
            Range = range;
            DamageMultiplier = damageMultiplier;
        }

        public BattleEnemyState Enemy { get; }

        public float Distance { get; }

        public float Range { get; }

        public float DamageMultiplier { get; }

        public bool HasEnemy => Enemy != null;
    }
}
