//
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using AltBeaconOrg.BoundBeacon;
//using Android.Util;
//
//namespace AndroidAltBeaconLibrary.Sample
//{
//	[Activity(Label = "Monitoring",
//		MainLauncher = false,
//		LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]			
//	public class MonitoringActivity : Activity, IDialogInterfaceOnDismissListener, IDialogInterfaceOnClickListener, IBeaconConsumer
//	{
//		protected const string TAG = "MonitoringActivity";
//		private BeaconManager beaconManager;
//		private RangeNotifier _rangeNotifier;
//		Region _emptyRegion;
//
//		public MonitoringActivity()
//		{
//			_rangeNotifier = new RangeNotifier();
//		}
//
//		protected override void OnCreate(Bundle bundle)
//		{
//			Log.Debug(TAG, "OnCreate");
//			base.OnCreate(bundle);
//
//			SetContentView(Resource.Layout.activity_monitoring);
//			VerifyBluetooth();
//
//			beaconManager = BeaconManager.GetInstanceForApplication(this);
//			beaconManager.Bind(this);
//		}
//
//		protected override void OnResume()
//		{
//			base.OnResume();
//
//			_rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;
//			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
//			((BeaconReferenceApplication) this.ApplicationContext).MonitoringActivity = this;
//		}
//
//		protected override void OnPause()
//		{
//			base.OnPause();
////			_rangeNotifier.DidRangeBeaconsInRegionComplete -= RangingBeaconsInRegion;
//			((BeaconReferenceApplication) this.ApplicationContext).MonitoringActivity = null;
//		}
//
//		private void VerifyBluetooth()
//		{
//			try
//			{
//				if(!BeaconManager.GetInstanceForApplication(this).CheckAvailability())
//				{
//					var builder = new AlertDialog.Builder(this);
//					builder.SetTitle("Bluetooth not enabled");
//					builder.SetMessage("Please enable bluetooth in settings and restart this application.");
//					builder.SetPositiveButton(Android.Resource.String.Ok, this);
//					builder.SetOnDismissListener(this);
//					builder.Show();
//				}
//			}
//			catch(AndroidRuntimeException ex)
//			{
//			}
//		}
//
//		public void OnDismiss(IDialogInterface dialog)
//		{
//			this.Finish();
//		}
//
//		public void OnClick(IDialogInterface dialog, int which)
//		{
//		}
//
//		public void LogToDisplay(string line) 
//		{
//			RunOnUiThread(() =>
//			{
//				var editText = this.FindViewById<EditText>(Resource.Id.monitoringText);
//				editText.Append(line + "\n");
//			});
//		}
//
//		void IBeaconConsumer.OnBeaconServiceConnect()
//		{
//			;
//			beaconManager.SetForegroundBetweenScanPeriod(2000);
//			beaconManager.SetBackgroundBetweenScanPeriod(2000);
//
//			_emptyRegion = new AltBeaconOrg.BoundBeacon.Region("myEmptyBeaconId", null, null, null);
//
//			beaconManager.SetRangeNotifier(_rangeNotifier);
//			beaconManager.StartRangingBeaconsInRegion(_emptyRegion);
//		}
//
//		async void RangingBeaconsInRegion(object sender, RangeEventArgs e)
//		{
//						await ClearData();
//			
//						var allBeacons = new List<Beacon>();
//						if(e.Beacons.Count > 0)
//						{
//							var beaconNumber = 0;
//							foreach(var b in e.Beacons)
//							{
//								allBeacons.Add(b);
//							}
//			
//							var orderedBeacons = allBeacons.OrderBy(b => b.Distance).ToList();
//			
//							orderedBeacons.ForEach(async (firstBeacon) =>
//							{
//								beaconNumber++;
//								await UpdateData(firstBeacon);
//							});
//						}
//						else
//						{
//							// unknown
//							await ClearData();
//						}
//		}
//	}
//}
//
