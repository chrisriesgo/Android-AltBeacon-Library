using System;
using NUnit.Framework;
using AltBeaconOrg.Bluetooth;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BleAdvertisementTest : TestBase
	{
		[Test]
	    public void testCanParsePdusFromAltBeacon() {
	        byte[] bytes = HexStringToByteArray("02011a1aff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c50900000000000000000000000000000000000000000000000000000000000000");
	        BleAdvertisement bleAdvert = new BleAdvertisement(bytes);
	        AssertEx.AreEqual("An AltBeacon advert should have two PDUs", 3, bleAdvert.Pdus.Count);
	    }
	
	    [Test]
	    public void testCanParsePdusFromOtherBeacon() {
	        byte[] bytes = HexStringToByteArray("0201060303aafe1516aafe00e72f234454f4911ba9ffa60000000000010c09526164426561636f6e20470000000000000000000000000000000000000000");
	        BleAdvertisement bleAdvert = new BleAdvertisement(bytes);
	        AssertEx.AreEqual("An otherBeacon advert should four three PDUs", 4, bleAdvert.Pdus.Count);
	        AssertEx.AreEqual("First PDU should be flags type 1", 1, bleAdvert.Pdus[0].Type);
	        AssertEx.AreEqual("Second PDU should be services type 3", 3, bleAdvert.Pdus[1].Type);
	        AssertEx.AreEqual("Third PDU should be serivce type 0x16", 0x16, bleAdvert.Pdus[2].Type);
	        AssertEx.AreEqual("Fourth PDU should be scan response type 9", 9, bleAdvert.Pdus[3].Type);
	
	    }
	}
}
