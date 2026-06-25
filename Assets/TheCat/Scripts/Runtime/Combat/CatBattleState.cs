using System;
using TheCat.Data;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;

namespace TheCat.Combat
{
    public sealed class CatBattleState
    {
        public CatBattleState(CatDefinition definition, float currentHp = -1f, float weakRemainingSeconds = 0f)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            Vital = new CatVitalState(definition.Id, definition.MaxHp, currentHp, weakRemainingSeconds);
            Statuses = new StatusEffectCollection();
        }

        public CatDefinition Definition { get; }

        public CatVitalState Vital { get; }

        public StatusEffectCollection Statuses { get; }

        public float ApplyDamage(float amount)
        {
            float unabsorbedDamage = Statuses.AbsorbDamage(StatusTagIds.Shield, amount);
            Vital.ApplyDamage(unabsorbedDamage);
            return unabsorbedDamage;
        }

        public void Heal(float amount)
        {
            Vital.Heal(amount);
        }

        public void ApplyStatus(StatusTagDefinition definition, float magnitudeOverride = -1f, float durationOverrideSeconds = -1f)
        {
            Statuses.Apply(definition, magnitudeOverride, durationOverrideSeconds);
        }

        public void Tick(float deltaSeconds)
        {
            Statuses.Tick(deltaSeconds);
            Vital.Tick(deltaSeconds);
        }
    }
}
