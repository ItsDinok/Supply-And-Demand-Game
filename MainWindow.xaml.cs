﻿using System.Media;
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


        // Stored here for ease of editing
        private readonly TimeSpan NotificationTime = TimeSpan.FromSeconds(10);
        private readonly TimeSpan KillTime = TimeSpan.FromSeconds(2);

        public MainWindow()
        {
            InitializeComponent();
            this.Game = new();

            // Sets initial status indicators
            CountLabels = SetCountLabels();
            SetToBagOrStashView(true);

            // Timer logic for tips and messages
            _timer = SetTimer();
            _timer.Start();
            UpdateStatusIndicators();
        }

        private Label[] SetCountLabels()
        {
            return [
                DownersCount,
                WeedCount,
                AcidCount,
                EcstacyCount,
                HeroinCount,
                CokeCount
            ];
        }

        private DispatcherTimer SetTimer()
        {
            DispatcherTimer timer = new()
            {
                Interval = NotificationTime
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
            // BUG: This will not display when in contacts view
            NotificationPopup.IsOpen = true;

            // This limits time for notifications for user convenience
            DispatcherTimer killTimer = new()
            {
                Interval = KillTime
            };
            killTimer.Tick += ClosePopup;
            killTimer.Start();
            return;
        }

        private void ClosePopup(object? source, EventArgs e) => NotificationPopup.IsOpen = false;

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
            SetCapacityBars();
        }

        private void UpdateHeatAndRespect()
        {
            HeatBar.Value = Game.Character.Heat;
            ReputationBar.Value = Game.Character.Respect;
        }

        private void SetCapacityBars()
        {
            // These are used to calculate the value to display on the bar
            float StashPercentage = (float)Game.Character.GetTotalCapacity(true) / 1500 * 100;
            float BagPercentage = (float)Game.Character.GetTotalCapacity(false) / 150 * 100;

            StashCapacityBar.Value = StashPercentage;
            BagCapacityBar.Value = BagPercentage;

            UpdateLabels();
        }

        private void UpdateLabels()
        {
            StashCapacityLabel.Content = Game.Character.GetTotalCapacity(true).ToString() + "/ 1500";
            BagCapacityLabel.Content = Game.Character.GetTotalCapacity(false).ToString() + "/ 150";
        }


        private void BagView_Click(object sender, RoutedEventArgs e) => SetToBagOrStashView(false);
        private void StashView_Click(object sender, RoutedEventArgs e) => SetToBagOrStashView(true);

        private void BuySell_Click(object sender, RoutedEventArgs e)
        {
            BuySellWindow second = new()
            {
                Owner = this
            };
            second.ShowDialog();
        }

        private void MoveDrugsButton_Click(object sender, RoutedEventArgs e)
        {
            MoveDrugsWindow moveDrugsWindow = new()
            {
                Owner = this
            };
            moveDrugsWindow.ShowDialog();
        }
    }
}