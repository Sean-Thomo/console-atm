using System;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class TransferService
    {
        private readonly string _usersFilePath;
        public TransferService() {
            _usersFilePath = Path.Combine("Data", "Users.json");
            Directory.CreateDirectory("Data");

            if (!File.Exists(_usersFilePath))
            {
                File.WriteAllText(_usersFilePath, "[]");
            }
        }

        private List<User> ReadUsers() {
            try
            {
                string json = File.ReadAllText(_usersFilePath);
                return JsonConvert.DeserializeObject<List<User>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading users: {ex.Message}");
                return [];
            }
        }

        private void WriteUsers(List<User> users)
        {
            try
            {
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(_usersFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing users: {ex.Message}");
            }
        }

        public void Transfer(string currentUserAccountNumber, string receivingAccountNumber, double amount) 
        {
            List<User> users = ReadUsers();
            User? sendingAccount = users.Find(u => u.AccountNumber == currentUserAccountNumber);
            User? receiveAccount = users.Find(u => u.AccountNumber == receivingAccountNumber);

            if (sendingAccount == null)
            {
                Console.WriteLine("Invalid sending account number.");
                return;
            }
            
            double senderBalance = sendingAccount.Balance - amount;

            if (senderBalance > 0 && receiveAccount != null)
            {
                Console.WriteLine("\n****** Transferring Money ******");
                sendingAccount.Balance = senderBalance;
                receiveAccount.Balance += amount;
                WriteUsers(users);

                Console.WriteLine("\n====== UPDATED BALANCE ======");
                Console.WriteLine($"\nR {senderBalance}");
            } else {
                Console.WriteLine("Insufficient funds or invalid account number.");
            }
        }

    }
}