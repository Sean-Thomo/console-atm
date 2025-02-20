namespace ATM.Models{
    public class User(string username, string accountNumber, string pin, decimal balance, string currency, bool isLogedIn)
    {
        public string UserName { get; set; } = username;
        public string AccountNumber { get; set; } = accountNumber;
        public string PIN { get; set; } = pin;
        public decimal Balance { get; set; } = balance;
        public string Currency { get; set; } = currency;
        public bool IsLogedIn { get; set; } = isLogedIn;
    }
}