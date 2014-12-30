using System;
using Xamarin.Forms;

namespace AltBeaconLibrary.Sample
{
	public class App : Application
	{
		public App()
		{
			var page = App.GetMainPage();
			NavigationPage.SetHasNavigationBar(page, true);
			MainPage = new NavigationPage(page);
		}

		public static Page GetMainPage()
		{	
			return new MainPage();
		}
	}
}