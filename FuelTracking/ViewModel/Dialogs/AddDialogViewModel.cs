using CommunityToolkit.Mvvm.ComponentModel;
using FuelTracking.Core.Enums;
using FuelTracking.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FuelTracking.ViewModel.Dialogs
{
	public class AddDialogViewModel : ObservableValidator
	{
		private GasStationEnum _gastStation = GasStationEnum.Gabriëls;

		public GasStationEnum GasStation
		{
			get { return _gastStation; }
			set
			{
				if (value != _gastStation)
				{
					_gastStation = value;
					OnPropertyChanged(nameof(GasStation));
				}
			}
		}

		private string _street;

		public string Street
		{
			get { return _street; }
			set { _street = value; OnPropertyChanged(nameof(Street)); }
		}

		private string _number;

		public string Number
		{
			get { return _number; }
			set { _number = value; OnPropertyChanged(nameof(Number)); }
		}

		private string _postalCode;

		public string PostalCode
		{
			get { return _postalCode; }
			set { _postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
		}

		private string _city;

		public string City
		{
			get { return _city; }
			set { _city = value; OnPropertyChanged(nameof(City)); }
		}

		private string _volume;

		[CustomValidation(typeof(AddDialogViewModel), nameof(CheckVolume))]
		public string Volume
		{
			get { return _volume; }
			set { SetProperty(ref _volume, value, true, nameof(Volume)); }
		}

		private string _price;

		[CustomValidation(typeof(AddDialogViewModel), nameof(CheckPrice))]
		public string Price
		{
			get { return _price; }
			set { SetProperty(ref _price, value, true, nameof(Price)); }
		}

		private DateTimeOffset? _selectedDate = DateTimeOffset.Now;

		public DateTimeOffset? SelectedDate
		{
			get { return _selectedDate; }
			set
			{
				if (value == null)
				{
					return;
				}
				_selectedDate = value;
				OnPropertyChanged(nameof(SelectedDate));
			}
		}

		public AddDialogViewModel()
		{
			ValidateProperty(_volume, nameof(Volume));
			ValidateProperty(_price, nameof(Price));
		}

		public static ValidationResult CheckVolume(string volume)
		{
			if (string.IsNullOrWhiteSpace(volume))
			{
				return new("Volume can't be empty.");
			}
			if (!double.TryParse(volume, out _) || volume.Any(c => char.IsLetter(c)))
			{
				return new("Volume is not a number.");
			}
			return ValidationResult.Success;
		}

		public static ValidationResult CheckPrice(string price)
		{
			if (string.IsNullOrWhiteSpace(price))
			{
				return new("Price can't be empty.");
			}
			if (!double.TryParse(price, out _) || price.Any(c => char.IsLetter(c)))
			{
				return new("Price is not a number.");
			}
			return ValidationResult.Success;
		}

		public FuelTrack GetFuelTrack()
		{
			if (!HasErrors)
			{
				GasStation gasStation = new() { Name = _gastStation, Street = _street, Number = _number, PostalCode = _postalCode, City = _city };
				return new FuelTrack { GasStation = gasStation, Price = double.Parse(string.Format("{0:0.00}", _price)), Volume = double.Parse(string.Format("{0:0.00}", _volume)), Date = (DateTime)(_selectedDate?.Date) };
			}
			return null;
		}
	}
}
