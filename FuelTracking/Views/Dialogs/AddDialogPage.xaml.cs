using FuelTracking.Core.Enums;
using FuelTracking.Core.Models;
using FuelTracking.ViewModel.Dialogs;
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

namespace FuelTracking.Views.Dialogs
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AddDialogPage : Page
	{
		private readonly ContentDialog _dialog;
		public AddDialogViewModel ViewModel { get { return this.DataContext as AddDialogViewModel; } }

		public AddDialogPage(ContentDialog contentDialog)
		{
			this.InitializeComponent();
			this.DataContext = new AddDialogViewModel();

			_dialog = contentDialog;

			this.GasStationCombobox.ItemsSource = Enum.GetValues<GasStationEnum>();
			this.GasStationCombobox.SelectedItem = Enum.GetValues<GasStationEnum>().First();

			this.CalendarDatePicker.MaxDate = DateTime.Now;

			ViewModel.ErrorsChanged += ViewModel_ErrorsChanged;
		}

		private void ViewModel_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
		{
			_dialog.IsPrimaryButtonEnabled = !ViewModel.HasErrors;
		}

		private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
		{
			if (args.NewDate == null)
			{
				sender.Date = args.OldDate;
			}
		}
	}
}
