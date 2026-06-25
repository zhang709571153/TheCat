using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Gameplay
{
    public sealed class CatRoomController : MonoBehaviour
    {
        public const string CatRoomSceneName = P0SceneFlow.CatRoomSceneName;

        private Vector2 scrollPosition;
        private GUIStyle wrappedLabel;
        private GUIStyle panelContentStyle;

        private void OnGUI()
        {
            P0CatRoomSurface surface = BuildCatRoomSurfaceForSmoke();
            Rect panelRect = P0ImGuiLayout.BuildLeftPanelRect(360f, 620f, 0.48f);
            P0ImGuiVisualAssetDrawer.DrawTexture(
                surface.UiShell.MainMenuBackground,
                new Rect(0f, 0f, Screen.width, Screen.height),
                ScaleMode.ScaleAndCrop);
            P0ImGuiVisualAssetDrawer.DrawTexture(surface.UiShell.DreamGlassPanel, panelRect, ScaleMode.StretchToFill);

            GUILayout.BeginArea(panelRect, PanelContentStyle);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            float logoWidth = Mathf.Min(P0ImGuiLayout.Scaled(260f), P0ImGuiLayout.ScrollContentWidth(panelRect));
            P0ImGuiVisualAssetDrawer.DrawGUILayoutTexture(surface.UiShell.TitleLogo, logoWidth, logoWidth * 0.5f);
            GUILayout.Label(surface.Title);
            GUILayout.Label(surface.Subtitle);
            GUILayout.Label(surface.ReturnFeedbackLabel, WrappedLabel);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);

            DrawValueRows(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawResourceRows(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawHotspots(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawActions(surface);

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public P0CatRoomSurface BuildCatRoomSurfaceForSmoke()
        {
            return P0CatRoomPresenter.BuildSurface(P0CatRoomSession.CurrentState);
        }

        public void EnterDream()
        {
            SceneManager.LoadScene(P0SceneFlow.RouteMapSceneName);
        }

        public void ReturnMainMenu()
        {
            SceneManager.LoadScene(P0SceneFlow.MainMenuSceneName);
        }

        private void DrawValueRows(P0CatRoomSurface surface)
        {
            GUILayout.Label("核心状态");
            for (int i = 0; i < surface.ValueRows.Count; i++)
            {
                GUILayout.Label(surface.ValueRows[i].BuildSummary(), WrappedLabel);
            }
        }

        private void DrawResourceRows(P0CatRoomSurface surface)
        {
            GUILayout.Label("猫房资源");
            for (int i = 0; i < surface.ResourceRows.Count; i++)
            {
                GUILayout.Label(surface.ResourceRows[i].BuildSummary(), WrappedLabel);
            }
        }

        private void DrawHotspots(P0CatRoomSurface surface)
        {
            GUILayout.Label("房间角落");
            for (int i = 0; i < surface.Hotspots.Count; i++)
            {
                GUILayout.Label(surface.Hotspots[i].BuildSummary(), WrappedLabel);
            }
        }

        private void DrawActions(P0CatRoomSurface surface)
        {
            for (int i = 0; i < surface.Actions.Count; i++)
            {
                P0CatRoomAction action = surface.Actions[i];
                GUI.enabled = action.IsEnabled;
                if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, action.BuildButtonLabel(), P0ImGuiLayout.ButtonHeight))
                {
                    ExecuteAction(action);
                }
            }

            GUI.enabled = true;
        }

        private void ExecuteAction(P0CatRoomAction action)
        {
            switch (action.ActionId)
            {
                case P0CatRoomActionIds.EnterDream:
                    EnterDream();
                    break;
                case P0CatRoomActionIds.ReturnMainMenu:
                    ReturnMainMenu();
                    break;
            }
        }

        private GUIStyle WrappedLabel
        {
            get
            {
                if (wrappedLabel == null)
                {
                    wrappedLabel = new GUIStyle(GUI.skin.label)
                    {
                        wordWrap = true
                    };
                }

                return wrappedLabel;
            }
        }

        private GUIStyle PanelContentStyle
        {
            get
            {
                if (panelContentStyle == null)
                {
                    panelContentStyle = new GUIStyle(GUIStyle.none);
                }

                panelContentStyle.padding = P0ImGuiLayout.Padding();
                return panelContentStyle;
            }
        }
    }
}
