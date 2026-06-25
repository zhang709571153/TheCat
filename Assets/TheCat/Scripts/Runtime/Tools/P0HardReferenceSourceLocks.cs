using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public delegate bool P0HardReferenceHashReader(string path, out string sha256, out string error);

    public enum P0HardReferenceSourceLockSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0HardReferenceSourceLockIssue
    {
        public P0HardReferenceSourceLockIssue(P0HardReferenceSourceLockSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0HardReferenceSourceLockSeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0HardReferenceSourceLockEntry
    {
        public P0HardReferenceSourceLockEntry(string lockId, string groupId, string path, string sha256)
        {
            LockId = lockId ?? string.Empty;
            GroupId = groupId ?? string.Empty;
            Path = path ?? string.Empty;
            Sha256 = (sha256 ?? string.Empty).Trim().ToLowerInvariant();
        }

        public string LockId { get; }

        public string GroupId { get; }

        public string Path { get; }

        public string Sha256 { get; }
    }

    public sealed class P0HardReferenceSourceLockReport
    {
        private readonly List<P0HardReferenceSourceLockIssue> issues = new List<P0HardReferenceSourceLockIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0HardReferenceSourceLockIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int LockCount { get; private set; }

        public int HashMatchedCount { get; private set; }

        public int ManifestLinkedAssetCount { get; private set; }

        public int FailureCount => Count(P0HardReferenceSourceLockSeverity.Failure);

        public bool IsReady => FailureCount == 0 && coveredChecks.Count >= P0HardReferenceSourceLocks.ExpectedCoveredCheckCount;

        public void SetCounts(int lockCount, int hashMatchedCount)
        {
            LockCount = lockCount;
            HashMatchedCount = hashMatchedCount;
        }

        public void SetManifestLinkedAssetCount(int manifestLinkedAssetCount)
        {
            ManifestLinkedAssetCount = manifestLinkedAssetCount;
        }

        public void AddIssue(P0HardReferenceSourceLockSeverity severity, string message)
        {
            issues.Add(new P0HardReferenceSourceLockIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsReady
                ? "P0 hard reference source locks ready for " + LockCount + " source file(s) and " + ManifestLinkedAssetCount + " manifest asset link(s)."
                : "P0 hard reference source locks have " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Locked source files: " + LockCount,
                "Source hashes matched: " + HashMatchedCount,
                "Manifest assets linked to source locks: " + ManifestLinkedAssetCount
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0HardReferenceSourceLockSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0HardReferenceSourceLocks
    {
        public const int ExpectedCoveredCheckCount = 8;
        public const int ExpectedManifestLinkedAssetCount = 28;

        private const string DesignRoot = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets";

        public static IReadOnlyList<P0HardReferenceSourceLockEntry> CreateP0Locks()
        {
            return new[]
            {
                Lock("saiban_turnaround_colored", P0PrototypeCatalog.SaibanId, DesignRoot + "/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png", "156a7fcb4ac3e9a75bf54788f12b7f18a43b6eaff3c14607ea689af612403dc1"),
                Lock("nephthys_turnaround_colored", P0PrototypeCatalog.NephthysId, DesignRoot + "/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png", "37ad1532c8a981baaff67c05009ddc38482e9f00e71e6addfb2b321dad31de06"),
                Lock("suzune_turnaround_colored", P0PrototypeCatalog.SuzuneId, DesignRoot + "/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png", "9b616470da7daa77ad27c70d2b0bf3b3f30649ce0b639f6550589b9e6fe700b3"),
                Lock("black_mud_concept", "black_mud_nightmare", DesignRoot + "/enemies/en01_black_mud_nightmare/concept/black_mud_nightmare_concept.png", "9b03cfa67397cf5f806fe98af6ecfdc8ffc747cf0ec1f814f0b9456a132a5bc0"),
                Lock("black_mud_animation", "black_mud_nightmare", DesignRoot + "/enemies/en01_black_mud_nightmare/animation/black_mud_nightmare_animation.png", "c9fc4d282aa8cafc7f32b5496b18881cbb34a493ea861d3260d7aa5647e73516"),
                Lock("cold_light_concept", "cold_light_shadow", DesignRoot + "/enemies/en04_cold_light_shadow/concept/cold_light_shadow_concept.png", "2e73b84e8f376d28e63850d891bd9b76f6ce55c9aa8d2ba85cd374812588a138"),
                Lock("cold_light_animation", "cold_light_shadow", DesignRoot + "/enemies/en04_cold_light_shadow/animation/cold_light_shadow_animation.png", "5b1b3a79e9e6e414ab5aec4e03911ed71ab0da4dd94fd5c88ef7bbeecb1ef8c2"),
                Lock("call_tyrant_concept", "call_tyrant", DesignRoot + "/enemies/en06_call_tyrant/concept/call_tyrant_concept.png", "070d2aaf140428690aac802bf0a893cd0b40fd6b60f2835f3600bb95c419cca1"),
                Lock("call_tyrant_animation", "call_tyrant", DesignRoot + "/enemies/en06_call_tyrant/animation/call_tyrant_animation.png", "b20a5e4d6b628ee1518af0b77c78b75327119e3cbb06b2fdeb80de5c2d31a22b"),
                Lock("bedroom_map_concept", "bedroom_dream", DesignRoot + "/levels/lv01_bedroom_dream/concept/bedroom_dream_map_concept.png", "0b75221152be925f2e0a4dab91b12dd5652d7f86e58b8d38cbf2f3185582c577"),
                Lock("bedroom_foreground_sprites", "bedroom_dream", DesignRoot + "/levels/lv01_bedroom_dream/sprites/bedroom_dream_foreground_sprites.png", "6b540c43525151e18e1cd5b2ecff9dfd7f172099c86377fb72a5ead0ff3973e7"),
                Lock("bedroom_mid_background_sprites", "bedroom_dream", DesignRoot + "/levels/lv01_bedroom_dream/sprites/bedroom_dream_mid_background_sprites.png", "0fca2de446eb4a6e2195bc2706930097d297e73bb57e634e33067b627cc887b9")
            };
        }

        public static P0HardReferenceSourceLockReport EvaluateP0Locks()
        {
            return Evaluate(CreateP0Locks(), P0AssetManifestCatalog.CreateP0PlannedManifest(), File.Exists, DefaultHashFile);
        }

        public static P0HardReferenceSourceLockReport Evaluate(
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks,
            Func<string, bool> fileExists,
            P0HardReferenceHashReader hashReader)
        {
            return Evaluate(locks, null, fileExists, hashReader);
        }

        public static P0HardReferenceSourceLockReport Evaluate(
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks,
            IReadOnlyList<P0AssetManifestEntry> manifest,
            Func<string, bool> fileExists,
            P0HardReferenceHashReader hashReader)
        {
            P0HardReferenceSourceLockReport report = new P0HardReferenceSourceLockReport();
            Func<string, bool> exists = fileExists ?? File.Exists;
            P0HardReferenceHashReader readHash = hashReader ?? DefaultHashFile;
            int lockCount = locks == null ? 0 : locks.Count;
            int hashMatchedCount = 0;
            HashSet<string> lockIds = new HashSet<string>(StringComparer.Ordinal);
            bool allLockIdsAreUnique = true;
            bool allHashesMatched = true;

            if (locks == null)
            {
                report.AddIssue(P0HardReferenceSourceLockSeverity.Failure, "Hard reference source locks are missing.");
                report.SetCounts(0, 0);
                return report;
            }

            for (int i = 0; i < locks.Count; i++)
            {
                P0HardReferenceSourceLockEntry current = locks[i];
                if (!lockIds.Add(current.LockId))
                {
                    allLockIdsAreUnique = false;
                    report.AddIssue(P0HardReferenceSourceLockSeverity.Failure, current.LockId + " has a duplicate hard reference source lock.");
                }

                if (HashMatches(current, exists, readHash, report))
                {
                    hashMatchedCount++;
                }
                else
                {
                    allHashesMatched = false;
                }
            }

            Require(
                report,
                lockCount == 12 && allLockIdsAreUnique,
                "Hard reference source locks cover the 12 P0 source authority files.",
                "Hard reference source locks must cover exactly 12 unique P0 source authority files.");
            Require(
                report,
                HasGroupCount(locks, P0PrototypeCatalog.SaibanId, 1)
                    && HasGroupCount(locks, P0PrototypeCatalog.NephthysId, 1)
                    && HasGroupCount(locks, P0PrototypeCatalog.SuzuneId, 1),
                "Starter cat colored turnarounds are locked as hard source files.",
                "Starter cat colored turnaround hard reference locks are missing.");
            Require(
                report,
                HasGroupCount(locks, "black_mud_nightmare", 2) && HasGroupCount(locks, "cold_light_shadow", 2),
                "Core enemy hard references are locked for concept and animation files.",
                "Core enemy hard reference locks are missing Black Mud or Cold Light concept/animation files.");
            Require(
                report,
                HasGroupCount(locks, "call_tyrant", 2),
                "Call Tyrant hard references are locked for concept and animation files.",
                "Call Tyrant hard reference locks are missing concept or animation files.");
            Require(
                report,
                HasGroupCount(locks, "bedroom_dream", 3),
                "Bedroom Dream hard references are locked for map concept and sprite sheets.",
                "Bedroom Dream hard reference locks are missing map or sprite-sheet files.");
            Require(
                report,
                allHashesMatched,
                "P0 hard reference source files match locked SHA-256 hashes.",
                "One or more P0 hard reference source files changed or are missing.");

            EvaluateManifestSourceLockLinks(locks, manifest, report);
            report.SetCounts(lockCount, hashMatchedCount);
            return report;
        }

        private static P0HardReferenceSourceLockEntry Lock(string lockId, string groupId, string path, string sha256)
        {
            return new P0HardReferenceSourceLockEntry(lockId, groupId, path, sha256);
        }

        private static bool HashMatches(
            P0HardReferenceSourceLockEntry current,
            Func<string, bool> fileExists,
            P0HardReferenceHashReader hashReader,
            P0HardReferenceSourceLockReport report)
        {
            if (string.IsNullOrWhiteSpace(current.Path) || !fileExists(current.Path))
            {
                report.AddIssue(P0HardReferenceSourceLockSeverity.Failure, current.LockId + " source file is missing at " + current.Path + ".");
                return false;
            }

            if (!hashReader(current.Path, out string actualHash, out string error))
            {
                report.AddIssue(P0HardReferenceSourceLockSeverity.Failure, current.LockId + " source hash could not be read: " + error);
                return false;
            }

            string normalizedActual = (actualHash ?? string.Empty).Trim().ToLowerInvariant();
            if (normalizedActual == current.Sha256)
            {
                return true;
            }

            report.AddIssue(
                P0HardReferenceSourceLockSeverity.Failure,
                current.LockId + " source hash should be " + current.Sha256 + " but is " + normalizedActual + ".");
            return false;
        }

        private static bool HasGroupCount(IReadOnlyList<P0HardReferenceSourceLockEntry> locks, string groupId, int expectedCount)
        {
            if (locks == null)
            {
                return false;
            }

            int count = 0;
            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].GroupId == groupId)
                {
                    count++;
                }
            }

            return count == expectedCount;
        }

        private static void EvaluateManifestSourceLockLinks(
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks,
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0HardReferenceSourceLockReport report)
        {
            if (manifest == null)
            {
                return;
            }

            int linkedAssetCount = 0;
            bool linkedIdsResolve = true;

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                for (int lockIndex = 0; lockIndex < entry.SourceLockIds.Count; lockIndex++)
                {
                    if (FindLock(locks, entry.SourceLockIds[lockIndex]) == null)
                    {
                        linkedIdsResolve = false;
                        report.AddIssue(
                            P0HardReferenceSourceLockSeverity.Failure,
                            entry.AssetId + " references unresolved source lock " + entry.SourceLockIds[lockIndex] + ".");
                    }
                }

                if (entry.SourceLockIds.Count > 0)
                {
                    linkedAssetCount++;
                }
            }

            report.SetManifestLinkedAssetCount(linkedAssetCount);
            Require(
                report,
                linkedIdsResolve,
                "Manifest source-lock ids resolve to locked hard reference source files.",
                "One or more manifest source-lock ids do not resolve to locked source files.");
            Require(
                report,
                linkedAssetCount == ExpectedManifestLinkedAssetCount && ExpectedManifestSourceLocksPresent(manifest, report),
                "Manifest source-sensitive starter cat, enemy, Boss, and bedroom rows declare exact source-lock ids.",
                "Manifest source-sensitive rows are missing exact source-lock ids.");
        }

        private static bool ExpectedManifestSourceLocksPresent(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0HardReferenceSourceLockReport report)
        {
            bool allPresent = true;
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.SaibanId, "sprite", "saiban_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.NephthysId, "sprite", "nephthys_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.SuzuneId, "sprite", "suzune_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.SaibanId, "avatar_icon", "saiban_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.NephthysId, "avatar_icon", "nephthys_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.SuzuneId, "avatar_icon", "suzune_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, "saiban_turnaround_reference_atlas", "reference_atlas", "saiban_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, "nephthys_turnaround_reference_atlas", "reference_atlas", "nephthys_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, "suzune_turnaround_reference_atlas", "reference_atlas", "suzune_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, "saiban_bedline_skill_vfx", "vfx", "saiban_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, "nephthys_moonsand_skill_vfx", "vfx", "nephthys_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, "suzune_lullaby_skill_vfx", "vfx", "suzune_turnaround_colored", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.BlackMudNightmareId, "sprite", "black_mud_concept", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.ColdLightShadowId, "sprite", "cold_light_concept", report);
            allPresent &= RequireSubjectLock(manifest, P0PrototypeCatalog.CallTyrantId, "concept", "call_tyrant_concept", report);
            allPresent &= RequireSubjectLock(manifest, "bedroom_dream_battle", "background", "bedroom_map_concept", report);
            allPresent &= RequireSubjectLock(manifest, "bed", "sprite", "bedroom_map_concept", report);
            allPresent &= RequireSubjectLock(manifest, "litter_box", "sprite", "bedroom_mid_background_sprites", report);
            allPresent &= RequireSubjectLock(manifest, "feeder", "sprite", "bedroom_mid_background_sprites", report);
            allPresent &= RequireSubjectLock(manifest, "call_tyrant_warning", "vfx", "call_tyrant_animation", report);
            allPresent &= RequireSubjectLock(manifest, "black_mud_bed_claw", "vfx", "black_mud_animation", report);
            allPresent &= RequireSubjectLock(manifest, "cold_light_beam_warning", "vfx", "cold_light_animation", report);
            allPresent &= RequireSubjectLock(manifest, "call_tyrant_app_throw", "vfx", "call_tyrant_animation", report);
            allPresent &= RequireSubjectLock(manifest, "call_tyrant_summon_portal", "vfx", "call_tyrant_animation", report);
            allPresent &= RequireSubjectLock(manifest, "black_mud_move", "framesheet", "black_mud_animation", report);
            allPresent &= RequireSubjectLock(manifest, "cold_light_cast", "framesheet", "cold_light_animation", report);
            allPresent &= RequireSubjectLock(manifest, "call_tyrant_boss_pattern", "framesheet", "call_tyrant_animation", report);
            allPresent &= RequireSubjectLock(manifest, "boss_route_node", "icon", "call_tyrant_concept", report);
            return allPresent;
        }

        private static bool RequireSubjectLock(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType,
            string expectedLockId,
            P0HardReferenceSourceLockReport report)
        {
            P0AssetManifestEntry entry = FindSubject(manifest, subjectId, assetType);
            if (entry != null && Contains(entry.SourceLockIds, expectedLockId))
            {
                return true;
            }

            report.AddIssue(
                P0HardReferenceSourceLockSeverity.Failure,
                subjectId + " " + assetType + " must reference source lock " + expectedLockId + ".");
            return false;
        }

        private static P0AssetManifestEntry FindSubject(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType)
        {
            if (manifest == null)
            {
                return null;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                if (manifest[i].SubjectId == subjectId && manifest[i].AssetType == assetType)
                {
                    return manifest[i];
                }
            }

            return null;
        }

        private static P0HardReferenceSourceLockEntry? FindLock(
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks,
            string lockId)
        {
            if (locks == null)
            {
                return null;
            }

            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].LockId == lockId)
                {
                    return locks[i];
                }
            }

            return null;
        }

        private static bool Contains(IReadOnlyList<string> values, string expected)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool DefaultHashFile(string path, out string sha256, out string error)
        {
            sha256 = string.Empty;
            error = string.Empty;
            try
            {
                using (FileStream stream = File.OpenRead(path))
                using (SHA256 algorithm = SHA256.Create())
                {
                    sha256 = ToHex(algorithm.ComputeHash(stream));
                    return true;
                }
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static string ToHex(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static void Require(
            P0HardReferenceSourceLockReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0HardReferenceSourceLockSeverity.Failure, failureMessage);
        }
    }
}
