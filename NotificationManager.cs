using System.Windows.Threading;
using System.Windows.Media;

namespace MarketGame 
{
	public class TimerEventArgs : EventArgs 
	{
		public DispatcherTimer Timer {get;}

		public TimerEventArgs(DispatcherTimer timer) 
		{
			Timer = timer;
		}
	}

}
