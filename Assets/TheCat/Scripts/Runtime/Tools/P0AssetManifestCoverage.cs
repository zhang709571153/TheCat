using System;
using System.Collections.Generic;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Tools
{
    public enum P0AssetManifestCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0AssetManifestCoverageIssue
    {
        public P0AssetManifestCoverageIssue(P0AssetManifestCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0AssetManifestCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0AssetManifestCoverageReport
    {
        private readonly List<P0AssetManifestCoverageIssue> issues = new List<P0AssetManifestCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0AssetManifestCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int FailureCount => Count(P0AssetManifestCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0 && coveredChecks.Count >= P0AssetManifestCoverage.ExpectedCoveredCheckCount;

        public void AddIssue(P0AssetManifestCoverageSeverity severity, string message)
        {
            issues.Add(new P0AssetManifestCoverageIssue(severity, message));
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
                ? "P0 asset manifest coverage complete for " + coveredChecks.Count + " asset check(s)."
                : "P0 asset manifest coverage has " + FailureCount + " failure(s) across " + coveredChecks.Count + " asset check(s).";
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

        private int Count(P0AssetManifestCoverageSeverity severity)
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

    public static class P0AssetManifestCoverage
    {
        public const int ExpectedCoveredCheckCount = 27;

        public static P0AssetManifestCoverageReport EvaluateP0Manifest()
        {
            return Evaluate(P0AssetManifestCatalog.CreateP0PlannedManifest());
        }

        public static P0AssetManifestCoverageReport Evaluate(IReadOnlyList<P0AssetManifestEntry> manifest)
        {
            P0AssetManifestCoverageReport report = new P0AssetManifestCoverageReport();
            EvaluateStyleAnchors(manifest, report);
            EvaluateStarterCats(manifest, report);
            EvaluateCoreEnemies(manifest, report);
            EvaluateBedroomProps(manifest, report);
            EvaluateCoreHudIcons(manifest, report);
            EvaluateCoreGaugeBars(manifest, report);
            EvaluateStatusTagIcons(manifest, report);
            EvaluateStatusTagCompactIcons(manifest, report);
            EvaluateBossReadiness(manifest, report);
            EvaluateEnemyWarningVfxAssets(manifest, report);
            EvaluateEnemyAnimationFramesheets(manifest, report);
            EvaluateRouteNodeIcons(manifest, report);
            EvaluateUiShellAssets(manifest, report);
            EvaluateRouteChoiceIcons(manifest, report);
            EvaluateRouteRewardCardFrames(manifest, report);
            EvaluateNonBattleNodeSummaryBanners(manifest, report);
            EvaluateShopItemCards(manifest, report);
            EvaluateDreamEventChoiceCards(manifest, report);
            EvaluateRestNestRecoveryCard(manifest, report);
            EvaluatePartnerChoiceCards(manifest, report);
            EvaluateAuthorityBlessingChoiceCards(manifest, report);
            EvaluateResultSettlementOutcomeBanners(manifest, report);
            EvaluateRouteRewardDetailBadges(manifest, report);
            EvaluateAuthorityBlessingSeals(manifest, report);
            EvaluateBattleFeedbackVfxAssets(manifest, report);
            EvaluateHardSourceReferenceNotes(manifest, report);
            EvaluateNamingPathsAndReferences(manifest, report);
            return report;
        }

        private static void EvaluateStyleAnchors(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "bedroom_dream", "background")
                && HasSubject(manifest, "bedroom_dream_battle", "background")
                && HasSubject(manifest, "starter_cats", "concept")
                && HasSubject(manifest, P0PrototypeCatalog.BlackMudNightmareId, "concept")
                && HasSubject(manifest, "p0_status_tags", "icon"),
                "Manifest includes style anchors and battle background for bedroom, starter cats, black mud, and status icons.",
                "Manifest is missing required P0 style anchor rows.");
        }

        private static void EvaluateStarterCats(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, P0PrototypeCatalog.SaibanId, "sprite")
                && HasSubject(manifest, P0PrototypeCatalog.NephthysId, "sprite")
                && HasSubject(manifest, P0PrototypeCatalog.SuzuneId, "sprite")
                && AllSubjectsReference(manifest, new[] { P0PrototypeCatalog.SaibanId, P0PrototypeCatalog.NephthysId, P0PrototypeCatalog.SuzuneId }, "thecat_style_startercats_lineup_2048_v001")
                && SubjectHasSourceLock(manifest, P0PrototypeCatalog.SaibanId, "sprite", "saiban_turnaround_colored")
                && SubjectHasSourceLock(manifest, P0PrototypeCatalog.NephthysId, "sprite", "nephthys_turnaround_colored")
                && SubjectHasSourceLock(manifest, P0PrototypeCatalog.SuzuneId, "sprite", "suzune_turnaround_colored")
                && AllSubjectsMention(manifest, new[] { P0PrototypeCatalog.SaibanId, P0PrototypeCatalog.NephthysId, P0PrototypeCatalog.SuzuneId }, "turnaround"),
                "Manifest includes starter cat sprites tied to the lineup style anchor and colored turnaround source-lock ids.",
                "Manifest is missing starter cat sprite rows, lineup references, source-lock ids, or colored turnaround notes.");
        }

        private static void EvaluateCoreEnemies(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, P0PrototypeCatalog.BlackMudNightmareId, "sprite")
                && HasSubject(manifest, P0PrototypeCatalog.ColdLightShadowId, "sprite")
                && HasSubject(manifest, P0PrototypeCatalog.CallTyrantId, "concept"),
                "Manifest includes P0 priority enemy assets for Black Mud, Cold Light, and Call Tyrant.",
                "Manifest is missing P0 priority enemy asset rows.");
        }

        private static void EvaluateBedroomProps(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "bedroom_dream_battle", "background")
                && SubjectHasSourceLock(manifest, "bedroom_dream_battle", "background", "bedroom_map_concept")
                && HasSubject(manifest, "bed", "sprite")
                && HasSubject(manifest, "litter_box", "sprite")
                && HasSubject(manifest, "feeder", "sprite"),
                "Manifest includes Bedroom Dream battle background plus bed, litter box, and feeder gameplay props.",
                "Manifest is missing required bedroom interaction prop rows.");
        }

        private static void EvaluateCoreHudIcons(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "owner_sleep", "icon")
                && HasSubject(manifest, "cat_hp", "icon")
                && HasSubject(manifest, "team_poop", "icon")
                && HasSubject(manifest, "team_hunger", "icon"),
                "Manifest includes HUD icons for owner sleep, cat HP, poop, and hunger.",
                "Manifest is missing one or more four-core HUD icon rows.");
        }

        private static void EvaluateCoreGaugeBars(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] frameSubjects =
            {
                "owner_sleep_gauge_frame",
                "cat_hp_gauge_frame",
                "team_poop_gauge_frame",
                "team_hunger_gauge_frame"
            };
            string[] fillSubjects =
            {
                "owner_sleep_gauge_fill",
                "cat_hp_gauge_fill",
                "team_poop_gauge_fill",
                "team_hunger_gauge_fill"
            };

            Require(
                report,
                HasSubject(manifest, "owner_sleep_gauge_frame", "frame")
                && HasSubject(manifest, "owner_sleep_gauge_fill", "bar")
                && HasSubject(manifest, "cat_hp_gauge_frame", "frame")
                && HasSubject(manifest, "cat_hp_gauge_fill", "bar")
                && HasSubject(manifest, "team_poop_gauge_frame", "frame")
                && HasSubject(manifest, "team_poop_gauge_fill", "bar")
                && HasSubject(manifest, "team_hunger_gauge_frame", "frame")
                && HasSubject(manifest, "team_hunger_gauge_fill", "bar")
                && SubjectReferences(manifest, "owner_sleep_gauge_frame", "frame", P0VisualAssetCatalog.OwnerSleepIconId)
                && SubjectReferences(manifest, "cat_hp_gauge_frame", "frame", P0VisualAssetCatalog.CatHpIconId)
                && SubjectReferences(manifest, "team_poop_gauge_frame", "frame", P0VisualAssetCatalog.TeamPoopIconId)
                && SubjectReferences(manifest, "team_hunger_gauge_frame", "frame", P0VisualAssetCatalog.TeamHungerIconId)
                && AllSubjectsMention(manifest, frameSubjects, "non-cat", "frame")
                && AllSubjectsMention(manifest, fillSubjects, "non-cat", "bar"),
                "Manifest includes non-cat core gauge bars for owner sleep, cat HP, poop, and hunger.",
                "Manifest is missing one or more non-cat four-core gauge frame/fill rows.");
        }

        private static void EvaluateStatusTagIcons(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, StatusTagIds.SleepStable, "icon")
                && HasSubject(manifest, StatusTagIds.Slow, "icon")
                && HasSubject(manifest, StatusTagIds.Knockback, "icon")
                && HasSubject(manifest, StatusTagIds.Mark, "icon")
                && HasSubject(manifest, StatusTagIds.Shield, "icon")
                && AllSubjectsReference(manifest, new[] { StatusTagIds.SleepStable, StatusTagIds.Slow, StatusTagIds.Knockback, StatusTagIds.Mark, StatusTagIds.Shield }, "thecat_style_status_icons_5x64_v001", "icon"),
                "Manifest includes source-split icons for Sleep Stable, Slow, Knockback, Mark, and Shield.",
                "Manifest is missing one or more source-split P0 status icon rows.");
        }

        private static void EvaluateStatusTagCompactIcons(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] compactSubjects =
            {
                "sleep_stable_compact",
                "slow_compact",
                "knockback_compact",
                "mark_compact",
                "shield_compact"
            };

            Require(
                report,
                HasSubject(manifest, "sleep_stable_compact", "icon")
                && HasSubject(manifest, "slow_compact", "icon")
                && HasSubject(manifest, "knockback_compact", "icon")
                && HasSubject(manifest, "mark_compact", "icon")
                && HasSubject(manifest, "shield_compact", "icon")
                && SubjectReferences(manifest, "sleep_stable_compact", "icon", P0VisualAssetCatalog.SleepStableStatusIconId)
                && SubjectReferences(manifest, "slow_compact", "icon", P0VisualAssetCatalog.SlowStatusIconId)
                && SubjectReferences(manifest, "knockback_compact", "icon", P0VisualAssetCatalog.KnockbackStatusIconId)
                && SubjectReferences(manifest, "mark_compact", "icon", P0VisualAssetCatalog.MarkStatusIconId)
                && SubjectReferences(manifest, "shield_compact", "icon", P0VisualAssetCatalog.ShieldStatusIconId)
                && AllSubjectsMention(manifest, compactSubjects, "32px compact", "icon")
                && AllSubjectsMention(manifest, compactSubjects, "no cat body derivative", "icon"),
                "Manifest includes 32px compact status HUD icons derived from the accepted 64px status icons.",
                "Manifest is missing one or more 32px compact status HUD icon rows or 64px source references.");
        }

        private static void EvaluateBossReadiness(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "call_tyrant_warning", "vfx")
                && HasSubject(manifest, "boss_route_node", "icon"),
                "Manifest includes Call Tyrant warning VFX and boss route node icon.",
                "Manifest is missing Boss readiness visual rows.");
        }

        private static void EvaluateEnemyWarningVfxAssets(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "black_mud_bed_claw", "vfx")
                && HasSubject(manifest, "cold_light_beam_warning", "vfx")
                && HasSubject(manifest, "call_tyrant_app_throw", "vfx")
                && HasSubject(manifest, "call_tyrant_summon_portal", "vfx")
                && SubjectHasSourceLock(manifest, "black_mud_bed_claw", "vfx", "black_mud_animation")
                && SubjectHasSourceLock(manifest, "cold_light_beam_warning", "vfx", "cold_light_animation")
                && SubjectHasSourceLock(manifest, "call_tyrant_app_throw", "vfx", "call_tyrant_animation")
                && SubjectHasSourceLock(manifest, "call_tyrant_summon_portal", "vfx", "call_tyrant_animation")
                && SubjectReferences(manifest, "black_mud_bed_claw", "vfx", P0VisualAssetCatalog.BlackMudCombatSpriteId)
                && SubjectReferences(manifest, "cold_light_beam_warning", "vfx", P0VisualAssetCatalog.ColdLightCombatSpriteId)
                && SubjectReferences(manifest, "call_tyrant_app_throw", "vfx", P0VisualAssetCatalog.CallTyrantConceptId)
                && SubjectReferences(manifest, "call_tyrant_summon_portal", "vfx", P0VisualAssetCatalog.CallTyrantConceptId),
                "Manifest includes source-locked enemy warning VFX for Black Mud, Cold Light, and Call Tyrant.",
                "Manifest is missing one or more source-locked enemy warning VFX rows.");
        }

        private static void EvaluateEnemyAnimationFramesheets(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "black_mud_move", "framesheet")
                && HasSubject(manifest, "cold_light_cast", "framesheet")
                && HasSubject(manifest, "call_tyrant_boss_pattern", "framesheet")
                && SubjectHasSourceLock(manifest, "black_mud_move", "framesheet", "black_mud_animation")
                && SubjectHasSourceLock(manifest, "cold_light_cast", "framesheet", "cold_light_animation")
                && SubjectHasSourceLock(manifest, "call_tyrant_boss_pattern", "framesheet", "call_tyrant_animation")
                && SubjectReferences(manifest, "black_mud_move", "framesheet", P0VisualAssetCatalog.BlackMudCombatSpriteId)
                && SubjectReferences(manifest, "cold_light_cast", "framesheet", P0VisualAssetCatalog.ColdLightCombatSpriteId)
                && SubjectReferences(manifest, "call_tyrant_boss_pattern", "framesheet", P0VisualAssetCatalog.CallTyrantConceptId),
                "Manifest includes source-locked enemy animation framesheets for Black Mud, Cold Light, and Call Tyrant.",
                "Manifest is missing one or more source-locked enemy animation framesheet rows.");
        }

        private static void EvaluateRouteNodeIcons(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "defense_route_node", "icon")
                && HasSubject(manifest, "elite_route_node", "icon")
                && HasSubject(manifest, "partner_route_node", "icon")
                && HasSubject(manifest, "shop_route_node", "icon")
                && HasSubject(manifest, "dream_event_route_node", "icon")
                && HasSubject(manifest, "blessing_route_node", "icon")
                && HasSubject(manifest, "rest_nest_route_node", "icon")
                && HasSubject(manifest, "boss_route_node", "icon"),
                "Manifest includes route node icons for all eight P0 roguelite node types.",
                "Manifest is missing one or more P0 route node icon rows.");
        }

        private static void EvaluateUiShellAssets(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "main_menu", "background")
                && HasSubject(manifest, "game_title", "frame")
                && HasSubject(manifest, "dreamglass_panel", "frame")
                && HasSubject(manifest, "primary_button", "frame")
                && HasSubject(manifest, "fish_treats", "icon")
                && HasSubject(manifest, "dream_shards", "icon")
                && AllFrameOrIconSubjectsReference(manifest, new[] { "dreamglass_panel", "primary_button", "fish_treats", "dream_shards" }, "thecat_style_status_icons_5x64_v001")
                && SubjectReferences(manifest, "main_menu", "background", "thecat_style_bedroomdream_anchor_1920x1080_v001")
                && SubjectReferences(manifest, "game_title", "frame", "thecat_style_bedroomdream_anchor_1920x1080_v001"),
                "Manifest includes UI shell background, title, panel, button, and settlement reward icons.",
                "Manifest is missing one or more P0 UI shell visual rows or style-anchor references.");
        }

        private static void EvaluateRouteChoiceIcons(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] choiceSubjects =
            {
                "partner_recruit_choice",
                "purchase_supply_choice",
                "authority_blessing_choice",
                "authority_upgrade_choice",
                "rest_supply_choice",
                "dream_event_modifier_choice"
            };

            Require(
                report,
                HasSubject(manifest, "partner_recruit_choice", "icon")
                && HasSubject(manifest, "purchase_supply_choice", "icon")
                && HasSubject(manifest, "authority_blessing_choice", "icon")
                && HasSubject(manifest, "authority_upgrade_choice", "icon")
                && HasSubject(manifest, "rest_supply_choice", "icon")
                && HasSubject(manifest, "dream_event_modifier_choice", "icon")
                && AllSubjectsReference(manifest, choiceSubjects, "thecat_style_status_icons_5x64_v001", "icon")
                && AllSubjectsMention(manifest, choiceSubjects, "non-cat", "icon")
                && AllSubjectsMention(manifest, choiceSubjects, "no cat body derivative", "icon"),
                "Manifest includes non-cat route choice icons for partner, shop, blessing, upgrade, rest, and dream-event modifier choices.",
                "Manifest is missing one or more non-cat route choice icon rows or style-anchor references.");
        }

        private static void EvaluateRouteRewardCardFrames(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] frameSubjects =
            {
                "partner_route_card",
                "shop_route_card",
                "blessing_route_card",
                "dream_event_route_card",
                "rest_nest_route_card"
            };

            Require(
                report,
                HasSubject(manifest, "partner_route_card", "frame")
                && HasSubject(manifest, "shop_route_card", "frame")
                && HasSubject(manifest, "blessing_route_card", "frame")
                && HasSubject(manifest, "dream_event_route_card", "frame")
                && HasSubject(manifest, "rest_nest_route_card", "frame")
                && AllSubjectsReference(manifest, frameSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "frame")
                && AllSubjectsMention(manifest, frameSubjects, "non-cat", "frame")
                && AllSubjectsMention(manifest, frameSubjects, "no cat body derivative", "frame"),
                "Manifest includes non-cat route reward card frames for partner, shop, blessing, dream event, and rest nest choices.",
                "Manifest is missing one or more non-cat route reward card frame rows or dreamglass panel references.");
        }

        private static void EvaluateRouteRewardDetailBadges(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] badgeSubjects =
            {
                "route_reward_gain_detail",
                "route_reward_cost_detail",
                "route_reward_recovery_detail",
                "route_reward_risk_detail",
                "route_reward_upgrade_detail"
            };

            Require(
                report,
                HasSubject(manifest, "route_reward_gain_detail", "badge")
                && HasSubject(manifest, "route_reward_cost_detail", "badge")
                && HasSubject(manifest, "route_reward_recovery_detail", "badge")
                && HasSubject(manifest, "route_reward_risk_detail", "badge")
                && HasSubject(manifest, "route_reward_upgrade_detail", "badge")
                && AllSubjectsReference(manifest, badgeSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "badge")
                && AllSubjectsMention(manifest, badgeSubjects, "non-cat", "badge")
                && AllSubjectsMention(manifest, badgeSubjects, "no cat body derivative", "badge"),
                "Manifest includes non-cat route reward detail badges for gain, cost, recovery, risk, and upgrade effects.",
                "Manifest is missing one or more non-cat route reward detail badge rows or dreamglass panel references.");
        }

        private static void EvaluateNonBattleNodeSummaryBanners(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] bannerSubjects =
            {
                "shop_node_summary_banner",
                "dream_event_node_summary_banner",
                "rest_nest_node_summary_banner"
            };

            Require(
                report,
                HasSubject(manifest, "shop_node_summary_banner", "banner")
                && HasSubject(manifest, "dream_event_node_summary_banner", "banner")
                && HasSubject(manifest, "rest_nest_node_summary_banner", "banner")
                && AllSubjectsReference(manifest, bannerSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "banner")
                && AllSubjectsMention(manifest, bannerSubjects, "non-cat", "banner")
                && AllSubjectsMention(manifest, bannerSubjects, "no cat body derivative", "banner"),
                "Manifest includes non-cat route node summary banners for shop, dream event, and rest nest current-node surfaces.",
                "Manifest is missing one or more non-cat route node summary banner rows or dreamglass panel references.");
        }

        private static void EvaluateShopItemCards(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] cardSubjects =
            {
                "shop_bed_patch",
                "shop_litter_sachet",
                "shop_late_kibble",
                "shop_free_sample"
            };

            Require(
                report,
                HasSubject(manifest, "shop_bed_patch", "card")
                && HasSubject(manifest, "shop_litter_sachet", "card")
                && HasSubject(manifest, "shop_late_kibble", "card")
                && HasSubject(manifest, "shop_free_sample", "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.ShopRouteCardFrameId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId, "card")
                && AllSubjectsMention(manifest, cardSubjects, "non-cat", "card")
                && AllSubjectsMention(manifest, cardSubjects, "no cat body derivative", "card"),
                "Manifest includes non-cat shop item cards for bed patch, litter sachet, late kibble, and free sample choices.",
                "Manifest is missing one or more non-cat shop item card rows or shop visual references.");
        }

        private static void EvaluateDreamEventChoiceCards(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] cardSubjects =
            {
                "dream_event_clear_notifications",
                "dream_event_catnip_residue",
                "dream_event_mark_all_read"
            };

            Require(
                report,
                HasSubject(manifest, "dream_event_clear_notifications", "card")
                && HasSubject(manifest, "dream_event_catnip_residue", "card")
                && HasSubject(manifest, "dream_event_mark_all_read", "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamEventRouteCardFrameId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId, "card")
                && AllSubjectsMention(manifest, cardSubjects, "non-cat", "card")
                && AllSubjectsMention(manifest, cardSubjects, "no cat body derivative", "card"),
                "Manifest includes non-cat dream-event choice cards for clear notifications, catnip residue, and mark all read choices.",
                "Manifest is missing one or more non-cat dream-event choice card rows or dream-event visual references.");
        }

        private static void EvaluateRestNestRecoveryCard(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] cardSubjects =
            {
                "rest_nest_recovery"
            };

            Require(
                report,
                HasSubject(manifest, "rest_nest_recovery", "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.RestNestRouteCardFrameId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.RestSupplyChoiceIconId, "card")
                && AllSubjectsMention(manifest, cardSubjects, "non-cat", "card")
                && AllSubjectsMention(manifest, cardSubjects, "no cat body derivative", "card"),
                "Manifest includes a non-cat RestNest recovery card for the rest_nest_recovery choice.",
                "Manifest is missing the non-cat RestNest recovery card row or RestNest visual references.");
        }

        private static void EvaluatePartnerChoiceCards(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] cardSubjects =
            {
                "partner_shadowmaru_preview",
                "partner_preview_duplicate_supply"
            };

            Require(
                report,
                HasSubject(manifest, "partner_shadowmaru_preview", "card")
                && HasSubject(manifest, "partner_preview_duplicate_supply", "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.PartnerRouteCardFrameId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.PartnerRouteNodeIconId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.PartnerRecruitChoiceIconId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.FishTreatRewardIconId, "card")
                && AllSubjectsMention(manifest, cardSubjects, "non-cat", "card")
                && AllSubjectsMention(manifest, cardSubjects, "no cat body derivative", "card")
                && AllSubjectsMention(manifest, cardSubjects, "does not define Shadowmaru body art", "card"),
                "Manifest includes non-cat partner choice cards for Shadowmaru preview and duplicate partner fallback choices.",
                "Manifest is missing one or more non-cat partner choice card rows or partner visual references.");
        }

        private static void EvaluateAuthorityBlessingChoiceCards(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] cardSubjects =
            {
                "blessing_authority_oath_bedline",
                "blessing_authority_dominion_sandglass",
                "blessing_authority_rhythm_lullaby"
            };

            Require(
                report,
                HasSubject(manifest, "blessing_authority_oath_bedline", "card")
                && HasSubject(manifest, "blessing_authority_dominion_sandglass", "card")
                && HasSubject(manifest, "blessing_authority_rhythm_lullaby", "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.BlessingRouteCardFrameId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.BlessingRouteNodeIconId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.AuthorityBlessingChoiceIconId, "card")
                && AllSubjectsReference(manifest, cardSubjects, P0VisualAssetCatalog.AuthorityUpgradeChoiceIconId, "card")
                && SubjectReferences(manifest, "blessing_authority_oath_bedline", "card", P0VisualAssetCatalog.AuthorityOathBedlineSealId)
                && SubjectReferences(manifest, "blessing_authority_dominion_sandglass", "card", P0VisualAssetCatalog.AuthorityDominionSandglassSealId)
                && SubjectReferences(manifest, "blessing_authority_rhythm_lullaby", "card", P0VisualAssetCatalog.AuthorityRhythmLullabySealId)
                && AllSubjectsMention(manifest, cardSubjects, "non-cat", "card")
                && AllSubjectsMention(manifest, cardSubjects, "no cat body derivative", "card")
                && AllSubjectsMention(manifest, cardSubjects, "colored turnaround", "card"),
                "Manifest includes non-cat authority blessing choice cards for Oath Bedline, Moon-Sand Dominion, and Lullaby Rhythm.",
                "Manifest is missing one or more non-cat authority blessing choice card rows or authority blessing visual references.");
        }

        private static void EvaluateResultSettlementOutcomeBanners(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] bannerSubjects =
            {
                "battle_result_victory",
                "battle_result_defeat",
                "settlement_run_cleared",
                "settlement_run_failed"
            };

            Require(
                report,
                HasSubject(manifest, "battle_result_victory", "banner")
                && HasSubject(manifest, "battle_result_defeat", "banner")
                && HasSubject(manifest, "settlement_run_cleared", "banner")
                && HasSubject(manifest, "settlement_run_failed", "banner")
                && AllSubjectsReference(manifest, bannerSubjects, P0VisualAssetCatalog.DreamGlassPanelId, "banner")
                && AllSubjectsMention(manifest, bannerSubjects, "non-cat", "banner")
                && AllSubjectsMention(manifest, bannerSubjects, "no cat body derivative", "banner")
                && AllSubjectsMention(manifest, bannerSubjects, "no UI text", "banner")
                && AllSubjectsMention(manifest, bannerSubjects, "colored turnaround", "banner"),
                "Manifest includes non-cat result and settlement outcome banners for battle victory, battle defeat, run cleared, and run failed.",
                "Manifest is missing one or more result or settlement outcome banner rows or non-cat consistency notes.");
        }

        private static void EvaluateAuthorityBlessingSeals(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            string[] sealSubjects =
            {
                "authority_oath_bedline_seal",
                "authority_dominion_sandglass_seal",
                "authority_rhythm_lullaby_seal"
            };

            Require(
                report,
                HasSubject(manifest, "authority_oath_bedline_seal", "icon")
                && HasSubject(manifest, "authority_dominion_sandglass_seal", "icon")
                && HasSubject(manifest, "authority_rhythm_lullaby_seal", "icon")
                && AllSubjectsReference(manifest, sealSubjects, P0VisualAssetCatalog.AuthorityBlessingChoiceIconId, "icon")
                && AllSubjectsReference(manifest, sealSubjects, P0VisualAssetCatalog.BlessingRouteCardFrameId, "icon")
                && AllSubjectsReference(manifest, sealSubjects, "thecat_style_status_icons_5x64_v001", "icon")
                && AllSubjectsMention(manifest, sealSubjects, "non-cat", "icon")
                && AllSubjectsMention(manifest, sealSubjects, "no cat body derivative", "icon"),
                "Manifest includes non-cat authority blessing seals for Oath Bedline, Moon-Sand Dominion, and Lullaby Rhythm.",
                "Manifest is missing one or more authority blessing seal rows or required non-cat references.");
        }

        private static void EvaluateBattleFeedbackVfxAssets(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                HasSubject(manifest, "hit_spark", "vfx")
                && HasSubject(manifest, "bed_shield_pulse", "vfx")
                && HasSubject(manifest, "sleep_stable_wave", "vfx")
                && HasSubject(manifest, "litter_cleanse", "vfx")
                && HasSubject(manifest, "feeder_kibble", "vfx")
                && HasSubject(manifest, "enemy_mark_ring", "vfx")
                && HasSubject(manifest, "saiban_bedline_skill_vfx", "vfx")
                && HasSubject(manifest, "nephthys_moonsand_skill_vfx", "vfx")
                && HasSubject(manifest, "suzune_lullaby_skill_vfx", "vfx")
                && AllSubjectsReference(manifest, new[] { "hit_spark", "bed_shield_pulse", "sleep_stable_wave", "enemy_mark_ring" }, "thecat_style_status_icons_5x64_v001", "vfx")
                && AllSubjectsReference(manifest, new[] { "litter_cleanse", "feeder_kibble" }, "thecat_style_bedroomdream_anchor_1920x1080_v001", "vfx")
                && SubjectHasSourceLock(manifest, "saiban_bedline_skill_vfx", "vfx", "saiban_turnaround_colored")
                && SubjectHasSourceLock(manifest, "nephthys_moonsand_skill_vfx", "vfx", "nephthys_turnaround_colored")
                && SubjectHasSourceLock(manifest, "suzune_lullaby_skill_vfx", "vfx", "suzune_turnaround_colored")
                && AllSubjectsMention(manifest, new[] { "saiban_bedline_skill_vfx", "nephthys_moonsand_skill_vfx", "suzune_lullaby_skill_vfx" }, "no cat body derivative", "vfx"),
                "Manifest includes generic battle feedback VFX plus Batch 61 symbolic starter skill VFX.",
                "Manifest is missing one or more P0 battle feedback VFX rows or style-anchor references.");
        }

        private static void EvaluateHardSourceReferenceNotes(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                StarterCatSourceNotesComplete(manifest)
                && EntryMentions(manifest, P0PrototypeCatalog.BlackMudNightmareId, "sprite", "source", "hard reference")
                && EntryMentions(manifest, P0PrototypeCatalog.ColdLightShadowId, "sprite", "source", "hard reference")
                && EntryMentions(manifest, P0PrototypeCatalog.CallTyrantId, "concept", "source", "hard reference")
                && EntryMentions(manifest, "bedroom_dream_battle", "background", "source", "hard reference")
                && EntryMentions(manifest, "bed", "sprite", "source", "hard reference")
                && EntryMentions(manifest, "litter_box", "sprite", "source", "hard reference")
                && EntryMentions(manifest, "feeder", "sprite", "source", "hard reference")
                && EntryMentions(manifest, "call_tyrant_warning", "vfx", "source", "hard reference")
                && EntryMentions(manifest, "black_mud_bed_claw", "vfx", "source", "locked")
                && EntryMentions(manifest, "cold_light_beam_warning", "vfx", "source", "locked")
                && EntryMentions(manifest, "call_tyrant_app_throw", "vfx", "source", "locked")
                && EntryMentions(manifest, "call_tyrant_summon_portal", "vfx", "source", "locked")
                && EntryMentions(manifest, "black_mud_move", "framesheet", "source", "locked")
                && EntryMentions(manifest, "cold_light_cast", "framesheet", "source", "locked")
                && EntryMentions(manifest, "call_tyrant_boss_pattern", "framesheet", "source", "locked")
                && EntryMentions(manifest, "boss_route_node", "icon", "source", "hard reference"),
                "Manifest records hard source reference notes for cats, enemies, Boss, and bedroom props.",
                "Manifest is missing hard source reference notes for one or more source-sensitive visual rows.");
        }

        private static void EvaluateNamingPathsAndReferences(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            P0AssetManifestCoverageReport report)
        {
            Require(
                report,
                manifest != null
                && manifest.Count >= P0AssetManifestCatalog.P0ManifestAssetCount
                && AllRowsHaveConsistentNaming(manifest)
                && AllReferencesResolve(manifest),
                "Manifest rows use versioned names, Unity Art paths, prompt paths, and resolved references.",
                "Manifest naming, import paths, prompt paths, or references are inconsistent.");
        }

        private static bool HasSubject(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType)
        {
            if (manifest == null)
            {
                return false;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (entry.SubjectId == subjectId && entry.AssetType == assetType)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AllSubjectsReference(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<string> subjectIds,
            string referenceAssetId)
        {
            return AllSubjectsReference(manifest, subjectIds, referenceAssetId, "sprite");
        }

        private static bool AllSubjectsReference(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<string> subjectIds,
            string referenceAssetId,
            string assetType)
        {
            if (manifest == null || subjectIds == null)
            {
                return false;
            }

            for (int i = 0; i < subjectIds.Count; i++)
            {
                P0AssetManifestEntry entry = FindSubject(manifest, subjectIds[i], assetType);
                if (entry == null || !Contains(entry.ReferenceAssetIds, referenceAssetId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AllFrameOrIconSubjectsReference(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<string> subjectIds,
            string referenceAssetId)
        {
            if (manifest == null || subjectIds == null)
            {
                return false;
            }

            for (int i = 0; i < subjectIds.Count; i++)
            {
                P0AssetManifestEntry entry = FindSubject(manifest, subjectIds[i], "frame")
                    ?? FindSubject(manifest, subjectIds[i], "icon");
                if (entry == null || !Contains(entry.ReferenceAssetIds, referenceAssetId))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool SubjectReferences(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType,
            string referenceAssetId)
        {
            P0AssetManifestEntry entry = FindSubject(manifest, subjectId, assetType);
            return entry != null && Contains(entry.ReferenceAssetIds, referenceAssetId);
        }

        private static bool AllRowsHaveConsistentNaming(IReadOnlyList<P0AssetManifestEntry> manifest)
        {
            if (manifest == null || manifest.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (!entry.AssetId.StartsWith("thecat_", StringComparison.Ordinal)
                    || !entry.AssetId.Contains("_v")
                    || entry.Priority != "P0"
                    || !P0AssetManifestStatus.IsKnown(entry.Status)
                    || !IsValidSourcePromptPath(entry.SourcePromptPath)
                    || !entry.UnityImportPath.StartsWith("Assets/TheCat/Art/", StringComparison.Ordinal)
                    || !entry.UnityImportPath.EndsWith(entry.AssetId + ".png", StringComparison.Ordinal)
                    || string.IsNullOrWhiteSpace(entry.Size)
                    || string.IsNullOrWhiteSpace(entry.ConsistencyNotes))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsValidSourcePromptPath(string sourcePromptPath)
        {
            return !string.IsNullOrWhiteSpace(sourcePromptPath)
                && (sourcePromptPath.StartsWith("design/development/prompts/", StringComparison.Ordinal)
                    || sourcePromptPath.StartsWith("design/development/agent_prompts/", StringComparison.Ordinal));
        }

        private static bool AllReferencesResolve(IReadOnlyList<P0AssetManifestEntry> manifest)
        {
            if (manifest == null)
            {
                return false;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                for (int referenceIndex = 0; referenceIndex < entry.ReferenceAssetIds.Count; referenceIndex++)
                {
                    if (FindAsset(manifest, entry.ReferenceAssetIds[referenceIndex]) == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static bool StarterCatSourceNotesComplete(IReadOnlyList<P0AssetManifestEntry> manifest)
        {
            return EntryMentions(manifest, P0PrototypeCatalog.SaibanId, "sprite", "turnaround", "hard reference")
                && EntryMentions(manifest, P0PrototypeCatalog.NephthysId, "sprite", "turnaround", "hard reference")
                && EntryMentions(manifest, P0PrototypeCatalog.SuzuneId, "sprite", "turnaround", "hard reference");
        }

        private static bool EntryMentions(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType,
            string firstExpectedText,
            string secondExpectedText)
        {
            P0AssetManifestEntry entry = FindSubject(manifest, subjectId, assetType);
            return entry != null
                && ContainsText(entry.ConsistencyNotes, firstExpectedText)
                && ContainsText(entry.ConsistencyNotes, secondExpectedText);
        }

        private static bool SubjectHasSourceLock(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType,
            string sourceLockId)
        {
            P0AssetManifestEntry entry = FindSubject(manifest, subjectId, assetType);
            return entry != null && Contains(entry.SourceLockIds, sourceLockId);
        }

        private static bool ContainsText(string value, string expectedText)
        {
            return value != null
                && expectedText != null
                && value.IndexOf(expectedText, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static P0AssetManifestEntry FindSubject(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType)
        {
            if (manifest == null)
            {
                return null;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                P0AssetManifestEntry entry = manifest[i];
                if (entry.SubjectId == subjectId && entry.AssetType == assetType)
                {
                    return entry;
                }
            }

            return null;
        }

        private static bool AllSubjectsMention(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<string> subjectIds,
            string expectedText)
        {
            if (manifest == null || subjectIds == null)
            {
                return false;
            }

            for (int i = 0; i < subjectIds.Count; i++)
            {
                P0AssetManifestEntry entry = FindSubject(manifest, subjectIds[i], "sprite");
                if (entry == null
                    || entry.ConsistencyNotes.IndexOf(expectedText, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool AllSubjectsMention(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            IReadOnlyList<string> subjectIds,
            string expectedText,
            string assetType)
        {
            if (manifest == null || subjectIds == null)
            {
                return false;
            }

            for (int i = 0; i < subjectIds.Count; i++)
            {
                P0AssetManifestEntry entry = FindSubject(manifest, subjectIds[i], assetType);
                if (entry == null
                    || entry.ConsistencyNotes.IndexOf(expectedText, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static P0AssetManifestEntry FindAsset(
            IReadOnlyList<P0AssetManifestEntry> manifest,
            string assetId)
        {
            if (manifest == null)
            {
                return null;
            }

            for (int i = 0; i < manifest.Count; i++)
            {
                if (manifest[i].AssetId == assetId)
                {
                    return manifest[i];
                }
            }

            return null;
        }

        private static bool Contains(IReadOnlyList<string> values, string expected)
        {
            if (values == null)
            {
                return false;
            }

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }

        private static void Require(
            P0AssetManifestCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0AssetManifestCoverageSeverity.Failure, failureMessage);
        }
    }
}
