using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts.v1.Responses.Payments
{
  public class GetAllPaymentsResponse
  {
    public List<GetPaymentResponse> Payments { get; set; }

    public GetAllPaymentsResponse()
    {
      Payments = new List<GetPaymentResponse>();
    }

  }
}
