using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Controls;
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

        private const float GeneralModifier = 1;

        // This keeps track of the REAL (not displayed) total for transfer
        int TradeQuantity = 0;
        private bool isSelling = true;
        private Merchandise FeaturedMerch = Merchandise.NotDefined;


        private readonly Dictionary<string, Merchandise> Pairs = new()
        {
            {"DownersButton", Merchandise.Downers },
            {"WeedButton", Merchandise.Weed },
            {"AcidButton", Merchandise.Acid },
            {"EcstacyButton", Merchandise.Ecstacy },
            {"HeroinButton", Merchandise.Heroin },
            {"CokeButton", Merchandise.Coke }
        };

        private readonly Dictionary<Merchandise, string> TypeDescriptors = new()
        {
            {Merchandise.Downers, "Depressants" },
            {Merchandise.Weed, "Depressants" },
            {Merchandise.Acid, "Hallucinogens" },
            {Merchandise.Ecstacy, "Hallucinogens" },
            {Merchandise.Coke, "Powders" },
            {Merchandise.Heroin, "Powders" }
        };

        public MerchantInventoryView()
        {
            InitializeComponent();
            Dealer = new(Factions.NotDefined);
            this.Character = host.Game.Character;
            AssignLabelValues();
            isSelling = true;
            SetBars();
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
                    labels[i].Content = Character.Bag.ElementAt(i).Value.ToString();
                    continue;
                }
                labels[i].Content = Dealer.DealerMerchandise.ElementAt(i % 6).Value.ToString();
            }

        }

        private void ConfirmDeal(object sender, RoutedEventArgs e)
        {
            if (isSelling) HandleSellTransaction();
            else HandleBuyTransaction();

            // Effects of trade
            ResetAfterSale();
            AssignLabelValues();
            SetBars();
            host.RefreshUI();
        }

        private void HandleBuyTransaction()
        {
            if (!CheckIfPurchaseValid()) return;
            Character.Buy(TradeQuantity, FeaturedMerch, CalculateRate(FeaturedMerch, false));
            Dealer.Sell(TradeQuantity, FeaturedMerch);
        }

        private void HandleSellTransaction()
        {
            Character.Sell(TradeQuantity, FeaturedMerch, CalculateRate(FeaturedMerch, true));
            Dealer.Buy(TradeQuantity, FeaturedMerch);
        }

        private void ResetAfterSale()
        {
            QuantityMovedIndicator.Content = 0;
            TradeQuantity = 0;

            UpdateHeatAndRespectAfterSale();
        }

        private void UpdateHeatAndRespectAfterSale()
        {
            Character.Heat += GameObject.HeatRespectValues[FeaturedMerch].Item1;
            Character.Respect += GameObject.HeatRespectValues[FeaturedMerch].Item2;
        }

        private bool CheckIfPurchaseValid()
        {
            if(CheckIfPlayerCanBuy() || CanDealerMakeTrade()) return false;
            return true;
        }

        private bool CheckIfPlayerCanBuy()
        {
            // Checks if player is broke or has too full a bag
            return (Character.Bag.Values.Sum() + TradeQuantity > Character.BagCapacity ||
                Character.GetCash() < TradeQuantity * Player.BASEPRICES[FeaturedMerch] * GeneralModifier);
        }

        private bool CanDealerMakeTrade()
        {
            // Checks if dealer has enough
            return (Dealer.DealerMerchandise[FeaturedMerch] - TradeQuantity < 0 || Dealer.DealerMerchandise[FeaturedMerch] == 0);
        }

        public void SetUpInventory()
        {
            // Should never reach this point
            if (Dealer.Faction == Factions.NotDefined) return;
            SetIcon();
        }

        private void SetIcon()
        {
            // Set message from message set
            int message = new Random().Next(IntroMessages.Length);
            MerchantSpeech.Content = IntroMessages[message];

            // Set image from dictionary
            MerchantIcon.Source = GetDealerIcon();
            RespectGainLabel.Content = Dealer.Faction.ToString();
        }

        private BitmapImage GetDealerIcon() 
        {
            return new BitmapImage(new Uri(Merchant.FactionIcons[Dealer.Faction]));
        }

        public void SetMerchant(Merchant dealer)
        {
            Dealer = dealer;
            SetUpInventory();
        }

        private float CalculateRate(Merchandise merch, bool isSell)
        {
            float modifier = GeneralModifier;
            if (isSell && Dealer.PrefferedBuy == merch || !isSell && Dealer.PrefferedSell == merch) modifier += 0.25f;
            if (isSell && host.Game.SellTip == merch) modifier += host.Game.SellModifier;
            else if (!isSell && host.Game.BuyTip == merch) modifier += host.Game.BuyModifier;

            return modifier;
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
            RateLabel.Content = "x" + CalculateRate(FeaturedMerch, isSelling);
        }

        private void BuyButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isSelling) return;

            isSelling = false;
            BuySellIndicator.Content = "BUYING";
            RateLabel.Content = "x" + CalculateRate(FeaturedMerch, isSelling);
        }

        private void UpArrowClick(object sender, RoutedEventArgs e)
        {
            if (!ValidityCheck(false)) return;

            TradeQuantity++;
            QuantityMovedIndicator.Content = TradeQuantity.ToString();

            SetPotential();
        }

        private void DownArrowClick(object sender, RoutedEventArgs e)
        {
            if (!ValidityCheck(true)) return;

            TradeQuantity--;
            QuantityMovedIndicator.Content = TradeQuantity.ToString();

            SetPotential();
        }

        private void SetPotential()
        {
            int potential = (int)(TradeQuantity * Player.BASEPRICES[FeaturedMerch] * GeneralModifier);
            MoneyChangeLabel.Content = "$" + potential;
        }

        private bool ValidityCheck(bool isDown)
        {
            if (FeaturedMerch == Merchandise.NotDefined) return false;
            if (isDown && TradeQuantity <= 0) return false;

            return true;
        }

        private void DrugSelectClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button) return;

            Button clickedButton = (Button)sender;
            FeaturedMerch = Pairs[clickedButton.Name];

            // Set image
            FeaturedDrugImage.Source = new BitmapImage(new Uri(GameObject.MerchandiseIcons[Pairs[clickedButton.Name]]));
            FeaturedDrugImage.Opacity = 1.0;

            // Set labels
            SetDrugLabels();
        }

        private void SetDrugLabels()
        {
            DrugIndicatorLabel.Content = FeaturedMerch.ToString();
            HeatGainLabel.Content = GameObject.HeatRespectValues[FeaturedMerch].Item1;
            RespectGainLabel.Content = GameObject.HeatRespectValues[FeaturedMerch].Item2;
            TypeLabel.Content = TypeDescriptors[FeaturedMerch];

            RateLabel.Content = "x" + CalculateRate(FeaturedMerch, isSelling);
        }
        #endregion
    }
}
