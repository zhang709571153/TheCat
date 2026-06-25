using System;

namespace TheCat.Data.Definitions
{
    public sealed class CatPresentation
    {
        public CatPresentation(
            string catId,
            string displayName,
            string title,
            string roleHint,
            string authorityLabel,
            string attributeLabel,
            string shortLabel = "",
            string signatureLine = "",
            string visualToken = "",
            string visualIdentity = "")
        {
            RequireText(catId, nameof(catId));
            RequireText(displayName, nameof(displayName));
            RequireText(title, nameof(title));
            RequireText(roleHint, nameof(roleHint));
            RequireText(authorityLabel, nameof(authorityLabel));
            RequireText(attributeLabel, nameof(attributeLabel));

            CatId = catId;
            DisplayName = displayName;
            Title = title;
            RoleHint = roleHint;
            AuthorityLabel = authorityLabel;
            AttributeLabel = attributeLabel;
            ShortLabel = string.IsNullOrWhiteSpace(shortLabel) ? displayName : shortLabel;
            SignatureLine = signatureLine ?? string.Empty;
            VisualToken = visualToken ?? string.Empty;
            VisualIdentity = visualIdentity ?? string.Empty;
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string Title { get; }

        public string RoleHint { get; }

        public string AuthorityLabel { get; }

        public string AttributeLabel { get; }

        public string ShortLabel { get; }

        public string SignatureLine { get; }

        public string VisualToken { get; }

        public string VisualIdentity { get; }

        public string BuildRosterLabel()
        {
            return DisplayName + " / " + Title;
        }

        public string BuildSelectionLabel()
        {
            return DisplayName
                + " - "
                + Title
                + " - "
                + RoleHint
                + " / "
                + AuthorityLabel
                + " / "
                + AttributeLabel;
        }

        public string BuildDesignLabel()
        {
            string line = string.IsNullOrWhiteSpace(SignatureLine) ? "暂无台词" : SignatureLine;
            string token = string.IsNullOrWhiteSpace(VisualToken) ? "暂无视觉符号" : VisualToken;
            string identity = string.IsNullOrWhiteSpace(VisualIdentity) ? "暂无视觉识别" : VisualIdentity;
            return token + " - " + identity + " - " + line;
        }

        public string BuildVitalLabel(float currentHp, float maxHp, bool isWeak, float weakRemainingSeconds)
        {
            string label = ShortLabel + " 生命 " + currentHp.ToString("0") + "/" + maxHp.ToString("0");
            if (isWeak)
            {
                label += " 虚弱 " + weakRemainingSeconds.ToString("0") + "s";
            }

            return label;
        }

        private static void RequireText(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is required.", name);
            }
        }
    }
}
