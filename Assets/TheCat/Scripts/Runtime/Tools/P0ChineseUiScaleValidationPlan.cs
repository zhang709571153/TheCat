using System;
using System.Collections.Generic;
using TheCat.Gameplay;
using UnityEngine;

namespace TheCat.Tools
{
    public enum P0ChineseUiScaleValidationSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0ChineseUiScaleValidationIssue
    {
        public P0ChineseUiScaleValidationIssue(P0ChineseUiScaleValidationSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0ChineseUiScaleValidationSeverity Severity { get; }

        public string Message { get; }
    }

    public readonly struct P0ChineseUiScaleSurface
    {
        public P0ChineseUiScaleSurface(
            string id,
            string displayName,
            bool requiresScrollViewCheck,
            bool requiresNarrowStackCheck,
            bool requiresScreenshot)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            RequiresScrollViewCheck = requiresScrollViewCheck;
            RequiresNarrowStackCheck = requiresNarrowStackCheck;
            RequiresScreenshot = requiresScreenshot;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public bool RequiresScrollViewCheck { get; }

        public bool RequiresNarrowStackCheck { get; }

        public bool RequiresScreenshot { get; }

        public string BuildSummary()
        {
            return DisplayName
                + " [" + Id + "]"
                + " screenshot=" + RequiresScreenshot
                + ", scroll=" + RequiresScrollViewCheck
                + ", stack=" + RequiresNarrowStackCheck;
        }
    }

    public readonly struct P0ChineseUiScaleResolution
    {
        public P0ChineseUiScaleResolution(string id, int width, int height, string label)
        {
            Id = id ?? string.Empty;
            Width = width;
            Height = height;
            Label = label ?? string.Empty;
        }

        public string Id { get; }

        public int Width { get; }

        public int Height { get; }

        public string Label { get; }

        public float Scale => P0ImGuiLayout.CalculateScale(Width, Height);

        public string BuildSummary()
        {
            return Label + " [" + Id + "] " + Width + "x" + Height + " scale " + Scale.ToString("0.00");
        }
    }

    public readonly struct P0ChineseUiScaleAcceptanceCheck
    {
        public P0ChineseUiScaleAcceptanceCheck(string id, string displayName)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public string BuildSummary()
        {
            return DisplayName + " [" + Id + "]";
        }
    }

    public sealed class P0ChineseUiScaleValidationReport
    {
        private readonly List<P0ChineseUiScaleValidationIssue> issues = new List<P0ChineseUiScaleValidationIssue>();
        private readonly List<string> coveredChecks = new List<string>();
        private readonly List<P0ChineseUiScaleSurface> surfaces = new List<P0ChineseUiScaleSurface>();
        private readonly List<P0ChineseUiScaleResolution> resolutions = new List<P0ChineseUiScaleResolution>();
        private readonly List<P0ChineseUiScaleAcceptanceCheck> acceptanceChecks = new List<P0ChineseUiScaleAcceptanceCheck>();

        public IReadOnlyList<P0ChineseUiScaleValidationIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public IReadOnlyList<P0ChineseUiScaleSurface> Surfaces => surfaces.AsReadOnly();

        public IReadOnlyList<P0ChineseUiScaleResolution> Resolutions => resolutions.AsReadOnly();

        public IReadOnlyList<P0ChineseUiScaleAcceptanceCheck> AcceptanceChecks => acceptanceChecks.AsReadOnly();

        public int FailureCount => Count(P0ChineseUiScaleValidationSeverity.Failure);

        public bool IsReady => FailureCount == 0 && coveredChecks.Count >= P0ChineseUiScaleValidationPlan.ExpectedCoveredCheckCount;

        public void AddSurface(P0ChineseUiScaleSurface surface)
        {
            surfaces.Add(surface);
        }

        public void AddResolution(P0ChineseUiScaleResolution resolution)
        {
            resolutions.Add(resolution);
        }

        public void AddAcceptanceCheck(P0ChineseUiScaleAcceptanceCheck check)
        {
            acceptanceChecks.Add(check);
        }

        public void AddIssue(P0ChineseUiScaleValidationSeverity severity, string message)
        {
            issues.Add(new P0ChineseUiScaleValidationIssue(severity, message));
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
            return IsReady
                ? "P0 Chinese UI scale validation plan ready for " + surfaces.Count + " surface(s), " + resolutions.Count + " resolution(s), and " + acceptanceChecks.Count + " acceptance check(s)."
                : "P0 Chinese UI scale validation plan has " + FailureCount + " failure(s) across " + coveredChecks.Count + " covered check(s).";
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

        private int Count(P0ChineseUiScaleValidationSeverity severity)
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

    public static class P0ChineseUiScaleValidationPlan
    {
        public const int ExpectedSurfaceCount = 5;
        public const int ExpectedResolutionCount = 4;
        public const int ExpectedAcceptanceCheckCount = 7;
        public const int ExpectedCoveredCheckCount = 8;

        public const string MainMenuSurfaceId = "main_menu_character_select";
        public const string RouteMapSurfaceId = "route_map";
        public const string BattleHudSurfaceId = "battle_hud";
        public const string SkillEnemyHudSurfaceId = "skill_enemy_hud";
        public const string ResultSettingsSurfaceId = "result_pause_settings";

        public const string TextChineseCheckId = "ui_text_chinese";
        public const string NoOverlapCheckId = "no_overlap";
        public const string NoClippingCheckId = "no_clipping";
        public const string ScrollViewCheckId = "scrollable_panels";
        public const string ControlStackCheckId = "hud_control_stacking";
        public const string ConsoleCleanCheckId = "console_clean";
        public const string ScreenshotMatrixCheckId = "screenshot_matrix_saved";

        public static IReadOnlyList<P0ChineseUiScaleSurface> CreateP0Surfaces()
        {
            return Array.AsReadOnly(new[]
            {
                new P0ChineseUiScaleSurface(
                    MainMenuSurfaceId,
                    "\u4e3b\u83dc\u5355 / \u89d2\u8272\u9009\u62e9",
                    true,
                    true,
                    true),
                new P0ChineseUiScaleSurface(
                    RouteMapSurfaceId,
                    "10 \u5c42\u8def\u7ebf\u5730\u56fe",
                    true,
                    true,
                    true),
                new P0ChineseUiScaleSurface(
                    BattleHudSurfaceId,
                    "\u5b88\u5e8a\u6218\u6597 HUD",
                    true,
                    true,
                    true),
                new P0ChineseUiScaleSurface(
                    SkillEnemyHudSurfaceId,
                    "\u6280\u80fd / \u654c\u4eba\u72b6\u6001",
                    false,
                    true,
                    true),
                new P0ChineseUiScaleSurface(
                    ResultSettingsSurfaceId,
                    "\u7ed3\u7b97 / \u6682\u505c\u8bbe\u7f6e",
                    true,
                    false,
                    true)
            });
        }

        public static IReadOnlyList<P0ChineseUiScaleResolution> CreateP0ResolutionMatrix()
        {
            return Array.AsReadOnly(new[]
            {
                new P0ChineseUiScaleResolution("compact_4_3", 1024, 768, "\u7d27\u51d1 4:3"),
                new P0ChineseUiScaleResolution("baseline_16_9", 1280, 720, "\u57fa\u51c6 16:9"),
                new P0ChineseUiScaleResolution("desktop_16_9", 1600, 900, "\u684c\u9762 16:9"),
                new P0ChineseUiScaleResolution("wide_1080p", 1920, 1080, "\u5bbd\u5c4f 1080p")
            });
        }

        public static IReadOnlyList<P0ChineseUiScaleAcceptanceCheck> CreateP0AcceptanceChecks()
        {
            return Array.AsReadOnly(new[]
            {
                new P0ChineseUiScaleAcceptanceCheck(TextChineseCheckId, "\u4e2d\u6587\u6587\u672c\u8986\u76d6"),
                new P0ChineseUiScaleAcceptanceCheck(NoOverlapCheckId, "\u65e0 UI \u91cd\u53e0"),
                new P0ChineseUiScaleAcceptanceCheck(NoClippingCheckId, "\u65e0\u6587\u5b57\u88c1\u5207"),
                new P0ChineseUiScaleAcceptanceCheck(ScrollViewCheckId, "\u957f\u9762\u677f\u53ef\u6eda\u52a8"),
                new P0ChineseUiScaleAcceptanceCheck(ControlStackCheckId, "\u7a84\u5bbd\u5ea6\u63a7\u4ef6\u6362\u884c"),
                new P0ChineseUiScaleAcceptanceCheck(ConsoleCleanCheckId, "Console \u65e0\u9519\u8bef"),
                new P0ChineseUiScaleAcceptanceCheck(ScreenshotMatrixCheckId, "\u622a\u56fe\u77e9\u9635\u5df2\u5b58\u6863")
            });
        }

        public static P0ChineseUiScaleValidationReport EvaluateCurrentPlan()
        {
            return Evaluate(
                CreateP0Surfaces(),
                CreateP0ResolutionMatrix(),
                CreateP0AcceptanceChecks(),
                P0ChineseUiCoverage.EvaluatePrototypeUi());
        }

        public static P0ChineseUiScaleValidationReport Evaluate(
            IReadOnlyList<P0ChineseUiScaleSurface> surfaces,
            IReadOnlyList<P0ChineseUiScaleResolution> resolutions,
            IReadOnlyList<P0ChineseUiScaleAcceptanceCheck> acceptanceChecks,
            P0ChineseUiCoverageReport chineseUi)
        {
            P0ChineseUiScaleValidationReport report = new P0ChineseUiScaleValidationReport();
            CopyInputs(report, surfaces, resolutions, acceptanceChecks);

            EvaluateSurfaceMatrix(report, surfaces);
            EvaluateResolutionMatrix(report, resolutions);
            EvaluateAcceptanceChecks(report, acceptanceChecks);
            EvaluateLayoutHelpers(report);
            EvaluateChineseUiDependency(report, chineseUi);

            return report;
        }

        public static string BuildCaptureMatrix(
            IReadOnlyList<P0ChineseUiScaleSurface> surfaces,
            IReadOnlyList<P0ChineseUiScaleResolution> resolutions)
        {
            List<string> lines = new List<string>
            {
                "surface,resolution,evidence"
            };

            if (surfaces == null || resolutions == null)
            {
                return string.Join(Environment.NewLine, lines);
            }

            for (int surfaceIndex = 0; surfaceIndex < surfaces.Count; surfaceIndex++)
            {
                for (int resolutionIndex = 0; resolutionIndex < resolutions.Count; resolutionIndex++)
                {
                    lines.Add(surfaces[surfaceIndex].Id
                        + ","
                        + resolutions[resolutionIndex].Width
                        + "x"
                        + resolutions[resolutionIndex].Height
                        + ","
                        + surfaces[surfaceIndex].DisplayName);
                }
            }

            return string.Join(Environment.NewLine, lines);
        }

        private static void CopyInputs(
            P0ChineseUiScaleValidationReport report,
            IReadOnlyList<P0ChineseUiScaleSurface> surfaces,
            IReadOnlyList<P0ChineseUiScaleResolution> resolutions,
            IReadOnlyList<P0ChineseUiScaleAcceptanceCheck> acceptanceChecks)
        {
            if (surfaces != null)
            {
                for (int i = 0; i < surfaces.Count; i++)
                {
                    report.AddSurface(surfaces[i]);
                }
            }

            if (resolutions != null)
            {
                for (int i = 0; i < resolutions.Count; i++)
                {
                    report.AddResolution(resolutions[i]);
                }
            }

            if (acceptanceChecks != null)
            {
                for (int i = 0; i < acceptanceChecks.Count; i++)
                {
                    report.AddAcceptanceCheck(acceptanceChecks[i]);
                }
            }
        }

        private static void EvaluateSurfaceMatrix(
            P0ChineseUiScaleValidationReport report,
            IReadOnlyList<P0ChineseUiScaleSurface> surfaces)
        {
            Require(
                report,
                surfaces != null && surfaces.Count == ExpectedSurfaceCount,
                "Surface matrix covers the five P0 UI surfaces that need Unity screenshot review.",
                "Chinese UI scale surface matrix is missing one or more P0 surfaces.");

            Require(
                report,
                HasSurface(surfaces, MainMenuSurfaceId)
                && HasSurface(surfaces, RouteMapSurfaceId)
                && HasSurface(surfaces, BattleHudSurfaceId)
                && HasSurface(surfaces, SkillEnemyHudSurfaceId)
                && HasSurface(surfaces, ResultSettingsSurfaceId),
                "Surface matrix includes menu, route map, battle HUD, skill/enemy HUD, and result/settings screens.",
                "Chinese UI scale surface matrix does not cover the expected P0 UI flow.");

            Require(
                report,
                AllSurfacesHaveChineseNames(surfaces)
                && AnySurfaceRequiresScroll(surfaces)
                && AnySurfaceRequiresStacking(surfaces)
                && AllSurfacesRequireScreenshots(surfaces),
                "Every surface has a Chinese label and explicit screenshot, scroll, or narrow-stack evidence requirements.",
                "Chinese UI scale surfaces are missing Chinese names or evidence requirements.");
        }

        private static void EvaluateResolutionMatrix(
            P0ChineseUiScaleValidationReport report,
            IReadOnlyList<P0ChineseUiScaleResolution> resolutions)
        {
            Require(
                report,
                resolutions != null && resolutions.Count == ExpectedResolutionCount,
                "Resolution matrix covers compact, baseline, desktop, and wide desktop sizes.",
                "Chinese UI scale resolution matrix is stale or incomplete.");

            Require(
                report,
                HasResolution(resolutions, 1024, 768)
                && HasResolution(resolutions, 1280, 720)
                && HasResolution(resolutions, 1600, 900)
                && HasResolution(resolutions, 1920, 1080),
                "Resolution matrix includes 1024x768, 1280x720, 1600x900, and 1920x1080 captures.",
                "Chinese UI scale resolution matrix is missing required capture sizes.");

            Require(
                report,
                ResolutionScalesAreClamped(resolutions),
                "Resolution matrix stays inside the IMGUI clamp range so layout checks mirror runtime scaling.",
                "Chinese UI scale resolution matrix has an invalid runtime scale.");
        }

        private static void EvaluateAcceptanceChecks(
            P0ChineseUiScaleValidationReport report,
            IReadOnlyList<P0ChineseUiScaleAcceptanceCheck> acceptanceChecks)
        {
            Require(
                report,
                acceptanceChecks != null && acceptanceChecks.Count == ExpectedAcceptanceCheckCount,
                "Acceptance checklist covers Chinese text, overlap, clipping, scroll, stacking, console, and screenshot evidence.",
                "Chinese UI scale acceptance checklist is stale or incomplete.");

            Require(
                report,
                HasCheck(acceptanceChecks, TextChineseCheckId)
                && HasCheck(acceptanceChecks, NoOverlapCheckId)
                && HasCheck(acceptanceChecks, NoClippingCheckId)
                && HasCheck(acceptanceChecks, ScrollViewCheckId)
                && HasCheck(acceptanceChecks, ControlStackCheckId)
                && HasCheck(acceptanceChecks, ConsoleCleanCheckId)
                && HasCheck(acceptanceChecks, ScreenshotMatrixCheckId),
                "Acceptance checklist includes every required P0 UI scale review gate.",
                "Chinese UI scale acceptance checklist is missing required review gates.");
        }

        private static void EvaluateLayoutHelpers(P0ChineseUiScaleValidationReport report)
        {
            Rect panel = new Rect(0f, 0f, 340f, 620f);
            float inner = P0ImGuiLayout.InnerWidth(panel, 14f);
            float scrollContent = P0ImGuiLayout.ScrollContentWidth(panel, 14f);
            bool narrowStacks = P0ImGuiLayout.ShouldStackControls(300f, 390f, 1f);
            bool wideInline = !P0ImGuiLayout.ShouldStackControls(520f, 390f, 1f);

            Require(
                report,
                scrollContent > 0f
                && scrollContent < inner
                && narrowStacks
                && wideInline,
                "Runtime IMGUI helpers reserve scroll width and switch controls between narrow stacked and wide inline modes.",
                "Runtime IMGUI helpers do not meet the P0 Chinese UI scale expectations.");
        }

        private static void EvaluateChineseUiDependency(
            P0ChineseUiScaleValidationReport report,
            P0ChineseUiCoverageReport chineseUi)
        {
            Require(
                report,
                chineseUi != null && chineseUi.IsComplete,
                "Chinese UI coverage gate is complete before scale screenshots are accepted.",
                "Chinese UI scale validation requires the Chinese UI coverage gate to pass first.");
        }

        private static bool HasSurface(IReadOnlyList<P0ChineseUiScaleSurface> surfaces, string id)
        {
            if (surfaces == null)
            {
                return false;
            }

            for (int i = 0; i < surfaces.Count; i++)
            {
                if (surfaces[i].Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AllSurfacesHaveChineseNames(IReadOnlyList<P0ChineseUiScaleSurface> surfaces)
        {
            if (surfaces == null || surfaces.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < surfaces.Count; i++)
            {
                if (!ContainsChinese(surfaces[i].DisplayName))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AnySurfaceRequiresScroll(IReadOnlyList<P0ChineseUiScaleSurface> surfaces)
        {
            if (surfaces == null)
            {
                return false;
            }

            for (int i = 0; i < surfaces.Count; i++)
            {
                if (surfaces[i].RequiresScrollViewCheck)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AnySurfaceRequiresStacking(IReadOnlyList<P0ChineseUiScaleSurface> surfaces)
        {
            if (surfaces == null)
            {
                return false;
            }

            for (int i = 0; i < surfaces.Count; i++)
            {
                if (surfaces[i].RequiresNarrowStackCheck)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AllSurfacesRequireScreenshots(IReadOnlyList<P0ChineseUiScaleSurface> surfaces)
        {
            if (surfaces == null || surfaces.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < surfaces.Count; i++)
            {
                if (!surfaces[i].RequiresScreenshot)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool HasResolution(IReadOnlyList<P0ChineseUiScaleResolution> resolutions, int width, int height)
        {
            if (resolutions == null)
            {
                return false;
            }

            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].Width == width && resolutions[i].Height == height)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ResolutionScalesAreClamped(IReadOnlyList<P0ChineseUiScaleResolution> resolutions)
        {
            if (resolutions == null || resolutions.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < resolutions.Count; i++)
            {
                float scale = resolutions[i].Scale;
                if (scale < P0ImGuiLayout.MinScale || scale > P0ImGuiLayout.MaxScale)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool HasCheck(IReadOnlyList<P0ChineseUiScaleAcceptanceCheck> checks, string id)
        {
            if (checks == null)
            {
                return false;
            }

            for (int i = 0; i < checks.Count; i++)
            {
                if (checks[i].Id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsChinese(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] >= '\u4e00' && text[i] <= '\u9fff')
                {
                    return true;
                }
            }

            return false;
        }

        private static void Require(
            P0ChineseUiScaleValidationReport report,
            bool condition,
            string coveredMessage,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredMessage);
            }
            else
            {
                report.AddIssue(P0ChineseUiScaleValidationSeverity.Failure, failureMessage);
            }
        }
    }
}
