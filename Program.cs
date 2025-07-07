using LockerManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace LockerManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllers();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();
            app.MapControllers();
            app.Run();

        }
    }
}
