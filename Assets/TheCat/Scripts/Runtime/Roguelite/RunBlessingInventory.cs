using System;
using System.Collections.Generic;

namespace TheCat.Roguelite
{
    public sealed class RunBlessingInventory
    {
        public const int MaxLevel = 3;

        private readonly List<AuthorityBlessingDefinition> blessings = new List<AuthorityBlessingDefinition>();
        private readonly HashSet<string> blessingIds = new HashSet<string>();
        private readonly Dictionary<string, int> levelsById = new Dictionary<string, int>();

        public IReadOnlyList<AuthorityBlessingDefinition> ActiveBlessings => blessings.AsReadOnly();

        public int Count => blessings.Count;

        public int TotalLevel
        {
            get
            {
                int totalLevel = 0;
                for (int i = 0; i < blessings.Count; i++)
                {
                    totalLevel += GetLevel(blessings[i].Id);
                }

                return totalLevel;
            }
        }

        public bool Has(string blessingId)
        {
            return !string.IsNullOrWhiteSpace(blessingId) && blessingIds.Contains(blessingId);
        }

        public int GetLevel(string blessingId)
        {
            if (string.IsNullOrWhiteSpace(blessingId))
            {
                return 0;
            }

            return levelsById.TryGetValue(blessingId, out int level) ? level : 0;
        }

        public bool CanUpgrade(string blessingId)
        {
            int level = GetLevel(blessingId);
            return level > 0 && level < MaxLevel;
        }

        public bool Add(AuthorityBlessingDefinition blessing)
        {
            if (blessing == null)
            {
                throw new ArgumentNullException(nameof(blessing));
            }

            if (!blessingIds.Add(blessing.Id))
            {
                return false;
            }

            blessings.Add(blessing);
            levelsById[blessing.Id] = 1;
            return true;
        }

        public bool Upgrade(string blessingId)
        {
            if (!CanUpgrade(blessingId))
            {
                return false;
            }

            levelsById[blessingId]++;
            return true;
        }

        public string BuildSummary()
        {
            if (blessings.Count == 0)
            {
                return "无";
            }

            List<string> summaries = new List<string>();
            for (int i = 0; i < blessings.Count; i++)
            {
                AuthorityBlessingDefinition blessing = blessings[i];
                summaries.Add(blessing.DisplayName + " 等级" + GetLevel(blessing.Id));
            }

            return string.Join("; ", summaries);
        }
    }
}
