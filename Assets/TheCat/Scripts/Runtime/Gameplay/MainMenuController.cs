using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TheCat.Gameplay
{
    public sealed class MainMenuController : MonoBehaviour
    {
        public const string MainMenuSceneName = P0SceneFlow.MainMenuSceneName;
        public const string GrayboxBattleSceneName = P0SceneFlow.GrayboxBattleSceneName;

        private readonly List<CatDefinition> starterCats = new List<CatDefinition>();

        private string message = "就绪";
        private bool includeSaiban = true;
        private bool includeNephthys = true;
        private bool includeSuzune = true;
        private bool showGrayboxTools;
        private Vector2 menuScrollPosition;
        private GUIStyle wrappedLabel;
        private GUIStyle panelContentStyle;
        private GUIStyle sectionTitleStyle;
        private GUIStyle starterCardStyle;
        private GUIStyle starterHeaderStyle;
        private GUIStyle helperFoldoutStyle;

        private void Awake()
        {
            starterCats.Clear();
            starterCats.AddRange(P0PrototypeCatalog.CreateStarterCats());
        }

        private void OnGUI()
        {
            P0MainMenuSurface surface = BuildMainMenuSurface();
            Rect panelRect = P0ImGuiLayout.BuildLeftPanelRect(460f, 760f, 0.42f);
            float innerWidth = P0ImGuiLayout.ScrollContentWidth(panelRect);
            P0ImGuiVisualAssetDrawer.DrawTexture(
                surface.UiShell.MainMenuBackground,
                new Rect(0f, 0f, Screen.width, Screen.height),
                ScaleMode.ScaleAndCrop);
            P0ImGuiVisualAssetDrawer.DrawTexture(surface.UiShell.DreamGlassPanel, panelRect, ScaleMode.StretchToFill);

            GUILayout.BeginArea(panelRect, PanelContentStyle);
            menuScrollPosition = GUILayout.BeginScrollView(menuScrollPosition, false, true);
            float logoWidth = Mathf.Min(P0ImGuiLayout.Scaled(300f), innerWidth);
            P0ImGuiVisualAssetDrawer.DrawGUILayoutTexture(surface.UiShell.TitleLogo, logoWidth, logoWidth * 0.5f);
            GUILayout.Label(surface.Title);
            GUILayout.Label(surface.Subtitle);
            GUILayout.Label(surface.PlayerPathLabel, WrappedLabel);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawStarterSelection(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawPrimaryPath(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            DrawGrayboxTools(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);
            GUILayout.Label(message, WrappedLabel);
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        public void StartP0Run()
        {
            StartP0Run(P0RunStartMode.RouteMap);
        }

        public void StartP0QuickBattle()
        {
            StartP0Run(P0RunStartMode.QuickBattle);
        }

        public P0MainMenuSurface BuildMainMenuSurfaceForSmoke()
        {
            return BuildMainMenuSurface();
        }

        public P0LoadingStartSurface BuildLoadingStartSurfaceForSmoke(P0RunStartMode startMode)
        {
            return P0LoadingStartPresenter.BuildRunStartSurface(startMode, 0.35f);
        }

        private void StartP0Run(P0RunStartMode startMode)
        {
            P0RunSession.StartNewRun(GetSelectedStarterIds());
            if (startMode == P0RunStartMode.CatRoom)
            {
                P0CatRoomSession.RecordFreshStart(P0RunSession.CurrentRun);
            }

            string sceneName = P0SceneFlow.GetStartSceneName(startMode);
            message = "正在加载 " + DescribeSceneName(sceneName);
            SceneManager.LoadScene(sceneName);
        }

        private P0MainMenuSurface BuildMainMenuSurface()
        {
            return P0MainMenuPresenter.BuildSurface(
                starterCats,
                GetSelectedStarterIds(),
                P0RouteCatalog.CreateTenLayerRoute(),
                message);
        }

        private void DrawStarterSelection(P0MainMenuSurface surface)
        {
            GUILayout.Label(surface.CharacterSelectLabel, SectionTitleStyle);
            GUILayout.Label("已选择 " + surface.SelectedStarterCount + " / " + surface.StarterCards.Count + " 只初始猫", WrappedLabel);
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                P0MainMenuStarterCard card = surface.StarterCards[i];
                bool selected = IsStarterSelected(card.CatId);
                GUILayout.BeginVertical(StarterCardStyle);
                GUILayout.BeginHorizontal();
                P0ImGuiVisualAssetDrawer.DrawInlineIcon(card.HudAvatar, P0ImGuiLayout.Scaled(42f));
                GUILayout.BeginVertical();
                bool nextSelected = GUILayout.Toggle(
                    selected,
                    card.DisplayName + " · " + card.RoleLabel + " · " + card.ReadyBadgeLabel,
                    StarterHeaderStyle);
                SetStarterSelected(card.CatId, nextSelected);
                GUILayout.Label(card.RoleHint + " / " + card.AuthorityLabel + " / " + card.AttributeLabel, WrappedLabel);
                GUILayout.Label("技能：" + card.BuildSkillPreview(), WrappedLabel);
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            if (!HasAnyStarterSelected())
            {
                GUILayout.Label("至少选择一只初始猫后进入猫房。", WrappedLabel);
            }

            if (GUILayout.Button("恢复默认三猫", GUILayout.Height(P0ImGuiLayout.CompactButtonHeight)))
            {
                SelectDefaultStarters();
                message = "已选择默认三猫。";
            }
        }

        private void DrawPrimaryPath(P0MainMenuSurface surface)
        {
            GUILayout.Label("今晚从猫房出发", SectionTitleStyle);
            P0MainMenuAction catRoomAction = GetAction(surface, P0MainMenuActionIds.EnterCatRoom);
            GUILayout.Label(catRoomAction.Detail, WrappedLabel);
            GUI.enabled = HasAnyStarterSelected();
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, catRoomAction.Label, P0ImGuiLayout.PrimaryButtonHeight))
            {
                StartP0Run(P0RunStartMode.CatRoom);
            }

            GUI.enabled = true;
        }

        private void DrawGrayboxTools(P0MainMenuSurface surface)
        {
            showGrayboxTools = GUILayout.Toggle(
                showGrayboxTools,
                showGrayboxTools ? "隐藏灰盒验证入口" : "显示灰盒验证入口",
                HelperFoldoutStyle,
                GUILayout.Height(P0ImGuiLayout.CompactButtonHeight));
            if (!showGrayboxTools)
            {
                return;
            }

            GUILayout.Label(surface.GrayboxHelperLabel, SectionTitleStyle);
            DrawRoutePreview(surface);
            GUILayout.Space(P0ImGuiLayout.SectionSpacing);

            P0MainMenuAction selectedRouteAction = GetAction(surface, P0MainMenuActionIds.StartSelectedRoute);
            GUI.enabled = HasAnyStarterSelected();
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, selectedRouteAction.Label, P0ImGuiLayout.ButtonHeight))
            {
                StartP0Run();
            }

            P0MainMenuAction defaultRouteAction = GetAction(surface, P0MainMenuActionIds.StartDefaultRoute);
            GUI.enabled = true;
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, defaultRouteAction.Label, P0ImGuiLayout.ButtonHeight))
            {
                SelectDefaultStarters();
                StartP0Run();
            }

            P0MainMenuAction quickBattleAction = GetAction(surface, P0MainMenuActionIds.QuickBattle);
            GUI.enabled = HasAnyStarterSelected();
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, quickBattleAction.Label, P0ImGuiLayout.ButtonHeight))
            {
                StartP0QuickBattle();
            }

            P0MainMenuAction clearAction = GetAction(surface, P0MainMenuActionIds.ClearSession);
            GUI.enabled = true;
            if (P0ImGuiVisualAssetDrawer.DrawTexturedButton(surface.UiShell.PrimaryButton, clearAction.Label, P0ImGuiLayout.ButtonHeight))
            {
                P0RunSession.Clear();
                message = "已清除当前进度。";
            }

            GUI.enabled = true;
        }

        private IReadOnlyList<string> GetSelectedStarterIds()
        {
            List<string> selected = new List<string>();
            if (includeSaiban)
            {
                selected.Add(P0PrototypeCatalog.SaibanId);
            }

            if (includeNephthys)
            {
                selected.Add(P0PrototypeCatalog.NephthysId);
            }

            if (includeSuzune)
            {
                selected.Add(P0PrototypeCatalog.SuzuneId);
            }

            return selected.AsReadOnly();
        }

        private bool HasAnyStarterSelected()
        {
            return includeSaiban || includeNephthys || includeSuzune;
        }

        private void SelectDefaultStarters()
        {
            includeSaiban = true;
            includeNephthys = true;
            includeSuzune = true;
        }

        private bool IsStarterSelected(string catId)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    return includeSaiban;
                case P0PrototypeCatalog.NephthysId:
                    return includeNephthys;
                case P0PrototypeCatalog.SuzuneId:
                    return includeSuzune;
                default:
                    return false;
            }
        }

        private void SetStarterSelected(string catId, bool selected)
        {
            switch (catId)
            {
                case P0PrototypeCatalog.SaibanId:
                    includeSaiban = selected;
                    break;
                case P0PrototypeCatalog.NephthysId:
                    includeNephthys = selected;
                    break;
                case P0PrototypeCatalog.SuzuneId:
                    includeSuzune = selected;
                    break;
            }
        }

        private void DrawRoutePreview(P0MainMenuSurface surface)
        {
            GUILayout.Label("路线：" + surface.RouteRows.Count + " 层");
            for (int i = 0; i < surface.RouteRows.Count; i++)
            {
                GUILayout.Label(surface.RouteRows[i].BuildPreviewLabel(), WrappedLabel);
            }
        }

        private static P0MainMenuAction GetAction(P0MainMenuSurface surface, string actionId)
        {
            if (surface != null && surface.TryGetAction(actionId, out P0MainMenuAction action))
            {
                return action;
            }

            return new P0MainMenuAction(
                actionId,
                actionId,
                false,
                P0RunStartMode.RouteMap,
                string.Empty,
                "缺失操作",
                P0MainMenuActionCategory.Utility);
        }

        private static string DescribeSceneName(string sceneName)
        {
            switch (sceneName)
            {
                case P0SceneFlow.RouteMapSceneName:
                    return "路线图";
                case P0SceneFlow.CatRoomSceneName:
                    return "猫房";
                case P0SceneFlow.GrayboxBattleSceneName:
                    return "卧室守床战斗";
                case P0SceneFlow.MainMenuSceneName:
                    return "主菜单";
                default:
                    return "未知场景";
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

        private GUIStyle StarterCardStyle
        {
            get
            {
                if (starterCardStyle == null)
                {
                    starterCardStyle = new GUIStyle(GUI.skin.box)
                    {
                        wordWrap = true
                    };
                }

                int horizontal = Mathf.RoundToInt(P0ImGuiLayout.Scaled(10f));
                int vertical = Mathf.RoundToInt(P0ImGuiLayout.Scaled(8f));
                starterCardStyle.padding = new RectOffset(horizontal, horizontal, vertical, vertical);
                starterCardStyle.margin = new RectOffset(0, 0, 0, Mathf.RoundToInt(P0ImGuiLayout.Scaled(6f)));
                starterCardStyle.normal.textColor = Color.white;
                return starterCardStyle;
            }
        }

        private GUIStyle StarterHeaderStyle
        {
            get
            {
                if (starterHeaderStyle == null)
                {
                    starterHeaderStyle = new GUIStyle(GUI.skin.toggle)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                starterHeaderStyle.fontSize = Mathf.RoundToInt(P0ImGuiLayout.Scaled(14f));
                starterHeaderStyle.normal.textColor = Color.white;
                starterHeaderStyle.onNormal.textColor = new Color(0.72f, 1f, 0.62f);
                return starterHeaderStyle;
            }
        }

        private GUIStyle HelperFoldoutStyle
        {
            get
            {
                if (helperFoldoutStyle == null)
                {
                    helperFoldoutStyle = new GUIStyle(GUI.skin.button)
                    {
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                }

                helperFoldoutStyle.fontSize = Mathf.RoundToInt(P0ImGuiLayout.Scaled(13f));
                return helperFoldoutStyle;
            }
        }
    }
}
