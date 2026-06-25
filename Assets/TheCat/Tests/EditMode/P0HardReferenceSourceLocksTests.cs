using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0HardReferenceSourceLocksTests
    {
        [Test]
        public void EvaluateP0Locks_CurrentHardReferenceSourcesMatchLockedHashes()
        {
            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.EvaluateP0Locks();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0HardReferenceSourceLocks.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(12, report.LockCount);
            Assert.AreEqual(12, report.HashMatchedCount);
            Assert.AreEqual(P0HardReferenceSourceLocks.ExpectedManifestLinkedAssetCount, report.ManifestLinkedAssetCount);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("hard reference source locks ready", report.BuildSummary());
            StringAssert.Contains("Starter cat colored turnarounds", report.BuildDetailedSummary());
            StringAssert.Contains("Core enemy hard references", report.BuildDetailedSummary());
            StringAssert.Contains("Bedroom Dream hard references", report.BuildDetailedSummary());
            StringAssert.Contains("Manifest source-lock ids", report.BuildDetailedSummary());
            StringAssert.Contains("source-sensitive starter cat", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingReferenceFileFails()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            P0HardReferenceSourceLockEntry missing = locks[0];

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                locks,
                path => path != missing.Path,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("source file is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ChangedReferenceHashFails()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            P0HardReferenceSourceLockEntry changed = locks[4];

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                locks,
                path => true,
                (string path, out string sha256, out string error) =>
                {
                    error = string.Empty;
                    if (path == changed.Path)
                    {
                        sha256 = "ffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff";
                        return true;
                    }

                    return HashFromLocks(locks)(path, out sha256, out error);
                });

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("source hash should be", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_DuplicateLockIdFails()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            var duplicateLocks = new[]
            {
                locks[0],
                locks[0],
                locks[1],
                locks[2],
                locks[3],
                locks[4],
                locks[5],
                locks[6],
                locks[7],
                locks[8],
                locks[9],
                locks[10],
                locks[11]
            };

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                duplicateLocks,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("duplicate hard reference source lock", report.BuildDetailedSummary());
            StringAssert.Contains("exactly 12 unique", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingBedroomReferenceGroupFails()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            List<P0HardReferenceSourceLockEntry> missingBedroom = new List<P0HardReferenceSourceLockEntry>();
            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].GroupId != "bedroom_dream")
                {
                    missingBedroom.Add(locks[i]);
                }
            }

            while (missingBedroom.Count < locks.Count)
            {
                missingBedroom.Add(locks[0]);
            }

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                missingBedroom,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("Bedroom Dream hard reference locks are missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ManifestSourceLockIdsResolveToLockedReferences()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            IReadOnlyList<P0AssetManifestEntry> manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                locks,
                manifest,
                path => true,
                HashFromLocks(locks));

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0HardReferenceSourceLocks.ExpectedManifestLinkedAssetCount, report.ManifestLinkedAssetCount);
            StringAssert.Contains("Manifest source-lock ids resolve", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_UnresolvedManifestSourceLockIdFails()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            IReadOnlyList<P0AssetManifestEntry> manifest = MutateManifest(
                "bed",
                "sprite",
                entry => WithSourceLocks(entry, "missing_bedroom_lock"));

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                locks,
                manifest,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("references unresolved source lock missing_bedroom_lock", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ColdLightMustReferenceColdLightSourceLock()
        {
            IReadOnlyList<P0HardReferenceSourceLockEntry> locks = P0HardReferenceSourceLocks.CreateP0Locks();
            IReadOnlyList<P0AssetManifestEntry> manifest = MutateManifest(
                P0PrototypeCatalog.ColdLightShadowId,
                "sprite",
                entry => WithSourceLocks(entry, "black_mud_concept"));

            P0HardReferenceSourceLockReport report = P0HardReferenceSourceLocks.Evaluate(
                locks,
                manifest,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("cold_light_shadow sprite must reference source lock cold_light_concept", report.BuildDetailedSummary());
        }

        private static IReadOnlyList<P0AssetManifestEntry> MutateManifest(
            string subjectId,
            string assetType,
            System.Func<P0AssetManifestEntry, P0AssetManifestEntry> mutate)
        {
            List<P0AssetManifestEntry> result = new List<P0AssetManifestEntry>();
            foreach (P0AssetManifestEntry entry in P0AssetManifestCatalog.CreateP0PlannedManifest())
            {
                result.Add(entry.SubjectId == subjectId && entry.AssetType == assetType
                    ? mutate(entry)
                    : entry);
            }

            return result;
        }

        private static P0AssetManifestEntry WithSourceLocks(P0AssetManifestEntry entry, params string[] sourceLockIds)
        {
            return new P0AssetManifestEntry(
                entry.AssetId,
                entry.SubjectId,
                entry.AssetType,
                entry.Priority,
                entry.SourcePromptPath,
                entry.ReferenceAssetIds,
                sourceLockIds,
                entry.UnityImportPath,
                entry.Size,
                entry.Status,
                entry.ConsistencyNotes);
        }

        private static P0HardReferenceHashReader HashFromLocks(IReadOnlyList<P0HardReferenceSourceLockEntry> locks)
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            for (int i = 0; i < locks.Count; i++)
            {
                hashes[locks[i].Path] = locks[i].Sha256;
            }

            return (string path, out string sha256, out string error) =>
            {
                if (hashes.TryGetValue(path, out sha256))
                {
                    error = string.Empty;
                    return true;
                }

                sha256 = string.Empty;
                error = "No test hash for " + path;
                return false;
            };
        }
    }
}
