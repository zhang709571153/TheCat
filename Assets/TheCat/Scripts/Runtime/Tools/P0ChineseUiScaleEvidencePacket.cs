using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheCat.Tools
{
    public delegate bool P0ChineseUiScaleEvidenceFileExists(string path);

    public delegate bool P0ChineseUiScaleEvidenceTextReader(string path, out string text, out string error);

    public delegate IReadOnlyList<string> P0ChineseUiScaleEvidenceFileEnumerator(string directory);

    public enum P0ChineseUiScaleEvidenceSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0ChineseUiScaleEvidenceIssue
    {
        public P0ChineseUiScaleEvidenceIssue(P0ChineseUiScaleEvidenceSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0ChineseUiScaleEvidenceSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0ChineseUiScaleEvidencePacketReport
    {
        private readonly List<P0ChineseUiScaleEvidenceIssue> issues = new List<P0ChineseUiScaleEvidenceIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<string> existingExpectedFiles = new List<string>();
        private readonly List<string> missingExpectedFiles = new List<string>();
        private readonly List<string> unexpectedMetaFiles = new List<string>();

        public IReadOnlyList<P0ChineseUiScaleEvidenceIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<string> ExistingExpectedFiles => existingExpectedFiles.AsReadOnly();

        public IReadOnlyList<string> MissingExpectedFiles => missingExpectedFiles.AsReadOnly();

        public IReadOnlyList<string> UnexpectedMetaFiles => unexpectedMetaFiles.AsReadOnly();

        public string BatchDirectory { get; private set; } = string.Empty;

        public int ExpectedFileCount { get; private set; }

        public int ExistingExpectedFileCount => existingExpectedFiles.Count;

        public int MissingExpectedFileCount => missingExpectedFiles.Count;

        public int UnexpectedMetaFileCount => unexpectedMetaFiles.Count;

        public int ManifestRowCount { get; private set; }

        public int ManifestExpectedSubjectCount { get; private set; }

        public int ManifestNonAssetsTemplatePathCount { get; private set; }

        public int ManifestSourceReferenceCount { get; private set; }

        public int ManifestTemplateOnlyRecommendationCount { get; private set; }

        public int ManifestNoRuntimeBindingNoteCount { get; private set; }

        public int CaptureMatrixRowCount { get; private set; }

        public int SurfaceCoverageCount { get; private set; }

        public int ResolutionCoverageCount { get; private set; }

        public int SurfaceResolutionPairCoverageCount { get; private set; }

        public int PendingCaptureRowCount { get; private set; }

        public bool ReviewNoteReady { get; private set; }

        public bool ProcessNoteReady { get; private set; }

        public int FailureCount => Count(P0ChineseUiScaleEvidenceSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0ChineseUiScaleEvidencePacket.ExpectedCoveredCheckCount
            && ExpectedFileCount == P0ChineseUiScaleEvidencePacket.ExpectedPacketFileCount
            && ExistingExpectedFileCount == ExpectedFileCount
            && MissingExpectedFileCount == 0
            && UnexpectedMetaFileCount == 0
            && ManifestRowCount == P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount
            && ManifestExpectedSubjectCount == P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount
            && ManifestNonAssetsTemplatePathCount == P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount
            && ManifestSourceReferenceCount == P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount
            && ManifestTemplateOnlyRecommendationCount == P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount
            && ManifestNoRuntimeBindingNoteCount == P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount
            && CaptureMatrixRowCount == P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount
            && SurfaceCoverageCount == P0ChineseUiScaleValidationPlan.ExpectedSurfaceCount
            && ResolutionCoverageCount == P0ChineseUiScaleValidationPlan.ExpectedResolutionCount
            && SurfaceResolutionPairCoverageCount == P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount
            && ReviewNoteReady
            && ProcessNoteReady;

        public void SetDirectory(string batchDirectory, int expectedFileCount)
        {
            BatchDirectory = batchDirectory ?? string.Empty;
            ExpectedFileCount = expectedFileCount;
        }

        public void SetManifestCounts(
            int rowCount,
            int expectedSubjectCount,
            int nonAssetsTemplatePathCount,
            int sourceReferenceCount,
            int templateOnlyRecommendationCount,
            int noRuntimeBindingNoteCount)
        {
            ManifestRowCount = rowCount;
            ManifestExpectedSubjectCount = expectedSubjectCount;
            ManifestNonAssetsTemplatePathCount = nonAssetsTemplatePathCount;
            ManifestSourceReferenceCount = sourceReferenceCount;
            ManifestTemplateOnlyRecommendationCount = templateOnlyRecommendationCount;
            ManifestNoRuntimeBindingNoteCount = noRuntimeBindingNoteCount;
        }

        public void SetCaptureMatrixCounts(
            int rowCount,
            int surfaceCoverageCount,
            int resolutionCoverageCount,
            int surfaceResolutionPairCoverageCount,
            int pendingCaptureRowCount)
        {
            CaptureMatrixRowCount = rowCount;
            SurfaceCoverageCount = surfaceCoverageCount;
            ResolutionCoverageCount = resolutionCoverageCount;
            SurfaceResolutionPairCoverageCount = surfaceResolutionPairCoverageCount;
            PendingCaptureRowCount = pendingCaptureRowCount;
        }

        public void SetNoteReadiness(bool reviewNoteReady, bool processNoteReady)
        {
            ReviewNoteReady = reviewNoteReady;
            ProcessNoteReady = processNoteReady;
        }

        public void AddExistingExpectedFile(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                existingExpectedFiles.Add(fileName);
            }
        }

        public void AddMissingExpectedFile(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                missingExpectedFiles.Add(fileName);
            }
        }

        public void AddUnexpectedMetaFile(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                unexpectedMetaFiles.Add(fileName);
            }
        }

        public void AddIssue(P0ChineseUiScaleEvidenceSeverity severity, string message)
        {
            issues.Add(new P0ChineseUiScaleEvidenceIssue(severity, message));
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
                ? "P0 Chinese UI scale evidence packet ready for " + SurfaceCoverageCount + " surface(s), " + ResolutionCoverageCount + " resolution(s), and " + CaptureMatrixRowCount + " capture row(s)."
                : "P0 Chinese UI scale evidence packet has " + FailureCount + " failure(s), " + MissingExpectedFileCount + " missing file(s), and " + UnexpectedMetaFileCount + " Unity meta file(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Batch directory: " + BatchDirectory,
                "Expected files: " + ExpectedFileCount,
                "Existing expected files: " + ExistingExpectedFileCount,
                "Missing expected files: " + MissingExpectedFileCount,
                "Unexpected Unity meta files: " + UnexpectedMetaFileCount,
                "Manifest rows: " + ManifestRowCount,
                "Manifest expected subjects: " + ManifestExpectedSubjectCount,
                "Manifest non-Assets template paths: " + ManifestNonAssetsTemplatePathCount,
                "Manifest source-reference rows: " + ManifestSourceReferenceCount,
                "Manifest template-only recommendations: " + ManifestTemplateOnlyRecommendationCount,
                "Manifest no-runtime-binding notes: " + ManifestNoRuntimeBindingNoteCount,
                "Capture matrix rows: " + CaptureMatrixRowCount,
                "Surface coverage: " + SurfaceCoverageCount,
                "Resolution coverage: " + ResolutionCoverageCount,
                "Surface/resolution pair coverage: " + SurfaceResolutionPairCoverageCount,
                "Pending Unity capture rows: " + PendingCaptureRowCount,
                "Review note ready: " + YesNo(ReviewNoteReady),
                "Process note ready: " + YesNo(ProcessNoteReady)
            };

            AppendList(lines, "Missing", missingExpectedFiles);
            AppendList(lines, "Unity meta files", unexpectedMetaFiles);

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

        private static void AppendList(List<string> lines, string label, IReadOnlyList<string> values)
        {
            if (values == null || values.Count == 0)
            {
                return;
            }

            lines.Add(label + ":");
            for (int i = 0; i < values.Count; i++)
            {
                lines.Add("- " + values[i]);
            }
        }

        private int Count(P0ChineseUiScaleEvidenceSeverity severity)
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

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }
    }

    public static class P0ChineseUiScaleEvidencePacket
    {
        public const int ExpectedPacketFileCount = 9;
        public const int ExpectedTemplateManifestRowCount = 4;
        public const int ExpectedCaptureMatrixRowCount = 20;
        public const int ExpectedCoveredCheckCount = 12;

        public const string BatchSlug = "batch_75_chinese_ui_scale_evidence_templates_2026-06-20";
        public const string DefaultBatchDirectory = "design/development/asset_candidates/ui/chinese_ui_scale_evidence/" + BatchSlug;

        public const string ManifestFileName = "chinese_ui_scale_batch75_manifest.csv";
        public const string CaptureMatrixFileName = "chinese_ui_scale_batch75_capture_matrix.csv";
        public const string CandidateReviewFileName = "chinese_ui_scale_batch75_candidate_review.md";
        public const string ProcessNoteFileName = "chinese_ui_scale_batch75_process_note.md";
        public const string ReviewSheetFileName = "thecat_ui_chinese_scale_batch75_review_sheet.png";
        public const string CaptureMatrixTemplateFileName = "thecat_ui_chinese_scale_capture_matrix_batch75_1920x1080_v001.png";
        public const string SafeAreaOverlayFileName = "thecat_ui_chinese_scale_safe_area_overlay_batch75_1920x1080_v001.png";
        public const string SurfaceNoteCardFileName = "thecat_ui_chinese_scale_surface_note_card_batch75_1280x720_v001.png";
        public const string ResolutionStripFileName = "thecat_ui_chinese_scale_resolution_strip_batch75_1600x320_v001.png";

        public static readonly string[] ExpectedPacketFileNames =
        {
            ManifestFileName,
            CaptureMatrixFileName,
            CandidateReviewFileName,
            ProcessNoteFileName,
            ReviewSheetFileName,
            CaptureMatrixTemplateFileName,
            SafeAreaOverlayFileName,
            SurfaceNoteCardFileName,
            ResolutionStripFileName
        };

        public static readonly string[] ExpectedTemplateSubjectIds =
        {
            "ui_scale_capture_matrix_sheet",
            "ui_scale_safe_area_overlay",
            "ui_scale_surface_note_card",
            "ui_scale_resolution_strip"
        };

        public static P0ChineseUiScaleEvidencePacketReport EvaluateCurrentPacket()
        {
            return Evaluate(
                DefaultBatchDirectory,
                DefaultFileExists,
                DefaultReadText,
                DefaultEnumerateFileNames);
        }

        public static P0ChineseUiScaleEvidencePacketReport Evaluate(
            string batchDirectory,
            P0ChineseUiScaleEvidenceFileExists fileExists,
            P0ChineseUiScaleEvidenceTextReader readText,
            P0ChineseUiScaleEvidenceFileEnumerator enumerateFileNames)
        {
            P0ChineseUiScaleEvidencePacketReport report = new P0ChineseUiScaleEvidencePacketReport();
            string directory = NormalizePath(batchDirectory);
            P0ChineseUiScaleEvidenceFileExists exists = fileExists ?? DefaultFileExists;
            P0ChineseUiScaleEvidenceTextReader reader = readText ?? DefaultReadText;
            P0ChineseUiScaleEvidenceFileEnumerator enumerate = enumerateFileNames ?? DefaultEnumerateFileNames;

            report.SetDirectory(directory, ExpectedPacketFileNames.Length);
            EvaluateExpectedFiles(report, directory, exists);
            EvaluateMetaFiles(report, directory, enumerate);

            string manifestPath = CombinePath(directory, ManifestFileName);
            string captureMatrixPath = CombinePath(directory, CaptureMatrixFileName);
            string candidateReviewPath = CombinePath(directory, CandidateReviewFileName);
            string processNotePath = CombinePath(directory, ProcessNoteFileName);

            string manifestText = ReadRequiredText(report, reader, manifestPath, "manifest");
            string captureMatrixText = ReadRequiredText(report, reader, captureMatrixPath, "capture matrix");
            string candidateReviewText = ReadRequiredText(report, reader, candidateReviewPath, "candidate review note");
            string processNoteText = ReadRequiredText(report, reader, processNotePath, "process note");

            EvaluateManifest(report, manifestText, directory, exists);
            EvaluateCaptureMatrix(report, captureMatrixText);
            EvaluateNotes(report, candidateReviewText, processNoteText);

            return report;
        }

        public static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            string resolved = ResolveFilePath(path);
            return !string.IsNullOrWhiteSpace(resolved) && File.Exists(resolved);
        }

        public static bool DefaultReadText(string path, out string text, out string error)
        {
            text = string.Empty;
            error = string.Empty;

            if (string.IsNullOrWhiteSpace(path))
            {
                error = "Path is empty.";
                return false;
            }

            try
            {
                string resolved = ResolveFilePath(path);
                if (string.IsNullOrWhiteSpace(resolved) || !File.Exists(resolved))
                {
                    error = "File does not exist: " + path;
                    return false;
                }

                text = File.ReadAllText(resolved, Encoding.UTF8);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        public static IReadOnlyList<string> DefaultEnumerateFileNames(string directory)
        {
            string resolved = ResolveDirectory(directory);
            if (string.IsNullOrWhiteSpace(resolved) || !Directory.Exists(resolved))
            {
                return Array.Empty<string>();
            }

            string[] paths = Directory.GetFiles(resolved, "*", SearchOption.AllDirectories);
            List<string> names = new List<string>(paths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                names.Add(NormalizePath(RelativePath(resolved, paths[i])));
            }

            names.Sort(StringComparer.Ordinal);
            return names.AsReadOnly();
        }

        private static void EvaluateExpectedFiles(
            P0ChineseUiScaleEvidencePacketReport report,
            string directory,
            P0ChineseUiScaleEvidenceFileExists exists)
        {
            bool allPresent = true;
            for (int i = 0; i < ExpectedPacketFileNames.Length; i++)
            {
                string fileName = ExpectedPacketFileNames[i];
                if (exists(CombinePath(directory, fileName)))
                {
                    report.AddExistingExpectedFile(fileName);
                }
                else
                {
                    allPresent = false;
                    report.AddMissingExpectedFile(fileName);
                }
            }

            Require(
                report,
                allPresent,
                "Batch 75 Chinese UI scale evidence packet contains all expected template, manifest, matrix, review, and process files.",
                "Batch 75 Chinese UI scale evidence packet is missing one or more expected files.");
        }

        private static void EvaluateMetaFiles(
            P0ChineseUiScaleEvidencePacketReport report,
            string directory,
            P0ChineseUiScaleEvidenceFileEnumerator enumerate)
        {
            IReadOnlyList<string> files = enumerate(directory) ?? Array.Empty<string>();
            for (int i = 0; i < files.Count; i++)
            {
                string fileName = NormalizePath(files[i]);
                if (fileName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                {
                    report.AddUnexpectedMetaFile(fileName);
                }
            }

            Require(
                report,
                report.UnexpectedMetaFileCount == 0,
                "Batch 75 candidate folder stays outside Unity import and has no .meta files.",
                "Batch 75 candidate folder contains Unity .meta files and may have been imported accidentally.");
        }

        private static string ReadRequiredText(
            P0ChineseUiScaleEvidencePacketReport report,
            P0ChineseUiScaleEvidenceTextReader reader,
            string path,
            string label)
        {
            if (reader(path, out string text, out string error))
            {
                return text ?? string.Empty;
            }

            report.AddIssue(P0ChineseUiScaleEvidenceSeverity.Failure, "Could not read Batch 75 " + label + ": " + error);
            return string.Empty;
        }

        private static void EvaluateManifest(
            P0ChineseUiScaleEvidencePacketReport report,
            string manifestText,
            string directory,
            P0ChineseUiScaleEvidenceFileExists exists)
        {
            List<string[]> rows = ParseCsvRows(manifestText);
            int rowCount = DataRowCount(rows);
            int subjectIndex = FindHeaderIndex(rows, "subject_id");
            int templatePathIndex = FindHeaderIndex(rows, "template_path");
            int sourceReferencesIndex = FindHeaderIndex(rows, "source_references");
            int recommendationIndex = FindHeaderIndex(rows, "recommendation");
            int notesIndex = FindHeaderIndex(rows, "notes");
            int batchSlugIndex = FindHeaderIndex(rows, "batch_slug");
            int intendedUseIndex = FindHeaderIndex(rows, "intended_use");

            HashSet<string> subjectIds = new HashSet<string>(StringComparer.Ordinal);
            int nonAssetsTemplatePathCount = 0;
            int sourceReferenceCount = 0;
            int templateOnlyRecommendationCount = 0;
            int noRuntimeBindingNoteCount = 0;

            for (int i = 1; i < rows.Count; i++)
            {
                string[] row = rows[i];
                string subjectId = Field(row, subjectIndex);
                string templatePath = Field(row, templatePathIndex);
                string sourceReferences = Field(row, sourceReferencesIndex);
                string recommendation = Field(row, recommendationIndex);
                string notes = Field(row, notesIndex);
                string batchSlug = Field(row, batchSlugIndex);
                string intendedUse = Field(row, intendedUseIndex);

                if (!string.IsNullOrWhiteSpace(subjectId))
                {
                    subjectIds.Add(subjectId);
                }

                if (batchSlug != BatchSlug)
                {
                    report.AddIssue(P0ChineseUiScaleEvidenceSeverity.Failure, "Manifest row " + subjectId + " has wrong batch slug: " + batchSlug);
                }

                if (!StartsWithAssetsPath(templatePath) && exists(templatePath))
                {
                    nonAssetsTemplatePathCount++;
                }

                if (ContainsText(sourceReferences, "P0ChineseUiScaleValidationPlan")
                    && ContainsText(sourceReferences, "P0ChineseUiCoverage")
                    && ContainsText(sourceReferences, "P0ImGuiLayout")
                    && ContainsText(sourceReferences, "UNITY_VALIDATION_BACKLOG item 188"))
                {
                    sourceReferenceCount++;
                }

                if (recommendation == "validation_template_only_do_not_import"
                    && ContainsText(intendedUse, "validation."))
                {
                    templateOnlyRecommendationCount++;
                }

                if (ContainsText(notes, "non_cat_ui_validation_template_no_runtime_binding"))
                {
                    noRuntimeBindingNoteCount++;
                }
            }

            int expectedSubjectCount = CountExpectedSubjectIds(subjectIds);
            report.SetManifestCounts(
                rowCount,
                expectedSubjectCount,
                nonAssetsTemplatePathCount,
                sourceReferenceCount,
                templateOnlyRecommendationCount,
                noRuntimeBindingNoteCount);

            Require(
                report,
                rowCount == ExpectedTemplateManifestRowCount,
                "Batch 75 manifest contains exactly four template rows.",
                "Batch 75 manifest row count is stale or incomplete.");

            Require(
                report,
                expectedSubjectCount == ExpectedTemplateManifestRowCount,
                "Batch 75 manifest includes all four expected UI scale template subject ids.",
                "Batch 75 manifest is missing one or more expected template subject ids.");

            Require(
                report,
                nonAssetsTemplatePathCount == ExpectedTemplateManifestRowCount,
                "Batch 75 template paths resolve outside Assets for every manifest row.",
                "Batch 75 manifest contains missing template files or unsafe Assets paths.");

            Require(
                report,
                sourceReferenceCount == ExpectedTemplateManifestRowCount,
                "Batch 75 manifest links every row back to Chinese UI scale validation, Chinese UI coverage, IMGUI layout, and backlog item 188.",
                "Batch 75 manifest rows are missing required source references.");

            Require(
                report,
                templateOnlyRecommendationCount == ExpectedTemplateManifestRowCount
                && noRuntimeBindingNoteCount == ExpectedTemplateManifestRowCount,
                "Batch 75 manifest keeps every template as validation-only, non-cat, and not runtime-bound.",
                "Batch 75 manifest contains unsafe import recommendations or runtime-binding notes.");
        }

        private static void EvaluateCaptureMatrix(
            P0ChineseUiScaleEvidencePacketReport report,
            string captureMatrixText)
        {
            List<string[]> rows = ParseCsvRows(captureMatrixText);
            int rowCount = DataRowCount(rows);
            int surfaceIndex = FindHeaderIndex(rows, "surface_id");
            int resolutionIndex = FindHeaderIndex(rows, "resolution");
            int statusIndex = FindHeaderIndex(rows, "unity_evidence_status");

            HashSet<string> surfaces = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> resolutions = new HashSet<string>(StringComparer.Ordinal);
            HashSet<string> pairs = new HashSet<string>(StringComparer.Ordinal);
            int pendingRows = 0;

            for (int i = 1; i < rows.Count; i++)
            {
                string[] row = rows[i];
                string surface = Field(row, surfaceIndex);
                string resolution = Field(row, resolutionIndex);
                string status = Field(row, statusIndex);
                if (!string.IsNullOrWhiteSpace(surface))
                {
                    surfaces.Add(surface);
                }

                if (!string.IsNullOrWhiteSpace(resolution))
                {
                    resolutions.Add(resolution);
                }

                if (!string.IsNullOrWhiteSpace(surface) && !string.IsNullOrWhiteSpace(resolution))
                {
                    pairs.Add(surface + "|" + resolution);
                }

                if (status == "pending_unity_mcp_or_editor_capture")
                {
                    pendingRows++;
                }
            }

            int expectedSurfaceCount = CountExpectedSurfaces(surfaces);
            int expectedResolutionCount = CountExpectedResolutions(resolutions);
            int expectedPairCount = CountExpectedSurfaceResolutionPairs(pairs);
            report.SetCaptureMatrixCounts(
                rowCount,
                expectedSurfaceCount,
                expectedResolutionCount,
                expectedPairCount,
                pendingRows);

            Require(
                report,
                rowCount == ExpectedCaptureMatrixRowCount,
                "Batch 75 capture matrix contains all 20 surface/resolution rows.",
                "Batch 75 capture matrix does not contain the required 20 surface/resolution rows.");

            Require(
                report,
                expectedSurfaceCount == P0ChineseUiScaleValidationPlan.ExpectedSurfaceCount,
                "Batch 75 capture matrix covers all five P0 UI surfaces.",
                "Batch 75 capture matrix is missing one or more P0 UI surfaces.");

            Require(
                report,
                expectedResolutionCount == P0ChineseUiScaleValidationPlan.ExpectedResolutionCount,
                "Batch 75 capture matrix covers 1024x768, 1280x720, 1600x900, and 1920x1080.",
                "Batch 75 capture matrix is missing one or more required resolutions.");

            Require(
                report,
                expectedPairCount == ExpectedCaptureMatrixRowCount,
                "Batch 75 capture matrix includes every required surface/resolution pair exactly once for validation coverage.",
                "Batch 75 capture matrix is missing required surface/resolution pair coverage.");
        }

        private static void EvaluateNotes(
            P0ChineseUiScaleEvidencePacketReport report,
            string candidateReviewText,
            string processNoteText)
        {
            bool reviewNoteReady = ContainsText(candidateReviewText, "Validation template only")
                && ContainsText(candidateReviewText, "Do not import")
                && ContainsText(candidateReviewText, "Non-cat UI validation only")
                && ContainsText(candidateReviewText, "Unity MCP")
                && ContainsText(candidateReviewText, "1024x768")
                && ContainsText(candidateReviewText, "1280x720")
                && ContainsText(candidateReviewText, "1600x900")
                && ContainsText(candidateReviewText, "1920x1080")
                && ContainsText(candidateReviewText, "Console");

            bool processNoteReady = ContainsText(processNoteText, BatchSlug)
                && ContainsText(processNoteText, "deterministic Pillow generation")
                && ContainsText(processNoteText, "No Unity `.meta` files")
                && ContainsText(processNoteText, "Cat consistency impact: none");

            report.SetNoteReadiness(reviewNoteReady, processNoteReady);

            Require(
                report,
                reviewNoteReady,
                "Batch 75 candidate review note states template-only, non-cat, non-import status and lists required resolution/Console checks.",
                "Batch 75 candidate review note is missing required template-only or UI scale review tokens.");

            Require(
                report,
                processNoteReady,
                "Batch 75 process note records deterministic generation, no-meta guard, cat-art isolation, and follow-up capture path.",
                "Batch 75 process note is missing required generation, no-meta, or cat-isolation tokens.");
        }

        private static int CountExpectedSubjectIds(HashSet<string> subjectIds)
        {
            int count = 0;
            for (int i = 0; i < ExpectedTemplateSubjectIds.Length; i++)
            {
                if (subjectIds.Contains(ExpectedTemplateSubjectIds[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountExpectedSurfaces(HashSet<string> surfaces)
        {
            int count = 0;
            if (surfaces.Contains(P0ChineseUiScaleValidationPlan.MainMenuSurfaceId))
            {
                count++;
            }

            if (surfaces.Contains(P0ChineseUiScaleValidationPlan.RouteMapSurfaceId))
            {
                count++;
            }

            if (surfaces.Contains(P0ChineseUiScaleValidationPlan.BattleHudSurfaceId))
            {
                count++;
            }

            if (surfaces.Contains(P0ChineseUiScaleValidationPlan.SkillEnemyHudSurfaceId))
            {
                count++;
            }

            if (surfaces.Contains(P0ChineseUiScaleValidationPlan.ResultSettingsSurfaceId))
            {
                count++;
            }

            return count;
        }

        private static int CountExpectedResolutions(HashSet<string> resolutions)
        {
            int count = 0;
            if (resolutions.Contains("1024x768"))
            {
                count++;
            }

            if (resolutions.Contains("1280x720"))
            {
                count++;
            }

            if (resolutions.Contains("1600x900"))
            {
                count++;
            }

            if (resolutions.Contains("1920x1080"))
            {
                count++;
            }

            return count;
        }

        private static int CountExpectedSurfaceResolutionPairs(HashSet<string> pairs)
        {
            string[] surfaces =
            {
                P0ChineseUiScaleValidationPlan.MainMenuSurfaceId,
                P0ChineseUiScaleValidationPlan.RouteMapSurfaceId,
                P0ChineseUiScaleValidationPlan.BattleHudSurfaceId,
                P0ChineseUiScaleValidationPlan.SkillEnemyHudSurfaceId,
                P0ChineseUiScaleValidationPlan.ResultSettingsSurfaceId
            };

            string[] resolutions =
            {
                "1024x768",
                "1280x720",
                "1600x900",
                "1920x1080"
            };

            int count = 0;
            for (int surfaceIndex = 0; surfaceIndex < surfaces.Length; surfaceIndex++)
            {
                for (int resolutionIndex = 0; resolutionIndex < resolutions.Length; resolutionIndex++)
                {
                    if (pairs.Contains(surfaces[surfaceIndex] + "|" + resolutions[resolutionIndex]))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private static List<string[]> ParseCsvRows(string text)
        {
            List<string[]> rows = new List<string[]>();
            if (string.IsNullOrEmpty(text))
            {
                return rows;
            }

            List<string> fields = new List<string>();
            StringBuilder field = new StringBuilder();
            bool inQuotes = false;
            bool hasContent = false;

            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                if (inQuotes)
                {
                    if (ch == '"')
                    {
                        if (i + 1 < text.Length && text[i + 1] == '"')
                        {
                            field.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(ch);
                    }

                    hasContent = true;
                    continue;
                }

                if (ch == '"')
                {
                    inQuotes = true;
                    hasContent = true;
                }
                else if (ch == ',')
                {
                    fields.Add(field.ToString());
                    field.Length = 0;
                    hasContent = true;
                }
                else if (ch == '\r' || ch == '\n')
                {
                    if (ch == '\r' && i + 1 < text.Length && text[i + 1] == '\n')
                    {
                        i++;
                    }

                    AddCsvRow(rows, fields, field, hasContent);
                    hasContent = false;
                }
                else
                {
                    field.Append(ch);
                    hasContent = true;
                }
            }

            AddCsvRow(rows, fields, field, hasContent);
            return rows;
        }

        private static void AddCsvRow(
            List<string[]> rows,
            List<string> fields,
            StringBuilder field,
            bool hasContent)
        {
            if (!hasContent && fields.Count == 0 && field.Length == 0)
            {
                return;
            }

            fields.Add(field.ToString());
            rows.Add(fields.ToArray());
            fields.Clear();
            field.Length = 0;
        }

        private static int DataRowCount(List<string[]> rows)
        {
            return rows == null || rows.Count == 0 ? 0 : rows.Count - 1;
        }

        private static int FindHeaderIndex(List<string[]> rows, string header)
        {
            if (rows == null || rows.Count == 0 || rows[0] == null)
            {
                return -1;
            }

            string[] headers = rows[0];
            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i] == header)
                {
                    return i;
                }
            }

            return -1;
        }

        private static string Field(string[] row, int index)
        {
            if (row == null || index < 0 || index >= row.Length)
            {
                return string.Empty;
            }

            return row[index] ?? string.Empty;
        }

        private static bool StartsWithAssetsPath(string path)
        {
            string normalized = NormalizePath(path);
            return normalized.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase);
        }

        private static string CombinePath(string directory, string fileName)
        {
            string normalizedDirectory = NormalizePath(directory).TrimEnd('/');
            if (string.IsNullOrWhiteSpace(normalizedDirectory))
            {
                return fileName ?? string.Empty;
            }

            return normalizedDirectory + "/" + (fileName ?? string.Empty);
        }

        private static string NormalizePath(string path)
        {
            return (path ?? string.Empty).Replace('\\', '/');
        }

        private static bool ContainsText(string text, string token)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(token))
            {
                return false;
            }

            return text.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string ResolveFilePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            if (File.Exists(path))
            {
                return path;
            }

            string normalized = NormalizePath(path);
            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 8 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, normalized.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return path;
        }

        private static string ResolveDirectory(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                return string.Empty;
            }

            if (Directory.Exists(directory))
            {
                return directory;
            }

            string normalized = NormalizePath(directory);
            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 8 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, normalized.Replace('/', Path.DirectorySeparatorChar));
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return directory;
        }

        private static string RelativePath(string root, string path)
        {
            Uri rootUri = new Uri(AppendDirectorySeparatorChar(Path.GetFullPath(root)));
            Uri pathUri = new Uri(Path.GetFullPath(path));
            return Uri.UnescapeDataString(rootUri.MakeRelativeUri(pathUri).ToString());
        }

        private static string AppendDirectorySeparatorChar(string path)
        {
            if (string.IsNullOrEmpty(path) || path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                return path;
            }

            return path + Path.DirectorySeparatorChar;
        }

        private static void Require(
            P0ChineseUiScaleEvidencePacketReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
            }
            else
            {
                report.AddIssue(P0ChineseUiScaleEvidenceSeverity.Failure, failureMessage);
            }
        }
    }
}
