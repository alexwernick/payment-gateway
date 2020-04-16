using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentAPI.Interfaces.Installers;
using PaymentAPI.Interfaces.Services;
using PaymentAPI.Services;
using PaymentAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Installers
{
  public class MvcInstaller : IInstaller
  {
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {


      services.AddMvc(options =>
      {
        options.EnableEndpointRouting = false;
      }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

      services.AddSwaggerGen(x =>
      {
        x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
        {
          Version = "v1",
          Title = "Payment Gateway API Documentation",
          Description = "This documemtation provides the information about the Payment" +
                  " Gateway API",
        });

        var openApiSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
        {
          Description = "JWT Authorization header using the bearer scheme",
          Name = "Authorization",
          In = Microsoft.OpenApi.Models.ParameterLocation.Header,
          Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
        };


        x.AddSecurityDefinition("Bearer", openApiSecurityScheme);

        x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
          {
                    { new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, new string[0] }
          });
      });


      var jwtSettings = new JwtSettings();
      configuration.Bind(nameof(jwtSettings), jwtSettings);
      services.AddSingleton(jwtSettings);

      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
    .AddJwtBearer(x =>
    {
      x.SaveToken = true;
      x.TokenValidationParameters = new TokenValidationParameters()
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true
      };
    });

      services.AddScoped<IIdentityService, IdentityService>();
    }
  }
}
