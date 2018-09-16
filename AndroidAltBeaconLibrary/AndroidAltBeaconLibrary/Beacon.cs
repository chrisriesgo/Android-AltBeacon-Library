using System;
using Android.Runtime;

namespace AltBeaconOrg.BoundBeacon
{
    // Metadata.xml XPath class reference: path="/api/package[@name='org.altbeacon.beacon']/class[@name='Beacon']"
    public partial class Beacon
    {
        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Android.OS.Parcel p, Android.OS.ParcelableWriteFlags f)
        {

        }
    }
}

