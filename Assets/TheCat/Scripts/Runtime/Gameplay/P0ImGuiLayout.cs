using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0ImGuiLayout
    {
        public const float ReferenceWidth = 1280f;
        public const float ReferenceHeight = 720f;
        public const float MinScale = 0.75f;
        public const float MaxScale = 1.15f;
        public const float NarrowControlsReferenceWidth = 390f;

        public static float Scale
        {
            get
            {
                return CalculateScale(Screen.width, Screen.height);
            }
        }

        public static float Margin => Scaled(16f);

        public static float CompactButtonHeight => Scaled(28f);

        public static float ButtonHeight => Scaled(36f);

        public static float PrimaryButtonHeight => Scaled(42f);

        public static float SectionSpacing => Scaled(8f);

        public static float ScrollbarReserve => Scaled(20f);

        public static float Scaled(float value)
        {
            return Scaled(value, Scale);
        }

        public static Rect BuildLeftPanelRect(float minWidth, float maxWidth, float widthFraction)
        {
            return BuildLeftPanelRect(Screen.width, Screen.height, minWidth, maxWidth, widthFraction);
        }

        public static float CalculateScale(float screenWidth, float screenHeight)
        {
            float widthScale = screenWidth <= 0f ? 1f : screenWidth / ReferenceWidth;
            float heightScale = screenHeight <= 0f ? 1f : screenHeight / ReferenceHeight;
            return Mathf.Clamp(Mathf.Min(widthScale, heightScale), MinScale, MaxScale);
        }

        public static float Scaled(float value, float scale)
        {
            return Mathf.Max(1f, value * scale);
        }

        public static Rect BuildLeftPanelRect(
            float screenWidth,
            float screenHeight,
            float minWidth,
            float maxWidth,
            float widthFraction)
        {
            float safeWidth = screenWidth <= 0f ? ReferenceWidth : screenWidth;
            float safeHeight = screenHeight <= 0f ? ReferenceHeight : screenHeight;
            float scale = CalculateScale(safeWidth, safeHeight);
            float margin = Scaled(16f, scale);
            float availableWidth = Mathf.Max(1f, safeWidth - margin * 2f);
            float availableHeight = Mathf.Max(1f, safeHeight - margin * 2f);
            float desiredWidth = Mathf.Clamp(
                safeWidth * widthFraction,
                Scaled(minWidth, scale),
                Scaled(maxWidth, scale));
            float width = Mathf.Min(desiredWidth, availableWidth);
            return new Rect(margin, margin, width, availableHeight);
        }

        public static RectOffset Padding(float basePadding = 14f)
        {
            int padding = Mathf.RoundToInt(Scaled(basePadding));
            return new RectOffset(padding, padding, padding, padding);
        }

        public static float InnerWidth(Rect rect, float baseHorizontalPadding = 14f)
        {
            return Mathf.Max(1f, rect.width - Scaled(baseHorizontalPadding) * 2f);
        }

        public static float ScrollContentWidth(Rect rect, float baseHorizontalPadding = 14f)
        {
            return Mathf.Max(1f, InnerWidth(rect, baseHorizontalPadding) - ScrollbarReserve);
        }

        public static bool ShouldStackControls(float availableWidth)
        {
            return ShouldStackControls(availableWidth, NarrowControlsReferenceWidth);
        }

        public static bool ShouldStackControls(float availableWidth, float referenceWidth)
        {
            return ShouldStackControls(availableWidth, referenceWidth, Scale);
        }

        public static bool ShouldStackControls(float availableWidth, float referenceWidth, float scale)
        {
            return availableWidth <= Scaled(referenceWidth, scale);
        }
    }
}
