using EventModels;
using System.Data;


namespace HallWebService
{
    public class CityCreator : IModelCreator<City>
    {
        /// <summary>
        /// יוצר מופע של אובייקט מתוך נתוני מקור נתונים.
        /// </summary>
        /// <param name="src"> IDataReaderאובייקט המכיל את פרטי העיר ממסד הנתונים</param>
        /// <returns> City אובייקט שמכיל את המידע על העיר</returns>
        /// 
        public City CreateModel(IDataReader src)
        {
            City city = new City()
            {
                Id = Convert.ToString(src["CityId"]),
                CityName = Convert.ToString(src["City"]),
                Halls = null
            };
            return city;
        }
    }
}
