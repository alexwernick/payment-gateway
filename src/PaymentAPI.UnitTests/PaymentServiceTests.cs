using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentAPI.Data;
using PaymentAPI.Domain;
using PaymentAPI.Services;
using System;
using System.Threading.Tasks;

namespace PaymentAPI.UnitTests
{
  [TestClass]
  public class PaymentServiceTests : UnitTestBase
  {
    private readonly PaymentService _paymentService;

    public PaymentServiceTests() :
      base()
    {
      _paymentService = new PaymentService(_context);
    }

    [TestMethod]
    public async Task CreatePaymentAsync_CreatePayment_Success()
    {
      var payment = new Payment()
      {

      };

      var result = await _paymentService.CreatePaymentAsync(payment);

      Assert.IsTrue(result);

    }

    [TestMethod]
    public async Task GetPaymentsByUserIdAsync_GetPaymentsForUser_TwoResultsReturned()
    {
      await Seed();

      var result = await _paymentService.GetPaymentsByUserIdAsync(_testUserId);

      Assert.IsTrue(result.Count == 2);
    }

    [TestMethod]
    public async Task GetPaymentsByUserIdAsync_GetPaymentsForDifferentUser_NoResultsReturned()
    {
      await Seed();

      var result = await _paymentService.GetPaymentsByUserIdAsync(Guid.NewGuid().ToString());

      Assert.IsTrue(result.Count == 0);
    }

    [TestMethod]
    public async Task GetPaymentByIdAndUserIdAsync_GetPaymentInDatabase_ReturnsPayment()
    {
      await Seed();

      var result = await _paymentService.GetPaymentByIdAndUserIdAsync(_payment1Id, _testUserId);

      Assert.AreEqual(result.Id, _payment1Id);
    }

    [TestMethod]
    public async Task GetPaymentByIdAndUserIdAsync_GetPaymentNotInDatabase_ReturnsNull()
    {
      await Seed();

      var result = await _paymentService.GetPaymentByIdAndUserIdAsync(Guid.NewGuid(), _testUserId);

      Assert.IsNull(result);

    }

  }
}
