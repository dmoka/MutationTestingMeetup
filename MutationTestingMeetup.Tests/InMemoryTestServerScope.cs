using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MutationTestingMeetup.Data;
using MutationTestingMeetup.Domain;

namespace MutationTestingMeetup.Tests
{
    public class InMemoryTestServerScope : IDisposable
    {
        public HttpClient Client { get; }

        public WarehouseDbContext WarehouseDbContext { get; }

        private TestServer TestServer { get; }

        public InMemoryTestServerScope()
        {
            TestServer = new TestServer(CreateWebHostBuilder());
;           Client = TestServer.CreateClient();
            WarehouseDbContext = TestServer.Host.Services.GetService<WarehouseDbContext>();
        }

        public T GetService<T>()
        {
            return TestServer.Host.Services.GetService<T>();
        }

        public async Task AddProductsToDbContext(params Product[] products)
        {
            foreach (var product in products)
            {
                WarehouseDbContext.Add(product);
            }
            await WarehouseDbContext.SaveChangesAsync();
        }

        public async Task AddStockLevel(StockLevel stockLevel)
        {
            WarehouseDbContext.Add(stockLevel);

            await WarehouseDbContext.SaveChangesAsync();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(Assembly.GetAssembly(typeof(Startup)).Location))
                .UseStartup<Startup>();
        }


        //Tear-down
        public void Dispose()
        {
            TestServer?.Dispose();
        }
    }
}
