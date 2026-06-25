using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StarterCatRuntimeCombatSpriteAuditEvidenceTests
    {
        [Test]
        public void EvaluateCurrentAudit_PassesCurrentBatch74Packet()
        {
            P0StarterCatRuntimeCombatSpriteAuditReport report =
                P0StarterCatRuntimeCombatSpriteAuditEvidence.EvaluateCurrentAudit();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount, report.ArtifactCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount, report.ManifestRowCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount, report.RuntimeSpriteCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount, report.RuntimeSpriteMetaCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount, report.SourceLockMentionCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount, report.SourceTurnaroundPathMentionCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount, report.FrontPlateMentionCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeBindingMentionCount, report.RuntimeBindingMentionCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
        }

        [Test]
        public void CreateP0Rows_ReusesCanonicalStarterCatSourceLocks()
        {
            IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> rows =
                P0StarterCatRuntimeCombatSpriteAuditEvidence.CreateP0Rows();
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks =
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount, rows.Count);
            for (int i = 0; i < rows.Count; i++)
            {
                P0StarterCatRuntimeCombatSpriteAuditEntry row = rows[i];
                P0StarterCatTurnaroundSourceLockEntry sourceLock = FindSourceLock(locks, row.CatId);

                Assert.AreEqual(sourceLock.AssetId, row.AssetId);
                Assert.AreEqual(sourceLock.SourceTurnaroundPath, row.SourceTurnaroundPath);
                Assert.AreEqual(sourceLock.SpritePath, row.RuntimeSpritePath);
                StringAssert.Contains("design/梦境支配者核心玩法/assets", row.SourceTurnaroundPath);
                Assert.IsFalse(ContainsMojibakePath(row.SourceTurnaroundPath), row.SourceTurnaroundPath);
            }
        }

        [Test]
        public void Evaluate_MissingSourceTurnaroundPathMentionFails()
        {
            IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> rows =
                P0StarterCatRuntimeCombatSpriteAuditEvidence.CreateP0Rows();
            string combinedText = BuildPacketText(rows, rows[0].SourceTurnaroundPath);

            P0StarterCatRuntimeCombatSpriteAuditReport report =
                P0StarterCatRuntimeCombatSpriteAuditEvidence.Evaluate(
                    rows,
                    _ => true,
                    (string path, out string text, out string error) =>
                    {
                        text = path == P0StarterCatRuntimeCombatSpriteAuditEvidence.ReviewSheetPath
                            ? string.Empty
                            : combinedText;
                        error = string.Empty;
                        return true;
                    });

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("source turnaround path mentions are incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingRuntimeSpriteFails()
        {
            IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> rows =
                P0StarterCatRuntimeCombatSpriteAuditEvidence.CreateP0Rows();
            string missingSpritePath = rows[0].RuntimeSpritePath;

            P0StarterCatRuntimeCombatSpriteAuditReport report =
                P0StarterCatRuntimeCombatSpriteAuditEvidence.Evaluate(
                    rows,
                    path => path != missingSpritePath,
                    (string path, out string text, out string error) =>
                    {
                        text = path == P0StarterCatRuntimeCombatSpriteAuditEvidence.ReviewSheetPath
                            ? string.Empty
                            : BuildPacketText(rows, string.Empty);
                        error = string.Empty;
                        return true;
                    });

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("runtime combat sprite is missing", report.BuildDetailedSummary());
        }

        private static string BuildPacketText(
            IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> rows,
            string omittedSourceTurnaroundPath)
        {
            List<string> lines = new List<string>
            {
                P0StarterCatRuntimeCombatSpriteAuditEvidence.Recommendation,
                "does not generate new cat body art",
                "does not replace any Unity sprite",
                "does not approve AI-generated"
            };

            for (int i = 0; i < rows.Count; i++)
            {
                P0StarterCatRuntimeCombatSpriteAuditEntry row = rows[i];
                lines.Add(row.CatId);
                lines.Add(row.AssetId);
                lines.Add(row.RuntimeSpritePath);
                lines.Add(row.RuntimeBindingId);
                lines.Add(row.VisualCatalogConstant);
                lines.Add(row.SourceLockId);
                lines.Add(row.FrontReferencePlatePath);
                if (row.SourceTurnaroundPath != omittedSourceTurnaroundPath)
                {
                    lines.Add(row.SourceTurnaroundPath);
                }
            }

            return string.Join("\n", lines);
        }

        private static P0StarterCatTurnaroundSourceLockEntry FindSourceLock(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            string catId)
        {
            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].CatId == catId)
                {
                    return locks[i];
                }
            }

            Assert.Fail("Missing starter cat source lock for " + catId);
            return default(P0StarterCatTurnaroundSourceLockEntry);
        }

        private static bool ContainsMojibakePath(string text)
        {
            return text.Contains("姊")
                || text.Contains("鏀")
                || text.Contains("?assets");
        }
    }
}
