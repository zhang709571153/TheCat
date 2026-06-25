using System;
using TheCat.Combat;
using TheCat.Data;

namespace TheCat.Gameplay
{
    public readonly struct P0CatPressureApplication
    {
        public P0CatPressureApplication(float incomingDamage, float damageTaken, bool becameWeak)
        {
            IncomingDamage = incomingDamage;
            DamageTaken = damageTaken;
            BecameWeak = becameWeak;
        }

        public float IncomingDamage { get; }

        public float DamageTaken { get; }

        public float DamageAbsorbed => Math.Max(0f, IncomingDamage - DamageTaken);

        public bool BecameWeak { get; }
    }

    public static class P0CatPressureApplier
    {
        public static P0CatPressureApplication Apply(
            NodeMetrics metrics,
            CatBattleState cat,
            BattleEnemyState enemy,
            float damageScale,
            float damageMultiplier)
        {
            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            if (cat == null)
            {
                throw new ArgumentNullException(nameof(cat));
            }

            if (enemy == null)
            {
                throw new ArgumentNullException(nameof(enemy));
            }

            if (damageScale < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damageScale), damageScale, "Damage scale must not be negative.");
            }

            if (damageMultiplier < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(damageMultiplier), damageMultiplier, "Damage multiplier must not be negative.");
            }

            bool wasWeak = cat.Vital.IsWeak;
            float incomingDamage = enemy.Definition.PlayerDamage * damageScale * damageMultiplier;
            float damageTaken = cat.ApplyDamage(incomingDamage);
            bool becameWeak = !wasWeak && cat.Vital.IsWeak;

            metrics.RecordCatPressure(incomingDamage, damageTaken);
            if (becameWeak)
            {
                metrics.RecordWeakIncident();
            }

            return new P0CatPressureApplication(incomingDamage, damageTaken, becameWeak);
        }
    }
}
