using System;

using Android.App;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using AltBeaconOrg.BoundBeacon;


namespace AltBeaconLibrary.Sample.Droid
{
	[Activity(Label = "AltBeacon Forms Sample", 
		Theme = "@style/Theme.AltBeacon",
		Icon = "@drawable/altbeacon",
		MainLauncher = true)]
	public class MainActivity : FormsApplicationActivity, IBeaconConsumer
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Xamarin.Forms.Forms.Init(this, savedInstanceState);

			LoadApplication(new App());
		}

		public void OnBeaconServiceConnect()
		{
			var beaconService = Xamarin.Forms.DependencyService.Get<IAltBeaconService>();

			beaconService.StartMonitoring("E4C8A4FC-F68B-470D-959F-29382AF72CE7");
			beaconService.StartRanging("E4C8A4FC-F68B-470D-959F-29382AF72CE7");
		}
	}
}

