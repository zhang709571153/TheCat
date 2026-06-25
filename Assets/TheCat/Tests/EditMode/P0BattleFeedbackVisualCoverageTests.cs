using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0BattleFeedbackVisualCoverageTests
    {
        [Test]
        public void Build_ActiveVisualIncludesAccentProgressFillAndAlpha()
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.SkillCast,
                P0BattleFeedbackLevel.Positive,
                "圆盾冲锋",
                "护盾 +35",
                1f,
                0.8f);

            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 0.25f);

            Assert.IsTrue(visual.HasVisual);
            Assert.AreEqual("green", visual.AccentToken);
            Assert.AreEqual(0.25f, visual.Progress01, 0.001f);
            Assert.AreEqual(0.75f, visual.PulseFill01, 0.001f);
            Assert.AreEqual(0.6f, visual.PulseAlpha, 0.001f);
            Assert.AreEqual(P0VisualAssetCatalog.SaibanBedlineSkillVfxId, visual.VisualAsset.AssetId);
            StringAssert.Contains("正向 技能释放", visual.BuildSummary());
            StringAssert.Contains("正向 技能释放 - 圆盾冲锋", visual.BuildTitleLabel());
            StringAssert.Contains("资产 " + P0VisualAssetCatalog.SaibanBedlineSkillVfxId, visual.BuildSummary());
            Assert.IsFalse(visual.BuildSummary().Contains("Positive"));
            Assert.IsFalse(visual.BuildSummary().Contains("SkillCast"));
            Assert.IsFalse(visual.BuildTitleLabel().Contains("Positive"));
            Assert.IsFalse(visual.BuildTitleLabel().Contains("SkillCast"));
        }

        [Test]
        public void Build_ExpiredVisualKeepsFeedbackVisibleWithoutPulseFill()
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.BattleResult,
                P0BattleFeedbackLevel.Result,
                "胜利",
                "路线推进",
                0.5f,
                1f);

            P0BattleFeedbackVisualState visual = P0BattleFeedbackVisualPresenter.Build(feedback, 2f);

            Assert.IsTrue(visual.HasVisual);
            Assert.AreEqual("violet", visual.AccentToken);
            Assert.AreEqual(1f, visual.Progress01, 0.001f);
            Assert.AreEqual(0f, visual.PulseFill01, 0.001f);
            Assert.AreEqual(0f, visual.PulseAlpha, 0.001f);
            Assert.AreEqual(0f, visual.RemainingSeconds, 0.001f);
            Assert.AreEqual(P0VisualAssetCatalog.SleepStableWaveVfxId, visual.VisualAsset.AssetId);
        }

        [Test]
        public void EvaluatePrototypeVisuals_CompletesVisualCoverage()
        {
            P0BattleFeedbackVisualCoverageReport report = P0BattleFeedbackVisualCoverage.EvaluatePrototypeVisuals();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleFeedbackVisualCoverage.ExpectedCoveredVisualCount, report.CoveredVisuals.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("battle feedback visual coverage complete", report.BuildSummary());
            StringAssert.Contains("Critical feedback visual", report.BuildDetailedSummary());
            StringAssert.Contains("Result feedback visual", report.BuildDetailedSummary());
            StringAssert.Contains("Interaction success feedback", report.BuildDetailedSummary());
            StringAssert.Contains("Starter skill feedback", report.BuildDetailedSummary());
            StringAssert.Contains("enemy mark ring", report.BuildDetailedSummary());
            StringAssert.Contains("Boss throw warning visual", report.BuildDetailedSummary());
        }
    }
}
