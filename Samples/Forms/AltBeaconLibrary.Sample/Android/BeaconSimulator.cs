using System;
using AltBeaconOrg.BoundBeacon.Simulator;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;

namespace AltBeaconLibrary.Sample.Droid
{
	public class BeaconSimulator : Java.Lang.Object, IBeaconSimulator
	{
		public bool UseSimulatedBeacons = true;

		private readonly List<Beacon> _beacons;

		public BeaconSimulator()
		{
			_beacons = new List<Beacon>();
		}

		public IList<Beacon> Beacons
		{
			get
			{
				return _beacons;
			}
		}

		public void CreateBasicSimulatedBeacons()
		{
			if(!UseSimulatedBeacons) return;

			var beacon1 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
				.SetId2("1").SetId3("1").SetRssi(-55).SetTxPower(-55).Build();

			var beacon2 = new AltBeacon.Builder().SetId1("DF7E1C78-43E9-44FF-886F-1D1F7DA6997A")
				.SetId2("2").SetId3("2").SetRssi(-55).SetTxPower(-55).Build();

			var beacon3 = new AltBeacon.Builder().SetId1("DF7E1C77-43E9-44FF-886F-1D1F7DA6997A")
				.SetId2("3").SetId3("3").SetRssi(-55).SetTxPower(-55).Build();

			var beacon4 = new AltBeacon.Builder().SetId1("DF7E1C76-43E9-44FF-886F-1D1F7DA6997A")
				.SetId2("4").SetId3("4").SetRssi(-55).SetTxPower(-55).Build();

			_beacons.AddRange(new [] { beacon1, beacon2, beacon3, beacon4 });
		}
	}
}

