using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public sealed class P0WorldVisualAssetView : MonoBehaviour
    {
        private const string SpriteObjectName = "RuntimeSpriteVisual";

        private SpriteRenderer spriteRenderer;
        private P0VisualAssetReference currentAsset;
        private string lastResolveMessage = string.Empty;

        public P0VisualAssetReference CurrentAsset => currentAsset;

        public string LastResolveMessage => lastResolveMessage;

        public bool HasSprite => spriteRenderer != null
            && spriteRenderer.enabled
            && spriteRenderer.sprite != null
            && spriteRenderer.gameObject.activeSelf;

        public bool SetAsset(
            P0VisualAssetReference asset,
            Color fallbackTint,
            Vector2 targetWorldSize,
            int sortingOrder,
            Vector3 localOffset)
        {
            _ = fallbackTint;
            currentAsset = asset;
            EnsureSpriteRenderer();

            if (!P0VisualAssetTextureResolver.TryResolveSprite(asset, out Sprite sprite, out lastResolveMessage))
            {
                ClearSprite();
                return false;
            }

            spriteRenderer.sprite = sprite;
            spriteRenderer.color = Color.white;
            spriteRenderer.sortingOrder = sortingOrder;
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.enabled = true;
            spriteRenderer.transform.localPosition = localOffset;
            spriteRenderer.transform.localRotation = Quaternion.identity;
            spriteRenderer.transform.localScale = CalculateFitScale(sprite, targetWorldSize);
            return true;
        }

        public void Clear()
        {
            currentAsset = default(P0VisualAssetReference);
            lastResolveMessage = string.Empty;
            ClearSprite();
        }

        public void SetVisible(bool visible)
        {
            if (spriteRenderer != null && spriteRenderer.gameObject.activeSelf != visible)
            {
                spriteRenderer.gameObject.SetActive(visible);
            }
        }

        private void EnsureSpriteRenderer()
        {
            if (spriteRenderer != null)
            {
                return;
            }

            Transform existing = transform.Find(SpriteObjectName);
            if (existing != null)
            {
                spriteRenderer = existing.GetComponent<SpriteRenderer>();
            }

            if (spriteRenderer == null)
            {
                GameObject spriteObject = new GameObject(SpriteObjectName);
                spriteObject.transform.SetParent(transform, false);
                spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
            }

            spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            spriteRenderer.receiveShadows = false;
        }

        private void ClearSprite()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            spriteRenderer.sprite = null;
            spriteRenderer.enabled = false;
            spriteRenderer.gameObject.SetActive(false);
        }

        private static Vector3 CalculateFitScale(Sprite sprite, Vector2 targetWorldSize)
        {
            if (sprite == null)
            {
                return Vector3.one;
            }

            Vector2 spriteSize = sprite.bounds.size;
            float targetWidth = Mathf.Max(0.01f, targetWorldSize.x);
            float targetHeight = Mathf.Max(0.01f, targetWorldSize.y);
            float widthScale = spriteSize.x <= 0f ? 1f : targetWidth / spriteSize.x;
            float heightScale = spriteSize.y <= 0f ? 1f : targetHeight / spriteSize.y;
            float scale = Mathf.Min(widthScale, heightScale);
            return new Vector3(scale, scale, 1f);
        }
    }
}
