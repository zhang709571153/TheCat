using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TheCat.Data.Catalogs;

namespace TheCat.Tools
{
    public enum P0StarterCatStrictCandidateSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatStrictCandidateIssue
    {
        public P0StarterCatStrictCandidateIssue(P0StarterCatStrictCandidateSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatStrictCandidateSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatStrictCandidateEntry
    {
        public P0StarterCatStrictCandidateEntry(
            string catId,
            string displayName,
            string batchSlug,
            string manifestPath,
            string alphaCandidatePath,
            string reviewSheetPath,
            string reviewNotePath,
            string processNotePath,
            string agentPromptPath,
            string baselineCandidatePath,
            string sourceLockId,
            string activeScreenshotFileName)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            BatchSlug = batchSlug ?? string.Empty;
            ManifestPath = manifestPath ?? string.Empty;
            AlphaCandidatePath = alphaCandidatePath ?? string.Empty;
            ReviewSheetPath = reviewSheetPath ?? string.Empty;
            ReviewNotePath = reviewNotePath ?? string.Empty;
            ProcessNotePath = processNotePath ?? string.Empty;
            AgentPromptPath = agentPromptPath ?? string.Empty;
            BaselineCandidatePath = baselineCandidatePath ?? string.Empty;
            SourceLockId = sourceLockId ?? string.Empty;
            ActiveScreenshotFileName = activeScreenshotFileName ?? string.Empty;
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string BatchSlug { get; }

        public string ManifestPath { get; }

        public string AlphaCandidatePath { get; }

        public string ReviewSheetPath { get; }

        public string ReviewNotePath { get; }

        public string ProcessNotePath { get; }

        public string AgentPromptPath { get; }

        public string BaselineCandidatePath { get; }

        public string SourceLockId { get; }

        public string ActiveScreenshotFileName { get; }
    }

    public sealed class P0StarterCatStrictCandidateEvidenceReport
    {
        private readonly List<P0StarterCatStrictCandidateIssue> issues = new List<P0StarterCatStrictCandidateIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0StarterCatStrictCandidateEntry> entries = new List<P0StarterCatStrictCandidateEntry>();

        public IReadOnlyList<P0StarterCatStrictCandidateIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0StarterCatStrictCandidateEntry> Entries => entries.AsReadOnly();

        public int CandidateCount { get; private set; }

        public int ManifestCount { get; private set; }

        public int AlphaCandidateCount { get; private set; }

        public int ReviewSheetCount { get; private set; }

        public int ReviewNoteCount { get; private set; }

        public int ProcessNoteCount { get; private set; }

        public int AgentPromptCount { get; private set; }

        public int ExplicitBlockNoteCount { get; private set; }

        public int BaselineMentionCount { get; private set; }

        public int ActiveScreenshotMentionCount { get; private set; }

        public int ManifestRecommendationCount { get; private set; }

        public int SourceTurnaroundExactPathCount { get; private set; }

        public int Batch47SpecManifestLockCount { get; private set; }

        public int Batch47SpecJsonIdentityLockCount { get; private set; }

        public int FailureCount => Count(P0StarterCatStrictCandidateSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatStrictCandidateEvidence.ExpectedCoveredCheckCount;

        public void SetCounts(
            int candidateCount,
            int manifestCount,
            int alphaCandidateCount,
            int reviewSheetCount,
            int reviewNoteCount,
            int processNoteCount,
            int agentPromptCount,
            int explicitBlockNoteCount,
            int baselineMentionCount,
            int activeScreenshotMentionCount,
            int manifestRecommendationCount,
            int sourceTurnaroundExactPathCount,
            int batch47SpecManifestLockCount,
            int batch47SpecJsonIdentityLockCount)
        {
            CandidateCount = candidateCount;
            ManifestCount = manifestCount;
            AlphaCandidateCount = alphaCandidateCount;
            ReviewSheetCount = reviewSheetCount;
            ReviewNoteCount = reviewNoteCount;
            ProcessNoteCount = processNoteCount;
            AgentPromptCount = agentPromptCount;
            ExplicitBlockNoteCount = explicitBlockNoteCount;
            BaselineMentionCount = baselineMentionCount;
            ActiveScreenshotMentionCount = activeScreenshotMentionCount;
            ManifestRecommendationCount = manifestRecommendationCount;
            SourceTurnaroundExactPathCount = sourceTurnaroundExactPathCount;
            Batch47SpecManifestLockCount = batch47SpecManifestLockCount;
            Batch47SpecJsonIdentityLockCount = batch47SpecJsonIdentityLockCount;
        }

        public void AddEntry(P0StarterCatStrictCandidateEntry entry)
        {
            if (entry != null)
            {
                entries.Add(entry);
            }
        }

        public void AddIssue(P0StarterCatStrictCandidateSeverity severity, string message)
        {
            issues.Add(new P0StarterCatStrictCandidateIssue(severity, message));
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
                ? "P0 starter cat strict candidate evidence ready for " + CandidateCount + " starter cat(s)."
                : "P0 starter cat strict candidate evidence has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Candidates: " + CandidateCount,
                "Manifests: " + ManifestCount,
                "Alpha candidates: " + AlphaCandidateCount,
                "Review sheets: " + ReviewSheetCount,
                "Review notes: " + ReviewNoteCount,
                "Process notes: " + ProcessNoteCount,
                "Agent prompts: " + AgentPromptCount,
                "Explicit block notes: " + ExplicitBlockNoteCount,
                "Baseline mentions: " + BaselineMentionCount,
                "Active screenshot mentions: " + ActiveScreenshotMentionCount,
                "Manifest safe recommendations: " + ManifestRecommendationCount,
                "Exact source turnaround paths: " + SourceTurnaroundExactPathCount,
                "Batch 47 spec manifest locks: " + Batch47SpecManifestLockCount,
                "Batch 47 JSON identity locks: " + Batch47SpecJsonIdentityLockCount
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

        private int Count(P0StarterCatStrictCandidateSeverity severity)
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

    public static class P0StarterCatStrictCandidateEvidence
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedCoveredCheckCount = 12;
        public const string Batch47StrictGenerationSpecSlug = "batch_47_starter_cat_strict_generation_spec_pack_2026-06-15";

        public static IReadOnlyList<P0StarterCatStrictCandidateEntry> CreateP0Candidates()
        {
            return new[]
            {
                new P0StarterCatStrictCandidateEntry(
                    P0PrototypeCatalog.SaibanId,
                    "Saiban / Sword Saint",
                    "batch_49_saiban_low_drift_refinement_2026-06-15",
                    "design/development/asset_candidates/starter_cats/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_manifest.csv",
                    "design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/thecat_cat_saiban_batch49_low_drift_alpha_1024_candidate_v001.png",
                    "design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/thecat_cat_saiban_batch49_low_drift_refinement_review_sheet.png",
                    "design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_review.md",
                    "design/development/asset_candidates/starter_cats/saiban/batch_49_saiban_low_drift_refinement_2026-06-15/saiban_batch49_low_drift_refinement_process_note.md",
                    "design/development/agent_prompts/p0_asset_batch_49_saiban_low_drift_refinement.md",
                    "design/development/asset_candidates/starter_cats/saiban/batch_48_saiban_ai_generation_pilot_2026-06-15/thecat_cat_saiban_batch48_ai_generation_alpha_1024_candidate_v001.png",
                    "saiban_turnaround_colored",
                    P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName),
                new P0StarterCatStrictCandidateEntry(
                    P0PrototypeCatalog.NephthysId,
                    "Nephthys / Moon-Sand Agent",
                    "batch_50_nephthys_strict_ai_generation_candidate_2026-06-15",
                    "design/development/asset_candidates/starter_cats/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_manifest.csv",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/thecat_cat_nephthys_batch50_strict_ai_alpha_1024_candidate_v001.png",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/thecat_cat_nephthys_batch50_strict_ai_generation_review_sheet.png",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_candidate_review.md",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_50_nephthys_strict_ai_generation_candidate_2026-06-15/nephthys_batch50_strict_ai_generation_process_note.md",
                    "design/development/agent_prompts/p0_asset_batch_50_nephthys_strict_ai_generation_candidate.md",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_37_nephthys_cutout_candidate_2026-06-15/thecat_cat_nephthys_cutout_alpha_1024_candidate_v001.png",
                    "nephthys_turnaround_colored",
                    P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName),
                new P0StarterCatStrictCandidateEntry(
                    P0PrototypeCatalog.SuzuneId,
                    "Suzune / Sleep Shrine Healer",
                    "batch_51_suzune_strict_ai_generation_candidate_2026-06-15",
                    "design/development/asset_candidates/starter_cats/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_manifest.csv",
                    "design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/thecat_cat_suzune_batch51_strict_ai_alpha_1024_candidate_v001.png",
                    "design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/thecat_cat_suzune_batch51_strict_ai_generation_review_sheet.png",
                    "design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_candidate_review.md",
                    "design/development/asset_candidates/starter_cats/suzune/batch_51_suzune_strict_ai_generation_candidate_2026-06-15/suzune_batch51_strict_ai_generation_process_note.md",
                    "design/development/agent_prompts/p0_asset_batch_51_suzune_strict_ai_generation_candidate.md",
                    "design/development/asset_candidates/starter_cats/suzune/batch_35_suzune_cutout_candidate_2026-06-15/thecat_cat_suzune_cutout_alpha_1024_candidate_v001.png",
                    "suzune_turnaround_colored",
                    P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName)
            };
        }

        public static P0StarterCatStrictCandidateEvidenceReport EvaluateCurrentCandidates()
        {
            return Evaluate(CreateP0Candidates(), DefaultFileExists, DefaultReadText);
        }

        public static P0StarterCatStrictCandidateEvidenceReport Evaluate(
            IReadOnlyList<P0StarterCatStrictCandidateEntry> candidates,
            Func<string, bool> fileExists,
            P0StarterCatReviewTextReader readText)
        {
            IReadOnlyList<P0StarterCatStrictCandidateEntry> entries = candidates ?? Array.Empty<P0StarterCatStrictCandidateEntry>();
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            P0StarterCatStrictCandidateEvidenceReport report = new P0StarterCatStrictCandidateEvidenceReport();

            int manifestCount = 0;
            int alphaCandidateCount = 0;
            int reviewSheetCount = 0;
            int reviewNoteCount = 0;
            int processNoteCount = 0;
            int agentPromptCount = 0;
            int explicitBlockNoteCount = 0;
            int baselineMentionCount = 0;
            int activeScreenshotMentionCount = 0;
            int manifestRecommendationCount = 0;
            int sourceTurnaroundExactPathCount = 0;
            int batch47SpecManifestLockCount = 0;
            int batch47SpecJsonIdentityLockCount = 0;
            bool allExpectedCatsPresent = entries.Count == ExpectedStarterCatCount;
            bool allFilesPresent = true;
            bool allCandidatesOutsideAssets = true;
            bool allNoMeta = true;
            bool allManifestSafe = true;
            bool allReviewNotesBlockImport = true;
            bool allPromptsUseBuiltInImageGen = true;
            bool allManifestsUseExactSourceLocks = true;
            bool allManifestsUseBatch47Specs = true;
            bool allBatch47SpecJsonsUseIdentityLocks = true;

            for (int i = 0; i < entries.Count; i++)
            {
                P0StarterCatStrictCandidateEntry entry = entries[i];
                report.AddEntry(entry);

                CheckFile(entry.ManifestPath, exists, report, ref manifestCount, ref allFilesPresent, "manifest");
                CheckFile(entry.AlphaCandidatePath, exists, report, ref alphaCandidateCount, ref allFilesPresent, "alpha candidate");
                CheckFile(entry.ReviewSheetPath, exists, report, ref reviewSheetCount, ref allFilesPresent, "review sheet");
                CheckFile(entry.ProcessNotePath, exists, report, ref processNoteCount, ref allFilesPresent, "process note");
                CheckFile(entry.AgentPromptPath, exists, report, ref agentPromptCount, ref allFilesPresent, "agent prompt");

                string reviewError = string.Empty;
                if (CheckFile(entry.ReviewNotePath, exists, report, ref reviewNoteCount, ref allFilesPresent, "review note")
                    && reader(entry.ReviewNotePath, out string reviewText, out reviewError))
                {
                    if (IsExplicitBlockNote(reviewText))
                    {
                        explicitBlockNoteCount++;
                    }
                    else
                    {
                        allReviewNotesBlockImport = false;
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " review note does not explicitly block Unity import.");
                    }

                    if (ContainsText(reviewText, entry.BaselineCandidatePath) || ContainsText(reviewText, BaselineBatchToken(entry.CatId)))
                    {
                        baselineMentionCount++;
                    }
                    else
                    {
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " review note does not mention its baseline candidate.");
                    }

                    if (ContainsText(reviewText, entry.ActiveScreenshotFileName))
                    {
                        activeScreenshotMentionCount++;
                    }
                    else
                    {
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " review note does not mention active screenshot " + entry.ActiveScreenshotFileName + ".");
                    }
                }
                else if (exists(entry.ReviewNotePath))
                {
                    allReviewNotesBlockImport = false;
                    report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " review note could not be read: " + reviewError);
                }

                if (reader(entry.ManifestPath, out string manifestText, out _))
                {
                    P0StarterCatTurnaroundSourceLockEntry sourceLock = FindSourceLock(entry.CatId);
                    string batch47JsonPath = Batch47JsonPathFor(entry.CatId);
                    string batch47CardPath = Batch47CardPathFor(entry.CatId);
                    string batch47JsonSha256 = ComputeSha256(batch47JsonPath);
                    string batch47CardSha256 = ComputeSha256(batch47CardPath);

                    if (ContainsText(manifestText, entry.CatId)
                        && ContainsText(manifestText, entry.SourceLockId)
                        && ContainsText(manifestText, entry.ActiveScreenshotFileName)
                        && ContainsText(manifestText, "candidate_review_only_do_not_import"))
                    {
                        manifestRecommendationCount++;
                    }
                    else
                    {
                        allManifestSafe = false;
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " manifest does not include safe recommendation, source lock, and active screenshot.");
                    }

                    if (ContainsText(manifestText, sourceLock.SourceTurnaroundPath)
                        && ContainsText(manifestText, sourceLock.SourceTurnaroundSha256)
                        && !ContainsMojibakePath(manifestText))
                    {
                        sourceTurnaroundExactPathCount++;
                    }
                    else
                    {
                        allManifestsUseExactSourceLocks = false;
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " manifest does not use the exact colored-turnaround source path and SHA-256 from the source lock.");
                    }

                    if (ContainsText(manifestText, batch47JsonPath)
                        && ContainsText(manifestText, batch47CardPath)
                        && ContainsText(manifestText, batch47JsonSha256)
                        && ContainsText(manifestText, batch47CardSha256))
                    {
                        batch47SpecManifestLockCount++;
                    }
                    else
                    {
                        allManifestsUseBatch47Specs = false;
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " manifest does not lock the current Batch 47 strict-generation JSON/card paths and hashes.");
                    }

                    if (reader(batch47JsonPath, out string batch47JsonText, out string batch47JsonError))
                    {
                        if (Batch47SpecJsonHasIdentityLock(batch47JsonText, sourceLock, entry))
                        {
                            batch47SpecJsonIdentityLockCount++;
                        }
                        else
                        {
                            allBatch47SpecJsonsUseIdentityLocks = false;
                            report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " Batch 47 JSON spec is missing exact source path, source lock, non-human body rule, must-keep, reject, prompt, or blocked recommendation clauses.");
                        }
                    }
                    else
                    {
                        allBatch47SpecJsonsUseIdentityLocks = false;
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " Batch 47 JSON spec could not be read: " + batch47JsonError);
                    }
                }

                if (reader(entry.AgentPromptPath, out string promptText, out _))
                {
                    if (!ContainsText(promptText, "built-in image_gen")
                        || !ContainsText(promptText, "chroma-key")
                        || !ContainsText(promptText, "do not import into Unity")
                        || ContainsText(promptText, "?assets"))
                    {
                        allPromptsUseBuiltInImageGen = false;
                        report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " agent prompt is missing imagegen/chroma/import-safety clauses or contains mojibake path text.");
                    }
                }

                if (StartsWithAssets(entry.AlphaCandidatePath)
                    || StartsWithAssets(entry.ReviewSheetPath)
                    || StartsWithAssets(entry.ManifestPath))
                {
                    allCandidatesOutsideAssets = false;
                }

                if (exists(entry.AlphaCandidatePath + ".meta") || exists(entry.ReviewSheetPath + ".meta"))
                {
                    allNoMeta = false;
                    report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, entry.CatId + " strict candidate has Unity .meta files in the candidate folder.");
                }
            }

            report.SetCounts(
                entries.Count,
                manifestCount,
                alphaCandidateCount,
                reviewSheetCount,
                reviewNoteCount,
                processNoteCount,
                agentPromptCount,
                explicitBlockNoteCount,
                baselineMentionCount,
                activeScreenshotMentionCount,
                manifestRecommendationCount,
                sourceTurnaroundExactPathCount,
                batch47SpecManifestLockCount,
                batch47SpecJsonIdentityLockCount);

            Require(report, allExpectedCatsPresent, "Strict candidate evidence covers exactly Saiban, Nephthys, and Suzune.", "Strict candidate evidence does not cover exactly the three starter cats.");
            Require(report, allFilesPresent, "Strict candidate manifests, alpha candidates, review sheets, notes, process notes, and prompts all exist.", "One or more strict candidate files are missing.");
            Require(report, allCandidatesOutsideAssets, "Strict candidate files remain outside Assets until Unity active-cat review approves import.", "Strict candidate files must not be staged inside Assets.");
            Require(report, allNoMeta, "Strict candidate folders have no Unity .meta files.", "Strict candidate folders contain Unity .meta files.");
            Require(report, allManifestSafe && manifestRecommendationCount == ExpectedStarterCatCount, "Strict candidate manifests keep candidate_review_only_do_not_import recommendations.", "Strict candidate manifests are missing safe recommendation coverage.");
            Require(report, allReviewNotesBlockImport && explicitBlockNoteCount == ExpectedStarterCatCount, "Strict candidate review notes explicitly block Unity import before active-cat review.", "Strict candidate review notes do not consistently block import.");
            Require(report, baselineMentionCount == ExpectedStarterCatCount, "Strict candidate review notes compare each new candidate to its prior baseline.", "Strict candidate review notes are missing baseline comparisons.");
            Require(report, activeScreenshotMentionCount == ExpectedStarterCatCount, "Strict candidate review notes name all active-cat screenshot gates.", "Strict candidate review notes are missing active-cat screenshot gates.");
            Require(report, allPromptsUseBuiltInImageGen, "Strict candidate prompts require built-in image_gen, chroma-key cleanup, and real source paths.", "Strict candidate prompts are incomplete or stale.");
            Require(report, allManifestsUseExactSourceLocks && sourceTurnaroundExactPathCount == ExpectedStarterCatCount, "Strict candidate manifests use exact colored-turnaround source paths and SHA-256 locks.", "Strict candidate manifests are missing exact colored-turnaround source locks.");
            Require(report, allManifestsUseBatch47Specs && batch47SpecManifestLockCount == ExpectedStarterCatCount, "Strict candidate manifests lock the current Batch 47 JSON/card paths and hashes.", "Strict candidate manifests are missing Batch 47 strict-generation spec locks.");
            Require(report, allBatch47SpecJsonsUseIdentityLocks && batch47SpecJsonIdentityLockCount == ExpectedStarterCatCount, "Batch 47 JSON specs keep exact source-lock, non-human body, must-keep, reject, prompt, and blocked-import identity clauses.", "Batch 47 JSON strict-generation specs are incomplete or stale.");
            return report;
        }

        private static bool CheckFile(
            string path,
            Func<string, bool> exists,
            P0StarterCatStrictCandidateEvidenceReport report,
            ref int count,
            ref bool allFilesPresent,
            string label)
        {
            if (exists(path))
            {
                count++;
                return true;
            }

            allFilesPresent = false;
            report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, "Missing strict candidate " + label + ": " + path);
            return false;
        }

        private static bool IsExplicitBlockNote(string text)
        {
            return ContainsText(text, "candidate review only")
                && ContainsText(text, "do not import into Unity")
                && ContainsText(text, "active-cat");
        }

        private static string BaselineBatchToken(string catId)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return "Batch 48";
                case P0PrototypeCatalog.NephthysId:
                    return "Batch 37";
                case P0PrototypeCatalog.SuzuneId:
                    return "Batch 35";
                default:
                    return string.Empty;
            }
        }

        private static string Batch47JsonPathFor(string catId)
        {
            return "design/development/asset_candidates/starter_cats/"
                + catId
                + "/"
                + Batch47StrictGenerationSpecSlug
                + "/thecat_cat_"
                + catId
                + "_batch47_strict_generation_spec_v001.json";
        }

        private static string Batch47CardPathFor(string catId)
        {
            return "design/development/asset_candidates/starter_cats/"
                + catId
                + "/"
                + Batch47StrictGenerationSpecSlug
                + "/thecat_cat_"
                + catId
                + "_batch47_strict_generation_spec_card_v001.png";
        }

        private static P0StarterCatTurnaroundSourceLockEntry FindSourceLock(string catId)
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].CatId == catId)
                {
                    return locks[i];
                }
            }

            return default(P0StarterCatTurnaroundSourceLockEntry);
        }

        private static bool Batch47SpecJsonHasIdentityLock(
            string text,
            P0StarterCatTurnaroundSourceLockEntry sourceLock,
            P0StarterCatStrictCandidateEntry entry)
        {
            return ContainsText(text, "\"cat_id\": \"" + entry.CatId + "\"")
                && ContainsText(text, "\"source_lock_id\": \"" + entry.SourceLockId + "\"")
                && ContainsText(text, "\"source_turnaround_path\": \"" + sourceLock.SourceTurnaroundPath + "\"")
                && ContainsText(text, "\"body_rule\"")
                && ContainsText(text, "\"must_keep\"")
                && ContainsText(text, "\"reject\"")
                && ContainsText(text, "\"positive_prompt\"")
                && ContainsText(text, "\"negative_prompt\"")
                && ContainsText(text, "non-human")
                && ContainsText(text, "human")
                && ContainsText(text, "palette drift")
                && ContainsText(text, "strict_generation_spec_only_do_not_import")
                && !ContainsMojibakePath(text);
        }

        private static bool ContainsMojibakePath(string text)
        {
            return ContainsText(text, "?assets");
        }

        private static string ComputeSha256(string path)
        {
            string resolved = ResolveFile(path);
            if (string.IsNullOrWhiteSpace(resolved) || !File.Exists(resolved))
            {
                return string.Empty;
            }

            using (SHA256 sha = SHA256.Create())
            using (FileStream stream = File.OpenRead(resolved))
            {
                byte[] hash = sha.ComputeHash(stream);
                StringBuilder builder = new StringBuilder(hash.Length * 2);
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private static bool StartsWithAssets(string path)
        {
            return path != null && path.StartsWith("Assets/", StringComparison.OrdinalIgnoreCase);
        }

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            return File.Exists(ResolveFile(path));
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

        private static bool ContainsText(string value, string token)
        {
            return value != null && token != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0StarterCatStrictCandidateEvidenceReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatStrictCandidateSeverity.Failure, failureMessage);
        }
    }
}
