using FuelTracking.Core.Enums;
using FuelTracking.Core.Interfaces;
using FuelTracking.Core.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuelTracking.Data.Data
{
	public class SQLiteDataAccess : IFuelTrackService
	{
		private string _connectionString;
		private SqliteConnection _connection;

		public SQLiteDataAccess(string connectionString)
		{
			_connectionString = connectionString;

			_ = InitializeConnectionAsync();

			DatabaseSetUp();
		}

		/// <summary>
		/// Asynchronously initializes the SQLiteConnection.
		/// </summary>
		private async Task InitializeConnectionAsync()
		{
			try
			{
				_connection = new SqliteConnection(string.Format("Data Source={0}", _connectionString));
				await _connection.OpenAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Asynchronously closes the open SQLiteConnection.
		/// </summary>
		/// <param name="dispose">If the connection needs to be disposed. (default = true)</param>
		/// <returns></returns>
		private async Task CloseConnectionAsync(bool dispose = true)
		{
			try
			{
				await _connection.CloseAsync();
				await _connection.DisposeAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Sets up the Database tables for the first time.
		/// </summary>
		private async void DatabaseSetUp()
		{
			try
			{
				string query = "CREATE TABLE IF NOT EXISTS fuel (Id INTEGER PRIMARY KEY," +
					"Name INTEGER NOT NULL, Street TEXT NULL, Number TEXT NULL, PostalCode TEXT NULL, City TEXT NULL," +
					"Volume REAL NOT NULL, Price REAL NOT NULL, Date TEXT NOT NULL);";
				SqliteCommand command = new(query, _connection);
				await command.ExecuteNonQueryAsync();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				await CloseConnectionAsync();
			}
		}

		/// <summary>
		/// Asynchronously adds a fueltrack to the database.
		/// </summary>
		/// <param name="fuelTrack">The fueltrack that needs to be added.</param>
		public async void AddFuelTrackAsync(FuelTrack fuelTrack)
		{
			try
			{
				await InitializeConnectionAsync();

				SqliteCommand command = new(@"INSERT INTO fuel (Name, Street, Number, PostalCode, City, Volume, Price, Date) VALUES " +
					"(@Name, @Street, @Number, @PostalCode, @City, @Volume, @Price, @Date);", _connection);
				command.Parameters.AddWithValue("@Name", (int)fuelTrack.GasStation.Name);
				command.Parameters.AddWithValue("@Street", fuelTrack.GasStation.Street == null ? DBNull.Value : fuelTrack.GasStation.Street);
				command.Parameters.AddWithValue("@Number", fuelTrack.GasStation.Number == null ? DBNull.Value : fuelTrack.GasStation.Number);
				command.Parameters.AddWithValue("@PostalCode", fuelTrack.GasStation.PostalCode == null ? DBNull.Value : fuelTrack.GasStation.PostalCode);
				command.Parameters.AddWithValue("@City", fuelTrack.GasStation.City == null ? DBNull.Value : fuelTrack.GasStation.City);
				command.Parameters.AddWithValue("@Volume", fuelTrack.Volume);
				command.Parameters.AddWithValue("@Price", fuelTrack.Price);
				command.Parameters.AddWithValue("@Date", fuelTrack.Date.ToString("yyyy-MM-dd"));
				await command.ExecuteNonQueryAsync();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				await CloseConnectionAsync();
			}
		}

		/// <summary>
		/// Asynchronously gets all the fueltracks.
		/// </summary>
		/// <returns>A list of type fueltrack.</returns>
		public async Task<List<FuelTrack>> GetAllFuelTracksAsync()
		{
			try
			{
				await InitializeConnectionAsync();

				SqliteCommand command = new("SELECT * FROM fuel;", _connection);
				SqliteDataReader reader = await command.ExecuteReaderAsync();
				List<FuelTrack> fuels = new List<FuelTrack>();
				while (reader.Read())
				{
					fuels.Add(new FuelTrack()
					{
						Id = int.Parse(reader["Id"].ToString()),
						Volume = (double)reader["Volume"],
						Price = (double)reader["Price"],
						Date = DateTime.Parse((string)reader["Date"]),
						GasStation = new GasStation()
						{
							Name = (GasStationEnum)(int.Parse(reader["Name"].ToString())),
							Street = await reader.IsDBNullAsync(2) ? null : (string)reader["Street"],
							Number = await reader.IsDBNullAsync(3) ? null : (string)reader["Number"],
							PostalCode = await reader.IsDBNullAsync(4) ? null : (string)reader["PostalCode"],
							City = await reader.IsDBNullAsync(5) ? null : (string)reader["City"],
						}
					});
				}
				return fuels;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				await CloseConnectionAsync();
			}
		}

		/// <summary>
		/// Asynchronously gets all the fueltracks by a specified date.
		/// </summary>
		/// <param name="date">The specified date</param>
		/// <returns></returns>
		public async Task<List<FuelTrack>> GetFuelTracksByDateAsync(DateTime date)
		{
			try
			{
				await InitializeConnectionAsync();

				SqliteCommand command = new(@"SELECT * FROM fuel WHERE strftime('%Y', Date) = @Year AND strftime('%m', Date) = @Month;", _connection);
				command.Parameters.AddWithValue("@Year", date.Year.ToString());
				command.Parameters.AddWithValue("@Month", date.Month.ToString("00"));
				SqliteDataReader reader = await command.ExecuteReaderAsync();
				List<FuelTrack> fuels = new List<FuelTrack>();
				while (reader.Read())
				{
					fuels.Add(new FuelTrack()
					{
						Id = int.Parse(reader["Id"].ToString()),
						Volume = (double)reader["Volume"],
						Price = (double)reader["Price"],
						Date = DateTime.Parse((string)reader["Date"]),
						GasStation = new GasStation()
						{
							Name = (GasStationEnum)(int.Parse(reader["Name"].ToString())),
							Street = await reader.IsDBNullAsync(2) ? null : (string)reader["Street"],
							Number = await reader.IsDBNullAsync(3) ? null : (string)reader["Number"],
							PostalCode = await reader.IsDBNullAsync(4) ? null : (string)reader["PostalCode"],
							City = await reader.IsDBNullAsync(5) ? null : (string)reader["City"],
						}
					});
				}
				return fuels;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				await CloseConnectionAsync();
			}
		}

		public async Task<List<FuelTrack>> GetSomeFuelTracksAsync(int limit = 10, bool asc = false)
		{
			try
			{
				await InitializeConnectionAsync();

				SqliteCommand command = new(string.Format("SELECT * FROM fuel ORDER BY Id {1} LIMIT {0};", limit, asc ? "ASC" : "DESC"), _connection);
				SqliteDataReader reader = await command.ExecuteReaderAsync();
				List<FuelTrack> fuels = new List<FuelTrack>();
				while (reader.Read())
				{
					fuels.Add(new FuelTrack()
					{
						Id = int.Parse(reader["Id"].ToString()),
						Volume = (double)reader["Volume"],
						Price = (double)reader["Price"],
						Date = DateTime.Parse((string)reader["Date"]),
						GasStation = new GasStation()
						{
							Name = (GasStationEnum)(int.Parse(reader["Name"].ToString())),
							Street = await reader.IsDBNullAsync(2) ? null : (string)reader["Street"],
							Number = await reader.IsDBNullAsync(3) ? null : (string)reader["Number"],
							PostalCode = await reader.IsDBNullAsync(4) ? null : (string)reader["PostalCode"],
							City = await reader.IsDBNullAsync(5) ? null : (string)reader["City"],
						}
					});
				}
				return fuels;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				await CloseConnectionAsync();
			}
		}

		public async Task DeleteFuelTrackByIdAsync(int? id)
		{
			try
			{
				await InitializeConnectionAsync();

				SqliteCommand command = new(string.Format("DELETE FROM fuel WHERE Id == {0};", id), _connection);
				await command.ExecuteNonQueryAsync();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				await CloseConnectionAsync();
			}
		}
	}
}
