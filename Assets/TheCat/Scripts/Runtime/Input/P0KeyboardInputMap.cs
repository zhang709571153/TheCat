using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace TheCat.Inputs
{
    public static class P0KeyboardInputMap
    {
        private static readonly P0InputBinding[] Bindings =
        {
            new P0InputBinding(P0InputCommand.SelectCat1, Key.Digit1, "1", Key.Numpad1, "Num1"),
            new P0InputBinding(P0InputCommand.SelectCat2, Key.Digit2, "2", Key.Numpad2, "Num2"),
            new P0InputBinding(P0InputCommand.SelectCat3, Key.Digit3, "3", Key.Numpad3, "Num3"),
            new P0InputBinding(P0InputCommand.Skill1, Key.Q, "Q"),
            new P0InputBinding(P0InputCommand.Skill2, Key.R, "R"),
            new P0InputBinding(P0InputCommand.Skill3, Key.E, "E"),
            new P0InputBinding(P0InputCommand.TogglePause, Key.P, "P", Key.Escape, "Esc"),
            new P0InputBinding(P0InputCommand.SpeedHalf, Key.F1, "F1"),
            new P0InputBinding(P0InputCommand.SpeedNormal, Key.F2, "F2"),
            new P0InputBinding(P0InputCommand.SpeedFast, Key.F3, "F3"),
            new P0InputBinding(P0InputCommand.ToggleDiagnosticsHud, Key.F10, "F10"),
            new P0InputBinding(P0InputCommand.UseBedCare, Key.B, "B"),
            new P0InputBinding(P0InputCommand.UseLitterBox, Key.L, "L"),
            new P0InputBinding(P0InputCommand.UseFeeder, Key.F, "F"),
            new P0InputBinding(P0InputCommand.ContinueRoute, Key.Enter, "Enter", Key.NumpadEnter, "NumEnter"),
            new P0InputBinding(P0InputCommand.RestartRun, Key.N, "N")
        };

        private static readonly Key[] MoveLeftKeys = { Key.LeftArrow, Key.A };
        private static readonly Key[] MoveRightKeys = { Key.RightArrow, Key.D };
        private static readonly Key[] MoveDownKeys = { Key.DownArrow, Key.S };
        private static readonly Key[] MoveUpKeys = { Key.UpArrow, Key.W };

        public static IReadOnlyList<P0InputBinding> AllBindings => Bindings;

        public static IReadOnlyList<Key> MovementLeftKeys => MoveLeftKeys;

        public static IReadOnlyList<Key> MovementRightKeys => MoveRightKeys;

        public static IReadOnlyList<Key> MovementDownKeys => MoveDownKeys;

        public static IReadOnlyList<Key> MovementUpKeys => MoveUpKeys;

        public static bool TryGetBinding(P0InputCommand command, out P0InputBinding binding)
        {
            for (int i = 0; i < Bindings.Length; i++)
            {
                if (Bindings[i].Command == command)
                {
                    binding = Bindings[i];
                    return true;
                }
            }

            binding = default(P0InputBinding);
            return false;
        }

        public static bool WasPressedThisFrame(P0InputCommand command)
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || !TryGetBinding(command, out P0InputBinding binding))
            {
                return false;
            }

            return WasPressedThisFrame(keyboard, binding.PrimaryKey)
                || WasPressedThisFrame(keyboard, binding.SecondaryKey);
        }

        public static Vector2 ReadMovementAxis()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null)
            {
                return Vector2.zero;
            }

            Vector2 axis = Vector2.zero;
            if (IsAnyPressed(keyboard, MoveLeftKeys))
            {
                axis.x -= 1f;
            }

            if (IsAnyPressed(keyboard, MoveRightKeys))
            {
                axis.x += 1f;
            }

            if (IsAnyPressed(keyboard, MoveDownKeys))
            {
                axis.y -= 1f;
            }

            if (IsAnyPressed(keyboard, MoveUpKeys))
            {
                axis.y += 1f;
            }

            if (axis.sqrMagnitude > 1f)
            {
                axis.Normalize();
            }

            return axis;
        }

        private static bool WasPressedThisFrame(Keyboard keyboard, Key key)
        {
            if (key == Key.None)
            {
                return false;
            }

            KeyControl control = keyboard[key];
            return control != null && control.wasPressedThisFrame;
        }

        private static bool IsPressed(Keyboard keyboard, Key key)
        {
            KeyControl control = keyboard[key];
            return control != null && control.isPressed;
        }

        private static bool IsAnyPressed(Keyboard keyboard, IReadOnlyList<Key> keys)
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (IsPressed(keyboard, keys[i]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
