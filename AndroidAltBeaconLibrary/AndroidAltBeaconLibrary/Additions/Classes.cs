using Java.Lang;

namespace Org.Altbeacon.Beacon
{
    partial class Identifier
    {
        int Java.Lang.IComparable.CompareTo(Java.Lang.Object other)
        {
            return CompareTo(other as Identifier);
        }
    }
}

namespace Org.Altbeacon.Beacon.Distance {
    partial class ModelSpecificDistanceUpdater : Android.OS.AsyncTask
    {
        protected override Object DoInBackground(params Object[] @params)
        {
            return DoInBackground((Java.Lang.Void[])@params);
        }
    }
}