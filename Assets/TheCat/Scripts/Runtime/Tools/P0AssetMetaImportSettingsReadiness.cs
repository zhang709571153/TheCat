using System;
using System.Collections.Generic;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public delegate bool P0AssetMetaTextReader(string unityMetaPath, out string text, out string error);

    public enum P0AssetMetaImportSettingsSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetMetaImportSettingsIssue
    {
        public P0AssetMetaImportSettingsIssue(P0AssetMetaImportSettingsSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetMetaImportSettingsSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetMetaImportSettingsReport
    {
        private readonly List<P0AssetMetaImportSettingsIssue> issues = new List<P0AssetMetaImportSettingsIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0AssetMetaImportSettingsIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int TotalAssetCount { get; private set; }

        public int RequiredMetaCount { get; private set; }

        public int ExistingMetaCount { get; private set; }

        public int SettingsMatchedCount { get; private set; }

        public int FailureCount => Count(P0AssetMetaImportSettingsSeverity.Failure);

        public bool IsReady => FailureCount == 0 && coveredChecks.Count >= P0AssetMetaImportSettingsReadiness.ExpectedCoveredCheckCount;

        public void SetCounts(int totalAssetCount, int requiredMetaCount, int existingMetaCount, int settingsMatchedCount)
        {
            TotalAssetCount = totalAssetCount;
            RequiredMetaCount = requiredMetaCount;
            ExistingMetaCount = existingMetaCount;
            SettingsMatchedCount = settingsMatchedCount;
        }

        public void AddIssue(P0AssetMetaImportSettingsSeverity severity, string message)
        {
            issues.Add(new P0AssetMetaImportSettingsIssue(severity, message));
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
                ? "P0 asset meta import settings ready for " + RequiredMetaCount + " generated/imported asset(s)."
                : "P0 asset meta import settings have " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Manifest assets: " + TotalAssetCount,
                "Meta settings required: " + RequiredMetaCount,
                "Meta files present: " + ExistingMetaCount,
                "Meta settings matched: " + SettingsMatchedCount
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

        private int Count(P0AssetMetaImportSettingsSeverity severity)
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

    public static class P0AssetMetaImportSettingsReadiness
    {
        public const int ExpectedCoveredCheckCount = 4;
        public const string ImportSettingsMarker = "TheCatP0ImportSettings";
        public const string ImportSettingsMarkerV1 = "TheCatP0ImportSettings:v1";
        public const string ImportSettingsMarkerV2 = "TheCatP0ImportSettings:v2";

        public static P0AssetMetaImportSettingsReport EvaluateP0Manifest()
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), DefaultMetaFileExists, DefaultReadMetaText);
        }

        public static P0AssetMetaImportSettingsReport EvaluateP0Manifest(Func<string, bool> metaFileExists, P0AssetMetaTextReader readMetaText)
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), metaFileExists, readMetaText);
        }

        public static P0AssetMetaImportSettingsReport Evaluate(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            Func<string, bool> metaFileExists,
            P0AssetMetaTextReader readMetaText)
        {
            P0AssetMetaImportSettingsReport report = new P0AssetMetaImportSettingsReport();
            Func<string, bool> exists = metaFileExists ?? DefaultMetaFileExists;
            P0AssetMetaTextReader read = readMetaText ?? DefaultReadMetaText;
            int totalAssetCount = manifest == null ? 0 : manifest.Count;
            int requiredMetaCount = 0;
            int existingMetaCount = 0;
            int settingsMatchedCount = 0;
            bool allRequiredMetaFilesExist = true;
            bool spriteSettingsMatched = true;
            bool defaultTextureSettingsMatched = true;
            bool allMarkersPresent = true;

            if (manifest == null)
            {
                report.AddIssue(P0AssetMetaImportSettingsSeverity.Failure, "Manifest is missing.");
                report.SetCounts(0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (!P0AssetManifestStatus.RequiresWorkspaceFile(entry.Status))
                {
                    continue;
                }

                requiredMetaCount++;
                string metaPath = BuildMetaPath(entry);
                if (!exists(metaPath))
                {
                    allRequiredMetaFilesExist = false;
                    report.AddIssue(P0AssetMetaImportSettingsSeverity.Failure, entry.AssetId + " is missing Unity meta file " + metaPath + ".");
                    continue;
                }

                existingMetaCount++;
                if (!read(metaPath, out string text, out string error))
                {
                    report.AddIssue(P0AssetMetaImportSettingsSeverity.Failure, entry.AssetId + " meta file could not be read: " + error);
                    continue;
                }

                bool entryMatches = ValidateEntry(entry, text, report, ref spriteSettingsMatched, ref defaultTextureSettingsMatched, ref allMarkersPresent);
                if (entryMatches)
                {
                    settingsMatchedCount++;
                }
            }

            Require(
                report,
                requiredMetaCount > 0 && allRequiredMetaFilesExist,
                "Generated/imported P0 assets have Unity .png.meta files.",
                "One or more generated/imported P0 assets are missing Unity .png.meta files.");
            Require(
                report,
                spriteSettingsMatched,
                "Sprite, frame, bar, icon, banner, card, skill HUD feedback, and VFX meta files use P0 Sprite import settings.",
                "One or more Sprite, frame, bar, icon, banner, card, skill HUD feedback, or VFX meta files are not using P0 Sprite import settings.");
            Require(
                report,
                defaultTextureSettingsMatched,
                "Background and concept meta files use P0 Default texture import settings.",
                "One or more background or concept meta files are not using P0 Default texture import settings.");
            Require(
                report,
                allMarkersPresent,
                "Generated/imported P0 meta files carry the TheCat P0 import settings marker.",
                "One or more generated/imported P0 meta files are missing the TheCat P0 import settings marker.");

            report.SetCounts(totalAssetCount, requiredMetaCount, existingMetaCount, settingsMatchedCount);
            return report;
        }

        private static bool ValidateEntry(
            P0AssetManifestEntry entry,
            string text,
            P0AssetMetaImportSettingsReport report,
            ref bool spriteSettingsMatched,
            ref bool defaultTextureSettingsMatched,
            ref bool allMarkersPresent)
        {
            bool matches = true;
            bool requiresSprite = RequiresSpriteImporter(entry);

            if (!ContainsLineValue(text, "TextureImporter", string.Empty, allowSectionHeader: true))
            {
                report.AddIssue(P0AssetMetaImportSettingsSeverity.Failure, entry.AssetId + " meta file is missing TextureImporter.");
                return false;
            }

            if (requiresSprite)
            {
                matches &= RequireValue(entry, text, "textureType", "8", "Sprite", report);
                matches &= RequireValue(entry, text, "spriteMode", RequiresMultipleSprites(entry) ? "2" : "1", RequiresMultipleSprites(entry) ? "Multiple" : "Single", report);
                matches &= RequireValue(entry, text, "enableMipMap", "0", "disabled mipmaps", report);
                matches &= RequireValue(entry, text, "alphaIsTransparency", "1", "alpha transparency", report);
                spriteSettingsMatched &= matches;
            }
            else
            {
                matches &= RequireValue(entry, text, "textureType", "0", "Default texture", report);
                matches &= RequireValue(entry, text, "enableMipMap", "0", "disabled mipmaps", report);
                defaultTextureSettingsMatched &= matches;
            }

            bool hasMarker = HasImportSettingsMarker(text);
            if (!hasMarker)
            {
                report.AddIssue(P0AssetMetaImportSettingsSeverity.Failure, entry.AssetId + " meta userData is missing " + ImportSettingsMarker + " marker.");
            }

            allMarkersPresent &= hasMarker;
            matches &= hasMarker;
            return matches;
        }

        private static bool RequireValue(
            P0AssetManifestEntry entry,
            string text,
            string key,
            string expectedValue,
            string label,
            P0AssetMetaImportSettingsReport report)
        {
            string actualValue = GetFieldValue(text, key);
            if (actualValue == expectedValue)
            {
                return true;
            }

            report.AddIssue(
                P0AssetMetaImportSettingsSeverity.Failure,
                entry.AssetId + " meta " + key + " should be " + expectedValue + " for " + label + " but is '" + actualValue + "'.");
            return false;
        }

        private static string BuildMetaPath(P0AssetManifestEntry entry)
        {
            return entry.UnityImportPath + ".meta";
        }

        private static bool RequiresSpriteImporter(P0AssetManifestEntry entry)
        {
            return entry.AssetType == "sprite"
                || entry.AssetType == "avatar_icon"
                || entry.AssetType == "frame"
                || entry.AssetType == "bar"
                || entry.AssetType == "icon"
                || entry.AssetType == "banner"
                || entry.AssetType == "badge"
                || entry.AssetType == "card"
                || entry.AssetType == "reference_atlas"
                || entry.AssetType == "skill_hud_feedback"
                || entry.AssetType == "framesheet"
                || entry.AssetType == "vfx"
                || entry.AssetId == P0VisualAssetCatalog.BedroomDreamBattleBackgroundId
                || entry.AssetId == P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId;
        }

        private static bool RequiresMultipleSprites(P0AssetManifestEntry entry)
        {
            return entry.AssetType == "icon"
                && entry.Size.IndexOf("icons", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool ContainsLineValue(string text, string key, string expectedValue, bool allowSectionHeader)
        {
            string actualValue = GetFieldValue(text, key);
            if (allowSectionHeader && actualValue == string.Empty)
            {
                return text != null && text.IndexOf(key + ":", StringComparison.Ordinal) >= 0;
            }

            return actualValue == expectedValue;
        }

        private static bool HasImportSettingsMarker(string text)
        {
            string userData = GetFieldValue(text, "userData");
            return userData.IndexOf(ImportSettingsMarkerV1, StringComparison.Ordinal) >= 0
                || userData.IndexOf(ImportSettingsMarkerV2, StringComparison.Ordinal) >= 0;
        }

        private static string GetFieldValue(string text, string key)
        {
            if (text == null)
            {
                return string.Empty;
            }

            string prefix = key + ":";
            string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                string trimmed = lines[i].TrimStart();
                if (!trimmed.StartsWith(prefix, StringComparison.Ordinal))
                {
                    continue;
                }

                return trimmed.Substring(prefix.Length).Trim();
            }

            return string.Empty;
        }

        private static bool DefaultMetaFileExists(string unityMetaPath)
        {
            return !string.IsNullOrWhiteSpace(unityMetaPath) && File.Exists(unityMetaPath);
        }

        private static bool DefaultReadMetaText(string unityMetaPath, out string text, out string error)
        {
            text = string.Empty;
            error = string.Empty;
            try
            {
                text = File.ReadAllText(unityMetaPath);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static void Require(
            P0AssetMetaImportSettingsReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetMetaImportSettingsSeverity.Failure, failureMessage);
        }
    }
}
