using NUnit.Framework;
using TheCat.Data;

namespace TheCat.Tests
{
    public sealed class StatusTagDefinitionTests
    {
        [Test]
        public void StatusTagIds_RecognizesP0Tags()
        {
            Assert.IsTrue(StatusTagIds.IsP0Tag(StatusTagIds.SleepStable));
            Assert.IsTrue(StatusTagIds.IsP0Tag(StatusTagIds.Slow));
            Assert.IsTrue(StatusTagIds.IsP0Tag(StatusTagIds.Knockback));
            Assert.IsTrue(StatusTagIds.IsP0Tag(StatusTagIds.Mark));
            Assert.IsTrue(StatusTagIds.IsP0Tag(StatusTagIds.Shield));
            Assert.IsFalse(StatusTagIds.IsP0Tag("reflect"));
        }

        [Test]
        public void Definition_ReportsP0Tag()
        {
            StatusTagDefinition definition = new StatusTagDefinition(
                StatusTagIds.Shield,
                "Shield",
                StatusTargetType.Cat,
                5f,
                20f,
                StatusStackPolicy.HighestMagnitude,
                "silver_edge");

            Assert.IsTrue(definition.IsP0Tag);
            Assert.AreEqual(StatusTargetType.Cat, definition.TargetType);
        }
    }
}
