using System.Text;
using TheCat.Combat;
using TheCat.Data.Definitions;
using UnityEngine;

namespace TheCat.Gameplay
{
    public sealed class GrayboxEnemyView : MonoBehaviour
    {
        private readonly StringBuilder labelBuilder = new StringBuilder();

        private BattleEnemyState state;
        private Vector3 spawnPosition;
        private Vector3 bedPosition;
        private float initialTimeToBedSeconds;
        private Renderer cachedRenderer;
        private TextMesh label;
        private P0WorldVisualAssetView visualAssetView;
        private P0EnemyWarningIndicatorView warningIndicatorView;
        private P0StatusIndicatorView statusIndicatorView;

        public int InstanceId => state == null ? 0 : state.InstanceId;

        public void ResetForPool()
        {
            state = null;
            if (label != null)
            {
                label.text = string.Empty;
            }

            if (statusIndicatorView != null)
            {
                statusIndicatorView.Sync(default(P0StatusIndicatorState));
            }

            if (warningIndicatorView != null)
            {
                warningIndicatorView.Sync(default(P0EnemyWarningIndicatorState));
            }

            if (visualAssetView != null)
            {
                visualAssetView.Clear();
            }

            SetFallbackRendererActive(true);
        }

        public void Initialize(
            BattleEnemyState enemyState,
            Vector3 spawn,
            Vector3 bed,
            Color color,
            P0VisualAssetReference visualAsset,
            bool showDiagnosticLabels = true,
            bool showWarningIndicators = true)
        {
            state = enemyState;
            spawnPosition = spawn;
            bedPosition = bed;
            initialTimeToBedSeconds = Mathf.Max(0.1f, enemyState.TimeToBedSeconds);
            cachedRenderer = GetComponent<Renderer>();
            if (cachedRenderer != null)
            {
                MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
                propertyBlock.SetColor("_Color", color);
                propertyBlock.SetColor("_BaseColor", color);
                cachedRenderer.SetPropertyBlock(propertyBlock);
            }

            bool hasSprite = SyncVisualAsset(visualAsset, color);
            SetFallbackRendererActive(!hasSprite);
            EnsureLabel();
            Sync(showDiagnosticLabels, showWarningIndicators);
        }

        public void Sync()
        {
            Sync(true, true);
        }

        public void Sync(bool showDiagnosticLabels, bool showWarningIndicators)
        {
            if (state == null)
            {
                return;
            }

            initialTimeToBedSeconds = Mathf.Max(initialTimeToBedSeconds, state.TimeToBedSeconds);
            float progress = 1f - Mathf.Clamp01(state.TimeToBedSeconds / initialTimeToBedSeconds);
            transform.position = Vector3.Lerp(spawnPosition, bedPosition, progress);

            float hpRatio = Mathf.Clamp01(state.CurrentHp / state.Definition.MaxHp);
            transform.localScale = new Vector3(0.7f + hpRatio * 0.3f, 0.7f + hpRatio * 0.3f, 0.7f + hpRatio * 0.3f);
            UpdateLabel(showDiagnosticLabels);
            UpdateStatusIndicator(showDiagnosticLabels);
            UpdateWarningIndicator(showWarningIndicators, showDiagnosticLabels);
        }

        private void EnsureLabel()
        {
            if (label != null)
            {
                return;
            }

            GameObject labelObject = new GameObject("Label");
            labelObject.transform.SetParent(transform, false);
            labelObject.transform.localPosition = new Vector3(0f, 1f, 0f);
            label = labelObject.AddComponent<TextMesh>();
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.12f;
            label.fontSize = 48;
            label.color = Color.white;
        }

        private void EnsureStatusIndicator()
        {
            if (statusIndicatorView != null)
            {
                return;
            }

            GameObject statusObject = new GameObject("EnemyStatusIndicator");
            statusObject.transform.SetParent(transform, false);
            statusIndicatorView = statusObject.AddComponent<P0StatusIndicatorView>();
            statusIndicatorView.SetLocalOffset(new Vector3(0f, 1.65f, 0f));
        }

        private void EnsureWarningIndicator()
        {
            if (warningIndicatorView != null)
            {
                return;
            }

            GameObject warningObject = new GameObject("EnemyWarningIndicator");
            warningObject.transform.SetParent(transform, false);
            warningIndicatorView = warningObject.AddComponent<P0EnemyWarningIndicatorView>();
        }

        private void EnsureVisualAssetView()
        {
            if (visualAssetView != null)
            {
                return;
            }

            GameObject visualObject = new GameObject("EnemyWorldVisual");
            visualObject.transform.SetParent(transform, false);
            visualAssetView = visualObject.AddComponent<P0WorldVisualAssetView>();
        }

        private bool SyncVisualAsset(P0VisualAssetReference visualAsset, Color fallbackColor)
        {
            EnsureVisualAssetView();
            return visualAssetView.SetAsset(
                visualAsset,
                fallbackColor,
                new Vector2(1.05f, 1.05f),
                18,
                new Vector3(0f, 0.45f, 0f));
        }

        private void SetFallbackRendererActive(bool active)
        {
            if (cachedRenderer != null)
            {
                cachedRenderer.enabled = active;
            }
        }

        private void UpdateLabel(bool showDiagnosticLabels)
        {
            if (label != null)
            {
                label.gameObject.SetActive(showDiagnosticLabels);
            }

            if (!showDiagnosticLabels)
            {
                return;
            }

            labelBuilder.Length = 0;
            labelBuilder.Append(state.Definition.DisplayName);
            labelBuilder.Append("\n生命 ");
            labelBuilder.Append(Mathf.CeilToInt(state.CurrentHp));
            labelBuilder.Append("/");
            labelBuilder.Append(Mathf.CeilToInt(state.Definition.MaxHp));
            if (!string.IsNullOrWhiteSpace(state.SpawnGateId))
            {
                labelBuilder.Append("\nGate ");
                labelBuilder.Append(state.SpawnGateId);
            }

            foreach (StatusEffectState effect in state.Statuses.ActiveEffects)
            {
                labelBuilder.Append("\n");
                labelBuilder.Append(StatusDisplayFormatter.Format(effect));
            }

            string warningText = EnemyWarningFormatter.Format(state);
            if (!string.IsNullOrWhiteSpace(warningText))
            {
                labelBuilder.Append("\n");
                labelBuilder.Append(warningText);
            }

            label.text = labelBuilder.ToString();
        }

        private void UpdateStatusIndicator(bool showDiagnosticLabels)
        {
            EnsureStatusIndicator();
            statusIndicatorView.Sync(showDiagnosticLabels
                ? P0StatusIndicatorPresenter.Build(state.Statuses)
                : default(P0StatusIndicatorState));
        }

        private void UpdateWarningIndicator(bool showWarningIndicators, bool showDiagnosticLabels)
        {
            EnsureWarningIndicator();
            if (!showWarningIndicators)
            {
                warningIndicatorView.Sync(default(P0EnemyWarningIndicatorState), false);
                return;
            }

            Vector2 enemyPosition = ToNavigationPosition(transform.position);
            Vector2 bed = ToNavigationPosition(bedPosition);
            warningIndicatorView.Sync(
                P0EnemyWarningIndicatorPresenter.Build(state, enemyPosition, bed),
                showDiagnosticLabels);
        }

        private static Vector2 ToNavigationPosition(Vector3 position)
        {
            return new Vector2(position.x, position.z);
        }
    }
}
