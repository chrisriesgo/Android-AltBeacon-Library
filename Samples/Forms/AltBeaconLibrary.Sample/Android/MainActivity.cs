using System;

using Android.App;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using AltBeaconOrg.BoundBeacon;


namespace AltBeaconLibrary.Sample.Droid
{
	[Activity(Label = "AltBeaconLibrary.Sample.Android.Android", MainLauncher = true)]
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
			beaconService.OnBeaconServiceConnect();
			Console.WriteLine("Starting");
		}
	}
}

