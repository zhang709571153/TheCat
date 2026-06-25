using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;
using TheCat.Roguelite;
using TheCat.Tools;

namespace TheCat.Tests
{
    public sealed class P0PlayableReadinessTests
    {
        [Test]
        public void EvaluatePrototypeBuild_PassesCurrentP0ReadinessGate()
        {
            P0PlayableReadinessReport report = P0PlayableReadiness.EvaluatePrototypeBuild();

            Assert.IsTrue(report.IsReady, report.BuildDetailedSummary());
            Assert.AreEqual(0, report.FailureCount);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.SceneFlowCheckId, out P0PlayableReadinessCheck sceneFlow));
            Assert.AreEqual(P0PlayableReadinessState.Passed, sceneFlow.State);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.EntryCharacterSelectCheckId, out P0PlayableReadinessCheck entry));
            Assert.AreEqual(P0PlayableReadinessState.Passed, entry.State);
            StringAssert.Contains("cat-room preparation", entry.Message);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.LoadingStartSurfaceCheckId, out P0PlayableReadinessCheck loadingStart));
            Assert.AreEqual(P0PlayableReadinessState.Passed, loadingStart.State);
            StringAssert.Contains("screenshot hook", loadingStart.Message);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.RuntimeSettingsAcceptanceCheckId, out P0PlayableReadinessCheck runtimeSettings));
            Assert.AreEqual(P0PlayableReadinessState.Passed, runtimeSettings.State);
            StringAssert.Contains("full settings hook", runtimeSettings.Message);
            StringAssert.Contains("restart confirmation", runtimeSettings.Message);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.SkillSelectionAcceptanceCheckId, out P0PlayableReadinessCheck skillSelection));
            Assert.AreEqual(P0PlayableReadinessState.Passed, skillSelection.State);
            StringAssert.Contains("runtime skill mapping", skillSelection.Message);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.BattleReadabilityAcceptanceCheckId, out P0PlayableReadinessCheck battleReadability));
            Assert.AreEqual(P0PlayableReadinessState.Passed, battleReadability.State);
            StringAssert.Contains("result actions", battleReadability.Message);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.StatusTagsCheckId, out P0PlayableReadinessCheck statusTags));
            Assert.AreEqual(P0PlayableReadinessState.Passed, statusTags.State);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.DreamMapCheckId, out P0PlayableReadinessCheck dreamMaps));
            Assert.AreEqual(P0PlayableReadinessState.Passed, dreamMaps.State);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.EgyptReadinessCheckId, out P0PlayableReadinessCheck egypt));
            Assert.AreEqual(P0PlayableReadinessState.Passed, egypt.State);
            StringAssert.Contains("placeholder", egypt.Message);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.GoldenPathCheckId, out P0PlayableReadinessCheck goldenPath));
            Assert.AreNotEqual(P0PlayableReadinessState.Failed, goldenPath.State);
        }

        [Test]
        public void Evaluate_MissingStarterSkillFailsReadiness()
        {
            List<SkillDefinition> skills = new List<SkillDefinition>(P0PrototypeCatalog.CreateStarterSkills());
            skills.RemoveAll(skill => skill.Id == "suzune_moon_torii");

            P0PlayableReadinessReport report = P0PlayableReadiness.Evaluate(CreateContext(skills: skills));

            Assert.IsFalse(report.IsReady);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.StarterSkillsCheckId, out P0PlayableReadinessCheck skillsCheck));
            Assert.AreEqual(P0PlayableReadinessState.Failed, skillsCheck.State);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.StatusTagsCheckId, out P0PlayableReadinessCheck statusCheck));
            Assert.AreEqual(P0PlayableReadinessState.Failed, statusCheck.State);
        }

        [Test]
        public void Evaluate_MissingRouteNodeTypeFailsRouteStructure()
        {
            RouteDefinition route = new RouteDefinition(
                "broken_route",
                new[]
                {
                    new[] { new RouteNodeDefinition(1, "defense_1", RouteNodeType.Defense, "layer_01_defense") },
                    new[] { new RouteNodeDefinition(2, "defense_2", RouteNodeType.Defense, "layer_01_defense") },
                    new[] { new RouteNodeDefinition(3, "defense_3", RouteNodeType.Defense, "layer_01_defense") },
                    new[] { new RouteNodeDefinition(4, "defense_4", RouteNodeType.Defense, "layer_01_defense") },
                    new[] { new RouteNodeDefinition(5, "defense_5", RouteNodeType.Defense, "layer_01_defense") },
                    new[] { new RouteNodeDefinition(6, "defense_6", RouteNodeType.Defense, "layer_06_defense") },
                    new[] { new RouteNodeDefinition(7, "defense_7", RouteNodeType.Defense, "layer_06_defense") },
                    new[] { new RouteNodeDefinition(8, "defense_8", RouteNodeType.Defense, "layer_06_defense") },
                    new[] { new RouteNodeDefinition(9, "defense_9", RouteNodeType.Defense, "layer_06_defense") },
                    new[] { new RouteNodeDefinition(10, P0RouteCatalog.BossNodeId, RouteNodeType.Boss, "boss_call_tyrant") }
                });

            P0PlayableReadinessReport report = P0PlayableReadiness.Evaluate(CreateContext(route: route));

            Assert.IsFalse(report.IsReady);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.RouteStructureCheckId, out P0PlayableReadinessCheck routeCheck));
            Assert.AreEqual(P0PlayableReadinessState.Failed, routeCheck.State);
        }

        [Test]
        public void Evaluate_MissingCombatWaveFailsBattleWaveCheck()
        {
            P0PlayableReadinessReport report = P0PlayableReadiness.Evaluate(CreateContext(waveResolver: contentId => null));

            Assert.IsFalse(report.IsReady);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.BattleWavesCheckId, out P0PlayableReadinessCheck waveCheck));
            Assert.AreEqual(P0PlayableReadinessState.Failed, waveCheck.State);
        }

        [Test]
        public void Evaluate_MissingGoldenPathAcceptanceFailsReadiness()
        {
            P0PlayableReadinessReport report = P0PlayableReadiness.Evaluate(CreateContext(includeGoldenPathAcceptance: false));

            Assert.IsFalse(report.IsReady);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.GoldenPathCheckId, out P0PlayableReadinessCheck goldenPath));
            Assert.AreEqual(P0PlayableReadinessState.Failed, goldenPath.State);
        }

        [Test]
        public void Evaluate_GoldenPathWithoutActionTelemetryFailsReadiness()
        {
            P0GoldenPathReport goldenPath = P0GoldenPathSimulator.SimulateRun(
                P0RunSession.CreateDefaultStarterCatIds(),
                new P0GoldenPathSimulationOptions(useAssistedOpeningActions: false));
            P0GoldenPathAcceptanceReport acceptance = P0GoldenPathAcceptance.Evaluate(goldenPath);

            P0PlayableReadinessReport report = P0PlayableReadiness.Evaluate(CreateContext(goldenPathAcceptance: acceptance));

            Assert.IsFalse(report.IsReady);
            Assert.IsTrue(report.TryGetCheck(P0PlayableReadiness.GoldenPathCheckId, out P0PlayableReadinessCheck goldenPathCheck));
            Assert.AreEqual(P0PlayableReadinessState.Failed, goldenPathCheck.State);
            StringAssert.Contains("cat switch telemetry", goldenPathCheck.Message);
            StringAssert.Contains("auto target telemetry", goldenPathCheck.Message);
            StringAssert.Contains("skill target telemetry", goldenPathCheck.Message);
            StringAssert.Contains("skill action telemetry", goldenPathCheck.Message);
            StringAssert.Contains("interaction telemetry", goldenPathCheck.Message);
        }

        [Test]
        public void BuildDetailedSummary_IncludesGateNames()
        {
            P0PlayableReadinessReport report = P0PlayableReadiness.EvaluatePrototypeBuild();

            string summary = report.BuildDetailedSummary();

            StringAssert.Contains("Scene Flow", summary);
            StringAssert.Contains("Entry Character Select", summary);
            StringAssert.Contains("Loading Start Surface", summary);
            StringAssert.Contains("Pause Settings Acceptance", summary);
            StringAssert.Contains("Skill Selection Acceptance", summary);
            StringAssert.Contains("Battle Readability Acceptance", summary);
            StringAssert.Contains("Starter Trio", summary);
            StringAssert.Contains("Egypt Readiness", summary);
            StringAssert.Contains("Golden Path", summary);
        }

        private static P0PlayableReadinessContext CreateContext(
            RouteDefinition route = null,
            IReadOnlyList<SkillDefinition> skills = null,
            P0WaveResolver waveResolver = null,
            P0GoldenPathAcceptanceReport goldenPathAcceptance = null,
            bool includeGoldenPathAcceptance = true)
        {
            IReadOnlyList<SkillDefinition> activeSkills = skills ?? P0PrototypeCatalog.CreateStarterSkills();
            P0GoldenPathAcceptanceReport acceptance = includeGoldenPathAcceptance
                ? goldenPathAcceptance ?? P0GoldenPathAcceptance.Evaluate(P0GoldenPathSimulator.SimulateDefaultRun())
                : null;

            return new P0PlayableReadinessContext(
                route ?? P0RouteCatalog.CreateTenLayerRoute(),
                P0PrototypeCatalog.CreateStarterCats(),
                activeSkills,
                P0PrototypeCatalog.CreateCoreEnemies(),
                P0PrototypeCatalog.CreateStatusTags(),
                waveResolver ?? P0PrototypeCatalog.CreateWaveForContentId,
                acceptance,
                P0StatusTagCoverage.Evaluate(P0PrototypeCatalog.CreateStatusTags(), activeSkills));
        }
    }
}
