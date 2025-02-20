using System;
using System.Collections.Generic;
using ATM.Models;

namespace ATM.Services
{
    public class AuthService(FileService<User> fileService)
    {
        private readonly FileService<User> _fileService = fileService;

        private List<User> ReadUsers() {
            return _fileService.ReadFromFile();
        }

        private void WriteUsers(List<User> users)
        {
            _fileService.WriteToFile(users);
        }

        public User Login(string accountNumber, string pin){
            List<User> users = ReadUsers();

            User? user = users.Find(u => u.AccountNumber == accountNumber && u.PIN == pin);

            if (user != null) 
            {
                user.IsLogedIn = true;
                WriteUsers(users);
            }
            return user;
        }

        public void Logout(string accountNumber) {
            List<User> users = ReadUsers();

            User? user = users.Find(u => u.AccountNumber == accountNumber);

            if (user != null)
            {
                user.IsLogedIn = false;
                WriteUsers(users);
            }
        }
    }
}
