using EventModels;  
using System.Data;


namespace HallWebService
{
    // מחלקה זו אחראית ליצור אובייקט Meet מתוך נתוני מקור נתונים.
    public class MeetCreator : IModelCreator<Meet>
    {
        /// <summary>
        /// יוצר מופע של אובייקט Meet מתוך נתוני מקור נתונים (IDataReader).
        /// </summary>
        /// <param name="src">אובייקט IDataReader המכיל את פרטי המפגש ממסד הנתונים.</param>
        /// <returns>אובייקט Meet המייצג את המפגש שנקלט ממסד הנתונים, כולל השדות הרלוונטיים.</returns>
        public Meet CreateModel(IDataReader src)
        {
            Meet meet = new Meet()
            {
                Id = Convert.ToString(src["MeetId"]),
                DateMeet = Convert.ToString(src["DateMeet"]),
                HallId = Convert.ToString(src["HallId"]),
                Hour = Convert.ToString(src["Hour"]),
                ReasonMeet = Convert.ToString(src["ReasonMeet"]),
                MeetingSummary = Convert.ToString(src["MeetingSummary"]),
                UserId = Convert.ToString(src["UserId"])
            };
            return meet;
        }
    }
}
