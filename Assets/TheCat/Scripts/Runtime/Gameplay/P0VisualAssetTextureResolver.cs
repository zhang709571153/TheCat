using TheCat.Data.Definitions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TheCat.Gameplay
{
    public static class P0VisualAssetTextureResolver
    {
        public static bool TryResolveTexture(
            P0VisualAssetReference asset,
            out Texture2D texture,
            out string message)
        {
            texture = null;
            if (!asset.HasAsset)
            {
                message = "Visual asset reference is empty.";
                return false;
            }

#if UNITY_EDITOR
            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(asset.UnityImportPath);
            if (texture != null)
            {
                message = asset.AssetId + " resolved to texture.";
                return true;
            }

            message = asset.AssetId + " could not resolve texture at " + asset.UnityImportPath + ".";
            return false;
#else
            message = asset.AssetId + " requires serialized Sprite binding outside the Unity editor.";
            return false;
#endif
        }

        public static bool CanResolveTexture(P0VisualAssetReference asset)
        {
            return TryResolveTexture(asset, out Texture2D texture, out string _)
                && texture != null;
        }

        public static bool TryResolveSprite(
            P0VisualAssetReference asset,
            out Sprite sprite,
            out string message)
        {
            sprite = null;
            if (!asset.HasAsset)
            {
                message = "Visual asset reference is empty.";
                return false;
            }

#if UNITY_EDITOR
            sprite = AssetDatabase.LoadAssetAtPath<Sprite>(asset.UnityImportPath);
            if (sprite != null)
            {
                message = asset.AssetId + " resolved to sprite.";
                return true;
            }

            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(asset.UnityImportPath);
            if (texture != null)
            {
                sprite = Sprite.Create(
                    texture,
                    new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100f);
                sprite.name = asset.AssetId + "_RuntimeSprite";
                message = asset.AssetId + " resolved to runtime sprite from texture.";
                return true;
            }

            message = asset.AssetId + " could not resolve sprite at " + asset.UnityImportPath + ".";
            return false;
#else
            message = asset.AssetId + " requires serialized Sprite binding outside the Unity editor.";
            return false;
#endif
        }

        public static bool CanResolveSprite(P0VisualAssetReference asset)
        {
            return TryResolveSprite(asset, out Sprite sprite, out string _)
                && sprite != null;
        }
    }
}
