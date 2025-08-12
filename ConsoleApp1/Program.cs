using System;
using System.Diagnostics;
using System.Globalization;

public record Transaction (int Id, DateTime Date, decimal Amount, string Category)
{

}

interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Processing bank transfer for transaction : {transaction.Amount:C} in {transaction.Category} category");
    }
}

class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"Processing mobile money transfer for transaction : {transaction.Amount:C} in {transaction.Category} category");
    }
}

class CryptoWalletProcessor : ITransactionProcessor
{
       public void Process(Transaction transaction)
    {
        Console.WriteLine($"Processing crypto transfer for transaction : {transaction.Amount:C} in {transaction.Category} category");
    }   
}


class Account(string accountNumber, decimal balance)
{
    public string AccountNumber { get; } = accountNumber;
    public decimal Balance { get; protected set; } = balance;

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. New balance: {Balance:N}");
        }
    }
}

class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        
        var account = new SavingsAccount("Account 1", 1000m);

        var t1 = new Transaction(1, DateTime.Now, 10m, "Groceries");
        var t2 = new Transaction(2, DateTime.Now, 10m, "Utilities");
        var t3 = new Transaction(3, DateTime.Now, 10m, "Entertainment");

       
        ITransactionProcessor mobileMoney = new MobileMoneyProcessor();
        ITransactionProcessor bankTransfer = new BankTransferProcessor();
        ITransactionProcessor cryptoWallet = new CryptoWalletProcessor();

        Console.WriteLine($"Total balance: " + account.Balance);

        mobileMoney.Process(t1);
        account.ApplyTransaction(t1);

        bankTransfer.Process(t2);
        account.ApplyTransaction(t2);

        cryptoWallet.Process(t3);
        account.ApplyTransaction(t3);


        _transactions.Add(t1);
        _transactions.Add(t2);
        _transactions.Add(t3);
    }
}

public class Program
{
       public static void Main()
    {
        var app = new FinanceApp();
        app.Run();
        
    }   
}