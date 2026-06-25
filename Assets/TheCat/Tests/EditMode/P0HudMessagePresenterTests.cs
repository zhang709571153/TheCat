using NUnit.Framework;
using TheCat.Gameplay;

namespace TheCat.Tests
{
    public sealed class P0HudMessagePresenterTests
    {
        [Test]
        public void BuildVisibleMessage_DefaultHudHidesDiagnosticsChannel()
        {
            Assert.AreEqual(
                string.Empty,
                P0HudMessagePresenter.BuildVisibleMessage(
                    "Smoke setup complete without a debug prefix.",
                    P0HudMessageChannel.Diagnostics,
                    false));
        }

        [Test]
        public void BuildVisibleMessage_DefaultHudKeepsPlayerFeedback()
        {
            const string message = "反馈：正向 交互成功 猫砂盆 - 已使用";

            Assert.AreEqual(
                message,
                P0HudMessagePresenter.BuildVisibleMessage(message, P0HudMessageChannel.Player, false));
        }

        [Test]
        public void BuildVisibleMessage_DiagnosticsHudKeepsDebugMessages()
        {
            const string message = "调试敌人 HUD 已准备：敌人 HUD：3 预警 1。";

            Assert.AreEqual(
                message,
                P0HudMessagePresenter.BuildVisibleMessage(message, P0HudMessageChannel.Diagnostics, true));
        }
    }
}
