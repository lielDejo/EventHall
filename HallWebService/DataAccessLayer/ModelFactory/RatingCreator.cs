using EventModels;
using System.Data;

namespace HallWebService
{
    // מחלקה זו אחראית ליצור אובייקט Rating מתוך נתוני מקור נתונים.
    public class RatingCreator : IModelCreator<Rating>
    {
        /// <summary>
        /// יוצר מופע של אובייקט Rating מתוך נתוני מקור נתונים (IDataReader).
        /// </summary>
        /// <param name="src">אובייקט IDataReader המכיל את פרטי הדירוג ממסד הנתונים.</param>
        /// <returns>אובייקט Rating המייצג את הדירוג שנקלט ממסד הנתונים, כולל השדות הרלוונטיים.</returns>
        public Rating CreateModel(IDataReader src)
        {
            Rating rating = new Rating()
            {
                Id = Convert.ToString(src["MeetId"]),
                HallId = Convert.ToString(src["HallId"]),
                Score = Convert.ToInt16(src["Score"])
            };
            return rating;
        }
    }
}
