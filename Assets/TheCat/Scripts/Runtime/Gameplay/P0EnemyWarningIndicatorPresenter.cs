using System;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0EnemyWarningIndicatorPresenter
    {
        public static P0EnemyWarningIndicatorState Build(
            BattleEnemyState enemy,
            Vector2 enemyPosition,
            Vector2 bedPosition)
        {
            if (enemy == null || !enemy.IsAlive)
            {
                return default(P0EnemyWarningIndicatorState);
            }

            P0EnemyWarningIndicatorState best = default(P0EnemyWarningIndicatorState);
            AddCandidate(ref best, BuildBedWarning(enemy, enemyPosition, bedPosition));
            AddCandidate(ref best, BuildBehaviorWarning(enemy, enemyPosition, bedPosition));
            AddCandidate(ref best, BuildBossSummonWarning(enemy, enemyPosition));
            AddCandidate(ref best, BuildBossThrowWarning(enemy, enemyPosition, bedPosition));
            return best;
        }

        public static Color GetColor(P0EnemyWarningKind kind)
        {
            switch (kind)
            {
                case P0EnemyWarningKind.BedContact:
                    return new Color(1f, 0.35f, 0.25f, 0.9f);
                case P0EnemyWarningKind.ChargeLane:
                    return new Color(1f, 0.8f, 0.15f, 0.9f);
                case P0EnemyWarningKind.RangedPressure:
                    return new Color(0.35f, 0.85f, 1f, 0.85f);
                case P0EnemyWarningKind.FlyerAttach:
                    return new Color(1f, 0.25f, 0.65f, 0.85f);
                case P0EnemyWarningKind.JumpSlam:
                    return new Color(0.95f, 0.45f, 1f, 0.85f);
                case P0EnemyWarningKind.BossSummon:
                    return new Color(0.95f, 0.2f, 0.15f, 0.95f);
                case P0EnemyWarningKind.BossThrow:
                    return new Color(1f, 0.15f, 0.05f, 0.95f);
                default:
                    return new Color(1f, 1f, 1f, 0.8f);
            }
        }

        private static P0EnemyWarningIndicatorState BuildBedWarning(
            BattleEnemyState enemy,
            Vector2 enemyPosition,
            Vector2 bedPosition)
        {
            if (enemy.TimeToBedSeconds > EnemyWarningFormatter.BedWarningThresholdSeconds)
            {
                return default(P0EnemyWarningIndicatorState);
            }

            return new P0EnemyWarningIndicatorState(
                P0EnemyWarningKind.BedContact,
                "压床",
                enemy.TimeToBedSeconds,
                enemyPosition,
                bedPosition,
                0.95f,
                GetBedWarningVisualAsset(enemy));
        }

        private static P0EnemyWarningIndicatorState BuildBehaviorWarning(
            BattleEnemyState enemy,
            Vector2 enemyPosition,
            Vector2 bedPosition)
        {
            switch (enemy.Definition.BehaviorType)
            {
                case EnemyBehaviorType.Charger:
                    return BuildTimedWarning(
                        enemy.TimeToBedSeconds,
                        EnemyWarningFormatter.ChargeWarningThresholdSeconds,
                        P0EnemyWarningKind.ChargeLane,
                        "冲锋路线",
                        enemyPosition,
                        bedPosition,
                        0.5f);
                case EnemyBehaviorType.RangedHarasser:
                    return BuildTimedWarning(
                        enemy.TimeToBedSeconds,
                        EnemyWarningFormatter.RangedPressureWarningThresholdSeconds,
                        P0EnemyWarningKind.RangedPressure,
                        "远程压制",
                        enemyPosition,
                        bedPosition,
                        0.65f,
                        P0VisualAssetCatalog.GetColdLightBeamWarningVfx());
                case EnemyBehaviorType.FlyingAttachment:
                    return BuildTimedWarning(
                        enemy.TimeToBedSeconds,
                        EnemyWarningFormatter.FlyingAttachWarningThresholdSeconds,
                        P0EnemyWarningKind.FlyerAttach,
                        "飞虫附着",
                        enemyPosition,
                        enemyPosition,
                        0.85f);
                case EnemyBehaviorType.EliteJumpSlam:
                    return BuildTimedWarning(
                        enemy.TimeToBedSeconds,
                        EnemyWarningFormatter.JumpSlamWarningThresholdSeconds,
                        P0EnemyWarningKind.JumpSlam,
                        "跳砸",
                        enemyPosition,
                        enemyPosition,
                        1.35f);
                default:
                    return default(P0EnemyWarningIndicatorState);
            }
        }

        private static P0EnemyWarningIndicatorState BuildBossSummonWarning(
            BattleEnemyState enemy,
            Vector2 enemyPosition)
        {
            if (enemy.Definition.BehaviorType != EnemyBehaviorType.BossCallTyrant)
            {
                return default(P0EnemyWarningIndicatorState);
            }

            return BuildTimedWarning(
                enemy.BossSummonRemainingSeconds,
                EnemyWarningFormatter.BossSummonWarningThresholdSeconds,
                P0EnemyWarningKind.BossSummon,
                "首领召唤",
                enemyPosition,
                enemyPosition,
                1.55f,
                P0VisualAssetCatalog.GetCallTyrantSummonPortalVfx());
        }

        private static P0EnemyWarningIndicatorState BuildBossThrowWarning(
            BattleEnemyState enemy,
            Vector2 enemyPosition,
            Vector2 bedPosition)
        {
            if (enemy.Definition.BehaviorType != EnemyBehaviorType.BossCallTyrant)
            {
                return default(P0EnemyWarningIndicatorState);
            }

            return BuildTimedWarning(
                enemy.BossThrowRemainingSeconds,
                EnemyWarningFormatter.BossThrowWarningThresholdSeconds,
                P0EnemyWarningKind.BossThrow,
                "首领投掷",
                enemyPosition,
                bedPosition,
                0.7f,
                P0VisualAssetCatalog.GetCallTyrantAppThrowVfx());
        }

        private static P0VisualAssetReference GetBedWarningVisualAsset(BattleEnemyState enemy)
        {
            if (enemy.Definition.Id == P0PrototypeCatalog.BlackMudNightmareId)
            {
                return P0VisualAssetCatalog.GetBlackMudBedClawWarningVfx();
            }

            return default(P0VisualAssetReference);
        }

        private static P0EnemyWarningIndicatorState BuildTimedWarning(
            float remainingSeconds,
            float thresholdSeconds,
            P0EnemyWarningKind kind,
            string label,
            Vector2 origin,
            Vector2 target,
            float radius,
            P0VisualAssetReference visualAsset = default(P0VisualAssetReference))
        {
            if (remainingSeconds > thresholdSeconds)
            {
                return default(P0EnemyWarningIndicatorState);
            }

            return new P0EnemyWarningIndicatorState(
                kind,
                label,
                Math.Max(0f, remainingSeconds),
                origin,
                target,
                radius,
                visualAsset);
        }

        private static void AddCandidate(
            ref P0EnemyWarningIndicatorState best,
            P0EnemyWarningIndicatorState candidate)
        {
            if (!candidate.HasWarning)
            {
                return;
            }

            if (!best.HasWarning || candidate.RemainingSeconds < best.RemainingSeconds)
            {
                best = candidate;
            }
        }
    }
}
