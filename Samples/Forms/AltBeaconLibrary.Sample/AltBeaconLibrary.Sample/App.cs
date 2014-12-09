using System;
using Xamarin.Forms;

namespace AltBeaconLibrary.Sample
{
	public class App
	{
		public static Page GetMainPage()
		{	
			return new PageOne();
		}
	}

	public class PageOne : ContentPage
	{
		public PageOne()
		{
			Content = new Label {
				Text = "Hello, Forms!",
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var beaconService = DependencyService.Get<IAltBeaconService>();
		}
	}
}

