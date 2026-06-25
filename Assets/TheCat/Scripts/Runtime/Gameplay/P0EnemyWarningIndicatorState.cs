using UnityEngine;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public readonly struct P0EnemyWarningIndicatorState
    {
        public P0EnemyWarningIndicatorState(
            P0EnemyWarningKind kind,
            string label,
            float remainingSeconds,
            Vector2 origin,
            Vector2 target,
            float radius)
            : this(kind, label, remainingSeconds, origin, target, radius, default(P0VisualAssetReference))
        {
        }

        public P0EnemyWarningIndicatorState(
            P0EnemyWarningKind kind,
            string label,
            float remainingSeconds,
            Vector2 origin,
            Vector2 target,
            float radius,
            P0VisualAssetReference visualAsset)
        {
            Kind = kind;
            Label = label ?? string.Empty;
            RemainingSeconds = remainingSeconds;
            Origin = origin;
            Target = target;
            Radius = radius;
            VisualAsset = visualAsset;
        }

        public P0EnemyWarningKind Kind { get; }

        public string Label { get; }

        public float RemainingSeconds { get; }

        public Vector2 Origin { get; }

        public Vector2 Target { get; }

        public float Radius { get; }

        public P0VisualAssetReference VisualAsset { get; }

        public bool HasWarning => Kind != P0EnemyWarningKind.None;

        public bool UsesLine => Kind == P0EnemyWarningKind.ChargeLane
            || Kind == P0EnemyWarningKind.RangedPressure
            || Kind == P0EnemyWarningKind.BossThrow;

        public bool UsesRing => Kind == P0EnemyWarningKind.BedContact
            || Kind == P0EnemyWarningKind.FlyerAttach
            || Kind == P0EnemyWarningKind.JumpSlam
            || Kind == P0EnemyWarningKind.BossSummon;

        public string BuildSummary()
        {
            if (!HasWarning)
            {
                return "敌人预警：无";
            }

            return "敌人预警："
                + Label
                + " "
                + RemainingSeconds.ToString("0.0")
                + "s"
                + (VisualAsset.HasAsset ? " 资产 " + VisualAsset.AssetId : string.Empty);
        }
    }
}
