using PaymentAPI.Contracts.v1.Responses.Payments.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts.v1.Responses.Payments
{
  public class CreatePaymentResponse
  {
    public Guid? Id { get; set; }

    public StatusType Status { get; set; }

    public string StatusDisplay
    {
      get
      {
        return Status.ToString();
      }
    }

    public List<string> Errors { get; set; }

    public CreatePaymentResponse()
    {
      Errors = new List<string>();
    }
  }
}
