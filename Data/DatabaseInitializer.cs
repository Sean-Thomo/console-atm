using System;
using System.Data.SQLite;

namespace ATM.Data
{
    public static class DatabaseInitializer
    {
        private static readonly string connectionString = "Data Source=Data/ATM.db;Version=3;";

        public static void InitializeDatabase()
        {
            SQLiteConnection sqlite_conn = CreateConnection();
            CreateUsersTable(sqlite_conn);
        }

        private static SQLiteConnection CreateConnection(){
            try
            {
                SQLiteConnection sqlite_conn = new(connectionString);
                sqlite_conn.Open();
                return sqlite_conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        private static void CreateUsersTable(SQLiteConnection sqlite_conn)
        {
            string createTableSQL = @"CREATE TABLE IF NOT EXISTS Users (
                AccountNumber TEXT PRIMARY KEY,
                UserName TEXT NOT NULL,
                Pin TEXT NOT NULL,
                Balance DECIMAL NOT NULL,
                Currency TEXT NOT NULL,
                IsLogedIn BOOLEAN NOT NULL
            )";

            SQLiteCommand createTableCmd = new(createTableSQL, sqlite_conn);
            createTableCmd.ExecuteNonQuery();
        }
    }    
}