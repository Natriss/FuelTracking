using FuelTracking.Core.Models;
using FuelTracking.Helpers;
using FuelTracking.ViewModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FuelTracking.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class FuelTrackingPage : Page
	{
		public FuelTrackingViewModel ViewModel { get { return this.DataContext as FuelTrackingViewModel; } }

		public FuelTrackingPage()
		{
			this.InitializeComponent();
			this.DataContext = new FuelTrackingViewModel();
			this.canvas.Loaded += Canvas_Loaded;
			this.SizeChanged += FuelTrackingPage_SizeChanged;
		}

		private void FuelTrackingPage_SizeChanged(object sender, Microsoft.UI.Xaml.SizeChangedEventArgs e)
		{
			if (this.canvas.IsLoaded)
			{
				DrawCanvas();
			}
		}

		private void Canvas_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
		{
			DrawCanvas();
		}

		private void DrawCanvas()
		{
			FuelTrackHelper.Chart = new(this.canvas);
			FuelTrackHelper.Chart.DrawChart(AppHelper.FuelTrackService.GetFuelTracksByDateAsync(ViewModel.Date).Result);
		}

		private void DatePickerFlyout_DatePicked(DatePickerFlyout sender, DatePickedEventArgs args)
		{
			ViewModel.Date = sender.Date.Date;
			DrawCanvas();
			ViewModel.ItemSource = AppHelper.FuelTrackService.GetFuelTracksByDateAsync(ViewModel.Date).Result;
		}

		private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int selectedIndex = ((ListView)sender).SelectedIndex;
			if (selectedIndex >= 0)
			{
				this.canvas.Children.Where(c => c.GetType() == typeof(Border)).ToList().ForEach(c =>
				{
					((Border)c).Background = Application.Current.Resources["ControlStrongFillColorDefaultBrush"] as Brush;
				});
				((Border)this.canvas.Children.Where(c => c.GetType() == typeof(Border)).ToList()[selectedIndex]).Background = Application.Current.Resources["AccentFillColorDefaultBrush"] as Brush;
			}
			else
			{
				this.canvas.Children.Where(c => c.GetType() == typeof(Border)).ToList().ForEach(c =>
				{
					((Border)c).Background = Application.Current.Resources["AccentFillColorSelectedTextBackgroundBrush"] as Brush;
				});
			}
		}

		private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
		{
			MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
			ViewModel.DeleteCommand.Execute(menuFlyoutItem.DataContext);
		}
	}
}
