using System.Windows.Media.Imaging;

namespace MarketGame
{
    public class Notification
    {
        public string Message;
        public string Name;
        public BitmapImage Icon;

        public string[] GangsterNames = [
            "Liang Chen", "Jin Wu", "Ming Zhao", // Triad Names
            "Johnny Russo", "Vinny Moretti", "Lucky Falcone", // Mob names
            "L-Train", "Dread", "Shay Williams", // Yardies names
            "Ivan Petrov", "Sergei Kozlov", "Dmitry Ivanov", // Russian 
            "Liam Cross", "Alex Steele", "Eddie Harrington", // Syndicate names
            "Razor", "Bones", "Wildcat", // Biker 
            //"Shek", "Charlie" // Friend names
        ];

        public Notification(Merchandise merch, bool isSell)
        {
            Name = GenerateName();
            Icon = SetImage();

            // Makes sure the string is valid and formatted
            string tempMessage = GenerateMessage(isSell);
            Message = ReplaceStringChunk(tempMessage, isSell, merch);
        }
         
        public Notification()
        {
            Random random = new();
            int index = random.Next(DialogueManager.SpamNames.Length);
            Name = DialogueManager.SpamNames[index];
            Message = DialogueManager.SpamTexts[index];
            Icon = new BitmapImage(new Uri("pack://application:,,/MarketGame;component/Icons/UnknownContact.png"));
        }

        private string GenerateName()
        {
            // Generates a random name
            Random random = new();
            return GangsterNames[random.Next(GangsterNames.Length)];
        }

        private static string GenerateMessage(bool isSell)
        {
            // Generates the message based on if the tip is buying or selling
            Random random = new();
            if (isSell) return DialogueManager.SellTips[random.Next(DialogueManager.SellTips.Length)];
            else return DialogueManager.BuyTips[random.Next(DialogueManager.BuyTips.Length)];
        }

        private static string ReplaceStringChunk(string input, bool isSell, Merchandise merch)
        {
            if (merch == Merchandise.NotDefined && isSell) return DialogueManager.PanicTextStrings[0];
            if (merch == Merchandise.NotDefined && !isSell) return DialogueManager.PanicTextStrings[0];

            if (isSell) return input.Replace("{}", merch.ToString());
            else return input.Replace("{}", merch.ToString());
        }

        // This gets the image
        private BitmapImage SetImage()
        {
            Factions faction = GetNameGangAffiliation();
            return new BitmapImage(new Uri(Merchant.FactionIcons[faction]));
        }

        private Factions GetNameGangAffiliation()
        {
            int nameIndex = Array.IndexOf(GangsterNames, Name);

            // Determines the gang affiliation depending on the name
            Factions faction = nameIndex switch
            {
                < 3 => Factions.Triad,
                < 6 => Factions.Mob,
                < 9 => Factions.Yardies,
                < 12 => Factions.Russians,
                < 15 => Factions.Syndicate,
                _ => Factions.Bikers,
            };

            return faction;
        }
    }
}
