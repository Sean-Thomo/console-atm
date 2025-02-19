using System;
using System.IO;
using System.Collections.Generic;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Service
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

        public User Login(string accountNumber, string pin){
            List<User> users = ReadUsers();

            User user = users.Find(u => u.accountNumber == accountNumber && u.PIN == pin);

            if (user != null)
            {
                Console.WriteLine($"Login successful. Welcome, {user.userName}!");
            } else {
                Console.WriteLine("Invalid credentials.");
            }

            return user;
        }
    }
}
