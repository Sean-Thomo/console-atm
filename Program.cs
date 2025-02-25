﻿using System;
using System.Runtime.CompilerServices;
using ATM.Models;
using ATM.Services;
using ATM.Utils;
using ATM.Data;
using System.Data.SQLite;


namespace ATM {
    class Program {

        private static User? currentUser;
        private static bool isLogedIn;

        static async Task Main(string[] args) {

            DatabaseInitializer.InitializeDatabase();
            using SQLiteConnection sqlite_conn = new("Data Source=ATM.db;Version=3;");
            sqlite_conn.Open();
            bool continueAtm = true;

            var authService = new AuthService(sqlite_conn);
            var transferService = new TransferService(sqlite_conn);
            var accountService = new AccountService(sqlite_conn);

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

                string choice = Console.ReadLine()!;

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
                        await Transfer(transferService);
                        break;
                    case "6":
                        ViewTransActions(accountService);
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

        private static void ViewTransActions(AccountService accountService)
        {
            if (isLogedIn && currentUser != null)
            {
                accountService.GetTransactions(currentUser.AccountNumber);
            }
        }

        public static void Login(AuthService authService) {

            Console.Write("\n====== LOGIN ======\n");
            Console.Write("\nAccount Number: ");
            string accountNumber = Console.ReadLine()!.Trim()!;
            Console.Write("Account PIN: ");
            string pin = Console.ReadLine()!.Trim();

            currentUser = authService.Login(accountNumber, pin)!;

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
            decimal amount = Math.Round(Convert.ToDecimal(Console.ReadLine().Trim()), 2);

            if (currentUser != null)
            {
                accountService.Deposit(currentUser.AccountNumber, amount);
            }
            else
            {
                Console.WriteLine("User is not logged in.");
            }
        }

        public static void WithDraw(AccountService accountService) {

            Console.WriteLine("\n====== WITHDRAW ======\n");
            Console.Write("Amount: ");
            decimal amount = Math.Round(Convert.ToDecimal(Console.ReadLine().Trim()), 2);

            accountService.Withdraw(currentUser.AccountNumber, amount);
        }

        public static async Task Transfer(TransferService transferService) {

            Console.WriteLine("\n====== TRANSFER ======\n");
            Console.Write("Account Number: ");
            string receivingAccountNumber = Console.ReadLine().Trim();
            Console.Write("Amount: ");
            decimal amount = Math.Round(Convert.ToDecimal(Console.ReadLine().Trim()), 2);

            User receivingUser = transferService.GetUserByAccountNumber(receivingAccountNumber);

            decimal rate = await CurrencyConverter.Convert(currentUser.Currency, receivingUser.Currency);

            transferService.Transfer(currentUser.AccountNumber, receivingAccountNumber, amount, rate);
        }
    }
}