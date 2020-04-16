using PaymentAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Interfaces.Services
{
  public interface IPaymentService
  {

    Task<List<Payment>> GetPaymentsByUserIdAsync(string userId);

    Task<Payment> GetPaymentByIdAndUserIdAsync(Guid paymentId, string userId);

    Task<bool> CreatePaymentAsync(Payment payment);

    //Task<bool> UserOwnsPaymentAsync(Guid paymentId, string userId);
  }
}
