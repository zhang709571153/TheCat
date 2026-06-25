using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0StatusTagCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StatusTagCoverageIssue
    {
        public P0StatusTagCoverageIssue(P0StatusTagCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StatusTagCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0StatusTagCoverageRow
    {
        public P0StatusTagCoverageRow(
            string statusTagId,
            string displayName,
            StatusTargetType targetType,
            string visualToken,
            IReadOnlyList<string> sourceSkillIds,
            string runtimeResponse)
        {
            StatusTagId = statusTagId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            TargetType = targetType;
            VisualToken = visualToken ?? string.Empty;
            SourceSkillIds = sourceSkillIds ?? Array.Empty<string>();
            RuntimeResponse = runtimeResponse ?? string.Empty;
        }

        public string StatusTagId { get; }

        public string DisplayName { get; }

        public StatusTargetType TargetType { get; }

        public string VisualToken { get; }

        public IReadOnlyList<string> SourceSkillIds { get; }

        public string RuntimeResponse { get; }

        public string BuildSummary()
        {
            return StatusTagId + " " + DisplayName
                + " 目标 " + FormatTargetType(TargetType)
                + " 视觉 " + VisualToken
                + " 来源 " + string.Join(", ", SourceSkillIds)
                + " 响应 " + RuntimeResponse;
        }

        private static string FormatTargetType(StatusTargetType targetType)
        {
            switch (targetType)
            {
                case StatusTargetType.BedZone:
                    return "床区";
                case StatusTargetType.Enemy:
                    return "敌人";
                case StatusTargetType.Cat:
                    return "猫";
                default:
                    return "未知";
            }
        }
    }

    public sealed class P0StatusTagCoverageReport
    {
        private readonly List<P0StatusTagCoverageIssue> issues = new List<P0StatusTagCoverageIssue>();
        private readonly List<P0StatusTagCoverageRow> rows = new List<P0StatusTagCoverageRow>();

        public IReadOnlyList<P0StatusTagCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<P0StatusTagCoverageRow> Rows => rows.AsReadOnly();

        public int FailureCount => Count(P0StatusTagCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0;

        public void AddIssue(P0StatusTagCoverageSeverity severity, string message)
        {
            issues.Add(new P0StatusTagCoverageIssue(severity, message));
        }

        public void AddRow(P0StatusTagCoverageRow row)
        {
            rows.Add(row);
        }

        public bool TryGetRow(string statusTagId, out P0StatusTagCoverageRow row)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].StatusTagId == statusTagId)
                {
                    row = rows[i];
                    return true;
                }
            }

            row = default(P0StatusTagCoverageRow);
            return false;
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 status tag coverage complete for " + rows.Count + " tag(s)."
                : "P0 status tag coverage has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };
            for (int i = 0; i < rows.Count; i++)
            {
                lines.Add("[Tag] " + rows[i].BuildSummary());
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0StatusTagCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0StatusTagCoverage
    {
        public static P0StatusTagCoverageReport EvaluatePrototypeCatalog()
        {
            return Evaluate(P0PrototypeCatalog.CreateStatusTags(), P0PrototypeCatalog.CreateStarterSkills());
        }

        public static P0StatusTagCoverageReport Evaluate(
            IReadOnlyList<StatusTagDefinition> statusTags,
            IReadOnlyList<SkillDefinition> skills)
        {
            P0StatusTagCoverageReport report = new P0StatusTagCoverageReport();
            Dictionary<string, StatusTagDefinition> statusesById = BuildStatusDictionary(statusTags);
            IReadOnlyList<P0StatusTagExpectation> expectations = CreateExpectations();

            for (int i = 0; i < expectations.Count; i++)
            {
                EvaluateExpectation(expectations[i], statusesById, skills, report);
            }

            return report;
        }

        private static void EvaluateExpectation(
            P0StatusTagExpectation expectation,
            Dictionary<string, StatusTagDefinition> statusesById,
            IReadOnlyList<SkillDefinition> skills,
            P0StatusTagCoverageReport report)
        {
            if (!statusesById.TryGetValue(expectation.StatusTagId, out StatusTagDefinition definition))
            {
                report.AddIssue(P0StatusTagCoverageSeverity.Failure, "Missing status tag definition: " + expectation.StatusTagId + ".");
                return;
            }

            if (definition.TargetType != expectation.TargetType)
            {
                report.AddIssue(
                    P0StatusTagCoverageSeverity.Failure,
                    expectation.StatusTagId + " target type should be " + expectation.TargetType + " but is " + definition.TargetType + ".");
            }

            if (string.IsNullOrWhiteSpace(definition.VisualToken))
            {
                report.AddIssue(P0StatusTagCoverageSeverity.Failure, expectation.StatusTagId + " is missing a visual token.");
            }

            List<string> foundSources = FindSourceSkills(expectation, skills);
            for (int i = 0; i < expectation.RequiredSourceSkillIds.Count; i++)
            {
                if (!foundSources.Contains(expectation.RequiredSourceSkillIds[i]))
                {
                    report.AddIssue(
                        P0StatusTagCoverageSeverity.Failure,
                        expectation.StatusTagId + " is missing source skill " + expectation.RequiredSourceSkillIds[i] + ".");
                }
            }

            if (string.IsNullOrWhiteSpace(expectation.RuntimeResponse))
            {
                report.AddIssue(P0StatusTagCoverageSeverity.Failure, expectation.StatusTagId + " is missing a runtime response note.");
            }

            report.AddRow(new P0StatusTagCoverageRow(
                definition.Id,
                definition.DisplayName,
                definition.TargetType,
                definition.VisualToken,
                foundSources.AsReadOnly(),
                expectation.RuntimeResponse));
        }

        private static List<string> FindSourceSkills(P0StatusTagExpectation expectation, IReadOnlyList<SkillDefinition> skills)
        {
            List<string> found = new List<string>();
            if (skills == null)
            {
                return found;
            }

            for (int i = 0; i < skills.Count; i++)
            {
                SkillDefinition skill = skills[i];
                if (skill == null || !Contains(expectation.RequiredSourceSkillIds, skill.Id))
                {
                    continue;
                }

                if (SkillContainsExpectedStatusEffect(skill, expectation))
                {
                    found.Add(skill.Id);
                }
            }

            return found;
        }

        private static bool SkillContainsExpectedStatusEffect(SkillDefinition skill, P0StatusTagExpectation expectation)
        {
            for (int i = 0; i < skill.Effects.Count; i++)
            {
                SkillEffectDefinition effect = skill.Effects[i];
                if (effect.StatusTagId != expectation.StatusTagId)
                {
                    continue;
                }

                for (int j = 0; j < expectation.SourceEffectTypes.Count; j++)
                {
                    if (effect.EffectType == expectation.SourceEffectTypes[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool Contains(IReadOnlyList<string> values, string value)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == value)
                {
                    return true;
                }
            }

            return false;
        }

        private static Dictionary<string, StatusTagDefinition> BuildStatusDictionary(IReadOnlyList<StatusTagDefinition> statusTags)
        {
            Dictionary<string, StatusTagDefinition> statusesById = new Dictionary<string, StatusTagDefinition>();
            if (statusTags == null)
            {
                return statusesById;
            }

            for (int i = 0; i < statusTags.Count; i++)
            {
                statusesById[statusTags[i].Id] = statusTags[i];
            }

            return statusesById;
        }

        private static IReadOnlyList<P0StatusTagExpectation> CreateExpectations()
        {
            return new[]
            {
                new P0StatusTagExpectation(
                    StatusTagIds.SleepStable,
                    StatusTargetType.BedZone,
                    new[] { "suzune_sleep_bell", "suzune_moon_torii" },
                    new[] { SkillEffectType.RestoreOwnerSleep },
                    "床获得安眠状态，主人睡眠恢复，并获得铃音屎意倒计时缓解。"),
                new P0StatusTagExpectation(
                    StatusTagIds.Slow,
                    StatusTargetType.Enemy,
                    new[] { "nephthys_moon_sand_obelisk", "nephthys_quicksand_trap" },
                    new[] { SkillEffectType.ApplyStatus },
                    "敌人移动倍率通过 BattleEnemyState.MovementRateMultiplier 降低。"),
                new P0StatusTagExpectation(
                    StatusTagIds.Knockback,
                    StatusTargetType.Enemy,
                    new[] { "saiban_sword_sweep", "saiban_sun_charge", "suzune_moon_torii" },
                    new[] { SkillEffectType.Knockback },
                    "可被击退的敌人会延后压床倒计时。"),
                new P0StatusTagExpectation(
                    StatusTagIds.Mark,
                    StatusTargetType.Enemy,
                    new[] { "nephthys_royal_mark" },
                    new[] { SkillEffectType.ApplyStatus },
                    "敌人承伤倍率提高，并启用奈芙蒂斯被动增伤。"),
                new P0StatusTagExpectation(
                    StatusTagIds.Shield,
                    StatusTargetType.Cat,
                    new[] { "saiban_oath_shield", "saiban_sun_charge" },
                    new[] { SkillEffectType.Shield },
                    "猫护盾吸收伤害，塞班被动会把护盾复制到床。")
            };
        }

        private readonly struct P0StatusTagExpectation
        {
            public P0StatusTagExpectation(
                string statusTagId,
                StatusTargetType targetType,
                IReadOnlyList<string> requiredSourceSkillIds,
                IReadOnlyList<SkillEffectType> sourceEffectTypes,
                string runtimeResponse)
            {
                StatusTagId = statusTagId;
                TargetType = targetType;
                RequiredSourceSkillIds = requiredSourceSkillIds;
                SourceEffectTypes = sourceEffectTypes;
                RuntimeResponse = runtimeResponse;
            }

            public string StatusTagId { get; }

            public StatusTargetType TargetType { get; }

            public IReadOnlyList<string> RequiredSourceSkillIds { get; }

            public IReadOnlyList<SkillEffectType> SourceEffectTypes { get; }

            public string RuntimeResponse { get; }
        }
    }
}
