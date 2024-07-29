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
            float StashPercentage = (float)game.Character.GetTotalCapacity(true) / 1500 * 100;
            float BagPercentage = (float)game.Character.GetTotalCapacity(false) / 150 * 100;
            stashBar.Value = StashPercentage;
            bagBar.Value = BagPercentage;
            stashLabel.Content = game.Character.GetTotalCapacity(true).ToString() + "/ 1500";
            bagLabel.Content = game.Character.GetTotalCapacity(false).ToString() + "/ 150";
        }
    }
}
