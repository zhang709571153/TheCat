using NUnit.Framework;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0SkillIndicatorViewTests
    {
        [Test]
        public void Sync_TargetReadyShowsRangeLineAndMarker()
        {
            GameObject root = new GameObject("IndicatorViewTest");
            try
            {
                P0SkillIndicatorView view = root.AddComponent<P0SkillIndicatorView>();
                view.Sync(new P0SkillIndicatorState(
                    "nephthys_royal_mark",
                    "王权标记",
                    0f,
                    requiresEnemyTarget: true,
                    targetDisplayName: "冷光灯影",
                    origin: Vector2.zero,
                    targetPosition: new Vector2(2f, 0f),
                    distance: 2f,
                    range: 5f));

                AssertChildActive(root.transform, "SkillRangeRing", true);
                AssertChildActive(root.transform, "SkillTargetLine", true);
                AssertChildActive(root.transform, "SkillTargetMarker", true);
                AssertChildActive(root.transform, "SkillMissingTargetCrossA", false);
                AssertChildActive(root.transform, "SkillMissingTargetCrossB", false);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_MissingTargetShowsRangeAndCross()
        {
            GameObject root = new GameObject("IndicatorViewTest");
            try
            {
                P0SkillIndicatorView view = root.AddComponent<P0SkillIndicatorView>();
                view.Sync(new P0SkillIndicatorState(
                    "saiban_sword_sweep",
                    "圆盾冲锋",
                    0f,
                    requiresEnemyTarget: true,
                    targetDisplayName: string.Empty,
                    origin: Vector2.zero,
                    targetPosition: Vector2.zero,
                    distance: 0f,
                    range: 2.35f));

                AssertChildActive(root.transform, "SkillRangeRing", true);
                AssertChildActive(root.transform, "SkillTargetLine", false);
                AssertChildActive(root.transform, "SkillTargetMarker", false);
                AssertChildActive(root.transform, "SkillMissingTargetCrossA", true);
                AssertChildActive(root.transform, "SkillMissingTargetCrossB", true);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_NoEnemyTargetSkillHidesRangeAndTargetVisuals()
        {
            GameObject root = new GameObject("IndicatorViewTest");
            try
            {
                P0SkillIndicatorView view = root.AddComponent<P0SkillIndicatorView>();
                view.Sync(new P0SkillIndicatorState(
                    "saiban_oath_shield",
                    "银誓护盾",
                    0f,
                    requiresEnemyTarget: false,
                    targetDisplayName: string.Empty,
                    origin: Vector2.zero,
                    targetPosition: Vector2.zero,
                    distance: 0f,
                    range: 0f));

                AssertChildActive(root.transform, "SkillRangeRing", false);
                AssertChildActive(root.transform, "SkillTargetLine", false);
                AssertChildActive(root.transform, "SkillTargetMarker", false);
                AssertChildActive(root.transform, "SkillMissingTargetCrossA", false);
                AssertChildActive(root.transform, "SkillMissingTargetCrossB", false);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_MissingSkillHidesAllVisuals()
        {
            GameObject root = new GameObject("IndicatorViewTest");
            try
            {
                P0SkillIndicatorView view = root.AddComponent<P0SkillIndicatorView>();
                view.Sync(default(P0SkillIndicatorState));

                AssertChildActive(root.transform, "SkillRangeRing", false);
                AssertChildActive(root.transform, "SkillTargetLine", false);
                AssertChildActive(root.transform, "SkillTargetMarker", false);
                AssertChildActive(root.transform, "SkillMissingTargetCrossA", false);
                AssertChildActive(root.transform, "SkillMissingTargetCrossB", false);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        private static void AssertChildActive(Transform parent, string childName, bool expected)
        {
            Transform child = parent.Find(childName);
            Assert.IsNotNull(child, childName);
            Assert.AreEqual(expected, child.gameObject.activeSelf, childName);
        }
    }
}
