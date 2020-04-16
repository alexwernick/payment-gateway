using PaymentAPI.AcquiringBankClient.v1.Contracts.Requests.BankPayments;
using PaymentAPI.AcquiringBankClient.v1.Contracts.Responses.BankPayments;
using PaymentAPI.AcquiringBankClient.v1.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.AcquiringBankClient.v1.Services
{
  public class BankPaymentSimulationService : IBankPaymentService
  {
    private const double _paymentSuccessRate = 0.5;

    public async Task<CreateBankPaymentResponse> CreatePaymentAsync(CreateBankPaymentRequest paymentRequest)
    {
      List<string> errors = ValidateCardDetails(paymentRequest);

      if (errors.Any())
      {
        return new CreateBankPaymentResponse()
        {
          Success = false,
          Errors = errors
        };
      }

      double result = new Random().NextDouble();
      bool paymentSuccess = false;

      if (result < _paymentSuccessRate)
        paymentSuccess = true;

      if (paymentSuccess)
      {
        return new CreateBankPaymentResponse()
        {
          Success = false,
          Errors = new List<string>() { "Payment failed" }
        };
      }
      else
      {
        return new CreateBankPaymentResponse()
        {
          Success = true,
          Id = Guid.NewGuid()
        };
      }
    }

    private List<string> ValidateCardDetails(CreateBankPaymentRequest paymentRequest)
    {
      List<string> errors = new List<string>();

      if (string.IsNullOrEmpty(paymentRequest.CardNumber))
        errors.Add("Card number not provided");

      if (paymentRequest.Amount <= 0)
        errors.Add("Amount not a postive number");

      if (string.IsNullOrEmpty(paymentRequest.Cvv))
        errors.Add("CVV not provided");

      if (string.IsNullOrEmpty(paymentRequest.NameOnCard))
        errors.Add("Name on card not provided");

      if (paymentRequest.ExpiryDate <= DateTime.Now)
        errors.Add("Card has expired");

      return errors;
    }
  }
}
