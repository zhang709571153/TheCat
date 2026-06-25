using System;

namespace TheCat.Combat
{
    public sealed class SkillCastResult
    {
        public SkillCastResult(string skillId, float hungerSpent)
        {
            if (string.IsNullOrWhiteSpace(skillId))
            {
                throw new ArgumentException("Skill id is required.", nameof(skillId));
            }

            SkillId = skillId;
            HungerSpent = hungerSpent;
        }

        public string SkillId { get; }

        public float HungerSpent { get; }

        public float DamageApplied { get; private set; }

        public float OwnerSleepRestored { get; private set; }

        public float CatHealingApplied { get; private set; }

        public float ShieldApplied { get; private set; }

        public float BedShieldApplied { get; private set; }

        public float PoopCountdownExtendedSeconds { get; private set; }

        public int StatusApplications { get; private set; }

        public int KnockbacksApplied { get; private set; }

        public int SummonsRequested { get; private set; }

        public bool HadEnemyTarget { get; private set; }

        internal void RecordEnemyTarget()
        {
            HadEnemyTarget = true;
        }

        internal void RecordDamage(float amount)
        {
            DamageApplied += amount;
        }

        internal void RecordOwnerSleepRestore(float amount)
        {
            OwnerSleepRestored += amount;
        }

        internal void RecordCatHeal(float amount)
        {
            CatHealingApplied += amount;
        }

        internal void RecordShield(float amount)
        {
            ShieldApplied += amount;
        }

        internal void RecordBedShield(float amount)
        {
            BedShieldApplied += amount;
        }

        internal void RecordPoopCountdownExtension(float seconds)
        {
            PoopCountdownExtendedSeconds += seconds;
        }

        internal void RecordStatusApplication()
        {
            StatusApplications++;
        }

        internal void RecordKnockback()
        {
            KnockbacksApplied++;
        }

        internal void RecordSummon()
        {
            SummonsRequested++;
        }
    }
}
