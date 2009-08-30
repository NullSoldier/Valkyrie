using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Net;
using System.Data;
using System.Drawing;
using ValkyrieLibrary.Network;

namespace ValkyrieServerLibrary.Network
{
	public class GameServerClient
	{
		private NpgsqlConnection connection;
		
		public GameServerClient(IPAddress address, string username, string password, int port, string database)
		{
			string connectionstring = string.Format("Server={0};User ID={1};Password={2};Port={3};Database={4}",
				address.ToString(), username, password, port, database);
			
			this.connection = new NpgsqlConnection(connectionstring);
		}

		public void Start()
		{
			this.connection.Open();
		}
		

		public bool Connected
		{
			get { return (this.connection.State == ConnectionState.Open
				  || this.connection.State == ConnectionState.Executing
				  || this.connection.State == ConnectionState.Fetching); }
		}

		public uint AuthenticateLogin(string username, string password)
		{
			NpgsqlCommand command = null;
			try
			{
				command = this.connection.CreateCommand();
				command.CommandText = string.Format("SELECT * FROM accounts WHERE \"accountUsername\"='{0}' AND \"accountPassword\"='{1}'", username, password);

				var result = command.ExecuteScalar();

				if (result != DBNull.Value)
					return (uint)Convert.ToUInt64(result);
				else
					return 0;
			}
			catch
			{
				return 0;
			}
			finally
			{
				if (command != null)
					command.Dispose();
			}
		}

		public CharacterDetails GetCharacterDetails(uint accountid)
		{
			NpgsqlCommand command = null;
			NpgsqlDataReader reader = null;

			try
			{
				command = this.connection.CreateCommand();
				command.CommandText = string.Format("SELECT \"characterName\", \"characterMapX\", \"characterMapY\", \"characterTileSheet\" FROM characters WHERE \"characterAccountID\"='{0}'", accountid);

				reader = command.ExecuteReader();
				if (reader.Read())
				{
					CharacterDetails details = new CharacterDetails();
					details.Name = reader.GetString(0);
					details.MapLocation = new Point((int)reader.GetInt64(1), (int)reader.GetInt64(2));
					details.TileSheet = reader.GetString(3);

					return details;
				}
				else
					return null;
			}
			catch
			{
				return null;
			}
			finally
			{
				command.Dispose();
				reader.Dispose();
			}
		}

		public void SaveCharacterDetails(NetworkPlayer player)
		{
			NpgsqlCommand command = null;
			try
			{
				command = this.connection.CreateCommand();
				command.CommandText = string.Format("UPDATE characters SET \"characterMapX\"='{0}', \"characterMapY\"='{1}' WHERE \"characterAccountID\"={2}", player.Location.X / 32, player.Location.Y / 32, player.AccountID);

				int result = command.ExecuteNonQuery();

				if (result <= 0)
					throw new SystemException("Error saving character.");
			}
			finally
			{
				if(command != null)
					command.Dispose();
			}
		}
	}
}
