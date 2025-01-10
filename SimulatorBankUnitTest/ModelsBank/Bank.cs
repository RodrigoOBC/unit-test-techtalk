namespace ModelsBank;

public class Bank
{
    private readonly List<Account> _accounts = new List<Account>();

    public Account CreateAccount(Client client, decimal balance)
    {
        if (client == null)
            throw new ArgumentNullException(nameof(client));
        
        client.Save();
        var account = new Account(client,balance);
        _accounts.Add(account);
        account.SaveaccountInDB();
        return account;
    }

    public async Task<bool> setBonus(Guid accountNumber)
    {
        var account = FindAccount(accountNumber);
        await account.setInitalBalance();
        await account.setBalance(account.Balance);
        return true;
    }

    public Account FindAccount(Guid accountNumber)
    {
        return _accounts.FirstOrDefault(a => a.Number == accountNumber)
            ?? throw new InvalidOperationException("Account not found.");
    }

    public async Task<bool> Deposit(Guid accountNumber, decimal amount)
    {
        var account = FindAccount(accountNumber);
        var value = await account.Deposit(amount);
        await account.setBalance(value);
        return true;
    }

    public async Task<bool> Withdraw(Guid accountNumber, decimal amount)
    {
        var account = FindAccount(accountNumber);
        var value = await account.Withdraw(amount);
        await account.setBalance(value);
        return true;
    }

    public bool ListAccounts()
    {

        if (_accounts.Count == 0)
        {
            Console.WriteLine("No accounts found.");
            return false;
        }

        foreach (var account in _accounts)
        {
            Console.WriteLine(account.Number);
        }

        return true;
    }
}
