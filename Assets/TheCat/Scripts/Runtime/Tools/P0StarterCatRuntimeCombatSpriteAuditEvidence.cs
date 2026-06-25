using System;
using System.Collections.Generic;
using System.IO;
using TheCat.Data.Catalogs;

namespace TheCat.Tools
{
    public readonly struct P0StarterCatRuntimeCombatSpriteAuditEntry
    {
        public P0StarterCatRuntimeCombatSpriteAuditEntry(
            string catId,
            string displayName,
            string assetId,
            string runtimeBindingId,
            string visualCatalogConstant,
            string sourceLockId,
            string sourceTurnaroundPath,
            string frontReferencePlatePath,
            string runtimeSpritePath)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            AssetId = assetId ?? string.Empty;
            RuntimeBindingId = runtimeBindingId ?? string.Empty;
            VisualCatalogConstant = visualCatalogConstant ?? string.Empty;
            SourceLockId = sourceLockId ?? string.Empty;
            SourceTurnaroundPath = sourceTurnaroundPath ?? string.Empty;
            FrontReferencePlatePath = frontReferencePlatePath ?? string.Empty;
            RuntimeSpritePath = runtimeSpritePath ?? string.Empty;
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string AssetId { get; }

        public string RuntimeBindingId { get; }

        public string VisualCatalogConstant { get; }

        public string SourceLockId { get; }

        public string SourceTurnaroundPath { get; }

        public string FrontReferencePlatePath { get; }

        public string RuntimeSpritePath { get; }

        public string RuntimeSpriteMetaPath => RuntimeSpritePath + ".meta";
    }

    public sealed class P0StarterCatRuntimeCombatSpriteAuditReport
    {
        private readonly List<P0StarterCatSourceLockPacketIssue> issues = new List<P0StarterCatSourceLockPacketIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatSourceLockPacketIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int ArtifactCount { get; private set; }

        public int ManifestRowCount { get; private set; }

        public int RuntimeSpriteCount { get; private set; }

        public int RuntimeSpriteMetaCount { get; private set; }

        public int SourceLockMentionCount { get; private set; }

        public int SourceTurnaroundPathMentionCount { get; private set; }

        public int FrontPlateMentionCount { get; private set; }

        public int RuntimeSpriteMentionCount { get; private set; }

        public int RuntimeBindingMentionCount { get; private set; }

        public int RecommendationMentionCount { get; private set; }

        public int ImportBlockMentionCount { get; private set; }

        public int FailureCount => Count(P0StarterCatSourceLockPacketSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedCoveredCheckCount;

        public void SetCounts(
            int artifactCount,
            int manifestRowCount,
            int runtimeSpriteCount,
            int runtimeSpriteMetaCount,
            int sourceLockMentionCount,
            int sourceTurnaroundPathMentionCount,
            int frontPlateMentionCount,
            int runtimeSpriteMentionCount,
            int runtimeBindingMentionCount,
            int recommendationMentionCount,
            int importBlockMentionCount)
        {
            ArtifactCount = artifactCount;
            ManifestRowCount = manifestRowCount;
            RuntimeSpriteCount = runtimeSpriteCount;
            RuntimeSpriteMetaCount = runtimeSpriteMetaCount;
            SourceLockMentionCount = sourceLockMentionCount;
            SourceTurnaroundPathMentionCount = sourceTurnaroundPathMentionCount;
            FrontPlateMentionCount = frontPlateMentionCount;
            RuntimeSpriteMentionCount = runtimeSpriteMentionCount;
            RuntimeBindingMentionCount = runtimeBindingMentionCount;
            RecommendationMentionCount = recommendationMentionCount;
            ImportBlockMentionCount = importBlockMentionCount;
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
                ? "P0 starter cat runtime combat sprite source audit ready for " + RuntimeSpriteCount + " runtime sprite(s)."
                : "P0 starter cat runtime combat sprite source audit has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Artifacts: " + ArtifactCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount,
                "Manifest rows: " + ManifestRowCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount,
                "Runtime sprites: " + RuntimeSpriteCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount,
                "Runtime sprite meta files: " + RuntimeSpriteMetaCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount,
                "Source lock mentions: " + SourceLockMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount,
                "Source turnaround path mentions: " + SourceTurnaroundPathMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount,
                "Front plate mentions: " + FrontPlateMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount,
                "Runtime sprite mentions: " + RuntimeSpriteMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount,
                "Runtime binding mentions: " + RuntimeBindingMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeBindingMentionCount,
                "Recommendation mentions: " + RecommendationMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedStarterCatCount,
                "Import block mentions: " + ImportBlockMentionCount + "/" + P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedImportBlockMentionCount
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

    public static class P0StarterCatRuntimeCombatSpriteAuditEvidence
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedArtifactCount = 5;
        public const int ExpectedRuntimeSpriteCount = 3;
        public const int ExpectedRuntimeBindingMentionCount = 3;
        public const int ExpectedImportBlockMentionCount = 3;
        public const int ExpectedCoveredCheckCount = 10;
        public const string BatchSlug = "batch_74_runtime_combat_sprite_source_audit_2026-06-15";
        public const string BatchDirectory = "design/development/asset_candidates/starter_cats/" + BatchSlug;
        public const string ManifestPath = BatchDirectory + "/starter_cat_runtime_combat_sprite_source_audit_batch74_manifest.csv";
        public const string ReviewSheetPath = BatchDirectory + "/thecat_cat_starter_runtime_combat_sprite_source_audit_batch74_review_sheet.png";
        public const string ReviewNotePath = BatchDirectory + "/starter_cat_runtime_combat_sprite_source_audit_batch74_review.md";
        public const string ProcessNotePath = BatchDirectory + "/starter_cat_runtime_combat_sprite_source_audit_batch74_process_note.md";
        public const string AgentPromptPath = "design/development/agent_prompts/p0_asset_batch_74_starter_cat_runtime_combat_sprite_source_audit.md";
        public const string Recommendation = "runtime_sprite_source_audit_ready_pending_unity_playmode_screenshot";

        private static readonly string[] PacketFiles =
        {
            ManifestPath,
            ReviewSheetPath,
            ReviewNotePath,
            ProcessNotePath,
            AgentPromptPath
        };

        public static IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> CreateP0Rows()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            return new[]
            {
                new P0StarterCatRuntimeCombatSpriteAuditEntry(
                    P0PrototypeCatalog.SaibanId,
                    "Saiban / Sword Saint",
                    P0VisualAssetCatalog.SaibanCombatSpriteId,
                    "cat.combat.saiban",
                    "SaibanCombatSpriteId",
                    "saiban_turnaround_colored",
                    FindSourceLock(locks, P0PrototypeCatalog.SaibanId).SourceTurnaroundPath,
                    "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_saiban_turnaround_front_reference_plate_768_batch70_v001.png",
                    "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png"),
                new P0StarterCatRuntimeCombatSpriteAuditEntry(
                    P0PrototypeCatalog.NephthysId,
                    "Nephthys / Moon-Sand Agent",
                    P0VisualAssetCatalog.NephthysCombatSpriteId,
                    "cat.combat.nephthys",
                    "NephthysCombatSpriteId",
                    "nephthys_turnaround_colored",
                    FindSourceLock(locks, P0PrototypeCatalog.NephthysId).SourceTurnaroundPath,
                    "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_nephthys_turnaround_front_reference_plate_768_batch70_v001.png",
                    "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png"),
                new P0StarterCatRuntimeCombatSpriteAuditEntry(
                    P0PrototypeCatalog.SuzuneId,
                    "Suzune / Sleep Shrine Healer",
                    P0VisualAssetCatalog.SuzuneCombatSpriteId,
                    "cat.combat.suzune",
                    "SuzuneCombatSpriteId",
                    "suzune_turnaround_colored",
                    FindSourceLock(locks, P0PrototypeCatalog.SuzuneId).SourceTurnaroundPath,
                    "design/development/asset_candidates/starter_cats/batch_70_source_turnaround_reference_plates_2026-06-15/thecat_cat_suzune_turnaround_front_reference_plate_768_batch70_v001.png",
                    "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png")
            };
        }

        public static P0StarterCatRuntimeCombatSpriteAuditReport EvaluateCurrentAudit()
        {
            return Evaluate(CreateP0Rows(), DefaultFileExists, DefaultReadText);
        }

        public static P0StarterCatRuntimeCombatSpriteAuditReport Evaluate(
            IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> rows,
            Func<string, bool> fileExists,
            P0StarterCatReviewTextReader readText)
        {
            P0StarterCatRuntimeCombatSpriteAuditReport report = new P0StarterCatRuntimeCombatSpriteAuditReport();
            IReadOnlyList<P0StarterCatRuntimeCombatSpriteAuditEntry> expectedRows = rows ?? Array.Empty<P0StarterCatRuntimeCombatSpriteAuditEntry>();
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;

            int artifactCount = 0;
            string combinedText = string.Empty;
            for (int i = 0; i < PacketFiles.Length; i++)
            {
                string path = PacketFiles[i];
                if (exists(path))
                {
                    artifactCount++;
                }
                else
                {
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Missing Batch 74 runtime sprite audit artifact: " + path);
                }

                if (reader(path, out string text, out string error))
                {
                    combinedText += "\n" + text;
                }
                else if (path != ReviewSheetPath)
                {
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Could not read Batch 74 runtime sprite audit artifact " + path + ": " + error);
                }
            }

            int manifestRowCount = 0;
            int runtimeSpriteCount = 0;
            int runtimeSpriteMetaCount = 0;
            int sourceLockMentionCount = 0;
            int sourceTurnaroundPathMentionCount = 0;
            int frontPlateMentionCount = 0;
            int runtimeSpriteMentionCount = 0;
            int runtimeBindingMentionCount = 0;
            int recommendationMentionCount = 0;

            for (int i = 0; i < expectedRows.Count; i++)
            {
                P0StarterCatRuntimeCombatSpriteAuditEntry row = expectedRows[i];
                if (ContainsText(combinedText, row.CatId)
                    && ContainsText(combinedText, row.AssetId)
                    && ContainsText(combinedText, row.RuntimeSpritePath))
                {
                    manifestRowCount++;
                }
                else
                {
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, row.CatId + " Batch 74 manifest/review row is missing.");
                }

                if (exists(row.RuntimeSpritePath))
                {
                    runtimeSpriteCount++;
                }
                else
                {
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, row.CatId + " runtime combat sprite is missing: " + row.RuntimeSpritePath);
                }

                if (exists(row.RuntimeSpriteMetaPath))
                {
                    runtimeSpriteMetaCount++;
                }
                else
                {
                    report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, row.CatId + " runtime combat sprite meta is missing: " + row.RuntimeSpriteMetaPath);
                }

                if (ContainsText(combinedText, row.SourceLockId))
                {
                    sourceLockMentionCount++;
                }

                if (ContainsText(combinedText, row.SourceTurnaroundPath))
                {
                    sourceTurnaroundPathMentionCount++;
                }

                if (ContainsText(combinedText, row.FrontReferencePlatePath))
                {
                    frontPlateMentionCount++;
                }

                if (ContainsText(combinedText, row.RuntimeSpritePath))
                {
                    runtimeSpriteMentionCount++;
                }

                if (ContainsText(combinedText, row.RuntimeBindingId)
                    && ContainsText(combinedText, row.VisualCatalogConstant))
                {
                    runtimeBindingMentionCount++;
                }

                if (ContainsText(combinedText, Recommendation))
                {
                    recommendationMentionCount++;
                }
            }

            int importBlockMentionCount = 0;
            foreach (string token in new[]
            {
                "does not generate new cat body art",
                "does not replace any Unity sprite",
                "does not approve AI-generated"
            })
            {
                if (ContainsText(combinedText, token))
                {
                    importBlockMentionCount++;
                }
            }

            report.SetCounts(
                artifactCount,
                manifestRowCount,
                runtimeSpriteCount,
                runtimeSpriteMetaCount,
                sourceLockMentionCount,
                sourceTurnaroundPathMentionCount,
                frontPlateMentionCount,
                runtimeSpriteMentionCount,
                runtimeBindingMentionCount,
                recommendationMentionCount,
                importBlockMentionCount);

            Require(report, expectedRows.Count == ExpectedStarterCatCount, "Batch 74 expects exactly three starter-cat runtime sprite audit rows.", "Batch 74 runtime sprite audit row specification is stale.");
            Require(report, artifactCount == ExpectedArtifactCount, "Batch 74 manifest, review sheet, review note, process note, and agent prompt exist.", "Batch 74 runtime sprite audit packet artifacts are missing.");
            Require(report, manifestRowCount == ExpectedStarterCatCount, "Batch 74 manifest/review records all three runtime combat sprites.", "Batch 74 runtime sprite audit rows are incomplete.");
            Require(report, runtimeSpriteCount == ExpectedRuntimeSpriteCount, "All three runtime-bound starter-cat combat sprites exist in Assets.", "One or more starter-cat runtime combat sprites are missing.");
            Require(report, runtimeSpriteMetaCount == ExpectedRuntimeSpriteCount, "All three starter-cat runtime combat sprites have Unity meta files.", "One or more starter-cat runtime combat sprite meta files are missing.");
            Require(report, sourceLockMentionCount == ExpectedStarterCatCount, "Batch 74 records all three colored-turnaround source locks.", "Batch 74 source-lock mentions are incomplete.");
            Require(report, sourceTurnaroundPathMentionCount == ExpectedStarterCatCount, "Batch 74 records all three exact colored-turnaround source paths.", "Batch 74 source turnaround path mentions are incomplete.");
            Require(report, frontPlateMentionCount == ExpectedStarterCatCount, "Batch 74 records all three Batch 70 front reference plates.", "Batch 74 front plate mentions are incomplete.");
            Require(report, runtimeBindingMentionCount == ExpectedRuntimeBindingMentionCount, "Batch 74 records all three runtime visual binding ids and catalog constants.", "Batch 74 runtime binding mentions are incomplete.");
            Require(report, importBlockMentionCount == ExpectedImportBlockMentionCount, "Batch 74 keeps AI/repainted body import blocked while preserving current runtime sprites.", "Batch 74 import-block text is incomplete.");
            return report;
        }

        private static P0StarterCatTurnaroundSourceLockEntry FindSourceLock(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            string catId)
        {
            if (locks != null)
            {
                for (int i = 0; i < locks.Count; i++)
                {
                    if (locks[i].CatId == catId)
                    {
                        return locks[i];
                    }
                }
            }

            return default(P0StarterCatTurnaroundSourceLockEntry);
        }

        private static bool ContainsText(string text, string expected)
        {
            return text != null && text.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultFileExists(string path)
        {
            string resolved = ResolveFile(path);
            return !string.IsNullOrWhiteSpace(resolved) && File.Exists(resolved);
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

        private static void Require(
            P0StarterCatRuntimeCombatSpriteAuditReport report,
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
