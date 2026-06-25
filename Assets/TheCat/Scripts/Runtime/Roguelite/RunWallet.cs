using System;

namespace TheCat.Roguelite
{
    public sealed class RunWallet
    {
        public int DreamShards { get; private set; }

        public int FishTreats { get; private set; }

        public void AddDreamShards(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must not be negative.");
            }

            DreamShards += amount;
        }

        public void AddFishTreats(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must not be negative.");
            }

            FishTreats += amount;
        }

        public bool SpendDreamShards(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must not be negative.");
            }

            if (DreamShards < amount)
            {
                return false;
            }

            DreamShards -= amount;
            return true;
        }

        public bool SpendFishTreats(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Amount must not be negative.");
            }

            if (FishTreats < amount)
            {
                return false;
            }

            FishTreats -= amount;
            return true;
        }
    }
}
