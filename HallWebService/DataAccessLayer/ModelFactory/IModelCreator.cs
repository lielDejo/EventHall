using System.Data;
namespace HallWebService
{
    // ממשק זה מגדיר את ההתנהגות של מחלקות שיוצרות מודלים מתוך נתוני מקור נתונים.
    // הוא מוודא שכל מחלקה שתיישם אותו תכיל מתודה ליצירת אובייקט מתוך IDataReader.
    public interface IModelCreator<T>
    {
        /// <summary>
        /// יצירת מופע של אובייקט מסוג T מתוך נתוני מקור נתונים (IDataReader).
        /// </summary>
        /// <param name="model">אובייקט מסוג IDataReader המכיל את הנתונים מתוך מסד הנתונים.</param>
        /// <returns>אובייקט מסוג T, המייצג את המודל שנוצר מתוך הנתונים.</returns>

        T CreateModel(IDataReader model);
    }
}
