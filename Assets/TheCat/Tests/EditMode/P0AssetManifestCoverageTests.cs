using System.Linq;
using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetManifestCoverageTests
    {
        [Test]
        public void EvaluateP0Manifest_CompletesAssetManifestGate()
        {
            P0AssetManifestCoverageReport report = P0AssetManifestCoverage.EvaluateP0Manifest();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetManifestCoverage.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("asset manifest coverage complete", report.BuildSummary());
            StringAssert.Contains("style anchors", report.BuildDetailedSummary());
            StringAssert.Contains("colored turnaround", report.BuildDetailedSummary());
            StringAssert.Contains("source-lock ids", report.BuildDetailedSummary());
            StringAssert.Contains("hard source reference", report.BuildDetailedSummary());
            StringAssert.Contains("bed, litter box, and feeder", report.BuildDetailedSummary());
            StringAssert.Contains("HUD icons", report.BuildDetailedSummary());
            StringAssert.Contains("core gauge bars", report.BuildDetailedSummary());
            StringAssert.Contains("source-split icons", report.BuildDetailedSummary());
            StringAssert.Contains("32px compact status HUD icons", report.BuildDetailedSummary());
            StringAssert.Contains("route node icons", report.BuildDetailedSummary());
            StringAssert.Contains("UI shell", report.BuildDetailedSummary());
            StringAssert.Contains("battle feedback VFX", report.BuildDetailedSummary());
            StringAssert.Contains("enemy warning VFX", report.BuildDetailedSummary());
            StringAssert.Contains("enemy animation framesheets", report.BuildDetailedSummary());
            StringAssert.Contains("route choice icons", report.BuildDetailedSummary());
            StringAssert.Contains("route reward card frames", report.BuildDetailedSummary());
            StringAssert.Contains("route node summary banners", report.BuildDetailedSummary());
            StringAssert.Contains("shop item cards", report.BuildDetailedSummary());
            StringAssert.Contains("dream-event choice cards", report.BuildDetailedSummary());
            StringAssert.Contains("RestNest recovery card", report.BuildDetailedSummary());
            StringAssert.Contains("partner choice cards", report.BuildDetailedSummary());
            StringAssert.Contains("authority blessing choice cards", report.BuildDetailedSummary());
            StringAssert.Contains("outcome banners", report.BuildDetailedSummary());
            StringAssert.Contains("route reward detail badges", report.BuildDetailedSummary());
            StringAssert.Contains("authority blessing seals", report.BuildDetailedSummary());
        }

        [Test]
        public void Catalog_CoversP0GenerationBatches()
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();

            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, manifest.Count);
            AssertHasSubject(manifest, "bedroom_dream_battle", "background");
            AssertSourceLock(manifest, "bedroom_dream_battle", "background", "bedroom_map_concept");
            AssertHasSubject(manifest, "starter_cats", "concept");
            AssertHasSubject(manifest, P0PrototypeCatalog.SaibanId, "sprite");
            AssertHasSubject(manifest, P0PrototypeCatalog.NephthysId, "sprite");
            AssertHasSubject(manifest, P0PrototypeCatalog.SuzuneId, "sprite");
            AssertSourceLock(manifest, P0PrototypeCatalog.SaibanId, "sprite", "saiban_turnaround_colored");
            AssertSourceLock(manifest, P0PrototypeCatalog.NephthysId, "sprite", "nephthys_turnaround_colored");
            AssertSourceLock(manifest, P0PrototypeCatalog.SuzuneId, "sprite", "suzune_turnaround_colored");
            AssertHasSubject(manifest, "saiban_turnaround_reference_atlas", "reference_atlas");
            AssertHasSubject(manifest, "nephthys_turnaround_reference_atlas", "reference_atlas");
            AssertHasSubject(manifest, "suzune_turnaround_reference_atlas", "reference_atlas");
            AssertSourceLock(manifest, "saiban_turnaround_reference_atlas", "reference_atlas", "saiban_turnaround_colored");
            AssertSourceLock(manifest, "nephthys_turnaround_reference_atlas", "reference_atlas", "nephthys_turnaround_colored");
            AssertSourceLock(manifest, "suzune_turnaround_reference_atlas", "reference_atlas", "suzune_turnaround_colored");
            AssertHasSubject(manifest, P0PrototypeCatalog.BlackMudNightmareId, "sprite");
            AssertHasSubject(manifest, P0PrototypeCatalog.ColdLightShadowId, "sprite");
            AssertHasSubject(manifest, P0PrototypeCatalog.CallTyrantId, "concept");
            AssertHasSubject(manifest, "bed", "sprite");
            AssertHasSubject(manifest, "litter_box", "sprite");
            AssertHasSubject(manifest, "feeder", "sprite");
            AssertHasSubject(manifest, "owner_sleep", "icon");
            AssertHasSubject(manifest, "team_hunger", "icon");
            AssertHasSubject(manifest, "owner_sleep_gauge_frame", "frame");
            AssertHasSubject(manifest, "owner_sleep_gauge_fill", "bar");
            AssertHasSubject(manifest, "cat_hp_gauge_frame", "frame");
            AssertHasSubject(manifest, "cat_hp_gauge_fill", "bar");
            AssertHasSubject(manifest, "team_poop_gauge_frame", "frame");
            AssertHasSubject(manifest, "team_poop_gauge_fill", "bar");
            AssertHasSubject(manifest, "team_hunger_gauge_frame", "frame");
            AssertHasSubject(manifest, "team_hunger_gauge_fill", "bar");
            AssertHasSubject(manifest, StatusTagIds.SleepStable, "icon");
            AssertHasSubject(manifest, StatusTagIds.Slow, "icon");
            AssertHasSubject(manifest, StatusTagIds.Knockback, "icon");
            AssertHasSubject(manifest, StatusTagIds.Mark, "icon");
            AssertHasSubject(manifest, StatusTagIds.Shield, "icon");
            AssertHasSubject(manifest, "sleep_stable_compact", "icon");
            AssertHasSubject(manifest, "slow_compact", "icon");
            AssertHasSubject(manifest, "knockback_compact", "icon");
            AssertHasSubject(manifest, "mark_compact", "icon");
            AssertHasSubject(manifest, "shield_compact", "icon");
            AssertHasSubject(manifest, "call_tyrant_warning", "vfx");
            AssertHasSubject(manifest, "black_mud_bed_claw", "vfx");
            AssertHasSubject(manifest, "cold_light_beam_warning", "vfx");
            AssertHasSubject(manifest, "call_tyrant_app_throw", "vfx");
            AssertHasSubject(manifest, "call_tyrant_summon_portal", "vfx");
            AssertSourceLock(manifest, "black_mud_bed_claw", "vfx", "black_mud_animation");
            AssertSourceLock(manifest, "cold_light_beam_warning", "vfx", "cold_light_animation");
            AssertSourceLock(manifest, "call_tyrant_app_throw", "vfx", "call_tyrant_animation");
            AssertSourceLock(manifest, "call_tyrant_summon_portal", "vfx", "call_tyrant_animation");
            AssertHasSubject(manifest, "black_mud_move", "framesheet");
            AssertHasSubject(manifest, "cold_light_cast", "framesheet");
            AssertHasSubject(manifest, "call_tyrant_boss_pattern", "framesheet");
            AssertSourceLock(manifest, "black_mud_move", "framesheet", "black_mud_animation");
            AssertSourceLock(manifest, "cold_light_cast", "framesheet", "cold_light_animation");
            AssertSourceLock(manifest, "call_tyrant_boss_pattern", "framesheet", "call_tyrant_animation");
            AssertHasSubject(manifest, "boss_route_node", "icon");
            AssertHasSubject(manifest, "defense_route_node", "icon");
            AssertHasSubject(manifest, "elite_route_node", "icon");
            AssertHasSubject(manifest, "partner_route_node", "icon");
            AssertHasSubject(manifest, "shop_route_node", "icon");
            AssertHasSubject(manifest, "dream_event_route_node", "icon");
            AssertHasSubject(manifest, "blessing_route_node", "icon");
            AssertHasSubject(manifest, "rest_nest_route_node", "icon");
            AssertHasSubject(manifest, "main_menu", "background");
            AssertHasSubject(manifest, "game_title", "frame");
            AssertHasSubject(manifest, "dreamglass_panel", "frame");
            AssertHasSubject(manifest, "primary_button", "frame");
            AssertHasSubject(manifest, "fish_treats", "icon");
            AssertHasSubject(manifest, "dream_shards", "icon");
            AssertHasSubject(manifest, "partner_recruit_choice", "icon");
            AssertHasSubject(manifest, "purchase_supply_choice", "icon");
            AssertHasSubject(manifest, "authority_blessing_choice", "icon");
            AssertHasSubject(manifest, "authority_upgrade_choice", "icon");
            AssertHasSubject(manifest, "rest_supply_choice", "icon");
            AssertHasSubject(manifest, "dream_event_modifier_choice", "icon");
            AssertHasSubject(manifest, "partner_route_card", "frame");
            AssertHasSubject(manifest, "shop_route_card", "frame");
            AssertHasSubject(manifest, "blessing_route_card", "frame");
            AssertHasSubject(manifest, "dream_event_route_card", "frame");
            AssertHasSubject(manifest, "rest_nest_route_card", "frame");
            AssertHasSubject(manifest, "shop_node_summary_banner", "banner");
            AssertHasSubject(manifest, "dream_event_node_summary_banner", "banner");
            AssertHasSubject(manifest, "rest_nest_node_summary_banner", "banner");
            AssertHasSubject(manifest, "shop_bed_patch", "card");
            AssertHasSubject(manifest, "shop_litter_sachet", "card");
            AssertHasSubject(manifest, "shop_late_kibble", "card");
            AssertHasSubject(manifest, "shop_free_sample", "card");
            AssertHasSubject(manifest, "dream_event_clear_notifications", "card");
            AssertHasSubject(manifest, "dream_event_catnip_residue", "card");
            AssertHasSubject(manifest, "dream_event_mark_all_read", "card");
            AssertHasSubject(manifest, "rest_nest_recovery", "card");
            AssertHasSubject(manifest, "partner_shadowmaru_preview", "card");
            AssertHasSubject(manifest, "partner_preview_duplicate_supply", "card");
            AssertHasSubject(manifest, "blessing_authority_oath_bedline", "card");
            AssertHasSubject(manifest, "blessing_authority_dominion_sandglass", "card");
            AssertHasSubject(manifest, "blessing_authority_rhythm_lullaby", "card");
            AssertHasSubject(manifest, "battle_result_victory", "banner");
            AssertHasSubject(manifest, "battle_result_defeat", "banner");
            AssertHasSubject(manifest, "settlement_run_cleared", "banner");
            AssertHasSubject(manifest, "settlement_run_failed", "banner");
            AssertHasSubject(manifest, "route_reward_gain_detail", "badge");
            AssertHasSubject(manifest, "route_reward_cost_detail", "badge");
            AssertHasSubject(manifest, "route_reward_recovery_detail", "badge");
            AssertHasSubject(manifest, "route_reward_risk_detail", "badge");
            AssertHasSubject(manifest, "route_reward_upgrade_detail", "badge");
            AssertHasSubject(manifest, "authority_oath_bedline_seal", "icon");
            AssertHasSubject(manifest, "authority_dominion_sandglass_seal", "icon");
            AssertHasSubject(manifest, "authority_rhythm_lullaby_seal", "icon");
            AssertHasSubject(manifest, "hit_spark", "vfx");
            AssertHasSubject(manifest, "bed_shield_pulse", "vfx");
            AssertHasSubject(manifest, "sleep_stable_wave", "vfx");
            AssertHasSubject(manifest, "litter_cleanse", "vfx");
            AssertHasSubject(manifest, "feeder_kibble", "vfx");
            AssertHasSubject(manifest, "enemy_mark_ring", "vfx");
            AssertHasSubject(manifest, "saiban_bedline_skill_vfx", "vfx");
            AssertHasSubject(manifest, "nephthys_moonsand_skill_vfx", "vfx");
            AssertHasSubject(manifest, "suzune_lullaby_skill_vfx", "vfx");
            AssertSourceLock(manifest, "saiban_bedline_skill_vfx", "vfx", "saiban_turnaround_colored");
            AssertSourceLock(manifest, "nephthys_moonsand_skill_vfx", "vfx", "nephthys_turnaround_colored");
            AssertSourceLock(manifest, "suzune_lullaby_skill_vfx", "vfx", "suzune_turnaround_colored");
        }

        [Test]
        public void Catalog_UsesVersionedUnityArtPaths()
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest();

            foreach (P0AssetManifestEntry entry in manifest)
            {
                StringAssert.StartsWith("thecat_", entry.AssetId);
                StringAssert.Contains("_v", entry.AssetId);
                StringAssert.StartsWith("Assets/TheCat/Art/", entry.UnityImportPath);
                StringAssert.EndsWith(entry.AssetId + ".png", entry.UnityImportPath);
                Assert.IsTrue(
                    entry.SourcePromptPath.StartsWith("design/development/prompts/")
                    || entry.SourcePromptPath.StartsWith("design/development/agent_prompts/"),
                    entry.AssetId + " source prompt path");
                Assert.AreEqual("P0", entry.Priority);
                Assert.IsTrue(P0AssetManifestStatus.IsKnown(entry.Status), entry.AssetId + " status");
                Assert.IsNotEmpty(entry.ConsistencyNotes);
            }
        }

        [Test]
        public void Evaluate_FailsWhenSourceSensitiveRowsLackHardReferenceNotes()
        {
            var manifest = P0AssetManifestCatalog.CreateP0PlannedManifest()
                .Select(entry => entry.SubjectId == P0PrototypeCatalog.BlackMudNightmareId && entry.AssetType == "sprite"
                    ? new P0AssetManifestEntry(
                        entry.AssetId,
                        entry.SubjectId,
                        entry.AssetType,
                        entry.Priority,
                        entry.SourcePromptPath,
                        entry.ReferenceAssetIds,
                        entry.UnityImportPath,
                        entry.Size,
                        entry.Status,
                        "Black Mud Nightmare gameplay sprite")
                    : entry)
                .ToList();

            P0AssetManifestCoverageReport report = P0AssetManifestCoverage.Evaluate(manifest);

            Assert.IsFalse(report.IsComplete, report.BuildDetailedSummary());
            Assert.Greater(report.FailureCount, 0);
            StringAssert.Contains("hard source reference", report.BuildDetailedSummary());
        }

        private static void AssertHasSubject(
            System.Collections.Generic.IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType)
        {
            Assert.IsTrue(
                manifest.Any(entry => entry.SubjectId == subjectId && entry.AssetType == assetType),
                subjectId + " " + assetType);
        }

        private static void AssertSourceLock(
            System.Collections.Generic.IReadOnlyList<P0AssetManifestEntry> manifest,
            string subjectId,
            string assetType,
            string sourceLockId)
        {
            Assert.IsTrue(
                manifest.Any(entry => entry.SubjectId == subjectId
                    && entry.AssetType == assetType
                    && entry.SourceLockIds.Contains(sourceLockId)),
                subjectId + " " + assetType + " source lock " + sourceLockId);
        }
    }
}
