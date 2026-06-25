namespace TheCat.Gameplay
{
    public enum P0HudMessageChannel
    {
        Player,
        Diagnostics
    }

    public static class P0HudMessagePresenter
    {
        public static string BuildVisibleMessage(
            string message,
            P0HudMessageChannel channel,
            bool showDiagnosticsHud)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return string.Empty;
            }

            return !showDiagnosticsHud && channel == P0HudMessageChannel.Diagnostics
                ? string.Empty
                : message;
        }
    }
}
