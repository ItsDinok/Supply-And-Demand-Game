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
            Random random = new();

            // Decide if it will be buy or sell
            float modifier = (float)(random.NextDouble() * 2 - 1);
            bool isSell = modifier >= 0;

            // Decide on merch
            int merchIndex = random.Next(Enum.GetValues(typeof(Merchandise)).Length);

            if (isSell)
            {
                SellTip = (Merchandise)merchIndex;
                SellModifier = modifier;
                BuyTip = Merchandise.NotDefined;
            }
            else
            {
                BuyTip = (Merchandise)merchIndex;
                BuyModifier = modifier;
                SellTip = Merchandise.NotDefined;
            }

            PushToNotification(window, isSell);
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
