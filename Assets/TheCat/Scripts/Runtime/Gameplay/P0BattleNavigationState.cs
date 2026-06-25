using System;
using UnityEngine;

namespace TheCat.Gameplay
{
    public sealed class P0BattleNavigationState
    {
        public const float BaseMoveSpeed = 3.5f;
        public const float DefaultInteractionRange = 1.15f;
        public const float DefaultBedCareRange = 1.75f;

        public static readonly Vector2 DefaultStartPosition = new Vector2(0f, -2.6f);
        public static readonly Vector2 DefaultArenaMin = new Vector2(-4.4f, -3.85f);
        public static readonly Vector2 DefaultArenaMax = new Vector2(4.4f, 4.85f);

        private readonly Vector2 arenaMin;
        private readonly Vector2 arenaMax;

        public P0BattleNavigationState()
            : this(DefaultStartPosition, DefaultArenaMin, DefaultArenaMax)
        {
        }

        public P0BattleNavigationState(Vector2 startPosition, Vector2 arenaMin, Vector2 arenaMax)
        {
            if (arenaMin.x >= arenaMax.x || arenaMin.y >= arenaMax.y)
            {
                throw new ArgumentException("Arena min must be lower than arena max.");
            }

            this.arenaMin = arenaMin;
            this.arenaMax = arenaMax;
            Position = Clamp(startPosition);
        }

        public Vector2 Position { get; private set; }

        public void Reset()
        {
            Position = Clamp(DefaultStartPosition);
        }

        public void ResetTo(Vector2 position)
        {
            Position = Clamp(position);
        }

        public Vector2 Move(Vector2 input, float deltaSeconds, float moveSpeedMultiplier)
        {
            if (deltaSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(deltaSeconds), deltaSeconds, "Delta must not be negative.");
            }

            if (moveSpeedMultiplier < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(moveSpeedMultiplier), moveSpeedMultiplier, "Speed multiplier must not be negative.");
            }

            if (input.sqrMagnitude > 1f)
            {
                input.Normalize();
            }

            Position = Clamp(Position + input * BaseMoveSpeed * moveSpeedMultiplier * deltaSeconds);
            return Position;
        }

        public float GetDistanceTo(Vector2 targetPosition)
        {
            return Vector2.Distance(Position, targetPosition);
        }

        public bool IsWithinRange(Vector2 targetPosition, float range)
        {
            if (range < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(range), range, "Range must not be negative.");
            }

            return GetDistanceTo(targetPosition) <= range;
        }

        public string BuildDistanceSummary(Vector2 bedPosition, Vector2 litterBoxPosition, Vector2 feederPosition)
        {
            return "位置 "
                + Position.x.ToString("0.0")
                + ","
                + Position.y.ToString("0.0")
                + "  床 "
                + GetDistanceTo(bedPosition).ToString("0.0")
                + "  猫砂盆 "
                + GetDistanceTo(litterBoxPosition).ToString("0.0")
                + "  喂食器 "
                + GetDistanceTo(feederPosition).ToString("0.0");
        }

        private Vector2 Clamp(Vector2 value)
        {
            return new Vector2(
                Mathf.Clamp(value.x, arenaMin.x, arenaMax.x),
                Mathf.Clamp(value.y, arenaMin.y, arenaMax.y));
        }
    }
}
