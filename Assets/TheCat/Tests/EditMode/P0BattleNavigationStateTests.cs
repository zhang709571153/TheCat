using NUnit.Framework;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0BattleNavigationStateTests
    {
        [Test]
        public void Move_NormalizesDiagonalInputAndAppliesSpeedMultiplier()
        {
            P0BattleNavigationState navigation = new P0BattleNavigationState(
                Vector2.zero,
                new Vector2(-10f, -10f),
                new Vector2(10f, 10f));

            Vector2 position = navigation.Move(new Vector2(1f, 1f), 1f, 0.5f);
            float expectedAxisDistance = P0BattleNavigationState.BaseMoveSpeed * 0.5f / Mathf.Sqrt(2f);

            Assert.AreEqual(expectedAxisDistance, position.x, 0.001f);
            Assert.AreEqual(expectedAxisDistance, position.y, 0.001f);
        }

        [Test]
        public void Move_ClampsToArenaBounds()
        {
            P0BattleNavigationState navigation = new P0BattleNavigationState(
                Vector2.zero,
                new Vector2(-1f, -1f),
                new Vector2(1f, 1f));

            navigation.Move(new Vector2(1f, 1f), 10f, 1f);

            Assert.AreEqual(1f, navigation.Position.x, 0.001f);
            Assert.AreEqual(1f, navigation.Position.y, 0.001f);

            navigation.ResetTo(new Vector2(-99f, 99f));

            Assert.AreEqual(-1f, navigation.Position.x, 0.001f);
            Assert.AreEqual(1f, navigation.Position.y, 0.001f);
        }

        [Test]
        public void IsWithinRange_GatesBedAndUtilityInteractions()
        {
            P0BattleNavigationState navigation = new P0BattleNavigationState();
            Vector2 bedPosition = new Vector2(0f, -3.5f);
            Vector2 litterBoxPosition = new Vector2(-3.4f, -2.8f);

            Assert.IsTrue(navigation.IsWithinRange(bedPosition, P0BattleNavigationState.DefaultBedCareRange));
            Assert.IsFalse(navigation.IsWithinRange(litterBoxPosition, P0BattleNavigationState.DefaultInteractionRange));

            navigation.ResetTo(new Vector2(-3.35f, -2.75f));

            Assert.IsTrue(navigation.IsWithinRange(litterBoxPosition, P0BattleNavigationState.DefaultInteractionRange));
        }

        [Test]
        public void BuildDistanceSummary_IncludesAllGrayboxTargets()
        {
            P0BattleNavigationState navigation = new P0BattleNavigationState();

            string summary = navigation.BuildDistanceSummary(
                new Vector2(0f, -3.5f),
                new Vector2(-3.4f, -2.8f),
                new Vector2(3.4f, -2.8f));

            StringAssert.Contains("Position", summary);
            StringAssert.Contains("Bed", summary);
            StringAssert.Contains("Litter", summary);
            StringAssert.Contains("Feeder", summary);
        }
    }
}
