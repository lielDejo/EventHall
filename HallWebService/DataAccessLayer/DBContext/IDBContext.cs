using System.Data;

namespace HallWebService
{
    public interface IDBContext
    {
        void OpenConection();
        void CloseConection();
        IDataReader Read(string sql);
        object ReadValue(string sql);
        bool UpDate(string sql);
        bool Insert(string sql);
        bool Delete(string sql);
        void Commit();
        void RollBack();

    }
}
