namespace EventHallWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // הוספת שירותי ה-Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // משך חיים של Session
                options.Cookie.HttpOnly = true;  // אבטחה - מונע גישה מצד לקוח
                options.Cookie.IsEssential = true; // מוודא שה-Session לא ייחסם
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Catalog}/{action=GetHallCatalog}/{id?}");

            app.Run();
        }
    }
}
