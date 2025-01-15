using SimulatorBankUnitTest.ModelsBank.DBConnect;
namespace ModelsBank;

public class Bank
{
    private List<Account> _accounts = new List<Account>();

    public Conector conector { get; set; }

    public Account CreateAccount(IClient client, decimal balance)
    {
        try
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            client.Save();
            var account = new Account(client as Client, balance, conector);
            _accounts.Add(account);
            account.SaveaccountInDB();
            return account;
        }
        catch (System.Exception)
        {
            throw new ArgumentException("Error to save account");
        }

    }

    public List<Account> GetAccounts()
    {
        return _accounts;
    }

    public void SetAccounts(Account account)

    {

        _accounts.Add(account);

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
        await account.upadteAccountBalance();
        return true;
    }

    public async Task<bool> Withdraw(Guid accountNumber, decimal amount)
    {
        var account = FindAccount(accountNumber);
        var value = await account.Withdraw(amount);
        await account.setBalance(value);
        await account.upadteAccountBalance();
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
