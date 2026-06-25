using System;
using NUnit.Framework;
using TheCat.Data;

namespace TheCat.Tests
{
    public sealed class P0TuningTests
    {
        [Test]
        public void Default_UsesLightPunishmentP0Values()
        {
            P0Tuning tuning = P0Tuning.Default;

            Assert.AreEqual(1f, tuning.OwnerSleepDamageMultiplier);
            Assert.AreEqual(10, tuning.PoopSleepMaxPenalty);
            Assert.AreEqual(0.3f, tuning.PoopNaturalGrowthPerSecond);
            Assert.AreEqual(2f, tuning.DigestionPoopMultiplier);
            Assert.AreEqual(60f, tuning.LitterBoxPoopReduction);
            Assert.AreEqual(1f, tuning.HungerDrainMultiplier);
            Assert.AreEqual(1f, tuning.LayerDifficultyMultiplier);
        }

        [Test]
        public void Constructor_RejectsInvalidKnobs()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0Tuning(0f, 10, 0.3f, 2f, 60f, 1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0Tuning(1f, -1, 0.3f, 2f, 60f, 1f, 1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => new P0Tuning(1f, 10, 0f, 2f, 60f, 1f, 1f));
        }

        [Test]
        public void With_ReplacesOnlySpecifiedKnobs()
        {
            P0Tuning tuning = P0Tuning.Default.With(layerDifficultyMultiplier: 1.25f);

            Assert.AreEqual(10, tuning.PoopSleepMaxPenalty);
            Assert.AreEqual(1.25f, tuning.LayerDifficultyMultiplier);
        }
    }
}
