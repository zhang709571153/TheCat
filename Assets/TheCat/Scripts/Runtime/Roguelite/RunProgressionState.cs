using System;
using System.Collections.Generic;
using TheCat.Data;

namespace TheCat.Roguelite
{
    public sealed class RunProgressionState
    {
        public RunProgressionState(RouteDefinition route, IEnumerable<string> starterCatIds)
        {
            Route = new RunRouteState(route ?? throw new ArgumentNullException(nameof(route)));
            Wallet = new RunWallet();
            Blessings = new RunBlessingInventory();
            EventItems = new RunEventItemInventory();
            Roster = new RunPartnerRoster();
            Metrics = new RunMetrics();
            CoreValues = new RunCoreValues();
            CatVitals = new RunCatVitals();
            PendingBattleModifiers = new RunPendingBattleModifiers();
            CatUpgrades = new RunCatUpgradeState();

            if (starterCatIds == null)
            {
                return;
            }

            foreach (string catId in starterCatIds)
            {
                Roster.AddCat(catId);
            }
        }

        public RunRouteState Route { get; }

        public DreamMapDefinition DreamMap => Route.Route.DreamMap;

        public RunWallet Wallet { get; }

        public RunBlessingInventory Blessings { get; }

        public RunEventItemInventory EventItems { get; }

        public RunPartnerRoster Roster { get; }

        public RunMetrics Metrics { get; }

        public RunCoreValues CoreValues { get; }

        public RunCatVitals CatVitals { get; }

        public RunPendingBattleModifiers PendingBattleModifiers { get; }

        public RunCatUpgradeState CatUpgrades { get; }

        public int DreamEventsResolved { get; private set; }

        public int ShopPurchases { get; private set; }

        public int RestNestUses { get; private set; }

        public int PlaceholderRewardsResolved { get; private set; }

        public void RecordDreamEvent()
        {
            DreamEventsResolved++;
            PlaceholderRewardsResolved++;
        }

        public void RecordShopPurchase()
        {
            ShopPurchases++;
            PlaceholderRewardsResolved++;
        }

        public void RecordRestNestUse()
        {
            RestNestUses++;
            PlaceholderRewardsResolved++;
        }

        public void RecordPlaceholderReward()
        {
            PlaceholderRewardsResolved++;
        }
    }
}
