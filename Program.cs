using System;
using ATM.Models;
using ATM.Service;

namespace ATM {
    class Program {
        static void Main(string[] args) {

            bool continueAtm = true;

            while (continueAtm)
            {
                Console.WriteLine("\n====== ATM MENU ======");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Check Balance");
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Withdraw");
                Console.WriteLine("5. Send Money");
                Console.WriteLine("6. View Transactions");
                Console.WriteLine("7. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Console.WriteLine("Check Balance Service Call ");
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

        public static void Login() {
            var authService = new AuthService();

            Console.Write("\n ====== LOGIN ======");
            Console.Write("Account Number: ");
            string accountNumber = Console.ReadLine();
            Console.Write("Account PIN: ");
            string pin = Console.ReadLine();

            User currentUser = authService.Login(accountNumber, pin);

            if (currentUser != null) {
                Console.WriteLine($"Welcome, {currentUser.userName}!");
            } else {
                Console.WriteLine("Invalid credentials.");
            }
        }
    }
}