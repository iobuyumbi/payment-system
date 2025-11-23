//using Solidaridad.Core.Common;

//namespace Solidaridad.Core.Entities.Wallets;

//public class Wallet : BaseEntity, IAuditedEntity
//{
//    // Properties
//    public decimal Balance { get; private set; }

//    public Guid OwnerId { get; set; }

//    public List<Transaction> Transactions { get; private set; }

//    public Guid CreatedBy { get; set; }

//    public DateTime CreatedOn { get; set; }

//    public Guid? UpdatedBy { get; set; }

//    public DateTime? UpdatedOn { get; set; }

//    // Constructor
//    public Wallet(Guid ownerId)
//    {
//        OwnerId = ownerId;
//        Balance = 0.0m;
//        Transactions = new List<Transaction>();
//    }

//    // Methods
//    //public void AddFunds(decimal amount, string description)
//    //{
//    //    if (amount <= 0)
//    //    {
//    //        throw new ArgumentException("Amount to add should be greater than zero.");
//    //    }
//    //    Balance += amount;
//    //    var transaction = new Transaction(Transactions.Count + 1, WalletID, amount, DateTime.Now, description, TransactionType.Credit);
//    //    Transactions.Add(transaction);
//    //    Console.WriteLine($"Added {amount:C} to wallet. New balance is {Balance:C}");
//    //}

//    //public void WithdrawFunds(decimal amount, string description)
//    //{
//    //    if (amount <= 0)
//    //    {
//    //        throw new ArgumentException("Amount to withdraw should be greater than zero.");
//    //    }
//    //    if (amount > Balance)
//    //    {
//    //        throw new InvalidOperationException("Insufficient balance.");
//    //    }
//    //    Balance -= amount;
//    //    var transaction = new Transaction(Transactions.Count + 1, WalletID, amount, DateTime.Now, description, TransactionType.Debit);
//    //    Transactions.Add(transaction);
//    //    Console.WriteLine($"Withdrew {amount:C} from wallet. New balance is {Balance:C}");
//    //}

//    public override string ToString()
//    {
//        return $"WalletID: {this.Id}, Owner: {OwnerId}, Balance: {Balance:C}, Number of Transactions: {Transactions.Count}";
//    }
//}

//public class Transaction : BaseEntity, IAuditedEntity
//{
//    // Properties
//    public Guid WalletId { get; set; }

//    public decimal Amount { get; set; }

//    public DateTime TransactionDate { get; set; }

//    public string Description { get; set; }

//    public TransactionType Type { get; set; }

//    public Guid CreatedBy { get; set; }

//    public DateTime CreatedOn { get; set; }

//    public Guid? UpdatedBy { get; set; }

//    public DateTime? UpdatedOn { get; set; }

//    // Constructor
//    public Transaction(decimal amount, DateTime transactionDate, string description, TransactionType type)
//    {
//        Amount = amount;
//        TransactionDate = transactionDate;
//        Description = description;
//        Type = type;
//    }

//    public override string ToString()
//    {
//        return $"TransactionID: {this.Id}, WalletID: {this.Id}, Amount: {Amount:C}, Date: {TransactionDate}, Description: {Description}, Type: {Type}";
//    }
//}

//public enum TransactionType
//{
//    Credit,
//    Debit
//}

//// Example usage
////class Program
////{
////    static void Main()
////    {
////        User user = new User(1, "Alice");
////        Wallet wallet = new Wallet(101, user);

////        wallet.AddFunds(100.00m, "Initial deposit");
////        wallet.WithdrawFunds(30.00m, "Grocery shopping");

////        Console.WriteLine(wallet.ToString());

////        foreach (var transaction in wallet.Transactions)
////        {
////            Console.WriteLine(transaction.ToString());
////        }
////    }
////}

