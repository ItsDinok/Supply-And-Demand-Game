using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

        private void SetTipButtons()
        {
            // Set icons to correct image or hide image from view
            if (host.Game.BuyTip != Merchandise.NotDefined)
            {
                BuyTipImage.Source = new BitmapImage(new Uri(GameObject.MerchandiseIcons[host.Game.BuyTip]));
            }
            else BuyTipImage.Opacity = 0;

            if (host.Game.SellTip != Merchandise.NotDefined)
            {
                SellTipImage.Source = new BitmapImage(new Uri(GameObject.MerchandiseIcons[host.Game.SellTip]));
            }
            else SellTipImage.Opacity = 0;
        }
    }
}
