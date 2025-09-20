using Database.Contexts;
using Microsoft.EntityFrameworkCore;
using Service.Implementation;
using Service.Interfaces;

namespace SportTrendTennisWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContextPool<GroupManagementContext>(options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("GroupManagementConnectionString")
        ));
        builder.Services.AddScoped<IGroupManagementService, GroupManagementService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.WithOrigins("http://localhost:5174")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}