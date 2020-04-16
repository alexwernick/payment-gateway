using PaymentAPI.Contracts.v1.Responses.Payments.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts.v1.Responses.Payments
{
  public class GetPaymentResponse
  {
    public Guid Id { get; set; }

    private string _cardNumber;
    public string CardNumber
    {
      get
      {
        if (string.IsNullOrEmpty(_cardNumber))
          return _cardNumber;

        List<char> charList = _cardNumber.ToList();

        string maskedCardNumber = new string(charList
                                  .Select((x, i) => i < charList.Count - 4
                                  ? '*' : x).ToArray());

        return maskedCardNumber;
      }
      set
      {
        _cardNumber = value;
      }
    }

    public DateTime ExpiryDate { get; set; }

    public float Amount { get; set; }

    public string Cvv { get; set; }

    public string NameOnCard { get; set; }

    public StatusType Status { get; set; }

    public string StatusDisplay
    {
      get
      {
        return Status.ToString();
      }
    }

    public Guid? AcquiringBankPaymentId { get; set; }

    public DateTime AddDate { get; set; }

  }
}
