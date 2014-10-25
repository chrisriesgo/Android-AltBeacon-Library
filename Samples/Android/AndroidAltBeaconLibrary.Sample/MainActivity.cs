using System;

using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Graphics;

using AltBeaconOrg.BoundBeacon;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndroidAltBeaconLibrary.Sample
{
	[Activity(Label = "AltBeacon Sample", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity, IDialogInterfaceOnDismissListener, IBeaconConsumer
	{
		const string TAG = "MainActivity";

		private readonly MonitorNotifier _monitorNotifier;
		private readonly RangeNotifier _rangeNotifier;

		AltBeaconOrg.BoundBeacon.Region _tagRegion, _emptyRegion;

		private RelativeLayout _layout;
		private Button _backgroundButton, _stopButton, _startButton;
		private EditText _editText;
		private BeaconManager _beaconManager;
		private bool _paused;
		private long _lineCount = 0;

		public MainActivity()
		{
			_monitorNotifier = new MonitorNotifier();
			_rangeNotifier = new RangeNotifier();
		}

		protected override void OnCreate(Bundle bundle)
		{
			Log.WriteLine(LogPriority.Info, TAG, "OnCreate");
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.MainActivity);

			_layout = FindViewById<RelativeLayout>(Resource.Id.layout);
			_backgroundButton = FindViewById<Button>(Resource.Id.backgroundButton);
			_stopButton = FindViewById<Button>(Resource.Id.stopButton);
			_startButton = FindViewById<Button>(Resource.Id.startButton);
			_editText = FindViewById<EditText>(Resource.Id.monitoringText);

			VerityBluetooth();

			_beaconManager = BeaconManager.GetInstanceForApplication(this);
			// By default the AndroidBeaconLibrary will only find AltBeacons.  If you wish to make it
			// find a different type of beacon, you must specify the byte layout for that beacon's
			// advertisement with a line like below.  The example shows how to find a beacon with the
			// same byte layout as AltBeacon but with a beaconTypeCode of 0xaabb
			//
			// beaconManager.getBeaconParsers().add(new BeaconParser().
			//        setBeaconLayout("m:2-3=aabb,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25"));
			//
			// In order to find out the proper BeaconLayout definition for other kinds of beacons, do
			// a Google search for "setBeaconLayout" (including the quotes in your search.)

			var iBeaconParser = new BeaconParser();
			//	Estimote > 2013
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			_beaconManager.BeaconParsers.Add(iBeaconParser);

			_beaconManager.Bind(this);
			_beaconManager.SetBackgroundMode(false);

			_monitorNotifier.EnterRegionComplete += EnteredRegion;
			_monitorNotifier.ExitRegionComplete += ExitedRegion;
			_monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;
		}

		protected override void OnResume()
		{
			base.OnResume();
			_paused = false;

			_backgroundButton.Click += OnBackgroundClick;
			_stopButton.Click += OnStopClick;
			_startButton.Click += OnStartClick;

			if(_beaconManager.IsBound(this))
			{
				_beaconManager.SetBackgroundMode(false);
			}
		}

		void OnStopClick (object sender, EventArgs e)
		{
			_stopButton.Enabled = false;
			_startButton.Enabled = true;

			_beaconManager.StopMonitoringBeaconsInRegion(_tagRegion);
			_beaconManager.StopRangingBeaconsInRegion(_tagRegion);

			_beaconManager.StopMonitoringBeaconsInRegion(_emptyRegion);
			_beaconManager.StopRangingBeaconsInRegion(_emptyRegion);
		}

		void OnStartClick(object sender, EventArgs e)
		{
			_startButton.Enabled = false;
			_stopButton.Enabled = true;

			_editText.Text = string.Empty;
			_lineCount = 0;

			_beaconManager.StartMonitoringBeaconsInRegion(_tagRegion);
			_beaconManager.StartRangingBeaconsInRegion(_tagRegion);

			_beaconManager.StartMonitoringBeaconsInRegion(_emptyRegion);
			_beaconManager.StartRangingBeaconsInRegion(_emptyRegion);
		}

		protected override void OnPause()
		{
			_paused = true;
			base.OnPause();

			_backgroundButton.Click -= OnBackgroundClick;
			_stopButton.Click -= OnStopClick;
			_startButton.Click -= OnStartClick;

			if(_beaconManager.IsBound(this))
			{
				_beaconManager.SetBackgroundMode(true);
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if(_beaconManager.IsBound(this)) _beaconManager.Unbind(this);
		}

		async void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			if(e.Beacons.Count > 0)
			{
				var beaconNumber = 0;
				var allBeacons = new List<Beacon>();
				foreach(var b in e.Beacons)
				{
					allBeacons.Add(b);
				}

				var orderedBeacons = allBeacons.OrderBy(b => b.Distance).ToList();

				orderedBeacons.ForEach(async (firstBeacon) =>
				{
					beaconNumber++;
					if(firstBeacon.Distance <= .5)
					{
						// ~immediate
						await UpdateDisplay("Beacon " + beaconNumber + " of " + orderedBeacons.Count + "\n" + firstBeacon.Id1 + "\nis about " + string.Format("{0:N2}", firstBeacon.Distance) + " meters away.", Color.Red);
					}
					else if(firstBeacon.Distance > .5 && firstBeacon.Distance <= 10)
					{
						// ~near
						await UpdateDisplay("Beacon " + beaconNumber + " of " + orderedBeacons.Count + "\n" + firstBeacon.Id1 + "\nis about " + string.Format("{0:N2}", firstBeacon.Distance) + " meters away.", Color.Yellow);
					}
					else if(firstBeacon.Distance > 10)
					{
						// ~far
						await UpdateDisplay("Beacon " + beaconNumber + " of " + orderedBeacons.Count + "\n" + firstBeacon.Id1 + "\nis about " + string.Format("{0:N2}", firstBeacon.Distance) + " meters away.", Color.Blue);
					}
					else
					{
						// ~unknown
						await UpdateDisplay("I'm not sure how close you are to the " + beaconNumber + " of " + orderedBeacons.Count + " beacon.\n", Color.Transparent);
					}
				});
			}
			else
			{
				// ~unknown
				await UpdateDisplay("I don't see any beacons nearby.", Color.Transparent);
			}
		}

		void OnBackgroundClick(object sender, EventArgs e)
		{
			var intent = new Intent(this, typeof(BackgroundActivity));
			StartActivity(intent);
		}

		void VerityBluetooth()
		{
			try 
			{
				if (!BeaconManager.GetInstanceForApplication(this).CheckAvailability())
				{
					var builder = new AlertDialog.Builder(this);
					builder.SetTitle("Bluetooth not enabled");
					builder.SetMessage("Please enable bluetooth in settings and restart this application.");
					EventHandler<DialogClickEventArgs> handler = null;
					builder.SetPositiveButton(Android.Resource.String.Ok, handler);
					builder.SetOnDismissListener(this);
					builder.Show();
				}		
			}
			catch (BleNotAvailableException e) 
			{
				Log.Debug("BleNotAvailableException", e.Message);

				var builder = new AlertDialog.Builder(this);
				builder.SetTitle("Bluetooth LE not available");
				builder.SetMessage("Sorry, this device does not support Bluetooth LE.");
				EventHandler<DialogClickEventArgs> handler = null;
				builder.SetPositiveButton(Android.Resource.String.Ok, handler);
				builder.SetOnDismissListener(this);
				builder.Show();
			}
		}

		private async Task UpdateDisplay(string message, Color color = default(Color))
		{
			Console.WriteLine(message);
			await Task.Run(() =>
			{
				RunOnUiThread(() =>
				{
					_lineCount++;
					_layout.SetBackgroundColor(color);
					_editText.Append(_lineCount + ": " + message + "\n\n");
				});
			});
		}

		async void EnteredRegion(object sender, MonitorEventArgs e)
		{
			await UpdateDisplay("A new beacon just showed up!");
		}

		async void ExitedRegion(object sender, MonitorEventArgs e)
		{
			await UpdateDisplay("They went away :(");
		}

		async void DeterminedStateForRegionComplete(object sender, MonitorEventArgs e)
		{
			await UpdateDisplay("I have just switched from seeing/not seeing beacons: " + e.State);
		}

		public bool BindService(Intent p0, IServiceConnection p1, int p2)
		{
			return true;
		}

		public void OnBeaconServiceConnect()
		{
			_beaconManager.SetMonitorNotifier(_monitorNotifier); 
			_beaconManager.SetRangeNotifier(_rangeNotifier);

			_tagRegion = new AltBeaconOrg.BoundBeacon.Region("myUniqueBeaconId", Identifier.Parse("2F234454-CF6D-4A0F-ADF2-F4911BA9FFA6"), null, null);
			_emptyRegion = new AltBeaconOrg.BoundBeacon.Region("myEmptyBeaconId", null, null, null);

			_beaconManager.StartMonitoringBeaconsInRegion(_tagRegion);
			_beaconManager.StartRangingBeaconsInRegion(_tagRegion);

			_beaconManager.StartMonitoringBeaconsInRegion(_emptyRegion);
			_beaconManager.StartRangingBeaconsInRegion(_emptyRegion);

			_startButton.Enabled = false;
		}

		public void OnDismiss(IDialogInterface dialog)
		{
			Finish();
		}
	}
}


