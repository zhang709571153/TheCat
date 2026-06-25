using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0CharacterDesignCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0CharacterDesignCoverageIssue
    {
        public P0CharacterDesignCoverageIssue(P0CharacterDesignCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0CharacterDesignCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0CharacterDesignCoverageReport
    {
        private readonly List<P0CharacterDesignCoverageIssue> issues = new List<P0CharacterDesignCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0CharacterDesignCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0CharacterDesignCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0CharacterDesignCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0CharacterDesignCoverageSeverity severity, string message)
        {
            issues.Add(new P0CharacterDesignCoverageIssue(severity, message));
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
            return IsComplete
                ? "P0 character design coverage complete for " + coveredChecks.Count + " design check(s)."
                : "P0 character design coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " design check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
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

        private int Count(P0CharacterDesignCoverageSeverity severity)
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

    public static class P0CharacterDesignCoverage
    {
        public const int ExpectedCoveredCheckCount = 5;

        public static P0CharacterDesignCoverageReport EvaluatePrototypeRoster()
        {
            return Evaluate(
                P0PrototypeCatalog.CreateStarterCats(),
                P0PrototypeCatalog.CreateStarterSkills());
        }

        public static P0CharacterDesignCoverageReport Evaluate(
            IReadOnlyList<CatDefinition> cats,
            IReadOnlyList<SkillDefinition> skills)
        {
            P0CharacterDesignCoverageReport report = new P0CharacterDesignCoverageReport();
            EvaluateStarterProfiles(cats, report);
            EvaluateDesignFacingPresentations(cats, report);
            EvaluateSignatureLines(cats, report);
            EvaluateVisualIdentity(cats, report);
            EvaluateRoleSkillSupport(cats, skills, report);
            return report;
        }

        private static void EvaluateStarterProfiles(
            IReadOnlyList<CatDefinition> cats,
            P0CharacterDesignCoverageReport report)
        {
            Require(
                report,
                cats != null
                && cats.Count == 3
                && HasCat(cats, P0PrototypeCatalog.SaibanId, CatRole.Defender, AuthorityIds.Oath, AttributeIds.Sun)
                && HasCat(cats, P0PrototypeCatalog.NephthysId, CatRole.Controller, AuthorityIds.Dominion, AttributeIds.Earth)
                && HasCat(cats, P0PrototypeCatalog.SuzuneId, CatRole.Healer, AuthorityIds.Rhythm, AttributeIds.Moon),
                "Starter trio matches P0 design roles, authorities, and attributes.",
                "Starter trio role / authority / attribute mapping does not match P0 design.");
        }

        private static void EvaluateDesignFacingPresentations(
            IReadOnlyList<CatDefinition> cats,
            P0CharacterDesignCoverageReport report)
        {
            Require(
                report,
                HasTitle(P0PrototypeCatalog.SaibanId, "Sacred Swordsman")
                && HasTitle(P0PrototypeCatalog.NephthysId, "Moon-Sand Agent")
                && HasTitle(P0PrototypeCatalog.SuzuneId, "Sleep Shrine Miko")
                && PresentationsMatchDefinitions(cats),
                "Starter trio exposes design-facing titles without raw ids.",
                "Starter trio presentation titles are missing or leaking raw ids.");
        }

        private static void EvaluateSignatureLines(
            IReadOnlyList<CatDefinition> cats,
            P0CharacterDesignCoverageReport report)
        {
            Require(
                report,
                HasSignature(P0PrototypeCatalog.SaibanId, "bed")
                && HasSignature(P0PrototypeCatalog.NephthysId, "dominion")
                && HasSignature(P0PrototypeCatalog.SuzuneId, "bells"),
                "Starter trio has signature lines for main-menu and future battle barks.",
                "Starter trio signature lines are incomplete.");
        }

        private static void EvaluateVisualIdentity(
            IReadOnlyList<CatDefinition> cats,
            P0CharacterDesignCoverageReport report)
        {
            Require(
                report,
                HasVisualIdentity(P0PrototypeCatalog.SaibanId, "silver_oath_sun_sword", "non-human")
                && HasVisualIdentity(P0PrototypeCatalog.NephthysId, "moon_sand_obelisk_crown", "non-human")
                && HasVisualIdentity(P0PrototypeCatalog.SuzuneId, "moon_bell_torii", "non-human"),
                "Starter trio has stable non-human cat visual identity tokens.",
                "Starter trio visual identity tokens are incomplete.");
        }

        private static void EvaluateRoleSkillSupport(
            IReadOnlyList<CatDefinition> cats,
            IReadOnlyList<SkillDefinition> skills,
            P0CharacterDesignCoverageReport report)
        {
            Require(
                report,
                CatHasEffect(cats, skills, P0PrototypeCatalog.SaibanId, SkillEffectType.Shield)
                && CatHasEffect(cats, skills, P0PrototypeCatalog.SaibanId, SkillEffectType.Knockback)
                && CatHasStatus(cats, skills, P0PrototypeCatalog.NephthysId, StatusTagIds.Slow)
                && CatHasStatus(cats, skills, P0PrototypeCatalog.NephthysId, StatusTagIds.Mark)
                && CatHasEffect(cats, skills, P0PrototypeCatalog.SuzuneId, SkillEffectType.RestoreOwnerSleep)
                && CatHasEffect(cats, skills, P0PrototypeCatalog.SuzuneId, SkillEffectType.HealCat),
                "Starter trio skills support defender, controller, and healer roles.",
                "Starter trio skills do not support the required P0 roles.");
        }

        private static bool HasCat(
            IReadOnlyList<CatDefinition> cats,
            string catId,
            CatRole role,
            string authorityId,
            string attributeId)
        {
            CatDefinition cat = FindCat(cats, catId);
            return cat != null
                && cat.Role == role
                && cat.AuthorityId == authorityId
                && cat.AttributeId == attributeId
                && cat.SkillIds.Count >= 3;
        }

        private static bool PresentationsMatchDefinitions(IReadOnlyList<CatDefinition> cats)
        {
            if (cats == null)
            {
                return false;
            }

            for (int i = 0; i < cats.Count; i++)
            {
                CatDefinition cat = cats[i];
                CatPresentation presentation = P0CatPresenter.Describe(cat);
                if (presentation.CatId != cat.Id
                    || presentation.DisplayName == cat.Id
                    || presentation.BuildSelectionLabel().Contains(cat.Id))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool HasTitle(string catId, string title)
        {
            return P0CatPresenter.Describe(catId).Title == title;
        }

        private static bool HasSignature(string catId, string requiredText)
        {
            string line = P0CatPresenter.Describe(catId).SignatureLine;
            return !string.IsNullOrWhiteSpace(line)
                && line.ToLowerInvariant().Contains(requiredText);
        }

        private static bool HasVisualIdentity(string catId, string visualToken, string requiredIdentityText)
        {
            CatPresentation presentation = P0CatPresenter.Describe(catId);
            return presentation.VisualToken == visualToken
                && !string.IsNullOrWhiteSpace(presentation.VisualIdentity)
                && presentation.VisualIdentity.Contains(requiredIdentityText);
        }

        private static bool CatHasEffect(
            IReadOnlyList<CatDefinition> cats,
            IReadOnlyList<SkillDefinition> skills,
            string catId,
            SkillEffectType effectType)
        {
            return CatHasEffect(cats, skills, catId, effect => effect.EffectType == effectType);
        }

        private static bool CatHasStatus(
            IReadOnlyList<CatDefinition> cats,
            IReadOnlyList<SkillDefinition> skills,
            string catId,
            string statusTagId)
        {
            return CatHasEffect(cats, skills, catId, effect => effect.StatusTagId == statusTagId);
        }

        private static bool CatHasEffect(
            IReadOnlyList<CatDefinition> cats,
            IReadOnlyList<SkillDefinition> skills,
            string catId,
            Func<SkillEffectDefinition, bool> predicate)
        {
            CatDefinition cat = FindCat(cats, catId);
            if (cat == null || skills == null || predicate == null)
            {
                return false;
            }

            for (int skillIndex = 0; skillIndex < skills.Count; skillIndex++)
            {
                SkillDefinition skill = skills[skillIndex];
                if (skill.OwnerCatId != cat.Id)
                {
                    continue;
                }

                for (int effectIndex = 0; effectIndex < skill.Effects.Count; effectIndex++)
                {
                    if (predicate(skill.Effects[effectIndex]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static CatDefinition FindCat(IReadOnlyList<CatDefinition> cats, string catId)
        {
            if (cats == null)
            {
                return null;
            }

            for (int i = 0; i < cats.Count; i++)
            {
                if (cats[i].Id == catId)
                {
                    return cats[i];
                }
            }

            return null;
        }

        private static void Require(
            P0CharacterDesignCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0CharacterDesignCoverageSeverity.Failure, failureMessage);
        }
    }
}
