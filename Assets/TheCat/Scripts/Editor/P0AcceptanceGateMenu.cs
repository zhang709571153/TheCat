using TheCat.Tools;
using UnityEditor;
using UnityEngine;

namespace TheCat.EditorTools
{
    internal static class P0AcceptanceGateMenu
    {
        private const string MenuPath = "TheCat/P0/Run Acceptance Gates (Log Only)";

        [MenuItem(MenuPath, false, 87)]
        private static void RunAcceptanceGatesLogOnly()
        {
            P0BatchmodeAcceptanceReport report = P0BatchmodeAcceptanceRunner.EvaluateFullP0Acceptance();
            bool passed = report.IsPassed;
            for (int i = 0; i < report.Gates.Count; i++)
            {
                P0BatchmodeGateResult gate = report.Gates[i];
                Log(gate.Title, gate.Passed, gate.Detail);
            }

            Log(
                "P0 Acceptance Gates",
                passed,
                passed
                    ? "P0 acceptance gates passed without blocking dialogs."
                    : "P0 acceptance gates found blocking issues. See prior log entries.");
        }

        private static void Log(string title, bool passed, string detail)
        {
            string message = "[TheCat] " + title + ": " + detail;
            if (passed)
            {
                Debug.Log(message);
                return;
            }

            Debug.LogError(message);
        }
    }
}
