using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public sealed class P0VisualAcceptanceReport
    {
        public P0VisualAcceptanceReport(
            P0PlayableReadinessReport playableReadiness,
            P0AssetProductionReadinessReport assetProductionReadiness,
            P0AssetReviewPacketReport assetReviewPacket,
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings,
            P0ChineseUiScaleEvidencePacketReport chineseUiScaleEvidence,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence,
            P0PlayModeEvidenceReport playModeEvidence,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport,
            P0PlayModeReportFileEvidenceReport playModeReportFileEvidence = null)
        {
            PlayableReadiness = playableReadiness;
            AssetProductionReadiness = assetProductionReadiness;
            AssetReviewPacket = assetReviewPacket;
            RuntimeVisualBindings = runtimeVisualBindings;
            ChineseUiScaleEvidence = chineseUiScaleEvidence;
            ScreenshotFileEvidence = screenshotFileEvidence;
            PlayModeEvidence = playModeEvidence;
            StarterCatFormalImport = starterCatFormalImport;
            PlayModeReportFileEvidence = playModeReportFileEvidence;
        }

        public P0PlayableReadinessReport PlayableReadiness { get; }

        public P0AssetProductionReadinessReport AssetProductionReadiness { get; }

        public P0AssetReviewPacketReport AssetReviewPacket { get; }

        public P0RuntimeVisualBindingCoverageReport RuntimeVisualBindings { get; }

        public P0ChineseUiScaleEvidencePacketReport ChineseUiScaleEvidence { get; }

        public P0PlayModeScreenshotFileEvidenceReport ScreenshotFileEvidence { get; }

        public P0PlayModeEvidenceReport PlayModeEvidence { get; }

        public P0StarterCatFormalImportReadinessReport StarterCatFormalImport { get; }

        public P0PlayModeReportFileEvidenceReport PlayModeReportFileEvidence { get; }

        public bool HasCompletePlayModeEvidence =>
            PlayModeEvidence != null && PlayModeEvidence.IsComplete
            || PlayModeReportFileEvidence != null && PlayModeReportFileEvidence.IsComplete;

        public bool IsP0DemoVisualEvidenceReady =>
            IsArchitectureReadyForSystematicAssetProduction
            && ScreenshotFileEvidence != null
            && ScreenshotFileEvidence.IsComplete
            && HasCompletePlayModeEvidence
            && StarterCatFormalImport != null
            && StarterCatFormalImport.IsGateValid;

        public bool IsArchitectureReadyForSystematicAssetProduction =>
            PlayableReadiness != null
            && PlayableReadiness.IsReady
            && AssetProductionReadiness != null
            && AssetProductionReadiness.IsReady
            && AssetReviewPacket != null
            && AssetReviewPacket.IsReady
            && RuntimeVisualBindings != null
            && RuntimeVisualBindings.IsComplete
            && ChineseUiScaleEvidence != null
            && ChineseUiScaleEvidence.IsReady
            && StarterCatFormalImport != null
            && StarterCatFormalImport.IsGateValid;

        public bool IsFinalP0VisualAcceptanceReady =>
            IsArchitectureReadyForSystematicAssetProduction
            && ScreenshotFileEvidence != null
            && ScreenshotFileEvidence.IsComplete
            && HasCompletePlayModeEvidence
            && StarterCatFormalImport != null
            && StarterCatFormalImport.IsImportAllowed;

        public bool IsStarterCatFormalImportBlocked =>
            StarterCatFormalImport != null
            && StarterCatFormalImport.IsGateValid
            && !StarterCatFormalImport.IsImportAllowed;

        public string BuildSummary()
        {
            if (IsFinalP0VisualAcceptanceReady)
            {
                return "P0 visual acceptance is ready for final review and systematic asset integration.";
            }

            if (IsP0DemoVisualEvidenceReady)
            {
                return "P0 demo visual evidence is ready for the current baseline, but final visual acceptance remains blocked by formal install, Console, human review, or starter-cat import gates.";
            }

            if (IsArchitectureReadyForSystematicAssetProduction)
            {
                return "P0 architecture is ready for systematic asset production, but final visual acceptance is still blocked by evidence or starter-cat import review.";
            }

            return "P0 architecture is not ready for systematic asset production.";
        }

        public string BuildMarkdown()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# P0 Visual Acceptance Report");
            builder.AppendLine();
            builder.AppendLine("## Summary");
            builder.AppendLine();
            builder.AppendLine("- Architecture ready for systematic asset production: " + YesNo(IsArchitectureReadyForSystematicAssetProduction));
            builder.AppendLine("- Current P0 demo visual evidence ready: " + YesNo(IsP0DemoVisualEvidenceReady));
            builder.AppendLine("- Final P0 visual acceptance ready: " + YesNo(IsFinalP0VisualAcceptanceReady));
            builder.AppendLine("- Starter cat formal import allowed: " + YesNo(StarterCatFormalImport != null && StarterCatFormalImport.IsImportAllowed));
            builder.AppendLine("- Starter cat formal import state: " + (StarterCatFormalImport == null ? "missing" : StarterCatFormalImport.State.ToString()));
            builder.AppendLine("- Runtime visual bindings: " + Count(RuntimeVisualBindings == null ? 0 : RuntimeVisualBindings.BindingCount, P0VisualAssetCatalog.P0RuntimeVisualBindingCount));
            builder.AppendLine("- Resolved runtime textures: " + Count(RuntimeVisualBindings == null ? 0 : RuntimeVisualBindings.ResolvedTextureCount, P0VisualAssetCatalog.P0RuntimeVisualBindingCount));
            builder.AppendLine("- Chinese UI scale evidence packet: " + ChineseUiScaleEvidenceCount());
            builder.AppendLine("- Chinese UI scale evidence directory: " + (ChineseUiScaleEvidence == null ? "missing" : Escape(ChineseUiScaleEvidence.BatchDirectory)));
            builder.AppendLine("- Screenshot file evidence: " + ScreenshotEvidenceCount());
            builder.AppendLine("- Play Mode report file evidence: " + (PlayModeReportFileEvidence == null ? "missing" : Escape(PlayModeReportFileEvidence.BuildSummary())));
            builder.AppendLine();
            builder.AppendLine(BuildSummary());

            AppendBlockingWork(builder);
            AppendGateSummaries(builder);
            AppendRuntimeVisualBindingTable(builder);
            AppendStarterCatImportRule(builder);
            return builder.ToString();
        }

        private void AppendBlockingWork(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("## Blocking Or Pending Work");
            builder.AppendLine();

            bool hasAny = false;
            if (!IsArchitectureReadyForSystematicAssetProduction)
            {
                hasAny = true;
                builder.AppendLine("- Fix architecture gates before starting broad asset production.");
            }

            if (ChineseUiScaleEvidence != null && !ChineseUiScaleEvidence.IsReady)
            {
                hasAny = true;
                builder.AppendLine("- Regenerate Batch 75 Chinese UI scale evidence packet in `" + Escape(ChineseUiScaleEvidence.BatchDirectory) + "`.");
                AppendInlineFileList(builder, "Missing Batch 75 files", ChineseUiScaleEvidence.MissingExpectedFiles);
                AppendInlineFileList(builder, "Unexpected Unity meta files", ChineseUiScaleEvidence.UnexpectedMetaFiles);
            }

            if (ScreenshotFileEvidence != null && !ScreenshotFileEvidence.IsComplete)
            {
                hasAny = true;
                builder.AppendLine("- Regenerate Play Mode screenshot evidence in `" + Escape(ScreenshotFileEvidence.ScreenshotDirectory) + "`.");
                AppendInlineFileList(builder, "Missing screenshots", ScreenshotFileEvidence.MissingExpectedFiles);
                AppendInlineFileList(builder, "Unexpected screenshots", ScreenshotFileEvidence.UnexpectedPngFiles);
            }

            if (!HasCompletePlayModeEvidence)
            {
                hasAny = true;
                builder.AppendLine("- Complete Play Mode evidence checks or refresh the project-owned Play Mode acceptance report before final P0 visual acceptance.");
            }

            if (IsStarterCatFormalImportBlocked)
            {
                hasAny = true;
                builder.AppendLine("- Keep starter-cat formal imports blocked until Saiban, Nephthys, and Suzune active-cat screenshots are approved against colored three-view turnarounds.");
            }

            if (!hasAny)
            {
                builder.AppendLine("- None.");
            }
        }

        private void AppendGateSummaries(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("## Gate Summaries");
            builder.AppendLine();
            AppendGateSummary(builder, "Playable Readiness", PlayableReadiness == null ? "missing" : PlayableReadiness.BuildSummary());
            AppendGateSummary(builder, "Asset Production Readiness", AssetProductionReadiness == null ? "missing" : AssetProductionReadiness.BuildSummary());
            AppendGateSummary(builder, "Asset Review Packet", AssetReviewPacket == null ? "missing" : AssetReviewPacket.BuildSummary());
            AppendGateSummary(builder, "Runtime Visual Bindings", RuntimeVisualBindings == null ? "missing" : RuntimeVisualBindings.BuildSummary());
            AppendGateSummary(builder, "Chinese UI Scale Evidence Packet", ChineseUiScaleEvidence == null ? "missing" : ChineseUiScaleEvidence.BuildSummary());
            AppendGateSummary(builder, "Screenshot File Evidence", ScreenshotFileEvidence == null ? "missing" : ScreenshotFileEvidence.BuildSummary());
            AppendGateSummary(builder, "Play Mode Evidence", PlayModeEvidence == null ? "missing" : PlayModeEvidence.BuildSummary());
            AppendGateSummary(builder, "Play Mode Report File Evidence", PlayModeReportFileEvidence == null ? "missing" : PlayModeReportFileEvidence.BuildSummary());
            AppendGateSummary(builder, "Starter Cat Formal Import", StarterCatFormalImport == null ? "missing" : StarterCatFormalImport.BuildSummary());
        }

        private static void AppendRuntimeVisualBindingTable(StringBuilder builder)
        {
            IReadOnlyList<P0VisualAssetBinding> bindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();
            builder.AppendLine();
            builder.AppendLine("## Runtime Visual Binding Slots");
            builder.AppendLine();
            builder.AppendLine("| binding | surface | subject | slot | asset | source authority |");
            builder.AppendLine("| --- | --- | --- | --- | --- | --- |");
            for (int i = 0; i < bindings.Count; i++)
            {
                P0VisualAssetBinding binding = bindings[i];
                builder.Append("| ");
                builder.Append(Escape(binding.BindingId));
                builder.Append(" | ");
                builder.Append(Escape(binding.SurfaceId));
                builder.Append(" | ");
                builder.Append(Escape(binding.SubjectId));
                builder.Append(" | ");
                builder.Append(Escape(binding.SlotId));
                builder.Append(" | ");
                builder.Append(Escape(binding.Asset.AssetId));
                builder.Append(" | ");
                builder.Append(Escape(binding.SourceAuthority));
                builder.AppendLine(" |");
            }
        }

        private void AppendStarterCatImportRule(StringBuilder builder)
        {
            builder.AppendLine();
            builder.AppendLine("## Starter Cat Import Rule");
            builder.AppendLine();
            builder.AppendLine("- Saiban, Nephthys, and Suzune combat derivatives must remain source-locked to the colored three-view turnaround references.");
            builder.AppendLine("- Formal import requires all three active-cat Play Mode screenshots: `" + Escape(P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName) + "`, `" + Escape(P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName) + "`, `" + Escape(P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName) + "`.");
            builder.AppendLine("- Review notes must switch consistently from blocked to approved before replacing runtime cat sprites.");
            if (StarterCatFormalImport != null)
            {
                builder.AppendLine("- Current decision: " + StarterCatFormalImport.State + ", active-cat screenshots " + StarterCatFormalImport.ActiveCatScreenshotCount + "/" + P0StarterCatFormalImportReadiness.ExpectedStarterCatCount + ".");
            }
        }

        private static void AppendGateSummary(StringBuilder builder, string title, string summary)
        {
            builder.AppendLine("- " + title + ": " + Escape(summary));
        }

        private static void AppendInlineFileList(StringBuilder builder, string title, IReadOnlyList<string> fileNames)
        {
            if (fileNames == null || fileNames.Count == 0)
            {
                return;
            }

            builder.Append("  - " + title + ": ");
            for (int i = 0; i < fileNames.Count; i++)
            {
                if (i > 0)
                {
                    builder.Append(", ");
                }

                builder.Append("`");
                builder.Append(Escape(fileNames[i]));
                builder.Append("`");
            }

            builder.AppendLine();
        }

        private string ScreenshotEvidenceCount()
        {
            if (ScreenshotFileEvidence == null)
            {
                return "missing";
            }

            return Count(ScreenshotFileEvidence.ExistingExpectedFileCount, ScreenshotFileEvidence.ExpectedFileCount)
                + ", missing "
                + ScreenshotFileEvidence.MissingExpectedFileCount
                + ", unexpected "
                + ScreenshotFileEvidence.UnexpectedPngFileCount;
        }

        private string ChineseUiScaleEvidenceCount()
        {
            if (ChineseUiScaleEvidence == null)
            {
                return "missing";
            }

            return Count(ChineseUiScaleEvidence.ExistingExpectedFileCount, ChineseUiScaleEvidence.ExpectedFileCount)
                + ", capture rows "
                + ChineseUiScaleEvidence.CaptureMatrixRowCount
                + "/"
                + P0ChineseUiScaleEvidencePacket.ExpectedCaptureMatrixRowCount
                + ", meta "
                + ChineseUiScaleEvidence.UnexpectedMetaFileCount;
        }

        private static string Count(int current, int expected)
        {
            return current + "/" + expected;
        }

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }

        private static string Escape(string value)
        {
            return (value ?? string.Empty).Replace("|", "/");
        }
    }

    public sealed class P0PlayModeReportFileEvidenceReport
    {
        public string ReportPath { get; private set; } = string.Empty;

        public bool ReportExists { get; private set; }

        public bool ResultPassed { get; private set; }

        public bool SmokeStatePassed { get; private set; }

        public bool EvidenceSummaryPassed { get; private set; }

        public bool ScreenshotSmokePassed { get; private set; }

        public bool RouteFlowPassed { get; private set; }

        public bool DefeatFlowPassed { get; private set; }

        public bool ScreenshotFileEvidencePassed { get; private set; }

        public bool ScreenshotDirectoryMatches { get; private set; }

        public bool ActiveCatCapturesListed { get; private set; }

        public bool IsComplete =>
            ReportExists
            && ResultPassed
            && SmokeStatePassed
            && EvidenceSummaryPassed
            && ScreenshotSmokePassed
            && RouteFlowPassed
            && DefeatFlowPassed
            && ScreenshotFileEvidencePassed
            && ScreenshotDirectoryMatches
            && ActiveCatCapturesListed;

        public void SetResult(
            string reportPath,
            bool reportExists,
            bool resultPassed,
            bool smokeStatePassed,
            bool evidenceSummaryPassed,
            bool screenshotSmokePassed,
            bool routeFlowPassed,
            bool defeatFlowPassed,
            bool screenshotFileEvidencePassed,
            bool screenshotDirectoryMatches,
            bool activeCatCapturesListed)
        {
            ReportPath = reportPath ?? string.Empty;
            ReportExists = reportExists;
            ResultPassed = resultPassed;
            SmokeStatePassed = smokeStatePassed;
            EvidenceSummaryPassed = evidenceSummaryPassed;
            ScreenshotSmokePassed = screenshotSmokePassed;
            RouteFlowPassed = routeFlowPassed;
            DefeatFlowPassed = defeatFlowPassed;
            ScreenshotFileEvidencePassed = screenshotFileEvidencePassed;
            ScreenshotDirectoryMatches = screenshotDirectoryMatches;
            ActiveCatCapturesListed = activeCatCapturesListed;
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 Play Mode acceptance report file evidence complete for current 11-capture route/defeat smoke."
                : "P0 Play Mode acceptance report file evidence incomplete at `" + ReportPath + "`.";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Report path: " + ReportPath,
                "Report exists: " + YesNo(ReportExists),
                "Result passed: " + YesNo(ResultPassed),
                "Smoke state passed: " + YesNo(SmokeStatePassed),
                "Evidence summary passed: " + YesNo(EvidenceSummaryPassed),
                "Screenshot smoke passed: " + YesNo(ScreenshotSmokePassed),
                "Route flow passed: " + YesNo(RouteFlowPassed),
                "Defeat flow passed: " + YesNo(DefeatFlowPassed),
                "Screenshot file evidence passed: " + YesNo(ScreenshotFileEvidencePassed),
                "Screenshot directory matches: " + YesNo(ScreenshotDirectoryMatches),
                "Active cat captures listed: " + YesNo(ActiveCatCapturesListed)
            };

            return string.Join(Environment.NewLine, lines);
        }

        private static string YesNo(bool value)
        {
            return value ? "yes" : "no";
        }
    }

    public static class P0PlayModeReportFileEvidence
    {
        public const string DefaultReportPath = "design/development/unity_batchmode/P0_PLAYMODE_ACCEPTANCE_SMOKE_REPORT.md";

        public static P0PlayModeReportFileEvidenceReport EvaluateCurrentReport()
        {
            return Evaluate(DefaultReportPath, DefaultFileExists, DefaultReadAllText);
        }

        public static P0PlayModeReportFileEvidenceReport Evaluate(
            string reportPath,
            Func<string, bool> fileExists,
            Func<string, string> readAllText)
        {
            string path = reportPath ?? string.Empty;
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            Func<string, string> read = readAllText ?? DefaultReadAllText;
            bool reportExists = exists(path);
            string text = reportExists ? read(path) ?? string.Empty : string.Empty;

            P0PlayModeReportFileEvidenceReport report = new P0PlayModeReportFileEvidenceReport();
            report.SetResult(
                path,
                reportExists,
                Contains(text, "- Result: passed"),
                Contains(text, "- Smoke state: Passed"),
                Contains(text, "8 passed check(s)") && Contains(text, "0 pending warning(s)"),
                Contains(text, "screenshot smoke passed with 11 screenshot(s)"),
                Contains(text, "Route-flow smoke passed")
                    && Contains(text, "cat-room return verified")
                    && Contains(text, "RestNest next-battle recovery verified")
                    && Contains(text, "DreamEvent catnip next-battle modifier verified")
                    && Contains(text, "Shop bed-patch next-battle sleep verified"),
                Contains(text, "Defeat-flow smoke passed")
                    && Contains(text, "failed cat-room return verified"),
                Contains(text, "[Passed] Screenshot File Evidence")
                    && Contains(text, "11/11 expected validated capture(s)"),
                Contains(NormalizeSlashes(text), P0PlayModeScreenshotFileEvidence.DefaultScreenshotDirectory),
                Contains(text, P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName)
                    && Contains(text, P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName)
                    && Contains(text, P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName));
            return report;
        }

        private static bool Contains(string text, string expected)
        {
            return !string.IsNullOrWhiteSpace(text)
                && !string.IsNullOrWhiteSpace(expected)
                && text.IndexOf(expected, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string NormalizeSlashes(string text)
        {
            return (text ?? string.Empty).Replace('\\', '/');
        }

        private static bool DefaultFileExists(string path)
        {
            return !string.IsNullOrWhiteSpace(ResolveFilePath(path));
        }

        private static string DefaultReadAllText(string path)
        {
            string resolved = ResolveFilePath(path);
            return string.IsNullOrWhiteSpace(resolved) ? string.Empty : File.ReadAllText(resolved);
        }

        private static string ResolveFilePath(string path)
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

            return string.Empty;
        }
    }

    public static class P0VisualAcceptance
    {
        public static P0VisualAcceptanceReport EvaluateCurrent()
        {
            return Evaluate(
                P0PlayableReadiness.EvaluatePrototypeBuild(),
                P0AssetProductionReadiness.EvaluateP0OfflineReadiness(),
                P0AssetReviewPacket.EvaluateP0Packet(),
                P0RuntimeVisualBindingCoverage.EvaluateP0Bindings(),
                P0ChineseUiScaleEvidencePacket.EvaluateCurrentPacket(),
                P0PlayModeScreenshotFileEvidence.EvaluateP0Directory(),
                P0PlayModeEvidenceChecklist.EvaluateCurrent(),
                P0StarterCatFormalImportReadiness.EvaluateCurrentGate(),
                P0PlayModeReportFileEvidence.EvaluateCurrentReport());
        }

        public static P0VisualAcceptanceReport Evaluate(
            P0PlayableReadinessReport playableReadiness,
            P0AssetProductionReadinessReport assetProductionReadiness,
            P0AssetReviewPacketReport assetReviewPacket,
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings,
            P0ChineseUiScaleEvidencePacketReport chineseUiScaleEvidence,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence,
            P0PlayModeEvidenceReport playModeEvidence,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport,
            P0PlayModeReportFileEvidenceReport playModeReportFileEvidence = null)
        {
            return new P0VisualAcceptanceReport(
                playableReadiness,
                assetProductionReadiness,
                assetReviewPacket,
                runtimeVisualBindings,
                chineseUiScaleEvidence,
                screenshotFileEvidence,
                playModeEvidence,
                starterCatFormalImport,
                playModeReportFileEvidence);
        }
    }
}
