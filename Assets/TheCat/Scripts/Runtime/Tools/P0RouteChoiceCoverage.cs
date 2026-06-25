using System;
using System.Collections.Generic;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0RouteChoiceCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0RouteChoiceCoverageIssue
    {
        public P0RouteChoiceCoverageIssue(P0RouteChoiceCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0RouteChoiceCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0RouteChoiceCoverageRow
    {
        public P0RouteChoiceCoverageRow(
            string nodeId,
            RouteNodeType nodeType,
            int choiceCount,
            string defaultChoiceId,
            string defaultChoiceSummary)
        {
            NodeId = nodeId ?? string.Empty;
            NodeType = nodeType;
            ChoiceCount = choiceCount;
            DefaultChoiceId = defaultChoiceId ?? string.Empty;
            DefaultChoiceSummary = defaultChoiceSummary ?? string.Empty;
        }

        public string NodeId { get; }

        public RouteNodeType NodeType { get; }

        public int ChoiceCount { get; }

        public string DefaultChoiceId { get; }

        public string DefaultChoiceSummary { get; }

        public string BuildSummary()
        {
            return NodeId
                + " " + NodeType
                + " choices " + ChoiceCount
                + " default " + DefaultChoiceSummary;
        }
    }

    public sealed class P0RouteChoiceCoverageReport
    {
        private readonly List<P0RouteChoiceCoverageIssue> issues = new List<P0RouteChoiceCoverageIssue>();
        private readonly List<P0RouteChoiceCoverageRow> rows = new List<P0RouteChoiceCoverageRow>();

        public IReadOnlyList<P0RouteChoiceCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<P0RouteChoiceCoverageRow> Rows => rows.AsReadOnly();

        public int FailureCount => Count(P0RouteChoiceCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0;

        public void AddIssue(P0RouteChoiceCoverageSeverity severity, string message)
        {
            issues.Add(new P0RouteChoiceCoverageIssue(severity, message));
        }

        public void AddRow(P0RouteChoiceCoverageRow row)
        {
            rows.Add(row);
        }

        public bool TryGetRow(string nodeId, out P0RouteChoiceCoverageRow row)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].NodeId == nodeId)
                {
                    row = rows[i];
                    return true;
                }
            }

            row = default(P0RouteChoiceCoverageRow);
            return false;
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 route choice coverage complete for " + rows.Count + " non-battle node(s)."
                : "P0 route choice coverage has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < rows.Count; i++)
            {
                lines.Add("[Node] " + rows[i].BuildSummary());
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0RouteChoiceCoverageSeverity severity)
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

    public static class P0RouteChoiceCoverage
    {
        public static P0RouteChoiceCoverageReport EvaluatePrototypeRoute()
        {
            return Evaluate(P0RouteCatalog.CreateTenLayerRoute());
        }

        public static P0RouteChoiceCoverageReport Evaluate(RouteDefinition route)
        {
            P0RouteChoiceCoverageReport report = new P0RouteChoiceCoverageReport();
            if (route == null)
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, "Route definition is missing.");
                return report;
            }

            for (int i = 0; i < route.Nodes.Count; i++)
            {
                RouteNodeDefinition node = route.Nodes[i];
                if (RouteNodeResolver.RequiresBattle(node.NodeType))
                {
                    continue;
                }

                EvaluateNode(route, node, report);
            }

            if (report.Rows.Count == 0)
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, "No non-battle route choice nodes were found.");
            }

            ValidateDreamEventContentIdDifferentiation(route, report);

            return report;
        }

        private static void EvaluateNode(
            RouteDefinition route,
            RouteNodeDefinition node,
            P0RouteChoiceCoverageReport report)
        {
            RunProgressionState previewRun = CreatePreparedRun(route);
            IReadOnlyList<RouteRewardChoice> choices = P0RouteRewardResolver.CreatePlaceholderChoices(node, previewRun);
            if (choices == null || choices.Count == 0)
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " has no route choices.");
                return;
            }

            for (int i = 0; i < choices.Count; i++)
            {
                ValidateChoice(node, choices[i], report);
            }

            RouteRewardChoice defaultChoice = P0RouteRewardResolver.GetDefaultPlaceholderChoice(node, previewRun);
            if (defaultChoice == null)
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " has no default route choice.");
                return;
            }

            RunProgressionState applyRun = CreatePreparedRun(route);
            if (!P0RouteRewardResolver.ApplyPlaceholderChoice(node, applyRun, defaultChoice))
            {
                report.AddIssue(
                    P0RouteChoiceCoverageSeverity.Failure,
                    node.Id + " default choice cannot be applied: " + defaultChoice.DisplayName + ".");
            }
            else
            {
                ValidateAppliedDefaultChoice(node, applyRun, defaultChoice, report);
            }

            report.AddRow(new P0RouteChoiceCoverageRow(
                node.Id,
                node.NodeType,
                choices.Count,
                defaultChoice.Id,
                defaultChoice.BuildSummary()));
        }

        private static void ValidateChoice(
            RouteNodeDefinition node,
            RouteRewardChoice choice,
            P0RouteChoiceCoverageReport report)
        {
            if (choice == null)
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " contains a null route choice.");
                return;
            }

            if (string.IsNullOrWhiteSpace(choice.DisplayName))
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " choice " + choice.Id + " has no display name.");
            }

            if (string.IsNullOrWhiteSpace(choice.Description))
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " choice " + choice.Id + " has no description.");
            }

            string summary = choice.BuildSummary();
            if (string.IsNullOrWhiteSpace(summary))
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " choice " + choice.Id + " has no summary.");
            }

            if (ContainsRawToken(choice.DisplayName) || ContainsRawToken(summary))
            {
                report.AddIssue(P0RouteChoiceCoverageSeverity.Failure, node.Id + " choice " + choice.Id + " exposes a raw id-like token.");
            }
        }

        private static void ValidateAppliedDefaultChoice(
            RouteNodeDefinition node,
            RunProgressionState run,
            RouteRewardChoice defaultChoice,
            P0RouteChoiceCoverageReport report)
        {
            if (node.NodeType != RouteNodeType.Partner
                || defaultChoice.ChoiceType != RouteRewardChoiceType.RecruitPartner)
            {
                return;
            }

            if (!run.Roster.HasCat(defaultChoice.PartnerCatId))
            {
                report.AddIssue(
                    P0RouteChoiceCoverageSeverity.Failure,
                    node.Id + " partner recruit default choice did not add the preview partner.");
            }

            if (!run.PendingBattleModifiers.HasPending)
            {
                report.AddIssue(
                    P0RouteChoiceCoverageSeverity.Failure,
                    node.Id + " partner recruit default choice did not queue next-battle support.");
                return;
            }

            RunPendingBattleModifierSnapshot snapshot = run.PendingBattleModifiers.Consume();
            if (!Approximately(snapshot.SkillDamageMultiplier, 1f)
                || !Approximately(snapshot.PoopGrowthMultiplier, 1f)
                || !Approximately(snapshot.ShieldMultiplier, 1.1f)
                || !Approximately(snapshot.EnemyStatusDurationMultiplier, 1.1f)
                || !Approximately(snapshot.OwnerSleepRestoreMultiplier, 1.1f)
                || !Approximately(snapshot.CatHealMultiplier, 1.1f))
            {
                report.AddIssue(
                    P0RouteChoiceCoverageSeverity.Failure,
                    node.Id + " partner recruit next-battle support modifiers are not the expected P0 preview values.");
            }
        }

        private static void ValidateDreamEventContentIdDifferentiation(
            RouteDefinition route,
            P0RouteChoiceCoverageReport report)
        {
            P0RouteChoiceCoverageRow softRainRow = default(P0RouteChoiceCoverageRow);
            P0RouteChoiceCoverageRow redDotRow = default(P0RouteChoiceCoverageRow);
            bool hasSoftRain = false;
            bool hasRedDot = false;

            for (int i = 0; i < route.Nodes.Count; i++)
            {
                RouteNodeDefinition node = route.Nodes[i];
                if (node.NodeType != RouteNodeType.DreamEvent)
                {
                    continue;
                }

                if (node.ContentId == P0RouteCatalog.SoftRainWindowEventContentId
                    && report.TryGetRow(node.Id, out softRainRow))
                {
                    hasSoftRain = true;
                }

                if (node.ContentId == P0RouteCatalog.UnreadRedDotRainEventContentId
                    && report.TryGetRow(node.Id, out redDotRow))
                {
                    hasRedDot = true;
                }
            }

            if (!hasSoftRain || !hasRedDot)
            {
                return;
            }

            if (softRainRow.DefaultChoiceId == redDotRow.DefaultChoiceId)
            {
                report.AddIssue(
                    P0RouteChoiceCoverageSeverity.Failure,
                    "Dream-event route rewards are not differentiated by ContentId.");
            }
        }

        private static RunProgressionState CreatePreparedRun(RouteDefinition route)
        {
            RunProgressionState run = new RunProgressionState(route, P0RunSession.CreateDefaultStarterCatIds());
            run.Wallet.AddDreamShards(20);
            run.Wallet.AddFishTreats(20);
            return run;
        }

        private static bool ContainsRawToken(string text)
        {
            return !string.IsNullOrWhiteSpace(text) && text.IndexOf("_", StringComparison.Ordinal) >= 0;
        }

        private static bool Approximately(float actual, float expected)
        {
            return Math.Abs(actual - expected) <= 0.001f;
        }
    }
}
