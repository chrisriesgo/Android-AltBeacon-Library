using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace AltBeaconLibrary.Sample
{
	public class MainPage : ContentPage
	{
		ObservableCollection<CommonBeacon> _data = new ObservableCollection<CommonBeacon>();
		ListView _list;

		public MainPage()
		{
			BackgroundColor = Color.White;

			_data = new ObservableCollection<CommonBeacon>();
//			_data.Add(new CommonBeacon {
//				Id = "E4C8A4FC-F68B-470D-959F-29382AF72CE7",
//				Distance = "1.3m",
//			});

			Content = BuildContent();
		}

		private View BuildContent()
		{
			_list = new ListView {
				VerticalOptions = LayoutOptions.FillAndExpand,
				ItemTemplate = new DataTemplate(typeof(ListItemView)),
				RowHeight = 90,
				ItemsSource = _data,
			};

			return _list;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var beaconService = DependencyService.Get<IAltBeaconService>();
			beaconService.ListChanged += (sender, e) => 
			{
				_data = new ObservableCollection<CommonBeacon>(e.Data);
			};
			beaconService.StartMonitoring("");
			beaconService.StartRanging("");
		}
	}

	public class CommonBeacon
	{
		public string Id { get; set; }
		public string Distance { get; set; }
	}
}

