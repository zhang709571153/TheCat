using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public enum P0BattleHudPromptLevel
    {
        Info,
        Warning,
        Critical,
        Result
    }

    public readonly struct P0BattleHudPrompt
    {
        public P0BattleHudPrompt(P0BattleHudPromptLevel level, string title, string actionText, string detailText = "")
        {
            Level = level;
            Title = title ?? string.Empty;
            ActionText = actionText ?? string.Empty;
            DetailText = detailText ?? string.Empty;
        }

        public P0BattleHudPromptLevel Level { get; }

        public string Title { get; }

        public string ActionText { get; }

        public string DetailText { get; }

        public string BuildSummary()
        {
            string summary = FormatLevel(Level) + "：" + Title;
            if (!string.IsNullOrWhiteSpace(ActionText))
            {
                summary += " - " + ActionText;
            }

            if (!string.IsNullOrWhiteSpace(DetailText))
            {
                summary += " (" + DetailText + ")";
            }

            return summary;
        }

        private static string FormatLevel(P0BattleHudPromptLevel level)
        {
            switch (level)
            {
                case P0BattleHudPromptLevel.Warning:
                    return "警告";
                case P0BattleHudPromptLevel.Critical:
                    return "危急";
                case P0BattleHudPromptLevel.Result:
                    return "结果";
                case P0BattleHudPromptLevel.Info:
                default:
                    return "提示";
            }
        }
    }

    public static class P0BattleHudPromptPresenter
    {
        public static P0BattleHudPrompt Build(BattleSimulation battle, IReadOnlyList<CatBattleState> cats)
        {
            if (battle == null)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Info,
                    "就绪",
                    "路线节点加载后即可开始战斗。");
            }

            if (battle.Outcome == BattleOutcome.Victory)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Result,
                    "胜利",
                    "继续路线。",
                    "用时 " + battle.NodeMetrics.DurationSeconds.ToString("0.0") + "s");
            }

            if (battle.Outcome == BattleOutcome.Defeat)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Result,
                    "失败",
                    "重开路线，或返回路线结算。",
                    "主人睡眠度崩溃");
            }

            P0BattleHudPrompt sleepPrompt = BuildSleepPrompt(battle.OwnerSleep);
            if (sleepPrompt.Level == P0BattleHudPromptLevel.Critical)
            {
                return sleepPrompt;
            }

            P0BattleHudPrompt poopPrompt = BuildPoopPrompt(battle.TeamPoop);
            if (poopPrompt.Level == P0BattleHudPromptLevel.Critical)
            {
                return poopPrompt;
            }

            P0BattleHudPrompt hungerPrompt = BuildHungerPrompt(battle.TeamHunger);
            if (hungerPrompt.Level == P0BattleHudPromptLevel.Critical)
            {
                return hungerPrompt;
            }

            P0BattleHudPrompt weakPrompt = BuildWeakCatPrompt(cats);
            if (weakPrompt.Level == P0BattleHudPromptLevel.Warning)
            {
                return weakPrompt;
            }

            P0BattleHudPrompt bossPrompt = BuildBossPrompt(battle.ActiveEnemies);
            if (bossPrompt.Level == P0BattleHudPromptLevel.Warning)
            {
                return bossPrompt;
            }

            P0BattleHudPrompt bedPrompt = BuildBedContactPrompt(battle.ActiveEnemies);
            if (bedPrompt.Level == P0BattleHudPromptLevel.Warning)
            {
                return bedPrompt;
            }

            if (sleepPrompt.Level == P0BattleHudPromptLevel.Warning)
            {
                return sleepPrompt;
            }

            if (poopPrompt.Level == P0BattleHudPromptLevel.Warning)
            {
                return poopPrompt;
            }

            if (hungerPrompt.Level == P0BattleHudPromptLevel.Warning)
            {
                return hungerPrompt;
            }

            if (battle.ActiveEnemies.Count > 0)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Info,
                    "守住床",
                    "释放技能并优先处理最近威胁。",
                    battle.ActiveEnemies.Count + " 个敌人");
            }

            return new P0BattleHudPrompt(
                P0BattleHudPromptLevel.Info,
                "准备",
                "靠近床并观察生成预警。");
        }

        private static P0BattleHudPrompt BuildSleepPrompt(OwnerSleepState sleep)
        {
            if (sleep == null)
            {
                return default(P0BattleHudPrompt);
            }

            switch (sleep.Stage)
            {
                case OwnerSleepStage.Critical:
                    return new P0BattleHudPrompt(
                        P0BattleHudPromptLevel.Critical,
                        "睡眠危急",
                        "立刻守住床。",
                        FormatSleep(sleep));
                case OwnerSleepStage.Danger:
                    return new P0BattleHudPrompt(
                        P0BattleHudPromptLevel.Critical,
                        "睡眠危险",
                        "使用床交互或铃音的安眠技能。",
                        FormatSleep(sleep));
                case OwnerSleepStage.Uneasy:
                    return new P0BattleHudPrompt(
                        P0BattleHudPromptLevel.Warning,
                        "睡眠不安",
                        "降低床边压力。",
                        FormatSleep(sleep));
                default:
                    return default(P0BattleHudPrompt);
            }
        }

        private static P0BattleHudPrompt BuildPoopPrompt(TeamPoopGauge poop)
        {
            if (poop == null)
            {
                return default(P0BattleHudPrompt);
            }

            if (poop.IsCountdownActive)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Critical,
                    "屎意倒计时",
                    "立刻使用猫砂盆。",
                    poop.CountdownRemainingSeconds.ToString("0") + "s");
            }

            if (poop.Stage == PoopStage.Critical)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Warning,
                    "屎意危急",
                    "尽快前往猫砂盆。",
                    poop.Current.ToString("0") + "/100");
            }

            if (poop.Stage == PoopStage.High)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Warning,
                    "屎意偏高",
                    "规划一次猫砂盆行程。",
                    poop.Current.ToString("0") + "/100");
            }

            return default(P0BattleHudPrompt);
        }

        private static P0BattleHudPrompt BuildHungerPrompt(TeamHungerGauge hunger)
        {
            if (hunger == null)
            {
                return default(P0BattleHudPrompt);
            }

            if (hunger.Stage == HungerStage.Empty)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Critical,
                    "饱肚见底",
                    "继续放技能前先使用喂食器。",
                    "伤害 " + hunger.DamageMultiplier.ToString("0.##") + " 倍");
            }

            if (hunger.Stage == HungerStage.Starving)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Warning,
                    "非常饥饿",
                    "尽快使用喂食器。",
                    "伤害 " + hunger.DamageMultiplier.ToString("0.##") + " 倍");
            }

            if (hunger.Stage == HungerStage.Hungry)
            {
                return new P0BattleHudPrompt(
                    P0BattleHudPromptLevel.Warning,
                    "饱肚偏低",
                    "技能伤害降低。",
                    "伤害 " + hunger.DamageMultiplier.ToString("0.##") + " 倍");
            }

            return default(P0BattleHudPrompt);
        }

        private static P0BattleHudPrompt BuildWeakCatPrompt(IReadOnlyList<CatBattleState> cats)
        {
            if (cats == null)
            {
                return default(P0BattleHudPrompt);
            }

            int weakCount = 0;
            string firstWeakName = string.Empty;
            for (int i = 0; i < cats.Count; i++)
            {
                CatBattleState cat = cats[i];
                if (cat == null || !cat.Vital.IsWeak)
                {
                    continue;
                }

                weakCount++;
                if (string.IsNullOrWhiteSpace(firstWeakName))
                {
                    firstWeakName = cat.Definition.DisplayName;
                }
            }

            if (weakCount <= 0)
            {
                return default(P0BattleHudPrompt);
            }

            return new P0BattleHudPrompt(
                P0BattleHudPromptLevel.Warning,
                "猫咪虚弱",
                "切换到健康猫咪并守住床。",
                firstWeakName + (weakCount > 1 ? " +" + (weakCount - 1) : string.Empty));
        }

        private static P0BattleHudPrompt BuildBossPrompt(IReadOnlyList<BattleEnemyState> enemies)
        {
            if (enemies == null)
            {
                return default(P0BattleHudPrompt);
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                BattleEnemyState enemy = enemies[i];
                if (enemy == null || enemy.Definition.BehaviorType != EnemyBehaviorType.BossCallTyrant)
                {
                    continue;
                }

                string warning = EnemyWarningFormatter.Format(enemy);
                if (warning.IndexOf("首领", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return new P0BattleHudPrompt(
                        P0BattleHudPromptLevel.Warning,
                        "首领行动",
                        "准备应对来电暴君压力。",
                        warning);
                }
            }

            return default(P0BattleHudPrompt);
        }

        private static P0BattleHudPrompt BuildBedContactPrompt(IReadOnlyList<BattleEnemyState> enemies)
        {
            if (enemies == null)
            {
                return default(P0BattleHudPrompt);
            }

            BattleEnemyState closest = null;
            for (int i = 0; i < enemies.Count; i++)
            {
                BattleEnemyState enemy = enemies[i];
                if (enemy == null || enemy.TimeToBedSeconds > EnemyWarningFormatter.BedWarningThresholdSeconds)
                {
                    continue;
                }

                if (closest == null || enemy.TimeToBedSeconds < closest.TimeToBedSeconds)
                {
                    closest = enemy;
                }
            }

            if (closest == null)
            {
                return default(P0BattleHudPrompt);
            }

            return new P0BattleHudPrompt(
                P0BattleHudPromptLevel.Warning,
                "即将压床",
                "击退或击败最近的敌人。",
                closest.Definition.DisplayName + " " + closest.TimeToBedSeconds.ToString("0.0") + "s");
        }

        private static string FormatSleep(OwnerSleepState sleep)
        {
            return sleep.Current.ToString("0") + "/" + sleep.Max.ToString("0");
        }
    }
}
