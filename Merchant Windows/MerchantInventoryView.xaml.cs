using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarketGame
{
    /// <summary>
    /// Interaction logic for MerchantInventoryView.xaml
    /// </summary>
    
    public partial class MerchantInventoryView : UserControl
    {
        public Merchant Dealer;
        private readonly string[] IntroMessages =
        [
            "Come on, buy already... I'm a little jumpy."
        ];
        
        // Aliases for readability
        private readonly MainWindow host = (MainWindow)Application.Current.MainWindow;
        private readonly Player Character;

        // TODO: This needs to be implemented
        private float GeneralModifier = 1;

        // This keeps track of the REAL (not displayed) total for transfer
        int TradeQuantity = 0;
        private readonly int[] RealQuantities = [0, 0, 0, 0, 0, 0];
        private bool isSelling = true;
        private Merchandise FeaturedMerch = Merchandise.NotDefined;

        private static readonly Dictionary<Merchandise, string> MerchandiseIcons = new()
        {
            {Merchandise.Downers, "pack://application:,,/MarketGame;component/Icons/Downers.png" },
            {Merchandise.Weed, "pack://application:,,/MarketGame;component/Icons/Weed.png" },
            {Merchandise.Acid, "pack://application:,,/MarketGame;component/Icons/Acid.png" },
            {Merchandise.Ecstacy, "pack://application:,,/MarketGame;component/Icons/Ecstacy.png" },
            {Merchandise.Heroin, "pack://application:,,/MarketGame;component/Icons/Heroin.png" },
            {Merchandise.Coke, "pack://application:,,/MarketGame;component/Icons/Coke.png" }
        };

        public MerchantInventoryView()
        {
            InitializeComponent();
            Dealer = new (Factions.NotDefined);
            this.Character = host.Game.Character;
            AssignLabelValues();
            SetDisplays();
            isSelling = true;
            SetBars();
        }
        
        private void SetDisplays()
        {
            QuantityMovedIndicator.Content = "0";
            DrugIndicatorLabel.Content = "Select Drug.";

            // TODO: This needs to be responsive to tipoffs
            RateLabel.Content = "x" + GeneralModifier.ToString();
            TypeLabel.Content = "Unselected.";
            RespectGainLabel.Content = "0";
            HeatGainLabel.Content = "0";
            MoneyChangeLabel.Content = "$0";
            FeaturedDrugImage.Opacity = 0;
        }

        private void SetBars()
        {
            BagCapacityBar.Value = (float)host.Game.Character.GetTotalCapacity(false) / 150 * 100;
            HeatBar.Value = host.Game.Character.Heat;
            RespectBar.Value = host.Game.Character.Respect;

            // Cash is not a bar, but it is affected by trades
            CashDisplayLabel.Content = "$" + host.Game.Character.DisplayCash;
        }

        public void AssignLabelValues()
        {
            Label[] labels =
            [
                DownersBagLabel, WeedBagLabel, AcidBagLabel,
                EcstacyBagLabel, HeroinBagLabel, CokeBagLabel,
                DownersDealerLabel, WeedDealerLabel, AcidDealerLabel,
                EcstacyDealerLabel, HeroinDealerLabel, CokeDealerLabel
            ];

            for (int i = 0; i < labels.Length; i++)
            {
                if (i < 6)
                {
                    labels[i].Content = host.Game.Character.Bag.ElementAt(i).Value.ToString();
                    continue;
                }
                labels[i].Content = Dealer.DealerMerchandise.ElementAt(i%6).Value.ToString();
            }

        }

        private void ConfirmDeal(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = [];
            if (isSelling)
            {
                host.Game.Character.Sell(TradeQuantity, FeaturedMerch, GeneralModifier);
                Dealer.Buy(TradeQuantity, FeaturedMerch);
                TradeQuantity = 0;

                // Trade effects
                Character.Heat += GameObject.HeatRespectValues[FeaturedMerch].Item1;
                Character.Respect += GameObject.HeatRespectValues[FeaturedMerch].Item2;

                AssignLabelValues();
                SetBars();
                QuantityMovedIndicator.Content = "0";
                GeneralModifier = 1;
            }
            else
            {
                if (CheckIfPurchaseValid())
                {
                    host.Game.Character.Buy(TradeQuantity, FeaturedMerch, GeneralModifier);
                    Dealer.Sell(TradeQuantity, FeaturedMerch);
                    TradeQuantity = 0;

                    // Trade effects
                    Character.Heat += GameObject.HeatRespectValues[FeaturedMerch].Item1;
                    Character.Respect += GameObject.HeatRespectValues[FeaturedMerch].Item2;

                    AssignLabelValues();
                    SetBars();
                    QuantityMovedIndicator.Content = "0";
                    host.UpdateStatusIndicators(); return;
                }
            }
            host.UpdateStatusIndicators();
        }

        private bool CheckIfPurchaseValid()
        {
            // Check if bag capacity
            if (Character.Bag.Values.Sum() + TradeQuantity > Character.BagCapacity)
            {
                return false;
            }
            // Check if broke
            if (Int32.Parse(Character.DisplayCash) < TradeQuantity * Player.BASEPRICES[FeaturedMerch] * GeneralModifier)
            {
                return false;
            }
            // Check if dealer has enough
            if (Dealer.DealerMerchandise[FeaturedMerch] - TradeQuantity < 0 || Dealer.DealerMerchandise[FeaturedMerch] == 0) { return false; }
            return true;
        }
        

        public void SetUpInventory()
        {
            // Should never reach this point
            if (Dealer.Faction == Factions.NotDefined) return;
            SetIcon();
        }

        private void SetIcon()
        {
            // TODO: Add more messages
            // Set message from message set
            Random random = new();
            int message = random.Next(IntroMessages.Length);
            MerchantSpeech.Content = IntroMessages[message];

            // Set image from dictionary
            MerchantIcon.Source = new BitmapImage(new Uri(Dealer.FactionIcons[Dealer.Faction]));
            RespectGainLabel.Content = Dealer.Faction.ToString();
        }

        public void SetMerchant(Merchant dealer)
        {
            Dealer = dealer;
            SetUpInventory();
        }

        #region Button Methods
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // HACK: Try and return user to merchant select while resetting everything
            BuySellWindow ParentWindow = (BuySellWindow)Window.GetWindow(this);
            ParentWindow.Close();
        }

        private void SellButtonClick(object sender, RoutedEventArgs e)
        {
            // Micro-optimisation
            if (isSelling) return;

            isSelling = true;
            BuySellIndicator.Content = "SELLING";
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isSelling) return;

            isSelling = false;
            BuySellIndicator.Content = "BUYING";
        }

        private void UpArrowClick(object sender, RoutedEventArgs e)
        {
            if (FeaturedMerch == Merchandise.NotDefined) return;

            TradeQuantity++;
            QuantityMovedIndicator.Content = TradeQuantity.ToString();

            int potential = (int)(TradeQuantity * Player.BASEPRICES[FeaturedMerch] * GeneralModifier);
            MoneyChangeLabel.Content = "$" + potential;
        }

        private void DownArrowClick(object sender, RoutedEventArgs e) 
        {
            if (TradeQuantity <= 0) return;
            if (FeaturedMerch == Merchandise.NotDefined) return;

            TradeQuantity--;
            QuantityMovedIndicator.Content = TradeQuantity.ToString();

            int potential = (int)(TradeQuantity * Player.BASEPRICES[FeaturedMerch] * GeneralModifier);
            MoneyChangeLabel.Content = "$" + potential;
        }

        private void DrugSelectClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string buttonName = clickedButton.Name;
                Dictionary<string, Merchandise> pairs = new()
                {
                    {"DownersButton", Merchandise.Downers },
                    {"WeedButton", Merchandise.Weed },
                    {"AcidButton", Merchandise.Acid },
                    {"EcstacyButton", Merchandise.Ecstacy },
                    {"HeroinButton", Merchandise.Heroin },
                    {"CokeButton", Merchandise.Coke }
                };

                Dictionary<Merchandise, string> typeDescriptors = new()
                {
                    {Merchandise.Downers, "Depressants" },
                    {Merchandise.Weed, "Depressants" },
                    {Merchandise.Acid, "Hallucinogens" },
                    {Merchandise.Ecstacy, "Hallucinogens" },
                    {Merchandise.Coke, "Powders" },
                    {Merchandise.Heroin, "Powders" }
                };

                FeaturedMerch = pairs[buttonName];

                FeaturedDrugImage.Source = new BitmapImage(new Uri(MerchandiseIcons[pairs[buttonName]]));
                FeaturedDrugImage.Opacity = 1.0;

                DrugIndicatorLabel.Content = FeaturedMerch.ToString();

                HeatGainLabel.Content = GameObject.HeatRespectValues[FeaturedMerch].Item1;
                RespectGainLabel.Content = GameObject.HeatRespectValues[FeaturedMerch].Item2;

                TypeLabel.Content = typeDescriptors[FeaturedMerch];

                // TODO: Better way to express this
                if (FeaturedMerch == Dealer.PrefferedSell) RateLabel.Content = "x1.25 (s)";
                else if (FeaturedMerch == Dealer.PrefferedBuy) RateLabel.Content = "x1.25 (b)";
                else RateLabel.Content = "x1.00";
            }
        }
        #endregion
    }
}
