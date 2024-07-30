using System.Printing;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarketGame
{
    public partial class MoveDrugsWindow : Window
    {
        // Aliases for convenience
        private readonly MainWindow host = (MainWindow)Application.Current.MainWindow;
        private readonly Player Character;

        // This keeps track of the REAL (not displayed) total for transfer
        private readonly int[] RealQuantities = [0, 0, 0, 0, 0, 0];

        public MoveDrugsWindow()
        {
            Character = host.Game.Character;
            InitializeComponent();
            AssignLabelValues();
            SetValueLabel();
            Helper.SetCapacityBars(StashCapacityBar, BagCapacityBar, StashCapacityLabel, BagCapacityLabel, host.Game);
            SetPoliceStatus();
            // Sets the tip indicators
            SetTipIndicators(host.Game.SellTip, InDemandText, InDemandIndicator, InDemandImage);
            SetTipIndicators(host.Game.BuyTip, LowDemandText, LowDemandIndicator, LowDemandImage);
        }

        private static void SetTipIndicators(Merchandise merch, Label indicatorText, Image indicatorImage, Image merchImage)
        {
            if (merch == Merchandise.NotDefined) {
                SetTipNull(indicatorImage, indicatorText, merchImage);
                return;
            }

            indicatorImage.Opacity = 1;
            indicatorText.Opacity = 1;
            merchImage.Source = new BitmapImage(new Uri(GameObject.MerchandiseIcons[merch]));
        }

        private static void SetTipNull(Image image, Label text, Image merchImage)
        {
            image.Opacity = 0;
            text.Opacity = 0;
            merchImage.Opacity = 0;
        }

        private void SetValueLabel()
        {
            int total = 0;
            // Accumulate all values in one dictionary for ease of calculation
            
            for (int i = 0; i < Character.Bag.Count; i++)
            {
                // Multiplies the value of each item by the baseprices
                total += Character.Bag.ElementAt(i).Value * Player.BASEPRICES.ElementAt(i).Value;
                total += Character.Stash.ElementAt(i).Value * Player.BASEPRICES.ElementAt(i).Value;
            }

            // Set label
            EstimatedTotalValueLabel.Content = Helper.ReturnMoneyString(total);
        }

        private void AssignLabelValues()
        {
            Label[] labels =
            {
                DownersBagLabel, WeedBagLabel, AcidBagLabel,
                EcstacyBagLabel, HeroinBagLabel, CokeBagLabel,

                DownersStashLabel, WeedStashLabel, AcidStashLabel,
                EcstacyStashLabel, HeroinStashLabel, CokeStashLabel
            };

            var bag = Character.Bag;
            var stash = Character.Stash;

            // Neat little function here that automatically assigns labels
            for (int i = 0; i < labels.Length; i++)
            {
                if (i < bag.Count)
                {
                    labels[i].Content = bag.ElementAt(i).Value.ToString();
                    continue;
                }
                labels[i].Content = stash.ElementAt(i % stash.Count).Value.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Simply closes window
            Close();
        }

        private void ArrowButtonClicks(object sender, RoutedEventArgs e)
        {
            // TODO: Rewrite this#
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

            // Determines if it is adding or subtracting
            bool isRight = accessString.Name.Contains("Right");

            // Determines the drug being changed
            int index = keyValuePairs.Keys.ToList().IndexOf(accessString.Name);
            // Changes and sets drug quantity
            int updatedQuantity = isRight ? RealQuantities[index] + 1 : RealQuantities[index] -1;
            // Changes text
            RealQuantities[index] = updatedQuantity;
            targeted.Text = Math.Abs(updatedQuantity).ToString();

            // Assign arrows as needed
            Label[] indicators = [DownersDirectionalIndicator, WeedDirectionalIndicator, AcidDirectionalIndicator,
                EcstacyDirectionalIndicator, HeroinDirectionalIndicator, CokeDirectionalIndicator];
            // Changes directional indicators
            indicators[index].Content = updatedQuantity < 0 ? "⬅️" : updatedQuantity > 0 ? "➞" : "";
            indicators[index].IsEnabled = true;
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
