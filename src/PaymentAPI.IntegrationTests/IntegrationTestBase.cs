using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentAPI.Contracts;
using PaymentAPI.Contracts.v1.Requests.Identity;
using PaymentAPI.Contracts.v1.Responses.Identity;
using PaymentAPI.Data;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PaymentAPI.IntegrationTests
{

  public class IntegrationTestBase : IDisposable
  {

    private readonly IServiceProvider _serviceProvider;
    protected readonly HttpClient _testClient;

    protected IntegrationTestBase()
    {
      var appFactory = new WebApplicationFactory<Startup>()
        .WithWebHostBuilder(builder =>
        {
          builder.ConfigureServices(services =>
          {
            services.RemoveAll(typeof(DataContext));
            services.AddDbContext<DataContext>(options =>
            {
              options.UseInMemoryDatabase("TestDb");
            });

          });
        });

      _testClient = appFactory.CreateClient();
      _serviceProvider = appFactory.Services;
    }

    public void Dispose()
    {
      using var serviceScope = _serviceProvider.CreateScope();
      var context = serviceScope.ServiceProvider.GetService<DataContext>();
      context.Database.EnsureDeleted();
    }

    protected async Task AuthenticateAsync(string email = "test@test.com")
    {
      _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync(email));
    }

    private async Task<string> GetJwtAsync(string email)
    {
      var response = await _testClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new RegistrationRequest()
      {
        Email = email,
        Password = "Password1!"
      });

      var registrationResponse = await response.Content.ReadAsAsync<RegistrationResponse>();

      return registrationResponse.Token;

    }

   
  }
}
