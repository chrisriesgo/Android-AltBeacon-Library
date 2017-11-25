using System.Collections.Generic;
using System.Linq;
using Org.Altbeacon.Beacon;
using Android.App;
using Android.OS;
using Android.Widget;

namespace BeaconReference.Droid
{
	[Activity]
	public class RangingActivity : Activity, IBeaconConsumer, IRangeNotifier
	{
		protected const string TAG = "RangingActivity";
		BeaconManager beaconManager = null;

		public RangingActivity()
		{
			beaconManager = BeaconManager.GetInstanceForApplication(this);
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			
			Title = "Ranging";
			SetContentView(Resource.Layout.activity_ranging);

			beaconManager.Bind(this);
		}
		
		protected override void OnDestroy()
		{
			base.OnDestroy();
			beaconManager.Unbind(this);
		}
		
		protected override void OnPause()
		{
			base.OnPause();
			if (beaconManager.IsBound(this)) beaconManager.SetBackgroundMode(true);
		}
		
		protected override void OnResume()
		{
			base.OnResume();
			if (beaconManager.IsBound(this)) beaconManager.SetBackgroundMode(false);
		}

		public void LogToDisplay(string line)
		{
			RunOnUiThread(() =>
			{
				var editText = FindViewById<EditText>(Resource.Id.rangingText);
       	    	editText.Append(line+"\n");
			});
		}

		#region IBeaconConsumer implementation
		public void OnBeaconServiceConnect()
		{
			beaconManager.AddRangeNotifier(this);
	
	        try {
            	beaconManager.StartRangingBeaconsInRegion(new Region("myRangingUniqueId", null, null, null));
			} 
			catch (RemoteException ex) {   
			}
		}
		#endregion
		
		#region IRangeNotifier implementation
		public void DidRangeBeaconsInRegion(ICollection<Beacon> beacons, Region region)
		{
			if (beacons.Count > 0) {
				Beacon firstBeacon = beacons.First();
				LogToDisplay("The first beacon " + firstBeacon.ToString() + " is about " + firstBeacon.Distance + " meters away.");
			}
		}
		#endregion
	}
}
