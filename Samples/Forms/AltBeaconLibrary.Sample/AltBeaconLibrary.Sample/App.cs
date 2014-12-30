using System;
using Xamarin.Forms;

namespace AltBeaconLibrary.Sample
{
	public class App : Application
	{
		public App()
		{
			MainPage = App.GetMainPage();
		}

		public static Page GetMainPage()
		{	
			return new MainPage();
		}
	}
}