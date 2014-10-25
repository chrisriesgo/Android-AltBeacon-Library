using Android.App;
using Android.OS;

namespace AndroidAltBeaconLibrary.Sample
{
	[Activity(Label = "BackgroundActivity")]			
	public class BackgroundActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ActivityBackground);
		}
	}
}

