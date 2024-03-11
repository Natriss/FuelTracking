using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FuelTracking.Core.Models;
using FuelTracking.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using WinRT.Interop;
using FuelTracking.Service;

namespace FuelTracking.ViewModel
{
	public class FuelTrackingViewModel : ObservableObject
	{
		public IRelayCommand AddCommand { get; private set; }
		public IRelayCommand ExportCommand { get; private set; }
		public IRelayCommand DeleteCommand { get; private set; }
		private DateTime _date = DateTime.Today;

		public DateTime Date
		{
			get { return _date; }
			set { _date = value; OnPropertyChanged(nameof(Date)); }
		}

		private List<FuelTrack> _fuelTracks;
		public List<FuelTrack> ItemSource
		{
			get { return _fuelTracks; }
			set { _fuelTracks = value; OnPropertyChanged(nameof(ItemSource)); }
		}

		private FuelTrack _selectedItem;

		public FuelTrack SelectedItem
		{
			get { return _selectedItem; }
			set { _selectedItem = value; OnPropertyChanged(nameof(SelectedItem)); }
		}

		public FuelTrackingViewModel()
		{
			_fuelTracks = AppHelper.FuelTrackService.GetFuelTracksByDateAsync(Date).Result;
			AddCommand = new RelayCommand(Add);
			ExportCommand = new RelayCommand(Export);
			DeleteCommand = new RelayCommand<FuelTrack>(Delete);
		}

		private async void Delete(FuelTrack fuelTrack)
		{
			await AppHelper.FuelTrackService.DeleteFuelTrackByIdAsync(fuelTrack.Id);
			FuelTrackHelper.Chart.DrawChart(AppHelper.FuelTrackService.GetFuelTracksByDateAsync(Date).Result);
			ItemSource = AppHelper.FuelTrackService.GetFuelTracksByDateAsync(Date).Result;
		}

		private async void Export()
		{
			FileSavePicker savePicker = new()
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				SuggestedFileName = "All tanked moments"
			};
			savePicker.FileTypeChoices.Add("Comma Separated Values", new List<string>() { ".csv" });
			savePicker.FileTypeChoices.Add("JavaScript Object Notation", new List<string>() { ".json" });

			IntPtr hWnd = WindowNative.GetWindowHandle(App.Window);
			InitializeWithWindow.Initialize(savePicker, hWnd);

			StorageFile file = await savePicker.PickSaveFileAsync();
			if (file != null)
			{
				// Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
				CachedFileManager.DeferUpdates(file);
				List<FuelTrack> list = AppHelper.FuelTrackService.GetAllFuelTracksAsync().Result;
				string formattedString = file.DisplayType switch
				{
					"JSON File" => JsonSerializer.Serialize(list, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull }),
					_ => string.Join<FuelTrack>(',', list)
				};
				await FileIO.WriteTextAsync(file, formattedString);

				// Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
				// Completing updates may require Windows to ask for user input.
				FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
			}
		}

		private async void Add()
		{
			await DialogService.ShowAddDialogAsync();
			
			FuelTrackHelper.Chart.DrawChart(AppHelper.FuelTrackService.GetFuelTracksByDateAsync(Date).Result);
			ItemSource = AppHelper.FuelTrackService.GetFuelTracksByDateAsync(Date).Result;
		}
	}
}
