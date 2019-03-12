using System;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Service;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class RangingDataTest
	{
		[SetUp]
	    public void before() {
	        BeaconManager.ManifestCheckingDisabled = true;
	    }
	
	    [Test]
	    [Ignore("Not testing serialization")]
	    public void testSerialization() {
	        //Context context = Android.App.Application.Context;
	        //var identifiers = new List<Identifier>();
	        //identifiers.Add(Identifier.Parse("2f234454-cf6d-4a0f-adf2-f4911ba9ffa6"));
	        //identifiers.Add(Identifier.Parse("1"));
	        //identifiers.Add(Identifier.Parse("2"));
	        //Region region = new Region("testRegion", identifiers);
	        //var beacons = new List<Beacon>();
	        //Beacon beacon = new Beacon.Builder().SetIdentifiers(identifiers).SetRssi(-1).setRunningAverageRssi(-2).setTxPower(-50).setBluetoothAddress("01:02:03:04:05:06").build();
	        //for (int i=0; i < 10; i++) {
	        //    beacons.Add(beacon);
	        //}
	        //RangingData data = new RangingData(beacons, region);
	        //Bundle bundle = data.ToBundle();
	        //RangingData data2 = RangingData.fromBundle(bundle);
	        //assertEquals("beacon count shouild be restored", 10, data2.getBeacons().size());
	        //assertEquals("beacon identifier 1 shouild be restored", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", data2.getBeacons().iterator().next().getId1().toString());
	        //assertEquals("region identifier 1 shouild be restored", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", data2.getRegion().getId1().toString());
	    }
	
	    [Test]
	    [Ignore("Not testing serialization")]
	    // On MacBookPro 2.5 GHz Core I7, 10000 serialization/deserialiation cycles of RangingData took 22ms
	    public void testSerializationBenchmark() {
	        //Context context = Android.App.Application.Context;
	        //var identifiers = new List<Identifier>();
	        //identifiers.Add(Identifier.Parse("2f234454-cf6d-4a0f-adf2-f4911ba9ffa6"));
	        //identifiers.Add(Identifier.Parse("1"));
	        //identifiers.Add(Identifier.Parse("2"));
	        //Region region = new Region("testRegion", identifiers);
	        //ArrayList<Beacon> beacons = new List<Beacon>();
	        //Beacon beacon = new Beacon.Builder().SetIdentifiers(identifiers).SetRssi(-1).setRunningAverageRssi(-2).setTxPower(-50).setBluetoothAddress("01:02:03:04:05:06").build();
	        //for (int i=0; i < 10; i++) {
	        //    beacons.Add(beacon);
	        //}
	        //RangingData data = new RangingData(beacons, region);
	        //long time1 = System.currentTimeMillis();
	        //for (int i=0; i< 10000; i++) {
	        //    Bundle bundle = data.toBundle();
	        //    RangingData data2 = RangingData.fromBundle(bundle);
	        //}
	        //long time2 = System.currentTimeMillis();
	        //System.out.println("*** Ranging Data Serialization benchmark: "+(time2-time1));
	    }
	}
}
