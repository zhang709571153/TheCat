using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0PlayModeDefeatFlowSmokeTests
    {
        [Test]
        public void StartDefaultDefeatSmoke_OutsidePlayModeFailsClearly()
        {
            bool started = P0PlayModeDefeatFlowSmoke.StartDefaultDefeatSmoke();

            Assert.IsFalse(started);
            Assert.AreEqual(P0PlayModeDefeatFlowSmokeState.Failed, P0PlayModeDefeatFlowSmoke.State);
            Assert.IsTrue(P0PlayModeDefeatFlowSmoke.IsFinished);
            StringAssert.Contains("requires Play Mode", P0PlayModeDefeatFlowSmoke.LastSummary);
            StringAssert.Contains("outside Play Mode", P0PlayModeDefeatFlowSmoke.LastDetailedLog);
        }
    }
}
