using System.Windows;
using System.Windows.Controls;

namespace MarketGame
{
    public partial class BuySellWindow : Window
    {
        public Merchant Dealer;
        readonly MerchantSelect SelectWindow;
        readonly MerchantInventoryView InventoryView;

        public BuySellWindow()
        {
            InitializeComponent();

            Dealer = new(Factions.NotDefined);

            SelectWindow = new();
            InventoryView = new();

            WindowSetter.Content = SelectWindow;
        }

        public void SetChange(bool isToInventory = false, Button? sender = null)
        {
            if (isToInventory)
            {
                if (sender != null)
                {
                    Factions faction = sender.Name switch
                    {
                        "TriadButton" => Factions.Triad,
                        "RussianButton" => Factions.Russians,
                        "MobButton" => Factions.Mob,
                        "SyndicateButton" => Factions.Syndicate,
                        "YardiesButton" => Factions.Yardies,
                        "BikerButton" => Factions.Bikers,
                        _ => Factions.NotDefined,// It should never reach this point
                    };

                    Dealer.SetFaction(faction);

                    InventoryView.SetMerchant(Dealer);
                    InventoryView.AssignLabelValues();
                    WindowSetter.Content = InventoryView;
                }
            }
            else
            {
                WindowSetter.Content = SelectWindow;
            }

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
