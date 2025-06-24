using EventModels;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

namespace HallWebService
{
    public class HallRepository : Repository, IRepository<EventHall>
    {
        // מחלקה זו אחראית לניהול פעולות CRUD עבור אובייקטי  Hal במסד הנתונים.
        // היא מממשת את הממשק IRepository<User> ומבצע פעולות כמו יצירה, עדכון, מחיקה ושאילתה על אובייקטים מסוג User.
        public HallRepository(DBContext context) : base(context) { }
        public bool Create(EventHall model)
        {
            //string sql = $@"Insert into EventHall 
            //             (HallName, GeographicalLocation, PeopleContent, TypeHall, DescriptionHall, CityId, [Rating], ownerId)
            //             values(@HallName, @GeographicalLocation, @PeopleContent, @TypeHall, @DescriptionHall, {model.City}, {model.Rating}, @ownerId)";

            
            string sql = $@"Insert into EventHall 
                         (HallName, GeographicalLocation, PeopleContent, TypeHall, DescriptionHall, CityId, [Rating], ownerId)
                         values('{model.HallName}', '{model.GeographicalLocation}', '{model.PeopleContent}', '{model.TypeHall}', '{model.DescriptionHall}', {model.City}, {model.Rating}, '{model.OwnerId}')";


            //this.dbContext.AddParameter("@HallName", model.HallName);
            //this.dbContext.AddParameter("@GeographicalLocation", model.GeographicalLocation);
            //this.dbContext.AddParameter("@PeopleContent", model.PeopleContent);
            //this.dbContext.AddParameter("@TypeHall", model.TypeHall);
            //this.dbContext.AddParameter("@DescriptionHall", model.DescriptionHall);
            ////this.dbContext.AddParameter("@CityId", model.City);
            //// this.dbContext.AddParameter("@Rating", model.Rating.ToString());
            //this.dbContext.AddParameter("@ownerId", model.OwnerId);

            return this.dbContext.Insert(sql);
        }

        public bool Delete(String id)
        {
            string sql = @"Delete from EventHall where HallId = @HallId";
            this.dbContext.AddParameter("@HallId", id);
            return this.dbContext.Delete(sql);
        }

        public bool Delete(EventHall model)
        {
            throw new NotImplementedException();
        }

        public List<EventHall> GetAll()
        {
            List<EventHall> list = new List<EventHall>();
            string sql = "Select * from EventHall";
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.EventHallCreator.CreateModel(reader));
                }
            }
            return list;
        }

        public EventHall GetById(string id)
        {
            string sql = "Select * from EventHall where HallId=@HallId";
            this.dbContext.AddParameter("@HallId", id);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                reader.Read();
                return this.modelFactory.EventHallCreator.CreateModel(reader);
            }
        }
        public string GetIDHallbyName(string name)
        {
            string sql = "SELECT HallId FROM EventHall WHERE HallName = @name";
            this.dbContext.AddParameter("@name", name);

            using (IDataReader reader = this.dbContext.Read(sql))
            {
                if (reader.Read())
                {
                    return reader["HallId"].ToString();
                }
                else
                {
                    return null; // או אפשר לזרוק חריגה אם זה מצב לא תקין מבחינת הלוגיקה שלך
                }
            }
        }

        public string GetNameById(string id)
        {
            return GetById(id).HallName;
        }

        public string GetLastId()
        {
            throw new NotImplementedException();
        }

        public bool UpDate(EventHall model)
        {
            string sql = @"UPDATE EventHall SET 
                HallName = @HallName, 
                GeographicalLocation = @GeographicalLocation,
                PeopleContent = @PeopleContent,
                TypeHall = @TypeHall,
                DescriptionHall = @DescriptionHall,
                CityId = @CityId,
                Rating = @Rating,
                OwnerId = @OwnerId
                WHERE HallId = @HallId"; 

            this.dbContext.AddParameter("@HallName", model.HallName);
            this.dbContext.AddParameter("@GeographicalLocation", model.GeographicalLocation);
            this.dbContext.AddParameter("@PeopleContent", model.PeopleContent.ToString());
            this.dbContext.AddParameter("@TypeHall", model.TypeHall);
            this.dbContext.AddParameter("@DescriptionHall", model.DescriptionHall);
            this.dbContext.AddParameter("@CityId", model.City.ToString());
            this.dbContext.AddParameter("@Rating", model.Rating.ToString());
            this.dbContext.AddParameter("@OwnerId", model.OwnerId);
            this.dbContext.AddParameter("@HallId", model.Id);

            
            return this.dbContext.UpDate(sql);
        }
        public List<EventHall> SortByCity(string cityN)
        {
            List<EventHall> list = new List<EventHall>();
            string sql = "SELECT EventHall.*, [City].City FROM [City] INNER JOIN EventHall ON [City].CityId = EventHall.CityId\r\nWHERE ((([City].City)=@city));";
            this.dbContext.AddParameter("@city", cityN);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.EventHallCreator.CreateModel(reader));
                }
            }
            return list;
        }
        public List<EventHall> SortByContent(string min, string max)
        {
            List<EventHall> list = new List<EventHall>();
            string sql = "SELECT EventHall.* FROM EventHall WHERE EventHall.PeopleContent BETWEEN @min AND @max";
            this.dbContext.AddParameter("@min", min);
            this.dbContext.AddParameter("@max", max);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.EventHallCreator.CreateModel(reader));
                }
            }
            return list;
        }
        public List<EventHall> SortByRatings(string rating)
        {
            List<EventHall> list = new List<EventHall>();
            string sql = "SELECT EventHall.* FROM EventHall WHERE EventHall.Rating=@rating;";
            this.dbContext.AddParameter("@rating", rating);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    EventHall eventHall = this.modelFactory.EventHallCreator.CreateModel(reader);
                    list.Add(eventHall);
                }
            }
            return list;
        }

        public List<EventHall> SortBytype(string type)
        {
            List<EventHall> list = new List<EventHall>();
            string temp = "";
            if (type == "אולם")
                temp = "1";
            else if (type == "גן אירועים")
                temp = "2";
            else if (type == "גן אירועים | אולם")
                temp = "3";
            // SQL מתוקן עם שימוש בפרמטרים
            string sql = "SELECT EventHall.* FROM EventHall WHERE EventHall.TypeHall=@temp OR EventHall.TypeHall=\"3\";";

            // הוספת פרמטר בצורה נכונה
            this.dbContext.AddParameter("@temp", temp);

            // קריאה למידע עם קריאת נתונים מהדאטהבייס
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    // יצירת המודל מהקורא
                    EventHall eventHall = this.modelFactory.EventHallCreator.CreateModel(reader);
                    list.Add(eventHall);
                }
            }

            return list;
        }


        public User GetManagerByHall(string hallId)
        {
            User user = new User();

            string sql = @"SELECT User.UserId, User.UserName, User.Password, User.PhoneNumber, User.Email, User.IsManage, Manager.MansgerId, EventHall.HallId
FROM [User] INNER JOIN (Manager INNER JOIN EventHall ON Manager.MansgerId = EventHall.ownerId) ON User.UserId = Manager.MansgerId
WHERE (((EventHall.HallId)=@hallId))";

            this.dbContext.AddParameter("@HallId", hallId);

            using (IDataReader reader = this.dbContext.Read(sql))
            {
                if (reader.Read()) // בדיקה אם יש נתונים
                {
                    user = this.modelFactory.UserCreator.CreateModel(reader);
                    this.dbContext.ClearParameter();
                }
            }

            return user;
        }
        public List<EventHall> GetHallsByManager(string manageId)
        {
            List<EventHall> halls = new List<EventHall>();

            string sql = @"SELECT EventHall.* 
                   FROM EventHall 
                   WHERE EventHall.ownerId = @manageId";

            this.dbContext.AddParameter("@manageId", manageId);

            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read()) // קורא לכל הרשומות
                {
                    EventHall hall = this.modelFactory.EventHallCreator.CreateModel(reader);
                    halls.Add(hall); // מוסיף כל אולם לרשימה
                }
            }

            return halls; // מחזיר את רשימת האולמות
        }

    }
}

