using EventModels;
using System.Data;

namespace HallWebService
{
    public class UserRepository : Repository
    {
        // מחלקה זו אחראית לניהול פעולות CRUD עבור אובייקטי User במסד הנתונים.
        // היא מממשת את הממשק IRepository<User> ומבצע פעולות כמו יצירה, עדכון, מחיקה ושאילתה על אובייקטים מסוג User.
        public UserRepository(DBContext context) : base(context) { }
        public string Create(string userName, string password, string phoneNumber, string email, bool isManage)
        {
            try
            {
                // שאילתה עם פרמטרים
                string sql = @"Insert into [User]
                        (UserName, [Password], PhoneNumber, Email, IsManage)
                        values (@UserName, @Password, @PhoneNumber, @Email, @IsManage)";

                // הוספת פרמטרים בצורה מאובטחת
                this.dbContext.AddParameter("@UserName", userName);
                this.dbContext.AddParameter("@Password", password);
                this.dbContext.AddParameter("@PhoneNumber", phoneNumber);
                this.dbContext.AddParameter("@Email", email);

                // המרה של ערך bool לערך מספרי עבור SQL
                this.dbContext.AddParameter("@IsManage", (isManage ? 1 : 0).ToString());

                // החדרת הרשומה למסד הנתונים
                this.dbContext.Insert(sql);
				
				return GetIDByUserNameAndPassword(userName, password);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה (לדוגמה: כתיבה ליומן אירועים)
                throw new Exception("Error while creating user", ex);
            }
        }


        public bool Delete(String id)
        {
            string sql = @"Delete from User where UserId = @UserId";
            this.dbContext.AddParameter("@UserId", id);
            return this.dbContext.Delete(sql);
        }

        public bool Delete(User model)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            List<User> list = new List<User>(); 
            string sql = "Select * from User";
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.UserCreator.CreateModel(reader));
                }
            }
            return list;
        }

        public User GetById(string id)
        {
            string sql = "Select * from User where UserId=@UserId";
            this.dbContext.AddParameter("@UserId", id);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                reader.Read();
                User user = this.modelFactory.UserCreator.CreateModel(reader);
                this.dbContext.ClearParameter();
                return user;
            }
        }
        public string GetUserNameById(string userId)
        {
            string sql = "SELECT [UserName] FROM [User] WHERE UserId = @UserId";
            this.dbContext.AddParameter("@UserId", userId);

            using (IDataReader reader = this.dbContext.Read(sql))
            {
                if (reader.Read())
                {
                    string userName = reader.GetString(0);
                    return userName;
                }
            }

            this.dbContext.ClearParameter();
            return ""; // או "לא נמצא"

        }

        public string GetIDByUserNameAndPassword(string userName, string password)
        {
            try
            {
                // שאילתה עם פרמטרים מאובטחים
                string sql = "SELECT * FROM [User] WHERE UserName = @userName AND [Password] = @password";

                // הוספת פרמטרים בצורה מאובטחת
                this.dbContext.AddParameter("@userName", userName);
                this.dbContext.AddParameter("@password", password);

                // ביצוע השאילתה
                using (IDataReader reader = this.dbContext.Read(sql))
                {
                    if (reader.Read()) // בדיקה אם יש תוצאות
                    {
                        User user = this.modelFactory.UserCreator.CreateModel(reader);
                        this.dbContext.ClearParameter(); // ניקוי פרמטרים
                        return user.Id;
                    }
                    else
                    {
                        this.dbContext.ClearParameter(); // ניקוי פרמטרים
                        return null; // אם אין תוצאות
                    }
                }
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה (למשל כתיבה ליומן אירועים)
                throw new Exception("Error while fetching user by username and password", ex);
            }
        }

 public string GetIDByAdminNameAndPassword(string userName, string password)
        {
            try
            {
                // שאילתה עם פרמטרים מאובטחים
                string sql = "SELECT * FROM [User] WHERE UserName = @userName AND [Password] = @password AND IsManage = TRUE";

                // הוספת פרמטרים בצורה מאובטחת
                this.dbContext.AddParameter("@userName", userName);
                this.dbContext.AddParameter("@password", password);

                // ביצוע השאילתה
                using (IDataReader reader = this.dbContext.Read(sql))
                {
                    if (reader.Read()) // בדיקה אם יש תוצאות
                    {
                        User user = this.modelFactory.UserCreator.CreateModel(reader);
                        this.dbContext.ClearParameter(); // ניקוי פרמטרים
                        return user.Id;
                    }
                    else
                    {
                        this.dbContext.ClearParameter(); // ניקוי פרמטרים
                        return ""; // אם אין תוצאות
                    }
                }
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה (למשל כתיבה ליומן אירועים)
                throw new Exception("Error while fetching user by username and password", ex);
            }
        }


        public string GetLastId()
        {
            throw new NotImplementedException();
        }

        public bool UpDate(User model)
        {
            try
            {
                string sql = @"Update [User] set 
                         UserName=@UserName, 
                         [Password]=@Password,    
                         PhoneNumber=@PhoneNumber,
                         Email=@Email,
                         IsManage=@IsManage
                         WHERE UserId = @UserId";

                // הוספת פרמטרים בצורה מאובטחת
                this.dbContext.AddParameter("@UserName", model.UserName);
                this.dbContext.AddParameter("@Password", model.Password);
                this.dbContext.AddParameter("@PhoneNumber", model.PhoneNumber);
                this.dbContext.AddParameter("@Email", model.Email);

                // המרה של ערך bool לערך מספרי (1 או 0) לפני הוספה
                this.dbContext.AddParameter("@IsManage", (model.IsManage ? 1 : 0).ToString());
                this.dbContext.AddParameter("@UserId", model.Id);

                // עדכון הרשומה במסד הנתונים
                return this.dbContext.UpDate(sql);
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה וזריקת חריגה עם מידע נוסף
                throw new Exception("Error while updating user", ex);
            }
        }

        public List<User> GetManagers()
        {
            List<User> list = new List<User>();
            string sql = "Select * from Users where IsManage = 1";
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.UserCreator.CreateModel(reader));
                }
            }
            return list;
        }

        public User GetManagerByHall(string hallId)
        {
            User  user= new User();

            string sql = @"SELECT User.UserId, User.UserName, User.Password, User.PhoneNumber, User.Email, User.IsManage, Manager.MansgerId, EventHall.HallId
FROM [User] INNER JOIN (Manager INNER JOIN EventHall ON Manager.MansgerId = EventHall.ownerId) ON User.UserId = Manager.MansgerId
WHERE (((EventHall.HallId)=@hallId));
";

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
       
    }
}
