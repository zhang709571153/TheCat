using NUnit.Framework;
using TheCat.Data;
using TheCat.Data.Catalogs;
using TheCat.Data.CoreValues;
using TheCat.Data.Definitions;
using TheCat.Roguelite;

namespace TheCat.Tests
{
    public sealed class CoreValueTests
    {
        [Test]
        public void OwnerSleep_AppliesDamageRestoreAndMaxPenalty()
        {
            OwnerSleepState sleep = new OwnerSleepState();

            sleep.ApplyDamage(70f);
            Assert.AreEqual(30f, sleep.Current);
            Assert.AreEqual(OwnerSleepStage.Danger, sleep.Stage);

            sleep.Restore(50f);
            Assert.AreEqual(80f, sleep.Current);

            float appliedPenalty = sleep.ApplyMaxPenalty(40f);
            Assert.AreEqual(40f, appliedPenalty);
            Assert.AreEqual(60f, sleep.Max);
            Assert.AreEqual(60f, sleep.Current);
        }

        [Test]
        public void OwnerSleep_FailsAtZero()
        {
            OwnerSleepState sleep = new OwnerSleepState();

            sleep.ApplyDamage(500f);

            Assert.IsTrue(sleep.IsFailed);
            Assert.AreEqual(OwnerSleepStage.Failed, sleep.Stage);
        }

        [Test]
        public void CatVital_EntersWeakAndRecoversWithP0SimplifiedRule()
        {
            CatVitalState cat = new CatVitalState("saiban", 200f);

            cat.ApplyDamage(250f);
            Assert.IsTrue(cat.IsWeak);
            Assert.IsFalse(cat.CanSwitchTo);

            cat.Tick(20f);
            Assert.IsFalse(cat.IsWeak);
            Assert.AreEqual(60f, cat.CurrentHp, 0.001f);
        }

        [Test]
        public void CatVital_RestoreToAtLeastCanClearWeak()
        {
            CatVitalState cat = new CatVitalState("saiban", 200f, 0f, 10f);

            cat.RestoreToAtLeast(140f);

            Assert.IsFalse(cat.IsWeak);
            Assert.AreEqual(140f, cat.CurrentHp, 0.001f);

            cat.RestoreToAtLeast(80f);

            Assert.AreEqual(140f, cat.CurrentHp, 0.001f);
        }

        [Test]
        public void TeamPoop_UsesEarlyCountdownAndLitterBox()
        {
            P0Tuning tuning = P0Tuning.Default;
            TeamPoopGauge poop = new TeamPoopGauge(95f);

            poop.Tick(20f, tuning, isDigesting: false, layer: 1);
            Assert.AreEqual(PoopStage.Critical, poop.Stage);
            Assert.AreEqual(TeamPoopGauge.EarlyLayerCountdownSeconds, poop.CountdownRemainingSeconds);

            poop.UseLitterBox(tuning);
            Assert.IsFalse(poop.IsCountdownActive);
            Assert.AreEqual(40f, poop.Current);
        }

        [Test]
        public void TeamPoop_IncidentResetsGaugeAfterCountdown()
        {
            P0Tuning tuning = P0Tuning.Default;
            TeamPoopGauge poop = new TeamPoopGauge(100f);

            poop.Tick(1f, tuning, isDigesting: false, layer: 7);
            poop.Tick(TeamPoopGauge.StandardCountdownSeconds, tuning, isDigesting: false, layer: 7);

            Assert.IsTrue(poop.TryConsumeIncident());
            Assert.IsFalse(poop.TryConsumeIncident());
            Assert.AreEqual(TeamPoopGauge.ResetAfterIncident, poop.Current);
        }

        [Test]
        public void TeamPoop_ExtendsActiveCountdownOnly()
        {
            P0Tuning tuning = P0Tuning.Default;
            TeamPoopGauge inactive = new TeamPoopGauge(50f);

            Assert.AreEqual(0f, inactive.ExtendCountdown(8f));
            Assert.AreEqual(0f, inactive.CountdownRemainingSeconds);

            TeamPoopGauge active = new TeamPoopGauge(100f);
            active.Tick(1f, tuning, isDigesting: false, layer: 7);
            float beforeExtension = active.CountdownRemainingSeconds;

            float extended = active.ExtendCountdown(8f);

            Assert.AreEqual(8f, extended);
            Assert.AreEqual(beforeExtension + 8f, active.CountdownRemainingSeconds);
        }

        [Test]
        public void TeamPoop_ExtendCountdownRejectsNegativeSeconds()
        {
            TeamPoopGauge poop = new TeamPoopGauge(100f);

            Assert.Throws<System.ArgumentOutOfRangeException>(() => poop.ExtendCountdown(-1f));
        }

        [Test]
        public void TeamHunger_SpendsFeedsAndDigests()
        {
            TeamHungerGauge hunger = new TeamHungerGauge(50f);

            hunger.SpendForSmallSkill();
            hunger.SpendForUltimate();
            Assert.AreEqual(39f, hunger.Current);
            Assert.AreEqual(HungerStage.Starving, hunger.Stage);
            Assert.AreEqual(0.8f, hunger.DamageMultiplier);

            hunger.UseFeeder();
            Assert.AreEqual(89f, hunger.Current);
            Assert.IsTrue(hunger.IsDigesting);

            hunger.Tick(45f, P0Tuning.Default);
            Assert.IsFalse(hunger.IsDigesting);
        }

        [Test]
        public void P0CoreValuePresenter_FormatsBattleCoreValuesWithActionHints()
        {
            OwnerSleepState sleep = new OwnerSleepState(20f, 80f, 100f);
            TeamPoopGauge poop = new TeamPoopGauge(100f);
            TeamHungerGauge hunger = new TeamHungerGauge(20f);

            poop.Tick(1f, P0Tuning.Default, isDigesting: false, layer: 7);
            hunger.UseFeeder();

            CoreValuePresentation sleepPresentation = P0CoreValuePresenter.DescribeOwnerSleep(sleep);
            CoreValuePresentation poopPresentation = P0CoreValuePresenter.DescribeTeamPoop(poop);
            CoreValuePresentation hungerPresentation = P0CoreValuePresenter.DescribeTeamHunger(hunger);

            Assert.AreEqual("主人睡眠度", sleepPresentation.Label);
            Assert.AreEqual(P0VisualAssetCatalog.OwnerSleepIconId, sleepPresentation.VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.OwnerSleepGaugeFrameId, sleepPresentation.GaugeFrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.OwnerSleepGaugeFillId, sleepPresentation.GaugeFillAsset.AssetId);
            Assert.AreEqual(0.25f, sleepPresentation.FillRatio, 0.001f);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopIconId, poopPresentation.VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopGaugeFrameId, poopPresentation.GaugeFrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamPoopGaugeFillId, poopPresentation.GaugeFillAsset.AssetId);
            Assert.AreEqual(1f, poopPresentation.FillRatio, 0.001f);
            Assert.AreEqual(P0VisualAssetCatalog.TeamHungerIconId, hungerPresentation.VisualAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamHungerGaugeFrameId, hungerPresentation.GaugeFrameAsset.AssetId);
            Assert.AreEqual(P0VisualAssetCatalog.TeamHungerGaugeFillId, hungerPresentation.GaugeFillAsset.AssetId);
            Assert.AreEqual(0.7f, hungerPresentation.FillRatio, 0.001f);
            StringAssert.Contains("20/80 危险", sleepPresentation.BuildSummary());
            StringAssert.Contains("上限损失 20", sleepPresentation.BuildSummary());
            StringAssert.Contains("使用床交互或安眠技能", sleepPresentation.BuildSummary());
            StringAssert.Contains("屎意值: 100/100 危急 倒计时", poopPresentation.BuildSummary());
            StringAssert.Contains("立刻使用猫砂盆", poopPresentation.BuildSummary());
            StringAssert.Contains("饱肚度: 70/100 充足 伤害 1 倍 消化中", hungerPresentation.BuildSummary());
        }

        [Test]
        public void P0CoreValuePresenter_FormatsRunCoreSummaryFromPersistentValues()
        {
            RunCoreValues values = new RunCoreValues(
                ownerSleepCurrent: 10f,
                ownerSleepMax: 70f,
                ownerSleepBaseMax: 100f,
                teamPoop: 92f,
                teamHunger: 8f);

            string summary = P0CoreValuePresenter.BuildRunCoreSummary(values);

            StringAssert.Contains("主人睡眠度: 10/70 危急 上限损失 30", summary);
            StringAssert.Contains("立刻守住床", summary);
            StringAssert.Contains("屎意值: 92/100 危急", summary);
            StringAssert.Contains("尽快使用猫砂盆", summary);
            StringAssert.Contains("饱肚度: 8/100 空腹 伤害 0.65 倍", summary);
            StringAssert.Contains("立刻使用喂食器", summary);
        }

        [Test]
        public void RunCoreValues_CapturesAndRestNestRecoversPressure()
        {
            RunCoreValues values = new RunCoreValues();

            values.Capture(
                ownerSleepCurrent: 42f,
                ownerSleepMax: 80f,
                ownerSleepBaseMax: 100f,
                teamPoop: 75f,
                teamHunger: 35f);
            values.ApplyRestNestRecovery();

            Assert.AreEqual(67f, values.OwnerSleepCurrent, 0.001f);
            Assert.AreEqual(80f, values.OwnerSleepMax, 0.001f);
            Assert.AreEqual(100f, values.OwnerSleepBaseMax, 0.001f);
            Assert.AreEqual(45f, values.TeamPoop, 0.001f);
            Assert.AreEqual(80f, values.TeamHunger, 0.001f);
        }

        [Test]
        public void RunCatVitals_CapturesAndRestNestRecoversWeakCats()
        {
            RunCatVitals vitals = new RunCatVitals();

            vitals.Capture("saiban", 200f, 20f, 9f);
            vitals.Capture("suzune", 100f, 90f, 0f);
            vitals.ApplyRestNestRecovery();

            Assert.IsTrue(vitals.TryGet("saiban", out RunCatVitalSnapshot saiban));
            Assert.IsTrue(vitals.TryGet("suzune", out RunCatVitalSnapshot suzune));
            Assert.AreEqual(140f, saiban.CurrentHp, 0.001f);
            Assert.AreEqual(0f, saiban.WeakRemainingSeconds, 0.001f);
            Assert.IsFalse(saiban.IsWeak);
            Assert.AreEqual(90f, suzune.CurrentHp, 0.001f);
            Assert.AreEqual(0, vitals.CountWeakCats());
            Assert.AreEqual(0.7f, vitals.GetLowestHpRatio(), 0.001f);
        }
    }
}
