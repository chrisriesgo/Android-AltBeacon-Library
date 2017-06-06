﻿using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;
using Android.Annotation;
using Android.Runtime;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BeaconParserTest : TestBase
	{
		[Test]
		public void TestSetBeaconLayout() 
		{
	        var bytes = HexStringToByteArray("02011a1bffbeac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	
	        Assert.AreEqual(2, parser.MatchingBeaconTypeCodeStartOffset, "parser should get beacon type code start offset");
	        Assert.AreEqual(3, parser.MatchingBeaconTypeCodeEndOffset, "parser should get beacon type code end offset");
	        Assert.AreEqual(Convert.ToInt64(0xbeacL), Convert.ToInt64(parser.MatchingBeaconTypeCode), "parser should get beacon type code");
	        Assert.AreEqual(4, parser.IdentifierStartOffsets[0], "parser should get identifier start offset");
	        AssertEx.AreEqual("parser should get identifier end offset", 19, parser.IdentifierEndOffsets[0]);
	        AssertEx.AreEqual("parser should get identifier start offset", 20, parser.IdentifierStartOffsets[1]);
	        AssertEx.AreEqual("parser should get identifier end offset", 21, parser.IdentifierEndOffsets[1]);
	        AssertEx.AreEqual("parser should get identifier start offset", 22, parser.IdentifierStartOffsets[2]);
	        AssertEx.AreEqual("parser should get identifier end offset", 23, parser.IdentifierEndOffsets[2]);
	        AssertEx.AreEqual("parser should get power start offset", 24, Convert.ToInt32(parser.PowerStartOffset));
	        AssertEx.AreEqual("parser should get power end offset", 24, Convert.ToInt32(parser.PowerEndOffset));
	        AssertEx.AreEqual("parser should get data start offset", 25, parser.DataStartOffsets[0]);
	        AssertEx.AreEqual("parser should get data end offset", 25, parser.DataEndOffsets[0]);
	    }
	    
	    [Test]
	    public void TestLongToByteArray() 
	    {
	        var bytes = BeaconParser.LongToByteArray(10, 1);
	        AssertEx.AreEqual("first byte should be 10", 10, bytes[0]);
	    }

    	[Test]
	    public void TestRecognizeBeacon() 
	    {
	        var bytes = HexStringToByteArray("02011a1aff180112342f234454cf6d4a0fadf2f4911ba9ffa600010002c5");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=1234,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("mRssi should be as passed in", -55, beacon.Rssi);
	        AssertEx.AreEqual("uuid should be parsed", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", beacon.GetIdentifier(0).ToString());
	        AssertEx.AreEqual("id2 should be parsed", "1", beacon.GetIdentifier(1).ToString());
	        AssertEx.AreEqual("id3 should be parsed", "2", beacon.GetIdentifier(2).ToString());
	        AssertEx.AreEqual("txPower should be parsed", -59, beacon.TxPower);
	        AssertEx.AreEqual("manufacturer should be parsed", 0x118 ,beacon.Manufacturer);
	    }

	    [Test]
	    public void TestAllowsAccessToParserIdentifier() 
	    {
	        var bytes = HexStringToByteArray("02011a1aff180112342f234454cf6d4a0fadf2f4911ba9ffa600010002c5");
	        var parser = new BeaconParser("my_beacon_type");
	        parser.SetBeaconLayout("m:2-3=1234,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("parser identifier should be accessible", "my_beacon_type", beacon.ParserIdentifier);
	    }
	
	    [Test]
	    public void TestParsesBeaconMissingDataField() 
	    {
	        var bytes = HexStringToByteArray("02011a1aff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c5000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("mRssi should be as passed in", -55, beacon.Rssi);
	        AssertEx.AreEqual("uuid should be parsed", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", beacon.GetIdentifier(0).ToString());
	        AssertEx.AreEqual("id2 should be parsed", "1", beacon.GetIdentifier(1).ToString());
	        AssertEx.AreEqual("id3 should be parsed", "2", beacon.GetIdentifier(2).ToString());
	        AssertEx.AreEqual("txPower should be parsed", -59, beacon.TxPower);
	        AssertEx.AreEqual("manufacturer should be parsed", 0x118 ,beacon.Manufacturer);
	        AssertEx.AreEqual("missing data field zero should be zero", Convert.ToInt64(0), Convert.ToInt64(beacon.DataFields[0]));
	
	    }	
	
	    [Test]
	    public void TestRecognizeBeaconWithFormatSpecifyingManufacturer() 
	    {
	        var bytes = HexStringToByteArray("02011a1bff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:0-3=1801beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("mRssi should be as passed in", -55, beacon.Rssi);
	        AssertEx.AreEqual("uuid should be parsed", "2f234454-cf6d-4a0f-adf2-f4911ba9ffa6", beacon.GetIdentifier(0).ToString());
	        AssertEx.AreEqual("id2 should be parsed", "1", beacon.GetIdentifier(1).ToString());
	        AssertEx.AreEqual("id3 should be parsed", "2", beacon.GetIdentifier(2).ToString());
	        AssertEx.AreEqual("txPower should be parsed", -59, beacon.TxPower);
	        AssertEx.AreEqual("manufacturer should be parsed", 0x118 ,beacon.Manufacturer);
	    }
	
	    [Test]
	    [TargetApi(Value = 10)]
	    public void TestReEncodesBeacon() 
	    {
	        var bytes = HexStringToByteArray("02011a1bff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        var regeneratedBytes = parser.GetBeaconAdvertisementData(beacon);
	        var expectedMatch = Java.Util.Arrays.CopyOfRange(bytes, 7, bytes.Length);
	        AssertEx.AreEqual("beacon advertisement bytes should be the same after re-encoding", expectedMatch, regeneratedBytes);
	    }
	
	    [TargetApi(Value = 10)]
	    [Test]
	    public void TestReEncodesBeaconForEddystoneTelemetry() 
	    {
	        var bytes = HexStringToByteArray("0201060303aafe1516aafe2001021203130414243405152535");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout(BeaconParser.EddystoneTlmLayout);
	        var beacon = parser.FromScanData(bytes, -55, null);
	        var regeneratedBytes = parser.GetBeaconAdvertisementData(beacon);
	        var expectedMatch = Java.Util.Arrays.CopyOfRange(bytes, 11, bytes.Length);
	        AssertEx.AreEqual("beacon advertisement bytes should be the same after re-encoding", ByteArrayToHexString(expectedMatch), ByteArrayToHexString(regeneratedBytes));
	    }
	
	    [Test]
	    public void TestLittleEndianIdentifierParsing() 
	    {
	        var bytes = HexStringToByteArray("02011a1bff1801beac0102030405060708090a0b0c0d0e0f1011121314c50900000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-9,i:10-15l,i:16-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("mRssi should be as passed in", -55, beacon.Rssi);
	        AssertEx.AreEqual("id1 should be big endian", "0x010203040506", beacon.GetIdentifier(0).ToString());
	        AssertEx.AreEqual("id2 should be little endian", "0x0c0b0a090807", beacon.GetIdentifier(1).ToString());
	        AssertEx.AreEqual("id3 should be big endian", "0x0d0e0f1011121314", beacon.GetIdentifier(2).ToString());
	        AssertEx.AreEqual("txPower should be parsed", -59, beacon.TxPower);
	        AssertEx.AreEqual("manufacturer should be parsed", 0x118, beacon.Manufacturer);
	    }
	
	    [TargetApi(Value = 10)]
	    [Test]
	    public void TestReEncodesLittleEndianBeacon() 
	    {
	        var bytes = HexStringToByteArray("02011a1bff1801beac0102030405060708090a0b0c0d0e0f1011121314c509");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-9,i:10-15l,i:16-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        var regeneratedBytes = parser.GetBeaconAdvertisementData(beacon);
	        var expectedMatch = Java.Util.Arrays.CopyOfRange(bytes, 7, bytes.Length);
	        AssertEx.AreEqual("beacon advertisement bytes should be the same after re-encoding", ByteArrayToHexString(expectedMatch), ByteArrayToHexString(regeneratedBytes));
	    }
		
	    [Test]
	    public void TestRecognizeBeaconCapturedManufacturer() 
	    {
	        var bytes = HexStringToByteArray("0201061bffaabbbeace2c56db5dffb48d2b060d0f5a71096e000010004c50000000000000000000000000000000000000000000000000000000000000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("manufacturer should be parsed", "bbaa", String.Format("{0:4X}", beacon.Manufacturer));
	    }
	
	    [Test]
	    public void TestParseGattIdentifierThatRunsOverPduLength() 
	    {
	        var bytes = HexStringToByteArray("0201060303aafe0d16aafe10e702676f6f676c65000c09526164426561636f6e204700000000000000000000000000000000000000000000000000000000");
	        var parser = new BeaconParser();
	        parser.SetAllowPduOverflow(Java.Lang.Boolean.False);
	        parser.SetBeaconLayout("s:0-1=feaa,m:2-2=10,p:3-3:-41,i:4-20");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.Null("beacon should not be parsed", beacon);
	    }
	
	    [Test]
	    public void TestLongUrlBeaconIdentifier() 
	    {
	        var bytes = HexStringToByteArray("0201060303aafe0d16aafe10e70102030405060708090a0b0c0d0e0f0102030405060708090a0b0c0d0e0f00000000000000000000000000000000000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("s:0-1=feaa,m:2-2=10,p:3-3:-41,i:4-20v");
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.AreEqual("URL Identifier should be truncated at 8 bytes", 8, beacon.Id1.ToByteArray().Length);
	    }
	
	    [Test]
	    public void TestParseManufacturerIdentifierThatRunsOverPduLength() 
	    {
	        // Note that the length field below is 0x16 instead of 0x1b, indicating that the packet ends
	        // one byte before the second identifier field starts
	        var bytes = HexStringToByteArray("02011a16ff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509000000");
	        var parser = new BeaconParser();
	        parser.SetAllowPduOverflow(Java.Lang.Boolean.False);
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.Null("beacon should not be parsed", beacon);
	    }
	
	    [Test]
	    public void TestParseProblematicBeaconFromIssue229() 
	    {
	        // Note that the length field below is 0x16 instead of 0x1b, indicating that the packet ends
	        // one byte before the second identifier field starts
	
	       	var bytes = HexStringToByteArray("0201061bffe000beac7777772e626c756b692e636f6d000100010001abaa000000");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("beacon should be parsed", beacon);
	    }
	
	
	    [Test]
	    public void TestCanParseLocationBeacon() 
	    {	
	        double latitude = 38.93;
	        double longitude = -77.23;
	        var beacon = new Beacon.Builder()
	                .SetManufacturer(0x0118) // Radius Networks
	                .SetId1("1") // device sequence number
	                .SetId2(String.Format("{0:X8}", (long)((latitude+90)*10000.0)))
	                .SetId3(String.Format("{0:X8}", (long)((longitude+180)*10000.0)))
	                .SetTxPower(-59) // The measured transmitter power at one meter in dBm
	                .Build();
	        // TODO: make this pass if data fields are little endian or > 4 bytes (or even > 2 bytes)
	        var p = new BeaconParser().
	                SetBeaconLayout("m:2-3=10ca,i:4-9,i:10-13,i:14-17,p:18-18");
	        var bytes = p.GetBeaconAdvertisementData(beacon);
	        var headerBytes = HexStringToByteArray("02011a1bff1801");
	        var advBytes = new byte[bytes.Length + headerBytes.Length];
	        Array.Copy(headerBytes, 0, advBytes, 0, headerBytes.Length);
	        Array.Copy(bytes, 0, advBytes, headerBytes.Length, bytes.Length);
	
	        Beacon parsedBeacon = p.FromScanData(advBytes, -59, null);
	        AssertEx.NotNull(String.Format("Parsed beacon from {0} should not be null", ByteArrayToHexString(advBytes)), parsedBeacon);
	        double parsedLatitude = Int64.Parse(parsedBeacon.Id2.ToString().Substring(2), System.Globalization.NumberStyles.HexNumber) / 10000.0 - 90.0;
	        double parsedLongitude = Int64.Parse(parsedBeacon.Id3.ToString().Substring(2), System.Globalization.NumberStyles.HexNumber) / 10000.0 - 180.0;
	
	        long encodedLatitude = (long)((latitude + 90)*10000.0);
	        AssertEx.AreEqual("encoded latitude hex should match", String.Format("{0:X8}", encodedLatitude), parsedBeacon.Id2.ToString());
	        AssertEx.AreEqual("device sequence num should be same", "0x000000000001", parsedBeacon.Id1.ToString());
	        AssertEx.AreEqual("latitude should be about right", latitude, parsedLatitude, 0.0001);
	        AssertEx.AreEqual("longitude should be about right", longitude, parsedLongitude, 0.0001);
	
	    }
	    
	    [Test]
	    public void TestCanGetAdvertisementDataForUrlBeacon() 
	    {
	        var beacon = new Beacon.Builder()
	                .SetManufacturer(0x0118)
	                .SetId1("02646576656c6f7065722e636f6d") // http://developer.com
	                .SetTxPower(-59) // The measured transmitter power at one meter in dBm
	                .Build();
	        var p = new BeaconParser().
	                SetBeaconLayout("s:0-1=feaa,m:2-2=10,p:3-3:-41,i:4-20v");
	        var bytes = p.GetBeaconAdvertisementData(beacon);
	        AssertEx.AreEqual("First byte of url should be in position 3", 0x02, bytes[2]);
	    }
	    
	    [Test]
	    public void DoesNotCashWithOverflowingByteCodeComparisonOnPdu() {
	        // Test for https://github.com/AltBeacon/android-beacon-library/issues/323
	
	        // Note that the length field below is 0x16 instead of 0x1b, indicating that the packet ends
	        // one byte before the second identifier field starts
	
	        var bytes = HexStringToByteArray("02010604ffe000be");
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=beac,i:4-19,i:20-21,i:22-23,p:24-24,d:25-25");
	
	        var beacon = parser.FromScanData(bytes, -55, null);
	        AssertEx.Null("beacon not be parsed without an exception being thrown", beacon);
	    }
	
	    [Test]
	    public void TestCanParseLongDataTypeOfDifferentSize()
	    {
	        // Create a beacon parser
	        var parser = new BeaconParser();
	        parser.SetBeaconLayout("m:2-3=0118,i:4-7,p:8-8,d:9-16,d:18-21,d:22-25");

			// Generate sample beacon for test purpose.
			var sampleData = new List<Java.Lang.Long>();
			var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
	        sampleData.Add(new Java.Lang.Long(now));
	        sampleData.Add(new Java.Lang.Long(1234L));
	        sampleData.Add(new Java.Lang.Long(9876L));
	        var beacon = new Beacon.Builder()
	                .SetManufacturer(0x0118)
	                .SetId1("02646576656c6f7065722e636f6d")
	                .SetTxPower(-59)
	                .SetDataFields(sampleData)
	                .Build();
	
	        AssertEx.AreEqual("beacon contains a valid data on index 0", now, Convert.ToInt64(beacon.DataFields[0]));
	
	        // Make byte array
	        byte[] headerBytes = HexStringToByteArray("1bff1801");
	        byte[] bodyBytes = parser.GetBeaconAdvertisementData(beacon);
	        byte[] bytes = new byte[headerBytes.Length + bodyBytes.Length];
	        Array.Copy(headerBytes, 0, bytes, 0, headerBytes.Length);
	        Array.Copy(bodyBytes, 0, bytes, headerBytes.Length, bodyBytes.Length);
	
	        // Try parsing the byte array
	        Beacon parsedBeacon = parser.FromScanData(bytes, -59, null);
	
	        AssertEx.AreEqual("parsed beacon should contain a valid data on index 0", now, parsedBeacon.DataFields[0]);
	        AssertEx.AreEqual("parsed beacon should contain a valid data on index 1", Convert.ToInt64(1234L), Convert.ToInt64(parsedBeacon.DataFields[1]));
	        AssertEx.AreEqual("parsed beacon should contain a valid data on index 2", Convert.ToInt64(9876L), Convert.ToInt64(parsedBeacon.DataFields[2]));
	    }
	}
}