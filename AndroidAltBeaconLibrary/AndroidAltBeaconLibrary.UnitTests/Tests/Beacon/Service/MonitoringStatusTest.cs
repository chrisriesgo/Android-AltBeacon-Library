using System;
using AltBeaconOrg.BoundBeacon;
using AltBeaconOrg.BoundBeacon.Service;
using Android.Content;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class MonitoringStatusTest
	{
		//[SetUp]
	    //public void before() {
	    //    BeaconManager.SetsManifestCheckingDisabled(true);
	    //}
	
	    //[Test]
	    //public void savesStatusOfUpTo50RegionsTest() {
	    //    Context context = Android.App.Application.Context;
	    //    MonitoringStatus monitoringStatus = new MonitoringStatus(context);
	    //    for (int i = 0; i < 50; i++) {
	    //        Region region = new Region(""+i, null, null, null);
	    //        monitoringStatus.AddRegion(region, null);
	    //    }
	    //    monitoringStatus.saveMonitoringStatusIfOn();
	    //    MonitoringStatus monitoringStatus2 = new MonitoringStatus(context);
	    //    AssertEx.AreEqual("restored regions should be same number as saved", 50, monitoringStatus2.regions().size());
	    //}
	
	    //[Test]
	    //public void clearsStatusOfOver50RegionsTest() {
	    //    Context context = Android.App.Application.Context;
	    //    MonitoringStatus monitoringStatus = new MonitoringStatus(context);
	    //    for (int i = 0; i < 51; i++) {
	    //        Region region = new Region(""+i, null, null, null);
	    //        monitoringStatus.AddRegion(region, null);
	    //    }
	    //    monitoringStatus.saveMonitoringStatusIfOn();
	    //    MonitoringStatus monitoringStatus2 = new MonitoringStatus(context);
	    //    AssertEx.AreEqual("restored regions should be none", 0, monitoringStatus2.regions().size());
	    //}
	
	    //[Test]
	    //public void refusesToRestoreRegionsIfTooMuchTimeHasPassedSinceSavingTest() {
	    //    Context context = Android.App.Application.Context;
	    //    MonitoringStatus monitoringStatus = new MonitoringStatus(context);
	    //    for (int i = 0; i < 50; i++) {
	    //        Region region = new Region(""+i, null, null, null);
	    //        monitoringStatus.AddRegion(region, null);
	    //    }
	    //    monitoringStatus.saveMonitoringStatusIfOn();
	    //    // Set update time to one hour ago
	    //    monitoringStatus.updateMonitoringStatusTime(System.currentTimeMillis() - 1000*3600l);
	    //    MonitoringStatus monitoringStatus2 = new MonitoringStatus(context);
	    //    AssertEx.AreEqual("restored regions should be none", 0, monitoringStatus2.regions().size());
	    //}
	
	    //[Test]
	    //public void allowsAccessToRegionsAfterRestore() {
	    //    Context context = Android.App.Application.Context;
	    //    MonitoringStatus monitoringStatus = new MonitoringStatus(context);
	    //    for (int i = 0; i < 50; i++) {
	    //        Region region = new Region(""+i, null, null, null);
	    //        monitoringStatus.AddRegion(region, null);
	    //    }
	    //    monitoringStatus.saveMonitoringStatusIfOn();
	    //    BeaconManager beaconManager = BeaconManager.GetInstanceForApplication(context);
	    //    var regions = beaconManager.MonitoredRegions;
	    //    AssertEx.AreEqual("beaconManager should return restored regions", 50, regions.size());
	    //}
	}
}
