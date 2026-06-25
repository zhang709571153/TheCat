using System;

namespace TheCat.Data
{
    public readonly struct StatusTagDefinition
    {
        public StatusTagDefinition(
            string id,
            string displayName,
            StatusTargetType targetType,
            float baseDurationSeconds,
            float magnitude,
            StatusStackPolicy stackPolicy,
            string visualToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Status tag id is required.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Display name is required.", nameof(displayName));
            }

            if (baseDurationSeconds < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(baseDurationSeconds), baseDurationSeconds, "Duration must not be negative.");
            }

            Id = id;
            DisplayName = displayName;
            TargetType = targetType;
            BaseDurationSeconds = baseDurationSeconds;
            Magnitude = magnitude;
            StackPolicy = stackPolicy;
            VisualToken = visualToken ?? string.Empty;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public StatusTargetType TargetType { get; }

        public float BaseDurationSeconds { get; }

        public float Magnitude { get; }

        public StatusStackPolicy StackPolicy { get; }

        public string VisualToken { get; }

        public bool IsP0Tag => StatusTagIds.IsP0Tag(Id);
    }
}
