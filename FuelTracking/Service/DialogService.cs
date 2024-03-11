using FuelTracking.Helpers;
using FuelTracking.ViewModel.Dialogs;
using FuelTracking.Views.Dialogs;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;

namespace FuelTracking.Service
{
	public static class DialogService
	{
		public static async Task<ContentDialogResult> ShowAddDialogAsync()
		{
			ContentDialog contentDialog = new()
			{
				XamlRoot = App.Window.Content.XamlRoot,
				Title = "Add record",
				PrimaryButtonText = "Add",
				CloseButtonText = "Cancel",
				DefaultButton = ContentDialogButton.Primary,
				IsPrimaryButtonEnabled = false,
			};
			contentDialog.Content = new AddDialogPage(contentDialog);

			ContentDialogResult result = await contentDialog.ShowAsync();

			if (result == ContentDialogResult.None)
			{
				return result;
			}

			AppHelper.FuelTrackService.AddFuelTrack(((AddDialogViewModel)((AddDialogPage)contentDialog.Content).DataContext).GetFuelTrack());

			return result;
		}
	}
}
