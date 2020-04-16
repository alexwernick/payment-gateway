using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentAPI.Core.CustomDataAnnotations
{
  public class CardExpiryDateAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      var dt = (DateTime)value;
      if (dt > DateTime.Now)
      {
        return true;
      }
      return false;
    }
  }
}
