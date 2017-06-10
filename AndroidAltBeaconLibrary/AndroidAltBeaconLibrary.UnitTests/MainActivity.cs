using System.Reflection;

using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;

namespace AndroidAltBeaconLibrary.UnitTests
{
	[Activity(Label = "AltBeacon Tests", MainLauncher = true)]
	public class MainActivity : TestSuiteActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			// tests can be inside the main assembly
			AddTest(Assembly.GetExecutingAssembly());
			// or in any reference assemblies
			// AddTest (typeof (Your.Library.TestClass).Assembly);

			// Once you called base.OnCreate(), you cannot add more assemblies.
			base.OnCreate(bundle);
		}
	}
	
	public static class Helpers
	{
	    public static T Cast<T>(this Java.Lang.Object obj) where T : class
	    {
	        var propertyInfo = obj.GetType().GetProperty("Instance");
	        return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
	    }
	}
}