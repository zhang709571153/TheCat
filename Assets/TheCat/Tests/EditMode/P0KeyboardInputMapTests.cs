using System;
using System.Collections.Generic;
using NUnit.Framework;
using TheCat.Inputs;
using UnityEngine.InputSystem;

namespace TheCat.Tests
{
    public sealed class P0KeyboardInputMapTests
    {
        [Test]
        public void AllP0Commands_HaveKeyboardBindings()
        {
            foreach (P0InputCommand command in Enum.GetValues(typeof(P0InputCommand)))
            {
                Assert.IsTrue(P0KeyboardInputMap.TryGetBinding(command, out P0InputBinding binding), command.ToString());
                Assert.AreEqual(command, binding.Command);
                Assert.IsFalse(string.IsNullOrWhiteSpace(binding.PrimaryKeyLabel));
            }
        }

        [Test]
        public void KeyboardBindings_DoNotReusePrimaryKeys()
        {
            HashSet<string> primaryKeyLabels = new HashSet<string>();
            IReadOnlyList<P0InputBinding> bindings = P0KeyboardInputMap.AllBindings;

            for (int i = 0; i < bindings.Count; i++)
            {
                Assert.IsTrue(primaryKeyLabels.Add(bindings[i].PrimaryKeyLabel), bindings[i].Command.ToString());
            }
        }

        [Test]
        public void MovementBindings_IncludeArrowKeysAndWsad()
        {
            AssertKeys(P0KeyboardInputMap.MovementLeftKeys, Key.LeftArrow, Key.A);
            AssertKeys(P0KeyboardInputMap.MovementRightKeys, Key.RightArrow, Key.D);
            AssertKeys(P0KeyboardInputMap.MovementDownKeys, Key.DownArrow, Key.S);
            AssertKeys(P0KeyboardInputMap.MovementUpKeys, Key.UpArrow, Key.W);
        }

        [Test]
        public void KeyboardCommandBindings_DoNotReuseMovementKeys()
        {
            HashSet<Key> movementKeys = new HashSet<Key>();
            AddKeys(movementKeys, P0KeyboardInputMap.MovementLeftKeys);
            AddKeys(movementKeys, P0KeyboardInputMap.MovementRightKeys);
            AddKeys(movementKeys, P0KeyboardInputMap.MovementDownKeys);
            AddKeys(movementKeys, P0KeyboardInputMap.MovementUpKeys);

            IReadOnlyList<P0InputBinding> bindings = P0KeyboardInputMap.AllBindings;
            for (int i = 0; i < bindings.Count; i++)
            {
                Assert.IsFalse(movementKeys.Contains(bindings[i].PrimaryKey), bindings[i].Command.ToString());
                if (bindings[i].HasSecondaryKey)
                {
                    Assert.IsFalse(movementKeys.Contains(bindings[i].SecondaryKey), bindings[i].Command.ToString());
                }
            }
        }

        [Test]
        public void DiagnosticsHudBinding_UsesDedicatedFunctionKey()
        {
            Assert.IsTrue(P0KeyboardInputMap.TryGetBinding(P0InputCommand.ToggleDiagnosticsHud, out P0InputBinding binding));
            Assert.AreEqual(Key.F10, binding.PrimaryKey);
            Assert.AreEqual("F10", binding.PrimaryKeyLabel);
        }

        private static void AssertKeys(IReadOnlyList<Key> actual, params Key[] expected)
        {
            Assert.AreEqual(expected.Length, actual.Count);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        private static void AddKeys(ISet<Key> target, IReadOnlyList<Key> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                target.Add(keys[i]);
            }
        }
    }
}
