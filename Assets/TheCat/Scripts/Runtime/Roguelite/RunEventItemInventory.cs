using System;
using System.Collections.Generic;

namespace TheCat.Roguelite
{
    public sealed class RunEventItemInventory
    {
        public const string FadedFishBagId = "faded_fish_bag";
        public const string FoldedCouponId = "folded_coupon";
        public const string OldDreamMapId = "old_dream_map";
        public const string PawStampId = "paw_stamp";
        public const string BlankWishTagId = "blank_wish_tag";

        public const int FadedFishBagNextEventFishBonus = 1;
        public const int FoldedCouponFishDiscount = 1;

        private readonly Dictionary<string, int> counts = new Dictionary<string, int>();

        public void Add(string itemId, int amount = 1)
        {
            RequireKnownItem(itemId);
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must be greater than zero.");
            }

            counts[itemId] = Count(itemId) + amount;
        }

        public bool Has(string itemId)
        {
            return Count(itemId) > 0;
        }

        public int Count(string itemId)
        {
            RequireKnownItem(itemId);
            return counts.TryGetValue(itemId, out int count) ? count : 0;
        }

        public bool Consume(string itemId, int amount = 1)
        {
            RequireKnownItem(itemId);
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must be greater than zero.");
            }

            int current = Count(itemId);
            if (current < amount)
            {
                return false;
            }

            int remaining = current - amount;
            if (remaining == 0)
            {
                counts.Remove(itemId);
            }
            else
            {
                counts[itemId] = remaining;
            }

            return true;
        }

        public int PreviewFishTreatGain(int baseAmount)
        {
            RequireNonNegative(baseAmount, nameof(baseAmount));
            return baseAmount + (Has(FadedFishBagId) ? FadedFishBagNextEventFishBonus : 0);
        }

        public int PreviewFishTreatCost(int baseCost)
        {
            RequireNonNegative(baseCost, nameof(baseCost));
            if (!Has(FoldedCouponId) || baseCost == 0)
            {
                return baseCost;
            }

            return Math.Max(0, baseCost - FoldedCouponFishDiscount);
        }

        public void ConsumeFishGainBonusIfNeeded(int baseAmount)
        {
            RequireNonNegative(baseAmount, nameof(baseAmount));
            if (baseAmount > 0 && Has(FadedFishBagId))
            {
                Consume(FadedFishBagId);
            }
        }

        public void ConsumeShopDiscountIfNeeded(int baseCost)
        {
            RequireNonNegative(baseCost, nameof(baseCost));
            if (baseCost > 0 && Has(FoldedCouponId))
            {
                Consume(FoldedCouponId);
            }
        }

        public string BuildSummary()
        {
            List<string> entries = new List<string>();
            Append(entries, FadedFishBagId);
            Append(entries, FoldedCouponId);
            Append(entries, OldDreamMapId);
            Append(entries, PawStampId);
            Append(entries, BlankWishTagId);
            return entries.Count == 0 ? "无" : string.Join("，", entries);
        }

        public static string GetDisplayName(string itemId)
        {
            switch (itemId)
            {
                case FadedFishBagId:
                    return "褪色鱼干袋";
                case FoldedCouponId:
                    return "折角优惠券";
                case OldDreamMapId:
                    return "旧梦地图";
                case PawStampId:
                    return "猫爪印章";
                case BlankWishTagId:
                    return "空白许愿签";
                default:
                    return "未知事件道具";
            }
        }

        private void Append(List<string> entries, string itemId)
        {
            int count = Count(itemId);
            if (count <= 0)
            {
                return;
            }

            entries.Add(GetDisplayName(itemId) + " x" + count);
        }

        private static void RequireKnownItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                throw new ArgumentException("Item id is required.", nameof(itemId));
            }

            switch (itemId)
            {
                case FadedFishBagId:
                case FoldedCouponId:
                case OldDreamMapId:
                case PawStampId:
                case BlankWishTagId:
                    return;
                default:
                    throw new ArgumentException("Unknown P0 event item id.", nameof(itemId));
            }
        }

        private static void RequireNonNegative(int value, string name)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(name, value, "Value must not be negative.");
            }
        }
    }
}
