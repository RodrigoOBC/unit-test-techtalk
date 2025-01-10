using System;
using System.Data;
using System.Data.SQLite;

namespace SimulatorBankUnitTest.ModelsBank.DBConnect
{
    public interface IConector
    {
        void OpenConnection();
        IDbConnection GetConnection();
        void CloseConnection();
        void AddUser(string id, string name, string numberIdentify);
        void UpdateUser(string id, string name, string numberIdentify);
        void AddAccount(string id, string id_user, decimal balance);
        void UpdateAccount(string id, decimal balance);
    }

    public class Conector : IConector
    {
        private readonly IDbConnection _connection;

        public Conector(string connectionString)
        {
            _connection = new SQLiteConnection(connectionString);
        }

        public Conector(IDbConnection connection)
        {
            _connection = connection;
        }

        public void OpenConnection()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection.State != ConnectionState.Closed)
            {
                _connection.Close();
            }
        }

        public void AddUser(string id, string name, string numberIdentify)
        {
            ExecuteCommand(
                "INSERT INTO user (id, name, numberID) VALUES (@id, @name, @numberID)",
                cmd =>
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", id));
                    cmd.Parameters.Add(new SQLiteParameter("@name", name));
                    cmd.Parameters.Add(new SQLiteParameter("@numberID", numberIdentify));
                });
        }

        public void UpdateUser(string id, string name, string numberIdentify)
        {
            ExecuteCommand(
                "UPDATE user SET name = @name, numberID = @numberID WHERE id = @id",
                cmd =>
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", id));
                    cmd.Parameters.Add(new SQLiteParameter("@name", name));
                    cmd.Parameters.Add(new SQLiteParameter("@numberID", numberIdentify));
                });
        }

        public void AddAccount(string id, string id_user, decimal balance)
        {
            ExecuteCommand(
                "INSERT INTO account (id, id_user, saldo) VALUES (@id, @id_user, @saldo)",
                cmd =>
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", id));
                    cmd.Parameters.Add(new SQLiteParameter("@id_user", id_user));
                    cmd.Parameters.Add(new SQLiteParameter("@saldo", balance));
                });
        }

        public void UpdateAccount(string id, decimal balance)
        {
            ExecuteCommand(
                "UPDATE account SET saldo = @saldo WHERE id = @id",
                cmd =>
                {
                    cmd.Parameters.Add(new SQLiteParameter("@id", id));
                    cmd.Parameters.Add(new SQLiteParameter("@saldo", balance));
                });
        }

        private void ExecuteCommand(string query, Action<IDbCommand> configureCommand)
        {
            try
            {
                OpenConnection();

                using (IDbCommand command = _connection.CreateCommand())
                {
                    command.CommandText = query;
                    configureCommand(command);
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                throw; // Re-throw exceptions for proper handling in tests or higher layers
            }
            finally
            {
                CloseConnection();
            }
        }
    }
}