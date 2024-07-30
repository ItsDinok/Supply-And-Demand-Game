using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Media;

namespace MarketGame
{
    #region Enum Declarations
    public enum Merchandise
    {
        Downers,
        Weed,
        Acid,
        Ecstacy,
        Heroin,
        Coke,

        NotDefined
    }

    public enum Factions
    {
        Triad,
        Mob,
        Yardies,
        Russians,
        Syndicate,
        Bikers,

        NotDefined
    }
    #endregion

    public class Player
    {
        // Inventory
        public Dictionary<Merchandise, int> Bag;
        public Dictionary<Merchandise, int> Stash;

        // Character metrics
        public int Respect = 30;
        public int Heat = 40;
        public int BagCapacity = 150;
        public int StashCapacity = 1500;
        private int Cash = 2000;
        private int Money = 500;

        // Display stats
        public string DisplayCash;
        public string DisplayMoney;
     
        // Base values of merchandise
        public static readonly Dictionary<Merchandise, int> BASEPRICES = new() {
            {Merchandise.Downers, 20 }, {Merchandise.Weed, 40 },
            {Merchandise.Acid, 250 }, {Merchandise.Ecstacy, 400 },
            {Merchandise.Heroin, 750 }, {Merchandise.Coke, 900 }
        };

        // Methods
        public Player() 
        {
            // Used for labels
            DisplayMoney = Money.ToString();
            DisplayCash = Cash.ToString();

            // Set inventories up
            Bag = new() {
                {Merchandise.Downers, 10 },
                {Merchandise.Weed, 40 },
                {Merchandise.Acid, 10 },
                {Merchandise.Ecstacy, 20 },
                {Merchandise.Heroin, 20 },
                {Merchandise.Coke, 30 }

            };
            Stash = new() {
                {Merchandise.Downers, 100 },
                {Merchandise.Weed, 200 },
                {Merchandise.Acid, 100 },
                {Merchandise.Ecstacy, 50 },
                {Merchandise.Heroin, 0 },
                {Merchandise.Coke, 0 }

            };
        }

        public int GetCash() { return Cash; }
        public int GetMoney() { return Money; } 

        public int GetTotalCapacity(bool isStash)
        {
            int total = 0;
            // Gets total amount of items in stash
            if (isStash)
            {
                foreach(var pair in Stash)
                {
                    total += pair.Value;
                }
            }
            // Gets total amount of items in bag
            else
            {
                foreach(var pair in Bag)
                {
                    total += pair.Value;
                }
            }

            return total;
        }

        // Consider making public for other functions
        private int CalculateTotalInBag()
        {
            return Bag.Values.Sum();
        }
        // Consider making public for other functions
        private int CalculateTotalInStash()
        {
            return Stash.Values.Sum();
        }

        public void MoveToBag(int amount, Merchandise type)
        {
            // Validity checks
            if (Stash[type] - amount < 0) return;
            if (CalculateTotalInBag() + amount > BagCapacity) return;

            // Move items
            Stash[type] -= amount;
            Bag[type] += amount;
        }

        public void MoveToStash(int amount, Merchandise type) 
        {
            // Validity checks
            if (Bag[type] - amount < 0) return;
            if (CalculateTotalInStash() + amount > StashCapacity) return;

            // Move items
            Stash[type] += amount;
            Bag[type] -= amount;
        }

        // Transactions can only be made with stuff on hand
        public void Sell(int amount, Merchandise type, float modifier) 
        {
            // Calculate value per unit
            int cost = (int)(BASEPRICES[type] * modifier);

            // Validity check
            if (Bag[type] - amount < 0) return;

            // Move items and money
            Bag[type] -= amount;
            Cash += (int)(amount * cost);

            // Change amount to be displayed
            DisplayCash = Cash.ToString();
        }
        public void Buy(int amount, Merchandise type, float modifier) 
        {

            // Calculate value per unit
            int cost = (int)(BASEPRICES[type] * modifier);

            // Validity check
            if (Cash - cost*amount < 0) return;

            // Move items and money
            Cash -= (amount * cost);
            Bag[type] += amount;

            // Change amount to be displayed
            DisplayCash = Cash.ToString();
        }

        public void CashConvert(int amount, bool toCash)
        {
            // Prevents negative numbers
            if (amount <= 0) return;

            if (toCash)
            {
                // Validity check
                if (Money - amount < 0) return;

                // Move money and change displays
                Cash += amount;
                Money -= amount;

                DisplayCash = Cash.ToString();
                DisplayMoney = Money.ToString();
            }
            else
            {
                // Validity check
                if (Cash - amount < 0) return;

                // Move money and change displays
                Money += amount;
                Cash -= amount;

                DisplayCash = Cash.ToString();
                DisplayMoney = Money.ToString();
            }
        }
    }

    public class Merchant
    {
        // Attributes
        public Factions Faction;
        public Merchandise PrefferedBuy;
        public Merchandise PrefferedSell;

        // Inventory
        public Dictionary<Merchandise, int> DealerMerchandise = new()
        {
            {Merchandise.Downers, 0 }, {Merchandise.Weed, 0 },
            {Merchandise.Acid, 0 }, {Merchandise.Ecstacy, 0 },
            {Merchandise.Heroin, 0 },{Merchandise.Coke, 0 },
        };

        // Used to get modifiers for buy/sell
        private readonly static Dictionary<Factions, (Merchandise, Merchandise)> Preferences = new()
        {
            { Factions.Triad, (Merchandise.Ecstacy, Merchandise.Acid) },
            { Factions.Mob, (Merchandise.Heroin, Merchandise.Coke) },
            { Factions.Yardies, (Merchandise.Weed, Merchandise.Downers) },
            { Factions.Russians, (Merchandise.Acid, Merchandise.Ecstacy) },
            { Factions.Syndicate, (Merchandise.Downers, Merchandise.Weed) },
            { Factions.Bikers, (Merchandise.Coke, Merchandise.Heroin) }
        };

        // Used to set icon in buy/sell
        public readonly Dictionary<Factions, string> FactionIcons = new()
        {
            {Factions.Yardies, "pack://application:,,,/MarketGame;component/Icons/Yardies.png" },
            {Factions.Triad, "pack://application:,,,/MarketGame;component/Icons/Triad.png" },
            {Factions.Mob, "pack://application:,,,/MarketGame;component/Icons/Mob.png" },
            {Factions.Syndicate, "pack://application:,,,/MarketGame;component/Icons/Syndicate.png" },
            {Factions.Russians, "pack://application:,,,/MarketGame;component/Icons/Russian.png" },
            {Factions.Bikers, "pack://application:,,,/MarketGame;component/Icons/Biker.png" }
        };

        public Merchant(Factions faction)
        {
            Faction = faction;
            
            GenerateMerchandise();
        }

        public void SetFaction(Factions faction)
        {
            Faction = faction;
            GenerateMerchandise();
            PrefferedBuy = Preferences[Faction].Item1;
            PrefferedSell = Preferences[Faction].Item2;
        }

        public void Sell(int amount, Merchandise type)
        {
            // Mainly validity checks
            if (DealerMerchandise == null) return;
            if (DealerMerchandise[type] - amount < 0) return;
            DealerMerchandise[type] -= amount;
        }
        public void Buy(int amount, Merchandise type)
        {
            if (DealerMerchandise == null) return;
            DealerMerchandise[type] += amount;
        }

        private void GenerateMerchandise()
        {
            Random random = new();
            // Internal function to set the values
            void SetMerchandise(int weed, int downers, int acid, int ecstacy, int heroin, int coke)
            {
                DealerMerchandise[Merchandise.Weed] = weed;
                DealerMerchandise[Merchandise.Downers] = downers;
                DealerMerchandise[Merchandise.Ecstacy] = ecstacy;
                DealerMerchandise[Merchandise.Acid] = acid;
                DealerMerchandise[Merchandise.Heroin] = heroin;
                DealerMerchandise[Merchandise.Coke] = coke;
            }

            // Very verbose. At some point change the probability distribution
            switch (Faction)
            {
                //                 Downers            Weed               Ecstacy              Acid                  Heroin            Coke
                case Factions.Mob:
                    SetMerchandise(0, 0, random.Next(0, 5), random.Next(0, 5), random.Next(20, 35), random.Next(10, 20)); return;
                case Factions.Bikers:
                    SetMerchandise(0, 0, random.Next(0, 5), random.Next(0, 5), random.Next(10, 20), random.Next(20, 35)); return;
                case Factions.Triad:
                    SetMerchandise(random.Next(0, 5), random.Next(0, 5), random.Next(20, 35), random.Next(10, 20), random.Next(0, 5), random.Next(0, 5)); return;
                case Factions.Russians:
                    SetMerchandise(random.Next(0, 5), random.Next(0, 5), random.Next(10, 20), random.Next(20, 35), random.Next(0, 5), random.Next(0, 5)); return;
                case Factions.Yardies:
                    SetMerchandise(random.Next(20, 35), random.Next(10, 20), random.Next(0, 5), random.Next(0, 5), 0, 0); return;
                case Factions.Syndicate:
                    SetMerchandise(random.Next(10, 20), random.Next(20, 35), random.Next(0, 5), random.Next(0, 5), 0, 0); return;
                // It should never reach this point
                default: return;
            };
        }

    }
}
