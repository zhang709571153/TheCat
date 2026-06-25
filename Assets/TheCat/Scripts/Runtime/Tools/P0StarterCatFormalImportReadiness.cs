using System;
using System.Collections.Generic;
using System.IO;

namespace TheCat.Tools
{
    public delegate bool P0StarterCatReviewTextReader(string path, out string text, out string error);

    public enum P0StarterCatFormalImportState
    {
        Blocked,
        Approved,
        Invalid
    }

    public enum P0StarterCatFormalImportSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatFormalImportIssue
    {
        public P0StarterCatFormalImportIssue(P0StarterCatFormalImportSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatFormalImportSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatFormalImportReadinessReport
    {
        private readonly List<P0StarterCatFormalImportIssue> issues = new List<P0StarterCatFormalImportIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatFormalImportIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public P0StarterCatFormalImportState State { get; private set; } = P0StarterCatFormalImportState.Invalid;

        public int StarterCatCount { get; private set; }

        public int ReviewNoteCount { get; private set; }

        public int ExplicitBlockNoteCount { get; private set; }

        public int ExplicitApprovalNoteCount { get; private set; }

        public int ActiveCatScreenshotCount { get; private set; }

        public int FailureCount => Count(P0StarterCatFormalImportSeverity.Failure);

        public bool IsGateValid => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatFormalImportReadiness.ExpectedCoveredCheckCount
            && State != P0StarterCatFormalImportState.Invalid;

        public bool IsImportAllowed => IsGateValid && State == P0StarterCatFormalImportState.Approved;

        public void SetCounts(
            int starterCatCount,
            int reviewNoteCount,
            int explicitBlockNoteCount,
            int explicitApprovalNoteCount,
            int activeCatScreenshotCount)
        {
            StarterCatCount = starterCatCount;
            ReviewNoteCount = reviewNoteCount;
            ExplicitBlockNoteCount = explicitBlockNoteCount;
            ExplicitApprovalNoteCount = explicitApprovalNoteCount;
            ActiveCatScreenshotCount = activeCatScreenshotCount;
        }

        public void SetState(P0StarterCatFormalImportState state)
        {
            State = state;
        }

        public void AddIssue(P0StarterCatFormalImportSeverity severity, string message)
        {
            issues.Add(new P0StarterCatFormalImportIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            if (IsImportAllowed)
            {
                return "P0 starter cat formal import gate approved for " + StarterCatCount + " starter cat(s).";
            }

            return IsGateValid
                ? "P0 starter cat formal import gate explicitly blocked for " + StarterCatCount + " starter cat(s)."
                : "P0 starter cat formal import gate invalid with " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "State: " + State,
                "Starter cats: " + StarterCatCount,
                "Review notes: " + ReviewNoteCount,
                "Explicit block notes: " + ExplicitBlockNoteCount,
                "Explicit approval notes: " + ExplicitApprovalNoteCount,
                "Active-cat screenshots: " + ActiveCatScreenshotCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0StarterCatFormalImportSeverity severity)
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

    public static class P0StarterCatFormalImportReadiness
    {
        public const int ExpectedStarterCatCount = 3;
        public const int ExpectedCoveredCheckCount = 5;
        public const string FormalImportBlockPhrase = "formal starter-cat import remains blocked pending active-cat screenshot comparison approval";
        public const string LegacyMissingActiveCatScreenshotBlockPhrase = "active-cat Play Mode screenshots are still missing";

        public static readonly string[] ActiveCatScreenshotFileNames =
        {
            P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName,
            P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName,
            P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName
        };

        public static readonly string[] ReviewNotePaths =
        {
            P0StarterCatDerivativeCandidateEvidence.CandidateRoot + "/saiban/" + P0StarterCatDerivativeCandidateEvidence.BatchSlug + "/saiban_batch05_source_locked_candidate_review.md",
            P0StarterCatDerivativeCandidateEvidence.CandidateRoot + "/nephthys/" + P0StarterCatDerivativeCandidateEvidence.BatchSlug + "/nephthys_batch05_source_locked_candidate_review.md",
            P0StarterCatDerivativeCandidateEvidence.CandidateRoot + "/suzune/" + P0StarterCatDerivativeCandidateEvidence.BatchSlug + "/suzune_batch05_source_locked_candidate_review.md"
        };

        public static P0StarterCatFormalImportReadinessReport EvaluateCurrentGate()
        {
            return Evaluate(
                P0StarterCatDerivativeCandidateEvidence.EvaluateBatch05(),
                P0StarterCatAssetProductionSpec.EvaluateP0Spec(),
                P0PlayModeScreenshotFileEvidence.EvaluateP0Directory(),
                ReviewNotePaths,
                DefaultReadText);
        }

        public static P0StarterCatFormalImportReadinessReport Evaluate(
            P0StarterCatDerivativeCandidateEvidenceReport candidateEvidence,
            P0StarterCatAssetProductionSpecReport productionSpec,
            P0PlayModeScreenshotFileEvidenceReport screenshotEvidence,
            IReadOnlyList<string> reviewNotePaths,
            P0StarterCatReviewTextReader readText)
        {
            P0StarterCatFormalImportReadinessReport report = new P0StarterCatFormalImportReadinessReport();
            P0StarterCatReviewTextReader reader = readText ?? DefaultReadText;
            IReadOnlyList<string> notes = reviewNotePaths ?? Array.Empty<string>();

            int reviewNoteCount = 0;
            int explicitBlockNoteCount = 0;
            int explicitApprovalNoteCount = 0;

            for (int i = 0; i < notes.Count; i++)
            {
                string path = notes[i] ?? string.Empty;
                if (!reader(path, out string text, out string error))
                {
                    report.AddIssue(P0StarterCatFormalImportSeverity.Failure, "Could not read starter cat review note " + path + ": " + error);
                    continue;
                }

                reviewNoteCount++;
                if (IsExplicitBlockNote(text))
                {
                    explicitBlockNoteCount++;
                }

                if (IsExplicitApprovalNote(text))
                {
                    explicitApprovalNoteCount++;
                }
            }

            int activeCatScreenshotCount = CountActiveCatScreenshots(screenshotEvidence);
            report.SetCounts(
                ExpectedStarterCatCount,
                reviewNoteCount,
                explicitBlockNoteCount,
                explicitApprovalNoteCount,
                activeCatScreenshotCount);

            bool candidateReady = candidateEvidence != null && candidateEvidence.IsReviewReady;
            bool specReady = productionSpec != null && productionSpec.IsReady;
            bool allNotesPresent = reviewNoteCount == ExpectedStarterCatCount;
            bool allScreenshotsPresent = activeCatScreenshotCount == ExpectedStarterCatCount;
            bool allExplicitlyBlocked = explicitBlockNoteCount == ExpectedStarterCatCount && explicitApprovalNoteCount == 0;
            bool allExplicitlyApproved = explicitApprovalNoteCount == ExpectedStarterCatCount && explicitBlockNoteCount == 0;

            Require(report, candidateReady, "Starter cat candidate evidence exists outside Assets and is review-ready.", "Starter cat candidate evidence is incomplete.");
            Require(report, specReady, "Starter cat production spec requires source locks, screenshots, side-by-side comparison, decisions, and rejection rules.", "Starter cat production spec is incomplete.");
            Require(report, allNotesPresent, "Starter cat review notes exist for Saiban, Nephthys, and Suzune.", "Starter cat formal import review notes are missing.");
            Require(report, allExplicitlyBlocked || allExplicitlyApproved, "Starter cat review notes contain a consistent explicit import decision.", "Starter cat review notes must be consistently blocked or approved.");

            if (allExplicitlyApproved)
            {
                Require(report, allScreenshotsPresent, "All three active-cat Play Mode screenshots exist before formal import approval.", "Formal starter cat import approval requires all three active-cat Play Mode screenshots.");
                report.SetState(allScreenshotsPresent && report.FailureCount == 0
                    ? P0StarterCatFormalImportState.Approved
                    : P0StarterCatFormalImportState.Invalid);
            }
            else if (allExplicitlyBlocked)
            {
                Require(report, explicitApprovalNoteCount == 0, "Formal starter cat imports remain blocked until explicit approval notes replace the block notes.", "Starter cat review notes contain conflicting approval text.");
                report.SetState(report.FailureCount == 0
                    ? P0StarterCatFormalImportState.Blocked
                    : P0StarterCatFormalImportState.Invalid);
            }
            else
            {
                report.SetState(P0StarterCatFormalImportState.Invalid);
            }

            return report;
        }

        private static int CountActiveCatScreenshots(P0PlayModeScreenshotFileEvidenceReport screenshotEvidence)
        {
            if (screenshotEvidence == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < ActiveCatScreenshotFileNames.Length; i++)
            {
                if (Contains(screenshotEvidence.ExistingExpectedFiles, ActiveCatScreenshotFileNames[i]))
                {
                    count++;
                }
            }

            return count;
        }

        private static bool IsExplicitBlockNote(string text)
        {
            return ContainsText(text, "do not import into Unity yet")
                && (ContainsText(text, FormalImportBlockPhrase)
                    || ContainsText(text, LegacyMissingActiveCatScreenshotBlockPhrase))
                && ContainsText(text, "colored turnaround contact sheet");
        }

        private static bool IsExplicitApprovalNote(string text)
        {
            return ContainsText(text, "approved for Unity import")
                && ContainsText(text, "active-cat Play Mode screenshot approved")
                && ContainsText(text, "colored turnaround contact sheet");
        }

        private static bool Contains(IReadOnlyList<string> values, string expected)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsText(string text, string expected)
        {
            return text != null && text.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultReadText(string path, out string text, out string error)
        {
            text = string.Empty;
            error = string.Empty;

            try
            {
                string resolved = ResolveFile(path);
                if (string.IsNullOrWhiteSpace(resolved) || !File.Exists(resolved))
                {
                    error = "file is missing";
                    return false;
                }

                text = File.ReadAllText(resolved);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static string ResolveFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            if (File.Exists(path))
            {
                return path;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                current = current.Parent;
            }

            return path;
        }

        private static void Require(
            P0StarterCatFormalImportReadinessReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatFormalImportSeverity.Failure, failureMessage);
        }
    }
}
