using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class P0SettlementPresenterTests
    {
        [Test]
        public void BuildRows_IncludesP0SettlementSections()
        {
            RunProgressionState run = CreateClearedRun();
            P0RunSettlementSummary summary = new P0RunSettlementSummary(run);

            IReadOnlyList<string> rows = P0SettlementPresenter.BuildRows(summary);

            Assert.AreEqual(11, rows.Count);
            Assert.IsTrue(Contains(rows, "结算：路线通关"));
            Assert.IsTrue(Contains(rows, "路线：10/10 节点"));
            Assert.IsTrue(Contains(rows, "战斗：5胜 / 0负"));
            Assert.IsTrue(Contains(rows, "敌人压力："));
            Assert.IsTrue(Contains(rows, "猫生命："));
            Assert.IsTrue(Contains(rows, "行动："));
            Assert.IsTrue(Contains(rows, "路线状态："));
            Assert.IsTrue(Contains(rows, "最终核心："));
            Assert.IsTrue(Contains(rows, "最终猫咪生命："));
            Assert.IsFalse(Contains(rows, "HP"));
            Assert.IsFalse(Contains(rows, " Lv"));
            Assert.IsFalse(Contains(rows, "阻止"));
            Assert.IsFalse(Contains(rows, "索敌"));
            Assert.IsFalse(Contains(rows, "缺失定义"));
        }

        [Test]
        public void HasP0ClearedSettlementRows_AcceptsClearedTenLayerRun()
        {
            P0RunSettlementSummary summary = new P0RunSettlementSummary(CreateClearedRun());

            Assert.IsTrue(P0SettlementPresenter.HasP0ClearedSettlementRows(summary));
            StringAssert.Contains("路线通关 路线 10/10", P0SettlementPresenter.BuildCompactSummary(summary));
            Assert.IsFalse(P0SettlementPresenter.BuildCompactSummary(summary).Contains(" Lv"));
        }

        [Test]
        public void HasP0ActionTelemetry_AcceptsClearedRunWithSkillsAndInteractions()
        {
            P0RunSettlementSummary summary = new P0RunSettlementSummary(CreateClearedRun());

            Assert.IsTrue(P0SettlementPresenter.HasP0ActionTelemetry(summary));
            string actionSummary = P0SettlementPresenter.BuildActionTelemetrySummary(summary);
            StringAssert.Contains("切换 1/1", actionSummary);
            StringAssert.Contains("自动锁定目标 1/1 技能锁定目标 1/1", actionSummary);
            StringAssert.Contains("技能 1/3", actionSummary);
            StringAssert.Contains("交互 1/2", actionSummary);
            Assert.IsFalse(actionSummary.Contains("阻止"));
            Assert.IsFalse(actionSummary.Contains("索敌"));
            Assert.IsFalse(actionSummary.Contains("缺失定义"));
        }

        [Test]
        public void HasP0ActionTelemetry_RejectsClearedRunWithoutPlayerActions()
        {
            P0RunSettlementSummary summary = new P0RunSettlementSummary(CreateClearedRun(recordActions: false));

            Assert.IsTrue(P0SettlementPresenter.HasP0ClearedSettlementRows(summary));
            Assert.IsFalse(P0SettlementPresenter.HasP0ActionTelemetry(summary));
            string actionSummary = P0SettlementPresenter.BuildActionTelemetrySummary(summary);
            StringAssert.Contains("切换 0/0", actionSummary);
            StringAssert.Contains("自动锁定目标 0/0 技能锁定目标 0/0", actionSummary);
            StringAssert.Contains("技能 0/0", actionSummary);
            StringAssert.Contains("交互 0/0", actionSummary);
            Assert.IsFalse(actionSummary.Contains("阻止"));
            Assert.IsFalse(actionSummary.Contains("索敌"));
            Assert.IsFalse(actionSummary.Contains("缺失定义"));
        }

        [Test]
        public void BuildPlayerFocusRows_ExposesReadableClearedSettlement()
        {
            P0RunSettlementSummary summary = new P0RunSettlementSummary(CreateClearedRun());

            IReadOnlyList<string> rows = P0SettlementPresenter.BuildPlayerFocusRows(summary);

            Assert.AreEqual(6, rows.Count);
            Assert.IsTrue(Contains(rows, "结算：路线通关"));
            Assert.IsTrue(Contains(rows, "路线：10/10 节点"));
            Assert.IsTrue(Contains(rows, "战斗：5胜 / 0负"));
            Assert.IsTrue(Contains(rows, "路线状态：梦屑"));
            Assert.IsTrue(Contains(rows, "小鱼干"));
            Assert.IsTrue(Contains(rows, "祝福"));
            Assert.IsTrue(Contains(rows, "最终核心："));
            Assert.IsTrue(Contains(rows, "最终猫咪生命："));
            Assert.IsFalse(Contains(rows, "HP"));
            Assert.IsFalse(Contains(rows, " Lv"));
            Assert.IsFalse(Contains(rows, "阻止"));
            Assert.IsFalse(Contains(rows, "索敌"));
            Assert.IsFalse(Contains(rows, "缺失定义"));
        }

        [Test]
        public void HasP0ClearedSettlementRows_RejectsIncompleteRun()
        {
            RunProgressionState run = new RunProgressionState(P0RouteCatalog.CreateTenLayerRoute(), new[] { "saiban" });
            P0RunSettlementSummary summary = new P0RunSettlementSummary(run);

            Assert.IsFalse(P0SettlementPresenter.HasP0ClearedSettlementRows(summary));
            Assert.IsFalse(P0SettlementPresenter.HasP0FailedSettlementRows(summary));
            StringAssert.Contains("路线进行中", P0SettlementPresenter.BuildCompactSummary(summary));
        }

        [Test]
        public void HasP0FailedSettlementRows_AcceptsFailedPartialRun()
        {
            P0RunSettlementSummary summary = new P0RunSettlementSummary(CreateFailedRun());
            IReadOnlyList<string> rows = P0SettlementPresenter.BuildRows(summary);

            Assert.IsTrue(P0SettlementPresenter.HasP0FailedSettlementRows(summary));
            Assert.IsFalse(P0SettlementPresenter.HasP0ClearedSettlementRows(summary));
            Assert.IsTrue(Contains(rows, "结算：路线失败"));
            Assert.IsTrue(Contains(rows, "路线：1/10 节点"));
            Assert.IsTrue(Contains(rows, "战斗：0胜 / 1负"));
            Assert.IsTrue(Contains(rows, "最终核心："));
            Assert.IsTrue(Contains(rows, "最终猫咪生命："));
            Assert.IsFalse(Contains(rows, "HP"));
            Assert.IsFalse(Contains(rows, " Lv"));
            Assert.IsFalse(Contains(rows, "阻止"));
            Assert.IsFalse(Contains(rows, "索敌"));
            Assert.IsFalse(Contains(rows, "缺失定义"));
            StringAssert.Contains("路线失败 路线 1/10", P0SettlementPresenter.BuildCompactSummary(summary));
            Assert.IsFalse(P0SettlementPresenter.BuildCompactSummary(summary).Contains(" Lv"));
        }

        private static RunProgressionState CreateClearedRun(bool recordActions = true)
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
            run.Wallet.AddDreamShards(9);
            run.Wallet.AddFishTreats(7);
            run.Blessings.Add(P0BlessingCatalog.CreateAuthorityBlessings()[0]);
            run.CoreValues.Capture(88f, 100f, 100f, 12f, 72f);
            run.CatVitals.Capture("saiban", 220f, 180f, 0f);
            run.CatVitals.Capture("nephthys", 160f, 100f, 0f);
            run.CatVitals.Capture("suzune", 120f, 80f, 0f);

            for (int i = 0; i < run.Route.Route.LayerCount; i++)
            {
                if (RouteNodeResolver.RequiresBattle(run.Route.CurrentNode.NodeType))
                {
                    NodeMetrics metrics = run.Metrics.BeginNode(run.Route.CurrentLayer, run.Route.CurrentNode.Id, run.CoreValues.OwnerSleepCurrent);
                    if (recordActions && run.Metrics.Nodes.Count == 1)
                    {
                        metrics.RecordBossThrowPressure(4f, 0f);
                        metrics.RecordCatPressure(12f, 5f);
                        metrics.RecordCatHeal(18f);
                        metrics.RecordCatShield(22f);
                        metrics.RecordCatSwitchSuccess();
                        metrics.RecordAutoTargetAcquired();
                        metrics.RecordSkillTargetAcquired();
                        metrics.RecordSkillCastSuccess();
                        metrics.RecordSkillCastBlockedByCooldown();
                        metrics.RecordSkillCastBlockedByTarget();
                        metrics.RecordLitterBoxUse();
                        metrics.RecordInteractionBlockedByRange();
                    }

                    metrics.Complete(NodeResult.Success, 8f, run.CoreValues.OwnerSleepCurrent);
                }

                run.Route.CompleteCurrentNode(NodeResult.Success);
            }

            return run;
        }

        private static RunProgressionState CreateFailedRun()
        {
            RunProgressionState run = new RunProgressionState(
                P0RouteCatalog.CreateTenLayerRoute(),
                new[] { "saiban", "nephthys", "suzune" });
            run.CoreValues.Capture(0f, 100f, 100f, 45f, 18f);
            run.CatVitals.Capture("saiban", 220f, 70f, 0f);
            run.CatVitals.Capture("nephthys", 160f, 30f, 0f);
            run.CatVitals.Capture("suzune", 120f, 90f, 0f);

            NodeMetrics metrics = run.Metrics.BeginNode(
                run.Route.CurrentLayer,
                run.Route.CurrentNode.Id,
                run.CoreValues.OwnerSleepMax);
            metrics.RecordBedPressure(24f, 4f);
            metrics.RecordCatPressure(18f, 6f);
            metrics.RecordAutoTargetAcquired();
            metrics.RecordSkillTargetAcquired();
            metrics.RecordSkillCastSuccess();
            metrics.RecordInteractionBlockedByRange();
            metrics.Complete(NodeResult.Failure, 18f, 0f);

            run.Route.CompleteCurrentNode(NodeResult.Failure);
            return run;
        }

        private static bool Contains(IReadOnlyList<string> rows, string expected)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                if (rows[i].Contains(expected))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
