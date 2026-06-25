using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StarterCatTurnaroundSourceLocksTests
    {
        [Test]
        public void EvaluateP0Locks_CurrentStarterCatSpritesMatchLockedTurnarounds()
        {
            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.EvaluateP0Locks();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatTurnaroundSourceLocks.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(3, report.LockCount);
            Assert.AreEqual(3, report.SourceHashMatchedCount);
            Assert.AreEqual(3, report.SpriteHashMatchedCount);
            Assert.AreEqual(3, report.ManifestMatchedCount);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("colored turnaround", report.BuildDetailedSummary());
            StringAssert.Contains("SHA-256", report.BuildDetailedSummary());
            StringAssert.Contains("source-lock ids", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateP0VisualConsistencyChecklist_CurrentChecklistPinsScreenshotsTraitsAndSourceLocks()
        {
            P0StarterCatVisualConsistencyReport report = P0StarterCatVisualConsistencyChecklist.EvaluateP0Checklist();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatVisualConsistencyChecklist.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(3, report.ChecklistCount);
            Assert.AreEqual(15, report.RequiredTraitCount);
            Assert.AreEqual(3, report.ScreenshotPlanMatchedCount);
            Assert.AreEqual(3, report.SourceLockMatchedCount);
            Assert.AreEqual(6, report.ExistingReferenceFileCount);
            StringAssert.Contains("colored-turnaround visual traits", report.BuildDetailedSummary());
            StringAssert.Contains("active-cat Play Mode screenshots", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateP0AssetProductionSpec_CurrentSpecPinsStrictDerivativeRules()
        {
            P0StarterCatAssetProductionSpecReport report = P0StarterCatAssetProductionSpec.EvaluateP0Spec();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatAssetProductionSpec.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(3, report.SpecCount);
            Assert.AreEqual(3, report.SourceLockMatchedCount);
            Assert.AreEqual(12, report.AllowedDerivativeAssetTypeCount);
            Assert.GreaterOrEqual(report.RequiredEvidenceCount, 24);
            Assert.GreaterOrEqual(report.PromptClauseCount, 21);
            Assert.GreaterOrEqual(report.RejectionRuleCount, 12);
            StringAssert.Contains("allowed derivatives", report.BuildDetailedSummary());
            StringAssert.Contains("colored turnarounds", report.BuildDetailedSummary());
            StringAssert.Contains("rejection rules", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateP0ProductionPromptReadiness_CurrentPromptsUseRealThreeViewSources()
        {
            P0StarterCatProductionPromptReadinessReport report = P0StarterCatProductionPromptReadiness.EvaluateCurrentPrompts();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatProductionPromptReadiness.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(P0StarterCatProductionPromptReadiness.ExpectedPromptCount, report.PromptCount);
            Assert.AreEqual(P0StarterCatProductionPromptReadiness.ExpectedPromptCount, report.ReadablePromptCount);
            Assert.AreEqual(P0StarterCatProductionPromptReadiness.ExpectedPromptCount, report.DesignRootMentionCount);
            Assert.AreEqual(0, report.MojibakePathMentionCount);
            Assert.GreaterOrEqual(report.SourceTurnaroundPathMentionCount, P0StarterCatProductionPromptReadiness.ExpectedPromptCount * 3);
            Assert.GreaterOrEqual(report.OneCatAtATimeMentionCount, 1);
            StringAssert.Contains("real UTF-8 design source path", report.BuildDetailedSummary());
            StringAssert.Contains("one-cat-at-a-time", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateProductionPromptReadiness_MojibakeDesignPathFailsGate()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            string[] promptPaths =
            {
                "design/development/agent_prompts/p0_asset_batch_17_starter_cat_turnaround_conformance_gate.md",
                "design/development/agent_prompts/p0_asset_batch_18_starter_cat_strict_candidate_production.md",
                "design/development/agent_prompts/p0_asset_batch_26_starter_cat_candidate_gate.md",
                "design/development/agent_prompts/p0_asset_batch_28_starter_cat_strict_reference_pack.md",
                "design/development/agent_prompts/p0_asset_batch_29_saiban_strict_turnaround_derivatives.md",
                "design/development/agent_prompts/p0_asset_batch_30_saiban_ai_refinement_candidate.md",
                "design/development/agent_prompts/p0_asset_batch_31_saiban_cutout_candidate.md",
                "design/development/agent_prompts/p0_asset_batch_32_nephthys_strict_turnaround_derivatives.md",
                "design/development/agent_prompts/p0_asset_batch_33_suzune_strict_turnaround_derivatives.md",
                "design/development/agent_prompts/p0_asset_batch_34_suzune_ai_refinement_candidate.md",
                "design/development/agent_prompts/p0_asset_batch_35_suzune_cutout_candidate.md",
                "design/development/agent_prompts/p0_asset_batch_36_nephthys_ai_refinement_candidate.md",
                "design/development/agent_prompts/p0_asset_batch_37_nephthys_cutout_candidate.md"
            };

            P0StarterCatProductionPromptReadinessReport report = P0StarterCatProductionPromptReadiness.Evaluate(
                promptPaths,
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticPromptText(locks);
                    if (path.EndsWith("p0_asset_batch_18_starter_cat_strict_candidate_production.md", StringComparison.Ordinal))
                    {
                        text += "\ndesign/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/characters/broken.png";
                    }

                    error = string.Empty;
                    return true;
                });

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.MojibakePathMentionCount);
            StringAssert.Contains("mojibake design paths", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateP0TurnaroundConformanceSpec_CurrentSpecPinsFrontSideBackAnchors()
        {
            P0StarterCatTurnaroundConformanceSpecReport report = P0StarterCatTurnaroundConformanceSpec.EvaluateP0Spec();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatTurnaroundConformanceSpec.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(3, report.SpecCount);
            Assert.AreEqual(3, report.SourceLockMatchedCount);
            Assert.AreEqual(3, report.ExistingSourceFileCount);
            Assert.AreEqual(9, report.FrontViewAnchorCount);
            Assert.AreEqual(9, report.SideViewAnchorCount);
            Assert.AreEqual(9, report.BackViewAnchorCount);
            Assert.AreEqual(9, report.PaletteAnchorCount);
            Assert.AreEqual(9, report.PropAndCostumeAnchorCount);
            Assert.AreEqual(12, report.ProhibitedDriftRuleCount);
            StringAssert.Contains("front, side, and back anchors", report.BuildDetailedSummary());
            StringAssert.Contains("colored three-view turnarounds", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingSourceTurnaroundFails()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry missing = locks[0];

            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.Evaluate(
                locks,
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                path => path != missing.SourceTurnaroundPath,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("source turnaround is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ChangedSpriteHashFails()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry changed = locks[1];

            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.Evaluate(
                locks,
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                path => true,
                (string path, out string sha256, out string error) =>
                {
                    error = string.Empty;
                    if (path == changed.SpritePath)
                    {
                        sha256 = "0000000000000000000000000000000000000000000000000000000000000000";
                        return true;
                    }

                    return HashFromLocks(locks)(path, out sha256, out error);
                });

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("locked sprite hash should be", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_DuplicateStarterCatLockFails()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            var duplicateLocks = new[]
            {
                locks[0],
                locks[0],
                locks[2]
            };

            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.Evaluate(
                duplicateLocks,
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("duplicate starter cat source lock", report.BuildDetailedSummary());
            StringAssert.Contains("do not cover exactly", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ManifestMustPointAtGeneratedLockedSprite()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry changed = locks[2];
            IReadOnlyList<P0AssetManifestEntry> manifest = MutateManifest(changed.AssetId, entry => new P0AssetManifestEntry(
                entry.AssetId,
                entry.SubjectId,
                entry.AssetType,
                entry.Priority,
                entry.SourcePromptPath,
                entry.ReferenceAssetIds,
                entry.SourceLockIds,
                "Assets/TheCat/Art/Characters/Sprites/wrong_suzune.png",
                entry.Size,
                P0AssetManifestStatus.Planned,
                entry.ConsistencyNotes));

            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.Evaluate(
                locks,
                manifest,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("manifest row does not match", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ManifestNotesRequireHardReferenceFrontView()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry changed = locks[0];
            IReadOnlyList<P0AssetManifestEntry> manifest = MutateManifest(changed.AssetId, entry => new P0AssetManifestEntry(
                entry.AssetId,
                entry.SubjectId,
                entry.AssetType,
                entry.Priority,
                entry.SourcePromptPath,
                entry.ReferenceAssetIds,
                entry.SourceLockIds,
                entry.UnityImportPath,
                entry.Size,
                entry.Status,
                "Generic cat sprite."));

            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.Evaluate(
                locks,
                manifest,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("colored turnaround hard reference front view", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_ManifestRequiresTurnaroundSourceLockId()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry changed = locks[1];
            IReadOnlyList<P0AssetManifestEntry> manifest = MutateManifest(changed.AssetId, entry => new P0AssetManifestEntry(
                entry.AssetId,
                entry.SubjectId,
                entry.AssetType,
                entry.Priority,
                entry.SourcePromptPath,
                entry.ReferenceAssetIds,
                new string[0],
                entry.UnityImportPath,
                entry.Size,
                entry.Status,
                entry.ConsistencyNotes));

            P0StarterCatTurnaroundSourceLockReport report = P0StarterCatTurnaroundSourceLocks.Evaluate(
                locks,
                manifest,
                path => true,
                HashFromLocks(locks));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("manifest row must reference source lock nephthys_turnaround_colored", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_VisualConsistencyChecklistRequiresCatSpecificTraits()
        {
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist = MutateChecklist(
                P0PrototypeCatalog.SaibanId,
                entry => new P0StarterCatVisualConsistencyChecklistEntry(
                    entry.CatId,
                    entry.DisplayName,
                    entry.SourceLockId,
                    entry.SourceTurnaroundPath,
                    entry.SpritePath,
                    entry.PlayModeScreenshotFileName,
                    new[]
                    {
                        "generic cute cat",
                        "soft dream painting",
                        "small readable sprite",
                        "blue accent",
                        "front pose"
                    },
                    entry.ProhibitedDriftRule));

            P0StarterCatVisualConsistencyReport report = P0StarterCatVisualConsistencyChecklist.Evaluate(
                checklist,
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                path => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("missing required colored-turnaround visual traits", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_VisualConsistencyChecklistRequiresRegisteredActiveCatScreenshot()
        {
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist = MutateChecklist(
                P0PrototypeCatalog.SuzuneId,
                entry => new P0StarterCatVisualConsistencyChecklistEntry(
                    entry.CatId,
                    entry.DisplayName,
                    entry.SourceLockId,
                    entry.SourceTurnaroundPath,
                    entry.SpritePath,
                    "wrong-suzune.png",
                    entry.RequiredTraits,
                    entry.ProhibitedDriftRule));

            P0StarterCatVisualConsistencyReport report = P0StarterCatVisualConsistencyChecklist.Evaluate(
                checklist,
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                path => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("registered active-cat Play Mode capture", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_TurnaroundConformanceSpecRequiresSideAndBackAnchors()
        {
            IReadOnlyList<P0StarterCatTurnaroundConformanceSpecEntry> specs = MutateTurnaroundConformanceSpec(
                P0PrototypeCatalog.SaibanId,
                entry => new P0StarterCatTurnaroundConformanceSpecEntry(
                    entry.CatId,
                    entry.DisplayName,
                    entry.SourceLockId,
                    entry.SourceTurnaroundPath,
                    entry.FrontViewAnchors,
                    new[] { "generic side pose" },
                    new[] { "generic back pose" },
                    entry.PaletteAnchors,
                    entry.PropAndCostumeAnchors,
                    entry.ProhibitedDriftRules));

            P0StarterCatTurnaroundConformanceSpecReport report = P0StarterCatTurnaroundConformanceSpec.Evaluate(
                specs,
                P0StarterCatVisualConsistencyChecklist.CreateP0Checklist(),
                path => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("strict front, side, and back view anchors", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_AssetProductionSpecRejectsGenericPromptClauses()
        {
            IReadOnlyList<P0StarterCatAssetProductionSpecEntry> specs = MutateProductionSpec(
                P0PrototypeCatalog.NephthysId,
                entry => new P0StarterCatAssetProductionSpecEntry(
                    entry.CatId,
                    entry.DisplayName,
                    entry.SourceLockId,
                    entry.SourceTurnaroundPath,
                    entry.CurrentSpritePath,
                    entry.ActiveCatScreenshotFileName,
                    entry.CandidateDirectory,
                    entry.ApprovedImportDirectory,
                    entry.AllowedDerivativeAssetTypes,
                    entry.RequiredEvidence,
                    new[]
                    {
                        "cute dream cat",
                        "soft ancient fantasy costume",
                        "small readable sprite",
                        "blue accent",
                        "front pose",
                        "nice expression"
                    },
                    entry.RejectionRules));

            P0StarterCatAssetProductionSpecReport report = P0StarterCatAssetProductionSpec.Evaluate(
                specs,
                P0StarterCatVisualConsistencyChecklist.CreateP0Checklist());

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("strict colored-turnaround prompt clauses", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_AssetProductionSpecRequiresCandidateReviewDirectory()
        {
            IReadOnlyList<P0StarterCatAssetProductionSpecEntry> specs = MutateProductionSpec(
                P0PrototypeCatalog.SaibanId,
                entry => new P0StarterCatAssetProductionSpecEntry(
                    entry.CatId,
                    entry.DisplayName,
                    entry.SourceLockId,
                    entry.SourceTurnaroundPath,
                    entry.CurrentSpritePath,
                    entry.ActiveCatScreenshotFileName,
                    "Assets/TheCat/Art/Characters/Sprites",
                    entry.ApprovedImportDirectory,
                    entry.AllowedDerivativeAssetTypes,
                    entry.RequiredEvidence,
                    entry.RequiredPromptClauses,
                    entry.RejectionRules));

            P0StarterCatAssetProductionSpecReport report = P0StarterCatAssetProductionSpec.Evaluate(
                specs,
                P0StarterCatVisualConsistencyChecklist.CreateP0Checklist());

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("keep candidates outside Assets", report.BuildDetailedSummary());
        }

        private static IReadOnlyList<P0AssetManifestEntry> MutateManifest(string assetId, Func<P0AssetManifestEntry, P0AssetManifestEntry> mutate)
        {
            List<P0AssetManifestEntry> result = new List<P0AssetManifestEntry>();
            foreach (P0AssetManifestEntry entry in P0AssetManifestCatalog.CreateP0PlannedManifest())
            {
                result.Add(entry.AssetId == assetId ? mutate(entry) : entry);
            }

            return result;
        }

        private static IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> MutateChecklist(
            string catId,
            Func<P0StarterCatVisualConsistencyChecklistEntry, P0StarterCatVisualConsistencyChecklistEntry> mutate)
        {
            List<P0StarterCatVisualConsistencyChecklistEntry> result = new List<P0StarterCatVisualConsistencyChecklistEntry>();
            foreach (P0StarterCatVisualConsistencyChecklistEntry entry in P0StarterCatVisualConsistencyChecklist.CreateP0Checklist())
            {
                result.Add(entry.CatId == catId ? mutate(entry) : entry);
            }

            return result;
        }

        private static IReadOnlyList<P0StarterCatAssetProductionSpecEntry> MutateProductionSpec(
            string catId,
            Func<P0StarterCatAssetProductionSpecEntry, P0StarterCatAssetProductionSpecEntry> mutate)
        {
            List<P0StarterCatAssetProductionSpecEntry> result = new List<P0StarterCatAssetProductionSpecEntry>();
            foreach (P0StarterCatAssetProductionSpecEntry entry in P0StarterCatAssetProductionSpec.CreateP0Spec())
            {
                result.Add(entry.CatId == catId ? mutate(entry) : entry);
            }

            return result;
        }

        private static IReadOnlyList<P0StarterCatTurnaroundConformanceSpecEntry> MutateTurnaroundConformanceSpec(
            string catId,
            Func<P0StarterCatTurnaroundConformanceSpecEntry, P0StarterCatTurnaroundConformanceSpecEntry> mutate)
        {
            List<P0StarterCatTurnaroundConformanceSpecEntry> result = new List<P0StarterCatTurnaroundConformanceSpecEntry>();
            foreach (P0StarterCatTurnaroundConformanceSpecEntry entry in P0StarterCatTurnaroundConformanceSpec.CreateP0Spec())
            {
                result.Add(entry.CatId == catId ? mutate(entry) : entry);
            }

            return result;
        }

        private static P0StarterCatHashReader HashFromLocks(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            Dictionary<string, string> hashes = new Dictionary<string, string>();
            for (int i = 0; i < locks.Count; i++)
            {
                hashes[locks[i].SourceTurnaroundPath] = locks[i].SourceTurnaroundSha256;
                hashes[locks[i].SpritePath] = locks[i].SpriteSha256;
            }

            return (string path, out string sha256, out string error) =>
            {
                if (hashes.TryGetValue(path, out sha256))
                {
                    error = string.Empty;
                    return true;
                }

                sha256 = string.Empty;
                error = "No test hash for " + path;
                return false;
            };
        }

        private static string BuildSyntheticPromptText(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            List<string> lines = new List<string>
            {
                P0StarterCatProductionPromptReadiness.DesignRoot,
                P0StarterCatProductionPromptReadiness.CandidateRoot,
                "Keep outputs outside `Assets`.",
                "Formal import remains blocked until active-cat Play Mode screenshot review passes.",
                "Use one-cat-at-a-time strict candidate production."
            };

            for (int i = 0; i < locks.Count; i++)
            {
                lines.Add(locks[i].SourceTurnaroundPath);
            }

            return string.Join("\n", lines);
        }
    }
}
