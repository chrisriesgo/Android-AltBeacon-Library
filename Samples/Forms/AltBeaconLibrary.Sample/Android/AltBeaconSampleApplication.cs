using System;
using Android.App;
using AltBeaconOrg.BoundBeacon.Startup;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Powersave;
using Android.Content;

namespace AltBeaconLibrary.Sample.Droid
{
	public class AltBeaconSampleApplication : Application, IBootstrapNotifier
	{
		BackgroundPowerSaver backgroundPowerSaver;

		public AltBeaconSampleApplication()
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
			backgroundPowerSaver = new BackgroundPowerSaver(this);
		}

		public void DidEnterRegion(Region region)
		{
		}

		public void DidDetermineStateForRegion(int state, Region region)
		{
		}

		public void DidExitRegion(Region region)
		{
		}
	}
}

