using System;
using System.Collections.Generic;
using System.IO.Packaging;
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

namespace MarketGame.Contact_Windows
{
    /// <summary>
    /// Interaction logic for ContactMenu.xaml
    /// </summary>
    public partial class ContactMenu : UserControl
    {
        private ContactWindow ParentWindow;
        private readonly string Character;

        public ContactMenu(string senderName)
        {
            InitializeComponent();
            ParentWindow = (ContactWindow)Window.GetWindow(this);
            Character = GetCharacterName(senderName);

            CharacterName.Content = Character;
            ContactIcon.Source = ContactWindow.GetCharacterImage(Character);

            CharacterDescription.Content = DialogueManager.CharacterTaglines[GetCharacterIndex(Character)];
        }

        public static int GetCharacterIndex(string character)
        {
            Dictionary<string, int> CharacterIndices = new()
            {
                { "The Broker", 0 },
                { "Igor Petrovitch", 1 },
                { "Officer Smith", 2 },
                { "The Runner", 3 },
                { "The Student Union", 4}
            };

            return CharacterIndices[character];
        }

        private static string GetCharacterName(string sender)
        {
            Dictionary<string, string> senderToCharacter = new()
            {
                { "BrokerButton", "The Broker" },
                { "IgorButton", "Igor Petrovitch" },
                { "OSButton", "Officer Smith" },
                { "RunnerButton", "The Runner" },
                { "SUButton", "The Student Union" }
            };

            if (!senderToCharacter.TryGetValue(sender, out string? value)) return sender;

            return value;
        }

        private void InformationButtonClick(object sender, RoutedEventArgs e)
        {
            ParentWindow = (ContactWindow)Window.GetWindow(this);
            ParentWindow.SwitchToInformation(Character);
        } 

        private void Back(object sender, RoutedEventArgs e) 
        {
            ParentWindow = (ContactWindow) Window.GetWindow(this);
            ParentWindow.Close();
        }

        private void LoadMessageScreen(object sender, RoutedEventArgs e)
        {
            ParentWindow = (ContactWindow)Window.GetWindow(this);
            ParentWindow.LoadMessageScreen(Character);
        }

        private void UniqueSender(object sender, RoutedEventArgs e)
        {
            Dictionary<string, Action> UniqueButtons = new()
            {
                { "The Broker", Tutorial },
                { "Igor Petrovitch", LowValueJob },
                { "Officer Smith", Bribe },
                { "The Runner", RunStuff },
                { "The Student Union", LowValueDeal }
            };
        }

        #region Dictionary Functions
        private void Tutorial()
        {

        }

        private void LowValueJob()
        {

        }

        private void LowValueDeal()
        {

        }

        private void RunStuff()
        {

        }

        private void Bribe()
        {
            // Do stuff
        }
        #endregion
    }
}
