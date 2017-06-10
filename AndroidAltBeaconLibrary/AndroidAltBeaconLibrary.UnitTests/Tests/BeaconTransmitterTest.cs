using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;
using Android.App;
using Android.Content;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BeaconTransmitterTest
	{
		[Test]
		[Ignore]
	    public void TestBeaconAdvertisingBytes() {
			Context context = Application.Context;
	
	        Beacon beacon = new Beacon.Builder()
	                .SetId1("2f234454-cf6d-4a0f-adf2-f4911ba9ffa6")
	                .SetId2("1")
	                .SetId3("2")
	                .SetManufacturer(0x0118)
	                .SetTxPower(-59)
	                .SetDataFields(new List<Java.Lang.Long> { new Java.Lang.Long(0L) })
	                .Build();
	        BeaconParser beaconParser = new BeaconParser()
	                .SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        byte[] data = beaconParser.GetBeaconAdvertisementData(beacon);
	        BeaconTransmitter beaconTransmitter = new BeaconTransmitter(context, beaconParser);
	        // TODO: can't actually start transmitter here because Robolectric does not support API 21
	
	        AssertEx.AreEqual("Data should be 24 bytes long", 24, data.Length);
	        String byteString = "";
	        for (int i = 0; i < data.Length; i++) {
	            byteString += String.Format("{0:x2}", data[i]);
	            byteString += " ";
	        }
	        AssertEx.AreEqual("Advertisement bytes should be as expected", "BE AC 2F 23 44 54 CF 6D 4A 0F AD F2 F4 91 1B A9 FF A6 00 01 00 02 C5 00 ", byteString);
	    }

	    [Test]
	    [Ignore]
	    public void TestBeaconAdvertisingBytesForEddystone() {
	        Context context = Application.Context;
	
	        Beacon beacon = new Beacon.Builder()
	                .SetId1("0x2f234454f4911ba9ffa6")
	                .SetId2("0x000000000001")
	                .SetManufacturer(0x0118)
	                .SetTxPower(-59)
	                .Build();
	        BeaconParser beaconParser = new BeaconParser()
	                .SetBeaconLayout("s:0-1=feaa,m:2-2=00,p:3-3:-41,i:4-13,i:14-19");
	        byte[] data = beaconParser.GetBeaconAdvertisementData(beacon);
	        BeaconTransmitter beaconTransmitter = new BeaconTransmitter(context, beaconParser);
	        // TODO: can't actually start transmitter here because Robolectric does not support API 21
	
	        String byteString = "";
	        for (int i = 0; i < data.Length; i++) {
	            byteString += String.Format("{0:x2}", data[i]);
	            byteString += " ";
	        }
	        
	        AssertEx.AreEqual("Data should be 24 bytes long", 18, data.Length);
	        AssertEx.AreEqual("Advertisement bytes should be as expected", "00 C5 2F 23 44 54 F4 91 1B A9 FF A6 00 00 00 00 00 01 ", byteString);
	    }
	}
}
