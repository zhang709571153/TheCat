using System;
using System.Collections.Generic;

namespace TheCat.Roguelite
{
    public sealed class RunPartnerRoster
    {
        private readonly HashSet<string> catIds = new HashSet<string>();
        private readonly List<string> orderedCatIds = new List<string>();

        public int Count => catIds.Count;

        public IReadOnlyList<string> CatIds => orderedCatIds.AsReadOnly();

        public bool HasCat(string catId)
        {
            return !string.IsNullOrWhiteSpace(catId) && catIds.Contains(catId);
        }

        public bool AddCat(string catId)
        {
            if (string.IsNullOrWhiteSpace(catId))
            {
                throw new ArgumentException("Cat id is required.", nameof(catId));
            }

            if (!catIds.Add(catId))
            {
                return false;
            }

            orderedCatIds.Add(catId);
            return true;
        }
    }
}
