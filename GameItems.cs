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
        Coke
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

    // TODO : Implement this, it is 100% better
    public class Inventory
    {
        int DownersQuantity;
        int WeedQuantity;
        int AcidQuantity;
        int EcstacyQuantity;
        int HeroinQuantity;
        int CokeQuantity;

        public Inventory(int dQ, int wQ, int aQ, int eQ, int hQ, int cQ)
        {
            DownersQuantity = dQ; WeedQuantity = wQ;
            AcidQuantity = aQ; EcstacyQuantity = eQ;
            HeroinQuantity = hQ; CokeQuantity = cQ;
        }
    }

    public class Player
    {
        // Attributes
        public Dictionary<Merchandise, int> Bag;
        public Dictionary<Merchandise, int> Stash;
        // TODO: Sort these out
        public int Respect = 30;
        public int Heat = 40;
        public string DisplayCash;
        public string DisplayMoney;
        // TODO: implement the logic for this
        public int BagCapacity = 150;
        public int StashCapacity = 1500;

        private int Cash = 2000;
        private int Money = 500;
        
        public readonly Dictionary<Merchandise, int> BASEPRICES = new() {
            {Merchandise.Downers, 20 }, {Merchandise.Weed, 40 },
            {Merchandise.Acid, 250 }, {Merchandise.Ecstacy, 400 },
            {Merchandise.Heroin, 750 }, {Merchandise.Coke, 900 }
        };

        // Methods
        public Player() 
        {
            DisplayMoney = Money.ToString();
            DisplayCash = Cash.ToString();

            // TODO: find a better way to do this
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

            // :)
            //Stash = Bag;
        }

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

        public void MoveToBag(int amount, Merchandise type)
        {
            if (Stash[type] - amount < 0) return;

            Stash[type] -= amount;
            Bag[type] += amount;
        }

        public void MoveToStash(int amount, Merchandise type) 
        {
            if (Bag[type] - amount < 0) return;

            Stash[type] += amount;
            Bag[type] -= amount;
        }

        // Transactions can only be made with stuff on hand
        public void Sell(int amount, Merchandise type, float modifier) 
        {
            int cost = (int)(BASEPRICES[type] * modifier);

            if (Bag[type] - amount < 0) return;

            Bag[type] -= amount;
            Cash += (int)(amount * cost);

            DisplayCash = Cash.ToString();
        }
        public void Buy(int amount, Merchandise type, float modifier) 
        {
            int cost = (int)(BASEPRICES[type] * modifier);

            if (Cash - cost*amount < 0) return;

            Cash -= (amount * cost);
            Bag[type] += amount;

            DisplayCash = Cash.ToString();
        }

        public void CashConvert(int amount, bool toCash)
        {
            // Prevents negative numbers
            if (amount <= 0) return;

            if (toCash)
            {
                if (Money - amount < 0) return;

                Cash += amount;
                Money -= amount;

                DisplayCash = Cash.ToString();
                DisplayMoney = Money.ToString();
            }
            else
            {
                if (Cash - amount < 0) return;

                Money += amount;
                Cash -= amount;

                DisplayCash = Cash.ToString();
                DisplayMoney = Money.ToString();
            }
        }
    }

    public class GameObject
    {
        public Player Character;

        // These names are relative to the player
        public float SellModifier;
        public float BuyModifier;

        public GameObject() 
        {
            Character = new Player();
        }

        private static float GenerateTipOff(bool isSell)
        {
            Random random = new();
            return (isSell) ? (float)(random.NextDouble() * (0.8 - 0.5) + 0.5) : (float)(random.NextDouble() * (3 - 1.5) + 1.5) ;
        }
    }

    public class Merchant
    {
        public Factions Faction;
        Dictionary<Merchandise, int> DealerMerchandise;

        public Merchant(Factions faction)
        {
            Faction = faction;
            DealerMerchandise = new(){
                {Merchandise.Downers, 0 }, {Merchandise.Weed, 0 },
                {Merchandise.Acid, 0 }, {Merchandise.Ecstacy, 0 },
                {Merchandise.Heroin, 0 },{Merchandise.Coke, 0 },
            };
            GenerateMerchandise();
        }

        public void SetFaction(Factions faction)
        {
            Faction = faction;
            GenerateMerchandise();
        }

        public void Sell(int amount, Merchandise type)
        {
            if (DealerMerchandise == null) return;
            if (DealerMerchandise[type] - amount < 0) return;
            DealerMerchandise[type] -= amount;
        }
        public void Buy(int amount, Merchandise type)
        {
            if (DealerMerchandise == null) return;
            DealerMerchandise[type] += amount;
        }

        public Dictionary<Merchandise, int> GetInventory() {
            return DealerMerchandise;
        }

        private void GenerateMerchandise()
        {
            Random random = new();

            void SetMerchandise(int weed, int downers, int acid, int ecstacy, int heroin, int coke)
            {
                DealerMerchandise[Merchandise.Weed] = weed;
                DealerMerchandise[Merchandise.Downers] = downers;
                DealerMerchandise[Merchandise.Ecstacy] = ecstacy;
                DealerMerchandise[Merchandise.Acid] = acid;
                DealerMerchandise[Merchandise.Heroin] = heroin;
                DealerMerchandise[Merchandise.Coke] = coke;
            }

            switch (Faction)
            {
                // TODO: Change probability distribution
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
