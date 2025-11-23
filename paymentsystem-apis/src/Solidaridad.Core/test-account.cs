using System;
using System.Collections.Generic;

namespace AccountManagementSystem
{
    public class Customer
    {
        // Properties
        public Guid CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Constructor
        public Customer(string name, string email, string phoneNumber)
        {
            CustomerID = Guid.NewGuid();
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        // Methods
        public override string ToString()
        {
            return $"CustomerID: {CustomerID}, Name: {Name}, Email: {Email}, PhoneNumber: {PhoneNumber}";
        }
    }

    public class Account
    {
        // Properties
        public Guid AccountID { get; set; }
        public Customer Owner { get; set; }
        public decimal Balance { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        // Constructor
        public Account(Customer owner)
        {
            AccountID = Guid.NewGuid();
            Owner = owner;
            Balance = 0.0m;
            Transactions = new List<Transaction>();
        }

        // Methods
        public void Deposit(decimal amount, string description)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to deposit should be greater than zero.");
            }
            Balance += amount;
            var transaction = new Transaction(AccountID, amount, DateTime.Now, description, TransactionType.Credit);
            Transactions.Add(transaction);
        }

        public void Withdraw(decimal amount, string description)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount to withdraw should be greater than zero.");
            }
            if (amount > Balance)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }
            Balance -= amount;
            var transaction = new Transaction(AccountID, amount, DateTime.Now, description, TransactionType.Debit);
            Transactions.Add(transaction);
        }

        public override string ToString()
        {
            return $"AccountID: {AccountID}, Owner: {Owner.Name}, Balance: {Balance:C}, Transactions: {Transactions.Count}";
        }
    }

    public class Transaction
    {
        // Properties
        public Guid TransactionID { get; set; }
        public Guid AccountID { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public TransactionType Type { get; set; }

        // Constructor
        public Transaction(Guid accountID, decimal amount, DateTime transactionDate, string description, TransactionType type)
        {
            TransactionID = Guid.NewGuid();
            AccountID = accountID;
            Amount = amount;
            TransactionDate = transactionDate;
            Description = description;
            Type = type;
        }

        // Methods
        public override string ToString()
        {
            return $"TransactionID: {TransactionID}, AccountID: {AccountID}, Amount: {Amount:C}, Date: {TransactionDate}, Description: {Description}, Type: {Type}";
        }
    }

    public enum TransactionType
    {
        Credit,
        Debit
    }

    public class AccountManagementSystem
    {
        // Properties
        private List<Customer> Customers { get; set; }
        private List<Account> Accounts { get; set; }

        // Constructor
        public AccountManagementSystem()
        {
            Customers = new List<Customer>();
            Accounts = new List<Account>();
        }

        // Methods
        public Customer CreateCustomer(string name, string email, string phoneNumber)
        {
            var customer = new Customer(name, email, phoneNumber);
            Customers.Add(customer);
            return customer;
        }

        public Account CreateAccount(Customer customer)
        {
            var account = new Account(customer);
            Accounts.Add(account);
            return account;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            return Customers.Find(c => c.CustomerID == customerId);
        }

        public Account GetAccountById(Guid accountId)
        {
            return Accounts.Find(a => a.AccountID == accountId);
        }

        public void DepositToAccount(Guid accountId, decimal amount, string description)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                account.Deposit(amount, description);
            }
            else
            {
                throw new ArgumentException("Account not found.");
            }
        }

        public void WithdrawFromAccount(Guid accountId, decimal amount, string description)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                account.Withdraw(amount, description);
            }
            else
            {
                throw new ArgumentException("Account not found.");
            }
        }

        // Method to display customer and account details for testing purposes
        public void DisplayDetails()
        {
            Console.WriteLine("Customers:");
            foreach (var customer in Customers)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\nAccounts:");
            foreach (var account in Accounts)
            {
                Console.WriteLine(account);
            }
        }
    }

    // Example usage
    class Program
    {
        static void Main()
        {
            var system = new AccountManagementSystem();

            // Create a customer
            var customer = system.CreateCustomer("Alice", "alice@example.com", "123-456-7890");

            // Create an account for the customer
            var account = system.CreateAccount(customer);

            // Perform some transactions
            system.DepositToAccount(account.AccountID, 500.00m, "Initial deposit");
            system.WithdrawFromAccount(account.AccountID, 100.00m, "Grocery shopping");

            // Display details
            system.DisplayDetails();
        }
    }
}
