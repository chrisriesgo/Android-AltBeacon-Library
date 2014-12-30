using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;

namespace AltBeaconLibrary.Sample
{
	public class BeaconViewModel
	{
		public BeaconViewModel()
		{
			Data = new List<SharedBeacon>();
		}

		public event EventHandler ListChanged;

		public List<SharedBeacon> Data { get; set; }

		public void Init()
		{
			var beaconService = DependencyService.Get<IAltBeaconService>();
			beaconService.ListChanged += (sender, e) => 
			{
				Data = e.Data;
				OnListChanged();
			};
			beaconService.DataClearing += (sender, e) => 
			{
				Data.Clear();
				OnListChanged();
			};

			beaconService.InitializeService();
		}

		private void OnListChanged()
		{
			var handler = ListChanged;
			if(handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
	}
}

