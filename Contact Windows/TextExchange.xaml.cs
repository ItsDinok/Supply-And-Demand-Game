using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;

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
        private readonly string[] Messages;
		private int MessageCount = 0;
        private readonly bool IsIntroduction;

        public TextExchange(string character, bool isIntroduction)
        {
            InitializeComponent();
			// This sets the character and retrieves messages from dialogue manager
            Character = character;
            IsIntroduction = isIntroduction;
            Messages = GetMessages();

			// Pretty stuff
            DisplayedTime = SetTime();
            Day = SetDay();

			// Sets up the contact and message times
            SetVisuals();
            SetMessageTimes();

            SetMessages();
        }

        private void SetVisuals()
        {
            ContactName.Content = Character;
            ContactImage.Source = ContactWindow.GetCharacterImage(Character);
            TimeLabel.Content = DisplayedTime;

            HideMembers();
        }

		// These set parameters at the top of the screen
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
			messageTime.Start();
            /*
             Every n seconds a message is played

            -> Swaps side of screen for existing messages and pushes them back
            -> Changes colours for messages
            -> For user turn messages keyboard sounds are played
            -> While not user messages ... is displayed while the contact 'types'
             */
        }

        private string[] GetMessages()
        {
            Dictionary<string, int> characterIndexes = new() { { "The Broker", 0 }, { "Igor Petrovitch", 1 }, { "Officer Smith", 2 }, { "The Runner", 3 }, { "The Student Union", 4 } };
            if (IsIntroduction)
            {
                return DialogueManager.IntroductionTextExchanges[characterIndexes[Character]];
            }
            // TODO: Make this scalable for more dialogue options
            return DialogueManager.ContactTextExchanges[characterIndexes[Character]];
        }

        private void MessageUpdates(object? source, EventArgs e)
        {
			MessageCount++;

			if (MessageCount >=4)
			{
				DispatcherTimer messageTime = (DispatcherTimer)source;
				messageTime.Stop();
			}

            // These are used to iterate
            StackPanel[] containers = [BoxOneContainer, BoxTwoContainer, BoxThreeContainer, BoxFourContainer];
            TextBlock[] messageBoxes = [BoxOne, BoxTwo, BoxThree, BoxFour];

            StackPanel lastContainer = GetFirstEmptyStackPanel(containers);

			int index = GetBoxIndex(containers);
			
            //PushBackLabels(messageBoxes, containers, index);
            SetNewText(lastContainer, index);
			lastContainer.Opacity = 100;
		}

        private void SetNewText(StackPanel lastContainer, int index) 
        {
			TextBlock[] labels = [BoxOne, BoxTwo, BoxThree, BoxFour];

			string message =  Messages[index];
			labels[index].Text = message;
			
        }

        private static StackPanel GetFirstEmptyStackPanel(StackPanel[] containers)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                if (containers[i].Opacity == 0) return containers[i];
            }
            // It will never reach this
            return new StackPanel();
        }

        private int GetBoxIndex(StackPanel[] containers)
        {
			for (int i = 0; i < containers.Length; i++) 
			{
				if (containers[i].Opacity == 0) return i;
			}
			return -1;
		}

        private void PushBackLabels(Label[] messages, StackPanel[] containers, int index)
        {
            Border[] borders =
            [
                BoxOneBorder,
                BoxTwoBorder,
                BoxThreeBorder,
                BoxFourBorder
            ];

            string tempLabel = "";
            string secondTemp = "";
            // This needs to swap the sides, colours, and push back the labels
            for(int i = 0; i < index; i++)
            {
                if (messages[i].Content.ToString() == null) return;

                secondTemp = messages[i].Content.ToString();
                messages[i].Content = tempLabel;
                tempLabel = secondTemp;

                // Colours will flip 
                bool isGreen = borders[i].Background.Equals(Colors.Green);
                borders[i].Background = isGreen ? new SolidColorBrush(Color.FromArgb(0xFF, 0x63, 0x63, 0x63)) : new SolidColorBrush(Colors.Green);

                // Position flip
                bool isAlignedLeft = containers[i].HorizontalAlignment.ToString() == "Left";
                containers[i].HorizontalAlignment = isAlignedLeft ? HorizontalAlignment.Right : HorizontalAlignment.Left;
				
            }
        }
    }
}
