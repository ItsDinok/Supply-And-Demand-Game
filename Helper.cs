using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

// Helper function to avoid repeated code
namespace MarketGame
{
    public static class Helper
    {
        public static void SetCapacityBars(ProgressBar stashBar, ProgressBar bagBar, Label stashLabel, Label bagLabel, GameObject game)
        {
            // These are used to calculate the value to display on the bar
            float StashPercentage = (float)(game.Character.GetTotalCapacity(true) / 1500) * 100;
            float BagPercentage = (float)(game.Character.GetTotalCapacity(false) / 150) * 100;
            
            
            stashBar.Value = StashPercentage;
            bagBar.Value = BagPercentage;

            UpdateLabels(stashLabel, bagLabel, game);
        }
        private static void UpdateLabels(Label stashLabel, Label bagLabel, GameObject game)
        {
            stashLabel.Content = game.Character.GetTotalCapacity(true).ToString() + "/ 1500";
            bagLabel.Content = game.Character.GetTotalCapacity(false).ToString() + "/ 150";
        }

        public static string ReturnMoneyString(int amount)
        {
            string toWork = amount.ToString();
            int index = 1;

            for (int i = toWork.Length - 1; i >= 0; i--)
            {
                // Must be where a comma goes
                if (index % 3 == 0 && i != 0)
                {
                    toWork = toWork.Insert(i, ",");
                }
                index++;
            }

            return "$" + toWork;
        }
    }

}
