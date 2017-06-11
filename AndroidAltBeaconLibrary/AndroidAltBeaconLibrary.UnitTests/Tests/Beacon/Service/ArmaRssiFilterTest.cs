using System;
using AltBeaconOrg.BoundBeacon.Service;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class ArmaRssiFilterTest
	{
		[Test]
	    public void initTest1() {
	        ArmaRssiFilter filter = new ArmaRssiFilter();
	        filter.AddMeasurement(new Java.Lang.Integer(-50));
	        AssertEx.AreEqual("First measurement should be -50", filter.CalculateRssi().ToString("F1"), "-50.0");
	    }
	}
}
