using System;
using System.Collections.Generic;
using System.IO;
using TheCat.Data.Catalogs;

namespace TheCat.Tools
{
    public readonly struct P0StarterCatUnityReferenceInstallIssue
    {
        public P0StarterCatUnityReferenceInstallIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatSourceLockPacketSeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0StarterCatUnityReferenceInstallEntry
    {
        public P0StarterCatUnityReferenceInstallEntry(
            string catId,
            string displayName,
            string batchDirectory,
            string assetId,
            string sourceLockId,
            string installedAtlasPath,
            string manifestPath,
            string reviewSheetPath,
            string reviewNotePath,
            string processNotePath,
            string agentPromptPath,
            string recommendation)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            BatchDirectory = batchDirectory ?? string.Empty;
            AssetId = assetId ?? string.Empty;
            SourceLockId = sourceLockId ?? string.Empty;
            InstalledAtlasPath = installedAtlasPath ?? string.Empty;
            InstalledMetaPath = string.IsNullOrWhiteSpace(installedAtlasPath) ? string.Empty : installedAtlasPath + ".meta";
            ManifestPath = manifestPath ?? string.Empty;
            ReviewSheetPath = reviewSheetPath ?? string.Empty;
            ReviewNotePath = reviewNotePath ?? string.Empty;
            ProcessNotePath = processNotePath ?? string.Empty;
            AgentPromptPath = agentPromptPath ?? string.Empty;
            Recommendation = recommendation ?? string.Empty;
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string BatchDirectory { get; }

        public string AssetId { get; }

        public string SourceLockId { get; }

        public string InstalledAtlasPath { get; }

        public string InstalledMetaPath { get; }

        public string ManifestPath { get; }

        public string ReviewSheetPath { get; }

        public string ReviewNotePath { get; }

        public string ProcessNotePath { get; }

        public string AgentPromptPath { get; }

        public string Recommendation { get; }
    }

    public sealed class P0StarterCatUnityReferenceInstallReport
    {
        private readonly List<P0StarterCatUnityReferenceInstallIssue> issues = new List<P0StarterCatUnityReferenceInstallIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatUnityReferenceInstallIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int ArtifactCount { get; private set; }

        public int InstalledAssetCount { get; private set; }

        public int ManifestRowCount { get; private set; }

        public int SourceLockMentionCount { get; private set; }

        public int ReferencePlateMentionCount { get; private set; }

        public int ImportSettingTokenCount { get; private set; }

        public int RuntimeBlockMentionCount { get; private set; }

        public int FailureCount => Count(P0StarterCatSourceLockPacketSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatUnityReferenceInstallEvidence.ExpectedCoveredCheckCount;

        public void SetCounts(
            int artifactCount,
            int installedAssetCount,
            int manifestRowCount,
            int sourceLockMentionCount,
            int referencePlateMentionCount,
            int importSettingTokenCount,
            int runtimeBlockMentionCount)
        {
            ArtifactCount = artifactCount;
            InstalledAssetCount = installedAssetCount;
            ManifestRowCount = manifestRowCount;
            SourceLockMentionCount = sourceLockMentionCount;
            ReferencePlateMentionCount = referencePlateMentionCount;
            ImportSettingTokenCount = importSettingTokenCount;
            RuntimeBlockMentionCount = runtimeBlockMentionCount;
        }

        public void AddIssue(P0StarterCatSourceLockPacketSeverity severity, string message)
        {
            issues.Add(new P0StarterCatUnityReferenceInstallIssue(severity, message));
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
                ? "P0 starter cat Unity reference install ready with " + InstalledAssetCount + " installed debug reference asset(s)."
                : "P0 starter cat Unity reference install has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Artifacts: " + ArtifactCount,
                "Installed assets: " + InstalledAssetCount,
                "Manifest rows: " + ManifestRowCount,
                "Source-lock mentions: " + SourceLockMentionCount,
                "Reference plate mentions: " + ReferencePlateMentionCount,
                "Import setting tokens: " + ImportSettingTokenCount,
                "Runtime replacement block mentions: " + RuntimeBlockMentionCount
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

    public static class P0StarterCatUnityReferenceInstallEvidence
    {
        public const int ExpectedCatInstallCount = 3;
        public const int ExpectedArtifactCount = 21;
        public const int ExpectedInstalledAssetCount = 3;
        public const int ExpectedManifestRowCount = 3;
        public const int ExpectedSourceLockMentionCount = 3;
        public const int ExpectedReferencePlateMentionCount = 9;
        public const int ExpectedImportSettingTokenCount = 18;
        public const int ExpectedRuntimeBlockMentionCount = 15;
        public const int ExpectedCoveredCheckCount = 7;
        public const string AssetType = "reference_atlas";
        public const string Recommendation = "installed_debug_reference_not_runtime_binding_pending_unity_visual_smoke";

        public const string BatchDirectory = "design/development/asset_candidates/starter_cats/saiban/batch_71_saiban_unity_reference_install_2026-06-15";
        public const string AssetId = "thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001";
        public const string InstalledAtlasPath = "Assets/TheCat/Art/Characters/References/thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001.png";
        public const string InstalledMetaPath = InstalledAtlasPath + ".meta";
        public const string ManifestPath = BatchDirectory + "/saiban_batch71_unity_reference_install_manifest.csv";
        public const string ReviewSheetPath = BatchDirectory + "/thecat_cat_saiban_batch71_unity_reference_install_review_sheet.png";
        public const string ReviewNotePath = BatchDirectory + "/saiban_batch71_unity_reference_install_review.md";
        public const string ProcessNotePath = BatchDirectory + "/saiban_batch71_unity_reference_install_process_note.md";
        public const string AgentPromptPath = "design/development/agent_prompts/p0_asset_batch_71_saiban_unity_reference_install.md";

        private static readonly string[] ImportSettingTokens =
        {
            "TextureImporter:",
            "textureType: 8",
            "spriteMode: 1",
            "enableMipMap: 0",
            "alphaIsTransparency: 1",
            "userData: TheCatP0ImportSettings:v1"
        };

        private static readonly string[] RuntimeBlockTokens =
        {
            "not runtime-bound",
            "does not replace",
            "Formal starter-cat body-art import remains blocked",
            "No AI-generated cat body",
            "Unity visual smoke remains pending"
        };

        public static IReadOnlyList<P0StarterCatUnityReferenceInstallEntry> CreateP0Installs()
        {
            return new[]
            {
                new P0StarterCatUnityReferenceInstallEntry(
                    P0PrototypeCatalog.SaibanId,
                    "Saiban",
                    BatchDirectory,
                    AssetId,
                    "saiban_turnaround_colored",
                    InstalledAtlasPath,
                    ManifestPath,
                    ReviewSheetPath,
                    ReviewNotePath,
                    ProcessNotePath,
                    AgentPromptPath,
                    Recommendation),
                new P0StarterCatUnityReferenceInstallEntry(
                    P0PrototypeCatalog.NephthysId,
                    "Nephthys",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15",
                    "thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001",
                    "nephthys_turnaround_colored",
                    "Assets/TheCat/Art/Characters/References/thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001.png",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/nephthys_batch72_unity_reference_install_manifest.csv",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/thecat_cat_nephthys_batch72_unity_reference_install_review_sheet.png",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/nephthys_batch72_unity_reference_install_review.md",
                    "design/development/asset_candidates/starter_cats/nephthys/batch_72_nephthys_unity_reference_install_2026-06-15/nephthys_batch72_unity_reference_install_process_note.md",
                    "design/development/agent_prompts/p0_asset_batch_72_nephthys_unity_reference_install.md",
                    Recommendation),
                new P0StarterCatUnityReferenceInstallEntry(
                    P0PrototypeCatalog.SuzuneId,
                    "Suzune",
                    "design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15",
                    "thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001",
                    "suzune_turnaround_colored",
                    "Assets/TheCat/Art/Characters/References/thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png",
                    "design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/suzune_batch73_unity_reference_install_manifest.csv",
                    "design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/thecat_cat_suzune_batch73_unity_reference_install_review_sheet.png",
                    "design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/suzune_batch73_unity_reference_install_review.md",
                    "design/development/asset_candidates/starter_cats/suzune/batch_73_suzune_unity_reference_install_2026-06-15/suzune_batch73_unity_reference_install_process_note.md",
                    "design/development/agent_prompts/p0_asset_batch_73_suzune_unity_reference_install.md",
                    Recommendation)
            };
        }

        public static P0StarterCatUnityReferenceInstallReport EvaluateCurrentInstall()
        {
            return Evaluate(
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                DefaultReadText,
                DefaultFileExists);
        }

        public static P0StarterCatUnityReferenceInstallReport Evaluate(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            P0StarterCatReviewTextReader readText,
            Func<string, bool> fileExists)
        {
            P0StarterCatUnityReferenceInstallReport report = new P0StarterCatUnityReferenceInstallReport();
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> expectedLocks = locks ?? Array.Empty<P0StarterCatTurnaroundSourceLockEntry>();
            IReadOnlyList<P0StarterCatUnityReferenceInstallEntry> installs = CreateP0Installs();
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            Func<string, bool> exists = fileExists ?? DefaultFileExists;

            int artifactCount = 0;
            int installedAssetCount = 0;
            int manifestRowCount = 0;
            int sourceLockMentionCount = 0;
            int referencePlateMentionCount = 0;
            int importSettingTokenCount = 0;
            int runtimeBlockMentionCount = 0;

            for (int i = 0; i < installs.Count; i++)
            {
                P0StarterCatUnityReferenceInstallEntry install = installs[i];
                string manifestText = ReadOptional(install.ManifestPath, reader, exists, report);
                string metaText = ReadOptional(install.InstalledMetaPath, reader, exists, report);
                string reviewText = ReadOptional(install.ReviewNotePath, reader, exists, report);
                string processText = ReadOptional(install.ProcessNotePath, reader, exists, report);
                string agentPromptText = ReadOptional(install.AgentPromptPath, reader, exists, report);
                string combinedText = manifestText + "\n" + reviewText + "\n" + processText + "\n" + agentPromptText;

                P0StarterCatTurnaroundSourceLockEntry sourceLock = FindLock(expectedLocks, install.CatId);
                string sourcePath = sourceLock.SourceTurnaroundPath ?? string.Empty;
                string lockedSpritePath = sourceLock.SpritePath ?? string.Empty;

                artifactCount += CountArtifacts(install, exists);
                if (exists(install.InstalledAtlasPath) && exists(install.InstalledMetaPath))
                {
                    installedAssetCount++;
                }

                if (ContainsManifestRow(manifestText, install, sourcePath, lockedSpritePath))
                {
                    manifestRowCount++;
                }

                if (ContainsText(combinedText, install.SourceLockId) && ContainsText(combinedText, sourcePath))
                {
                    sourceLockMentionCount++;
                }

                referencePlateMentionCount += CountReferencePlateMentions(combinedText, install.CatId);
                importSettingTokenCount += CountTokens(metaText, ImportSettingTokens);
                runtimeBlockMentionCount += CountTokens(combinedText, RuntimeBlockTokens);
            }

            report.SetCounts(
                artifactCount,
                installedAssetCount,
                manifestRowCount,
                sourceLockMentionCount,
                referencePlateMentionCount,
                importSettingTokenCount,
                runtimeBlockMentionCount);

            Require(report, artifactCount == ExpectedArtifactCount, "Batch 71-73 Unity reference install artifacts are present.", "Batch 71-73 Unity reference install artifacts are incomplete.");
            Require(report, installedAssetCount == ExpectedInstalledAssetCount, "Saiban, Nephthys, and Suzune Unity reference atlases and metas are installed.", "One or more starter-cat Unity reference atlases or meta files are missing.");
            Require(report, manifestRowCount == ExpectedManifestRowCount, "Batch 71-73 manifest rows record installed reference atlases, source locks, combat sprites, and recommendations.", "One or more Batch 71-73 manifest rows are missing or stale.");
            Require(report, sourceLockMentionCount == ExpectedSourceLockMentionCount, "Batch 71-73 packets repeat all starter-cat colored-turnaround source locks and exact source paths.", "One or more Batch 71-73 packets are missing starter-cat source-lock evidence.");
            Require(report, referencePlateMentionCount == ExpectedReferencePlateMentionCount, "Batch 71-73 packets link all nine Batch 70 starter-cat front, side, and back reference plates.", "Batch 71-73 packets are missing one or more Batch 70 reference plate links.");
            Require(report, importSettingTokenCount == ExpectedImportSettingTokenCount, "Installed reference atlas metas use P0 Sprite import settings.", "One or more installed reference atlas metas are missing P0 Sprite import setting tokens.");
            Require(report, runtimeBlockMentionCount == ExpectedRuntimeBlockMentionCount, "Batch 71-73 reviews keep the atlases debug-only, non-runtime-bound, non-replacement, and pending Unity visual smoke.", "Batch 71-73 reviews do not clearly block runtime replacement or formal body-art import.");
            return report;
        }

        public static string ReferencePlatePathFor(string view)
        {
            return ReferencePlatePathFor(P0PrototypeCatalog.SaibanId, view);
        }

        public static string ReferencePlatePathFor(string catId, string view)
        {
            return P0StarterCatReferencePlateEvidence.ReferencePlatePathFor(catId, view);
        }

        public static P0StarterCatUnityReferenceInstallEntry FindInstall(string path)
        {
            IReadOnlyList<P0StarterCatUnityReferenceInstallEntry> installs = CreateP0Installs();
            for (int i = 0; i < installs.Count; i++)
            {
                P0StarterCatUnityReferenceInstallEntry install = installs[i];
                if (path == install.InstalledAtlasPath
                    || path == install.InstalledMetaPath
                    || path == install.ManifestPath
                    || path == install.ReviewSheetPath
                    || path == install.ReviewNotePath
                    || path == install.ProcessNotePath
                    || path == install.AgentPromptPath)
                {
                    return install;
                }
            }

            return default(P0StarterCatUnityReferenceInstallEntry);
        }

        private static P0StarterCatTurnaroundSourceLockEntry FindLock(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            string catId)
        {
            if (locks == null)
            {
                return default(P0StarterCatTurnaroundSourceLockEntry);
            }

            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].CatId == catId)
                {
                    return locks[i];
                }
            }

            return default(P0StarterCatTurnaroundSourceLockEntry);
        }

        private static bool ContainsManifestRow(
            string manifestText,
            P0StarterCatUnityReferenceInstallEntry install,
            string sourcePath,
            string lockedSpritePath)
        {
            return ContainsText(manifestText, install.CatId)
                && ContainsText(manifestText, install.AssetId)
                && ContainsText(manifestText, AssetType)
                && ContainsText(manifestText, install.InstalledAtlasPath)
                && ContainsText(manifestText, install.InstalledMetaPath)
                && ContainsText(manifestText, install.SourceLockId)
                && ContainsText(manifestText, sourcePath)
                && ContainsText(manifestText, lockedSpritePath)
                && ContainsText(manifestText, install.Recommendation);
        }

        private static int CountReferencePlateMentions(string text, string catId)
        {
            int count = 0;
            if (ContainsText(text, ReferencePlatePathFor(catId, "front")))
            {
                count++;
            }

            if (ContainsText(text, ReferencePlatePathFor(catId, "side")))
            {
                count++;
            }

            if (ContainsText(text, ReferencePlatePathFor(catId, "back")))
            {
                count++;
            }

            return count;
        }

        private static int CountTokens(string text, IReadOnlyList<string> tokens)
        {
            int count = 0;
            if (tokens == null)
            {
                return count;
            }

            for (int i = 0; i < tokens.Count; i++)
            {
                if (ContainsText(text, tokens[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountArtifacts(
            P0StarterCatUnityReferenceInstallEntry install,
            Func<string, bool> fileExists)
        {
            int count = 0;
            string[] requiredArtifactPaths =
            {
                install.InstalledAtlasPath,
                install.InstalledMetaPath,
                install.ManifestPath,
                install.ReviewSheetPath,
                install.ReviewNotePath,
                install.ProcessNotePath,
                install.AgentPromptPath
            };

            for (int i = 0; i < requiredArtifactPaths.Length; i++)
            {
                if (fileExists(requiredArtifactPaths[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static string ReadOptional(
            string path,
            P0StarterCatReviewTextReader reader,
            Func<string, bool> exists,
            P0StarterCatUnityReferenceInstallReport report)
        {
            if (!exists(path))
            {
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Starter-cat Unity reference artifact is missing: " + path);
                return string.Empty;
            }

            if (!reader(path, out string text, out string error))
            {
                report.AddIssue(P0StarterCatSourceLockPacketSeverity.Failure, "Could not read starter-cat Unity reference artifact: " + path + " (" + error + ")");
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

            if (Path.IsPathRooted(path))
            {
                return path;
            }

            return Path.Combine(Directory.GetCurrentDirectory(), path.Replace('/', Path.DirectorySeparatorChar));
        }

        private static bool ContainsText(string text, string expectedText)
        {
            return text != null
                && expectedText != null
                && text.IndexOf(expectedText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(
            P0StarterCatUnityReferenceInstallReport report,
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
