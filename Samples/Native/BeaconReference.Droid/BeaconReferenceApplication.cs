using System;
using Android.App;
using Org.Altbeacon.Beacon.Startup;
using Org.Altbeacon.Beacon;
using Org.Altbeacon.Beacon.Powersave;
using System.Diagnostics;
using Android.Content;
using NotificationCompat = Android.Support.V4.App.NotificationCompat;
using Android.Runtime;

namespace BeaconReference.Droid
{
	[Application]
	public class BeaconReferenceApplication : Application, IBootstrapNotifier
	{
		const string TAG = "BeaconReferenceApp";

		RegionBootstrap regionBootstrap;
		BackgroundPowerSaver backgroundPowerSaver;
		bool haveDetectedBeaconsSinceBoot = false;
		MonitoringActivity monitoringActivity = null;
		
		public BeaconReferenceApplication(IntPtr handle, JniHandleOwnership ownerShip) : base(handle, ownerShip)
	    {
	    }

		public override void OnCreate()
		{
			base.OnCreate();

			var beaconManager = BeaconManager.GetInstanceForApplication(this);

			// By default the AndroidBeaconLibrary will only find AltBeacons.  If you wish to make it
			// find a different type of beacon, you must specify the byte layout for that beacon's
			// advertisement with a line like below.  The example shows how to find a beacon with the
			// same byte layout as AltBeacon but with a beaconTypeCode of 0xaabb.  To find the proper
			// layout expression for other beacon types, do a web search for "setBeaconLayout"
			// including the quotes.
			//
			//beaconManager.getBeaconParsers().clear();
			//beaconManager.getBeaconParsers().add(new BeaconParser().
			//        setBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25"));
			//
			// Estimote
			// iBeacons
			//beaconManager.getBeaconParsers().add(new BeaconParser().
			//	      setBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24"));
			
			var altBeaconParser = new BeaconParser();
			altBeaconParser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
			beaconManager.BeaconParsers.Add(altBeaconParser);
			
			var iBeaconParser = new BeaconParser();
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			beaconManager.BeaconParsers.Add(iBeaconParser);

			Debug.WriteLine("setting up background monitoring for beacons and power saving", TAG);

			// wake up the app when a beacon is seen
			var region = new Region("backgroundRegion", null, null, null);
			regionBootstrap = new RegionBootstrap(this, region);

			// simply constructing this class and holding a reference to it in your custom Application
			// class will automatically cause the BeaconLibrary to save battery whenever the application
			// is not visible.  This reduces bluetooth power usage by about 60%
			backgroundPowerSaver = new BackgroundPowerSaver(this);

			// If you wish to test beacon detection in the Android Emulator, you can use code like this:
			//BeaconManager.BeaconSimulator = new TimedBeaconSimulator();
			//if (BeaconManager.BeaconSimulator is TimedBeaconSimulator simulator) {
			//	//simulator.CreateTimedSimulatedBeacons();
			//	simulator.CreateBasicSimulatedBeacons();
			//}
		}
		
		public void SetMonitoringActivity(MonitoringActivity activity) 
		{
	        monitoringActivity = activity;
	    }

		#region IBootstrapNotifier implementation
		public void DidEnterRegion(Region region)
		{
			// In this example, this class sends a notification to the user whenever a Beacon
			// matching a Region (defined above) are first seen.
			Debug.WriteLine("did enter region.", TAG);

			if (!haveDetectedBeaconsSinceBoot)
			{
				Debug.WriteLine("auto launching MainActivity", TAG);

				// The very first time since boot that we detect an beacon, we launch the
				// MainActivity
				var intent = new Intent(this, typeof(MonitoringActivity));
				intent.SetFlags(ActivityFlags.NewTask);

				// Important:  make sure to add android:launchMode="singleInstance" in the manifest
				// to keep multiple copies of this activity from getting created if the user has
				// already manually launched the app.
				this.StartActivity(intent);

				haveDetectedBeaconsSinceBoot = true;
			}
			else
			{
				if (monitoringActivity != null)
				{
					// If the Monitoring Activity is visible, we log info about the beacons we have
					// seen on its display
					monitoringActivity.LogToDisplay("I see a beacon again");
				}
				else
				{
					// If we have already seen beacons before, but the monitoring activity is not in
					// the foreground, we send a notification to the user on subsequent detections.
					Debug.WriteLine("Sending notification.", TAG);
					SendNotification();
				}
			}
		}

		public void DidExitRegion(Region region)
		{
			if (monitoringActivity != null)
			{
				monitoringActivity.LogToDisplay("I no longer see a beacon.");
			}
		}

		public void DidDetermineStateForRegion(int state, Region region)
		{
			if (monitoringActivity != null)
			{
				monitoringActivity.LogToDisplay("I have just switched from seeing/not seeing beacons: " + state);
			}
		}
		#endregion

		void SendNotification()
		{
			NotificationCompat.Builder builder =
					new NotificationCompat.Builder(this)
							.SetContentTitle("Beacon Reference Application")
							.SetContentText("An beacon is nearby.")
							.SetSmallIcon(Resource.Mipmap.Icon);

			var stackBuilder = TaskStackBuilder.Create(this);
			stackBuilder.AddNextIntent(new Intent(this, typeof(MonitoringActivity)));
			PendingIntent resultPendingIntent =
					stackBuilder.GetPendingIntent(
							0,
							PendingIntentFlags.UpdateCurrent
					);
			builder.SetContentIntent(resultPendingIntent);
			NotificationManager notificationManager =
					(NotificationManager)this.GetSystemService(Context.NotificationService);
			notificationManager.Notify(1, builder.Build());
		}
	}
}
