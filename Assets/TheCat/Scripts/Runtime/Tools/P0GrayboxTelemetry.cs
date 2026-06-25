using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0GrayboxTelemetrySeverity
    {
        Info,
        Warning,
        Failure
    }

    public readonly struct P0GrayboxTelemetryIssue
    {
        public P0GrayboxTelemetryIssue(P0GrayboxTelemetrySeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0GrayboxTelemetrySeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0GrayboxTelemetryNodeRow
    {
        public P0GrayboxTelemetryNodeRow(NodeMetrics metrics)
        {
            if (metrics == null)
            {
                throw new ArgumentNullException(nameof(metrics));
            }

            Layer = metrics.Layer;
            NodeId = metrics.NodeId;
            Result = metrics.Result;
            IsComplete = metrics.IsComplete;
            DurationSeconds = metrics.DurationSeconds;
            SleepDelta = metrics.SleepDelta;
            LitterBoxUses = metrics.LitterBoxUses;
            FeederUses = metrics.FeederUses;
            BedCareUses = metrics.BedCareUses;
            PoopIncidents = metrics.PoopIncidents;
            SleepMaxLost = metrics.SleepMaxLost;
            WeakIncidents = metrics.WeakIncidents;
            CatPressureEvents = metrics.CatPressureEvents;
            CatDamageIncoming = metrics.CatDamageIncoming;
            CatDamageTaken = metrics.CatDamageTaken;
            CatDamageAbsorbed = metrics.CatDamageAbsorbed;
            CatHealEvents = metrics.CatHealEvents;
            CatHealingApplied = metrics.CatHealingApplied;
            CatShieldEvents = metrics.CatShieldEvents;
            CatShieldApplied = metrics.CatShieldApplied;
            BedPressureHits = metrics.BedPressureHits;
            BossThrowPressureHits = metrics.BossThrowPressureHits;
            EnemySleepPressureEvents = metrics.EnemySleepPressureEvents;
            EnemySleepDamageIncoming = metrics.EnemySleepDamageIncoming;
            EnemySleepDamageTaken = metrics.EnemySleepDamageTaken;
            EnemySleepDamageAbsorbed = metrics.EnemySleepDamageAbsorbed;
            CatSwitchAttempts = metrics.CatSwitchAttempts;
            CatSwitchesSucceeded = metrics.CatSwitchesSucceeded;
            CatSwitchesBlockedByWeak = metrics.CatSwitchesBlockedByWeak;
            AutoTargetAttempts = metrics.AutoTargetAttempts;
            AutoTargetsAcquired = metrics.AutoTargetsAcquired;
            SkillTargetAttempts = metrics.SkillTargetAttempts;
            SkillTargetsAcquired = metrics.SkillTargetsAcquired;
            SkillCastAttempts = metrics.SkillCastAttempts;
            SkillCastsSucceeded = metrics.SkillCastsSucceeded;
            SkillCastsBlockedByCooldown = metrics.SkillCastsBlockedByCooldown;
            SkillCastsBlockedByTarget = metrics.SkillCastsBlockedByTarget;
            SkillCastsBlockedByMissingDefinition = metrics.SkillCastsBlockedByMissingDefinition;
            InteractionAttempts = metrics.InteractionAttempts;
            InteractionSuccesses = metrics.InteractionSuccesses;
            InteractionBlockedByRange = metrics.InteractionBlockedByRange;
        }

        public int Layer { get; }

        public string NodeId { get; }

        public NodeResult Result { get; }

        public bool IsComplete { get; }

        public float DurationSeconds { get; }

        public float SleepDelta { get; }

        public int LitterBoxUses { get; }

        public int FeederUses { get; }

        public int BedCareUses { get; }

        public int PoopIncidents { get; }

        public float SleepMaxLost { get; }

        public int WeakIncidents { get; }

        public int CatPressureEvents { get; }

        public float CatDamageIncoming { get; }

        public float CatDamageTaken { get; }

        public float CatDamageAbsorbed { get; }

        public int CatHealEvents { get; }

        public float CatHealingApplied { get; }

        public int CatShieldEvents { get; }

        public float CatShieldApplied { get; }

        public int BedPressureHits { get; }

        public int BossThrowPressureHits { get; }

        public int EnemySleepPressureEvents { get; }

        public float EnemySleepDamageIncoming { get; }

        public float EnemySleepDamageTaken { get; }

        public float EnemySleepDamageAbsorbed { get; }

        public int CatSwitchAttempts { get; }

        public int CatSwitchesSucceeded { get; }

        public int CatSwitchesBlockedByWeak { get; }

        public int AutoTargetAttempts { get; }

        public int AutoTargetsAcquired { get; }

        public int SkillTargetAttempts { get; }

        public int SkillTargetsAcquired { get; }

        public int SkillCastAttempts { get; }

        public int SkillCastsSucceeded { get; }

        public int SkillCastsBlockedByCooldown { get; }

        public int SkillCastsBlockedByTarget { get; }

        public int SkillCastsBlockedByMissingDefinition { get; }

        public int InteractionAttempts { get; }

        public int InteractionSuccesses { get; }

        public int InteractionBlockedByRange { get; }

        public string BuildSummary()
        {
            return "layer " + Layer
                + " " + NodeId
                + " result " + Result
                + " time " + DurationSeconds.ToString("0.0") + "s"
                + " sleep " + SleepDelta.ToString("0.0")
                + " poop " + PoopIncidents
                + " maxLost " + SleepMaxLost.ToString("0")
                + " litter " + LitterBoxUses
                + " feeder " + FeederUses
                + " bed " + BedCareUses
                + " weak " + WeakIncidents
                + " catPressure " + CatPressureEvents
                + " catDamage " + CatDamageTaken.ToString("0.#") + "/" + CatDamageIncoming.ToString("0.#")
                + " catAbsorbed " + CatDamageAbsorbed.ToString("0.#")
                + " catHeal " + CatHealEvents + "/" + CatHealingApplied.ToString("0.#")
                + " catShield " + CatShieldEvents + "/" + CatShieldApplied.ToString("0.#")
                + " enemyPressure " + EnemySleepPressureEvents
                + " bedHits " + BedPressureHits
                + " bossThrows " + BossThrowPressureHits
                + " sleep " + EnemySleepDamageTaken.ToString("0.#") + "/" + EnemySleepDamageIncoming.ToString("0.#")
                + " absorbed " + EnemySleepDamageAbsorbed.ToString("0.#")
                + " switches " + CatSwitchesSucceeded + "/" + CatSwitchAttempts
                + " switchBlocks weak " + CatSwitchesBlockedByWeak
                + " targets auto " + AutoTargetsAcquired + "/" + AutoTargetAttempts
                + " skill " + SkillTargetsAcquired + "/" + SkillTargetAttempts
                + " skills " + SkillCastsSucceeded + "/" + SkillCastAttempts
                + " skillBlocks cd " + SkillCastsBlockedByCooldown + " target " + SkillCastsBlockedByTarget + " missing " + SkillCastsBlockedByMissingDefinition
                + " interactions " + InteractionSuccesses + "/" + InteractionAttempts
                + " rangeBlocks " + InteractionBlockedByRange;
        }
    }

    public sealed class P0GrayboxTelemetryReport
    {
        private readonly List<P0GrayboxTelemetryIssue> issues = new List<P0GrayboxTelemetryIssue>();
        private readonly List<P0GrayboxTelemetryNodeRow> nodes = new List<P0GrayboxTelemetryNodeRow>();

        public P0GrayboxTelemetryReport(RunMetricsSummary summary, P0RunSettlementSummary settlement)
        {
            Summary = summary;
            Settlement = settlement;
        }

        public RunMetricsSummary Summary { get; }

        public P0RunSettlementSummary Settlement { get; }

        public IReadOnlyList<P0GrayboxTelemetryIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<P0GrayboxTelemetryNodeRow> Nodes => nodes.AsReadOnly();

        public int FailureCount => Count(P0GrayboxTelemetrySeverity.Failure);

        public int WarningCount => Count(P0GrayboxTelemetrySeverity.Warning);

        public bool IsUsable => FailureCount == 0;

        public int CompletedNodeCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].IsComplete)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public void AddIssue(P0GrayboxTelemetrySeverity severity, string message)
        {
            issues.Add(new P0GrayboxTelemetryIssue(severity, message));
        }

        public void AddNode(P0GrayboxTelemetryNodeRow row)
        {
            nodes.Add(row);
        }

        public string BuildSummary()
        {
            if (!IsUsable)
            {
                return "P0 graybox telemetry has " + FailureCount + " failure(s) and " + WarningCount + " warning(s).";
            }

            return "P0 graybox telemetry captured " + CompletedNodeCount + "/" + Nodes.Count
                + " node(s), success " + Summary.SuccessCount
                + ", failure " + Summary.FailureCount
                + ", time " + Summary.DurationSeconds.ToString("0.0") + "s"
                + ", poop " + Summary.PoopIncidents
                + ", maxLost " + Summary.SleepMaxLost.ToString("0")
                + ", litter " + Summary.LitterBoxUses
                + ", feeder " + Summary.FeederUses
                + ", weak " + Summary.WeakIncidents
                + ", cat pressure " + Summary.CatPressureEvents
                + " damage " + Summary.CatDamageTaken.ToString("0.#") + "/" + Summary.CatDamageIncoming.ToString("0.#")
                + " shields " + Summary.CatShieldEvents + "/" + Summary.CatShieldApplied.ToString("0.#")
                + ", pressure " + Summary.EnemySleepPressureEvents
                + " sleep " + Summary.EnemySleepDamageTaken.ToString("0.#") + "/" + Summary.EnemySleepDamageIncoming.ToString("0.#")
                + ", switches " + Summary.CatSwitchesSucceeded + "/" + Summary.CatSwitchAttempts
                + ", targets auto " + Summary.AutoTargetsAcquired + "/" + Summary.AutoTargetAttempts
                + " skill " + Summary.SkillTargetsAcquired + "/" + Summary.SkillTargetAttempts
                + ", skills " + Summary.SkillCastsSucceeded + "/" + Summary.SkillCastAttempts
                + ", interactions " + Summary.InteractionSuccesses + "/" + Summary.InteractionAttempts
                + ".";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Settlement: " + Settlement.ResultLabel
                    + " route " + Settlement.CompletedNodes + "/" + Settlement.TotalLayers
                    + " battles " + Settlement.BattleSuccesses + "W/" + Settlement.BattleFailures + "L"
                    + " sleep " + Settlement.OwnerSleepCurrent.ToString("0.#") + "/" + Settlement.OwnerSleepMax.ToString("0.#")
                    + " poop " + Settlement.TeamPoop.ToString("0.#")
                    + " hunger " + Settlement.TeamHunger.ToString("0.#")
            };

            for (int i = 0; i < nodes.Count; i++)
            {
                lines.Add("[Node] " + nodes[i].BuildSummary());
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0GrayboxTelemetrySeverity severity)
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

    public static class P0GrayboxTelemetry
    {
        public static P0GrayboxTelemetryReport EvaluateGoldenPath()
        {
            return Evaluate(P0GoldenPathSimulator.SimulateDefaultRun());
        }

        public static P0GrayboxTelemetryReport Evaluate(P0GoldenPathReport report)
        {
            if (report == null)
            {
                return Evaluate((RunProgressionState)null);
            }

            return Evaluate(report.Run);
        }

        public static P0GrayboxTelemetryReport Evaluate(RunProgressionState run)
        {
            if (run == null)
            {
                P0GrayboxTelemetryReport missing = new P0GrayboxTelemetryReport(
                    default(RunMetricsSummary),
                    default(P0RunSettlementSummary));
                missing.AddIssue(P0GrayboxTelemetrySeverity.Failure, "Run progression state is missing.");
                return missing;
            }

            RunMetricsSummary summary = run.Metrics.GetSummary();
            P0GrayboxTelemetryReport report = new P0GrayboxTelemetryReport(summary, new P0RunSettlementSummary(run));
            for (int i = 0; i < run.Metrics.Nodes.Count; i++)
            {
                NodeMetrics node = run.Metrics.Nodes[i];
                report.AddNode(new P0GrayboxTelemetryNodeRow(node));
                if (!node.IsComplete)
                {
                    report.AddIssue(
                        P0GrayboxTelemetrySeverity.Warning,
                        "Node metrics are still in progress: " + node.NodeId + ".");
                }
            }

            EvaluateCoverage(report, summary);
            return report;
        }

        private static void EvaluateCoverage(P0GrayboxTelemetryReport report, RunMetricsSummary summary)
        {
            if (summary.NodeCount <= 0)
            {
                report.AddIssue(P0GrayboxTelemetrySeverity.Failure, "No battle node telemetry has been recorded.");
                return;
            }

            if (report.CompletedNodeCount <= 0)
            {
                report.AddIssue(P0GrayboxTelemetrySeverity.Failure, "No completed battle node telemetry has been recorded.");
            }

            report.AddIssue(
                P0GrayboxTelemetrySeverity.Info,
                "Tracked metrics: node success/failure time, owner sleep delta, poop incidents, sleep max loss, enemy sleep pressure, cat pressure/heal/shield, litter box uses, feeder uses, bed care uses, and cat weak incidents.");

            report.AddIssue(
                P0GrayboxTelemetrySeverity.Info,
                "Tracked action metrics: cat switch attempts/successes, weak switch blocks, auto target attempts/acquisitions, skill target attempts/acquisitions, skill attempts/successes, cooldown blocks, target blocks, missing skill blocks, interaction attempts/successes, and range blocks.");

            if (summary.FailureCount > 0)
            {
                report.AddIssue(P0GrayboxTelemetrySeverity.Warning, "Telemetry includes failed battle node(s): " + summary.FailureCount + ".");
            }
        }
    }
}
