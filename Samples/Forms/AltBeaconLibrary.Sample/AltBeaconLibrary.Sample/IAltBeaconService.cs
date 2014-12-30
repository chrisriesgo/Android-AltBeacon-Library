using System;

namespace AltBeaconLibrary.Sample
{
	public interface IAltBeaconService
	{
		void StartMonitoring(string identifier);
		void StartRanging(string identifier);
		void OnBeaconServiceConnect();
		event EventHandler<ListChangedEventArgs> ListChanged;
	}

	public class ListChangedEventArgs : EventArgs
	{
		public System.Collections.Generic.List<CommonBeacon> Data { get; protected set; }
		public ListChangedEventArgs(System.Collections.Generic.List<CommonBeacon> data)
		{
			Data = data;
		}
	}
}

