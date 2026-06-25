using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0PlayModeAcceptanceSmokeTests
    {
        [Test]
        public void AcceptanceSequencePlan_CoversScreenshotRouteDefeatAndEvidenceGate()
        {
            Assert.IsTrue(P0PlayModeAcceptanceSmoke.HasP0AcceptanceSequencePlan());
            Assert.AreEqual(P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckCount, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds.Count);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.ScreenshotCapturePlanCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[0]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.RuntimeVisualScreenshotPlanCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[1]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.RuntimeVisualContactSheetCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[2]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.ScreenshotFileEvidenceCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[3]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.UnityRuntimeValidationPlanCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[4]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.ScreenshotSmokeCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[5]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.RouteFlowSmokeCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[6]);
            Assert.AreEqual(P0PlayModeEvidenceChecklist.DefeatFlowSmokeCheckId, P0PlayModeAcceptanceSmoke.ExpectedEvidenceCheckIds[7]);
        }
    }
}
