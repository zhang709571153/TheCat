using System;
using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public sealed class P0GoldenPathAcceptanceProfile
    {
        public P0GoldenPathAcceptanceProfile(
            int expectedTotalLayers = 10,
            int expectedBattleCount = 5,
            float minimumOwnerSleep = 1f,
            float warningOwnerSleep = 25f,
            float warningTeamPoop = 80f,
            float warningTeamHunger = 20f,
            float warningTotalDurationSeconds = 360f,
            float warningBattleDurationSeconds = 130f,
            int minimumEnemySleepPressureEvents = 1,
            float minimumEnemySleepDamageIncoming = 1f,
            int minimumCatShieldEvents = 1,
            int minimumCatSwitchesPerBattle = 2,
            int minimumAutoTargetsPerBattle = 1,
            int minimumSkillTargetsPerBattle = 1,
            int minimumSkillCastsPerBattle = 1,
            int minimumInteractionsPerBattle = 1)
        {
            ExpectedTotalLayers = RequirePositive(expectedTotalLayers, nameof(expectedTotalLayers));
            ExpectedBattleCount = RequirePositive(expectedBattleCount, nameof(expectedBattleCount));
            MinimumOwnerSleep = RequireNonNegative(minimumOwnerSleep, nameof(minimumOwnerSleep));
            WarningOwnerSleep = RequireNonNegative(warningOwnerSleep, nameof(warningOwnerSleep));
            WarningTeamPoop = RequireNonNegative(warningTeamPoop, nameof(warningTeamPoop));
            WarningTeamHunger = RequireNonNegative(warningTeamHunger, nameof(warningTeamHunger));
            WarningTotalDurationSeconds = RequirePositive(warningTotalDurationSeconds, nameof(warningTotalDurationSeconds));
            WarningBattleDurationSeconds = RequirePositive(warningBattleDurationSeconds, nameof(warningBattleDurationSeconds));
            MinimumEnemySleepPressureEvents = RequireNonNegative(minimumEnemySleepPressureEvents, nameof(minimumEnemySleepPressureEvents));
            MinimumEnemySleepDamageIncoming = RequireNonNegative(minimumEnemySleepDamageIncoming, nameof(minimumEnemySleepDamageIncoming));
            MinimumCatShieldEvents = RequireNonNegative(minimumCatShieldEvents, nameof(minimumCatShieldEvents));
            MinimumCatSwitchesPerBattle = RequireNonNegative(minimumCatSwitchesPerBattle, nameof(minimumCatSwitchesPerBattle));
            MinimumAutoTargetsPerBattle = RequireNonNegative(minimumAutoTargetsPerBattle, nameof(minimumAutoTargetsPerBattle));
            MinimumSkillTargetsPerBattle = RequireNonNegative(minimumSkillTargetsPerBattle, nameof(minimumSkillTargetsPerBattle));
            MinimumSkillCastsPerBattle = RequireNonNegative(minimumSkillCastsPerBattle, nameof(minimumSkillCastsPerBattle));
            MinimumInteractionsPerBattle = RequireNonNegative(minimumInteractionsPerBattle, nameof(minimumInteractionsPerBattle));
        }

        public static P0GoldenPathAcceptanceProfile Default => new P0GoldenPathAcceptanceProfile();

        public int ExpectedTotalLayers { get; }

        public int ExpectedBattleCount { get; }

        public float MinimumOwnerSleep { get; }

        public float WarningOwnerSleep { get; }

        public float WarningTeamPoop { get; }

        public float WarningTeamHunger { get; }

        public float WarningTotalDurationSeconds { get; }

        public float WarningBattleDurationSeconds { get; }

        public int MinimumEnemySleepPressureEvents { get; }

        public float MinimumEnemySleepDamageIncoming { get; }

        public int MinimumCatShieldEvents { get; }

        public int MinimumCatSwitchesPerBattle { get; }

        public int MinimumAutoTargetsPerBattle { get; }

        public int MinimumSkillTargetsPerBattle { get; }

        public int MinimumSkillCastsPerBattle { get; }

        public int MinimumInteractionsPerBattle { get; }

        private static int RequirePositive(int value, string name)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be greater than zero.");
            }

            return value;
        }

        private static float RequirePositive(float value, string name)
        {
            if (value <= 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must be greater than zero.");
            }

            return value;
        }

        private static int RequireNonNegative(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }

            return value;
        }

        private static float RequireNonNegative(float value, string name)
        {
            if (value < 0f)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }

            return value;
        }
    }

    public enum P0GoldenPathAcceptanceSeverity
    {
        Info,
        Warning,
        Failure
    }

    public readonly struct P0GoldenPathAcceptanceIssue
    {
        public P0GoldenPathAcceptanceIssue(P0GoldenPathAcceptanceSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0GoldenPathAcceptanceSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0GoldenPathAcceptanceReport
    {
        private readonly List<P0GoldenPathAcceptanceIssue> issues = new List<P0GoldenPathAcceptanceIssue>();

        public IReadOnlyList<P0GoldenPathAcceptanceIssue> Issues => issues.AsReadOnly();

        public int FailureCount => Count(P0GoldenPathAcceptanceSeverity.Failure);

        public int WarningCount => Count(P0GoldenPathAcceptanceSeverity.Warning);

        public bool IsAccepted => FailureCount == 0;

        public void Add(P0GoldenPathAcceptanceSeverity severity, string message)
        {
            issues.Add(new P0GoldenPathAcceptanceIssue(severity, message));
        }

        public string BuildSummary()
        {
            return IsAccepted
                ? "P0 golden path accepted with " + WarningCount + " warning(s)."
                : "P0 golden path rejected with " + FailureCount + " failure(s) and " + WarningCount + " warning(s).";
        }

        public string BuildDetailedSummary()
        {
            if (issues.Count == 0)
            {
                return BuildSummary();
            }

            List<string> lines = new List<string>
            {
                BuildSummary()
            };
            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0GoldenPathAcceptanceSeverity severity)
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

    public static class P0GoldenPathAcceptance
    {
        public static P0GoldenPathAcceptanceReport Evaluate(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceProfile profile = null)
        {
            P0GoldenPathAcceptanceProfile activeProfile = profile ?? P0GoldenPathAcceptanceProfile.Default;
            P0GoldenPathAcceptanceReport acceptance = new P0GoldenPathAcceptanceReport();
            if (report == null)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Golden path report is missing.");
                return acceptance;
            }

            EvaluateRoute(report, activeProfile, acceptance);
            EvaluateBattles(report, activeProfile, acceptance);
            EvaluateRouteContent(report, acceptance);
            EvaluateEnemyPressure(report, activeProfile, acceptance);
            EvaluateCatVitals(report, activeProfile, acceptance);
            EvaluateCoreValues(report, activeProfile, acceptance);
            return acceptance;
        }

        private static void EvaluateRoute(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceProfile profile,
            P0GoldenPathAcceptanceReport acceptance)
        {
            if (!report.IsCleared)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Run did not clear the route.");
            }

            if (report.Settlement.IsFailed)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Run is marked failed.");
            }

            if (report.Settlement.TotalLayers != profile.ExpectedTotalLayers)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Expected " + profile.ExpectedTotalLayers + " route layers but saw " + report.Settlement.TotalLayers + ".");
            }

            if (report.Settlement.CompletedNodes != report.Settlement.TotalLayers)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Completed nodes " + report.Settlement.CompletedNodes + " do not match total layers " + report.Settlement.TotalLayers + ".");
            }
        }

        private static void EvaluateBattles(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceProfile profile,
            P0GoldenPathAcceptanceReport acceptance)
        {
            if (report.BattleCount != profile.ExpectedBattleCount)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Expected " + profile.ExpectedBattleCount + " battle nodes but saw " + report.BattleCount + ".");
            }

            if (report.Settlement.BattleSuccesses != report.BattleCount || report.Settlement.BattleFailures != 0)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Battle result mismatch: " + report.Settlement.BattleSuccesses + " success, "
                    + report.Settlement.BattleFailures + " failure, " + report.BattleCount + " report(s).");
            }

            for (int i = 0; i < report.BattleReports.Count; i++)
            {
                P0GoldenPathBattleReport battle = report.BattleReports[i];
                if (battle.Outcome != BattleOutcome.Victory)
                {
                    acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, battle.NodeId + " ended with " + battle.Outcome + ".");
                }

                if (battle.DurationSeconds > profile.WarningBattleDurationSeconds)
                {
                    acceptance.Add(
                        P0GoldenPathAcceptanceSeverity.Warning,
                        battle.NodeId + " duration " + battle.DurationSeconds.ToString("0.0")
                        + "s exceeds tuning review threshold " + profile.WarningBattleDurationSeconds.ToString("0.0") + "s.");
                }

                if (battle.CatSwitchesSucceeded < profile.MinimumCatSwitchesPerBattle
                    || battle.CatSwitchAttempts < profile.MinimumCatSwitchesPerBattle)
                {
                    acceptance.Add(
                        P0GoldenPathAcceptanceSeverity.Failure,
                        battle.NodeId + " cat switch telemetry " + battle.CatSwitchesSucceeded + "/"
                        + battle.CatSwitchAttempts + " is below required "
                        + profile.MinimumCatSwitchesPerBattle + " per battle.");
                }

                if (battle.AutoTargetsAcquired < profile.MinimumAutoTargetsPerBattle
                    || battle.AutoTargetAttempts < profile.MinimumAutoTargetsPerBattle)
                {
                    acceptance.Add(
                        P0GoldenPathAcceptanceSeverity.Failure,
                        battle.NodeId + " auto target telemetry " + battle.AutoTargetsAcquired + "/"
                        + battle.AutoTargetAttempts + " is below required "
                        + profile.MinimumAutoTargetsPerBattle + " per battle.");
                }

                if (battle.SkillTargetsAcquired < profile.MinimumSkillTargetsPerBattle
                    || battle.SkillTargetAttempts < profile.MinimumSkillTargetsPerBattle)
                {
                    acceptance.Add(
                        P0GoldenPathAcceptanceSeverity.Failure,
                        battle.NodeId + " skill target telemetry " + battle.SkillTargetsAcquired + "/"
                        + battle.SkillTargetAttempts + " is below required "
                        + profile.MinimumSkillTargetsPerBattle + " per battle.");
                }

                if (battle.SkillCastsSucceeded < profile.MinimumSkillCastsPerBattle
                    || battle.SkillCastAttempts < profile.MinimumSkillCastsPerBattle)
                {
                    acceptance.Add(
                        P0GoldenPathAcceptanceSeverity.Failure,
                        battle.NodeId + " skill action telemetry " + battle.SkillCastsSucceeded + "/"
                        + battle.SkillCastAttempts + " is below required "
                        + profile.MinimumSkillCastsPerBattle + " per battle.");
                }

                if (battle.InteractionSuccesses < profile.MinimumInteractionsPerBattle
                    || battle.InteractionAttempts < profile.MinimumInteractionsPerBattle)
                {
                    acceptance.Add(
                        P0GoldenPathAcceptanceSeverity.Failure,
                        battle.NodeId + " interaction telemetry " + battle.InteractionSuccesses + "/"
                        + battle.InteractionAttempts + " is below required "
                        + profile.MinimumInteractionsPerBattle + " per battle.");
                }
            }

            acceptance.Add(
                P0GoldenPathAcceptanceSeverity.Info,
                "Action telemetry: switches " + report.Settlement.CatSwitchesSucceeded + "/"
                + report.Settlement.CatSwitchAttempts + ", targets auto "
                + report.Settlement.AutoTargetsAcquired + "/" + report.Settlement.AutoTargetAttempts
                + " skill " + report.Settlement.SkillTargetsAcquired + "/" + report.Settlement.SkillTargetAttempts
                + ", skills " + report.Settlement.SkillCastsSucceeded + "/"
                + report.Settlement.SkillCastAttempts + ", interactions "
                + report.Settlement.InteractionSuccesses + "/"
                + report.Settlement.InteractionAttempts + ".");

            if (!report.BossBattleCleared)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Boss battle did not clear.");
            }

            if (!report.BossBehaviorObserved)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Boss summon and throw behavior were not both observed.");
            }
        }

        private static void EvaluateRouteContent(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceReport acceptance)
        {
            if (report.Settlement.DreamEventsResolved <= 0)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "No dream event was resolved.");
            }

            if (report.Settlement.ShopPurchases <= 0)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "No shop node was resolved.");
            }

            if (report.Settlement.RestNestUses <= 0)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "No rest nest node was resolved.");
            }

            if (report.Settlement.BlessingCount <= 0)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "No authority blessing was gained.");
            }

            if (!report.Run.Roster.HasCat(P0RouteRewardResolver.PreviewPartnerId))
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Preview partner was not recruited.");
            }

            if (report.Settlement.DreamShards <= 0 || report.Settlement.FishTreats <= 0)
            {
                acceptance.Add(P0GoldenPathAcceptanceSeverity.Failure, "Run did not end with both dream shards and fish treats.");
            }
        }

        private static void EvaluateEnemyPressure(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceProfile profile,
            P0GoldenPathAcceptanceReport acceptance)
        {
            if (report.Settlement.EnemySleepPressureEvents < profile.MinimumEnemySleepPressureEvents)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Enemy sleep pressure events " + report.Settlement.EnemySleepPressureEvents
                    + " are below required minimum " + profile.MinimumEnemySleepPressureEvents + ".");
            }

            if (report.Settlement.EnemySleepDamageIncoming < profile.MinimumEnemySleepDamageIncoming)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Enemy sleep pressure incoming damage " + report.Settlement.EnemySleepDamageIncoming.ToString("0.#")
                    + " is below required minimum " + profile.MinimumEnemySleepDamageIncoming.ToString("0.#") + ".");
            }

            acceptance.Add(
                P0GoldenPathAcceptanceSeverity.Info,
                "Enemy pressure: events " + report.Settlement.EnemySleepPressureEvents
                + ", bed " + report.Settlement.BedPressureHits
                + ", boss throws " + report.Settlement.BossThrowPressureHits
                + ", sleep " + report.Settlement.EnemySleepDamageTaken.ToString("0.#")
                + "/" + report.Settlement.EnemySleepDamageIncoming.ToString("0.#")
                + ", absorbed " + report.Settlement.EnemySleepDamageAbsorbed.ToString("0.#") + ".");
        }

        private static void EvaluateCatVitals(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceProfile profile,
            P0GoldenPathAcceptanceReport acceptance)
        {
            if (report.Settlement.CatShieldEvents < profile.MinimumCatShieldEvents)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Cat shield telemetry " + report.Settlement.CatShieldEvents
                    + " is below required minimum " + profile.MinimumCatShieldEvents + ".");
            }

            acceptance.Add(
                P0GoldenPathAcceptanceSeverity.Info,
                "Cat vitality: pressure " + report.Settlement.CatPressureEvents
                + ", damage " + report.Settlement.CatDamageTaken.ToString("0.#")
                + "/" + report.Settlement.CatDamageIncoming.ToString("0.#")
                + ", absorbed " + report.Settlement.CatDamageAbsorbed.ToString("0.#")
                + ", heals " + report.Settlement.CatHealEvents
                + "/" + report.Settlement.CatHealingApplied.ToString("0.#")
                + ", shields " + report.Settlement.CatShieldEvents
                + "/" + report.Settlement.CatShieldApplied.ToString("0.#") + ".");
        }

        private static void EvaluateCoreValues(
            P0GoldenPathReport report,
            P0GoldenPathAcceptanceProfile profile,
            P0GoldenPathAcceptanceReport acceptance)
        {
            if (report.Settlement.OwnerSleepCurrent < profile.MinimumOwnerSleep)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Failure,
                    "Owner sleep " + report.Settlement.OwnerSleepCurrent.ToString("0.#")
                    + " is below required minimum " + profile.MinimumOwnerSleep.ToString("0.#") + ".");
            }

            if (report.Settlement.OwnerSleepCurrent < profile.WarningOwnerSleep)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Warning,
                    "Owner sleep ends low at " + report.Settlement.OwnerSleepCurrent.ToString("0.#") + ".");
            }

            if (report.Settlement.TeamPoop >= profile.WarningTeamPoop)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Warning,
                    "Team poop ends high at " + report.Settlement.TeamPoop.ToString("0.#") + ".");
            }

            if (report.Settlement.TeamHunger <= profile.WarningTeamHunger)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Warning,
                    "Team hunger ends low at " + report.Settlement.TeamHunger.ToString("0.#") + ".");
            }

            if (report.Settlement.DurationSeconds > profile.WarningTotalDurationSeconds)
            {
                acceptance.Add(
                    P0GoldenPathAcceptanceSeverity.Warning,
                    "Total battle duration " + report.Settlement.DurationSeconds.ToString("0.0")
                    + "s exceeds tuning review threshold " + profile.WarningTotalDurationSeconds.ToString("0.0") + "s.");
            }

            acceptance.Add(
                P0GoldenPathAcceptanceSeverity.Info,
                "Final core values: sleep " + report.Settlement.OwnerSleepCurrent.ToString("0.#")
                + "/" + report.Settlement.OwnerSleepMax.ToString("0.#")
                + ", poop " + report.Settlement.TeamPoop.ToString("0.#")
                + ", hunger " + report.Settlement.TeamHunger.ToString("0.#") + ".");
        }
    }
}
