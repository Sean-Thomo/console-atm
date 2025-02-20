using System;
using ATM.Models;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class AccountService
    {

        private readonly string _usersFilePath;

            public AccountService() {
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
    }
}