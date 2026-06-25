using System;
using System.Collections.Generic;
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
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            PlayableReadiness = playableReadiness;
            AssetProductionReadiness = assetProductionReadiness;
            AssetReviewPacket = assetReviewPacket;
            RuntimeVisualBindings = runtimeVisualBindings;
            ChineseUiScaleEvidence = chineseUiScaleEvidence;
            ScreenshotFileEvidence = screenshotFileEvidence;
            PlayModeEvidence = playModeEvidence;
            StarterCatFormalImport = starterCatFormalImport;
        }

        public P0PlayableReadinessReport PlayableReadiness { get; }

        public P0AssetProductionReadinessReport AssetProductionReadiness { get; }

        public P0AssetReviewPacketReport AssetReviewPacket { get; }

        public P0RuntimeVisualBindingCoverageReport RuntimeVisualBindings { get; }

        public P0ChineseUiScaleEvidencePacketReport ChineseUiScaleEvidence { get; }

        public P0PlayModeScreenshotFileEvidenceReport ScreenshotFileEvidence { get; }

        public P0PlayModeEvidenceReport PlayModeEvidence { get; }

        public P0StarterCatFormalImportReadinessReport StarterCatFormalImport { get; }

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
            && PlayModeEvidence != null
            && PlayModeEvidence.IsComplete
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
            builder.AppendLine("- Final P0 visual acceptance ready: " + YesNo(IsFinalP0VisualAcceptanceReady));
            builder.AppendLine("- Starter cat formal import allowed: " + YesNo(StarterCatFormalImport != null && StarterCatFormalImport.IsImportAllowed));
            builder.AppendLine("- Starter cat formal import state: " + (StarterCatFormalImport == null ? "missing" : StarterCatFormalImport.State.ToString()));
            builder.AppendLine("- Runtime visual bindings: " + Count(RuntimeVisualBindings == null ? 0 : RuntimeVisualBindings.BindingCount, P0VisualAssetCatalog.P0RuntimeVisualBindingCount));
            builder.AppendLine("- Resolved runtime textures: " + Count(RuntimeVisualBindings == null ? 0 : RuntimeVisualBindings.ResolvedTextureCount, P0VisualAssetCatalog.P0RuntimeVisualBindingCount));
            builder.AppendLine("- Chinese UI scale evidence packet: " + ChineseUiScaleEvidenceCount());
            builder.AppendLine("- Chinese UI scale evidence directory: " + (ChineseUiScaleEvidence == null ? "missing" : Escape(ChineseUiScaleEvidence.BatchDirectory)));
            builder.AppendLine("- Screenshot file evidence: " + ScreenshotEvidenceCount());
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

            if (PlayModeEvidence != null && !PlayModeEvidence.IsComplete)
            {
                hasAny = true;
                builder.AppendLine("- Complete Play Mode evidence checks before final P0 visual acceptance.");
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
                P0StarterCatFormalImportReadiness.EvaluateCurrentGate());
        }

        public static P0VisualAcceptanceReport Evaluate(
            P0PlayableReadinessReport playableReadiness,
            P0AssetProductionReadinessReport assetProductionReadiness,
            P0AssetReviewPacketReport assetReviewPacket,
            P0RuntimeVisualBindingCoverageReport runtimeVisualBindings,
            P0ChineseUiScaleEvidencePacketReport chineseUiScaleEvidence,
            P0PlayModeScreenshotFileEvidenceReport screenshotFileEvidence,
            P0PlayModeEvidenceReport playModeEvidence,
            P0StarterCatFormalImportReadinessReport starterCatFormalImport)
        {
            return new P0VisualAcceptanceReport(
                playableReadiness,
                assetProductionReadiness,
                assetReviewPacket,
                runtimeVisualBindings,
                chineseUiScaleEvidence,
                screenshotFileEvidence,
                playModeEvidence,
                starterCatFormalImport);
        }
    }
}
