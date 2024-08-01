using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MarketGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        readonly public GameObject Game;
        public bool IsStashInView = false;
        public Notification TipNotification = new();

        // This is used in tips
        private readonly DispatcherTimer _timer;
        private readonly Label[] CountLabels;

        public MainWindow()
        {
            InitializeComponent();
            this.Game = new();

            // Sets initial status indicators
            UpdateStatusIndicators();

            CountLabels =
            [
                DownersCount,
                WeedCount,
                AcidCount,
                EcstacyCount,
                HeroinCount,
                CokeCount
            ];

            SetToBagOrStashView(true);
            
            // Timer logic for tips and messages
            _timer = SetTimer();
            _timer.Start();
        }

        private DispatcherTimer SetTimer()
        {
            DispatcherTimer timer = new()
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            timer.Tick += OnTimedEvent;

            return timer;
        }


        // This is called every time the timer reaches the time threshold
        private void OnTimedEvent(object? source, EventArgs e)
        {
            // Generate tip off and work out type
            if (new Random().Next(0,10) <= 3) Game.GenerateTipOff(this);
            else TipNotification = new Notification();
            
            // Display notification
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
            this.DealerImage.Source = TipNotification.Icon;
            this.DealerName.Content = TipNotification.Name;
            this.NotificationText.Text = TipNotification.Message;

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

        private void MoneyInput_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string input = MoneyInput.Text[1..];
                if (!Int32.TryParse(input, out int value)) return;

                MoveMoney(value);
            }
        }

        private void MoveMoney(int amount)
        {
            bool? isChecked = ToCashButton.IsChecked;
            if (isChecked == null) return;
            else Game.Character.CashConvert(amount, (bool)isChecked);

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
            
            SolidColorBrush activeColour = new (Colors.LightSteelBlue);
            SolidColorBrush inactiveColour = new(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));

            Dictionary<Merchandise, int> items = isStash ? Game.Character.Stash : Game.Character.Bag;

            UpdateLabels(items);
            UpdateView(isStash, activeColour, inactiveColour);
        }

        private void UpdateLabels(IDictionary<Merchandise, int> items)
        {
            for (int i = 0; i < CountLabels.Length; i++)
            {
                CountLabels[i].Content = items.ElementAt(i).Value.ToString();
            }
        }

        private void UpdateView(bool isStash, SolidColorBrush active, SolidColorBrush inactive)
        {
            IsStashInView = isStash;
            StashView.Background = isStash ? active : inactive;
            BagView.Background = isStash ? inactive : active;
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