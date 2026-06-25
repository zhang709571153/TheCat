using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public enum P0AssetImportApplySeverity
    {
        Info,
        Warning,
        Error
    }

    public readonly struct P0AssetImportApplyIssue
    {
        public P0AssetImportApplyIssue(P0AssetImportApplySeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetImportApplySeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetImportApplyReport
    {
        private readonly List<P0AssetImportApplyIssue> issues = new List<P0AssetImportApplyIssue>();

        public IReadOnlyList<P0AssetImportApplyIssue> Issues => issues.AsReadOnly();

        public int RequiredImportCount { get; private set; }

        public int ImporterFoundCount { get; private set; }

        public int ChangedImporterCount { get; private set; }

        public int ReimportedCount { get; private set; }

        public int ErrorCount => Count(P0AssetImportApplySeverity.Error);

        public int WarningCount => Count(P0AssetImportApplySeverity.Warning);

        public bool IsApplied => ErrorCount == 0;

        public void SetCounts(
            int requiredImportCount,
            int importerFoundCount,
            int changedImporterCount,
            int reimportedCount)
        {
            RequiredImportCount = requiredImportCount;
            ImporterFoundCount = importerFoundCount;
            ChangedImporterCount = changedImporterCount;
            ReimportedCount = reimportedCount;
        }

        public void Add(P0AssetImportApplySeverity severity, string message)
        {
            issues.Add(new P0AssetImportApplyIssue(severity, message));
        }

        public string BuildSummary()
        {
            return IsApplied
                ? "P0 asset import settings applied to " + ChangedImporterCount + " importer(s); " + ReimportedCount + " reimported."
                : "P0 asset import settings apply found " + ErrorCount + " error(s) and " + WarningCount + " warning(s).";
        }

        public string BuildDetailedLog()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Import settings required: " + RequiredImportCount,
                "Texture importers found: " + ImporterFoundCount,
                "Texture importers changed: " + ChangedImporterCount,
                "Texture importers reimported: " + ReimportedCount
            };

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0AssetImportApplySeverity severity)
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

    public static class P0AssetImportSettingsApplier
    {
        private const string MenuPath = "TheCat/P0/Apply P0 Asset Import Settings";

        [MenuItem(MenuPath, false, 92)]
        private static void ApplyP0AssetImportSettings()
        {
            P0AssetImportApplyReport applyReport = ApplyP0Manifest(refreshAssetDatabase: true, reimportChanged: true);
            LogApplyReport(applyReport);

            P0AssetImportSettingsReport validationReport = P0AssetImportSettingsValidator.ValidateP0Manifest(refreshAssetDatabase: false);
            LogValidationReport(validationReport);

            EditorUtility.DisplayDialog(
                applyReport.IsApplied && validationReport.IsValid
                    ? "P0 Asset Import Settings Applied"
                    : "P0 Asset Import Settings Need Attention",
                applyReport.BuildSummary() + Environment.NewLine + validationReport.BuildSummary(),
                "OK");
        }

        public static void ApplyAndValidateP0AssetImportsForBatchmode()
        {
            P0AssetImportApplyReport applyReport = ApplyP0Manifest(refreshAssetDatabase: true, reimportChanged: true);
            LogApplyReport(applyReport);

            P0AssetImportSettingsReport validationReport = P0AssetImportSettingsValidator.ValidateP0Manifest(refreshAssetDatabase: false);
            LogValidationReport(validationReport);

            if (!applyReport.IsApplied || !validationReport.IsValid)
            {
                EditorApplication.Exit(1);
                return;
            }

            EditorApplication.Exit(0);
        }

        public static P0AssetImportApplyReport ApplyP0Manifest(bool refreshAssetDatabase, bool reimportChanged)
        {
            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            return Apply(P0AssetManifestCatalog.CreateP0PlannedManifest(), reimportChanged);
        }

        public static P0AssetImportApplyReport Apply(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            bool reimportChanged)
        {
            P0AssetImportApplyReport report = new P0AssetImportApplyReport();
            if (manifest == null)
            {
                report.Add(P0AssetImportApplySeverity.Error, "Manifest is missing.");
                report.SetCounts(0, 0, 0, 0);
                return report;
            }

            int requiredImportCount = 0;
            int importerFoundCount = 0;
            int changedImporterCount = 0;
            int reimportedCount = 0;

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (!P0AssetManifestStatus.RequiresWorkspaceFile(entry.Status))
                {
                    continue;
                }

                requiredImportCount++;
                TextureImporter importer = AssetImporter.GetAtPath(entry.UnityImportPath) as TextureImporter;
                if (importer == null)
                {
                    report.Add(
                        P0AssetImportApplySeverity.Error,
                        entry.AssetId + " is marked " + entry.Status + " but Unity has no TextureImporter at " + entry.UnityImportPath + ".");
                    continue;
                }

                importerFoundCount++;
                if (!TryApplyEntrySettings(entry, importer, report, out bool changed))
                {
                    continue;
                }

                if (!importer.userData.Contains("TheCatP0ImportSettings:v1"))
                {
                    importer.userData = AppendUserData(importer.userData, "TheCatP0ImportSettings:v1");
                    changed = true;
                }

                if (!changed)
                {
                    continue;
                }

                changedImporterCount++;
                if (reimportChanged)
                {
                    importer.SaveAndReimport();
                    reimportedCount++;
                }
            }

            report.SetCounts(requiredImportCount, importerFoundCount, changedImporterCount, reimportedCount);
            return report;
        }

        private static bool TryApplyEntrySettings(
            P0AssetManifestEntry entry,
            TextureImporter importer,
            P0AssetImportApplyReport report,
            out bool changed)
        {
            changed = false;
            if (!P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out int expectedWidth, out int expectedHeight))
            {
                report.Add(
                    P0AssetImportApplySeverity.Error,
                    entry.AssetId + " has unsupported manifest size '" + entry.Size + "'.");
                return false;
            }

            int maxTextureSize = NextPowerOfTwo(Math.Max(expectedWidth, expectedHeight));
            changed |= SetMaxTextureSizeIfNeeded(importer, maxTextureSize);
            changed |= SetNpotScaleIfNeeded(importer, TextureImporterNPOTScale.None);
            changed |= SetSrgbIfNeeded(importer, true);

            if (P0AssetImportSettingsValidator.RequiresSpriteImporter(entry))
            {
                changed |= ApplySpriteSettings(entry, importer);
                return true;
            }

            changed |= ApplyDefaultTextureSettings(importer);
            return true;
        }

        private static bool ApplySpriteSettings(P0AssetManifestEntry entry, TextureImporter importer)
        {
            bool changed = false;
            changed |= SetTextureTypeIfNeeded(importer, TextureImporterType.Sprite);
            changed |= SetSpriteImportModeIfNeeded(importer, P0AssetImportSettingsValidator.RequiresMultipleSprites(entry)
                ? SpriteImportMode.Multiple
                : SpriteImportMode.Single);
            changed |= SetMipmapIfNeeded(importer, false);
            changed |= SetAlphaSourceIfNeeded(importer, TextureImporterAlphaSource.FromInput);
            changed |= SetAlphaIsTransparencyIfNeeded(importer, true);
            changed |= SetTextureCompressionIfNeeded(importer, TextureImporterCompression.Uncompressed);
            changed |= SetCrunchedCompressionIfNeeded(importer, false);
            return changed;
        }

        private static bool ApplyDefaultTextureSettings(TextureImporter importer)
        {
            bool changed = false;
            changed |= SetTextureTypeIfNeeded(importer, TextureImporterType.Default);
            changed |= SetMipmapIfNeeded(importer, false);
            changed |= SetAlphaSourceIfNeeded(importer, TextureImporterAlphaSource.FromInput);
            changed |= SetTextureCompressionIfNeeded(importer, TextureImporterCompression.Uncompressed);
            changed |= SetCrunchedCompressionIfNeeded(importer, false);
            return changed;
        }

        private static int NextPowerOfTwo(int value)
        {
            int result = 32;
            while (result < value)
            {
                result *= 2;
            }

            return Math.Min(result, 8192);
        }

        private static string AppendUserData(string userData, string marker)
        {
            if (string.IsNullOrWhiteSpace(userData))
            {
                return marker;
            }

            return userData + ";" + marker;
        }

        private static bool SetMaxTextureSizeIfNeeded(TextureImporter importer, int maxTextureSize)
        {
            if (importer.maxTextureSize >= maxTextureSize)
            {
                return false;
            }

            importer.maxTextureSize = maxTextureSize;
            return true;
        }

        private static bool SetSrgbIfNeeded(TextureImporter importer, bool value)
        {
            if (importer.sRGBTexture == value)
            {
                return false;
            }

            importer.sRGBTexture = value;
            return true;
        }

        private static bool SetNpotScaleIfNeeded(TextureImporter importer, TextureImporterNPOTScale value)
        {
            if (importer.npotScale == value)
            {
                return false;
            }

            importer.npotScale = value;
            return true;
        }

        private static bool SetTextureTypeIfNeeded(TextureImporter importer, TextureImporterType value)
        {
            if (importer.textureType == value)
            {
                return false;
            }

            importer.textureType = value;
            return true;
        }

        private static bool SetSpriteImportModeIfNeeded(TextureImporter importer, SpriteImportMode value)
        {
            if (importer.spriteImportMode == value)
            {
                return false;
            }

            importer.spriteImportMode = value;
            return true;
        }

        private static bool SetMipmapIfNeeded(TextureImporter importer, bool value)
        {
            if (importer.mipmapEnabled == value)
            {
                return false;
            }

            importer.mipmapEnabled = value;
            return true;
        }

        private static bool SetAlphaSourceIfNeeded(TextureImporter importer, TextureImporterAlphaSource value)
        {
            if (importer.alphaSource == value)
            {
                return false;
            }

            importer.alphaSource = value;
            return true;
        }

        private static bool SetAlphaIsTransparencyIfNeeded(TextureImporter importer, bool value)
        {
            if (importer.alphaIsTransparency == value)
            {
                return false;
            }

            importer.alphaIsTransparency = value;
            return true;
        }

        private static bool SetTextureCompressionIfNeeded(TextureImporter importer, TextureImporterCompression value)
        {
            if (importer.textureCompression == value)
            {
                return false;
            }

            importer.textureCompression = value;
            return true;
        }

        private static bool SetCrunchedCompressionIfNeeded(TextureImporter importer, bool value)
        {
            if (importer.crunchedCompression == value)
            {
                return false;
            }

            importer.crunchedCompression = value;
            return true;
        }

        private static void LogApplyReport(P0AssetImportApplyReport report)
        {
            string message = "[TheCat] P0 Asset Import Settings Apply: " + report.BuildDetailedLog();
            if (report.IsApplied)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }

        private static void LogValidationReport(P0AssetImportSettingsReport report)
        {
            string message = "[TheCat] P0 Asset Import Settings Validation: " + report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }
    }
}
