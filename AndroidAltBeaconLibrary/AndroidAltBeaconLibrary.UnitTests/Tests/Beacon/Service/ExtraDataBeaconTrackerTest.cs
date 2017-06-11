using System;
using System.Collections.Generic;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Service;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class ExtraDataBeaconTrackerTest
	{
		Beacon getManufacturerBeacon() {
	        return new Beacon.Builder().SetId1("1")
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .Build();
	    }
	
	    Beacon getGattBeacon() {
	        return new Beacon.Builder().SetId1("1")
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .SetServiceUuid(1234)
	                .Build();
	    }
	
	    Beacon getGattBeaconUpdate() {
	        return new Beacon.Builder().SetId1("1")
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .SetServiceUuid(1234)
	                .SetRssi(-50)
	                .SetDataFields(getDataFields())
	                .Build();
	    }
	
	    List<Java.Lang.Long> getDataFields() {
	        List<Java.Lang.Long> list = new List<Java.Lang.Long>();
	        list.Add(new Java.Lang.Long(1L));
	        list.Add(new Java.Lang.Long(2L));
	        return list;
	    }
	
	    List<Java.Lang.Long> getDataFields2() {
	        List<Java.Lang.Long> list = new List<Java.Lang.Long>();
	        list.Add(new Java.Lang.Long(3L));
	        list.Add(new Java.Lang.Long(4L));
	        return list;
	    }
	
	    Beacon getGattBeaconExtraData() {
	        return new Beacon.Builder()
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .SetServiceUuid(1234)
	                .SetDataFields(getDataFields())
	                .Build();
	    }
	
	    Beacon getGattBeaconExtraData2() {
	        return new Beacon.Builder()
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .SetServiceUuid(1234)
	                .SetDataFields(getDataFields2())
	                .Build();
	    }
	
	    Beacon getMultiFrameBeacon() {
	        return new Beacon.Builder().SetId1("1")
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .SetServiceUuid(1234)
	                .SetMultiFrameBeacon(true)
	                .Build();
	    }
	
	    Beacon getMultiFrameBeaconUpdateDifferentServiceUUID() {
	        return new Beacon.Builder()
	                .SetBluetoothAddress("01:02:03:04:05:06")
	                .SetServiceUuid(5678)
	                .SetRssi(-50)
	                .SetDataFields(getDataFields())
	                .SetMultiFrameBeacon(true)
	                .Build();
	    }
	
	    [Test]
	    public void trackingManufacturerBeaconReturnsSelf() {
	        Beacon beacon = getManufacturerBeacon();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker();
	        Beacon trackedBeacon = tracker.Track(beacon);
	        AssertEx.AreEqual("Returns itself", trackedBeacon, beacon);
	    }
	
	    [Test]
	    public void gattBeaconExtraDataIsNotReturned() {
	        Beacon extraDataBeacon = getGattBeaconExtraData();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker();
	        Beacon trackedBeacon = tracker.Track(extraDataBeacon);
	        AssertEx.Null("trackedBeacon should be null", trackedBeacon);
	    }
	
	    [Test]
	    public void gattBeaconExtraDataGetUpdated() {
	        Beacon beacon = getGattBeacon();
	        Beacon extraDataBeacon = getGattBeaconExtraData();
	        Beacon extraDataBeacon2 = getGattBeaconExtraData2();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker();
	        tracker.Track(beacon);
	        tracker.Track(extraDataBeacon);
	        tracker.Track(extraDataBeacon2);
	        Beacon trackedBeacon = tracker.Track(beacon);
	        AssertEx.AreEqual("extra data is updated", extraDataBeacon2.DataFields, trackedBeacon.ExtraDataFields);
	    }
	
	    [Test]
	    public void gattBeaconExtraDataAreNotOverwritten() {
	        Beacon beacon = getGattBeacon();
	        Beacon extraDataBeacon = getGattBeaconExtraData();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker();
	        tracker.Track(beacon);
	        tracker.Track(extraDataBeacon);
	        Beacon trackedBeacon = tracker.Track(beacon);
	        AssertEx.AreEqual("extra data should not be overwritten", extraDataBeacon.DataFields, trackedBeacon.ExtraDataFields);
	    }
	
	    [Test]
	    public void gattBeaconFieldsGetUpdated() {
	        Beacon beacon = getGattBeacon();
	        Beacon beaconUpdate = getGattBeaconUpdate();
	        Beacon extraDataBeacon = getGattBeaconExtraData();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker();
	        tracker.Track(beacon);
	        Beacon trackedBeacon = tracker.Track(beaconUpdate);
	        AssertEx.AreEqual("rssi should be updated", beaconUpdate.Rssi, trackedBeacon.Rssi);
	        AssertEx.AreEqual("data fields should be updated", beaconUpdate.DataFields, trackedBeacon.DataFields);
	    }
	
	    [Test]
	    public void multiFrameBeaconDifferentServiceUUIDFieldsNotUpdated() {
	        Beacon beacon = getMultiFrameBeacon();
	        Beacon beaconUpdate = getMultiFrameBeaconUpdateDifferentServiceUUID();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker();
	        tracker.Track(beacon);
	        tracker.Track(beaconUpdate);
	        Beacon trackedBeacon = tracker.Track(beacon);
	        AssertEx.AreNotEqual("rssi should NOT be updated", beaconUpdate.Rssi, trackedBeacon.Rssi);
	        AssertEx.AreNotEqual("data fields should NOT be updated", beaconUpdate.DataFields, trackedBeacon.ExtraDataFields);
	    }
	
	    [Test]
	    public void multiFrameBeaconProgramaticParserAssociationDifferentServiceUUIDFieldsGetUpdated() {
	        Beacon beacon = getMultiFrameBeacon();
	        Beacon beaconUpdate = getMultiFrameBeaconUpdateDifferentServiceUUID();
	        ExtraDataBeaconTracker tracker = new ExtraDataBeaconTracker(false);
	        tracker.Track(beacon);
	        tracker.Track(beaconUpdate);
	        Beacon trackedBeacon = tracker.Track(beacon);
	        AssertEx.AreEqual("rssi should be updated", beaconUpdate.Rssi, trackedBeacon.Rssi);
	        AssertEx.AreEqual("data fields should be updated", beaconUpdate.DataFields, trackedBeacon.ExtraDataFields);
	    }
	}
}
