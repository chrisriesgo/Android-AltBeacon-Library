using System;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Simulator;
using NUnit.Framework;
using System.Collections.Generic;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BeaconSimulatorTest : TestBase
	{
		[Test]
	    public void testSetBeacons(){
	        StaticBeaconSimulator staticBeaconSimulator = new StaticBeaconSimulator();
	        byte[] beaconBytes = HexStringToByteArray("02011a1bff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509");
	        Beacon beacon = new AltBeaconParser().FromScanData(beaconBytes, -55, null);
	        List<Beacon> beacons = new List<Beacon>();
	        beacons.Add(beacon);
	        staticBeaconSimulator.Beacons = beacons;
	        AssertEx.AreEqual("getBeacons should match values entered with setBeacons", staticBeaconSimulator.Beacons, beacons);
	    }
	
	    [Test]
	    public void testSetBeaconsEmpty(){
	        StaticBeaconSimulator staticBeaconSimulator = new StaticBeaconSimulator();
	        List<Beacon> beacons = new List<Beacon>();
	        staticBeaconSimulator.Beacons = beacons;
	        AssertEx.AreEqual("getBeacons should match values entered with setBeacons even when empty", staticBeaconSimulator.Beacons, beacons);
	    }
	
	    [Test]
	    public void testSetBeaconsNull(){
	        StaticBeaconSimulator staticBeaconSimulator = new StaticBeaconSimulator();
	        staticBeaconSimulator.Beacons = null;
	        AssertEx.AreEqual("getBeacons should return null",staticBeaconSimulator.Beacons, null);
	    }
	}
}
