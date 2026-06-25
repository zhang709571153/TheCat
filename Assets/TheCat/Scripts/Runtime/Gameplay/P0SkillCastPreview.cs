using System;
using TheCat.Data.Definitions;

namespace TheCat.Gameplay
{
    public readonly struct P0SkillCastPreview
    {
        public P0SkillCastPreview(
            SkillDefinition skill,
            float cooldownSeconds,
            float currentHunger,
            P0SkillTargetResult target)
        {
            Skill = skill;
            CooldownSeconds = cooldownSeconds;
            CurrentHunger = currentHunger;
            Target = target;
        }

        public SkillDefinition Skill { get; }

        public float CooldownSeconds { get; }

        public float CurrentHunger { get; }

        public P0SkillTargetResult Target { get; }

        public bool IsReady => Skill != null && CooldownSeconds <= 0f && Target.CanCast;

        public string BuildSummary()
        {
            if (Skill == null)
            {
                return "技能预览：缺少技能";
            }

            if (CooldownSeconds > 0f)
            {
                return "技能预览：冷却 " + CooldownSeconds.ToString("0.0") + "s";
            }

            string summary = "技能预览：";
            if (Target.RequiresEnemyTarget)
            {
                if (Target.HasEnemyTarget)
                {
                    summary += "目标 "
                        + Target.Enemy.Definition.DisplayName
                        + " "
                        + Target.Distance.ToString("0.0")
                        + "/"
                        + Target.Range.ToString("0.0")
                        + "m";
                }
                else
                {
                    summary += "无目标 <= " + Target.Range.ToString("0.0") + "m";
                }
            }
            else
            {
                summary += "不需要敌人目标";
            }

            summary += " | 饱肚 " + CurrentHunger.ToString("0") + "->" + Math.Max(0f, CurrentHunger - Skill.HungerCost).ToString("0");
            if (CurrentHunger < Skill.HungerCost)
            {
                summary += " 偏低";
            }

            return summary;
        }
    }
}
