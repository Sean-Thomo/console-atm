using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ATM.Services
{
    public class FileService<T>
    {
        private readonly string _usersFilePath;

        public FileService(string filePath) {
            _usersFilePath = filePath;
            Directory.CreateDirectory(Path.GetDirectoryName(_usersFilePath));

            if (!File.Exists(_usersFilePath))
            {
                File.WriteAllText(_usersFilePath, "[]");
            }
        }

        public List<T> ReadFromFile() {
            try
            {
                string json = File.ReadAllText(_usersFilePath);
                return JsonConvert.DeserializeObject<List<T>>(json) ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from file: {ex.Message}");
                return [];
            }
        }

        public void WriteToFile(List<T> users)
        {
            try
            {
                string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                File.WriteAllText(_usersFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}