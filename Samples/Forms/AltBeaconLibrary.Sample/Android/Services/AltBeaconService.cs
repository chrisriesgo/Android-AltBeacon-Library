using System;
using AltBeaconOrg.BoundBeacon;
using Android.Content;
using AltBeaconLibrary.Sample.Droid.Services;
using Android.Widget;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Android.App;

[assembly: Xamarin.Forms.Dependency(typeof(AltBeaconService))]

namespace AltBeaconLibrary.Sample.Droid.Services
{
	public class AltBeaconService : Java.Lang.Object, IAltBeaconService//, IBeaconConsumer
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
		}

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

		public void StartMonitoring(string identifier)
		{
			_beaconManager = InitializeBeaconManager();
		}

		public void StartRanging(string identifier)
		{
		}

		private BeaconManager InitializeBeaconManager()
		{
			// Enable the BeaconManager 
			BeaconManager bm = BeaconManager.GetInstanceForApplication(Xamarin.Forms.Forms.Context);

			var iBeaconParser = new BeaconParser();
			//	Estimote > 2013
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			bm.BeaconParsers.Add(iBeaconParser);

			_monitorNotifier.EnterRegionComplete += EnteredRegion;
			_monitorNotifier.ExitRegionComplete += ExitedRegion;
			_monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
			_rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

			bm.SetBackgroundMode(false);
			bm.Bind((IBeaconConsumer)Xamarin.Forms.Forms.Context);

			return bm;
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
//			((Activity)ApplicationContext).RunOnUiThread(() =>
//			{
//				_data.Clear();
//				((ListSource)_list.Adapter).UpdateList(_data);
//			});
		}

		private void UpdateList()
		{
			((Activity)Xamarin.Forms.Forms.Context).RunOnUiThread(() => 
			{
				OnListChanged();
//				((ListSource)_list.Adapter).UpdateList(_data);
			});
		}

		public event EventHandler<ListChangedEventArgs> ListChanged;
		private void OnListChanged()
		{
			var handler = ListChanged;
			if(handler != null)
			{
				var data = new List<CommonBeacon>();
				_data.ForEach(b =>
				{
					data.Add(new CommonBeacon { Id = b.Id1.ToString(), Distance = b.Distance.ToString() });
				});
				handler(this, new ListChangedEventArgs(data));
			}
		}

		#region IBeaconConsumer Implementation

		/// <summary>
		/// Binds the service.
		/// </summary>
		/// <returns><c>true</c>, if service was bound, <c>false</c> otherwise.</returns>
		/// <param name="service">Service.</param>
		/// <param name="conn">Conn.</param>
		/// <param name="flags">Flags.</param>
//		public bool BindService(Intent service, IServiceConnection conn, Bind flags)
//		{
//			return true;
//		}

		/// <summary>
		/// Gets the application context.
		/// </summary>
		/// <value>The application context.</value>
//		public Context ApplicationContext
//		{
//			get
//			{
//				return Xamarin.Forms.Forms.Context;
//			}
//		}
			
		/// <summary>
		/// Raises the beacon service connect event.
		/// </summary>
		public void OnBeaconServiceConnect()
		{
			Console.WriteLine("Starting");

			BeaconManagerImpl.SetForegroundBetweenScanPeriod(5000); // 5000 milliseconds

			BeaconManagerImpl.SetMonitorNotifier(_monitorNotifier); 
			BeaconManagerImpl.SetRangeNotifier(_rangeNotifier);

			_tagRegion = new AltBeaconOrg.BoundBeacon.Region("myUniqueBeaconId", Identifier.Parse("E4C8A4FC-F68B-470D-959F-29382AF72CE7"), null, null);
			_emptyRegion = new AltBeaconOrg.BoundBeacon.Region("myEmptyBeaconId", null, null, null);

			_beaconManager.StartMonitoringBeaconsInRegion(_tagRegion);
			_beaconManager.StartRangingBeaconsInRegion(_tagRegion);

			_beaconManager.StartMonitoringBeaconsInRegion(_emptyRegion);
			_beaconManager.StartRangingBeaconsInRegion(_emptyRegion);
		}

		/// <summary>
		/// Unbinds the service.
		/// </summary>
		/// <param name="connection">Service connection</param>
//		public void UnbindService(IServiceConnection connection)
//		{
//			try
//			{
//				ApplicationContext.UnbindService(connection);	
//			}
//			catch(Exception ex)
//			{
//				var a = ex;
//			}
//		}
		#endregion
	}

	public class ListSource : BaseAdapter<Beacon>
	{
		private List<Beacon> _data;
		private Func<List<Beacon>, int, Android.Views.View, Android.Views.ViewGroup, Android.Views.View> _getView;

		public ListSource(Func<List<Beacon>, int, Android.Views.View, Android.Views.ViewGroup, Android.Views.View> getView)
		{
			_getView = getView;
			_data = new List<Beacon>();
		}

		public override long GetItemId(int position)
		{
			return position;
		}
		public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			return _getView(_data, position, convertView, parent);
		}

		public override int Count
		{
			get
			{
				return _data.Count;
			}
		}

		public override Beacon this[int index]
		{
			get
			{
				return _data[index];
			}
		}

		public void UpdateList(List<Beacon> list)
		{
			_data = list;
			NotifyDataSetChanged();				
		}
	}
}

