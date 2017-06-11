using System;
using NUnit.Framework;
using AltBeaconOrg.BoundBeacon;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon.Utils;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class EddystoneTelemetryAccessorTest : TestBase
	{
		[Test]
	    public void testAllowsAccessToTelemetryBytes() {
	        var telemetryFields = new List<Java.Lang.Long>();
	        telemetryFields.Add(new Java.Lang.Long(0x01L)); // version
	        telemetryFields.Add(new Java.Lang.Long(0x0212L)); // battery level
	        telemetryFields.Add(new Java.Lang.Long(0x0313L)); // temperature
	        telemetryFields.Add(new Java.Lang.Long(0x04142434L)); // pdu count
	        telemetryFields.Add(new Java.Lang.Long(0x05152535L)); // uptime
	
	        Beacon beaconWithTelemetry = new Beacon.Builder().SetId1("0x0102030405060708090a").SetId2("0x01020304050607").SetTxPower(-59).SetExtraDataFields(telemetryFields).Build();
	        byte[] telemetryBytes = new EddystoneTelemetryAccessor().GetTelemetryBytes(beaconWithTelemetry);
	
	        byte[] expectedBytes = {0x20, 0x01, 0x02, 0x12, 0x03, 0x13, 0x04, 0x14, 0x24, 0x34, 0x05, 0x15, 0x25, 0x35};
	        AssertEx.AreEqual("Should be equal", ByteArrayToHexString(telemetryBytes), ByteArrayToHexString(expectedBytes));
	    }
	
	
	    [Test]
	    public void testAllowsAccessToBase64EncodedTelemetryBytes() {
	        var telemetryFields = new List<Java.Lang.Long>();
	        telemetryFields.Add(new Java.Lang.Long(0x01L)); // version
	        telemetryFields.Add(new Java.Lang.Long(0x0212L)); // battery level
	        telemetryFields.Add(new Java.Lang.Long(0x0313L)); // temperature
	        telemetryFields.Add(new Java.Lang.Long(0x04142434L)); // pdu count
	        telemetryFields.Add(new Java.Lang.Long(0x05152535L)); // uptime
	
	        Beacon beaconWithTelemetry = new Beacon.Builder().SetId1("0x0102030405060708090a").SetId2("0x01020304050607").SetTxPower(-59).SetExtraDataFields(telemetryFields).Build();
	        byte[] telemetryBytes = new EddystoneTelemetryAccessor().GetTelemetryBytes(beaconWithTelemetry);
	
	        String encodedTelemetryBytes = new EddystoneTelemetryAccessor().GetBase64EncodedTelemetry(beaconWithTelemetry);
	        AssertEx.NotNull("Should not be null", telemetryBytes);
	    }
	}
}
