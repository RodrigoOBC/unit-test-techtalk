namespace ModelsBank;


using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SimulatorBankUnitTest.ModelsBank.DBConnect;

public class Account
{
    public Guid Number { get; private set; }
    public Client Holder { get; private set; }

    public string baseUrl { get; private set; }

    public decimal Balance { get; private set; }

    private Conector Conector { get; set; }


    public Account(Client holder, decimal balance)
    {
        Number = Guid.NewGuid();
        Holder = holder ?? throw new ArgumentNullException(nameof(holder));
        baseUrl = "https://fd757376-23b1-4fa3-a13f-93fa8cf11485.mock.pstmn.io";
        Balance =  0;
        Conector = new Conector("Data Source=DB/myBank.db"); 
    }

    public bool SaveaccountInDB()
    {
        try
        {
            Conector.AddAccount(Number.ToString(), Holder.Id.ToString(), Balance);
            return true;
        }
        catch (System.Exception)
        {
            throw new ArgumentException("Error to save account");
        }
    }

    public async Task<decimal> Deposit(decimal value)
    {
        var Balance = await GetBalance();

        if (value <= 0 || value == 0)
            throw new ArgumentException("The deposit value must be positive.");

        Balance += value;

        return Balance;

    }

    public async Task<decimal> Withdraw(decimal value)
    {
        var Balance = await GetBalance();

        if (value <= 0 || value == 0)
            throw new ArgumentException("The withdrawal value must be positive.");

        if (value > Balance)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= value;

        return Balance;
    }

    public async Task setInitalBalance()
    {
        using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var response = await client.GetAsync("/Balance?NumberIdentify=12345678900");
                var content = await response.Content.ReadAsStringAsync();
                var balanceResponse = JsonSerializer.Deserialize<BalanceResponse>(content);
                Balance = balanceResponse.Balance;
                Conector.UpdateAccount(Number.ToString(), Balance);

            }
            
    }

    public async Task<decimal> GetBalance(){
        return Balance;
    }

    public async Task<bool> setBalance(decimal value)
    {
        Balance = value;
        Conector.UpdateAccount(Number.ToString(), Balance);
        return true;
    }

    private class BalanceResponse
        {
            public decimal Balance { get; set; }
        }
    

}
