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
    /// Interaction logic for InformationControl.xaml
    /// </summary>
    public partial class InformationControl : UserControl
    {
        private readonly string Character;
        private readonly int CharacterIndex;

        public InformationControl(string character)
        {
            InitializeComponent();
            CharacterIndex = ContactMenu.GetCharacterIndex(character);
            Character = character;

            SetVisuals();
        }

        private ContactWindow SetParentWindow() => (ContactWindow)Window.GetWindow(this);
        

        private void SetVisuals()
        {
            CharacterName.Content = Character;
            CharacterDescription.Content = DialogueManager.CharacterTaglines[CharacterIndex];
            CharacterIntroduction.Text = DialogueManager.ContactDescriptions[CharacterIndex];
            ContactIcon.Source = ContactWindow.CharacterNamesToImages[Character];
        }

        private void Back(object sender, RoutedEventArgs e) 
        {
            ContactWindow parent = SetParentWindow();
            parent.SwitchToMain(Character);
        }
        
    }
}
