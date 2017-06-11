using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Utils;
using Android.App;
using Android.Content;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class GattBeaconTest : TestBase
	{
		[Test]
	    public void TestDetectsGattBeacon() {
	        byte[] bytes = HexStringToByteArray("020106030334121516341200e72f234454f4911ba9ffa6000000000001000000000000000000000000000000000000000000000000000000000000000000");
	        BeaconParser parser = new BeaconParser().SetBeaconLayout("s:0-1=1234,m:2-2=00,p:3-3:-41,i:4-13,i:14-19");
	        AssertEx.NotNull("Service uuid parsed should not be null", parser.ServiceUuid);
	        Beacon gattBeacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("GattBeacon should be not null if parsed successfully", gattBeacon);
	        AssertEx.AreEqual("id1 should be parsed", "0x2f234454f4911ba9ffa6", gattBeacon.Id1.ToString());
	        AssertEx.AreEqual("id2 should be parsed", "0x000000000001", gattBeacon.Id2.ToString());
	        AssertEx.AreEqual("serviceUuid should be parsed", 0x1234, gattBeacon.ServiceUuid);
	        AssertEx.AreEqual("txPower should be parsed", -66, gattBeacon.TxPower);
	    }
	
	    [Test]
	    public void TestDetectsGattBeacon2MaxLength() {
	        byte[] bytes = HexStringToByteArray("020106030334121616341210ec007261646975736e6574776f726b7373070000000000000000000000000000000000000000000000000000000000000000");
	        BeaconParser parser = new BeaconParser().SetBeaconLayout("s:0-1=1234,m:2-2=10,p:3-3:-41,i:4-20v");
	        Beacon gattBeacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("GattBeacon should be not null if parsed successfully", gattBeacon);
	        AssertEx.AreEqual("GattBeacon identifier length should be proper length",
	                17,
	                gattBeacon.Id1.ToByteArray().Length);
	
	    }
	
	    [Test]
	    public void TestDetectsGattBeacon2WithShortIdentifier() {
	        byte[] bytes = HexStringToByteArray("020106030334121516341210ec007261646975736e6574776f726b7307000000000000000000000000000000000000000000000000000000000000000000");
	        BeaconParser parser = new BeaconParser().SetBeaconLayout("s:0-1=1234,m:2-2=10,p:3-3:-41,i:4-20v");
	        Beacon gattBeacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("GattBeacon should be not null if parsed successfully", gattBeacon);
	        AssertEx.AreEqual("GattBeacon identifier length should be adjusted smaller if packet is short",
	                     16,
	                     gattBeacon.Id1.ToByteArray().Length);
	        AssertEx.AreEqual("GattBeacon identifier should have proper first byte",
	                (byte)0x00,
	                gattBeacon.Id1.ToByteArray()[0]);
	        AssertEx.AreEqual("GattBeacon identifier should have proper second to last byte",
	                (byte) 0x73,
	                gattBeacon.Id1.ToByteArray()[14]);
	        AssertEx.AreEqual("GattBeacon identifier should have proper last byte",
	                (byte)0x07,
	                gattBeacon.Id1.ToByteArray()[15]);
	
	    }
	
	
	    [Test]
	    public void TestDetectsEddystoneUID() {
	        byte[] bytes = HexStringToByteArray("0201060303aafe1516aafe00e700010203040506070809010203040506000000000000000000000000000000000000000000000000000000000000000000");
	        BeaconParser parser = new BeaconParser().SetBeaconLayout(BeaconParser.EddystoneUidLayout);
	        Beacon eddystoneUidBeacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("Eddystone-UID should be not null if parsed successfully", eddystoneUidBeacon);
	    }
	
	
	    [Test]
	    public void TestDetectsGattBeaconWithCnn() {
	        byte[] bytes = HexStringToByteArray("020106030334120a16341210ed00636e6e070000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
	        BeaconParser parser = new BeaconParser().SetBeaconLayout("s:0-1=1234,m:2-2=10,p:3-3:-41,i:4-20v");
	        
	        Beacon gattBeacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("GattBeacon should be not null if parsed successfully", gattBeacon);
	        AssertEx.AreEqual("GattBeacon identifier length should be adjusted smaller if packet is short",
	                5,
	                gattBeacon.Id1.ToByteArray().Length);
	    }
	
	    [Test]
	    [Ignore]
	    public void testBeaconAdvertisingBytes() {
	        Context context = Application.Context;
	
	
	        Beacon beacon = new Beacon.Builder()
	                .SetId1("0x454452e29735323d81c0")
	                .SetId2("0x060504030201")
	                .SetDataFields(new List<Java.Lang.Long> { new Java.Lang.Long(0x25L) })
	                .SetTxPower(-59)
	                .Build();
	        // TODO: need to use something other than the d: prefix here for an internally generated field
	        BeaconParser beaconParser = new BeaconParser()
	                .SetBeaconLayout("s:0-1=0123,m:2-2=00,d:3-3,p:4-4,i:5-14,i:15-20");
	        byte[] data = beaconParser.GetBeaconAdvertisementData(beacon);
	        BeaconTransmitter beaconTransmitter = new BeaconTransmitter(context, beaconParser);
	        // TODO: can't actually start transmitter here because Robolectric does not support API 21
	
	        AssertEx.AreEqual("Data should be 19 bytes long", 19, data.Length);
	        String byteString = "";
	        for (int i = 0; i < data.Length; i++) {
	            byteString += String.Format("{0:x2}", data[i]);
	            byteString += " ";
	        }
	        AssertEx.AreEqual("Advertisement bytes should be as expected", "00 25 C5 45 44 52 E2 97 35 32 3D 81 C0 06 05 04 03 02 01 ", byteString);
	    }
	
	    [Test]
	    public void TestDetectsUriBeacon() {
	        //"https://goo.gl/hqBXE1"
	        byte[] bytes = {2, 1, 4, 3, 3, (byte) 216, (byte) 254, 19, 22, (byte) 216, (byte) 254, 0, (byte) 242, 3, 103, 111, 111, 46, 103, 108, 47, 104, 113, 66, 88, 69, 49, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
	        BeaconParser parser = new BeaconParser().SetBeaconLayout("s:0-1=fed8,m:2-2=00,p:3-3:-41,i:4-21v");
	        
	        Beacon uriBeacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("UriBeacon should be not null if parsed successfully", uriBeacon);
	        AssertEx.AreEqual("UriBeacon identifier length should be correct",
	                14,
	                uriBeacon.Id1.ToByteArray().Length);
	        String urlString = UrlBeaconUrlCompressor.Uncompress(uriBeacon.Id1.ToByteArray());
	        AssertEx.AreEqual("URL should be decompressed successfully", "https://goo.gl/hqBXE1", urlString);
	    }
	}
}
