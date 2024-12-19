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

        // These will never be null but it makes the IDE calm down
        private TextBox[] TextBoxes = [];
        private Label[] DirectionalIndicators = [];
        private Dictionary<string, TextBox> TextBoxNames = [];
        private Label[] BagLabels = [];

        public MoveDrugsWindow()
        {
            Character = host.Game.Character;
            InitializeComponent();
            AssignMajorValues();

            // Sets things the player sees
            SetInitialSeen();
        }

        private void SetInitialSeen()
        {
            // Sets the tip indicators
            SetTipIndicators([host.Game.SellTip, host.Game.BuyTip]);
            SetPoliceStatus();
            AssignLabelValues();
            Helper.SetCapacityBars(StashCapacityBar, BagCapacityBar, StashCapacityLabel, BagCapacityLabel, host.Game);
            SetValueLabel();
        }

        private void AssignMajorValues()
        {
            // Makes the constructor less cluttered
            TextBoxes = [DownersTextBox, WeedTextBox, AcidTextBox, EcstacyTextBox, HeroinTextBox, CokeTextBox];
            DirectionalIndicators = [DownersDirectionalIndicator, WeedDirectionalIndicator, AcidDirectionalIndicator,
                                     EcstacyDirectionalIndicator, HeroinDirectionalIndicator, CokeDirectionalIndicator];
            TextBoxNames = new() { { "DownersLeftButton", DownersTextBox }, { "DownersRightButton", DownersTextBox },
                                   { "WeedLeftButton", WeedTextBox }, { "WeedRightButton", WeedTextBox },
                                   { "AcidLeftButton", AcidTextBox }, { "AcidRightButton", AcidTextBox },
                                   { "EcstacyLeftButton", EcstacyTextBox }, { "EcstacyRightButton", EcstacyTextBox },
                                   { "HeroinLeftButton", HeroinTextBox }, { "HeroinRightButton", HeroinTextBox },
                                   { "CokeLeftButton", CokeTextBox }, { "CokeRightButton", CokeTextBox }};
            BagLabels = [DownersBagLabel, WeedBagLabel, AcidBagLabel,
                EcstacyBagLabel, HeroinBagLabel, CokeBagLabel,

                DownersStashLabel, WeedStashLabel, AcidStashLabel,
                EcstacyStashLabel, HeroinStashLabel, CokeStashLabel];
        }

        // I FUCKING COOKED HERE
        private void SetTipIndicators(Merchandise[] merch)
        {
            // These are flipped, I do not know why
            // HACK
            Label[] indicators = [InDemandText, LowDemandText];
            Image[] indicatorImages = [InDemandImage, LowDemandImage];
            Image[] merchImages = [InDemandIndicator, LowDemandIndicator];

            // Sets the low then the high values
            for (int i = 0; i < merch.Length; i++) 
            {
                if (merch[i] == Merchandise.NotDefined) 
                {
                    SetTipNull(indicatorImages[i], indicators[i], merchImages[i]);
                    continue;
                }
                indicators[i].Opacity = 1;
                merchImages[i].Opacity = 1;
                indicatorImages[i].Source = GetMerchIcon(merch[i]);
            }
        }

        private static BitmapImage GetMerchIcon(Merchandise merch) => new BitmapImage(new Uri(GameObject.MerchandiseIcons[merch]));

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
            var bag = Character.Bag;
            var stash = Character.Stash;

            // Neat little function here that automatically assigns labels
            for (int i = 0; i < BagLabels.Length; i++)
            {
                if (i < bag.Count)
                {
                    BagLabels[i].Content = bag.ElementAt(i).Value.ToString();
                    continue;
                }
                BagLabels[i].Content = stash.ElementAt(i % stash.Count).Value.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e) => Close();

        private void ArrowButtonClicks(object sender, RoutedEventArgs e)
        {
            // TODO: Rewrite this#
            TextBox targeted;
            Button accessString = (Button)sender;

            targeted = TextBoxNames[accessString.Name];

            // Determines if it is adding or subtracting
            bool isRight = accessString.Name.Contains("Right");

            // Determines the drug being changed
            int index = TextBoxNames.Keys.ToList().IndexOf(accessString.Name) / 2;
            // Changes and sets drug quantity
            int updatedQuantity = isRight ? RealQuantities[index] + 1 : RealQuantities[index] - 1;
            // Changes text
            RealQuantities[index] = updatedQuantity;
            targeted.Text = Math.Abs(updatedQuantity).ToString();

            // Assign arrows as needed

            DirectionalIndicators[index].Content = updatedQuantity < 0 ? "⬅️" : updatedQuantity > 0 ? "➞" : "";
            DirectionalIndicators[index].IsEnabled = true;
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
            ResetBars();
            Helper.SetCapacityBars(StashCapacityBar, BagCapacityBar, StashCapacityLabel, BagCapacityLabel, host.Game);
		
			if (host.IsStashInView) host.UpdateLabels(host.Game.Character.Stash);
			else host.UpdateLabels(host.Game.Character.Bag);
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
            // Set all values to 0
            Array.Fill(RealQuantities, 0);


            // Reset textboxes and directional indicators
            foreach (TextBox box in TextBoxes)
            {
                box.Text = "0";
            }
            foreach (Label indicator in DirectionalIndicators)
            {
                indicator.Content = "";
            }
        }
    }
}
