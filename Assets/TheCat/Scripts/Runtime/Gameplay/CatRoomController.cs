using TheCat.Roguelite;
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
        private GUIStyle sectionTitleStyle;
        private GUIStyle dreamChoiceStyle;
        private GUIStyle dreamChoiceHeaderStyle;
        private GUIStyle dreamChoiceDisabledHeaderStyle;

        private void OnGUI()
        {
            P0CatRoomSurface surface = BuildCatRoomSurfaceForSmoke();
            Rect panelRect = P0ImGuiLayout.BuildLeftPanelRect(460f, 760f, 0.42f);
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

            DrawDreamChoices(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawActions(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawValueRows(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawResourceRows(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawHotspots(surface);

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public P0CatRoomSurface BuildCatRoomSurfaceForSmoke()
        {
            return P0CatRoomPresenter.BuildSurface(P0CatRoomSession.CurrentState);
        }

        public void EnterDream()
        {
            P0RunSession.EnsureBedroomDreamRun();
            SceneManager.LoadScene(P0SceneFlow.RouteMapSceneName);
        }

        public void EnterEgyptDream()
        {
            P0RunSession.EnsureEgyptDreamRun();
            SceneManager.LoadScene(P0SceneFlow.RouteMapSceneName);
        }

        public void ReturnMainMenu()
        {
            SceneManager.LoadScene(P0SceneFlow.MainMenuSceneName);
        }

        private void DrawValueRows(P0CatRoomSurface surface)
        {
            GUILayout.Label("核心状态", SectionTitleStyle);
            for (int i = 0; i < surface.ValueRows.Count; i++)
            {
                GUILayout.Label(surface.ValueRows[i].BuildSummary(), WrappedLabel);
            }
        }

        private void DrawResourceRows(P0CatRoomSurface surface)
        {
            GUILayout.Label("猫房资源", SectionTitleStyle);
            for (int i = 0; i < surface.ResourceRows.Count; i++)
            {
                GUILayout.Label(surface.ResourceRows[i].BuildSummary(), WrappedLabel);
            }
        }

        private void DrawDreamChoices(P0CatRoomSurface surface)
        {
            GUILayout.Label("梦境主题", SectionTitleStyle);
            for (int i = 0; i < surface.DreamChoices.Count; i++)
            {
                P0CatRoomDreamChoice choice = surface.DreamChoices[i];
                GUILayout.BeginVertical(DreamChoiceStyle);
                GUILayout.Label(BuildDreamChoiceHeader(choice), GetDreamChoiceHeaderStyle(choice));
                GUILayout.Label(choice.ThemeLabel + " / 守护目标：" + choice.TargetLabel, WrappedLabel);
                GUILayout.Label(choice.Detail, WrappedLabel);
                GUILayout.EndVertical();
            }
        }

        private static string BuildDreamChoiceHeader(P0CatRoomDreamChoice choice)
        {
            if (choice.IsEnabled)
            {
                return choice.Label + "  可进入当前Demo路线";
            }

            if (!choice.IsPlayable)
            {
                return choice.Label + "  占位，不可进入，无跳转";
            }

            return choice.Label + "  已锁定，等待开局条件";
        }

        private GUIStyle GetDreamChoiceHeaderStyle(P0CatRoomDreamChoice choice)
        {
            return choice.IsEnabled ? DreamChoiceHeaderStyle : DreamChoiceDisabledHeaderStyle;
        }

        private void DrawHotspots(P0CatRoomSurface surface)
        {
            GUILayout.Label("房间角落", SectionTitleStyle);
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
                case P0CatRoomActionIds.EnterEgyptDream:
                    EnterEgyptDream();
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

                wrappedLabel.fontSize = Mathf.RoundToInt(P0ImGuiLayout.Scaled(14f));
                wrappedLabel.normal.textColor = Color.white;
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

        private GUIStyle SectionTitleStyle
        {
            get
            {
                if (sectionTitleStyle == null)
                {
                    sectionTitleStyle = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                sectionTitleStyle.fontSize = Mathf.RoundToInt(P0ImGuiLayout.Scaled(16f));
                sectionTitleStyle.normal.textColor = new Color(1f, 0.88f, 0.52f);
                return sectionTitleStyle;
            }
        }

        private GUIStyle DreamChoiceStyle
        {
            get
            {
                if (dreamChoiceStyle == null)
                {
                    dreamChoiceStyle = new GUIStyle(GUI.skin.box)
                    {
                        wordWrap = true
                    };
                }

                int horizontal = Mathf.RoundToInt(P0ImGuiLayout.Scaled(10f));
                int vertical = Mathf.RoundToInt(P0ImGuiLayout.Scaled(8f));
                dreamChoiceStyle.padding = new RectOffset(horizontal, horizontal, vertical, vertical);
                dreamChoiceStyle.margin = new RectOffset(0, 0, 0, Mathf.RoundToInt(P0ImGuiLayout.Scaled(6f)));
                dreamChoiceStyle.normal.textColor = Color.white;
                return dreamChoiceStyle;
            }
        }

        private GUIStyle DreamChoiceHeaderStyle
        {
            get
            {
                if (dreamChoiceHeaderStyle == null)
                {
                    dreamChoiceHeaderStyle = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                dreamChoiceHeaderStyle.fontSize = Mathf.RoundToInt(P0ImGuiLayout.Scaled(15f));
                dreamChoiceHeaderStyle.normal.textColor = new Color(0.72f, 1f, 0.62f);
                return dreamChoiceHeaderStyle;
            }
        }

        private GUIStyle DreamChoiceDisabledHeaderStyle
        {
            get
            {
                if (dreamChoiceDisabledHeaderStyle == null)
                {
                    dreamChoiceDisabledHeaderStyle = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                dreamChoiceDisabledHeaderStyle.fontSize = Mathf.RoundToInt(P0ImGuiLayout.Scaled(15f));
                dreamChoiceDisabledHeaderStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
                return dreamChoiceDisabledHeaderStyle;
            }
        }
    }
}
