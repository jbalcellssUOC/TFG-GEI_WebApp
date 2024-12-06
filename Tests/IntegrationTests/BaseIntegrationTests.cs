using BusinessLogicLayer.Classes;
using DataAccessLayer.Classes;
using Entities.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class BaseIntegrationTests
{
    protected WebApplicationFactory<PresentationLayer.Startup>? Factory { get; private set; }
    protected HttpClient? Client { get; private set; }

    [SetUp]
    public void BaseSetUp()
    {
        Factory = new WebApplicationFactory<PresentationLayer.Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<BBDDContext>(options =>
                        options.UseInMemoryDatabase("TestDatabase"));

                    services.AddDALInjectionExtensions();
                    services.AddBLInjectionExtensions();

                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<BBDDContext>();
                        db.Database.EnsureCreated();
                    }
                });
            });

        Client = Factory.CreateClient();
    }

    [TearDown]
    public void BaseTearDown()
    {
        Client!.Dispose();
        Factory!.Dispose();
    }
}
