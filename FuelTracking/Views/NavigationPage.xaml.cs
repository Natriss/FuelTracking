using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FuelTracking.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NavigationPage : Page
	{
		public NavigationPage()
		{
			this.InitializeComponent();
			App.Window.SetTitleBar(this.AppTitleBar);
			AppNavigationView.SelectionChanged += AppNavigationView_SelectionChanged;
			AppNavigationView.SelectedItem = AppNavigationView.MenuItems.First();
		}

		private void AppNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
		{
			if (args.IsSettingsSelected)
			{
				
			}
			else
			{
				NavigationViewItem selectedItem = (NavigationViewItem)sender.SelectedItem;
				sender.Header = selectedItem.Content as string;
				string pagePath = string.Format("FuelTracking.Views.{0}", selectedItem.Tag.ToString());
				Type page = Type.GetType(pagePath);
				this.ContentFrame.Navigate(page);
			}
		}

		/// <summary>
		/// When de DisplayMode changes. We set the correct VisualState for the AppTitleBar 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void AppNavigationView_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
		{
			if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
			{
				VisualStateManager.GoToState(this, "Minimal", true);
			}
			else
			{
				VisualStateManager.GoToState(this, "Normal", true);
			}
		}
	}
}
