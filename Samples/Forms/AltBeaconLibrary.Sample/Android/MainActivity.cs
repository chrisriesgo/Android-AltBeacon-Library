using System;

using Android.App;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Org.Altbeacon.Beacon;
using Android.Content;

namespace AltBeaconLibrary.Sample.Droid
{
	[Activity(Label = "AltBeacon Forms Sample",
		Theme = "@style/Theme.AltBeacon",
		Icon = "@drawable/altbeacon",
		MainLauncher = true,
		LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
	public class MainActivity : FormsApplicationActivity, IBeaconConsumer
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Xamarin.Forms.Forms.Init(this, savedInstanceState);

			LoadApplication(new App());
		}

        public bool BindService(Intent p0, IServiceConnection p1, int p2)
        {
            return true;
        }

        public void OnBeaconServiceConnect()
        {
            var beaconService = Xamarin.Forms.DependencyService.Get<IAltBeaconService>();

            beaconService.StartMonitoring();
            beaconService.StartRanging();
        }
    }
}

