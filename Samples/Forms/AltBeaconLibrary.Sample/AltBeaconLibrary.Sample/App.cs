using System;
using Xamarin.Forms;

namespace AltBeaconLibrary.Sample
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage( App.GetMainPage() );
		}

		public static Page GetMainPage()
		{	
			return new MainPage();
		}
	}
}