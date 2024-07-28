using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarketGame
{
    /// <summary>
    /// Interaction logic for MerchantInventoryView.xaml
    /// </summary>
    /// TODO: Genuinely consider a per-drug screen
    /// TODO: Still needs to display capacity
    /// BUG: Labels don't update when changing vendors
    /// TODO: A lot of this can be generalised to avoid double declarations
    
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
        private readonly float Modifier = 1;

        // This keeps track of the REAL (not displayed) total for transfer
        private readonly int[] RealQuantities = [0, 0, 0, 0, 0, 0];
        private bool isSelling = true;
        
        // TODO: consider putting this in merchant class
        private static readonly Dictionary<Factions, string> FactionIcons = new()
        {
            {Factions.Yardies, "pack://application:,,,/MarketGame;component/Icons/Yardies.png" },
            {Factions.Triad, "pack://application:,,,/MarketGame;component/Icons/Triad.png" },
            {Factions.Mob, "pack://application:,,,/MarketGame;component/Icons/Mob.png" },
            {Factions.Syndicate, "pack://application:,,,/MarketGame;component/Icons/Syndicate.png" },
            {Factions.Russians, "pack://application:,,,/MarketGame;component/Icons/Russian.png" },
            {Factions.Bikers, "pack://application:,,,/MarketGame;component/Icons/Biker.png" }
        };

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
                labels[i].Content = Dealer.GetInventory().ElementAt(i%6).Value.ToString();
                HeatGainLabel.Content = Dealer.Faction.ToString();
            }

        }


        /*
         Buying/Selling protocol:

        - Check if bag will exceed
        - Iteratively call buy or sell
        - Update cash/heat/respect display
        - Bust will happen before calling merchant functions
         */
        private void ConfirmDeal(object sender, RoutedEventArgs e)
        {
            TextBox[] textBoxes = [];
            if (isSelling)
            {
                // Selling
                int value;
                Merchandise type;
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    if (textBoxes[i].Text == "0") continue;

                    value = Int32.Parse(textBoxes[i].Text);
                    type = Character.Bag.ElementAt(i).Key;
                    Character.Sell(value, type, Modifier);
                    Dealer.Buy(value, type);

                    // Raise heat and respect
                    // TODO: Respect should depend on quality of deal
                    Character.Heat += 10;
                    Character.Respect += 10;
                }
                AssignLabelValues();
            }
            else
            {
                if (CheckIfPurchaseValid())
                {
                    int value;
                    Merchandise type;
                    for (int i = 0; i < textBoxes.Length; i++)
                    {
                        if (textBoxes[i].Text == "0") continue;

                        value = Int32.Parse(textBoxes[i].Text);
                        type = Character.Bag.ElementAt(i).Key;
                        Character.Buy(value, type, Modifier);
                        Dealer.Sell(value, type);

                        // Raise heat and respect
                        // TODO: Respect should depend on quality of deal
                        Character.Heat += 10;
                        Character.Respect += 10;
                    }
                    AssignLabelValues();
                }
            }
            host.UpdateStatusIndicators();
        }

        private bool CheckIfPurchaseValid()
        {
            // TODO : Display error or warning
            // Buying
            Dictionary<Merchandise, int> Bag = Character.Bag;
            int totalValue = 0;
            for (int i = 0; i < Bag.Count; i++)
            {
                totalValue += (Bag.ElementAt(i).Value + RealQuantities[i]);
            }
            // TODO: Display error or warning
            if (totalValue > Character.BagCapacity) return false;
            // TODO: Explort a money check to player class for verification
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
            MerchantIcon.Source = new BitmapImage(new Uri(FactionIcons[Dealer.Faction]));
            RespectGainLabel.Content = Dealer.Faction.ToString();
        }

        public void SetMerchant(Merchant dealer)
        {
            Dealer = dealer;
            SetUpInventory();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BuySellWindow ParentWindow = (BuySellWindow)Window.GetWindow(this);
            // HACK: See if I can't fix this for a better user experience
            //ParentWindow.SetChange();
            ParentWindow.Close();
        }

        public event EventHandler<SizeChangedEventArgs>? DesiredSizeChanged;

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DesiredSizeChanged?.Invoke(this, e);
        }

    }
}
