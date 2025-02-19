namespace ATM.Models{
    public class User(string username, string accountNumber, string pin, double balance, string country)
    {
        public string userName { get; set; } = username;
        public string accountNumber { get; set; } = accountNumber;
        public string PIN { get; set; } = pin;
        public double balance { get; set; } = balance;
        public string country { get; set; } = country;
    }
}