using UnityEngine;

namespace TheCat.Gameplay
{
    public sealed class P0SkillIndicatorView : MonoBehaviour
    {
        private const int RingSegments = 64;
        private const float IndicatorY = 0.12f;
        private const float TargetY = 0.28f;
        private const float LineWidth = 0.02f;
        private const float TargetMarkerScale = 0.26f;
        private const float MissingCrossSize = 0.16f;

        private LineRenderer rangeRing;
        private LineRenderer targetLine;
        private LineRenderer missingCrossA;
        private LineRenderer missingCrossB;
        private Transform targetMarker;
        private Renderer targetMarkerRenderer;
        private Material lineMaterial;
        private Material markerMaterial;

        public void Sync(P0SkillIndicatorState indicator)
        {
            EnsureRenderers();

            if (!indicator.HasSkill)
            {
                SetVisualsActive(false);
                return;
            }

            SyncRangeRing(indicator);
            SyncTargetLine(indicator);
            SyncMissingTargetCross(indicator);
        }

        private void EnsureRenderers()
        {
            if (lineMaterial == null)
            {
                lineMaterial = CreateIndicatorMaterial("P0SkillIndicator_Line");
            }

            if (markerMaterial == null)
            {
                markerMaterial = CreateIndicatorMaterial("P0SkillIndicator_Target");
            }

            if (rangeRing == null)
            {
                rangeRing = CreateLineRenderer("SkillRangeRing", true);
            }

            if (targetLine == null)
            {
                targetLine = CreateLineRenderer("SkillTargetLine", false);
            }

            if (missingCrossA == null)
            {
                missingCrossA = CreateLineRenderer("SkillMissingTargetCrossA", false);
            }

            if (missingCrossB == null)
            {
                missingCrossB = CreateLineRenderer("SkillMissingTargetCrossB", false);
            }

            if (targetMarker == null)
            {
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.name = "SkillTargetMarker";
                marker.transform.SetParent(transform, false);
                marker.transform.localScale = Vector3.one * TargetMarkerScale;
                targetMarker = marker.transform;
                targetMarkerRenderer = marker.GetComponent<Renderer>();
                if (targetMarkerRenderer != null)
                {
                    targetMarkerRenderer.sharedMaterial = markerMaterial;
                }
            }
        }

        private LineRenderer CreateLineRenderer(string objectName, bool loop)
        {
            GameObject lineObject = new GameObject(objectName);
            lineObject.transform.SetParent(transform, false);
            LineRenderer line = lineObject.AddComponent<LineRenderer>();
            line.useWorldSpace = true;
            line.loop = loop;
            line.widthMultiplier = LineWidth;
            line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            line.receiveShadows = false;
            line.sharedMaterial = new Material(lineMaterial);
            return line;
        }

        private Material CreateIndicatorMaterial(string materialName)
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
            material.name = materialName;
            return material;
        }

        private void SyncRangeRing(P0SkillIndicatorState indicator)
        {
            bool active = indicator.ShowsRange;
            SetRendererActive(rangeRing, active);
            if (!active)
            {
                return;
            }

            Color color = GetRangeColor(indicator);
            ApplyColor(rangeRing, color);
            rangeRing.positionCount = RingSegments;
            Vector3 origin = ToWorldPosition(indicator.Origin, IndicatorY);
            for (int i = 0; i < RingSegments; i++)
            {
                float angle = i * Mathf.PI * 2f / RingSegments;
                Vector3 position = origin + new Vector3(
                    Mathf.Cos(angle) * indicator.Range,
                    0f,
                    Mathf.Sin(angle) * indicator.Range);
                rangeRing.SetPosition(i, position);
            }
        }

        private void SyncTargetLine(P0SkillIndicatorState indicator)
        {
            bool active = indicator.ShowsTarget;
            SetRendererActive(targetLine, active);
            if (targetMarker != null && targetMarker.gameObject.activeSelf != active)
            {
                targetMarker.gameObject.SetActive(active);
            }

            if (!active)
            {
                return;
            }

            Color color = indicator.CanCast
                ? new Color(0.22f, 0.85f, 0.72f, 0.34f)
                : new Color(0.48f, 0.55f, 0.68f, 0.24f);
            ApplyColor(targetLine, color);
            if (targetMarkerRenderer != null)
            {
                targetMarkerRenderer.sharedMaterial.color = color;
            }

            Vector3 origin = ToWorldPosition(indicator.Origin, TargetY);
            Vector3 target = ToWorldPosition(indicator.TargetPosition, TargetY);
            targetLine.positionCount = 2;
            targetLine.SetPosition(0, origin);
            targetLine.SetPosition(1, target);
            targetMarker.position = target;
        }

        private void SyncMissingTargetCross(P0SkillIndicatorState indicator)
        {
            bool active = indicator.RequiresEnemyTarget && !indicator.HasTarget;
            SetRendererActive(missingCrossA, active);
            SetRendererActive(missingCrossB, active);
            if (!active)
            {
                return;
            }

            Color color = new Color(0.78f, 0.48f, 0.38f, 0.28f);
            ApplyColor(missingCrossA, color);
            ApplyColor(missingCrossB, color);
            Vector3 origin = ToWorldPosition(indicator.Origin, TargetY);
            missingCrossA.positionCount = 2;
            missingCrossB.positionCount = 2;
            missingCrossA.SetPosition(0, origin + new Vector3(-MissingCrossSize, 0f, -MissingCrossSize));
            missingCrossA.SetPosition(1, origin + new Vector3(MissingCrossSize, 0f, MissingCrossSize));
            missingCrossB.SetPosition(0, origin + new Vector3(-MissingCrossSize, 0f, MissingCrossSize));
            missingCrossB.SetPosition(1, origin + new Vector3(MissingCrossSize, 0f, -MissingCrossSize));
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

        private static Color GetRangeColor(P0SkillIndicatorState indicator)
        {
            if (indicator.CanCast)
            {
                return new Color(0.18f, 0.78f, 0.46f, 0.3f);
            }

            return indicator.IsCoolingDown
                ? new Color(0.42f, 0.54f, 0.72f, 0.24f)
                : new Color(0.7f, 0.48f, 0.32f, 0.26f);
        }

        private void SetVisualsActive(bool active)
        {
            SetRendererActive(rangeRing, active);
            SetRendererActive(targetLine, active);
            SetRendererActive(missingCrossA, active);
            SetRendererActive(missingCrossB, active);
            if (targetMarker != null && targetMarker.gameObject.activeSelf != active)
            {
                targetMarker.gameObject.SetActive(active);
            }
        }

        private static void SetRendererActive(LineRenderer renderer, bool active)
        {
            if (renderer != null && renderer.gameObject.activeSelf != active)
            {
                renderer.gameObject.SetActive(active);
            }
        }

        private static Vector3 ToWorldPosition(Vector2 navigationPosition, float y)
        {
            return new Vector3(navigationPosition.x, y, navigationPosition.y);
        }
    }
}
