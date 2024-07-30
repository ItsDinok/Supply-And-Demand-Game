using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MarketGame
{
    public class GameObject
    {
        public Player Character;

        // These names are relative to the player
        public float SellModifier;
        public float BuyModifier;

        // Used to set extra modifiers
        public Merchandise BuyTip = Merchandise.NotDefined;
        public Merchandise SellTip = Merchandise.NotDefined;

        // TODO: Racism check some of these names, thanks chatGPT
        public string[] GangsterNames = {
            // Triad names
            "Liang Chen", "Jin Wu", "Ming Zhao",
            // Mob names
            "Johnny Russo", "Vinny Moretti", "Lucky Falcone",
            // Yardie names
            "L-Train", "Dread", "Shay Williams",
            // Russians
            "Ivan Petrov", "Sergei Kozlov", "Dmitry Ivanov",
            // Syndicate
            "Liam Cross", "Alex Steele", "Eddie Harrington",
            // Bikers
            "Razor", "Bones", "Wildcat",
            // Friends
            //"Shek", "Charlie" 
        };

        // NOTE: Tried to set values on the perceptions of each drug. I have no clue how these will be balanced
        public static readonly Dictionary<Merchandise, (int, int)> HeatRespectValues = new()
        {
            { Merchandise.Downers, (10, 4) },
            { Merchandise.Weed, (5, 10) },
            { Merchandise.Acid, (10, 10) },
            { Merchandise.Ecstacy, (10, 15) },
            { Merchandise.Heroin, (25, 15) },
            { Merchandise.Coke, (10, 25) }
        };

        // Used in a few places
        public static readonly Dictionary<Merchandise, string> MerchandiseIcons = new()
        {
            {Merchandise.Downers, "pack://application:,,/MarketGame;component/Icons/Downers.png" },
            {Merchandise.Weed, "pack://application:,,/MarketGame;component/Icons/Weed.png" },
            {Merchandise.Acid, "pack://application:,,/MarketGame;component/Icons/Acid.png" },
            {Merchandise.Ecstacy, "pack://application:,,/MarketGame;component/Icons/Ecstacy.png" },
            {Merchandise.Heroin, "pack://application:,,/MarketGame;component/Icons/Heroin.png" },
            {Merchandise.Coke, "pack://application:,,/MarketGame;component/Icons/Coke.png" }
        };

        public GameObject()
        {
            Character = new Player();
        }

        public void GenerateTipOff(MainWindow window)
        {
            Random random = new();
            
            // Decide if it will be buy or sell
            float modifier = (float)(random.NextDouble() * 2 - 1);
            bool isSell = modifier >= 0;

            // Decide on merch
            int merchIndex = random.Next(Enum.GetValues(typeof(Merchandise)).Length);

            if (isSell)
            {
                SellTip = (Merchandise)merchIndex;
                SellModifier = modifier;
                BuyTip = Merchandise.NotDefined;
            }
            else
            {
                BuyTip = (Merchandise)merchIndex;
                BuyModifier = modifier;
                SellTip = Merchandise.NotDefined;
            }

            // TODO: Decide on contact
            string contact = GangsterNames[random.Next(GangsterNames.Length)];
            string message = GenerateTipMessage(isSell);
            PushToNotification(message, contact, window, isSell);
        }

        private static string GenerateTipMessage(bool isSell)
        {
            Random random = new();
            string[] tips = isSell ? DialogueManager.SellTips : DialogueManager.BuyTips;
            return tips[random.Next(tips.Length)];
        }

        private void PushToNotification(string message, string contact, MainWindow window, bool isSell)
        {
            window.TipNotification.Name = contact;
            window.TipNotification.Message = ReplaceStringChunk(message, isSell);
            window.TipNotification.PathToImage = GetImagePath(contact);
        }

        private string GetImagePath(string name)
        {
            // There will always be n / 6 names
            int nameIndex = (Array.IndexOf(GangsterNames, name) % 6);

            string path = nameIndex switch
            {
                < 1 => "pack://application:,,,/MarketGame;component/Icons/Triad.png",
                < 2 => "pack://application:,,,/MarketGame;component/Icons/Mob.png",
                < 3 => "pack://application:,,,/MarketGame;component/Icons/Yardies.png",
                < 4 => "pack://application:,,,/MarketGame;component/Icons/Russian.png",
                < 5 => "pack://application:,,,/MarketGame;component/Icons/Syndicate.png",
                _ => "pack://application:,,,/MarketGame;component/Icons/Biker.png",
            };

            return path;
        }

        public string ReplaceStringChunk(string input, bool isSell)
        {
            if (SellTip == Merchandise.NotDefined && isSell)  return DialogueManager.PanicTextStrings[0];
            if (BuyTip == Merchandise.NotDefined && !isSell) return DialogueManager.PanicTextStrings[0];

            if (isSell) return input.Replace("{}", SellTip.ToString());
            else return input.Replace("{}", BuyTip.ToString());
        }
    }
}
