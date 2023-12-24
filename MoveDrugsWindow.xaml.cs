using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MoveDrugsWindow.xaml
    /// </summary>
    public partial class MoveDrugsWindow : Window
    {
        public MoveDrugsWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ArrowButtonClicks(object sender, RoutedEventArgs e)
        {
            bool isAdd = false;
            TextBox targeted = new();

            // This is beautiful
            switch(((Button)sender).Name)
            {
                // Left buttons
                case "DownersLeftButton":
                    targeted = DownersTextBox; break;
                case "WeedLeftButton":
                    targeted = WeedTextBox; break;
                case "AcidLeftButton":
                    targeted = AcidTextBox; break;
                case "EcstacyLeftButton":
                    targeted = EcstacyTextBox; break;
                case "HeroinLeftButton":
                    targeted = HeroinTextBox; break;
                case "CokeLeftButton":
                    targeted = CokeTextBox; break;

                // Right buttons
                case "DownersRightButton":
                    targeted = DownersTextBox; isAdd = true; break;
                case "WeedRightButton":
                    targeted = WeedTextBox; isAdd = true; break;
                case "AcidRightButton":
                    targeted = AcidTextBox; isAdd = true; break;
                case "EcstacyRightButton":
                    targeted = EcstacyTextBox; isAdd = true; break;
                case "HeroinRightButton":
                    targeted = HeroinTextBox; isAdd = true; break;
                case "CokeRightButton":
                    targeted = CokeTextBox; isAdd = true; break;
            }

            if (!Int32.TryParse(targeted.Text, out int x)) return;
            if (!isAdd && x - 1 < 0) return;

            _ = isAdd == true ? targeted.Text = (x + 1).ToString() : targeted.Text = (x - 1).ToString();
        }

        private int[] GetAmount() 
        {
            string[] amounts =
            [
                DownersTextBox.Text,
                WeedTextBox.Text,
                AcidTextBox.Text,
                EcstacyTextBox.Text,
                HeroinTextBox.Text,
                CokeTextBox.Text,
            ];

            // Returns an ordered amounts and checks for user tampering
            int[] returnAmounts = new int[amounts.Length];
            for (int i = 0; i < amounts.Length; i++)
            {
                if (Int32.TryParse(amounts[i], out int x)) returnAmounts[i] = x;
                else returnAmounts[i] = 0;
            }

            return returnAmounts;
        }

#pragma warning disable CS8600
#pragma warning disable CS8602
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // This is the parent window, ignore the scary code
            MainWindow host = Application.Current.MainWindow as MainWindow;
            int[] quantities = GetAmount();

            // This can never happen, I hate WPF sometimes
            if (ToBagRadioButton.IsChecked == null) return;

            if((bool)ToBagRadioButton.IsChecked)
            {
                for (int i = 0; i <  quantities.Length; i++)
                {
                    host.Game.Character.MoveToBag(quantities[i], (Merchandise)i);
                }
                host.SetToBagOrStashView(true);
            }
            else
            {
                for (int i = 0; i < quantities.Length; i++)
                {
                    host.Game.Character.MoveToStash(quantities[i], (Merchandise)i);
                }
                host.SetToBagOrStashView(false);
            }
        }
#pragma warning restore CS8600
#pragma warning restore CS8602
    }
}
