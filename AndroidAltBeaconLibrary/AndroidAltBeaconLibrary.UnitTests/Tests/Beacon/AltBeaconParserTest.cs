using System;
using Org.Altbeacon.Beacon;
using Org.Altbeacon.Beacon.Logging;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	public class AltBeaconParserTest : TestBase
	{    	
    	[Test]
    	public void TestRecognizeBeacon() 
    	{
			var beaconManager = BeaconManager.GetInstanceForApplication(Android.App.Application.Context);
	        var bytes = HexStringToByteArray("02011a1bff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c50900");
	        AltBeaconParser parser = new AltBeaconParser();
	        Beacon beacon = parser.FromScanData(bytes, -55, null);
	        Assert.AreEqual(1, beacon.DataFields.Count, "Beacon should have one data field");
	        Assert.AreEqual(9, ((AltBeacon) beacon).MfgReserved, "manData should be parsed");
	    }
	    
	    [Test]
	    public void TestDetectsDaveMHardwareBeacon() 
	    {
	        var bytes = HexStringToByteArray("02011a1bff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600050003be020e09526164426561636f6e20555342020a0300000000000000000000000000");
	        var parser = new AltBeaconParser();
	        var beacon = parser.FromScanData(bytes, -55, null);
	        Assert.NotNull(beacon, "Beacon should be not null if parsed successfully");
	    }
	    
	    [Test]
	    public void TestDetectsAlternateBeconType() 
	    {
	        var bytes = HexStringToByteArray("02011a1bff1801aabb2f234454cf6d4a0fadf2f4911ba9ffa600010002c50900");
	        var parser = new AltBeaconParser();
	        parser.SetMatchingBeaconTypeCode(new Java.Lang.Long(0xaabbL));
	        var beacon = parser.FromScanData(bytes, -55, null);
	        Assert.NotNull(beacon, "Beacon should be not null if parsed successfully");
	    }
	    
	    [Test]
	    public void TestParseWrongFormatReturnsNothing() 
	    {
	        LogManager.D("XXX", "testParseWrongFormatReturnsNothing start");
	        var bytes = HexStringToByteArray("02011a1aff1801ffff2f234454cf6d4a0fadf2f4911ba9ffa600010002c509");
	        var parser = new AltBeaconParser();
	        var beacon = parser.FromScanData(bytes, -55, null);
	        LogManager.D("XXX", "testParseWrongFormatReturnsNothing end");
	        Assert.Null(beacon, "Beacon should be null if not parsed successfully");
	    }
	    
	    [Test]
	    public void TestParsesBeaconMissingDataField() 
	    {
	        var bytes = HexStringToByteArray("02011a1aff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c5000000");
	        var parser = new AltBeaconParser();
	        var beacon = parser.FromScanData(bytes, -55, null);
	        Assert.AreEqual(-55, beacon.Rssi, "mRssi should be as passed in");
	        Assert.AreEqual("2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", beacon.GetIdentifier(0).ToString(), "uuid should be parsed");
	        Assert.AreEqual("1", beacon.GetIdentifier(1).ToString(), "id2 should be parsed");
	        Assert.AreEqual("2", beacon.GetIdentifier(2).ToString(), "id3 should be parsed");
	        Assert.AreEqual(-59, beacon.TxPower, "txPower should be parsed");
	        Assert.AreEqual(0x118 ,beacon.Manufacturer, "manufacturer should be parsed");
	        Assert.AreEqual(Convert.ToInt64(new Java.Lang.Long(0)), Convert.ToInt64(beacon.DataFields[0]), "missing data field zero should be zero");
	    }
	}
}