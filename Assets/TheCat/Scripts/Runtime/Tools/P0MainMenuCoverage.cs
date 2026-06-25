using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Gameplay;
using TheCat.Roguelite;

namespace TheCat.Tools
{
    public enum P0MainMenuCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0MainMenuCoverageIssue
    {
        public P0MainMenuCoverageIssue(P0MainMenuCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0MainMenuCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0MainMenuCoverageReport
    {
        private readonly List<P0MainMenuCoverageIssue> issues = new List<P0MainMenuCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0MainMenuCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0MainMenuCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0MainMenuCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0MainMenuCoverageSeverity severity, string message)
        {
            issues.Add(new P0MainMenuCoverageIssue(severity, message));
        }

        public void AddCoveredCheck(string check)
        {
            if (!string.IsNullOrWhiteSpace(check))
            {
                coveredChecks.Add(check);
            }
        }

        public string BuildSummary()
        {
            return IsComplete
                ? "P0 main menu coverage complete for " + coveredChecks.Count + " start check(s)."
                : "P0 main menu coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " start check(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary()
            };

            for (int i = 0; i < coveredChecks.Count; i++)
            {
                lines.Add("- " + coveredChecks[i]);
            }

            for (int i = 0; i < issues.Count; i++)
            {
                lines.Add("[" + issues[i].Severity + "] " + issues[i].Message);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private int Count(P0MainMenuCoverageSeverity severity)
        {
            int count = 0;
            for (int i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public static class P0MainMenuCoverage
    {
        public const int ExpectedCoveredCheckCount = 10;

        public static P0MainMenuCoverageReport EvaluatePrototypeSurface()
        {
            P0MainMenuCoverageReport report = new P0MainMenuCoverageReport();
            P0MainMenuSurface defaultSurface = CreateSurface(P0RunSession.CreateDefaultStarterCatIds());

            EvaluateStarterTrio(defaultSurface, report);
            EvaluateStarterDesignIdentity(defaultSurface, report);
            EvaluateCharacterSelectContract(defaultSurface, report);
            EvaluateEntryContract(defaultSurface, report);
            EvaluateStartActions(defaultSurface, report);
            EvaluateQuickBattle(defaultSurface, report);
            EvaluateEmptySelection(report);
            EvaluateRoutePreview(defaultSurface, report);
            EvaluateUiShell(defaultSurface, report);
            EvaluateCompactSummary(defaultSurface, report);

            return report;
        }

        private static void EvaluateStarterDesignIdentity(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            Require(
                report,
                HasStarterDesignIdentity(surface, P0PrototypeCatalog.SaibanId, "silver_oath_sun_sword", "oath")
                && HasStarterDesignIdentity(surface, P0PrototypeCatalog.NephthysId, "moon_sand_obelisk_crown", "dominion")
                && HasStarterDesignIdentity(surface, P0PrototypeCatalog.SuzuneId, "moon_bell_torii", "bells"),
                "Main menu starter cards expose design signature lines and visual identity tokens.",
                "Main menu starter design identity is incomplete.");
        }

        private static void EvaluateStarterTrio(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            Require(
                report,
                P0MainMenuPresenter.HasP0MainMenuSurface(surface)
                && surface.SelectedStarterCount == 3
                && HasStarterRole(surface, P0PrototypeCatalog.SaibanId, "DEF")
                && HasStarterRole(surface, P0PrototypeCatalog.NephthysId, "CTRL")
                && HasStarterRole(surface, P0PrototypeCatalog.SuzuneId, "HEAL")
                && HasStarterSkillPreview(surface, P0PrototypeCatalog.SaibanId, "银誓护盾")
                && HasStarterSkillPreview(surface, P0PrototypeCatalog.SuzuneId, "安眠铃"),
                "Main menu starter cards expose the default defender/controller/healer trio with skill previews.",
                "Main menu starter trio cards are incomplete.");
        }

        private static void EvaluateCharacterSelectContract(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            Require(
                report,
                surface.StarterCards.Count == 3
                && HasCharacterSelectCard(surface, P0PrototypeCatalog.SaibanId, "DEF", P0VisualAssetCatalog.SaibanHudAvatarId)
                && HasCharacterSelectCard(surface, P0PrototypeCatalog.NephthysId, "CTRL", P0VisualAssetCatalog.NephthysHudAvatarId)
                && HasCharacterSelectCard(surface, P0PrototypeCatalog.SuzuneId, "HEAL", P0VisualAssetCatalog.SuzuneHudAvatarId)
                && !PlayerFacingStarterTextContainsInternalTokens(surface),
                "Character-select contract exposes three starter cards with ready state, role chip, skills, and source-locked HUD avatars.",
                "Character-select starter-card contract is incomplete or leaks internal visual tokens.");
        }

        private static void EvaluateEntryContract(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            bool hasCatRoomPrimary = TryGetAction(surface, P0MainMenuActionIds.EnterCatRoom, out P0MainMenuAction catRoom)
                && catRoom.IsEnabled
                && catRoom.TargetSceneName == P0SceneFlow.CatRoomSceneName
                && catRoom.ActionCategory == P0MainMenuActionCategory.PlayerPrimary
                && catRoom.Detail.Contains("猫房")
                && catRoom.Detail.Contains("卧室梦境")
                && catRoom.Detail.Contains("守护中心床");

            Require(
                report,
                hasCatRoomPrimary
                && CountActionsByCategory(surface, P0MainMenuActionCategory.PlayerPrimary) == 1
                && surface.PlayerPathLabel.Contains("猫房")
                && surface.PlayerPathLabel.Contains("卧室梦境")
                && surface.PlayerPathLabel.Contains("守护中心床"),
                "Entry contract makes cat-room preparation the only player-primary path into the bedroom dream loop.",
                "Main menu entry contract must expose exactly one player-primary cat-room path.");
        }

        private static void EvaluateStartActions(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            Require(
                report,
                HasEnabledAction(surface, P0MainMenuActionIds.StartSelectedRoute, P0SceneFlow.RouteMapSceneName, P0MainMenuActionCategory.DevRouteHelper)
                && HasEnabledAction(surface, P0MainMenuActionIds.StartDefaultRoute, P0SceneFlow.RouteMapSceneName, P0MainMenuActionCategory.DevRouteHelper)
                && HasEnabledAction(surface, P0MainMenuActionIds.EnterCatRoom, P0SceneFlow.CatRoomSceneName, P0MainMenuActionCategory.PlayerPrimary)
                && HasEnabledAction(surface, P0MainMenuActionIds.ClearSession, string.Empty, P0MainMenuActionCategory.Utility),
                "Main menu UI shell exposes one cat-room player CTA plus route helpers and clear-session utility.",
                "Main menu route start actions are incomplete.");
        }

        private static void EvaluateQuickBattle(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            Require(
                report,
                HasEnabledAction(surface, P0MainMenuActionIds.QuickBattle, P0SceneFlow.GrayboxBattleSceneName, P0MainMenuActionCategory.GrayboxBattleHelper)
                && TryGetAction(surface, P0MainMenuActionIds.QuickBattle, out P0MainMenuAction action)
                && action.Label.Contains("调试"),
                "Main menu quick battle action is retained as a debug-classified graybox bedroom battle shortcut.",
                "Main menu quick battle action is missing, primary-classified, or points to the wrong scene.");
        }

        private static void EvaluateEmptySelection(P0MainMenuCoverageReport report)
        {
            P0MainMenuSurface surface = CreateSurface(Array.Empty<string>());

            Require(
                report,
                !surface.HasAnyStarterSelected
                && TryGetAction(surface, P0MainMenuActionIds.StartSelectedRoute, out P0MainMenuAction selected)
                && !selected.IsEnabled
                && TryGetAction(surface, P0MainMenuActionIds.EnterCatRoom, out P0MainMenuAction catRoom)
                && !catRoom.IsEnabled
                && catRoom.ActionCategory == P0MainMenuActionCategory.PlayerPrimary
                && TryGetAction(surface, P0MainMenuActionIds.QuickBattle, out P0MainMenuAction quickBattle)
                && !quickBattle.IsEnabled
                && HasEnabledAction(surface, P0MainMenuActionIds.StartDefaultRoute, P0SceneFlow.RouteMapSceneName),
                "Main menu disables selected/quick starts when no starter is checked but keeps default start available.",
                "Main menu empty-selection gating is incorrect.");
        }

        private static void EvaluateRoutePreview(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            bool hasBoss = false;
            bool hasBranch = false;
            bool hasNonBattle = false;
            for (int i = 0; i < surface.RouteRows.Count; i++)
            {
                hasBoss |= surface.RouteRows[i].HasBoss;
                hasBranch |= surface.RouteRows[i].OptionCount > 1;
                hasNonBattle |= !surface.RouteRows[i].HasBattle;
            }

            Require(
                report,
                surface.RouteRows.Count == 10
                && hasBoss
                && hasBranch
                && hasNonBattle,
                "Main menu route preview shows ten layers, branch choices, non-battle nodes, and the boss layer.",
                "Main menu route preview is incomplete.");
        }

        private static void EvaluateUiShell(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            Require(
                report,
                P0UiShellPresenter.HasP0UiShellSurface(surface.UiShell)
                && surface.UiShell.MainMenuBackground.AssetId == P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId
                && surface.UiShell.TitleLogo.AssetId == P0VisualAssetCatalog.TitleLogoId
                && surface.UiShell.PrimaryButton.AssetId == P0VisualAssetCatalog.PrimaryButtonId,
                "Main menu surface consumes the Batch 08 background, title logo, panel, and primary button shell assets.",
                "Main menu UI shell assets are missing from the surface.");
        }

        private static void EvaluateCompactSummary(P0MainMenuSurface surface, P0MainMenuCoverageReport report)
        {
            string summary = P0MainMenuPresenter.BuildCompactSummary(surface);

            Require(
                report,
                summary.Contains("starters 3")
                && summary.Contains("selected 3")
                && summary.Contains("route 10")
                && summary.Contains("actions 5")
                && summary.Contains("uiShell 6"),
                "Main menu compact summary reports starter count, selected count, route layers, and actions.",
                "Main menu compact summary is missing required totals.");
        }

        private static P0MainMenuSurface CreateSurface(IReadOnlyList<string> selectedStarterIds)
        {
            return P0MainMenuPresenter.BuildSurface(
                P0PrototypeCatalog.CreateStarterCats(),
                selectedStarterIds,
                P0RouteCatalog.CreateTenLayerRoute(),
                "Ready");
        }

        private static bool HasStarterRole(P0MainMenuSurface surface, string catId, string roleToken)
        {
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                if (surface.StarterCards[i].CatId == catId)
                {
                    return surface.StarterCards[i].RoleToken == roleToken
                        && surface.StarterCards[i].IsSelected
                        && surface.StarterCards[i].SkillCount > 0;
                }
            }

            return false;
        }

        private static bool HasStarterSkillPreview(P0MainMenuSurface surface, string catId, string skillLabel)
        {
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                if (surface.StarterCards[i].CatId != catId)
                {
                    continue;
                }

                for (int skillIndex = 0; skillIndex < surface.StarterCards[i].SkillLabels.Count; skillIndex++)
                {
                    if (surface.StarterCards[i].SkillLabels[skillIndex] == skillLabel)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool HasStarterDesignIdentity(
            P0MainMenuSurface surface,
            string catId,
            string visualToken,
            string requiredSignatureText)
        {
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                if (surface.StarterCards[i].CatId != catId)
                {
                    continue;
                }

                return surface.StarterCards[i].VisualToken == visualToken
                    && surface.StarterCards[i].VisualIdentity.Contains("non-human")
                    && surface.StarterCards[i].SignatureLine.ToLowerInvariant().Contains(requiredSignatureText);
            }

            return false;
        }

        private static bool HasCharacterSelectCard(
            P0MainMenuSurface surface,
            string catId,
            string roleToken,
            string hudAvatarId)
        {
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                P0MainMenuStarterCard card = surface.StarterCards[i];
                if (card.CatId != catId)
                {
                    continue;
                }

                return card.RoleToken == roleToken
                    && card.IsSelected
                    && card.SelectionStateLabel == "已加入猫队"
                    && card.ReadyBadgeLabel == "已准备"
                    && card.SkillCount > 0
                    && card.HudAvatar.AssetId == hudAvatarId
                    && !string.IsNullOrWhiteSpace(card.BuildCharacterSelectSummary());
            }

            return false;
        }

        private static bool PlayerFacingStarterTextContainsInternalTokens(P0MainMenuSurface surface)
        {
            for (int i = 0; i < surface.StarterCards.Count; i++)
            {
                string text = surface.StarterCards[i].BuildSelectionLabel()
                    + " "
                    + surface.StarterCards[i].BuildCharacterSelectSummary();
                if (text.Contains("silver_oath_sun_sword")
                    || text.Contains("moon_sand_obelisk_crown")
                    || text.Contains("moon_bell_torii")
                    || text.Contains("non-human")
                    || text.Contains("Sacred Swordsman")
                    || text.Contains("Moon-Sand Agent")
                    || text.Contains("Sleep Shrine Miko"))
                {
                    return true;
                }
            }

            return false;
        }

        private static int CountActionsByCategory(P0MainMenuSurface surface, P0MainMenuActionCategory category)
        {
            int count = 0;
            for (int i = 0; i < surface.Actions.Count; i++)
            {
                count += surface.Actions[i].ActionCategory == category ? 1 : 0;
            }

            return count;
        }

        private static bool HasEnabledAction(P0MainMenuSurface surface, string actionId, string targetSceneName)
        {
            return TryGetAction(surface, actionId, out P0MainMenuAction action)
                && action.IsEnabled
                && action.TargetSceneName == targetSceneName
                && !string.IsNullOrWhiteSpace(action.Label);
        }

        private static bool HasEnabledAction(
            P0MainMenuSurface surface,
            string actionId,
            string targetSceneName,
            P0MainMenuActionCategory category)
        {
            return HasEnabledAction(surface, actionId, targetSceneName)
                && TryGetAction(surface, actionId, out P0MainMenuAction action)
                && action.ActionCategory == category;
        }

        private static bool TryGetAction(P0MainMenuSurface surface, string actionId, out P0MainMenuAction action)
        {
            if (surface == null)
            {
                action = default(P0MainMenuAction);
                return false;
            }

            return surface.TryGetAction(actionId, out action);
        }

        private static void Require(
            P0MainMenuCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0MainMenuCoverageSeverity.Failure, failureMessage);
        }
    }
}
