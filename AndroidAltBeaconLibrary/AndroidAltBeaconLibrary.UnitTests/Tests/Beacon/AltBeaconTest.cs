using System;
using Org.Altbeacon.Beacon;
using Android.OS;
using NUnit.Framework;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class AltBeaconTest : TestBase
	{
		[Test]
		public void TestRecognizeBeacon() 
		{
			var bytes = HexStringToByteArray("02011a1bff1801beac2f234454cf6d4a0fadf2f4911ba9ffa600010002c509");
			var parser = new AltBeaconParser();
			var beacon = parser.FromScanData(bytes, -55, null);
			Assert.AreEqual(9, ((AltBeacon) beacon).MfgReserved, "manData should be parsed");
		}
		
		[Test]
		public void TestCanSerializeParcelable() 
		{
	    }
	}
}