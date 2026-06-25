using UnityEngine;

namespace TheCat.Gameplay
{
    public sealed class P0StatusIndicatorView : MonoBehaviour
    {
        private Vector3 localOffset = new Vector3(0f, 1.45f, 0f);
        private TextMesh label;
        private Transform badge;
        private Renderer badgeRenderer;
        private Material badgeMaterial;

        public void SetLocalOffset(Vector3 offset)
        {
            localOffset = offset;
            transform.localPosition = localOffset;
        }

        public void Sync(P0StatusIndicatorState state)
        {
            EnsureVisuals();
            transform.localPosition = localOffset;
            if (!state.HasStatuses)
            {
                SetVisualsActive(false);
                return;
            }

            SetVisualsActive(true);
            label.text = state.Text;
            label.color = state.AccentColor;
            if (badgeRenderer != null)
            {
                badgeRenderer.material.color = state.AccentColor;
            }
        }

        private void EnsureVisuals()
        {
            if (badgeMaterial == null)
            {
                badgeMaterial = CreateMaterial();
            }

            if (badge == null)
            {
                GameObject badgeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                badgeObject.name = "StatusBadge";
                badgeObject.transform.SetParent(transform, false);
                badgeObject.transform.localPosition = new Vector3(0f, -0.08f, 0f);
                badgeObject.transform.localScale = new Vector3(0.55f, 0.08f, 0.08f);
                Component collider = badgeObject.GetComponent("Collider");
                if (collider != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(collider);
                    }
                    else
                    {
                        DestroyImmediate(collider);
                    }
                }

                badge = badgeObject.transform;
                badgeRenderer = badgeObject.GetComponent<Renderer>();
                if (badgeRenderer != null)
                {
                    badgeRenderer.material = badgeMaterial;
                }
            }

            if (label == null)
            {
                GameObject labelObject = new GameObject("StatusLabel");
                labelObject.transform.SetParent(transform, false);
                labelObject.transform.localPosition = Vector3.zero;
                label = labelObject.AddComponent<TextMesh>();
                label.anchor = TextAnchor.MiddleCenter;
                label.alignment = TextAlignment.Center;
                label.characterSize = 0.105f;
                label.fontSize = 48;
            }
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
            material.name = "P0StatusIndicator";
            return material;
        }

        private void SetVisualsActive(bool active)
        {
            if (badge != null && badge.gameObject.activeSelf != active)
            {
                badge.gameObject.SetActive(active);
            }

            if (label != null && label.gameObject.activeSelf != active)
            {
                label.gameObject.SetActive(active);
            }
        }
    }
}
