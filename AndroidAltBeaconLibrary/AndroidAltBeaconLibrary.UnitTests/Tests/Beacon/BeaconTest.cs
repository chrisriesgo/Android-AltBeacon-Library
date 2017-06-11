using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Distance;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BeaconTest : TestBase
	{
		[SetUp]
		public void BeforeEachTest()
		{
			Beacon.SetHardwareEqualityEnforced(false);
		}
		
		[Test]
		public void TestAccessBeaconIdentifiers() {
	        Beacon beacon = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        AssertEx.AreEqual("First beacon id should be 1", beacon.GetIdentifier(0).ToString(), "1");
	        AssertEx.AreEqual("Second beacon id should be 1", beacon.GetIdentifier(1).ToString(), "2");
	        AssertEx.AreEqual("Third beacon id should be 1", beacon.GetIdentifier(2).ToString(), "3");
	        AssertEx.AreEqual("First beacon id should be 1", beacon.Id1.ToString(), "1");
	        AssertEx.AreEqual("Second beacon id should be 1", beacon.Id2.ToString(), "2");
	        AssertEx.AreEqual("Third beacon id should be 1", beacon.Id3.ToString(), "3");
	    }
	    
	    [Test]
	    public void TestBeaconsWithSameIdentifersAreEqual() {
	        Beacon beacon1 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Beacon beacon2 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        //AssertEx.AreEqual("Beacons with same identifiers are equal", beacon1, beacon2);
	        AssertEx.True("Beacons with same identifiers are equal", beacon1.Equals(beacon2));
	    }
	
	    [Test]
	    public void TestBeaconsWithDifferentId1AreNotEqual() {
	        Beacon beacon1 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Beacon beacon2 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("11").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        AssertEx.True("Beacons with different id1 are not equal", !beacon1.Equals(beacon2));
	    }
	
	    [Test]
	    public void TestBeaconsWithDifferentId2AreNotEqual() {
	        Beacon beacon1 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Beacon beacon2 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("12").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        AssertEx.True("Beacons with different id2 are not equal", !beacon1.Equals(beacon2));
	    }
	
	    [Test]
	    public void TestBeaconsWithDifferentId3AreNotEqual() {
	        Beacon beacon1 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Beacon beacon2 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("13").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        AssertEx.True("Beacons with different id3 are not equal", !beacon1.Equals(beacon2));
	    }
	
	
	    [Test]
	    public void TestBeaconsWithSameMacsAreEqual() {
	        Beacon.SetHardwareEqualityEnforced(true);
	        Beacon beacon1 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Beacon beacon2 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        AssertEx.True("Beacons with same same macs are equal", beacon1.Equals(beacon2));
	    }
	
	    [Test]
	    public void TestBeaconsWithDifferentMacsAreNotEqual() {
	        Beacon.SetHardwareEqualityEnforced(true);
	        Beacon beacon1 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        Beacon beacon2 = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:666666").Build();
	        AssertEx.True("Beacons with different same macs are not equal", !beacon1.Equals(beacon2));
	    }
	
	
	    [Test]
	    public void TestCalculateAccuracyWithRssiEqualsPower() {
	        Beacon.DistanceCalculator = new ModelSpecificDistanceCalculator(null, null);
	        double accuracy = Beacon.DistanceCalculator.CalculateDistance(-55, -55);
	        AssertEx.AreEqual("Distance should be one meter if mRssi is the same as power", 1.0, accuracy, 0.1);
	    }
	
	    [Test]
	    public void TestCalculateAccuracyWithRssiGreaterThanPower() {
	        Beacon.DistanceCalculator = new ModelSpecificDistanceCalculator(null, null);
	        double accuracy = Beacon.DistanceCalculator.CalculateDistance(-55, -50);
	        AssertEx.True("Distance should be under one meter if mRssi is less negative than power.  Accuracy was " + accuracy, accuracy < 1.0);
	    }
	
	    [Test]
	    public void TestCalculateAccuracyWithRssiLessThanPower() {
	        Beacon.DistanceCalculator = new ModelSpecificDistanceCalculator(null, null);
	        double accuracy = Beacon.DistanceCalculator.CalculateDistance(-55, -60);
	        AssertEx.True("Distance should be over one meter if mRssi is less negative than power. Accuracy was "+accuracy,  accuracy > 1.0);
	    }
	
	    [Test]
	    public void TestCalculateAccuracyWithRssiEqualsPowerOnInternalProperties() {
	        Beacon.DistanceCalculator = new ModelSpecificDistanceCalculator(null, null);
	        var beacon = new Beacon.Builder().SetTxPower(-55).SetRssi(-55).Build();
	        double distance = beacon.Distance;
	        AssertEx.AreEqual("Distance should be one meter if mRssi is the same as power", 1.0, distance, 0.1);
	    }
	
	    [Test]
	    public void TestCalculateAccuracyWithRssiEqualsPowerOnInternalPropertiesAndRunningAverage() {
	        var beacon = new Beacon.Builder().SetTxPower(-55).SetRssi(0).Build();
	        beacon.SetRunningAverageRssi(-55);
	        double distance = beacon.Distance;
	        AssertEx.AreEqual("Distance should be one meter if mRssi is the same as power", 1.0, distance, 0.1);
	    }
	
	    [Test]
	    [Ignore]
	    //TODO: Implement ISerializable
	    public void TestCanSerialize() {
	        var beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothName("xx")
	                .SetBluetoothAddress("1:2:3:4:5:6").SetDataFields(new List<Java.Lang.Long> { new Java.Lang.Long(100L) }).Build();
	        byte[] serializedBeacon = ConvertToBytes(beacon);
			Beacon beacon2 = (Beacon) ConvertFromBytes(serializedBeacon);
	        AssertEx.AreEqual("Right number of identifiers after deserialization", 3, beacon2.Identifiers.Count);
	        AssertEx.AreEqual("id1 is same after deserialization", beacon.GetIdentifier(0), beacon2.GetIdentifier(0));
	        AssertEx.AreEqual("id2 is same after deserialization", beacon.GetIdentifier(1), beacon2.GetIdentifier(1));
	        AssertEx.AreEqual("id3 is same after deserialization", beacon.GetIdentifier(2), beacon2.GetIdentifier(2));
	        AssertEx.AreEqual("txPower is same after deserialization", beacon.TxPower, beacon2.TxPower);
	        AssertEx.AreEqual("rssi is same after deserialization", beacon.Rssi, beacon2.Rssi);
	        AssertEx.AreEqual("distance is same after deserialization", beacon.Distance, beacon2.Distance, 0.001);
	        AssertEx.AreEqual("bluetoothAddress is same after deserialization", beacon.BluetoothAddress, beacon2.BluetoothAddress);
	        AssertEx.AreEqual("bluetoothAddress is same after deserialization", beacon.BluetoothName, beacon2.BluetoothName);
	        AssertEx.AreEqual("beaconTypeCode is same after deserialization", beacon.BeaconTypeCode, beacon2.BeaconTypeCode);
	        AssertEx.AreEqual("manufacturer is same after deserialization", beacon.Manufacturer, beacon2.Manufacturer);
	        AssertEx.AreEqual("data field 0 is the same after deserialization", beacon.DataFields[0], beacon2.DataFields[0]);
	        AssertEx.AreEqual("data field 0 is the right value", beacon.DataFields[0], new Java.Lang.Long(100L));
	    }
	
	    [Test]
	    public void NoDoubleWrappingOfExtraDataFields() {
	        Beacon beacon = new AltBeacon.Builder().SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6).SetBluetoothName("xx")
	                .SetBluetoothAddress("1:2:3:4:5:6").SetDataFields(new List<Java.Lang.Long> { new Java.Lang.Long(100L) }).Build();
	        var list = beacon.ExtraDataFields;
	        beacon.ExtraDataFields = list;
	        AssertEx.True("getter should return same object after first wrap ", beacon.ExtraDataFields == list);
	    }
	
	    [Test]
	    public void TestHashCodeWithNullIdentifier() {
	        Beacon beacon = new AltBeacon.Builder()
	                .SetIdentifiers(new List<Identifier> { Identifier.Parse("0x1234"), null })
	                .Build();
	        AssertEx.True("hashCode() should not throw exception", beacon.GetHashCode() >= int.MinValue);
	    }
	}
}
