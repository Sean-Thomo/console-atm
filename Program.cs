using System;
using System.Runtime.CompilerServices;
using ATM.Models;
using ATM.Services;
using ATM.Utils;
using ATM.Data;


namespace ATM {
    class Program {

        private static User currentUser;
        private static bool isLogedIn;

        static void Main(string[] args) {

            DatabaseInitializer.InitializeDatabase();
            bool continueAtm = true;

            var fileService = new FileService<User>(Path.Combine("Data", "Users.json"));
            var authService = new AuthService();
            var transferService = new TransferService(fileService);
            var accountService = new AccountService(fileService);

            while (continueAtm)
            {
                Console.WriteLine("\n====== ATM MENU ======\n");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Check Balance");
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Withdraw");
                Console.WriteLine("5. Send Money");
                Console.WriteLine("6. View Transactions");
                Console.WriteLine("7. Exit");
                Console.Write("\nChoose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login(authService);
                        break;
                    case "2":
                        CheckBalance();
                        break;
                    case "3":
                        Deposit(accountService);
                        break;
                    case "4":
                        WithDraw(accountService);
                        break;
                    case "5":
                        Transfer(transferService);
                        break;
                    case "6":
                        Console.WriteLine("View Transactions Service Call");
                        break;
                    case "7":
                        continueAtm = false;
                        Logout(authService);
                        Console.WriteLine("Thank you for using the ATM. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Pleae try again.");
                        break;
                }
            }
        }

        public static void Login(AuthService authService) {

            Console.Write("\n====== LOGIN ======\n");
            Console.Write("\nAccount Number: ");
            string accountNumber = Console.ReadLine().Trim();
            Console.Write("Account PIN: ");
            string pin = Console.ReadLine().Trim();

            currentUser = authService.Login(accountNumber, pin);

            if (currentUser != null) {
                isLogedIn = true;
                Console.WriteLine($"\nWelcome, {currentUser.UserName}!");
            } else {
                Console.WriteLine("Invalid credentials.");
            }
        }

        public static void Logout(AuthService authService) {

            if (isLogedIn && currentUser != null)
            {
                authService.Logout(currentUser.AccountNumber);
                isLogedIn = false;
                currentUser = null;
            }
        }

        public static void CheckBalance() {

            if (isLogedIn && currentUser != null)
            {
                Console.WriteLine("\n====== BALANCE ======\n");
                Console.WriteLine($"{currentUser.Currency} {currentUser.Balance.ToString("C")}");
            }

        }

        public static void Deposit(AccountService accountService) {

            Console.WriteLine("\n====== DEPOSIT ======\n");
            Console.Write("Amount: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine().Trim());

            accountService.Deposit(currentUser.AccountNumber, amount);
        }

        public static void WithDraw(AccountService accountService) {

            Console.WriteLine("\n====== WITHDRAW ======\n");
            Console.Write("Amount: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine().Trim());

            accountService.Withdraw(currentUser.AccountNumber, amount);
        }

        public static async Task Transfer(TransferService transferService) {

            Console.WriteLine("\n====== TRANSFER ======\n");
            Console.Write("Account Number: ");
            string receivingAccountNumber = Console.ReadLine().Trim();
            Console.Write("Amount: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine().Trim());

            User receivingUser = transferService.GetUserByAccountNumber(receivingAccountNumber);

            decimal rate = await CurrencyConverter.Convert(currentUser.Currency, receivingUser.Currency);

            transferService.Transfer(currentUser.AccountNumber, receivingAccountNumber, amount, rate);
        }
    }
}