using System;

namespace TheCat.Roguelite
{
    public sealed class AuthorityBlessingDefinition
    {
        public AuthorityBlessingDefinition(
            string id,
            string ownerCatId,
            string authorityId,
            string displayName,
            string description)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Blessing id is required.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(ownerCatId))
            {
                throw new ArgumentException("Owner cat id is required.", nameof(ownerCatId));
            }

            if (string.IsNullOrWhiteSpace(authorityId))
            {
                throw new ArgumentException("Authority id is required.", nameof(authorityId));
            }

            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Display name is required.", nameof(displayName));
            }

            Id = id;
            OwnerCatId = ownerCatId;
            AuthorityId = authorityId;
            DisplayName = displayName;
            Description = description ?? string.Empty;
        }

        public string Id { get; }

        public string OwnerCatId { get; }

        public string AuthorityId { get; }

        public string DisplayName { get; }

        public string Description { get; }
    }
}
