using System;
using System.Collections.Generic;

namespace LoanManagementSystem
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

    public class Loan
    {
        // Properties
        public Guid LoanID { get; set; }
        public Customer Borrower { get; set; }
        public decimal PrincipalAmount { get; set; }
        public decimal InterestRate { get; set; }
        public int TermInMonths { get; set; }
        public decimal Balance { get; private set; }
        public List<Payment> Payments { get; private set; }

        // Constructor
        public Loan(Customer borrower, decimal principalAmount, decimal interestRate, int termInMonths)
        {
            LoanID = Guid.NewGuid();
            Borrower = borrower;
            PrincipalAmount = principalAmount;
            InterestRate = interestRate;
            TermInMonths = termInMonths;
            Balance = principalAmount;
            Payments = new List<Payment>();
        }

        // Methods
        public void MakePayment(decimal amount, DateTime paymentDate)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Payment amount should be greater than zero.");
            }
            if (amount > Balance)
            {
                throw new InvalidOperationException("Payment amount exceeds loan balance.");
            }

            Balance -= amount;
            var payment = new Payment(LoanID, amount, paymentDate);
            Payments.Add(payment);
        }

        public decimal CalculateMonthlyInstallment()
        {
            // Using simple interest formula for demonstration. Replace with appropriate formula as needed.
            decimal monthlyRate = InterestRate / 12 / 100;
            int numberOfPayments = TermInMonths;
            decimal installment = PrincipalAmount * monthlyRate / (1 - (decimal)Math.Pow(1 + (double)monthlyRate, -numberOfPayments));
            return installment;
        }

        public override string ToString()
        {
            return $"LoanID: {LoanID}, Borrower: {Borrower.Name}, PrincipalAmount: {PrincipalAmount:C}, InterestRate: {InterestRate}%, Term: {TermInMonths} months, Balance: {Balance:C}";
        }
    }

    public class Payment
    {
        // Properties
        public Guid PaymentID { get; set; }
        public Guid LoanID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        // Constructor
        public Payment(Guid loanID, decimal amount, DateTime paymentDate)
        {
            PaymentID = Guid.NewGuid();
            LoanID = loanID;
            Amount = amount;
            PaymentDate = paymentDate;
        }

        public override string ToString()
        {
            return $"PaymentID: {PaymentID}, LoanID: {LoanID}, Amount: {Amount:C}, PaymentDate: {PaymentDate}";
        }
    }

    public class LoanManagementSystem
    {
        // Properties
        private List<Customer> Customers { get; set; }
        private List<Loan> Loans { get; set; }

        // Constructor
        public LoanManagementSystem()
        {
            Customers = new List<Customer>();
            Loans = new List<Loan>();
        }

        // Methods
        public Customer CreateCustomer(string name, string email, string phoneNumber)
        {
            var customer = new Customer(name, email, phoneNumber);
            Customers.Add(customer);
            return customer;
        }

        public Loan CreateLoan(Customer borrower, decimal principalAmount, decimal interestRate, int termInMonths)
        {
            var loan = new Loan(borrower, principalAmount, interestRate, termInMonths);
            Loans.Add(loan);
            return loan;
        }

        public Customer GetCustomerById(Guid customerId)
        {
            return Customers.Find(c => c.CustomerID == customerId);
        }

        public Loan GetLoanById(Guid loanId)
        {
            return Loans.Find(l => l.LoanID == loanId);
        }

        public void MakePayment(Guid loanId, decimal amount, DateTime paymentDate)
        {
            var loan = GetLoanById(loanId);
            if (loan != null)
            {
                loan.MakePayment(amount, paymentDate);
            }
            else
            {
                throw new ArgumentException("Loan not found.");
            }
        }

        // Method to display customer and loan details for testing purposes
        public void DisplayDetails()
        {
            Console.WriteLine("Customers:");
            foreach (var customer in Customers)
            {
                Console.WriteLine(customer);
            }
            Console.WriteLine("\nLoans:");
            foreach (var loan in Loans)
            {
                Console.WriteLine(loan);
            }
        }
    }

    // Example usage
    class Program
    {
        static void Main()
        {
            var system = new LoanManagementSystem();

            // Create a customer
            var customer = system.CreateCustomer("Alice", "alice@example.com", "123-456-7890");

            // Create a loan for the customer
            var loan = system.CreateLoan(customer, 10000.00m, 5.0m, 24);

            // Display loan details
            Console.WriteLine("Loan created:");
            Console.WriteLine(loan.ToString());

            // Make a payment towards the loan
            system.MakePayment(loan.LoanID, 500.00m, DateTime.Now);

            // Display updated loan details
            Console.WriteLine("\nUpdated loan details:");
            Console.WriteLine(loan.ToString());

            // Display all details
            system.DisplayDetails();
        }
    }
}
