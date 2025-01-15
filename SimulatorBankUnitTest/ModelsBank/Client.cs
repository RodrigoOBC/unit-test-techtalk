namespace ModelsBank;

using System;
using SimulatorBankUnitTest.ModelsBank.DBConnect;


public interface IClient
    {
        

        bool Save();
        bool upadate();
        void ChangeName(string name);
        void ChangeCPF(string numberIdentify);

        void setConector(Conector conector);

        Guid Id { get; }

        string NumberIdentify { get; }

        string Name { get; }
    }

public class Client : IClient
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NumberIdentify { get; private set; }
    private Conector Conector { get; set; }

    public Client(string name, string numberIdentify)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        NumberIdentify = numberIdentify ?? throw new ArgumentNullException(nameof(numberIdentify));
        Conector = new Conector("Data Source=DB/myBank.db"); 
    }
    
    public void setConector(Conector conector)
    {
        Conector = conector;
    }

    public bool Save()
    {   
        try
        {
            Conector.AddUser(Id.ToString(), Name, NumberIdentify);
            return true;
        }
        catch (System.Exception ex )
        {
            if (ex.Message.Contains("UNIQUE constraint failed: user.numberID"))
                throw new ArgumentException("Error to save user: User already exists");
            throw new ArgumentException("Error to save user");
        }
        
    }

    public bool upadate()
    {
        try
        {
            Conector.UpdateUser(Id.ToString(), Name, NumberIdentify);
            return true;
        }
        catch (System.Exception)
        {
            
            throw new ArgumentException("Error to update user");
        }
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new Exception("Name is required");
        if (name.Length < 3 || name.Length > 100)
            throw new Exception("Name must be between 3 and 100 characters");

        Name = name;
        
    }

    public void ChangeCPF(string numberIdentify)
    {
        if (string.IsNullOrEmpty(numberIdentify))
            throw new Exception("Name is required");

        NumberIdentify = numberIdentify;
    }
}
