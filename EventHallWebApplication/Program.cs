namespace EventHallWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ����� ������ �-Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // ��� ���� �� Session
                options.Cookie.HttpOnly = true;  // ����� - ���� ���� ��� ����
                options.Cookie.IsEssential = true; // ����� ��-Session �� �����
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
