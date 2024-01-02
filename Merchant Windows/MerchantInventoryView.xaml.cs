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

        private readonly string[] IntroMessages =
        {
            "Come on, buy already... I'm a little jumpy."
        };

        public MerchantInventoryView()
        {
            InitializeComponent();
            Dealer = new(Factions.NotDefined);
        }

        public void SetUpInventory()
        {
            // Should never reach this point
            if (Dealer.Faction == Factions.NotDefined) return;
            SetIcon();

        }

        private void SetIcon()
        {
            Random random = new();
            int message = random.Next(IntroMessages.Length);

            switch(Dealer.Faction)
            {
                case Factions.Yardies:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Yardies.png"));
                    break;
                case Factions.Triad:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Triad.png"));
                    break;
                case Factions.Mob:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Mob.png"));
                    break;
                case Factions.Syndicate:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Syndicate.png"));
                    break;
                case Factions.Russians:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Russian.png"));
                    break;
                case Factions.Bikers:
                    MerchantIcon.Source = new BitmapImage(new Uri("pack://application:,,,/MarketGame;component/Icons/Biker.png"));
                    break;
            }

            MerchantSpeech.Content = IntroMessages[message];
        }

        public void SetMerchant(Merchant dealer)
        {
            Dealer = dealer;
            SetUpInventory();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            BuySellWindow ParentWindow = (BuySellWindow)Window.GetWindow(this);
            ParentWindow.SetChange();
        }
    }
}
