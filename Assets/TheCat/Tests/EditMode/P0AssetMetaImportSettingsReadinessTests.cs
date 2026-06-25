using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetMetaImportSettingsReadinessTests
    {
        private const string SingleSpriteMeta =
            "TextureImporter:\n"
            + "  mipmaps:\n"
            + "    enableMipMap: 0\n"
            + "  spriteMode: 1\n"
            + "  alphaIsTransparency: 1\n"
            + "  textureType: 8\n"
            + "  userData: TheCatP0ImportSettings:v1\n";

        private const string SingleSpriteMetaV2 =
            "TextureImporter:\n"
            + "  mipmaps:\n"
            + "    enableMipMap: 0\n"
            + "  spriteMode: 1\n"
            + "  alphaIsTransparency: 1\n"
            + "  textureType: 8\n"
            + "  userData: TheCatP0ImportSettings:v2;SourceFramesheetSingleSprite;UseSlicedSprites\n";

        private const string MultipleSpriteMeta =
            "TextureImporter:\n"
            + "  mipmaps:\n"
            + "    enableMipMap: 0\n"
            + "  spriteMode: 2\n"
            + "  alphaIsTransparency: 1\n"
            + "  textureType: 8\n"
            + "  userData: TheCatP0ImportSettings:v1\n";

        private const string DefaultTextureMeta =
            "TextureImporter:\n"
            + "  mipmaps:\n"
            + "    enableMipMap: 0\n"
            + "  spriteMode: 0\n"
            + "  alphaIsTransparency: 0\n"
            + "  textureType: 0\n"
            + "  userData: TheCatP0ImportSettings:v1\n";

        [Test]
        public void EvaluateP0Manifest_CurrentGeneratedMetasUseP0ImportSettings()
        {
            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.EvaluateP0Manifest();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetMetaImportSettingsReadiness.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.TotalAssetCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.RequiredMetaCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.ExistingMetaCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.SettingsMatchedCount);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("Sprite import settings", report.BuildDetailedSummary());
            StringAssert.Contains("Default texture import settings", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingMetaFails()
        {
            var manifest = WithOnlyGeneratedAsset("thecat_cat_saiban_combat_sprite_512_v001");

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => false,
                ReadMeta(SingleSpriteMeta));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("missing Unity meta file", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_DefaultSpriteImportSettingsFail()
        {
            var manifest = WithOnlyGeneratedAsset("thecat_cat_saiban_combat_sprite_512_v001");

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                ReadMeta(
                    "TextureImporter:\n"
                    + "  mipmaps:\n"
                    + "    enableMipMap: 1\n"
                    + "  spriteMode: 0\n"
                    + "  alphaIsTransparency: 0\n"
                    + "  textureType: 0\n"
                    + "  userData: \n"));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("textureType should be 8", report.BuildDetailedSummary());
            StringAssert.Contains("enableMipMap should be 0", report.BuildDetailedSummary());
            StringAssert.Contains("alphaIsTransparency should be 1", report.BuildDetailedSummary());
            StringAssert.Contains("missing TheCatP0ImportSettings", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_IconSheetRequiresMultipleSpriteMode()
        {
            var manifest = WithOnlyGeneratedAsset("thecat_style_status_icons_5x64_v001");

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                ReadMeta(SingleSpriteMeta));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("spriteMode should be 2", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_BattleBackgroundRequiresSpriteImportSettings()
        {
            var manifest = WithOnlyGeneratedAsset(P0VisualAssetCatalog.BedroomDreamBattleBackgroundId);

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                ReadMeta(DefaultTextureMeta));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("textureType should be 8", report.BuildDetailedSummary());
            StringAssert.Contains("spriteMode should be 1", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_RouteRewardDetailBadgesRequireSpriteImportSettings()
        {
            var manifest = WithOnlyGeneratedAsset(P0VisualAssetCatalog.RouteRewardGainBadgeId);

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                ReadMeta(DefaultTextureMeta));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("textureType should be 8", report.BuildDetailedSummary());
            StringAssert.Contains("spriteMode should be 1", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_EnemyFramesheetsRequireSingleSpriteImportSettings()
        {
            var manifest = WithOnlyGeneratedAsset(P0VisualAssetCatalog.BlackMudMoveFramesheetId);

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                ReadMeta(DefaultTextureMeta));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("textureType should be 8", report.BuildDetailedSummary());
            StringAssert.Contains("spriteMode should be 1", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_EnemyFramesheetsAcceptP0ImportSettingsV2Marker()
        {
            var manifest = WithOnlyGeneratedAsset(P0VisualAssetCatalog.BlackMudMoveFramesheetId);

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                ReadMeta(SingleSpriteMetaV2));

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(1, report.SettingsMatchedCount);
        }

        [Test]
        public void Evaluate_MixedGeneratedManifestPassesWithExpectedMetaKinds()
        {
            var manifest = WithGeneratedAssets(
                "thecat_style_bedroomdream_anchor_1920x1080_v001",
                "thecat_cat_saiban_combat_sprite_512_v001",
                P0VisualAssetCatalog.RouteRewardGainBadgeId,
                P0VisualAssetCatalog.BlackMudMoveFramesheetId,
                "thecat_style_status_icons_5x64_v001");

            P0AssetMetaImportSettingsReport report = P0AssetMetaImportSettingsReadiness.Evaluate(
                manifest,
                path => true,
                (string path, out string text, out string error) =>
                {
                    error = string.Empty;
                    if (path.Contains("status_icons"))
                    {
                        text = MultipleSpriteMeta;
                    }
                    else if (path.Contains("framesheet"))
                    {
                        text = SingleSpriteMetaV2;
                    }
                    else if (path.Contains("combat_sprite") || path.Contains("reward_detail"))
                    {
                        text = SingleSpriteMeta;
                    }
                    else
                    {
                        text = DefaultTextureMeta;
                    }

                    return true;
                });

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(5, report.RequiredMetaCount);
            Assert.AreEqual(5, report.SettingsMatchedCount);
        }

        private static IReadOnlyList<P0AssetManifestEntry> WithOnlyGeneratedAsset(string assetId)
        {
            return WithGeneratedAssets(assetId);
        }

        private static IReadOnlyList<P0AssetManifestEntry> WithGeneratedAssets(params string[] assetIds)
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();
            HashSet<string> generated = new HashSet<string>(assetIds);
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
                    generated.Contains(entry.AssetId) ? P0AssetManifestStatus.Generated : P0AssetManifestStatus.Planned,
                    entry.ConsistencyNotes));
            }

            return result;
        }

        private static P0AssetMetaTextReader ReadMeta(string metaText)
        {
            return (string path, out string text, out string error) =>
            {
                text = metaText;
                error = string.Empty;
                return true;
            };
        }
    }
}
