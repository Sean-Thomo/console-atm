using System;
using System.Data.SQLite;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class TransferService(SQLiteConnection sqliteConnection)
    {
        private readonly SQLiteConnection _sqliteConnection = sqliteConnection;

        public void Transfer(string currentUserAccountNumber, string receivingAccountNumber, decimal amount, decimal rate) 
        {
            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber AND RecipientAccount = @RecipientAccount";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", currentUserAccountNumber);
            cmd.Parameters.AddWithValue("@RecipientAccount", receivingAccountNumber);

            using SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                decimal senderBalance = Convert.ToDecimal(reader["Balance"]) - amount;
                decimal recipientBalance = Convert.ToDecimal(reader["RecipientBalance"]) + amount * rate;

                if (senderBalance > 0)
                {
                    string updateSenderBalanceQuery = "UPDATE Users SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    using SQLiteCommand updateSenderBalanceCmd = new(updateSenderBalanceQuery, _sqliteConnection);
                    updateSenderBalanceCmd.Parameters.AddWithValue("@Balance", senderBalance);
                    updateSenderBalanceCmd.Parameters.AddWithValue("@AccountNumber", currentUserAccountNumber);
                    updateSenderBalanceCmd.ExecuteNonQuery();

                    string updateRecipientBalanceQuery = "UPDATE Users SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    using SQLiteCommand updateRecipientBalanceCmd = new(updateRecipientBalanceQuery, _sqliteConnection);
                    updateRecipientBalanceCmd.Parameters.AddWithValue("@Balance", recipientBalance);
                    updateRecipientBalanceCmd.Parameters.AddWithValue("@AccountNumber", receivingAccountNumber);
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
                    Convert.ToDecimal(reader["Balance"]),
                    reader["Currency"]?.ToString() ?? string.Empty,
                    true
                );
            }
            return null;
        }

    }
}