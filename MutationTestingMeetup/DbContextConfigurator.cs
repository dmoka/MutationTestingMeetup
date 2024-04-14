using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MutationTestingMeetup.Data;

namespace MutationTestingMeetup
{
    public static class DbContextConfigurator
    {
        public static void ConfigureDbContextUsingInMemorySQLite(this IServiceCollection services)
        {
            services.AddDbContext<WebShopDbContext>(options =>
                {
                    var connectionString = $"datasource=inmemorydb{Guid.NewGuid()};mode=memory;";
                    var connection = new SqliteConnection(connectionString);
                    connection.Open();
                    options.UseSqlite(connection);
                    options.EnableSensitiveDataLogging();
                    //Console.WriteLine("INFO - Using inmemory SQLite");
                },
                ServiceLifetime.Singleton);
        }
    }
}
