using System;

namespace TheCat.Data.Definitions
{
    public sealed class CoreValuePresentation
    {
        public CoreValuePresentation(
            string label,
            string valueText,
            string stateLabel,
            string actionHint = "",
            string detailText = "")
            : this(
                label,
                valueText,
                stateLabel,
                actionHint,
                detailText,
                default(P0VisualAssetReference),
                default(P0VisualAssetReference),
                default(P0VisualAssetReference),
                0f)
        {
        }

        public CoreValuePresentation(
            string label,
            string valueText,
            string stateLabel,
            string actionHint,
            string detailText,
            P0VisualAssetReference visualAsset)
            : this(
                label,
                valueText,
                stateLabel,
                actionHint,
                detailText,
                visualAsset,
                default(P0VisualAssetReference),
                default(P0VisualAssetReference),
                0f)
        {
        }

        public CoreValuePresentation(
            string label,
            string valueText,
            string stateLabel,
            string actionHint,
            string detailText,
            P0VisualAssetReference visualAsset,
            P0VisualAssetReference gaugeFrameAsset,
            P0VisualAssetReference gaugeFillAsset,
            float fillRatio)
        {
            RequireText(label, nameof(label));
            RequireText(valueText, nameof(valueText));
            RequireText(stateLabel, nameof(stateLabel));

            Label = label;
            ValueText = valueText;
            StateLabel = stateLabel;
            ActionHint = actionHint ?? string.Empty;
            DetailText = detailText ?? string.Empty;
            VisualAsset = visualAsset;
            GaugeFrameAsset = gaugeFrameAsset;
            GaugeFillAsset = gaugeFillAsset;
            FillRatio = Clamp01(fillRatio);
        }

        public string Label { get; }

        public string ValueText { get; }

        public string StateLabel { get; }

        public string ActionHint { get; }

        public string DetailText { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public P0VisualAssetReference GaugeFrameAsset { get; }

        public P0VisualAssetReference GaugeFillAsset { get; }

        public float FillRatio { get; }

        public string BuildSummary()
        {
            string summary = Label + ": " + ValueText + " " + StateLabel;
            if (!string.IsNullOrWhiteSpace(DetailText))
            {
                summary += " " + DetailText;
            }

            if (!string.IsNullOrWhiteSpace(ActionHint))
            {
                summary += " - " + ActionHint;
            }

            return summary;
        }

        private static void RequireText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", name);
            }
        }

        private static float Clamp01(float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }
}
