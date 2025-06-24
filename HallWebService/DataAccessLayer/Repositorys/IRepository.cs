namespace HallWebService
{
    // ממשק כללי המגדיר פעולות CRUD (יצירה, קריאה, עדכון, מחיקה) עבור אובייקטים מסוג T.
    // כל מחלקה שתרצה לנהל אובייקטים מסוג T תצטרך לממש את הפונקציות הללו.
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(string id);
        string GetLastId();
        bool Create(T model);
        bool UpDate(T model);
        bool Delete(T model);
    }
}
