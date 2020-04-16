using Microsoft.EntityFrameworkCore;
using PaymentAPI.Data;
using PaymentAPI.Domain;
using PaymentAPI.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.Services
{
  public class PaymentService : IPaymentService
  {

    private readonly DataContext _dataContext;

    public PaymentService(DataContext dataContext)
    {
      _dataContext = dataContext;
    }

    public async Task<Payment> GetPaymentByIdAndUserIdAsync(Guid paymentId, string userId)
    {
      return await _dataContext.Payments.SingleOrDefaultAsync(x => x.Id == paymentId && x.UserId == userId);
    }

    public async Task<List<Payment>> GetPaymentsByUserIdAsync(string userId)
    {
      var results = await _dataContext.Payments.Where(x => x.UserId == userId).ToListAsync();
      return results;
    }

    public async Task<bool> CreatePaymentAsync(Payment payment)
    {
      await _dataContext.Payments.AddAsync(payment);

      var result = await _dataContext.SaveChangesAsync();

      return result > 0;
    }

  }
}
