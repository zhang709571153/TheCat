using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0AssetGenerationBatchCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetGenerationBatchCoverageIssue
    {
        public P0AssetGenerationBatchCoverageIssue(P0AssetGenerationBatchCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetGenerationBatchCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetGenerationBatchCoverageReport
    {
        private readonly List<P0AssetGenerationBatchCoverageIssue> issues = new List<P0AssetGenerationBatchCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0AssetGenerationBatchCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0AssetGenerationBatchCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0AssetGenerationBatchCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0AssetGenerationBatchCoverageSeverity severity, string message)
        {
            issues.Add(new P0AssetGenerationBatchCoverageIssue(severity, message));
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
            return IsComplete
                ? "P0 asset generation batches cover " + coveredChecks.Count + " check(s)."
                : "P0 asset generation batches have " + FailureCount + " failure(s) across " + coveredChecks.Count + " check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
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

        private int Count(P0AssetGenerationBatchCoverageSeverity severity)
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

    public static class P0AssetGenerationBatchCoverage
    {
        public const int ExpectedCoveredCheckCount = 4;

        public static P0AssetGenerationBatchCoverageReport EvaluateP0Batches()
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest(), P0AssetGenerationBatchCatalog.CreateP0Batches());
        }

        public static P0AssetGenerationBatchCoverageReport Evaluate(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches)
        {
            P0AssetGenerationBatchCoverageReport report = new P0AssetGenerationBatchCoverageReport();
            EvaluateBatchOrder(batches, report);
            EvaluateManifestCoverage(manifest, batches, report);
            EvaluatePromptPaths(batches, report);
            EvaluateDependencyOrder(manifest, batches, report);
            return report;
        }

        private static void EvaluateBatchOrder(
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches,
            P0AssetGenerationBatchCoverageReport report)
        {
            string[] expectedOrder =
            {
                P0AssetGenerationBatchCatalog.StyleAnchorBatchId,
                P0AssetGenerationBatchCatalog.GameplayPlaceholderBatchId,
                P0AssetGenerationBatchCatalog.BossReadinessBatchId,
                P0AssetGenerationBatchCatalog.RouteNodeIconBatchId,
                P0AssetGenerationBatchCatalog.UiShellBatchId,
                P0AssetGenerationBatchCatalog.BattleFeedbackVfxBatchId,
                P0AssetGenerationBatchCatalog.EnemyWarningVfxBatchId,
                P0AssetGenerationBatchCatalog.EnemyAnimationFramesheetBatchId,
                P0AssetGenerationBatchCatalog.RouteChoiceIconBatchId,
                P0AssetGenerationBatchCatalog.RouteRewardCardFrameBatchId,
                P0AssetGenerationBatchCatalog.StatusCompactIconBatchId,
                P0AssetGenerationBatchCatalog.RouteRewardDetailBadgeBatchId,
                P0AssetGenerationBatchCatalog.AuthorityBlessingSealBatchId,
                P0AssetGenerationBatchCatalog.NonBattleNodeSummaryBannerBatchId,
                P0AssetGenerationBatchCatalog.ShopItemCardBatchId,
                P0AssetGenerationBatchCatalog.DreamEventChoiceCardBatchId,
                P0AssetGenerationBatchCatalog.RestNestRecoveryCardBatchId,
                P0AssetGenerationBatchCatalog.PartnerChoiceCardBatchId,
                P0AssetGenerationBatchCatalog.BlessingChoiceCardBatchId,
                P0AssetGenerationBatchCatalog.ResultSettlementBannerBatchId,
                P0AssetGenerationBatchCatalog.CoreGaugeBarBatchId,
                P0AssetGenerationBatchCatalog.StarterCatHudAvatarBatchId,
                P0AssetGenerationBatchCatalog.SkillHudFeedbackBatchId,
                P0AssetGenerationBatchCatalog.StarterSkillVfxBatchId,
                P0AssetGenerationBatchCatalog.SaibanReferenceAtlasBatchId,
                P0AssetGenerationBatchCatalog.NephthysReferenceAtlasBatchId,
                P0AssetGenerationBatchCatalog.SuzuneReferenceAtlasBatchId
            };

            Require(
                report,
                MatchesExpectedOrder(batches, expectedOrder),
                "Generation order covers style anchors, gameplay placeholders, Boss readiness, route nodes, UI shell, battle feedback VFX, enemy warning VFX, enemy animation framesheets, route choice icons, route reward card frames, status compact icons, route reward detail badges, authority blessing seals, non-battle node summary banners, shop item cards, dream-event choice cards, RestNest recovery card, partner choice cards, blessing choice cards, result settlement banners, core gauge bars, starter cat HUD avatars, skill HUD feedback, starter skill VFX, and starter reference atlases.",
                "P0 asset generation batches are missing or out of order.");
        }

        private static void EvaluateManifestCoverage(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches,
            P0AssetGenerationBatchCoverageReport report)
        {
            Require(
                report,
                manifest != null
                && batches != null
                && AllManifestAssetsAssignedExactlyOnce(manifest, batches),
                "Generation batches assign every manifest asset exactly once.",
                "Generation batches do not exactly cover the P0 asset manifest.");
        }

        private static void EvaluatePromptPaths(
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches,
            P0AssetGenerationBatchCoverageReport report)
        {
            bool promptPathsValid = batches != null && batches.Count > 0;
            if (promptPathsValid)
            {
                for (int i = 0; i < batches.Count; i++)
                {
                    string path = batches[i].ExecutionPromptPath;
                    if (string.IsNullOrWhiteSpace(path)
                        || !path.StartsWith("design/development/agent_prompts/", StringComparison.Ordinal)
                        || !path.EndsWith(".md", StringComparison.Ordinal))
                    {
                        promptPathsValid = false;
                        break;
                    }
                }
            }

            Require(
                report,
                promptPathsValid,
                "Each generation batch points to a scoped asset agent prompt.",
                "One or more generation batches are missing a scoped asset agent prompt.");
        }

        private static void EvaluateDependencyOrder(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches,
            P0AssetGenerationBatchCoverageReport report)
        {
            Require(
                report,
                manifest != null
                && batches != null
                && RequiredAssetsComeFromEarlierBatches(batches)
                && AssetReferencesDoNotPointForward(manifest, batches),
                "Batch dependencies and manifest references never point to a later batch.",
                "A batch dependency or manifest reference points to an asset generated too late.");
        }

        private static bool AllManifestAssetsAssignedExactlyOnce(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches)
        {
            HashSet<string> manifestAssetIds = new HashSet<string>();
            for (int i = 0; i < manifest.Count; i++)
            {
                manifestAssetIds.Add(manifest[i].AssetId);
            }

            HashSet<string> assignedAssetIds = new HashSet<string>();
            int assignedCount = 0;
            for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
            {
                IReadOnlyList<string> assetIds = batches[batchIndex].AssetIds;
                for (int assetIndex = 0; assetIndex < assetIds.Count; assetIndex++)
                {
                    assignedCount++;
                    if (!manifestAssetIds.Contains(assetIds[assetIndex])
                        || !assignedAssetIds.Add(assetIds[assetIndex]))
                    {
                        return false;
                    }
                }
            }

            return assignedCount == manifest.Count && assignedAssetIds.Count == manifest.Count;
        }

        private static bool RequiredAssetsComeFromEarlierBatches(IReadOnlyList<P0AssetGenerationBatchDefinition> batches)
        {
            for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
            {
                IReadOnlyList<string> requiredAssetIds = batches[batchIndex].RequiredAssetIds;
                for (int requiredIndex = 0; requiredIndex < requiredAssetIds.Count; requiredIndex++)
                {
                    int requiredBatchIndex = FindBatchIndexForAsset(batches, requiredAssetIds[requiredIndex]);
                    if (requiredBatchIndex < 0 || requiredBatchIndex >= batchIndex)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool AssetReferencesDoNotPointForward(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches)
        {
            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                int entryBatchIndex = FindBatchIndexForAsset(batches, entry.AssetId);
                if (entryBatchIndex < 0)
                {
                    return false;
                }

                for (int referenceIndex = 0; referenceIndex < entry.ReferenceAssetIds.Count; referenceIndex++)
                {
                    int referenceBatchIndex = FindBatchIndexForAsset(batches, entry.ReferenceAssetIds[referenceIndex]);
                    if (referenceBatchIndex < 0 || referenceBatchIndex > entryBatchIndex)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static int FindBatchIndexForAsset(
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches,
            string assetId)
        {
            if (batches == null)
            {
                return -1;
            }

            for (int batchIndex = 0; batchIndex < batches.Count; batchIndex++)
            {
                IReadOnlyList<string> assetIds = batches[batchIndex].AssetIds;
                for (int assetIndex = 0; assetIndex < assetIds.Count; assetIndex++)
                {
                    if (assetIds[assetIndex] == assetId)
                    {
                        return batchIndex;
                    }
                }
            }

            return -1;
        }

        private static bool MatchesExpectedOrder(
            IReadOnlyList<P0AssetGenerationBatchDefinition> batches,
            IReadOnlyList<string> expectedOrder)
        {
            if (batches == null || expectedOrder == null || batches.Count != expectedOrder.Count)
            {
                return false;
            }

            for (int i = 0; i < expectedOrder.Count; i++)
            {
                if (batches[i].BatchId != expectedOrder[i])
                {
                    return false;
                }
            }

            return true;
        }

        private static void Require(
            P0AssetGenerationBatchCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetGenerationBatchCoverageSeverity.Failure, failureMessage);
        }
    }
}
