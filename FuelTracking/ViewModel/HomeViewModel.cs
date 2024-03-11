using CommunityToolkit.Mvvm.ComponentModel;
using FuelTracking.Core.Models;
using FuelTracking.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelTracking.ViewModel
{
	public class HomeViewModel : ObservableObject
	{
		private List<FuelTrack> _fuelTracks;
		public List<FuelTrack> ItemSource
		{
			get { return _fuelTracks; }
			set { _fuelTracks = value; OnPropertyChanged(nameof(ItemSource)); }
		}

		public HomeViewModel()
		{
			ItemSource = AppHelper.FuelTrackService.GetSomeFuelTracksAsync().Result;
		}
	}
}
