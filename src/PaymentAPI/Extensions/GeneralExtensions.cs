using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Extensions
{
  public static class HttpContextExtensions
  {
    public static string GetUserId(this HttpContext httpContext)
    {
      return GetUserClaim(httpContext, "id");
    }

    public static string GetUserClaim(this HttpContext httpContext, string claim)
    {
      if (httpContext.User == null)
      {
        return string.Empty;
      }
      return httpContext.User.Claims.FirstOrDefault(x => x.Type == claim).Value;
    }
  }
}
