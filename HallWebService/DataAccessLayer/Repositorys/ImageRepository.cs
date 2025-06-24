using EventModels;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace HallWebService
{
    public class ImageRepository : Repository, IRepository<Image_>
    {
        // מחלקה זו אחראית לניהול פעולות CRUD עבור אובייקטי  Image במסד הנתונים.
        // היא מממשת את הממשק IRepository<User> ומבצע פעולות כמו יצירה, עדכון, מחיקה ושאילתה על אובייקטים מסוג Image.
        public ImageRepository(DBContext context) : base(context) { }
        public bool Create(Image_ model)
        {
            string sql = @"INSERT INTO [Image] ([ImageName], [ImageAddress], [HallId])
VALUES (@ImageName, @ImageAddress, @HallId);
";

            this.dbContext.AddParameter("@ImageName", model.ImageName);
            this.dbContext.AddParameter("@ImageAddress", model.ImageAddress);
            this.dbContext.AddParameter("@HallID", model.HallId);

            return this.dbContext.Insert(sql);
        }

        public bool Delete(string id)
        {
            string sql = @"Delete from [Image] where ImageId = @ImageId";
            this.dbContext.AddParameter("@ImageId", id);
            return this.dbContext.Delete(sql);
        }

        public bool Delete(Image_ model)
        {
            throw new NotImplementedException();
        }

        public List<Image_> GetLsPicByHallId(string id)
        {
            List <Image_> list = new List<Image_>();
            string sql = "SELECT Image.ImageId, Image.ImageName, Image.ImageAddress, Image.HallId\r\nFROM [Image]\r\nWHERE (((Image.HallId)=@HallId));\r\n";
            this.dbContext.AddParameter("@HallId", id);
            using (IDataReader reader = this.dbContext.Read(sql))
            {
                while (reader.Read())
                {
                    list.Add(this.modelFactory.ImageCreator.CreateModel(reader));
                }
                reader.Read();
            }
            return list;    
        }
        public Image_ GetMainPicByHallId(string id)
        {
            string sql = @"SELECT Image.ImageId, Image.ImageName, Image.ImageAddress, Image.HallId
                   FROM [Image]
                   WHERE Image.HallId = @HallId AND Image.ImageName = '1'";

            this.dbContext.AddParameter("@HallId", id);

            using (IDataReader reader = this.dbContext.Read(sql))
            {
                if (reader.Read()) // Ensure there's data to read
                {
                    return this.modelFactory.ImageCreator.CreateModel(reader);
                }
            }

            // Return a default or null Image object if no record is found
            return null;
        }


        public string GetLastId()
        {
            throw new NotImplementedException();
        }

        public List<Image_> GetAll()
        {
            throw new NotImplementedException();
        }

        public Image_ GetById(string id)
        {
            throw new NotImplementedException();
        }

        public bool UpDate(Image_ model)
        {
            throw new NotImplementedException();
        }
    }
}

