using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace ValkyrieMapEditor.Core
{
	public class SettingsManager
	{
		public int CurrentProfileID
		{
			get { return this.currentprofileid; }
		}

		public void Initialize()
		{
			string userdatafolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			FileInfo dbfile = new FileInfo(Path.Combine(userdatafolder, "Griffin\\user.db3"));
			
			if(!dbfile.Exists)
			{
				Directory.CreateDirectory(dbfile.Directory.FullName);

				SQLiteConnection.CreateFile(dbfile.FullName);
				this.SetupNewDatabase(dbfile.FullName);
			}

			this.OpenDatabase(dbfile.FullName);
			this.LoadDefaultProfile();
		}

		public string GetSetting (string name)
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "SELECT value FROM settings WHERE name = @boundparameter1 AND profileid = @boundparameter2;";
			command.Parameters.Add(new SQLiteParameter("@boundparameter1", name));
			command.Parameters.Add(new SQLiteParameter("@boundparameter2", this.currentprofileid));

			return (string)command.ExecuteScalar();
		}

		public void SetSetting (string name, string value)
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "UPDATE settings SET value = @boundparameter1 WHERE name = @boundparameter2 AND profileid = @boundparameter3";
			command.Parameters.Add(new SQLiteParameter("@boundparameter1", value));
			command.Parameters.Add(new SQLiteParameter("@boundparameter2", name));
			command.Parameters.Add(new SQLiteParameter("@boundparameter3", this.currentprofileid));

			command.ExecuteNonQuery();
		}

		public void AddProfile (string name)
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "INSERT INTO profiles (name) VALUES (@boundparameter);";
			command.Parameters.Add(new SQLiteParameter("@boundparameter", name));

			command.ExecuteNonQuery();
		}

		public void RenameCurrentProfile (string name)
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "UPDATE profiles SET name = @boundparameter WHERE id = @boundparameter1;";
			command.Parameters.Add(new SQLiteParameter("@boundparameter", name));
			command.Parameters.Add(new SQLiteParameter("@boundparameter1", this.currentprofileid));

			command.ExecuteNonQuery();
		}

		public bool RemoveProfile (int profileid)
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "DELETE FROM profiles WHERE id = @boundparameter;";
			command.Parameters.Add(new SQLiteParameter("@boundparameter", profileid));

			int affected = command.ExecuteNonQuery();

			return (affected > 0);
		}

		public Dictionary<int, string> GetProfiles ()
		{
			Dictionary<int, string> profiles = new Dictionary<int, string>();

			var command = this.connection.CreateCommand();
			command.CommandText = "SELECT id, name FROM profiles;";
			
			SQLiteDataReader reader = command.ExecuteReader();
			while(reader.Read())
			{
				profiles.Add(reader.GetInt32(0), reader.GetString(1));
			}

			return profiles;
		}

		public void SetCurrentProfile (int profileid)
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "SELECT id FROM profiles WHERE id = @boundparameter;";
			command.Parameters.Add(new SQLiteParameter("@boundparameter", profileid));

			var result = command.ExecuteScalar();
			if(result == null)
				throw new ArgumentOutOfRangeException("id", string.Format("Profile {0} does not exist", profileid));

			command.CommandText = "UPDATE profiles SET current = 0; UPDATE profiles SET current = 1 WHERE id = @boundparameter;";
			command.ExecuteNonQuery();

			this.currentprofileid = profileid;
		}

		private bool IsConnected
		{
			get { return this.connection.State == ConnectionState.Open; }
		}

		private void SetupNewDatabase (string path)
		{
			if(!this.IsConnected)
				this.OpenDatabase(path);

			var command = this.connection.CreateCommand();
			command.CommandText ="CREATE TABLE profiles (id INTEGER PRIMARY KEY, name VARCHAR(50), current INTEGER DEFAULT 0);";
			command.ExecuteNonQuery();

			command.CommandText = "CREATE TABLE settings (id INTEGER PRIMARY KEY, profileid INTEGER NOT NULL, name VARCHAR(50), value TEXT)";
			command.ExecuteNonQuery();

			command.CommandText = "INSERT INTO profiles (name, current) VALUES ('Default', 1); SELECT id FROM profiles WHERE name = 'Default';";
			int result = Convert.ToInt32(command.ExecuteScalar());

			command.CommandText = string.Format("INSERT INTO settings (profileid, name, value) VALUES ({0}, 'AssemblyDirectories', '');", result);
			command.ExecuteNonQuery();
		}


		private void OpenDatabase (string path)
		{
			if(this.IsConnected)
				return;

			this.connection = new SQLiteConnection(string.Format("Data Source={0}", path));
			this.connection.Open();
		}

		private void LoadDefaultProfile()
		{
			var command = this.connection.CreateCommand();
			command.CommandText = "SELECT id FROM profiles WHERE current = 1";
			var result = command.ExecuteScalar();

			if(result == null)
			{
				command.CommandText = "SELECT id FROM profiles LIMIT 1;";
				this.SetCurrentProfile(Convert.ToInt32(command.ExecuteScalar()));
			}
			else
				this.currentprofileid = Convert.ToInt32(result);
		}

		private void CloseDatabase ()
		{
			if(!this.IsConnected)
				return;

			this.connection.Close();
		}

		private SQLiteConnection connection = new SQLiteConnection();
		private int currentprofileid = 0;
	}
}
