using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StarterCatSourceLockPacketEvidenceTests
    {
        [Test]
        public void EvaluateCurrentPacket_CurrentPacketPinsStarterCatSourceLocks()
        {
            P0StarterCatSourceLockPacketReport report = P0StarterCatSourceLockPacketEvidence.EvaluateCurrentPacket();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.IsTrue(report.PacketMarkdownPresent);
            Assert.IsTrue(report.PacketCsvPresent);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount, report.CatEntryCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount, report.SourceHashMentionCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount, report.SpriteHashMentionCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount, report.ActiveScreenshotMentionCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedStarterCatCount, report.CandidateReviewSheetMentionCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount, report.CoreDocumentCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount, report.CoreDocumentSourcePathMentionCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount, report.CoreDocumentImportBlockMentionCount);
            Assert.AreEqual(0, report.CoreDocumentMojibakeMentionCount);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("turnaround hashes", report.BuildDetailedSummary());
            StringAssert.Contains("candidate review sheets", report.BuildDetailedSummary());
            StringAssert.Contains("core source-lock documents", report.BuildDetailedSummary());
            StringAssert.Contains("mojibake", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingPacketFilesBlocksReadiness()
        {
            P0StarterCatSourceLockPacketReport report = P0StarterCatSourceLockPacketEvidence.Evaluate(
                P0StarterCatSourceLockPacketEvidence.PacketMarkdownPath,
                P0StarterCatSourceLockPacketEvidence.PacketCsvPath,
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                MissingReader,
                _ => false);

            Assert.IsFalse(report.IsReady);
            Assert.IsFalse(report.PacketMarkdownPresent);
            Assert.IsFalse(report.PacketCsvPresent);
            StringAssert.Contains("markdown packet is missing", report.BuildDetailedSummary());
            StringAssert.Contains("CSV packet is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingSpriteHashBlocksReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry omitted = locks[0];

            P0StarterCatSourceLockPacketReport report = P0StarterCatSourceLockPacketEvidence.Evaluate(
                P0StarterCatSourceLockPacketEvidence.PacketMarkdownPath,
                P0StarterCatSourceLockPacketEvidence.PacketCsvPath,
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticPacketText(locks).Replace(omitted.SpriteSha256, string.Empty);
                    error = string.Empty;
                    return true;
                },
                _ => true);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(2, report.SpriteHashMentionCount);
            StringAssert.Contains("locked Unity sprite path or hash is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MojibakeCoreSourceLockDocumentFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            P0StarterCatSourceLockPacketReport report = P0StarterCatSourceLockPacketEvidence.Evaluate(
                P0StarterCatSourceLockPacketEvidence.PacketMarkdownPath,
                P0StarterCatSourceLockPacketEvidence.PacketCsvPath,
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticPacketText(locks);
                    if (path == P0StarterCatSourceLockPacketEvidence.StrictReferencePackPath)
                    {
                        text += "\ndesign/姊﹀鏀厤鑰呮牳蹇冪帺娉?assets/characters/broken.png";
                    }

                    error = string.Empty;
                    return true;
                },
                _ => true);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(1, report.CoreDocumentMojibakeMentionCount);
            StringAssert.Contains("contains mojibake design path text", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_CoreSourceLockDocumentsMustRepeatEveryExactSourcePath()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            string missingSourcePath = locks[2].SourceTurnaroundPath;

            P0StarterCatSourceLockPacketReport report = P0StarterCatSourceLockPacketEvidence.Evaluate(
                P0StarterCatSourceLockPacketEvidence.PacketMarkdownPath,
                P0StarterCatSourceLockPacketEvidence.PacketCsvPath,
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticPacketText(locks);
                    if (path == P0StarterCatSourceLockPacketEvidence.TurnaroundConformanceSpecPath)
                    {
                        text = text.Replace(missingSourcePath, "design/梦境支配者核心玩法/assets/characters/ch03_suzune_sleep_shrine_maiden/turnaround/wrong.png");
                    }

                    error = string.Empty;
                    return true;
                },
                _ => true);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(P0StarterCatSourceLockPacketEvidence.ExpectedCoreDocumentCount - 1, report.CoreDocumentSourcePathMentionCount);
            StringAssert.Contains("must mention all three exact colored-turnaround source paths", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentComparisonAudit_CurrentBatch69AuditIsReady()
        {
            P0StarterCatTurnaroundComparisonAuditReport report = P0StarterCatTurnaroundComparisonAuditEvidence.EvaluateCurrentAudit();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount, report.ArtifactCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.ManifestRowCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.SourceLockMentionCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.SourcePathMentionCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.SpritePathMentionCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.ActiveScreenshotMentionCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.RecommendationMentionCount);
            Assert.AreEqual(0, report.MetaFileCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("turnaround comparison audit ready", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateComparisonAudit_MissingReviewSheetFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            P0StarterCatTurnaroundComparisonAuditReport report = P0StarterCatTurnaroundComparisonAuditEvidence.Evaluate(
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticComparisonAuditText(path, locks);
                    error = string.Empty;
                    return true;
                },
                path => path != P0StarterCatTurnaroundComparisonAuditEvidence.ReviewSheetPath
                    && !path.EndsWith(".meta", StringComparison.Ordinal));

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount - 1, report.ArtifactCount);
            StringAssert.Contains("review sheet is missing", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateComparisonAudit_ImportApprovalTextFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            P0StarterCatTurnaroundComparisonAuditReport report = P0StarterCatTurnaroundComparisonAuditEvidence.Evaluate(
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticComparisonAuditText(path, locks);
                    if (path == P0StarterCatTurnaroundComparisonAuditEvidence.ReviewNotePath)
                    {
                        text = text.Replace("audit-only", "approved")
                            .Replace("Do not import into Unity yet", "Import into Unity now");
                    }

                    error = string.Empty;
                    return true;
                },
                _ => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("unsafe import or generation decision", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentReferencePlates_CurrentBatch70PlatesAreReady()
        {
            P0StarterCatReferencePlateReport report = P0StarterCatReferencePlateEvidence.EvaluateCurrentPlates();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedArtifactCount, report.ArtifactCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedPlateCount, report.PlateCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedPlateCount, report.ManifestRowCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedStarterCatCount, report.SourceLockMentionCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedStarterCatCount, report.SourcePathMentionCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedPlateCount, report.ViewRowCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedPlateCount, report.ImportBlockCount);
            Assert.AreEqual(0, report.MetaFileCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("reference plates ready", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateReferencePlates_MissingPlateFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            string missingPlate = P0StarterCatReferencePlateEvidence.ReferencePlatePathFor(locks[0].CatId, "front");

            P0StarterCatReferencePlateReport report = P0StarterCatReferencePlateEvidence.Evaluate(
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticReferencePlateText(path, locks);
                    error = string.Empty;
                    return true;
                },
                path => path != missingPlate && !path.EndsWith(".meta", StringComparison.Ordinal));

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedArtifactCount - 1, report.ArtifactCount);
            StringAssert.Contains("reference plates are incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateReferencePlates_ImportApprovalTextFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            P0StarterCatReferencePlateReport report = P0StarterCatReferencePlateEvidence.Evaluate(
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticReferencePlateText(path, locks);
                    if (path == P0StarterCatReferencePlateEvidence.ReviewNotePath)
                    {
                        text = text.Replace("reference-only", "approved")
                            .Replace("Do not import into Unity yet", "Import into Unity now");
                    }

                    error = string.Empty;
                    return true;
                },
                path => !path.EndsWith(".meta", StringComparison.Ordinal));

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("import status is unsafe", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateCurrentUnityReferenceInstall_CurrentBatch71To73InstallsAreReady()
        {
            P0StarterCatUnityReferenceInstallReport report = P0StarterCatUnityReferenceInstallEvidence.EvaluateCurrentInstall();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount, report.ArtifactCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount, report.InstalledAssetCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedManifestRowCount, report.ManifestRowCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedSourceLockMentionCount, report.SourceLockMentionCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedReferencePlateMentionCount, report.ReferencePlateMentionCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedImportSettingTokenCount, report.ImportSettingTokenCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedRuntimeBlockMentionCount, report.RuntimeBlockMentionCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            StringAssert.Contains("Unity reference install ready", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateUnityReferenceInstall_MissingAtlasFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            P0StarterCatUnityReferenceInstallReport report = P0StarterCatUnityReferenceInstallEvidence.Evaluate(
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticUnityReferenceInstallText(path, locks);
                    error = string.Empty;
                    return true;
                },
                path => path != P0StarterCatUnityReferenceInstallEvidence.InstalledAtlasPath);

            Assert.IsFalse(report.IsReady);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount - 1, report.ArtifactCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount - 1, report.InstalledAssetCount);
            StringAssert.Contains("artifacts are incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void EvaluateUnityReferenceInstall_RuntimeReplacementTextFailsReadiness()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();

            P0StarterCatUnityReferenceInstallReport report = P0StarterCatUnityReferenceInstallEvidence.Evaluate(
                locks,
                (string path, out string text, out string error) =>
                {
                    text = BuildSyntheticUnityReferenceInstallText(path, locks);
                    if (path == P0StarterCatUnityReferenceInstallEvidence.ReviewNotePath)
                    {
                        text = "runtime-bound replacement. Formal starter-cat body-art import approved. AI-generated cat body imported.";
                    }

                    error = string.Empty;
                    return true;
                },
                _ => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("does not clearly block runtime replacement", report.BuildDetailedSummary());
        }

        private static bool MissingReader(string path, out string text, out string error)
        {
            text = string.Empty;
            error = "missing";
            return false;
        }

        private static string BuildSyntheticPacketText(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            List<string> lines = new List<string>
            {
                "colored turnaround human palette do not import into Unity yet"
            };

            for (int i = 0; i < locks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = locks[i];
                lines.Add(current.CatId);
                lines.Add(current.CatId + "_turnaround_colored");
                lines.Add(current.SourceTurnaroundPath);
                lines.Add(current.SourceTurnaroundSha256);
                lines.Add(current.SpritePath);
                lines.Add(current.SpriteSha256);
                lines.Add(P0StarterCatSourceLockPacketEvidence.ActiveScreenshotFor(current.CatId));
                lines.Add(P0StarterCatSourceLockPacketEvidence.CandidateReviewSheetFor(current.CatId));
            }

            return string.Join("\n", lines);
        }

        private static string BuildSyntheticComparisonAuditText(
            string path,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            List<string> lines = new List<string>
            {
                "cat_id,source_lock_id,source_turnaround_path,current_sprite_path,active_screenshot_target,recommendation"
            };

            for (int i = 0; i < locks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = locks[i];
                lines.Add(current.CatId
                    + ","
                    + current.CatId
                    + "_turnaround_colored,"
                    + current.SourceTurnaroundPath
                    + ","
                    + current.SpritePath
                    + ","
                    + P0StarterCatSourceLockPacketEvidence.ActiveScreenshotFor(current.CatId)
                    + ",audit_only_no_import_pending_active_cat_playmode_screenshot");
            }

            if (path == P0StarterCatTurnaroundComparisonAuditEvidence.ManifestPath)
            {
                return string.Join("\n", lines);
            }

            lines.Clear();

            if (path == P0StarterCatTurnaroundComparisonAuditEvidence.ReviewNotePath)
            {
                lines.Add("audit-only");
                lines.Add("Do not import into Unity yet");
                lines.Add("active-cat Play Mode screenshot");
                lines.Add("colored three-view turnaround");
            }
            else if (path == P0StarterCatTurnaroundComparisonAuditEvidence.ProcessNotePath)
            {
                lines.Add("No image generation was performed");
            }

            for (int i = 0; i < locks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = locks[i];
                lines.Add(current.CatId + "_turnaround_colored");
                lines.Add(current.SourceTurnaroundPath);
                lines.Add(current.SpritePath);
                lines.Add(P0StarterCatSourceLockPacketEvidence.ActiveScreenshotFor(current.CatId));
            }

            return string.Join("\n", lines);
        }

        private static string BuildSyntheticReferencePlateText(
            string path,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            string[] views =
            {
                "front",
                "side",
                "back"
            };

            List<string> lines = new List<string>();
            if (path == P0StarterCatReferencePlateEvidence.ManifestPath)
            {
                lines.Add("cat_id,view,source_lock_id,source_turnaround_path,reference_plate_path,import_status");
                for (int i = 0; i < locks.Count; i++)
                {
                    P0StarterCatTurnaroundSourceLockEntry current = locks[i];
                    for (int viewIndex = 0; viewIndex < views.Length; viewIndex++)
                    {
                        string view = views[viewIndex];
                        lines.Add(current.CatId
                            + ","
                            + view
                            + ","
                            + current.CatId
                            + "_turnaround_colored,"
                            + current.SourceTurnaroundPath
                            + ","
                            + P0StarterCatReferencePlateEvidence.ReferencePlatePathFor(current.CatId, view)
                            + ",reference_only_do_not_import_pending_active_cat_playmode_screenshot");
                    }
                }

                return string.Join("\n", lines);
            }

            if (path == P0StarterCatReferencePlateEvidence.ReviewNotePath)
            {
                lines.Add("reference-only");
                lines.Add("Do not import into Unity yet");
                lines.Add("no image generation");
                lines.Add("deterministic crop");
                lines.Add("body proportion face markings palette costume props civilization motifs");
                lines.Add("active-cat Play Mode screenshot");
            }
            else if (path == P0StarterCatReferencePlateEvidence.ProcessNotePath)
            {
                lines.Add("No image generation was performed");
            }

            for (int i = 0; i < locks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = locks[i];
                lines.Add(current.CatId + "_turnaround_colored");
                lines.Add(current.SourceTurnaroundPath);
                for (int viewIndex = 0; viewIndex < views.Length; viewIndex++)
                {
                    lines.Add(P0StarterCatReferencePlateEvidence.ReferencePlatePathFor(current.CatId, views[viewIndex]));
                }
            }

            return string.Join("\n", lines);
        }

        private static string BuildSyntheticUnityReferenceInstallText(
            string path,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            P0StarterCatUnityReferenceInstallEntry install = P0StarterCatUnityReferenceInstallEvidence.FindInstall(path);
            if (path.EndsWith(".meta", StringComparison.Ordinal))
            {
                return "TextureImporter:\n"
                    + "  mipmaps:\n"
                    + "    enableMipMap: 0\n"
                    + "  textureType: 8\n"
                    + "  spriteMode: 1\n"
                    + "  alphaIsTransparency: 1\n"
                    + "  userData: TheCatP0ImportSettings:v1\n";
            }

            List<string> lines = new List<string>();
            if (path.EndsWith("_manifest.csv", StringComparison.Ordinal))
            {
                lines.Add("cat_id,batch_slug,asset_id,asset_type,unity_import_path,unity_meta_path,source_lock_id,source_turnaround_path,locked_combat_sprite_path,recommendation");
                IReadOnlyList<P0StarterCatUnityReferenceInstallEntry> installs = P0StarterCatUnityReferenceInstallEvidence.CreateP0Installs();
                for (int i = 0; i < installs.Count; i++)
                {
                    P0StarterCatUnityReferenceInstallEntry current = installs[i];
                    P0StarterCatTurnaroundSourceLockEntry locked = FindLock(locks, current.CatId);
                    lines.Add(current.CatId
                        + ","
                        + current.BatchDirectory
                        + ","
                        + current.AssetId
                        + ","
                        + P0StarterCatUnityReferenceInstallEvidence.AssetType
                        + ","
                        + current.InstalledAtlasPath
                        + ","
                        + current.InstalledMetaPath
                        + ","
                        + current.SourceLockId
                        + ","
                        + locked.SourceTurnaroundPath
                        + ","
                        + locked.SpritePath
                        + ","
                        + current.Recommendation);
                }

                return string.Join("\n", lines);
            }

            if (path == install.ReviewNotePath)
            {
                lines.Add("source-derived");
                lines.Add("not runtime-bound");
                lines.Add("does not replace");
                lines.Add("Formal starter-cat body-art import remains blocked");
                lines.Add("No AI-generated cat body");
                lines.Add("Unity visual smoke remains pending");
            }
            else if (path == install.ProcessNotePath)
            {
                lines.Add("No image generation was performed");
                lines.Add("deterministically from Batch 70");
            }

            P0StarterCatTurnaroundSourceLockEntry sourceLock = FindLock(locks, install.CatId);
            lines.Add(install.SourceLockId);
            lines.Add(sourceLock.SourceTurnaroundPath);
            lines.Add(sourceLock.SpritePath);
            lines.Add(P0StarterCatUnityReferenceInstallEvidence.ReferencePlatePathFor(install.CatId, "front"));
            lines.Add(P0StarterCatUnityReferenceInstallEvidence.ReferencePlatePathFor(install.CatId, "side"));
            lines.Add(P0StarterCatUnityReferenceInstallEvidence.ReferencePlatePathFor(install.CatId, "back"));
            return string.Join("\n", lines);
        }

        private static P0StarterCatTurnaroundSourceLockEntry FindLock(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            string catId)
        {
            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].CatId == catId)
                {
                    return locks[i];
                }
            }

            return default(P0StarterCatTurnaroundSourceLockEntry);
        }
    }
}
