using System.Collections.Generic;

namespace TheCat.Data
{
    public static class StatusTagIds
    {
        public const string SleepStable = "sleep_stable";
        public const string Slow = "slow";
        public const string Knockback = "knockback";
        public const string Mark = "mark";
        public const string Shield = "shield";

        private static readonly HashSet<string> p0Tags = new HashSet<string>
        {
            SleepStable,
            Slow,
            Knockback,
            Mark,
            Shield
        };

        public static bool IsP0Tag(string tagId)
        {
            return !string.IsNullOrWhiteSpace(tagId) && p0Tags.Contains(tagId);
        }
    }
}
