using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0StatusIndicatorPresenterTests
    {
        [Test]
        public void Build_NoStatusesReturnsEmptyState()
        {
            P0StatusIndicatorState state = P0StatusIndicatorPresenter.Build(
                new StatusEffectCollection(),
                "Enemy");

            Assert.IsFalse(state.HasStatuses);
            Assert.AreEqual(0, state.Count);
        }

        [Test]
        public void Build_UsesDisplayNameVisualTokenMagnitudeAndOwnerLabel()
        {
            StatusEffectCollection statuses = new StatusEffectCollection();
            statuses.Apply(new StatusTagDefinition(
                StatusTagIds.Mark,
                "标记",
                StatusTargetType.Enemy,
                5f,
                0.25f,
                StatusStackPolicy.RefreshDuration,
                "royal_eye"));

            P0StatusIndicatorState state = P0StatusIndicatorPresenter.Build(statuses, "冷光灯影");

            Assert.IsTrue(state.HasStatuses);
            Assert.AreEqual(1, state.Count);
            StringAssert.Contains("冷光灯影", state.Text);
            StringAssert.Contains("标记 强度 0.25 5.0s", state.Text);
        }

        [Test]
        public void Build_PrioritizesShieldAndCoversP0Tokens()
        {
            StatusEffectCollection statuses = new StatusEffectCollection();
            statuses.Apply(new StatusTagDefinition(StatusTagIds.SleepStable, "安眠", StatusTargetType.BedZone, 8f, 1f, StatusStackPolicy.RefreshDuration, "soft_blue_note"));
            statuses.Apply(new StatusTagDefinition(StatusTagIds.Slow, "缓速", StatusTargetType.Enemy, 4f, 0.35f, StatusStackPolicy.HighestMagnitude, "moon_sand"));
            statuses.Apply(new StatusTagDefinition(StatusTagIds.Knockback, "击退", StatusTargetType.Enemy, 0.25f, 1f, StatusStackPolicy.RefreshDuration, "silver_impact"));
            statuses.Apply(new StatusTagDefinition(StatusTagIds.Mark, "标记", StatusTargetType.Enemy, 5f, 0.25f, StatusStackPolicy.RefreshDuration, "royal_eye"));
            statuses.Apply(new StatusTagDefinition(StatusTagIds.Shield, "护盾", StatusTargetType.Cat, 6f, 35f, StatusStackPolicy.HighestMagnitude, "oath_edge"));

            P0StatusIndicatorState state = P0StatusIndicatorPresenter.Build(statuses);

            Assert.AreEqual(5, state.Count);
            StringAssert.StartsWith("护盾 强度", state.Text);
            StringAssert.Contains("安眠 强度", state.Text);
            StringAssert.Contains("缓速 强度", state.Text);
            StringAssert.Contains("击退 强度", state.Text);
            StringAssert.Contains("标记 强度", state.Text);
            Assert.AreEqual(P0StatusIndicatorPresenter.GetAccentColor(StatusTagIds.Shield), state.AccentColor);
        }
    }
}
