using System.Data;
using System.Data.OleDb;
using System.Transactions;

namespace HallWebService
{
    public class DBContext : IDBContext
    {
        OleDbCommand command;   
        OleDbConnection connection;
        OleDbTransaction transaction;

        private static DBContext dBContext;
        public static DBContext GetInstance()
        {
            if (dBContext == null)
                dBContext = new DBContext();
            return dBContext;
        }
        private bool ChangeDB(string sql)
        {
            this.command.CommandText = sql;
            bool ok =  this.command.ExecuteNonQuery() > 0;
            ClearParameter();
            return ok;
        }
        private DBContext()
        {
            this.connection = new OleDbConnection();
            this.connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\FinalProjectEvent\EventHall\HallWebService\App_Data\Database-90Minute1.accdb";
            this.command = new OleDbCommand();
            this.command = this.connection.CreateCommand();
        }

        public void CloseConection()
        {
           this.connection.Close();
        }

        public void Commit()
        {
            this.transaction.Commit();
        }

        public void beginTranzaction()
        {
            this.transaction = this.connection.BeginTransaction();
            this.command.Transaction = this.transaction;

        }

      
        public bool Delete(string sql)
        {
         
            return ChangeDB(sql);
        }

        public bool Insert(string sql)
        {
            return ChangeDB(sql);
        }

        public void OpenConection()
        {
            this.connection.Open();
        }

        public IDataReader Read(string sql)
        {
            this.command.CommandText = sql;
            IDataReader reader = this.command.ExecuteReader();
            ClearParameter();
            return reader;
        }

        public object ReadValue(string sql)
        {
            this.command.CommandText = sql;
            object obj = this.command.ExecuteScalar();
            ClearParameter();
            return obj;
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public bool UpDate(string sql)
        {
            return ChangeDB(sql);
        }

        public void AddParameter(string name, string value)
        {
            this.command.Parameters.Add(new OleDbParameter(name, value));
        }
        public void ClearParameter()
        {
            this.command.Parameters.Clear();
        }
    }
}
