using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class P0VisualAssetCatalogTests
    {
        [Test]
        public void GetStarterCatCombatSprite_ResolvesTurnaroundLockedSprites()
        {
            P0VisualAssetReference saiban = P0VisualAssetCatalog.GetStarterCatCombatSprite(P0PrototypeCatalog.SaibanId);
            P0VisualAssetReference nephthys = P0VisualAssetCatalog.GetStarterCatCombatSprite(P0PrototypeCatalog.NephthysId);
            P0VisualAssetReference suzune = P0VisualAssetCatalog.GetStarterCatCombatSprite(P0PrototypeCatalog.SuzuneId);

            Assert.AreEqual(P0VisualAssetCatalog.SaibanCombatSpriteId, saiban.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.NephthysCombatSpriteId, nephthys.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SuzuneCombatSpriteId, suzune.AssetId);
            Assert.IsTrue(Contains(saiban.SourceLockIds, "saiban_turnaround_colored"));
            Assert.IsTrue(Contains(nephthys.SourceLockIds, "nephthys_turnaround_colored"));
            Assert.IsTrue(Contains(suzune.SourceLockIds, "suzune_turnaround_colored"));
        }

        [Test]
        public void GetStarterCatHudAvatar_ResolvesTurnaroundLockedAvatars()
        {
            P0VisualAssetReference saiban = P0VisualAssetCatalog.GetStarterCatHudAvatar(P0PrototypeCatalog.SaibanId);
            P0VisualAssetReference nephthys = P0VisualAssetCatalog.GetStarterCatHudAvatar(P0PrototypeCatalog.NephthysId);
            P0VisualAssetReference suzune = P0VisualAssetCatalog.GetStarterCatHudAvatar(P0PrototypeCatalog.SuzuneId);

            Assert.AreEqual(P0VisualAssetCatalog.SaibanHudAvatarId, saiban.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.NephthysHudAvatarId, nephthys.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SuzuneHudAvatarId, suzune.AssetId);
            Assert.AreEqual("avatar_icon", saiban.AssetType);
            Assert.AreEqual("avatar_icon", nephthys.AssetType);
            Assert.AreEqual("avatar_icon", suzune.AssetType);
            Assert.IsTrue(Contains(saiban.SourceLockIds, "saiban_turnaround_colored"));
            Assert.IsTrue(Contains(nephthys.SourceLockIds, "nephthys_turnaround_colored"));
            Assert.IsTrue(Contains(suzune.SourceLockIds, "suzune_turnaround_colored"));
        }

        [Test]
        public void GetCoreValueIcons_ResolveHudIconAssets()
        {
            Assert.AreEqual(P0VisualAssetCatalog.OwnerSleepIconId, P0VisualAssetCatalog.GetOwnerSleepIcon().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.CatHpIconId, P0VisualAssetCatalog.GetCatHpIcon().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopIconId, P0VisualAssetCatalog.GetTeamPoopIcon().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamHungerIconId, P0VisualAssetCatalog.GetTeamHungerIcon().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopIconId, P0VisualAssetCatalog.GetCoreValueIcon("team_poop").AssetId);
            Assert.IsFalse(P0VisualAssetCatalog.GetCoreValueIcon("missing").HasAsset);
        }

        [Test]
        public void GetSkillHudFeedback_ResolveBatch60InstalledAssets()
        {
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetSkillHudStatusFeedback("ready"), P0VisualAssetCatalog.SkillReadyFrameId);
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetSkillHudStatusFeedback("cooldown"), P0VisualAssetCatalog.SkillCooldownOverlayId);
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetSkillHudStatusFeedback("no_target"), P0VisualAssetCatalog.SkillNoTargetMarkerId);
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetSkillHudStatusFeedback("low_hunger"), P0VisualAssetCatalog.SkillHungerCostChipId);
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetSkillHudHungerCostChip(), P0VisualAssetCatalog.SkillHungerCostChipId);
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetAutoTargetReticle(), P0VisualAssetCatalog.AutoTargetReticleId);
            AssertSkillHudFeedback(P0VisualAssetCatalog.GetInteractionRangeRipple(), P0VisualAssetCatalog.InteractionRangeRippleId);
            Assert.IsFalse(P0VisualAssetCatalog.GetSkillHudStatusFeedback("missing").HasAsset);
        }

        [Test]
        public void GetCoreGaugeBars_ResolveFrameAndFillAssets()
        {
            Assert.AreEqual(P0VisualAssetCatalog.OwnerSleepGaugeFrameId, P0VisualAssetCatalog.GetCoreGaugeFrame("owner_sleep").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.OwnerSleepGaugeFillId, P0VisualAssetCatalog.GetCoreGaugeFill("owner_sleep").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.CatHpGaugeFrameId, P0VisualAssetCatalog.GetCoreGaugeFrame("cat_hp").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.CatHpGaugeFillId, P0VisualAssetCatalog.GetCoreGaugeFill("cat_hp").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopGaugeFrameId, P0VisualAssetCatalog.GetCoreGaugeFrame("team_poop").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopGaugeFillId, P0VisualAssetCatalog.GetCoreGaugeFill("team_poop").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamHungerGaugeFrameId, P0VisualAssetCatalog.GetCoreGaugeFrame("team_hunger").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamHungerGaugeFillId, P0VisualAssetCatalog.GetCoreGaugeFill("team_hunger").AssetId);
            Assert.AreEqual("frame", P0VisualAssetCatalog.GetCoreGaugeFrame("cat_hp").AssetType);
            Assert.AreEqual("bar", P0VisualAssetCatalog.GetCoreGaugeFill("cat_hp").AssetType);
            Assert.IsFalse(P0VisualAssetCatalog.GetCoreGaugeFrame("missing").HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetCoreGaugeFill("missing").HasAsset);
        }

        [Test]
        public void GetStatusIcons_ResolveP0StatusTagIconAssets()
        {
            Assert.AreEqual(P0VisualAssetCatalog.SleepStableStatusIconId, P0VisualAssetCatalog.GetStatusIcon(StatusTagIds.SleepStable).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SlowStatusIconId, P0VisualAssetCatalog.GetStatusIcon(StatusTagIds.Slow).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.KnockbackStatusIconId, P0VisualAssetCatalog.GetStatusIcon(StatusTagIds.Knockback).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.MarkStatusIconId, P0VisualAssetCatalog.GetStatusIcon(StatusTagIds.Mark).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.ShieldStatusIconId, P0VisualAssetCatalog.GetStatusIcon(StatusTagIds.Shield).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SleepStableStatusCompactIconId, P0VisualAssetCatalog.GetCompactStatusIcon(StatusTagIds.SleepStable).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SlowStatusCompactIconId, P0VisualAssetCatalog.GetCompactStatusIcon(StatusTagIds.Slow).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.KnockbackStatusCompactIconId, P0VisualAssetCatalog.GetCompactStatusIcon(StatusTagIds.Knockback).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.MarkStatusCompactIconId, P0VisualAssetCatalog.GetCompactStatusIcon(StatusTagIds.Mark).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.ShieldStatusCompactIconId, P0VisualAssetCatalog.GetCompactStatusIcon(StatusTagIds.Shield).AssetId);
            Assert.IsFalse(P0VisualAssetCatalog.GetStatusIcon("missing").HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetCompactStatusIcon("missing").HasAsset);
        }

        [Test]
        public void GetEnemyCombatSprites_ResolveWorldEnemyAssets()
        {
            P0VisualAssetReference blackMud = P0VisualAssetCatalog.GetEnemyCombatSprite(P0PrototypeCatalog.BlackMudNightmareId);
            P0VisualAssetReference coldLight = P0VisualAssetCatalog.GetEnemyCombatSprite(P0PrototypeCatalog.ColdLightShadowId);
            P0VisualAssetReference callTyrant = P0VisualAssetCatalog.GetEnemyCombatSprite(P0PrototypeCatalog.CallTyrantId);

            Assert.AreEqual(P0VisualAssetCatalog.BlackMudCombatSpriteId, blackMud.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.ColdLightCombatSpriteId, coldLight.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.CallTyrantConceptId, callTyrant.AssetId);
            Assert.IsTrue(Contains(blackMud.SourceLockIds, "black_mud_concept"));
            Assert.IsTrue(Contains(coldLight.SourceLockIds, "cold_light_concept"));
            Assert.IsTrue(Contains(callTyrant.SourceLockIds, "call_tyrant_concept"));
        }

        [Test]
        public void GetInteractableSprites_ResolveBedroomWorldProps()
        {
            Assert.AreEqual(P0VisualAssetCatalog.BedSpriteId, P0VisualAssetCatalog.GetBedSprite().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.LitterBoxSpriteId, P0VisualAssetCatalog.GetLitterBoxSprite().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.FeederSpriteId, P0VisualAssetCatalog.GetFeederSprite().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.LitterBoxSpriteId, P0VisualAssetCatalog.GetInteractableSprite("litter_box").AssetId);
            Assert.IsFalse(P0VisualAssetCatalog.GetInteractableSprite("missing").HasAsset);
        }

        [Test]
        public void GetBossRouteNodeIcon_ResolvesGeneratedManifestAsset()
        {
            P0VisualAssetReference asset = P0VisualAssetCatalog.GetBossRouteNodeIcon();

            Assert.AreEqual(P0VisualAssetCatalog.BossRouteNodeIconId, asset.AssetId);
            Assert.AreEqual("icon", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith("thecat_ui_route_bossnode_icon_128_v001.png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.SourceLockIds, "call_tyrant_concept"));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.CallTyrantConceptId));
        }

        [Test]
        public void GetRouteNodeIcon_ResolvesAllP0RouteNodeTypeIcons()
        {
            Assert.AreEqual(P0VisualAssetCatalog.DefenseRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Defense).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.EliteRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Elite).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PartnerRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Partner).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.ShopRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Shop).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamEventRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.DreamEvent).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.BlessingRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.BlessingOffering).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.RestNestRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.RestNest).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.BossRouteNodeIconId, P0VisualAssetCatalog.GetRouteNodeIcon(RouteNodeType.Boss).AssetId);
        }

        [Test]
        public void GetRouteNodeSummaryBanners_ResolveNonBattleNodeBanners()
        {
            AssertRouteNodeSummaryBanner(
                P0VisualAssetCatalog.GetRouteNodeSummaryBanner(RouteNodeType.Shop),
                P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId);
            AssertRouteNodeSummaryBanner(
                P0VisualAssetCatalog.GetRouteNodeSummaryBanner(RouteNodeType.DreamEvent),
                P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId);
            AssertRouteNodeSummaryBanner(
                P0VisualAssetCatalog.GetRouteNodeSummaryBanner(RouteNodeType.RestNest),
                P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId);
            Assert.IsFalse(P0VisualAssetCatalog.GetRouteNodeSummaryBanner(RouteNodeType.Defense).HasAsset);
        }

        [Test]
        public void GetShopItemCards_ResolveSpecificShopChoiceCards()
        {
            AssertShopItemCard(
                P0VisualAssetCatalog.GetShopItemCard(new RouteRewardChoice("shop_bed_patch", "Bed Patch", "Bed Patch", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 3, ownerSleepRestored: 20)),
                P0VisualAssetCatalog.ShopBedPatchCardId);
            AssertShopItemCard(
                P0VisualAssetCatalog.GetShopItemCard(new RouteRewardChoice("shop_litter_sachet", "Litter Sachet", "Litter Sachet", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 2, poopReduced: 30)),
                P0VisualAssetCatalog.ShopLitterSachetCardId);
            AssertShopItemCard(
                P0VisualAssetCatalog.GetShopItemCard(new RouteRewardChoice("shop_late_kibble", "Late Kibble", "Late Kibble", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 2, hungerSafeLine: 85)),
                P0VisualAssetCatalog.ShopLateKibbleCardId);
            AssertShopItemCard(
                P0VisualAssetCatalog.GetShopItemCard(new RouteRewardChoice("shop_free_sample", "Free Sample", "Free Sample", RouteRewardChoiceType.GainFishTreats, fishTreatsGained: 1)),
                P0VisualAssetCatalog.ShopFreeSampleCardId);
            Assert.IsFalse(P0VisualAssetCatalog.GetShopItemCard(new RouteRewardChoice("shop_authority_oath", "Authority", "Authority", RouteRewardChoiceType.GainAuthorityBlessing, authorityBlessing: P0BlessingCatalog.CreateAuthorityBlessings()[0])).HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetShopItemCard(null).HasAsset);
        }

        [Test]
        public void GetDreamEventChoiceCards_ResolveSpecificDreamEventChoiceCards()
        {
            AssertDreamEventChoiceCard(
                P0VisualAssetCatalog.GetDreamEventChoiceCard(new RouteRewardChoice("dream_event_clear_notifications", "Clear Red Dots", "Clear Red Dots", RouteRewardChoiceType.GainFishTreats, fishTreatsGained: 2)),
                P0VisualAssetCatalog.DreamEventClearNotificationsCardId);
            AssertDreamEventChoiceCard(
                P0VisualAssetCatalog.GetDreamEventChoiceCard(new RouteRewardChoice("dream_event_catnip_residue", "Breathe Catnip Residue", "Breathe Catnip Residue", RouteRewardChoiceType.DreamEventModifier, nextBattleSkillDamagePercent: 20, nextBattlePoopGrowthPercent: 50)),
                P0VisualAssetCatalog.DreamEventCatnipResidueCardId);
            AssertDreamEventChoiceCard(
                P0VisualAssetCatalog.GetDreamEventChoiceCard(new RouteRewardChoice("dream_event_red_dot_ignore", "Ignore Red Dots", "Ignore Red Dots", RouteRewardChoiceType.GainEventItem, nextBattlePoopGrowthPercent: 75, eventItemId: RunEventItemInventory.BlankWishTagId, eventItemCount: 1)),
                P0VisualAssetCatalog.DreamEventCatnipResidueCardId);
            AssertDreamEventChoiceCard(
                P0VisualAssetCatalog.GetDreamEventChoiceCard(new RouteRewardChoice("dream_event_mark_all_read", "Mark All Read", "Mark All Read", RouteRewardChoiceType.GainFishTreats, ownerSleepRestored: 12)),
                P0VisualAssetCatalog.DreamEventMarkAllReadCardId);
            AssertDreamEventChoiceCard(
                P0VisualAssetCatalog.GetDreamEventChoiceCard(new RouteRewardChoice("dream_event_blank_wish_extra", "Wish Tag", "Wish Tag", RouteRewardChoiceType.GainEventItem, eventItemId: RunEventItemInventory.FadedFishBagId, eventItemCount: 1)),
                P0VisualAssetCatalog.DreamEventMarkAllReadCardId);
            Assert.IsFalse(P0VisualAssetCatalog.GetDreamEventChoiceCard(new RouteRewardChoice("shop_bed_patch", "Bed Patch", "Bed Patch", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 3)).HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetDreamEventChoiceCard(null).HasAsset);
        }

        [Test]
        public void GetRestNestChoiceCard_ResolvesRestNestRecoveryCard()
        {
            AssertRestNestRecoveryCard(
                P0VisualAssetCatalog.GetRestNestChoiceCard(new RouteRewardChoice("rest_nest_recovery", "Repair the Bedline", "Repair the Bedline", RouteRewardChoiceType.RestSupply, ownerSleepRestored: 25, poopReduced: 35, hungerSafeLine: 75, catHpSafePercent: 60)),
                P0VisualAssetCatalog.RestNestRecoveryCardId);
            Assert.IsFalse(P0VisualAssetCatalog.GetRestNestChoiceCard(new RouteRewardChoice("shop_bed_patch", "Bed Patch", "Bed Patch", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 3)).HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetRestNestChoiceCard(null).HasAsset);
        }

        [Test]
        public void GetPartnerChoiceCards_ResolveSpecificPartnerChoiceCards()
        {
            AssertPartnerChoiceCard(
                P0VisualAssetCatalog.GetPartnerChoiceCard(new RouteRewardChoice("partner_shadowmaru_preview", "Invite Shadowmaru", "Invite Shadowmaru", RouteRewardChoiceType.RecruitPartner, fishTreatsGained: 1, partnerCatId: P0RouteRewardResolver.PreviewPartnerId)),
                P0VisualAssetCatalog.PartnerShadowmaruPreviewCardId);
            AssertPartnerChoiceCard(
                P0VisualAssetCatalog.GetPartnerChoiceCard(new RouteRewardChoice("partner_preview_duplicate_supply", "Share Night Fish", "Share Night Fish", RouteRewardChoiceType.GainFishTreats, fishTreatsGained: 2)),
                P0VisualAssetCatalog.PartnerDuplicateSupplyCardId);
            Assert.IsFalse(P0VisualAssetCatalog.GetPartnerChoiceCard(new RouteRewardChoice("shop_bed_patch", "Bed Patch", "Bed Patch", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 3)).HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetPartnerChoiceCard(null).HasAsset);
        }

        [Test]
        public void GetAuthorityBlessingChoiceCards_ResolveGainAndUpgradeCards()
        {
            var blessings = P0BlessingCatalog.CreateAuthorityBlessings();

            AssertAuthorityBlessingChoiceCard(
                P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(new RouteRewardChoice("blessing_authority_oath_bedline", "誓约床线", "誓约床线", RouteRewardChoiceType.GainAuthorityBlessing, authorityBlessing: blessings[0])),
                P0VisualAssetCatalog.AuthorityOathBedlineCardId,
                P0VisualAssetCatalog.AuthorityOathBedlineSealId);
            AssertAuthorityBlessingChoiceCard(
                P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(new RouteRewardChoice("blessing_authority_dominion_sandglass", "Moon-Sand Dominion", "Moon-Sand Dominion", RouteRewardChoiceType.GainAuthorityBlessing, authorityBlessing: blessings[1])),
                P0VisualAssetCatalog.AuthorityDominionSandglassCardId,
                P0VisualAssetCatalog.AuthorityDominionSandglassSealId);
            AssertAuthorityBlessingChoiceCard(
                P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(new RouteRewardChoice("blessing_authority_rhythm_lullaby", "Lullaby Rhythm", "Lullaby Rhythm", RouteRewardChoiceType.GainAuthorityBlessing, authorityBlessing: blessings[2])),
                P0VisualAssetCatalog.AuthorityRhythmLullabyCardId,
                P0VisualAssetCatalog.AuthorityRhythmLullabySealId);
            AssertAuthorityBlessingChoiceCard(
                P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(new RouteRewardChoice("blessing_upgrade_authority_oath_bedline", "Deepen Oath", "Deepen Oath", RouteRewardChoiceType.UpgradeAuthorityBlessing, authorityBlessingUpgradeId: P0BlessingCatalog.SaibanBedlineId)),
                P0VisualAssetCatalog.AuthorityOathBedlineCardId,
                P0VisualAssetCatalog.AuthorityOathBedlineSealId);
            Assert.IsFalse(P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(new RouteRewardChoice("shop_bed_patch", "Bed Patch", "Bed Patch", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 3)).HasAsset);
            Assert.IsFalse(P0VisualAssetCatalog.GetAuthorityBlessingChoiceCard(null).HasAsset);
        }

        [Test]
        public void GetRouteChoiceCard_ResolvesShopDreamEventRestNestPartnerAndBlessingChoiceCards()
        {
            AuthorityBlessingDefinition oath = P0BlessingCatalog.CreateAuthorityBlessings()[0];

            Assert.AreEqual(
                P0VisualAssetCatalog.ShopBedPatchCardId,
                P0VisualAssetCatalog.GetRouteChoiceCard(new RouteRewardChoice("shop_bed_patch", "Bed Patch", "Bed Patch", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 3)).AssetId);
            Assert.AreEqual(
                P0VisualAssetCatalog.DreamEventCatnipResidueCardId,
                P0VisualAssetCatalog.GetRouteChoiceCard(new RouteRewardChoice("dream_event_catnip_residue", "Breathe Catnip Residue", "Breathe Catnip Residue", RouteRewardChoiceType.DreamEventModifier, nextBattleSkillDamagePercent: 20, nextBattlePoopGrowthPercent: 50)).AssetId);
            Assert.AreEqual(
                P0VisualAssetCatalog.RestNestRecoveryCardId,
                P0VisualAssetCatalog.GetRouteChoiceCard(new RouteRewardChoice("rest_nest_recovery", "Repair the Bedline", "Repair the Bedline", RouteRewardChoiceType.RestSupply, ownerSleepRestored: 25, poopReduced: 35, hungerSafeLine: 75, catHpSafePercent: 60)).AssetId);
            Assert.AreEqual(
                P0VisualAssetCatalog.PartnerShadowmaruPreviewCardId,
                P0VisualAssetCatalog.GetRouteChoiceCard(new RouteRewardChoice("partner_shadowmaru_preview", "Invite Shadowmaru", "Invite Shadowmaru", RouteRewardChoiceType.RecruitPartner, fishTreatsGained: 1, partnerCatId: P0RouteRewardResolver.PreviewPartnerId)).AssetId);
            Assert.AreEqual(
                P0VisualAssetCatalog.AuthorityOathBedlineCardId,
                P0VisualAssetCatalog.GetRouteChoiceCard(new RouteRewardChoice("authority", "Authority", "Authority", RouteRewardChoiceType.GainAuthorityBlessing, authorityBlessing: oath)).AssetId);
        }

        [Test]
        public void GetBedroomDreamBattleBackground_ResolvesSourceLockedBackground()
        {
            P0VisualAssetReference asset = P0VisualAssetCatalog.GetBedroomDreamBattleBackground();

            Assert.AreEqual(P0VisualAssetCatalog.BedroomDreamBattleBackgroundId, asset.AssetId);
            Assert.AreEqual("background", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith("thecat_bg_bedroomdream_battle_1920x1080_v001.png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.SourceLockIds, "bedroom_map_concept"));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, "thecat_style_bedroomdream_anchor_1920x1080_v001"));
        }

        [Test]
        public void GetUiShellAssets_ResolveMenuAndSettlementAssets()
        {
            Assert.AreEqual(P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId, P0VisualAssetCatalog.GetMainMenuBackground().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TitleLogoId, P0VisualAssetCatalog.GetTitleLogo().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamGlassPanelId, P0VisualAssetCatalog.GetDreamGlassPanel().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.PrimaryButtonId, P0VisualAssetCatalog.GetPrimaryButton().AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, P0VisualAssetCatalog.GetRewardIcon("fish_treats").AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamShardRewardIconId, P0VisualAssetCatalog.GetRewardIcon("dream_shards").AssetId);
            Assert.IsFalse(P0VisualAssetCatalog.GetRewardIcon("missing").HasAsset);
        }

        [Test]
        public void GetRouteChoiceIcons_ResolveChoiceTypeAssets()
        {
            Assert.AreEqual(P0VisualAssetCatalog.DreamShardRewardIconId, P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.GainDreamShards).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.GainFishTreats).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.PurchaseFishTreats).AssetId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.RecruitPartner), P0VisualAssetCatalog.PartnerRecruitChoiceIconId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.PurchaseSupply), P0VisualAssetCatalog.PurchaseSupplyChoiceIconId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.GainAuthorityBlessing), P0VisualAssetCatalog.AuthorityBlessingChoiceIconId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.UpgradeAuthorityBlessing), P0VisualAssetCatalog.AuthorityUpgradeChoiceIconId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.RestSupply), P0VisualAssetCatalog.RestSupplyChoiceIconId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.DreamEventModifier), P0VisualAssetCatalog.DreamEventModifierChoiceIconId);
            AssertRouteChoiceIcon(P0VisualAssetCatalog.GetRouteChoiceIcon(RouteRewardChoiceType.GainEventItem), P0VisualAssetCatalog.DreamEventModifierChoiceIconId);
        }

        [Test]
        public void GetAuthorityBlessingSeals_ResolveP0AuthorityAssets()
        {
            AssertAuthorityBlessingSeal(
                P0VisualAssetCatalog.GetAuthorityBlessingSeal(P0BlessingCatalog.SaibanBedlineId),
                P0VisualAssetCatalog.AuthorityOathBedlineSealId);
            AssertAuthorityBlessingSeal(
                P0VisualAssetCatalog.GetAuthorityBlessingSeal(P0BlessingCatalog.NephthysSandglassId),
                P0VisualAssetCatalog.AuthorityDominionSandglassSealId);
            AssertAuthorityBlessingSeal(
                P0VisualAssetCatalog.GetAuthorityBlessingSeal(P0BlessingCatalog.SuzuneLullabyId),
                P0VisualAssetCatalog.AuthorityRhythmLullabySealId);
            Assert.IsFalse(P0VisualAssetCatalog.GetAuthorityBlessingSeal("missing").HasAsset);
        }

        [Test]
        public void GetRouteChoiceIcon_UsesSpecificAuthorityBlessingSealAssets()
        {
            AuthorityBlessingDefinition oath = P0BlessingCatalog.CreateAuthorityBlessings()[0];

            P0VisualAssetReference gain = P0VisualAssetCatalog.GetRouteChoiceIcon(new RouteRewardChoice(
                "gain_oath",
                "Gain Oath",
                "Gain Oath",
                RouteRewardChoiceType.GainAuthorityBlessing,
                authorityBlessing: oath));

            P0VisualAssetReference upgrade = P0VisualAssetCatalog.GetRouteChoiceIcon(new RouteRewardChoice(
                "upgrade_oath",
                "Upgrade Oath",
                "Upgrade Oath",
                RouteRewardChoiceType.UpgradeAuthorityBlessing,
                authorityBlessingUpgradeId: P0BlessingCatalog.SaibanBedlineId));

            Assert.AreEqual(P0VisualAssetCatalog.AuthorityOathBedlineSealId, gain.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.AuthorityOathBedlineSealId, upgrade.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.FishTreatRewardIconId, P0VisualAssetCatalog.GetRouteChoiceIcon(new RouteRewardChoice("fish", "Fish", "Fish", RouteRewardChoiceType.GainFishTreats, fishTreatsGained: 1)).AssetId);
        }

        [Test]
        public void GetRouteRewardCardFrames_ResolveNonBattleNodeFrames()
        {
            AssertRouteCardFrame(P0VisualAssetCatalog.GetRouteRewardCardFrame(RouteNodeType.Partner), P0VisualAssetCatalog.PartnerRouteCardFrameId);
            AssertRouteCardFrame(P0VisualAssetCatalog.GetRouteRewardCardFrame(RouteNodeType.Shop), P0VisualAssetCatalog.ShopRouteCardFrameId);
            AssertRouteCardFrame(P0VisualAssetCatalog.GetRouteRewardCardFrame(RouteNodeType.BlessingOffering), P0VisualAssetCatalog.BlessingRouteCardFrameId);
            AssertRouteCardFrame(P0VisualAssetCatalog.GetRouteRewardCardFrame(RouteNodeType.DreamEvent), P0VisualAssetCatalog.DreamEventRouteCardFrameId);
            AssertRouteCardFrame(P0VisualAssetCatalog.GetRouteRewardCardFrame(RouteNodeType.RestNest), P0VisualAssetCatalog.RestNestRouteCardFrameId);
            Assert.AreEqual(P0VisualAssetCatalog.DreamGlassPanelId, P0VisualAssetCatalog.GetRouteRewardCardFrame(RouteNodeType.Defense).AssetId);
        }

        [Test]
        public void GetRouteRewardDetailBadges_ResolveChoiceEffectBadges()
        {
            AssertRouteRewardDetailBadge(
                P0VisualAssetCatalog.GetRouteRewardDetailBadge(new RouteRewardChoice("gain", "gain", "gain", RouteRewardChoiceType.GainFishTreats, fishTreatsGained: 1)),
                P0VisualAssetCatalog.RouteRewardGainBadgeId);
            AssertRouteRewardDetailBadge(
                P0VisualAssetCatalog.GetRouteRewardDetailBadge(new RouteRewardChoice("cost", "cost", "cost", RouteRewardChoiceType.PurchaseSupply, fishTreatsCost: 1)),
                P0VisualAssetCatalog.RouteRewardCostBadgeId);
            AssertRouteRewardDetailBadge(
                P0VisualAssetCatalog.GetRouteRewardDetailBadge(new RouteRewardChoice("recovery", "recovery", "recovery", RouteRewardChoiceType.RestSupply, ownerSleepRestored: 4)),
                P0VisualAssetCatalog.RouteRewardRecoveryBadgeId);
            AssertRouteRewardDetailBadge(
                P0VisualAssetCatalog.GetRouteRewardDetailBadge(new RouteRewardChoice("risk", "risk", "risk", RouteRewardChoiceType.DreamEventModifier, ownerSleepDamaged: 2)),
                P0VisualAssetCatalog.RouteRewardRiskBadgeId);
            AssertRouteRewardDetailBadge(
                P0VisualAssetCatalog.GetRouteRewardDetailBadge(new RouteRewardChoice("upgrade", "upgrade", "upgrade", RouteRewardChoiceType.UpgradeAuthorityBlessing, authorityBlessingUpgradeId: "blessing")),
                P0VisualAssetCatalog.RouteRewardUpgradeBadgeId);
            Assert.IsFalse(P0VisualAssetCatalog.GetRouteRewardDetailBadge(null).HasAsset);
        }

        [Test]
        public void GetCallTyrantWarningVfx_ResolvesGeneratedManifestAsset()
        {
            P0VisualAssetReference asset = P0VisualAssetCatalog.GetCallTyrantWarningVfx();

            Assert.AreEqual(P0VisualAssetCatalog.CallTyrantWarningVfxId, asset.AssetId);
            Assert.AreEqual("vfx", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith("thecat_vfx_calltyrant_warning_512_v001.png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.SourceLockIds, "call_tyrant_animation"));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.CallTyrantConceptId));
        }

        [Test]
        public void GetBattleFeedbackVfx_ResolveGeneratedManifestAssets()
        {
            AssertBattleFeedbackVfx(P0VisualAssetCatalog.GetHitSparkVfx(), P0VisualAssetCatalog.HitSparkVfxId, "thecat_style_status_icons_5x64_v001");
            AssertBattleFeedbackVfx(P0VisualAssetCatalog.GetBedShieldPulseVfx(), P0VisualAssetCatalog.BedShieldPulseVfxId, "thecat_style_status_icons_5x64_v001");
            AssertBattleFeedbackVfx(P0VisualAssetCatalog.GetSleepStableWaveVfx(), P0VisualAssetCatalog.SleepStableWaveVfxId, "thecat_style_status_icons_5x64_v001");
            AssertBattleFeedbackVfx(P0VisualAssetCatalog.GetLitterCleanseVfx(), P0VisualAssetCatalog.LitterCleanseVfxId, "thecat_style_bedroomdream_anchor_1920x1080_v001");
            AssertBattleFeedbackVfx(P0VisualAssetCatalog.GetFeederKibbleVfx(), P0VisualAssetCatalog.FeederKibbleVfxId, "thecat_style_bedroomdream_anchor_1920x1080_v001");
            AssertBattleFeedbackVfx(P0VisualAssetCatalog.GetEnemyMarkRingVfx(), P0VisualAssetCatalog.EnemyMarkRingVfxId, "thecat_style_status_icons_5x64_v001");
        }

        [Test]
        public void GetStarterSkillVfx_ResolveBatch61InstalledAssets()
        {
            AssertStarterSkillVfx(P0VisualAssetCatalog.GetStarterSkillVfx("saiban_oath_shield"), P0VisualAssetCatalog.SaibanBedlineSkillVfxId, "saiban_turnaround_colored");
            AssertStarterSkillVfx(P0VisualAssetCatalog.GetStarterSkillVfx("saiban_sword_sweep"), P0VisualAssetCatalog.SaibanBedlineSkillVfxId, "saiban_turnaround_colored");
            AssertStarterSkillVfx(P0VisualAssetCatalog.GetStarterSkillVfx("nephthys_moon_sand_obelisk"), P0VisualAssetCatalog.NephthysMoonsandSkillVfxId, "nephthys_turnaround_colored");
            AssertStarterSkillVfx(P0VisualAssetCatalog.GetStarterSkillVfx("nephthys_royal_mark"), P0VisualAssetCatalog.NephthysMoonsandSkillVfxId, "nephthys_turnaround_colored");
            AssertStarterSkillVfx(P0VisualAssetCatalog.GetStarterSkillVfx("suzune_sleep_bell"), P0VisualAssetCatalog.SuzuneLullabySkillVfxId, "suzune_turnaround_colored");
            AssertStarterSkillVfx(P0VisualAssetCatalog.GetStarterSkillVfx("suzune_moon_torii"), P0VisualAssetCatalog.SuzuneLullabySkillVfxId, "suzune_turnaround_colored");
            Assert.IsFalse(P0VisualAssetCatalog.GetStarterSkillVfx("missing_skill").HasAsset);
        }

        [Test]
        public void GetEnemyWarningVfx_ResolveSourceLockedManifestAssets()
        {
            AssertEnemyWarningVfx(P0VisualAssetCatalog.GetBlackMudBedClawWarningVfx(), P0VisualAssetCatalog.BlackMudBedClawWarningVfxId, P0VisualAssetCatalog.BlackMudCombatSpriteId, "black_mud_animation");
            AssertEnemyWarningVfx(P0VisualAssetCatalog.GetColdLightBeamWarningVfx(), P0VisualAssetCatalog.ColdLightBeamWarningVfxId, P0VisualAssetCatalog.ColdLightCombatSpriteId, "cold_light_animation");
            AssertEnemyWarningVfx(P0VisualAssetCatalog.GetCallTyrantAppThrowVfx(), P0VisualAssetCatalog.CallTyrantAppThrowVfxId, P0VisualAssetCatalog.CallTyrantConceptId, "call_tyrant_animation");
            AssertEnemyWarningVfx(P0VisualAssetCatalog.GetCallTyrantSummonPortalVfx(), P0VisualAssetCatalog.CallTyrantSummonPortalVfxId, P0VisualAssetCatalog.CallTyrantConceptId, "call_tyrant_animation");
        }

        [Test]
        public void GetEnemyAnimationFramesheets_ResolveSourceLockedManifestAssets()
        {
            AssertEnemyFramesheet(P0VisualAssetCatalog.GetEnemyAnimationFramesheet(P0PrototypeCatalog.BlackMudNightmareId), P0VisualAssetCatalog.BlackMudMoveFramesheetId, P0VisualAssetCatalog.BlackMudCombatSpriteId, "black_mud_animation");
            AssertEnemyFramesheet(P0VisualAssetCatalog.GetEnemyAnimationFramesheet(P0PrototypeCatalog.ColdLightShadowId), P0VisualAssetCatalog.ColdLightCastFramesheetId, P0VisualAssetCatalog.ColdLightCombatSpriteId, "cold_light_animation");
            AssertEnemyFramesheet(P0VisualAssetCatalog.GetEnemyAnimationFramesheet(P0PrototypeCatalog.CallTyrantId), P0VisualAssetCatalog.CallTyrantBossPatternFramesheetId, P0VisualAssetCatalog.CallTyrantConceptId, "call_tyrant_animation");
            Assert.IsFalse(P0VisualAssetCatalog.GetEnemyAnimationFramesheet("missing_enemy").HasAsset);
        }

        [Test]
        public void GetOutcomeBanners_ResolveBattleAndSettlementAssets()
        {
            Assert.AreEqual(P0VisualAssetCatalog.BattleResultVictoryBannerId, P0VisualAssetCatalog.GetBattleResultOutcomeBanner(BattleOutcome.Victory).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.BattleResultDefeatBannerId, P0VisualAssetCatalog.GetBattleResultOutcomeBanner(BattleOutcome.Defeat).AssetId);
            Assert.IsFalse(P0VisualAssetCatalog.GetBattleResultOutcomeBanner(BattleOutcome.InProgress).HasAsset);
            Assert.AreEqual(P0VisualAssetCatalog.SettlementRunClearedBannerId, P0VisualAssetCatalog.GetSettlementOutcomeBanner(true).AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SettlementRunFailedBannerId, P0VisualAssetCatalog.GetSettlementOutcomeBanner(false).AssetId);
        }


        [Test]
        public void CreateP0RuntimeBindings_CoversStarterCatsWorldPropsCoreHudAndBossSurfaces()
        {
            P0VisualAssetBinding[] bindings = P0VisualAssetCatalog.CreateP0RuntimeBindings();

            Assert.AreEqual(P0VisualAssetCatalog.P0RuntimeVisualBindingCount, bindings.Length);
            AssertBinding(bindings, "background.bedroom_dream", P0VisualAssetCatalog.BedroomDreamBattleBackgroundId, "battle_world", "background", "bedroom_map_concept");
            AssertBinding(bindings, "cat.combat.saiban", P0VisualAssetCatalog.SaibanCombatSpriteId, "cat_hud", "combat_sprite", "turnaround");
            AssertBinding(bindings, "cat.combat.nephthys", P0VisualAssetCatalog.NephthysCombatSpriteId, "cat_hud", "combat_sprite", "turnaround");
            AssertBinding(bindings, "cat.combat.suzune", P0VisualAssetCatalog.SuzuneCombatSpriteId, "cat_hud", "combat_sprite", "turnaround");
            AssertBinding(bindings, "cat.avatar.saiban", P0VisualAssetCatalog.SaibanHudAvatarId, "cat_hud", "avatar_icon", "Batch 58");
            AssertBinding(bindings, "cat.avatar.nephthys", P0VisualAssetCatalog.NephthysHudAvatarId, "cat_hud", "avatar_icon", "Batch 58");
            AssertBinding(bindings, "cat.avatar.suzune", P0VisualAssetCatalog.SuzuneHudAvatarId, "cat_hud", "avatar_icon", "Batch 58");
            AssertBinding(bindings, "enemy.combat.black_mud", P0VisualAssetCatalog.BlackMudCombatSpriteId, "battle_world", "combat_sprite", "black_mud_concept");
            AssertBinding(bindings, "enemy.combat.cold_light", P0VisualAssetCatalog.ColdLightCombatSpriteId, "battle_world", "combat_sprite", "cold_light_concept");
            AssertBinding(bindings, "enemy.combat.call_tyrant", P0VisualAssetCatalog.CallTyrantConceptId, "battle_world", "combat_visual", "call_tyrant_concept");
            AssertBinding(bindings, "enemy.anim.black_mud_move", P0VisualAssetCatalog.BlackMudMoveFramesheetId, "battle_world", "animation_framesheet", "black_mud_animation");
            AssertBinding(bindings, "enemy.anim.cold_light_cast", P0VisualAssetCatalog.ColdLightCastFramesheetId, "battle_world", "animation_framesheet", "cold_light_animation");
            AssertBinding(bindings, "enemy.anim.call_tyrant_boss_pattern", P0VisualAssetCatalog.CallTyrantBossPatternFramesheetId, "battle_world", "animation_framesheet", "call_tyrant_animation");
            AssertBinding(bindings, "prop.bed", P0VisualAssetCatalog.BedSpriteId, "battle_world", "interactable_sprite", "bedroom_map_concept");
            AssertBinding(bindings, "prop.litter_box", P0VisualAssetCatalog.LitterBoxSpriteId, "battle_world", "interactable_sprite", "bedroom_mid_background_sprites");
            AssertBinding(bindings, "prop.feeder", P0VisualAssetCatalog.FeederSpriteId, "battle_world", "interactable_sprite", "bedroom_mid_background_sprites");
            AssertBinding(bindings, "core.owner_sleep", P0VisualAssetCatalog.OwnerSleepIconId, "battle_hud", "core_icon", "status icon");
            AssertBinding(bindings, "core.cat_hp", P0VisualAssetCatalog.CatHpIconId, "cat_hud", "core_icon", "status icon");
            AssertBinding(bindings, "skill_hud.ready_frame", P0VisualAssetCatalog.SkillReadyFrameId, "skill_hud", "ready_frame", "Batch 60");
            AssertBinding(bindings, "skill_hud.cooldown_overlay", P0VisualAssetCatalog.SkillCooldownOverlayId, "skill_hud", "cooldown_overlay", "Batch 60");
            AssertBinding(bindings, "skill_hud.no_target_marker", P0VisualAssetCatalog.SkillNoTargetMarkerId, "skill_hud", "no_target_marker", "Batch 60");
            AssertBinding(bindings, "skill_hud.hunger_cost_chip", P0VisualAssetCatalog.SkillHungerCostChipId, "skill_hud", "hunger_cost_chip", "Batch 60");
            AssertBinding(bindings, "skill_hud.auto_target_reticle", P0VisualAssetCatalog.AutoTargetReticleId, "skill_hud", "auto_target_reticle", "Batch 60");
            AssertBinding(bindings, "battle_hud.interaction_range_ripple", P0VisualAssetCatalog.InteractionRangeRippleId, "battle_hud", "interaction_range_ripple", "Batch 60");
            AssertBinding(bindings, "skill_vfx.saiban_bedline", P0VisualAssetCatalog.SaibanBedlineSkillVfxId, "battle_feedback", "starter_skill_vfx", "Batch 61");
            AssertBinding(bindings, "skill_vfx.nephthys_moonsand", P0VisualAssetCatalog.NephthysMoonsandSkillVfxId, "battle_feedback", "starter_skill_vfx", "Batch 61");
            AssertBinding(bindings, "skill_vfx.suzune_lullaby", P0VisualAssetCatalog.SuzuneLullabySkillVfxId, "battle_feedback", "starter_skill_vfx", "Batch 61");
            AssertBinding(bindings, "core_gauge.owner_sleep.frame", P0VisualAssetCatalog.OwnerSleepGaugeFrameId, "battle_hud", "gauge_frame", "batch 27");
            AssertBinding(bindings, "core_gauge.owner_sleep.fill", P0VisualAssetCatalog.OwnerSleepGaugeFillId, "battle_hud", "gauge_fill", "batch 27");
            AssertBinding(bindings, "core_gauge.cat_hp.frame", P0VisualAssetCatalog.CatHpGaugeFrameId, "cat_hud", "gauge_frame", "batch 27");
            AssertBinding(bindings, "core_gauge.cat_hp.fill", P0VisualAssetCatalog.CatHpGaugeFillId, "cat_hud", "gauge_fill", "batch 27");
            AssertBinding(bindings, "core_gauge.team_poop.frame", P0VisualAssetCatalog.TeamPoopGaugeFrameId, "battle_hud", "gauge_frame", "batch 27");
            AssertBinding(bindings, "core_gauge.team_poop.fill", P0VisualAssetCatalog.TeamPoopGaugeFillId, "battle_hud", "gauge_fill", "batch 27");
            AssertBinding(bindings, "core_gauge.team_hunger.frame", P0VisualAssetCatalog.TeamHungerGaugeFrameId, "battle_hud", "gauge_frame", "batch 27");
            AssertBinding(bindings, "core_gauge.team_hunger.fill", P0VisualAssetCatalog.TeamHungerGaugeFillId, "battle_hud", "gauge_fill", "batch 27");
            AssertBinding(bindings, "status.sleep_stable", P0VisualAssetCatalog.SleepStableStatusIconId, "status_hud", "status_icon", "cell 0");
            AssertBinding(bindings, "status.slow", P0VisualAssetCatalog.SlowStatusIconId, "status_hud", "status_icon", "cell 1");
            AssertBinding(bindings, "status.knockback", P0VisualAssetCatalog.KnockbackStatusIconId, "status_hud", "status_icon", "cell 2");
            AssertBinding(bindings, "status.mark", P0VisualAssetCatalog.MarkStatusIconId, "status_hud", "status_icon", "cell 3");
            AssertBinding(bindings, "status.shield", P0VisualAssetCatalog.ShieldStatusIconId, "status_hud", "status_icon", "cell 4");
            AssertBinding(bindings, "status_compact.sleep_stable", P0VisualAssetCatalog.SleepStableStatusCompactIconId, "status_hud", "compact_status_icon", "batch 14");
            AssertBinding(bindings, "status_compact.slow", P0VisualAssetCatalog.SlowStatusCompactIconId, "status_hud", "compact_status_icon", "batch 14");
            AssertBinding(bindings, "status_compact.knockback", P0VisualAssetCatalog.KnockbackStatusCompactIconId, "status_hud", "compact_status_icon", "batch 14");
            AssertBinding(bindings, "status_compact.mark", P0VisualAssetCatalog.MarkStatusCompactIconId, "status_hud", "compact_status_icon", "batch 14");
            AssertBinding(bindings, "status_compact.shield", P0VisualAssetCatalog.ShieldStatusCompactIconId, "status_hud", "compact_status_icon", "batch 14");
            AssertBinding(bindings, "route.defense_node", P0VisualAssetCatalog.DefenseRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.elite_node", P0VisualAssetCatalog.EliteRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.partner_node", P0VisualAssetCatalog.PartnerRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.shop_node", P0VisualAssetCatalog.ShopRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.dream_event_node", P0VisualAssetCatalog.DreamEventRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.blessing_node", P0VisualAssetCatalog.BlessingRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.rest_nest_node", P0VisualAssetCatalog.RestNestRouteNodeIconId, "route_map", "node_icon", "batch 06");
            AssertBinding(bindings, "route.boss_node", P0VisualAssetCatalog.BossRouteNodeIconId, "route_map", "node_icon", "call_tyrant_concept");
            AssertBinding(bindings, "route_summary.shop", P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId, "route_node_summary", "summary_banner", "batch 19");
            AssertBinding(bindings, "route_summary.dream_event", P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId, "route_node_summary", "summary_banner", "batch 19");
            AssertBinding(bindings, "route_summary.rest_nest", P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId, "route_node_summary", "summary_banner", "batch 19");
            AssertBinding(bindings, "shop_item.bed_patch", P0VisualAssetCatalog.ShopBedPatchCardId, "shop_item_card", "item_card", "batch 20");
            AssertBinding(bindings, "shop_item.litter_sachet", P0VisualAssetCatalog.ShopLitterSachetCardId, "shop_item_card", "item_card", "batch 20");
            AssertBinding(bindings, "shop_item.late_kibble", P0VisualAssetCatalog.ShopLateKibbleCardId, "shop_item_card", "item_card", "batch 20");
            AssertBinding(bindings, "shop_item.free_sample", P0VisualAssetCatalog.ShopFreeSampleCardId, "shop_item_card", "item_card", "batch 20");
            AssertBinding(bindings, "dream_event_choice.clear_notifications", P0VisualAssetCatalog.DreamEventClearNotificationsCardId, "dream_event_choice_card", "choice_card", "batch 21");
            AssertBinding(bindings, "dream_event_choice.catnip_residue", P0VisualAssetCatalog.DreamEventCatnipResidueCardId, "dream_event_choice_card", "choice_card", "batch 21");
            AssertBinding(bindings, "dream_event_choice.mark_all_read", P0VisualAssetCatalog.DreamEventMarkAllReadCardId, "dream_event_choice_card", "choice_card", "batch 21");
            AssertBinding(bindings, "rest_nest_choice.recovery", P0VisualAssetCatalog.RestNestRecoveryCardId, "rest_nest_choice_card", "choice_card", "batch 22");
            AssertBinding(bindings, "partner_choice.shadowmaru_preview", P0VisualAssetCatalog.PartnerShadowmaruPreviewCardId, "partner_choice_card", "choice_card", "batch 23");
            AssertBinding(bindings, "partner_choice.duplicate_supply", P0VisualAssetCatalog.PartnerDuplicateSupplyCardId, "partner_choice_card", "choice_card", "batch 23");
            AssertBinding(bindings, "blessing_choice.oath_bedline", P0VisualAssetCatalog.AuthorityOathBedlineCardId, "blessing_choice_card", "choice_card", "batch 24");
            AssertBinding(bindings, "blessing_choice.dominion_sandglass", P0VisualAssetCatalog.AuthorityDominionSandglassCardId, "blessing_choice_card", "choice_card", "batch 24");
            AssertBinding(bindings, "blessing_choice.rhythm_lullaby", P0VisualAssetCatalog.AuthorityRhythmLullabyCardId, "blessing_choice_card", "choice_card", "batch 24");
            AssertBinding(bindings, "warning.call_tyrant", P0VisualAssetCatalog.CallTyrantWarningVfxId, "battle_warning", "warning_vfx", "call_tyrant_animation");
            AssertBinding(bindings, "warning.black_mud_bed_claw", P0VisualAssetCatalog.BlackMudBedClawWarningVfxId, "battle_warning", "warning_vfx", "black_mud_animation");
            AssertBinding(bindings, "warning.cold_light_beam", P0VisualAssetCatalog.ColdLightBeamWarningVfxId, "battle_warning", "warning_vfx", "cold_light_animation");
            AssertBinding(bindings, "warning.call_tyrant_app_throw", P0VisualAssetCatalog.CallTyrantAppThrowVfxId, "battle_warning", "warning_vfx", "call_tyrant_animation");
            AssertBinding(bindings, "warning.call_tyrant_summon_portal", P0VisualAssetCatalog.CallTyrantSummonPortalVfxId, "battle_warning", "warning_vfx", "call_tyrant_animation");
            AssertBinding(bindings, "feedback.hit_spark", P0VisualAssetCatalog.HitSparkVfxId, "battle_feedback", "feedback_vfx", "batch 09");
            AssertBinding(bindings, "feedback.bed_shield_pulse", P0VisualAssetCatalog.BedShieldPulseVfxId, "battle_feedback", "feedback_vfx", "batch 09");
            AssertBinding(bindings, "feedback.sleep_stable_wave", P0VisualAssetCatalog.SleepStableWaveVfxId, "battle_feedback", "feedback_vfx", "batch 09");
            AssertBinding(bindings, "feedback.litter_cleanse", P0VisualAssetCatalog.LitterCleanseVfxId, "battle_feedback", "feedback_vfx", "batch 09");
            AssertBinding(bindings, "feedback.feeder_kibble", P0VisualAssetCatalog.FeederKibbleVfxId, "battle_feedback", "feedback_vfx", "batch 09");
            AssertBinding(bindings, "feedback.enemy_mark_ring", P0VisualAssetCatalog.EnemyMarkRingVfxId, "battle_feedback", "feedback_vfx", "batch 09");
            AssertBinding(bindings, "main_menu.background", P0VisualAssetCatalog.MainMenuDreamEntryBackgroundId, "main_menu", "background", "batch 08");
            AssertBinding(bindings, "main_menu.title_logo", P0VisualAssetCatalog.TitleLogoId, "main_menu", "logo", "batch 08");
            AssertBinding(bindings, "ui.panel.dreamglass", P0VisualAssetCatalog.DreamGlassPanelId, "ui_shell", "panel_frame", "batch 08");
            AssertBinding(bindings, "ui.button.primary", P0VisualAssetCatalog.PrimaryButtonId, "ui_shell", "button_frame", "batch 08");
            AssertBinding(bindings, "settlement.reward.fish_treat", P0VisualAssetCatalog.FishTreatRewardIconId, "settlement", "reward_icon", "batch 08");
            AssertBinding(bindings, "settlement.reward.dream_shard", P0VisualAssetCatalog.DreamShardRewardIconId, "settlement", "reward_icon", "batch 08");
            AssertBinding(bindings, "battle_result.victory_banner", P0VisualAssetCatalog.BattleResultVictoryBannerId, "battle_result", "outcome_banner", "batch 25");
            AssertBinding(bindings, "battle_result.defeat_banner", P0VisualAssetCatalog.BattleResultDefeatBannerId, "battle_result", "outcome_banner", "batch 25");
            AssertBinding(bindings, "settlement.run_cleared_banner", P0VisualAssetCatalog.SettlementRunClearedBannerId, "settlement", "outcome_banner", "batch 25");
            AssertBinding(bindings, "settlement.run_failed_banner", P0VisualAssetCatalog.SettlementRunFailedBannerId, "settlement", "outcome_banner", "batch 25");
            AssertBinding(bindings, "route_choice.partner_recruit", P0VisualAssetCatalog.PartnerRecruitChoiceIconId, "route_choice", "choice_icon", "batch 12");
            AssertBinding(bindings, "route_choice.purchase_supply", P0VisualAssetCatalog.PurchaseSupplyChoiceIconId, "route_choice", "choice_icon", "batch 12");
            AssertBinding(bindings, "route_choice.authority_blessing", P0VisualAssetCatalog.AuthorityBlessingChoiceIconId, "route_choice", "choice_icon", "batch 12");
            AssertBinding(bindings, "route_choice.authority_upgrade", P0VisualAssetCatalog.AuthorityUpgradeChoiceIconId, "route_choice", "choice_icon", "batch 12");
            AssertBinding(bindings, "route_choice.rest_supply", P0VisualAssetCatalog.RestSupplyChoiceIconId, "route_choice", "choice_icon", "batch 12");
            AssertBinding(bindings, "route_choice.dream_event_modifier", P0VisualAssetCatalog.DreamEventModifierChoiceIconId, "route_choice", "choice_icon", "batch 12");
            AssertBinding(bindings, "route_reward_card.partner", P0VisualAssetCatalog.PartnerRouteCardFrameId, "route_reward_card", "card_frame", "batch 13");
            AssertBinding(bindings, "route_reward_card.shop", P0VisualAssetCatalog.ShopRouteCardFrameId, "route_reward_card", "card_frame", "batch 13");
            AssertBinding(bindings, "route_reward_card.blessing", P0VisualAssetCatalog.BlessingRouteCardFrameId, "route_reward_card", "card_frame", "batch 13");
            AssertBinding(bindings, "route_reward_card.dream_event", P0VisualAssetCatalog.DreamEventRouteCardFrameId, "route_reward_card", "card_frame", "batch 13");
            AssertBinding(bindings, "route_reward_card.rest_nest", P0VisualAssetCatalog.RestNestRouteCardFrameId, "route_reward_card", "card_frame", "batch 13");
            AssertBinding(bindings, "route_reward_detail.gain", P0VisualAssetCatalog.RouteRewardGainBadgeId, "route_reward_detail", "detail_badge", "batch 15");
            AssertBinding(bindings, "route_reward_detail.cost", P0VisualAssetCatalog.RouteRewardCostBadgeId, "route_reward_detail", "detail_badge", "batch 15");
            AssertBinding(bindings, "route_reward_detail.recovery", P0VisualAssetCatalog.RouteRewardRecoveryBadgeId, "route_reward_detail", "detail_badge", "batch 15");
            AssertBinding(bindings, "route_reward_detail.risk", P0VisualAssetCatalog.RouteRewardRiskBadgeId, "route_reward_detail", "detail_badge", "batch 15");
            AssertBinding(bindings, "route_reward_detail.upgrade", P0VisualAssetCatalog.RouteRewardUpgradeBadgeId, "route_reward_detail", "detail_badge", "batch 15");
            AssertBinding(bindings, "blessing_detail.oath_bedline", P0VisualAssetCatalog.AuthorityOathBedlineSealId, "blessing_detail", "blessing_seal", "batch 16");
            AssertBinding(bindings, "blessing_detail.dominion_sandglass", P0VisualAssetCatalog.AuthorityDominionSandglassSealId, "blessing_detail", "blessing_seal", "batch 16");
            AssertBinding(bindings, "blessing_detail.rhythm_lullaby", P0VisualAssetCatalog.AuthorityRhythmLullabySealId, "blessing_detail", "blessing_seal", "batch 16");
        }

        [Test]
        public void Find_UnknownAssetReturnsEmptyReference()
        {
            P0VisualAssetReference asset = P0VisualAssetCatalog.Find("missing_asset");

            Assert.IsFalse(asset.HasAsset);
            Assert.AreEqual("visual asset: none", asset.BuildSummary());
        }

        private static bool Contains(System.Collections.Generic.IReadOnlyList<string> values, string expected)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == expected)
                {
                    return true;
                }
            }

            return false;
        }

        private static void AssertSkillHudFeedback(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("skill_hud_feedback", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
        }

        private static void AssertBattleFeedbackVfx(P0VisualAssetReference asset, string assetId, string referenceAssetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("vfx", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, referenceAssetId));
        }

        private static void AssertStarterSkillVfx(P0VisualAssetReference asset, string assetId, string sourceLockId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("vfx", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.SourceLockIds, sourceLockId));
            StringAssert.Contains("Assets/TheCat/Art/VFX/", asset.UnityImportPath);
        }

        private static void AssertRouteChoiceIcon(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("icon", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, "thecat_style_status_icons_5x64_v001"));
        }

        private static void AssertRouteCardFrame(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("frame", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
        }

        private static void AssertRouteRewardDetailBadge(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("badge", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
        }

        private static void AssertRouteNodeSummaryBanner(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("banner", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
        }

        private static void AssertShopItemCard(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("card", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.ShopRouteCardFrameId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.ShopRouteNodeSummaryBannerId));
        }

        private static void AssertDreamEventChoiceCard(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("card", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamEventRouteCardFrameId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamEventRouteNodeSummaryBannerId));
        }

        private static void AssertRestNestRecoveryCard(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("card", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.RestNestRouteCardFrameId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.RestNestRouteNodeSummaryBannerId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.RestSupplyChoiceIconId));
        }

        private static void AssertPartnerChoiceCard(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("card", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.PartnerRouteCardFrameId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.PartnerRouteNodeIconId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.PartnerRecruitChoiceIconId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.FishTreatRewardIconId));
        }

        private static void AssertAuthorityBlessingChoiceCard(P0VisualAssetReference asset, string assetId, string sealAssetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("card", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.DreamGlassPanelId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.BlessingRouteCardFrameId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.BlessingRouteNodeIconId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.AuthorityBlessingChoiceIconId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.AuthorityUpgradeChoiceIconId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, sealAssetId));
        }

        private static void AssertAuthorityBlessingSeal(P0VisualAssetReference asset, string assetId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("icon", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.AuthorityBlessingChoiceIconId));
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, P0VisualAssetCatalog.BlessingRouteCardFrameId));
        }

        private static void AssertEnemyWarningVfx(
            P0VisualAssetReference asset,
            string assetId,
            string referenceAssetId,
            string sourceLockId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("vfx", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, referenceAssetId));
            Assert.IsTrue(Contains(asset.SourceLockIds, sourceLockId));
        }

        private static void AssertEnemyFramesheet(
            P0VisualAssetReference asset,
            string assetId,
            string referenceAssetId,
            string sourceLockId)
        {
            Assert.AreEqual(assetId, asset.AssetId);
            Assert.AreEqual("framesheet", asset.AssetType);
            Assert.AreEqual(P0AssetManifestStatus.Generated, asset.Status);
            Assert.IsTrue(asset.RequiresWorkspaceFile);
            StringAssert.EndsWith(assetId + ".png", asset.UnityImportPath);
            Assert.IsTrue(Contains(asset.ReferenceAssetIds, referenceAssetId));
            Assert.IsTrue(Contains(asset.SourceLockIds, sourceLockId));
        }

        private static void AssertBinding(
            P0VisualAssetBinding[] bindings,
            string bindingId,
            string assetId,
            string surfaceId,
            string slotId,
            string sourceAuthorityToken)
        {
            for (int i = 0; i < bindings.Length; i++)
            {
                if (bindings[i].BindingId != bindingId)
                {
                    continue;
                }

                Assert.IsTrue(bindings[i].IsReady, bindings[i].BuildSummary());
                Assert.AreEqual(assetId, bindings[i].Asset.AssetId);
                Assert.AreEqual(surfaceId, bindings[i].SurfaceId);
                Assert.AreEqual(slotId, bindings[i].SlotId);
                StringAssert.Contains(sourceAuthorityToken, bindings[i].SourceAuthority);
                return;
            }

            Assert.Fail("Missing binding: " + bindingId);
        }
    }
}
