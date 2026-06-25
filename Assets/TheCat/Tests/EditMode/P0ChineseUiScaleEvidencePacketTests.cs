using System;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0ChineseUiScaleEvidencePacketTests
    {
        [Test]
        public void EvaluateCurrentPacket_IsReadyForUnityScaleEvidenceCapture()
        {
            P0ChineseUiScaleEvidencePacketReport report = P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0ChineseUiScaleEvidencePacket.ExpectedPacketFileCount, report.ExistingExpectedFileCount);
            Assert.AreEqual(P0ChineseUiScaleEvidencePacket.ExpectedTemplateManifestRowCount, report.ManifestRowCount);
            Assert.AreEqual(P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount, report.CaptureMatrixRowCount);
            Assert.AreEqual(P0ChineseUiScaleValidationPlan.ExpectedSurfaceCount, report.SurfaceCoverageCount);
            Assert.AreEqual(P0ChineseUiScaleValidationPlan.ExpectedResolutionCount, report.ResolutionCoverageCount);
            Assert.AreEqual(P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount, report.SurfaceResolutionPairCoverageCount);
            Assert.AreEqual(0, report.UnexpectedMetaFileCount);
            Assert.IsTrue(report.ReviewNoteReady);
            Assert.IsTrue(report.ProcessNoteReady);
            StringAssert.Contains(P0ChineseUiScaleEvidencePacket.BatchSlug, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingManifestFailsReadiness()
        {
            P0ChineseUiScaleEvidencePacketReport report = P0ChineseUiScaleEvidencePacket.Evaluate(
                P0ChineseUiScaleEvidencePacket.DefaultBatchDirectory,
                path => !path.EndsWith(P0ChineseUiScaleEvidencePacket.ManifestFileName, StringComparison.Ordinal)
                    && P0ChineseUiScaleEvidencePacket.DefaultFileExists(path),
                P0ChineseUiScaleEvidencePacket.DefaultReadText,
                P0ChineseUiScaleEvidencePacket.DefaultEnumerateFileNames);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.MissingExpectedFileCount);
            StringAssert.Contains(P0ChineseUiScaleEvidencePacket.ManifestFileName, report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingResolutionPairFailsReadiness()
        {
            P0ChineseUiScaleEvidencePacketReport report = P0ChineseUiScaleEvidencePacket.Evaluate(
                P0ChineseUiScaleEvidencePacket.DefaultBatchDirectory,
                P0ChineseUiScaleEvidencePacket.DefaultFileExists,
                ReadWithBrokenWideResolution,
                P0ChineseUiScaleEvidencePacket.DefaultEnumerateFileNames);

            Assert.IsFalse(report.IsReady);
            Assert.Less(report.ResolutionCoverageCount, P0ChineseUiScaleValidationPlan.ExpectedResolutionCount);
            Assert.Less(report.SurfaceResolutionPairCoverageCount, P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount);
            StringAssert.Contains("missing one or more required resolutions", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MetaFilePresenceFailsReadiness()
        {
            P0ChineseUiScaleEvidencePacketReport report = P0ChineseUiScaleEvidencePacket.Evaluate(
                P0ChineseUiScaleEvidencePacket.DefaultBatchDirectory,
                P0ChineseUiScaleEvidencePacket.DefaultFileExists,
                P0ChineseUiScaleEvidencePacket.DefaultReadText,
                _ => new[] { "thecat_ui_chinese_scale_capture_matrix_batch75_1920x1080_v001.png.meta" });

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.UnexpectedMetaFileCount);
            StringAssert.Contains("may have been imported accidentally", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_UnsafeManifestRecommendationFailsReadiness()
        {
            P0ChineseUiScaleEvidencePacketReport report = P0ChineseUiScaleEvidencePacket.Evaluate(
                P0ChineseUiScaleEvidencePacket.DefaultBatchDirectory,
                P0ChineseUiScaleEvidencePacket.DefaultFileExists,
                ReadWithUnsafeRecommendation,
                P0ChineseUiScaleEvidencePacket.DefaultEnumerateFileNames);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(0, report.ManifestTemplateOnlyRecommendationCount);
            StringAssert.Contains("unsafe import recommendations", report.BuildDetailedSummary());
        }

        private static bool ReadWithBrokenWideResolution(string path, out string text, out string error)
        {
            bool ok = P0ChineseUiScaleEvidencePacket.DefaultReadText(path, out text, out error);
            if (ok && path.EndsWith(P0ChineseUiScaleEvidencePacket.CaptureMatrixFileName, StringComparison.Ordinal))
            {
                text = text.Replace("1920x1080", "1919x1080");
            }

            return ok;
        }

        private static bool ReadWithUnsafeRecommendation(string path, out string text, out string error)
        {
            bool ok = P0ChineseUiScaleEvidencePacket.DefaultReadText(path, out text, out error);
            if (ok && path.EndsWith(P0ChineseUiScaleEvidencePacket.ManifestFileName, StringComparison.Ordinal))
            {
                text = text.Replace("validation_template_only_do_not_import", "import_runtime_now");
            }

            return ok;
        }
    }
}
