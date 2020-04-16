
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentAPI.AcquiringBankClient.v1.Contracts.Requests.BankPayments
{
  public class CreateBankPaymentRequest
  {

    public string CardNumber { get; set; }

    public DateTime ExpiryDate { get; set; }

    public float Amount { get; set; }

    public string Cvv { get; set; }

    public string NameOnCard { get; set; }
  }
}
