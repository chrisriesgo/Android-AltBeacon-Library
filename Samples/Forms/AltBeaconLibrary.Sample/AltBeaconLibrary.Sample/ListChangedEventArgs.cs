using System;

namespace AltBeaconLibrary.Sample
{

	public class ListChangedEventArgs : EventArgs
	{
		public System.Collections.Generic.List<CommonBeacon> Data { get; protected set; }
		public ListChangedEventArgs(System.Collections.Generic.List<CommonBeacon> data)
		{
			Data = data;
		}
	}
}
