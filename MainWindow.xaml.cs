using System.ComponentModel;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly public GameObject Game;
        public bool IsStashInView = false;

        public MainWindow()
        {
            ValueFromChildWindow = "";

            InitializeComponent();
            this.Game = new();
            HeatBar.Value = Game.Character.Heat;
            ReputationBar.Value = Game.Character.Respect;

            MoneyLabel.Content = "$" + Game.Character.DisplayMoney;
            CashLabel.Content = "$" + Game.Character.DisplayCash;

            SetToBagOrStashView(true);

            float StashPercentage = (float)Game.Character.GetTotalCapacity(true) / 1500 * 100;
            float BagPercentage = (float)Game.Character.GetTotalCapacity(false) / 150 * 100;

            StashCapacityBar.Value = StashPercentage;
            BagCapacityBar.Value = BagPercentage;

            StashCapacityLabel.Content = Game.Character.GetTotalCapacity(true).ToString() + "/ 1500";
            BagCapacityLabel.Content = Game.Character.GetTotalCapacity(false).ToString() + "/ 150";
        }

        // Stores value passed from child window
        public string ValueFromChildWindow { get; set; }

        private void MoneyInput_KeyUp(object sender, KeyEventArgs e) { 
            if (e.Key == Key.Enter)
            {
                string input = MoneyInput.Text[1..];
                if (!Int32.TryParse(input, out int value)) return;

                MoveMoney(value);
            }
        }

        private void MoveMoney(int amount)
        {
            if (ToCashButton.IsChecked == null) return;

            if ((bool)ToCashButton.IsChecked)
            {
                Game.Character.CashConvert(amount, true);
            }
            else
            {
                Game.Character.CashConvert(amount, false);
            }

            CashLabel.Content = "$" + Game.Character.DisplayCash;
            MoneyLabel.Content = "$" + Game.Character.DisplayMoney;
        }

        private void MoveCashButton_Click(object sender, RoutedEventArgs e)
        {
            string input = MoneyInput.Text[1..];
            if (!Int32.TryParse(input, out int value)) return;

            MoveMoney(value);
        }

        public void SetToBagOrStashView(bool isStash)
        {
            // TODO: Research sunken
            if (isStash)
            {
                DownersCount.Content = Game.Character.Stash[Merchandise.Downers];
                WeedCount.Content = Game.Character.Stash[Merchandise.Weed];
                AcidCount.Content = Game.Character.Stash[Merchandise.Acid];
                EcstacyCount.Content = Game.Character.Stash[Merchandise.Ecstacy];
                HeroinCount.Content = Game.Character.Stash[Merchandise.Heroin];
                CokeCount.Content = Game.Character.Stash[Merchandise.Coke];

                IsStashInView = true;

                BagView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
                // #FFDDDDDD
                StashView.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
            else
            {
                DownersCount.Content = Game.Character.Bag[Merchandise.Downers];
                WeedCount.Content = Game.Character.Bag[Merchandise.Weed];
                AcidCount.Content = Game.Character.Bag[Merchandise.Acid];
                EcstacyCount.Content = Game.Character.Bag[Merchandise.Ecstacy];
                HeroinCount.Content = Game.Character.Bag[Merchandise.Heroin];
                CokeCount.Content = Game.Character.Bag[Merchandise.Coke];

                IsStashInView = false;

                StashView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
                BagView.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
        }

        private void BagView_Click(object sender, RoutedEventArgs e)
        {
            SetToBagOrStashView(false);
        }

        private void StashView_Click(object sender, RoutedEventArgs e)
        {
            SetToBagOrStashView(true);
        }

        private void BuySell_Click(object sender, RoutedEventArgs e)
        {
            // Prevents duplicates
            if (Application.Current.Windows.OfType<BuySellWindow>().Any()) return;

            BuySellWindow second = new()
            {
                Owner = this
            };
            second.ShowDialog();
        }

        private void MoveDrugsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<MoveDrugsWindow>().Any()) return;

            MoveDrugsWindow moveDrugsWindow = new()
            {
                Owner = this
            };
            moveDrugsWindow.ShowDialog();
        }

        private void MerchantInventoryView_DesirableSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Width = 600;
            this.Height = 450;
        }
    }
}