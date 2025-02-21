using System;
using System.Data.SQLite;
using ATM.Models;

namespace ATM.Services
{
    public class AuthService(SQLiteConnection sqliteConnection)
    {
        private readonly SQLiteConnection _sqliteConnection = sqliteConnection;

        public User? Login(string accountNumber, string pin){

            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber AND Pin = @Pin";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
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

            string query = "UPDATE Users SET IsLogedIn = 0 WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            cmd.ExecuteNonQuery();
        }
    }
}
