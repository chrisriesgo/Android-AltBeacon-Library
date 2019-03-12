using System;
using NUnit.Framework;
using AltBeaconOrg.BoundBeacon;
using Android.OS;
using System.Text;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[TestFixture]
	public class SBeaconTest : TestBase
	{
		[Test]
	    public void testDetectsSBeacon() {
	        byte[] bytes = HexStringToByteArray("02011a1bff1801031501000100c502000000000000000003");
	        SBeaconParser parser = new SBeaconParser();
	        SBeacon sBeacon = (SBeacon) parser.FromScanData(bytes, -55, null);
	        AssertEx.NotNull("SBeacon should be not null if parsed successfully", sBeacon);
	        AssertEx.AreEqual("id should be parsed", "0x000000000003", sBeacon.Id);
	        AssertEx.AreEqual("group should be parsed", 1, sBeacon.Group);
	        AssertEx.AreEqual("time should be parsed", 2, sBeacon.Time);
	        AssertEx.AreEqual("txPower should be parsed", -59, sBeacon.TxPower);
	    }
	
	    protected class SBeacon : Beacon {
	        int mTime;
	
	        public SBeacon(int grouping, String id, int time, int txPower, int rssi, int beaconTypeCode, String bluetoothAddress) {
	            TxPower = txPower;
				Rssi = rssi;
	            BeaconTypeCode = beaconTypeCode;
	            BluetoothAddress = bluetoothAddress;
				Identifiers = new System.Collections.Generic.List<Identifier>();
	            Identifiers.Add(Identifier.FromInt(grouping));
	            Identifiers.Add(Identifier.Parse(id));
	            mTime = time;
	        }
	
	        public SBeacon(Parcel parcel) {
	            // TODO: Implement me
	        }
	
	        public int Group => ((Identifier)Identifiers[0]).ToInt();

			public int Time => mTime;

			public String Id => Identifiers[1].ToString();
	        
			public int DescribeContents()
			{
				return 0;
			}
			
			public void WriteToParcel(Parcel @out, ParcelableWriteFlags flags)
			{
				// TODO: Implement me
			}
	    }
	
	    internal class SBeaconParser : BeaconParser {
	        //private static final String TAG = "SBeaconParser";

			public override Beacon FromScanData(byte[] scanData, int rssi, Android.Bluetooth.BluetoothDevice device)
			{
				int startByte = 2;
	            while (startByte <= 5) {
	                // "m:2-3=0203,i:2-2,i:7-8,i:14-19,d:10-13,p:9-9"
	                if (((int)scanData[startByte+3] & 0xff) == 0x03 &&
	                        ((int)scanData[startByte+4] & 0xff) == 0x15) {
	                    //BeaconManager.logDebug(TAG, "This is a SBeacon beacon advertisement");
	                    // startByte+0 company id (2 bytes)
	                    // startByte+2 = 02 (1) byte header
	                    // startByte+3 = 0315 (2 bytes) header
	                    // startByte+5 = Beacon Type 0x01
	                    // startByte+6 = Reserved (1 bytes)
	                    // startByte+7 = Security Code (2 bytes) => Major little endian
	                    // startByte+9 = Tx Power => Tx Power
	                    // startByte+10 = Timestamp (4 bytes) => Minor (2 LSBs) little endian
	                    // startByte+14 = Beacon ID (6 bytes) -> UUID little endian
	                    int grouping = (scanData[startByte+8] & 0xff) * 0x100 + (scanData[startByte+7] & 0xff);
	                    int clock = (scanData[startByte+13] & 0xff) * 0x1000000 + (scanData[startByte+12] & 0xff) * 0x10000 + (scanData[startByte+11] & 0xff) * 0x100 + (scanData[startByte+10] & 0xff);
	                    int txPower = (int)(sbyte)scanData[startByte+9]; // this one is signed
	
	                    byte[] beaconId = new byte[6];
	                    //Java.Lang.JavaSystem.Arraycopy(scanData, startByte+14, beaconId, 0, 6);
	                    Array.Copy(scanData, startByte+14, beaconId, 0, 6);
	                    String hexString = BytesToHex(beaconId);
	                    StringBuilder sb = new StringBuilder();
	                    sb.Append(hexString.Substring(0,12));
	                    String id = "0x" + sb.ToString();
	                    int beaconTypeCode = (scanData[startByte+3] & 0xff) * 0x100 + (scanData[startByte+2] & 0xff);
	
	
	                    String mac = null;
	                    if (device != null) {
	                        mac = device.Address;
	                    }
	                    Beacon beacon = new SBeacon(grouping, id, clock, txPower, rssi, beaconTypeCode, mac);
	                    return beacon;
	                }
	                startByte++;
	            }
	            return null;
			}
	    }
	}
}
