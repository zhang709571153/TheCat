using System;
using System.Collections.Generic;
using System.IO;
using TheCat.Data.Catalogs;

namespace TheCat.Tools
{
    public enum P0StarterCatSourceLockPacketSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatSourceLockPacketIssue
    {
        public P0StarterCatSourceLockPacketIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatSourceLockPacketSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatSourceLockPacketReport
    {
        private readonly List<P0StarterCatSourceLockPacketIssue> issues = new List<P0StarterCatSourceLockPacketIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatSourceLockPacketIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public bool PacketMarkdownPresent { get; private set; }

        public bool PacketCsvPresent { get; private set; }

        public int CatEntryCount { get; private set; }

        public int SourceHashMentionCount { get; private set; }

        public int SpriteHashMentionCount { get; private set; }

        public int ActiveScreenshotMentionCount { get; private set; }

        public int CandidateReviewSheetMentionCount { get; private set; }

        public int CoreDocumentCount { get; private set; }

        public int CoreDocumentSourcePathMentionCount { get; private set; }

        public int CoreDocumentImportBlockMentionCount { get; private set; }

        public int CoreDocumentMojibakeMentionCount { get; private set; }

        public int FailureCount => Count(P0StarterCatSourceLockPacketSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatSourceLockPacketEvidence.ExpectedCoveredCheckCount;

        public void SetCounts(
            bool packetMarkdownPresent,
            bool packetCsvPresent,
            int catEntryCount,
            int sourceHashMentionCount,
            int spriteHashMentionCount,
            int activeScreenshotMentionCount,
            int candidateReviewSheetMentionCount,
            int coreDocumentCount,
            int coreDocumentSourcePathMentionCount,
            int coreDocumentImportBlockMentionCount,
            int coreDocumentMojibakeMentionCount)
        {
            PacketMarkdownPresent = packetMarkdownPresent;
            PacketCsvPresent = packetCsvPresent;
            CatEntryCount = catEntryCount;
            SourceHashMentionCount = sourceHashMentionCount;
            SpriteHashMentionCount = spriteHashMentionCount;
            ActiveScreenshotMentionCount = activeScreenshotMentionCount;
            CandidateReviewSheetMentionCount = candidateReviewSheetMentionCount;
            CoreDocumentCount = coreDocumentCount;
            CoreDocumentSourcePathMentionCount = coreDocumentSourcePathMentionCount;
            CoreDocumentImportBlockMentionCount = coreDocumentImportBlockMentionCount;
            CoreDocumentMojibakeMentionCount = coreDocumentMojibakeMentionCount;
        }

        public void AddIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            issues.Add(new P0StarterCatSourceLockPacketIssue(severity, message));
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
                ? "P0 starter cat source-lock packet ready for " + CatEntryCount + " starter cat(s)."
                : "P0 starter cat source-lock packet has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Markdown packet present: " + (PacketMarkdownPresent ? "yes" : "no"),
                "CSV packet present: " + (PacketCsvPresent ? "yes" : "no"),
                "Cat entries: " + CatEntryCount,
                "Source hash mentions: " + SourceHashMentionCount,
                "Sprite hash mentions: " + SpriteHashMentionCount,
                "Active screenshot mentions: " + ActiveScreenshotMentionCount,
                "Candidate review sheet mentions: " + CandidateReviewSheetMentionCount,
                "Core source-lock documents: " + CoreDocumentCount,
                "Core source-lock documents with exact source paths: " + CoreDocumentSourcePathMentionCount,
                "Core source-lock documents with import block: " + CoreDocumentImportBlockMentionCount,
                "Core source-lock document mojibake mentions: " + CoreDocumentMojibakeMentionCount
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

        private int Count(P0StarterCatSourceLockPacketSeverity severity)
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

    public static class P0StarterCatSourceLockPacketEvidence
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedCoreDocumentCount = 3;
        public const int ExpectedCoveredCheckCount = 11;
        public const string PacketMarkdownPath = "design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.md";
        public const string PacketCsvPath = "design/development/asset_review/p0_starter_cat_source_lock_packet_2026-06-14.csv";
        public const string TurnaroundConformanceSpecPath = "design/development/asset_review/p0_starter_cat_turnaround_conformance_spec_2026-06-14.md";
        public const string StrictReferencePackPath = "design/development/asset_review/p0_starter_cat_strict_reference_pack_2026-06-14.md";

        public static readonly string[] CoreDocumentPaths =
        {
            PacketMarkdownPath,
            TurnaroundConformanceSpecPath,
            StrictReferencePackPath
        };

        public static P0StarterCatSourceLockPacketReport EvaluateCurrentPacket()
        {
            return Evaluate(
                PacketMarkdownPath,
                PacketCsvPath,
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                DefaultReadText,
                DefaultFileExists);
        }

        public static P0StarterCatSourceLockPacketReport Evaluate(
            string markdownPath,
            string csvPath,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            P0StarterCatReviewTextReader readText,
            Func<string, bool> fileExists)
        {
            P0StarterCatSourceLockPacketReport report = new P0StarterCatSourceLockPacketReport();
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> expectedLocks = locks ?? Array.Empty<P0StarterCatTurnaroundSourceLockEntry>();

            bool markdownPresent = exists(markdownPath);
            bool csvPresent = exists(csvPath);
            string markdownText = string.Empty;
            string csvText = string.Empty;

            if (markdownPresent && !reader(markdownPath, out markdownText, out string markdownError))
            {
                markdownPresent = false;
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Could not read starter cat source-lock markdown packet: " + markdownError);
            }

            if (csvPresent && !reader(csvPath, out csvText, out string csvError))
            {
                csvPresent = false;
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Could not read starter cat source-lock CSV packet: " + csvError);
            }

            int catEntryCount = 0;
            int sourceHashMentionCount = 0;
            int spriteHashMentionCount = 0;
            int activeScreenshotMentionCount = 0;
            int candidateReviewSheetMentionCount = 0;
            bool allExpectedCatsPresent = expectedLocks.Count == ExpectedStarterCatCount;
            bool allSourceLocksPresent = true;
            bool allSpriteLocksPresent = true;
            bool allScreenshotsPresent = true;
            bool allCandidateReviewSheetsPresent = true;
            bool allDriftBlockersPresent = true;
            int coreDocumentCount = 0;
            int coreDocumentSourcePathMentionCount = 0;
            int coreDocumentImportBlockMentionCount = 0;
            int coreDocumentMojibakeMentionCount = 0;
            bool allCoreDocumentsPresent = true;
            bool allCoreDocumentsPinSourcePaths = true;
            bool allCoreDocumentsBlockImport = true;
            bool noCoreDocumentMojibake = true;

            for (int i = 0; i < expectedLocks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry entry = expectedLocks[i];
                string combined = markdownText + "\n" + csvText;
                string sourceLockId = entry.CatId + "_turnaround_colored";
                if (ContainsText(combined, entry.CatId) && ContainsText(combined, sourceLockId))
                {
                    catEntryCount++;
                }
                else
                {
                    allExpectedCatsPresent = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, entry.CatId + " source-lock packet row is missing.");
                }

                if (ContainsText(combined, entry.SourceTurnaroundPath) && ContainsText(combined, entry.SourceTurnaroundSha256))
                {
                    sourceHashMentionCount++;
                }
                else
                {
                    allSourceLocksPresent = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, entry.CatId + " source turnaround path or hash is missing from the source-lock packet.");
                }

                if (ContainsText(combined, entry.SpritePath) && ContainsText(combined, entry.SpriteSha256))
                {
                    spriteHashMentionCount++;
                }
                else
                {
                    allSpriteLocksPresent = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, entry.CatId + " locked Unity sprite path or hash is missing from the source-lock packet.");
                }

                if (ContainsText(combined, ActiveScreenshotFor(entry.CatId)))
                {
                    activeScreenshotMentionCount++;
                }
                else
                {
                    allScreenshotsPresent = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, entry.CatId + " active-cat screenshot filename is missing from the source-lock packet.");
                }

                if (ContainsText(combined, CandidateReviewSheetFor(entry.CatId)))
                {
                    candidateReviewSheetMentionCount++;
                }
                else
                {
                    allCandidateReviewSheetsPresent = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, entry.CatId + " candidate review sheet is missing from the source-lock packet.");
                }

                if (!ContainsText(combined, "colored turnaround")
                    || !ContainsText(combined, "human")
                    || !ContainsText(combined, "palette")
                    || !ContainsText(combined, "do not import into Unity yet"))
                {
                    allDriftBlockersPresent = false;
                }
            }

            for (int i = 0; i < CoreDocumentPaths.Length; i++)
            {
                string path = CoreDocumentPaths[i];
                bool isPacketMarkdown = path == markdownPath;
                bool present = isPacketMarkdown ? markdownPresent : exists(path);
                string text = isPacketMarkdown ? markdownText : string.Empty;

                if (present && !isPacketMarkdown && !reader(path, out text, out string error))
                {
                    present = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Could not read starter cat core source-lock document: " + path + " (" + error + ")");
                }

                if (!present)
                {
                    allCoreDocumentsPresent = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Starter cat core source-lock document is missing: " + path);
                    continue;
                }

                coreDocumentCount++;

                if (ContainsAllSourceTurnaroundPaths(text, expectedLocks))
                {
                    coreDocumentSourcePathMentionCount++;
                }
                else
                {
                    allCoreDocumentsPinSourcePaths = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, path + " must mention all three exact colored-turnaround source paths.");
                }

                if (ContainsFormalImportBlock(text))
                {
                    coreDocumentImportBlockMentionCount++;
                }
                else
                {
                    allCoreDocumentsBlockImport = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, path + " must keep formal starter-cat Unity import blocked.");
                }

                if (ContainsMojibakePath(text))
                {
                    coreDocumentMojibakeMentionCount++;
                    noCoreDocumentMojibake = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, path + " contains mojibake design path text.");
                }
            }

            report.SetCounts(
                markdownPresent,
                csvPresent,
                catEntryCount,
                sourceHashMentionCount,
                spriteHashMentionCount,
                activeScreenshotMentionCount,
                candidateReviewSheetMentionCount,
                coreDocumentCount,
                coreDocumentSourcePathMentionCount,
                coreDocumentImportBlockMentionCount,
                coreDocumentMojibakeMentionCount);

            Require(report, markdownPresent, "Starter cat source-lock markdown packet exists.", "Starter cat source-lock markdown packet is missing.");
            Require(report, csvPresent, "Starter cat source-lock CSV packet exists.", "Starter cat source-lock CSV packet is missing.");
            Require(report, allExpectedCatsPresent && catEntryCount == ExpectedStarterCatCount, "Starter cat source-lock packet covers exactly Saiban, Nephthys, and Suzune.", "Starter cat source-lock packet does not cover exactly the three starter cats.");
            Require(report, allSourceLocksPresent && sourceHashMentionCount == ExpectedStarterCatCount, "Starter cat source-lock packet records colored turnaround paths and SHA-256 hashes.", "Starter cat source-lock packet is missing colored turnaround hashes.");
            Require(report, allSpriteLocksPresent && spriteHashMentionCount == ExpectedStarterCatCount, "Starter cat source-lock packet records locked Unity sprite paths and SHA-256 hashes.", "Starter cat source-lock packet is missing locked sprite hashes.");
            Require(report, allScreenshotsPresent && activeScreenshotMentionCount == ExpectedStarterCatCount, "Starter cat source-lock packet names all active-cat Play Mode screenshot targets.", "Starter cat source-lock packet is missing active-cat screenshot targets.");
            Require(report, allCandidateReviewSheetsPresent && candidateReviewSheetMentionCount == ExpectedStarterCatCount, "Starter cat source-lock packet links all Batch 05 candidate review sheets.", "Starter cat source-lock packet is missing Batch 05 review sheet links.");
            Require(report, allDriftBlockersPresent, "Starter cat source-lock packet keeps formal import blocked and names colored-turnaround, human-proportion, and palette drift blockers.", "Starter cat source-lock packet is missing formal import blockers.");
            Require(report, allCoreDocumentsPresent && coreDocumentCount == ExpectedCoreDocumentCount, "Starter cat core source-lock documents are present.", "Starter cat core source-lock documents are missing.");
            Require(report, allCoreDocumentsPinSourcePaths && coreDocumentSourcePathMentionCount == ExpectedCoreDocumentCount, "Starter cat core source-lock documents repeat the exact colored-turnaround source paths.", "Starter cat core source-lock documents do not repeat the exact source paths.");
            Require(report, allCoreDocumentsBlockImport && coreDocumentImportBlockMentionCount == ExpectedCoreDocumentCount && noCoreDocumentMojibake && coreDocumentMojibakeMentionCount == 0, "Starter cat core source-lock documents block import and contain no mojibake design paths.", "Starter cat core source-lock documents contain mojibake paths or weak import blockers.");
            return report;
        }

        public static string ActiveScreenshotFor(string catId)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName;
                case P0PrototypeCatalog.NephthysId:
                    return P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName;
                case P0PrototypeCatalog.SuzuneId:
                    return P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName;
                default:
                    return string.Empty;
            }
        }

        public static string CandidateReviewSheetFor(string catId)
        {
            string directory = P0StarterCatDerivativeCandidateEvidence.CandidateRoot
                + "/" + catId
                + "/" + P0StarterCatDerivativeCandidateEvidence.BatchSlug;
            return directory + "/thecat_cat_" + catId + "_batch05_source_locked_review_sheet.png";
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

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return File.Exists(ResolveFile(path));
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

        private static bool ContainsText(string value, string token)
        {
            return value != null && token != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool ContainsAllSourceTurnaroundPaths(string text, IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            if (locks == null || locks.Count != ExpectedStarterCatCount)
            {
                return false;
            }

            for (int i = 0; i < locks.Count; i++)
            {
                if (!ContainsText(text, locks[i].SourceTurnaroundPath))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ContainsFormalImportBlock(string text)
        {
            return ContainsText(text, "do not import into Unity yet")
                || (ContainsText(text, "formal") && ContainsText(text, "import") && ContainsText(text, "blocked"));
        }

        private static bool ContainsMojibakePath(string text)
        {
            return ContainsText(text, "design/姊")
                || ContainsText(text, "design/濮")
                || ContainsText(text, "design/韫")
                || ContainsText(text, "design/閺")
                || ContainsText(text, "鏀")
                || ContainsText(text, "蹇")
                || ContainsText(text, "娉?");
        }

        private static void Require(P0StarterCatSourceLockPacketReport report, bool condition, string coveredCheck, string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, failureMessage);
        }
    }

    public readonly struct P0StarterCatTurnaroundComparisonAuditIssue
    {
        public P0StarterCatTurnaroundComparisonAuditIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatSourceLockPacketSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatTurnaroundComparisonAuditReport
    {
        private readonly List<P0StarterCatTurnaroundComparisonAuditIssue> issues = new List<P0StarterCatTurnaroundComparisonAuditIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatTurnaroundComparisonAuditIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int ArtifactCount { get; private set; }

        public int ManifestRowCount { get; private set; }

        public int SourceLockMentionCount { get; private set; }

        public int SourcePathMentionCount { get; private set; }

        public int SpritePathMentionCount { get; private set; }

        public int ActiveScreenshotMentionCount { get; private set; }

        public int RecommendationMentionCount { get; private set; }

        public int MetaFileCount { get; private set; }

        public int FailureCount => Count(P0StarterCatSourceLockPacketSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedCoveredCheckCount;

        public void SetCounts(
            int artifactCount,
            int manifestRowCount,
            int sourceLockMentionCount,
            int sourcePathMentionCount,
            int spritePathMentionCount,
            int activeScreenshotMentionCount,
            int recommendationMentionCount,
            int metaFileCount)
        {
            ArtifactCount = artifactCount;
            ManifestRowCount = manifestRowCount;
            SourceLockMentionCount = sourceLockMentionCount;
            SourcePathMentionCount = sourcePathMentionCount;
            SpritePathMentionCount = spritePathMentionCount;
            ActiveScreenshotMentionCount = activeScreenshotMentionCount;
            RecommendationMentionCount = recommendationMentionCount;
            MetaFileCount = metaFileCount;
        }

        public void AddIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            issues.Add(new P0StarterCatTurnaroundComparisonAuditIssue(severity, message));
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
                ? "P0 starter cat turnaround comparison audit ready for " + ManifestRowCount + " starter cat(s)."
                : "P0 starter cat turnaround comparison audit has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Artifacts: " + ArtifactCount,
                "Manifest rows: " + ManifestRowCount,
                "Source lock mentions: " + SourceLockMentionCount,
                "Source path mentions: " + SourcePathMentionCount,
                "Sprite path mentions: " + SpritePathMentionCount,
                "Active screenshot mentions: " + ActiveScreenshotMentionCount,
                "Recommendation mentions: " + RecommendationMentionCount,
                "Meta files: " + MetaFileCount
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

        private int Count(P0StarterCatSourceLockPacketSeverity severity)
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

    public static class P0StarterCatTurnaroundComparisonAuditEvidence
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedArtifactCount = 4;
        public const int ExpectedCoveredCheckCount = 9;
        public const string BatchDirectory = "design/development/asset_candidates/starter_cats/batch_69_turnaround_runtime_comparison_audit_2026-06-15";
        public const string ManifestPath = BatchDirectory + "/starter_cat_turnaround_runtime_comparison_batch69_manifest.csv";
        public const string ReviewSheetPath = BatchDirectory + "/thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png";
        public const string ReviewNotePath = BatchDirectory + "/starter_cat_turnaround_runtime_comparison_batch69_review.md";
        public const string ProcessNotePath = BatchDirectory + "/starter_cat_turnaround_runtime_comparison_batch69_process_note.md";
        private const string RequiredRecommendation = "audit_only_no_import_pending_active_cat_playmode_screenshot";

        public static P0StarterCatTurnaroundComparisonAuditReport EvaluateCurrentAudit()
        {
            return Evaluate(
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                DefaultReadText,
                DefaultFileExists);
        }

        public static P0StarterCatTurnaroundComparisonAuditReport Evaluate(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            P0StarterCatReviewTextReader readText,
            Func<string, bool> fileExists)
        {
            P0StarterCatTurnaroundComparisonAuditReport report = new P0StarterCatTurnaroundComparisonAuditReport();
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> expectedLocks = locks ?? Array.Empty<P0StarterCatTurnaroundSourceLockEntry>();
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            Func<string, bool> exists = fileExists ?? DefaultFileExists;

            string manifestText = ReadOptional(ManifestPath, reader, exists, report);
            string reviewText = ReadOptional(ReviewNotePath, reader, exists, report);
            string processText = ReadOptional(ProcessNotePath, reader, exists, report);
            string combined = manifestText + "\n" + reviewText + "\n" + processText;

            int artifactCount = CountArtifacts(exists);
            int manifestRowCount = CountManifestRows(manifestText);
            int sourceLockMentionCount = 0;
            int sourcePathMentionCount = 0;
            int spritePathMentionCount = 0;
            int activeScreenshotMentionCount = 0;
            int recommendationMentionCount = 0;
            int metaFileCount = CountMetaFiles(exists);
            bool allLocksMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allSourcePathsMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allSpritePathsMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allScreenshotsMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allRecommendationsMentioned = expectedLocks.Count == ExpectedStarterCatCount;

            for (int i = 0; i < expectedLocks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = expectedLocks[i];
                string sourceLockId = current.CatId + "_turnaround_colored";
                if (ContainsText(combined, sourceLockId))
                {
                    sourceLockMentionCount++;
                }
                else
                {
                    allLocksMentioned = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + " source lock is missing from Batch 69 audit.");
                }

                if (ContainsText(combined, current.SourceTurnaroundPath))
                {
                    sourcePathMentionCount++;
                }
                else
                {
                    allSourcePathsMentioned = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + " exact colored-turnaround source path is missing from Batch 69 audit.");
                }

                if (ContainsText(combined, current.SpritePath))
                {
                    spritePathMentionCount++;
                }
                else
                {
                    allSpritePathsMentioned = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + " current Unity sprite path is missing from Batch 69 audit.");
                }

                if (ContainsText(combined, P0StarterCatSourceLockPacketEvidence.ActiveScreenshotFor(current.CatId)))
                {
                    activeScreenshotMentionCount++;
                }
                else
                {
                    allScreenshotsMentioned = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + " active-cat screenshot target is missing from Batch 69 audit.");
                }

                if (ContainsRecommendationFor(manifestText, current.CatId))
                {
                    recommendationMentionCount++;
                }
                else
                {
                    allRecommendationsMentioned = false;
                }
            }

            bool importBlocked = ContainsText(combined, "audit-only")
                && ContainsText(combined, "Do not import into Unity yet")
                && ContainsText(combined, "active-cat Play Mode screenshot");
            bool noImageGeneration = ContainsText(processText, "No image generation was performed");

            report.SetCounts(
                artifactCount,
                manifestRowCount,
                sourceLockMentionCount,
                sourcePathMentionCount,
                spritePathMentionCount,
                activeScreenshotMentionCount,
                recommendationMentionCount,
                metaFileCount);

            Require(report, artifactCount == ExpectedArtifactCount, "Batch 69 comparison audit artifacts are present.", "Batch 69 comparison audit artifacts are missing.");
            Require(report, exists(ReviewSheetPath), "Batch 69 comparison review sheet exists.", "Batch 69 comparison review sheet is missing.");
            Require(report, manifestRowCount == ExpectedStarterCatCount, "Batch 69 comparison manifest has one row per starter cat.", "Batch 69 comparison manifest row count is stale.");
            Require(report, allLocksMentioned && sourceLockMentionCount == ExpectedStarterCatCount, "Batch 69 comparison audit names all starter-cat source locks.", "Batch 69 comparison audit is missing source locks.");
            Require(report, allSourcePathsMentioned && sourcePathMentionCount == ExpectedStarterCatCount, "Batch 69 comparison audit repeats all exact colored-turnaround source paths.", "Batch 69 comparison audit is missing exact source paths.");
            Require(report, allSpritePathsMentioned && spritePathMentionCount == ExpectedStarterCatCount, "Batch 69 comparison audit names all current Unity combat sprites.", "Batch 69 comparison audit is missing current Unity sprite paths.");
            Require(report, allScreenshotsMentioned && activeScreenshotMentionCount == ExpectedStarterCatCount, "Batch 69 comparison audit names all active-cat Play Mode screenshot targets.", "Batch 69 comparison audit is missing active-cat screenshot targets.");
            Require(report, allRecommendationsMentioned && recommendationMentionCount == ExpectedStarterCatCount && importBlocked && noImageGeneration, "Batch 69 comparison audit is audit-only and blocks import pending active-cat screenshots.", "Batch 69 comparison audit has an unsafe import or generation decision.");
            Require(report, metaFileCount == 0, "Batch 69 comparison audit has no Unity .meta files.", "Batch 69 comparison audit must stay outside Unity import.");
            return report;
        }

        private static int CountArtifacts(Func<string, bool> fileExists)
        {
            string[] paths =
            {
                ManifestPath,
                ReviewSheetPath,
                ReviewNotePath,
                ProcessNotePath
            };

            int count = 0;
            for (int i = 0; i < paths.Length; i++)
            {
                if (fileExists(paths[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountMetaFiles(Func<string, bool> fileExists)
        {
            string[] paths =
            {
                ManifestPath + ".meta",
                ReviewSheetPath + ".meta",
                ReviewNotePath + ".meta",
                ProcessNotePath + ".meta"
            };

            int count = 0;
            for (int i = 0; i < paths.Length; i++)
            {
                if (fileExists(paths[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountManifestRows(string manifestText)
        {
            if (string.IsNullOrWhiteSpace(manifestText))
            {
                return 0;
            }

            string[] lines = manifestText.Replace("\r\n", "\n").Split('\n');
            int count = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static bool ContainsRecommendationFor(string manifestText, string catId)
        {
            if (string.IsNullOrWhiteSpace(manifestText) || string.IsNullOrWhiteSpace(catId))
            {
                return false;
            }

            string[] lines = manifestText.Replace("\r\n", "\n").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (ContainsText(line, catId) && ContainsText(line, RequiredRecommendation))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ReadOptional(
            string path,
            P0StarterCatReviewTextReader reader,
            Func<string, bool> exists,
            P0StarterCatTurnaroundComparisonAuditReport report)
        {
            if (!exists(path))
            {
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Batch 69 audit artifact is missing: " + path);
                return string.Empty;
            }

            if (!reader(path, out string text, out string error))
            {
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Batch 69 audit artifact could not be read: " + path + " (" + error + ")");
                return string.Empty;
            }

            return text ?? string.Empty;
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

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return File.Exists(ResolveFile(path));
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

        private static bool ContainsText(string value, string token)
        {
            return value != null && token != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0StarterCatTurnaroundComparisonAuditReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, failureMessage);
        }
    }

    public readonly struct P0StarterCatReferencePlateIssue
    {
        public P0StarterCatReferencePlateIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatSourceLockPacketSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatReferencePlateReport
    {
        private readonly List<P0StarterCatReferencePlateIssue> issues = new List<P0StarterCatReferencePlateIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatReferencePlateIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int ArtifactCount { get; private set; }

        public int PlateCount { get; private set; }

        public int ManifestRowCount { get; private set; }

        public int SourceLockMentionCount { get; private set; }

        public int SourcePathMentionCount { get; private set; }

        public int ViewRowCount { get; private set; }

        public int ImportBlockCount { get; private set; }

        public int MetaFileCount { get; private set; }

        public int FailureCount => Count(P0StarterCatSourceLockPacketSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatReferencePlateEvidence.ExpectedCoveredCheckCount;

        public void SetCounts(
            int artifactCount,
            int plateCount,
            int manifestRowCount,
            int sourceLockMentionCount,
            int sourcePathMentionCount,
            int viewRowCount,
            int importBlockCount,
            int metaFileCount)
        {
            ArtifactCount = artifactCount;
            PlateCount = plateCount;
            ManifestRowCount = manifestRowCount;
            SourceLockMentionCount = sourceLockMentionCount;
            SourcePathMentionCount = sourcePathMentionCount;
            ViewRowCount = viewRowCount;
            ImportBlockCount = importBlockCount;
            MetaFileCount = metaFileCount;
        }

        public void AddIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            issues.Add(new P0StarterCatReferencePlateIssue(severity, message));
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
                ? "P0 starter cat source turnaround reference plates ready for " + PlateCount + " plate(s)."
                : "P0 starter cat source turnaround reference plates have " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Artifacts: " + ArtifactCount,
                "Plates: " + PlateCount,
                "Manifest rows: " + ManifestRowCount,
                "Source lock mentions: " + SourceLockMentionCount,
                "Source path mentions: " + SourcePathMentionCount,
                "View rows: " + ViewRowCount,
                "Import blocks: " + ImportBlockCount,
                "Meta files: " + MetaFileCount
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

        private int Count(P0StarterCatSourceLockPacketSeverity severity)
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

    public static class P0StarterCatReferencePlateEvidence
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedViewCount = 3;
        public const int ExpectedPlateCount = 9;
        public const int ExpectedArtifactCount = 13;
        public const int ExpectedCoveredCheckCount = 9;
        public const string BatchDirectory = "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15";
        public const string ManifestPath = BatchDirectory + "/starter_cat_turnaround_reference_plates_batch70_manifest.csv";
        public const string ReviewSheetPath = BatchDirectory + "/thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png";
        public const string ReviewNotePath = BatchDirectory + "/starter_cat_turnaround_reference_plates_batch70_review.md";
        public const string ProcessNotePath = BatchDirectory + "/starter_cat_turnaround_reference_plates_batch70_process_note.md";
        private const string ImportBlock = "reference_only_do_not_import_pending_active_cat_playmode_screenshot";

        private static readonly string[] Views =
        {
            "front",
            "side",
            "back"
        };

        public static P0StarterCatReferencePlateReport EvaluateCurrentPlates()
        {
            return Evaluate(
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                DefaultReadText,
                DefaultFileExists);
        }

        public static P0StarterCatReferencePlateReport Evaluate(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            P0StarterCatReviewTextReader readText,
            Func<string, bool> fileExists)
        {
            P0StarterCatReferencePlateReport report = new P0StarterCatReferencePlateReport();
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> expectedLocks = locks ?? Array.Empty<P0StarterCatTurnaroundSourceLockEntry>();
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            Func<string, bool> exists = fileExists ?? DefaultFileExists;

            string manifestText = ReadOptional(ManifestPath, reader, exists, report);
            string reviewText = ReadOptional(ReviewNotePath, reader, exists, report);
            string processText = ReadOptional(ProcessNotePath, reader, exists, report);
            string combined = manifestText + "\n" + reviewText + "\n" + processText;

            int plateCount = CountPlates(expectedLocks, exists);
            int artifactCount = CountArtifacts(expectedLocks, exists);
            int manifestRowCount = CountManifestRows(manifestText);
            int sourceLockMentionCount = 0;
            int sourcePathMentionCount = 0;
            int viewRowCount = 0;
            int importBlockCount = 0;
            int metaFileCount = CountMetaFiles(expectedLocks, exists);
            bool allLocksMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allSourcePathsMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allViewsMentioned = expectedLocks.Count == ExpectedStarterCatCount;
            bool allImportBlocksMentioned = expectedLocks.Count == ExpectedStarterCatCount;

            for (int i = 0; i < expectedLocks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = expectedLocks[i];
                string sourceLockId = current.CatId + "_turnaround_colored";
                if (ContainsText(combined, sourceLockId))
                {
                    sourceLockMentionCount++;
                }
                else
                {
                    allLocksMentioned = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + " source lock is missing from Batch 70 reference plates.");
                }

                if (ContainsText(combined, current.SourceTurnaroundPath))
                {
                    sourcePathMentionCount++;
                }
                else
                {
                    allSourcePathsMentioned = false;
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + " exact colored-turnaround path is missing from Batch 70 reference plates.");
                }

                bool allViewsForCat = true;
                bool allBlocksForCat = true;
                for (int viewIndex = 0; viewIndex < Views.Length; viewIndex++)
                {
                    string view = Views[viewIndex];
                    string platePath = ReferencePlatePathFor(current.CatId, view);
                    if (ContainsManifestRowFor(manifestText, current.CatId, view, platePath))
                    {
                        viewRowCount++;
                    }
                    else
                    {
                        allViewsForCat = false;
                        report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, current.CatId + "/" + view + " reference plate row is missing from Batch 70 manifest.");
                    }

                    if (ContainsImportBlockFor(manifestText, current.CatId, view))
                    {
                        importBlockCount++;
                    }
                    else
                    {
                        allBlocksForCat = false;
                    }
                }

                if (!allViewsForCat)
                {
                    allViewsMentioned = false;
                }

                if (!allBlocksForCat)
                {
                    allImportBlocksMentioned = false;
                }
            }

            bool reviewBlocksImport = ContainsText(reviewText, "reference-only")
                && ContainsText(reviewText, "Do not import into Unity yet")
                && ContainsText(reviewText, "active-cat Play Mode screenshot");
            bool reviewLocksTraits = ContainsText(reviewText, "body proportion")
                && ContainsText(reviewText, "face markings")
                && ContainsText(reviewText, "palette")
                && ContainsText(reviewText, "costume")
                && ContainsText(reviewText, "props")
                && ContainsText(reviewText, "civilization motifs");
            bool noImageGeneration = ContainsText(reviewText, "no image generation")
                && ContainsText(processText, "No image generation was performed");

            report.SetCounts(
                artifactCount,
                plateCount,
                manifestRowCount,
                sourceLockMentionCount,
                sourcePathMentionCount,
                viewRowCount,
                importBlockCount,
                metaFileCount);

            Require(report, artifactCount == ExpectedArtifactCount, "Batch 70 source-turnaround reference plate artifacts are present.", "Batch 70 source-turnaround reference plate artifacts are missing.");
            Require(report, plateCount == ExpectedPlateCount, "Batch 70 has front, side, and back reference plates for every starter cat.", "Batch 70 reference plates are incomplete.");
            Require(report, manifestRowCount == ExpectedPlateCount, "Batch 70 manifest has one row per starter-cat view.", "Batch 70 manifest row count is stale.");
            Require(report, allLocksMentioned && sourceLockMentionCount == ExpectedStarterCatCount, "Batch 70 reference plates name all starter-cat source locks.", "Batch 70 reference plates are missing source locks.");
            Require(report, allSourcePathsMentioned && sourcePathMentionCount == ExpectedStarterCatCount, "Batch 70 reference plates repeat all exact colored-turnaround source paths.", "Batch 70 reference plates are missing exact source paths.");
            Require(report, allViewsMentioned && viewRowCount == ExpectedPlateCount, "Batch 70 reference plates cover front, side, and back rows for all starter cats.", "Batch 70 reference plate view rows are incomplete.");
            Require(report, allImportBlocksMentioned && importBlockCount == ExpectedPlateCount && reviewBlocksImport, "Batch 70 reference plates remain reference-only and block Unity import pending screenshots.", "Batch 70 reference plate import status is unsafe.");
            Require(report, noImageGeneration && reviewLocksTraits, "Batch 70 reference plates are deterministic source-derived inputs with strict trait locks.", "Batch 70 reference plate review is missing source-derived or trait-lock language.");
            Require(report, metaFileCount == 0, "Batch 70 reference plates have no Unity .meta files.", "Batch 70 reference plate batch must stay outside Unity import.");
            return report;
        }

        public static string ReferencePlatePathFor(string catId, string view)
        {
            return BatchDirectory + "/thecat_cat_" + catId + "_turnaround_" + view + "_reference_plate_768_batch70_v001.png";
        }

        private static int CountArtifacts(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks, Func<string, bool> fileExists)
        {
            int count = 0;
            string[] textArtifacts =
            {
                ManifestPath,
                ReviewSheetPath,
                ReviewNotePath,
                ProcessNotePath
            };

            for (int i = 0; i < textArtifacts.Length; i++)
            {
                if (fileExists(textArtifacts[i]))
                {
                    count++;
                }
            }

            return count + CountPlates(locks, fileExists);
        }

        private static int CountPlates(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks, Func<string, bool> fileExists)
        {
            int count = 0;
            for (int i = 0; i < locks.Count; i++)
            {
                for (int viewIndex = 0; viewIndex < Views.Length; viewIndex++)
                {
                    if (fileExists(ReferencePlatePathFor(locks[i].CatId, Views[viewIndex])))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int CountMetaFiles(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks, Func<string, bool> fileExists)
        {
            int count = 0;
            string[] textArtifacts =
            {
                ManifestPath,
                ReviewSheetPath,
                ReviewNotePath,
                ProcessNotePath
            };

            for (int i = 0; i < textArtifacts.Length; i++)
            {
                if (fileExists(textArtifacts[i] + ".meta"))
                {
                    count++;
                }
            }

            for (int i = 0; i < locks.Count; i++)
            {
                for (int viewIndex = 0; viewIndex < Views.Length; viewIndex++)
                {
                    if (fileExists(ReferencePlatePathFor(locks[i].CatId, Views[viewIndex]) + ".meta"))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static int CountManifestRows(string manifestText)
        {
            if (string.IsNullOrWhiteSpace(manifestText))
            {
                return 0;
            }

            string[] lines = manifestText.Replace("\r\n", "\n").Split('\n');
            int count = 0;
            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static bool ContainsManifestRowFor(string manifestText, string catId, string view, string platePath)
        {
            return ContainsManifestLine(manifestText, catId, view, platePath);
        }

        private static bool ContainsImportBlockFor(string manifestText, string catId, string view)
        {
            return ContainsManifestLine(manifestText, catId, view, ImportBlock);
        }

        private static bool ContainsManifestLine(string manifestText, string catId, string view, string token)
        {
            if (string.IsNullOrWhiteSpace(manifestText))
            {
                return false;
            }

            string[] lines = manifestText.Replace("\r\n", "\n").Split('\n');
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (ContainsText(line, catId) && ContainsText(line, view) && ContainsText(line, token))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ReadOptional(
            string path,
            P0StarterCatReviewTextReader reader,
            Func<string, bool> exists,
            P0StarterCatReferencePlateReport report)
        {
            if (!exists(path))
            {
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Batch 70 artifact is missing: " + path);
                return string.Empty;
            }

            if (!reader(path, out string text, out string error))
            {
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Batch 70 artifact could not be read: " + path + " (" + error + ")");
                return string.Empty;
            }

            return text ?? string.Empty;
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

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return File.Exists(ResolveFile(path));
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

        private static bool ContainsText(string value, string token)
        {
            return value != null && token != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0StarterCatReferencePlateReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, failureMessage);
        }
    }
}
