using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetImportReadinessTests
    {
        [Test]
        public void EvaluateP0Manifest_CurrentManifestAllowsAllGeneratedAssets()
        {
            P0AssetImportReadinessReport report = P0AssetImportReadiness.EvaluateP0Manifest();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetImportReadiness.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.TotalAssetCount);
            Assert.AreEqual(0, report.PlannedAssetCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.WorkspaceFileRequiredCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.ExistingWorkspaceFileCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.DimensionCheckedFileCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.DimensionMatchedFileCount);
            Assert.AreEqual(0, report.DimensionMismatchCount);
            Assert.AreEqual(0, report.FailureCount);
            Assert.AreEqual(0, report.WarningCount);
            StringAssert.Contains("0 planned", report.BuildSummary());
            StringAssert.Contains("PNG dimensions matched: " + P0AssetManifestCatalog.P0ManifestAssetCount, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_GeneratedAssetRequiresWorkspaceFile()
        {
            string assetId = "thecat_style_bedroomdream_anchor_1920x1080_v001";
            var manifest = WithOnlyGeneratedAsset(assetId);

            P0AssetImportReadinessReport report = P0AssetImportReadiness.Evaluate(manifest, path => false);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.WorkspaceFileRequiredCount);
            Assert.AreEqual(1, report.MissingWorkspaceFileCount);
            StringAssert.Contains("missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_GeneratedAssetWithWorkspaceFilePasses()
        {
            string assetId = "thecat_style_bedroomdream_anchor_1920x1080_v001";
            var manifest = WithOnlyGeneratedAsset(assetId);
            string generatedPath = FindAsset(manifest, assetId).UnityImportPath;

            P0AssetImportReadinessReport report = P0AssetImportReadiness.Evaluate(manifest, path => path == generatedPath);

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(1, report.WorkspaceFileRequiredCount);
            Assert.AreEqual(1, report.ExistingWorkspaceFileCount);
            Assert.AreEqual(1, report.DimensionMatchedFileCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount - 1, report.PlannedAssetCount);
        }

        [Test]
        public void Evaluate_IconSheetSizeMultipliesManifestWidth()
        {
            string assetId = "thecat_style_status_icons_5x64_v001";
            var manifest = WithOnlyGeneratedAsset(assetId);
            string generatedPath = FindAsset(manifest, assetId).UnityImportPath;

            P0AssetImportReadinessReport report = P0AssetImportReadiness.Evaluate(
                manifest,
                path => path == generatedPath,
                ReadDimensions(320, 64));

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(1, report.DimensionCheckedFileCount);
            Assert.AreEqual(1, report.DimensionMatchedFileCount);
            Assert.AreEqual(0, report.DimensionMismatchCount);
        }

        [Test]
        public void Evaluate_GeneratedPngDimensionMismatchFails()
        {
            string assetId = "thecat_cat_saiban_combat_sprite_512_v001";
            var manifest = WithOnlyGeneratedAsset(assetId);
            string generatedPath = FindAsset(manifest, assetId).UnityImportPath;

            P0AssetImportReadinessReport report = P0AssetImportReadiness.Evaluate(
                manifest,
                path => path == generatedPath,
                ReadDimensions(256, 512));

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.DimensionCheckedFileCount);
            Assert.AreEqual(0, report.DimensionMatchedFileCount);
            Assert.AreEqual(1, report.DimensionMismatchCount);
            StringAssert.Contains("expected 512x512 but found 256x512", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_PlannedWorkspaceFileProducesWarningOnly()
        {
            string assetId = "thecat_enemy_blackmud_combat_sprite_512_v001";
            var manifest = WithStatusOverrides(new Dictionary<string, string>
            {
                { assetId, P0AssetManifestStatus.Planned }
            });
            string prematurePath = FindAsset(manifest, assetId).UnityImportPath;

            P0AssetImportReadinessReport report = P0AssetImportReadiness.Evaluate(
                manifest,
                path => path == prematurePath || IsGeneratedAcceptedAssetPath(path));

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(0, report.FailureCount);
            Assert.AreEqual(1, report.WarningCount);
            StringAssert.Contains("still marked planned", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_GeneratedDependentAssetRequiresReadyReference()
        {
            string assetId = "thecat_cat_saiban_combat_sprite_512_v001";
            var manifest = WithStatusOverrides(new Dictionary<string, string>
            {
                { "thecat_style_startercats_lineup_2048_v001", P0AssetManifestStatus.Planned },
                { assetId, P0AssetManifestStatus.Generated }
            });
            string generatedPath = FindAsset(manifest, assetId).UnityImportPath;

            P0AssetImportReadinessReport report = P0AssetImportReadiness.Evaluate(
                manifest,
                path => path == generatedPath || IsGeneratedAcceptedAssetPath(path));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("references a row that is not ready", report.BuildDetailedSummary());
        }

        private static IReadOnlyList<P0AssetManifestEntry> WithOnlyGeneratedAsset(string assetId)
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();
            Dictionary<string, string> statusOverrides = new Dictionary<string, string>();
            foreach (P0AssetManifestEntry entry in manifest)
            {
                statusOverrides[entry.AssetId] = entry.AssetId == assetId
                    ? P0AssetManifestStatus.Generated
                    : P0AssetManifestStatus.Planned;
            }

            return WithStatusOverrides(statusOverrides);
        }

        private static IReadOnlyList<P0AssetManifestEntry> WithStatusOverrides(IReadOnlyDictionary<string, string> statusOverrides)
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();
            List<P0AssetManifestEntry> result = new List<P0AssetManifestEntry>();

            foreach (P0AssetManifestEntry entry in manifest)
            {
                result.Add(new P0AssetManifestEntry(
                    entry.AssetId,
                    entry.SubjectId,
                    entry.AssetType,
                    entry.Priority,
                    entry.SourcePromptPath,
                    entry.ReferenceAssetIds,
                    entry.UnityImportPath,
                    entry.Size,
                    statusOverrides != null && statusOverrides.ContainsKey(entry.AssetId)
                        ? statusOverrides[entry.AssetId]
                        : entry.Status,
                    entry.ConsistencyNotes));
            }

            return result;
        }

        private static bool IsGeneratedAcceptedAssetPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            foreach (P0AssetManifestEntry entry in P0AssetManifestCatalog.CreateP0PlannedManifest())
            {
                if (entry.Status == P0AssetManifestStatus.Generated
                    && path == entry.UnityImportPath)
                {
                    return true;
                }
            }

            return false;
        }

        private static P0AssetPngDimensionReader ReadDimensions(int width, int height)
        {
            return (string path, out int actualWidth, out int actualHeight, out string error) =>
            {
                actualWidth = width;
                actualHeight = height;
                error = string.Empty;
                return true;
            };
        }

        private static P0AssetManifestEntry FindAsset(IReadOnlyList<P0AssetManifestEntry> manifest, string assetId)
        {
            foreach (P0AssetManifestEntry entry in manifest)
            {
                if (entry.AssetId == assetId)
                {
                    return entry;
                }
            }

            Assert.Fail("Missing asset " + assetId);
            return null;
        }
    }
}
