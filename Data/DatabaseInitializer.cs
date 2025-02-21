using System;
using System.Data.SQLite;
using System.IO;

namespace ATM.Data
{
    public static class DatabaseInitializer
    {
        private static readonly string databasePath = "ATM.db";
        private static readonly string connectionString = $"Data Source={databasePath};Version=3;";

        public static void InitializeDatabase()
        {
            EnsureDatabaseExists();
            using SQLiteConnection sqlite_conn = CreateConnection();
            if (sqlite_conn != null)
            {
                CreateUsersTable(sqlite_conn);
                InsertUsers(sqlite_conn);
            }
        }

        private static void EnsureDatabaseExists()
        {
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
            }
        }

        private static SQLiteConnection CreateConnection()
        {
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

            using SQLiteCommand createTableCmd = new(createTableSQL, sqlite_conn);
            createTableCmd.ExecuteNonQuery();
        }

        private static void InsertUsers(SQLiteConnection sqlite_conn)
        {
            string insertSQL = @"INSERT INTO Users (AccountNumber, UserName, Pin, Balance, Currency, IsLogedIn) 
                                VALUES (@AccountNumber, @UserName, @Pin, @Balance, @Currency, @IsLogedIn) 
                                ON CONFLICT(AccountNumber) DO NOTHING;";

            using SQLiteCommand insertCmd = new(insertSQL, sqlite_conn);

            var users = new[]
            {
                new { UserName = "Sean Thomo", AccountNumber = "1234", PIN = "1234", Balance = 4179.0, Currency = "ZAR", IsLogedIn = false },
                new { UserName = "John Doe", AccountNumber = "5678", PIN = "5678", Balance = 2000.0, Currency = "GBP", IsLogedIn = false },
                new { UserName = "Jane Doe", AccountNumber = "4321", PIN = "4321", Balance = 353563.53, Currency = "NGN", IsLogedIn = false }
            };

            foreach (var user in users)
            {
                insertCmd.Parameters.Clear();
                insertCmd.Parameters.AddWithValue("@AccountNumber", user.AccountNumber);
                insertCmd.Parameters.AddWithValue("@UserName", user.UserName);
                insertCmd.Parameters.AddWithValue("@Pin", user.PIN);
                insertCmd.Parameters.AddWithValue("@Balance", user.Balance);
                insertCmd.Parameters.AddWithValue("@Currency", user.Currency);
                insertCmd.Parameters.AddWithValue("@IsLogedIn", user.IsLogedIn);
                insertCmd.ExecuteNonQuery();
            }
        }
    }
}
