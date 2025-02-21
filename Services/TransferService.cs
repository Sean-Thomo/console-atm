using System;
using System.Data.SQLite;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class TransferService(SQLiteConnection sqliteConnection)
    {
        private readonly SQLiteConnection _sqliteConnection = sqliteConnection;

        public void Transfer(string accountNumber, string recipientAccountNumber, decimal amount, decimal rate) 
        {
            // SENDER QUERY
            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            using SQLiteDataReader senderReader = cmd.ExecuteReader();

            // RECEPIENT QUERY
            string recipientQuery = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand recipientCmd = new(recipientQuery, _sqliteConnection);
            recipientCmd.Parameters.AddWithValue("@AccountNumber", recipientAccountNumber);
            using SQLiteDataReader recipientReader = recipientCmd.ExecuteReader();

            if (senderReader.Read() && recipientReader.Read())
            {
                decimal senderBalance = Math.Round(Convert.ToDecimal(senderReader["Balance"]) - amount, 2);
                decimal recipientBalance = Math.Round(Convert.ToDecimal(recipientReader["Balance"]) + amount * rate, 2);

                Console.WriteLine($"\nSENDER:    R {senderBalance}");
                Console.WriteLine($"\nRECIPIENT: R {recipientBalance}");

                if (senderBalance > 0)
                {
                    string updateSenderBalanceQuery = "UPDATE Users SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    using SQLiteCommand updateSenderBalanceCmd = new(updateSenderBalanceQuery, _sqliteConnection);
                    updateSenderBalanceCmd.Parameters.AddWithValue("@Balance", senderBalance);
                    updateSenderBalanceCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    updateSenderBalanceCmd.ExecuteNonQuery();

                    string updateRecipientBalanceQuery = "UPDATE Users SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    using SQLiteCommand updateRecipientBalanceCmd = new(updateRecipientBalanceQuery, _sqliteConnection);
                    updateRecipientBalanceCmd.Parameters.AddWithValue("@Balance", recipientBalance);
                    updateRecipientBalanceCmd.Parameters.AddWithValue("@AccountNumber", recipientAccountNumber);
                    updateRecipientBalanceCmd.ExecuteNonQuery();

                    Console.WriteLine("\n****** Transferring Money ******");
                    Console.WriteLine("\n====== UPDATED BALANCE ======");
                    Console.WriteLine($"\nR {senderBalance}");
                } else {
                    Console.WriteLine("Insufficient funds.");
                }
            } else {
                Console.WriteLine("Invalid account number.");
            }
        }

        public User GetUserByAccountNumber(string accountNumber)
        {
            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

            using SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User(
                    reader["AccountNumber"]?.ToString() ?? string.Empty,
                    reader["UserName"]?.ToString() ?? string.Empty,
                    reader["Pin"]?.ToString() ?? string.Empty,
                    Math.Round(Convert.ToDecimal(reader["Balance"]), 2),
                    reader["Currency"]?.ToString() ?? string.Empty,
                    true
                );
            }
            return null;
        }

    }
}