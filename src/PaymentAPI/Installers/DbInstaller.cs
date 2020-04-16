using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentAPI.Data;
using PaymentAPI.Interfaces.Installers;
using PaymentAPI.Interfaces.Services;
using PaymentAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Installers
{
  public class DbInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddDbContext<DataContext>(options =>
      options.UseSqlServer(
          configuration.GetConnectionString("DefaultConnection")));

      services.AddDefaultIdentity<IdentityUser>()
          .AddEntityFrameworkStores<DataContext>();

      services.AddScoped<IPaymentService, PaymentService>();
    }
  }
}
