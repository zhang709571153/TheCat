using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0CharacterDesignCoverageTests
    {
        [Test]
        public void EvaluatePrototypeRoster_CompletesStarterDesignGate()
        {
            P0CharacterDesignCoverageReport report = P0CharacterDesignCoverage.EvaluatePrototypeRoster();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0CharacterDesignCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("character design coverage complete", report.BuildSummary());
            StringAssert.Contains("roles, authorities, and attributes", report.BuildDetailedSummary());
            StringAssert.Contains("signature lines", report.BuildDetailedSummary());
            StringAssert.Contains("visual identity tokens", report.BuildDetailedSummary());
        }

        [Test]
        public void StarterPresentations_ExposeVoiceAndVisualIdentity()
        {
            AssertStarterPresentation(
                P0PrototypeCatalog.SaibanId,
                "Sacred Swordsman",
                "silver_oath_sun_sword",
                "bed");
            AssertStarterPresentation(
                P0PrototypeCatalog.NephthysId,
                "Moon-Sand Agent",
                "moon_sand_obelisk_crown",
                "dominion");
            AssertStarterPresentation(
                P0PrototypeCatalog.SuzuneId,
                "Sleep Shrine Miko",
                "moon_bell_torii",
                "bells");
        }

        private static void AssertStarterPresentation(
            string catId,
            string expectedTitle,
            string expectedVisualToken,
            string requiredSignatureText)
        {
            var presentation = P0CatPresenter.Describe(catId);

            Assert.AreEqual(expectedTitle, presentation.Title);
            Assert.AreEqual(expectedVisualToken, presentation.VisualToken);
            StringAssert.Contains("non-human", presentation.VisualIdentity);
            StringAssert.Contains(requiredSignatureText, presentation.SignatureLine.ToLowerInvariant());
            StringAssert.Contains(expectedVisualToken, presentation.BuildDesignLabel());
        }
    }
}
