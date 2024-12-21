using System.IO.Packaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MarketGame.Contact_Windows
{
    /// <summary>
    /// Interaction logic for ContactWindow.xaml
    /// </summary>

    public partial class ContactWindow : Window
    {

        public static readonly Dictionary<string, BitmapImage> CharacterNamesToImages = new()
        {
            { "The Broker", new BitmapImage(new Uri("pack://application:,,,/Icons/UnknownContact.png")) },
            { "Igor Petrovitch", new BitmapImage(new Uri("pack://application:,,,/Icons/Russian.png")) },
            { "Officer Smith", new BitmapImage(new Uri("pack://application:,,,/Icons/Mob.png")) },
            { "The Runner", new BitmapImage(new Uri("pack://application:,,,/Icons/Syndicate.png")) },
            { "The Student Union", new BitmapImage(new Uri("pack://application:,,,/Icons/Ecstacy.png")) }
        };

        public ContactWindow(string senderName)
        {
            InitializeComponent();

            WindowSetter.Content = new ContactMenu(senderName);
        }

        private void Back(object sender, RoutedEventArgs e) => this.Close();

        public void SwitchToInformation(string character) => WindowSetter.Content = new InformationControl(character);

        public void SwitchToMain(string character) => WindowSetter.Content = new ContactMenu(character);

        public void LoadMessageScreen(string character) => WindowSetter.Content = new TextExchange(character, false);

        public void LoadIntroductionScreen(string character) => WindowSetter.Content = new TextExchange(character, true); 

        public static BitmapImage GetCharacterImage(string character)
        {
            return CharacterNamesToImages[character];
        }
    }
}
