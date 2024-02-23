using FuelTracking.Core.Enums;

namespace FuelTracking.Core.Models
{
	public class GasStation
	{
		public GasStationEnum Name { get; set; }
#nullable enable
		public string? Street { get; set; }
		public string? Number { get; set; }
		public string? PostalCode { get; set; }
		public string? City { get; set; }
#nullable disable
	}
}
