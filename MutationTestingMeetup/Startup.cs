using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MutationTestingMeetup.Data;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //Configuration = configuration;
        }

        //public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.ConfigureDbContextUsingInMemorySQLite();

            services.AddScoped<IProductsFinder, ProductsFinder>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStockLevelRepository, StockLevelRepository>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IHandler<ProductCreatedEvent>, ProductCreatedEventHandler>();
            services.AddScoped<IHandler<ProductPickedEvent>, ProductPickedEventHandler>();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
