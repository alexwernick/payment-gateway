using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PaymentAPI.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.UnitTests
{
  public class UnitTestBase : IDisposable
  {
    protected readonly DataContext _context;
    protected const string _testUserEmail = "test@test.com";
    protected string _testUserId { get; private set; }
    protected Guid _payment1Id { get; private set; }
    protected Guid _payment2Id { get; private set; }
    protected UnitTestBase()
    {

      var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase("TestDb")
        .Options;
      _context = new DataContext(options);
      _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
      _context.Database.EnsureDeleted();
      _context.Dispose();
    }

    protected async Task Seed()
    {
      var newUser = new IdentityUser
      {
        Email = _testUserEmail,
        UserName = _testUserEmail
      };

      await _context.Users.AddAsync(newUser);
      await _context.SaveChangesAsync();
      _testUserId = newUser.Id;

      var payment1 = new Domain.Payment()
      {
        UserId = newUser.Id,
        CardNumber = "1111111111111111",
        ExpiryDate = DateTime.Now.AddYears(1),
        Amount = 100,
        Cvv = "111",
        NameOnCard = "test",
        Status = Domain.Enumerations.StatusType.Success,
        AcquiringBankPaymentId = Guid.NewGuid(),
        AddDate = DateTime.UtcNow
      };

      var payment2 = new Domain.Payment()
      {
        UserId = newUser.Id,
        CardNumber = "1111111111111111",
        ExpiryDate = DateTime.Now.AddYears(1),
        Amount = 200,
        Cvv = "111",
        NameOnCard = "test",
        Status = Domain.Enumerations.StatusType.Failed,
        AcquiringBankPaymentId = Guid.NewGuid(),
        AddDate = DateTime.UtcNow
      };

      await _context.Payments.AddAsync(payment1);

      await _context.Payments.AddAsync(payment2);

      var result = await _context.SaveChangesAsync();

      _payment1Id = payment1.Id;
      _payment2Id = payment2.Id;

    }



  }
}
