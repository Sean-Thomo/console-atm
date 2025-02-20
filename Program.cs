using System;
using System.Runtime.CompilerServices;
using ATM.Models;
using ATM.Services;
using Microsoft.VisualBasic;

namespace ATM {
    class Program {

        private static User currentUser;
        private static bool isLogedIn;

        static void Main(string[] args) {

            bool continueAtm = true;

            var authService = new AuthService();

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
                        Console.WriteLine("Deposit Service Call");
                        break;
                    case "4":
                        Console.WriteLine("Withdraw Service Call");
                        break;
                    case "5":
                        Console.WriteLine("Send Money Service Call");
                        break;
                    case "6":
                        Console.WriteLine("View Transactions Service Call");
                        break;
                    case "7":
                        continueAtm = false;
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
            string accountNumber = Console.ReadLine();
            Console.Write("Account PIN: ");
            string pin = Console.ReadLine();

            currentUser = authService.Login(accountNumber, pin);

            if (currentUser != null) {
                isLogedIn = true;
                Console.WriteLine($"\nWelcome, {currentUser.UserName}!");
            } else {
                Console.WriteLine("Invalid credentials.");
            }
        }

        public static void CheckBalance() {

            if (isLogedIn && currentUser != null)
            {
                Console.WriteLine("\n====== BALANCE ======\n");
                Console.WriteLine($"R {currentUser.Balance}");
            }

        }
    }
}