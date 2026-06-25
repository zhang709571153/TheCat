using System;
using System.Collections.Generic;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;

namespace TheCat.Tools
{
    public delegate bool P0VisualAssetAvailability(P0VisualAssetReference asset);

    public enum P0RuntimeVisualBindingCoverageSeverity
    {
        Info,
        Failure
    }

    public readonly struct P0RuntimeVisualBindingCoverageIssue
    {
        public P0RuntimeVisualBindingCoverageIssue(P0RuntimeVisualBindingCoverageSeverity severity, string message)
        {
            Severity = severity;
            Message = message ?? string.Empty;
        }

        public P0RuntimeVisualBindingCoverageSeverity Severity { get; }

        public string Message { get; }
    }

    public sealed class P0RuntimeVisualBindingCoverageReport
    {
        private readonly List<P0RuntimeVisualBindingCoverageIssue> issues = new List<P0RuntimeVisualBindingCoverageIssue>();
        private readonly List<string> coveredChecks = new List<string>();

        public IReadOnlyList<P0RuntimeVisualBindingCoverageIssue> Issues => issues.AsReadOnly();

        public IReadOnlyList<string> CoveredChecks => coveredChecks.AsReadOnly();

        public int BindingCount { get; private set; }

        public int ResolvedTextureCount { get; private set; }

        public int FailureCount => Count(P0RuntimeVisualBindingCoverageSeverity.Failure);

        public bool IsComplete => FailureCount == 0
            && coveredChecks.Count >= P0RuntimeVisualBindingCoverage.ExpectedCoveredCheckCount;

        public void SetCounts(int bindingCount, int resolvedTextureCount)
        {
            BindingCount = bindingCount;
            ResolvedTextureCount = resolvedTextureCount;
        }

        public void AddIssue(P0RuntimeVisualBindingCoverageSeverity severity, string message)
        {
            issues.Add(new P0RuntimeVisualBindingCoverageIssue(severity, message));
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
                ? "P0 runtime visual bindings complete for " + BindingCount + " binding(s) and " + ResolvedTextureCount + " resolved texture(s)."
                : "P0 runtime visual bindings have " + FailureCount + " failure(s).";
        }

        public string BuildDetailedSummary()
        {
            List<string> lines = new List<string>
            {
                BuildSummary(),
                "Runtime visual bindings: " + BindingCount,
                "Resolved textures: " + ResolvedTextureCount
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

        private int Count(P0RuntimeVisualBindingCoverageSeverity severity)
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

    public static class P0RuntimeVisualBindingCoverage
    {
        public const int ExpectedCoveredCheckCount = 30;

        public static P0RuntimeVisualBindingCoverageReport EvaluateP0Bindings()
        {
            return Evaluate(
                P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                P0VisualAssetTextureResolver.CanResolveTexture);
        }

        public static P0RuntimeVisualBindingCoverageReport Evaluate(
            IReadOnlyList<P0VisualAssetBinding> bindings,
            P0VisualAssetAvailability availability)
        {
            P0RuntimeVisualBindingCoverageReport report = new P0RuntimeVisualBindingCoverageReport();
            int bindingCount = bindings == null ? 0 : bindings.Count;
            int resolvedTextureCount = CountResolvedTextures(bindings, availability, report);

            Require(report, bindingCount == P0VisualAssetCatalog.P0RuntimeVisualBindingCount, "Runtime visual catalog declares the " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " P0 bound visual slots.", "Runtime visual catalog must declare exactly " + P0VisualAssetCatalog.P0RuntimeVisualBindingCount + " P0 bound visual slots.");
            Require(report, AllBindingsReady(bindings), "All runtime visual bindings have ids, surfaces, slots, and manifest assets.", "One or more runtime visual bindings are incomplete.");
            Require(report, BattleBackgroundBindingPresent(bindings), "Battle world background binding uses the Bedroom Dream map source lock.", "Battle world background binding is incomplete or missing source lock.");
            Require(report, StarterCatBindingsUseTurnaroundLocks(bindings), "Starter cat bindings use colored-turnaround source-locked sprites and HUD avatars.", "Starter cat bindings must use colored-turnaround source-locked sprites and HUD avatars.");
            Require(report, EnemyWorldBindingsPresent(bindings), "Enemy world bindings cover Black Mud, Cold Light, and Call Tyrant source-locked visuals.", "Enemy world visual bindings are incomplete or missing source locks.");
            Require(report, PropWorldBindingsPresent(bindings), "Battle world prop bindings cover bed, litter box, and feeder source-locked sprites.", "Battle world prop visual bindings are incomplete or missing source locks.");
            Require(report, CoreHudBindingsPresent(bindings), "Core HUD bindings cover owner sleep, cat HP, poop, and hunger icons.", "Core HUD visual bindings are incomplete.");
            Require(report, SkillHudFeedbackBindingsPresent(bindings), "Skill HUD feedback bindings cover ready, cooldown, no-target, hunger, auto-target, and interaction-range cues.", "Skill HUD feedback visual bindings are incomplete.");
            Require(report, StarterSkillVfxBindingsPresent(bindings), "Starter skill VFX bindings cover Saiban, Nephthys, and Suzune symbolic skill feedback.", "Starter skill VFX bindings are incomplete.");
            Require(report, CoreGaugeBindingsPresent(bindings), "Core gauge bar bindings cover owner sleep, cat HP, poop, and hunger frame/fill pairs.", "Core gauge bar visual bindings are incomplete.");
            Require(report, StatusIconBindingsPresent(bindings), "Status HUD bindings cover Sleep Stable, Slow, Knockback, Mark, and Shield icons.", "Status HUD icon bindings are incomplete.");
            Require(report, StatusCompactIconBindingsPresent(bindings), "Status HUD compact bindings cover 32px Sleep Stable, Slow, Knockback, Mark, and Shield icons.", "Status HUD compact icon bindings are incomplete.");
            Require(report, RouteNodeBindingsPresent(bindings), "Route map bindings cover all eight P0 roguelite node icons.", "Route map node icon bindings are incomplete.");
            Require(report, BossBindingsPresent(bindings), "Boss route and Call Tyrant warning bindings use source-locked assets.", "Boss visual bindings are incomplete or missing source locks.");
            Require(report, EnemyWarningVfxBindingsPresent(bindings), "Enemy warning VFX bindings cover Black Mud bed pressure, Cold Light beam, Call Tyrant throw, and Call Tyrant summon warnings.", "Enemy warning VFX bindings are incomplete or missing source locks.");
            Require(report, EnemyAnimationFramesheetBindingsPresent(bindings), "Enemy animation framesheet bindings cover Black Mud move, Cold Light cast, and Call Tyrant Boss pattern loops.", "Enemy animation framesheet bindings are incomplete or missing source locks.");
            Require(report, UiShellBindingsPresent(bindings), "UI shell bindings cover main menu, panel, button, and settlement reward slots.", "UI shell visual bindings are incomplete.");
            Require(report, RouteChoiceIconBindingsPresent(bindings), "Route choice icon bindings cover partner, shop, blessing, upgrade, rest, and dream-event modifier choices.", "Route choice icon bindings are incomplete.");
            Require(report, RouteRewardCardFrameBindingsPresent(bindings), "Route reward card frame bindings cover partner, shop, blessing, dream event, and rest nest cards.", "Route reward card frame bindings are incomplete.");
            Require(report, RouteRewardDetailBadgeBindingsPresent(bindings), "Route reward detail badge bindings cover gain, cost, recovery, risk, and upgrade effects.", "Route reward detail badge bindings are incomplete.");
            Require(report, AuthorityBlessingSealBindingsPresent(bindings), "Authority blessing detail bindings cover Oath Bedline, Moon-Sand Dominion, and Lullaby Rhythm seals.", "Authority blessing seal bindings are incomplete.");
            Require(report, RouteNodeSummaryBannerBindingsPresent(bindings), "Route node summary banner bindings cover shop, dream event, and rest nest current-node banners.", "Route node summary banner bindings are incomplete.");
            Require(report, ShopItemCardBindingsPresent(bindings), "Shop item card bindings cover bed patch, litter sachet, late kibble, and free sample choices.", "Shop item card bindings are incomplete.");
            Require(report, DreamEventChoiceCardBindingsPresent(bindings), "Dream-event choice card bindings cover clear notifications, catnip residue, and mark all read choices.", "Dream-event choice card bindings are incomplete.");
            Require(report, RestNestRecoveryCardBindingsPresent(bindings), "RestNest recovery card binding covers the concrete rest_nest_recovery choice.", "RestNest recovery card binding is incomplete.");
            Require(report, PartnerChoiceCardBindingsPresent(bindings), "Partner choice card bindings cover Shadowmaru preview and duplicate-supply fallback choices.", "Partner choice card bindings are incomplete.");
            Require(report, AuthorityBlessingChoiceCardBindingsPresent(bindings), "Authority blessing choice card bindings cover Oath Bedline, Moon-Sand Dominion, and Lullaby Rhythm cards.", "Authority blessing choice card bindings are incomplete.");
            Require(report, OutcomeBannerBindingsPresent(bindings), "Outcome banner bindings cover battle victory, battle defeat, cleared settlement, and failed settlement slots.", "Outcome banner bindings are incomplete.");
            Require(report, BattleFeedbackVfxBindingsPresent(bindings), "Battle feedback VFX bindings cover hit, shield, sleep, litter, feeder, and mark feedback slots.", "Battle feedback VFX bindings are incomplete.");
            Require(report, resolvedTextureCount == bindingCount && bindingCount > 0, "All runtime visual bindings resolve to editor-loadable textures.", "One or more runtime visual bindings could not resolve a texture.");

            report.SetCounts(bindingCount, resolvedTextureCount);
            return report;
        }

        private static int CountResolvedTextures(
            IReadOnlyList<P0VisualAssetBinding> bindings,
            P0VisualAssetAvailability availability,
            P0RuntimeVisualBindingCoverageReport report)
        {
            if (bindings == null || availability == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < bindings.Count; i++)
            {
                if (availability(bindings[i].Asset))
                {
                    count++;
                    continue;
                }

                report.AddIssue(
                    P0RuntimeVisualBindingCoverageSeverity.Failure,
                    bindings[i].BindingId + " could not resolve " + bindings[i].Asset.AssetId + " at " + bindings[i].Asset.UnityImportPath + ".");
            }

            return count;
        }

        private static bool AllBindingsReady(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            if (bindings == null || bindings.Count == 0)
            {
                return false;
            }

            HashSet<string> ids = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < bindings.Count; i++)
            {
                if (!bindings[i].IsReady || !ids.Add(bindings[i].BindingId) || !bindings[i].Asset.RequiresWorkspaceFile)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool StarterCatBindingsUseTurnaroundLocks(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "cat.combat.saiban", P0VisualAssetCatalog.SaibanCombatSpriteId, "saiban_turnaround_colored")
                && BindingHasSourceLock(bindings, "cat.combat.nephthys", P0VisualAssetCatalog.NephthysCombatSpriteId, "nephthys_turnaround_colored")
                && BindingHasSourceLock(bindings, "cat.combat.suzune", P0VisualAssetCatalog.SuzuneCombatSpriteId, "suzune_turnaround_colored")
                && BindingHasSourceLock(bindings, "cat.avatar.saiban", P0VisualAssetCatalog.SaibanHudAvatarId, "saiban_turnaround_colored")
                && BindingHasSourceLock(bindings, "cat.avatar.nephthys", P0VisualAssetCatalog.NephthysHudAvatarId, "nephthys_turnaround_colored")
                && BindingHasSourceLock(bindings, "cat.avatar.suzune", P0VisualAssetCatalog.SuzuneHudAvatarId, "suzune_turnaround_colored");
        }

        private static bool BattleBackgroundBindingPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "background.bedroom_dream", P0VisualAssetCatalog.BedroomDreamBattleBackgroundId, "bedroom_map_concept");
        }

        private static bool CoreHudBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "core.owner_sleep", P0VisualAssetCatalog.OwnerSleepIconId)
                && BindingHasAsset(bindings, "core.cat_hp", P0VisualAssetCatalog.CatHpIconId)
                && BindingHasAsset(bindings, "core.team_poop", P0VisualAssetCatalog.TeamPoopIconId)
                && BindingHasAsset(bindings, "core.team_hunger", P0VisualAssetCatalog.TeamHungerIconId);
        }

        private static bool SkillHudFeedbackBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "skill_hud.ready_frame", P0VisualAssetCatalog.SkillReadyFrameId)
                && BindingHasAsset(bindings, "skill_hud.cooldown_overlay", P0VisualAssetCatalog.SkillCooldownOverlayId)
                && BindingHasAsset(bindings, "skill_hud.no_target_marker", P0VisualAssetCatalog.SkillNoTargetMarkerId)
                && BindingHasAsset(bindings, "skill_hud.hunger_cost_chip", P0VisualAssetCatalog.SkillHungerCostChipId)
                && BindingHasAsset(bindings, "skill_hud.auto_target_reticle", P0VisualAssetCatalog.AutoTargetReticleId)
                && BindingHasAsset(bindings, "battle_hud.interaction_range_ripple", P0VisualAssetCatalog.InteractionRangeRippleId);
        }

        private static bool StarterSkillVfxBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "skill_vfx.saiban_bedline", P0VisualAssetCatalog.SaibanBedlineSkillVfxId, "saiban_turnaround_colored")
                && BindingHasSourceLock(bindings, "skill_vfx.nephthys_moonsand", P0VisualAssetCatalog.NephthysMoonsandSkillVfxId, "nephthys_turnaround_colored")
                && BindingHasSourceLock(bindings, "skill_vfx.suzune_lullaby", P0VisualAssetCatalog.SuzuneLullabySkillVfxId, "suzune_turnaround_colored");
        }

        private static bool CoreGaugeBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "core_gauge.owner_sleep.frame", P0VisualAssetCatalog.OwnerSleepGaugeFrameId)
                && BindingHasAsset(bindings, "core_gauge.owner_sleep.fill", P0VisualAssetCatalog.OwnerSleepGaugeFillId)
                && BindingHasAsset(bindings, "core_gauge.cat_hp.frame", P0VisualAssetCatalog.CatHpGaugeFrameId)
                && BindingHasAsset(bindings, "core_gauge.cat_hp.fill", P0VisualAssetCatalog.CatHpGaugeFillId)
                && BindingHasAsset(bindings, "core_gauge.team_poop.frame", P0VisualAssetCatalog.TeamPoopGaugeFrameId)
                && BindingHasAsset(bindings, "core_gauge.team_poop.fill", P0VisualAssetCatalog.TeamPoopGaugeFillId)
                && BindingHasAsset(bindings, "core_gauge.team_hunger.frame", P0VisualAssetCatalog.TeamHungerGaugeFrameId)
                && BindingHasAsset(bindings, "core_gauge.team_hunger.fill", P0VisualAssetCatalog.TeamHungerGaugeFillId);
        }

        private static bool StatusIconBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "status.sleep_stable", P0VisualAssetCatalog.SleepStableStatusIconId)
                && BindingHasAsset(bindings, "status.slow", P0VisualAssetCatalog.SlowStatusIconId)
                && BindingHasAsset(bindings, "status.knockback", P0VisualAssetCatalog.KnockbackStatusIconId)
                && BindingHasAsset(bindings, "status.mark", P0VisualAssetCatalog.MarkStatusIconId)
                && BindingHasAsset(bindings, "status.shield", P0VisualAssetCatalog.ShieldStatusIconId);
        }

        private static bool StatusCompactIconBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "status_compact.sleep_stable", P0VisualAssetCatalog.SleepStableStatusCompactIconId)
                && BindingHasAsset(bindings, "status_compact.slow", P0VisualAssetCatalog.SlowStatusCompactIconId)
                && BindingHasAsset(bindings, "status_compact.knockback", P0VisualAssetCatalog.KnockbackStatusCompactIconId)
                && BindingHasAsset(bindings, "status_compact.mark", P0VisualAssetCatalog.MarkStatusCompactIconId)
                && BindingHasAsset(bindings, "status_compact.shield", P0VisualAssetCatalog.ShieldStatusCompactIconId);
        }

        private static bool EnemyWorldBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "enemy.combat.black_mud", P0VisualAssetCatalog.BlackMudCombatSpriteId, "black_mud_concept")
                && BindingHasSourceLock(bindings, "enemy.combat.cold_light", P0VisualAssetCatalog.ColdLightCombatSpriteId, "cold_light_concept")
                && BindingHasSourceLock(bindings, "enemy.combat.call_tyrant", P0VisualAssetCatalog.CallTyrantConceptId, "call_tyrant_concept");
        }

        private static bool PropWorldBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "prop.bed", P0VisualAssetCatalog.BedSpriteId, "bedroom_map_concept")
                && BindingHasSourceLock(bindings, "prop.litter_box", P0VisualAssetCatalog.LitterBoxSpriteId, "bedroom_mid_background_sprites")
                && BindingHasSourceLock(bindings, "prop.feeder", P0VisualAssetCatalog.FeederSpriteId, "bedroom_mid_background_sprites");
        }

        private static bool BossBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "route.boss_node", P0VisualAssetCatalog.BossRouteNodeIconId, "call_tyrant_concept")
                && BindingHasSourceLock(bindings, "warning.call_tyrant", P0VisualAssetCatalog.CallTyrantWarningVfxId, "call_tyrant_animation");
        }

        private static bool EnemyWarningVfxBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "warning.black_mud_bed_claw", P0VisualAssetCatalog.BlackMudBedClawWarningVfxId, "black_mud_animation")
                && BindingHasSourceLock(bindings, "warning.cold_light_beam", P0VisualAssetCatalog.ColdLightBeamWarningVfxId, "cold_light_animation")
                && BindingHasSourceLock(bindings, "warning.call_tyrant_app_throw", P0VisualAssetCatalog.CallTyrantAppThrowVfxId, "call_tyrant_animation")
                && BindingHasSourceLock(bindings, "warning.call_tyrant_summon_portal", P0VisualAssetCatalog.CallTyrantSummonPortalVfxId, "call_tyrant_animation");
        }

        private static bool EnemyAnimationFramesheetBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasSourceLock(bindings, "enemy.anim.black_mud_move", P0VisualAssetCatalog.BlackMudMoveFramesheetId, "black_mud_animation")
                && BindingHasSourceLock(bindings, "enemy.anim.cold_light_cast", P0VisualAssetCatalog.ColdLightCastFramesheetId, "cold_light_animation")
                && BindingHasSourceLock(bindings, "enemy.anim.call_tyrant_boss_pattern", P0VisualAssetCatalog.CallTyrantBossPatternFramesheetId, "call_tyrant_animation");
        }

        private static bool RouteNodeBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "route.defense_node", P0VisualAssetCatalog.DefenseRouteNodeIconId)
                && BindingHasAsset(bindings, "route.elite_node", P0VisualAssetCatalog.EliteRouteNodeIconId)
                && BindingHasAsset(bindings, "route.partner_node", P0VisualAssetCatalog.PartnerRouteNodeIconId)
                && BindingHasAsset(bindings, "route.shop_node", P0VisualAssetCatalog.ShopRouteNodeIconId)
                && BindingHasAsset(bindings, "route.dream_event_node", P0VisualAssetCatalog.DreamEventRouteNodeIconId)
                && BindingHasAsset(bindings, "route.blessing_node", P0VisualAssetCatalog.BlessingRouteNodeIconId)
                && BindingHasAsset(bindings, "route.rest_nest_node", P0VisualAssetCatalog.RestNestRouteNodeIconId)
                && BindingHasAsset(bindings, "route.boss_node", P0VisualAssetCatalog.BossRouteNodeIconId);
        }

        private static bool UiShellBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "main_menu.background", P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId)
                && BindingHasAsset(bindings, "main_menu.title_logo", P0VisualAssetCatalog.TitleLogoId)
                && BindingHasAsset(bindings, "ui.panel.dreamglass", P0VisualAssetCatalog.DreamGlassPanelId)
                && BindingHasAsset(bindings, "ui.button.primary", P0VisualAssetCatalog.PrimaryButtonId)
                && BindingHasAsset(bindings, "settlement.reward.fish_treat", P0VisualAssetCatalog.FishTreatRewardIconId)
                && BindingHasAsset(bindings, "settlement.reward.dream_shard", P0VisualAssetCatalog.DreamShardRewardIconId);
        }

        private static bool RouteChoiceIconBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "route_choice.partner_recruit", P0VisualAssetCatalog.PartnerRecruitChoiceIconId)
                && BindingHasAsset(bindings, "route_choice.purchase_supply", P0VisualAssetCatalog.PurchaseSupplyChoiceIconId)
                && BindingHasAsset(bindings, "route_choice.authority_blessing", P0VisualAssetCatalog.AuthorityBlessingChoiceIconId)
                && BindingHasAsset(bindings, "route_choice.authority_upgrade", P0VisualAssetCatalog.AuthorityUpgradeChoiceIconId)
                && BindingHasAsset(bindings, "route_choice.rest_supply", P0VisualAssetCatalog.RestSupplyChoiceIconId)
                && BindingHasAsset(bindings, "route_choice.dream_event_modifier", P0VisualAssetCatalog.DreamEventModifierChoiceIconId);
        }

        private static bool RouteRewardCardFrameBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "route_reward_card.partner", P0VisualAssetCatalog.PartnerRouteCardFrameId)
                && BindingHasAsset(bindings, "route_reward_card.shop", P0VisualAssetCatalog.ShopRouteCardFrameId)
                && BindingHasAsset(bindings, "route_reward_card.blessing", P0VisualAssetCatalog.BlessingRouteCardFrameId)
                && BindingHasAsset(bindings, "route_reward_card.dream_event", P0VisualAssetCatalog.DreamEventRouteCardFrameId)
                && BindingHasAsset(bindings, "route_reward_card.rest_nest", P0VisualAssetCatalog.RestNestRouteCardFrameId);
        }

        private static bool RouteRewardDetailBadgeBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "route_reward_detail.gain", P0VisualAssetCatalog.RouteRewardGainBadgeId)
                && BindingHasAsset(bindings, "route_reward_detail.cost", P0VisualAssetCatalog.RouteRewardCostBadgeId)
                && BindingHasAsset(bindings, "route_reward_detail.recovery", P0VisualAssetCatalog.RouteRewardRecoveryBadgeId)
                && BindingHasAsset(bindings, "route_reward_detail.risk", P0VisualAssetCatalog.RouteRewardRiskBadgeId)
                && BindingHasAsset(bindings, "route_reward_detail.upgrade", P0VisualAssetCatalog.RouteRewardUpgradeBadgeId);
        }

        private static bool AuthorityBlessingSealBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "blessing_detail.oath_bedline", P0VisualAssetCatalog.AuthorityOathBedlineSealId)
                && BindingHasAsset(bindings, "blessing_detail.dominion_sandglass", P0VisualAssetCatalog.AuthorityDominionSandglassSealId)
                && BindingHasAsset(bindings, "blessing_detail.rhythm_lullaby", P0VisualAssetCatalog.AuthorityRhythmLullabySealId);
        }

        private static bool RouteNodeSummaryBannerBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "route_summary.shop", P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId)
                && BindingHasAsset(bindings, "route_summary.dream_event", P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId)
                && BindingHasAsset(bindings, "route_summary.rest_nest", P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId);
        }

        private static bool ShopItemCardBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "shop_item.bed_patch", P0VisualAssetCatalog.ShopBedPatchCardId)
                && BindingHasAsset(bindings, "shop_item.litter_sachet", P0VisualAssetCatalog.ShopLitterSachetCardId)
                && BindingHasAsset(bindings, "shop_item.late_kibble", P0VisualAssetCatalog.ShopLateKibbleCardId)
                && BindingHasAsset(bindings, "shop_item.free_sample", P0VisualAssetCatalog.ShopFreeSampleCardId);
        }

        private static bool DreamEventChoiceCardBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "dream_event_choice.clear_notifications", P0VisualAssetCatalog.DreamEventClearNotificationsCardId)
                && BindingHasAsset(bindings, "dream_event_choice.catnip_residue", P0VisualAssetCatalog.DreamEventCatnipResidueCardId)
                && BindingHasAsset(bindings, "dream_event_choice.mark_all_read", P0VisualAssetCatalog.DreamEventMarkAllReadCardId);
        }

        private static bool RestNestRecoveryCardBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "rest_nest_choice.recovery", P0VisualAssetCatalog.RestNestRecoveryCardId);
        }

        private static bool PartnerChoiceCardBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "partner_choice.shadowmaru_preview", P0VisualAssetCatalog.PartnerShadowmaruPreviewCardId)
                && BindingHasAsset(bindings, "partner_choice.duplicate_supply", P0VisualAssetCatalog.PartnerDuplicateSupplyCardId);
        }

        private static bool AuthorityBlessingChoiceCardBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "blessing_choice.oath_bedline", P0VisualAssetCatalog.AuthorityOathBedlineCardId)
                && BindingHasAsset(bindings, "blessing_choice.dominion_sandglass", P0VisualAssetCatalog.AuthorityDominionSandglassCardId)
                && BindingHasAsset(bindings, "blessing_choice.rhythm_lullaby", P0VisualAssetCatalog.AuthorityRhythmLullabyCardId);
        }

        private static bool OutcomeBannerBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "battle_result.victory_banner", P0VisualAssetCatalog.BattleResultVictoryBannerId)
                && BindingHasAsset(bindings, "battle_result.defeat_banner", P0VisualAssetCatalog.BattleResultDefeatBannerId)
                && BindingHasAsset(bindings, "settlement.run_cleared_banner", P0VisualAssetCatalog.SettlementRunClearedBannerId)
                && BindingHasAsset(bindings, "settlement.run_failed_banner", P0VisualAssetCatalog.SettlementRunFailedBannerId);
        }

        private static bool BattleFeedbackVfxBindingsPresent(IReadOnlyList<P0VisualAssetBinding> bindings)
        {
            return BindingHasAsset(bindings, "feedback.hit_spark", P0VisualAssetCatalog.HitSparkVfxId)
                && BindingHasAsset(bindings, "feedback.bed_shield_pulse", P0VisualAssetCatalog.BedShieldPulseVfxId)
                && BindingHasAsset(bindings, "feedback.sleep_stable_wave", P0VisualAssetCatalog.SleepStableWaveVfxId)
                && BindingHasAsset(bindings, "feedback.litter_cleanse", P0VisualAssetCatalog.LitterCleanseVfxId)
                && BindingHasAsset(bindings, "feedback.feeder_kibble", P0VisualAssetCatalog.FeederKibbleVfxId)
                && BindingHasAsset(bindings, "feedback.enemy_mark_ring", P0VisualAssetCatalog.EnemyMarkRingVfxId);
        }

        private static bool BindingHasAsset(IReadOnlyList<P0VisualAssetBinding> bindings, string bindingId, string assetId)
        {
            P0VisualAssetBinding? binding = FindBinding(bindings, bindingId);
            return binding.HasValue && binding.Value.Asset.AssetId == assetId;
        }

        private static bool BindingHasSourceLock(
            IReadOnlyList<P0VisualAssetBinding> bindings,
            string bindingId,
            string assetId,
            string sourceLockId)
        {
            P0VisualAssetBinding? binding = FindBinding(bindings, bindingId);
            return binding.HasValue
                && binding.Value.Asset.AssetId == assetId
                && Contains(binding.Value.Asset.SourceLockIds, sourceLockId);
        }

        private static P0VisualAssetBinding? FindBinding(IReadOnlyList<P0VisualAssetBinding> bindings, string bindingId)
        {
            if (bindings == null)
            {
                return null;
            }

            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].BindingId == bindingId)
                {
                    return bindings[i];
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
            P0RuntimeVisualBindingCoverageReport report,
            bool condition,
            string coveredCheck,
            string failureMessage)
        {
            if (condition)
            {
                report.AddCoveredCheck(coveredCheck);
                return;
            }

            report.AddIssue(P0RuntimeVisualBindingCoverageSeverity.Failure, failureMessage);
        }
    }
}
