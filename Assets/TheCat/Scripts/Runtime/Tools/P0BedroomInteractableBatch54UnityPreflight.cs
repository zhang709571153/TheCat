using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public sealed class P0BedroomInteractableBatch54UnityPreflightReport
    {
        private readonly List<string> issues = new List<string>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<string> blockingItems = new List<string>();
        private readonly List<P0BedroomInteractableBatch54ManifestRow> selectedRows = new List<P0BedroomInteractableBatch54ManifestRow>();

        public IReadOnlyList<string> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<string> BlockingItems => blockingItems.AsReadOnly();

        public IReadOnlyList<P0BedroomInteractableBatch54ManifestRow> SelectedRows => selectedRows.AsReadOnly();

        public int ManifestRowCount { get; private set; }

        public int SubjectCount { get; private set; }

        public int AlphaCandidateCount { get; private set; }

        public int CandidateOutsideAssetsCount { get; private set; }

        public int CandidateNoMetaCount { get; private set; }

        public int CandidateRecommendationLockedCount { get; private set; }

        public int CurrentUnitySpriteCount { get; private set; }

        public int CurrentUnitySpriteMetaCount { get; private set; }

        public int SourceLockCount { get; private set; }

        public int RuntimeBindingCount { get; private set; }

        public int UnityEvidenceCount { get; private set; }

        public int UnityEvidenceRequiredCount { get; private set; }

        public bool QueueEntryReadyForUnityReview { get; private set; }

        public bool UnityEditorValidationReady { get; private set; }

        public bool FormalInstallAllowed { get; private set; }

        public int FailureCount => issues.Count;

        public bool IsReadyForUnityPreflight => FailureCount == 0
            && coveredChecks.Count >= P0BedroomInteractableBatch54UnityPreflight.ExpectedCoveredCheckCount;

        public void SetCounts(
            int manifestRowCount,
            int subjectCount,
            int alphaCandidateCount,
            int candidateOutsideAssetsCount,
            int candidateNoMetaCount,
            int candidateRecommendationLockedCount,
            int currentUnitySpriteCount,
            int currentUnitySpriteMetaCount,
            int sourceLockCount,
            int runtimeBindingCount,
            int unityEvidenceCount,
            int unityEvidenceRequiredCount,
            bool queueEntryReadyForUnityReview,
            bool unityEditorValidationReady)
        {
            ManifestRowCount = manifestRowCount;
            SubjectCount = subjectCount;
            AlphaCandidateCount = alphaCandidateCount;
            CandidateOutsideAssetsCount = candidateOutsideAssetsCount;
            CandidateNoMetaCount = candidateNoMetaCount;
            CandidateRecommendationLockedCount = candidateRecommendationLockedCount;
            CurrentUnitySpriteCount = currentUnitySpriteCount;
            CurrentUnitySpriteMetaCount = currentUnitySpriteMetaCount;
            SourceLockCount = sourceLockCount;
            RuntimeBindingCount = runtimeBindingCount;
            UnityEvidenceCount = unityEvidenceCount;
            UnityEvidenceRequiredCount = unityEvidenceRequiredCount;
            QueueEntryReadyForUnityReview = queueEntryReadyForUnityReview;
            UnityEditorValidationReady = unityEditorValidationReady;
            FormalInstallAllowed = IsReadyForUnityPreflight
                && UnityEditorValidationReady
                && UnityEvidenceCount >= UnityEvidenceRequiredCount;
        }

        public void AddSelectedRow(P0BedroomInteractableBatch54ManifestRow row)
        {
            selectedRows.Add(row);
        }

        public void AddIssue(string issue)
        {
            if (!string.IsNullOrWhiteSpace(issue))
            {
                issues.Add(issue);
            }
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public void AddBlockingItem(string item)
        {
            if (!string.IsNullOrWhiteSpace(item))
            {
                blockingItems.Add(item);
            }
        }

        public string BuildSummary()
        {
            if (!IsReadyForUnityPreflight)
            {
                return "Batch 54 bedroom interactable Unity preflight has " + FailureCount + " failure(s).";
            }

            return "Batch 54 bedroom interactable candidates are ready for Unity review; formal install remains blocked by runtime evidence.";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Manifest rows: " + ManifestRowCount,
                "Subjects: " + SubjectCount,
                "Alpha candidates: " + AlphaCandidateCount,
                "Candidates outside Assets: " + CandidateOutsideAssetsCount,
                "Candidates without Unity meta: " + CandidateNoMetaCount,
                "Candidate recommendation locks: " + CandidateRecommendationLockedCount,
                "Current Unity sprites: " + CurrentUnitySpriteCount,
                "Current Unity sprite meta files: " + CurrentUnitySpriteMetaCount,
                "Source locks: " + SourceLockCount,
                "Runtime bindings: " + RuntimeBindingCount,
                "Unity evidence: " + UnityEvidenceCount + "/" + UnityEvidenceRequiredCount,
                "Queue entry ready for Unity review: " + (QueueEntryReadyForUnityReview ? "yes" : "no"),
                "Unity editor validation ready: " + (UnityEditorValidationReady ? "yes" : "no"),
                "Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no")
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < blockingItems.Count; i++)
            {
                lines.Add("[Blocked] " + blockingItems[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[Failure] " + issues[i]);
            }

            return string.Join(Environment.NewLine, lines);
        }

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Batch 54 Bedroom Interactable Unity Preflight");
            builder.AppendLine();
            builder.AppendLine(BuildSummary());
            builder.AppendLine();
            builder.AppendLine("## Decision");
            builder.AppendLine();
            builder.AppendLine("- Ready for Unity review: " + (IsReadyForUnityPreflight ? "yes" : "no"));
            builder.AppendLine("- Formal install allowed: " + (FormalInstallAllowed ? "yes" : "no"));
            builder.AppendLine("- Unity evidence: " + UnityEvidenceCount + "/" + UnityEvidenceRequiredCount);
            builder.AppendLine("- Unity editor validation ready: " + (UnityEditorValidationReady ? "yes" : "no"));
            builder.AppendLine("- Candidate policy: `candidate_review_only_do_not_import`");
            builder.AppendLine();
            builder.AppendLine("## Selected Candidate Rows");
            builder.AppendLine();
            builder.AppendLine("| subject | candidate | current Unity sprite | source lock |");
            builder.AppendLine("| --- | --- | --- | --- |");

            for (int i = 0; i < selectedRows.Count; i++)
            {
                P0BedroomInteractableBatch54ManifestRow row = selectedRows[i];
                builder.Append("| ");
                builder.Append(EscapeTable(row.SubjectId));
                builder.Append(" | `");
                builder.Append(EscapeTable(row.CandidatePath));
                builder.Append("` | `");
                builder.Append(EscapeTable(row.CurrentUnitySpritePath));
                builder.Append("` | `");
                builder.Append(EscapeTable(row.SourceLockId));
                builder.AppendLine("` |");
            }

            builder.AppendLine();
            builder.AppendLine("## Blocking Unity Evidence");
            builder.AppendLine();
            if (blockingItems.Count == 0)
            {
                builder.AppendLine("- none");
            }
            else
            {
                for (int i = 0; i < blockingItems.Count; i++)
                {
                    builder.AppendLine("- " + blockingItems[i]);
                }
            }

            builder.AppendLine();
            builder.AppendLine("## Protected Runtime State");
            builder.AppendLine();
            builder.AppendLine("- Current runtime prop sprites remain under `Assets/TheCat/Art/Scenes/BedroomDream`.");
            builder.AppendLine("- Batch 54 PNGs remain candidate-only under `design/development/asset_candidates`.");
            builder.AppendLine("- Do not replace bed, litter box, or feeder sprites until Unity screenshot scale, Console, pathing, interaction feedback, and prefab/scene binding evidence all pass.");

            if (issues.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine("## Issues");
                builder.AppendLine();
                for (int i = 0; i < issues.Count; i++)
                {
                    builder.AppendLine("- [Failure] " + issues[i]);
                }
            }

            return builder.ToString();
        }

        private static string EscapeTable(string value)
        {
            return (value ?? string.Empty)
                .Replace("|", "\\|")
                .Replace("\r", " ")
                .Replace("\n", " ");
        }
    }

    public readonly struct P0BedroomInteractableBatch54ManifestRow
    {
        private readonly IReadOnlyDictionary<string, string> values;

        public P0BedroomInteractableBatch54ManifestRow(IReadOnlyDictionary<string, string> values)
        {
            this.values = values ?? new Dictionary<string, string>();
        }

        public string SubjectId => Get("subject_id");

        public string AssetType => Get("asset_type");

        public string CandidatePath => Get("candidate_path");

        public string SourceReferencePath => Get("source_reference_path");

        public string SourceLockId => Get("source_lock_id");

        public string CurrentUnitySpritePath => Get("current_unity_sprite_path");

        public string Recommendation => Get("recommendation");

        private string Get(string key)
        {
            if (values != null && values.TryGetValue(key, out string value))
            {
                return value ?? string.Empty;
            }

            return string.Empty;
        }
    }

    public static class P0BedroomInteractableBatch54UnityPreflight
    {
        public const int ExpectedCoveredCheckCount = 8;
        public const int ExpectedManifestRowCount = 21;
        public const int ExpectedSubjectCount = 3;
        public const int ExpectedAlphaCandidateCount = 3;
        public const int ExpectedRuntimeBindingCount = 3;
        public const string BatchSlug = "batch_54_bedroom_interactable_candidates_2026-06-15";
        public const string DefaultManifestPath = "design/development/asset_candidates/props/bedroom_interactables/batch_54_bedroom_interactable_candidates_2026-06-15/bedroom_interactables_batch54_manifest.csv";
        public const string CandidateRecommendation = "candidate_review_only_do_not_import";

        private static readonly string[] ExpectedSubjects =
        {
            "bed",
            "litter_box",
            "feeder"
        };

        private static readonly string[] ExpectedSourceLocks =
        {
            "bedroom_map_concept",
            "bedroom_mid_background_sprites"
        };

        private static readonly string[] RequiredUnityEvidencePaths =
        {
            "design/development/asset_review/batch_54_bedroom_interactables_unity_preflight/runtime_scale_screenshot_sheet.md",
            "design/development/asset_review/batch_54_bedroom_interactables_unity_preflight/console_clean_report.md",
            "design/development/asset_review/batch_54_bedroom_interactables_unity_preflight/sprite_import_settings.md",
            "design/development/asset_review/batch_54_bedroom_interactables_unity_preflight/scene_prefab_binding_review.md",
            "design/development/asset_review/batch_54_bedroom_interactables_unity_preflight/pathing_readability_review.md",
            "design/development/asset_review/batch_54_bedroom_interactables_unity_preflight/interaction_feedback_review.md"
        };

        public static P0BedroomInteractableBatch54UnityPreflightReport EvaluateCurrentPreflight()
        {
            return Evaluate(DefaultManifestPath, DefaultReadAllText, DefaultFileExists, unityEditorValidationReady: false);
        }

        public static P0BedroomInteractableBatch54UnityPreflightReport Evaluate(
            string manifestPath,
            Func<string, string> readAllText,
            Func<string, bool> fileExists,
            bool unityEditorValidationReady)
        {
            P0BedroomInteractableBatch54UnityPreflightReport report = new P0BedroomInteractableBatch54UnityPreflightReport();
            Func<string, string> read = readAllText ?? DefaultReadAllText;
            Func<string, bool> exists = fileExists ?? DefaultFileExists;

            bool manifestExists = exists(manifestPath);
            Require(report, manifestExists, "Batch 54 manifest exists for bedroom interactable candidate review.", "Batch 54 manifest is missing.");
            if (!manifestExists)
            {
                report.SetCounts(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, RequiredUnityEvidencePaths.Length, false, unityEditorValidationReady);
                return report;
            }

            IReadOnlyList<P0BedroomInteractableBatch54ManifestRow> rows = ParseRows(read(manifestPath));
            List<string> subjects = new List<string>();
            List<string> alphaSubjects = new List<string>();
            List<string> currentUnitySpritePaths = new List<string>();
            List<string> sourceLocks = new List<string>();

            int candidateOutsideAssetsCount = 0;
            int candidateNoMetaCount = 0;
            int candidateRecommendationLockedCount = 0;
            int currentUnitySpriteCount = 0;
            int currentUnitySpriteMetaCount = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                P0BedroomInteractableBatch54ManifestRow row = rows[i];
                AddUnique(subjects, row.SubjectId);
                AddUnique(sourceLocks, row.SourceLockId);

                if (row.AssetType == "alpha_candidate_1024")
                {
                    AddUnique(alphaSubjects, row.SubjectId);
                    report.AddSelectedRow(row);
                }

                if (IsDesignCandidatePath(row.CandidatePath))
                {
                    candidateOutsideAssetsCount++;
                }

                if (!exists(row.CandidatePath + ".meta"))
                {
                    candidateNoMetaCount++;
                }

                if (row.Recommendation == CandidateRecommendation)
                {
                    candidateRecommendationLockedCount++;
                }

                if (!ContainsOrdinal(row.CurrentUnitySpritePath, "Assets/TheCat/Art/Scenes/BedroomDream/"))
                {
                    report.AddIssue(row.SubjectId + " current Unity sprite path is outside BedroomDream: " + row.CurrentUnitySpritePath);
                }

                if (AddUnique(currentUnitySpritePaths, row.CurrentUnitySpritePath))
                {
                    if (exists(row.CurrentUnitySpritePath))
                    {
                        currentUnitySpriteCount++;
                    }

                    if (exists(row.CurrentUnitySpritePath + ".meta"))
                    {
                        currentUnitySpriteMetaCount++;
                    }
                }
            }

            int runtimeBindingCount = CountRuntimeBindings();
            int unityEvidenceCount = 0;
            for (int i = 0; i < RequiredUnityEvidencePaths.Length; i++)
            {
                string evidencePath = RequiredUnityEvidencePaths[i];
                if (exists(evidencePath))
                {
                    unityEvidenceCount++;
                }
                else
                {
                    report.AddBlockingItem("Missing Unity evidence: `" + evidencePath + "`");
                }
            }

            bool queueEntryReady = IsQueueEntryReadyForUnityReview();
            report.SetCounts(
                rows.Count,
                CountExpectedSubjects(subjects),
                alphaSubjects.Count,
                candidateOutsideAssetsCount,
                candidateNoMetaCount,
                candidateRecommendationLockedCount,
                currentUnitySpriteCount,
                currentUnitySpriteMetaCount,
                CountExpectedSourceLocks(sourceLocks),
                runtimeBindingCount,
                unityEvidenceCount,
                RequiredUnityEvidencePaths.Length,
                queueEntryReady,
                unityEditorValidationReady);

            Require(report, rows.Count == ExpectedManifestRowCount, "Batch 54 manifest keeps the expected 21 rows for bed, litter box, and feeder review variants.", "Batch 54 manifest row count is stale.");
            Require(report, report.SubjectCount == ExpectedSubjectCount && report.AlphaCandidateCount == ExpectedAlphaCandidateCount, "Batch 54 selected alpha candidates cover bed, litter box, and feeder.", "Batch 54 selected alpha candidates are incomplete.");
            Require(report, candidateOutsideAssetsCount == rows.Count && candidateNoMetaCount == rows.Count, "Batch 54 candidate PNGs stay outside Assets and have no Unity meta files.", "Batch 54 candidates are unsafe for review-only handling.");
            Require(report, candidateRecommendationLockedCount == rows.Count, "Batch 54 manifest keeps every row locked to candidate_review_only_do_not_import.", "Batch 54 manifest contains an unsafe recommendation.");
            Require(report, currentUnitySpriteCount == ExpectedSubjectCount && currentUnitySpriteMetaCount == ExpectedSubjectCount, "Current installed BedroomDream prop sprites and Unity meta files are present.", "Current BedroomDream prop sprites or meta files are missing.");
            Require(report, report.SourceLockCount == ExpectedSourceLocks.Length, "Batch 54 references the Bedroom Dream map and mid-sprite source locks.", "Batch 54 source locks are incomplete.");
            Require(report, runtimeBindingCount == ExpectedRuntimeBindingCount, "Runtime bindings still point bed, litter box, and feeder to installed placeholder sprites.", "Runtime prop bindings are incomplete or already drifted.");
            Require(report, queueEntryReady && !report.FormalInstallAllowed, "Batch 54 remains a Unity-review candidate pack and is not formally installable without runtime evidence.", "Batch 54 queue state or formal install gate is unsafe.");
            return report;
        }

        private static IReadOnlyList<P0BedroomInteractableBatch54ManifestRow> ParseRows(string csv)
        {
            List<List<string>> records = ParseCsv(csv ?? string.Empty);
            List<P0BedroomInteractableBatch54ManifestRow> rows = new List<P0BedroomInteractableBatch54ManifestRow>();
            if (records.Count < 2)
            {
                return rows;
            }

            List<string> headers = records[0];
            for (int i = 1; i < records.Count; i++)
            {
                List<string> record = records[i];
                if (record.Count == 1 && string.IsNullOrWhiteSpace(record[0]))
                {
                    continue;
                }

                Dictionary<string, string> values = new Dictionary<string, string>(StringComparer.Ordinal);
                for (int column = 0; column < headers.Count; column++)
                {
                    string value = column < record.Count ? record[column] : string.Empty;
                    values[headers[column]] = value;
                }

                rows.Add(new P0BedroomInteractableBatch54ManifestRow(values));
            }

            return rows;
        }

        private static List<List<string>> ParseCsv(string csv)
        {
            List<List<string>> records = new List<List<string>>();
            List<string> row = new List<string>();
            StringBuilder field = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < csv.Length; i++)
            {
                char c = csv[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < csv.Length && csv[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    row.Add(field.ToString());
                    field.Length = 0;
                }
                else if ((c == '\r' || c == '\n') && !inQuotes)
                {
                    if (c == '\r' && i + 1 < csv.Length && csv[i + 1] == '\n')
                    {
                        i++;
                    }

                    row.Add(field.ToString());
                    records.Add(row);
                    row = new List<string>();
                    field.Length = 0;
                }
                else
                {
                    field.Append(c);
                }
            }

            if (field.Length > 0 || row.Count > 0)
            {
                row.Add(field.ToString());
                records.Add(row);
            }

            return records;
        }

        private static int CountRuntimeBindings()
        {
            int count = 0;
            count += BindingReady("bed", P0VisualAssetCatalog.BedSpriteId, "bedroom_map_concept") ? 1 : 0;
            count += BindingReady("litter_box", P0VisualAssetCatalog.LitterBoxSpriteId, "bedroom_mid_background_sprites") ? 1 : 0;
            count += BindingReady("feeder", P0VisualAssetCatalog.FeederSpriteId, "bedroom_mid_background_sprites") ? 1 : 0;
            return count;
        }

        private static bool BindingReady(string subjectId, string assetId, string sourceLock)
        {
            P0VisualAssetReference asset = P0VisualAssetCatalog.GetInteractableSprite(subjectId);
            return asset.HasAsset
                && asset.AssetId == assetId
                && ContainsOrdinal(asset.UnityImportPath, "Assets/TheCat/Art/Scenes/BedroomDream/")
                && ContainsList(asset.SourceLockIds, sourceLock);
        }

        private static bool IsQueueEntryReadyForUnityReview()
        {
            IReadOnlyList<P0AssetProductionQueueEntry> queue = P0AssetProductionQueueCatalog.CreateP0Queue();
            for (int i = 0; i < queue.Count; i++)
            {
                P0AssetProductionQueueEntry entry = queue[i];
                if (entry.QueueId == P0AssetProductionQueueCatalog.BedroomInteractableCandidateQueueId)
                {
                    return entry.State == P0AssetProductionQueueState.CandidatePackCompletePendingUnityReview
                        && entry.Phase == P0AssetProductionQueuePhase.CodexCandidateProduction
                        && ContainsOrdinal(entry.CandidateDirectory, BatchSlug)
                        && ContainsOrdinal(entry.NextAction, "Unity");
                }
            }

            return false;
        }

        private static int CountExpectedSubjects(IReadOnlyList<string> subjects)
        {
            int count = 0;
            for (int i = 0; i < ExpectedSubjects.Length; i++)
            {
                count += ContainsList(subjects, ExpectedSubjects[i]) ? 1 : 0;
            }

            return count;
        }

        private static int CountExpectedSourceLocks(IReadOnlyList<string> sourceLocks)
        {
            int count = 0;
            for (int i = 0; i < ExpectedSourceLocks.Length; i++)
            {
                count += ContainsList(sourceLocks, ExpectedSourceLocks[i]) ? 1 : 0;
            }

            return count;
        }

        private static bool IsDesignCandidatePath(string path)
        {
            return ContainsOrdinal(path, "design/development/asset_candidates/")
                && !ContainsOrdinal(path, "Assets/");
        }

        private static bool AddUnique(List<string> values, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || ContainsList(values, value))
            {
                return false;
            }

            values.Add(value);
            return true;
        }

        private static bool ContainsList(IReadOnlyList<string> values, string value)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (string.Equals(values[i], value, StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsOrdinal(string value, string token)
        {
            return (value ?? string.Empty).IndexOf(token ?? string.Empty, StringComparison.Ordinal) >= 0;
        }

        private static string DefaultReadAllText(string path)
        {
            return File.ReadAllText(ResolveProjectPath(path));
        }

        private static bool DefaultFileExists(string path)
        {
            return File.Exists(ResolveProjectPath(path));
        }

        private static string ResolveProjectPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || Path.IsPathRooted(path))
            {
                return path ?? string.Empty;
            }

            string normalized = path.Replace('/', Path.DirectorySeparatorChar);
            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, normalized);
                if (File.Exists(candidate) || Directory.Exists(Path.GetDirectoryName(candidate) ?? string.Empty))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return normalized;
        }

        private static void Require(
            P0BedroomInteractableBatch54UnityPreflightReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(failureMessage);
        }
    }
}
