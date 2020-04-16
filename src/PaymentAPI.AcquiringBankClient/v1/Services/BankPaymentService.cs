using PaymentAPI.AcquiringBankClient.v1.Contracts.Requests.BankPayments;
using PaymentAPI.AcquiringBankClient.v1.Contracts.Responses.BankPayments;
using PaymentAPI.AcquiringBankClient.v1.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.AcquiringBankClient.v1.Services
{
  public class BankPaymentService : IBankPaymentService
  {
    public Task<CreateBankPaymentResponse> CreatePaymentAsync(CreateBankPaymentRequest paymentRequest)
    {
      throw new NotImplementedException();
    }
  }
}
