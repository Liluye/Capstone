using System;
using System.Collections.Generic;

using DarkRift;
using DarkRift.ConfigTools;
using DarkRift.Storage;

using MySql.Data.MySqlClient;

namespace DBConnector
{
	class Connector : Database
	{
		public override string name {
			get {
				return "DBConnector";
			}
		}

		public override string version {
			get {
				return "0.1";
			}
		}

		public override Command[] commands {
			get {
				return new Command[0];
			}
		}

		public override string author {
			get {
				return "Liluye";
			}
		}


		public override string supportEmail {
			get {
				return "april.lautenbach@gmail.com";
			}
		}

		public override string databaseName {
			get {
				return "MySQL";
			}
		}

		string connectionString = "";

		//Create a settings file if it doesn't exist, and read settings file
		public Connector() {
			InstallSubdirectory(
				new Dictionary<string, byte[]> {
					{"settings.cnf",    System.Text.Encoding.ASCII.GetBytes("ConnectionString:\t\tSERVER=localhost;DATABASE=DarkRift;USERNAME=username;PASSWORD=password")}
				}
			);

			try {
				ConfigReader reader = new ConfigReader(GetSubdirectory() + @"\settings.cnf");

				if( reader["ConnectionString"] == null ) {
					Interface.LogFatal("ConnectionString was not defined in the DBConnector's settings.cnf file!");
					DarkRiftServer.Close(true);
				}
				connectionString = reader["ConnectionString"];
			}
			catch(System.IO.FileNotFoundException) {
				Interface.LogFatal("DBConnector's settings file is missing!");
				DarkRiftServer.Close(true);
			}
		}

		//Function to query DB and return an array of rows
		public override DatabaseRow[] ExecuteQuery (string query, params QueryParameter[] parameters) {
			try {
				using (MySqlConnection connection = new MySqlConnection (connectionString)) {
					using (MySqlCommand command = new MySqlCommand (query, connection)) {
						foreach (QueryParameter parameter in parameters){
							command.Parameters.AddWithValue(parameter.name, parameter.obj);
						}

						connection.Open();

						using (MySqlDataReader reader = command.ExecuteReader()) {
							int fieldCount = reader.FieldCount;
							List<DatabaseRow> rows = new List<DatabaseRow> ();

							while (reader.Read()){
								//For each row create a DatabaseRow
								DatabaseRow row = new DatabaseRow();

								//And add each field to it
								for (int i = 0; i < fieldCount; i++) {
									row.Add(reader.GetName(i),reader.GetValue(i));
								}

								//Add it to the rows
								rows.Add(row);
							}

							return rows.ToArray();
						}
					}
				}
			}
			catch(MySqlException e){
				throw new DatabaseException(e.Message, e);
			}
		}

		//Remove characters that would allow SQL injection
		public override string EscapeString(string s){
			return MySql.Data.MySqlClient.MySqlHelper.EscapeString (s);
		}


		public override void Dispose(){

		}
	}
}
