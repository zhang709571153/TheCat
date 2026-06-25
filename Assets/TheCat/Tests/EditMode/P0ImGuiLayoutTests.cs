using NUnit.Framework;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0ImGuiLayoutTests
    {
        [TestCase(1280f, 720f, 1f)]
        [TestCase(1920f, 1080f, P0ImGuiLayout.MaxScale)]
        [TestCase(800f, 600f, P0ImGuiLayout.MinScale)]
        [TestCase(1024f, 768f, 0.8f)]
        public void CalculateScale_UsesReferenceResolutionAndClamp(float width, float height, float expectedScale)
        {
            Assert.AreEqual(expectedScale, P0ImGuiLayout.CalculateScale(width, height), 0.001f);
        }

        [TestCase(1024f, 768f)]
        [TestCase(1280f, 720f)]
        [TestCase(1600f, 900f)]
        [TestCase(1920f, 1080f)]
        [TestCase(640f, 360f)]
        [TestCase(320f, 240f)]
        public void BuildLeftPanelRect_StaysInsideScreenAtCommonAndNarrowResolutions(float width, float height)
        {
            Rect rect = P0ImGuiLayout.BuildLeftPanelRect(width, height, 360f, 600f, 0.44f);

            Assert.GreaterOrEqual(rect.xMin, 0f);
            Assert.GreaterOrEqual(rect.yMin, 0f);
            Assert.LessOrEqual(rect.xMax, width);
            Assert.LessOrEqual(rect.yMax, height);
            Assert.Greater(rect.width, 0f);
            Assert.Greater(rect.height, 0f);
        }

        [Test]
        public void BuildLeftPanelRect_UsesExpectedRouteMapWidthAtReferenceResolution()
        {
            Rect rect = P0ImGuiLayout.BuildLeftPanelRect(1280f, 720f, 360f, 600f, 0.44f);

            Assert.AreEqual(16f, rect.x, 0.001f);
            Assert.AreEqual(16f, rect.y, 0.001f);
            Assert.AreEqual(563.2f, rect.width, 0.001f);
            Assert.AreEqual(688f, rect.height, 0.001f);
        }

        [Test]
        public void BuildLeftPanelRect_FallsBackToReferenceSizeWhenScreenSizeIsUnavailable()
        {
            Rect rect = P0ImGuiLayout.BuildLeftPanelRect(0f, 0f, 360f, 600f, 0.44f);

            Assert.AreEqual(16f, rect.x, 0.001f);
            Assert.AreEqual(16f, rect.y, 0.001f);
            Assert.AreEqual(563.2f, rect.width, 0.001f);
            Assert.AreEqual(688f, rect.height, 0.001f);
        }

        [Test]
        public void ScrollContentWidth_ReservesScrollbarWithoutCollapsing()
        {
            Rect rect = P0ImGuiLayout.BuildLeftPanelRect(640f, 360f, 360f, 600f, 0.44f);

            float innerWidth = P0ImGuiLayout.InnerWidth(rect);
            float scrollContentWidth = P0ImGuiLayout.ScrollContentWidth(rect);

            Assert.Greater(scrollContentWidth, 0f);
            Assert.Less(scrollContentWidth, innerWidth);
        }

        [TestCase(270f, true)]
        [TestCase(390f, true)]
        [TestCase(520f, false)]
        public void ShouldStackControls_UsesReadablePanelWidth(float availableWidth, bool expected)
        {
            Assert.AreEqual(expected, P0ImGuiLayout.ShouldStackControls(availableWidth, 390f, 1f));
        }
    }
}
