using System.Reflection;
using NUnit.Framework;
using TheCat.Gameplay;
using TheCat.Inputs;
using TheCat.Roguelite;
using UnityEngine;

namespace TheCat.Tests
{
    public sealed class P0GrayboxBattleWorldDiagnosticsTests
    {
        [Test]
        public void CollapseDiagnosticsHudForSmoke_HidesWorldTextWhileKeepingWarningShapes()
        {
            P0RunSession.Clear();
            GameObject root = new GameObject("GrayboxBattleWorldDiagnosticsTest");
            try
            {
                GrayboxBattleController controller = root.AddComponent<GrayboxBattleController>();
                InvokeAwake(controller);
                controller.BeginBattle();
                controller.ExecuteInputCommand(P0InputCommand.ToggleDiagnosticsHud);

                Assert.IsTrue(controller.PrimeStatusHudForSmoke());
                Assert.IsTrue(controller.PrimeEnemyHudForSmoke());
                controller.AdvanceGraybox(0f);

                Assert.IsTrue(HasActiveWorldObject(root.transform, "StatusLabel"));
                Assert.IsTrue(HasActiveWorldObject(root.transform, "EnemyWarningLabel"));

                controller.CollapseDiagnosticsHudForSmoke();
                controller.AdvanceGraybox(0f);

                Assert.IsFalse(HasActiveWorldObject(root.transform, "StatusLabel"));
                Assert.IsFalse(HasActiveWorldObject(root.transform, "EnemyWarningLabel"));
                Assert.IsTrue(
                    HasActiveWorldObject(root.transform, "EnemyWarningLine")
                    || HasActiveWorldObject(root.transform, "EnemyWarningRing"));
                Assert.Greater(controller.BuildStatusHudEntriesForSmoke().Count, 0);
                Assert.Greater(controller.BuildEnemyHudCardsForSmoke().Count, 0);
            }
            finally
            {
                Object.DestroyImmediate(root);
                P0RunSession.Clear();
            }
        }

        private static bool HasActiveWorldObject(Transform root, string objectName)
        {
            Transform[] transforms = root.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < transforms.Length; i++)
            {
                Transform child = transforms[i];
                if (child.name == objectName && child.gameObject.activeInHierarchy)
                {
                    return true;
                }
            }

            return false;
        }

        private static void InvokeAwake(GrayboxBattleController controller)
        {
            MethodInfo awake = typeof(GrayboxBattleController).GetMethod(
                "Awake",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(awake);
            awake.Invoke(controller, null);
        }
    }
}
