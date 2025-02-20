using System;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class TransferService(FileService<User> fileService)
    {
        private readonly FileService<User> _fileService = fileService;

        private List<User> ReadUsers() {
            return _fileService.ReadFromFile();
        }

        private void WriteUsers(List<User> users)
        {
            _fileService.WriteToFile(users);
        }

        public void Transfer(string currentUserAccountNumber, string receivingAccountNumber, decimal amount, decimal rate) 
        {
            List<User> users = ReadUsers();
            User? sendingAccount = users.Find(u => u.AccountNumber == currentUserAccountNumber);
            User? receiveAccount = users.Find(u => u.AccountNumber == receivingAccountNumber);

            if (sendingAccount == null)
            {
                Console.WriteLine("Invalid sending account number.");
                return;
            }

            decimal senderBalance = sendingAccount.Balance - amount;

            if (senderBalance > 0 && receiveAccount != null)
            {
                Console.WriteLine("\n****** Transferring Money ******");
                sendingAccount.Balance = senderBalance;
                receiveAccount.Balance += amount * rate;
                WriteUsers(users);

                Console.WriteLine("\n====== UPDATED BALANCE ======");
                Console.WriteLine($"\nR {senderBalance}");
            } else {
                Console.WriteLine("Insufficient funds or invalid account number.");
            }
        }

        public User GetUserByAccountNumber(string accountNumber)
        {
            List<User> users = ReadUsers();
            User? user = users.Find(u => u.AccountNumber == accountNumber);

            if (user != null)
            {
                return user;
            } else {
                return null;
            }
        }

    }
}