using NUnit.Framework;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0EnemyWarningIndicatorViewTests
    {
        [Test]
        public void Sync_LineWarningShowsLineAndLabel()
        {
            GameObject root = new GameObject("EnemyWarningViewTest");
            try
            {
                P0EnemyWarningIndicatorView view = root.AddComponent<P0EnemyWarningIndicatorView>();
                view.Sync(new P0EnemyWarningIndicatorState(
                    P0EnemyWarningKind.ChargeLane,
                    "冲锋路线",
                    3f,
                    Vector2.zero,
                    new Vector2(0f, -3f),
                    0.5f));

                AssertChildActive(root.transform, "EnemyWarningRing", false);
                AssertChildActive(root.transform, "EnemyWarningLine", true);
                AssertChildActive(root.transform, "EnemyWarningLabel", true);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_RingWarningShowsRingAndLabel()
        {
            GameObject root = new GameObject("EnemyWarningViewTest");
            try
            {
                P0EnemyWarningIndicatorView view = root.AddComponent<P0EnemyWarningIndicatorView>();
                view.Sync(new P0EnemyWarningIndicatorState(
                    P0EnemyWarningKind.JumpSlam,
                    "跳砸",
                    4f,
                    Vector2.zero,
                    Vector2.zero,
                    1.35f));

                AssertChildActive(root.transform, "EnemyWarningRing", true);
                AssertChildActive(root.transform, "EnemyWarningLine", false);
                AssertChildActive(root.transform, "EnemyWarningLabel", true);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_CanHideTextLabelWhileKeepingWarningVisuals()
        {
            GameObject root = new GameObject("EnemyWarningViewTest");
            try
            {
                P0EnemyWarningIndicatorView view = root.AddComponent<P0EnemyWarningIndicatorView>();
                view.Sync(
                    new P0EnemyWarningIndicatorState(
                        P0EnemyWarningKind.ChargeLane,
                        "鍐查攱璺嚎",
                        3f,
                        Vector2.zero,
                        new Vector2(0f, -3f),
                        0.5f),
                    false);

                AssertChildActive(root.transform, "EnemyWarningRing", false);
                AssertChildActive(root.transform, "EnemyWarningLine", true);
                AssertChildActive(root.transform, "EnemyWarningLabel", false);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_NoWarningHidesAllVisuals()
        {
            GameObject root = new GameObject("EnemyWarningViewTest");
            try
            {
                P0EnemyWarningIndicatorView view = root.AddComponent<P0EnemyWarningIndicatorView>();
                view.Sync(default(P0EnemyWarningIndicatorState));

                AssertChildActive(root.transform, "EnemyWarningRing", false);
                AssertChildActive(root.transform, "EnemyWarningLine", false);
                AssertChildActive(root.transform, "EnemyWarningLabel", false);
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
