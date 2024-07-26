using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MerchantSelect.xaml
    /// </summary>
    public partial class MerchantSelect : UserControl
    {
        public BuySellWindow? ParentWindow;
        private readonly MainWindow host = (MainWindow)Application.Current.MainWindow;

        public MerchantSelect()
        {
            InitializeComponent();
            SetTipButtons();
        }

        private void MerchantSelected(object sender, RoutedEventArgs e)
        {
            ParentWindow = (BuySellWindow)Window.GetWindow(this);
            ParentWindow.SetChange(true, (Button)sender);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow = (BuySellWindow)Window.GetWindow(this);
            ParentWindow.Close();
        }

        private void SetTipButtons() {

            Dictionary<Merchandise, string> MerchandiseIcons = new() 
            {
            {Merchandise.Downers, "pack://application:,,,/MarketGame;component/Icons/Downers.png" },
            {Merchandise.Weed, "pack://application:,,,/MarketGame;component/Icons/Weed.png" },
            {Merchandise.Ecstacy, "pack://application:,,,/MarketGame;component/Icons/Ecstacy.png" },
            {Merchandise.Acid, "pack://application:,,,/MarketGame;component/Icons/Acid.png" },
            {Merchandise.Coke, "pack://application:,,,/MarketGame;component/Icons/Coke.png" },
            {Merchandise.Heroin, "pack://application:,,,/MarketGame;component/Icons/Heroin.png" }
            };

            // Set icons to correct image or hide image from view
            if (host.Game.BuyTip != Merchandise.NotDefined)
            {
                BuyTipImage.Source = new BitmapImage(new Uri(MerchandiseIcons[host.Game.BuyTip]));
            }   else BuyTipImage.Opacity = 0;

            if (host.Game.SellTip != Merchandise.NotDefined)
            {
                SellTipImage.Source = new BitmapImage(new Uri(MerchandiseIcons[host.Game.BuyTip]));
            }   else SellTipImage.Opacity = 0;
        }
    }
}
