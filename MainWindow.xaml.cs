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
            // Sets initial status indicators
            UpdateStatusIndicators();
            SetToBagOrStashView(true);
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

            CashLabel.Content = GameObject.ReturnMoneyString(Game.Character.GetCash());
            MoneyLabel.Content = GameObject.ReturnMoneyString(Game.Character.GetMoney());
        }

        public void UpdateMoney()
        {
            CashLabel.Content = GameObject.ReturnMoneyString(Game.Character.GetCash());
            MoneyLabel.Content = GameObject.ReturnMoneyString(Game.Character.GetCash());
        }

        private void MoveCashButton_Click(object sender, RoutedEventArgs e)
        {
            string input = MoneyInput.Text[1..];
            if (!Int32.TryParse(input, out int value)) return;

            MoveMoney(value);
        }

        public void SetToBagOrStashView(bool isStash)
        {
            // Used to automate
            Label[] countLabels =
            {
                DownersCount,
                WeedCount,
                AcidCount,
                EcstacyCount,
                HeroinCount,
                CokeCount
            };

            if (isStash)
            {
                for (int i = 0; i < countLabels.Length; i++)
                {
                    countLabels[i].Content = Game.Character.Stash.ElementAt(i).Value;
                }

                IsStashInView = true;

                // Chnage button colours
                BagView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
                StashView.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
            else
            {
                for (int i = 0; i < countLabels.Length; i++)
                {
                    countLabels[i].Content = Game.Character.Bag.ElementAt(i).Value;
                }

                IsStashInView = false;

                StashView.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
                BagView.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
        }

        public void UpdateStatusIndicators()
        {
            UpdateMoney();
            UpdateHeatAndRespect();
            Helper.SetCapacityBars(BagCapacityBar, StashCapacityBar, BagCapacityLabel, StashCapacityLabel, Game);
        }

        private void UpdateHeatAndRespect()
        {
            HeatBar.Value = Game.Character.Heat;
            ReputationBar.Value = Game.Character.Respect;
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