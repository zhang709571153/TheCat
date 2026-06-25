using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0StatusTagCoverageTests
    {
        [Test]
        public void EvaluatePrototypeCatalog_CoversAllP0StatusTags()
        {
            P0StatusTagCoverageReport report = P0StatusTagCoverage.EvaluatePrototypeCatalog();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(5, report.Rows.Count);
            Assert.IsTrue(report.TryGetRow(StatusTagIds.SleepStable, out P0StatusTagCoverageRow sleep));
            Assert.AreEqual(StatusTargetType.BedZone, sleep.TargetType);
            Assert.IsTrue(report.TryGetRow(StatusTagIds.Slow, out P0StatusTagCoverageRow slow));
            Assert.AreEqual(StatusTargetType.Enemy, slow.TargetType);
            Assert.IsTrue(report.TryGetRow(StatusTagIds.Knockback, out P0StatusTagCoverageRow knockback));
            Assert.AreEqual(StatusTargetType.Enemy, knockback.TargetType);
            Assert.IsTrue(report.TryGetRow(StatusTagIds.Mark, out P0StatusTagCoverageRow mark));
            Assert.AreEqual(StatusTargetType.Enemy, mark.TargetType);
            Assert.IsTrue(report.TryGetRow(StatusTagIds.Shield, out P0StatusTagCoverageRow shield));
            Assert.AreEqual(StatusTargetType.Cat, shield.TargetType);
        }

        [Test]
        public void EvaluatePrototypeCatalog_ReportsSkillSourcesAndRuntimeResponses()
        {
            P0StatusTagCoverageReport report = P0StatusTagCoverage.EvaluatePrototypeCatalog();

            Assert.IsTrue(report.TryGetRow(StatusTagIds.Mark, out P0StatusTagCoverageRow mark));
            CollectionAssert.Contains(mark.SourceSkillIds, "nephthys_royal_mark");
            StringAssert.Contains("承伤倍率提高", mark.RuntimeResponse);
            Assert.IsTrue(report.TryGetRow(StatusTagIds.Shield, out P0StatusTagCoverageRow shield));
            CollectionAssert.Contains(shield.SourceSkillIds, "saiban_oath_shield");
            StringAssert.Contains("猫护盾吸收", shield.RuntimeResponse);
        }

        [Test]
        public void Evaluate_MissingStatusDefinitionFailsCoverage()
        {
            List<StatusTagDefinition> statuses = new List<StatusTagDefinition>(P0PrototypeCatalog.CreateStatusTags());
            statuses.RemoveAll(status => status.Id == StatusTagIds.Mark);

            P0StatusTagCoverageReport report = P0StatusTagCoverage.Evaluate(
                statuses,
                P0PrototypeCatalog.CreateStarterSkills());

            Assert.IsFalse(report.IsComplete);
            Assert.Greater(report.FailureCount, 0);
            StringAssert.Contains("Missing status tag definition: mark", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingSourceSkillFailsCoverage()
        {
            List<SkillDefinition> skills = new List<SkillDefinition>(P0PrototypeCatalog.CreateStarterSkills());
            skills.RemoveAll(skill => skill.Id == "suzune_moon_torii");

            P0StatusTagCoverageReport report = P0StatusTagCoverage.Evaluate(
                P0PrototypeCatalog.CreateStatusTags(),
                skills);

            Assert.IsFalse(report.IsComplete);
            StringAssert.Contains("knockback is missing source skill suzune_moon_torii", report.BuildDetailedSummary());
            StringAssert.Contains("sleep_stable is missing source skill suzune_moon_torii", report.BuildDetailedSummary());
        }

        [Test]
        public void BuildSummary_ReportsCoverageCount()
        {
            P0StatusTagCoverageReport report = P0StatusTagCoverage.EvaluatePrototypeCatalog();

            StringAssert.Contains("complete for 5 tag", report.BuildSummary());
        }
    }
}
