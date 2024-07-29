using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarketGame
{
    public partial class MoveDrugsWindow : Window
    {
        readonly MainWindow host = (MainWindow)Application.Current.MainWindow;

        // This keeps track of the REAL (not displayed) total for transfer
        private readonly int[] RealQuantities = [0, 0, 0, 0, 0, 0];

        public MoveDrugsWindow()
        {
            InitializeComponent();
            AssignLabelValues();
            SetValueLabel();
            Helper.SetCapacityBars(StashCapacityBar, BagCapacityBar, StashCapacityLabel, BagCapacityLabel, host.Game);
            SetPoliceStatus();
        }

        private void SetValueLabel()
        {
            int total = 0;
            // Accumulate all values in one dictionary for ease of calculation
            for (int i = 0; i < 6; i++)
            {
                total += host.Game.Character.Bag.ElementAt(i).Value * Player.BASEPRICES.ElementAt(i).Value;
                total += host.Game.Character.Stash.ElementAt(i).Value * Player.BASEPRICES.ElementAt(i).Value;
            }

            // Set label
            EstimatedTotalValueLabel.Content = GameObject.ReturnMoneyString(total);
        }

        private void AssignLabelValues()
        {
            Label[] labels =
            [
                DownersBagLabel, WeedBagLabel, AcidBagLabel,
                EcstacyBagLabel, HeroinBagLabel, CokeBagLabel,

                DownersStashLabel, WeedStashLabel, AcidStashLabel,
                EcstacyStashLabel, HeroinStashLabel, CokeStashLabel
            ];

            // Neat little function here that automatically assigns labels
            for (int i = 0; i < labels.Length; i++)
            {
                if (i < 6)
                {
                    labels[i].Content = host.Game.Character.Bag.ElementAt(i).Value.ToString();
                    continue;
                }
                labels[i].Content = host.Game.Character.Stash.ElementAt(i%6).Value.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Simply closes window
            Close();
        }

        private void ArrowButtonClicks(object sender, RoutedEventArgs e)
        {
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
            else index = UpdateQuantities(targeted, x - 1);

            targeted.Text = Math.Abs(RealQuantities[index]).ToString();

            // Assign arrows as needed
            Label[] indicators = [DownersDirectionalIndicator, WeedDirectionalIndicator, AcidDirectionalIndicator,
                EcstacyDirectionalIndicator, HeroinDirectionalIndicator, CokeDirectionalIndicator];

            if (RealQuantities[index] < 0) indicators[index].Content = "⬅️";
            else if (RealQuantities[index] == 0) indicators[index].Content = "";
            else indicators[index].Content = "➞";

            indicators[index].IsEnabled = true;
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

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            int index = 0;
            foreach (int entry in RealQuantities)
            {
                // If the number is negative, it gets sent to bag, otherwise stash
                if (entry < 0) host.Game.Character.MoveToBag(Math.Abs(entry), (Merchandise)index);
                else host.Game.Character.MoveToStash(Math.Abs(entry), (Merchandise)index);
                index++;
            }

            AssignLabelValues();
            UpdateCapacityBars();
            ResetBars();
            Helper.SetCapacityBars(StashCapacityBar, BagCapacityBar, StashCapacityLabel, BagCapacityLabel, host.Game);
        }

        private void SetPoliceStatus()
        {
            // This is better than sex.
            // Look at this switch statement.
            int heat = host.Game.Character.Heat;
            (string content, SolidColorBrush foreground) = heat switch
            {
                < 25 => ("You don't exist.", new SolidColorBrush(Colors.White)),
                < 50 => ("They have heard reports.", new SolidColorBrush(Colors.White)),
                < 75 => ("Lie Low.", new SolidColorBrush(Colors.Tomato)),
                _ => ("Good Luck. You will need it.", new SolidColorBrush(Colors.Maroon))
            };

            PoliceAwarenessLabel.Content = content;
            PoliceAwarenessLabel.Foreground = foreground;
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
            Label[] indicators = [DownersDirectionalIndicator,
                WeedDirectionalIndicator,
                AcidDirectionalIndicator,
                EcstacyDirectionalIndicator,
                HeroinDirectionalIndicator,
                CokeDirectionalIndicator];
            for (int i = 0; i < indicators.Length; i++)
            {
                indicators[i].Content = "";
            }
        }

        private void UpdateCapacityBars()
        {
            // Set bars
            float stashPercentage = (float)host.Game.Character.GetTotalCapacity(true) / 1500 * 100;
            float bagPercentage = (float)host.Game.Character.GetTotalCapacity(false) / 150 * 100;

            host.StashCapacityBar.Value = stashPercentage;
            host.BagCapacityBar.Value = bagPercentage;


            // Set label
            host.StashCapacityLabel.Content = host.Game.Character.GetTotalCapacity(true).ToString() + "/ 1500";
            host.BagCapacityLabel.Content = host.Game.Character.GetTotalCapacity(false).ToString() + "/ 150";
        }
    }
}
