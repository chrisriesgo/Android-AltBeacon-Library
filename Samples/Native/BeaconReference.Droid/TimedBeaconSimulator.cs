using System;
using System.Collections.Generic;
using Org.Altbeacon.Beacon;
using Org.Altbeacon.Beacon.Simulator;
using Java.Lang;
using Java.Util.Concurrent;

namespace BeaconReference.Droid
{
	public class TimedBeaconSimulator : Java.Lang.Object, IBeaconSimulator
	{
		protected const string TAG = "TimedBeaconSimulator";
		
		IScheduledExecutorService scheduleTaskExecutor;
		
		public IList<Beacon> Beacons { get; set; }

		public TimedBeaconSimulator()
		{
			Beacons = new List<Beacon>();
		}
		
		/*
		 * You may simulate detection of beacons by creating a class like this in your project.
		 * This is especially useful for when you are testing in an Emulator or on a device without BluetoothLE capability.
		 * 
		 * Uncomment the lines in BeaconReferenceApplication starting with:
		 *     // If you wish to test beacon detection in the Android Emulator, you can use code like this:
		 * Then set USE_SIMULATED_BEACONS = true to initialize the sample code in this class.
		 * If using a Bluetooth incapable test device (i.e. Emulator), you will want to comment
		 * out the verifyBluetooth() in MonitoringActivity.java as well.
		 * 
		 * Any simulated beacons will automatically be ignored when building for production.
		 */
		public bool USE_SIMULATED_BEACONS = true;
		
		public void CreateBasicSimulatedBeacons() {
			if (USE_SIMULATED_BEACONS) {
	            Beacon beacon1 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("1").SetRssi(-55).SetTxPower(-55).Build();
	            Beacon beacon2 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("2").SetRssi(-55).SetTxPower(-55).Build();
	            Beacon beacon3 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("3").SetRssi(-55).SetTxPower(-55).Build();
	            Beacon beacon4 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("4").SetRssi(-55).SetTxPower(-55).Build();
				
				Beacons.Add(beacon1);
				Beacons.Add(beacon2);
				Beacons.Add(beacon3);
				Beacons.Add(beacon4);
			}
		}
		
		public void CreateTimedSimulatedBeacons() {
			if (USE_SIMULATED_BEACONS){
				Beacons = new List<Beacon>();
				
	            Beacon beacon1 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("1").SetRssi(-55).SetTxPower(-55).Build();
	            Beacon beacon2 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("2").SetRssi(-55).SetTxPower(-55).Build();
	            Beacon beacon3 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("3").SetRssi(-55).SetTxPower(-55).Build();
	            Beacon beacon4 = new AltBeacon.Builder().SetId1("DF7E1C79-43E9-44FF-886F-1D1F7DA6997A")
	                    .SetId2("1").SetId3("4").SetRssi(-55).SetTxPower(-55).Build();
				
				Beacons.Add(beacon1);
				Beacons.Add(beacon2);
				Beacons.Add(beacon3);
				Beacons.Add(beacon4);
				
				var finalBeacons = new List<Beacon>(Beacons);
	
				//Clearing beacons list to prevent all beacons from appearing immediately.
				//These will be added back into the beacons list from finalBeacons later.
				Beacons.Clear();
	
				scheduleTaskExecutor = Executors.NewScheduledThreadPool(5);

				// This schedules an beacon to appear every 10 seconds:
				scheduleTaskExecutor.ScheduleAtFixedRate(new Runnable(() =>
				{
					try{
						//putting a single beacon back into the beacons list.
						if (finalBeacons.Count > Beacons.Count)
							Beacons.Add(finalBeacons[Beacons.Count]);
						else 
							scheduleTaskExecutor.Shutdown();
					}
					catch(Java.Lang.Exception ex) {
						ex.PrintStackTrace();
					}
				}), 0, 10, TimeUnit.Seconds);
			} 
		}
	}
}
