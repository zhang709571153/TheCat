using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class PrototypeCatalogTests
    {
        [Test]
        public void StarterCats_ContainThreeP0Roles()
        {
            var cats = P0PrototypeCatalog.CreateStarterCats();

            Assert.AreEqual(3, cats.Count);
            Assert.IsTrue(cats.Any(cat => cat.Id == P0PrototypeCatalog.SaibanId && cat.Role == CatRole.Defender));
            Assert.IsTrue(cats.Any(cat => cat.Id == P0PrototypeCatalog.NephthysId && cat.Role == CatRole.Controller));
            Assert.IsTrue(cats.Any(cat => cat.Id == P0PrototypeCatalog.SuzuneId && cat.Role == CatRole.Healer));
        }

        [Test]
        public void StarterCats_HavePlayerFacingPresentations()
        {
            var cats = P0PrototypeCatalog.CreateStarterCats();

            foreach (CatDefinition cat in cats)
            {
                CatPresentation presentation = P0CatPresenter.Describe(cat);
                string selectionLabel = presentation.BuildSelectionLabel();

                Assert.AreEqual(cat.Id, presentation.CatId);
                Assert.AreEqual(cat.DisplayName, presentation.DisplayName);
                Assert.IsNotEmpty(presentation.Title, cat.Id);
                Assert.IsNotEmpty(presentation.RoleHint, cat.Id);
                Assert.IsNotEmpty(presentation.AuthorityLabel, cat.Id);
                Assert.IsNotEmpty(presentation.AttributeLabel, cat.Id);
                Assert.IsNotEmpty(presentation.SignatureLine, cat.Id);
                Assert.IsNotEmpty(presentation.VisualToken, cat.Id);
                Assert.IsNotEmpty(presentation.VisualIdentity, cat.Id);
                Assert.IsTrue(cat.CombatSprite.HasAsset, cat.Id);
                Assert.AreEqual("sprite", cat.CombatSprite.AssetType, cat.Id);
                Assert.AreEqual(P0AssetManifestStatus.Generated, cat.CombatSprite.Status, cat.Id);
                Assert.IsTrue(cat.CombatSprite.RequiresWorkspaceFile, cat.Id);
                StringAssert.Contains("turnaround_colored", string.Join(" ", cat.CombatSprite.SourceLockIds), cat.Id);
                Assert.AreNotEqual(cat.AuthorityId, presentation.AuthorityLabel, cat.Id);
                Assert.AreNotEqual(cat.AttributeId, presentation.AttributeLabel, cat.Id);
                StringAssert.DoesNotContain(cat.Id, selectionLabel);
                StringAssert.Contains("生命", presentation.BuildVitalLabel(cat.MaxHp, cat.MaxHp, false, 0f));
                StringAssert.DoesNotContain("HP", presentation.BuildVitalLabel(cat.MaxHp, cat.MaxHp, false, 0f));
            }
        }

        [Test]
        public void P0CatPresenter_MapsPreviewPartnerAndFallback()
        {
            CatPresentation partner = P0CatPresenter.Describe(P0CatPresenter.ShadowmaruPreviewId);
            CatPresentation fallback = P0CatPresenter.Describe("future_cat");

            Assert.IsNotEmpty(partner.DisplayName);
            Assert.AreNotEqual(P0CatPresenter.ShadowmaruPreviewId, partner.DisplayName);
            StringAssert.DoesNotContain(P0CatPresenter.ShadowmaruPreviewId, partner.BuildRosterLabel());
            Assert.IsNotEmpty(fallback.DisplayName);
            Assert.AreNotEqual("future_cat", fallback.DisplayName);
            StringAssert.DoesNotContain("future_cat", fallback.BuildRosterLabel());
        }

        [Test]
        public void StarterSkills_MapToKnownStarterCats()
        {
            var catIds = new HashSet<string>(P0PrototypeCatalog.CreateStarterCats().Select(cat => cat.Id));
            var catSkillIds = new HashSet<string>(P0PrototypeCatalog.CreateStarterCats().SelectMany(cat => cat.SkillIds));
            var skills = P0PrototypeCatalog.CreateStarterSkills();
            var skillIds = new HashSet<string>(skills.Select(skill => skill.Id));

            Assert.IsNotEmpty(skills);
            Assert.IsTrue(skills.All(skill => catIds.Contains(skill.OwnerCatId)));
            Assert.IsTrue(catSkillIds.All(skillId => skillIds.Contains(skillId)));
            Assert.IsTrue(skills.Any(skill => skill.Effects.Any(effect => effect.StatusTagId == "shield")));
            Assert.IsTrue(skills.Any(skill => skill.Effects.Any(effect => effect.StatusTagId == "slow")));
            Assert.IsTrue(skills.Any(skill => skill.Effects.Any(effect => effect.StatusTagId == "mark")));
            Assert.IsTrue(skills.Any(skill => skill.Effects.Any(effect => effect.StatusTagId == "sleep_stable")));
        }

        [Test]
        public void StarterSkills_HavePlayerFacingPresentations()
        {
            var skills = P0PrototypeCatalog.CreateStarterSkills();

            foreach (SkillDefinition skill in skills)
            {
                SkillPresentation presentation = P0SkillPresenter.Describe(skill);
                string summary = presentation.BuildSummary(skill);

                Assert.AreEqual(skill.Id, presentation.SkillId);
                Assert.IsNotEmpty(presentation.DisplayName, skill.Id);
                Assert.AreNotEqual(skill.Id, presentation.DisplayName, skill.Id);
                Assert.IsNotEmpty(presentation.RoleHint, skill.Id);
                Assert.IsNotEmpty(presentation.EffectHint, skill.Id);
                StringAssert.DoesNotContain("missing", presentation.EffectHint);
                StringAssert.Contains("冷却 " + skill.CooldownSeconds.ToString("0.#") + "s", summary);
                StringAssert.Contains("消耗 " + skill.HungerCost.ToString("0.#") + " 饱肚度", summary);
                StringAssert.DoesNotContain("cost", summary);
                StringAssert.DoesNotContain("hunger", summary);
            }
        }

        [Test]
        public void P0SkillPresenter_UsesDesignFacingStarterSkillNames()
        {
            string[] skillIds =
            {
                "saiban_sword_sweep",
                "saiban_sun_charge",
                "nephthys_moon_sand_obelisk",
                "nephthys_quicksand_trap",
                "suzune_sleep_bell",
                "suzune_healing_bell",
                "suzune_moon_torii"
            };

            foreach (string skillId in skillIds)
            {
                SkillPresentation presentation = P0SkillPresenter.Describe(skillId);
                Assert.AreEqual(skillId, presentation.SkillId);
                Assert.IsNotEmpty(presentation.DisplayName, skillId);
                Assert.AreNotEqual(skillId, presentation.DisplayName, skillId);
                StringAssert.DoesNotContain("_", presentation.DisplayName, skillId);
            }
        }

        [Test]
        public void P0SkillPresenter_CatUpgradeSkillsUseLocalizedPlayerFacingCopy()
        {
            SkillDefinition[] skills = P0CatUpgradeRuntimeCatalog.CreateUpgradeSkillDefinitions();

            foreach (SkillDefinition skill in skills)
            {
                SkillPresentation presentation = P0SkillPresenter.Describe(skill);

                Assert.AreEqual(skill.Id, presentation.SkillId);
                AssertPlayerFacingSkillPresentation(presentation, skill.Id);
            }
        }

        [Test]
        public void P0SkillPresenter_BuildButtonLabelIncludesCooldownCostAndEffect()
        {
            SkillDefinition skill = P0PrototypeCatalog.CreateStarterSkills()
                .Single(candidate => candidate.Id == "suzune_sleep_bell");

            string readyLabel = P0SkillPresenter.BuildButtonLabel(skill, cooldownSeconds: 0f);
            string cooldownLabel = P0SkillPresenter.BuildButtonLabel(skill, cooldownSeconds: 3.5f);
            SkillPresentation presentation = P0SkillPresenter.Describe(skill);

            StringAssert.StartsWith(presentation.DisplayName, readyLabel);
            StringAssert.Contains(skill.HungerCost.ToString("0.#"), readyLabel);
            StringAssert.Contains(presentation.EffectHint, readyLabel);
            StringAssert.Contains("(3.5s)", cooldownLabel);
        }

        private static void AssertPlayerFacingSkillPresentation(SkillPresentation presentation, string context)
        {
            Assert.IsNotEmpty(presentation.DisplayName, context);
            Assert.IsNotEmpty(presentation.RoleHint, context);
            Assert.IsNotEmpty(presentation.EffectHint, context);
            Assert.IsNotEmpty(presentation.VoiceLine, context);
            Assert.AreNotEqual(context, presentation.DisplayName, context);

            string visibleText = presentation.DisplayName
                + " " + presentation.RoleHint
                + " " + presentation.EffectHint
                + " " + presentation.VoiceLine;
            StringAssert.DoesNotContain("_", visibleText, context);
            StringAssert.DoesNotContain("upgrade", visibleText, context);
            StringAssert.DoesNotContain("Defender", visibleText, context);
            StringAssert.DoesNotContain("Controller", visibleText, context);
            StringAssert.DoesNotContain("Healer", visibleText, context);
            StringAssert.DoesNotContain("Ultimate", visibleText, context);
            StringAssert.DoesNotContain("Focus", visibleText, context);
            StringAssert.DoesNotContain("Prison", visibleText, context);
            StringAssert.DoesNotContain("Cleanse", visibleText, context);
        }

        [Test]
        public void CoreEnemies_ContainP0PriorityEnemies()
        {
            var enemies = P0PrototypeCatalog.CreateCoreEnemies();

            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.BlackMudNightmareId && enemy.BedDamage > 0f));
            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.DreamRailToyTrainId && enemy.BehaviorType == EnemyBehaviorType.Charger));
            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.ColdLightShadowId && enemy.PlayerDamage > 0f));
            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.RedEyeAlarmId && enemy.BehaviorType == EnemyBehaviorType.RangedHarasser));
            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.UnreadRedDotFlyerId && enemy.BehaviorType == EnemyBehaviorType.FlyingAttachment));
            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.FallingDreamTeddyId && enemy.BehaviorType == EnemyBehaviorType.EliteJumpSlam));
            Assert.IsTrue(enemies.Any(enemy => enemy.Id == P0PrototypeCatalog.CallTyrantId && enemy.BehaviorType == EnemyBehaviorType.BossCallTyrant));
        }

        [Test]
        public void StatusTags_ContainAllP0Tags()
        {
            var statusIds = new HashSet<string>(P0PrototypeCatalog.CreateStatusTags().Select(status => status.Id));

            Assert.IsTrue(statusIds.Contains(StatusTagIds.SleepStable));
            Assert.IsTrue(statusIds.Contains(StatusTagIds.Slow));
            Assert.IsTrue(statusIds.Contains(StatusTagIds.Knockback));
            Assert.IsTrue(statusIds.Contains(StatusTagIds.Mark));
            Assert.IsTrue(statusIds.Contains(StatusTagIds.Shield));
        }

        [Test]
        public void LayerOneWave_UsesOnlyKnownEnemies()
        {
            var enemyIds = new HashSet<string>(P0PrototypeCatalog.CreateCoreEnemies().Select(enemy => enemy.Id));
            WaveDefinition wave = P0PrototypeCatalog.CreateLayerOneWave();

            Assert.AreEqual(1, wave.Layer);
            Assert.IsTrue(wave.SpawnGroups.All(group => enemyIds.Contains(group.EnemyId)));
        }

        [Test]
        public void CallTyrantBossWave_SpawnsBossAndKnownAdds()
        {
            var enemyIds = new HashSet<string>(P0PrototypeCatalog.CreateCoreEnemies().Select(enemy => enemy.Id));
            WaveDefinition wave = P0PrototypeCatalog.CreateCallTyrantBossWave();

            Assert.AreEqual(10, wave.Layer);
            Assert.IsTrue(wave.SpawnGroups.Any(group => group.EnemyId == P0PrototypeCatalog.CallTyrantId));
            Assert.IsTrue(wave.SpawnGroups.All(group => enemyIds.Contains(group.EnemyId)));
        }

        [Test]
        public void LayerSixWave_AddsDreamRailToyTrainPressure()
        {
            var enemyIds = new HashSet<string>(P0PrototypeCatalog.CreateCoreEnemies().Select(enemy => enemy.Id));
            WaveDefinition wave = P0PrototypeCatalog.CreateLayerSixDefenseWave();

            Assert.AreEqual(6, wave.Layer);
            Assert.IsTrue(wave.SpawnGroups.Any(group => group.EnemyId == P0PrototypeCatalog.DreamRailToyTrainId));
            Assert.IsTrue(wave.SpawnGroups.All(group => enemyIds.Contains(group.EnemyId)));
        }

        [Test]
        public void RedEyeAlarmEliteWave_SpawnsAlarmAndUnreadFlyers()
        {
            var enemyIds = new HashSet<string>(P0PrototypeCatalog.CreateCoreEnemies().Select(enemy => enemy.Id));
            WaveDefinition wave = P0PrototypeCatalog.CreateRedEyeAlarmEliteWave();

            Assert.AreEqual(9, wave.Layer);
            Assert.AreEqual("elite_red_eye_alarm", wave.Id);
            Assert.IsTrue(wave.SpawnGroups.Any(group => group.EnemyId == P0PrototypeCatalog.RedEyeAlarmId));
            Assert.IsTrue(wave.SpawnGroups.Any(group => group.EnemyId == P0PrototypeCatalog.UnreadRedDotFlyerId));
            Assert.IsTrue(wave.SpawnGroups.All(group => enemyIds.Contains(group.EnemyId)));
        }

        [Test]
        public void FallingDreamTeddyEliteWave_SpawnsTeddyAndPressureAdds()
        {
            var enemyIds = new HashSet<string>(P0PrototypeCatalog.CreateCoreEnemies().Select(enemy => enemy.Id));
            WaveDefinition wave = P0PrototypeCatalog.CreateFallingDreamTeddyEliteWave();

            Assert.AreEqual("elite_falling_dream_teddy", wave.Id);
            Assert.IsTrue(wave.SpawnGroups.Any(group => group.EnemyId == P0PrototypeCatalog.FallingDreamTeddyId));
            Assert.IsTrue(wave.SpawnGroups.Any(group => group.EnemyId == P0PrototypeCatalog.DreamRailToyTrainId));
            Assert.IsTrue(wave.SpawnGroups.All(group => enemyIds.Contains(group.EnemyId)));
        }

        [Test]
        public void CreateWaveForContentId_MapsRouteCombatNodes()
        {
            Assert.AreEqual("layer_01_defense", P0PrototypeCatalog.CreateWaveForContentId("layer_01_defense").Id);
            Assert.AreEqual("layer_06_defense", P0PrototypeCatalog.CreateWaveForContentId("layer_06_defense").Id);
            Assert.AreEqual("elite_red_eye_alarm", P0PrototypeCatalog.CreateWaveForContentId("elite_red_eye_alarm").Id);
            Assert.AreEqual("elite_falling_dream_teddy", P0PrototypeCatalog.CreateWaveForContentId("elite_falling_dream_teddy").Id);
            Assert.AreEqual("boss_call_tyrant", P0PrototypeCatalog.CreateWaveForContentId("boss_call_tyrant").Id);
        }
    }
}
