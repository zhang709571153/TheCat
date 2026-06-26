using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TheCat.Tools
{
    public delegate bool P0ScreenshotEvidenceFileExists(string path);

    public delegate bool P0ScreenshotEvidenceFileIsUsable(string path);

    public delegate IReadOnlyList<string> P0ScreenshotEvidenceFileEnumerator(string directory);

    public sealed class P0PlayModeScreenshotFileEvidenceReport
    {
        private readonly List<string> existingExpectedFiles = new List<string>();
        private readonly List<string> missingExpectedFiles = new List<string>();
        private readonly List<string> unusableExpectedFiles = new List<string>();
        private readonly List<string> unexpectedPngFiles = new List<string>();

        public IReadOnlyList<string> ExistingExpectedFiles => existingExpectedFiles.AsReadOnly();

        public IReadOnlyList<string> MissingExpectedFiles => missingExpectedFiles.AsReadOnly();

        public IReadOnlyList<string> UnusableExpectedFiles => unusableExpectedFiles.AsReadOnly();

        public IReadOnlyList<string> UnexpectedPngFiles => unexpectedPngFiles.AsReadOnly();

        public string ScreenshotDirectory { get; private set; } = string.Empty;

        public int ExpectedFileCount { get; private set; }

        public int ExistingExpectedFileCount => existingExpectedFiles.Count;

        public int MissingExpectedFileCount => missingExpectedFiles.Count;

        public int UnusableExpectedFileCount => unusableExpectedFiles.Count;

        public int UnexpectedPngFileCount => unexpectedPngFiles.Count;

        public bool IsComplete => ExpectedFileCount > 0
            && ExistingExpectedFileCount == ExpectedFileCount
            && MissingExpectedFileCount == 0
            && UnusableExpectedFileCount == 0
            && UnexpectedPngFileCount == 0;

        public void SetDirectory(string screenshotDirectory, int expectedFileCount)
        {
            ScreenshotDirectory = screenshotDirectory ?? string.Empty;
            ExpectedFileCount = expectedFileCount;
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

        public void AddUnusableExpectedFile(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                unusableExpectedFiles.Add(fileName);
            }
        }

        public void AddUnexpectedPngFile(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                unexpectedPngFiles.Add(fileName);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 Play Mode screenshot file evidence complete for " + ExistingExpectedFileCount + "/" + ExpectedFileCount + " expected validated capture(s)."
                : "P0 Play Mode screenshot file evidence incomplete: " + ExistingExpectedFileCount + "/" + ExpectedFileCount + " expected capture(s), " + MissingExpectedFileCount + " missing, " + UnusableExpectedFileCount + " unusable, " + UnexpectedPngFileCount + " unexpected PNG file(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Screenshot directory: " + ScreenshotDirectory,
                "Expected files: " + ExpectedFileCount,
                "Existing expected files: " + ExistingExpectedFileCount,
                "Missing expected files: " + MissingExpectedFileCount,
                "Unusable expected files: " + UnusableExpectedFileCount,
                "Unexpected PNG files: " + UnexpectedPngFileCount
            };

            AppendList(lines, "Existing", existingExpectedFiles);
            AppendList(lines, "Missing", missingExpectedFiles);
            AppendList(lines, "Unusable", unusableExpectedFiles);
            AppendList(lines, "Unexpected", unexpectedPngFiles);
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
    }

    public static class P0PlayModeScreenshotFileEvidence
    {
        public const string DefaultScreenshotDirectory = "design/development/screenshots/p0-playmode-smoke";
        private const int MinimumScreenshotWidth = 640;
        private const int MinimumScreenshotHeight = 480;
        private const int MinimumDistinctSampleColors = 8;

        public static P0PlayModeScreenshotFileEvidenceReport EvaluateP0Directory()
        {
            return Evaluate(
                DefaultScreenshotDirectory,
                P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames,
                DefaultFileExists,
                DefaultFileIsUsable,
                DefaultEnumeratePngFileNames);
        }

        public static P0PlayModeScreenshotFileEvidenceReport Evaluate(
            string screenshotDirectory,
            IReadOnlyList<string> expectedFileNames,
            P0ScreenshotEvidenceFileExists fileExists,
            P0ScreenshotEvidenceFileEnumerator enumeratePngFileNames)
        {
            return Evaluate(
                screenshotDirectory,
                expectedFileNames,
                fileExists,
                AssumeExistingFileIsUsable,
                enumeratePngFileNames);
        }

        public static P0PlayModeScreenshotFileEvidenceReport Evaluate(
            string screenshotDirectory,
            IReadOnlyList<string> expectedFileNames,
            P0ScreenshotEvidenceFileExists fileExists,
            P0ScreenshotEvidenceFileIsUsable fileIsUsable,
            P0ScreenshotEvidenceFileEnumerator enumeratePngFileNames)
        {
            IReadOnlyList<string> expected = expectedFileNames ?? Array.Empty<string>();
            P0ScreenshotEvidenceFileExists exists = fileExists ?? DefaultFileExists;
            P0ScreenshotEvidenceFileIsUsable isUsable = fileIsUsable ?? AssumeExistingFileIsUsable;
            P0ScreenshotEvidenceFileEnumerator enumerate = enumeratePngFileNames ?? DefaultEnumeratePngFileNames;
            P0PlayModeScreenshotFileEvidenceReport report = new P0PlayModeScreenshotFileEvidenceReport();
            string directory = screenshotDirectory ?? string.Empty;
            report.SetDirectory(directory, expected.Count);

            HashSet<string> expectedSet = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < expected.Count; i++)
            {
                string fileName = expected[i] ?? string.Empty;
                expectedSet.Add(fileName);
                string path = Path.Combine(directory, fileName).Replace('\\', '/');
                if (exists(path))
                {
                    report.AddExistingExpectedFile(fileName);
                    if (!isUsable(path))
                    {
                        report.AddUnusableExpectedFile(fileName);
                    }
                }
                else
                {
                    report.AddMissingExpectedFile(fileName);
                }
            }

            IReadOnlyList<string> existingPngs = enumerate(directory) ?? Array.Empty<string>();
            for (int i = 0; i < existingPngs.Count; i++)
            {
                string fileName = existingPngs[i] ?? string.Empty;
                if (!expectedSet.Contains(fileName))
                {
                    report.AddUnexpectedPngFile(fileName);
                }
            }

            return report;
        }

        private static bool AssumeExistingFileIsUsable(string path)
        {
            return !string.IsNullOrWhiteSpace(path);
        }

        private static bool DefaultFileExists(string path)
        {
            return !string.IsNullOrWhiteSpace(ResolveFilePath(path));
        }

        private static bool DefaultFileIsUsable(string path)
        {
            string resolved = ResolveFilePath(path);
            if (string.IsNullOrWhiteSpace(resolved))
            {
                return false;
            }

            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                byte[] bytes = File.ReadAllBytes(resolved);
                if (!ImageConversion.LoadImage(texture, bytes, false))
                {
                    return false;
                }

                return texture.width >= MinimumScreenshotWidth
                    && texture.height >= MinimumScreenshotHeight
                    && CountDistinctSampleColors(texture) >= MinimumDistinctSampleColors;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                // Evidence validation can decode all captures in one editor tick.
                // Release the transient texture immediately so Play Mode report
                // generation does not accumulate full-size screenshot textures.
                UnityEngine.Object.DestroyImmediate(texture);
            }
        }

        private static int CountDistinctSampleColors(Texture2D texture)
        {
            HashSet<Color32> colors = new HashSet<Color32>();
            int stepX = Math.Max(1, texture.width / 32);
            int stepY = Math.Max(1, texture.height / 32);
            for (int x = 0; x < texture.width; x += stepX)
            {
                for (int y = 0; y < texture.height; y += stepY)
                {
                    colors.Add(texture.GetPixel(x, y));
                    if (colors.Count >= MinimumDistinctSampleColors)
                    {
                        return colors.Count;
                    }
                }
            }

            return colors.Count;
        }

        private static IReadOnlyList<string> DefaultEnumeratePngFileNames(string directory)
        {
            string resolved = ResolveDirectory(directory);
            if (string.IsNullOrWhiteSpace(resolved) || !Directory.Exists(resolved))
            {
                return Array.Empty<string>();
            }

            string[] paths = Directory.GetFiles(resolved, "*.png", SearchOption.TopDirectoryOnly);
            List<string> names = new List<string>(paths.Length);
            for (int i = 0; i < paths.Length; i++)
            {
                names.Add(Path.GetFileName(paths[i]));
            }

            names.Sort(StringComparer.Ordinal);
            return names.AsReadOnly();
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

            return string.Empty;
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

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, directory.Replace('/', Path.DirectorySeparatorChar));
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return directory;
        }
    }
}
