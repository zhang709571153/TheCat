using UnityEngine;

namespace TheCat.Gameplay
{
    public readonly struct P0StatusIndicatorState
    {
        public P0StatusIndicatorState(string text, Color accentColor, int count)
        {
            Text = text ?? string.Empty;
            AccentColor = accentColor;
            Count = count;
        }

        public string Text { get; }

        public Color AccentColor { get; }

        public int Count { get; }

        public bool HasStatuses => Count > 0 && !string.IsNullOrWhiteSpace(Text);
    }
}
