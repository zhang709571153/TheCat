using System.Collections.Generic;
using TheCat.Combat;
using TheCat.Data.Catalogs;
using TheCat.Data.Definitions;

namespace TheCat.Roguelite
{
    public static class P0BlessingCatalog
    {
        public const string SaibanBedlineId = "authority_oath_bedline";
        public const string NephthysSandglassId = "authority_dominion_sandglass";
        public const string SuzuneLullabyId = "authority_rhythm_lullaby";

        public static IReadOnlyList<AuthorityBlessingDefinition> CreateAuthorityBlessings()
        {
            return new[]
            {
                new AuthorityBlessingDefinition(
                    SaibanBedlineId,
                    P0PrototypeCatalog.SaibanId,
                    AuthorityIds.Oath,
                    "誓约床线",
                    "塞班的誓约强化床边护盾与击退。"),
                new AuthorityBlessingDefinition(
                    NephthysSandglassId,
                    P0PrototypeCatalog.NephthysId,
                    AuthorityIds.Dominion,
                    "月砂支配",
                    "奈芙蒂斯延长对梦境入侵者的缓速与标记控制。"),
                new AuthorityBlessingDefinition(
                    SuzuneLullabyId,
                    P0PrototypeCatalog.SuzuneId,
                    AuthorityIds.Rhythm,
                    "摇篮曲韵律",
                    "铃音提升安眠回复与治疗安全性。")
            };
        }

        public static BattleModifierSet CreateBattleModifiers(RunBlessingInventory inventory)
        {
            if (inventory == null || inventory.Count == 0)
            {
                return BattleModifierSet.Neutral;
            }

            float shieldMultiplier = 1f;
            float knockbackMultiplier = 1f;
            float enemyStatusDurationMultiplier = 1f;
            float ownerSleepRestoreMultiplier = 1f;
            float catHealMultiplier = 1f;

            IReadOnlyList<AuthorityBlessingDefinition> blessings = inventory.ActiveBlessings;
            for (int i = 0; i < blessings.Count; i++)
            {
                switch (blessings[i].Id)
                {
                    case SaibanBedlineId:
                        shieldMultiplier *= ScaleMultiplier(1.25f, inventory.GetLevel(blessings[i].Id));
                        knockbackMultiplier *= ScaleMultiplier(1.15f, inventory.GetLevel(blessings[i].Id));
                        break;
                    case NephthysSandglassId:
                        enemyStatusDurationMultiplier *= ScaleMultiplier(1.25f, inventory.GetLevel(blessings[i].Id));
                        break;
                    case SuzuneLullabyId:
                        ownerSleepRestoreMultiplier *= ScaleMultiplier(1.2f, inventory.GetLevel(blessings[i].Id));
                        catHealMultiplier *= ScaleMultiplier(1.2f, inventory.GetLevel(blessings[i].Id));
                        break;
                }
            }

            return new BattleModifierSet(
                shieldMultiplier: shieldMultiplier,
                knockbackMultiplier: knockbackMultiplier,
                enemyStatusDurationMultiplier: enemyStatusDurationMultiplier,
                ownerSleepRestoreMultiplier: ownerSleepRestoreMultiplier,
                catHealMultiplier: catHealMultiplier);
        }

        private static float ScaleMultiplier(float levelOneMultiplier, int level)
        {
            int effectiveLevel = level <= 0 ? 1 : level;
            return 1f + (levelOneMultiplier - 1f) * effectiveLevel;
        }

        public static AuthorityBlessingDefinition GetFirstMissing(RunBlessingInventory inventory)
        {
            IReadOnlyList<AuthorityBlessingDefinition> blessings = CreateAuthorityBlessings();
            for (int i = 0; i < blessings.Count; i++)
            {
                if (inventory == null || !inventory.Has(blessings[i].Id))
                {
                    return blessings[i];
                }
            }

            return null;
        }

        public static string GetAuthorityBlessingDisplayName(string blessingId)
        {
            if (string.IsNullOrWhiteSpace(blessingId))
            {
                return "未知祝福";
            }

            IReadOnlyList<AuthorityBlessingDefinition> blessings = CreateAuthorityBlessings();
            for (int i = 0; i < blessings.Count; i++)
            {
                if (blessings[i].Id == blessingId)
                {
                    return blessings[i].DisplayName;
                }
            }

            return "未知祝福";
        }
    }
}
