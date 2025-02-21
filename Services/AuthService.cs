using System;
using System.Data.SQLite;
using ATM.Models;

namespace ATM.Services
{
    public class AuthService()
    {
        private readonly string connectionString = "Data Source=ATM.db;Version=3;";


        public User? Login(string accountNumber, string pin){

            using SQLiteConnection sqlite_conn = new(connectionString);
            sqlite_conn.Open();

            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber AND Pin = @Pin";
            using SQLiteCommand cmd = new(query, sqlite_conn);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            cmd.Parameters.AddWithValue("@Pin", pin);

            using SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User(
                    reader["AccountNumber"]?.ToString() ?? string.Empty,
                    reader["UserName"]?.ToString() ?? string.Empty,
                    reader["Pin"]?.ToString() ?? string.Empty,
                    Convert.ToDecimal(reader["Balance"]),
                    reader["Currency"]?.ToString() ?? string.Empty,
                    true
                );
            }
            return null;
        }

        public void Logout(string accountNumber) {
            using SQLiteConnection sqlite_conn = new(connectionString);
            sqlite_conn.Open();

            string query = "UPDATE Users SET IsLogedIn = 0 WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, sqlite_conn);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            cmd.ExecuteNonQuery();
        }
    }
}
