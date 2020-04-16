using PaymentAPI.Core.CustomDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Contracts.v1.Requests.Payments
{
  public class CreatePaymentRequest
  {
    [Required]
    public string CardNumber { get; set; }

    [CardExpiryDate(ErrorMessage = "The ExpiryDate is in the past")]
    public DateTime ExpiryDate { get; set; }

    [Range(1, float.MaxValue, ErrorMessage = "Please enter an Amount bigger than {1}")]
    public float Amount { get; set; }

    [Required]
    public string Cvv { get; set; }

    [Required]
    public string NameOnCard { get; set; }
  }
}
