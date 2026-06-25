using System;
using System.Collections.Generic;
using System.IO;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public delegate bool P0AssetPngDimensionReader(string unityImportPath, out int width, out int height, out string error);

    public enum P0AssetImportReadinessSeverity
    {
        Info,
        Warning,
        Failure
    }

    public readonly struct P0AssetImportReadinessIssue
    {
        public P0AssetImportReadinessIssue(P0AssetImportReadinessSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetImportReadinessSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetImportReadinessReport
    {
        private readonly List<P0AssetImportReadinessIssue> issues = new List<P0AssetImportReadinessIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0AssetImportReadinessIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int TotalAssetCount { get; private set; }

        public int PlannedAssetCount { get; private set; }

        public int WorkspaceFileRequiredCount { get; private set; }

        public int ExistingWorkspaceFileCount { get; private set; }

        public int MissingWorkspaceFileCount { get; private set; }

        public int DimensionCheckedFileCount { get; private set; }

        public int DimensionMatchedFileCount { get; private set; }

        public int DimensionMismatchCount { get; private set; }

        public int FailureCount => Count(P0AssetImportReadinessSeverity.Failure);

        public int WarningCount => Count(P0AssetImportReadinessSeverity.Warning);

        public bool IsReady => FailureCount == 0 && coveredChecks.Count >= P0AssetImportReadiness.ExpectedCoveredCheckCount;

        public void SetCounts(
            int totalAssetCount,
            int plannedAssetCount,
            int workspaceFileRequiredCount,
            int existingWorkspaceFileCount,
            int missingWorkspaceFileCount)
        {
            TotalAssetCount = totalAssetCount;
            PlannedAssetCount = plannedAssetCount;
            WorkspaceFileRequiredCount = workspaceFileRequiredCount;
            ExistingWorkspaceFileCount = existingWorkspaceFileCount;
            MissingWorkspaceFileCount = missingWorkspaceFileCount;
        }

        public void SetDimensionCounts(
            int dimensionCheckedFileCount,
            int dimensionMatchedFileCount,
            int dimensionMismatchCount)
        {
            DimensionCheckedFileCount = dimensionCheckedFileCount;
            DimensionMatchedFileCount = dimensionMatchedFileCount;
            DimensionMismatchCount = dimensionMismatchCount;
        }

        public void AddIssue(P0AssetImportReadinessSeverity severity, string message)
        {
            issues.Add(new P0AssetImportReadinessIssue(severity, message));
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
                ? "P0 asset import readiness passed for " + TotalAssetCount + " asset(s): " + ExistingWorkspaceFileCount + " workspace file(s), " + PlannedAssetCount + " planned."
                : "P0 asset import readiness failed with " + FailureCount + " failure(s), " + WarningCount + " warning(s), and " + MissingWorkspaceFileCount + " missing workspace file(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Workspace files required: " + WorkspaceFileRequiredCount,
                "Workspace files present: " + ExistingWorkspaceFileCount,
                "Workspace files missing: " + MissingWorkspaceFileCount,
                "PNG dimensions checked: " + DimensionCheckedFileCount,
                "PNG dimensions matched: " + DimensionMatchedFileCount,
                "PNG dimension mismatches: " + DimensionMismatchCount
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

        private int Count(P0AssetImportReadinessSeverity severity)
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

    public static class P0AssetImportReadiness
    {
        public const int ExpectedCoveredCheckCount = 6;

        public static P0AssetImportReadinessReport EvaluateP0Manifest()
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), DefaultFileExists, DefaultReadPngDimensions);
        }

        public static P0AssetImportReadinessReport EvaluateP0Manifest(Func<string, bool> fileExists)
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), fileExists, DefaultReadPngDimensions);
        }

        public static P0AssetImportReadinessReport EvaluateP0Manifest(
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions)
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), fileExists, readPngDimensions);
        }

        public static P0AssetImportReadinessReport Evaluate(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            Func<string, bool> fileExists)
        {
            return Evaluate(manifest, fileExists, DefaultReadPngDimensions);
        }

        public static P0AssetImportReadinessReport Evaluate(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            Func<string, bool> fileExists,
            P0AssetPngDimensionReader readPngDimensions)
        {
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            P0AssetPngDimensionReader readDimensions = readPngDimensions ?? DefaultReadPngDimensions;
            P0AssetImportReadinessReport report = new P0AssetImportReadinessReport();

            int totalAssetCount = manifest == null ? 0 : manifest.Count;
            int plannedAssetCount = 0;
            int workspaceFileRequiredCount = 0;
            int existingWorkspaceFileCount = 0;
            int missingWorkspaceFileCount = 0;
            int dimensionCheckedFileCount = 0;
            int dimensionMatchedFileCount = 0;
            int dimensionMismatchCount = 0;

            if (manifest != null)
            {
                for (int i = 0; i < manifest.Count; i++)
                {
                    P0AssetManifestEntry entry = manifest[i];
                    bool entryExists = exists(entry.UnityImportPath);
                    if (P0AssetManifestStatus.IsPending(entry.Status))
                    {
                        plannedAssetCount++;
                        if (entryExists)
                        {
                            report.AddIssue(
                                P0AssetImportReadinessSeverity.Warning,
                                entry.AssetId + " exists at " + entry.UnityImportPath + " but is still marked planned.");
                        }
                    }

                    if (P0AssetManifestStatus.RequiresWorkspaceFile(entry.Status))
                    {
                        workspaceFileRequiredCount++;
                        if (entryExists)
                        {
                            existingWorkspaceFileCount++;
                            dimensionCheckedFileCount++;
                            if (ValidatePngDimensions(entry, readDimensions, report))
                            {
                                dimensionMatchedFileCount++;
                            }
                            else
                            {
                                dimensionMismatchCount++;
                            }
                        }
                        else
                        {
                            missingWorkspaceFileCount++;
                            report.AddIssue(
                                P0AssetImportReadinessSeverity.Failure,
                                entry.AssetId + " is marked " + entry.Status + " but missing " + entry.UnityImportPath + ".");
                        }
                    }
                }
            }

            report.SetCounts(
                totalAssetCount,
                plannedAssetCount,
                workspaceFileRequiredCount,
                existingWorkspaceFileCount,
                missingWorkspaceFileCount);
            report.SetDimensionCounts(
                dimensionCheckedFileCount,
                dimensionMatchedFileCount,
                dimensionMismatchCount);

            EvaluateStatusVocabulary(manifest, report);
            EvaluatePendingAssets(manifest, plannedAssetCount, report);
            EvaluateWorkspaceFileRequirement(missingWorkspaceFileCount, report);
            EvaluateGeneratedDimensions(dimensionCheckedFileCount, dimensionMismatchCount, report);
            EvaluatePlannedFileWarnings(report);
            EvaluateReferenceReadiness(manifest, report);
            return report;
        }

        private static void EvaluateStatusVocabulary(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetImportReadinessReport report)
        {
            bool statusesKnown = manifest != null && manifest.Count > 0;
            if (statusesKnown)
            {
                for (int i = 0; i < manifest.Count; i++)
                {
                    if (!P0AssetManifestStatus.IsKnown(manifest[i].Status))
                    {
                        statusesKnown = false;
                        break;
                    }
                }
            }

            Require(
                report,
                statusesKnown,
                "Manifest statuses use the allowed asset lifecycle vocabulary.",
                "Manifest contains an unknown asset lifecycle status.");
        }

        private static void EvaluatePendingAssets(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            int plannedAssetCount,
            P0AssetImportReadinessReport report)
        {
            Require(
                report,
                manifest != null && plannedAssetCount >= 0,
                "Planned assets may remain fileless before their generation batch runs.",
                "Asset manifest is missing, so pending asset readiness cannot be evaluated.");
        }

        private static void EvaluateWorkspaceFileRequirement(
            int missingWorkspaceFileCount,
            P0AssetImportReadinessReport report)
        {
            Require(
                report,
                missingWorkspaceFileCount == 0,
                "Generated, imported, and replaced assets have workspace files.",
                "One or more generated/imported asset rows are missing workspace files.");
        }

        private static void EvaluateGeneratedDimensions(
            int dimensionCheckedFileCount,
            int dimensionMismatchCount,
            P0AssetImportReadinessReport report)
        {
            Require(
                report,
                dimensionCheckedFileCount > 0 && dimensionMismatchCount == 0,
                "Generated, imported, and replaced PNG files match their manifest dimensions.",
                "One or more generated/imported PNG files do not match their manifest dimensions.");
        }

        private static void EvaluatePlannedFileWarnings(P0AssetImportReadinessReport report)
        {
            report.AddCoveredCheck("Premature files on planned rows are reported as warnings.");
        }

        private static void EvaluateReferenceReadiness(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetImportReadinessReport report)
        {
            Require(
                report,
                manifest != null && GeneratedAssetsDoNotOutrunReferences(manifest),
                "Generated/imported assets only reference generated, imported, or replaced assets.",
                "A generated/imported asset references a row that is not ready yet.");
        }

        private static bool GeneratedAssetsDoNotOutrunReferences(IReadOnlyList<P0AssetManifestEntry> manifest)
        {
            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (!P0AssetManifestStatus.RequiresWorkspaceFile(entry.Status))
                {
                    continue;
                }

                for (int referenceIndex = 0; referenceIndex < entry.ReferenceAssetIds.Count; referenceIndex++)
                {
                    P0AssetManifestEntry reference = FindAsset(manifest, entry.ReferenceAssetIds[referenceIndex]);
                    if (reference == null || !P0AssetManifestStatus.RequiresWorkspaceFile(reference.Status))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static P0AssetManifestEntry FindAsset(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string assetId)
        {
            for (int i = 0; i < manifest.Count; i++)
            {
                if (manifest[i].AssetId == assetId)
                {
                    return manifest[i];
                }
            }

            return null;
        }

        private static bool ValidatePngDimensions(
            P0AssetManifestEntry entry,
            P0AssetPngDimensionReader readPngDimensions,
            P0AssetImportReadinessReport report)
        {
            if (!entry.UnityImportPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                report.AddIssue(
                    P0AssetImportReadinessSeverity.Failure,
                    entry.AssetId + " is marked " + entry.Status + " but is not a PNG path: " + entry.UnityImportPath + ".");
                return false;
            }

            if (!TryGetExpectedPngDimensions(entry.Size, out int expectedWidth, out int expectedHeight))
            {
                report.AddIssue(
                    P0AssetImportReadinessSeverity.Failure,
                    entry.AssetId + " has unsupported manifest size '" + entry.Size + "'.");
                return false;
            }

            if (!readPngDimensions(entry.UnityImportPath, out int actualWidth, out int actualHeight, out string error))
            {
                report.AddIssue(
                    P0AssetImportReadinessSeverity.Failure,
                    entry.AssetId + " could not read PNG dimensions from " + entry.UnityImportPath + ": " + error);
                return false;
            }

            if (actualWidth == expectedWidth && actualHeight == expectedHeight)
            {
                return true;
            }

            report.AddIssue(
                P0AssetImportReadinessSeverity.Failure,
                entry.AssetId + " expected " + expectedWidth + "x" + expectedHeight + " but found " + actualWidth + "x" + actualHeight + " at " + entry.UnityImportPath + ".");
            return false;
        }

        public static bool TryGetExpectedPngDimensions(string size, out int width, out int height)
        {
            width = 0;
            height = 0;

            if (string.IsNullOrWhiteSpace(size))
            {
                return false;
            }

            string[] parts = size.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return false;
            }

            int horizontalCount = 1;
            if (parts.Length >= 3
                && int.TryParse(parts[0], out int parsedCount)
                && parts[1].IndexOf("icon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                horizontalCount = parsedCount;
            }

            string dimensionPart = parts[parts.Length - 1];
            int separatorIndex = dimensionPart.IndexOf('x');
            if (separatorIndex <= 0 || separatorIndex >= dimensionPart.Length - 1)
            {
                return false;
            }

            if (!int.TryParse(dimensionPart.Substring(0, separatorIndex), out int parsedWidth)
                || !int.TryParse(dimensionPart.Substring(separatorIndex + 1), out int parsedHeight))
            {
                return false;
            }

            width = parsedWidth * horizontalCount;
            height = parsedHeight;
            return width > 0 && height > 0;
        }

        private static bool DefaultFileExists(string unityImportPath)
        {
            if (string.IsNullOrWhiteSpace(unityImportPath))
            {
                return false;
            }

            return File.Exists(unityImportPath);
        }

        private static bool DefaultReadPngDimensions(string unityImportPath, out int width, out int height, out string error)
        {
            width = 0;
            height = 0;
            error = string.Empty;

            try
            {
                using (FileStream stream = File.OpenRead(unityImportPath))
                {
                    byte[] header = new byte[24];
                    int read = stream.Read(header, 0, header.Length);
                    if (read < header.Length)
                    {
                        error = "file is shorter than a PNG header";
                        return false;
                    }

                    if (header[0] != 0x89
                        || header[1] != 0x50
                        || header[2] != 0x4E
                        || header[3] != 0x47
                        || header[4] != 0x0D
                        || header[5] != 0x0A
                        || header[6] != 0x1A
                        || header[7] != 0x0A
                        || header[12] != 0x49
                        || header[13] != 0x48
                        || header[14] != 0x44
                        || header[15] != 0x52)
                    {
                        error = "file header is not PNG IHDR";
                        return false;
                    }

                    width = ReadBigEndianInt32(header, 16);
                    height = ReadBigEndianInt32(header, 20);
                    return width > 0 && height > 0;
                }
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static int ReadBigEndianInt32(byte[] bytes, int startIndex)
        {
            return (bytes[startIndex] << 24)
                | (bytes[startIndex + 1] << 16)
                | (bytes[startIndex + 2] << 8)
                | bytes[startIndex + 3];
        }

        private static void Require(
            P0AssetImportReadinessReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetImportReadinessSeverity.Failure, failureMessage);
        }
    }
}
