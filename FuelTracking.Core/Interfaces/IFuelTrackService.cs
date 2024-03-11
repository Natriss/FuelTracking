using FuelTracking.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuelTracking.Core.Interfaces
{
	public interface IFuelTrackService
	{
		void AddFuelTrackAsync(FuelTrack fuelTrack);
		Task<List<FuelTrack>> GetAllFuelTracksAsync();
		Task<List<FuelTrack>> GetSomeFuelTracksAsync(int limit = 10, bool asc = false);
		Task<List<FuelTrack>> GetFuelTracksByDateAsync(DateTime date);
		Task DeleteFuelTrackByIdAsync(int? id);
	}
}
