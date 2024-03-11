using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Windows.Input;

namespace FuelTracking.Core.Models
{
	public class FuelTrack
	{
		public int? Id { get; set; }
		public GasStation GasStation { get; set; }
		public double Volume { get; set; }
		public double Price { get; set; }
		public DateTime Date { get; set; }

		public override string ToString()
		{
			return string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8}", Id, GasStation.Name, GasStation.Street, GasStation.Number, GasStation.PostalCode, GasStation.City, Volume, Price, Date);
		}
	}
}
