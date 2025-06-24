namespace HallWebService
{
    public class Repository
    {
        // מחלקה זו מייצגת את מאגר הנתונים ומספקת גישה לאובייקטי נתונים מתוך DBContext.
        protected DBContext dbContext;
        protected ModelFactory modelFactory;

        /// <summary>
        /// יוצר מופע של Repository ומאתח את אובייקט ה-DBContext וה-ModelFactory.
        /// </summary>
        /// <param name="dbContext">אובייקט DBContext שמספק את החיבור למסד הנתונים.</param>
        public Repository(DBContext dbContext)
        {
            this.dbContext = dbContext;
            this.modelFactory = new ModelFactory();
        }
        public string GetLastID()
        {
            string sql = "Select @@Identity";
            return this.dbContext.ReadValue(sql).ToString();
        }

    }
   
}
