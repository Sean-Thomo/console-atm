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
    }
}