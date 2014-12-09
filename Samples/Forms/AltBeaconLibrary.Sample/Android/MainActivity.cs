using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using AltBeaconOrg.BoundBeacon;


namespace AltBeaconLibrary.Sample.Android
{
	[Activity(Label = "AltBeaconLibrary.Sample.Android.Android", MainLauncher = true]
	public class MainActivity : AndroidActivity, IBeaconConsumer
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			Xamarin.Forms.Forms.Init(this, bundle);

			SetPage(App.GetMainPage());
		}

		public void OnBeaconServiceConnect()
		{
			Console.WriteLine("Starting");
		}
	}
}

