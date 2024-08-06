using System.Linq;

namespace MarketGame
{
    public class GameObject
    {
        public Player Character;

        // These names are relative to the player
        public float SellModifier;
        public float BuyModifier;

        // Used to set extra modifiers
        public Merchandise BuyTip = Merchandise.NotDefined;
        public Merchandise SellTip = Merchandise.NotDefined;

        // This prevents a notdefined bug
        private const int RELEVANTMERCH = 5;

        // NOTE: Tried to set values on the perceptions of each drug. I have no clue how these will be balanced
        public static readonly Dictionary<Merchandise, (int, int)> HeatRespectValues = new()
        {
            { Merchandise.Downers, (10, 4) },
            { Merchandise.Weed, (5, 10) },
            { Merchandise.Acid, (10, 10) },
            { Merchandise.Ecstacy, (10, 15) },
            { Merchandise.Heroin, (25, 15) },
            { Merchandise.Coke, (10, 25) }
        };

        // Used in a few places
        public static readonly Dictionary<Merchandise, string> MerchandiseIcons = new()
        {
            {Merchandise.Downers, "pack://application:,,/MarketGame;component/Icons/Downers.png" },
            {Merchandise.Weed, "pack://application:,,/MarketGame;component/Icons/Weed.png" },
            {Merchandise.Acid, "pack://application:,,/MarketGame;component/Icons/Acid.png" },
            {Merchandise.Ecstacy, "pack://application:,,/MarketGame;component/Icons/Ecstacy.png" },
            {Merchandise.Heroin, "pack://application:,,/MarketGame;component/Icons/Heroin.png" },
            {Merchandise.Coke, "pack://application:,,/MarketGame;component/Icons/Coke.png" }
        };

        public GameObject()
        {
            Character = new Player();
        }

        public void GenerateTipOff(MainWindow window)
        {
            float modifier;
            // Decide if it will be buy or sell
            bool isSell = (modifier = (float)(new Random().NextDouble() * 2 - 1)) >= 0;

            SetTipCounters(modifier, isSell, GetRandomMerch());
            PushToNotification(window, isSell);
        }

        private static Merchandise GetRandomMerch() => (Merchandise)new Random().Next(RELEVANTMERCH);

        private void SetTipCounters(float modifier, bool isSell, Merchandise toTip)
        {
            SellTip = isSell ? toTip : Merchandise.NotDefined;
            BuyTip = isSell ? Merchandise.NotDefined : toTip;
            SellModifier = isSell ? modifier : default;
            BuyModifier = isSell ? default : modifier;
        }

        private void PushToNotification(MainWindow window, bool isSell)
        {
            Notification notification;
            if (isSell) notification = new(SellTip, isSell);
            else notification = new(BuyTip, isSell);
            window.TipNotification = notification;
        }
    }
}
