
using LandlordCardGameApi.Services;
using LandlordCardGameApi.Settings;

namespace LandlordCardGameApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            // Add services to the container.
            builder.Services.Configure<AcsSettings>(
                builder.Configuration.GetSection("AcsSettings"));

            builder.Services.Configure<SessionStorageSettings>(
                builder.Configuration.GetSection("SessionStorageSettings"));

            builder.Services.AddSingleton<IAcsService, AcsService>();
            builder.Services.AddSingleton<ISessionStorageService, SessionStorageService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();

            // Use CORS
            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }
    }
}
