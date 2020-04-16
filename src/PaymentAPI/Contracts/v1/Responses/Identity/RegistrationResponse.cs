using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts.v1.Responses.Identity
{
  public class RegistrationResponse
  {
    public string Token { get; set; }

    public bool Success { get; set; }

    public IEnumerable<string> Errors { get; set; }
  }
}
