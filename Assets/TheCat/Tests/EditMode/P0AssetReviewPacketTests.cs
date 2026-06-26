using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0AssetReviewPacketTests
    {
        [Test]
        public void EvaluateP0Packet_BuildsCurrentAssetReviewPacket()
        {
            P0AssetReviewPacketReport report = P0AssetReviewPacket.EvaluateP0Packet();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.ReviewAssetCount);
            Assert.AreEqual(P0AssetManifestCatalog.P0ManifestAssetCount, report.ExistingWorkspaceFileCount);
            Assert.AreEqual(P0HardReferenceSourceLocks.ExpectedManifestLinkedAssetCount, report.SourceLockedEntryCount);
            Assert.AreEqual(P0AssetReviewPacket.ExpectedStarterCatReviewEntryCount, report.StarterCatEntryCount);
            Assert.AreEqual(3, report.StarterCatVisualChecklistCount);
            Assert.AreEqual(15, report.StarterCatVisualTraitCount);
            Assert.AreEqual(3, report.StarterCatTurnaroundConformanceSpecCount);
            Assert.AreEqual(27, report.StarterCatTurnaroundViewAnchorCount);
            Assert.AreEqual(3, report.StarterCatProductionSpecCount);
            Assert.AreEqual(12, report.StarterCatProductionAllowedTypeCount);
            Assert.IsTrue(report.StarterCatSourceLockPacketReady);
            Assert.IsTrue(report.StarterCatTurnaroundComparisonAuditReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedArtifactCount, report.StarterCatTurnaroundComparisonAuditArtifactCount);
            Assert.AreEqual(P0StarterCatTurnaroundComparisonAuditEvidence.ExpectedStarterCatCount, report.StarterCatTurnaroundComparisonAuditManifestRowCount);
            Assert.IsTrue(report.StarterCatReferencePlateReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedArtifactCount, report.StarterCatReferencePlateArtifactCount);
            Assert.AreEqual(P0StarterCatReferencePlateEvidence.ExpectedPlateCount, report.StarterCatReferencePlateCount);
            Assert.IsTrue(report.StarterCatUnityReferenceInstallReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedArtifactCount, report.StarterCatUnityReferenceInstallArtifactCount);
            Assert.AreEqual(P0StarterCatUnityReferenceInstallEvidence.ExpectedInstalledAssetCount, report.StarterCatUnityReferenceInstallAssetCount);
            Assert.IsTrue(report.StarterCatRuntimeCombatSpriteAuditReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedArtifactCount, report.StarterCatRuntimeCombatSpriteAuditArtifactCount);
            Assert.AreEqual(P0StarterCatRuntimeCombatSpriteAuditEvidence.ExpectedRuntimeSpriteCount, report.StarterCatRuntimeCombatSpriteAuditRuntimeSpriteCount);
            Assert.IsTrue(report.StarterCatStrictCandidateReady, report.BuildDetailedSummary());
            Assert.AreEqual(3, report.StarterCatStrictCandidateCount);
            Assert.IsTrue(report.StarterCatFormalImportGateValid, report.BuildDetailedSummary());
            Assert.IsFalse(report.StarterCatFormalImportAllowed, report.BuildDetailedSummary());
            Assert.AreEqual(P0StarterCatFormalImportState.Blocked, report.StarterCatFormalImportState);
            Assert.AreEqual(3, report.StarterCatFormalReviewNoteCount);
            Assert.AreEqual(3, report.StarterCatFormalBlockNoteCount);
            Assert.AreEqual(0, report.StarterCatFormalApprovalNoteCount);
            Assert.AreEqual(P0StarterCatFormalImportReadiness.ExpectedStarterCatCount, report.StarterCatActiveScreenshotCount);
            Assert.IsTrue(report.AssetProductionQueueReady, report.BuildDetailedSummary());
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedP0QueueCount, report.AssetProductionQueueCount);
            Assert.AreEqual(0, report.AssetProductionQueueCodexRunnableCount);
            Assert.AreEqual(P0AssetProductionQueueCatalog.ExpectedCandidatePackCompletePendingUnityReviewCount, report.AssetProductionQueueCandidatePackCompletePendingUnityReviewCount);
            Assert.AreEqual(5, report.AssetProductionQueueUnityBlockedCount);
            Assert.AreEqual(P0VisualAssetCatalog.P0RuntimeVisualBindingCount, report.RuntimeBoundEntryCount);
            Assert.AreEqual(P0AssetReviewPacket.ExpectedCoveredCheckCount, report.CoveredChecks.Count);
        }

        [Test]
        public void BuildMarkdown_ListsStarterCatSourceLocksAndRuntimeBindings()
        {
            P0AssetReviewPacketReport report = P0AssetReviewPacket.Evaluate(
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                P0HardReferenceSourceLocks.CreateP0Locks(),
                P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                _ => true);

            string markdown = report.BuildMarkdown();

            StringAssert.Contains("Starter Cat Turnaround Review", markdown);
            StringAssert.Contains("saiban_turnaround_colored", markdown);
            StringAssert.Contains("nephthys_turnaround_colored", markdown);
            StringAssert.Contains("suzune_turnaround_colored", markdown);
            StringAssert.Contains("cat.combat.saiban", markdown);
            StringAssert.Contains("Starter Cat Visual Consistency Checklist", markdown);
            StringAssert.Contains(P0PlayModeScreenshotSmoke.ActiveCatSaibanCaptureFileName, markdown);
            StringAssert.Contains(P0PlayModeScreenshotSmoke.ActiveCatNephthysCaptureFileName, markdown);
            StringAssert.Contains(P0PlayModeScreenshotSmoke.ActiveCatSuzuneCaptureFileName, markdown);
            StringAssert.Contains("tabby face", markdown);
            StringAssert.Contains("moon-sand Egyptian motif", markdown);
            StringAssert.Contains("calico markings", markdown);
            StringAssert.Contains("Starter Cat Turnaround Conformance Spec", markdown);
            StringAssert.Contains("front view round sun shield", markdown);
            StringAssert.Contains("side view hood volume", markdown);
            StringAssert.Contains("back view large vermilion bow", markdown);
            StringAssert.Contains("Reject generated-lineup", markdown);
            StringAssert.Contains("Starter Cat Asset Production Spec", markdown);
            StringAssert.Contains("design/development/asset_candidates/starter_cats/saiban", markdown);
            StringAssert.Contains("combat_sprite_refinement_512", markdown);
            StringAssert.Contains("side-by-side comparison", markdown);
            StringAssert.Contains("Reject if the candidate drifts from the colored turnaround", markdown);
            StringAssert.Contains("Starter Cat Production Prompt Readiness", markdown);
            StringAssert.Contains("p0_asset_batch_28_starter_cat_strict_reference_pack.md", markdown);
            StringAssert.Contains("p0_asset_batch_29_saiban_strict_turnaround_derivatives.md", markdown);
            StringAssert.Contains("p0_asset_batch_30_saiban_ai_refinement_candidate.md", markdown);
            StringAssert.Contains("p0_asset_batch_31_saiban_cutout_candidate.md", markdown);
            StringAssert.Contains("p0_asset_batch_32_nephthys_strict_turnaround_derivatives.md", markdown);
            StringAssert.Contains("p0_asset_batch_33_suzune_strict_turnaround_derivatives.md", markdown);
            StringAssert.Contains("p0_asset_batch_34_suzune_ai_refinement_candidate.md", markdown);
            StringAssert.Contains("p0_asset_batch_35_suzune_cutout_candidate.md", markdown);
            StringAssert.Contains("p0_asset_batch_36_nephthys_ai_refinement_candidate.md", markdown);
            StringAssert.Contains("p0_asset_batch_37_nephthys_cutout_candidate.md", markdown);
            StringAssert.Contains("Mojibake path mentions: 0", markdown);
            StringAssert.Contains("design/梦境支配者核心玩法/assets", markdown);
            StringAssert.Contains("Starter Cat Source-Lock Packet Evidence", markdown);
            StringAssert.Contains("p0_starter_cat_source_lock_packet_2026-06-14.md", markdown);
            StringAssert.Contains("Source hash mentions", markdown);
            StringAssert.Contains("Core source-lock documents: 3", markdown);
            StringAssert.Contains("Core source-lock documents with exact source paths: 3", markdown);
            StringAssert.Contains("Core source-lock document mojibake mentions: 0", markdown);
            StringAssert.Contains("Starter Cat Turnaround Runtime Comparison Audit", markdown);
            StringAssert.Contains("batch_69_turnaround_runtime_comparison_audit_2026-06-15", markdown);
            StringAssert.Contains("thecat_cat_starter_turnaround_runtime_comparison_batch69_review_sheet.png", markdown);
            StringAssert.Contains("Audit-only recommendations: 3", markdown);
            StringAssert.Contains("Unity .meta files in candidate batch: 0", markdown);
            StringAssert.Contains("Starter Cat Source Turnaround Reference Plates", markdown);
            StringAssert.Contains("batch_70_source_turnaround_reference_plates_2026-06-15", markdown);
            StringAssert.Contains("thecat_cat_starter_turnaround_reference_plates_batch70_review_sheet.png", markdown);
            StringAssert.Contains("Reference plates: 9", markdown);
            StringAssert.Contains("Front/side/back view rows: 9", markdown);
            StringAssert.Contains("Batch 71-73 Starter Cat Unity Reference Installs", markdown);
            StringAssert.Contains("batch_71_saiban_unity_reference_install_2026-06-15", markdown);
            StringAssert.Contains("batch_72_nephthys_unity_reference_install_2026-06-15", markdown);
            StringAssert.Contains("batch_73_suzune_unity_reference_install_2026-06-15", markdown);
            StringAssert.Contains("thecat_cat_saiban_turnaround_reference_atlas_2304x768_v001.png", markdown);
            StringAssert.Contains("thecat_cat_nephthys_turnaround_reference_atlas_2304x768_v001.png", markdown);
            StringAssert.Contains("thecat_cat_suzune_turnaround_reference_atlas_2304x768_v001.png", markdown);
            StringAssert.Contains("Installed debug reference assets: 3", markdown);
            StringAssert.Contains("not runtime-bound", markdown);
            StringAssert.Contains("Starter Cat Runtime Combat Sprite Source Audit", markdown);
            StringAssert.Contains("P0StarterCatRuntimeCombatSpriteAuditEvidence", markdown);
            StringAssert.Contains("runtime-bound sprites; no Unity sprite replacement", markdown);
            StringAssert.Contains("Exact colored-turnaround source paths: 3", markdown);
            StringAssert.Contains("Starter Cat Derivative Candidate Evidence", markdown);
            StringAssert.Contains("batch_05_source_locked_derivatives_2026-06-14", markdown);
            StringAssert.Contains("Candidate PNG files", markdown);
            StringAssert.Contains("registered active-cat screenshots exist, but import remains blocked pending explicit colored-turnaround comparison approval", markdown);
            StringAssert.Contains("Turnaround conformance review notes: yes", markdown);
            StringAssert.Contains("Conformance spec mentions", markdown);
            StringAssert.Contains("Front/side/back anchor sections", markdown);
            StringAssert.Contains("Palette, prop/costume, prohibited-drift sections", markdown);
            StringAssert.Contains("Starter Cat Strict Candidate Evidence", markdown);
            StringAssert.Contains("candidate review only; do not import into Unity", markdown);
            StringAssert.Contains("batch_49_saiban_low_drift_refinement_2026-06-15", markdown);
            StringAssert.Contains("batch_50_nephthys_strict_ai_generation_candidate_2026-06-15", markdown);
            StringAssert.Contains("batch_51_suzune_strict_ai_generation_candidate_2026-06-15", markdown);
            StringAssert.Contains("thecat_cat_saiban_batch49_low_drift_alpha_1024_candidate_v001.png", markdown);
            StringAssert.Contains("thecat_cat_nephthys_batch50_strict_ai_alpha_1024_candidate_v001.png", markdown);
            StringAssert.Contains("thecat_cat_suzune_batch51_strict_ai_alpha_1024_candidate_v001.png", markdown);
            StringAssert.Contains("Exact source turnaround paths: 3", markdown);
            StringAssert.Contains("Batch 47 spec manifest locks: 3", markdown);
            StringAssert.Contains("Batch 47 JSON identity locks: 3", markdown);
            StringAssert.Contains("Starter Cat Formal Import Readiness", markdown);
            StringAssert.Contains("Import allowed: no", markdown);
            StringAssert.Contains("State: Blocked", markdown);
            StringAssert.Contains("Asset Production Queue", markdown);
            StringAssert.Contains("Starter Cat Active Screenshot Validation", markdown);
            StringAssert.Contains("Bedroom Interactable Candidate Pack", markdown);
            StringAssert.Contains("Starter Skill VFX Installed Unity Validation", markdown);
            StringAssert.Contains("Skill HUD Feedback Installed Unity Validation", markdown);
            StringAssert.Contains("Cat Room Preflight Candidate Pack", markdown);
            StringAssert.Contains("Character Select Preflight Candidate Pack", markdown);
            StringAssert.Contains("Battle HUD Preflight Candidate Pack", markdown);
            StringAssert.Contains("Skill Selection Preflight Candidate Pack", markdown);
            StringAssert.Contains("Runtime Control Icon Candidate Pack", markdown);
            StringAssert.Contains("Runtime Control Panel Candidate Pack", markdown);
            StringAssert.Contains("Secondary Enemy Warning Candidate Pack", markdown);
            StringAssert.Contains("CandidatePackCompletePendingUnityReview", markdown);
            StringAssert.Contains("BlockedByUnityValidation", markdown);
            StringAssert.Contains("batch_54_bedroom_interactable_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_55_starter_skill_vfx_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_61_starter_skill_vfx_install_2026-06-15", markdown);
            StringAssert.Contains("batch_57_skill_hud_feedback_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_60_skill_hud_feedback_install_2026-06-15", markdown);
            StringAssert.Contains("batch_90_cat_room_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_cat_room_batch90_manifest.csv", markdown);
            StringAssert.Contains("batch_88_character_select_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_character_select_batch88_manifest.csv", markdown);
            StringAssert.Contains("batch_87_battle_hud_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_battle_hud_batch87_manifest.csv", markdown);
            StringAssert.Contains("batch_89_skill_selection_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_skill_selection_batch89_manifest.csv", markdown);
            StringAssert.Contains("batch_62_runtime_control_icon_candidates_2026-06-15", markdown);
            StringAssert.Contains("runtime_control_icons_batch62_manifest.csv", markdown);
            StringAssert.Contains("batch_63_runtime_control_panel_candidates_2026-06-15", markdown);
            StringAssert.Contains("batch_64_secondary_enemy_warning_candidates_2026-06-15", markdown);
            StringAssert.Contains("runtime_control_panels_batch63_manifest.csv", markdown);
            StringAssert.Contains("Route Map Readability Candidate Pack", markdown);
            StringAssert.Contains("batch_65_route_map_readability_candidates_2026-06-15", markdown);
            StringAssert.Contains("route_map_readability_batch65_manifest.csv", markdown);
            StringAssert.Contains("Bedroom Interaction Affordance Candidate Pack", markdown);
            StringAssert.Contains("batch_67_bedroom_interaction_affordance_candidates_2026-06-15", markdown);
            StringAssert.Contains("bedroom_interaction_affordance_batch67_manifest.csv", markdown);
            StringAssert.Contains("Loading Start Preflight Candidate Pack", markdown);
            StringAssert.Contains("batch_83_loading_start_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_loading_start_batch83_manifest.csv", markdown);
            StringAssert.Contains("Result Settlement Preflight Candidate Pack", markdown);
            StringAssert.Contains("batch_84_result_settlement_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_result_settlement_batch84_manifest.csv", markdown);
            StringAssert.Contains("Settings Pause Preflight Candidate Pack", markdown);
            StringAssert.Contains("batch_85_settings_pause_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_settings_pause_batch85_manifest.csv", markdown);
            StringAssert.Contains("Dream Route Preflight Candidate Pack", markdown);
            StringAssert.Contains("batch_86_dream_route_preflight_2026-06-25", markdown);
            StringAssert.Contains("thecat_ui_dream_route_batch86_manifest.csv", markdown);
            StringAssert.Contains("p0_asset_batch_56_formal_install_decision_packet.md", markdown);
            StringAssert.Contains("Play Mode Screenshot File Evidence", markdown);
            StringAssert.Contains("Existing expected captures", markdown);
            StringAssert.Contains("Runtime Visual Contact Sheet Evidence", markdown);
            StringAssert.Contains("p0_runtime_visual_contact_sheet_2026-06-14.png", markdown);
            StringAssert.Contains("Runtime Visual Binding Review", markdown);
            StringAssert.Contains("background.bedroom_dream", markdown);
            StringAssert.Contains("thecat_bg_bedroomdream_battle_1920x1080_v001", markdown);
            StringAssert.Contains("route.defense_node", markdown);
            StringAssert.Contains("route.rest_nest_node", markdown);
            StringAssert.Contains("status.sleep_stable", markdown);
            StringAssert.Contains("status.shield", markdown);
            StringAssert.Contains("thecat_ui_status_shield_64_v001", markdown);
            StringAssert.Contains("status_compact.sleep_stable", markdown);
            StringAssert.Contains("status_compact.shield", markdown);
            StringAssert.Contains("thecat_ui_status_shield_32_v001", markdown);
            StringAssert.Contains("core_gauge.owner_sleep.frame", markdown);
            StringAssert.Contains("core_gauge.cat_hp.fill", markdown);
            StringAssert.Contains("core_gauge.team_poop.frame", markdown);
            StringAssert.Contains("core_gauge.team_hunger.fill", markdown);
            StringAssert.Contains("thecat_ui_core_hunger_gauge_fill_384x48_v001", markdown);
            StringAssert.Contains("main_menu.background", markdown);
            StringAssert.Contains("thecat_ui_mainmenu_dreamentry_bg_1920x1080_v001", markdown);
            StringAssert.Contains("ui.panel.dreamglass", markdown);
            StringAssert.Contains("settlement.reward.dream_shard", markdown);
            StringAssert.Contains("feedback.hit_spark", markdown);
            StringAssert.Contains("feedback.enemy_mark_ring", markdown);
            StringAssert.Contains("thecat_vfx_bed_shield_pulse_256_v001", markdown);
            StringAssert.Contains("warning.black_mud_bed_claw", markdown);
            StringAssert.Contains("warning.cold_light_beam", markdown);
            StringAssert.Contains("warning.call_tyrant_app_throw", markdown);
            StringAssert.Contains("warning.call_tyrant_summon_portal", markdown);
            StringAssert.Contains("thecat_vfx_calltyrant_summon_portal_256_v001", markdown);
            StringAssert.Contains("enemy.anim.black_mud_move", markdown);
            StringAssert.Contains("enemy.anim.cold_light_cast", markdown);
            StringAssert.Contains("enemy.anim.call_tyrant_boss_pattern", markdown);
            StringAssert.Contains("thecat_enemy_calltyrant_bosspattern_framesheet_4x256_v001", markdown);
            StringAssert.Contains("route_choice.partner_recruit", markdown);
            StringAssert.Contains("route_choice.purchase_supply", markdown);
            StringAssert.Contains("route_choice.authority_blessing", markdown);
            StringAssert.Contains("route_choice.authority_upgrade", markdown);
            StringAssert.Contains("route_choice.rest_supply", markdown);
            StringAssert.Contains("route_choice.dream_event_modifier", markdown);
            StringAssert.Contains("thecat_ui_choice_dreamevent_modifier_icon_128_v001", markdown);
            StringAssert.Contains("route_reward_card.partner", markdown);
            StringAssert.Contains("route_reward_card.shop", markdown);
            StringAssert.Contains("route_reward_card.blessing", markdown);
            StringAssert.Contains("route_reward_card.dream_event", markdown);
            StringAssert.Contains("route_reward_card.rest_nest", markdown);
            StringAssert.Contains("thecat_ui_routecard_restnest_frame_512x256_v001", markdown);
            StringAssert.Contains("route_summary.shop", markdown);
            StringAssert.Contains("route_summary.dream_event", markdown);
            StringAssert.Contains("route_summary.rest_nest", markdown);
            StringAssert.Contains("shop_item.bed_patch", markdown);
            StringAssert.Contains("shop_item.litter_sachet", markdown);
            StringAssert.Contains("shop_item.late_kibble", markdown);
            StringAssert.Contains("shop_item.free_sample", markdown);
            StringAssert.Contains("thecat_ui_shop_item_free_sample_card_384x160_v001", markdown);
            StringAssert.Contains("dream_event_choice.clear_notifications", markdown);
            StringAssert.Contains("dream_event_choice.catnip_residue", markdown);
            StringAssert.Contains("dream_event_choice.mark_all_read", markdown);
            StringAssert.Contains("thecat_ui_dreamevent_catnip_residue_card_384x160_v001", markdown);
            StringAssert.Contains("rest_nest_choice.recovery", markdown);
            StringAssert.Contains("thecat_ui_restnest_recovery_card_384x160_v001", markdown);
            StringAssert.Contains("partner_choice.shadowmaru_preview", markdown);
            StringAssert.Contains("partner_choice.duplicate_supply", markdown);
            StringAssert.Contains("thecat_ui_partner_shadowmaru_preview_card_384x160_v001", markdown);
            StringAssert.Contains("thecat_ui_partner_duplicate_supply_card_384x160_v001", markdown);
            StringAssert.Contains("blessing_choice.oath_bedline", markdown);
            StringAssert.Contains("blessing_choice.dominion_sandglass", markdown);
            StringAssert.Contains("blessing_choice.rhythm_lullaby", markdown);
            StringAssert.Contains("thecat_ui_blessing_oath_bedline_card_384x160_v001", markdown);
            StringAssert.Contains("thecat_ui_blessing_dominion_sandglass_card_384x160_v001", markdown);
            StringAssert.Contains("thecat_ui_blessing_rhythm_lullaby_card_384x160_v001", markdown);
            StringAssert.Contains("battle_result.victory_banner", markdown);
            StringAssert.Contains("battle_result.defeat_banner", markdown);
            StringAssert.Contains("settlement.run_cleared_banner", markdown);
            StringAssert.Contains("settlement.run_failed_banner", markdown);
            StringAssert.Contains("thecat_ui_battle_result_victory_banner_512x160_v001", markdown);
            StringAssert.Contains("thecat_ui_settlement_run_failed_banner_512x160_v001", markdown);
            StringAssert.Contains("thecat_ui_node_restnest_summary_banner_512x160_v001", markdown);
            StringAssert.Contains("blessing_detail.oath_bedline", markdown);
            StringAssert.Contains("blessing_detail.dominion_sandglass", markdown);
            StringAssert.Contains("blessing_detail.rhythm_lullaby", markdown);
            StringAssert.Contains("thecat_ui_blessing_oath_bedline_seal_128_v001", markdown);
        }

        [Test]
        public void Evaluate_MissingStarterCatContactSheetFailsReviewPacket()
        {
            P0AssetReviewPacketReport report = P0AssetReviewPacket.Evaluate(
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                P0HardReferenceSourceLocks.CreateP0Locks(),
                P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                path => path != P0AssetProductionReadiness.StarterCatTurnaroundContactSheetPath);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("Starter cat visual checklist is incomplete", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_MissingRuntimeBindingFailsReviewPacket()
        {
            List<P0VisualAssetBinding> bindings = new List<P0VisualAssetBinding>(P0VisualAssetCatalog.CreateP0RuntimeBindings());
            bindings.RemoveAt(0);

            P0AssetReviewPacketReport report = P0AssetReviewPacket.Evaluate(
                P0AssetManifestCatalog.CreateP0PlannedManifest(),
                P0HardReferenceSourceLocks.CreateP0Locks(),
                bindings,
                _ => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("Runtime visual bindings are not fully represented", report.BuildDetailedSummary());
        }

        [Test]
        public void Evaluate_UnknownSourceLockFailsReviewPacket()
        {
            IReadOnlyList<P0AssetManifestEntry> manifest = MutateManifest(
                P0VisualAssetCatalog.SaibanCombatSpriteId,
                entry => new P0AssetManifestEntry(
                    entry.AssetId,
                    entry.SubjectId,
                    entry.AssetType,
                    entry.Priority,
                    entry.SourcePromptPath,
                    entry.ReferenceAssetIds,
                    new[] { "missing_turnaround_lock" },
                    entry.UnityImportPath,
                    entry.Size,
                    entry.Status,
                    entry.ConsistencyNotes));

            P0AssetReviewPacketReport report = P0AssetReviewPacket.Evaluate(
                manifest,
                P0HardReferenceSourceLocks.CreateP0Locks(),
                P0VisualAssetCatalog.CreateP0RuntimeBindings(),
                _ => true);

            Assert.IsFalse(report.IsReady);
            StringAssert.Contains("unknown source lock missing_turnaround_lock", report.BuildDetailedSummary());
        }

        private static IReadOnlyList<P0AssetManifestEntry> MutateManifest(
            string assetId,
            Func<P0AssetManifestEntry, P0AssetManifestEntry> mutate)
        {
            List<P0AssetManifestEntry> result = new List<P0AssetManifestEntry>();
            foreach (P0AssetManifestEntry entry in P0AssetManifestCatalog.CreateP0PlannedManifest())
            {
                result.Add(entry.AssetId == assetId ? mutate(entry) : entry);
            }

            return result;
        }
    }
}
