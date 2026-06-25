using System;
using UnityEngine.InputSystem;

namespace TheCat.Inputs
{
    public readonly struct P0InputBinding
    {
        public P0InputBinding(
            P0InputCommand command,
            Key primaryKey,
            string primaryKeyLabel,
            Key secondaryKey = Key.None,
            string secondaryKeyLabel = "")
        {
            if (primaryKey == Key.None)
            {
                throw new ArgumentException("A P0 input binding requires a primary key.", nameof(primaryKey));
            }

            if (string.IsNullOrWhiteSpace(primaryKeyLabel))
            {
                throw new ArgumentException("A P0 input binding requires a primary key label.", nameof(primaryKeyLabel));
            }

            Command = command;
            PrimaryKey = primaryKey;
            PrimaryKeyLabel = primaryKeyLabel;
            SecondaryKey = secondaryKey;
            SecondaryKeyLabel = secondaryKeyLabel ?? string.Empty;
        }

        public P0InputCommand Command { get; }

        public Key PrimaryKey { get; }

        public string PrimaryKeyLabel { get; }

        public Key SecondaryKey { get; }

        public string SecondaryKeyLabel { get; }

        public bool HasSecondaryKey => SecondaryKey != Key.None;
    }
}
