using System;
using Android.App;
using AltBeaconOrg.BoundBeacon.Startup;
using AltBeaconOrg.BoundBeacon;
using Android.Support.V4.App;
using Android.Content;
using AltBeaconOrg.BoundBeacon.Powersave;
using Android.Util;

namespace AltBeaconLibrary.Sample.Droid
{
	[Application(Label = "AltBeacon Sample", Icon = "@drawable/altbeacon")]
	public class AltBeaconSampleApplication : Application, IBootstrapNotifier
	{
		private const string TAG = "AltBeaconSampleApplication";

		BeaconManager _beaconManager;

		private RegionBootstrap regionBootstrap;
		private Region _backgroundRegion;
		private BackgroundPowerSaver backgroundPowerSaver;
		private bool haveDetectedBeaconsSinceBoot = false;

		private MainActivity mainActivity = null;
		public MainActivity MainActivity
		{
			get { return mainActivity; }
			set { mainActivity = value; }
		}

		public AltBeaconSampleApplication() : base() { }
		public AltBeaconSampleApplication(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override void OnCreate()
		{
			base.OnCreate();

			_beaconManager = BeaconManager.GetInstanceForApplication(this);

			var iBeaconParser = new BeaconParser();
			//	Estimote > 2013
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			_beaconManager.BeaconParsers.Add(iBeaconParser);

			Log.Debug(TAG, "setting up background monitoring for beacons and power saving");
			// wake up the app when a beacon is seen
			_backgroundRegion = new Region("backgroundRegion", null, null, null);
			regionBootstrap = new RegionBootstrap(this, _backgroundRegion);

			// simply constructing this class and holding a reference to it in your custom Application
			// class will automatically cause the BeaconLibrary to save battery whenever the application
			// is not visible.  This reduces bluetooth power usage by about 60%
			backgroundPowerSaver = new BackgroundPowerSaver(this);
		}

		public void DidDetermineStateForRegion(int state, AltBeaconOrg.BoundBeacon.Region region)
		{
		}

		public void DidEnterRegion(AltBeaconOrg.BoundBeacon.Region region)
		{
			// In this example, this class sends a notification to the user whenever a Beacon
			// matching a Region (defined above) are first seen.
			Log.Debug(TAG, "did enter region.");
			if(!haveDetectedBeaconsSinceBoot)
			{
				Log.Debug(TAG, "auto launching MonitoringActivity");

				// The very first time since boot that we detect an beacon, we launch the
				// MainActivity
				var intent = new Intent(this, typeof(MainActivity));
				intent.SetFlags(ActivityFlags.NewTask);
				// Important:  make sure to add android:launchMode="singleInstance" in the manifest
				// to keep multiple copies of this activity from getting created if the user has
				// already manually launched the app.
				this.StartActivity(intent);
				haveDetectedBeaconsSinceBoot = true;
			}
			else
			{
				if(mainActivity != null)
				{
					Log.Debug(TAG, "I see a beacon again");
				}
				else
				{
					// If we have already seen beacons before, but the monitoring activity is not in
					// the foreground, we send a notification to the user on subsequent detections.
					Log.Debug(TAG, "Sending notification.");
					SendNotification();
				}
			}
		}

		public void DidExitRegion(AltBeaconOrg.BoundBeacon.Region region)
		{
			Log.Debug(TAG, "did exit region.");
		}

		private void SendNotification()
		{
			var builder =
				new NotificationCompat.Builder(this)
					.SetContentTitle("AltBeacon Reference Application")
					.SetContentText("A beacon is nearby.")
					.SetSmallIcon(Android.Resource.Drawable.IcDialogInfo);

			var stackBuilder = Android.App.TaskStackBuilder.Create(this);
			stackBuilder.AddNextIntent(new Intent(this, typeof(MainActivity)));
			var resultPendingIntent =
				stackBuilder.GetPendingIntent(
					0,
					PendingIntentFlags.UpdateCurrent
				);
			builder.SetContentIntent(resultPendingIntent);
			var notificationManager =
				(NotificationManager)this.GetSystemService(Context.NotificationService);
			notificationManager.Notify(1, builder.Build());
		}
	}
}

