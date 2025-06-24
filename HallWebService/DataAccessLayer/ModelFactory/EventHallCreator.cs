using EventModels;
using System.Data;

namespace HallWebService
{
    // מחלקה זו אחראית ליצור מודל של אולמות אירועים מתוך נתוני מקור נתונים.
    public class EventHallCreator : IModelCreator<EventHall>
    {
        /// <summary>
        /// יוצר מופע של אובייקט EventHall מתוך נתוני מקור נתונים (IDataReader).
        /// </summary>
        /// <param name="src">אובייקט IDataReader המכיל את פרטי האולם מתוך מסד הנתונים.</param>
        /// <returns>אובייקט EventHall המייצג את האולם שנקלט ממסד הנתונים, כולל השדות הרלוונטיים.</returns>
        public EventHall CreateModel(IDataReader src)
        {
            EventHall eventHall = new EventHall()
            {
                Id = Convert.ToString(src["HallId"]),
                City = Convert.ToInt16(src["CityId"]),
                DescriptionHall = Convert.ToString(src["DescriptionHall"]),
                GeographicalLocation = Convert.ToString(src["GeographicalLocation"]),
                HallImage = null,
                HallName = Convert.ToString(src["HallName"]),
                PeopleContent = Convert.ToString(src["PeopleContent"]),
                Rating = Convert.ToInt16(src["Rating"]),
                TypeHall = Convert.ToString(src["TypeHall"]),
                OwnerId = Convert.ToString(src["ownerId"])
            };
            return eventHall;
        }
    }
}
