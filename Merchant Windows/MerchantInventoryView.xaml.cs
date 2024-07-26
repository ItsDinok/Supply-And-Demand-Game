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
        // TODO: Put this in gameobjects
        private static readonly Dictionary<string, int> DrugIndices = new()
        {
            { "DownersTextBox", 0 },
            { "WeedTextBox", 1 },
            { "AcidTextBox", 2 },
            { "EcstacyTextBox", 3 },
            { "HeroinTextBox", 4 },
            { "CokeTextBox", 5 }
        };

        public MerchantInventoryView()
        {
            InitializeComponent();
            this.Character = host.Game.Character;
            Dealer = new(Factions.NotDefined);
            AssignLabelValues();
        }

        private void DealerView_Click(object sender, RoutedEventArgs e)
        {
            AssignLabelValues();
            AlterToggleButton();
            isSelling = false;
        }

        private void BagView_Click(object sender, RoutedEventArgs e)
        {
            AssignLabelValues();
            AlterToggleButton();
            isSelling = true;
        }

        private void AssignLabelValues()
        {
            Label[] labels =
            [
                DownersBagLabel,
                WeedBagLabel,
                AcidBagLabel,
                EcstacyBagLabel,
                HeroinBagLabel,
                CokeBagLabel
            ];

            if (isSelling)
            {
                // This assigns player bag labels, assuming the player wants to start by selling
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Content = Character.Bag.ElementAt(i).Value.ToString();
                }
            }
            else
            {
                // This assigns merchant bag labels
                for (int i = 0; i < labels.Length; i++)
                {
                    labels[i].Content = Dealer.GetInventory().ElementAt(i).Value.ToString();
                }
            }
            AlterToggleButton();

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
            TextBox[] textBoxes = [DownersTextBox, WeedTextBox, AcidTextBox, EcstacyTextBox, HeroinTextBox, CokeTextBox];
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
            ResetBars();
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

        private void AlterToggleButton()
        {
            if (isSelling)
            {
                DealerView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
                // #FFDDDDDD
                BagView.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
            else
            {
                BagView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
                // #FFDDDDDD
                DealerView.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
        }

        private int UpdateQuantities(TextBox drugType, int amount)
        {
            // The index is used in the negative function
            int index;
            index = DrugIndices[drugType.Name];
            this.RealQuantities[index] = amount;
            return index;
        }

        private void ArrowButtonClicks(object sender, RoutedEventArgs e)
        {
            // This function is modified from the move merchandise window, there are two key changes:
            // 1) The ability to move negative quantities has been removes
            // 2) There are no directional indicators

            bool isRight;
            TextBox targeted;
            Button accessString = (Button)sender;

            // This assigns a value to the textbox that sent it
            Dictionary<string, TextBox> keyValuePairs = new()
            {
                { "DownersLeftButton", DownersTextBox },
                { "DownersRightButton", DownersTextBox },
                { "WeedLeftButton", WeedTextBox },
                { "WeedRightButton", WeedTextBox },
                { "AcidLeftButton", AcidTextBox },
                { "AcidRightButton", AcidTextBox },
                { "EcstacyLeftButton", EcstacyTextBox },
                { "EcstacyRightButton", EcstacyTextBox },
                { "HeroinLeftButton", HeroinTextBox },
                { "HeroinRightButton", HeroinTextBox },
                { "CokeLeftButton", CokeTextBox },
                { "CokeRightButton", CokeTextBox }
            };
            targeted = keyValuePairs[accessString.Name];

            // Determine if targeted is left or right
            // NOTE: It is scarily lucky no merch contains L, if this ever changes, fix this
            if (!accessString.Name.Contains('L')) isRight = true;
            else isRight = false;

            int x = RealQuantities[(keyValuePairs.Keys.ToList().IndexOf(accessString.Name)) / 2];
            int index;

            // Update text values
            if (isRight) index = UpdateQuantities(targeted, x + 1);
            else if (x - 1 < 0) return;
            else index = UpdateQuantities(targeted, x - 1);

            targeted.Text = Math.Abs(RealQuantities[index]).ToString();
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

        private void ResetBars()
        {
            DownersTextBox.Text = "0";
            WeedTextBox.Text = "0";
            AcidTextBox.Text = "0";
            EcstacyTextBox.Text = "0";
            HeroinTextBox.Text = "0";
            CokeTextBox.Text = "0";

            for (int i = 0; i < RealQuantities.Length; i++)
            {
                RealQuantities[i] = 0;
            }
        }
    }
}
