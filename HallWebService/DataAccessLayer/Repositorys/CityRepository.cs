using EventModels;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HallWebService
{
    public class CityRepository : Repository, IRepository<City>
    {
        // מחלקה זו אחראית לניהול פעולות CRUD עבור אובייקטי  Rating במסד הנתונים.
        // היא מממשת את הממשק IRepository<Rating> ומבצע פעולות כמו יצירה, עדכון, מחיקה ושאילתה על אובייקטים מסוג User.
        public CityRepository(DBContext context) : base(context) { }
        public bool Create(string cityName)
        {
            // בדיקה אם העיר כבר קיימת
            string checkSql = "SELECT CityID FROM City WHERE City = @City";
            this.dbContext.AddParameter("@City", cityName);

            var existingCityID = this.dbContext.ReadValue(checkSql);
            if (existingCityID != null)
            {
                return true; // העיר כבר קיימת
            }

            // להוסיף את העיר (ולהחזיר את ה-ID אם הצליח)
            string insertSql = "INSERT INTO City (City) VALUES (@City)";
            this.dbContext.AddParameter("@City", cityName);
            // קריאה לפקודת INSERT, מחפשים את מספר השורות שהוספו
            var result = this.dbContext.Insert(insertSql);

            // אם result לא null, אז העיר התווספה בהצלחה
            return result ;
        }
        public int GetCityIdByName(string cityName)
        {
            string query = "SELECT CityID FROM City WHERE City = @City";
            this.dbContext.AddParameter("@City", cityName);

            var result = this.dbContext.ReadValue(query);
            if (result != null)
                return Convert.ToInt32(result);
            return 0;
        }

        public bool Create(City model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(String id)
        {
            string sql = @"Delete from City where CityId = @CityId";
            this.dbContext.AddParameter("@CityId", id);
            return this.dbContext.Delete(sql);
        }
        public bool Delete(City model)
        {
            throw new NotImplementedException();
        }

        public List<City> GetAll()
        {
            List<City> list = new List<City>();
            string sql = "Select * from City";
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.CityCreator.CreateModel(reader));
                }
            }
            return list;
        }

        public City GetById(string id)
        {
            throw new NotImplementedException();
        }

        public string GetLastId()
        {
            throw new NotImplementedException();
        }

        public bool UpDate(City model)
        {
            throw new NotImplementedException();
        }
    }
      
}