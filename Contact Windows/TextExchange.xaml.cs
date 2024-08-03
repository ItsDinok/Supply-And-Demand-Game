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
using System.Windows.Threading;

namespace MarketGame.Contact_Windows
{
    /// <summary>
    /// Interaction logic for TextExchange.xaml
    /// </summary>
    public partial class TextExchange : UserControl
    {
        private readonly string Character;
        private readonly string DisplayedTime;
        private readonly string Day;

        public TextExchange(string character)
        {
            InitializeComponent();
            Character = character;
        
            ContactName.Content = character;
            ContactImage.Source = ContactWindow.GetCharacterImage(Character);

            DisplayedTime = SetTime();
            Day = SetDay();

            TimeLabel.Content = DisplayedTime;
            SetMessageTimes();
            HideMembers();

            SetMessages();
        }

        private static string SetTime() => DateTime.Now.ToString("HH:mm");

        private static string SetDay() => DateTime.Now.DayOfWeek.ToString() + " ";

        private void HideMembers()
        {
            StackPanel[] containers =
            [
                BoxOneContainer,
                BoxTwoContainer,
                BoxThreeContainer,
                BoxFourContainer,
            ];

            foreach (var item in containers)
            {
                item.Opacity = 0;
            }
        }

        private void SetMessageTimes()
        {
            Label[] metaLabels = [
                BoxOneMetalabel,
                BoxTwoMetalabel,
                BoxThreeMetalabel,
                BoxFourMetalabel
            ];

            for (int i = 0; i < metaLabels.Length; i++)
            {
                metaLabels[i].Content = Day + DisplayedTime;
            }
        }

        private void SetMessages()
        {
            DispatcherTimer messageTime = new()
            {
                Interval = TimeSpan.FromSeconds(3)
            };
            messageTime.Tick += MessageUpdates;
            /*
             Every n seconds a message is played

            -> Swaps side of screen for existing messages and pushes them back
            -> Changes colours for messages
            -> For user turn messages keyboard sounds are played
            -> While not user messages ... is displayed while the contact 'types'
             */
        }

        private void MessageUpdates(object? source, EventArgs e) { }
    }
}
