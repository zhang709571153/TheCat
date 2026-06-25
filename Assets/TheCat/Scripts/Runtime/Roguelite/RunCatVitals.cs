using System;
using System.Collections.Generic;
using TheCat.Data.Definitions;

namespace TheCat.Roguelite
{
    public sealed class RunCatVitals
    {
        public const float RestNestHpSafeRatio = 0.7f;

        private readonly Dictionary<string, RunCatVitalSnapshot> snapshotsByCatId =
            new Dictionary<string, RunCatVitalSnapshot>();

        public int Count => snapshotsByCatId.Count;

        public RunCatVitalSnapshot GetOrCreate(CatDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (!snapshotsByCatId.TryGetValue(definition.Id, out RunCatVitalSnapshot snapshot))
            {
                snapshot = new RunCatVitalSnapshot(definition.Id, definition.MaxHp, definition.MaxHp, 0f);
                snapshotsByCatId.Add(definition.Id, snapshot);
            }

            return snapshot;
        }

        public bool TryGet(string catId, out RunCatVitalSnapshot snapshot)
        {
            if (string.IsNullOrWhiteSpace(catId))
            {
                snapshot = default(RunCatVitalSnapshot);
                return false;
            }

            return snapshotsByCatId.TryGetValue(catId, out snapshot);
        }

        public void Capture(string catId, float maxHp, float currentHp, float weakRemainingSeconds)
        {
            RunCatVitalSnapshot snapshot = new RunCatVitalSnapshot(catId, maxHp, currentHp, weakRemainingSeconds);
            snapshotsByCatId[catId] = snapshot;
        }

        public void ApplyRestNestRecovery(float hpSafeRatio = RestNestHpSafeRatio)
        {
            if (hpSafeRatio <= 0f || hpSafeRatio > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(hpSafeRatio), hpSafeRatio, "HP safe ratio must be between zero and one.");
            }

            List<string> catIds = new List<string>(snapshotsByCatId.Keys);
            for (int i = 0; i < catIds.Count; i++)
            {
                RunCatVitalSnapshot snapshot = snapshotsByCatId[catIds[i]];
                float safeHp = snapshot.MaxHp * hpSafeRatio;
                float recoveredHp = snapshot.CurrentHp < safeHp ? safeHp : snapshot.CurrentHp;
                snapshotsByCatId[catIds[i]] = new RunCatVitalSnapshot(snapshot.CatId, snapshot.MaxHp, recoveredHp, 0f);
            }
        }

        public int CountWeakCats()
        {
            int count = 0;
            foreach (RunCatVitalSnapshot snapshot in snapshotsByCatId.Values)
            {
                if (snapshot.IsWeak)
                {
                    count++;
                }
            }

            return count;
        }

        public float GetLowestHpRatio()
        {
            if (snapshotsByCatId.Count == 0)
            {
                return 1f;
            }

            float lowest = 1f;
            foreach (RunCatVitalSnapshot snapshot in snapshotsByCatId.Values)
            {
                if (snapshot.HpRatio < lowest)
                {
                    lowest = snapshot.HpRatio;
                }
            }

            return lowest;
        }
    }
}
