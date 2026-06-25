using UnityEngine;

namespace TheCat.Gameplay
{
    public sealed class P0EnemyWarningIndicatorView : MonoBehaviour
    {
        private const int RingSegments = 48;
        private const float WarningY = 0.18f;
        private const float LineWidth = 0.07f;

        private LineRenderer ring;
        private LineRenderer line;
        private TextMesh label;
        private Material lineMaterial;
        private P0WorldVisualAssetView visualAssetView;

        public void Sync(P0EnemyWarningIndicatorState warning)
        {
            Sync(warning, true);
        }

        public void Sync(P0EnemyWarningIndicatorState warning, bool showTextLabel)
        {
            EnsureVisuals();
            if (!warning.HasWarning)
            {
                SetVisualsActive(false);
                return;
            }

            SyncRing(warning);
            SyncLine(warning);
            SyncLabel(warning, showTextLabel);
            SyncVisualAsset(warning);
        }

        private void EnsureVisuals()
        {
            if (lineMaterial == null)
            {
                lineMaterial = CreateMaterial();
            }

            if (ring == null)
            {
                ring = CreateLineRenderer("EnemyWarningRing", true);
            }

            if (line == null)
            {
                line = CreateLineRenderer("EnemyWarningLine", false);
            }

            if (label == null)
            {
                GameObject labelObject = new GameObject("EnemyWarningLabel");
                labelObject.transform.SetParent(transform, false);
                label = labelObject.AddComponent<TextMesh>();
                label.anchor = TextAnchor.MiddleCenter;
                label.alignment = TextAlignment.Center;
                label.characterSize = 0.13f;
                label.fontSize = 48;
            }

            if (visualAssetView == null)
            {
                GameObject visualObject = new GameObject("EnemyWarningWorldVisual");
                visualObject.transform.SetParent(transform, false);
                visualAssetView = visualObject.AddComponent<P0WorldVisualAssetView>();
            }
        }

        private LineRenderer CreateLineRenderer(string objectName, bool loop)
        {
            GameObject lineObject = new GameObject(objectName);
            lineObject.transform.SetParent(transform, false);
            LineRenderer renderer = lineObject.AddComponent<LineRenderer>();
            renderer.useWorldSpace = true;
            renderer.loop = loop;
            renderer.widthMultiplier = LineWidth;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.sharedMaterial = new Material(lineMaterial);
            return renderer;
        }

        private Material CreateMaterial()
        {
            Shader shader = Shader.Find("Sprites/Default");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            if (shader == null)
            {
                shader = Shader.Find("Unlit/Color");
            }

            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            Material material = new Material(shader);
            material.name = "P0EnemyWarningIndicator";
            return material;
        }

        private void SyncRing(P0EnemyWarningIndicatorState warning)
        {
            bool active = warning.UsesRing;
            SetRendererActive(ring, active);
            if (!active)
            {
                return;
            }

            ApplyColor(ring, P0EnemyWarningIndicatorPresenter.GetColor(warning.Kind));
            ring.positionCount = RingSegments;
            Vector3 origin = ToWorldPosition(warning.Origin);
            float radius = Mathf.Max(0.1f, warning.Radius);
            for (int i = 0; i < RingSegments; i++)
            {
                float angle = i * Mathf.PI * 2f / RingSegments;
                Vector3 position = origin + new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
                ring.SetPosition(i, position);
            }
        }

        private void SyncLine(P0EnemyWarningIndicatorState warning)
        {
            bool active = warning.UsesLine;
            SetRendererActive(line, active);
            if (!active)
            {
                return;
            }

            ApplyColor(line, P0EnemyWarningIndicatorPresenter.GetColor(warning.Kind));
            line.positionCount = 2;
            line.SetPosition(0, ToWorldPosition(warning.Origin));
            line.SetPosition(1, ToWorldPosition(warning.Target));
        }

        private void SyncLabel(P0EnemyWarningIndicatorState warning, bool showTextLabel)
        {
            label.gameObject.SetActive(showTextLabel);
            label.color = P0EnemyWarningIndicatorPresenter.GetColor(warning.Kind);
            label.text = warning.Label + "\n" + warning.RemainingSeconds.ToString("0.0") + "s";
            label.transform.position = ToWorldPosition(warning.Origin) + new Vector3(0f, 0.95f, 0f);
        }

        private void SyncVisualAsset(P0EnemyWarningIndicatorState warning)
        {
            if (visualAssetView == null)
            {
                return;
            }

            if (!warning.VisualAsset.HasAsset)
            {
                visualAssetView.Clear();
                return;
            }

            visualAssetView.transform.position = ToWorldPosition(warning.Origin) + new Vector3(0f, 0.2f, 0f);
            bool hasSprite = visualAssetView.SetAsset(
                warning.VisualAsset,
                P0EnemyWarningIndicatorPresenter.GetColor(warning.Kind),
                new Vector2(Mathf.Max(1.2f, warning.Radius * 2f), Mathf.Max(1.2f, warning.Radius * 2f)),
                40,
                Vector3.zero);
            visualAssetView.SetVisible(hasSprite);
        }

        private void SetVisualsActive(bool active)
        {
            SetRendererActive(ring, active);
            SetRendererActive(line, active);
            if (label != null && label.gameObject.activeSelf != active)
            {
                label.gameObject.SetActive(active);
            }

            if (visualAssetView != null)
            {
                visualAssetView.SetVisible(active);
            }
        }

        private static void ApplyColor(LineRenderer renderer, Color color)
        {
            if (renderer == null)
            {
                return;
            }

            renderer.startColor = color;
            renderer.endColor = color;
            if (renderer.sharedMaterial != null)
            {
                renderer.sharedMaterial.color = color;
            }
        }

        private static void SetRendererActive(LineRenderer renderer, bool active)
        {
            if (renderer != null && renderer.gameObject.activeSelf != active)
            {
                renderer.gameObject.SetActive(active);
            }
        }

        private static Vector3 ToWorldPosition(Vector2 navigationPosition)
        {
            return new Vector3(navigationPosition.x, WarningY, navigationPosition.y);
        }
    }
}
