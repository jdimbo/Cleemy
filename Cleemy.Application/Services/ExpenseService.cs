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

        public async Task<List<Expense>> GetExpensesAsync(int? userId, string? sortColumn)
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
                return new (ErrorCodes.EXPENSE_ENTITY_IS_NULL, null );
            }

            Currency? currency = context.Currency.FirstOrDefault(f => f.Symbol == entity.Currency);

            if (currency is null)
            {
                return new(ErrorCodes.EXPENSE_CURRENCY_NOT_FOUND, null);
            }

            User? user = context.User.FirstOrDefault(f => f.Id == entity.UserId);

            if (user is null)
            {
                return new(ErrorCodes.EXPENSE_USER_NOT_FOUND, null);
            }

            if (entity.Date >= DateTime.Now.Date)
            {
                return new(ErrorCodes.EXPENSE_DATE_FUTURE, null);
            }
            if (entity.Date <= DateTime.Now.Date.AddMonths(-3))
            {
                return new(ErrorCodes.EXPENSE_DATE_PAST_3_MONTH, null);
            }
            if (string.IsNullOrWhiteSpace(entity.Description))
            {
                return new(ErrorCodes.EXPENSE_DESCRITPTION_IS_MANDATORY, null);
            } 

            if (user.Currency != currency)
            {
                return new(ErrorCodes.EXPENSE_CURRENCY_NOT_USER_CURRENCY, null);
            }

            Expense? expenseExist = context.Expense.FirstOrDefault(e => e.UserId == entity.UserId && e.Date.Date == entity.Date.Date);
            if (expenseExist != null)
            {
                return new(ErrorCodes.EXPENSE_ALREADY_EXISTS_SAME_DATE, null);
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
