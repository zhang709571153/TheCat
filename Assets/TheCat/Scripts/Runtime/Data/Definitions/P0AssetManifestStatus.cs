namespace TheCat.Data.Definitions
{
    public static class P0AssetManifestStatus
    {
        public const string Planned = "planned";
        public const string Generated = "generated";
        public const string Imported = "imported";
        public const string Rejected = "rejected";
        public const string Replaced = "replaced";

        public static bool IsKnown(string status)
        {
            return status == Planned
                || status == Generated
                || status == Imported
                || status == Rejected
                || status == Replaced;
        }

        public static bool RequiresWorkspaceFile(string status)
        {
            return status == Generated
                || status == Imported
                || status == Replaced;
        }

        public static bool IsPending(string status)
        {
            return status == Planned;
        }
    }
}
