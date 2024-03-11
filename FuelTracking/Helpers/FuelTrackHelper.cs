using FuelTracking.Core.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelTracking.Helpers
{
	public static class FuelTrackHelper
	{
		private static Chart _chart;

		public static Chart Chart
		{
			get { return _chart; }
			set { _chart = value; }
		}
	}
}
