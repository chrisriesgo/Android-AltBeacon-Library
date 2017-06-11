using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;
using Java.Lang;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class RegionTest
	{
		[Test]
	    public void testBeaconMatchesRegionWithSameIdentifiers() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", Identifier.Parse("1"), Identifier.Parse("2"), Identifier.Parse("3"));
	        AssertEx.True("Beacon should match region with all identifiers the same", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithSameIdentifier1() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", Identifier.Parse("1"), null, null);
	        AssertEx.True("Beacon should match region with first identifier the same", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithSameIdentifier1And2() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", Identifier.Parse("1"), Identifier.Parse("2"), null);
	        AssertEx.True("Beacon should match region with first two identifiers the same", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithDifferentIdentifier1() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", Identifier.Parse("22222"), null, null);
	        AssertEx.True("Beacon should not match region with first identifier different", !region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithShorterIdentifierList() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", new List<Identifier> { Identifier.Parse("1") });
	        AssertEx.True("Beacon should match region with first identifier equal and shorter Identifier list", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithSingleNullIdentifierList() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        var identifiers = new List<Identifier>();
	        identifiers.Add(null);
	        Region region = new Region("all-beacons-region", identifiers);
	        AssertEx.True("Beacon should match region with first identifier null and shorter Identifier list", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconDoesntMatchRegionWithLongerIdentifierList() {
	        Beacon beacon = new Beacon.Builder().SetId1("1").SetId2("2").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", Identifier.Parse("1"), Identifier.Parse("2"), Identifier.Parse("3"));
	        AssertEx.False("Beacon should not match region with more identifers than the beacon", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconDoesMatchRegionWithLongerIdentifierListWithSomeNull() {
	        Beacon beacon = new Beacon.Builder().SetId1("1").SetId2("2").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Region region = new Region("myRegion", null, null, null);
	        AssertEx.True("Beacon should match region with more identifers than the beacon, if the region identifiers are null", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithSameBluetoothMac() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("01:02:03:04:05:06").Build();
	        Region region = new Region("myRegion", "01:02:03:04:05:06");
	        AssertEx.True("Beacon should match region with mac the same", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconDoesNotMatchRegionWithDiffrentBluetoothMac() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("01:02:03:04:05:06").Build();
	        Region region = new Region("myRegion", "01:02:03:04:05:99");
	        AssertEx.False("Beacon should match region with mac the same", region.MatchesBeacon(beacon));
	    }
	
	    [Test]
	    public void testBeaconMatchesRegionWithSameBluetoothMacAndIdentifiers() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothAddress("01:02:03:04:05:06").Build();
	        var identifiers = new List<Identifier>();
	        identifiers.Add(Identifier.Parse("1"));
	        identifiers.Add(Identifier.Parse("2"));
	        identifiers.Add(Identifier.Parse("3"));
	        Region region = new Region("myRegion", identifiers , "01:02:03:04:05:06");
	        AssertEx.True("Beacon should match region with mac the same", region.MatchesBeacon(beacon));
	    }
	
	
	    [Test]
	    [Ignore("Figure out serialization")]
	    public void testCanSerialize() {
	        Region region = new Region("myRegion", Identifier.Parse("1"), Identifier.Parse("2"), null);
	        //TODO: figure out serialization
	        //byte[] serializedRegion = convertToBytes(region);
	        //Region region2 = (Region) convertFromBytes(serializedRegion);
	        //AssertEx.AreEqual("Right number of identifiers after deserialization", 3, region2.Identifiers.Count);
	        //AssertEx.AreEqual("uniqueId is same after deserialization", region.UniqueId, region2.UniqueId);
	        //AssertEx.AreEqual("id1 is same after deserialization", region.GetIdentifier(0), region2.GetIdentifier(0));
	        //AssertEx.AreEqual("id2 is same after deserialization", region.GetIdentifier(1), region2.GetIdentifier(1));
	        //AssertEx.Null("id3 is null before deserialization", region.GetIdentifier(2));
	        //AssertEx.Null("id3 is null after deserialization", region2.GetIdentifier(2));
	    }
	
	    [Test]
	    [Ignore("Figure out serialization")]
	    public void testCanSerializeWithMac() {
	        Region region = new Region("myRegion", "1B:2a:03:4C:6E:9F");
	        //TODO: figure out serialization
	        //byte[] serializedRegion = convertToBytes(region);
	        //Region region2 = (Region) convertFromBytes(serializedRegion);
	        //AssertEx.AreEqual("Right number of identifiers after deserialization", 0, region2.Identifiers.Count);
	        //AssertEx.AreEqual("ac is same after deserialization", region.BluetoothAddress, region2.BluetoothAddress);
	    }
	
	    [Test]
	    public void rejectsInvalidMac() {
	        try {
	            Region region = new Region("myRegion", "this string is not a valid mac address!");
	            AssertEx.True("IllegalArgumentException should have been thrown", false);
	        }
	        catch (IllegalArgumentException e) {
	            AssertEx.AreEqual("Error message should be as expected",
	                    "Invalid mac address: 'this string is not a valid mac address!' Must be 6 hex bytes separated by colons.",
	                    e.Message);
	        }
	    }
	
	
	    [Test]
	    public void testToString() {
	        Region region = new Region("myRegion", Identifier.Parse("1"), Identifier.Parse("2"), null);
	        AssertEx.AreEqual("Not equal", "id1: 1 id2: 2 id3: null", region.ToString());
	    }
	
	    [Test]
	    public void testConvenienceIdentifierAccessors() {
	        Region region = new Region("myRegion", Identifier.Parse("1"), Identifier.Parse("2"), Identifier.Parse("3"));
	        AssertEx.AreEqual("Not equal", "1", region.Id1.ToString());
	        AssertEx.AreEqual("Not equal", "2", region.Id2.ToString());
	        AssertEx.AreEqual("Not equal", "3", region.Id3.ToString());
	    }
	}
}
