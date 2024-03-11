using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuelTracking.Helpers
{
	public static class ResourceHelper
	{
		private static readonly ResourceLoader _resourceLoader = new();
		public static string GetLocalized(this string propertyName)
		{
			return _resourceLoader.GetString(propertyName);
		}
	}
}
