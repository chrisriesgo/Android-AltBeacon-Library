using System;
using AltBeaconOrg.BoundBeacon;
using Android.Content;
using AltBeaconLibrary.Sample.Android.Services;

[assembly: Xamarin.Forms.Dependency(typeof(AltBeaconService))]

namespace AltBeaconLibrary.Sample.Android.Services
{
	public class AltBeaconService : Java.Lang.Object, IAltBeaconService, IBeaconConsumer
	{
		private readonly MonitorNotifier monitorNotifier;
		private BeaconManager _beaconManager;

		public AltBeaconService()
		{
			monitorNotifier = new MonitorNotifier();
		}

		public BeaconManager BeaconManager
		{
			get {
				if (_beaconManager == null)
				{
					_beaconManager = InitializeBeaconManager();
				}
				return _beaconManager;
			}
		}

		public void StartMonitoring(string identifier, bool monitorRanging)
		{
			// TODO  Change this ..
			var dd = BeaconManager;
		}

		private BeaconManager InitializeBeaconManager()
		{
			// Enable the BeaconManager 
			BeaconManager bm = BeaconManager.GetInstanceForApplication(ApplicationContext);

			var iBeaconParser = new BeaconParser();
			//	Estimote > 2013
			iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
			bm.BeaconParsers.Add(iBeaconParser);

			bm.Bind(this);
			bm.SetBackgroundMode(false);

			monitorNotifier.EnterRegionComplete += EnteredRegion;
			monitorNotifier.ExitRegionComplete += ExitedRegion;
			monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
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

//		protected override void Dispose(bool disposing)
//		{
//			base.Dispose(disposing);
//			StopAllMonitoring();
//		}

		#region IBeaconConsumer Implementation

		/// <summary>
		/// Binds the service.
		/// </summary>
		/// <returns><c>true</c>, if service was bound, <c>false</c> otherwise.</returns>
		/// <param name="service">Service.</param>
		/// <param name="conn">Conn.</param>
		/// <param name="flags">Flags.</param>
		public bool BindService(Intent service, IServiceConnection conn, Bind flags)
		{
			return true;
		}

		/// <summary>
		/// Gets the application context.
		/// </summary>
		/// <value>The application context.</value>
		public Context ApplicationContext
		{
			get
			{
				return Xamarin.Forms.Forms.Context;
			}
		}
			
		/// <summary>
		/// Raises the beacon service connect event.
		/// </summary>
		public void OnBeaconServiceConnect()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Unbinds the service.
		/// </summary>
		/// <param name="connection">Service connection</param>
		public void UnbindService(IServiceConnection connection)
		{
			try
			{
				ApplicationContext.UnbindService(connection);	
			}
			catch(Exception ex)
			{
				var a = ex;
			}
		}
		#endregion
	}
}

