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

        public WebShopDbContext WebShopDbContext { get; }

        private TestServer TestServer { get; }

        public InMemoryTestServerScope()
        {
            TestServer = new TestServer(CreateWebHostBuilder());
;           Client = TestServer.CreateClient();
            WebShopDbContext = TestServer.Host.Services.GetService<WebShopDbContext>();
        }

        public T GetService<T>()
        {
            return TestServer.Host.Services.GetService<T>();
        }

        public async Task AddProductsToDbContext(params Product[] products)
        {
            foreach (var product in products)
            {
                WebShopDbContext.Add(product);
            }
            await WebShopDbContext.SaveChangesAsync();
        }

        public async Task AddStockLevel(StockLevel stockLevel)
        {
            WebShopDbContext.Add(stockLevel);

            await WebShopDbContext.SaveChangesAsync();
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
