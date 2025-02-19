namespace ATM.Models{
    public class User {
    private string userName { get; set; }
    private int accountNumber { get; set; }
    private string PIN { get; set; }
    private double balance { get; set; }
    private string country { get; set;}

    public User(string username, int accountNumber, string pin, double balance, string country) {
        userName = username;
        PIN = pin;
        balance = balance;
        country = country;
    }
}
}