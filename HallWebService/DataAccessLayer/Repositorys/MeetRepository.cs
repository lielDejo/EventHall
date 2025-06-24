using EventModels;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HallWebService
{
    public class MeetRepository : Repository, IRepository<Meet>
    {
        // מחלקה זו אחראית לניהול פעולות CRUD עבור אובייקטי  Rating במסד הנתונים.
        // היא מממשת את הממשק IRepository<Rating> ומבצע פעולות כמו יצירה, עדכון, מחיקה ושאילתה על אובייקטים מסוג User.
        public MeetRepository(DBContext context) : base(context) { }
        public bool Create(Meet model)
        {
            try
            {
                // Define the SQL query
                //string sql = @"Insert into Meet 
                //        (HallId, UserId, DateMeed, Hour, ReasonMeet)
                //        values(@HallId, @UserId, @DateMeed, @Hour, @ReasonMeet)";

                string sql = $@"Insert into Meet 
                        (HallId, UserId, DateMeet, [Hour], ReasonMeet)
                        values({model.HallId}, {model.UserId}, '{model.DateMeet}', '{model.Hour}', '{model.ReasonMeet}')";

                //// Log the SQL for debugging
                //Console.WriteLine("Executing SQL: " + sql);

                //// Add parameters
                //this.dbContext.AddParameter("@HallId", model.HallId);
                //this.dbContext.AddParameter("@UserId", model.UserId);
                //this.dbContext.AddParameter("@DateMeed", model.DateMeed);
                //this.dbContext.AddParameter("@Hour", model.Hour);
                //this.dbContext.AddParameter("@ReasonMeet", model.ReasonMeet);

                // Execute the insert operation
                return this.dbContext.Insert(sql);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error inserting Meet: {ex.Message}");
                return false;
            }
        }


        public bool Delete(String id)
        {
            string sql = @"Delete from Meet where MeetId = @MeetId";
            this.dbContext.AddParameter("@MeetId", id);
            return this.dbContext.Delete(sql);
        }
        public bool Delete(Meet model)
        {
            return Delete(model.Id);
        }



        public Meet GetById(string id)
        {
            string sql = "Select * from Meet where MeetId=@MeetId";
            this.dbContext.AddParameter("@MeetId", id);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                reader.Read();
                Meet meet = this.modelFactory.MeetCreator.CreateModel(reader);
                this.dbContext.ClearParameter();
                return meet;
            }
        }
        public List<Meet> GetMeetByHallId(string id)
        {
            List<Meet> meets = new List<Meet>();

            try
            {
                // שאילתה עם שימוש בפרמטר מאובטח
                string sql = "SELECT * FROM Meet WHERE HallId = @HallId";

                // הוספת הפרמטר בצורה מאובטחת
                this.dbContext.AddParameter("@HallId", id);

                // קריאה לבסיס הנתונים
                using (IDataReader reader = this.dbContext.Read(sql))
                {
                    while (reader.Read())
                    {
                        // יצירת אובייקט Meet באמצעות ה-ModelFactory
                        Meet meet = this.modelFactory.MeetCreator.CreateModel(reader);
                        meets.Add(meet);
                    }
                }
            }
            catch (Exception ex)
            {
                // טיפול בשגיאה, הדפסה או רישום ל-Logger
                Console.WriteLine($"Error in GetMeetByHallId: {ex.Message}");
                return new List<Meet>();
            }
            finally
            {
                // ניקוי פרמטרים בכל מקרה
                this.dbContext.ClearParameter();
            }

            return meets;
        }


        public List<Meet> GetMeetsByUserId(string id)
        {
            List<Meet> meets = new List<Meet>();
            string sql = "SELECT * FROM Meet WHERE UserId = @UserId";

            try
            {
                // Add the parameter for the query
                this.dbContext.AddParameter("@UserId", id);

                // Execute the query and read the results
                using (IDataReader reader = this.dbContext.Read(sql))
                {
                    while (reader.Read()) // Loop through all rows
                    {
                        // Create a Meet object for each record and add it to the list
                        Meet meet = this.modelFactory.MeetCreator.CreateModel(reader);
                        meets.Add(meet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMeetsByUserId: {ex.Message}");
            }
            finally
            {
                // Clear the parameters after the operation is complete
                this.dbContext.ClearParameter();
            }

            return meets; // Return the list of Meet objects
        }


        public string GetLastId()
        {
            throw new NotImplementedException();
        }

        public bool UpDate(Meet model)
        {
            string sql = $@"UPDATE Meet 
                   SET DateMeet = '{model.DateMeet}',
                       [Hour] = '{model.Hour}',
                       ReasonMeet = '{model.ReasonMeet}',
                       MeetingSummary = '{model.MeetingSummary}'
                       WHERE MeetId = {model.Id}";

            //string sql = @"UPDATE Meet 
            //       SET HallId = @HallId,
            //           UserId = @UserId,
            //           DateMeed = @DateMeed,
            //           Hour = @Hour,
            //           ReasonMeet = @ReasonMeet
            //       WHERE MeetId = @MeetId";

       
            //this.dbContext.AddParameter("@HallId", model.HallId);
            //this.dbContext.AddParameter("@UserId", model.UserId);
            //this.dbContext.AddParameter("@DateMeed", model.DateMeed);
            //this.dbContext.AddParameter("@Hour", model.Hour);
            //this.dbContext.AddParameter("@ReasonMeet", model.ReasonMeet);
            //this.dbContext.AddParameter("@MeetId", model.Id);

            return this.dbContext.UpDate(sql);
        }
        public List<Meet> GetAll()
        {
            List<Meet> meets = new List<Meet>();
            string sql = "SELECT * FROM Meet";

            try
            {
                using (IDataReader reader = this.dbContext.Read(sql))
                {
                    while (reader.Read())
                    {
                        // יצירת אובייקט Meet מכל שורה בטבלה
                        Meet meet = this.modelFactory.MeetCreator.CreateModel(reader);
                        meets.Add(meet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll: {ex.Message}");
            }
            finally
            {
                this.dbContext.ClearParameter();
            }

            return meets;
        }

    }

}