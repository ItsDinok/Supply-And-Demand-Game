using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;

namespace MarketGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    // TODO: Consider putting this in gameobjects
    public struct Notification
    {
        public string Message;
        public string Name;
        public string PathToImage;
    }

    public partial class MainWindow : Window
    {
        readonly public GameObject Game;
        public bool IsStashInView = false;
        public Notification TipNotification = new();

        // This is used in tips
        private readonly DispatcherTimer _timer;


        public MainWindow()
        {
            InitializeComponent();
            this.Game = new();
            // Sets initial status indicators
            UpdateStatusIndicators();
            SetToBagOrStashView(true);

            // Timer logic for tips and messages
            _timer = new()
            {
                Interval = TimeSpan.FromSeconds(10)
            };
            _timer.Tick += OnTimedEvent;
            _timer.Start();
        }

        // This is called every time the timer reaches the time threshold
        private void OnTimedEvent(object ?source, EventArgs e)
        {
            Game.GenerateTipOff(this);
            NotificationGenerated();

            // This needs to be tuned.
            Game.Character.Heat--;
            Game.Character.Respect--;
            UpdateHeatAndRespect();
        }

        // TODO: Maybe export to class
        private void NotificationGenerated()
        {
            // Play sound
            PlayNotificationSound();

            // Set popup attributes
            DealerImage.Source = new BitmapImage(new Uri(TipNotification.PathToImage));
            DealerName.Content = TipNotification.Name;
            NotificationText.Text = TipNotification.Message;

            PopUpWindow();
        }

        private void PopUpWindow()
        {
            NotificationPopup.IsOpen = true;
        }

        private static void PlayNotificationSound()
        {
            using SoundPlayer player = new("Sound/TextNotification.wav");
            player.Play();
        }

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

            UpdateMoney();
        }

        public void UpdateMoney()
        {
            CashLabel.Content = Helper.ReturnMoneyString(Game.Character.GetCash());
            MoneyLabel.Content = Helper.ReturnMoneyString(Game.Character.GetMoney());
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
            [
                DownersCount,
                WeedCount,
                AcidCount,
                EcstacyCount,
                HeroinCount,
                CokeCount
            ];

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
    }
}