using NUnit.Framework;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0StatusIndicatorViewTests
    {
        [Test]
        public void Sync_StatusStateShowsBadgeAndLabel()
        {
            GameObject root = new GameObject("StatusIndicatorViewTest");
            try
            {
                P0StatusIndicatorView view = root.AddComponent<P0StatusIndicatorView>();
                view.SetLocalOffset(new Vector3(0f, 2f, 0f));
                view.Sync(new P0StatusIndicatorState(
                    "护盾 强度 35 6.0s",
                    P0StatusIndicatorPresenter.GetAccentColor("shield"),
                    1));

                AssertChildActive(root.transform, "StatusBadge", true);
                AssertChildActive(root.transform, "StatusLabel", true);
                Assert.AreEqual(new Vector3(0f, 2f, 0f), root.transform.localPosition);
                TextMesh label = root.transform.Find("StatusLabel").GetComponent<TextMesh>();
                StringAssert.Contains("护盾", label.text);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Sync_EmptyStateHidesBadgeAndLabel()
        {
            GameObject root = new GameObject("StatusIndicatorViewTest");
            try
            {
                P0StatusIndicatorView view = root.AddComponent<P0StatusIndicatorView>();
                view.Sync(default(P0StatusIndicatorState));

                AssertChildActive(root.transform, "StatusBadge", false);
                AssertChildActive(root.transform, "StatusLabel", false);
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
