using Solidaridad.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solidaridad.Core.Entities.Payments;
[Table("Payments")]
public class Payment : BaseEntity, IAuditedEntity
{
    // Properties
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public string Status { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    // Constructor
    public Payment(decimal amount, DateTime paymentDate, PaymentMethod paymentMethod, string status)
    {
        Amount = amount;
        PaymentDate = paymentDate;
        PaymentMethod = paymentMethod;
        Status = status;
    }

    // Methods
    public void ProcessPayment()
    {
        // Implement payment processing logic here
        // This could involve calling an external payment gateway API
        Console.WriteLine("Processing payment...");
        Status = "Processed";
    }

    public override string ToString()
    {
        return $"Amount: {Amount:C}, PaymentDate: {PaymentDate}, PaymentMethod: {PaymentMethod.MethodName}, Status: {Status}";
    }
}

public class PaymentMethod : BaseEntity
{
    // Properties
    public string MethodName { get; set; }

    // Constructor
    public PaymentMethod(string methodName)
    {
        MethodName = methodName;
    }

    public override string ToString()
    {
        return $"MethodName: {MethodName}";
    }
}

public class Transaction : BaseEntity, IAuditedEntity
{
    // Properties
    public Payment Payment { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Status { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    // Constructor
    public Transaction(Payment payment, DateTime transactionDate, string status)
    {
        Payment = payment;
        TransactionDate = transactionDate;
        Status = status;
    }

    public void CompleteTransaction()
    {
        // Implement transaction completion logic here
        Payment.ProcessPayment();
        Status = "Completed";
        Console.WriteLine("Transaction completed.");
    }

    public override string ToString()
    {
        return $"Payment: [{Payment}], TransactionDate: {TransactionDate}, Status: {Status}";
    }
}

// Example usage
//class Program
//{
//    static void Main()
//    {
//        PaymentMethod creditCard = new PaymentMethod(1, "Credit Card");
//        Payment payment = new Payment(1001, 150.00m, DateTime.Now, creditCard, "Pending");
//        Transaction transaction = new Transaction(5001, payment, DateTime.Now, "Initiated");

//        Console.WriteLine(payment.ToString());
//        Console.WriteLine(transaction.ToString());

//        transaction.CompleteTransaction();

//        Console.WriteLine(payment.ToString());
//        Console.WriteLine(transaction.ToString());
//    }
//}

