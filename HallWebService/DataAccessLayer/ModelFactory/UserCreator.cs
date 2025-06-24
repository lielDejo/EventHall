using EventModels;
using System.Data;

namespace HallWebService
{
    public class UserCreator : IModelCreator<User>
    {
        // מחלקה זו אחראית ליצור אובייקט User מתוך נתוני מקור נתונים.
        public User CreateModel(IDataReader src)
        {
            /// <summary>
            /// יוצר מופע של אובייקט User מתוך נתוני מקור נתונים (IDataReader).
            /// </summary>
            /// <param name="src">אובייקט IDataReader המכיל את פרטי המשתמש ממסד הנתונים.</param>
            /// <returns>אובייקט User המייצג את המשתמש שנקלט ממסד הנתונים, כולל השדות הרלוונטיים.</returns>
            User user = new User()
            {
                Id = Convert.ToString(src["UserId"]),
                UserName = Convert.ToString(src["UserName"]),
                Password = Convert.ToString(src["Password"]),
                PhoneNumber = Convert.ToString(src["PhoneNumber"]),
                Email = Convert.ToString(src["Email"]),
                IsManage = Convert.ToBoolean(src["IsManage"])
            };
            return user;
        }

    }
}
