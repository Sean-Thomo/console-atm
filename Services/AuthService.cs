using System;
using System.IO;
using System.Collections.Generic;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class AuthService
    {
        private readonly string _usersFilePath;
        public AuthService() {
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

        public User Login(string accountNumber, string pin){
            List<User> users = ReadUsers();

            User user = users.Find(u => u.AccountNumber == accountNumber && u.PIN == pin);

            if (user != null) 
            {
                user.IsLogedIn = true;
                WriteUsers(users);
            }
            return user;
        }

        public void Logout(string accountNumber) {
            List<User> users = ReadUsers();

            User user = users.Find(u => u.AccountNumber == accountNumber);

            if (user != null)
            {
                user.IsLogedIn = false;
                WriteUsers(users);
            }
        }
    }
}
