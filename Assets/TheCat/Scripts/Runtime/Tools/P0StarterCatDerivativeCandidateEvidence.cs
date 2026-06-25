using System;
using System.Collections.Generic;
using System.IO;

namespace TheCat.Tools
{
    public sealed class P0StarterCatDerivativeCandidateEvidenceReport
    {
        private readonly List<string> expectedFiles = new List<string>();
        private readonly List<string> existingFiles = new List<string>();
        private readonly List<string> missingFiles = new List<string>();

        public IReadOnlyList<string> ExpectedFiles => expectedFiles.AsReadOnly();

        public IReadOnlyList<string> ExistingFiles => existingFiles.AsReadOnly();

        public IReadOnlyList<string> MissingFiles => missingFiles.AsReadOnly();

        public string BatchDirectory { get; private set; } = string.Empty;

        public int ExpectedFileCount => expectedFiles.Count;

        public int ExistingFileCount => existingFiles.Count;

        public int MissingFileCount => missingFiles.Count;

        public int CandidatePngCount { get; private set; }

        public int ReviewNoteCount { get; private set; }

        public int ReviewSheetCount { get; private set; }

        public bool HasCandidateOutsideAssets { get; private set; }

        public int ReviewNoteConformanceSpecMentionCount { get; private set; }

        public int ReviewNoteFrontAnchorSectionCount { get; private set; }

        public int ReviewNoteSideAnchorSectionCount { get; private set; }

        public int ReviewNoteBackAnchorSectionCount { get; private set; }

        public int ReviewNotePaletteAnchorSectionCount { get; private set; }

        public int ReviewNotePropCostumeAnchorSectionCount { get; private set; }

        public int ReviewNoteProhibitedDriftSectionCount { get; private set; }

        public bool HasTurnaroundConformanceReviewNotes =>
            ReviewNoteConformanceSpecMentionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewNoteFrontAnchorSectionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewNoteSideAnchorSectionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewNoteBackAnchorSectionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewNotePaletteAnchorSectionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewNotePropCostumeAnchorSectionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewNoteProhibitedDriftSectionCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount;

        public bool IsReviewReady => ExpectedFileCount >= P0StarterCatDerivativeCandidateEvidence.ExpectedEvidenceFileCount
            && CandidatePngCount == P0StarterCatDerivativeCandidateEvidence.ExpectedCandidatePngCount
            && ReviewNoteCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewNoteCount
            && ReviewSheetCount == P0StarterCatDerivativeCandidateEvidence.ExpectedReviewSheetCount
            && MissingFileCount == 0
            && HasCandidateOutsideAssets
            && HasTurnaroundConformanceReviewNotes;

        public void SetBatchDirectory(string batchDirectory)
        {
            BatchDirectory = batchDirectory ?? string.Empty;
        }

        public void AddExpectedFile(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                expectedFiles.Add(path);
            }
        }

        public void AddExistingFile(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                existingFiles.Add(path);
            }
        }

        public void AddMissingFile(string path)
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                missingFiles.Add(path);
            }
        }

        public void SetCounts(int candidatePngCount, int reviewNoteCount, int reviewSheetCount, bool hasCandidateOutsideAssets)
        {
            CandidatePngCount = candidatePngCount;
            ReviewNoteCount = reviewNoteCount;
            ReviewSheetCount = reviewSheetCount;
            HasCandidateOutsideAssets = hasCandidateOutsideAssets;
        }

        public void SetReviewNoteConformanceCounts(
            int conformanceSpecMentionCount,
            int frontAnchorSectionCount,
            int sideAnchorSectionCount,
            int backAnchorSectionCount,
            int paletteAnchorSectionCount,
            int propCostumeAnchorSectionCount,
            int prohibitedDriftSectionCount)
        {
            ReviewNoteConformanceSpecMentionCount = conformanceSpecMentionCount;
            ReviewNoteFrontAnchorSectionCount = frontAnchorSectionCount;
            ReviewNoteSideAnchorSectionCount = sideAnchorSectionCount;
            ReviewNoteBackAnchorSectionCount = backAnchorSectionCount;
            ReviewNotePaletteAnchorSectionCount = paletteAnchorSectionCount;
            ReviewNotePropCostumeAnchorSectionCount = propCostumeAnchorSectionCount;
            ReviewNoteProhibitedDriftSectionCount = prohibitedDriftSectionCount;
        }

        public string BuildSummary()
        {
            return IsReviewReady
                ? "P0 starter cat derivative candidate evidence ready for review: " + ExistingFileCount + "/" + ExpectedFileCount + " file(s)."
                : "P0 starter cat derivative candidate evidence incomplete: " + ExistingFileCount + "/" + ExpectedFileCount + " file(s), " + MissingFileCount + " missing, conformance notes ready: " + (HasTurnaroundConformanceReviewNotes ? "yes" : "no") + ".";
        }
    }

    public static class P0StarterCatDerivativeCandidateEvidence
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedCandidatePngCount = 12;
        public const int ExpectedReviewNoteCount = 3;
        public const int ExpectedReviewSheetCount = 3;
        public const int ExpectedEvidenceFileCount = 20;
        public const string TurnaroundConformanceSpecPath = "design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md";

        public const string CandidateRoot = "design/development/asset_candidates/starter_cats";
        public const string BatchSlug = "batch_05_source_locked_derivatives_2026-06-14";
        public const string BatchDirectory = CandidateRoot + "/" + BatchSlug;

        public static P0StarterCatDerivativeCandidateEvidenceReport EvaluateBatch05()
        {
            return Evaluate(CreateBatch05ExpectedFiles(), DefaultFileExists, DefaultReadText);
        }

        public static P0StarterCatDerivativeCandidateEvidenceReport Evaluate(
            IReadOnlyList<string> expectedFiles,
            P0ScreenshotEvidenceFileExists fileExists)
        {
            return Evaluate(expectedFiles, fileExists, DefaultReadText);
        }

        public static P0StarterCatDerivativeCandidateEvidenceReport Evaluate(
            IReadOnlyList<string> expectedFiles,
            P0ScreenshotEvidenceFileExists fileExists,
            P0StarterCatReviewTextReader readText)
        {
            IReadOnlyList<string> expected = expectedFiles ?? Array.Empty<string>();
            P0ScreenshotEvidenceFileExists exists = fileExists ?? DefaultFileExists;
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            P0StarterCatDerivativeCandidateEvidenceReport report = new P0StarterCatDerivativeCandidateEvidenceReport();
            report.SetBatchDirectory(BatchDirectory);

            int candidatePngCount = 0;
            int reviewNoteCount = 0;
            int reviewSheetCount = 0;
            bool allCandidateFilesOutsideAssets = true;
            int conformanceSpecMentionCount = 0;
            int frontAnchorSectionCount = 0;
            int sideAnchorSectionCount = 0;
            int backAnchorSectionCount = 0;
            int paletteAnchorSectionCount = 0;
            int propCostumeAnchorSectionCount = 0;
            int prohibitedDriftSectionCount = 0;

            for (int i = 0; i < expected.Count; i++)
            {
                string path = expected[i] ?? string.Empty;
                report.AddExpectedFile(path);
                bool isReviewNote = path.EndsWith("_candidate_review.md", StringComparison.Ordinal);
                if (path.EndsWith(".png", StringComparison.Ordinal))
                {
                    if (path.Contains("_candidate_", StringComparison.Ordinal))
                    {
                        candidatePngCount++;
                    }

                    if (path.Contains("_review_sheet", StringComparison.Ordinal))
                    {
                        reviewSheetCount++;
                    }
                }

                if (isReviewNote)
                {
                    reviewNoteCount++;
                }

                if (path.Contains("_candidate_", StringComparison.Ordinal)
                    && path.StartsWith("Assets/", StringComparison.Ordinal))
                {
                    allCandidateFilesOutsideAssets = false;
                }

                bool existing = exists(path);
                if (existing)
                {
                    report.AddExistingFile(path);
                    if (isReviewNote && reader(path, out string text, out _))
                    {
                        CountReviewNoteConformanceSections(
                            text,
                            ref conformanceSpecMentionCount,
                            ref frontAnchorSectionCount,
                            ref sideAnchorSectionCount,
                            ref backAnchorSectionCount,
                            ref paletteAnchorSectionCount,
                            ref propCostumeAnchorSectionCount,
                            ref prohibitedDriftSectionCount);
                    }
                }
                else
                {
                    report.AddMissingFile(path);
                }
            }

            report.SetCounts(
                candidatePngCount,
                reviewNoteCount,
                reviewSheetCount,
                allCandidateFilesOutsideAssets);
            report.SetReviewNoteConformanceCounts(
                conformanceSpecMentionCount,
                frontAnchorSectionCount,
                sideAnchorSectionCount,
                backAnchorSectionCount,
                paletteAnchorSectionCount,
                propCostumeAnchorSectionCount,
                prohibitedDriftSectionCount);
            return report;
        }

        public static IReadOnlyList<string> CreateBatch05ExpectedFiles()
        {
            List<string> files = new List<string>
            {
                BatchDirectory + "/README.md",
                BatchDirectory + "/starter_cat_batch05_candidate_manifest.csv"
            };

            AppendCatFiles(files, "saiban");
            AppendCatFiles(files, "nephthys");
            AppendCatFiles(files, "suzune");
            return files.AsReadOnly();
        }

        private static void AppendCatFiles(List<string> files, string catId)
        {
            string directory = CandidateRoot + "/" + catId + "/" + BatchSlug;
            string prefix = directory + "/thecat_cat_" + catId;
            files.Add(prefix + "_combat_sprite_refinement_512_candidate_v001.png");
            files.Add(prefix + "_front_animation_keyframe_512_idle_center_candidate_v001.png");
            files.Add(prefix + "_hud_avatar_256_candidate_v001.png");
            files.Add(prefix + "_skill_icon_motif_128_candidate_v001.png");
            files.Add(prefix + "_batch05_source_locked_review_sheet.png");
            files.Add(directory + "/" + catId + "_batch05_source_locked_candidate_review.md");
        }

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (File.Exists(path))
            {
                return true;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return true;
                }

                current = current.Parent;
            }

            return false;
        }

        private static void CountReviewNoteConformanceSections(
            string text,
            ref int conformanceSpecMentionCount,
            ref int frontAnchorSectionCount,
            ref int sideAnchorSectionCount,
            ref int backAnchorSectionCount,
            ref int paletteAnchorSectionCount,
            ref int propCostumeAnchorSectionCount,
            ref int prohibitedDriftSectionCount)
        {
            if (ContainsText(text, TurnaroundConformanceSpecPath))
            {
                conformanceSpecMentionCount++;
            }

            if (ContainsText(text, "Front-view anchors"))
            {
                frontAnchorSectionCount++;
            }

            if (ContainsText(text, "Side-view anchors"))
            {
                sideAnchorSectionCount++;
            }

            if (ContainsText(text, "Back-view anchors"))
            {
                backAnchorSectionCount++;
            }

            if (ContainsText(text, "Palette anchors"))
            {
                paletteAnchorSectionCount++;
            }

            if (ContainsText(text, "Prop/costume anchors"))
            {
                propCostumeAnchorSectionCount++;
            }

            if (ContainsText(text, "Prohibited drift"))
            {
                prohibitedDriftSectionCount++;
            }
        }

        private static bool DefaultReadText(string path, out string text, out string error)
        {
            text = string.Empty;
            error = string.Empty;

            try
            {
                string resolved = ResolveFile(path);
                if (string.IsNullOrWhiteSpace(resolved) || !File.Exists(resolved))
                {
                    error = "file is missing";
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

        private static bool ContainsText(string text, string expected)
        {
            return text != null && text.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
