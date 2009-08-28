using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Net;
using System.Data;

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

		public bool AuthenticateLogin(string username, string password)
		{
			try
			{
				var command = this.connection.CreateCommand();
				command.CommandText = string.Format("SELECT * FROM accounts WHERE \"accountUsername\"='{0}' AND \"accountPassword\"='{1}'", username, password);

				var result = command.ExecuteScalar();
				return (result != DBNull.Value);
			}
			catch
			{
				return false;
			}
		}
	}
}
