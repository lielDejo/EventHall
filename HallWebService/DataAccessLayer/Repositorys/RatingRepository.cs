using EventModels;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HallWebService
{
    public class RatingRepository : Repository, IRepository<Rating>
    {
        // מחלקה זו אחראית לניהול פעולות CRUD עבור אובייקטי  Rating במסד הנתונים.
        // היא מממשת את הממשק IRepository<Rating> ומבצע פעולות כמו יצירה, עדכון, מחיקה ושאילתה על אובייקטים מסוג User.
        public RatingRepository(DBContext context) : base(context) { }
        public bool Create(Rating model)
        {
            string sql = $@"Insert into rating 
                         ([score], [UserId], [comment], [HallId])
                         values(@score, @UserId, @HallId)";

            this.dbContext.AddParameter("@UserId", model.UserId);
            this.dbContext.AddParameter("@score", model.Score.ToString());
            this.dbContext.AddParameter("@HallId", model.HallId);

            return this.dbContext.Insert(sql);
        }

        public bool Delete(String id)
        {
            string sql = @"Delete from Rating where RatingId = @RatingId";
            this.dbContext.AddParameter("@RatingId", id);
            return this.dbContext.Delete(sql);
        }

        public bool Delete(Rating model)
        {
            throw new NotImplementedException();
        }

        public List<Rating> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Rating> GetRatingByHallId(string id)
        {
            List<Rating> list = new List<Rating>();
            string sql = "Select * from Rating where HallId=@HallId";
            this.dbContext.AddParameter("@HallId", id);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.RatingCreator.CreateModel(reader));
                }
            }
            return list;
        }

        public string GetLastId()
        {
            throw new NotImplementedException();
        }

        public bool UpDate(EventHall model)
        {
            throw new NotImplementedException();
        }

        public Rating GetById(string id)
        {
            throw new NotImplementedException();
        }

        public bool UpDate(Rating model)
        {
            string sql = @"UPDATE rating 
                   SET [score] = @Score, 
                       [UserId] = @UserId, 
                       [HallId] = @HallId 
                   WHERE RatingId = @RatingId";

            this.dbContext.AddParameter("@Score", model.Score.ToString());
            this.dbContext.AddParameter("@UserId", model.UserId);
            this.dbContext.AddParameter("@HallId", model.HallId);
            this.dbContext.AddParameter("@RatingId", model.Id);

            return this.dbContext.UpDate(sql);
        }

    }
}