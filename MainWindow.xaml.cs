using System.ComponentModel;
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
        readonly GameObject Game;

        public MainWindow()
        {
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

        // Update value from child window
        public void UpdateFromBuySellWindow(string value)
        {

        }

        private void MoveCashButton_Click(object sender, RoutedEventArgs e)
        {
            string input = MoneyInput.Text[1..];
            if (!Int32.TryParse(input, out int value)) return;

            if (ToCashButton.IsChecked == null) return;

            if ((bool)ToCashButton.IsChecked)
            {
                Game.Character.CashConvert(value, true);
            }
            else
            {
                Game.Character.CashConvert(value, false);
            }

            CashLabel.Content = "$" + Game.Character.DisplayCash;
            MoneyLabel.Content = "$" + Game.Character.DisplayMoney;
        }

        private void SetToBagOrStashView(bool isStash)
        {
            if (isStash)
            {
                DownersCount.Content = Game.Character.Stash[Merchandise.Downers];
                WeedCount.Content = Game.Character.Stash[Merchandise.Weed];
                AcidCount.Content = Game.Character.Stash[Merchandise.Acid];
                EcstacyCount.Content = Game.Character.Stash[Merchandise.Ecstacy];
                HeroinCount.Content = Game.Character.Stash[Merchandise.Heroin];
                CokeCount.Content = Game.Character.Stash[Merchandise.Coke];
            }
            else
            {
                DownersCount.Content = Game.Character.Bag[Merchandise.Downers];
                WeedCount.Content = Game.Character.Bag[Merchandise.Weed];
                AcidCount.Content = Game.Character.Bag[Merchandise.Acid];
                EcstacyCount.Content = Game.Character.Bag[Merchandise.Ecstacy];
                HeroinCount.Content = Game.Character.Bag[Merchandise.Heroin];
                CokeCount.Content = Game.Character.Bag[Merchandise.Coke];
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

            BuySellWindow second = new();
            second.Owner = this;
            second.ShowDialog();
        }

        private void MoveDrugsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Windows.OfType<MoveDrugsWindow>().Any()) return;

            MoveDrugsWindow moveDrugsWindow = new();
            moveDrugsWindow.Owner = this;
            moveDrugsWindow.ShowDialog();
        }
    }
}