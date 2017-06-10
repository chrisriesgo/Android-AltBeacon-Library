using System;
using AltBeaconOrg.BoundBeacon;
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
	        var parcel = Parcel.Obtain();
	        var beacon = new AltBeacon.Builder().SetMfgReserved(7).SetId1("1").SetId2("2").SetId3("3").SetRssi(4)
	                .SetBeaconTypeCode(5).SetTxPower(6)
	                .SetBluetoothAddress("1:2:3:4:5:6").Build();
	        beacon.WriteToParcel(parcel, 0);
	        parcel.SetDataPosition(0);
			var beacon2 = new AltBeacon(parcel);
	        Assert.AreEqual(((AltBeacon)beacon).MfgReserved, ((AltBeacon)beacon2).MfgReserved, "beaconMfgReserved is same after deserialization");
	    }
	}
}