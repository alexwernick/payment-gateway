using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentAPI.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PaymentAPI.Services;
using PaymentAPI.Interfaces.Services;
using PaymentAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PaymentAPI.Interfaces.Installers;
using PaymentAPI.Installers;
using Microsoft.Extensions.Hosting;

namespace PaymentAPI
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.InstallServicesInassembly(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseAuthentication();

      app.UseHttpsRedirection();
      app.UseMvc();

      var swaggerSettings = new SwaggerSettings();

      Configuration.GetSection(nameof(SwaggerSettings)).Bind(swaggerSettings);


      app.UseSwagger(option =>
      {
        option.RouteTemplate = swaggerSettings.JsonRoute;
      });


      app.UseSwaggerUI(option =>
      {
        option.SwaggerEndpoint(swaggerSettings.UIEndpoint, swaggerSettings.Description);
      });
    }
  }
}
