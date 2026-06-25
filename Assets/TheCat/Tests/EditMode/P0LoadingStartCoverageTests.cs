using NUnit.Framework;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0LoadingStartCoverageTests
    {
        [Test]
        public void Presenter_BuildsLoadingStartSurfaceForCatRoom()
        {
            P0LoadingStartSurface surface = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, 0.35f);

            Assert.IsTrue(P0LoadingStartPresenter.HasP0LoadingStartSurface(surface));
            Assert.AreEqual(P0SceneFlow.CatRoomSceneName, surface.TargetSceneName);
            Assert.AreEqual(0.35f, surface.Progress01);
            Assert.AreEqual("35%", surface.ProgressLabel);
            Assert.IsTrue(surface.HasScreenshotHook);
            StringAssert.Contains("35%", surface.BuildStatusLine());
            Assert.IsFalse(surface.BuildSummary().Contains("batch_83"));
            Assert.IsFalse(surface.BuildSummary().Contains(".png"));
        }

        [Test]
        public void Presenter_ClampsProgressForScreenshotStates()
        {
            P0LoadingStartSurface low = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.RouteMap, -1f);
            P0LoadingStartSurface high = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.RouteMap, 2f);

            Assert.AreEqual(0f, low.Progress01);
            Assert.AreEqual(1f, high.Progress01);
            Assert.AreEqual("0%", low.ProgressLabel);
            Assert.AreEqual("100%", high.ProgressLabel);
        }

        [Test]
        public void Presenter_RejectsCandidateTokensInDetailRows()
        {
            P0LoadingStartSurface valid = P0LoadingStartPresenter.BuildRunStartSurface(P0RunStartMode.CatRoom, 0.35f);
            P0LoadingStartSurface withCandidatePath = new P0LoadingStartSurface(
                valid.TitleLabel,
                valid.TargetSceneName,
                valid.TargetLabel,
                valid.StateLabel,
                valid.Progress01,
                valid.ProgressLabel,
                valid.SpinnerLabel,
                new[]
                {
                    "Assets/TheCat/Art/UI/batch_83_loading_start_preflight_2026-06-25/mock.png",
                    "candidate_v001.png",
                    ".meta"
                },
                valid.HasScreenshotHook);

            Assert.IsTrue(P0LoadingStartPresenter.HasP0LoadingStartSurface(valid));
            Assert.IsFalse(P0LoadingStartPresenter.HasP0LoadingStartSurface(withCandidatePath));
        }

        [Test]
        public void EvaluatePrototypeLoadingStart_CompletesCoverage()
        {
            P0LoadingStartCoverageReport report = P0LoadingStartCoverage.EvaluatePrototypeLoadingStart();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0LoadingStartCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("loading/start coverage complete", report.BuildSummary());
            StringAssert.Contains("Batch 83 boundary", report.BuildDetailedSummary());
        }
    }
}
