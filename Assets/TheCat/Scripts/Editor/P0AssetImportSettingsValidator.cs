using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    public enum P0AssetImportSettingsSeverity
    {
        Info,
        Warning,
        Error
    }

    public readonly struct P0AssetImportSettingsIssue
    {
        public P0AssetImportSettingsIssue(P0AssetImportSettingsSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetImportSettingsSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetImportSettingsReport
    {
        private readonly List<P0AssetImportSettingsIssue> issues = new List<P0AssetImportSettingsIssue>();

        public IReadOnlyList<P0AssetImportSettingsIssue> Issues => issues.AsReadOnly();

        public int TotalAssetCount { get; private set; }

        public int RequiredImportCount { get; private set; }

        public int TextureAssetCount { get; private set; }

        public int TextureImporterCount { get; private set; }

        public int DimensionMatchedCount { get; private set; }

        public int ImportSettingsMatchedCount { get; private set; }

        public int ErrorCount => Count(P0AssetImportSettingsSeverity.Error);

        public int WarningCount => Count(P0AssetImportSettingsSeverity.Warning);

        public bool IsValid => ErrorCount == 0;

        public void SetCounts(
            int totalAssetCount,
            int requiredImportCount,
            int textureAssetCount,
            int textureImporterCount,
            int dimensionMatchedCount,
            int importSettingsMatchedCount)
        {
            TotalAssetCount = totalAssetCount;
            RequiredImportCount = requiredImportCount;
            TextureAssetCount = textureAssetCount;
            TextureImporterCount = textureImporterCount;
            DimensionMatchedCount = dimensionMatchedCount;
            ImportSettingsMatchedCount = importSettingsMatchedCount;
        }

        public void Add(P0AssetImportSettingsSeverity severity, string message)
        {
            issues.Add(new P0AssetImportSettingsIssue(severity, message));
        }

        public string BuildSummary()
        {
            return IsValid
                ? "P0 asset import settings valid for " + RequiredImportCount + " generated/imported asset(s) with " + WarningCount + " warning(s)."
                : "P0 asset import settings have " + ErrorCount + " error(s) and " + WarningCount + " warning(s).";
        }

        public string BuildDetailedLog()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Manifest assets: " + TotalAssetCount,
                "Import settings required: " + RequiredImportCount,
                "Texture assets loaded: " + TextureAssetCount,
                "Texture importers found: " + TextureImporterCount,
                "Texture dimensions matched: " + DimensionMatchedCount,
                "Import settings matched: " + ImportSettingsMatchedCount
            };

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0AssetImportSettingsSeverity severity)
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

    public static class P0AssetImportSettingsValidator
    {
        private const string MenuPath = "TheCat/P0/Validate P0 Asset Imports";

        [MenuItem(MenuPath, false, 91)]
        private static void ValidateP0AssetImports()
        {
            P0AssetImportSettingsReport report = ValidateP0Manifest(refreshAssetDatabase: true);
            string log = "[TheCat] " + report.BuildDetailedLog();
            if (report.IsValid)
            {
                Debug.Log(log);
            }
            else
            {
                Debug.LogError(log);
            }

            EditorUtility.DisplayDialog(
                report.IsValid ? "P0 Asset Imports Valid" : "P0 Asset Imports Need Attention",
                report.BuildSummary(),
                "OK");
        }

        public static P0AssetImportSettingsReport ValidateP0Manifest(bool refreshAssetDatabase)
        {
            if (refreshAssetDatabase)
            {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }

            return Validate(P0AssetManifestCatalog.CreateP0PlannedManifest());
        }

        public static P0AssetImportSettingsReport Validate(IReadOnlyList<P0AssetManifestEntry> manifest)
        {
            P0AssetImportSettingsReport report = new P0AssetImportSettingsReport();
            int totalAssetCount = manifest == null ? 0 : manifest.Count;
            int requiredImportCount = 0;
            int textureAssetCount = 0;
            int textureImporterCount = 0;
            int dimensionMatchedCount = 0;
            int importSettingsMatchedCount = 0;

            if (manifest == null)
            {
                report.Add(P0AssetImportSettingsSeverity.Error, "Manifest is missing.");
                report.SetCounts(0, 0, 0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (P0AssetManifestStatus.IsPending(entry.Status))
                {
                    WarnIfPlannedAssetAlreadyImported(entry, report);
                    continue;
                }

                if (!P0AssetManifestStatus.RequiresWorkspaceFile(entry.Status))
                {
                    continue;
                }

                requiredImportCount++;
                ValidateGeneratedEntry(
                    entry,
                    report,
                    ref textureAssetCount,
                    ref textureImporterCount,
                    ref dimensionMatchedCount,
                    ref importSettingsMatchedCount);
            }

            report.SetCounts(
                totalAssetCount,
                requiredImportCount,
                textureAssetCount,
                textureImporterCount,
                dimensionMatchedCount,
                importSettingsMatchedCount);
            return report;
        }

        private static void WarnIfPlannedAssetAlreadyImported(
            P0AssetManifestEntry entry,
            P0AssetImportSettingsReport report)
        {
            if (AssetDatabase.LoadMainAssetAtPath(entry.UnityImportPath) != null)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Warning,
                    entry.AssetId + " exists in Unity but is still marked planned: " + entry.UnityImportPath);
            }
        }

        private static void ValidateGeneratedEntry(
            P0AssetManifestEntry entry,
            P0AssetImportSettingsReport report,
            ref int textureAssetCount,
            ref int textureImporterCount,
            ref int dimensionMatchedCount,
            ref int importSettingsMatchedCount)
        {
            if (!TryLoadTexture(entry.UnityImportPath, out Texture2D texture))
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " is marked " + entry.Status + " but Unity cannot load a Texture2D at " + entry.UnityImportPath + ".");
                return;
            }

            textureAssetCount++;

            TextureImporter importer = AssetImporter.GetAtPath(entry.UnityImportPath) as TextureImporter;
            if (importer == null)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " is marked " + entry.Status + " but Unity has no TextureImporter at " + entry.UnityImportPath + ".");
                return;
            }

            textureImporterCount++;

            if (ValidateTextureDimensions(entry, texture, importer, report))
            {
                dimensionMatchedCount++;
            }

            if (ValidateImporterSettings(entry, importer, report))
            {
                importSettingsMatchedCount++;
            }
        }

        private static bool ValidateTextureDimensions(
            P0AssetManifestEntry entry,
            Texture2D texture,
            TextureImporter importer,
            P0AssetImportSettingsReport report)
        {
            if (!P0AssetImportReadiness.TryGetExpectedPngDimensions(entry.Size, out int expectedWidth, out int expectedHeight))
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " has unsupported manifest size '" + entry.Size + "'.");
                return false;
            }

            bool valid = true;
            if (texture.width != expectedWidth || texture.height != expectedHeight)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " imported as " + texture.width + "x" + texture.height + " but manifest expects " + expectedWidth + "x" + expectedHeight + ".");
                valid = false;
            }

            int requiredMaxTextureSize = Math.Max(expectedWidth, expectedHeight);
            if (importer.maxTextureSize < requiredMaxTextureSize)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " maxTextureSize is " + importer.maxTextureSize + " but needs at least " + requiredMaxTextureSize + ".");
                valid = false;
            }

            return valid;
        }

        private static bool ValidateImporterSettings(
            P0AssetManifestEntry entry,
            TextureImporter importer,
            P0AssetImportSettingsReport report)
        {
            if (RequiresSpriteImporter(entry))
            {
                return ValidateSpriteImporter(entry, importer, report);
            }

            return ValidateDefaultTextureImporter(entry, importer, report);
        }

        private static bool ValidateSpriteImporter(
            P0AssetManifestEntry entry,
            TextureImporter importer,
            P0AssetImportSettingsReport report)
        {
            bool valid = true;
            if (importer.textureType != TextureImporterType.Sprite)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " should import as Sprite but is " + importer.textureType + ".");
                valid = false;
            }

            SpriteImportMode expectedMode = RequiresMultipleSprites(entry)
                ? SpriteImportMode.Multiple
                : SpriteImportMode.Single;
            if (importer.spriteImportMode != expectedMode)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " spriteImportMode should be " + expectedMode + " but is " + importer.spriteImportMode + ".");
                valid = false;
            }

            if (importer.mipmapEnabled)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Error,
                    entry.AssetId + " should disable mipmaps for crisp 2D P0 UI/gameplay assets.");
                valid = false;
            }

            if (!importer.alphaIsTransparency)
            {
                report.Add(
                    P0AssetImportSettingsSeverity.Warning,
                    entry.AssetId + " should enable alphaIsTransparency for transparent gameplay/UI art.");
            }

            return valid;
        }

        private static bool ValidateDefaultTextureImporter(
            P0AssetManifestEntry entry,
            TextureImporter importer,
            P0AssetImportSettingsReport report)
        {
            if (importer.textureType == TextureImporterType.Default)
            {
                return true;
            }

            report.Add(
                P0AssetImportSettingsSeverity.Error,
                entry.AssetId + " should import as Default texture for " + entry.AssetType + " assets but is " + importer.textureType + ".");
            return false;
        }

        private static bool TryLoadTexture(string assetPath, out Texture2D texture)
        {
            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (texture != null)
            {
                return true;
            }

            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null && sprite.texture != null)
            {
                texture = sprite.texture;
                return true;
            }

            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            for (int i = 0; i < assets.Length; i++)
            {
                if (assets[i] is Texture2D nestedTexture)
                {
                    texture = nestedTexture;
                    return true;
                }

                if (assets[i] is Sprite nestedSprite && nestedSprite.texture != null)
                {
                    texture = nestedSprite.texture;
                    return true;
                }
            }

            return false;
        }

        internal static bool RequiresSpriteImporter(P0AssetManifestEntry entry)
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

        internal static bool RequiresMultipleSprites(P0AssetManifestEntry entry)
        {
            return entry.AssetType == "icon"
                && entry.Size.IndexOf("icons", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
