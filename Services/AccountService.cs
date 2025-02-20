using System;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class AccountService(FileService<User> fileService)
    {

        private readonly FileService<User> _fileService = fileService;

        private List<User> ReadUsers() {
            return _fileService.ReadFromFile();
        }

        private void WriteUsers(List<User> users)
        {
            _fileService.WriteToFile(users);
        }

        public void Deposit(string accountNumber, decimal amount) 
        {
            List<User> users = ReadUsers();
            User? user = users.Find(u => u.AccountNumber == accountNumber);
            
            if (user != null)
            {
                user.Balance += amount;
                WriteUsers(users);
                Console.WriteLine("\n====== DEPOSIT SUCCESSFUL ======");
                Console.WriteLine($"\nR {amount} deposited successfully.");
                Console.WriteLine($"\n====== UPDATED BALANCE ======");
                Console.WriteLine($"\nR {user.Balance}");
            } else {
                Console.WriteLine("Invalid account number.");
            }
        }

        public void Withdraw(string accountNumber, decimal amount) 
        {
            List<User> users = ReadUsers();
            User? user = users.Find(u => u.AccountNumber == accountNumber);
            
            if (user != null)
            {
                decimal newBalance = user.Balance - amount;
                if (newBalance >= 0)
                {
                    user.Balance = newBalance;
                    WriteUsers(users);
                    Console.WriteLine("\n====== WITHDRAWAL SUCCESSFUL ======");
                    Console.WriteLine($"\nR {amount} withdrawn successfully.");
                    Console.WriteLine($"\n====== UPDATED BALANCE ======");
                    Console.WriteLine($"\nR {user.Balance}");
                } else {
                    Console.WriteLine("Insufficient funds.");
                }
            } else {
                Console.WriteLine("Invalid account number.");
            }
        }
    }
}