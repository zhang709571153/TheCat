using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0RuntimeVisualBindingCoverageTests
    {
        [Test]
        public void EvaluateP0Bindings_AllCurrentBindingsResolveEditorTextures()
        {
            P0RuntimeVisualBindingCoverageReport report = P0RuntimeVisualBindingCoverage.EvaluateP0Bindings();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0VisualAssetCatalog.P0RuntimeVisualBindingCount, report.BindingCount);
            Assert.AreEqual(P0VisualAssetCatalog.P0RuntimeVisualBindingCount, report.ResolvedTextureCount);
            Assert.AreEqual(P0RuntimeVisualBindingCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("Battle world background binding", report.BuildDetailedSummary());
            StringAssert.Contains("Core gauge bar bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Status HUD bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Status HUD compact bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Route map bindings", report.BuildDetailedSummary());
            StringAssert.Contains("UI shell bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Enemy warning VFX bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Enemy animation framesheet bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Battle feedback VFX bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Route choice icon bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Route reward card frame bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Route reward detail badge bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Authority blessing detail bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Route node summary banner bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Shop item card bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Dream-event choice card bindings", report.BuildDetailedSummary());
            StringAssert.Contains("RestNest recovery card binding", report.BuildDetailedSummary());
            StringAssert.Contains("Partner choice card bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Authority blessing choice card bindings", report.BuildDetailedSummary());
            StringAssert.Contains("Outcome banner bindings", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingTextureFailsBindingCoverage()
        {
            P0RuntimeVisualBindingCoverageReport report = P0RuntimeVisualBindingCoverage.Evaluate(
                P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                asset => asset.AssetId != P0VisualAssetCatalog.SaibanCombatSpriteId);

            Assert.IsFalse(report.IsComplete);
            StringAssert.Contains("cat.combat.saiban could not resolve", report.BuildDetailedSummary());
            StringAssert.Contains("One or more runtime visual bindings could not resolve a texture", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingStarterCatSourceLockFailsBindingCoverage()
        {
            P0VisualAssetBinding[] bindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            ReplaceBindingAssetWithoutSourceLocks(bindings, "cat.combat.saiban");

            P0RuntimeVisualBindingCoverageReport report = P0RuntimeVisualBindingCoverage.Evaluate(bindings, _ => true);

            Assert.IsFalse(report.IsComplete);
            StringAssert.Contains("Starter cat bindings must use colored-turnaround source-locked sprites", report.BuildDetailedSummary());
        }

        private static void ReplaceBindingAssetWithoutSourceLocks(
            IList<P0VisualAssetBinding> bindings,
            string bindingId)
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].BindingId != bindingId)
                {
                    continue;
                }

                P0VisualAssetReference asset = bindings[i].Asset;
                P0VisualAssetReference unlockedAsset = new P0VisualAssetReference(
                    asset.AssetId,
                    asset.UnityImportPath,
                    asset.AssetType,
                    asset.Status,
                    asset.ReferenceAssetIds,
                    null);

                bindings[i] = new P0VisualAssetBinding(
                    bindings[i].BindingId,
                    bindings[i].SurfaceId,
                    bindings[i].SubjectId,
                    bindings[i].SlotId,
                    unlockedAsset,
                    bindings[i].SourceAuthority);
                return;
            }

            Assert.Fail("Missing binding: " + bindingId);
        }
    }
}
