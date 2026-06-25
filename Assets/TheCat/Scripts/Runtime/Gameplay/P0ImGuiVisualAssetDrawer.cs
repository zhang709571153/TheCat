using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public static class P0ImGuiVisualAssetDrawer
    {
        private static GUIStyle texturedButtonStyle;
        private static GUIStyle routeChoiceCardButtonStyle;
        private static GUIStyle gaugeBarLabelStyle;

        public static bool TryResolveTexture(P0VisualAssetReference asset, out Texture2D texture)
        {
            return P0VisualAssetTextureResolver.TryResolveTexture(asset, out texture, out string _)
                && texture != null;
        }

        public static bool DrawTexture(
            P0VisualAssetReference asset,
            Rect rect,
            ScaleMode scaleMode = ScaleMode.ScaleToFit,
            bool alphaBlend = true)
        {
            if (!TryResolveTexture(asset, out Texture2D texture))
            {
                return false;
            }

            GUI.DrawTexture(rect, texture, scaleMode, alphaBlend);
            return true;
        }

        public static bool DrawGUILayoutTexture(
            P0VisualAssetReference asset,
            float width,
            float height,
            ScaleMode scaleMode = ScaleMode.ScaleToFit)
        {
            Rect rect = GUILayoutUtility.GetRect(
                width,
                height,
                GUILayout.Width(width),
                GUILayout.Height(height));
            return DrawTexture(asset, rect, scaleMode);
        }

        public static bool DrawTexturedButton(
            P0VisualAssetReference backgroundAsset,
            string label,
            float height)
        {
            Rect rect = GUILayoutUtility.GetRect(
                1f,
                height,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(height));

            DrawTexture(backgroundAsset, rect, ScaleMode.StretchToFill);
            return GUI.Button(rect, label, TexturedButtonStyle);
        }

        public static bool DrawRouteChoiceCardButton(
            P0VisualAssetReference frameAsset,
            P0VisualAssetReference iconAsset,
            string label,
            float height)
        {
            return DrawRouteChoiceCardButton(
                frameAsset,
                iconAsset,
                default(P0VisualAssetReference),
                label,
                height);
        }

        public static bool DrawRouteChoiceCardButton(
            P0VisualAssetReference frameAsset,
            P0VisualAssetReference iconAsset,
            P0VisualAssetReference detailBadgeAsset,
            string label,
            float height)
        {
            Rect rect = GUILayoutUtility.GetRect(
                1f,
                height,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(height));

            DrawTexture(frameAsset, rect, ScaleMode.StretchToFill);
            if (TryResolveTexture(iconAsset, out Texture2D icon))
            {
                float iconSize = Mathf.Min(P0ImGuiLayout.Scaled(38f), height - P0ImGuiLayout.Scaled(16f));
                Rect iconRect = new Rect(rect.x + P0ImGuiLayout.Scaled(18f), rect.y + (height - iconSize) * 0.5f, iconSize, iconSize);
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit, true);
            }

            if (TryResolveTexture(detailBadgeAsset, out Texture2D badge))
            {
                float badgeHeight = Mathf.Min(P0ImGuiLayout.Scaled(34f), height - P0ImGuiLayout.Scaled(14f));
                float badgeWidth = badgeHeight * 3f;
                Rect badgeRect = new Rect(
                    rect.xMax - badgeWidth - P0ImGuiLayout.Scaled(14f),
                    rect.y + (height - badgeHeight) * 0.5f,
                    badgeWidth,
                    badgeHeight);
                GUI.DrawTexture(badgeRect, badge, ScaleMode.ScaleToFit, true);
            }

            return GUI.Button(rect, label, RouteChoiceCardButtonStyle);
        }

        public static bool DrawIcon(P0VisualAssetReference asset, float size)
        {
            if (!TryResolveTexture(asset, out Texture2D texture))
            {
                return false;
            }

            GUILayout.Label(
                new GUIContent(texture, asset.AssetId),
                GUILayout.Width(size),
                GUILayout.Height(size));
            return true;
        }

        public static bool DrawInlineIcon(P0VisualAssetReference asset, float size)
        {
            if (!TryResolveTexture(asset, out Texture2D texture))
            {
                return false;
            }

            Rect rect = GUILayoutUtility.GetRect(size, size, GUILayout.Width(size), GUILayout.Height(size));
            GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit, true);
            return true;
        }

        public static bool DrawGaugeBar(
            P0VisualAssetReference frameAsset,
            P0VisualAssetReference fillAsset,
            string label,
            float fillRatio,
            float height)
        {
            Rect rect = GUILayoutUtility.GetRect(
                1f,
                height,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(height));
            bool drewAny = false;

            if (TryResolveTexture(fillAsset, out Texture2D fillTexture))
            {
                float inset = Mathf.Min(6f, height * 0.25f);
                Rect fillRect = new Rect(
                    rect.x + inset,
                    rect.y + inset,
                    Mathf.Max(0f, (rect.width - inset * 2f) * Mathf.Clamp01(fillRatio)),
                    Mathf.Max(0f, rect.height - inset * 2f));
                GUI.BeginGroup(fillRect);
                GUI.DrawTexture(
                    new Rect(0f, 0f, Mathf.Max(0f, rect.width - inset * 2f), fillRect.height),
                    fillTexture,
                    ScaleMode.StretchToFill,
                    true);
                GUI.EndGroup();
                drewAny = true;
            }

            if (DrawTexture(frameAsset, rect, ScaleMode.StretchToFill))
            {
                drewAny = true;
            }

            if (!string.IsNullOrWhiteSpace(label))
            {
                GUI.Label(rect, label, GaugeBarLabelStyle);
            }

            return drewAny;
        }

        private static GUIStyle TexturedButtonStyle
        {
            get
            {
                if (texturedButtonStyle == null)
                {
                    texturedButtonStyle = new GUIStyle(GUI.skin.button)
                    {
                        wordWrap = true
                    };
                    texturedButtonStyle.normal.background = null;
                    texturedButtonStyle.hover.background = null;
                    texturedButtonStyle.active.background = null;
                    texturedButtonStyle.focused.background = null;
                    texturedButtonStyle.normal.textColor = Color.white;
                    texturedButtonStyle.hover.textColor = Color.white;
                    texturedButtonStyle.active.textColor = Color.white;
                    texturedButtonStyle.focused.textColor = Color.white;
                }

                return texturedButtonStyle;
            }
        }

        private static GUIStyle RouteChoiceCardButtonStyle
        {
            get
            {
                if (routeChoiceCardButtonStyle == null)
                {
                    routeChoiceCardButtonStyle = new GUIStyle(GUI.skin.button)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        wordWrap = true
                    };
                    routeChoiceCardButtonStyle.normal.background = null;
                    routeChoiceCardButtonStyle.hover.background = null;
                    routeChoiceCardButtonStyle.active.background = null;
                    routeChoiceCardButtonStyle.focused.background = null;
                    routeChoiceCardButtonStyle.normal.textColor = Color.white;
                    routeChoiceCardButtonStyle.hover.textColor = Color.white;
                    routeChoiceCardButtonStyle.active.textColor = Color.white;
                    routeChoiceCardButtonStyle.focused.textColor = Color.white;
                }

                routeChoiceCardButtonStyle.padding = new RectOffset(
                    Mathf.RoundToInt(P0ImGuiLayout.Scaled(72f)),
                    Mathf.RoundToInt(P0ImGuiLayout.Scaled(126f)),
                    Mathf.RoundToInt(P0ImGuiLayout.Scaled(8f)),
                    Mathf.RoundToInt(P0ImGuiLayout.Scaled(8f)));
                return routeChoiceCardButtonStyle;
            }
        }

        private static GUIStyle GaugeBarLabelStyle
        {
            get
            {
                if (gaugeBarLabelStyle == null)
                {
                    gaugeBarLabelStyle = new GUIStyle(GUI.skin.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 10,
                        wordWrap = false
                    };
                    gaugeBarLabelStyle.normal.textColor = Color.white;
                }

                return gaugeBarLabelStyle;
            }
        }
    }
}
