using System;
using TheCat.Data.Definitions;

namespace TheCat.Roguelite
{
    public readonly struct RouteNodePresentation
    {
        public RouteNodePresentation(
            string title,
            string riskHint,
            string rewardHint,
            string detail,
            bool requiresBattle)
            : this(title, riskHint, rewardHint, detail, requiresBattle, default(P0VisualAssetReference))
        {
        }

        public RouteNodePresentation(
            string title,
            string riskHint,
            string rewardHint,
            string detail,
            bool requiresBattle,
            P0VisualAssetReference visualAsset)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title is required.", nameof(title));
            }

            Title = title;
            RiskHint = riskHint ?? string.Empty;
            RewardHint = rewardHint ?? string.Empty;
            Detail = detail ?? string.Empty;
            RequiresBattle = requiresBattle;
            VisualAsset = visualAsset;
        }

        public string Title { get; }

        public string RiskHint { get; }

        public string RewardHint { get; }

        public string Detail { get; }

        public bool RequiresBattle { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public string BuildCompactLabel()
        {
            string label = Title;
            if (!string.IsNullOrWhiteSpace(RiskHint))
            {
                label += " [" + RiskHint + "]";
            }

            if (!string.IsNullOrWhiteSpace(RewardHint))
            {
                label += " -> " + RewardHint;
            }

            return label;
        }

        public string BuildDetailLabel()
        {
            string label = BuildCompactLabel();
            if (!string.IsNullOrWhiteSpace(Detail))
            {
                label += "\n" + Detail;
            }

            return label;
        }
    }
}
