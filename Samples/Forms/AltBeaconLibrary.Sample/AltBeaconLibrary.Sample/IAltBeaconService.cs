using System;

namespace AltBeaconLibrary.Sample
{
	public interface IAltBeaconService
	{
		void InitializeService();
		void StartMonitoring(string identifier);
		void StartRanging(string identifier);

		event EventHandler<ListChangedEventArgs> ListChanged;
		event EventHandler DataClearing;
	}
}