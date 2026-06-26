using NUnit.Framework;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0BattleResultCoverageTests
    {
        [Test]
        public void EvaluatePrototypeSurface_CompletesBattleResultCoverage()
        {
            P0BattleResultCoverageReport report = P0BattleResultCoverage.EvaluatePrototypeSurface();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0BattleResultCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("battle result coverage complete", report.BuildSummary());
            StringAssert.Contains("Victory result surface", report.BuildDetailedSummary());
            StringAssert.Contains("outcome banner assets", report.BuildDetailedSummary());
            StringAssert.Contains("In-progress result surface", report.BuildDetailedSummary());
            StringAssert.Contains("Player-facing result focus rows", report.BuildDetailedSummary());
            StringAssert.Contains("Result action shortcut labels", report.BuildDetailedSummary());
        }

        [Test]
        public void Action_BuildButtonLabelIncludesShortcutAndLockedState()
        {
            P0BattleResultAction action = new P0BattleResultAction(
                P0BattleResultActionIds.ContinueRoute,
                P0InputCommand.ContinueRoute,
                "继续路线",
                "返回路线图。",
                "Enter",
                P0SceneFlow.RouteMapSceneName,
                false,
                "战斗结束后可用");

            string label = action.BuildButtonLabel();

            StringAssert.Contains("继续路线", label);
            StringAssert.Contains("Enter", label);
            StringAssert.Contains("战斗结束后可用", label);
            Assert.IsFalse(label.Contains("Continue Route"));
            Assert.IsFalse(label.Contains("locked"));
            Assert.IsFalse(label.Contains("未解锁"));
        }
    }
}
