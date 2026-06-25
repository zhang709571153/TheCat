using System;

namespace TheCat.Roguelite
{
    public sealed class DreamMapDefinition
    {
        public DreamMapDefinition(
            string id,
            string displayName,
            string themeLabel,
            string defenseTargetLabel,
            string entranceLabel,
            string battleArenaLabel,
            string routePurposeLabel,
            bool isPlayableInP0)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Dream map id is required.", nameof(id));
            }

            Id = id;
            DisplayName = displayName ?? string.Empty;
            ThemeLabel = themeLabel ?? string.Empty;
            DefenseTargetLabel = defenseTargetLabel ?? string.Empty;
            EntranceLabel = entranceLabel ?? string.Empty;
            BattleArenaLabel = battleArenaLabel ?? string.Empty;
            RoutePurposeLabel = routePurposeLabel ?? string.Empty;
            IsPlayableInP0 = isPlayableInP0;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public string ThemeLabel { get; }

        public string DefenseTargetLabel { get; }

        public string EntranceLabel { get; }

        public string BattleArenaLabel { get; }

        public string RoutePurposeLabel { get; }

        public bool IsPlayableInP0 { get; }

        public bool IsPlaceholder => !IsPlayableInP0;

        public string BuildSummary()
        {
            return DisplayName
                + " / " + ThemeLabel
                + " / 守护 " + DefenseTargetLabel
                + (IsPlayableInP0 ? " / P0可玩" : " / P0占位");
        }
    }
}
