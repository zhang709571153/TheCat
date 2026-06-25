using NUnit.Framework;
using TheCat.Combat;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Gameplay;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0CatHudCoverageTests
    {
        [Test]
        public void BuildCard_ActiveCatIncludesRoleHpAndReadySkills()
        {
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SaibanId));

            P0CatHudCard card = P0CatHudPresenter.BuildCard(cat, true, _ => 0f);

            Assert.IsTrue(card.IsActive);
            Assert.IsFalse(card.CanSwitch);
            Assert.AreEqual("当前", card.SlotState);
            Assert.AreEqual("DEF", card.RoleToken);
            Assert.AreEqual("健康", card.HpStateToken);
            Assert.AreEqual(1f, card.HpRatio, 0.001f);
            Assert.AreEqual("技能就绪", card.CooldownLabel);
            Assert.AreEqual(P0VisualAssetCatalog.SaibanCombatSpriteId, card.CombatSprite.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SaibanHudAvatarId, card.HudAvatar.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.SaibanHudAvatarId, card.PrimaryHudIcon.AssetId);
            Assert.AreEqual("avatar_icon", card.HudAvatar.AssetType);
            Assert.AreEqual(P0VisualAssetCatalog.CatHpIconId, card.HpIcon.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.CatHpGaugeFrameId, card.HpGaugeFrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.CatHpGaugeFillId, card.HpGaugeFillAsset.AssetId);
            Assert.IsTrue(card.CombatSprite.RequiresWorkspaceFile);
            Assert.IsTrue(card.HudAvatar.RequiresWorkspaceFile);
            Assert.IsTrue(card.HpIcon.RequiresWorkspaceFile);
            Assert.IsTrue(card.HpGaugeFrameAsset.RequiresWorkspaceFile);
            Assert.IsTrue(card.HpGaugeFillAsset.RequiresWorkspaceFile);
            StringAssert.Contains("当前", card.BuildButtonLabel());
            StringAssert.Contains("生命", card.BuildSummary());
            Assert.IsFalse(card.BuildSummary().Contains("HP"));
        }

        [Test]
        public void BuildCard_WeakShieldedCatShowsStateAndShield()
        {
            CatBattleState cat = new CatBattleState(GetCat(P0PrototypeCatalog.SuzuneId), currentHp: 12f, weakRemainingSeconds: 8f);
            cat.ApplyStatus(GetStatus(StatusTagIds.Shield), 20f);

            P0CatHudCard card = P0CatHudPresenter.BuildCard(cat, false, _ => 4f);

            Assert.IsTrue(card.IsWeak);
            Assert.IsFalse(card.CanSwitch);
            Assert.IsTrue(card.HasShield);
            Assert.AreEqual(20f, card.ShieldAmount, 0.001f);
            Assert.AreEqual("虚弱", card.HpStateToken);
            Assert.AreEqual("冷却 4.0s", card.CooldownLabel);
            StringAssert.Contains("护盾", card.BuildButtonLabel());
            StringAssert.Contains("虚弱 8.0s", card.BuildSummary());
        }

        [Test]
        public void EvaluatePrototypeCards_CompletesCatHudCoverage()
        {
            P0CatHudCoverageReport report = P0CatHudCoverage.EvaluatePrototypeCards();

            Assert.IsTrue(report.IsComplete, report.BuildDetailedSummary());
            Assert.AreEqual(P0CatHudCoverage.ExpectedCoveredCardCount, report.CoveredCards.Count);
            Assert.AreEqual(0, report.FailureCount);
            StringAssert.Contains("cat HUD coverage complete", report.BuildSummary());
            StringAssert.Contains("Starter cat HUD cards", report.BuildDetailedSummary());
            StringAssert.Contains("skill count", report.BuildDetailedSummary());
        }

        private static CatDefinition GetCat(string catId)
        {
            foreach (CatDefinition cat in P0PrototypeCatalog.CreateStarterCats())
            {
                if (cat.Id == catId)
                {
                    return cat;
                }
            }

            Assert.Fail("Missing cat: " + catId);
            return null;
        }

        private static StatusTagDefinition GetStatus(string statusId)
        {
            foreach (StatusTagDefinition status in P0PrototypeCatalog.CreateStatusTags())
            {
                if (status.Id == statusId)
                {
                    return status;
                }
            }

            Assert.Fail("Missing status: " + statusId);
            return default(StatusTagDefinition);
        }
    }
}
