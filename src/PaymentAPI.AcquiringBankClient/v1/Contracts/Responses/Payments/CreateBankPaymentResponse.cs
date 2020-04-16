using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentAPI.AcquiringBankClient.v1.Contracts.Responses.BankPayments
{
  public class CreateBankPaymentResponse
  {
    public Guid? Id { get; set; }

    public bool Success { get; set; }

    public List<string> Errors { get; set; }

    public CreateBankPaymentResponse()
    {
      Errors = new List<string>();
    }
  }
}
