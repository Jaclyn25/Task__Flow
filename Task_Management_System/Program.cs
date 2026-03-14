namespace Task_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("task");

            builder.Services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IRepostriy<TaskModel>, GenericRepository<TaskModel>>();
            builder.Services.AddScoped(typeof(IRepostriy<>), typeof(GenericRepository<>));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                              .AddEntityFrameworkStores<TaskDbContext>()
                              .AddDefaultTokenProviders();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
