using Cleemy.Application;
using Cleemy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using Cleemy.Domain;
using System.Linq;
using System.Collections.Generic;

namespace Cleemy.Test
{
    public class ExpenseServiceTest
    {
        [Fact]
        public async void CreateExpense_DescriptionMandatory()
        {
            Mock<CleemyDbContext> mockContext = MockContext();

            var expenseService = new ExpenseService(mockContext.Object);

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = string.Empty,
                Date = System.DateTime.Today.AddDays(-1),
                Amount = 0,
                Type = Domain.ExpenseTypeEnum.Hotel,
                UserId = -1,
                Currency = "$"
            };

            var result = await expenseService.AddAsync(expenseCreateModel);

            Assert.True(ErrorCodes.EXPENSE_DESCRITPTION_IS_MANDATORY == result.Item1);
        }

        [Fact]
        public async void CreateExpense_DateFuture()
        {
            Mock<CleemyDbContext> mockContext = MockContext();

            var expenseService = new ExpenseService(mockContext.Object);

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = "test",
                Date = System.DateTime.Today,
                Amount = 0,
                Type = Domain.ExpenseTypeEnum.Hotel,
                UserId = -1,
                Currency = "$"
            };

            var result = await expenseService.AddAsync(expenseCreateModel);

            Assert.True(ErrorCodes.EXPENSE_DATE_FUTURE ==  result.Item1);
        }

        [Fact]
        public async void CreateExpense_DateLess3Month()
        {
            Mock<CleemyDbContext> mockContext = MockContext();

            var expenseService = new ExpenseService(mockContext.Object);

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = "test",
                Date = System.DateTime.Today.AddMonths(-3).AddDays(-1),
                Amount = 0,
                Type = Domain.ExpenseTypeEnum.Hotel,
                UserId = -1,
                Currency = "$"
            };

            var result = await expenseService.AddAsync(expenseCreateModel);

            Assert.True(ErrorCodes.EXPENSE_DATE_PAST_3_MONTH == result.Item1);
        }

        [Fact]
        public async void CreateExpense_UserNotExist()
        {
            Mock<CleemyDbContext> mockContext = MockContext();

            var expenseService = new ExpenseService(mockContext.Object);

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = "test",
                Date = System.DateTime.Today.AddDays(-1),
                Amount = 0,
                Type = Domain.ExpenseTypeEnum.Hotel,
                UserId = 1,
                Currency = "$"
            };

            var result = await expenseService.AddAsync(expenseCreateModel);

            Assert.True(ErrorCodes.EXPENSE_USER_NOT_FOUND == result.Item1);
        }

        [Fact]
        public async void CreateExpense_UserHasExpenseSameDay()
        {

            Mock<CleemyDbContext> mockContext = MockContext();

            var expenseService = new ExpenseService(mockContext.Object);

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = "UserHasExpenseSameDay",
                Date = System.DateTime.Today.AddDays(-1),
                Amount = 0,
                Type = Domain.ExpenseTypeEnum.Hotel,
                UserId = -1,
                Currency = "$"
            };

            var result = await expenseService.AddAsync(expenseCreateModel);

            Assert.True(ErrorCodes.EXPENSE_ALREADY_EXISTS_SAME_DATE == result.Item1);
        }

        [Fact]
        public async void CreateExpense_ExpenseNotSameCurrenncy()
        {

            Mock<CleemyDbContext> mockContext = MockContext();

            var expenseService = new ExpenseService(mockContext.Object);

            ExpenseCreateModel expenseCreateModel = new ExpenseCreateModel
            {
                Description = "ExpenseNotSameCurrency",
                Date = System.DateTime.Today.AddDays(-1),
                Amount = 0,
                Type = Domain.ExpenseTypeEnum.Hotel,
                UserId = -1,
                Currency = "€"
            };

            var result = await expenseService.AddAsync(expenseCreateModel);

            Assert.True(ErrorCodes.EXPENSE_CURRENCY_NOT_USER_CURRENCY == result.Item1);
        }

        private Mock<CleemyDbContext> MockContext()
        {
            var mockContext = new Mock<CleemyDbContext>();
            var mockSetExpense = new Mock<DbSet<Expense>>();
            // Expense
            var expenseData = new List<Expense>
            {
            new Expense { Date = System.DateTime.Today.AddDays(-1), UserId = -1 }
            }.AsQueryable();

            mockSetExpense.As<IQueryable<Expense>>().Setup(m => m.Provider).Returns(expenseData.Provider);
            mockSetExpense.As<IQueryable<Expense>>().Setup(m => m.Expression).Returns(expenseData.Expression);
            mockSetExpense.As<IQueryable<Expense>>().Setup(m => m.ElementType).Returns(expenseData.ElementType);
           
            
            // currency
            var mockSetCurrency = new Mock<DbSet<Currency>>();
            var currencyData = new List<Currency>
            {
              new Currency { Id = -1, Symbol = "$", Name = "Dollar" },
               new Currency { Id = -2, Symbol = "€", Name = "Euro" }
            }.AsQueryable();

            mockSetCurrency.As<IQueryable<Currency>>().Setup(m => m.Provider).Returns(currencyData.Provider);
            mockSetCurrency.As<IQueryable<Currency>>().Setup(m => m.Expression).Returns(currencyData.Expression);
            mockSetCurrency.As<IQueryable<Currency>>().Setup(m => m.ElementType).Returns(currencyData.ElementType);

            // user
            var mockSetUser = new Mock<DbSet<User>>();
            var userData = new List<User>
            {
                new User { Id = -1, CurrencyId = -1, Currency = currencyData.First() }
            }.AsQueryable();

            mockSetUser.As<IQueryable<User>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockSetUser.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userData.ElementType);

            mockContext.Setup(m => m.Expense).Returns(mockSetExpense.Object);
            mockContext.Setup(m => m.Currency).Returns(mockSetCurrency.Object);
            mockContext.Setup(m => m.User).Returns(mockSetUser.Object);

            return mockContext;
        }

    }
}