using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetGenerationBatchCoverageTests
    {
        [Test]
        public void EvaluateP0Batches_CompletesGenerationBatchGate()
        {
            P0AssetGenerationBatchCoverageReport report = P0AssetGenerationBatchCoverage.EvaluateP0Batches();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetGenerationBatchCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("asset generation batches cover", report.BuildSummary());
            StringAssert.Contains("style anchors", report.BuildDetailedSummary());
            StringAssert.Contains("agent prompt", report.BuildDetailedSummary());
        }

        [Test]
        public void Catalog_AssignsEachManifestAssetExactlyOnce()
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();
            var batches = P0AssetGenerationBatchCatalog.CreateP0Batches();
            HashSet<string> assigned = new HashSet<string>();

            foreach (P0AssetGenerationBatchDefinition batch in batches)
            {
                Assert.IsNotEmpty(batch.AssetIds, batch.BatchId);
                foreach (string assetId in batch.AssetIds)
                {
                    Assert.IsTrue(assigned.Add(assetId), assetId);
                }
            }

            Assert.AreEqual(manifest.Count, assigned.Count);
            foreach (P0AssetManifestEntry entry in manifest)
            {
                Assert.IsTrue(assigned.Contains(entry.AssetId), entry.AssetId);
            }
        }

        [Test]
        public void Catalog_DefinesScopedAgentPrompts()
        {
            var batches = P0AssetGenerationBatchCatalog.CreateP0Batches();

            Assert.AreEqual(P0AssetGenerationBatchCatalog.StyleAnchorBatchId, batches[0].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.GameplayPlaceholderBatchId, batches[1].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.BossReadinessBatchId, batches[2].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.RouteNodeIconBatchId, batches[3].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.UiShellBatchId, batches[4].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.BattleFeedbackVfxBatchId, batches[5].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.EnemyWarningVfxBatchId, batches[6].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.EnemyAnimationFramesheetBatchId, batches[7].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.RouteChoiceIconBatchId, batches[8].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.RouteRewardCardFrameBatchId, batches[9].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.StatusCompactIconBatchId, batches[10].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.RouteRewardDetailBadgeBatchId, batches[11].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.AuthorityBlessingSealBatchId, batches[12].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.NonBattleNodeSummaryBannerBatchId, batches[13].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.ShopItemCardBatchId, batches[14].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.DreamEventChoiceCardBatchId, batches[15].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.RestNestRecoveryCardBatchId, batches[16].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.PartnerChoiceCardBatchId, batches[17].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.BlessingChoiceCardBatchId, batches[18].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.ResultSettlementBannerBatchId, batches[19].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.CoreGaugeBarBatchId, batches[20].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.StarterCatHudAvatarBatchId, batches[21].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.SkillHudFeedbackBatchId, batches[22].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.StarterSkillVfxBatchId, batches[23].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.SaibanReferenceAtlasBatchId, batches[24].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.NephthysReferenceAtlasBatchId, batches[25].BatchId);
            Assert.AreEqual(P0AssetGenerationBatchCatalog.SuzuneReferenceAtlasBatchId, batches[26].BatchId);

            foreach (P0AssetGenerationBatchDefinition batch in batches)
            {
                StringAssert.StartsWith("design/development/agent_prompts/", batch.ExecutionPromptPath);
                StringAssert.EndsWith(".md", batch.ExecutionPromptPath);
                StringAssert.Contains("P0", batch.DisplayName);
            }
        }

        [Test]
        public void Evaluate_DuplicateBatchAssignmentFailsCoverage()
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();
            var batches = new[]
            {
                new P0AssetGenerationBatchDefinition(
                    P0AssetGenerationBatchCatalog.StyleAnchorBatchId,
                    "P0 Broken Style Anchors",
                    "design/development/agent_prompts/p0_broken.md",
                    new[]
                    {
                        "thecat_style_bedroomdream_anchor_1920x1080_v001",
                        "thecat_style_bedroomdream_anchor_1920x1080_v001"
                    },
                    new string[0])
            };

            P0AssetGenerationBatchCoverageReport report = P0AssetGenerationBatchCoverage.Evaluate(manifest, batches);

            Assert.IsFalse(report.IsComplete);
            Assert.Greater(report.FailureCount, 0);
            StringAssert.Contains("do not exactly cover", report.BuildDetailedSummary());
        }
    }
}
