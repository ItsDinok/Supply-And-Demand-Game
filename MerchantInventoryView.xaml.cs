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
    /// Interaction logic for MerchantInventoryView.xaml
    /// </summary>
    public partial class MerchantInventoryView : UserControl
    {
        public Merchant Dealer;

        public MerchantInventoryView()
        {
            InitializeComponent();
            Dealer = new(Factions.NotDefined);
        }

        public void SetUpInventory()
        {
            // Should never reach this point
            if (Dealer.Faction == Factions.NotDefined) return;


        }

        public void SetMerchant(Merchant dealer)
        {
            Dealer = dealer;
        }
    }
}
