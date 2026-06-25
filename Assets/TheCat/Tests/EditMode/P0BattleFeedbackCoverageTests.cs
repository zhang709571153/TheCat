using NUnit.Framework;
using TheCat.Combat;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0BattleFeedbackCoverageTests
    {
        [Test]
        public void Feedback_BuildSummaryIncludesLevelKindPulseAndIntensity()
        {
            P0BattleFeedback feedback = new P0BattleFeedback(
                P0BattleFeedbackKind.InteractionSuccess,
                P0BattleFeedbackLevel.Positive,
                "守床照看",
                "睡眠 +12",
                0.55f,
                0.5f);

            string summary = feedback.BuildSummary();

            StringAssert.Contains("正向", summary);
            StringAssert.Contains("交互成功", summary);
            StringAssert.Contains("守床照看", summary);
            StringAssert.Contains("脉冲 0.55s", summary);
            StringAssert.Contains("强度 0.50", summary);
        }

        [Test]
        public void BattleResult_FeedbackDistinguishesVictoryAndDefeat()
        {
            P0BattleFeedback victory = P0BattleFeedbackPresenter.BuildBattleResult(BattleOutcome.Victory, 31f, "路线推进");
            P0BattleFeedback defeat = P0BattleFeedbackPresenter.BuildBattleResult(BattleOutcome.Defeat, 42f, "主人睡眠崩溃");

            Assert.AreEqual(P0BattleFeedbackLevel.Result, victory.Level);
            Assert.AreEqual(P0BattleFeedbackLevel.Critical, defeat.Level);
            StringAssert.Contains("胜利", victory.BuildSummary());
            StringAssert.Contains("主人睡眠崩溃", defeat.BuildSummary());
        }

        [Test]
        public void EvaluatePrototypeFeedback_CompletesFeedbackCoverage()
        {
            P0BattleFeedbackCoverageReport report = P0BattleFeedbackCoverage.EvaluatePrototypeFeedback();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleFeedbackCoverage.ExpectedCoveredFeedbackCount, report.CoveredFeedback.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("battle feedback coverage complete", report.BuildSummary());
            StringAssert.Contains("Skill cast feedback", report.BuildDetailedSummary());
            StringAssert.Contains("Cat pressure feedback", report.BuildDetailedSummary());
            StringAssert.Contains("Shielded cat pressure", report.BuildDetailedSummary());
            StringAssert.Contains("Battle result feedback", report.BuildDetailedSummary());
        }
    }
}
