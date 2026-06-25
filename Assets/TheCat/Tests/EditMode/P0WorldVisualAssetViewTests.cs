using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0WorldVisualAssetViewTests
    {
        [Test]
        public void SetAsset_WithManifestSprite_EnablesSpriteRenderer()
        {
            GameObject root = new GameObject("P0WorldVisualAssetViewTest");
            try
            {
                P0WorldVisualAssetView view = root.AddComponent<P0WorldVisualAssetView>();

                bool resolved = view.SetAsset(
                    P0VisualAssetCatalog.GetBedSprite(),
                    Color.white,
                    new Vector2(1.4f, 1.4f),
                    12,
                    new Vector3(0f, 0.2f, 0f));

                Assert.IsTrue(resolved, view.LastResolveMessage);
                Assert.IsTrue(view.HasSprite, view.LastResolveMessage);
                Assert.AreEqual(P0VisualAssetCatalog.BedSpriteId, view.CurrentAsset.AssetId);

                SpriteRenderer renderer = root.GetComponentInChildren<SpriteRenderer>();
                Assert.NotNull(renderer);
                Assert.AreEqual(12, renderer.sortingOrder);
                Assert.AreEqual(new Vector3(0f, 0.2f, 0f), renderer.transform.localPosition);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Clear_DisablesSpriteRendererAndClearsAsset()
        {
            GameObject root = new GameObject("P0WorldVisualAssetViewClearTest");
            try
            {
                P0WorldVisualAssetView view = root.AddComponent<P0WorldVisualAssetView>();
                Assert.IsTrue(view.SetAsset(
                    P0VisualAssetCatalog.GetFeederSprite(),
                    Color.white,
                    Vector2.one,
                    3,
                    Vector3.zero));

                view.Clear();

                Assert.IsFalse(view.HasSprite);
                Assert.IsFalse(view.CurrentAsset.HasAsset);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void SetAsset_WithTextureOnlyConcept_CreatesRuntimeSprite()
        {
            GameObject root = new GameObject("P0WorldVisualAssetViewConceptTest");
            try
            {
                P0WorldVisualAssetView view = root.AddComponent<P0WorldVisualAssetView>();

                bool resolved = view.SetAsset(
                    P0VisualAssetCatalog.GetCallTyrantConcept(),
                    Color.red,
                    new Vector2(1.5f, 1.5f),
                    18,
                    Vector3.zero);

                Assert.IsTrue(resolved, view.LastResolveMessage);
                Assert.IsTrue(view.HasSprite, view.LastResolveMessage);
                Assert.AreEqual(P0VisualAssetCatalog.CallTyrantConceptId, view.CurrentAsset.AssetId);
                StringAssert.Contains("runtime sprite from texture", view.LastResolveMessage);
            }
            finally
            {
                Object.DestroyImmediate(root);
            }
        }
    }
}
