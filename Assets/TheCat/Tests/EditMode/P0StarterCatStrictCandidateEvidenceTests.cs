using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StarterCatStrictCandidateEvidenceTests
    {
        [Test]
        public void EvaluateCurrentCandidates_CurrentStrictCandidatesAreReady()
        {
            P0StarterCatStrictCandidateEvidenceReport report = P0StarterCatStrictCandidateEvidence.EvaluateCurrentCandidates();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.CandidateCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.ManifestCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.AlphaCandidateCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.ReviewSheetCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.ReviewNoteCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.AgentPromptCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.ExplicitBlockNoteCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.ActiveScreenshotMentionCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.SourceTurnaroundExactPathCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.Batch47SpecManifestLockCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount, report.Batch47SpecJsonIdentityLockCount);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("strict candidate evidence ready", report.BuildSummary());
            StringAssert.Contains("exact colored-turnaround source paths", report.BuildDetailedSummary());
            StringAssert.Contains("Batch 47 JSON specs keep exact source-lock", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingAlphaCandidateFailsReadiness()
        {
            IReadOnlyList<P0StarterCatStrictCandidateEntry> candidates = P0StarterCatStrictCandidateEvidence.CreateP0Candidates();
            string missingPath = candidates[0].AlphaCandidatePath;

            P0StarterCatStrictCandidateEvidenceReport report = P0StarterCatStrictCandidateEvidence.Evaluate(
                candidates,
                path => path != missingPath && File.Exists(ResolveFile(path)),
                ReadText);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount - 1, report.AlphaCandidateCount);
            StringAssert.Contains("Missing strict candidate alpha candidate", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MetaFileBesideCandidateFailsReadiness()
        {
            IReadOnlyList<P0StarterCatStrictCandidateEntry> candidates = P0StarterCatStrictCandidateEvidence.CreateP0Candidates();
            string metaPath = candidates[0].AlphaCandidatePath + ".meta";

            P0StarterCatStrictCandidateEvidenceReport report = P0StarterCatStrictCandidateEvidence.Evaluate(
                candidates,
                path => path == metaPath || File.Exists(ResolveFile(path)),
                ReadText);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("strict candidate has Unity .meta files", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MojibakeManifestSourcePathFailsIdentityGate()
        {
            IReadOnlyList<P0StarterCatStrictCandidateEntry> candidates = P0StarterCatStrictCandidateEvidence.CreateP0Candidates();
            string manifestPath = candidates[0].ManifestPath;
            string saibanSourcePath = FindSourcePath(P0PrototypeCatalog.SaibanId);

            P0StarterCatStrictCandidateEvidenceReport report = P0StarterCatStrictCandidateEvidence.Evaluate(
                candidates,
                path => File.Exists(ResolveFile(path)),
                (string path, out string text, out string error) =>
                {
                    bool result = ReadText(path, out text, out error);
                    if (result && path == manifestPath)
                    {
                        text = text.Replace(saibanSourcePath, "design/?assets/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png");
                    }

                    return result;
                });

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(P0StarterCatStrictCandidateEvidence.ExpectedStarterCatCount - 1, report.SourceTurnaroundExactPathCount);
            StringAssert.Contains("exact colored-turnaround source locks", report.BuildDetailedSummary());
        }

        private static bool ReadText(string path, out string text, out string error)
        {
            text = string.Empty;
            error = string.Empty;

            try
            {
                string resolved = ResolveFile(path);
                if (!File.Exists(resolved))
                {
                    error = "missing";
                    return false;
                }

                text = File.ReadAllText(resolved);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static string ResolveFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            if (File.Exists(path))
            {
                return path;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return path;
        }

        private static string FindSourcePath(string catId)
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].CatId == catId)
                {
                    return locks[i].SourceTurnaroundPath;
                }
            }

            return string.Empty;
        }
    }
}
