
using System;
using AltBeaconOrg.BoundBeacon;
using Android;
using Android.Annotation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Java.Lang;
using Debug = System.Diagnostics.Debug;

namespace BeaconReference.Droid
{
	[Activity(MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]
	public class MonitoringActivity : Activity, IDialogInterfaceOnDismissListener
	{
		protected const string TAG = "MonitoringActivity";
		const int PERMISSION_REQUEST_COARSE_LOCATION = 1;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			Debug.WriteLine("onCreate", TAG);
			base.OnCreate(savedInstanceState);
			Title = "Monitoring";
			SetContentView(Resource.Layout.activity_monitoring);

			VerifyBluetooth();
			LogToDisplay("Application just launched");

			if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
			{
				// Android M Permission check
				if (this.CheckSelfPermission(Manifest.Permission.AccessCoarseLocation) != Android.Content.PM.Permission.Granted)
				{
					var builder = new AlertDialog.Builder(this);
					builder.SetTitle("This app needs location access");
					builder.SetMessage("Please grant location access so this app can detect beacons in the background.");
					builder.SetPositiveButton(Android.Resource.String.Ok, (sender, args) => { });
					builder.SetOnDismissListener(this);
					builder.Show();
				}
			}
		}
		
		protected override void OnResume()
		{
			base.OnResume();
			if (this.ApplicationContext is BeaconReferenceApplication app) {
				app.SetMonitoringActivity(this);
			}
		}
		
		protected override void OnPause()
		{
			base.OnPause();
			if (this.ApplicationContext is BeaconReferenceApplication app) {
				app.SetMonitoringActivity(null);
			}
		}
		
		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			
			switch (requestCode) {
				case PERMISSION_REQUEST_COARSE_LOCATION: {
					if (grantResults[0] == Android.Content.PM.Permission.Granted) {
						Debug.WriteLine("coarse location permission granted", TAG);
					} 
					else {
						var builder = new AlertDialog.Builder(this);
						builder.SetTitle("Functionality limited");
						builder.SetMessage("Since location access has not been granted, this app will not be able to discover beacons when in the background.");
						builder.SetPositiveButton(Android.Resource.String.Ok, (obj, sender) => { });
						builder.SetOnDismissListener(null);
						builder.Show();
					}
					return;
				}
			}
		}
		
		void VerifyBluetooth() 
		{

			try {
				if (!BeaconManager.GetInstanceForApplication(this).CheckAvailability()) {
					var builder = new AlertDialog.Builder(this);
					builder.SetTitle("Bluetooth not enabled");			
					builder.SetMessage("Please enable bluetooth in settings and restart this application.");
					builder.SetPositiveButton(Android.Resource.String.Ok, (obj, sender) => { });
					builder.SetOnDismissListener(new DialogExitOnDismissListener(this));
					builder.Show();
				}			
			}
			catch (RuntimeException ex) {
				var builder = new AlertDialog.Builder(this);
				builder.SetTitle("Bluetooth LE not available");			
				builder.SetMessage("Sorry, this device does not support Bluetooth LE.");
				builder.SetPositiveButton(Android.Resource.String.Ok, (obj, sender) => { });
				builder.SetOnDismissListener(new DialogExitOnDismissListener(this));
				builder.Show();				
			}
			
		}

		[Export("OnRangingClicked")]
		public void OnRangingClicked(View view) 
		{
			Intent myIntent = new Intent(this, typeof(RangingActivity));
			this.StartActivity(myIntent);
		}
	
		public void LogToDisplay(string line)
		{
			RunOnUiThread(() =>
			{
				var editText = FindViewById<EditText>(Resource.Id.monitoringText);
				editText.Append(line + "\n");
			});
		}

		#region IDialogInterfaceOnDismissListener implementation
		[TargetApi(Value = 23)]
		public void OnDismiss(IDialogInterface dialog)
		{
			RequestPermissions(new string[]{Manifest.Permission.AccessCoarseLocation},
	                                PERMISSION_REQUEST_COARSE_LOCATION);
		}
		#endregion

		class DialogExitOnDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
		{
			WeakReference<Activity> _weakActivity;

			public DialogExitOnDismissListener(Activity activity)
			{
				_weakActivity = new WeakReference<Activity>(activity);
			}
			
			public void OnDismiss(IDialogInterface dialog)
			{
				if (_weakActivity.TryGetTarget(out Activity activity))
				{
					activity.Finish();
					JavaSystem.Exit(0);
				}
			}
		}
	}
}
