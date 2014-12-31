using System;

namespace AltBeaconLibrary.Sample
{
	public interface IAltBeaconService
	{
		void InitializeService();
		void StartMonitoring();
		void StartRanging();

		event EventHandler<ListChangedEventArgs> ListChanged;
		event EventHandler DataClearing;
	}
}