namespace ATM.Models
{
    public class Transaction(int transactionId, string accountNumber, string recipientAccount, decimal amount, string transactionType, string timeStamp)
    {
        public int TransactionId { get; set; } = transactionId;
        public string AccountNumber { get; set; } = accountNumber;
        public string RecipientAccount { get; set; } = recipientAccount;
        public decimal Amount { get; set; } = amount;
        public string TransactionType { get; set; } = transactionType;
        public string TimeStamp { get; set; } = timeStamp;
    }
}