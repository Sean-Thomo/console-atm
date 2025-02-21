using System;
using System.Data.SQLite;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class AccountService(SQLiteConnection sqliteConnection)
    {

        private readonly SQLiteConnection _sqliteConnection = sqliteConnection;

        public void Deposit(string accountNumber, decimal amount) 
        {
            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

            using SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                decimal newBalance = Math.Round(Convert.ToDecimal(reader["Balance"]) + amount, 2);
                string updateBalanceQuery = "UPDATE Users SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                using SQLiteCommand updateBalanceCmd = new(updateBalanceQuery, _sqliteConnection);
                updateBalanceCmd.Parameters.AddWithValue("@Balance", newBalance);
                updateBalanceCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                updateBalanceCmd.ExecuteNonQuery();

                Console.WriteLine("\n****** Depositing Money ******");
                Console.WriteLine("\n====== DEPOSIT SUCCESSFUL ======");
                Console.WriteLine($"\nR {amount} deposited successfully.");
                Console.WriteLine($"\n====== UPDATED BALANCE ======");
                Console.WriteLine($"\nR {newBalance}");
            } else {
                Console.WriteLine("Invalid account number.");
            }
        }

        public void Withdraw(string accountNumber, decimal amount) 
        {
            string query = "SELECT * FROM Users WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

            using SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                decimal newBalance = Math.Round(Convert.ToDecimal(reader["Balance"]) - amount, 2);
                if (newBalance >= 0)
                {
                    string updateBalanceQuery = "UPDATE Users SET Balance = @Balance WHERE AccountNumber = @AccountNumber";
                    using SQLiteCommand updateBalanceCmd = new(updateBalanceQuery, _sqliteConnection);
                    updateBalanceCmd.Parameters.AddWithValue("@Balance", newBalance);
                    updateBalanceCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    updateBalanceCmd.ExecuteNonQuery();

                    Console.WriteLine("\n****** Withdrawing Money ******");
                    Console.WriteLine("\n====== WITHDRAW SUCCESSFUL ======");
                    Console.WriteLine($"\nR {amount} withdrawn successfully.");
                    Console.WriteLine($"\n====== UPDATED BALANCE ======");
                    Console.WriteLine($"\nR {newBalance}");
                } else {
                    Console.WriteLine("Insufficient funds.");
                }
            }
        }

        public void GetTransactions(string accountNumber)
        {
            string query = "SELECT * FROM Transactions WHERE AccountNumber = @AccountNumber";
            using SQLiteCommand cmd = new(query, _sqliteConnection);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

            using SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Console.WriteLine("\n====== TRANSACTIONS ======\n");
                while (reader.Read())
                {
                    Console.WriteLine($"Transaction ID    : {Convert.ToInt32(reader["TransactionId"])}");
                    Console.WriteLine($"Account Number    : {reader["AccountNumber"]?.ToString() ?? string.Empty}");
                    Console.WriteLine($"Recipient Account : {reader["RecipientAccount"]?.ToString() ?? string.Empty}");
                    Console.WriteLine($"Amount            : {Math.Round(Convert.ToDecimal(reader["Amount"]), 2)}");
                    Console.WriteLine($"Transaction Type  : {reader["TransactionType"]?.ToString() ?? string.Empty}");
                    Console.WriteLine($"Timestamp         : {reader["Timestamp"]?.ToString() ?? string.Empty}");
                    Console.WriteLine("--------------------------------------------------------------");
                }
            } else {
                Console.WriteLine("No transactions found.");
            }
        }
    }
}