using System;
using AltBeaconOrg.BoundBeacon;
using AltBeaconLibrary.Sample.Droid.Services;
using Android.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(AltBeaconService))]

namespace AltBeaconLibrary.Sample.Droid.Services
{
	public class AltBeaconService : Java.Lang.Object, IAltBeaconService
	{
		private readonly MonitorNotifier _monitorNotifier;
		private readonly RangeNotifier _rangeNotifier;
		private BeaconManager _beaconManager;

		Region _tagRegion;

		Region _emptyRegion;
		private ListView _list;
		private readonly List<Beacon> _data;

		public AltBeaconService()
		{
			_monitorNotifier = new MonitorNotifier();
			_rangeNotifier = new RangeNotifier();
			_data = new List<Beacon>();
		}

		public event EventHandler<ListChangedEventArgs> ListChanged;
		public event EventHandler DataClearing;

		public BeaconManager BeaconManagerImpl
		{
			get {
				if (_beaconManager == null)
				{
					_beaconManager = InitializeBeaconManager();
				}
				return _beaconManager;
			}
		}

		public void InitializeService()
		{
			_beaconManager = InitializeBeaconManager();
		}

		private BeaconManager InitializeBeaconManager()
		{
			// Enable the BeaconManager 
			BeaconManager bm = BeaconManager.GetInstanceForApplication(Xamarin.Forms.Forms.Context);

			#region Set up Beacon Simulator if testing without a BLE device
//			var beaconSimulator = new BeaconSimulator();
//			beaconSimulator.CreateBasicSimulatedBeacons();
//
//			BeaconManager.BeaconSimulator = beaconSimulator;
			#endregion

			var iBeaconParser = new BeaconParser();
			//	Estimote > 2013
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			bm.BeaconParsers.Add(iBeaconParser);

			_monitorNotifier.EnterRegionComplete += EnteredRegion;
			_monitorNotifier.ExitRegionComplete += ExitedRegion;
			_monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

			_tagRegion = new AltBeaconOrg.BoundBeacon.Region("myUniqueBeaconId", Identifier.Parse("E4C8A4FC-F68B-470D-959F-29382AF72CE7"), null, null);
			_emptyRegion = new AltBeaconOrg.BoundBeacon.Region("myEmptyBeaconId", null, null, null);

			bm.SetBackgroundMode(false);
			bm.Bind((IBeaconConsumer)Xamarin.Forms.Forms.Context);

			return bm;
		}

		public void StartMonitoring()
		{
			BeaconManagerImpl.SetForegroundBetweenScanPeriod(5000); // 5000 milliseconds

			BeaconManagerImpl.SetMonitorNotifier(_monitorNotifier); 
			_beaconManager.StartMonitoringBeaconsInRegion(_tagRegion);
			_beaconManager.StartMonitoringBeaconsInRegion(_emptyRegion);
		}

		public void StartRanging()
		{
			BeaconManagerImpl.SetForegroundBetweenScanPeriod(5000); // 5000 milliseconds

			BeaconManagerImpl.SetRangeNotifier(_rangeNotifier);
			_beaconManager.StartRangingBeaconsInRegion(_tagRegion);
			_beaconManager.StartRangingBeaconsInRegion(_emptyRegion);
		}

		private void DeterminedStateForRegionComplete(object sender, MonitorEventArgs e)
		{
			Console.WriteLine("DeterminedStateForRegionComplete");
		}

		private void ExitedRegion(object sender, MonitorEventArgs e)
		{
			Console.WriteLine("ExitedRegion");
		}

		private void EnteredRegion(object sender, MonitorEventArgs e)
		{
			Console.WriteLine("EnteredRegion");
		}

		async void RangingBeaconsInRegion(object sender, RangeEventArgs e)
		{
			await ClearData();

			var allBeacons = new List<Beacon>();
			if(e.Beacons.Count > 0)
			{
				var beaconNumber = 0;
				foreach(var b in e.Beacons)
				{
					allBeacons.Add(b);
				}

				var orderedBeacons = allBeacons.OrderBy(b => b.Distance).ToList();

				orderedBeacons.ForEach(async (firstBeacon) =>
				{
					beaconNumber++;
					await UpdateData(firstBeacon);
				});
			}
			else
			{
				// unknown
				await ClearData();
			}
		}

		private async Task UpdateData(Beacon beacon)
		{
			await Task.Run(() =>
			{
				if(_data.All(d => d.Id1.ToString() != beacon.Id1.ToString()))
				{
					((Activity)Xamarin.Forms.Forms.Context).RunOnUiThread(() =>
					{
						_data.Add(beacon);
						_data.Sort((x,y) => x.Distance.CompareTo(y.Distance));
						UpdateList();
					});
				}
			});
		}

		private async Task ClearData()
		{
			((Activity)Xamarin.Forms.Forms.Context).RunOnUiThread(() =>
			{
				_data.Clear();
				OnDataClearing();
			});
		}

		private void OnDataClearing()
		{
			var handler = DataClearing;
			if(handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		private void UpdateList()
		{
			((Activity)Xamarin.Forms.Forms.Context).RunOnUiThread(() => 
			{
				OnListChanged();
			});
		}

		private void OnListChanged()
		{
			var handler = ListChanged;
			if(handler != null)
			{
				var data = new List<SharedBeacon>();
				_data.ForEach(b =>
				{
					data.Add(new SharedBeacon { Id = b.Id1.ToString(), Distance = string.Format("{0:N2}m", b.Distance)});
				});
				handler(this, new ListChangedEventArgs(data));
			}
		}
	}
}

