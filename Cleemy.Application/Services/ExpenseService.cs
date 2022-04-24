using Cleemy.Domain;
using Cleemy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleemy.Application
{
    public class ExpenseService : IExpenseService
    {
        private readonly CleemyDbContext context;

        public ExpenseService(CleemyDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Expense>> GetExpensesAsync(int? userId, string sortColumn)
        {
            IQueryable<Expense> queryable = context.Expense.Include(e => e.User).Include(e => e.Currency).AsNoTracking();

            if (userId.HasValue)
            {
                // Start the predicate builder
                queryable = queryable.Where(x => x.UserId == userId.Value);
            }

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortColumn.ToLower().EndsWith("amount"))
                {
                    queryable = sortColumn.StartsWith("-") ? queryable.OrderByDescending(x => x.Amount) : queryable.OrderBy(x => x.Amount);
                }
                if (sortColumn.ToLower().EndsWith("date"))
                {
                    queryable = sortColumn.StartsWith("-") ? queryable.OrderByDescending(x => x.Date) : queryable.OrderBy(x => x.Date);
                }
            }

            return await queryable.ToListAsync();
        }


        public async Task< (string , Expense? )> AddAsync(ExpenseCreateModel entity)
        {
            if (entity is null)
            {
                return new ( "entity is empty", null );
            }

            Currency? currency = context.Currency.FirstOrDefault(f => f.Symbol == entity.Currency);

            if (currency is null)
            {
                return new("currency is not valid", null);
            }

            User? user = context.User.FirstOrDefault(f => f.Id == entity.UserId);

            if (user is null)
            {
                return new("user is not exist", null);
            }

            if (entity.Date >= DateOnly.FromDateTime(DateTime.Now))
            {
                return new("date is equal to the current date or in the future", null);
            }
            if (entity.Date <= DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)))
            {
                return new("date is lower to the current - three month", null);
            }
            if (string.IsNullOrWhiteSpace(entity.Description))
            {
                return new("Description is mandatory", null);
            }

            if (user.Currency != currency)
            {
                return new("user Currency is not equal to the expense Currency", null);
            }

            Expense? expenseExist = context.Expense.FirstOrDefault(e => e.UserId == entity.UserId && e.Date == entity.Date);
            if (expenseExist != null)
            {
                return new("Expense already exist for the same user and the same date", null);
            }

            Expense newExpense = new Expense {
                Description = entity.Description,
                Date = entity.Date,
                ExpenseType = entity.Type,
                Currency = currency,
                User = user,
                Amount = Math.Round(entity.Amount, 2, MidpointRounding.AwayFromZero)  
            };

            // add entity to database
            await context.Expense.AddAsync(newExpense);

            await context.SaveChangesAsync();

            return new(null, newExpense);

        }



    }
}
