using PaymentAPI.AcquiringBankClient.v1.Contracts.Requests.BankPayments;
using PaymentAPI.AcquiringBankClient.v1.Contracts.Responses.BankPayments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.AcquiringBankClient.v1.Interfaces.Services
{
  public interface IBankPaymentService
  {
    Task<CreateBankPaymentResponse> CreatePaymentAsync(CreateBankPaymentRequest paymentRequest);
  }
}
