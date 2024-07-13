using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarketGame
{
    /// <summary>
    /// Interaction logic for MerchantInventoryView.xaml
    /// </summary>
    /// TODO: Genuinely consider a per-drug screen
    /// TODO: Still needs to display cash and capacity
    public partial class MerchantInventoryView : UserControl
    {
        public Merchant Dealer;
        private readonly string[] IntroMessages =
        {
            "Come on, buy already... I'm a little jumpy."
        };
        readonly MainWindow host = (MainWindow)Application.Current.MainWindow;

        // This keeps track of the REAL (not displayed) total for transfer
        private readonly int[] RealQuantities = [0, 0, 0, 0, 0, 0];

        public MerchantInventoryView()
        {
            InitializeComponent();
            Dealer = new(Factions.NotDefined);
            AssignLabelValues(true);

        }

        private void DealerView_Click(object sender, RoutedEventArgs e)
        {
            AssignLabelValues(false);
            AlterToggleButton(false);
        }

        private void BagView_Click(object sender, RoutedEventArgs e)
        {
            AssignLabelValues(true);
            AlterToggleButton(true);
        }

        private void AssignLabelValues(bool isPlayer)
        {
            Label[] leftLabels =
            [
                DownersBagLabel,
                WeedBagLabel,
                AcidBagLabel,
                EcstacyBagLabel,
                HeroinBagLabel,
                CokeBagLabel
            ];

            if (isPlayer)
            {
                // This assigns player bag labels, assuming the player wants to start by selling
                for (int i = 0; i < leftLabels.Length; i++)
                {
                    leftLabels[i].Content = host.Game.Character.Bag.ElementAt(i).Value.ToString();
                }
            }
            else
            {
                // This assigns merchant bag labels
                for (int i = 0; i < leftLabels.Length; i++)
                {
                    leftLabels[i].Content = Dealer.GetInventory().ElementAt(i).Value.ToString();
                }
            }
            AlterToggleButton(true);

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
            // Despicable way to work out if I am buying or selling
            SolidColorBrush tester = new(Colors.LightSteelBlue);
            if (DealerView.Background == tester)
            {
                // Must be selling
                
            }
            else
            {
                Dictionary<Merchandise, int> Bag = host.Game.Character.Bag;
                int totalValue = 0;
                for (int i = 0; i < Bag.Count; i++)
                {
                    totalValue += (Bag.ElementAt(i).Value + RealQuantities[i]);
                }
                // TODO: Display error or warning
                if (totalValue > host.Game.Character.BagCapacity) return; 
            }
        }

        private void AlterToggleButton(bool isBagView)
        {
            if (isBagView)
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

            // Determines which drug it is
            switch (drugType.Name)
            {
                case "DownersTextBox":
                    this.RealQuantities[0] = amount;
                    index = 0;
                    break;
                case "WeedTextBox":
                    this.RealQuantities[1] = amount;
                    index = 1;
                    break;
                case "AcidTextBox":
                    this.RealQuantities[2] = amount;
                    index = 2;
                    break;

                case "EcstacyTextBox":
                    this.RealQuantities[3] = amount;
                    index = 3;
                    break;
                case "HeroinTextBox":
                    this.RealQuantities[4] = amount;
                    index = 4;
                    break;
                case "CokeTextBox":
                    this.RealQuantities[5] = amount;
                    index = 5;
                    break;
                // Thanks to buttons, this will never be called
                default:
                    index = -1;
                    break;
            }

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
            else if (x-1 < 0) return; 
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
            Random random = new();
            int message = random.Next(IntroMessages.Length);

            switch(Dealer.Faction)
            {
                case Factions.Yardies:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Yardies.png"));
                    break;
                case Factions.Triad:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Triad.png"));
                    break;
                case Factions.Mob:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Mob.png"));
                    break;
                case Factions.Syndicate:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Syndicate.png"));
                    break;
                case Factions.Russians:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Russian.png"));
                    break;
                case Factions.Bikers:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Biker.png"));
                    break;
            }

            MerchantSpeech.Content = IntroMessages[message];
        }

        public void SetMerchant(Merchant dealer)
        {
            Dealer = dealer;
            SetUpInventory();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BuySellWindow ParentWindow = (BuySellWindow)Window.GetWindow(this);
            ParentWindow.SetChange();
        }

        public event EventHandler<SizeChangedEventArgs>? DesiredSizeChanged;

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DesiredSizeChanged?.Invoke(this, e);
        }
    }
}
