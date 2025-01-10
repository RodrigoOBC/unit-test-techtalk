using System;
using ModelsBank;

class CabralBankProgram
{
    static async Task Main(string[] args)
    {
        var bank = new Bank();

        // Create clients and accounts
        var client1 = new Client("Alice", "123.456.789-00");
        var client2 = new Client("Andreza", "987.654.321-00");

        var account1 = bank.CreateAccount(client1,0);
        var account2 = bank.CreateAccount(client2,0);

        // Perform operations
        await bank.setBonus(account1.Number);
        await bank.setBonus(account2.Number);
        await bank.Deposit(account1.Number, 500);
        await bank.Deposit(account2.Number, 500);
        await bank.Withdraw(account2.Number, 100);

      

        // List accounts
        Console.WriteLine(bank.ListAccounts());
    }
}