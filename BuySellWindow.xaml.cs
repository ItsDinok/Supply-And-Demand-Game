using System.ComponentModel;
using System.Text;
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
    public partial class BuySellWindow : Window
    {
        public Merchant Dealer;

        public BuySellWindow()
        {
            InitializeComponent();
         
            UserControl selectWindow = new MerchantSelect();
            UserControl inventoryView = new MerchantInventoryView();

            WindowSetter.Content = selectWindow;
        }

        public void SetChange()
        {
            WindowSetter.Content = new MerchantInventoryView();
        }

        public void ForceClose()
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MerchantInventoryView newWindow = new();
            ShowWindow(newWindow);
        }

        private void ShowWindow(UserControl window)
        {
            WindowSetter.Content = window;
        }
    }
}
