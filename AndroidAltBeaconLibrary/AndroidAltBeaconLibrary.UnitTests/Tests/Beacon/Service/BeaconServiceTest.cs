using System;
using AltBeaconOrg.BoundBeacon;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class BeaconServiceTest
	{
		[SetUp]
	    public void before() {
	        BeaconManager.ManifestCheckingDisabled = true;
	    }
	
	    /**
	     * This test verifies that processing a beacon in a scan (which starts its own thread) does not
	     * affect the size of the available threads in the main Android AsyncTask.THREAD_POOL_EXECUTOR
	     * @throws Exception
	     */
	    [Test]
	    [Ignore("Can't test real beacons")]
	    public void beaconScanCallbackTest() {
	        //final ServiceController<BeaconService> beaconServiceServiceController =
	        //        Robolectric.buildService(BeaconService.class);
	        //beaconServiceServiceController.attach();
	        //BeaconService beaconService = beaconServiceServiceController.get();
	        //beaconService.onCreate();
	        //CycledLeScanCallback callback = beaconService.mCycledLeScanCallback;
	
	        //ThreadPoolExecutor executor = (ThreadPoolExecutor) AsyncTask.THREAD_POOL_EXECUTOR;
	        //int activeThreadCountBeforeScan = executor.getActiveCount();
	
	        //byte[] scanRecord = new byte[1];
	        //callback.onLeScan(null, -59, scanRecord);
	
	        //int activeThreadCountAfterScan = executor.getActiveCount();
	
	        //assertEquals("The size of the Android thread pool should be unchanged by beacon scanning",
	        //        activeThreadCountBeforeScan, activeThreadCountAfterScan);
	
	        //// Need to sleep here until the thread in the above method completes, otherwise an exception
	        //// is thrown.  Maybe we don't care about this exception, so we could remove this.
	        //Thread.sleep(100);
	    }
	}
}
