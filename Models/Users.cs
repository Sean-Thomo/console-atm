namespace ATM.Models{
    public class User(string username, string accountNumber, string pin, double balance, string country, bool isLogedIn)
    {
        public string UserName { get; set; } = username;
        public string AccountNumber { get; set; } = accountNumber;
        public string PIN { get; set; } = pin;
        public double Balance { get; set; } = balance;
        public string Country { get; set; } = country;
        public bool IsLogedIn { get; set; } = isLogedIn;
    }
}