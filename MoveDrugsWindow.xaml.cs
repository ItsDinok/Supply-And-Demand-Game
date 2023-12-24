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
        // This keeps track of the REAL (not displayed) total for transfer
        private readonly int[] RealQuantities = [0, 0, 0, 0, 0, 0];

        public MoveDrugsWindow()
        {
            InitializeComponent();
            AssignLabelValues();
        }

        private void AssignLabelValues()
        {
            MainWindow host = (MainWindow)Application.Current.MainWindow;
            Label[] leftLabels =
            [
                DownersBagLabel,
                WeedBagLabel,
                AcidBagLabel,
                EcstacyBagLabel,
                HeroinBagLabel,
                CokeBagLabel
            ];

            for (int i = 0; i < leftLabels.Length; i++)
            {
                leftLabels[i].Content = host.Game.Character.Bag.ElementAt(i).Value.ToString();
            }

            Label[] rightLabels = 
            [
                DownersStashLabel,
                WeedStashLabel,
                AcidStashLabel,
                EcstacyStashLabel,
                HeroinStashLabel,
                CokeStashLabel
            ]; 

            for (int i = 0; i < rightLabels.Length;i++)
            {
                rightLabels[i].Content = host.Game.Character.Stash.ElementAt(i).Value.ToString();
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
            if (isRight)
            {
                index = UpdateQuantities(targeted, x + 1);
            }
            else
            {
                index = UpdateQuantities(targeted, x - 1);
            }
            targeted.Text = Math.Abs(RealQuantities[index]).ToString();

            // Assign arrows as needed
            Label[] indicators = [DownersDirectionalIndicator, WeedDirectionalIndicator, AcidDirectionalIndicator,
                EcstacyDirectionalIndicator, HeroinDirectionalIndicator, CokeDirectionalIndicator];
            // TODO: Assign PNGs
            if (RealQuantities[index] < 0) indicators[index].Content = "<---";
            else if (RealQuantities[index] == 0) indicators[index].Content = "";
            else indicators[index].Content = "--->";

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
            // This is the parent window, ignore the scary code
            MainWindow host = (MainWindow)Application.Current.MainWindow;

            int index = 0;
            foreach (int entry in RealQuantities)
            {
                // If the number is negative, it gets sent to bag, otherwise stash
                if (entry < 0) host.Game.Character.MoveToBag(Math.Abs(entry), (Merchandise)index);
                else host.Game.Character.MoveToStash(Math.Abs(entry), (Merchandise)index);
                index++;
            }

            AssignLabelValues();
        }
    }
}
