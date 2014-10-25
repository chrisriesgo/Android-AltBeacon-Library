using System;
using AltBeaconOrg.BoundBeacon;
using System.Collections.Generic;

namespace AndroidAltBeaconLibrary.Sample
{
	public class RangeEventArgs : EventArgs
	{
		public Region Region { get; set; }
		public ICollection<Beacon> Beacons { get; set; }
	}

	public class RangeNotifier : Java.Lang.Object, IRangeNotifier
	{
		public event EventHandler<RangeEventArgs> DidRangeBeaconsInRegionComplete;

		public void DidRangeBeaconsInRegion(ICollection<Beacon> beacons, Region region)
		{
			OnDidRangeBeaconsInRegion(beacons, region);
		}

		private void OnDidRangeBeaconsInRegion(ICollection<Beacon> beacons, Region region)
		{
			if (DidRangeBeaconsInRegionComplete != null)
			{
				DidRangeBeaconsInRegionComplete(this, new RangeEventArgs { Beacons = beacons, Region = region });
			}
		}
	}
}

