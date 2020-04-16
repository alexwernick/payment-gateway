using Microsoft.AspNetCore.Identity;
using PaymentAPI.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Domain
{
  public class Payment
  {
    [Key]
    public Guid Id { get; set; }

    public string UserId { get; set; }

    public string CardNumber { get; set; }

    public DateTime ExpiryDate { get; set; }

    public float Amount { get; set; }

    public string Cvv { get; set; }

    public string NameOnCard { get; set; }

    public StatusType Status { get; set; }

    public Guid? AcquiringBankPaymentId { get; set; }

    public string AcquiringBankErrors { get; set; }

    public DateTime AddDate { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser User { get; set; }

  }
}
