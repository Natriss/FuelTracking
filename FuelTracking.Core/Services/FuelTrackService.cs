using FuelTracking.Core.Interfaces;
using FuelTracking.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuelTracking.Core.Services
{
	public class FuelTrackService
	{
		private IFuelTrackService _service;

		public FuelTrackService(IFuelTrackService service)
		{
			_service = service;
		}

		public void AddFuelTrack(FuelTrack fuelTrack)
		{
			_service.AddFuelTrackAsync(fuelTrack);
		}

		public async Task<List<FuelTrack>> GetAllFuelTracksAsync()
		{
			return await _service.GetAllFuelTracksAsync();
		}

		public async Task<List<FuelTrack>> GetSomeFuelTracksAsync(int limit = 10, bool asc = false)
		{
			return await _service.GetSomeFuelTracksAsync(limit, asc);
		}

		public async Task<List<FuelTrack>> GetFuelTracksByDateAsync(DateTime date)
		{
			return await _service.GetFuelTracksByDateAsync(date);
		}

		public async Task DeleteFuelTrackByIdAsync(int? id)
		{
			await _service.DeleteFuelTrackByIdAsync(id);
		}
	}
}
