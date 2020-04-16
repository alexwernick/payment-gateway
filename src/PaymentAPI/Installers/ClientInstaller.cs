using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentAPI.AcquiringBankClient.v1.Interfaces.Services;
using PaymentAPI.AcquiringBankClient.v1.Services;
using PaymentAPI.Data;
using PaymentAPI.Interfaces.Installers;
using PaymentAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Installers
{
  public class ClientInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
      services.AddScoped<IBankPaymentService, BankPaymentSimulationService>();
    }
  }
}
