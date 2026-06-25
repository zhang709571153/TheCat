using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public delegate bool P0StarterCatHashReader(string path, out string sha256, out string error);
    public delegate bool P0StarterCatPromptTextReader(string path, out string text, out string error);

    public enum P0StarterCatTurnaroundSourceLockSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatTurnaroundSourceLockIssue
    {
        public P0StarterCatTurnaroundSourceLockIssue(P0StarterCatTurnaroundSourceLockSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatTurnaroundSourceLockSeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0StarterCatTurnaroundSourceLockEntry
    {
        public P0StarterCatTurnaroundSourceLockEntry(
            string catId,
            string assetId,
            string sourceTurnaroundPath,
            string sourceTurnaroundSha256,
            string spritePath,
            string spriteSha256)
        {
            CatId = catId ?? string.Empty;
            AssetId = assetId ?? string.Empty;
            SourceTurnaroundPath = sourceTurnaroundPath ?? string.Empty;
            SourceTurnaroundSha256 = NormalizeHash(sourceTurnaroundSha256);
            SpritePath = spritePath ?? string.Empty;
            SpriteSha256 = NormalizeHash(spriteSha256);
        }

        public string CatId { get; }

        public string AssetId { get; }

        public string SourceTurnaroundPath { get; }

        public string SourceTurnaroundSha256 { get; }

        public string SpritePath { get; }

        public string SpriteSha256 { get; }

        private static string NormalizeHash(string value)
        {
            return (value ?? string.Empty).Trim().ToLowerInvariant();
        }
    }

    public readonly struct P0StarterCatVisualConsistencyChecklistEntry
    {
        private readonly IReadOnlyList<string> requiredTraits;

        public P0StarterCatVisualConsistencyChecklistEntry(
            string catId,
            string displayName,
            string sourceLockId,
            string sourceTurnaroundPath,
            string spritePath,
            string playModeScreenshotFileName,
            IReadOnlyList<string> requiredTraits,
            string prohibitedDriftRule)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            SourceLockId = sourceLockId ?? string.Empty;
            SourceTurnaroundPath = sourceTurnaroundPath ?? string.Empty;
            SpritePath = spritePath ?? string.Empty;
            PlayModeScreenshotFileName = playModeScreenshotFileName ?? string.Empty;
            this.requiredTraits = requiredTraits == null
                ? Array.Empty<string>()
                : new List<string>(requiredTraits).AsReadOnly();
            ProhibitedDriftRule = prohibitedDriftRule ?? string.Empty;
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string SourceLockId { get; }

        public string SourceTurnaroundPath { get; }

        public string SpritePath { get; }

        public string PlayModeScreenshotFileName { get; }

        public IReadOnlyList<string> RequiredTraits => requiredTraits ?? Array.Empty<string>();

        public string ProhibitedDriftRule { get; }

        public bool IsComplete => !string.IsNullOrWhiteSpace(CatId)
            && !string.IsNullOrWhiteSpace(DisplayName)
            && !string.IsNullOrWhiteSpace(SourceLockId)
            && !string.IsNullOrWhiteSpace(SourceTurnaroundPath)
            && !string.IsNullOrWhiteSpace(SpritePath)
            && !string.IsNullOrWhiteSpace(PlayModeScreenshotFileName)
            && RequiredTraits.Count >= P0StarterCatVisualConsistencyChecklist.MinimumRequiredTraitsPerCat
            && !string.IsNullOrWhiteSpace(ProhibitedDriftRule);
    }

    public enum P0StarterCatVisualConsistencySeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatVisualConsistencyIssue
    {
        public P0StarterCatVisualConsistencyIssue(P0StarterCatVisualConsistencySeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatVisualConsistencySeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatVisualConsistencyReport
    {
        private readonly List<P0StarterCatVisualConsistencyIssue> issues = new List<P0StarterCatVisualConsistencyIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatVisualConsistencyIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int ChecklistCount { get; private set; }

        public int RequiredTraitCount { get; private set; }

        public int ScreenshotPlanMatchedCount { get; private set; }

        public int SourceLockMatchedCount { get; private set; }

        public int ExistingReferenceFileCount { get; private set; }

        public int FailureCount => Count(P0StarterCatVisualConsistencySeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatVisualConsistencyChecklist.ExpectedCoveredCheckCount;

        public void SetCounts(
            int checklistCount,
            int requiredTraitCount,
            int screenshotPlanMatchedCount,
            int sourceLockMatchedCount,
            int existingReferenceFileCount)
        {
            ChecklistCount = checklistCount;
            RequiredTraitCount = requiredTraitCount;
            ScreenshotPlanMatchedCount = screenshotPlanMatchedCount;
            SourceLockMatchedCount = sourceLockMatchedCount;
            ExistingReferenceFileCount = existingReferenceFileCount;
        }

        public void AddIssue(P0StarterCatVisualConsistencySeverity severity, string message)
        {
            issues.Add(new P0StarterCatVisualConsistencyIssue(severity, message));
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
            return IsReady
                ? "P0 starter cat visual consistency checklist ready for " + ChecklistCount + " cat(s) and " + RequiredTraitCount + " required trait(s)."
                : "P0 starter cat visual consistency checklist has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Checklist cats: " + ChecklistCount,
                "Required traits: " + RequiredTraitCount,
                "Screenshot plan matches: " + ScreenshotPlanMatchedCount,
                "Source-lock matches: " + SourceLockMatchedCount,
                "Existing reference files: " + ExistingReferenceFileCount
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

        private int Count(P0StarterCatVisualConsistencySeverity severity)
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

    public static class P0StarterCatVisualConsistencyChecklist
    {
        public const int ExpectedCoveredCheckCount = 8;
        public const int MinimumRequiredTraitsPerCat = 5;

        public static IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> CreateP0Checklist()
        {
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks = P0StarterCatTurnaroundSourceLocks.CreateP0Locks();
            P0StarterCatTurnaroundSourceLockEntry saiban = FindLock(locks, P0PrototypeCatalog.SaibanId);
            P0StarterCatTurnaroundSourceLockEntry nephthys = FindLock(locks, P0PrototypeCatalog.NephthysId);
            P0StarterCatTurnaroundSourceLockEntry suzune = FindLock(locks, P0PrototypeCatalog.SuzuneId);

            return new[]
            {
                new P0StarterCatVisualConsistencyChecklistEntry(
                    P0PrototypeCatalog.SaibanId,
                    "Saiban",
                    "saiban_turnaround_colored",
                    saiban.SourceTurnaroundPath,
                    saiban.SpritePath,
                    P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName,
                    new[]
                    {
                        "silver-blue armored non-human cat proportions",
                        "front-view tabby face markings",
                        "oath shield silhouette",
                        "sword silhouette",
                        "cape and helm read from colored turnaround"
                    },
                    "Block if Saiban drifts from the colored turnaround into human knight proportions or loses shield, sword, tabby face, cape, or helm identity."),
                new P0StarterCatVisualConsistencyChecklistEntry(
                    P0PrototypeCatalog.NephthysId,
                    "Nephthys",
                    "nephthys_turnaround_colored",
                    nephthys.SourceTurnaroundPath,
                    nephthys.SpritePath,
                    P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName,
                    new[]
                    {
                        "hooded non-human cat body",
                        "moon-sand Egyptian motif read",
                        "floating pyramid / obelisk prop silhouette",
                        "gold and blue palette from colored turnaround",
                        "dream-script controller identity"
                    },
                    "Block if Nephthys drifts from the colored turnaround into Cleopatra costume cliche, human body language, or loses hood, pyramid/obelisk, gold-blue palette, or moon-sand motifs."),
                new P0StarterCatVisualConsistencyChecklistEntry(
                    P0PrototypeCatalog.SuzuneId,
                    "Suzune",
                    "suzune_turnaround_colored",
                    suzune.SourceTurnaroundPath,
                    suzune.SpritePath,
                    P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName,
                    new[]
                    {
                        "calico markings from colored turnaround",
                        "shrine outfit on non-human cat body",
                        "bell ornaments",
                        "wand / branch healer silhouette",
                        "vermilion, warm white, and moon-blue healer palette"
                    },
                    "Block if Suzune drifts from the colored turnaround into human shrine maiden proportions or loses calico markings, shrine outfit, bells, wand/branch, or healer palette.")
            };
        }

        public static P0StarterCatVisualConsistencyReport EvaluateP0Checklist()
        {
            return Evaluate(
                CreateP0Checklist(),
                P0StarterCatTurnaroundSourceLocks.CreateP0Locks(),
                DefaultFileExists);
        }

        public static P0StarterCatVisualConsistencyReport Evaluate(
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            Func<string, bool> fileExists)
        {
            P0StarterCatVisualConsistencyReport report = new P0StarterCatVisualConsistencyReport();
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            int checklistCount = checklist == null ? 0 : checklist.Count;
            int requiredTraitCount = 0;
            int screenshotPlanMatchedCount = 0;
            int sourceLockMatchedCount = 0;
            int existingReferenceFileCount = 0;
            bool allExpectedCatsPresent = true;
            bool allEntriesComplete = true;
            bool allSourceLocksMatch = true;
            bool allReferenceFilesExist = true;
            bool allScreenshotsMatchPlan = true;
            bool allRequiredTraitListsReady = true;
            bool allDriftRulesReady = true;
            HashSet<string> uniqueCats = new HashSet<string>(StringComparer.Ordinal);

            if (checklist == null)
            {
                report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, "Starter cat visual consistency checklist is missing.");
                report.SetCounts(0, 0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < checklist.Count; i++)
            {
                P0StarterCatVisualConsistencyChecklistEntry entry = checklist[i];
                if (!IsExpectedStarterCat(entry.CatId) || !uniqueCats.Add(entry.CatId))
                {
                    allExpectedCatsPresent = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.CatId + " is not a unique P0 starter cat checklist entry.");
                }

                if (!entry.IsComplete)
                {
                    allEntriesComplete = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " checklist entry is incomplete.");
                }

                requiredTraitCount += entry.RequiredTraits.Count;
                if (!RequiredTraitsReady(entry))
                {
                    allRequiredTraitListsReady = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " checklist is missing required colored-turnaround visual traits.");
                }

                if (!DriftRuleReady(entry))
                {
                    allDriftRulesReady = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " drift rule must mention colored turnaround and human-proportion drift blockers.");
                }

                if (SourceLockMatches(entry, locks))
                {
                    sourceLockMatchedCount++;
                }
                else
                {
                    allSourceLocksMatch = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " checklist does not match the locked colored-turnaround source paths.");
                }

                if (exists(entry.SourceTurnaroundPath))
                {
                    existingReferenceFileCount++;
                }
                else
                {
                    allReferenceFilesExist = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " colored turnaround source is missing: " + entry.SourceTurnaroundPath + ".");
                }

                if (exists(entry.SpritePath))
                {
                    existingReferenceFileCount++;
                }
                else
                {
                    allReferenceFilesExist = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " locked sprite is missing: " + entry.SpritePath + ".");
                }

                if (ScreenshotPlanMatches(entry))
                {
                    screenshotPlanMatchedCount++;
                }
                else
                {
                    allScreenshotsMatchPlan = false;
                    report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, entry.DisplayName + " checklist screenshot must match the registered active-cat Play Mode capture.");
                }
            }

            allExpectedCatsPresent &= checklistCount == 3
                && uniqueCats.Contains(P0PrototypeCatalog.SaibanId)
                && uniqueCats.Contains(P0PrototypeCatalog.NephthysId)
                && uniqueCats.Contains(P0PrototypeCatalog.SuzuneId);

            bool contactSheetExists = exists(P0AssetProductionReadiness.StarterCatTurnaroundContactSheetPath);

            Require(report, allExpectedCatsPresent, "Starter cat visual consistency checklist covers exactly Saiban, Nephthys, and Suzune.", "Starter cat visual consistency checklist must cover exactly the three P0 starter cats.");
            Require(report, allEntriesComplete, "Each starter cat checklist entry declares source lock, sprite path, screenshot, traits, and drift blocker.", "One or more starter cat checklist entries are incomplete.");
            Require(report, allSourceLocksMatch, "Starter cat visual checklist entries match locked colored-turnaround source paths and sprite paths.", "One or more starter cat visual checklist entries drifted from source locks.");
            Require(report, allReferenceFilesExist, "Starter cat visual checklist source turnarounds and locked sprites exist on disk.", "One or more starter cat visual checklist files are missing.");
            Require(report, allScreenshotsMatchPlan, "Starter cat visual checklist points at the registered active-cat Play Mode screenshots.", "Starter cat visual checklist screenshots do not match the Play Mode capture plan.");
            Require(report, allRequiredTraitListsReady && requiredTraitCount >= 15, "Starter cat checklist requires at least 15 colored-turnaround visual traits across the three cats.", "Starter cat checklist has too few colored-turnaround visual traits.");
            Require(report, allDriftRulesReady, "Starter cat checklist blocks colored-turnaround drift and human-proportion drift.", "Starter cat checklist drift blockers are incomplete.");
            Require(report, contactSheetExists, "Starter cat colored-turnaround contact sheet exists for human visual review.", "Starter cat colored-turnaround contact sheet is missing.");

            report.SetCounts(checklistCount, requiredTraitCount, screenshotPlanMatchedCount, sourceLockMatchedCount, existingReferenceFileCount);
            return report;
        }

        private static bool SourceLockMatches(P0StarterCatVisualConsistencyChecklistEntry entry, IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            P0StarterCatTurnaroundSourceLockEntry locked = FindLock(locks, entry.CatId);
            return locked.CatId == entry.CatId
                && entry.SourceLockId == entry.CatId + "_turnaround_colored"
                && entry.SourceTurnaroundPath == locked.SourceTurnaroundPath
                && entry.SpritePath == locked.SpritePath;
        }

        private static P0StarterCatTurnaroundSourceLockEntry FindLock(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks, string catId)
        {
            if (locks == null)
            {
                return default(P0StarterCatTurnaroundSourceLockEntry);
            }

            for (int i = 0; i < locks.Count; i++)
            {
                if (locks[i].CatId == catId)
                {
                    return locks[i];
                }
            }

            return default(P0StarterCatTurnaroundSourceLockEntry);
        }

        private static bool RequiredTraitsReady(P0StarterCatVisualConsistencyChecklistEntry entry)
        {
            if (entry.RequiredTraits.Count < MinimumRequiredTraitsPerCat)
            {
                return false;
            }

            switch (entry.CatId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return TraitsMention(entry, "shield")
                        && TraitsMention(entry, "sword")
                        && TraitsMention(entry, "tabby")
                        && TraitsMention(entry, "proportions");
                case P0PrototypeCatalog.NephthysId:
                    return TraitsMention(entry, "hood")
                        && TraitsMention(entry, "pyramid")
                        && TraitsMention(entry, "palette")
                        && TraitsMention(entry, "non-human");
                case P0PrototypeCatalog.SuzuneId:
                    return TraitsMention(entry, "calico")
                        && TraitsMention(entry, "shrine")
                        && TraitsMention(entry, "bell")
                        && TraitsMention(entry, "healer");
                default:
                    return false;
            }
        }

        private static bool DriftRuleReady(P0StarterCatVisualConsistencyChecklistEntry entry)
        {
            return ContainsText(entry.ProhibitedDriftRule, "colored turnaround")
                && ContainsText(entry.ProhibitedDriftRule, "human");
        }

        private static bool TraitsMention(P0StarterCatVisualConsistencyChecklistEntry entry, string token)
        {
            for (int i = 0; i < entry.RequiredTraits.Count; i++)
            {
                if (ContainsText(entry.RequiredTraits[i], token))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ScreenshotPlanMatches(P0StarterCatVisualConsistencyChecklistEntry entry)
        {
            string expected = ExpectedScreenshot(entry.CatId);
            return entry.PlayModeScreenshotFileName == expected
                && ContainsExact(P0PlayModeScreenshotSmoke.ExpectedCaptureFileNames, entry.PlayModeScreenshotFileName);
        }

        private static string ExpectedScreenshot(string catId)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName;
                case P0PrototypeCatalog.NephthysId:
                    return P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName;
                case P0PrototypeCatalog.SuzuneId:
                    return P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName;
                default:
                    return string.Empty;
            }
        }

        private static bool ContainsExact(IReadOnlyList<string> values, string expected)
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

        private static bool IsExpectedStarterCat(string catId)
        {
            return catId == P0PrototypeCatalog.SaibanId
                || catId == P0PrototypeCatalog.NephthysId
                || catId == P0PrototypeCatalog.SuzuneId;
        }

        private static bool ContainsText(string value, string token)
        {
            return value != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (File.Exists(path))
            {
                return true;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return true;
                }

                current = current.Parent;
            }

            return false;
        }

        private static void Require(P0StarterCatVisualConsistencyReport report, bool condition, string coveredCheck, string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatVisualConsistencySeverity.Failure, failureMessage);
        }
    }

    public readonly struct P0StarterCatTurnaroundConformanceSpecEntry
    {
        private readonly IReadOnlyList<string> frontViewAnchors;
        private readonly IReadOnlyList<string> sideViewAnchors;
        private readonly IReadOnlyList<string> backViewAnchors;
        private readonly IReadOnlyList<string> paletteAnchors;
        private readonly IReadOnlyList<string> propAndCostumeAnchors;
        private readonly IReadOnlyList<string> prohibitedDriftRules;

        public P0StarterCatTurnaroundConformanceSpecEntry(
            string catId,
            string displayName,
            string sourceLockId,
            string sourceTurnaroundPath,
            IReadOnlyList<string> frontViewAnchors,
            IReadOnlyList<string> sideViewAnchors,
            IReadOnlyList<string> backViewAnchors,
            IReadOnlyList<string> paletteAnchors,
            IReadOnlyList<string> propAndCostumeAnchors,
            IReadOnlyList<string> prohibitedDriftRules)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            SourceLockId = sourceLockId ?? string.Empty;
            SourceTurnaroundPath = sourceTurnaroundPath ?? string.Empty;
            this.frontViewAnchors = CopyList(frontViewAnchors);
            this.sideViewAnchors = CopyList(sideViewAnchors);
            this.backViewAnchors = CopyList(backViewAnchors);
            this.paletteAnchors = CopyList(paletteAnchors);
            this.propAndCostumeAnchors = CopyList(propAndCostumeAnchors);
            this.prohibitedDriftRules = CopyList(prohibitedDriftRules);
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string SourceLockId { get; }

        public string SourceTurnaroundPath { get; }

        public IReadOnlyList<string> FrontViewAnchors => frontViewAnchors ?? Array.Empty<string>();

        public IReadOnlyList<string> SideViewAnchors => sideViewAnchors ?? Array.Empty<string>();

        public IReadOnlyList<string> BackViewAnchors => backViewAnchors ?? Array.Empty<string>();

        public IReadOnlyList<string> PaletteAnchors => paletteAnchors ?? Array.Empty<string>();

        public IReadOnlyList<string> PropAndCostumeAnchors => propAndCostumeAnchors ?? Array.Empty<string>();

        public IReadOnlyList<string> ProhibitedDriftRules => prohibitedDriftRules ?? Array.Empty<string>();

        public bool IsComplete => !string.IsNullOrWhiteSpace(CatId)
            && !string.IsNullOrWhiteSpace(DisplayName)
            && !string.IsNullOrWhiteSpace(SourceLockId)
            && !string.IsNullOrWhiteSpace(SourceTurnaroundPath)
            && FrontViewAnchors.Count >= P0StarterCatTurnaroundConformanceSpec.MinimumAnchorsPerView
            && SideViewAnchors.Count >= P0StarterCatTurnaroundConformanceSpec.MinimumAnchorsPerView
            && BackViewAnchors.Count >= P0StarterCatTurnaroundConformanceSpec.MinimumAnchorsPerView
            && PaletteAnchors.Count >= P0StarterCatTurnaroundConformanceSpec.MinimumPaletteAnchorsPerCat
            && PropAndCostumeAnchors.Count >= P0StarterCatTurnaroundConformanceSpec.MinimumPropAnchorsPerCat
            && ProhibitedDriftRules.Count >= P0StarterCatTurnaroundConformanceSpec.MinimumDriftRulesPerCat;

        private static IReadOnlyList<string> CopyList(IReadOnlyList<string> values)
        {
            return values == null ? Array.Empty<string>() : new List<string>(values).AsReadOnly();
        }
    }

    public enum P0StarterCatTurnaroundConformanceSpecSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatTurnaroundConformanceSpecIssue
    {
        public P0StarterCatTurnaroundConformanceSpecIssue(P0StarterCatTurnaroundConformanceSpecSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatTurnaroundConformanceSpecSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatTurnaroundConformanceSpecReport
    {
        private readonly List<P0StarterCatTurnaroundConformanceSpecIssue> issues = new List<P0StarterCatTurnaroundConformanceSpecIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatTurnaroundConformanceSpecIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int SpecCount { get; private set; }

        public int SourceLockMatchedCount { get; private set; }

        public int ExistingSourceFileCount { get; private set; }

        public int FrontViewAnchorCount { get; private set; }

        public int SideViewAnchorCount { get; private set; }

        public int BackViewAnchorCount { get; private set; }

        public int PaletteAnchorCount { get; private set; }

        public int PropAndCostumeAnchorCount { get; private set; }

        public int ProhibitedDriftRuleCount { get; private set; }

        public int FailureCount => Count(P0StarterCatTurnaroundConformanceSpecSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatTurnaroundConformanceSpec.ExpectedCoveredCheckCount;

        public void SetCounts(
            int specCount,
            int sourceLockMatchedCount,
            int existingSourceFileCount,
            int frontViewAnchorCount,
            int sideViewAnchorCount,
            int backViewAnchorCount,
            int paletteAnchorCount,
            int propAndCostumeAnchorCount,
            int prohibitedDriftRuleCount)
        {
            SpecCount = specCount;
            SourceLockMatchedCount = sourceLockMatchedCount;
            ExistingSourceFileCount = existingSourceFileCount;
            FrontViewAnchorCount = frontViewAnchorCount;
            SideViewAnchorCount = sideViewAnchorCount;
            BackViewAnchorCount = backViewAnchorCount;
            PaletteAnchorCount = paletteAnchorCount;
            PropAndCostumeAnchorCount = propAndCostumeAnchorCount;
            ProhibitedDriftRuleCount = prohibitedDriftRuleCount;
        }

        public void AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity severity, string message)
        {
            issues.Add(new P0StarterCatTurnaroundConformanceSpecIssue(severity, message));
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
            return IsReady
                ? "P0 starter cat turnaround conformance spec ready for " + SpecCount + " cat(s), " + (FrontViewAnchorCount + SideViewAnchorCount + BackViewAnchorCount) + " view anchor(s), and " + ProhibitedDriftRuleCount + " drift rule(s)."
                : "P0 starter cat turnaround conformance spec has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Spec cats: " + SpecCount,
                "Source-lock matches: " + SourceLockMatchedCount,
                "Existing source files: " + ExistingSourceFileCount,
                "Front-view anchors: " + FrontViewAnchorCount,
                "Side-view anchors: " + SideViewAnchorCount,
                "Back-view anchors: " + BackViewAnchorCount,
                "Palette anchors: " + PaletteAnchorCount,
                "Prop/costume anchors: " + PropAndCostumeAnchorCount,
                "Prohibited drift rules: " + ProhibitedDriftRuleCount
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

        private int Count(P0StarterCatTurnaroundConformanceSpecSeverity severity)
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

    public static class P0StarterCatTurnaroundConformanceSpec
    {
        public const int ExpectedCoveredCheckCount = 9;
        public const int MinimumAnchorsPerView = 3;
        public const int MinimumPaletteAnchorsPerCat = 3;
        public const int MinimumPropAnchorsPerCat = 3;
        public const int MinimumDriftRulesPerCat = 4;

        public static IReadOnlyList<P0StarterCatTurnaroundConformanceSpecEntry> CreateP0Spec()
        {
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist = P0StarterCatVisualConsistencyChecklist.CreateP0Checklist();
            P0StarterCatVisualConsistencyChecklistEntry saiban = FindChecklistEntry(checklist, P0PrototypeCatalog.SaibanId);
            P0StarterCatVisualConsistencyChecklistEntry nephthys = FindChecklistEntry(checklist, P0PrototypeCatalog.NephthysId);
            P0StarterCatVisualConsistencyChecklistEntry suzune = FindChecklistEntry(checklist, P0PrototypeCatalog.SuzuneId);

            return new[]
            {
                new P0StarterCatTurnaroundConformanceSpecEntry(
                    P0PrototypeCatalog.SaibanId,
                    "Saiban",
                    saiban.SourceLockId,
                    saiban.SourceTurnaroundPath,
                    new[]
                    {
                        "front view silver-gray tabby face stripes and pale green eyes",
                        "front view red torn cape collar over silver-gold armor",
                        "front view round sun shield on the left side and single sword on the right side"
                    },
                    new[]
                    {
                        "side view compact non-human cat muzzle and upright ears",
                        "side view red cape trails behind armor with striped tail visible",
                        "side view shield disk and angled sword silhouette remain readable"
                    },
                    new[]
                    {
                        "back view gray tabby head stripes and rounded cat head",
                        "back view torn red cape covers armor with dark holes along the lower edge",
                        "back view striped tail sits below the cape with sword silhouette at the side"
                    },
                    new[]
                    {
                        "silver-gray fur with darker tabby stripes",
                        "deep red cape cloth",
                        "silver armor with gold trim and blue gem accents"
                    },
                    new[]
                    {
                        "round sun oath shield",
                        "single straight sword",
                        "silver-gold armor, helm read, belt, and torn cape"
                    },
                    new[]
                    {
                        "Reject generated-lineup or generic knight-cat drift over the colored three-view turnaround.",
                        "Reject human knight torso, long human legs, or biped costume posture.",
                        "Reject palette drift away from silver-gray fur, red cape, silver armor, gold trim, and blue gems.",
                        "Reject missing front, side, or back anchors including shield, sword, cape, tabby face, and striped tail."
                    }),
                new P0StarterCatTurnaroundConformanceSpecEntry(
                    P0PrototypeCatalog.NephthysId,
                    "Nephthys",
                    nephthys.SourceLockId,
                    nephthys.SourceTurnaroundPath,
                    new[]
                    {
                        "front view gold-brown tabby face, large golden eyes, and dark blue hood",
                        "front view crescent hood ornament with blue tear gem and gold script border",
                        "front view floating pyramid over inverted obelisk prop beside raised paw"
                    },
                    new[]
                    {
                        "side view hood volume wraps the cat head while ears stay visible",
                        "side view blue cloak layers and gold script trim sweep behind the compact body",
                        "side view floating pyramid/obelisk prop remains in front of the paw"
                    },
                    new[]
                    {
                        "back view dark blue hood and cloak with centered vertical gold script strip",
                        "back view winged blue gem and ankh emblem on the shoulder mantle",
                        "back view split cloak exposes gold-brown striped tail"
                    },
                    new[]
                    {
                        "gold-brown tabby fur",
                        "deep navy cloak and hood",
                        "sand-gold trim with blue gems and cyan magic particles"
                    },
                    new[]
                    {
                        "floating pyramid over inverted obelisk controller prop",
                        "crescent moon hood ornament and blue teardrop gem",
                        "gold script trim, ankh symbol, winged chest and back jewel"
                    },
                    new[]
                    {
                        "Reject generated-lineup or generic Egyptian fantasy drift over the colored three-view turnaround.",
                        "Reject Cleopatra costume cliche, human body language, or human robe posture.",
                        "Reject palette drift away from gold-brown fur, deep navy cloth, sand-gold trim, and blue gems.",
                        "Reject missing front, side, or back anchors including hood, script trim, pyramid prop, ankh, and striped tail."
                    }),
                new P0StarterCatTurnaroundConformanceSpecEntry(
                    P0PrototypeCatalog.SuzuneId,
                    "Suzune",
                    suzune.SourceLockId,
                    suzune.SourceTurnaroundPath,
                    new[]
                    {
                        "front view calico orange, black, and white face patches with blue eyes",
                        "front view white shrine robe, vermilion skirt, sash, and central gold bell",
                        "front view bell wand/branch cluster with blue paper talismans"
                    },
                    new[]
                    {
                        "side view calico head patches continue across ear and cheek",
                        "side view white sleeve with blue snowflake motif and red stitch trim",
                        "side view red ribbons, hanging bells, and bell wand remain readable"
                    },
                    new[]
                    {
                        "back view orange and black calico head patches across both ears",
                        "back view large vermilion bow with gold bell over white robe",
                        "back view white sleeves show blue snowflake marks and calico tail"
                    },
                    new[]
                    {
                        "warm white fur and robe fabric",
                        "vermilion red skirt, sash, bow, and ribbons",
                        "gold bells with moon-blue talismans and sleep effects"
                    },
                    new[]
                    {
                        "clustered kagura bell wand/branch",
                        "red-white flower hair ornament with hanging bells",
                        "paper talismans, blue snowflake charms, central bell, and back bow"
                    },
                    new[]
                    {
                        "Reject generated-lineup or generic shrine-cat drift over the colored three-view turnaround.",
                        "Reject human shrine maiden proportions, human sleeves-as-arms pose, or human costume posture.",
                        "Reject palette drift away from calico patches, white robe, vermilion cloth, gold bells, and blue talismans.",
                        "Reject missing front, side, or back anchors including calico markings, bells, wand, snowflake sleeves, and back bow."
                    })
            };
        }

        public static P0StarterCatTurnaroundConformanceSpecReport EvaluateP0Spec()
        {
            return Evaluate(
                CreateP0Spec(),
                P0StarterCatVisualConsistencyChecklist.CreateP0Checklist(),
                DefaultFileExists);
        }

        public static P0StarterCatTurnaroundConformanceSpecReport Evaluate(
            IReadOnlyList<P0StarterCatTurnaroundConformanceSpecEntry> specs,
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist,
            Func<string, bool> fileExists)
        {
            P0StarterCatTurnaroundConformanceSpecReport report = new P0StarterCatTurnaroundConformanceSpecReport();
            Func<string, bool> exists = fileExists ?? DefaultFileExists;
            int specCount = specs == null ? 0 : specs.Count;
            int sourceLockMatchedCount = 0;
            int existingSourceFileCount = 0;
            int frontViewAnchorCount = 0;
            int sideViewAnchorCount = 0;
            int backViewAnchorCount = 0;
            int paletteAnchorCount = 0;
            int propAndCostumeAnchorCount = 0;
            int prohibitedDriftRuleCount = 0;
            bool allExpectedCatsPresent = true;
            bool allEntriesComplete = true;
            bool allSourceLocksMatch = true;
            bool allSourceFilesExist = true;
            bool allViewAnchorsReady = true;
            bool allPaletteAnchorsReady = true;
            bool allPropAnchorsReady = true;
            bool allDriftRulesReady = true;
            bool allCatSpecificAnchorsReady = true;
            HashSet<string> uniqueCats = new HashSet<string>(StringComparer.Ordinal);

            if (specs == null)
            {
                report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, "Starter cat turnaround conformance spec is missing.");
                report.SetCounts(0, 0, 0, 0, 0, 0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < specs.Count; i++)
            {
                P0StarterCatTurnaroundConformanceSpecEntry entry = specs[i];
                if (!IsExpectedStarterCat(entry.CatId) || !uniqueCats.Add(entry.CatId))
                {
                    allExpectedCatsPresent = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.CatId + " is not a unique P0 starter cat turnaround conformance entry.");
                }

                if (!entry.IsComplete)
                {
                    allEntriesComplete = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " turnaround conformance entry is incomplete.");
                }

                frontViewAnchorCount += entry.FrontViewAnchors.Count;
                sideViewAnchorCount += entry.SideViewAnchors.Count;
                backViewAnchorCount += entry.BackViewAnchors.Count;
                paletteAnchorCount += entry.PaletteAnchors.Count;
                propAndCostumeAnchorCount += entry.PropAndCostumeAnchors.Count;
                prohibitedDriftRuleCount += entry.ProhibitedDriftRules.Count;

                if (SourceLockMatches(entry, checklist))
                {
                    sourceLockMatchedCount++;
                }
                else
                {
                    allSourceLocksMatch = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " turnaround conformance spec does not match the visual checklist source lock.");
                }

                if (exists(entry.SourceTurnaroundPath))
                {
                    existingSourceFileCount++;
                }
                else
                {
                    allSourceFilesExist = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " colored three-view turnaround is missing: " + entry.SourceTurnaroundPath + ".");
                }

                if (!ViewAnchorsReady(entry))
                {
                    allViewAnchorsReady = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " must define strict front, side, and back view anchors.");
                }

                if (!PaletteAnchorsReady(entry))
                {
                    allPaletteAnchorsReady = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " must define locked colored-turnaround palette anchors.");
                }

                if (!PropAnchorsReady(entry))
                {
                    allPropAnchorsReady = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " must define prop and costume anchors from the colored turnaround.");
                }

                if (!DriftRulesReady(entry))
                {
                    allDriftRulesReady = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " drift rules must block generated-lineup drift, human proportions, palette drift, and missing view anchors.");
                }

                if (!CatSpecificAnchorsReady(entry))
                {
                    allCatSpecificAnchorsReady = false;
                    report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, entry.DisplayName + " turnaround conformance spec is missing cat-specific three-view anchors.");
                }
            }

            allExpectedCatsPresent &= specCount == 3
                && uniqueCats.Contains(P0PrototypeCatalog.SaibanId)
                && uniqueCats.Contains(P0PrototypeCatalog.NephthysId)
                && uniqueCats.Contains(P0PrototypeCatalog.SuzuneId);

            Require(report, allExpectedCatsPresent, "Starter cat turnaround conformance spec covers exactly Saiban, Nephthys, and Suzune.", "Starter cat turnaround conformance spec must cover exactly the three P0 starter cats.");
            Require(report, allEntriesComplete, "Each starter cat turnaround conformance entry declares source, front, side, back, palette, prop, and drift anchors.", "One or more starter cat turnaround conformance entries are incomplete.");
            Require(report, allSourceLocksMatch, "Starter cat turnaround conformance specs reuse the visual checklist colored-turnaround source locks.", "One or more starter cat turnaround conformance specs drifted from the visual source locks.");
            Require(report, allSourceFilesExist, "Starter cat colored three-view turnaround source files exist on disk.", "One or more starter cat colored three-view turnaround files are missing.");
            Require(report, allViewAnchorsReady && frontViewAnchorCount >= 9 && sideViewAnchorCount >= 9 && backViewAnchorCount >= 9, "Starter cat turnaround conformance defines front, side, and back anchors for every starter cat.", "Starter cat three-view anchors are incomplete.");
            Require(report, allPaletteAnchorsReady && paletteAnchorCount >= 9, "Starter cat turnaround conformance defines locked palette anchors for every starter cat.", "Starter cat palette anchors are incomplete.");
            Require(report, allPropAnchorsReady && propAndCostumeAnchorCount >= 9, "Starter cat turnaround conformance defines prop and costume anchors for every starter cat.", "Starter cat prop/costume anchors are incomplete.");
            Require(report, allDriftRulesReady && prohibitedDriftRuleCount >= 12, "Starter cat turnaround conformance blocks generated-lineup drift, human proportions, palette drift, and missing view anchors.", "Starter cat prohibited drift rules are incomplete.");
            Require(report, allCatSpecificAnchorsReady, "Starter cat turnaround conformance includes cat-specific front/side/back identity anchors.", "Starter cat conformance spec is missing cat-specific identity anchors.");

            report.SetCounts(
                specCount,
                sourceLockMatchedCount,
                existingSourceFileCount,
                frontViewAnchorCount,
                sideViewAnchorCount,
                backViewAnchorCount,
                paletteAnchorCount,
                propAndCostumeAnchorCount,
                prohibitedDriftRuleCount);
            return report;
        }

        private static P0StarterCatVisualConsistencyChecklistEntry FindChecklistEntry(
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist,
            string catId)
        {
            if (checklist == null)
            {
                return default(P0StarterCatVisualConsistencyChecklistEntry);
            }

            for (int i = 0; i < checklist.Count; i++)
            {
                if (checklist[i].CatId == catId)
                {
                    return checklist[i];
                }
            }

            return default(P0StarterCatVisualConsistencyChecklistEntry);
        }

        private static bool SourceLockMatches(
            P0StarterCatTurnaroundConformanceSpecEntry entry,
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist)
        {
            P0StarterCatVisualConsistencyChecklistEntry visual = FindChecklistEntry(checklist, entry.CatId);
            return visual.CatId == entry.CatId
                && entry.SourceLockId == visual.SourceLockId
                && entry.SourceTurnaroundPath == visual.SourceTurnaroundPath;
        }

        private static bool ViewAnchorsReady(P0StarterCatTurnaroundConformanceSpecEntry entry)
        {
            return ContainsListText(entry.FrontViewAnchors, "front view")
                && ContainsListText(entry.SideViewAnchors, "side view")
                && ContainsListText(entry.BackViewAnchors, "back view");
        }

        private static bool PaletteAnchorsReady(P0StarterCatTurnaroundConformanceSpecEntry entry)
        {
            return entry.PaletteAnchors.Count >= MinimumPaletteAnchorsPerCat
                && ContainsListText(entry.PaletteAnchors, "fur")
                && (ContainsListText(entry.PaletteAnchors, "red")
                    || ContainsListText(entry.PaletteAnchors, "navy")
                    || ContainsListText(entry.PaletteAnchors, "vermilion"));
        }

        private static bool PropAnchorsReady(P0StarterCatTurnaroundConformanceSpecEntry entry)
        {
            return entry.PropAndCostumeAnchors.Count >= MinimumPropAnchorsPerCat
                && (ContainsListText(entry.PropAndCostumeAnchors, "shield")
                    || ContainsListText(entry.PropAndCostumeAnchors, "pyramid")
                    || ContainsListText(entry.PropAndCostumeAnchors, "bell"));
        }

        private static bool DriftRulesReady(P0StarterCatTurnaroundConformanceSpecEntry entry)
        {
            return ContainsListText(entry.ProhibitedDriftRules, "generated-lineup")
                && ContainsListText(entry.ProhibitedDriftRules, "human")
                && ContainsListText(entry.ProhibitedDriftRules, "palette")
                && ContainsListText(entry.ProhibitedDriftRules, "front, side, or back anchors");
        }

        private static bool CatSpecificAnchorsReady(P0StarterCatTurnaroundConformanceSpecEntry entry)
        {
            switch (entry.CatId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return ContainsListText(entry.FrontViewAnchors, "shield")
                        && ContainsListText(entry.SideViewAnchors, "sword")
                        && ContainsListText(entry.BackViewAnchors, "striped tail");
                case P0PrototypeCatalog.NephthysId:
                    return ContainsListText(entry.FrontViewAnchors, "pyramid")
                        && ContainsListText(entry.SideViewAnchors, "hood")
                        && ContainsListText(entry.BackViewAnchors, "ankh");
                case P0PrototypeCatalog.SuzuneId:
                    return ContainsListText(entry.FrontViewAnchors, "calico")
                        && ContainsListText(entry.SideViewAnchors, "bell")
                        && ContainsListText(entry.BackViewAnchors, "bow");
                default:
                    return false;
            }
        }

        private static bool IsExpectedStarterCat(string catId)
        {
            return catId == P0PrototypeCatalog.SaibanId
                || catId == P0PrototypeCatalog.NephthysId
                || catId == P0PrototypeCatalog.SuzuneId;
        }

        private static bool ContainsListText(IReadOnlyList<string> values, string token)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (ContainsText(values[i], token))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsText(string value, string token)
        {
            return value != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultFileExists(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (File.Exists(path))
            {
                return true;
            }

            DirectoryInfo current = new DirectoryInfo(Directory.GetCurrentDirectory());
            for (int i = 0; i < 6 && current != null; i++)
            {
                string candidate = Path.Combine(current.FullName, path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate))
                {
                    return true;
                }

                current = current.Parent;
            }

            return false;
        }

        private static void Require(P0StarterCatTurnaroundConformanceSpecReport report, bool condition, string coveredCheck, string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatTurnaroundConformanceSpecSeverity.Failure, failureMessage);
        }
    }

    public readonly struct P0StarterCatAssetProductionSpecEntry
    {
        private readonly IReadOnlyList<string> allowedDerivativeAssetTypes;
        private readonly IReadOnlyList<string> requiredEvidence;
        private readonly IReadOnlyList<string> requiredPromptClauses;
        private readonly IReadOnlyList<string> rejectionRules;

        public P0StarterCatAssetProductionSpecEntry(
            string catId,
            string displayName,
            string sourceLockId,
            string sourceTurnaroundPath,
            string currentSpritePath,
            string activeCatScreenshotFileName,
            string candidateDirectory,
            string approvedImportDirectory,
            IReadOnlyList<string> allowedDerivativeAssetTypes,
            IReadOnlyList<string> requiredEvidence,
            IReadOnlyList<string> requiredPromptClauses,
            IReadOnlyList<string> rejectionRules)
        {
            CatId = catId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            SourceLockId = sourceLockId ?? string.Empty;
            SourceTurnaroundPath = sourceTurnaroundPath ?? string.Empty;
            CurrentSpritePath = currentSpritePath ?? string.Empty;
            ActiveCatScreenshotFileName = activeCatScreenshotFileName ?? string.Empty;
            CandidateDirectory = candidateDirectory ?? string.Empty;
            ApprovedImportDirectory = approvedImportDirectory ?? string.Empty;
            this.allowedDerivativeAssetTypes = CopyList(allowedDerivativeAssetTypes);
            this.requiredEvidence = CopyList(requiredEvidence);
            this.requiredPromptClauses = CopyList(requiredPromptClauses);
            this.rejectionRules = CopyList(rejectionRules);
        }

        public string CatId { get; }

        public string DisplayName { get; }

        public string SourceLockId { get; }

        public string SourceTurnaroundPath { get; }

        public string CurrentSpritePath { get; }

        public string ActiveCatScreenshotFileName { get; }

        public string CandidateDirectory { get; }

        public string ApprovedImportDirectory { get; }

        public IReadOnlyList<string> AllowedDerivativeAssetTypes => allowedDerivativeAssetTypes ?? Array.Empty<string>();

        public IReadOnlyList<string> RequiredEvidence => requiredEvidence ?? Array.Empty<string>();

        public IReadOnlyList<string> RequiredPromptClauses => requiredPromptClauses ?? Array.Empty<string>();

        public IReadOnlyList<string> RejectionRules => rejectionRules ?? Array.Empty<string>();

        public bool IsComplete => !string.IsNullOrWhiteSpace(CatId)
            && !string.IsNullOrWhiteSpace(DisplayName)
            && !string.IsNullOrWhiteSpace(SourceLockId)
            && !string.IsNullOrWhiteSpace(SourceTurnaroundPath)
            && !string.IsNullOrWhiteSpace(CurrentSpritePath)
            && !string.IsNullOrWhiteSpace(ActiveCatScreenshotFileName)
            && !string.IsNullOrWhiteSpace(CandidateDirectory)
            && !string.IsNullOrWhiteSpace(ApprovedImportDirectory)
            && AllowedDerivativeAssetTypes.Count >= P0StarterCatAssetProductionSpec.MinimumAllowedDerivativeTypesPerCat
            && RequiredEvidence.Count >= P0StarterCatAssetProductionSpec.MinimumRequiredEvidencePerCat
            && RequiredPromptClauses.Count >= P0StarterCatAssetProductionSpec.MinimumPromptClausesPerCat
            && RejectionRules.Count >= P0StarterCatAssetProductionSpec.MinimumRejectionRulesPerCat;

        private static IReadOnlyList<string> CopyList(IReadOnlyList<string> values)
        {
            return values == null ? Array.Empty<string>() : new List<string>(values).AsReadOnly();
        }
    }

    public enum P0StarterCatAssetProductionSpecSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatAssetProductionSpecIssue
    {
        public P0StarterCatAssetProductionSpecIssue(P0StarterCatAssetProductionSpecSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatAssetProductionSpecSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatAssetProductionSpecReport
    {
        private readonly List<P0StarterCatAssetProductionSpecIssue> issues = new List<P0StarterCatAssetProductionSpecIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatAssetProductionSpecIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int SpecCount { get; private set; }

        public int SourceLockMatchedCount { get; private set; }

        public int AllowedDerivativeAssetTypeCount { get; private set; }

        public int RequiredEvidenceCount { get; private set; }

        public int PromptClauseCount { get; private set; }

        public int RejectionRuleCount { get; private set; }

        public int FailureCount => Count(P0StarterCatAssetProductionSpecSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatAssetProductionSpec.ExpectedCoveredCheckCount;

        public void SetCounts(
            int specCount,
            int sourceLockMatchedCount,
            int allowedDerivativeAssetTypeCount,
            int requiredEvidenceCount,
            int promptClauseCount,
            int rejectionRuleCount)
        {
            SpecCount = specCount;
            SourceLockMatchedCount = sourceLockMatchedCount;
            AllowedDerivativeAssetTypeCount = allowedDerivativeAssetTypeCount;
            RequiredEvidenceCount = requiredEvidenceCount;
            PromptClauseCount = promptClauseCount;
            RejectionRuleCount = rejectionRuleCount;
        }

        public void AddIssue(P0StarterCatAssetProductionSpecSeverity severity, string message)
        {
            issues.Add(new P0StarterCatAssetProductionSpecIssue(severity, message));
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
            return IsReady
                ? "P0 starter cat asset production spec ready for " + SpecCount + " cat(s), " + RequiredEvidenceCount + " evidence item(s), and " + RejectionRuleCount + " rejection rule(s)."
                : "P0 starter cat asset production spec has " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Spec cats: " + SpecCount,
                "Source-lock matches: " + SourceLockMatchedCount,
                "Allowed derivative asset types: " + AllowedDerivativeAssetTypeCount,
                "Required evidence items: " + RequiredEvidenceCount,
                "Prompt clauses: " + PromptClauseCount,
                "Rejection rules: " + RejectionRuleCount
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

        private int Count(P0StarterCatAssetProductionSpecSeverity severity)
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

    public static class P0StarterCatAssetProductionSpec
    {
        public const int ExpectedCoveredCheckCount = 7;
        public const int MinimumAllowedDerivativeTypesPerCat = 4;
        public const int MinimumRequiredEvidencePerCat = 7;
        public const int MinimumPromptClausesPerCat = 6;
        public const int MinimumRejectionRulesPerCat = 4;

        private const string CandidateRoot = "design/development/asset_candidates/starter_cats";
        private const string ApprovedSpriteDirectory = "Assets/TheCat/Art/Characters/Sprites";

        public static IReadOnlyList<P0StarterCatAssetProductionSpecEntry> CreateP0Spec()
        {
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist = P0StarterCatVisualConsistencyChecklist.CreateP0Checklist();
            List<P0StarterCatAssetProductionSpecEntry> result = new List<P0StarterCatAssetProductionSpecEntry>();
            for (int i = 0; i < checklist.Count; i++)
            {
                P0StarterCatVisualConsistencyChecklistEntry entry = checklist[i];
                result.Add(CreateEntry(entry));
            }

            return result.AsReadOnly();
        }

        public static P0StarterCatAssetProductionSpecReport EvaluateP0Spec()
        {
            return Evaluate(CreateP0Spec(), P0StarterCatVisualConsistencyChecklist.CreateP0Checklist());
        }

        public static P0StarterCatAssetProductionSpecReport Evaluate(
            IReadOnlyList<P0StarterCatAssetProductionSpecEntry> specs,
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist)
        {
            P0StarterCatAssetProductionSpecReport report = new P0StarterCatAssetProductionSpecReport();
            int specCount = specs == null ? 0 : specs.Count;
            int sourceLockMatchedCount = 0;
            int allowedDerivativeAssetTypeCount = 0;
            int requiredEvidenceCount = 0;
            int promptClauseCount = 0;
            int rejectionRuleCount = 0;
            bool allExpectedCatsPresent = true;
            bool allEntriesComplete = true;
            bool allSourceLocksMatch = true;
            bool allOutputPolicyReady = true;
            bool allEvidenceReady = true;
            bool allPromptClausesReady = true;
            bool allRejectionRulesReady = true;
            HashSet<string> uniqueCats = new HashSet<string>(StringComparer.Ordinal);

            if (specs == null)
            {
                report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, "Starter cat asset production spec is missing.");
                report.SetCounts(0, 0, 0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < specs.Count; i++)
            {
                P0StarterCatAssetProductionSpecEntry entry = specs[i];
                if (!IsExpectedStarterCat(entry.CatId) || !uniqueCats.Add(entry.CatId))
                {
                    allExpectedCatsPresent = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.CatId + " is not a unique P0 starter cat asset-production spec entry.");
                }

                if (!entry.IsComplete)
                {
                    allEntriesComplete = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.DisplayName + " asset-production spec entry is incomplete.");
                }

                allowedDerivativeAssetTypeCount += entry.AllowedDerivativeAssetTypes.Count;
                requiredEvidenceCount += entry.RequiredEvidence.Count;
                promptClauseCount += entry.RequiredPromptClauses.Count;
                rejectionRuleCount += entry.RejectionRules.Count;

                if (SourceLockMatches(entry, checklist))
                {
                    sourceLockMatchedCount++;
                }
                else
                {
                    allSourceLocksMatch = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.DisplayName + " asset-production spec does not match the visual consistency source lock.");
                }

                if (!OutputPolicyReady(entry))
                {
                    allOutputPolicyReady = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.DisplayName + " asset-production spec must keep candidates outside Assets and approved imports inside the character sprite directory.");
                }

                if (!EvidenceReady(entry))
                {
                    allEvidenceReady = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.DisplayName + " asset-production spec is missing required candidate evidence items.");
                }

                if (!PromptClausesReady(entry))
                {
                    allPromptClausesReady = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.DisplayName + " asset-production spec is missing strict colored-turnaround prompt clauses.");
                }

                if (!RejectionRulesReady(entry))
                {
                    allRejectionRulesReady = false;
                    report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, entry.DisplayName + " asset-production spec is missing drift rejection rules.");
                }
            }

            allExpectedCatsPresent &= specCount == 3
                && uniqueCats.Contains(P0PrototypeCatalog.SaibanId)
                && uniqueCats.Contains(P0PrototypeCatalog.NephthysId)
                && uniqueCats.Contains(P0PrototypeCatalog.SuzuneId);

            Require(report, allExpectedCatsPresent, "Starter cat asset-production spec covers exactly Saiban, Nephthys, and Suzune.", "Starter cat asset-production spec must cover exactly the three P0 starter cats.");
            Require(report, allEntriesComplete, "Each starter cat asset-production spec declares source, output policy, evidence, prompt clauses, and rejection rules.", "One or more starter cat asset-production specs are incomplete.");
            Require(report, allSourceLocksMatch, "Starter cat asset-production specs reuse the visual checklist colored-turnaround source locks.", "One or more starter cat asset-production specs drifted from the visual source locks.");
            Require(report, allOutputPolicyReady, "Starter cat asset-production specs keep candidates in review folders and approved imports in character sprite folders.", "Starter cat asset-production output policy is incomplete.");
            Require(report, allEvidenceReady && requiredEvidenceCount >= 21, "Starter cat asset-production specs require source, candidate, screenshot, comparison, and decision evidence.", "Starter cat asset-production evidence requirements are incomplete.");
            Require(report, allPromptClausesReady && promptClauseCount >= 18, "Starter cat asset-production prompts must name colored turnarounds, non-human cat bodies, front-view derivation, and cat-specific traits.", "Starter cat asset-production prompt clauses are incomplete.");
            Require(report, allRejectionRulesReady && rejectionRuleCount >= 12, "Starter cat asset-production rejection rules block colored-turnaround drift, human proportions, palette drift, and prop/costume loss.", "Starter cat asset-production rejection rules are incomplete.");

            report.SetCounts(specCount, sourceLockMatchedCount, allowedDerivativeAssetTypeCount, requiredEvidenceCount, promptClauseCount, rejectionRuleCount);
            return report;
        }

        private static P0StarterCatAssetProductionSpecEntry CreateEntry(P0StarterCatVisualConsistencyChecklistEntry entry)
        {
            return new P0StarterCatAssetProductionSpecEntry(
                entry.CatId,
                entry.DisplayName,
                entry.SourceLockId,
                entry.SourceTurnaroundPath,
                entry.SpritePath,
                entry.PlayModeScreenshotFileName,
                CandidateRoot + "/" + entry.CatId,
                ApprovedSpriteDirectory,
                new[]
                {
                    "combat_sprite_refinement_512",
                    "hud_avatar_256",
                    "skill_icon_motif_128",
                    "front_animation_keyframe_512"
                },
                new[]
                {
                    "locked colored turnaround source image",
                    "current Unity combat sprite",
                    "registered active-cat Play Mode screenshot",
                    "candidate PNG path under candidate review directory",
                    "side-by-side comparison against starter cat contact sheet",
                    "turnaround conformance spec front/side/back anchor checklist",
                    "colored-turnaround trait coverage notes",
                    "acceptance or rejection decision with reason"
                },
                PromptClauses(entry),
                RejectionRules(entry));
        }

        private static IReadOnlyList<string> PromptClauses(P0StarterCatVisualConsistencyChecklistEntry entry)
        {
            List<string> clauses = new List<string>
            {
                "Use " + Path.GetFileName(entry.SourceTurnaroundPath) + " as the only character source of truth for " + entry.DisplayName + ".",
                "Preserve non-human cat body proportions from the colored turnaround.",
                "Preserve the front, side, and back turnaround anchors even when producing a front-view derivative.",
                "Generate front-view derivatives until side/back variants are explicitly requested.",
                "Do not use the generated starter cat lineup as the primary reference.",
                "Preserve the current Unity combat sprite identity when refining existing runtime art.",
                "Keep transparent-background game asset readability for Unity import."
            };

            for (int i = 0; i < entry.RequiredTraits.Count; i++)
            {
                clauses.Add("Preserve " + entry.RequiredTraits[i] + ".");
            }

            return clauses.AsReadOnly();
        }

        private static IReadOnlyList<string> RejectionRules(P0StarterCatVisualConsistencyChecklistEntry entry)
        {
            return new[]
            {
                "Reject if the candidate drifts from the colored turnaround markings, costume, props, or silhouette.",
                "Reject if the candidate introduces human body proportions or human costume posture.",
                "Reject if the palette shifts away from the locked colored turnaround.",
                "Reject if any required cat-specific trait is missing: " + string.Join("; ", entry.RequiredTraits) + "."
            };
        }

        private static bool SourceLockMatches(
            P0StarterCatAssetProductionSpecEntry entry,
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist)
        {
            P0StarterCatVisualConsistencyChecklistEntry visual = FindChecklistEntry(checklist, entry.CatId);
            return visual.CatId == entry.CatId
                && entry.SourceLockId == visual.SourceLockId
                && entry.SourceTurnaroundPath == visual.SourceTurnaroundPath
                && entry.CurrentSpritePath == visual.SpritePath
                && entry.ActiveCatScreenshotFileName == visual.PlayModeScreenshotFileName;
        }

        private static P0StarterCatVisualConsistencyChecklistEntry FindChecklistEntry(
            IReadOnlyList<P0StarterCatVisualConsistencyChecklistEntry> checklist,
            string catId)
        {
            if (checklist == null)
            {
                return default(P0StarterCatVisualConsistencyChecklistEntry);
            }

            for (int i = 0; i < checklist.Count; i++)
            {
                if (checklist[i].CatId == catId)
                {
                    return checklist[i];
                }
            }

            return default(P0StarterCatVisualConsistencyChecklistEntry);
        }

        private static bool OutputPolicyReady(P0StarterCatAssetProductionSpecEntry entry)
        {
            return StartsWith(entry.CandidateDirectory, CandidateRoot + "/")
                && StartsWith(entry.ApprovedImportDirectory, ApprovedSpriteDirectory);
        }

        private static bool EvidenceReady(P0StarterCatAssetProductionSpecEntry entry)
        {
            return ContainsListText(entry.RequiredEvidence, "source")
                && ContainsListText(entry.RequiredEvidence, "current Unity")
                && ContainsListText(entry.RequiredEvidence, "screenshot")
                && ContainsListText(entry.RequiredEvidence, "candidate PNG")
                && ContainsListText(entry.RequiredEvidence, "side-by-side")
                && ContainsListText(entry.RequiredEvidence, "decision");
        }

        private static bool PromptClausesReady(P0StarterCatAssetProductionSpecEntry entry)
        {
            return ContainsListText(entry.RequiredPromptClauses, "colored turnaround")
                && ContainsListText(entry.RequiredPromptClauses, "non-human cat")
                && ContainsListText(entry.RequiredPromptClauses, "front, side, and back")
                && ContainsListText(entry.RequiredPromptClauses, "front-view")
                && ContainsListText(entry.RequiredPromptClauses, "Do not use the generated starter cat lineup")
                && CatSpecificPromptReady(entry);
        }

        private static bool CatSpecificPromptReady(P0StarterCatAssetProductionSpecEntry entry)
        {
            switch (entry.CatId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return ContainsListText(entry.RequiredPromptClauses, "shield")
                        && ContainsListText(entry.RequiredPromptClauses, "sword")
                        && ContainsListText(entry.RequiredPromptClauses, "tabby");
                case P0PrototypeCatalog.NephthysId:
                    return ContainsListText(entry.RequiredPromptClauses, "hood")
                        && ContainsListText(entry.RequiredPromptClauses, "pyramid")
                        && ContainsListText(entry.RequiredPromptClauses, "gold and blue");
                case P0PrototypeCatalog.SuzuneId:
                    return ContainsListText(entry.RequiredPromptClauses, "calico")
                        && ContainsListText(entry.RequiredPromptClauses, "shrine")
                        && ContainsListText(entry.RequiredPromptClauses, "bell");
                default:
                    return false;
            }
        }

        private static bool RejectionRulesReady(P0StarterCatAssetProductionSpecEntry entry)
        {
            return ContainsListText(entry.RejectionRules, "colored turnaround")
                && ContainsListText(entry.RejectionRules, "human")
                && ContainsListText(entry.RejectionRules, "palette")
                && ContainsListText(entry.RejectionRules, "required cat-specific trait");
        }

        private static bool IsExpectedStarterCat(string catId)
        {
            return catId == P0PrototypeCatalog.SaibanId
                || catId == P0PrototypeCatalog.NephthysId
                || catId == P0PrototypeCatalog.SuzuneId;
        }

        private static bool ContainsListText(IReadOnlyList<string> values, string token)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (ContainsText(values[i], token))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool StartsWith(string value, string prefix)
        {
            return value != null && value.StartsWith(prefix, StringComparison.Ordinal);
        }

        private static bool ContainsText(string value, string token)
        {
            return value != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(P0StarterCatAssetProductionSpecReport report, bool condition, string coveredCheck, string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatAssetProductionSpecSeverity.Failure, failureMessage);
        }
    }

    public enum P0StarterCatProductionPromptReadinessSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0StarterCatProductionPromptReadinessIssue
    {
        public P0StarterCatProductionPromptReadinessIssue(
            P0StarterCatProductionPromptReadinessSeverity severity,
            string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0StarterCatProductionPromptReadinessSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0StarterCatProductionPromptReadinessReport
    {
        private readonly List<P0StarterCatProductionPromptReadinessIssue> issues = new List<P0StarterCatProductionPromptReadinessIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatProductionPromptReadinessIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int PromptCount { get; private set; }

        public int ReadablePromptCount { get; private set; }

        public int DesignRootMentionCount { get; private set; }

        public int SourceTurnaroundPathMentionCount { get; private set; }

        public int CandidatePolicyMentionCount { get; private set; }

        public int FormalImportBlockMentionCount { get; private set; }

        public int OneCatAtATimeMentionCount { get; private set; }

        public int MojibakePathMentionCount { get; private set; }

        public int FailureCount => Count(P0StarterCatProductionPromptReadinessSeverity.Failure);

        public bool IsReady => FailureCount == 0
            && coveredChecks.Count >= P0StarterCatProductionPromptReadiness.ExpectedCoveredCheckCount;

        public void SetCounts(
            int promptCount,
            int readablePromptCount,
            int designRootMentionCount,
            int sourceTurnaroundPathMentionCount,
            int candidatePolicyMentionCount,
            int formalImportBlockMentionCount,
            int oneCatAtATimeMentionCount,
            int mojibakePathMentionCount)
        {
            PromptCount = promptCount;
            ReadablePromptCount = readablePromptCount;
            DesignRootMentionCount = designRootMentionCount;
            SourceTurnaroundPathMentionCount = sourceTurnaroundPathMentionCount;
            CandidatePolicyMentionCount = candidatePolicyMentionCount;
            FormalImportBlockMentionCount = formalImportBlockMentionCount;
            OneCatAtATimeMentionCount = oneCatAtATimeMentionCount;
            MojibakePathMentionCount = mojibakePathMentionCount;
        }

        public void AddIssue(P0StarterCatProductionPromptReadinessSeverity severity, string message)
        {
            issues.Add(new P0StarterCatProductionPromptReadinessIssue(severity, message));
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
            return IsReady
                ? "P0 starter cat production prompts ready for strict colored-turnaround asset work."
                : "P0 starter cat production prompts have " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Prompt files: " + PromptCount,
                "Readable prompt files: " + ReadablePromptCount,
                "Design-root mentions: " + DesignRootMentionCount,
                "Source-turnaround path mentions: " + SourceTurnaroundPathMentionCount,
                "Candidate policy mentions: " + CandidatePolicyMentionCount,
                "Formal-import block mentions: " + FormalImportBlockMentionCount,
                "One-cat-at-a-time mentions: " + OneCatAtATimeMentionCount,
                "Mojibake path mentions: " + MojibakePathMentionCount
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

        private int Count(P0StarterCatProductionPromptReadinessSeverity severity)
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

    public static class P0StarterCatProductionPromptReadiness
    {
        public const int ExpectedCoveredCheckCount = 7;
        public const int ExpectedPromptCount = 13;
        public const string DesignRoot = "design/梦境支配者核心玩法/assets";
        public const string CandidateRoot = "design/development/asset_candidates/starter_cats";

        public static readonly string[] PromptPaths =
        {
            "design/development/agent_prompts/p0_asset_batch_17_starter_cat_turnaround_conformance_gate.md",
            "design/development/agent_prompts/p0_asset_batch_18_starter_cat_strict_candidate_production.md",
            "design/development/agent_prompts/p0_asset_batch_26_starter_cat_candidate_gate.md",
            "design/development/agent_prompts/p0_asset_batch_28_starter_cat_strict_reference_pack.md",
            "design/development/agent_prompts/p0_asset_batch_29_saiban_strict_turnaround_derivatives.md",
            "design/development/agent_prompts/p0_asset_batch_30_saiban_ai_refinement_candidate.md",
            "design/development/agent_prompts/p0_asset_batch_31_saiban_cutout_candidate.md",
            "design/development/agent_prompts/p0_asset_batch_32_nephthys_strict_turnaround_derivatives.md",
            "design/development/agent_prompts/p0_asset_batch_33_suzune_strict_turnaround_derivatives.md",
            "design/development/agent_prompts/p0_asset_batch_34_suzune_ai_refinement_candidate.md",
            "design/development/agent_prompts/p0_asset_batch_35_suzune_cutout_candidate.md",
            "design/development/agent_prompts/p0_asset_batch_36_nephthys_ai_refinement_candidate.md",
            "design/development/agent_prompts/p0_asset_batch_37_nephthys_cutout_candidate.md"
        };

        public static P0StarterCatProductionPromptReadinessReport EvaluateCurrentPrompts()
        {
            return Evaluate(PromptPaths, P0StarterCatTurnaroundSourceLocks.CreateP0Locks(), DefaultReadPrompt);
        }

        public static P0StarterCatProductionPromptReadinessReport Evaluate(
            IReadOnlyList<string> promptPaths,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            P0StarterCatPromptTextReader reader)
        {
            P0StarterCatProductionPromptReadinessReport report = new P0StarterCatProductionPromptReadinessReport();
            P0StarterCatPromptTextReader read = reader ?? DefaultReadPrompt;
            int promptCount = promptPaths == null ? 0 : promptPaths.Count;
            int readablePromptCount = 0;
            int designRootMentionCount = 0;
            int sourceTurnaroundPathMentionCount = 0;
            int candidatePolicyMentionCount = 0;
            int formalImportBlockMentionCount = 0;
            int oneCatAtATimeMentionCount = 0;
            int mojibakePathMentionCount = 0;
            bool allPromptsReadable = true;
            bool allPromptPathsScoped = promptCount == ExpectedPromptCount;
            bool allDesignRootsReal = true;
            bool allSourcePathsPinned = true;
            bool allCandidatePoliciesPinned = true;
            bool allFormalBlocksPinned = true;
            bool noMojibakePaths = true;
            bool oneCatAtATimePolicyReady = false;

            if (promptPaths == null)
            {
                report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, "Starter cat production prompt list is missing.");
                report.SetCounts(0, 0, 0, 0, 0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < promptPaths.Count; i++)
            {
                string path = promptPaths[i] ?? string.Empty;
                if (!IsScopedPromptPath(path))
                {
                    allPromptPathsScoped = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, "Starter cat production prompt path is not scoped: " + path);
                }

                string text;
                string error;
                if (!read(path, out text, out error))
                {
                    allPromptsReadable = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, "Starter cat production prompt could not be read: " + path + " (" + error + ")");
                    continue;
                }

                readablePromptCount++;
                if (ContainsText(text, DesignRoot))
                {
                    designRootMentionCount++;
                }
                else
                {
                    allDesignRootsReal = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, path + " must use the real UTF-8 design asset root.");
                }

                if (ContainsMojibakePath(text))
                {
                    mojibakePathMentionCount++;
                    noMojibakePaths = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, path + " still contains mojibake design path text.");
                }

                int pathMentionsForPrompt = CountSourcePathMentions(text, locks);
                sourceTurnaroundPathMentionCount += pathMentionsForPrompt;
                if (pathMentionsForPrompt < ExpectedStarterCatCount(locks))
                {
                    allSourcePathsPinned = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, path + " must mention all three locked colored turnaround PNG paths.");
                }

                if (HasCandidatePolicy(text))
                {
                    candidatePolicyMentionCount++;
                }
                else
                {
                    allCandidatePoliciesPinned = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, path + " must keep starter-cat candidates outside Assets.");
                }

                if (HasFormalImportBlockPolicy(text))
                {
                    formalImportBlockMentionCount++;
                }
                else
                {
                    allFormalBlocksPinned = false;
                    report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, path + " must keep formal starter-cat import blocked until active-cat screenshot review.");
                }

                if (HasOneCatAtATimePolicy(text))
                {
                    oneCatAtATimeMentionCount++;
                    oneCatAtATimePolicyReady = true;
                }
            }

            Require(report, allPromptPathsScoped && promptCount == ExpectedPromptCount, "Starter cat production prompt set includes Batch 17, Batch 18, Batch 26, and Batch 28-37 scoped prompts.", "Starter cat production prompt set is missing a required batch prompt.");
            Require(report, allPromptsReadable && readablePromptCount == ExpectedPromptCount, "Starter cat production prompts are readable from the workspace.", "One or more starter cat production prompts are unreadable.");
            Require(report, allDesignRootsReal && designRootMentionCount == ExpectedPromptCount, "Starter cat production prompts use the real UTF-8 design source path.", "Starter cat production prompts must not use stale or invalid design roots.");
            Require(report, noMojibakePaths && mojibakePathMentionCount == 0, "Starter cat production prompts contain no mojibake design paths.", "Starter cat production prompts still contain mojibake design paths.");
            Require(report, allSourcePathsPinned && sourceTurnaroundPathMentionCount >= ExpectedPromptCount * ExpectedStarterCatCount(locks), "Starter cat production prompts pin all three colored turnaround PNG source paths.", "Starter cat production prompts do not pin all colored turnaround PNG source paths.");
            Require(report, allCandidatePoliciesPinned && candidatePolicyMentionCount == ExpectedPromptCount, "Starter cat production prompts keep candidates in review folders outside Assets.", "Starter cat production prompts do not consistently keep candidates outside Assets.");
            Require(report, allFormalBlocksPinned && oneCatAtATimePolicyReady, "Starter cat production prompts keep formal import blocked and require one-cat-at-a-time candidate production.", "Starter cat production prompts must block formal import and enforce one-cat-at-a-time production.");

            report.SetCounts(
                promptCount,
                readablePromptCount,
                designRootMentionCount,
                sourceTurnaroundPathMentionCount,
                candidatePolicyMentionCount,
                formalImportBlockMentionCount,
                oneCatAtATimeMentionCount,
                mojibakePathMentionCount);
            return report;
        }

        private static bool DefaultReadPrompt(string path, out string text, out string error)
        {
            text = string.Empty;
            error = string.Empty;
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                {
                    error = "file missing";
                    return false;
                }

                text = File.ReadAllText(path);
                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static int CountSourcePathMentions(
            string text,
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            if (locks == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < locks.Count; i++)
            {
                if (ContainsText(text, locks[i].SourceTurnaroundPath))
                {
                    count++;
                }
            }

            return count;
        }

        private static int ExpectedStarterCatCount(IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks)
        {
            return locks == null ? 3 : locks.Count;
        }

        private static bool IsScopedPromptPath(string path)
        {
            return path.StartsWith("design/development/agent_prompts/p0_asset_batch_", StringComparison.Ordinal)
                && path.EndsWith(".md", StringComparison.Ordinal);
        }

        private static bool HasCandidatePolicy(string text)
        {
            return ContainsText(text, CandidateRoot)
                && ContainsText(text, "outside `Assets`");
        }

        private static bool HasFormalImportBlockPolicy(string text)
        {
            return ContainsText(text, "formal import")
                && ContainsText(text, "blocked")
                && ContainsText(text, "active-cat");
        }

        private static bool HasOneCatAtATimePolicy(string text)
        {
            return ContainsText(text, "one-cat-at-a-time")
                || ContainsText(text, "one source-locked starter-cat candidate pack at a time")
                || ContainsText(text, "one cat at a time");
        }

        private static bool ContainsMojibakePath(string text)
        {
            return ContainsText(text, "姊")
                || ContainsText(text, "鏀")
                || ContainsText(text, "蹇")
                || ContainsText(text, "娉?");
        }

        private static bool ContainsText(string value, string token)
        {
            return value != null && token != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void Require(P0StarterCatProductionPromptReadinessReport report, bool condition, string coveredCheck, string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatProductionPromptReadinessSeverity.Failure, failureMessage);
        }
    }

    public sealed class P0StarterCatTurnaroundSourceLockReport
    {
        private readonly List<P0StarterCatTurnaroundSourceLockIssue> issues = new List<P0StarterCatTurnaroundSourceLockIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0StarterCatTurnaroundSourceLockIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int LockCount { get; private set; }

        public int SourceHashMatchedCount { get; private set; }

        public int SpriteHashMatchedCount { get; private set; }

        public int ManifestMatchedCount { get; private set; }

        public int FailureCount => Count(P0StarterCatTurnaroundSourceLockSeverity.Failure);

        public bool IsReady => FailureCount == 0 && coveredChecks.Count >= P0StarterCatTurnaroundSourceLocks.ExpectedCoveredCheckCount;

        public void SetCounts(int lockCount, int sourceHashMatchedCount, int spriteHashMatchedCount, int manifestMatchedCount)
        {
            LockCount = lockCount;
            SourceHashMatchedCount = sourceHashMatchedCount;
            SpriteHashMatchedCount = spriteHashMatchedCount;
            ManifestMatchedCount = manifestMatchedCount;
        }

        public void AddIssue(P0StarterCatTurnaroundSourceLockSeverity severity, string message)
        {
            issues.Add(new P0StarterCatTurnaroundSourceLockIssue(severity, message));
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
            return IsReady
                ? "P0 starter cat turnaround source locks ready for " + LockCount + " cat sprite(s)."
                : "P0 starter cat turnaround source locks have " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Locked cat sprites: " + LockCount,
                "Source hashes matched: " + SourceHashMatchedCount,
                "Sprite hashes matched: " + SpriteHashMatchedCount,
                "Manifest rows matched: " + ManifestMatchedCount
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

        private int Count(P0StarterCatTurnaroundSourceLockSeverity severity)
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

    public static class P0StarterCatTurnaroundSourceLocks
    {
        public const int ExpectedCoveredCheckCount = 6;

        private const string DesignRoot = "design/\u68a6\u5883\u652f\u914d\u8005\u6838\u5fc3\u73a9\u6cd5/assets";

        public static IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> CreateP0Locks()
        {
            return new[]
            {
                new P0StarterCatTurnaroundSourceLockEntry(
                    P0PrototypeCatalog.SaibanId,
                    "thecat_cat_saiban_combat_sprite_512_v001",
                    DesignRoot + "/characters/ch01_saiban_swordsaint/turnaround/saiban_turnaround_colored_2026-06-03.png",
                    "156a7fcb4ac3e9a75bf54788f12b7f18a43b6eaff3c14607ea689af612403dc1",
                    "Assets/TheCat/Art/Characters/Sprites/thecat_cat_saiban_combat_sprite_512_v001.png",
                    "fe13afc3758f19f66fd87debc56943a19b946ac78a44eab22d9ee1b146cc106b"),
                new P0StarterCatTurnaroundSourceLockEntry(
                    P0PrototypeCatalog.NephthysId,
                    "thecat_cat_nephthys_combat_sprite_512_v001",
                    DesignRoot + "/characters/ch02_nephthys_moonsand_agent/turnaround/nephthys_turnaround_colored_2026-06-03.png",
                    "37ad1532c8a981baaff67c05009ddc38482e9f00e71e6addfb2b321dad31de06",
                    "Assets/TheCat/Art/Characters/Sprites/thecat_cat_nephthys_combat_sprite_512_v001.png",
                    "6eabcb75078dd2bfe9c5f0ba5191af40376ad7f9ea545b12d75f39b8ffb45a20"),
                new P0StarterCatTurnaroundSourceLockEntry(
                    P0PrototypeCatalog.SuzuneId,
                    "thecat_cat_suzune_combat_sprite_512_v001",
                    DesignRoot + "/characters/ch03_suzune_sleep_shrine_maiden/turnaround/suzune_turnaround_colored_2026-06-03.png",
                    "9b616470da7daa77ad27c70d2b0bf3b3f30649ce0b639f6550589b9e6fe700b3",
                    "Assets/TheCat/Art/Characters/Sprites/thecat_cat_suzune_combat_sprite_512_v001.png",
                    "246ddd74501b4e81482d03e329b27c898ddd61b580bc30abeeeadef3aa61eaae")
            };
        }

        public static P0StarterCatTurnaroundSourceLockReport EvaluateP0Locks()
        {
            return Evaluate(CreateP0Locks(), P0AssetManifestCatalog.CreateP0PlannedManifest(), File.Exists, DefaultHashFile);
        }

        public static P0StarterCatTurnaroundSourceLockReport Evaluate(
            IReadOnlyList<P0StarterCatTurnaroundSourceLockEntry> locks,
            IReadOnlyList<P0AssetManifestEntry> manifest,
            Func<string, bool> fileExists,
            P0StarterCatHashReader hashReader)
        {
            P0StarterCatTurnaroundSourceLockReport report = new P0StarterCatTurnaroundSourceLockReport();
            Func<string, bool> exists = fileExists ?? File.Exists;
            P0StarterCatHashReader readHash = hashReader ?? DefaultHashFile;
            int lockCount = locks == null ? 0 : locks.Count;
            int sourceHashMatchedCount = 0;
            int spriteHashMatchedCount = 0;
            int manifestMatchedCount = 0;
            HashSet<string> lockedCatIds = new HashSet<string>(StringComparer.Ordinal);
            bool allLockedCatIdsAreUnique = true;
            bool allSourceHashesMatched = true;
            bool allSpriteHashesMatched = true;
            bool allManifestRowsMatched = true;
            bool allManifestNotesMatched = true;
            bool allManifestSourceLocksMatched = true;

            if (locks == null)
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, "Starter cat source locks are missing.");
                report.SetCounts(0, 0, 0, 0);
                return report;
            }

            for (int i = 0; i < locks.Count; i++)
            {
                P0StarterCatTurnaroundSourceLockEntry current = locks[i];
                if (!IsExpectedStarterCat(current.CatId))
                {
                    report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, current.CatId + " is not an expected starter cat source lock.");
                }
                else if (!lockedCatIds.Add(current.CatId))
                {
                    allLockedCatIdsAreUnique = false;
                    report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, current.CatId + " has a duplicate starter cat source lock.");
                }

                if (HashMatches(current.SourceTurnaroundPath, current.SourceTurnaroundSha256, exists, readHash, report, current.CatId + " source turnaround"))
                {
                    sourceHashMatchedCount++;
                }
                else
                {
                    allSourceHashesMatched = false;
                }

                if (HashMatches(current.SpritePath, current.SpriteSha256, exists, readHash, report, current.CatId + " locked sprite"))
                {
                    spriteHashMatchedCount++;
                }
                else
                {
                    allSpriteHashesMatched = false;
                }

                if (ManifestMatches(current, manifest, report, ref allManifestNotesMatched, ref allManifestSourceLocksMatched))
                {
                    manifestMatchedCount++;
                }
                else
                {
                    allManifestRowsMatched = false;
                }
            }

            bool allStarterCatsLocked = lockCount == 3
                && allLockedCatIdsAreUnique
                && lockedCatIds.Contains(P0PrototypeCatalog.SaibanId)
                && lockedCatIds.Contains(P0PrototypeCatalog.NephthysId)
                && lockedCatIds.Contains(P0PrototypeCatalog.SuzuneId);

            Require(
                report,
                allStarterCatsLocked,
                "Saiban, Nephthys, and Suzune all have starter cat source locks.",
                "Starter cat source locks do not cover exactly the three P0 starter cats.");
            Require(
                report,
                allSourceHashesMatched,
                "Starter cat colored turnaround source files match locked SHA-256 hashes.",
                "One or more starter cat colored turnaround source files changed or are missing.");
            Require(
                report,
                allSpriteHashesMatched,
                "Starter cat Unity sprites match locked SHA-256 hashes.",
                "One or more starter cat Unity sprites changed or are missing.");
            Require(
                report,
                allManifestRowsMatched,
                "Starter cat manifest rows point at locked sprite paths and generated status.",
                "One or more starter cat manifest rows no longer point at locked generated sprites.");
            Require(
                report,
                allManifestNotesMatched,
                "Starter cat manifest rows document colored turnaround hard-reference extraction.",
                "One or more starter cat manifest rows lost colored turnaround hard-reference notes.");
            Require(
                report,
                allManifestSourceLocksMatched,
                "Starter cat manifest rows declare colored turnaround source-lock ids.",
                "One or more starter cat manifest rows lost colored turnaround source-lock ids.");

            report.SetCounts(lockCount, sourceHashMatchedCount, spriteHashMatchedCount, manifestMatchedCount);
            return report;
        }

        private static bool HashMatches(
            string path,
            string expectedHash,
            Func<string, bool> fileExists,
            P0StarterCatHashReader hashReader,
            P0StarterCatTurnaroundSourceLockReport report,
            string label)
        {
            if (string.IsNullOrWhiteSpace(path) || !fileExists(path))
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, label + " is missing at " + path + ".");
                return false;
            }

            if (!hashReader(path, out string actualHash, out string error))
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, label + " hash could not be read: " + error);
                return false;
            }

            string normalizedActual = (actualHash ?? string.Empty).Trim().ToLowerInvariant();
            if (normalizedActual == expectedHash)
            {
                return true;
            }

            report.AddIssue(
                P0StarterCatTurnaroundSourceLockSeverity.Failure,
                label + " hash should be " + expectedHash + " but is " + normalizedActual + ".");
            return false;
        }

        private static bool ManifestMatches(
            P0StarterCatTurnaroundSourceLockEntry current,
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0StarterCatTurnaroundSourceLockReport report,
            ref bool allManifestNotesMatched,
            ref bool allManifestSourceLocksMatched)
        {
            P0AssetManifestEntry entry = FindManifestEntry(manifest, current.AssetId);
            if (entry == null)
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, current.AssetId + " manifest row is missing.");
                allManifestNotesMatched = false;
                return false;
            }

            bool rowMatches = entry.SubjectId == current.CatId
                && entry.AssetType == "sprite"
                && entry.Status == P0AssetManifestStatus.Generated
                && entry.UnityImportPath == current.SpritePath;
            if (!rowMatches)
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, current.AssetId + " manifest row does not match the locked generated sprite.");
            }

            bool notesMatch = ContainsText(entry.ConsistencyNotes, "colored turnaround")
                && ContainsText(entry.ConsistencyNotes, "hard reference")
                && ContainsText(entry.ConsistencyNotes, "front view");
            if (!notesMatch)
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, current.AssetId + " manifest notes must mention colored turnaround hard reference front view.");
            }

            allManifestNotesMatched &= notesMatch;
            string expectedLockId = current.CatId + "_turnaround_colored";
            bool sourceLockMatches = Contains(entry.SourceLockIds, expectedLockId);
            if (!sourceLockMatches)
            {
                report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, current.AssetId + " manifest row must reference source lock " + expectedLockId + ".");
            }

            allManifestSourceLocksMatched &= sourceLockMatches;
            return rowMatches && notesMatch && sourceLockMatches;
        }

        private static bool Contains(IReadOnlyList<string> values, string token)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == token)
                {
                    return true;
                }
            }

            return false;
        }

        private static P0AssetManifestEntry FindManifestEntry(IReadOnlyList<P0AssetManifestEntry> manifest, string assetId)
        {
            if (manifest == null)
            {
                return null;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                if (manifest[i].AssetId == assetId)
                {
                    return manifest[i];
                }
            }

            return null;
        }

        private static bool IsExpectedStarterCat(string catId)
        {
            return catId == P0PrototypeCatalog.SaibanId
                || catId == P0PrototypeCatalog.NephthysId
                || catId == P0PrototypeCatalog.SuzuneId;
        }

        private static bool ContainsText(string value, string token)
        {
            return value != null && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool DefaultHashFile(string path, out string sha256, out string error)
        {
            sha256 = string.Empty;
            error = string.Empty;
            try
            {
                using (FileStream stream = File.OpenRead(path))
                using (SHA256 algorithm = SHA256.Create())
                {
                    byte[] hash = algorithm.ComputeHash(stream);
                    sha256 = ToHex(hash);
                    return true;
                }
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return false;
            }
        }

        private static string ToHex(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        private static void Require(
            P0StarterCatTurnaroundSourceLockReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0StarterCatTurnaroundSourceLockSeverity.Failure, failureMessage);
        }
    }
}
