using System.Text;
using TheCat.Data.Definitions;

namespace TheCat.Combat
{
    public static class EnemyWarningFormatter
    {
        public const float BedWarningThresholdSeconds = 2.5f;
        public const float ChargeWarningThresholdSeconds = 4f;
        public const float RangedPressureWarningThresholdSeconds = 5f;
        public const float FlyingAttachWarningThresholdSeconds = 3f;
        public const float JumpSlamWarningThresholdSeconds = 6f;
        public const float BossSummonWarningThresholdSeconds = 2f;
        public const float BossThrowWarningThresholdSeconds = 2f;

        public static string Format(BattleEnemyState enemy)
        {
            if (enemy == null)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            AppendBedWarning(enemy, builder);
            AppendBehaviorWarning(enemy, builder);
            AppendBossWarnings(enemy, builder);
            return builder.ToString();
        }

        private static void AppendBedWarning(BattleEnemyState enemy, StringBuilder builder)
        {
            if (enemy.TimeToBedSeconds > BedWarningThresholdSeconds)
            {
                return;
            }

            AppendWarning(builder, "压床", enemy.TimeToBedSeconds);
        }

        private static void AppendBehaviorWarning(BattleEnemyState enemy, StringBuilder builder)
        {
            switch (enemy.Definition.BehaviorType)
            {
                case EnemyBehaviorType.Charger:
                    if (enemy.TimeToBedSeconds <= ChargeWarningThresholdSeconds)
                    {
                        AppendWarning(builder, "冲锋路线", enemy.TimeToBedSeconds);
                    }

                    break;
                case EnemyBehaviorType.RangedHarasser:
                    if (enemy.TimeToBedSeconds <= RangedPressureWarningThresholdSeconds)
                    {
                        AppendWarning(builder, "远程压制", enemy.TimeToBedSeconds);
                    }

                    break;
                case EnemyBehaviorType.FlyingAttachment:
                    if (enemy.TimeToBedSeconds <= FlyingAttachWarningThresholdSeconds)
                    {
                        AppendWarning(builder, "飞虫附着", enemy.TimeToBedSeconds);
                    }

                    break;
                case EnemyBehaviorType.EliteJumpSlam:
                    if (enemy.TimeToBedSeconds <= JumpSlamWarningThresholdSeconds)
                    {
                        AppendWarning(builder, "跳砸", enemy.TimeToBedSeconds);
                    }

                    break;
            }
        }

        private static void AppendBossWarnings(BattleEnemyState enemy, StringBuilder builder)
        {
            if (enemy.Definition.BehaviorType != EnemyBehaviorType.BossCallTyrant)
            {
                return;
            }

            if (enemy.BossSummonRemainingSeconds <= BossSummonWarningThresholdSeconds)
            {
                AppendWarning(builder, "首领召唤", enemy.BossSummonRemainingSeconds);
            }

            if (enemy.BossThrowRemainingSeconds <= BossThrowWarningThresholdSeconds)
            {
                AppendWarning(builder, "首领投掷", enemy.BossThrowRemainingSeconds);
            }
        }

        private static void AppendWarning(StringBuilder builder, string label, float remainingSeconds)
        {
            AppendSeparator(builder);
            builder.Append(label);
            builder.Append(" ");
            builder.Append(remainingSeconds.ToString("0.0"));
            builder.Append("s");
        }

        private static void AppendSeparator(StringBuilder builder)
        {
            if (builder.Length > 0)
            {
                builder.Append(" | ");
            }
        }
    }
}
