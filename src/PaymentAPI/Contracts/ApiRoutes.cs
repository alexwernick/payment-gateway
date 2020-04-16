using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts
{
  public static class ApiRoutes
  {

    public const string Version = "v1";
    public const string Root = "api";
    public const string Base = Root + "/" + Version;

    public static class Payments
    {
      public const string GetAll = Base + "/payments";
      public const string Get = Base + "/payments/{paymentId}";
      public const string Create = Base + "/payments";
    }

    public static class Identity
    {
      public const string Login = Base + "/identity/login";
      public const string Register = Base + "/identity/register";
    }
  }
}
