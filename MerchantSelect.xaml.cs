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
        public BuySellWindow ?ParentWindow; 

        public MerchantSelect()
        {
            InitializeComponent();
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
    }
}
