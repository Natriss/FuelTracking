using System;

namespace FuelTracking.Core.Models
{
	public class FuelTrack
	{
		public GasStation GasStation { get; set; }
		public double Volume { get; set; }
		public double Price { get; set; }
		public DateTime Date { get; set; }
	}
}
